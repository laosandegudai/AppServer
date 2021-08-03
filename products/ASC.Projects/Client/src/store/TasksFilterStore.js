const { makeAutoObservable } = require("mobx");
import api from "@appserver/common/api";
import history from "@appserver/common/history";
import { combineUrl } from "@appserver/common/utils";
import { AppServerConfig } from "@appserver/common/constants";
import config from "../../package.json";
import { FolderKey } from "../constants";
import moment from "moment";

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

    filterData.total = data.total;

    this.setTasksFilter(filterData);
    this.projectsStore.setFilter(filterData);

    if (data.items) {
      this.setTasks(data.items);
    }

    this.projectsStore.setItems(this.TaskList);
    const items = {
      items: this.tasks,
    };
    return Promise.resolve(items);
  };

  fetchTasks = (filter, folderName, isFolderSelect) => {
    const filterData = filter ? filter.clone() : TasksFilter.getDefault;

    filterData.folder = folderName ? folderName : filterData.folder;

    if (isFolderSelect) {
      switch (filterData.folder) {
        case FolderKey.MyTasks:
          filterData.status = "open";
          filterData.participant = this.projectsStore.userStore.user.id;
          break;

        case FolderKey.TasksUpcoming:
          const now = moment();
          filterData.participant = this.projectsStore.userStore.user.id;
          filterData.deadlineStart = now.format();
          filterData.deadlineStop = now.add(1, "week").format();

        default:
          break;
      }

      return api.projects
        .getTasksList(filterData)
        .then(async (data) => this.resolveData(data, filterData));
    }

    return api.projects
      .getTasksList(filter)
      .then(async (data) => this.resolveData(data, filter));
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
      const { title, status, id, creator, createdBy, deadline } = item;

      return {
        title,
        status,
        id,
        creator,
        createdBy,
        deadline,
      };
    });
    return list;
  }

  getTaskFilterSortDataOptions = (translation) => {
    const options = [
      {
        key: "create_on",
        label: translation.byCreationDate,
        default: true,
      },
      { key: "deadline", label: translation.dueDate, default: false },

      { key: "start_date", label: translation.startDate, default: false },
      { key: "priority", label: translation.priority, default: false },
      { key: "title", label: translation.byTitle, default: false },
      { key: "sort_order", label: translation.order, default: false },
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
    const { groupsCaption } = customNames;
    const options = [
      {
        key: "filter-author-participant",
        group: "filter-author-participant",
        label: translation.responsible,
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
        isSelector: true,
        defaultOptionLabel: translation.meLabel,
        defaultSelectLabel: translation.select,
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "no-responsible",
        group: "filter-author-participant",
        label: translation.noResponsible,
      },
      {
        key: "milestone",
        group: "filter-milestone",
        label: translation.milestone,
        isRowHeader: true,
      },
      {
        key: "filter-my-milestones",
        group: "filter-my-milestones",
        label: translation.milestonesWithMyTasks,
      },
      {
        key: "no-milestone",
        group: "no-milestone",
        label: translation.noMilestone,
      },
      {
        key: "other-milestones",
        group: "filter-milestone",
        label: translation.otherMilestones,
      },
      {
        key: "filter-author-manager",
        group: "filter-author-manager",
        label: translation.creator,
        isHeader: true,
      },
      {
        key: "user-manager-me",
        group: "filter-author-manager",
        label: translation.me,
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
        key: "filter-status",
        group: "filter-status",
        label: translation.status,
        isRowHeader: true,
      },
      {
        key: "open",
        group: "filter-status",
        label: translation.open,
      },
      {
        key: "closed",
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
        group: "filter-my-project",
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
        key: "notag",
        group: "notag",
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
        group: "filter-overdue",
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
