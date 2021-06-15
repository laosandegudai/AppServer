import React from "react";
import Loaders from "@appserver/common/components/Loaders";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import { isMobileOnly } from "react-device-detect";
import FilterInput from "@appserver/common/components/FilterInput";

const SectionFilterContent = ({ sectionWidth, isLoaded, t }) => {
  const getSelectedFilterData = () => {
    const selectedFilterData = {
      filterValues: [{ group: "filter-filterType", key: "null" }],
      sortDirection: "desc",
      sortId: "DateAndTime",
      inputValue: null,
    };
    return selectedFilterData;
  };
  const selectedFilterData = getSelectedFilterData();

  const getData = () => {
    const options = [
      {
        key: "filter-manager",
        group: "filter-manager",
        label: t("Manager"),
        isHeader: true,
      },
      {
        key: "1",
        group: "filter-manager",
        label: t("My"),
      },
      {
        key: "2",
        group: "filter-manager",
        label: t("NoContactManager"),
      },
      {
        key: "3",
        group: "filter-manager",
        label: t("Custom"),
      },
      {
        key: "filter-access",
        group: "filter-access",
        label: t("Accessibility"),
        isRowHeader: true,
      },
      {
        key: "5",
        group: "filter-access",
        label: t("Public"),
      },
      {
        key: "6",
        group: "filter-access",
        label: t("Restricted"),
      },
      {
        key: "filter-other",
        group: "filter-Other",
        label: t("Other"),
        isRowHeader: true,
      },
      {
        key: "7",
        group: "filter-other",
        label: t("TemperatureLevel"),
      },
      {
        key: "8",
        group: "filter-other",
        label: t("ContactType"),
      },
      {
        key: "9",
        group: "filter-other",
        label: t("WithTag"),
      },
    ];

    const filterOptions = [
      ...options,
      {
        key: "filter-creation-date",
        group: "filter-creation-date",
        label: t("CreationDate"),
        isHeader: true,
      },
      {
        key: "10",
        group: "filter-creation-date",
        label: t("LastMonth"),
      },
      {
        key: "11",
        group: "filter-creation-date",
        label: t("Yesterday"),
      },
      {
        key: "12",
        group: "filter-creation-date",
        label: t("Today"),
      },
      {
        key: "13",
        group: "filter-creation-date",
        label: t("ThisMonth"),
      },
      {
        key: "14",
        group: "filter-creation-date",
        label: t("Custom"),
      },
      {
        key: "filter-show",
        group: "filter-show",
        label: t("Show"),
        isRowHeader: true,
      },
      {
        key: "15",
        group: "filter-show",
        label: t("Companies"),
      },
      {
        key: "16",
        group: "filter-show",
        label: t("Persons"),
      },
      {
        key: "17",
        group: "filter-show",
        label: t("WithOpportunities"),
      },
    ];

    return filterOptions;
  };

  const getSortData = () => {
    const commonOptions = [
      { key: "DateAndTimeCreation", label: t("ByCreationDate"), default: true },
      { key: "AZ", label: t("ByTitle"), default: true },
      {
        key: "TemperatureLevel",
        label: t("ByTemperatureLevel"),
        default: true,
      },
      { key: "History", label: t("ByHistory"), default: true },
    ];

    const viewSettings = [
      { key: "row", label: t("ViewList"), isSetting: true, default: true },
      { key: "tile", label: t("ViewTiles"), isSetting: true, default: true },
    ];
    return window.innerWidth < 460
      ? [...commonOptions, ...viewSettings]
      : commonOptions;
  };

  const onFilter = () => {};

  const filterColumnCount =
    window.innerWidth < 500 ? {} : { filterColumnCount: 2 };

  return isLoaded ? (
    <FilterInput
      sectionWidth={sectionWidth}
      getFilterData={getData}
      getSortData={getSortData}
      selectedFilterData={selectedFilterData}
      onFilter={onFilter}
      directionAscLabel={t("Common:DirectionAscLabel")}
      directionDescLabel={t("Common:DirectionDescLabel")}
      placeholder={t("Common:Search")}
      {...filterColumnCount}
      contextMenuHeader={t("Common:AddFilter")}
      isMobile={isMobileOnly}
    />
  ) : (
    <Loaders.Filter />
  );
};

export default inject(({ crmStore }) => {
  const { isLoaded } = crmStore;

  return {
    isLoaded,
  };
})(withTranslation(["Home", "Common"])(observer(SectionFilterContent)));
