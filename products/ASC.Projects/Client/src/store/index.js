import ProjectsStore from "./ProjectsStore";
import TreeFoldersStore from "./TreeFoldersStore";

const projectsStore = new ProjectsStore();
const treeFoldersStore = new TreeFoldersStore();

const stores = {
  projectsStore,
  treeFoldersStore,
};

export default stores;
