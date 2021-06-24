const folderTree = [
  {
    id: "folder-projects",
    key: "projects",
    title: "Projects",
    rootFolderName: "@projects",
    rootFolderType: 1,
    folders: [
      { title: "My Projects", rootFolderType: 0, key: "projects1" },
      { title: "Followed", rootFolderType: 0, key: "projects2" },
      { title: "Active", rootFolderType: 0, key: "projects3" },
    ],
  },
  {
    id: "folder-milestones",
    key: "milestones",
    title: "Milestones",
    rootFolderName: "@milestones",
    rootFolderType: 2,
    folders: [
      {
        title: "Milestones with My Tasks",
        rootFolderType: 2,
        key: "milestones1",
      },
      { title: "Upcoming", rootFolderType: 0, key: "milestones2" },
    ],
  },
  {
    id: "folder-tasks",
    key: "tasks",
    title: "Tasks",
    rootFolderName: "@tasks",
    rootFolderType: 3,
    folders: [
      { title: "My Tasks", rootFolderType: 0, key: "tasks1" },
      { title: "Upcoming", rootFolderType: 0, key: "tasks2" },
    ],
  },
  {
    id: "folder-discussions",
    key: "discussions",
    title: "Discussions",
    rootFolderName: "@discussions",
    rootFolderType: 4,
    folders: [
      { title: "My Discussions", rootFolderType: 0, key: "discussions1" },
      { title: "Latest", rootFolderType: 0, key: "discussions2" },
    ],
  },
  {
    id: "folder-ganttChart",
    key: "ganttChart",
    title: "Gantt Chart",
    rootFolderName: "@ganttChart",
    rootFolderType: 5,
    folders: [],
  },
  {
    id: "folder-timeTracking",
    key: "timeTracking",
    title: "Time Tracking",
    rootFolderName: "@timeTracking",
    rootFolderType: 6,
    folders: [],
  },
  {
    id: "folder-documents",
    key: "documents",
    title: "Documents",
    rootFolderName: "@documents",
    rootFolderType: 7,
    folders: [],
  },
  {
    id: "folder-reports",
    key: "reports",
    title: "Reports",
    rootFolderName: "@reports",
    rootFolderType: 8,
    folders: [],
  },
  {
    id: "folder-projectsTemplates",
    key: "projectsTemplates",
    title: "Projects Templates",
    rootFolderName: "@projectsTemplates",
    rootFolderType: 9,
    folders: [],
  },
];

export function getFolders() {
  return Promise.resolve(folderTree);
}
