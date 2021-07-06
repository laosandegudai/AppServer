import React from "react";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import { withRouter } from "react-router";
import { RowProjectOptionStatus } from "../../../../constants";
import FilterInput from "@appserver/common/components/FilterInput";

const PureSectionFilterContent = (props) => {
  const { t } = props;
  const getData = () => {
    const options = [
      {
        key: "filter-filterType",
        group: "filter-filterType",
        label: t("Status"),
        isHeader: true,
      },
      {
        key: RowProjectOptionStatus.Active.toString(),
        group: "filter-filterType",
        label: t("Active"),
      },
      {
        key: RowProjectOptionStatus.Paused.toString(),
        group: "filter-filterType",
        label: t("Paused"),
      },
      {
        key: RowProjectOptionStatus.Closed.toString(),
        group: "filter-filterType",
        label: t("Closed"),
      },
    ];

    const filterOptions = [
      ...options,
      {
        key: "filter-author",
        group: "filter-author",
        label: t("ByProjectManager"),
        isHeader: true,
      },
      {
        key: "me",
        group: "filter-author",
        label: t("Me"),
        defaultOptionLabel: t("Common:MeLabel"),
        defaultSelectLabel: t("Common:Select"),
        // groupsCaption,
        // defaultOption: user,
        // selectedItem,
      },
      {
        key: "other-users",
        group: "filter-author",
        label: t("OtherUsers"),
      },
      {
        key: "other",
        group: "filter-author",
        label: t("Other"),
        isHeader: true,
      },
      {
        key: "followed",
        group: "filter-author",
        label: t("Followed"),
      },
      {
        key: "withTag",
        group: "filter-author",
        label: t("WithTag"),
      },
      {
        key: "withoutTag",
        group: "filter-author",
        label: t("WithoutTag"),
      },
      {
        key: "filter-test",
        group: "filter-member",
        label: t("TeamMember"),
        isHeader: true,
      },
      {
        key: "team-me",
        group: "filter-member",
        label: t("Me"),
      },
      {
        key: "team-otherUsers",
        group: "filter-member",
        label: t("OtherUsers"),
      },
      {
        key: "team-groups",
        group: "filter-member",
        label: t("Groups"),
      },
    ];

    return filterOptions;
  };

  const getSortData = () => {
    const commonOptions = [
      { key: "DateAndTimeCreation", label: t("ByCreationDate"), default: true },
      { key: "AZ", label: t("ByTitle"), default: true },
    ];

    const viewSettings = [
      { key: "row", label: t("ViewList"), isSetting: true, default: true },
      { key: "tile", label: t("ViewTiles"), isSetting: true, default: true },
    ];

    return window.innerWidth < 460
      ? [...commonOptions, ...viewSettings]
      : commonOptions;
  };
  const filterColumnCount =
    window.innerWidth < 500 ? {} : { filterColumnCount: 4 };

  const selectedFilterData = {
    filterValues: [
      {
        key: "null",
        group: "filter-filterType",
      },
    ],
    inputValue: null,
    sortDirection: "desc",
    sortId: "AZ",
  };

  return (
    <FilterInput
      getFilterData={getData}
      getSortData={getSortData}
      onFilter={(result) => {
        console.log(result);
      }}
      selectedFilterData={selectedFilterData}
      viewAs={false}
      directionAscLabel={t("Common:DirectionAscLabel")}
      directionDescLabel={t("Common:DirectionDescLabel")}
      placeholder={t("Common:Search")}
      {...filterColumnCount}
      contextMenuHeader={t("Common:AddFilter")}
    />
  );
};

const SectionFilterContent = withTranslation(["Home", "Common"])(
  withRouter(PureSectionFilterContent)
);

export default inject(({ auth }) => {
  const { customNames } = auth.settingsStore;
  return {
    customNames,
  };
})(observer(SectionFilterContent));
