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
    createBy,
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

  // const email = commonData ? commonData[0].data : "";
  // const phone = commonData ? commonData[1].data : "";

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
          key: "key3",
          label: "Edit e-mail",
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
        {/* <Link
          color="#A3A9AE"
          fontSize="12px"
          isTextOverflow
          title={phone}
          type="action"
          containerMinWidth="120px"
        >
          {phone}
        </Link>
        <Link
          color="#A3A9AE"
          fontSize="12px"
          isTextOverflow
          title={email}
          type="page"
          containerMinWidth="120px"
        >
          {email}
        </Link> */}
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
