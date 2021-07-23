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
    this.projectsStore.setFilter(filterData);
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

  getTaskFilterSortDataOptions = (translation) => {
    const options = [
      {
        key: "DateAndTimeCreation",
        label: translation.byCreationDate,
        default: true,
      },
      { key: "DueDate", label: translation.dueDate, default: false },

      { key: "StartDate", label: translation.startDate, default: false },
      { key: "Priority", label: translation.priority, default: false },
      { key: "AZ", label: translation.byTitle, default: false },
      { key: "Order", label: translation.order, default: false },
    ];

    return options;
  };

  getTaskFilterRowContextOptions = (translation, id) => {
    const options = [
      {
        key: "accept",
        label: translation.accept,
        "data-id": id,
      },
      {
        key: "add-subtask",
        label: translation.addSubtask,
        "data-id": id,
      },
      {
        key: "moveto-milestone",
        label: translation.moveToMilestone,
        "data-id": id,
      },
      {
        key: "notify",
        label: translation.notifyResponsible,
        "data-id": id,
      },
      {
        key: "track-time",
        label: translation.trackTime,
        "data-id": id,
      },
      {
        isSeparator: true,
        key: "task-separator",
      },
      {
        key: "edit",
        label: translation.edit,
        "data-id": id,
      },
      {
        key: "copy",
        label: translation.copy,
        "data-id": id,
      },
      {
        key: "delete",
        label: translation.delete,
        "data-id": id,
      },
    ];

    return options;
  };

  getTaskFilterCommonOptions = (
    translation,
    customNames,
    selectedItem,
    user
  ) => {
    const { usersCaption, groupsCaption } = customNames;
    const customPeriod = "customPeriod";
    const options = [
      {
        key: "filter-responsible",
        group: "filter-author",
        label: translation.responsible,
        isHeader: true,
      },
      {
        key: "me",
        group: "filter-author",
        label: translation.me,
      },
      {
        key: "user-task",
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
        key: "groups-task",
        group: "filter-author",
        label: translation.groups,
        isSelector: true,
        defaultOptionLabel: translation.meLabel,
        defaultSelectLabel: translation.select,
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "no-responsible",
        group: "filter-author",
        label: translation.noResponsible,
      },
      {
        key: "milestone",
        group: "filter-milestone",
        label: translation.milestone,
        isRowHeader: true,
      },
      {
        key: "milestone-with-task",
        group: "filter-milestone",
        label: translation.milestonesWithMyTasks,
      },
      {
        key: "no-milestone",
        group: "filter-milestone",
        label: translation.noMilestone,
      },
      {
        key: "other-milestones",
        group: "filter-milestone",
        label: translation.otherMilestones,
      },

      {
        key: "empty",
        group: "filter-milestone",
        label: null,
        isSelector: true,
      },
      {
        key: "filter-creator",
        group: "filter-author-creator",
        label: translation.creator,
        isHeader: true,
      },
      {
        key: "filter-creator-me",
        group: "filter-author-creator",
        label: translation.me,
      },
      {
        key: "filter-creator-users",
        group: "filter-author-creator",
        label: translation.otherUsers,
        isSelector: true,
        defaultOptionLabel: translation.meLabel,
        defaultSelectLabel: translation.select,
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "filter-task-status",
        group: "filter-status",
        label: translation.status,
        isRowHeader: true,
      },
      {
        key: "filter-task-status-open",
        group: "filter-status",
        label: translation.open,
      },
      {
        key: "filter-task-status-allClosed",
        group: "filter-status",
        label: translation.allClosed,
      },
      {
        key: "filter-project",
        group: "filter-project",
        label: translation.project,
        isHeader: true,
      },
      {
        key: "filter-my-project",
        group: "filter-project",
        label: translation.myProject,
      },
      {
        key: "filter-other-project",
        group: "filter-project",
        label: translation.otherProjects,
      },
      {
        key: "filter-project-withTag",
        group: "filter-project",
        label: translation.withTag,
      },
      {
        key: "filter-project-withoutTag",
        group: "filter-project",
        label: translation.withoutTag,
      },
      {
        key: "filter-duedate",
        group: "filter-duedate",
        label: translation.dueDate,
        isRowHeader: true,
      },
      {
        key: "filter-overdue",
        group: "filter-duedate",
        label: translation.overdue,
      },
      {
        key: "filter-today",
        group: "filter-duedate",
        label: translation.today,
      },
      {
        key: "filter-upcoming",
        group: "filter-duedate",
        label: translation.upcoming,
      },
      {
        key: "custom-period",
        group: "filter-duedate",
        label: translation.customPeriod,
        isSelector: true,
        defaultOptionLabel: translation.meLabel,
        defaultSelectLabel: translation.select,
        selectedItem,
      },
    ];
    return options;
  };
}

export default TasksFilterStore;
