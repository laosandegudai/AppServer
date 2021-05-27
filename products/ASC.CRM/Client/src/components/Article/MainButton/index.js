import React from "react";
import { withRouter } from "react-router";
import MainButton from "@appserver/components/main-button";
import DropDownItem from "@appserver/components/drop-down-item";
import Loaders from "@appserver/common/components/Loaders";
import { withTranslation } from "react-i18next";
import { inject, observer } from "mobx-react";

class PureArticleMainButtonContent extends React.Component {
  onCreateNewCompany = () => console.log("New Company");
  onCreateNewPerson = () => console.log("New Person");
  onCreateNewTask = () => console.log("New Task");
  onCreateNewOpportunity = () => console.log("New Opportunity");
  onCreateNewInvoice = () => console.log("New Invoice");
  onCreateNewCase = () => console.log("New Case");

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
          onClick={this.onCreateNewCompany}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.person.react.svg'
          label={t("NewPerson")}
          onClick={this.onCreateNewPerson}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.task.react.svg'
          label={t("NewTask")}
          onClick={this.onCreateNewTask}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.opportunity.react.svg'
          label={t("NewOpportunity")}
          onClick={this.onCreateNewOpportunity}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.invoice.react.svg'
          label={t("NewInvoice")}
          onClick={this.onCreateNewInvoice}
        />
        <DropDownItem
          className='main-button_drop-down'
          icon='images/actions.case.react.svg'
          label={t("NewCase")}
          onClick={this.onCreateNewCase}
        />
      </MainButton>
    );
  }
}

const ArticleMainButtonContent = withTranslation("Article")(
  PureArticleMainButtonContent
);

export default inject(({ crmStore }) => {
  const { firstLoad } = crmStore;

  return {
    firstLoad,
  };
})(withRouter(observer(ArticleMainButtonContent)));
