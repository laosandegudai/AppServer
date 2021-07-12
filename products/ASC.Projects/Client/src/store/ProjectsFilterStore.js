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
    this.setProjectFilter(filterData);
    this.projectsStore.setFilter(filterData);
    setSelectedNode([filterData.folder]);
    this.setProjects(data);
    this.projectsStore.setItems(this.projectList);
    const items = {
      items: this.projectList,
    };
    return Promise.resolve(items);
  };

  fetchProjects = (filter, folderName) => {
    const newFilter = filter.clone();
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
      return api.projects
        .getActiveProjectsList(true)
        .then(async (data) => this.resolveData(data, filterData));
    }

    if (filterData.follow || folderName === FolderKey.ProjectsFollowed) {
      filterData.follow = true;
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

  get projectList() {
    const list = this.projects.map((item) => {
      const {
        status,
        id,
        title,
        created,
        description,
        canEdit,
        canDelete,
        taskCount,
        taskCountTotal,
        participantCount,
      } = item;

      return {
        status,
        id,
        title,
        created,
        description,
        canEdit,
        canDelete,
        taskCount,
        taskCountTotal,
        participantCount,
      };
    });
    return list;
  }
}

export default ProjectsFilterStore;
