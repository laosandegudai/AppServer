import { action, makeObservable, observable } from "mobx";
import CrmFilter from "@appserver/common/api/crm/filter";
import config from "../../package.json";
import { combineUrl } from "@appserver/common/utils";
import { AppServerConfig } from "@appserver/common/constants";
import {
  getFilterContactsTypes,
  getFilterTempLevels,
} from "@appserver/common/api/crm";

class FilterStore {
  filter = CrmFilter.getDefault();
  filterContactsTypes = [];
  filterTempLevels = [];

  constructor() {
    makeObservable(this, {
      filter: observable,
      setFilterParams: action,
      setFilterUrl: action,
      resetFilter: action,
      setFilter: action,
      filterContactsTypes: observable,
      filterTempLevels: observable,
    });
  }

  setFilterUrl = (filter) => {
    const urlFilter = filter.toUrlParams();
    window.history.replaceState(
      "",
      "",
      combineUrl(
        AppServerConfig.proxyURL,
        config.homepage,
        `/contact/filter?${urlFilter}`
      )
    );
  };

  setFilterParams = (data) => {
    this.setFilterUrl(data);
    this.setFilter(data);
  };

  resetFilter = () => {
    this.setFilter(CrmFilter.getDefault());
  };

  setFilter = (filter) => {
    this.filter = filter;
  };

  setFilterContactsTypes = (types) => {
    this.filterContactsTypes = types;
  };

  getFilterContactsTypes = async () => {
    const response = await getFilterContactsTypes();
    const types = response.map((item) => item.title);
    this.setFilterContactsTypes(types);

    return types;
  };

  setFilterTempLevels = (levels) => {
    this.filterTempLevels = levels;
  };

  getFilterTempLevels = async () => {
    const response = await getFilterTempLevels();
    const levels = response.map((item) => item.title);
    this.setFilterTempLevels(levels);
    console.log("levels", levels);
    return levels;
  };
}

export default FilterStore;
