import React, { useEffect } from "react";
import { Provider as ProjectProvider, inject, observer } from "mobx-react";
import { Switch } from "react-router-dom";
import ProjectsStore from "./store/ProjectsStore";
import ErrorBoundary from "@appserver/common/components/ErrorBoundary";
import toastr from "studio/toastr";
import PrivateRoute from "@appserver/common/components/PrivateRoute";
import AppLoader from "@appserver/common/components/AppLoader";
import { combineUrl, updateTempContent } from "@appserver/common/utils";
import config from "../package.json";
import i18n from "./i18n";
import { I18nextProvider } from "react-i18next";
import Home from "./pages/Home";
import { AppServerConfig } from "@appserver/common/constants";
import stores from "./store/index";

const { proxyURL } = AppServerConfig;
const homepage = config.homepage;
const PROXY_HOMEPAGE_URL = combineUrl(proxyURL, homepage);

const Error404 = React.lazy(() => import("studio/Error404"));

const Error404Route = (props) => (
  <React.Suspense fallback={<AppLoader />}>
    <ErrorBoundary>
      <Error404 {...props} />
    </ErrorBoundary>
  </React.Suspense>
);

const ProjectsContent = (props) => {
  const { isLoaded, loadBaseInfo, setIsLoaded } = props;

  useEffect(() => {
    loadBaseInfo()
      .catch((err) => toastr.error(err))
      .finally(() => {
        setIsLoaded(true);
        updateTempContent();
      });
  }, []);

  return (
    <Switch>
      <PrivateRoute exact path={PROXY_HOMEPAGE_URL} component={Home} />
      <PrivateRoute component={Error404Route} />
    </Switch>
  );
};

const Projects = inject(({ auth, projectsStore }) => {
  return {
    user: auth.userStore.user,
    isAuthenticated: auth.isAuthenticated,
    isLoaded: auth.isLoaded && projectsStore.isLoaded,
    setIsLoaded: projectsStore.setIsLoaded,
    loadBaseInfo: async () => {
      await projectsStore.init();
      auth.setProductVersion(config.version);
    },
    isLoaded: auth.isLoaded && projectsStore.isLoaded,
  };
})(observer(ProjectsContent));
console.log(stores);
export default (props) => (
  <ProjectProvider {...stores}>
    <I18nextProvider i18n={i18n}>
      <Projects {...props} />
    </I18nextProvider>
  </ProjectProvider>
);
