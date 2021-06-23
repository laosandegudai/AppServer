import ProjectsStore from "./ProjectsStore";
import TreeFoldersStore from "./TreeFoldersStore";
import TreeSettingsStore from "./TreeSettingsStore";

const projectsStore = new ProjectsStore();
const treeFoldersStore = new TreeFoldersStore();
const treeSettingsStore = new TreeSettingsStore();
const stores = {
  projectsStore,
  treeFoldersStore,
  treeSettingsStore,
};

export default stores;
