import React, { useEffect } from "react";
import TreeMenu from "@appserver/components/tree-menu";
import TreeNode from "@appserver/components/tree-menu/sub-components/tree-node";
import styled from "styled-components";
import { observer, inject } from "mobx-react";
import { withTranslation } from "react-i18next";
import SettingsIcon from "../../../../../../../public/images/settings.react.svg";
import ExpanderDownIcon from "../../../../../../../public/images/expander-down.react.svg";
import ExpanderRightIcon from "../../../../../../../public/images/expander-right.react.svg";
import commonIconsStyles from "@appserver/components/utils/common-icons-style";

const StyledTreeMenu = styled(TreeMenu)`
  margin-top: 40px !important;

  .rc-tree-node-selected {
    background: #dfe2e3 !important;
  }

  .rc-tree-treenode-disabled > span:not(.rc-tree-switcher),
  .rc-tree-treenode-disabled > a,
  .rc-tree-treenode-disabled > a span {
    cursor: wait;
  }
`;

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

const PureTreeSettings = ({ t, expandedSetting, setExpandSettingsTree }) => {
  useEffect(() => {
    if (!expandedSetting) setExpandSettingsTree(["settings"]);
  }, [expandedSetting, setExpandSettingsTree]);

  const switcherIcon = (obj) => {
    if (obj.isLeaf) {
      return null;
    }
    if (obj.expanded) {
      return <StyledExpanderDownIcon size='scale' />;
    } else {
      return <StyledExpanderRightIcon size='scale' />;
    }
  };

  const renderTreeNode = () => {
    return (
      <TreeNode
        id='settings'
        key='settings'
        title={t("TreeSettingsMenuTitle")}
        isLeaf={false}
        icon={<StyledSettingsIcon size='scale' />}
      >
        <TreeNode
          className='settings-node'
          id='common-settings'
          key='common'
          isLeaf={true}
          title={t("TreeSettingsCommonSettings")}
        />
        <TreeNode
          className='settings-node'
          id='currency-settings'
          key='currency'
          isLeaf={true}
          title={t("TreeSettingsCurrencySettings")}
        />
      </TreeNode>
    );
  };

  const nodes = renderTreeNode();

  return (
    <StyledTreeMenu
      className='settings-tree-menu'
      showIcon={true}
      gapBetweenNodes='22'
      gapBetweenNodesTablet='26'
      defaultExpandParent={false}
      switcherIcon={switcherIcon}
      isFullFillSelection={false}
    >
      {nodes}
    </StyledTreeMenu>
  );
};

const TreeSettings = withTranslation("Article")(PureTreeSettings);

export default inject(({ settingsStore }) => {
  const { expandedSetting, setExpandSettingsTree } = settingsStore;

  return {
    expandedSetting,

    setExpandSettingsTree,
  };
})(observer(TreeSettings));
