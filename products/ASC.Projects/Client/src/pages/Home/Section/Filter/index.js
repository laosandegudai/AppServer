import React, { useEffect, useState } from "react";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import result from "lodash/result";
import find from "lodash/find";
import { withRouter } from "react-router";
import Loaders from "@appserver/common/components/Loaders";
import FilterInput from "@appserver/common/components/FilterInput";
import api from "@appserver/common/api";
import { isMobileOnly } from "react-device-detect";
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
  const {
    t,
    filter,
    customNames,
    fetchProjects,
    tReady,
    getProjectFilterCommonOptions,
    getTaskFilterCommonOptions,
    getProjectFilterSortDataOptions,
    getTaskFilterSortDataOptions,
    sectionWidth,
  } = props;

  const translations = {
    status: t("Status"),
    active: t("Active"),
    paused: t("Paused"),
    closed: t("Closed"),
    byProjectManager: t("ByProjectManager"),
    me: t("Me"),
    meLabel: t("Common:MeLabel"),
    select: t("Common:Select"),
    otherUsers: t("OtherUsers"),
    other: t("Other"),
    followed: t("Followed"),
    withTag: t("WithTag"),
    withoutTag: t("WithoutTag"),
    teamMember: t("TeamMember"),
    groups: t("Groups"),

    responsible: t("Responsible"),
    noResponsible: t("noResponsible"),
    milestone: t("Milestone"),
    milestonesWithMyTasks: t("MilestonesWithMyTasks"),
    noMilestone: t("NoMilestone"),
    otherMilestones: t("otherMilestones"),
    allClosed: t("AllClosed"),
    creator: t("Creator"),
    open: t("Open"),
    project: t("Project"),
    myProject: t("MyProject"),
    otherProjects: t("OtherProjects"),
    dueDate: t("DueDate"),
    overdue: t("Overdue"),
    today: t("Today"),
    upcoming: t("Upcoming"),
    customPeriod: t("CustomPeriod"),

    byCreationDate: t("ByCreationDate"),
    byTitle: t("ByTitle"),
    priority: t("Priority"),
    startDate: t("StartDate"),
    order: t("Order"),
  };

  const onFilter = (data) => {
    // // вообще подумать как это все лучше обрабатывать с несколькими фильтрами
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

    // if (filter instanceof ProjectsFilter) {
    //   const newFilter = filter.clone();
    //   newFilter.page = 0;
    //   newFilter.sortBy = sortBy;
    //   newFilter.sortOrder = sortOrder;
    //   newFilter.search = search;
    //   newFilter.selectedItem = selectedFilterItem;
    //   newFilter.status = status;
    //   console.log(settings);
    //   // сделать здесь обнуление
    //   // newFilter.follow = null;
    //   if (settings === "follow") {
    //     newFilter[settings] = true;
    //   }
    //   fetchProjects(newFilter);
    // }
  };

  const getData = () => {
    const { user } = props;
    const { selectedItem } = filter;
    if (filter instanceof ProjectsFilter) {
      return getProjectFilterCommonOptions(
        translations,
        customNames,
        selectedItem,
        user
      );
    } else if (filter instanceof TasksFilter) {
      return getTaskFilterCommonOptions(
        translations,
        customNames,
        selectedItem,
        user
      );
    }
  };

  const getFilterSortData = () => {
    if (filter instanceof ProjectsFilter) {
      return getProjectFilterSortDataOptions(translations);
    }

    if (filter instanceof TasksFilter) {
      return getTaskFilterSortDataOptions(translations);
    }
  };
  const getSortData = () => {
    const commonOptions = getFilterSortData();

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

    return selectedFilterData;
  };

  const selectedFilterData = getSelectedFilterData();

  return !tReady ? (
    <Loaders.Filter />
  ) : (
    <FilterInput
      sectionWidth={sectionWidth}
      getFilterData={getData}
      getSortData={getSortData}
      onFilter={onFilter}
      selectedFilterData={selectedFilterData}
      isReady={tReady}
      directionAscLabel={t("Common:DirectionAscLabel")}
      directionDescLabel={t("Common:DirectionDescLabel")}
      isMobile={isMobileOnly}
      placeholder={t("Common:Search")}
      {...filterColumnCount}
      contextMenuHeader={t("Common:AddFilter")}
    />
  );
};

const SectionFilterContent = withTranslation(["Home", "Common"])(
  withRouter(PureSectionFilterContent)
);

export default inject(
  ({ auth, projectsStore, projectsFilterStore, tasksFilterStore }) => {
    const { customNames } = auth.settingsStore;
    const {
      fetchProjects,
      getProjectFilterCommonOptions,
      getProjectFilterSortDataOptions,
    } = projectsFilterStore;
    const { user } = auth.userStore;
    const { filter } = projectsStore;

    const {
      getTaskFilterCommonOptions,
      getTaskFilterSortDataOptions,
    } = tasksFilterStore;
    return {
      customNames,
      filter,
      user,
      fetchProjects,
      getProjectFilterCommonOptions,
      getTaskFilterCommonOptions,
      getProjectFilterSortDataOptions,
      getTaskFilterSortDataOptions,
    };
  }
)(observer(SectionFilterContent));
