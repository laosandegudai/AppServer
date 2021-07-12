import React from "react";
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

  .rc-tree-title {
    padding-left: 0px !important;
  }

  .node-setting {
    padding-left: 20px !important;
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

const PureTreeSettings = ({
  t,
  expandedKeys,
  selectedTreeNode,
  setSelectedNode,
  setExpandedKeys,
  addExpandSettingsTree,
}) => {
  const onExpand = (data, expandedData) => {
    setExpandedKeys(data);
    console.log("expanded", expandedData.node.props.title);
  };

  const onSelect = (data, selectedNode) => {
    if (!selectedNode.node.props.isLeaf) {
      addExpandSettingsTree(data);
    }
    setSelectedNode(data);
    console.log("selected", selectedNode.node.props.title);
  };

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

  const renderTreeNode = () => {
    return (
      <TreeNode
        id="settings"
        key="settings"
        title={t("Common:Settings")}
        isLeaf={false}
        icon={<StyledSettingsIcon size="scale" />}
      >
        <TreeNode
          className="node-setting"
          id="common-settings"
          key="common"
          isLeaf={true}
          title={t("TreeSettingsCommonSettings")}
        />
        <TreeNode
          id="currency-settings"
          key="currency"
          isLeaf={true}
          title={t("TreeSettingsCurrencySettings")}
          className="node-setting"
        />

        <TreeNode
          id="contact-settings"
          key="contact"
          title={t("TreeSettingsContactSettings")}
          isLeaf={false}
          className="node-setting"
        >
          <TreeNode
            className="settings-node"
            id="temp-level-settings"
            key="contact-levels"
            isLeaf={true}
            title={t("TreeSettingsContactTemperatureLevels")}
          />
          <TreeNode
            className="settings-node"
            id="contact-types-settings"
            key="contact-types"
            isLeaf={true}
            title={t("TreeSettingsContactTypes")}
          />
        </TreeNode>

        <TreeNode
          id="invoice-settings"
          key="invoice"
          title={t("TreeSettingsInvoiceSettings")}
          isLeaf={false}
          className="node-setting"
        >
          <TreeNode
            className="settings-node"
            id="products-services-settings"
            key="products-services"
            isLeaf={true}
            title={t("TreeSettingsProductsAndServices")}
          />
          <TreeNode
            className="settings-node"
            id="taxes-settings"
            key="taxes"
            isLeaf={true}
            title={t("TreeSettingsTaxes")}
          />
          <TreeNode
            className="settings-node"
            id="organization-profile-settings"
            key="organization-profile"
            isLeaf={true}
            title={t("TreeSettingsOrganizationProfile")}
          />
        </TreeNode>

        <TreeNode
          id="other-settings"
          key="other"
          title={t("TreeSettingsOtherSettings")}
          isLeaf={false}
          className="node-setting"
        >
          <TreeNode
            className="settings-node"
            id="user-fields-settings"
            key="user-fields"
            isLeaf={true}
            title={t("TreeSettingsUserFields")}
          />
          <TreeNode
            className="settings-node"
            id="history-event-settings"
            key="history-event"
            isLeaf={true}
            title={t("TreeSettingsHistoryEventCategories")}
          />
          <TreeNode
            className="settings-node"
            id="tasks-categories-settings"
            key="tasks-categories"
            isLeaf={true}
            title={t("TreeSettingsTasksCategories")}
          />
          <TreeNode
            className="settings-node"
            id="opportunity-stages-settings"
            key="opportunity-stages"
            isLeaf={true}
            title={t("TreeSettingsOpportunityStages")}
          />
          <TreeNode
            className="settings-node"
            id="tags-settings"
            key="tags"
            isLeaf={true}
            title={t("TreeSettingsTags")}
          />
        </TreeNode>

        <TreeNode
          id="website-contact-settings"
          key="website"
          isLeaf={true}
          title={t("TreeSettingsWebsiteContactForm")}
          className="node-setting"
        />
        <TreeNode
          id="access-settings"
          key="access"
          isLeaf={true}
          title={t("TreeSettingsPortalAccessRights")}
          className="node-setting"
        />
      </TreeNode>
    );
  };

  const nodes = renderTreeNode();

  return (
    <StyledTreeMenu
      className="settings-tree-menu"
      showIcon={true}
      gapBetweenNodes="22"
      gapBetweenNodesTablet="26"
      defaultExpandParent={false}
      switcherIcon={switcherIcon}
      isFullFillSelection={true}
      onSelect={onSelect}
      onExpand={onExpand}
      expandedKeys={expandedKeys}
      selectedKeys={selectedTreeNode}
    >
      {nodes}
    </StyledTreeMenu>
  );
};

const TreeSettings = withTranslation(["Home", "Common"])(PureTreeSettings);

export default inject(({ treeFoldersStore }) => {
  const {
    selectedTreeNode,
    setSelectedNode,
    expandedKeys,
    addExpandSettingsTree,
    setExpandedKeys,
  } = treeFoldersStore;

  return {
    expandedKeys,
    selectedTreeNode,

    setSelectedNode,
    addExpandSettingsTree,
    setExpandedKeys,
  };
})(observer(TreeSettings));
