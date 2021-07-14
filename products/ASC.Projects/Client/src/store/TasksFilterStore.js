const { makeAutoObservable } = require("mobx");
import api from "@appserver/common/api";
import history from "@appserver/common/history";
import { combineUrl } from "@appserver/common/utils";
import { AppServerConfig } from "@appserver/common/constants";
import config from "../../package.json";
import { FolderKey } from "../constants";

const { TasksFilter } = api;

class TasksFilterStore {
  tasks;
  filter = TasksFilter.getDefault();

  constructor(projectsStore, treeFoldersStore) {
    this.treeFoldersStore = treeFoldersStore;
    this.projectsStore = projectsStore;
    makeAutoObservable(this);
  }

  setTasksFilter = (filter) => {
    this.setFilterUrl(filter);
    this.filter = filter;
  };

  setTasks = (tasks) => {
    this.tasks = tasks;
  };

  resolveData = async (data, filterData) => {
    const { setSelectedNode } = this.treeFoldersStore;
    setSelectedNode([filterData.folder]);
    this.setTasksFilter(filterData);
    this.setTasks(data);
    this.projectsStore.setItems(this.TaskList);
    const items = {
      items: this.tasks,
    };
    return Promise.resolve(items);
  };

  fetchTasks = (filter, folderName) => {
    console.log(filter);
    const newFilter = filter.clone();
    newFilter.page = 0;
    newFilter.startIndex = 0;

    const filterData = newFilter ? newFilter.clone() : TasksFilter.getDefault;

    filterData.folder = folderName ? folderName : filterData.folder;

    if (
      folderName === FolderKey.MyTasks ||
      filterData.creator === "10000000-0000-0000-0000-000000000000"
    ) {
      filterData.creator = "10000000-0000-0000-0000-000000000000";
      return api.projects
        .getMyTaskList()
        .then(async (data) => this.resolveData(data, filterData));
    }
    if (folderName === FolderKey.Tasks || !folderName) {
      const newFilter = TasksFilter.getDefault();
      newFilter.folder = "tasks";
      return api.projects
        .getAllTaskList(true)
        .then(async (data) => this.resolveData(data, newFilter));
    }
  };

  setFilterUrl = (filter) => {
    const urlFilter = filter.toUrlParams();
    history.push(
      combineUrl(
        AppServerConfig.proxyURL,
        config.homepage,
        `/task/filter?${urlFilter}`
      )
    );
  };

  get TaskList() {
    const list = this.tasks.map((item) => {
      const { title, status, id, creator } = item;

      const firstLinkTitle = `AddSubtask`;
      const secondLinkTitle = `due 12.12.2020`;

      const commonFilterOptions = [];

      return {
        title,
        status,
        id,
        creator,
        firstLinkTitle,
        secondLinkTitle,
      };
    });
    return list;
  }
}

export default TasksFilterStore;
