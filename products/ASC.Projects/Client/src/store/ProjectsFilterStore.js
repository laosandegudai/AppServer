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
    this.setProjects([]);

    filterData.total = data.total;

    this.setProjectFilter(filterData);
    this.projectsStore.setFilter(filterData);
    setSelectedNode([filterData.folder]);

    if (data.items) {
      this.setProjects(data.items);
    }

    this.projectsStore.setItems(this.projectList);
    const items = {
      items: this.projectList,
    };
    return Promise.resolve(items);
  };

  fetchProjects = (filter, folderName, isFolderSelect) => {
    //const newFilter = filter.clone();
    // вот здесь нужно скорее всего убрать
    // newFilter.page = 0;
    // newFilter.startIndex = 0;

    const filterData = filter ? filter.clone() : ProjectsFilter.getDefault();

    filterData.folder = folderName ? folderName : filterData.folder;

    if (isFolderSelect) {
      switch (filterData.folder) {
        case FolderKey.ProjectsFollowed:
          filterData.follow = true;
          filterData.status = "open";
          break;

        case FolderKey.ProjectsActive:
          filterData.status = "open";
          break;

        case FolderKey.MyProjects:
          filterData.participant = this.projectsStore.userStore.user.id;
          filterData.status = "open";
          break;

        default:
          break;
      }
      return api.projects
        .getProjectsList(filterData)
        .then(async (data) => this.resolveData(data, filterData));
    }

    return api.projects
      .getProjectsList(filter)
      .then(async (data) => this.resolveData(data, filterData));
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
        key: "create_on",
        label: translation.byCreationDate,
        default: true,
      },
      { key: "title", label: translation.byTitle, default: true },
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
        key: "filter-author-manager",
        group: "filter-author-manager",
        label: translation.byProjectManager,
        isHeader: true,
      },
      {
        key: "user-manager-me",
        group: "filter-author-manager",
        label: translation.me,
        // isSelector: true,
        //defaultOptionLabel: translation.meLabel,
        //defaultSelectLabel: translation.select,
        //groupsCaption,
        //defaultOption: user,
        //selectedItem,
      },
      {
        key: "user-manager-other",
        group: "filter-author-manager",
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
        group: "follow",
        label: translation.followed,
      },
      {
        key: "tag",
        group: "tag",
        label: translation.withTag,
      },
      {
        key: "notag",
        group: "notag",
        label: translation.withoutTag,
      },
      {
        key: "filter-author-participant",
        group: "filter-author-participant",
        label: translation.teamMember,
        isHeader: true,
      },
      {
        key: "user-team-member-me",
        group: "filter-author-participant",
        label: translation.me,
      },
      {
        key: "user-team-member-other",
        group: "filter-author-participant",
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
        group: "filter-author-departament",
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
        responsible,
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
        responsible,
      };
    });
    return list;
  }
}

export default ProjectsFilterStore;
