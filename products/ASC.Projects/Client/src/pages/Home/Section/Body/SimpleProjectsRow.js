import { inject, observer } from "mobx-react";
import React from "react";
import { withRouter } from "react-router";
import Row from "@appserver/components/row";
import ComboBox from "@appserver/components/combobox";
import ListContent from "./ListContent";
import { useTranslation } from "react-i18next";
import { ProjectOptionStatus, TaskOptionStatus } from "../../../../constants";
import styled from "styled-components";
import api from "@appserver/common/api";
const { ProjectsFilter, TasksFilter } = api;

const StyledComboBox = styled(ComboBox)`
  .combo-button_selected-icon {
    path {
      fill: #a3a9ae;
    }
  }
`;

const SimpleProjectsRow = ({
  list,
  sectionWidth,
  isMobile,
  selectProject,
  deselectProject,
  checked,
  filter,
  getProjectFilterRowContextOptions,
  getTaskFilterRowContextOptions,
}) => {
  const {
    status,
    id,
    title,
    created,
    description,
    canEdit,
    canDelete,
    taskCount,
    taskCountTotal,
    statusType,
  } = list;

  const { t } = useTranslation(["Home", "Common"]);

  const translations = {
    accept: t("Accept"),
    addSubtask: t("AddSubtask"),
    moveToMilestone: t("MoveToMilestone"),
    notifyResponsible: t("NotifyResponsible"),
    trackTime: t("TrackTime"),
    edit: t("Edit"),
    copy: t("Copy"),
    delete: t("Common:Delete"),
    editProject: t("EditProject"),
    deleteProject: t("DeleteProject"),
  };

  const checkedProps = { checked };

  const onContentRowSelect = (checked, list) =>
    checked ? selectProject(list) : deselectProject(list);

  const getProjectsContextOptions = (id) => {
    if (filter instanceof ProjectsFilter) {
      return getProjectFilterRowContextOptions(translations, id);
    }

    if (filter instanceof TasksFilter) {
      return getTaskFilterRowContextOptions(translations, id);
    }
  };

  const contextOptionsProps = {
    contextOptions: getProjectsContextOptions(id),
  };

  const getSelectedOption = () => {
    if (filter instanceof ProjectsFilter) {
      switch (list.status) {
        case ProjectOptionStatus.Active:
          return {
            icon: "images/catalog.status-play.react.svg",
            key: 1,
          };
        case ProjectOptionStatus.Closed:
          return {
            icon: "images/catalog.status-closed.react.svg",
            key: 2,
          };
        case ProjectOptionStatus.Paused:
          return {
            icon: "images/catalog.status-pause.react.svg",
            key: 3,
          };
      }
    }

    if (filter instanceof TasksFilter) {
      switch (list.status) {
        case TaskOptionStatus.Active:
          return {
            icon: "images/catalog.status-play.react.svg",
            key: 1,
          };

        case TaskOptionStatus.Closed:
          return {
            icon: "images/catalog.status-closed.react.svg",
            key: 2,
          };
      }
    }
  };

  const getComboBoxOptions = () => {
    const options = [
      {
        icon: "images/catalog.status-play.react.svg",
        key: 1,
        label: t("ComboOpen"),
      },
      {
        icon: "images/catalog.status-closed.react.svg",
        key: 2,
        label: t("ComboClosed"),
      },
    ];

    return filter instanceof ProjectsFilter
      ? [
          ...options,
          {
            icon: "images/catalog.status-pause.react.svg",
            key: 3,
            label: t("ComboPaused"),
          },
        ]
      : options;
  };

  const element = (
    <StyledComboBox
      options={getComboBoxOptions()}
      scaled={false}
      noBorder={true}
      selectedOption={getSelectedOption()}
      size="content"
    />
  );

  return (
    <Row
      onSelect={onContentRowSelect}
      checked={false}
      key={id}
      status={status}
      data={list}
      element={element}
      {...checkedProps}
      {...contextOptionsProps}
    >
      <ListContent
        isMobile={isMobile}
        sectionWidth={sectionWidth}
        list={list}
        filter={filter}
      />
    </Row>
  );
};

export default withRouter(
  inject(
    (
      { auth, projectsStore, projectsFilterStore, tasksFilterStore },
      { list }
    ) => {
      const { selectProject, deselectProject, filter } = projectsStore;
      const { getProjectFilterRowContextOptions } = projectsFilterStore;
      const { getTaskFilterRowContextOptions } = tasksFilterStore;
      return {
        auth,
        list,
        checked: projectsStore.selection.some((el) => el.id === list.id),
        selectProject,
        deselectProject,
        filter,
        getProjectFilterRowContextOptions,
        getTaskFilterRowContextOptions,
      };
    }
  )(observer(SimpleProjectsRow))
);
