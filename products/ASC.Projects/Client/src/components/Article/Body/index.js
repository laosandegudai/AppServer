import React from "react";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router-dom";
import Loaders from "@appserver/common/components/Loaders";
import isEmpty from "lodash/isEmpty";
import TreeFolders from "./TreeFolders";
import { useEffect } from "react";
import TreeSettings from "./TreeSettings";

const ArticleBodyContent = (props) => {
  console.log(props);
  const {
    treeFolders,
    fetchTreeFolders,
    fetchTreeSettings,
    treeSettings,
    isLoading,
  } = props;

  useEffect(() => {
    fetchTreeFolders();
    fetchTreeSettings();
  }, []);

  return isEmpty(treeFolders) ? (
    <Loaders.TreeFolders />
  ) : (
    <>
      <TreeFolders data={treeFolders} />
      <TreeSettings data={treeSettings} isLoading={isLoading} />
    </>
  );
};

export default inject(
  ({ projectsStore, treeFoldersStore, treeSettingsStore }) => {
    const { treeFolders, fetchTreeFolders } = treeFoldersStore;
    const { treeSettings, fetchTreeSettings } = treeSettingsStore;

    const { isLoading } = projectsStore;
    console.log(fetchTreeFolders);

    return {
      treeFolders,
      treeSettings,
      fetchTreeSettings,
      fetchTreeFolders,
      isLoading,
    };
  }
)(observer(withRouter(ArticleBodyContent)));
