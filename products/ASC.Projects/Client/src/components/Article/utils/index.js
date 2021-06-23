import { folderTree } from "./FolderTree";
import { settingsTree } from "./settingsTree";

export const getFolderTree = async () => {
  return Promise.resolve(folderTree);
};

export const getSettingsTree = async () => {
  return Promise.resolve(settingsTree);
};
