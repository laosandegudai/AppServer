import { makeAutoObservable } from "mobx";
import api from "@appserver/common/api";

class ModuleStore {
  peopleModuleUsers = [];
  peopleModuleGroups = [];
  currentTab = "0";
  selection = [];
  selected = "none";

  constructor() {
    makeAutoObservable(this);
  }

  setPeopleModuleUsers = (users) => {
    this.peopleModuleUsers = users;
  };

  setPeopleModuleGroups = (groups) => {
    this.peopleModuleGroups = groups;
  };

  addPeopleModuleUsers = (users) => {
    this.peopleModuleUsers.concat(users);
  };

  addPeopleModuleGroups = (groups) => {
    this.peopleModuleGroups.concat(groups);
  };

  setCurrentTab = (key) => {
    this.currentTab = key;
  };

  getSecuritySettings = (moduleId) => {
    return api.settings.getSecuritySettings(moduleId);
  };

  setSecuritySettings = (moduleId, enabled, userOrGroupIdList) => {
    return api.settings.setSecuritySettings(
      moduleId,
      enabled,
      userOrGroupIdList
    );
  };

  setSelection = (selection) => {
    this.selection = selection;
  };

  selectUser = (user) => {
    const newSelection = [...this.selection, user];
    this.setSelection(newSelection);
    return this.selection;
  };

  deselectUser = (user) => {
    if (!user) {
      this.selected = "none";
      this.selection = [];
      return;
    }

    const newData = this.selection.filter((el) => el.id !== user.id);
    return (this.selection = newData);
  };

  selectAll = () => {
    const list = this.peopleModuleUsers.concat(this.peopleModuleGroups);
    this.setSelection(list);
  };

  clearSelection = () => {
    return this.setSelection([]);
  };

  selectByStatus = (status) => {
    const list = this.peopleStore.usersStore.peopleList.filter(
      (u) => u.status === status
    );

    return (this.selection = list);
  };

  getUserChecked = () => {
    switch (this.selected) {
      case "all":
        return true;
      default:
        return false;
    }
  };

  getUsersBySelected = (users) => {
    let newSelection = [];
    users.forEach((user) => {
      const checked = this.getUserChecked();

      if (checked) newSelection.push(user);
    });

    return newSelection;
  };

  isUserSelected = (userId) => {
    return this.selection.some((el) => el.id === userId);
  };

  setSelected = (selected) => {
    this.selected = selected;
    this.setSelection(this.getUsersBySelected(this.peopleModuleUsers));
    this.setSelection(this.getUsersBySelected(this.peopleModuleGroups));

    return selected;
  };

  get isHeaderVisible() {
    return !!this.selection.length || this.selected !== "none";
  }

  get isHeaderIndeterminate() {
    console.log("RUN isHeaderIndeterminate");
    return (
      this.isHeaderVisible &&
      !!this.selection.length &&
      this.selection.length <
        this.peopleModuleUsers.length + this.peopleModuleGroups.length
    );
  }

  get isHeaderChecked() {
    return (
      this.isHeaderVisible &&
      this.selection.length ===
        this.peopleModuleUsers.length + this.peopleModuleGroups.length
    );
  }
}

export default ModuleStore;
