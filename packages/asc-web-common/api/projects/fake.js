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

const filterProjects = [
  {
    canDelete: true,
    canEdit: true,
    created: "2021-06-25T15:57:44.0000000+05:00",
    createdById: "da436a14-3f06-45de-8ee6-0957e190bb12",
    description: "213213",
    id: 262363,
    participantCount: 1,
    projectFolder: "2352104",
    taskCount: 2,
    title: "test",
    status: 1,
    updated: "2021-06-25T15:57:44.0000000+05:00",
    updatedById: "da436a14-3f06-45de-8ee6-0957e190bb12",
  },
  {
    canDelete: true,
    canEdit: true,
    created: "2021-06-25T16:00:44.0000000+05:00",
    createdById: "da436a14-3f06-45de-8ee6-0957e190bb12",
    description: "test44444",
    id: 262362,
    participantCount: 2,
    taskCount: 3,
    projectFolder: "2352104",
    title: "project1",
    status: 1,
    updated: "2021-06-25T16:00:44.0000000+05:00",
    updatedById: "da436a14-3f06-45de-8ee6-0957e190bb12",
  },
];

export function getFolders() {
  return Promise.resolve(folderTree);
}

export function getFilterProjects() {
  return Promise.resolve(filterProjects);
}
