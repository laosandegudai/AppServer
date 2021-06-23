import React, { useEffect } from "react";
import TreeNode from "@appserver/components/tree-menu/sub-components/tree-node";
import TreeMenu from "@appserver/components/tree-menu";
import styled from "styled-components";
import { FolderType } from "../../../constants";
import { ReactSVG } from "react-svg";
import ExpanderDownIcon from "../../../../public/images/expander-down.react.svg";
import ExpanderRightIcon from "../../../../public/images/expander-right.react.svg";
import commonIconsStyles from "@appserver/components/utils/common-icons-style";
import { inject, observer } from "mobx-react";

const StyledTreeMenu = styled(TreeMenu)`
  .rc-tree-node-content-wrapper {
    background: ${(props) => !props.dragging && "none !important"};
  }

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

const TreeFolders = (props) => {
  const { isLoading, treeFolders, data } = props;
  //console.log(props);

  const getFolderIcon = (item) => {
    let iconUrl = "images/catalog.folder.react.svg";
    switch (item.rootFolderType) {
      case FolderType.Projects:
        iconUrl = "images/action.projects.react.svg";
        break;
      case FolderType.Milestones:
        iconUrl = "images/action.spreadsheet.react.svg";
        break;
      case FolderType.Tasks:
        iconUrl = "images/action.task.react.svg";
        break;
      case FolderType.Discussions:
        iconUrl = "images/action.discussion.react.svg";
        break;
      case FolderType.GanttChart:
        iconUrl = "images/catalog.gantt.react.svg";
        break;
      case FolderType.TimeTracking:
        iconUrl = "images/action.timer.react.svg";
        break;
      case FolderType.Documents:
        iconUrl = "images/catalog.document.react.svg";
        break;
      case FolderType.Reports:
        iconUrl = "images/catalog.reports.react.svg";
        break;
      case FolderType.ProjectsTemplate:
        iconUrl = "images/action.template.react.svg";
        break;
      default:
        iconUrl = "images/catalog.folder.react.svg";
        break;
    }

    return (
      <StyledFolderSVG>
        <ReactSVG src={iconUrl} />
      </StyledFolderSVG>
    );
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

  const getItems = (data) => {
    return data.map((item) => {
      let className = `tree-drag tree-id_${item.id}`;
      if (item.folders && item.folders.length > 0) {
        return (
          <TreeNode
            id={item.id}
            key={item.key}
            className={className}
            title={item.title}
            icon={getFolderIcon(item)}
            isLeaf={false}
          >
            {getItems(item.folders ? item.folders : [])}
          </TreeNode>
        );
      }
      return (
        <TreeNode
          id={item.id}
          key={item.key}
          className={className}
          title={item.title}
          icon={getFolderIcon(item)}
          isLeaf={true}
        />
      );
    });
  };

  return (
    <StyledTreeMenu
      className="files-tree-menu"
      checkable={false}
      draggable
      disabled={isLoading}
      multiple={false}
      switcherIcon={switcherIcon}
      showIcon
      gapBetweenNodes="22"
      gapBetweenNodesTablet="26"
      isFullFillSelection={false}
    >
      {getItems(data || treeFolders)}
    </StyledTreeMenu>
  );
};

export default inject(({ projectsStore }) => {
  const { isLoading, treeFoldersStore } = projectsStore;
  const {
    treeFolders,
    fetchTreeFolders,
    expandedKeys,
    setExpandedKeys,
    setExpandedPanelKeys,
  } = treeFoldersStore;
  return {
    treeFolders,
    isLoading,
    fetchTreeFolders,
    expandedKeys,
    setExpandedKeys,
    setExpandedPanelKeys,
  };
})(observer(TreeFolders));
