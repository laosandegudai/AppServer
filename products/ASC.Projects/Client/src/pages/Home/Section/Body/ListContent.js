import React from "react";
import { withRouter } from "react-router";
import RowContent from "@appserver/components/row-content";
import Link from "@appserver/components/link";
import api from "@appserver/common/api";
const { ProjectsFilter, TasksFilter } = api;
import { useTranslation } from "react-i18next";
import moment from "moment";

const ListContent = ({ isMobile, list, sectionWidth, filter }) => {
  const {
    title,
    participantCount,
    taskCount,
    deadline,
    createdBy,
    responsible,
  } = list;
  const { t } = useTranslation(["Home", "Common"]);

  const firstTitle =
    filter instanceof ProjectsFilter
      ? `${t("OpenTask")}: ${taskCount}`
      : filter instanceof TasksFilter
      ? `${t("AddSubtask")}`
      : "title";

  const secondTitle =
    filter instanceof ProjectsFilter
      ? `${t("Team")}: ${participantCount}`
      : filter instanceof TasksFilter
      ? `${moment(deadline).format("l")}`
      : "title";

  const creator = responsible
    ? responsible.displayName
    : createdBy
    ? createdBy.displayName
    : "byTitle";
  return (
    <RowContent isMobile={isMobile} sectionWidth={sectionWidth} disableSideInfo>
      <Link
        containerWidth="28%"
        type="page"
        fontWeight={600}
        fontSize="15px"
        title={title}
      >
        {title}
      </Link>
      <React.Fragment key=".1" />
      <Link
        color="#A3A9AE"
        containerMinWidth="60px"
        containerWidth="10%"
        type="page"
        title={firstTitle}
      >
        {firstTitle}
      </Link>
      <Link
        color="#A3A9AE"
        containerMinWidth="60px"
        containerWidth="8%"
        type="page"
        title={secondTitle}
      >
        {secondTitle}
      </Link>
      <Link
        color="#A3A9AE"
        containerMinWidth="60px"
        containerWidth="9%"
        type="page"
        title="by Title"
      >
        {creator}
      </Link>
    </RowContent>
  );
};

export default withRouter(ListContent);
