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

const ArticleBodyContent = (props) => {
  //console.log(props);
  const {
    treeFolders,
    fetchTreeFolders,
    treeSettings,
    fetchTreeSettings,
    isLoading,
    fetchFollowedProjects,
    selectedNode,
    setSelectedNode,
    filter,
    fetchAllProjects,
    fetchMyProjects,
    fetchActiveProjects,
    setFilterType,
    fetchProjects,
  } = props;

  const onSelect = (data, e) => {
    console.log(data);
    setSelectedNode(data);

    setFilterType(data[0]);

    // const newFilter = filter.clone();
    // newFilter.page = 0;
    // newFilter.startIndex = 0;

    const selectedFolderTitle =
      (e.node && e.node.props && e.node.props.title) || null;

    selectedFolderTitle
      ? setDocumentTitle(selectedFolderTitle)
      : setDocumentTitle();

    if (window.location.pathname.indexOf("/projects/filter") > 0) {
      fetchProjects(filter, data[0]);
    }

    // if (window.location.pathname.indexOf("/filter") > 0) {
    //   // здесь проверка на project/filter, кидаем в общую функцию и там уже проверяем
    //   switch (data[0]) {
    //     case FolderKey.Projects:
    //       fetchAllProjects(newFilter);
    //       break;
    //     case FolderKey.MyProjects:
    //       fetchMyProjects(newFilter);
    //       break;
    //     case FolderKey.ProjectsFollowed:
    //       newFilter.follow = true;
    //       fetchFollowedProjects(newFilter);
    //       break;
    //     case FolderKey.ProjectsActive:
    //       newFilter.status = "open";
    //       fetchActiveProjects(newFilter);

    //     default:
    //       break;
    //   }
    // }
  };

  useEffect(() => {
    fetchTreeFolders();
  }, []);

  return isEmpty(treeFolders) ? (
    <Loaders.TreeFolders />
  ) : (
    <>
      <TreeFolders data={treeFolders} onSelect={onSelect} />
      <TreeSettings isLoading={isLoading} />
    </>
  );
};

export default inject(({ projectsStore }) => {
  const {
    isLoading,
    treeFoldersStore,
    projectsFilterStore,
    setFilterType,
    filter,
  } = projectsStore;
  const {
    fetchAllProjects,
    fetchFollowedProjects,
    fetchActiveProjects,
    fetchMyProjects,
    fetchProjects,
  } = projectsFilterStore;
  const { treeFolders, fetchTreeFolders, setSelectedNode } = treeFoldersStore;
  const selectedNode = treeFoldersStore.selectedTreeNode;

  return {
    treeFolders,
    selectedNode,
    setSelectedNode,
    fetchTreeFolders,
    isLoading,
    filter,
    fetchAllProjects,
    fetchFollowedProjects,
    fetchActiveProjects,
    fetchMyProjects,
    setFilterType,
    fetchProjects,
  };
})(observer(withRouter(ArticleBodyContent)));
