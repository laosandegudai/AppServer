import React, { useEffect } from "react";
import { withRouter } from "react-router";
import PageLayout from "@appserver/common/components/PageLayout";
import { withTranslation } from "react-i18next";
import { inject } from "mobx-react";
import i18n from "../../i18n";
import { I18nextProvider } from "react-i18next";
import {
  ArticleBodyContent,
  ArticleHeaderContent,
  ArticleMainButtonContent,
} from "../../components/Article";
import {
  SectionBodyContent,
  SectionFilterContent,
  SectionHeaderContent,
  SectionPagingContent,
} from "./Section";
import api from "@appserver/common/api";
import config from "../../../package.json";

const { ProjectsFilter, TasksFilter } = api;
const Home = ({
  homepage,
  setIsLoading,
  firstLoad,
  setFirstLoad,
  fetchProjects,
  history,
  isLoading,
  fetchTasks,
  setExpandedKeys,
}) => {
  const { location } = history;
  const { pathname } = location;
  useEffect(() => {
    const reg = new RegExp(`${homepage}((/?)$|/filter)`, "gm");
    const match = window.location.pathname.match(reg);
    let filterObj = null;

    if (match && match.length > 0) {
      filterObj = TasksFilter.getFilter(window.location);

      if (!filterObj) {
        filterObj = TasksFilter.getDefault();
        filterObj.folder = "tasks";
        setIsLoading(true);
        fetchTasks(filterObj, null, true).finally(() => {
          setIsLoading(false);
          setFirstLoad(false);
        });
      }
    }

    if (pathname.indexOf("/projects/filter") > -1) {
      const newFilter = ProjectsFilter.getFilter(location);
      setIsLoading(true);
      fetchProjects(newFilter, newFilter.folder).finally(() => {
        setIsLoading(false);
        setFirstLoad(false);
      });
      setExpandedKeys(["projects"]);
    }

    if (pathname.indexOf("/task/filter") > -1) {
      const newFilter = TasksFilter.getFilter(location);
      setIsLoading(true);
      fetchTasks(newFilter, newFilter.folder).finally(() => {
        setIsLoading(false);
        setFirstLoad(false);
      });
      setExpandedKeys(["tasks"]);
    }
  }, []);

  return (
    <PageLayout
      isLoaded={!firstLoad}
      firstLoad={firstLoad}
      isLoading={isLoading}
    >
      <PageLayout.ArticleHeader>
        <ArticleHeaderContent />
      </PageLayout.ArticleHeader>

      <PageLayout.ArticleMainButton>
        <ArticleMainButtonContent />
      </PageLayout.ArticleMainButton>

      <PageLayout.ArticleBody>
        <ArticleBodyContent />
      </PageLayout.ArticleBody>
      <PageLayout.SectionHeader>
        <SectionHeaderContent />
      </PageLayout.SectionHeader>
      <PageLayout.SectionFilter>
        <SectionFilterContent />
      </PageLayout.SectionFilter>
      <PageLayout.SectionBody>
        <SectionBodyContent />
      </PageLayout.SectionBody>

      <PageLayout.SectionPaging>
        <SectionPagingContent />
      </PageLayout.SectionPaging>
    </PageLayout>
  );
};

const HomeWrapper = inject(
  ({
    auth,
    projectsStore,
    projectsFilterStore,
    tasksFilterStore,
    treeFoldersStore,
  }) => {
    const {
      isLoading,
      firstLoad,
      setIsLoading,
      setFirstLoad,
      filter,
    } = projectsStore;
    const { setExpandedKeys } = treeFoldersStore;
    const { fetchTasks } = tasksFilterStore;
    const { projects, fetchProjects } = projectsFilterStore;
    return {
      modules: auth.moduleStore.modules,
      isLoaded: auth.isLoaded,
      setCurrentProductId: auth.settingsStore.setCurrentProductId,
      homepage: config.homepage,
      fetchProjects,
      isLoading,
      firstLoad,
      setIsLoading,
      setFirstLoad,
      filter,
      projects,
      fetchTasks,
      setExpandedKeys,
    };
  }
)(withRouter(withTranslation(["Home", "Common", "Article"])(Home)));

export default (props) => (
  <I18nextProvider i18n={i18n}>
    <HomeWrapper {...props} />
  </I18nextProvider>
);
