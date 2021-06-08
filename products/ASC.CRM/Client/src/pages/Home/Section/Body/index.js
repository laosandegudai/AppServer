import React, { useEffect } from "react";
import { inject, observer } from "mobx-react";
import RowContainer from "@appserver/components/row-container";
import { Consumer } from "@appserver/components/utils/context";
import SimpleContactRow from "./SimpleContactRow";
import { isMobile } from "react-device-detect";
import Loaders from "@appserver/common/components/Loaders";

const SectionBodyContent = ({ isLoaded, contacts, fetchContacts }) => {
  useEffect(() => {
      fetchContacts();
  }, []);

  return (
      !isLoaded ? <Loaders.Rows isRectangle={false} /> :
    <>
      <Consumer>
        {(context) => (
          <RowContainer className="people-row-container" useReactWindow={false}>
            {contacts.map((contact) => (
              <SimpleContactRow
                key={contact.id}
                contact={contact}
                sectionWidth={context.sectionWidth}
                isMobile={isMobile}
              />
            ))}
          </RowContainer>
        )}
      </Consumer>
    </>
  );
};


export default inject(({ contactsStore, crmStore }) => {
  const { contacts, fetchContacts } = contactsStore;
  const { isLoaded } = crmStore;

  return {
    contacts,
    isLoaded,

    fetchContacts,
  };
})(observer(SectionBodyContent));
