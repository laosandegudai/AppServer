import {
  getFilterProjects,
  getProjectsList,
} from "@appserver/common/api/projects";
import { action, computed, makeObservable, observable } from "mobx";
import history from "@appserver/common/history";
import { combineUrl } from "@appserver/common/utils";
import {
  FolderType,
  FilterType,
  FileType,
  FileAction,
  AppServerConfig,
  FilesFormats,
} from "@appserver/common/constants";
import config from "../../package.json";
import api from "@appserver/common/api";
import { FolderKey } from "../constants";

const { ProjectsFilter } = api;

class ProjectsFilterStore {
  filter = ProjectsFilter.getDefault();
  folder = [];
  projects = [];
  projectsStore;

  constructor(projectsStore) {
    this.projectsStore = projectsStore;
    makeObservable(this, {
      projects: observable,
      fetchAllProjects: action,
      fetchFollowedProjects: action,
      fetchActiveProjects: action,
      fetchMyProjects: action,
      setProjects: action,

      projectList: computed,
    });
  }

  setProjectFilter = (filter) => {
    this.setFilterUrl(filter);
    this.filter = filter;
  };

  fetchAllProjects = (filter, folderId) => {
    const filterData = filter ? filter.clone() : ProjectsFilter.getDefault();
    const request = () => {
      api.projects.getAllProjectsList(true, filter).then(async (data) => {
        this.setProjectFilter(filterData);
        this.setProjects(data);

        return Promise.resolve(data);
      });
    };
    return request();
  };

  // пока функции повторяются.
  resolveData = async (data, filterData) => {
    this.setProjects([]);
    this.setProjectFilter(filterData);
    this.setProjects(data);
    console.log(this.projectList);
    console.log(this.projectsStore);
    this.projectsStore.setItems(data);
  };

  fetchProjects = (filter, folderId) => {
    const newFilter = filter.clone();
    newFilter.page = 0;
    newFilter.startIndex = 0;
    const filterData = newFilter
      ? newFilter.clone()
      : ProjectsFilter.getDefault;

    console.log(filter, folderId);
    if (!folderId) {
      api.projects
        .getAllProjectsList(true)
        .then((data) => this.resolveData(data, filterData));
    }
    switch (folderId) {
      case FolderKey.Projects:
        api.projects
          .getAllProjectsList(true)
          .then((data) => this.resolveData(data, filterData));
        break;
      case FolderKey.MyProjects:
        filterData.manager = "10000000-0000-0000-0000-000000000000";
        api.projects
          .getMyProjectsList(true)
          .then((data) => this.resolveData(data, filterData));
        break;
      case FolderKey.ProjectsFollowed:
        filterData.follow = true;
        api.projects
          .getFollowedProjectsList(true)
          .then((data) => this.resolveData(data, filterData));
        break;
      case FolderKey.ProjectsActive:
        filterData.status = "open";
        api.projects
          .getActiveProjectsList(true)
          .then((data) => this.resolveData(data, filterData));
      default:
        break;
    }
  };

  fetchMyProjects = (filter) => {
    const filterData = filter ? filter.clone() : ProjectsFilter.getDefault;
    api.projects.getMyProjectsList(true).then(async (data) => {
      this.setProjects([]);
      this.setProjectFilter(filterData);
      this.setProjects(this.projectList);
    });
  };
  fetchFollowedProjects = (filter) => {
    const filterData = filter ? filter.clone() : ProjectsFilter.getDefault;
    api.projects
      .getFollowedProjectsList(true, filter, true)
      .then(async (data) => {
        this.setProjects([]);
        this.setProjectFilter(filterData);
        this.setProjects(data);
      });
  };

  fetchActiveProjects = (filter) => {
    const filterData = filter ? filter.clone() : ProjectsFilter.getDefault;
    console.log(filterData);
    api.projects.getActiveProjectsList(true).then(async (data) => {
      this.setProjects([]);
      this.setProjectFilter(filterData);
      this.setProjects(data);
    });
  };

  setFilter = (filter) => {
    this.filter = filter;
  };

  setFilterUrl = (filter) => {
    const urlFilter = filter.toUrlParams();
    console.log(urlFilter);
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
