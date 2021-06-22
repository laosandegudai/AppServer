import React, { Component } from "react";
import { withRouter } from "react-router";
import { withTranslation } from "react-i18next";
import styled from "styled-components";
import Text from "@appserver/components/text";
import Avatar from "@appserver/components/avatar";
import Row from "@appserver/components/row";
import RowContainer from "@appserver/components/row-container";
import ToggleButton from "@appserver/components/toggle-button";
import SearchInput from "@appserver/components/search-input";
import TabContainer from "@appserver/components/tabs-container";
import EmptyScreenContainer from "@appserver/components/empty-screen-container";
import Link from "@appserver/components/link";
import Box from "@appserver/components/box";

import PeopleSelector from "people/PeopleSelector";
import GroupSelector from "people/GroupSelector";

import toastr from "@appserver/components/toast/toastr";
import Loader from "@appserver/components/loader";
import { showLoader, hideLoader } from "@appserver/common/utils";
import { getUserRole } from "@appserver/people/src/helpers/people-helpers";

import { setDocumentTitle } from "../../../../../../helpers/utils";
import { inject } from "mobx-react";

const MainContainer = styled.div`
  width: 100%;

  .access-for-all-wrapper {
    background-color: #f8f9f9;
    padding: 14px;
    margin-bottom: 24px;
  }

  .toggle-btn {
    padding-top: 3px;
  }

  .text-wrapper {
    margin-left: 48px;
  }

  .search_container {
    margin: 12px 0;
  }

  .page_loader {
    position: fixed;
    left: 50%;
  }

  .empty_screen_children {
    padding-right: 7px;
  }
`;

class PeopleUsers extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isLoaded: false,
      accessForAll: false,
      searchValue: "",
      moduleId: "",
    };
  }

  async componentDidMount() {
    const { setAddUsers, modules } = this.props;
    showLoader();

    setAddUsers(this.addUsers);
    const peopleModule = modules.filter(
      (module) => module.appName === "people"
    );

    this.updatePeopleAccessList(peopleModule[0].id);

    this.setState({
      isLoaded: true,
      moduleId: peopleModule[0].id,
    });

    hideLoader();
  }

  componentWillUnmount() {
    const { setAddUsers, setCurrentTab, setSelected } = this.props;
    setAddUsers("");
    setCurrentTab("0");
    setSelected("none");
  }

  updatePeopleAccessList = async (moduleId) => {
    const {
      getSecuritySettings,
      setPeopleModuleGroups,
      setPeopleModuleUsers,
    } = this.props;

    const list = await getSecuritySettings(moduleId);

    setPeopleModuleGroups(list[0].groups);
    setPeopleModuleUsers(list[0].users);
  };

  onAccessForAllClick() {
    console.log("Access for all users");

    const { accessForAll } = this.state;
    this.setState({ accessForAll: !accessForAll });
  }

  onSearchChange = (value) => {
    if (this.state.searchValue === value) return false;

    this.setState({
      searchValue: value,
    });
  };

  onSelect = (items) => {
    const { toggleSelector } = this.props;

    toggleSelector(false);
    this.addUsers(items);
  };

  onGroupSelect = (items) => {
    const { toggleGroupSelector } = this.props;

    toggleGroupSelector(false);
    this.addGroups(items);
  };

  onCancelSelector = () => {
    this.props.toggleSelector(false);
  };

  onCancelGroupSelector = () => {
    this.props.toggleGroupSelector(false);
  };

  onToggleSelector = (isOpen = !this.props.selectorIsOpen) => {
    this.props.toggleSelector(isOpen);
  };

  onToggleGroupSelector = (isOpen = !this.props.groupSelectorIsOpen) => {
    this.props.toggleGroupSelector(isOpen);
  };

  getListKeys = () => {
    const { peopleModuleUsers, peopleModuleGroups } = this.props;
    const usersKey = peopleModuleUsers.map((user) => user.id);
    const groupsKey = peopleModuleGroups.map((group) => group.id);

    const allKeys = usersKey.concat(groupsKey);
    return allKeys;
  };

  addUsers = (users) => {
    const { t, setSecuritySettings, addPeopleModuleUsers } = this.props;
    const { moduleId } = this.state;
    const usersKey = users.map((user) => user.key);
    const allKeys = this.getListKeys().concat(usersKey);

    addPeopleModuleUsers(users);
    setSecuritySettings(moduleId, true, allKeys).then(() =>
      toastr.success(t("PeopleUsersAddedSuccessfully"))
    );
  };

  addGroups = (groups) => {
    const { t, setSecuritySettings, addPeopleModuleGroups } = this.props;
    const { moduleId } = this.state;
    const groupsKey = groups.map((group) => group.key);
    const allKeys = this.getListKeys().concat(groupsKey);

    addPeopleModuleGroups(groups);
    setSecuritySettings(moduleId, true, allKeys).then(() =>
      toastr.success(t("PeopleGroupsAddedSuccessfully"))
    );
  };

  selectTab = (e) => {
    this.props.setCurrentTab(e.key);
  };

  onRowSelect = (checked, user) => {
    const { selectUser, deselectUser } = this.props;

    if (checked) {
      selectUser(user);
    } else {
      deselectUser(user);
    }
  };

  getFilteredUsers = (users, searchValue) => {
    const filteredUsers = users.filter((user) => {
      if (
        user.displayName.toLowerCase().indexOf(searchValue.toLowerCase()) !== -1
      )
        return true;
      return false;
    });

    return filteredUsers;
  };

  getFilteredGroups = (groups, searchValue) => {
    const filteredGroups = groups.filter((group) => {
      if (group.name.toLowerCase().indexOf(searchValue.toLowerCase()) !== -1)
        return true;
      return false;
    });

    return filteredGroups;
  };

  getUsersContent = () => {
    const { searchValue } = this.state;
    const { isUserSelected, peopleModuleUsers } = this.props;

    const filteredUsers = searchValue
      ? this.getFilteredUsers(peopleModuleUsers, searchValue)
      : peopleModuleUsers;

    return (
      <RowContainer useReactWindow={false}>
        {filteredUsers.map((user) => {
          const userRole = getUserRole(user);

          const element = (
            <Avatar
              size="min"
              role={userRole}
              userName={user.displayName}
              source={user.avatarSmall}
            />
          );

          const checked = isUserSelected(user.id);
          console.log(this.props.selection);
          return (
            <Row
              key={user.id}
              onSelect={this.onRowSelect}
              data={user}
              element={element}
              checkbox={true}
              checked={checked}
              contextButtonSpacerWidth={"0px"}
            >
              <Text fontSize="15px" fontWeight="600" truncate={true}>
                {user.displayName}
              </Text>
            </Row>
          );
        })}
      </RowContainer>
    );
  };

  getGroupsContent = () => {
    const { peopleModuleGroups, isUserSelected } = this.props;
    const { searchValue } = this.state;

    const filteredGroups = searchValue
      ? this.getFilteredGroups(peopleModuleGroups, searchValue)
      : peopleModuleGroups;

    return (
      <RowContainer useReactWindow={false}>
        {filteredGroups.map((group) => {
          const checked = isUserSelected(group.id);
          console.log(this.props.selection);

          return (
            <Row
              key={group.id}
              onSelect={this.onRowSelect}
              data={group}
              checkbox={true}
              checked={checked}
              contextButtonSpacerWidth={"0px"}
            >
              <Text fontSize="15px" fontWeight="600" truncate={true}>
                {group.name}
              </Text>
            </Row>
          );
        })}
      </RowContainer>
    );
  };

  getEmptyScreen = () => {
    const { searchValue } = this.state;
    const { t } = this.props;

    return searchValue && searchValue.length > 0 ? (
      <EmptyScreenContainer
        imageSrc="products/people/images/empty_screen_filter.png"
        imageAlt="Empty Screen Filter image"
        headerText={t("NotFoundTitle")}
        descriptionText={t("NotFoundDescription")}
        buttons={
          <>
            <Link
              type="action"
              isHovered={true}
              onClick={this.onSearchChange.bind(this, "")}
            >
              {t("Common:ClearButton")}
            </Link>
          </>
        }
      />
    ) : (
      <EmptyScreenContainer
        imageSrc="products/people/images/empty_screen_filter.png"
        imageAlt="Empty List image"
        headerText={t("EmptyList")}
        descriptionText={t("EmptyListDescription")}
        buttons={
          <Box>
            <img
              className="empty_screen_children"
              src="products/files/images/plus.svg"
              alt="plus_icon"
            />

            <Link
              type="action"
              isHovered={true}
              onClick={this.onToggleSelector}
              className="empty_screen_children"
            >
              {t("AddUsers")},
            </Link>
            <Link
              type="action"
              isHovered={true}
              onClick={this.onToggleGroupSelector}
            >
              {t("AddGroups")}
            </Link>
          </Box>
        }
      />
    );
  };
  render() {
    const { isLoaded, accessForAll, searchValue } = this.state;
    const {
      t,
      selectorIsOpen,
      groupSelectorIsOpen,
      groupsCaption,
      peopleModuleUsers,
      peopleModuleGroups,
    } = this.props;

    const tabItems = [
      {
        key: "0",
        title: t("Users") + ` (${peopleModuleUsers.length})`,
        content:
          peopleModuleUsers.length > 0
            ? this.getUsersContent()
            : this.getEmptyScreen(),
      },
      {
        key: "1",
        title: t("Groups") + ` (${peopleModuleGroups.length})`,
        content:
          peopleModuleGroups.length > 0
            ? this.getGroupsContent()
            : this.getEmptyScreen(),
      },
    ];

    return !isLoaded ? (
      <Loader className="pageLoader" type="rombs" size="40px" />
    ) : (
      <MainContainer>
        <div className="access-for-all-wrapper">
          <div>
            <ToggleButton
              className="toggle-btn"
              isChecked={accessForAll}
              onChange={() => this.onAccessForAllClick()}
              isDisabled={false}
            />
          </div>
          <div className="text-wrapper">
            <Text isBold={true} fontWeight="600">
              {t("AccessForAllUsers")}
            </Text>
            <Text fontSize="12px">{t("AccessForAllUsersDescription")}</Text>
          </div>
        </div>

        <Text isBold={true} fontWeight="600" fontSize="16px">
          {t("AccessList")}
        </Text>

        <SearchInput
          className="search_container"
          placeholder={t("Common:Search")}
          onChange={this.onSearchChange}
          onClearSearch={this.onSearchChange}
          value={searchValue}
        />
        <PeopleSelector
          isMultiSelect={true}
          displayType="aside"
          isOpen={!!selectorIsOpen}
          onSelect={this.onSelect}
          groupsCaption={groupsCaption}
          onCancel={this.onCancelSelector}
        />
        <GroupSelector
          isOpen={!!groupSelectorIsOpen}
          isMultiSelect={true}
          onCancel={this.onCancelGroupSelector}
          onSelect={this.onGroupSelect}
          displayType="aside"
        />

        <TabContainer elements={tabItems} onSelect={this.selectTab} />
      </MainContainer>
    );
  }
}

export default inject(({ auth, setup }) => {
  const { moduleStore } = auth;
  const { modules } = moduleStore;
  const {
    setAddUsers,
    toggleSelector,
    toggleGroupSelector,
    getUsersByIds,
    setCurrentTab,
    getSecuritySettings,
    setSecuritySettings,
    setPeopleModuleUsers,
    setPeopleModuleGroups,
    addPeopleModuleUsers,
    addPeopleModuleGroups,
  } = setup;

  const {
    selectorIsOpen,
    groupSelectorIsOpen,
    peopleModuleUsers,
    peopleModuleGroups,
  } = setup.security.accessRight;

  const {
    selectUser,
    deselectUser,
    selection,
    isUserSelected,
    setSelected,
  } = setup.selectionStore;

  return {
    organizationName: auth.settingsStore.organizationName,
    modules,
    setAddUsers,
    toggleSelector,
    toggleGroupSelector,
    groupSelectorIsOpen,
    getUsersByIds,
    selectorIsOpen,
    groupsCaption: auth.settingsStore.customNames.groupsCaption,
    setCurrentTab,
    selectUser,
    deselectUser,
    selection,
    isUserSelected,
    setSelected,
    getSecuritySettings,
    setSecuritySettings,
    peopleModuleUsers,
    peopleModuleGroups,
    addPeopleModuleUsers,
    addPeopleModuleGroups,
    setPeopleModuleUsers,
    setPeopleModuleGroups,
  };
})(withTranslation(["Settings", "Common"])(withRouter(PeopleUsers)));
