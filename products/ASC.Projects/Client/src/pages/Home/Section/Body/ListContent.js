import React from "react";
import { withRouter } from "react-router";
import RowContent from "@appserver/components/row-content";
import Link from "@appserver/components/link";
import LinkWithDropdown from "@appserver/components/link-with-dropdown";
import Text from "@appserver/components/text";
import Box from "@appserver/components/box";
import { useTranslation } from "react-i18next";

const ListContent = ({ isMobile, list, sectionWidth }) => {
  const { title, description, participantCount, taskCount } = list;
  const { t } = useTranslation(["Home", "Common"]);

  const taskCountTitle = t("OpenTask") + ": " + taskCount;
  const participantCountTitle = t("Team") + ": " + participantCount;
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
        containerWidth="7%"
        type="page"
        title={taskCountTitle}
      >
        {taskCountTitle}
      </Link>
      <Link
        color="#A3A9AE"
        containerMinWidth="60px"
        containerWidth="5%"
        type="page"
        title={participantCountTitle}
      >
        {participantCountTitle}
      </Link>
      <Link
        color="#A3A9AE"
        containerMinWidth="60px"
        containerWidth="15%"
        type="page"
        title="byTitle"
      >
        byTitle
      </Link>
    </RowContent>
  );
};

export default withRouter(ListContent);
