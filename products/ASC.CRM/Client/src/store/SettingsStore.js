import { makeAutoObservable } from "mobx";

class SettingsStore {
  expandedSetting = null;

  constructor() {
    makeAutoObservable(this);
  }

  setExpandSettingsTree = (expandedSetting) => {
    this.expandedSetting = expandedSetting;
  };
}

export default SettingsStore;
