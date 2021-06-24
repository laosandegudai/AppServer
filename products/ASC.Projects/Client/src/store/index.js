import ProjectsStore from "./ProjectsStore";
import TreeFoldersStore from "./TreeFoldersStore";

import store from "studio/store";

const treeFoldersStore = new TreeFoldersStore();

const projectsStore = new ProjectsStore(
  store.auth,
  store.auth.settingsStore,
  store.auth.userStore,
  treeFoldersStore
);

const stores = {
  projectsStore,
  treeFoldersStore,
};

export default stores;
