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
import styled from "styled-components";
import { isMobile, isTablet, isIOS } from "react-device-detect";

import { setDocumentTitle } from "../../helpers/utils";
import { inject } from "mobx-react";
import i18n from "../../i18n";
import { I18nextProvider } from "react-i18next";
import { toCommunityHostname, deleteCookie } from "@appserver/common/utils";
import {
  ArticleHeaderContent,
  ArticleMainButtonContent,
} from "../../components/Article";
const Home = () => {
  return (
    <PageLayout>
      <PageLayout.ArticleHeader>
        <ArticleHeaderContent />
      </PageLayout.ArticleHeader>

      <PageLayout.ArticleMainButton>
        <ArticleMainButtonContent />
      </PageLayout.ArticleMainButton>
    </PageLayout>
  );
};

const HomeWrapper = inject(({ auth }) => ({
  modules: auth.moduleStore.modules,
  isLoaded: auth.isLoaded,
  setCurrentProductId: auth.settingsStore.setCurrentProductId,
}))(withRouter(withTranslation(["Article", "Common"])(Home)));

export default (props) => (
  <I18nextProvider i18n={i18n}>
    <HomeWrapper {...props} />
  </I18nextProvider>
);
