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
import ProjectsFilter from "@appserver/common/api/projects/filter";
import config from "../../../package.json";
const Home = ({
  homepage,
  fetchAllProjects,
  projects,
  setIsLoading,
  firstLoad,
  setFirstLoad,
  fetchProjects,
  history,
}) => {
  const { location } = history;
  const { pathname } = location;
  useEffect(() => {
    // спросить по этому участку Илью
    const reg = new RegExp(`${homepage}((/?)$|/filter)`, "gm");
    const match = window.location.pathname.match(reg);
    let filterObj = null;

    console.log(match);

    if (match && match.length > 0) {
      filterObj = ProjectsFilter.getFilter(window.location);
      console.log(filterObj);

      if (!filterObj) {
        filterObj = ProjectsFilter.getDefault();
        // setIsLoading(true);
        console.log("da");
        fetchProjects(filterObj);
        // .finally(() => {
        //   setIsLoading(false);
        //   setFirstLoad(false);
        // });
      }
    }

    if (!filterObj) return;
    // вот здесь мы должны определить url путь и уже делать запрос
    if (pathname.indexOf("/projects/filter") > -1) {
      const newFilter = ProjectsFilter.getFilter(location);
      fetchAllProjects(newFilter);
    }
    const newFilter = filterObj ? filterObj.clone() : FilesFilter.getDefault();

    console.log(filterObj);
    const filter = ProjectsFilter.getDefault();
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

const HomeWrapper = inject(({ auth, projectsStore, projectsFilterStore }) => {
  const { isLoading, firstLoad, setIsLoading, setFirstLoad } = projectsStore;
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
    fetchProjectsItems: projectsStore.projectsFilterStore.fetchProjectsItems,
    homepage: config.homepage,
    fetchProjects,
    isLoading,
    firstLoad,
    setIsLoading,
    setFirstLoad,

    fetchAllProjects,
    filter,
    projects,
  };
})(withRouter(withTranslation(["Article", "Common"])(Home)));

export default (props) => (
  <I18nextProvider i18n={i18n}>
    <HomeWrapper {...props} />
  </I18nextProvider>
);
