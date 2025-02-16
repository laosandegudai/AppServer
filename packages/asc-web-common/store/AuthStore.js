import { makeAutoObservable } from "mobx";
import api from "../api";
import { setWithCredentialsStatus } from "../api/client";
import history from "../history";
import ModuleStore from "./ModuleStore";
import SettingsStore from "./SettingsStore";
import UserStore from "./UserStore";
import TfaStore from "./TfaStore";
import { logout as logoutDesktop, desktopConstants } from "../desktop";
import { combineUrl, isAdmin } from "../utils";
import isEmpty from "lodash/isEmpty";
import { AppServerConfig, LANGUAGE } from "../constants";
const { proxyURL } = AppServerConfig;

class AuthStore {
  userStore = null;
  moduleStore = null;
  settingsStore = null;
  tfaStore = null;

  isLoading = false;
  isAuthenticated = false;
  version = null;
  skipModules = false;
  providers = [];
  isInit = false;

  constructor() {
    this.userStore = new UserStore();
    this.moduleStore = new ModuleStore();
    this.settingsStore = new SettingsStore();
    this.tfaStore = new TfaStore();

    makeAutoObservable(this);
  }

  init = async (skipModules = false) => {
    if (this.isInit) return;
    this.isInit = true;

    this.skipModules = skipModules;

    await this.userStore.init();

    if (this.userStore.user) this.setIsAuthenticated(true);

    const requests = [];
    requests.push(this.settingsStore.init());

    if (this.isAuthenticated && !skipModules) {
      requests.push(this.moduleStore.init());
    }

    return Promise.all(requests);
  };
  setLanguage() {
    if (this.userStore.user.cultureName) {
      localStorage.getItem(LANGUAGE) !== this.userStore.user.cultureName &&
        localStorage.setItem(LANGUAGE, this.userStore.user.cultureName);
    } else {
      localStorage.setItem(LANGUAGE, this.settingsStore.culture || "en-US");
    }
  }
  get isLoaded() {
    let success = false;
    if (this.isAuthenticated) {
      success = this.userStore.isLoaded && this.settingsStore.isLoaded;

      if (!this.skipModules) success = success && this.moduleStore.isLoaded;

      success && this.setLanguage();
    } else {
      success = this.settingsStore.isLoaded;
    }

    return success;
  }

  get language() {
    return (
      (this.userStore.user && this.userStore.user.cultureName) ||
      this.settingsStore.culture ||
      "en-US"
    );
  }

  get isAdmin() {
    const { user } = this.userStore;
    const { currentProductId } = this.settingsStore;

    if (!user || !user.id) return false;

    return isAdmin(user, currentProductId);
  }

  get product() {
    return (
      this.moduleStore.modules.find(
        (item) => item.id === this.settingsStore.currentProductId
      ) || ""
    );
  }

  get availableModules() {
    const { modules } = this.moduleStore;
    if (isEmpty(modules) || isEmpty(this.userStore.user)) {
      return [];
    }

    const customProducts = this.getCustomModules();
    const readyProducts = [];
    const inProgressProducts = [];
    modules.forEach((p) => {
      if (p.appName === "people" || p.appName === "files") {
        readyProducts.push(p);
      } else {
        inProgressProducts.push(p);
      }
    });

    return [
      {
        separator: true,
        id: "nav-products-separator",
      },
      ...readyProducts,
      ...customProducts,
      {
        separator: true,
        dashed: true,
        id: "nav-dummy-products-separator",
      },
      ...inProgressProducts,
    ];
  }

  getCustomModules = () => {
    if (!this.userStore.user.isAdmin) {
      return [];
    }
    const settingsModuleWrapper = this.moduleStore.toModuleWrapper({
      id: "settings",
      title: "Settings",
      link: "/settings",
      iconUrl: "/static/images/settings.react.svg",
    });

    settingsModuleWrapper.onClick = this.onClick;
    settingsModuleWrapper.onBadgeClick = this.onBadgeClick;

    return [settingsModuleWrapper];
  };

  login = async (user, hash) => {
    try {
      const response = await api.user.login(user, hash);

      if (!response || (!response.token && !response.tfa))
        throw response.error.message;

      if (response.tfa && response.confirmUrl) {
        const url = response.confirmUrl.replace(window.location.origin, "");
        return Promise.resolve({ url, user, hash });
      }

      setWithCredentialsStatus(true);

      this.reset();

      this.init();

      return Promise.resolve({ url: this.settingsStore.defaultPage });
    } catch (e) {
      return Promise.reject(e);
    }
  };

  loginWithCode = async (userName, passwordHash, code) => {
    await this.tfaStore.loginWithCode(userName, passwordHash, code);
    setWithCredentialsStatus(true);

    this.init();

    return Promise.resolve(this.settingsStore.defaultPage);
  };

  thirdPartyLogin = async (SerializedProfile) => {
    try {
      const response = await api.user.thirdPartyLogin(SerializedProfile);

      if (!response || !response.token) throw new Error("Empty API response");

      setWithCredentialsStatus(true);

      this.init();

      return Promise.resolve(true);
    } catch (e) {
      return Promise.reject(e);
    }
  };

  reset = () => {
    this.isInit = false;
    this.skipModules = false;
    this.userStore = new UserStore();
    this.moduleStore = new ModuleStore();
    this.settingsStore = new SettingsStore();
  };

  logout = async (withoutRedirect) => {
    await api.user.logout();

    //console.log("Logout response ", response);

    setWithCredentialsStatus(false);

    const { isDesktopClient: isDesktop, personal } = this.settingsStore;

    isDesktop && logoutDesktop();

    this.reset();

    this.init();

    if (!withoutRedirect) {
      if (personal) {
        window.location.replace("/");
      } else {
        history.push(combineUrl(proxyURL, "/login"));
      }
    }
  };

  setIsAuthenticated = (isAuthenticated) => {
    this.isAuthenticated = isAuthenticated;
  };

  getEncryptionAccess = (fileId) => {
    return api.files
      .getEncryptionAccess(fileId)
      .then((keys) => {
        return Promise.resolve(keys);
      })
      .catch((err) => console.error(err));
  };

  replaceFileStream = (fileId, file, encrypted, forcesave) => {
    return api.files.updateFileStream(file, fileId, encrypted, forcesave);
  };

  setEncryptionAccess = (file) => {
    return this.getEncryptionAccess(file.id).then((keys) => {
      return new Promise((resolve, reject) => {
        try {
          window.AscDesktopEditor.cloudCryptoCommand(
            "share",
            {
              cryptoEngineId: desktopConstants.guid,
              file: [file.viewUrl],
              keys: keys,
            },
            (obj) => {
              let resFile = null;
              if (obj.isCrypto) {
                let bytes = obj.bytes;
                let filename = "temp_name";
                resFile = new File([bytes], filename);
              }
              resolve(resFile);
            }
          );
        } catch (e) {
          reject(e);
        }
      });
    });
  };

  setDocumentTitle = (subTitle = null) => {
    let title;

    const currentModule = this.settingsStore.product;
    const organizationName = this.settingsStore.organizationName;

    if (subTitle) {
      if (this.isAuthenticated && currentModule) {
        title = subTitle + " - " + currentModule.title;
      } else {
        title = subTitle + " - " + organizationName;
      }
    } else if (currentModule && organizationName) {
      title = currentModule.title + " - " + organizationName;
    } else {
      title = organizationName;
    }

    document.title = title;
  };

  setProductVersion = (version) => {
    this.version = version;
  };

  setProviders = (providers) => {
    this.providers = providers;
  };
}

export default new AuthStore();
