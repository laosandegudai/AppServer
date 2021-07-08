import ProjectsStore from "./ProjectsStore";
import TreeFoldersStore from "./TreeFoldersStore";
import store from "studio/store";
import ProjectsFilterStore from "./ProjectsFilterStore";
import TasksFilterStore from "./TasksFilterStore";

const treeFoldersStore = new TreeFoldersStore();
const projectsStore = new ProjectsStore(
  store.auth,
  store.auth.settingsStore,
  store.auth.userStore,
  treeFoldersStore
);
const projectsFilterStore = new ProjectsFilterStore(
  projectsStore,
  treeFoldersStore
);
const tasksFilterStore = new TasksFilterStore(projectsStore, treeFoldersStore);
const stores = {
  projectsStore,
  treeFoldersStore,
  projectsFilterStore,
  tasksFilterStore,
};

export default stores;
