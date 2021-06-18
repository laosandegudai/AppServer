import { getObjectByLocation, toUrlParams } from "../../utils";

const DEFAULT_PAGE = 0;
const DEFAULT_PAGE_COUNT = 25;
const DEFAULT_TOTAL = 0;
const DEFAULT_SORT_BY = "displayname";
const DEFAULT_SORT_ORDER = "descending";
const DEFAULT_SEARCH = "";

const DEFAULT_CONTACT_TYPE = "";
const CONTACT_TYPE = "contactType";
const PAGE = "page";
const PAGE_COUNT = "count";
const SORT_BY = "sortby";
const SORT_ORDER = "sortorder";
const SEARCH = "search";

class CrmFilter {
    static getDefault(total = DEFAULT_TOTAL) {
        return new CrmFilter(DEFAULT_PAGE, DEFAULT_PAGE_COUNT, total);
      }

       static getFilter(location) {
         return this.getDefault()
       }

      constructor(
        page = DEFAULT_PAGE,
        pageCount = DEFAULT_PAGE_COUNT,
        total = DEFAULT_TOTAL,
        sortBy = DEFAULT_SORT_BY,
        sortOrder = DEFAULT_SORT_ORDER,
        contactType = DEFAULT_CONTACT_TYPE,
        search = DEFAULT_SEARCH,
      ) {
        this.page = page;
        this.pageCount = pageCount;
        this.sortBy = sortBy;
        this.sortOrder = sortOrder;
        this.contactType = contactType;
        this.search = search;
        this.total = total;
      }

      toApiUrlParams = () => {
          const {filterType, page, pageCount, sortBy, sortOrder, search } = this;
 let dtoFilter = {
// sortBy: 'displayname',
// sortOrder: 'descending',
// contactStage: -1,
// contactType: -1,
// StartIndex: 0,
// Count: 25
    };


            const str = toUrlParams(dtoFilter, true);
    return str;
      }

           toUrlParams = () => {
          const {filterType, page, pageCount, sortBy, sortOrder, search } = this;
            const dtoFilter = {};
             if (pageCount !== DEFAULT_PAGE_COUNT) {
      dtoFilter[PAGE_COUNT] = pageCount;
    }

    dtoFilter[PAGE] = page + 1;
    dtoFilter[SORT_BY] = sortBy;
    dtoFilter[SORT_ORDER] = sortOrder;

    const str = toUrlParams(dtoFilter, true);
    return str;
      }

      clone() {
        return new CrmFilter(
          this.page,
          this.pageCount,
          this.total,
          this.sortBy,
          this.sortOrder,
          this.contactType,
          this.search,
        );
      }
    
}

export default CrmFilter;