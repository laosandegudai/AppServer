import React from "react";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router-dom";
import Loaders from "@appserver/common/components/Loaders";
import isEmpty from "lodash/isEmpty";
import TreeFolders from "./TreeFolders";
import { useEffect } from "react";
import TreeSettings from "./TreeSettings";

const ArticleBodyContent = (props) => {
  //console.log(props);
  const {
    treeFolders,
    fetchTreeFolders,
    treeSettings,
    fetchTreeSettings,
    isLoading,
  } = props;

  useEffect(() => {
    fetchTreeFolders();
  }, []);

  return isEmpty(treeFolders) ? (
    <Loaders.TreeFolders />
  ) : (
    <>
      <TreeFolders data={treeFolders} />
      <TreeSettings isLoading={isLoading} />
    </>
  );
};

export default inject(({ projectsStore }) => {
  const { isLoading, treeFoldersStore } = projectsStore;
  const { treeFolders, fetchTreeFolders } = treeFoldersStore;

  return {
    treeFolders,
    fetchTreeFolders,
    isLoading,
  };
})(observer(withRouter(ArticleBodyContent)));
