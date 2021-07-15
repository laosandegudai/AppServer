import { action, makeObservable, observable } from "mobx";
import config from "../../package.json";
import { updateTempContent } from "@appserver/common/utils";
import api from "@appserver/common/api";
import { getProjectStatus } from "../helpers/projects-helper";

const { ProjectsFilter } = api;

class ProjectsStore {
  authStore;
  settingsStore;
  userStore;
  projectsFilterStore;
  filter = ProjectsFilter.getDefault();
  treeFoldersStore;
  isLoading = false;
  isLoaded = false;
  isInit = false;
  items = [];
  selection = [];
  selected = "close";
  // filterCommonOptions = [];

  firstLoad = true;

  constructor(
    authStore,
    settingsStore,
    userStore,
    treeFoldersStore,
    projectsFilterStore
  ) {
    this.authStore = authStore;
    this.settingsStore = settingsStore;
    this.userStore = userStore;
    this.treeFoldersStore = treeFoldersStore;
    this.projectsFilterStore = projectsFilterStore;

    makeObservable(this, {
      isLoading: observable,
      firstLoad: observable,
      items: observable,
      selection: observable,
      selected: observable,
      // filterCommonOptions: observable,
      filter: observable,
      isLoaded: observable,
      setIsLoading: action,
      setSelected: action,
      setSelection: action,
      setFirstLoad: action,
      setIsLoaded: action,
      setItems: action,
      init: action,
      setFilter: action,
      // setFilterCommonOptions: action,
    });
  }

  init = () => {
    //if (this.isInit) return;

    const { isAuthenticated } = this.authStore;
    const { getPortalCultures, setModuleInfo } = this.settingsStore;
    const { fetchTreeFolders } = this.treeFoldersStore;

    setModuleInfo(config.homepage, config.id);

    const requests = [];

    updateTempContent();

    if (!isAuthenticated) {
      return this.setIsLoaded(true);
    } else {
      updateTempContent(isAuthenticated);
    }

    requests.push(getPortalCultures(), fetchTreeFolders());

    this.setIsLoaded(true);

    return Promise.all(requests).then(() => {
      this.setIsLoaded(true);
      this.isInit = true;
    });
  };

  setIsLoading = (loading) => {
    this.isLoading = loading;
  };

  setFirstLoad = (firstLoad) => {
    this.firstLoad = firstLoad;
  };

  setIsLoaded = (isLoaded) => {
    this.isLoaded = isLoaded;
  };

  setItems = (items) => {
    this.items = items;
  };

  setFilter = (filter) => {
    console.log(filter);
    this.filter = filter;
  };

  selectProject = (item) => {
    console.log(item);
    return this.selection.push(item);
  };

  deselectProject = (item) => {
    const newData = this.selection.filter((el) => el.id !== item.id);
    return (this.selection = newData);
  };

  getProjectsChecked = (item, selected) => {
    const status = getProjectStatus(item);
    console.log(status);
    switch (selected) {
      case "all":
        return true;
      case "active":
        return status === "active";
      case "paused":
        return status === "paused";
      case "closed":
        return status === "closed";
      default:
        return false;
    }
  };

  getItemsBySelected = (items, selected) => {
    let newSelection = [];
    items.forEach((item) => {
      const checked = this.getProjectsChecked(item, selected);
      if (checked) newSelection.push(item);
    });

    return newSelection;
  };

  setSelected = (selected) => {
    this.selected = selected;
    const items = this.items;
    this.selection = this.getItemsBySelected(items, selected);
    console.log(this.selection);
  };

  setSelection = (selection) => {
    this.selection = selection;
  };

  get isHeaderIndeterminate() {
    return (
      this.isHeaderVisible &&
      !!this.selection.length &&
      this.selection.length < this.items.length
    );
  }

  get isHeaderVisible() {
    return this.selection.length > 0 || this.selected !== "close";
  }

  get isHeaderChecked() {
    const items = [...this.items];
    return this.isHeaderVisible && this.selection.length === items.length;
  }
}

export default ProjectsStore;
