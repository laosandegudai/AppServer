export const FolderType = Object.freeze({
  Projects: 0,
  Milestones: 1,
  Tasks: 2,
  Discussions: 3,
  GanttChart: 4,
  TimeTracking: 5,
  Documents: 6,
  Reports: 7,
  ProjectsTemplate: 8,
});

export const ProjectStatus = Object.freeze({
  Default: 0,
});

export const FolderKey = Object.freeze({
  Projects: "projects",
  MyProjects: "my-projects",
  ProjectsFollowed: "projects-followed",
  ProjectsActive: "projects-active",
  Tasks: "tasks",
  MyTasks: "my-tasks",
  TasksUpcoming: "tasks-upcoming",
});

export const ProjectOptionStatus = Object.freeze({
  Active: 0,
  Closed: 1,
  Paused: 2,
});

export const TaskOptionStatus = Object.freeze({
  Active: 1,
  Closed: 2,
});
