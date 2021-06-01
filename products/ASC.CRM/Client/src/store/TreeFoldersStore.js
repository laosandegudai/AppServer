import { makeAutoObservable } from "mobx";
import { getFoldersTree } from "@appserver/common/api/crm";

class TreeFoldersStore {
  treeFolders = [];
  selectedTreeNode = [];
  expandedKeys = [];

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
}

export default TreeFoldersStore;
