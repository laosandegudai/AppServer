import ProjectsStore from "./ProjectsStore";
import TreeFoldersStore from "./TreeFoldersStore";
import store from "studio/store";
import ProjectsFilterStore from "./ProjectsFilterStore";

const treeFoldersStore = new TreeFoldersStore();
const projectsStore = new ProjectsStore(
  store.auth,
  store.auth.settingsStore,
  store.auth.userStore,
  treeFoldersStore
);
const projectsFilterStore = new ProjectsFilterStore(projectsStore);
const stores = {
  projectsStore,
  treeFoldersStore,
  projectsFilterStore,
};

export default stores;
