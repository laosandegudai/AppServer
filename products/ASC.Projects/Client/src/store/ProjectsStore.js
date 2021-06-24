import { action, makeObservable, observable } from "mobx";
import config from "../../package.json";
import { updateTempContent } from "@appserver/common/utils";

class ProjectsStore {
  authStore;
  settingsStore;
  userStore;
  treeFoldersStore;
  isLoading = false;
  isLoaded = false;
  isInit = false;

  firstLoad = true;

  constructor(authStore, settingsStore, userStore, treeFoldersStore) {
    this.authStore = authStore;
    this.settingsStore = settingsStore;
    this.userStore = userStore;
    this.treeFoldersStore = treeFoldersStore;

    makeObservable(this, {
      isLoading: observable,
      isLoaded: observable,
      setIsLoading: action,
      setIsLoaded: action,
      init: action,
    });
  }

  init = () => {
    //if (this.isInit) return;

    const { isAuthenticated } = this.authStore;
    const { getPortalCultures, setModuleInfo } = this.settingsStore;
    const { fetchTreeFolders } = this.treeFoldersStore;

    setModuleInfo(config.homepage, config.id);

    const requests = [];

    updateTempContent();

    if (!isAuthenticated) {
      return this.setIsLoaded(true);
    } else {
      updateTempContent(isAuthenticated);
    }

    requests.push(getPortalCultures(), fetchTreeFolders());

    this.setIsLoaded(true);

    return Promise.all(requests).then(() => {
      this.setIsLoaded(true);
      this.isInit = true;
    });
  };

  setIsLoading = (loading) => {
    this.isLoading = loading;
  };

  setIsLoaded = (isLoaded) => {
    this.isLoaded = isLoaded;
  };
}

export default ProjectsStore;
