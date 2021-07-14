import React from "react";
import Row from "@appserver/components/row";
import Avatar from "@appserver/components/avatar";
import { inject, observer } from "mobx-react";
import RowContent from "@appserver/components/row-content";
import Link from "@appserver/components/link";
import find from "lodash/find";
import result from "lodash/result";
import { Trans, useTranslation } from "react-i18next";

const SimpleContactRow = ({ contact, sectionWidth }) => {
  const { t } = useTranslation("Home");
  const { commonData, displayName, isCompany } = contact;

  const email = result(
    find(commonData, (value) => {
      return value.infoType === "email";
    }),
    "data"
  );

  const phone = result(
    find(commonData, (value) => {
      return value.infoType === "phone";
    }),
    "data"
  );

  const element = (
    <Avatar
      size="min"
      userName={displayName}
      source=""
      role="user"
      isCompany={isCompany}
    />
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
          label: t("OpenInTab"),
          onClick: function noRefCheck() {},
        },
        {
          key: "key2",
          label: t("AddPhone"),
          onClick: function noRefCheck() {},
        },
        {
          key: "key3",
          label: t("EditEmail"),
          onClick: function noRefCheck() {},
        },
        {
          key: "key4",
          label: t("WriteMessage"),
          onClick: function noRefCheck() {},
        },
        {
          isSeparator: true,
          key: "key5",
        },
        {
          key: "key6",
          label: t("EditContact"),
          onClick: function noRefCheck() {},
        },
        {
          key: "key6",
          label: t("DeleteContact"),
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
