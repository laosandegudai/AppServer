import React from "react";
import { Provider as FilesProvider } from "mobx-react";
import { inject, observer } from "mobx-react";
import { Switch } from "react-router-dom";
import config from "../package.json";
import PrivateRoute from "@appserver/common/components/PrivateRoute";
import AppLoader from "@appserver/common/components/AppLoader";
import toastr from "studio/toastr";
import { combineUrl, updateTempContent } from "@appserver/common/utils";
import stores from "./store/index";
import i18n from "./i18n";
import { I18nextProvider, withTranslation } from "react-i18next";
import { regDesktop } from "@appserver/common/desktop";
import Home from "./pages/Home";
import Settings from "./pages/Settings";
import VersionHistory from "./pages/VersionHistory";
import ErrorBoundary from "@appserver/common/components/ErrorBoundary";
import Panels from "./components/FilesPanels";
import { AppServerConfig } from "@appserver/common/constants";
import plugin from "js-plugin";

const { proxyURL } = AppServerConfig;
const homepage = config.homepage;

const PROXY_HOMEPAGE_URL = combineUrl(proxyURL, homepage);

const HOME_URL = combineUrl(PROXY_HOMEPAGE_URL, "/");
const SETTINGS_URL = combineUrl(PROXY_HOMEPAGE_URL, "/settings/:setting");
const HISTORY_URL = combineUrl(PROXY_HOMEPAGE_URL, "/:fileId/history");
const FILTER_URL = combineUrl(PROXY_HOMEPAGE_URL, "/filter");

if (!window.AppServer) {
  window.AppServer = {};
}

window.AppServer.files = {
  HOME_URL,
  SETTINGS_URL,
  HISTORY_URL,
  FILTER_URL,
};

/*******************************************/
// Registration Plugin
/*******************************************/

const script = document.createElement("script");
script.setAttribute("type", "text/javascript");
script.setAttribute("id", "files-viewer-plugin");

script.onload = () => {
  console.log("Plugin has been loaded");
  if (window.Plugins instanceof Array && window.Plugins.length > 0) {
    window.Plugins.forEach((p) => {
      console.log("Plugin found", p);
      plugin.register(p);
    });
  }
};

script.src = "/plugins/files-viewer-plugin.js";
script.async = true;

console.log("PureEditor componentDidMount: added script");
document.body.appendChild(script);

/*******************************************/
// Registration Plugin1
/*******************************************/
// plugin.register({
//   name: "plugin1",
//   files: {
//     article: {
//       main_button: {
//         processOptions(items) {
//           items.push({
//             key: "plugin1",
//             icon: "images/tick.rounded.svg",
//             label: "Say Hello!",
//             onClick: () => {
//               toastr.success("Hello, Pavlik!");
//             },
//           });
//         },
//       },
//     },
//   },
// });

/*******************************************/
// Registration Plugin2
/*******************************************/
// plugin.register({
//   name: "plugin2",
//   files: {
//     context: {
//       processOptions(options, item) {
//         options.push({
//           key: "plugin2-options",
//           label: "Display file info! (Plugin 2)",
//           icon: "images/tick.rounded.svg",
//           onClick: (e) => {
//             const { title, contentLength } = item;
//             toastr.info(`'${title}' (${contentLength})`);
//           },
//           disabled: false,
//         });
//       },
//     },
//   },
// });

/*******************************************/
// Registration Plugin3
/*******************************************/
// plugin.register({
//   name: "plugin3",
//   files: {
//     context: {
//       processOptions(options, item) {
//         options.push({
//           key: "plugin3-options",
//           label: "Display file info! (Plugin 3)",
//           icon: "images/tick.rounded.svg",
//           onClick: (e) => {
//             alert("Hell");
//           },
//           disabled: false,
//         });
//       },
//     },
//   },
// });

const Error404 = React.lazy(() => import("studio/Error404"));

const Error404Route = (props) => (
  <React.Suspense fallback={<AppLoader />}>
    <ErrorBoundary>
      <Error404 {...props} />
    </ErrorBoundary>
  </React.Suspense>
);

class FilesContent extends React.Component {
  constructor(props) {
    super(props);

    const pathname = window.location.pathname.toLowerCase();
    this.isEditor = pathname.indexOf("doceditor") !== -1;
    this.isDesktopInit = false;
  }

  componentDidMount() {
    this.props
      .loadFilesInfo()
      .catch((err) => toastr.error(err))
      .finally(() => {
        this.props.setIsLoaded(true);
        updateTempContent();
      });
  }

  componentDidUpdate(prevProps) {
    const {
      isAuthenticated,
      user,
      isEncryption,
      encryptionKeys,
      setEncryptionKeys,
      isLoaded,
    } = this.props;
    //console.log("componentDidUpdate: ", this.props);
    if (isAuthenticated && !this.isDesktopInit && isEncryption && isLoaded) {
      this.isDesktopInit = true;
      regDesktop(
        user,
        isEncryption,
        encryptionKeys,
        setEncryptionKeys,
        this.isEditor,
        this.props.t
      );
      console.log(
        "%c%s",
        "color: green; font: 1.2em bold;",
        "Current keys is: ",
        encryptionKeys
      );
    }
  }

  render() {
    //const { /*, isDesktop*/ } = this.props;

    return (
      <>
        <Panels />
        <Switch>
          <PrivateRoute exact path={SETTINGS_URL} component={Settings} />
          <PrivateRoute exact path={HISTORY_URL} component={VersionHistory} />
          <PrivateRoute exact path={HOME_URL} component={Home} />
          <PrivateRoute path={FILTER_URL} component={Home} />
          <PrivateRoute component={Error404Route} />
        </Switch>
      </>
    );
  }
}

const Files = inject(({ auth, filesStore }) => {
  return {
    isDesktop: auth.settingsStore.isDesktopClient,
    user: auth.userStore.user,
    isAuthenticated: auth.isAuthenticated,
    encryptionKeys: auth.settingsStore.encryptionKeys,
    isEncryption: auth.settingsStore.isEncryptionSupport,
    isLoaded: auth.isLoaded && filesStore.isLoaded,
    setIsLoaded: filesStore.setIsLoaded,
    setEncryptionKeys: auth.settingsStore.setEncryptionKeys,
    loadFilesInfo: async () => {
      //await auth.init();
      await filesStore.initFiles();
      auth.setProductVersion(config.version);
    },
  };
})(withTranslation("Common")(observer(FilesContent)));

export default () => (
  <FilesProvider {...stores}>
    <I18nextProvider i18n={i18n}>
      <Files />
    </I18nextProvider>
  </FilesProvider>
);
