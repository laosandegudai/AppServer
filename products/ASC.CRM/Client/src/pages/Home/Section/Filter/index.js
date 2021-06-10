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
    const managerOptions = [
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
    ];

        const accessOptions = [
          {
            key: "filter-access",
            group: "filter-access",
            label: t("Accessibility"),
            isHeader: true,
          },
          {
            key: "1",
            group: "filter-access",
            label: t("Public"),
          },
          {
            key: "2",
            group: "filter-access",
            label: t("Restricted"),
          },
        ];

         const otherOptions = [
           {
             key: "filter-Other",
             group: "filter-Other",
             label: t("Other"),
             isHeader: true,
           },
           {
             key: "1",
             group: "filter-Other",
             label: t("TemperatureLevel"),
           },
           {
             key: "2",
             group: "filter-Other",
             label: t("ContactType"),
           },
           {
             key: "3",
             group: "filter-Other",
             label: t("WithTag"),
           }
         ];

    const filterOptions = [...managerOptions, ...accessOptions, ...otherOptions]
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
    window.innerWidth < 500 ? {} : { filterColumnCount: 3 };

  return isLoaded ? (
    <FilterInput
      sectionWidth={sectionWidth}
      getFilterData={getData}
      getSortData={getSortData}
      selectedFilterData={selectedFilterData}
      onFilter={onFilter}
      directionAscLabel={t("Common:DirectionAscLabel")}
      directionDescLabel={t("Common:DirectionDescLabel")}
      placeholder={t("Search")}
      {...filterColumnCount}
      contextMenuHeader={t("AddFilter")}
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
