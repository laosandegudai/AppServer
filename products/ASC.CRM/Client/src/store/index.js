import CrmStore from "./CrmStore";
import TreeFoldersStore from "./TreeFoldersStore";

const crmStore = new CrmStore();
const treeFoldersStore = new TreeFoldersStore();

const stores = {
  crmStore,
  treeFoldersStore,
};

export default stores;
