import React from "react";
import { inject, observer } from "mobx-react";
import styled, { css } from "styled-components";
import { withRouter } from "react-router";
import { withTranslation } from "react-i18next";
import Headline from "@appserver/common/components/Headline";
import IconButton from "@appserver/components/icon-button";
import GroupButtonsMenu from "@appserver/components/group-buttons-menu";
import DropDownItem from "@appserver/components/drop-down-item";

import { tablet, desktop } from "@appserver/components/utils/device";

import {
  getKeyByLink,
  settingsTree,
  getTKeyByKey,
  checkPropertyByLink,
} from "../../../utils";
import { combineUrl } from "@appserver/common/utils";
import { AppServerConfig } from "@appserver/common/constants";
import { isMobile } from "react-device-detect";

const HeaderContainer = styled.div`
  position: relative;
  display: flex;
  align-items: center;
  max-width: calc(100vw - 32px);

  .action-wrapper {
    flex-grow: 1;

    .action-button {
      margin-left: auto;
    }
  }

  .arrow-button {
    margin-right: 16px;

    @media ${tablet} {
      padding: 8px 0 8px 8px;
      margin-left: -8px;
    }
  }
`;

const StyledContainer = styled.div`
  .group-button-menu-container {
    margin: 0 -16px;
    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
    padding-bottom: 56px;

    ${isMobile &&
    css`
      position: sticky;
    `}

    ${(props) =>
      !props.isTabletView
        ? props.width &&
          isMobile &&
          css`
            width: ${props.width + 40 + "px"};
          `
        : props.width &&
          isMobile &&
          css`
            width: ${props.width + 32 + "px"};
          `}

    @media ${tablet} {
      padding-bottom: 0;
      ${!isMobile &&
      css`
        height: 56px;
      `}
      & > div:first-child {
        ${(props) =>
          !isMobile &&
          props.width &&
          css`
            width: ${props.width + 16 + "px"};
          `}

        position: absolute;
        ${(props) =>
          !props.isDesktop &&
          css`
            top: 56px;
          `}
        z-index: 180;
      }
    }

    @media ${desktop} {
      margin: 0 -24px;
    }
  }
`;

class SectionHeaderContent extends React.Component {
  constructor(props) {
    super(props);

    const { match, location } = props;
    const fullSettingsUrl = match.url;
    const locationPathname = location.pathname;

    const fullSettingsUrlLength = fullSettingsUrl.length;

    const resultPath = locationPathname.slice(fullSettingsUrlLength + 1);
    const arrayOfParams = resultPath.split("/");

    const key = getKeyByLink(arrayOfParams, settingsTree);
    const header = getTKeyByKey(key, settingsTree);
    const isCategory = checkPropertyByLink(
      arrayOfParams,
      settingsTree,
      "isCategory"
    );
    const isHeader = checkPropertyByLink(
      arrayOfParams,
      settingsTree,
      "isHeader"
    );
    this.state = {
      header,
      isCategoryOrHeader: isCategory || isHeader,
      showSelector: false,
      isHeaderVisible: false,
    };
  }

  componentDidUpdate() {
    const arrayOfParams = this.getArrayOfParams();

    const key = getKeyByLink(arrayOfParams, settingsTree);
    const header = getTKeyByKey(key, settingsTree);
    const isCategory = checkPropertyByLink(
      arrayOfParams,
      settingsTree,
      "isCategory"
    );
    const isHeader = checkPropertyByLink(
      arrayOfParams,
      settingsTree,
      "isHeader"
    );
    const isCategoryOrHeader = isCategory || isHeader;

    header !== this.state.header && this.setState({ header });
    isCategoryOrHeader !== this.state.isCategoryOrHeader &&
      this.setState({ isCategoryOrHeader });
  }

  onBackToParent = () => {
    let newArrayOfParams = this.getArrayOfParams();
    newArrayOfParams.splice(-1, 1);
    const newPath = "/settings/" + newArrayOfParams.join("/");
    this.props.history.push(combineUrl(AppServerConfig.proxyURL, newPath));
  };

  getArrayOfParams = () => {
    const { match, location } = this.props;
    const fullSettingsUrl = match.url;
    const locationPathname = location.pathname;

    const fullSettingsUrlLength = fullSettingsUrl.length;
    const resultPath = locationPathname.slice(fullSettingsUrlLength + 1);
    const arrayOfParams = resultPath.split("/").filter((param) => {
      return param !== "filter";
    });
    return arrayOfParams;
  };

  addUsers = (items) => {
    const { addUsers } = this.props;
    if (!addUsers) return;
    addUsers(items);
  };

  onToggleSelector = (isOpen = !this.props.selectorIsOpen) => {
    const { toggleSelector } = this.props;
    toggleSelector(isOpen);
  };

  onToggleGroupSelector = (isOpen = !this.props.groupSelectorIsOpen) => {
    const { toggleGroupSelector } = this.props;
    toggleGroupSelector(isOpen);
  };

  onClose = () => {
    const { deselectUser, moduleDeselectUser } = this.props;
    deselectUser();
    moduleDeselectUser();
  };

  onCheck = (checked) => {
    const { location, setSelected, moduleSetSelected } = this.props;

    const isPeopleModuleUsers = location.pathname.includes("people-users");

    if (isPeopleModuleUsers) {
      moduleSetSelected(checked ? "all" : "close");
    } else {
      setSelected(checked ? "all" : "close");
    }
  };

  onSelectAll = () => {
    const { location, setSelected, moduleSelectAll } = this.props;
    const isPeopleModuleUsers = location.pathname.includes("people-users");

    if (isPeopleModuleUsers) {
      moduleSelectAll("all");
    } else {
      setSelected("all");
    }
  };

  removeAdmins = () => {
    const { removeAdmins } = this.props;
    if (!removeAdmins) return;
    removeAdmins();
  };

  render() {
    const {
      t,
      addUsers,
      isHeaderIndeterminate,
      isHeaderChecked,
      isHeaderVisible,
      selection,
      currentTab,
      moduleIsHeaderVisible,
      moduleIsHeaderIndeterminate,
      moduleIsHeaderChecked,
    } = this.props;
    const { header, isCategoryOrHeader } = this.state;
    const arrayOfParams = this.getArrayOfParams();

    const menuItems = [
      {
        label: t("Common:Select"),
        isDropdown: true,
        isSeparator: true,
        isSelect: true,
        fontWeight: "bold",
        children: [
          <DropDownItem
            key="all"
            label={t("Common:SelectAll")}
            data-index={0}
            onClick={this.onSelectAll}
          />,
        ],
      },
      {
        label: t("Common:Delete"),
        disabled: !selection || !selection.length > 0,
        onClick: this.removeAdmins,
      },
    ];

    const headerVisible = isHeaderVisible || moduleIsHeaderVisible;
    const checked = isHeaderChecked || moduleIsHeaderChecked;
    const indeterminate = isHeaderIndeterminate || moduleIsHeaderIndeterminate;

    return (
      <StyledContainer isHeaderVisible={headerVisible}>
        {headerVisible ? (
          <div className="group-button-menu-container">
            <GroupButtonsMenu
              checked={checked}
              isIndeterminate={indeterminate}
              onChange={this.onCheck}
              menuItems={menuItems}
              visible={true}
              moreLabel={t("Common:More")}
              closeTitle={t("Common:CloseButton")}
              onClose={this.onClose}
              selected={menuItems[0].label}
            />
          </div>
        ) : (
          <HeaderContainer>
            {!isCategoryOrHeader && arrayOfParams[0] && (
              <IconButton
                iconName="/static/images/arrow.path.react.svg"
                size="17"
                color="#A3A9AE"
                hoverColor="#657077"
                isFill={true}
                onClick={this.onBackToParent}
                className="arrow-button"
              />
            )}
            <Headline type="content" truncate={true}>
              {t(header)}
            </Headline>
            {addUsers && (
              <div className="action-wrapper">
                <IconButton
                  iconName="/static/images/actions.header.touch.react.svg"
                  size="17"
                  color="#A3A9AE"
                  hoverColor="#657077"
                  isFill={true}
                  onClick={
                    currentTab === "0"
                      ? this.onToggleSelector
                      : this.onToggleGroupSelector
                  }
                  className="action-button"
                />
              </div>
            )}
          </HeaderContainer>
        )}
      </StyledContainer>
    );
  }
}

export default inject(({ auth, setup, module }) => {
  const { customNames } = auth.settingsStore;
  const { addUsers, removeAdmins } = setup.headerAction;
  const { toggleSelector, toggleGroupSelector } = setup;
  const {
    selected,
    setSelected,
    isHeaderIndeterminate,
    isHeaderChecked,
    isHeaderVisible,
    deselectUser,
    selectAll,
    selection,
  } = setup.selectionStore;
  const {
    admins,
    selectorIsOpen,
    groupSelectorIsOpen,
  } = setup.security.accessRight;

  const { currentTab } = module;

  return {
    addUsers,
    removeAdmins,
    groupsCaption: customNames.groupsCaption,
    selected,
    setSelected,
    admins,
    isHeaderIndeterminate,
    isHeaderChecked,
    isHeaderVisible,
    deselectUser,
    selectAll,
    selection,

    moduleIsHeaderVisible: module.isHeaderVisible,
    moduleIsHeaderIndeterminate: module.isHeaderIndeterminate,
    moduleIsHeaderChecked: module.isHeaderChecked,
    moduleDeselectUser: module.deselectUser,
    moduleSelectAll: module.selectAll,
    moduleSetSelected: module.setSelected,

    toggleSelector,
    selectorIsOpen,
    toggleGroupSelector,
    groupSelectorIsOpen,
    currentTab,
  };
})(
  withRouter(
    withTranslation(["Settings", "Common"])(observer(SectionHeaderContent))
  )
);
