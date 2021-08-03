var faker = require("faker");
const folderTree = [
  {
    id: "folder-projects",
    key: "projects",
    title: "Projects",
    rootFolderName: "@projects",
    rootFolderType: 0,
    folders: [
      {
        title: "My Projects",
        rootFolderType: 9,
        parentId: "folder-projects",
        key: "my-projects",
        rootFolderName: "@my-projects",
      },
      {
        title: "Followed",
        rootFolderType: 9,
        parentId: "folder-projects",
        key: "projects-followed",
        rootFolderName: "@projects-followed",
      },
      {
        title: "Active",
        rootFolderType: 9,
        parentId: "folder-projects",
        key: "projects-active",
        rootFolderName: "@projects-active",
      },
    ],
  },
  {
    id: "folder-milestones",
    key: "milestones",
    title: "Milestones",
    rootFolderName: "@milestones",
    rootFolderType: 1,
    folders: [
      {
        title: "Milestones with My Tasks",
        rootFolderType: 9,
        key: "milestones1",
      },
      { title: "Upcoming", rootFolderType: 9, key: "milestones2" },
    ],
  },
  {
    id: "folder-tasks",
    key: "tasks",
    title: "Tasks",
    rootFolderName: "@tasks",
    rootFolderType: 2,
    folders: [
      { title: "My Tasks", rootFolderType: 9, key: "my-tasks" },
      { title: "Upcoming", rootFolderType: 9, key: "tasks-upcoming" },
    ],
  },
  {
    id: "folder-discussions",
    key: "discussions",
    title: "Discussions",
    rootFolderName: "@discussions",
    rootFolderType: 3,
    folders: [
      { title: "My Discussions", rootFolderType: 9, key: "discussions1" },
      { title: "Latest", rootFolderType: 9, key: "discussions2" },
    ],
  },
  {
    id: "folder-ganttChart",
    key: "ganttChart",
    title: "Gantt Chart",
    rootFolderName: "@ganttChart",
    rootFolderType: 4,
    folders: [],
  },
  {
    id: "folder-timeTracking",
    key: "timeTracking",
    title: "Time Tracking",
    rootFolderName: "@timeTracking",
    rootFolderType: 5,
    folders: [],
  },
  {
    id: "folder-documents",
    key: "documents",
    title: "Documents",
    rootFolderName: "@documents",
    rootFolderType: 6,
    folders: [],
  },
  {
    id: "folder-reports",
    key: "reports",
    title: "Reports",
    rootFolderName: "@reports",
    rootFolderType: 7,
    folders: [],
  },
  {
    id: "folder-projectsTemplates",
    key: "projectsTemplates",
    title: "Projects Templates",
    rootFolderName: "@projectsTemplates",
    rootFolderType: 8,
    folders: [],
  },
];

export function getFolders() {
  return Promise.resolve(folderTree);
}
