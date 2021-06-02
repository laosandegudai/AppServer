import { makeAutoObservable } from "mobx";
import { getFoldersTree } from "@appserver/common/api/crm";

class TreeFoldersStore {
  treeFolders = [];
  selectedTreeNode = [];
  expandedKeys = [];

  title = null;

  constructor() {
    makeAutoObservable(this);
  }

  fetchTreeFolders = async () => {
    const treeFolders = await getFoldersTree();
    this.setTreeFolders(treeFolders);

    return treeFolders;
  };

  setTreeFolders = (treeFolders) => {
    this.treeFolders = treeFolders;
  };

  setSelectedNode = (node) => {
    if (node[0]) {
      this.selectedTreeNode = node;
    }
  };

  setExpandedKeys = (expandedKeys) => {
    this.expandedKeys = expandedKeys;
  };

  addExpandedKeys = (item) => {
    this.expandedKeys.push(item);
  };

  addExpandSettingsTree = (item) => {
    this.expandedKeys.push(item[0]);
  };

  setTitle = (title) => {
    this.title = title;
  };
}

export default TreeFoldersStore;
