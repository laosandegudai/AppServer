import React from "react";
import Row from "@appserver/components/row";
import Avatar from "@appserver/components/avatar";
import { inject, observer } from "mobx-react";
import { Trans, useTranslation } from "react-i18next";
import { AppServerConfig, EmployeeStatus } from "@appserver/common/constants";

const SimpleContactRow = ({ contact, sectionWidth, isMobile }) => {
  return (
    <Row sectionWidth={sectionWidth}>
      <RowContent></RowContent>
    </Row>
  );
};

export default inject(({ contactsStore }) => {
  const { contacts } = contactsStore;

  return {
    contacts,
  };
})(observer(SimpleContactRow));
