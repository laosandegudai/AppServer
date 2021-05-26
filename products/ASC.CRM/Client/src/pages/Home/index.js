import React from "react";
import { withRouter } from "react-router";

import PageLayout from "@appserver/common/components/PageLayout";
import {
  ArticleHeaderContent,
  // ArticleMainButtonContent,
  ArticleBodyContent,
} from "../../components/Article";
import { withTranslation } from "react-i18next";
import { observer, inject } from "mobx-react";

const PureHome = (props) => {
  return (
    <PageLayout>
      <PageLayout.ArticleHeader>
        <ArticleHeaderContent />
      </PageLayout.ArticleHeader>

      {/* <PageLayout.ArticleMainButton>
        <ArticleMainButtonContent />
      </PageLayout.ArticleMainButton> */}

      <PageLayout.ArticleBody>
        <ArticleBodyContent />
      </PageLayout.ArticleBody>
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
