import React, { useCallback, useMemo } from "react";
import { isMobile } from "react-device-detect";
import Paging from "@appserver/components/paging";
import { useTranslation } from "react-i18next";
import { inject, observer } from "mobx-react";
import Loaders from "@appserver/common/components/Loaders";

const SectionPagingContent = ({
  filter,
  fetchContacts,
  setIsLoading,
  tReady,
  isLoaded,
}) => {
  const { t } = useTranslation("Home");
  const onNextClick = useCallback(
    (e) => {
      if (!filter.hasNext()) {
        e.preventDefault();
        return;
      }
      console.log("Next Clicked", e);

      const newFilter = filter.clone();
      newFilter.page++;

      setIsLoading(true);
      fetchContacts(newFilter).finally(() => setIsLoading(false));
    },
    [filter, setIsLoading, fetchContacts]
  );

  const onPrevClick = useCallback(
    (e) => {
      if (!filter.hasPrev()) {
        e.preventDefault();
        return;
      }

      console.log("Prev Clicked", e);

      const newFilter = filter.clone();
      newFilter.page--;

      setIsLoading(true);
      fetchContacts(newFilter).finally(() => setIsLoading(false));
    },
    [filter, setIsLoading, fetchContacts]
  );

  const onChangePageSize = useCallback(
    (pageItem) => {
      console.log("Paging onChangePageSize", pageItem);

      const newFilter = filter.clone();
      newFilter.page = 0;
      newFilter.count = pageItem.key;

      setIsLoading(true);
      fetchContacts(newFilter).finally(() => setIsLoading(false));
    },
    [filter, setIsLoading, fetchContacts]
  );

  const onChangePage = useCallback(
    (pageItem) => {
      console.log("Paging onChangePage", pageItem);

      const newFilter = filter.clone();
      newFilter.page = pageItem.key;

      setIsLoading(true);
      fetchContacts(newFilter).finally(() => setIsLoading(false));
    },
    [filter, setIsLoading, fetchContacts]
  );

  const countItems = useMemo(
    () => [
      {
        key: 25,
        label: t("Common:CountPerPage", { count: 25 }),
      },
      {
        key: 50,
        label: t("Common:CountPerPage", { count: 50 }),
      },
      {
        key: 100,
        label: t("Common:CountPerPage", { count: 100 }),
      },
    ],
    [t]
  );

  const pageItems = useMemo(() => {
    if (filter.total < filter.count) return [];
    const totalPages = Math.ceil(filter.total / filter.count);
    return [...Array(totalPages).keys()].map((item) => {
      return {
        key: item,
        label: t("Common:PageOfTotalPage", {
          page: item + 1,
          totalPage: totalPages,
        }),
      };
    });
  }, [filter.total, filter.count, t]);

  const emptyPageSelection = {
    key: 0,
    label: t("Common:PageOfTotalPage", { page: 1, totalPage: 1 }),
  };

  const emptyCountSelection = {
    key: 0,
    label: t("Common:CountPerPage", { count: 25 }),
  };

  const selectedPageItem =
    pageItems.find((x) => x.key === filter.page) || emptyPageSelection;
  const selectedCountItem =
    countItems.find((x) => x.key === filter.count) || emptyCountSelection;

  return isLoaded ? (
    !filter || filter.total < filter.count || !tReady ? (
      <></>
    ) : (
      <Paging
        previousLabel={t("Common:Previous")}
        nextLabel={t("Common:Next")}
        pageItems={pageItems}
        onSelectPage={onChangePage}
        countItems={countItems}
        onSelectCount={onChangePageSize}
        displayItems={false}
        disablePrevious={!filter.hasPrev()}
        disableNext={!filter.hasNext()}
        disableHover={isMobile}
        previousAction={onPrevClick}
        nextAction={onNextClick}
        openDirection="top"
        selectedPageItem={selectedPageItem} //FILTER CURRENT PAGE
        selectedCountItem={selectedCountItem} //FILTER PAGE COUNT
        showCountItem={showCountItem}
      />
    )
  ) : (
    <Loaders.Filter />
  );
};

export default inject(({ auth, contactsStore, crmStore }) => {
  const { isLoaded } = auth;
  const { setIsLoading } = crmStore;
  const { filterStore, getContactsList: fetchContacts } = contactsStore;
  const { filter } = filterStore;
  return {
    isLoaded,
    fetchContacts,
    filter,
    setIsLoading,
  };
})(observer(SectionPagingContent));
