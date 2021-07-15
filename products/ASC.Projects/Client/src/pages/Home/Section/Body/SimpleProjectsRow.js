import { inject, observer } from "mobx-react";
import React from "react";
import { withRouter } from "react-router";
import Row from "@appserver/components/row";
import ComboBox from "@appserver/components/combobox";
import ListContent from "./ListContent";
import { useTranslation } from "react-i18next";
import { RowProjectOptionStatus } from "../../../../constants";
import styled from "styled-components";
import { ReactSVG } from "react-svg";

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

  const checkedProps = { checked };

  const onContentRowSelect = (checked, list) => {
    console.log("dadada", checked);
    return checked ? selectProject(list) : deselectProject(list);
  };
  const getProjectsContextOptions = (id) => {
    const options = [
      {
        key: "accept",
        label: t("Accept"),
        "data-id": id,
      },
      {
        key: "add-subtask",
        label: t("AddSubtask"),
        "data-id": id,
      },
      {
        key: "moveto-milestone",
        label: t("MoveToMilestone"),
        "data-id": id,
      },
      {
        key: "notify",
        label: t("NotifyResponsible"),
        "data-id": id,
      },
      {
        key: "track-time",
        label: t("TrackTime"),
        "data-id": id,
      },
      {
        key: "edit",
        label: t("Edit"),
        "data-id": id,
      },
      {
        key: "copy",
        label: t("Copy"),
        "data-id": id,
      },
      {
        key: "delete",
        label: t("Delete"),
        "data-id": id,
      },
    ];
    return options;
  };

  const contextOptionsProps = {
    contextOptions: getProjectsContextOptions(id),
  };

  const getSelectedOption = () => {
    switch (list.status) {
      case RowProjectOptionStatus.Active:
        return {
          icon: "images/catalog.status-play.react.svg",
          key: 0,
          //label: "Open",
        };
      case RowProjectOptionStatus.Closed:
        return {
          icon: "images/catalog.status-closed.react.svg",
          key: 2,
          //label: "Closed",
        };
      case RowProjectOptionStatus.Paused:
        return {
          icon: "images/catalog.status-pause.react.svg",
          key: 1,
          //label: "Paused",
        };
    }
  };

  const element = (
    <StyledComboBox
      options={[
        {
          icon: "images/catalog.status-play.react.svg",
          key: 1,
          label: "Open",
        },
        {
          icon: "images/catalog.status-pause.react.svg",
          key: 2,
          label: "Paused",
        },
        {
          icon: "images/catalog.status-closed.react.svg",
          key: 3,
          label: "Closed",
        },
      ]}
      scaled={false}
      noBorder={true}
      style={{ fill: "#A3A9AE" }}
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
      />
    </Row>
  );
};

export default withRouter(
  inject(({ auth, projectsStore }, { list }) => {
    const { selectProject, deselectProject } = projectsStore;
    return {
      auth,
      list,
      checked: projectsStore.selection.some((el) => el.id === list.id),
      selectProject,
      deselectProject,
    };
  })(observer(SimpleProjectsRow))
);
