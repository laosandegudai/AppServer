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

function randomNum(min, max) {
  let rand = min - 0.5 + Math.random() * (max - min + 1);
  return Math.round(rand);
}

const generateProjects = () => {
  const generate = (id) => {
    const project = {
      title: faker.commerce.productName(),
      projectFolder: faker.datatype.number(),
      id: faker.datatype.number(),
      follow: faker.datatype.boolean(),
      taskCount: randomNum(0, 15),
      participantCount: randomNum(0, 20),
      status: randomNum(0, 2),
      createdBy: {
        id: id || faker.datatype.uuid(),
        displayName: faker.name.findName(),
        title: "Manager",
      },
    };
    return project;
  };

  const allProjects = Array.from({ length: 23 }, generate);

  const myProjects = Array.from({ length: 5 }, () =>
    generate("00000000-0000-0000-0000-000000000000")
  );
  return [...allProjects, ...myProjects];
};

const generateTasks = (creatorId) => {
  const task = {
    title: faker.internet.userName(),
    status: randomNum(0, 1),
    id: faker.datatype.number(),
    creator: creatorId || faker.datatype.uuid(),
  };

  return task;
};

const allTasks = Array.from({ length: 4 }, generateTasks);
const myTasks = Array.from({ length: 2 }, () =>
  generateTasks("00000000-0000-0000-0000-000000000000")
);

const projects = generateProjects();

export function getFolders() {
  return Promise.resolve(folderTree);
}

export function getAllProjects(page = 0) {
  console.log(page);
  return page === 0
    ? Promise.resolve(projects.slice(0, 25))
    : Promise.resolve(projects.slice(25));
  // return Promise.resolve(projects);
}

export function getMyProjects() {
  const fakeUserId = "00000000-0000-0000-0000-000000000000";
  return Promise.resolve(projects.filter((i) => i.createdBy.id === fakeUserId));
}

export function getFollowedProjects() {
  return Promise.resolve(projects.filter((i) => i.follow));
}

export function getActiveProjects() {
  return Promise.resolve(projects.filter((i) => i.status === 0));
}

export function getPausedProjects() {
  return Promise.resolve(projects.filter((i) => i.status === 1));
}

export function getClosedProjects() {
  return Promise.resolve(projects.filter((i) => i.status === 2));
}

export function getAllTasks() {
  return Promise.resolve(allTasks);
}

export function getMyTasks() {
  return Promise.resolve(myTasks);
}
