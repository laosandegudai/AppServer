import React, { useEffect } from "react";
import { withRouter } from "react-router";
import CrmFilter from "@appserver/common/api/crm/filter";
import { isMobile } from "react-device-detect";
import PageLayout from "@appserver/common/components/PageLayout";
import {
  ArticleHeaderContent,
  ArticleMainButtonContent,
  ArticleBodyContent,
} from "../../components/Article";
import {
  SectionHeaderContent,
  SectionFilterContent,
  SectionBodyContent,
  SectionPagingContent,
} from "./Section";
import { withTranslation } from "react-i18next";
import { observer, inject } from "mobx-react";

const PureHome = ({ history, getContactsList, tReady }) => {
  const { location } = history;
  const { pathname } = location;

  useEffect(() => {
    if (pathname.indexOf("/crm/contact/filter") > -1) {
      const newFilter = CrmFilter.getFilter(location);

      getContactsList(newFilter);
    }
  }, [pathname, location]);

  return (
    <PageLayout withBodyScroll withBodyAutoFocus={!isMobile}>
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
        <SectionPagingContent tReady={tReady} />
      </PageLayout.SectionPaging>
    </PageLayout>
  );
};

const Home = withTranslation("Home")(PureHome);

export default inject(({ contactsStore }) => {
  return {
    getContactsList: contactsStore.getContactsList,
  };
})(withRouter(observer(Home)));
