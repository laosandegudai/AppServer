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
const { ProjectsFilter } = api;

const ArticleBodyContent = (props) => {
  //console.log(props);
  const {
    treeFolders,
    fetchTreeFolders,
    treeSettings,
    fetchTreeSettings,
    isLoading,
    selectedNode,
    setSelectedNode,
    filter,
    setIsLoading,

    setFilter,
    fetchProjects,
  } = props;

  const onSelect = (data, e) => {
    console.log(data);
    setSelectedNode(data);
    setIsLoading(true);

    const selectedFolderTitle =
      (e.node && e.node.props && e.node.props.title) || null;

    selectedFolderTitle
      ? setDocumentTitle(selectedFolderTitle)
      : setDocumentTitle();

    if (window.location.pathname.indexOf("/projects/filter") > 0) {
      setFilter(ProjectsFilter.getDefault());
      fetchProjects(filter, data[0]).finally(() => setIsLoading(false));
    }
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

export default inject(({ projectsStore, projectsFilterStore }) => {
  const {
    isLoading,
    treeFoldersStore,
    setFilterType,
    filter,
    setFilter,
    setIsLoading,
  } = projectsStore;
  const { fetchProjects } = projectsFilterStore;
  const { treeFolders, fetchTreeFolders, setSelectedNode } = treeFoldersStore;
  const selectedNode = treeFoldersStore.selectedTreeNode;

  return {
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
  };
})(observer(withRouter(ArticleBodyContent)));
