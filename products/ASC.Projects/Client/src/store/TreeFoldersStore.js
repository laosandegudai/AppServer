import { makeAutoObservable } from "mobx";
//import { getFolderTree } from "../components/Article/utils";
import { getFolders } from "@appserver/common/api/projects";
import { FolderType } from "../constants";

class TreeFoldersStore {
  treeFolders = [];
  selectedTreeNode = [];
  expandedKeys = [];
  expandedPanelKeys = null;

  constructor() {
    makeAutoObservable(this);
  }

  getFoldersTree = () => getFolders();

  fetchTreeFolders = async () => {
    const treeFolders = await getFolders();
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

  setExpandedPanelKeys = (expandedPanelKeys) => {
    this.expandedPanelKeys = expandedPanelKeys;
  };

  // здесь делаем проверку на root папку. а не на вложенные
  getRootFolder = (rootFolderType) => {
    return this.treeFolders.find((x) => x.rootFolderType === rootFolderType);
  };
}

export default TreeFoldersStore;
