import { makeAutoObservable } from "mobx";
import { getFolderTree } from "../components/Article/utils";

class TreeFoldersStore {
  treeFolders = [];
  expandedKeys = [];
  expandedPanelKeys = null;

  constructor() {
    makeAutoObservable(this);
  }

  fetchTreeFolders = async () => {
    const treeFolders = await getFolderTree();
    this.setTreeFolders(treeFolders);

    return treeFolders;
  };

  setTreeFolders = (treeFolders) => {
    this.treeFolders = treeFolders;
  };

  setExpandedKeys = (expandedKeys) => {
    this.expandedKeys = expandedKeys;
  };

  setExpandedPanelKeys = (expandedPanelKeys) => {
    this.expandedPanelKeys = expandedPanelKeys;
  };
}

export default TreeFoldersStore;
