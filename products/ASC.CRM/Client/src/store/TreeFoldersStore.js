import { makeAutoObservable } from "mobx";
import { getFoldersTree } from "@appserver/common/api/crm";

class TreeFoldersStore {
  treeFolders = [];

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
}

export default TreeFoldersStore;
