import ProjectsStore from "./ProjectsStore";
import TreeFoldersStore from "./TreeFoldersStore";
import store from "studio/store";
import FilterStore from "./FilterStore";

const treeFoldersStore = new TreeFoldersStore();
const filterStore = new FilterStore();

const projectsStore = new ProjectsStore(
  store.auth,
  store.auth.settingsStore,
  store.auth.userStore,
  treeFoldersStore,
  filterStore
);

const stores = {
  projectsStore,
  treeFoldersStore,
  filterStore,
};

export default stores;
