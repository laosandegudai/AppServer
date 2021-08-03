import React, { useCallback, useMemo } from "react";
import { inject, observer } from "mobx-react";
import { isMobile } from "react-device-detect";
import Paging from "@appserver/components/paging";
import { useTranslation } from "react-i18next";
import api from "@appserver/common/api";
const { ProjectsFilter, TasksFilter } = api;

const SectionPagingContent = ({
  filter,
  selectedTreeNode,
  fetchProjects,
  setIsLoading,
  fetchTasks,
}) => {
  const { t } = useTranslation("Home");
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

      if (filter instanceof ProjectsFilter) {
        setIsLoading(true);
        fetchProjects(newFilter, selectedTreeNode[0]).finally(() =>
          setIsLoading(false)
        );
      }

      console.log(filter, fetchTasks);

      if (filter instanceof TasksFilter) {
        setIsLoading(true);
        fetchTasks(newFilter, selectedTreeNode[0]).finally(() =>
          setIsLoading(false)
        );
      }
    },
    [filter, selectedTreeNode, fetchProjects, fetchTasks]
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

      if (filter instanceof ProjectsFilter) {
        setIsLoading(true);
        fetchProjects(newFilter, selectedTreeNode[0]).finally(() =>
          setIsLoading(false)
        );
      }

      if (filter instanceof TasksFilter) {
        setIsLoading(true);
        fetchTasks(newFilter, selectedTreeNode[0]).finally(() =>
          setIsLoading(false)
        );
      }
    },
    [filter, selectedTreeNode, fetchProjects, fetchTasks]
  );

  const onPrevClick = useCallback(
    (e) => {
      if (!filter.hasPrev()) {
        e.preventDefault();
        return;
      }
      const newFilter = filter.clone();
      newFilter.page--;

      if (filter instanceof ProjectsFilter) {
        setIsLoading(true);
        fetchProjects(newFilter, selectedTreeNode[0]).finally(() =>
          setIsLoading(false)
        );
      }

      if (filter instanceof TasksFilter) {
        setIsLoading(true);
        fetchTasks(newFilter, selectedTreeNode[0]).finally(() =>
          setIsLoading(false)
        );
      }
    },
    [selectedTreeNode, filter, fetchProjects, fetchTasks]
  );

  const onNextClick = useCallback(
    (e) => {
      if (!filter.hasNext()) {
        e.preventDefault();
        return;
      }
      const newFilter = filter.clone();
      newFilter.page++;
      if (filter instanceof ProjectsFilter) {
        setIsLoading(true);
        fetchProjects(newFilter, selectedTreeNode[0]).finally(() =>
          setIsLoading(false)
        );
      }

      if (filter instanceof TasksFilter) {
        setIsLoading(true);
        fetchTasks(newFilter, selectedTreeNode[0]).finally(() =>
          setIsLoading(false)
        );
      }
    },
    [selectedTreeNode, filter, fetchProjects, fetchTasks]
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
      selectedPageItem={selectedPageItem}
      selectedCountItem={selectedCountItem}
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
    const { filter, items, setIsLoading } = projectsStore;
    const { fetchProjects } = projectsFilterStore;
    const { fetchTasks } = tasksFilterStore;
    const { selectedTreeNode } = treeFoldersStore;

    return {
      filter,
      items,
      selectedTreeNode,
      fetchProjects,
      fetchTasks,
      setIsLoading,
    };
  }
)(observer(SectionPagingContent));
