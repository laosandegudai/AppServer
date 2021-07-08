import React, { useEffect } from "react";
import { ReactSVG } from "react-svg";
import PropTypes from "prop-types";
import { withRouter } from "react-router";
import Text from "@appserver/components/text";
import Link from "@appserver/components/link";
import Badge from "@appserver/components/badge";
import Box from "@appserver/components/box";
import EmptyScreenContainer from "@appserver/components/empty-screen-container";
import ExternalLinkIcon from "../../../../../../public/images/external.link.react.svg";
import Loaders from "@appserver/common/components/Loaders";
import toastr from "studio/toastr";
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
  selectedTreeNode,
  fetchTasks,
}) => {
  const { location } = history;
  const { pathname } = location;
  useEffect(() => {
    const reg = new RegExp(`${homepage}((/?)$|/filter)`, "gm");
    const match = window.location.pathname.match(reg);
    console.log(reg, match);
    let filterObj = null;

    if (match && match.length > 0) {
      filterObj = ProjectsFilter.getFilter(window.location);

      if (!filterObj) {
        filterObj = ProjectsFilter.getDefault();
        setIsLoading(true);
        fetchProjects(filterObj).finally(() => {
          setIsLoading(false);
          setFirstLoad(false);
        });
      }
    }

    if (pathname.indexOf("/projects/filter") > -1) {
      const newFilter = ProjectsFilter.getFilter(location);
      setIsLoading(true);
      fetchProjects(newFilter).finally(() => {
        setIsLoading(false);
        setFirstLoad(false);
      });
    }

    if (pathname.indexOf("/task/filter") > -1) {
      const newFilter = TasksFilter.getFilter(location);
      setIsLoading(true);
      fetchTasks(newFilter).finally(() => {
        setIsLoading(false);
        setFirstLoad(false);
      });
    }
  }, []);

  return (
    <PageLayout>
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
    const { isLoading, firstLoad, setIsLoading, setFirstLoad } = projectsStore;
    const { selectedTreeNode } = treeFoldersStore;
    const { fetchTasks } = tasksFilterStore;
    const {
      fetchAllProjects,
      projects,
      filter,
      fetchProjects,
    } = projectsFilterStore;
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
      selectedTreeNode,
      fetchAllProjects,
      filter,
      projects,
      fetchTasks,
    };
  }
)(withRouter(withTranslation(["Article", "Common"])(Home)));

export default (props) => (
  <I18nextProvider i18n={i18n}>
    <HomeWrapper {...props} />
  </I18nextProvider>
);
