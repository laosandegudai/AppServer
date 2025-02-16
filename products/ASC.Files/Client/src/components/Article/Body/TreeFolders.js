import React from "react";
import TreeMenu from "@appserver/components/tree-menu";
import TreeNode from "@appserver/components/tree-menu/sub-components/tree-node";
import styled from "styled-components";
import { FolderType, ShareAccessRights } from "@appserver/common/constants";
import toastr from "studio/toastr";

import { onConvertFiles } from "../../../helpers/files-converter";
import { ReactSVG } from "react-svg";
import ExpanderDownIcon from "../../../../../../../public/images/expander-down.react.svg";
import ExpanderRightIcon from "../../../../../../../public/images/expander-right.react.svg";
import commonIconsStyles from "@appserver/components/utils/common-icons-style";
import withLoader from "../../../HOCs/withLoader";
import Loaders from "@appserver/common/components/Loaders";

import { observer, inject } from "mobx-react";
import { runInAction } from "mobx";
import { withTranslation } from "react-i18next";

const backgroundDragColor = "#EFEFB2";
const backgroundDragEnterColor = "#F8F7BF";

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
  /*
  span.rc-tree-iconEle {
    margin-left: 4px;
  }*/
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
class TreeFolders extends React.Component {
  constructor(props) {
    super(props);

    this.state = { isExpand: false };
  }

  onBadgeClick = (e) => {
    e.stopPropagation();
    const id = e.currentTarget.dataset.id;
    this.props.onBadgeClick && this.props.onBadgeClick(id);
  };

  getFolderIcon = (item) => {
    let iconUrl = "images/catalog.folder.react.svg";

    switch (item.rootFolderType) {
      case FolderType.USER:
        iconUrl = "/static/images/catalog.user.react.svg";
        break;
      case FolderType.SHARE:
        iconUrl = "/static/images/catalog.shared.react.svg";
        break;
      case FolderType.COMMON:
        iconUrl = "/static/images/catalog.portfolio.react.svg";
        break;
      case FolderType.Favorites:
        iconUrl = "/static/images/catalog.favorites.react.svg";
        break;
      case FolderType.Recent:
        iconUrl = "/static/images/catalog.recent.react.svg";
        break;
      case FolderType.Privacy:
        iconUrl = "/static/images/catalog.private.react.svg";
        break;
      case FolderType.TRASH:
        iconUrl = "/static/images/catalog.trash.react.svg";
        break;
      default:
        break;
    }

    if (item.parentId !== 0)
      iconUrl = "/static/images/catalog.folder.react.svg";

    switch (item.providerKey) {
      case "GoogleDrive":
        iconUrl = "/static/images/cloud.services.google.drive.react.svg";
        break;
      case "Box":
        iconUrl = "/static/images/cloud.services.box.react.svg";
        break;
      case "DropboxV2":
        iconUrl = "/static/images/cloud.services.dropbox.react.svg";
        break;
      case "OneDrive":
        iconUrl = "/static/images/cloud.services.onedrive.react.svg";
        break;
      case "SharePoint":
        iconUrl = "/static/images/cloud.services.onedrive.react.svg";
        break;
      case "kDrive":
        iconUrl = "/static/images/catalog.folder.react.svg";
        break;
      case "Yandex":
        iconUrl = "/static/images/catalog.folder.react.svg";
        break;
      case "NextCloud":
        iconUrl = "/static/images/cloud.services.nextcloud.react.svg";
        break;
      case "OwnCloud":
        iconUrl = "/static/images/catalog.folder.react.svg";
        break;
      case "WebDav":
        iconUrl = "/static/images/catalog.folder.react.svg";
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

  showDragItems = (item) => {
    const {
      isAdmin,
      myId,
      commonId,
      //rootFolderType,
      currentId,
      draggableItems,
    } = this.props;
    if (item.id === currentId) {
      return false;
    }

    if (!draggableItems || draggableItems.find((x) => x.id === item.id))
      return false;

    // const isMy = rootFolderType === FolderType.USER;
    // const isCommon = rootFolderType === FolderType.COMMON;
    // const isShare = rootFolderType === FolderType.SHARE;

    if (
      item.rootFolderType === FolderType.SHARE &&
      item.access === ShareAccessRights.FullAccess
    ) {
      return true;
    }

    if (isAdmin) {
      //if (isMy || isCommon || isShare) {
      if (
        (item.pathParts &&
          (item.pathParts[0] === myId || item.pathParts[0] === commonId)) ||
        item.rootFolderType === FolderType.USER ||
        item.rootFolderType === FolderType.COMMON
      ) {
        return true;
      }
      //}
    } else {
      //if (isMy || isCommon || isShare) {
      if (
        (item.pathParts && item.pathParts[0] === myId) ||
        item.rootFolderType === FolderType.USER
      ) {
        return true;
      }
      //}
    }

    return false;
  };

  getItems = (data) => {
    const { withoutProvider } = this.props;
    return data.map((item) => {
      const dragging = this.props.dragging ? this.showDragItems(item) : false;

      const showBadge = item.newItems
        ? item.newItems > 0 && this.props.needUpdate
        : false;

      const provider = item.providerKey;

      const serviceFolder = !!item.providerKey;
      let className = `tree-drag tree-id_${item.id}`;

      if (withoutProvider && provider) return;

      if (dragging) className += " dragging";
      if ((item.folders && item.folders.length > 0) || serviceFolder) {
        return (
          <TreeNode
            id={item.id}
            key={item.id}
            className={className}
            title={item.title}
            icon={this.getFolderIcon(item)}
            dragging={dragging}
            isLeaf={
              item.rootFolderType === FolderType.Privacy &&
              !this.props.isDesktop
                ? true
                : null
            }
            newItems={
              !this.props.isDesktop &&
              item.rootFolderType === FolderType.Privacy
                ? null
                : item.newItems
            }
            providerKey={item.providerKey}
            onBadgeClick={this.onBadgeClick}
            showBadge={showBadge}
          >
            {item.rootFolderType === FolderType.Privacy && !this.props.isDesktop
              ? null
              : this.getItems(item.folders ? item.folders : [])}
          </TreeNode>
        );
      }
      return (
        <TreeNode
          id={item.id}
          key={item.id}
          className={className}
          title={item.title}
          needTopMargin={item.rootFolderType === FolderType.TRASH}
          dragging={dragging}
          isLeaf={
            (item.rootFolderType === FolderType.Privacy &&
              !this.props.isDesktop) ||
            !item.foldersCount
              ? true
              : false
          }
          icon={this.getFolderIcon(item)}
          newItems={
            !this.props.isDesktop && item.rootFolderType === FolderType.Privacy
              ? null
              : item.newItems
          }
          providerKey={item.providerKey}
          onBadgeClick={this.onBadgeClick}
          showBadge={showBadge}
        />
      );
    });
  };

  switcherIcon = (obj) => {
    if (obj.isLeaf) {
      return null;
    }
    if (obj.expanded) {
      return <StyledExpanderDownIcon size="scale" />;
    } else {
      return <StyledExpanderRightIcon size="scale" />;
    }
  };

  loop = (data, child, pos) => {
    let newPos = pos.split("-");
    let newData = data;

    newPos.shift();
    while (newPos.length !== 1) {
      const index = +newPos[0];
      newData = newData[index].folders;
      newPos.shift();
    }

    runInAction(() => {
      newData[newPos].folders = child;
    });
  };

  getNewTreeData(treeData, curId, child, pos) {
    this.loop(treeData, child, pos);
    this.setLeaf(treeData, curId, 10);
  }

  setLeaf(treeData, curKey, level) {
    const loopLeaf = (data, lev) => {
      const l = lev - 1;
      data.forEach((item) => {
        if (
          item.key.length > curKey.length
            ? item.key.indexOf(curKey) !== 0
            : curKey.indexOf(item.key) !== 0
        ) {
          return;
        }
        if (item.folders) {
          loopLeaf(item.folders, l);
        } else if (l < 1) {
          item.isLeaf = true;
        }
      });
    };
    loopLeaf(treeData, level + 1);
  }

  generateTreeNodes = (treeNode) => {
    const folderId = treeNode.props.id;
    const level = treeNode.props.pos;

    let arrayFolders;
    return this.props.getSubfolders(folderId).then((data) => {
      arrayFolders = data;

      const folderIndex = treeNode.props.pos;
      let i = 0;
      for (let item of arrayFolders) {
        item["key"] = `${folderIndex}-${i}`;
        i++;
      }

      return { folders: arrayFolders, listIds: [], level };
    });
  };

  onLoadData = (treeNode, isExpand) => {
    const { data: incomingDate, certainFolders } = this.props;
    isExpand && this.setState({ isExpand: true });
    this.props.setIsLoading && this.props.setIsLoading(true);
    //console.log("load data...", treeNode);

    if (this.state.isExpand && !isExpand) {
      return Promise.resolve();
    }

    return this.generateTreeNodes(treeNode)
      .then((data) => {
        const itemId = treeNode.props.id.toString();
        const listIds = data.listIds;
        listIds.push(itemId);

        const treeData = certainFolders
          ? incomingDate
          : [...this.props.treeFolders];

        this.getNewTreeData(treeData, listIds, data.folders, data.level);
        this.props.setTreeFolders(treeData);
      })
      .catch((err) => console.log("err", err))
      .finally(() => {
        this.setState({ isExpand: false });
        this.props.setIsLoading && this.props.setIsLoading(false);
      });
  };

  onExpand = (expandedKeys, treeNode) => {
    if (treeNode.node && !treeNode.node.props.children) {
      if (treeNode.expanded) {
        this.onLoadData(treeNode.node, true);
      }
    }
    if (this.props.needUpdate) {
      this.props.setExpandedKeys(expandedKeys);
    } else {
      this.props.setExpandedPanelKeys(expandedKeys);
    }
  };

  onDragOver = (data) => {
    const parentElement = data.event.target.parentElement;
    const existElement = parentElement.classList.contains(
      "rc-tree-node-content-wrapper"
    );

    if (existElement) {
      if (data.node.props.dragging) {
        parentElement.style.background = backgroundDragColor;
      }
    }
  };

  onDragLeave = (data) => {
    const parentElement = data.event.target.parentElement;
    const existElement = parentElement.classList.contains(
      "rc-tree-node-content-wrapper"
    );

    if (existElement) {
      if (data.node.props.dragging) {
        parentElement.style.background = backgroundDragEnterColor;
      }
    }
  };

  onDrop = (data) => {
    const { setDragging, onTreeDrop } = this.props;
    const { dragging, id } = data.node.props;
    //if (dragging) {
    setDragging(false);
    const promise = new Promise((resolve) =>
      onConvertFiles(data.event, resolve)
    );
    promise.then((files) => onTreeDrop(files, id));
    //}
  };

  render() {
    const {
      selectedKeys,
      isLoading,
      setIsLoading,
      onSelect,
      dragging,
      expandedKeys,
      expandedPanelKeys,
      treeFolders,
      data,
    } = this.props;

    return (
      <StyledTreeMenu
        className="files-tree-menu"
        checkable={false}
        draggable={dragging}
        disabled={isLoading}
        multiple={false}
        showIcon
        switcherIcon={this.switcherIcon}
        onSelect={onSelect}
        selectedKeys={selectedKeys}
        loadData={this.onLoadData}
        expandedKeys={expandedPanelKeys ? expandedPanelKeys : expandedKeys}
        onExpand={this.onExpand}
        onDragOver={this.onDragOver}
        onDragLeave={this.onDragLeave}
        onDrop={this.onDrop}
        dragging={dragging}
        gapBetweenNodes="22"
        gapBetweenNodesTablet="26"
        isFullFillSelection={false}
      >
        {this.getItems(data || treeFolders)}
      </StyledTreeMenu>
    );
  }
}

TreeFolders.defaultProps = {
  selectedKeys: [],
  needUpdate: true,
};

export default inject(
  (
    { auth, filesStore, treeFoldersStore, selectedFolderStore },
    { useDefaultSelectedKeys, selectedKeys }
  ) => {
    const {
      selection,
      setIsLoading,
      isLoading,
      dragging,
      setDragging,
    } = filesStore;

    const {
      treeFolders,
      setTreeFolders,
      myFolderId,
      commonFolderId,
      isPrivacyFolder,
      expandedKeys,
      setExpandedKeys,
      setExpandedPanelKeys,
      getSubfolders,
    } = treeFoldersStore;
    const { id /* rootFolderType */ } = selectedFolderStore;

    return {
      isAdmin: auth.isAdmin,
      isDesktop: auth.settingsStore.isDesktopClient,
      dragging,
      //rootFolderType,
      currentId: id,
      myId: myFolderId,
      commonId: commonFolderId,
      isPrivacy: isPrivacyFolder,
      draggableItems: dragging ? selection : null,
      expandedKeys,
      treeFolders,
      isLoading,
      selectedKeys: useDefaultSelectedKeys
        ? treeFoldersStore.selectedKeys
        : selectedKeys,

      setDragging,
      setIsLoading,
      setTreeFolders,
      setExpandedKeys,
      setExpandedPanelKeys,
      getSubfolders,
    };
  }
)(
  withTranslation(["Home", "Common"])(
    withLoader(observer(TreeFolders))(<Loaders.TreeFolders />)
  )
);
