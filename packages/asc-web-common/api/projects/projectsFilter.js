import { getObjectByLocation, toUrlParams } from "../../utils";
import queryString from "query-string";

const DEFAULT_PAGE = 0;
const DEFAULT_PAGE_COUNT = 25;
const DEFAULT_TOTAL = 0;
const DEFAULT_SORT_BY = "create_on";
const DEFAULT_SORT_ORDER = "descending";
const DEFAULT_VIEW = "row";
const DEFAULT_SEARCH = null;
const DEFAULT_SELECTED_ITEM = {};
const DEFAULT_TAG = 0;
const DEFAULT_NOTAG = null;
const DEFAULT_STATUS = null;
const DEFAULT_PARTICIPANT = null;
const DEFAULT_MANAGER = null;
const DEFAULT_DEPARTAMENT = null;
const DEFAULT_FOLLOW = null;
const DEFAULT_FOLDER = null;
const DEFAULT_FILTER_TYPE = null;

const PAGE = "page";
const PAGE_COUNT = "count";
const SORT_BY = "sortby";
const SORT_ORDER = "sortorder";
const VIEW_AS = "viewas";
const SEARCH = "search";
const TAG = "tag";
const NOTAG = "notag";
const STATUS = "status";
const PARTICIPANT = "participant";
const MANAGER = "manager";
const DEPARTAMENT = "departament";
const FOLLOW = "follow";
const FOLDER = "folder";
const FILTER_TYPE = "filterType";

class ProjectsFilter {
  static getDefault(total = DEFAULT_TOTAL) {
    return new ProjectsFilter(DEFAULT_PAGE, DEFAULT_PAGE_COUNT, total);
  }

  static getFilter(location) {
    if (!location) return this.getDefault();

    const urlFilter = getObjectByLocation(location);

    if (!urlFilter) return null;

    const defaultFilter = ProjectsFilter.getDefault();

    const filterType =
      (urlFilter[FILTER_TYPE] && +urlFilter[FILTER_TYPE]) ||
      defaultFilter.filterType;

    const page =
      (urlFilter[PAGE] && +urlFilter[PAGE] - 1) || defaultFilter.page;
    const pageCount =
      (urlFilter[PAGE_COUNT] && +urlFilter[PAGE_COUNT]) ||
      defaultFilter.pageCount;

    const sortBy = urlFilter[SORT_BY] || defaultFilter.sortBy;
    const sortOrder = urlFilter[SORT_ORDER] || defaultFilter.sortOrder;
    const viewAs = urlFilter[VIEW_AS] || defaultFilter.viewAs;
    const search = urlFilter[SEARCH] || defaultFilter.search;

    const tag = urlFilter[TAG] || defaultFilter.tag;
    const notag = urlFilter[NOTAG] || defaultFilter.notag;
    const status = urlFilter[STATUS] || defaultFilter.status;
    const participant = urlFilter[PARTICIPANT] || defaultFilter.participant;
    const manager = urlFilter[MANAGER] || defaultFilter.manager;
    const departament = urlFilter[DEPARTAMENT] || defaultFilter.departament;
    const follow = urlFilter[FOLLOW] || defaultFilter.follow;
    const folder = urlFilter[FOLDER] || defaultFilter.folder;

    const newFilter = new ProjectsFilter(
      page,
      pageCount,
      defaultFilter.total,
      sortBy,
      sortOrder,
      viewAs,
      search,
      defaultFilter.selectedItem,
      tag,
      notag,
      status,
      participant,
      manager,
      departament,
      follow,
      folder,
      filterType
    );
    return newFilter;
  }

  constructor(
    page = DEFAULT_PAGE,
    pageCount = DEFAULT_PAGE_COUNT,
    total = DEFAULT_TOTAL,
    sortBy = DEFAULT_SORT_BY,
    sortOrder = DEFAULT_SORT_ORDER,
    viewAs = DEFAULT_VIEW,
    search = DEFAULT_SEARCH,
    selectedItem = DEFAULT_SELECTED_ITEM,
    tag = DEFAULT_TAG,
    notag = DEFAULT_NOTAG,
    status = DEFAULT_STATUS,
    participant = DEFAULT_PARTICIPANT,
    manager = DEFAULT_MANAGER,
    departament = DEFAULT_DEPARTAMENT,
    follow = DEFAULT_FOLLOW,
    folder = DEFAULT_FOLDER,
    filterType = DEFAULT_FILTER_TYPE
  ) {
    this.page = page;
    this.pageCount = pageCount;
    this.total = total;
    this.sortBy = sortBy;
    this.sortOrder = sortOrder;
    this.viewAs = viewAs;
    this.search = search;
    this.selectedItem = selectedItem;
    this.tag = tag;
    this.notag = notag;
    this.status = status;
    this.participant = participant;
    this.manager = manager;
    this.departament = departament;
    this.follow = follow;
    this.folder = folder;
    this.filterType = filterType;
  }

  getStartIndex = () => {
    return this.page * this.pageCount;
  };

  hasNext = () => {
    return this.total - this.getStartIndex() > this.pageCount;
  };

  hasPrev = () => {
    return this.page > 0;
  };

  toApiUrlParams = () => {
    const {
      page,
      pageCount,
      search,
      sortBy,
      sortOrder,
      tag,
      notag,
      follow,
      manager,
      participant,
      status,
      departament,
      folder,
      filterType,
    } = this;

    console.log(tag, notag);

    const dtoFilter = {
      count: pageCount,
      startIndex: this.getStartIndex(),
      page: page,
      sortby: sortBy,
      sortOrder: sortOrder,
      filterValue: (search ?? "").trim(),
      tag: notag ? -1 : tag,
      // notag: notag,
      follow: follow,
      manager: manager,
      participant: participant,
      status: status,
      departament: departament,
      folder: folder,
      filterType: filterType,
    };

    const str = toUrlParams(dtoFilter, true);
    return str;
  };

  toUrlParams = () => {
    const {
      page,
      pageCount,
      search,
      sortBy,
      sortOrder,
      tag,
      notag,
      follow,
      manager,
      participant,
      status,
      departament,
      folder,
      filterType,
    } = this;

    const dtoFilter = {};

    const URLParams = queryString.parse(window.location.href);

    if (search) {
      dtoFilter[SEARCH] = search.trim();
    }

    if (pageCount !== DEFAULT_PAGE_COUNT) {
      dtoFilter[PAGE_COUNT] = pageCount;
    }

    if (tag !== DEFAULT_TAG) {
      tag === -1 ? (dtoFilter["notag"] = true) : (dtoFilter[tag] = tag);
    }
    if (follow !== DEFAULT_FOLLOW) {
      dtoFilter[FOLLOW] = follow;
    }
    if (manager !== DEFAULT_MANAGER) {
      dtoFilter[MANAGER] = manager;
    }
    if (participant !== DEFAULT_PARTICIPANT) {
      dtoFilter[PARTICIPANT] = participant;
    }
    if (status !== DEFAULT_STATUS) {
      dtoFilter[STATUS] = status;
    }
    if (departament !== DEFAULT_DEPARTAMENT) {
      dtoFilter[DEFAULT_DEPARTAMENT] = departament;
    }

    if (notag) {
      dtoFilter["notag"] = true;
    }

    console.log(notag, tag);

    // if (notag) {
    //   dtoFilter[TAG] = -1;
    // }

    if (folder) {
      dtoFilter[FOLDER] = folder;
    }

    if (filterType) {
      dtoFilter[FILTER_TYPE] = filterType;
    }

    if (URLParams.preview) {
      dtoFilter[PREVIEW] = URLParams.preview;
    }

    dtoFilter[PAGE] = page + 1;
    dtoFilter[SORT_BY] = sortBy;
    dtoFilter[SORT_ORDER] = sortOrder;

    console.log(dtoFilter);

    const str = toUrlParams(dtoFilter, true);
    return str;
  };

  clone() {
    return new ProjectsFilter(
      this.page,
      this.pageCount,
      this.total,
      this.sortBy,
      this.sortOrder,
      this.viewAs,
      this.search,
      this.selectedItem,
      this.tag,
      this.notag,
      this.status,
      this.participant,
      this.manager,
      this.departament,
      this.follow,
      this.folder,
      this.filterType
    );
  }

  equals(filter) {
    const equals =
      this.filterType === filter.filterType &&
      this.tag === filter.tag &&
      this.notag === filter.notag &&
      this.manager === filter.manager &&
      this.status === filter.status &&
      this.follow === filter.follow &&
      this.participant === filter.participant &&
      this.departament === filter.departament &&
      this.search === filter.search &&
      this.sortBy === filter.sortBy &&
      this.sortOrder === filter.sortOrder &&
      this.viewAs === filter.viewAs &&
      this.page === filter.page &&
      this.selectedItem.key === filter.selectedItem.key &&
      this.folder === filter.folder &&
      this.pageCount === filter.pageCount;

    return equals;
  }
}

export default ProjectsFilter;
