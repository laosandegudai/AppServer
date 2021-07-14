import React from "react";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import result from "lodash/result";
import find from "lodash/find";
import { withRouter } from "react-router";
import Loaders from "@appserver/common/components/Loaders";
import FilterInput from "@appserver/common/components/FilterInput";
import api from "@appserver/common/api";
const { ProjectsFilter, TasksFilter } = api;

const getStatus = (filterValues) => {
  const status = result(
    find(filterValues, (value) => {
      return value.group === "filter-status";
    }),
    "key"
  );
  return status ? status : null;
};

const getManager = (filterValues) => {
  const manager = result(
    find(filterValues, (value) => {
      return value.group === "filter-author";
    }),
    "key"
  );
  return manager ? manager : null;
};

const getOtherSettings = (filterValues) => {
  const settings = result(
    find(filterValues, (value) => {
      return value.group === "filter-settings";
    }),
    "key"
  );
  return settings ? settings : null;
};

const getSelectedItem = (filterValues, type) => {
  const selectedItem = filterValues.find((item) => item.key === type);
  return selectedItem || null;
};
const PureSectionFilterContent = (props) => {
  const { t, filter, customNames, user, fetchProjects, tReady } = props;

  const onFilter = (data) => {
    // вообще подумать как это все лучше обрабатывать с несколькими фильтрами
    const status = getStatus(data.filterValues) || null;
    const search = data.inputValue || "";
    const sortBy = data.sortId;
    const sortOrder =
      data.sortDirection === "desc" ? "descending" : "ascending";
    const manager = getManager(data.filterValues) || null;
    const settings = getOtherSettings(data.filterValues) || null;
    const selectedItem = manager
      ? getSelectedItem(data.filterValues, manager)
      : null;
    const selectedFilterItem = {};
    if (selectedItem) {
      selectedFilterItem.key = selectedItem.selectedItem.key;
      selectedFilterItem.label = selectedItem.selectedItem.label;
      selectedFilterItem.type = selectedItem.typeSelector;
    }

    if (filter instanceof ProjectsFilter) {
      const newFilter = filter.clone();
      newFilter.page = 0;
      newFilter.sortBy = sortBy;
      newFilter.sortOrder = sortOrder;
      newFilter.search = search;
      newFilter.selectedItem = selectedFilterItem;
      newFilter.status = status;
      console.log(settings);
      // сделать здесь обнуление
      // newFilter.follow = null;
      if (settings === "follow") {
        newFilter[settings] = true;
      }
      fetchProjects(newFilter);
    }
  };
  const getData = () => {
    const { usersCaption, groupsCaption } = customNames;
    const { selectedItem } = filter;
    const options = [
      {
        key: "filter-status",
        group: "filter-status",
        label: t("Status"),
        isHeader: true,
      },
      {
        key: "open",
        group: "filter-status",
        label: t("Active"),
      },
      {
        key: "paused",
        group: "filter-status",
        label: t("Paused"),
      },
      {
        key: "closed",
        group: "filter-status",
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
        key: "user1",
        group: "filter-author",
        label: t("Me"),
        isSelector: true,
        defaultOptionLabel: t("Common:MeLabel"),
        defaultSelectLabel: t("Common:Select"),
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "user2",
        group: "filter-author",
        label: t("OtherUsers"),
        isSelector: true,
        defaultOptionLabel: t("Common:MeLabel"),
        defaultSelectLabel: t("Common:Select"),
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "filter-settings",
        group: "filter-settings",
        label: t("Other"),
        isHeader: true,
      },
      {
        key: "follow",
        group: "filter-settings",
        label: t("Followed"),
      },
      {
        key: "tag",
        group: "filter-settings",
        label: t("WithTag"),
      },
      {
        key: "withoutTag",
        group: "filter-settings",
        label: t("WithoutTag"),
      },
      {
        key: "filter-team",
        group: "filter-author",
        label: t("TeamMember"),
        isHeader: true,
      },
      {
        key: "team-me",
        group: "filter-author",
        label: t("Me"),
      },
      {
        key: "user3",
        group: "filter-author",
        label: t("OtherUsers"),
        isSelector: true,
        defaultOptionLabel: t("Common:MeLabel"),
        defaultSelectLabel: t("Common:Select"),
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "group",
        group: "filter-author",
        label: groupsCaption,
        defaultSelectLabel: t("Common:Select"),
        isSelector: true,
        selectedItem,
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
  const getSelectedFilterData = () => {
    const selectedFilterData = {
      filterValues: [],
      sortDirection: filter.sortOrder === "ascending" ? "asc" : "desc",
      sortId: filter.sortBy,
    };

    selectedFilterData.inputValue = filter.search;

    // if (filter.status) {
    //   selectedFilterData.filterValues.push({
    //     key: `${filter.status}`,
    //     group: "filter-status",
    //   });
    // }

    // if (filter.follow) {
    //   selectedFilterData.filterValues.push({
    //     key: `follow`,
    //     group: "filter-settings",
    //   });
    // }

    if (filter.filterType >= 0) {
      selectedFilterData.filterValues.push({
        key: `${filter.filterType}`,
        group: "filter-filterType",
      });
    }
    console.log(selectedFilterData);

    return selectedFilterData;
  };

  const selectedFilterData = getSelectedFilterData();

  return !tReady ? (
    <Loaders.Filter />
  ) : (
    <FilterInput
      getFilterData={getData}
      getSortData={getSortData}
      onFilter={onFilter}
      selectedFilterData={selectedFilterData}
      viewAs={false}
      isReady={tReady}
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

export default inject(({ auth, projectsStore, projectsFilterStore }) => {
  const { customNames } = auth.settingsStore;
  const { fetchProjects } = projectsFilterStore;
  const { user } = auth.userStore;
  const { filter } = projectsStore;
  return {
    customNames,
    filter,
    user,
    fetchProjects,
  };
})(observer(SectionFilterContent));
