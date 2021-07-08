import { getObjectByLocation, toUrlParams } from "../../utils";
import queryString from "query-string";

const DEFAULT_PAGE = 0;
const DEFAULT_PAGE_COUNT = 25;
const DEFAULT_TOTAL = 0;
const DEFAULT_SORT_BY = "DateAndTime";
const DEFAULT_SORT_ORDER = "descending";
const DEFAULT_VIEW = "row";
const DEFAULT_SEARCH = null;
const DEFAULT_SELECTED_ITEM = {};
const DEFAULT_PROJECT_ID = null;
const DEFAULT_TAG = null;
const DEFAULT_STATUS = null;
const DEFAULT_SUBSTATUS = null;
const DEFAULT_DEPARTAMENT = null;
const DEFAULT_PARTICIPANT = null;
const DEFAULT_CREATOR = null;
const DEFAULT_MILESTONE = null;
const DEFAULT_DEADLINE_START = null;
const DEFAULT_DEADLINE_STOP = null;
const DEFAULT_LAST_ID = null;
const DEFAULT_MY_PROJECTS = null;
const DEFAULT_MY_MILESTONES = null;
const DEFAULT_NO_MILESTONE = null;
const DEFAULT_FOLLOW = null;
const DEFAULT_FOLDER = null;

const PAGE = "page";
const PAGE_COUNT = "count";
const SORT_BY = "sortby";
const SORT_ORDER = "sortorder";
const VIEW_AS = "viewas";
const SEARCH = "search";
const PROJECT_ID = "projectid";
const TAG = "tag";
const STATUS = "status";
const SUBSTATUS = "substatus";
const DEPARTAMENT = "departament";
const PARTICIPANT = "participant";
const CREATOR = "creator";
const MILESTONE = "milestone";
const DEADLINE_START = "deadlineStart";
const DEADLINE_STOP = "deadlineStop";
const LAST_ID = "lastId";
const MY_PROJECTS = "myProjects";
const MY_MILESTONES = "myMilestones";
const NO_MILESTONE = "noMilestone";
const FOLLOW = "follow";
const FOLDER = "folder";

class TasksFilter {
  static getDefault(total = DEFAULT_TOTAL) {
    return new TasksFilter(DEFAULT_PAGE, DEFAULT_PAGE_COUNT, total);
  }

  static getFilter(location) {
    if (!location) return this.getDefault();

    const urlFilter = getObjectByLocation(location);
    if (!urlFilter) return null;

    const defaultFilter = TasksFilter.getDefault();

    const page =
      (urlFilter[PAGE] && +urlFilter[PAGE] - 1) || defaultFilter.page;
    const pageCount =
      (urlFilter[PAGE_COUNT] && +urlFilter[PAGE_COUNT]) ||
      defaultFilter.pageCount;

    const sortBy = urlFilter[SORT_BY] || defaultFilter.sortBy;
    const sortOrder = urlFilter[SORT_ORDER] || defaultFilter.sortOrder;
    const viewAs = urlFilter[VIEW_AS] || defaultFilter.viewAs;
    const search = urlFilter[SEARCH] || defaultFilter.search;
    const projectId = urlFilter[PROJECT_ID] || defaultFilter.projectId;
    const tag = urlFilter[TAG] || defaultFilter.tag;
    const status = urlFilter[STATUS] || defaultFilter.status;
    const substatus = urlFilter[SUBSTATUS] || defaultFilter.substatus;
    const departament = urlFilter[DEPARTAMENT] || defaultFilter.departament;
    const participant = urlFilter[PARTICIPANT] || defaultFilter.participant;
    const creator = urlFilter[CREATOR] || defaultFilter.creator;
    const milestone = urlFilter[MILESTONE] || defaultFilter.milestone;
    const deadlineStart =
      urlFilter[DEADLINE_START] || defaultFilter.deadlineStart;
    const deadlineStop = urlFilter[DEADLINE_STOP] || defaultFilter.deadlineStop;
    const lastId = urlFilter[LAST_ID] || defaultFilter.lastId;
    const myProjects = urlFilter[MY_PROJECTS] || defaultFilter.myProjects;
    const myMilestones = urlFilter[MY_MILESTONES] || defaultFilter.myMilestones;
    const noMilestone = urlFilter[NO_MILESTONE] || defaultFilter.noMilestone;
    const follow = urlFilter[FOLLOW] || defaultFilter.follow;
    const folder = urlFilter[FOLDER] || defaultFilter.folder;

    const newFilter = new TasksFilter(
      page,
      pageCount,
      defaultFilter.total,
      sortBy,
      sortOrder,
      viewAs,
      search,
      defaultFilter.selectedItem,
      projectId,
      tag,
      status,
      substatus,
      departament,
      participant,
      creator,
      milestone,
      deadlineStart,
      deadlineStop,
      lastId,
      myProjects,
      myMilestones,
      noMilestone,
      follow,
      folder
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
    projectId = DEFAULT_PROJECT_ID,
    tag = DEFAULT_TAG,
    status = DEFAULT_STATUS,
    substatus = DEFAULT_SUBSTATUS,
    departament = DEFAULT_DEPARTAMENT,
    participant = DEFAULT_PARTICIPANT,
    creator = DEFAULT_CREATOR,
    milestone = DEFAULT_MILESTONE,
    deadlineStart = DEFAULT_DEADLINE_START,
    deadlineStop = DEFAULT_DEADLINE_STOP,
    lastId = DEFAULT_LAST_ID,
    myProjects = DEFAULT_MY_PROJECTS,
    myMilestones = DEFAULT_MY_MILESTONES,
    noMilestone = DEFAULT_NO_MILESTONE,
    follow = DEFAULT_FOLLOW,
    folder = DEFAULT_FOLDER
  ) {
    this.page = page;
    this.pageCount = pageCount;
    this.total = total;
    this.sortBy = sortBy;
    this.sortOrder = sortOrder;
    this.viewAs = viewAs;
    this.search = search;
    this.selectedItem = selectedItem;
    this.projectId = projectId;
    this.tag = tag;
    this.status = status;
    this.substatus = substatus;
    this.departament = departament;
    this.participant = participant;
    this.creator = creator;
    this.milestone = milestone;
    this.deadlineStart = deadlineStart;
    this.deadlineStop = deadlineStop;
    this.lastId = lastId;
    this.myProjects = myProjects;
    this.myMilestones = myMilestones;
    this.noMilestone = noMilestone;
    this.follow = follow;
    this.folder = folder;
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
      projectId,
      tag,
      status,
      substatus,
      departament,
      participant,
      creator,
      milestone,
      deadlineStart,
      deadlineStop,
      lastId,
      myProjects,
      myMilestones,
      noMilestone,
      follow,
      folder,
    } = this;

    const dtoFilter = {
      count: pageCount,
      startIndex: this.getStartIndex(),
      page: page,
      sortby: sortBy,
      sortOrder: sortOrder,
      filterValue: (search ?? "").trim(),
      projectId: projectId,
      tag: tag,
      status: status,
      substatus: substatus,
      departament: departament,
      participant: participant,
      creator: creator,
      milestone: milestone,
      deadlineStart: deadlineStart,
      deadlineStop: deadlineStop,
      lastId: lastId,
      myProjects: myProjects,
      myMilestones: myMilestones,
      noMilestone: noMilestone,
      follow: follow,
      folder: folder,
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
      projectId,
      tag,
      status,
      substatus,
      departament,
      participant,
      creator,
      milestone,
      deadlineStart,
      deadlineStop,
      lastId,
      myProjects,
      myMilestones,
      noMilestone,
      follow,
      folder,
    } = this;

    const dtoFilter = {};

    const URLParams = queryString.parse(window.location.href);

    if (search) {
      dtoFilter[SEARCH] = search.trim();
    }

    if (pageCount !== DEFAULT_PAGE_COUNT) {
      dtoFilter[PAGE_COUNT] = pageCount;
    }

    if (projectId) {
      dtoFilter[PROJECT_ID] = projectId;
    }

    if (tag) {
      dtoFilter[TAG] = tag;
    }

    if (status) {
      dtoFilter[STATUS] = status;
    }

    if (substatus) {
      dtoFilter[SUBSTATUS] = substatus;
    }

    if (departament) {
      dtoFilter[DEPARTAMENT] = departament;
    }

    if (participant) {
      dtoFilter[PARTICIPANT] = participant;
    }

    if (creator) {
      dtoFilter[CREATOR] = creator;
    }

    if (milestone) {
      dtoFilter[MILESTONE] = milestone;
    }

    if (deadlineStart) {
      dtoFilter[DEADLINE_START] = deadlineStart;
    }

    if (deadlineStop) {
      dtoFilter[DEADLINE_STOP] = deadlineStop;
    }

    if (lastId) {
      dtoFilter[lastId] = lastId;
    }

    if (myProjects) {
      dtoFilter[MY_PROJECTS] = myProjects;
    }

    if (myMilestones) {
      dtoFilter[MY_MILESTONES] = myMilestones;
    }

    if (noMilestone) {
      dtoFilter[NO_MILESTONE] = noMilestone;
    }

    if (follow) {
      dtoFilter[FOLLOW] = follow;
    }

    if (folder) {
      dtoFilter[FOLDER] = folder;
    }

    if (URLParams.preview) {
      dtoFilter[PREVIEW] = URLParams.preview;
    }

    dtoFilter[PAGE] = page + 1;
    dtoFilter[SORT_BY] = sortBy;
    dtoFilter[SORT_ORDER] = sortOrder;

    const str = toUrlParams(dtoFilter, true);
    return str;
  };

  equals(filter) {
    const equals =
      this.page === filter.page &&
      this.pageCount === filter.pageCount &&
      this.sortBy === filter.sortBy &&
      this.sortOrder === filter.sortOrder &&
      this.viewAs === filter.viewAs &&
      this.search === filter.search &&
      this.selectedItem.key === filter.selectedItem.key &&
      this.projectId === filter.projectId &&
      this.tag === filter.tag &&
      this.status === filter.status &&
      this.substatus === filter.substatus &&
      this.departament === filter.departament &&
      this.participant === filter.participant &&
      this.creator === filter.creator &&
      this.milestone === filter.milestone &&
      this.deadlineStart === filter.deadlineStart &&
      this.deadlineStop === filter.deadlineStop &&
      this.lastId === filter.lastId &&
      this.myProjects === filter.myProjects &&
      this.myMilestones === filter.myMilestones &&
      this.noMilestone === filter.noMilestone &&
      this.follow === filter.follow &&
      this.folder === filter.folder;
    return equals;
  }

  clone() {
    return new TasksFilter(
      this.page,
      this.pageCount,
      this.total,
      this.sortBy,
      this.sortOrder,
      this.viewAs,
      this.search,
      this.selectedItem,
      this.projectId,
      this.tag,
      this.status,
      this.substatus,
      this.departament,
      this.participant,
      this.creator,
      this.milestone,
      this.deadlineStart,
      this.deadlineStop,
      this.lastId,
      this.myProjects,
      this.myMilestones,
      this.noMilestone,
      this.follow,
      this.folder
    );
  }
}

export default TasksFilter;
