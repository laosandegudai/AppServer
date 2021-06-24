import React from "react";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import { withRouter } from "react-router";
import { FilterType } from "../../../../constants";
import FilterInput from "@appserver/common/components/FilterInput";

const PureSectionFilterContent = (props) => {
  const getData = () => {
    const { t, customNames } = props;
    const options = [
      {
        key: "filter-filterType",
        group: "filter-filterType",
        label: t("Common:Status"),
        isHeader: true,
      },
      {
        key: FilterType.Active.toString(),
        group: "filter-filterType",
        label: t("Common:Active"),
      },
      {
        key: FilterType.Paused.toString(),
        group: "filter-filterType",
        label: t("Common:Paused"),
      },
      {
        key: FilterType.Closed.toString(),
        group: "filter-filterType",
        label: t("Common:Closed"),
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
        label: t("Common:Me"),
        // defaultOptionLabel: t("Common:MeLabel"),
        // defaultSelectLabel: t("Common:Select"),
        // groupsCaption,
        // defaultOption: user,
        // selectedItem,
      },
      {
        key: "other",
        group: "filter-author",
        label: t("Common:OtherUsers"),
      },
      {
        key: "filter-author",
        group: "filter-author",
        label: t("Other"),
        isHeader: true,
      },
    ];

    return filterOptions;
  };

  const getSortData = () => {
    const commonOptions = [
      { key: "DateAndTimeCreation", label: t("ByCreationDate"), default: true },
      { key: "AZ", label: t("ByTitle"), default: true },
      { key: "Title", label: t("Title"), default: true },
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
    window.innerWidth < 500 ? {} : { filterColumnCount: 3 };

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

const SectionFilterContent = withTranslation(["Article", "Common"])(
  withRouter(PureSectionFilterContent)
);

export default inject(({ auth }) => {
  const { customNames } = auth.settingsStore;
  console.log(customNames.usersCaption);
  return {
    customNames,
  };
})(observer(SectionFilterContent));
