import { getFilterProjects } from "@appserver/common/api/projects";
import { action, computed, makeObservable, observable } from "mobx";
import { ProjectStatus } from "../constants";

class FilterStore {
  filterItems = [];

  constructor() {
    makeObservable(this, {
      filterItems: observable,
      setFilterItems: action,
      fetchProjectsItems: action,
      projectList: computed,
    });
  }

  get projectList() {
    const list = this.filterItems.map((item) => {
      const {
        status,
        id,
        title,
        created,
        description,
        canEdit,
        canDelete,
        taskCount,
        taskCountTotal,
        participantCount,
      } = item;

      return {
        status,
        id,
        title,
        created,
        description,
        canEdit,
        canDelete,
        taskCount,
        taskCountTotal,
        participantCount,
      };
    });
    return list;
  }

  fetchProjectsItems = async () => {
    const projectItems = await getFilterProjects();
    this.setFilterItems(projectItems);

    return projectItems;
  };

  setFilterItems = (filterItems) => {
    this.filterItems = filterItems;
  };
}

export default FilterStore;
