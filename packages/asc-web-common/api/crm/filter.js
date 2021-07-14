import { getObjectByLocation, toUrlParams } from "../../utils";

const DEFAULT_PAGE = 0;
const DEFAULT_PAGE_COUNT = 25;
const DEFAULT_TOTAL = 0;
const DEFAULT_START_INDEX = 0;
const DEFAULT_SORT_BY = "created";
const DEFAULT_SORT_ORDER = "descending";
const DEFAULT_CONTACT_STAGE = -1;
const DEFAULT_CONTACT_TYPE = -1;
const DEFAULT_SEARCH = null;
const DEFAULT_ACCESSIBILITY_TYPE = null;
const DEFAULT_CONTACT_LIST_VIEW = null;
const DEFAULT_AUTHOR_TYPE = null;
const DEFAULT_FROM_DATE_TYPE = null;
const DEFAULT_TO_DATE_TYPE = null;
const DEFAULT_SELECTED_ITEM = {};
// const DEFAULT_TAGS_TYPE = ["Клиент", "Конкурент"];

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
const AUTHOR_TYPE = "authorType";
const FROM_DATE_TYPE = "fromDate";
const TO_DATE_TYPE = "toDate";
const TAGS_TYPE = "tags";

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
    const count =
      (urlFilter[PAGE_COUNT] && +urlFilter[PAGE_COUNT]) || defaultFilter.count;
    const startIndex = urlFilter[START_INDEX] || defaultFilter.startIndex;
    const total = defaultFilter.total;
    const contactType = urlFilter[CONTACT_TYPE] || defaultFilter.contactType;
    const contactStage = urlFilter[CONTACT_STAGE] || defaultFilter.contactStage;
    const isShared = urlFilter[ACCESSIBILITY_TYPE] || defaultFilter.isShared;
    const contactListView =
      urlFilter[CONTACT_LIST_VIEW] || defaultFilter.contactListView;
    const authorType =
      (urlFilter[AUTHOR_TYPE] &&
        urlFilter[AUTHOR_TYPE].includes("_") &&
        urlFilter[AUTHOR_TYPE]) ||
      defaultFilter.authorType;
    const search = urlFilter[SEARCH] || defaultFilter.search;
    const fromDate = urlFilter[FROM_DATE_TYPE] || defaultFilter.fromDate;
    const toDate = urlFilter[TO_DATE_TYPE] || defaultFilter.toDate;
    const selectedItem = defaultFilter.selectedItem;
    // const tags = urlFilter[TAGS_TYPE] || defaultFilter.tags;

    const newFilter = new CrmFilter({
      sortBy,
      sortOrder,
      startIndex,
      contactType,
      contactStage,
      count,
      total,
      isShared,
      contactListView,
      authorType,
      search,
      fromDate,
      toDate,
      selectedItem,
      // tags,
    });

    return newFilter;
  }

  constructor({
    sortBy = DEFAULT_SORT_BY,
    sortOrder = DEFAULT_SORT_ORDER,
    startIndex = DEFAULT_START_INDEX,
    count = DEFAULT_PAGE_COUNT,
    total = DEFAULT_TOTAL,
    page = DEFAULT_PAGE,
    contactType = DEFAULT_CONTACT_TYPE,
    contactStage = DEFAULT_CONTACT_STAGE,
    search = DEFAULT_SEARCH,
    isShared = DEFAULT_ACCESSIBILITY_TYPE,
    contactListView = DEFAULT_CONTACT_LIST_VIEW,
    authorType = DEFAULT_AUTHOR_TYPE,
    selectedItem = DEFAULT_SELECTED_ITEM,
    fromDate = DEFAULT_FROM_DATE_TYPE,
    toDate = DEFAULT_TO_DATE_TYPE,
    // tags = DEFAULT_TAGS_TYPE,
  }) {
    this.page = page;
    this.count = count;
    this.sortBy = sortBy;
    this.sortOrder = sortOrder;
    this.startIndex = startIndex;
    this.contactType = contactType;
    this.contactStage = contactStage;
    this.search = search;
    this.total = total;
    this.isShared = isShared;
    this.contactListView = contactListView;
    this.authorType = authorType;
    this.selectedItem = selectedItem;
    this.fromDate = fromDate;
    this.toDate = toDate;
    // this.tags = tags;
  }

  toApiUrlParams = () => {
    const {
      page,
      count,
      sortBy,
      sortOrder,
      startIndex,
      isShared,
      contactListView,
      authorType,
      search,
      fromDate,
      toDate,
      contactStage,
      contactType,
      // tags,
    } = this;

    const responsibleid =
      authorType && authorType.includes("_")
        ? authorType.slice(authorType.indexOf("_") + 1)
        : null;

    const dtoFilter = {
      StartIndex: startIndex,
      sortBy: sortBy,
      sortOrder: sortOrder,
      Count: count,
      isShared: isShared,
      contactListView: contactListView,
      responsibleid,
      filterValue: (search ?? "").trim(),
      fromDate: fromDate,
      toDate: toDate,
      contactStage: contactStage,
      contactType: contactType,
      // tags: tags,
    };

    const str = toUrlParams(dtoFilter, true);

    return str;
  };

  toUrlParams = () => {
    const {
      sortBy,
      sortOrder,
      count,
      startIndex,
      isShared,
      contactListView,
      authorType,
      search,
      fromDate,
      toDate,
      contactStage,
      contactType,
      // tags,
    } = this;

    const dtoFilter = {};

    if (isShared) {
      dtoFilter[ACCESSIBILITY_TYPE] = isShared;
    }

    if (contactListView) {
      dtoFilter[CONTACT_LIST_VIEW] = contactListView;
    }

    if (authorType) {
      dtoFilter[AUTHOR_TYPE] = authorType;
    }

    if (search) {
      dtoFilter[SEARCH] = search.trim();
    }

    if (fromDate && toDate) {
      dtoFilter[FROM_DATE_TYPE] = fromDate;
      dtoFilter[TO_DATE_TYPE] = toDate;
    }

    // if (tags && tags.length) {
    //   dtoFilter[TAGS_TYPE] = tags;
    // }

    dtoFilter[SORT_BY] = sortBy;
    dtoFilter[SORT_ORDER] = sortOrder;
    dtoFilter[START_INDEX] = startIndex;
    dtoFilter[PAGE_COUNT] = count;
    dtoFilter[CONTACT_STAGE] = contactStage;
    dtoFilter[CONTACT_TYPE] = contactType;

    const str = toUrlParams(dtoFilter, true);

    return str;
  };

  clone() {
    return new CrmFilter({
      page: this.page,
      count: this.count,
      total: this.total,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder,
      startIndex: this.startIndex,
      contactType: this.contactType,
      total: this.total,
      isShared: this.isShared,
      contactListView: this.contactListView,
      authorType: this.authorType,
      search: this.search,
      fromDate: this.fromDate,
      toDate: this.toDate,
      selectedItem: this.selectedItem,
      contactStage: this.contactStage,
      // tags: this.tags,
    });
  }
}

export default CrmFilter;
