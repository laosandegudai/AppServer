import { getObjectByLocation, toUrlParams } from "../../utils";

const DEFAULT_PAGE = 0;
const DEFAULT_PAGE_COUNT = 25;
const DEFAULT_TOTAL = 0;
const DEFAULT_START_INDEX = 0;
const DEFAULT_SORT_BY = "created";
const DEFAULT_SORT_ORDER = "descending";
const DEFAULT_CONTACT_STAGE = -1;
const DEFAULT_CONTACT_TYPE = -1;
const DEFAULT_SEARCH = "";
const DEFAULT_ACCESSIBILITY_TYPE = null;
const DEFAULT_CONTACT_LIST_VIEW = null;
const DEFAULT_MANAGER = null;

const CONTACT_TYPE = "contactType";
const PAGE = "page";
const PAGE_COUNT = "Сount";
const SORT_BY = "sortBy";
const SORT_ORDER = "sortOrder";
const START_INDEX = "StartIndex";
const SEARCH = "search";
const CONTACT_STAGE = "contactStage";
const ACCESSIBILITY_TYPE = "isShared";
const CONTACT_LIST_VIEW = "contactListView";
const MANAGER = "responsibleid";

class CrmFilter {
  static getDefault() {
    return new CrmFilter({});
  }

  static getFilter(location) {
    if (!location) return this.getDefault();

    const urlFilter = getObjectByLocation(location);

    if (!urlFilter) return null;

    const defaultFilter = CrmFilter.getDefault();

    const sortBy = urlFilter[SORT_BY] || defaultFilter.sortBy;
    const sortOrder = urlFilter[SORT_ORDER] || defaultFilter.sortOrder;
    const pageCount =
      (urlFilter[PAGE_COUNT] && +urlFilter[PAGE_COUNT]) ||
      defaultFilter.pageCount;
    const startIndex = urlFilter[START_INDEX] || defaultFilter.startIndex;
    const total = defaultFilter.total;
    const contactType = urlFilter[CONTACT_TYPE] || defaultFilter.contactType;
    const contactStage = urlFilter[CONTACT_STAGE] || defaultFilter.contactStage;
    const isShared = urlFilter[ACCESSIBILITY_TYPE] || defaultFilter.isShared;
    const contactListView =
      urlFilter[CONTACT_LIST_VIEW] || defaultFilter.contactListView;
    const manager = urlFilter[MANAGER] || defaultFilter.manager;
    const search = urlFilter[SEARCH] || defaultFilter.search;

    const newFilter = new CrmFilter({
      sortBy,
      sortOrder,
      startIndex,
      contactType,
      contactStage,
      pageCount,
      total,
      isShared,
      contactListView,
      manager,
      search,
    });

    return newFilter;
  }

  constructor({
    sortBy = DEFAULT_SORT_BY,
    sortOrder = DEFAULT_SORT_ORDER,
    startIndex = DEFAULT_START_INDEX,
    pageCount = DEFAULT_PAGE_COUNT,
    total = DEFAULT_TOTAL,
    page = DEFAULT_PAGE,
    contactType = DEFAULT_CONTACT_TYPE,
    contactStage = DEFAULT_CONTACT_STAGE,
    search = DEFAULT_SEARCH,
    isShared = DEFAULT_ACCESSIBILITY_TYPE,
    contactListView = DEFAULT_CONTACT_LIST_VIEW,
    manager = DEFAULT_MANAGER,
  }) {
    this.page = page;
    this.pageCount = pageCount;
    this.sortBy = sortBy;
    this.sortOrder = sortOrder;
    this.startIndex = startIndex;
    this.contactType = contactType;
    this.contactStage = contactStage;
    this.search = search;
    this.total = total;
    this.isShared = isShared;
    this.contactListView = contactListView;
    this.manager = manager;
  }

  toApiUrlParams = () => {
    const {
      page,
      pageCount,
      sortBy,
      sortOrder,
      startIndex,
      isShared,
      contactListView,
      manager,
      search,
    } = this;

    let dtoFilter = {
      StartIndex: startIndex,
      sortBy: sortBy,
      pageCount: pageCount,
      sortOrder: sortOrder,
      Count: pageCount,
      isShared: isShared,
      contactListView: contactListView,
      responsibleid: manager,
      filtervalue: (search ?? "").trim(),
    };

    const str = toUrlParams(dtoFilter, true);

    return str;
  };

  toUrlParams = () => {
    const {
      sortBy,
      sortOrder,
      pageCount,
      startIndex,
      isShared,
      contactListView,
      manager,
      search,
    } = this;

    const dtoFilter = {};

    if (isShared) {
      dtoFilter[ACCESSIBILITY_TYPE] = isShared;
    }

    if (contactListView) {
      dtoFilter[CONTACT_LIST_VIEW] = contactListView;
    }

    if (manager) {
      dtoFilter[MANAGER] = manager;
    }

    if (search) {
      dtoFilter[SEARCH] = search.trim();
    }

    dtoFilter[SORT_BY] = sortBy;
    dtoFilter[SORT_ORDER] = sortOrder;
    dtoFilter[START_INDEX] = startIndex;
    dtoFilter[PAGE_COUNT] = pageCount;
    dtoFilter[MANAGER] = manager;

    const str = toUrlParams(dtoFilter, true);

    return str;
  };

  clone() {
    return new CrmFilter({
      page: this.page,
      pageCount: this.pageCount,
      total: this.total,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder,
      startIndex: this.startIndex,
      contactType: this.contactType,
      search: this.search,
      total: this.total,
      isShared: this.isShared,
      contactListView: this.contactListView,
      manager: this.manager,
      search: this.search,
    });
  }
}

export default CrmFilter;
