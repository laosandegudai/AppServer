import React from "react";
import Row from "@appserver/components/row";
import Avatar from "@appserver/components/avatar";
import { inject, observer } from "mobx-react";
import { Trans, useTranslation } from "react-i18next";
import { AppServerConfig, EmployeeStatus } from "@appserver/common/constants";
import RowContent from "@appserver/components/row-content";
import Link from "@appserver/components/link";

const SimpleContactRow = ({ contact, sectionWidth, isMobile }) => {
  const {
    about,
    accessList,
    canDelete,
    canEdit,
    commonData,
    createdBy,
    created,
    currency,
    displayName,
    firstName,
    lastName,
    haveLateTasks,
    id,
    industry,
    isCompany,
    isPrivate,
    isShared,
    mediumFotoUrl,
    shareType,
    smallFotoUrl,
    title,
  } = contact;

  const element = (
    <Avatar size="min" userName={displayName} source="" role="user" />
  );

  return (
    <Row
      sectionWidth={sectionWidth}
      data={contact}
      element={element}
      checked={false}
      contextOptions={[
        {
          key: "key1",
          label: "Open in a new tab",
          onClick: function noRefCheck() {},
        },
        {
          key: "key2",
          label: "Add phone",
          onClick: function noRefCheck() {},
        },
        {
          key: "Edit e-mail",
          label: "Open in a new tab",
          onClick: function noRefCheck() {},
        },
        {
          key: "key4",
          label: "Write message",
          onClick: function noRefCheck() {},
        },
        {
          isSeparator: true,
          key: "key5",
        },
        {
          key: "key6",
          label: "Edit contact",
          onClick: function noRefCheck() {},
        },
        {
          key: "key6",
          label: "Delete contact",
          onClick: function noRefCheck() {},
        },
      ]}
    >
      <RowContent>
        <Link
          color="#333333"
          fontSize="15px"
          isBold
          isTextOverflow
          title={displayName}
          type="page"
        >
          {displayName}
        </Link>
        <React.Fragment key=".1" />
        <div
          style={{
            width: "110px",
          }}
        />
        <Link
          color="#A3A9AE"
          fontSize="12px"
          isTextOverflow
          title="Visitors"
          type="action"
        >
          Visitors
        </Link>
        <Link
          color="#A3A9AE"
          fontSize="12px"
          isTextOverflow
          title="+7 715 6018678"
          type="page"
        >
          +7 715 6018678
        </Link>
        <Link
          color="#A3A9AE"
          containerWidth="220px"
          fontSize="12px"
          isTextOverflow
          title="fidel_kerlu@hotmail.com"
          type="page"
        >
          fidel_kerlu@hotmail.com
        </Link>
      </RowContent>
    </Row>
  );
};

export default inject(({ contactsStore }) => {
  const { contacts } = contactsStore;

  return {
    contacts,
  };
})(observer(SimpleContactRow));
