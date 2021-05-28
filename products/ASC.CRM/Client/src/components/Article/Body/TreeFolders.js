import React, { useEffect } from "react";
import TreeMenu from "@appserver/components/tree-menu";
import TreeNode from "@appserver/components/tree-menu/sub-components/tree-node";
import { observer, inject } from "mobx-react";

const TreeFolders = (props) => {
  useEffect(() => {
    props.fetchTreeFolders();
  }, []);

  const getItems = (data) => {
    return data.map((folder) => (
      <TreeNode key={folder.key} title={folder.title} />
    ));
  };

  const treeFoldersData = getItems(props.treeFolders) || [];

  return <TreeMenu>{treeFoldersData}</TreeMenu>;
};

export default inject(({ treeFoldersStore }) => {
  const { treeFolders, fetchTreeFolders } = treeFoldersStore;
  return {
    treeFolders,
    fetchTreeFolders,
  };
})(observer(TreeFolders));
