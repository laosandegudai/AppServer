import React from "react";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router-dom";
import Loaders from "@appserver/common/components/Loaders";
import isEmpty from "lodash/isEmpty";
import TreeFolders from "./TreeFolders";
import { useEffect } from "react";
import TreeSettings from "./TreeSettings";
import { setDocumentTitle } from "../../../helpers/utils";
import { FolderKey } from "../../../constants";
import api from "@appserver/common/api";
// import history from "@appserver/common/history";
import combineUrl from "@appserver/common/utils/combineUrl";
import { AppServerConfig } from "@appserver/common/constants";
import config from "../../../../package.json";
const { ProjectsFilter, TasksFilter } = api;

const ArticleBodyContent = (props) => {
  //console.log(props);
  const {
    treeFolders,
    fetchTreeFolders,
    isLoading,
    selectedNode,
    setSelectedNode,
    setIsLoading,
    fetchProjects,
    fetchTasks,
    setSelection,
    history,
  } = props;

  const onSelect = (data, e) => {
    setSelectedNode(data);
    setSelection([]);
    setIsLoading(true);

    console.log(data);

    const selectedFolderTitle =
      (e.node && e.node.props && e.node.props.title) || null;

    selectedFolderTitle
      ? setDocumentTitle(selectedFolderTitle)
      : setDocumentTitle();

    let filterObj = null;
    switch (data[0]) {
      case FolderKey.Projects:
      case FolderKey.ProjectsActive:
      case FolderKey.ProjectsFollowed:
      case FolderKey.MyProjects:
        filterObj = ProjectsFilter.getDefault();
        fetchProjects(filterObj, data[0], true).finally(() =>
          setIsLoading(false)
        );
        break;

      case FolderKey.Tasks:
      case FolderKey.MyTasks:
      case FolderKey.TasksUpcoming:
        filterObj = TasksFilter.getDefault();
        fetchTasks(filterObj, data[0], true).finally(() => setIsLoading(false));
        break;

      default:
        setIsLoading(false);
        return history.push(
          combineUrl(
            AppServerConfig.proxyURL,
            config.homepage,
            "/projects-soon"
          )
        );

        break;
    }
  };

  useEffect(() => {
    fetchTreeFolders();
  }, []);

  return isEmpty(treeFolders) ? (
    <Loaders.TreeFolders />
  ) : (
    <>
      <TreeFolders
        data={treeFolders}
        onSelect={onSelect}
        selectedKeys={selectedNode}
      />
      <TreeSettings isLoading={isLoading} />
    </>
  );
};

export default inject(
  ({ projectsStore, projectsFilterStore, tasksFilterStore }) => {
    const { fetchTasks, getTaskFilterCommonOptions } = tasksFilterStore;
    const {
      isLoading,
      treeFoldersStore,
      setFilterType,
      filter,
      setFilter,
      setIsLoading,
      setFilterCommonOptions,
      setSelection,
    } = projectsStore;
    const {
      fetchProjects,
      getProjectFilterCommonOptions,
      fetchMyProjects,
    } = projectsFilterStore;
    const {
      treeFolders,
      fetchTreeFolders,
      setSelectedNode,
      findRootFolder,
      isProjectsFolder,
    } = treeFoldersStore;
    const selectedNode = treeFoldersStore.selectedTreeNode;

    return {
      findRootFolder,
      treeFolders,
      selectedNode,
      setSelectedNode,
      fetchTreeFolders,
      isLoading,
      filter,
      setFilter,
      setFilterType,
      fetchProjects,
      setIsLoading,
      fetchTasks,
      isProjectsFolder,
      setFilterCommonOptions,
      getTaskFilterCommonOptions,
      getProjectFilterCommonOptions,
      fetchMyProjects,
      setSelection,
    };
  }
)(observer(withRouter(ArticleBodyContent)));
