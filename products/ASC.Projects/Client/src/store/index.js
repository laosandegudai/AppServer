import ProjectsStore from "./ProjectsStore";
import TreeFoldersStore from "./TreeFoldersStore";
import store from "studio/store";
import ProjectsFilterStore from "./ProjectsFilterStore";

const treeFoldersStore = new TreeFoldersStore();
const projectsFilterStore = new ProjectsFilterStore(
  new ProjectsStore(store.auth, store.auth.settingsStore, store.auth.userStore)
);

const projectsStore = new ProjectsStore(
  store.auth,
  store.auth.settingsStore,
  store.auth.userStore,
  treeFoldersStore,
  projectsFilterStore
  // projectsFilterStore.projectsStore.items
);

const stores = {
  projectsStore,
  treeFoldersStore,
  projectsFilterStore,
};

export default stores;
