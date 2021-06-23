import React, { useEffect } from "react";
import { inject, observer } from "mobx-react";
import styled from "styled-components";
import TreeMenu from "@appserver/components/tree-menu";
import TreeNode from "@appserver/components/tree-menu/sub-components/tree-node";
import ExpanderDownIcon from "../../../../public/images/expander-down.react.svg";
import ExpanderRightIcon from "../../../../public/images/expander-right.react.svg";
import commonIconsStyles from "@appserver/components/utils/common-icons-style";
import SettingsIcon from "../../../../public/images/settings.react.svg";

const StyledTreeMenu = styled(TreeMenu)`
  margin-top: 18px !important;
  @media (max-width: 1024px) {
    margin-top: 14px !important;
  }

  .rc-tree-node-content-wrapper {
    width: 98% !important;
  }

  .rc-tree-node-selected {
    background: #dfe2e3 !important;
  }

  .rc-tree-treenode-disabled > span:not(.rc-tree-switcher),
  .rc-tree-treenode-disabled > a,
  .rc-tree-treenode-disabled > a span {
    cursor: wait;
  }

  .rc-tree-child-tree .rc-tree-node-content-wrapper > .rc-tree-title {
    width: 99% !important;
    padding-left: 4px !important;
  }

  .rc-tree-child-tree span.rc-tree-node-selected {
    max-width: 106%;
  }

  .rc-tree-child-tree {
    margin-left: 22px;
  }

  @media (max-width: 1024px) {
    .settings-node {
      margin-left: 18px !important;
    }
  }
`;
const switcherIcon = (obj) => {
  if (obj.isLeaf) {
    return null;
  }
  if (obj.expanded) {
    return <StyledExpanderDownIcon size="scale" />;
  } else {
    return <StyledExpanderRightIcon size="scale" />;
  }
};
const StyledExpanderDownIcon = styled(ExpanderDownIcon)`
  ${commonIconsStyles}
  path {
    fill: dimgray;
  }
`;
const StyledExpanderRightIcon = styled(ExpanderRightIcon)`
  ${commonIconsStyles}
  path {
    fill: dimgray;
  }
`;
const StyledSettingsIcon = styled(SettingsIcon)`
  ${commonIconsStyles}
  path {
    fill: dimgray;
  }
`;

const TreeSettings = (props) => {
  const { treeSettings, fetchTreeSettings, data, isLoading } = props;
  const renderTreeNode = () => {
    return (
      <TreeNode
        id="settings"
        key="settings"
        title={"Settings"}
        isLeaf={false}
        icon={<StyledSettingsIcon size="scale" />}
      >
        <TreeNode
          className="settings-node"
          id="common-settings"
          key="common"
          isLeaf={true}
          title={"Common Settings"}
        />
      </TreeNode>
    );
  };

  const nodes = renderTreeNode();
  return (
    <StyledTreeMenu
      switcherIcon={switcherIcon}
      defaultExpandParent={false}
      disabled={isLoading}
      className="settings-tree-menu"
      showIcon={true}
      isFullFillSelection={false}
      gapBetweenNodes="22"
      gapBetweenNodesTablet="26"
    >
      {nodes}
    </StyledTreeMenu>
  );
};

export default inject(({ treeSettingsStore }) => {
  const { treeSettings, fetchTreeSettings } = treeSettingsStore;
  return {
    treeSettings,
    fetchTreeSettings,
  };
})(observer(TreeSettings));
