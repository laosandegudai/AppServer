import CrmStore from "./CrmStore";
import TreeFoldersStore from "./TreeFoldersStore";
import SettingsStore from "./SettingsStore";

const crmStore = new CrmStore();
const treeFoldersStore = new TreeFoldersStore();
const settingsStore = new SettingsStore();

const stores = {
  crmStore,
  treeFoldersStore,
  settingsStore,
};

export default stores;
