import { action, computed, makeObservable, observable } from "mobx";
import history from "@appserver/common/history";
import { combineUrl } from "@appserver/common/utils";
import { AppServerConfig } from "@appserver/common/constants";
import config from "../../package.json";
import api from "@appserver/common/api";
import { FolderKey } from "../constants";

const { ProjectsFilter } = api;

class ProjectsFilterStore {
  filter = ProjectsFilter.getDefault();
  folder = [];
  projects = [];
  projectsStore;

  constructor(projectsStore, treeFoldersStore) {
    this.projectsStore = projectsStore;
    this.treeFoldersStore = treeFoldersStore;
    makeObservable(this, {
      projects: observable,
      setProjects: action,
      fetchProjects: action,

      projectList: computed,
    });
  }

  setProjectFilter = (filter) => {
    this.setFilterUrl(filter);
    this.filter = filter;
  };

  // пока функции повторяются.
  resolveData = async (data, filterData) => {
    const { setSelectedNode } = this.treeFoldersStore;
    console.log(filterData);
    this.setProjects([]);
    this.setProjectFilter(filterData);
    this.projectsStore.setFilter(filterData);
    setSelectedNode([filterData.folder]);
    this.setProjects(data.items);
    this.projectsStore.setItems(this.projectList);
    const items = {
      items: this.projectList,
    };
    return Promise.resolve(items);
  };

  fetchProjects = (filter, folderName) => {
    const newFilter = filter.clone();
    // вот здесь нужно скорее всего убрать
    newFilter.page = 0;
    newFilter.startIndex = 0;

    const filterData = newFilter
      ? newFilter.clone()
      : ProjectsFilter.getDefault;

    filterData.folder = folderName ? folderName : filterData.folder;

    if (
      filterData.status === "open" ||
      folderName === FolderKey.ProjectsActive
    ) {
      filterData.status = "open";
      // for example
      filterData.total = 10;
      return api.projects
        .getActiveProjectsList(true)
        .then(async (data) => this.resolveData(data, filterData));
    }

    if (filterData.status === "closed") {
      filterData.total = 10;
      return api.projects
        .getClosedProjectsList(true)
        .then(async (data) => this.resolveData(data, filterData));
    }

    if (filterData.status === "paused") {
      filterData.total = 10;
      return api.projects
        .getPausedProjectsList()
        .then(async (data) => this.resolveData(data, filterData));
    }
    if (filterData.follow || folderName === FolderKey.ProjectsFollowed) {
      filterData.follow = true;
      filterData.total = 10;
      return api.projects
        .getFollowedProjectsList(true)
        .then(async (data) => this.resolveData(data, filterData));
    }
    // for example
    if (
      filterData.manager === "10000000-0000-0000-0000-000000000000" ||
      FolderKey.MyProjects === folderName
    ) {
      filterData.manager = "10000000-0000-0000-0000-000000000000";
      return api.projects
        .getMyProjectsList(true)
        .then(async (data) => this.resolveData(data, filterData));
    }

    if (folderName === FolderKey.Projects || !folderName) {
      const newFilter = ProjectsFilter.getDefault();
      newFilter.folder = "projects";
      // тест пагинации
      newFilter.total = 27;
      newFilter.page = filter.page;
      return api.projects
        .getAllProjectsList(true, newFilter)
        .then(async (data) => this.resolveData(data, newFilter));
    }
  };

  setFilter = (filter) => {
    this.filter = filter;
  };

  setFilterUrl = (filter) => {
    const urlFilter = filter.toUrlParams();
    history.push(
      combineUrl(
        AppServerConfig.proxyURL,
        config.homepage,
        `/filter?${urlFilter}`
      )
    );
  };

  setProjects = (projects) => {
    this.projects = projects;
  };

  getProjectFilterSortDataOptions = (translation) => {
    const options = [
      {
        key: "DateAndTimeCreation",
        label: translation.byCreationDate,
        default: true,
      },
      { key: "AZ", label: translation.byTitle, default: true },
    ];

    return options;
  };

  getProjectFilterRowContextOptions = (translation, id) => {
    const options = [
      {
        key: "edit-project",
        label: translation.editProject,
        "data-id": id,
      },
      {
        key: "delete-project",
        label: translation.deleteProject,
        "data-id": id,
      },
    ];

    return options;
  };

  getProjectFilterCommonOptions = (
    translation,
    customNames,
    selectedItem,
    user
  ) => {
    const { usersCaption, groupsCaption } = customNames;
    const options = [
      {
        key: "filter-status",
        group: "filter-status",
        label: translation.status,
        isHeader: true,
      },
      {
        key: "open",
        group: "filter-status",
        label: translation.active,
      },
      {
        key: "paused",
        group: "filter-status",
        label: translation.paused,
      },
      {
        key: "closed",
        group: "filter-status",
        label: translation.closed,
      },
      {
        key: "filter-author",
        group: "filter-author",
        label: translation.byProjectManager,
        isHeader: true,
      },
      {
        key: "user1",
        group: "filter-author",
        label: translation.me,
        isSelector: true,
        defaultOptionLabel: translation.meLabel,
        defaultSelectLabel: translation.select,
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "user2",
        group: "filter-author",
        label: translation.otherUsers,
        isSelector: true,
        defaultOptionLabel: translation.meLabel,
        defaultSelectLabel: translation.select,
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "filter-settings",
        group: "filter-settings",
        label: translation.other,
        isRowHeader: true,
      },
      {
        key: "follow",
        group: "filter-settings",
        label: translation.followed,
      },
      {
        key: "tag",
        group: "filter-settings",
        label: translation.withTag,
      },
      {
        key: "withoutTag",
        group: "filter-settings",
        label: translation.withoutTag,
      },
      {
        key: "filter-team",
        group: "filter-author",
        label: translation.teamMember,
        isHeader: true,
      },
      {
        key: "team-me",
        group: "filter-author",
        label: translation.me,
      },
      {
        key: "user3",
        group: "filter-author",
        label: translation.otherUsers,
        isSelector: true,
        defaultOptionLabel: translation.meLabel,
        defaultSelectLabel: translation.select,
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "group",
        group: "filter-author",
        label: translation.groups,
        defaultSelectLabel: translation.select,
        isSelector: true,
        selectedItem,
      },
    ];
    return options;
  };

  get projectList() {
    const list = this.projects.map((item) => {
      const {
        status,
        id,
        title,
        taskCount,
        follow,
        createdBy,
        participantCount,
      } = item;

      const openTask = `OpenTask: ${taskCount}`;
      const secondLinkTitle = `Team: ${participantCount}`;

      return {
        status,
        id,
        follow,
        title,
        taskCount,
        participantCount,
        createdBy,
        openTask,
        secondLinkTitle,
      };
    });
    return list;
  }
}

export default ProjectsFilterStore;
