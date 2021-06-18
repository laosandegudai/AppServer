import React, {useEffect} from "react";
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
  // SectionBodyContent,
} from "./Section";
import { withTranslation } from "react-i18next";
import { observer, inject } from "mobx-react";

const PureHome = ({history, getContactsList}) => {
  const { location } = history;
  const { pathname } = location;

    useEffect(() => {
      console.log('!!!!', pathname)
    if (pathname.indexOf("/crm/filter") > -1) {
      const newFilter = CrmFilter.getFilter(location);
      console.log("CONTACTS URL changed", pathname, newFilter);
      getContactsList(newFilter)
    }
  }, [pathname, location]);

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

      {/* <PageLayout.SectionBody>
        <SectionBodyContent />
      </PageLayout.SectionBody> */}
    </PageLayout>
  );
};

const Home = withTranslation("Home")(PureHome);

export default inject(({ auth, contactsStore }) => {
  return {
    modules: auth.moduleStore.modules,
    isLoaded: auth.isLoaded,
    setCurrentProductId: auth.settingsStore.setCurrentProductId,
    getContactsList: contactsStore.getContactsList
  };
})(withRouter(observer(Home)));
