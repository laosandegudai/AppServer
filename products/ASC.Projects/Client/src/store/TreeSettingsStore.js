import { makeAutoObservable } from "mobx";
import { getSettingsTree } from "../components/Article/utils";

class TreeSettingsStore {
  treeSettings = [];
  constructor() {
    makeAutoObservable(this);
  }

  fetchTreeSettings = async () => {
    const treeSettings = await getSettingsTree();
    this.setTreeSettings(treeSettings);
    return treeSettings;
  };

  setTreeSettings = (treeSettings) => {
    this.treeSettings = treeSettings;
  };
}

export default TreeSettingsStore;
