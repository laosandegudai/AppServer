import React from "react";
import { withRouter } from "react-router";
import MainButton from "@appserver/components/main-button";
import DropDownItem from "@appserver/components/drop-down-item";
import { withTranslation } from "react-i18next";
import Loaders from "@appserver/common/components/Loaders";
import { inject, observer } from "mobx-react";

class PureArticleMainButtonContent extends React.Component {
  render() {
    const { t, firstLoad } = this.props;
    return firstLoad ? (
      <Loaders.Rectangle />
    ) : (
      <MainButton isDropdown={true} text={t("Common:Actions")}>
        <DropDownItem
          className="main-button_drop-down"
          icon="images/action.projects.react.svg"
          label={t("NewProject")}
        />
        <DropDownItem
          className="main-button_drop-down"
          icon="images/action.spreadsheet.react.svg"
          label={t("NewMilestone")}
        />
        <DropDownItem
          className="main-button_drop-down"
          icon="images/action.task.react.svg"
          label={t("NewTask")}
        />
        <DropDownItem
          className="main-button_drop-down"
          icon="images/action.discussion.react.svg"
          label={t("NewDiscussion")}
        />
        <DropDownItem
          className="main-button_drop-down"
          icon="images/action.template.react.svg"
          label={t("NewTemplate")}
        />
        <DropDownItem
          className="main-button_drop-down"
          icon="images/action.timer.react.svg"
          label={t("TimerStart/Stop")}
        />
      </MainButton>
    );
  }
}

const ArticleMainButtonContent = withTranslation(["Article", "Common"])(
  PureArticleMainButtonContent
);

export default inject(({ projectsStore }) => {
  const { firstLoad } = projectsStore;

  return {
    firstLoad,
  };
})(withRouter(observer(ArticleMainButtonContent)));
