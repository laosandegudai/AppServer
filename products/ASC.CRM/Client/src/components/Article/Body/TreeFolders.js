import React, { useEffect } from "react";
import TreeMenu from "@appserver/components/tree-menu";
import TreeNode from "@appserver/components/tree-menu/sub-components/tree-node";
import { ReactSVG } from "react-svg";
import { FolderType } from "@appserver/common/constants";
import styled from "styled-components";
import { observer, inject } from "mobx-react";
import { withTranslation } from "react-i18next";
import ExpanderDownIcon from "../../../../../../../public/images/expander-down.react.svg";
import ExpanderRightIcon from "../../../../../../../public/images/expander-right.react.svg";
import commonIconsStyles from "@appserver/components/utils/common-icons-style";

const StyledTreeMenu = styled(TreeMenu)`
  .rc-tree-node-selected {
    background: #dfe2e3 !important;
  }

  .rc-tree-treenode-disabled > span:not(.rc-tree-switcher),
  .rc-tree-treenode-disabled > a,
  .rc-tree-treenode-disabled > a span {
    cursor: wait;
  }
`;

const StyledFolderSVG = styled.div`
  svg {
    width: 100%;

    path {
      fill: #657077;
    }
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

const PureTreeFolders = (props) => {
  const { t } = props;

  useEffect(() => {
    props.fetchTreeFolders();
  }, []);

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

  const getFolderIcon = (item) => {
    let iconUrl = "images/catalog.folder.react.svg";

    switch (item.rootFolderType) {
      case FolderType.Contacts:
        iconUrl = "images/catalog.contacts.react.svg";
        break;
      case FolderType.Tasks:
        iconUrl = "images/catalog.tasks.react.svg";
        break;
      case FolderType.Opportunities:
        iconUrl = "images/catalog.opportunities.react.svg";
        break;
      case FolderType.Invoices:
        iconUrl = "images/catalog.invoices.react.svg";
        break;
      case FolderType.Cases:
        iconUrl = "images/catalog.cases.react.svg";
        break;
      case FolderType.Reports:
        iconUrl = "images/catalog.reports.react.svg";
        break;
      default:
        break;
    }

    return (
      <StyledFolderSVG>
        <ReactSVG src={iconUrl} />
      </StyledFolderSVG>
    );
  };

  const getItems = (data) => {
    return data.map((item) => {
      if (item.folders && item.folders.length > 0) {
        return (
          <TreeNode
            id={item.id}
            key={item.key}
            title={t(item.title)}
            icon={getFolderIcon(item)}
            isLeaf={false}
          />
        );
      } else {
        return (
          <TreeNode
            id={item.id}
            key={item.key}
            title={t(item.title)}
            icon={getFolderIcon(item)}
            isLeaf={true}
          />
        );
      }
    });
  };

  const treeFoldersData = getItems(props.treeFolders) || [];

  return (
    <StyledTreeMenu
      className='files-tree-menu'
      showIcon={true}
      gapBetweenNodes='22'
      gapBetweenNodesTablet='26'
      switcherIcon={switcherIcon}
    >
      {treeFoldersData}
    </StyledTreeMenu>
  );
};

const TreeFolders = withTranslation("Article")(PureTreeFolders);

export default inject(({ treeFoldersStore }) => {
  const { treeFolders, fetchTreeFolders } = treeFoldersStore;
  return {
    treeFolders,
    fetchTreeFolders,
  };
})(observer(TreeFolders));
