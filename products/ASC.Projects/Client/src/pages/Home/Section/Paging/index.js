import React, { useCallback, useMemo } from "react";
import { inject, observer } from "mobx-react";
import { isMobile } from "react-device-detect";
import Paging from "@appserver/components/paging";
import { useTranslation } from "react-i18next";

const SectionPagingContent = ({
  filter,
  items,
  selectedTreeNode,
  fetchProjects,
}) => {
  const { t } = useTranslation("Home");

  const pathname = window.location.pathname;

  const pageItems = useMemo(() => {
    const totalPages = Math.ceil(filter.total / filter.pageCount);
    return [...Array(totalPages).keys()].map((item) => {
      return {
        key: item,
        label: t("Common:PageOfTotalPage", {
          page: item + 1,
          totalPage: totalPages,
        }),
      };
    });
  }, [filter.total, filter.pageCount, t]);

  const onChangePage = useCallback(
    (pageItems) => {
      console.log(pageItems);
      const newFilter = filter.clone();
      newFilter.page = pageItems.key;

      if (pathname.indexOf("/projects/filter") > -1) {
        console.log("daa");
        // добавить лоадинг
        fetchProjects(newFilter, selectedTreeNode[0]);
      }
    },
    [filter, selectedTreeNode, fetchProjects]
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

  const onChangePageSize = useCallback(
    (pageItem) => {
      const newFilter = filter.clone();
      newFilter.page = 0;
      newFilter.pageCount = pageItem.key;

      if (pathname.indexOf("/projects/filter") > -1) {
        console.log("daa");
        // добавить лоадинг
        fetchProjects(newFilter, selectedTreeNode[0]);
      }
    },
    [filter, selectedTreeNode, fetchProjects]
  );

  const onPrevClick = useCallback(
    (e) => {
      if (!filter.hasPrev()) {
        e.preventDefault();
        return;
      }
      const newFilter = filter.clone();
      newFilter.page--;

      if (pathname.indexOf("/projects/filter") > -1) {
        console.log("daa");
        // добавить лоадинг
        fetchProjects(newFilter, selectedTreeNode[0]);
      }
    },
    [selectedTreeNode, filter, fetchProjects]
  );

  const onNextClick = useCallback(
    (e) => {
      if (!filter.hasNext()) {
        e.preventDefault();
        return;
      }
      const newFilter = filter.clone();
      newFilter.page++;
      if (pathname.indexOf("/projects/filter") > -1) {
        console.log("daa");
        // добавить лоадинг
        fetchProjects(newFilter, selectedTreeNode[0]);
      }
    },
    [selectedTreeNode, filter, fetchProjects]
  );

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
    countItems.find((x) => x.key === filter.pageCount) || emptyCountSelection;

  return filter.total < filter.pageCount && filter.total < 26 ? (
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
      //showCountItem={showCountItem}
    />
  );
};

export default inject(
  ({
    projectsStore,
    treeFoldersStore,
    projectsFilterStore,
    tasksFilterStore,
  }) => {
    const { filter, items } = projectsStore;
    const { fetchProjects } = projectsFilterStore;
    const { selectedTreeNode } = treeFoldersStore;
    return {
      filter,
      items,
      selectedTreeNode,
      fetchProjects,
    };
  }
)(observer(SectionPagingContent));
