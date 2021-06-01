import React from "react";
import { withRouter } from "react-router";
import MainButton from "@appserver/components/main-button";
import DropDownItem from "@appserver/components/drop-down-item";
import Loaders from "@appserver/common/components/Loaders";
import { withTranslation } from "react-i18next";
import { inject, observer } from "mobx-react";

class PureArticleMainButtonContent extends React.Component {
  render() {
    const { t, firstLoad } = this.props;

    return !firstLoad ? (
      <Loaders.Rectangle />
    ) : (
      <MainButton isDropdown={true} text={t("Actions")}>
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.company.react.svg'
          label={t("NewCompany")}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.person.react.svg'
          label={t("NewPerson")}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.task.react.svg'
          label={t("NewTask")}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.opportunity.react.svg'
          label={t("NewOpportunity")}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.invoice.react.svg'
          label={t("NewInvoice")}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.case.react.svg'
          label={t("NewCase")}
        />
      </MainButton>
    );
  }
}

const ArticleMainButtonContent = withTranslation("Home")(
  PureArticleMainButtonContent
);

export default inject(({ crmStore }) => {
  const { firstLoad } = crmStore;

  return {
    firstLoad,
  };
})(withRouter(observer(ArticleMainButtonContent)));
