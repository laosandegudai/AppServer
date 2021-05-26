import React from "react";
import { withRouter } from "react-router";
import { isMobile } from "react-device-detect";
import PageLayout from "@appserver/common/components/PageLayout";
import {
  ArticleHeaderContent,
  ArticleMainButtonContent,
  ArticleBodyContent,
} from "../../components/Article";
import { SectionHeaderContent, SectionFilterContent } from "./Section";
import { withTranslation } from "react-i18next";
import { observer, inject } from "mobx-react";

const PureHome = () => {
  return (
    <PageLayout withBodyScroll uploadFiles withBodyAutoFocus={!isMobile}>
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
    </PageLayout>
  );
};

const Home = withTranslation("Home")(PureHome);

export default inject(({ auth }) => {
  return {
    modules: auth.moduleStore.modules,
    isLoaded: auth.isLoaded,
    setCurrentProductId: auth.settingsStore.setCurrentProductId,
  };
})(withRouter(observer(Home)));
