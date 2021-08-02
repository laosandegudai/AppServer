import React from "react";
import { inject, observer } from "mobx-react";
import RowContainer from "@appserver/components/row-container";
import { Consumer } from "@appserver/components/utils/context";
import SimpleContactRow from "./SimpleContactRow";
import { isMobile } from "react-device-detect";
import Loaders from "@appserver/common/components/Loaders";
import EmptyScreen from "./EmptyScreen";

const SectionBodyContent = ({ isLoaded, contactsList, tReady }) => {
  return !isLoaded ? (
    <Loaders.Rows isRectangle={false} />
  ) : contactsList.length > 0 ? (
    <>
      <Consumer>
        {(context) => (
          <RowContainer
            className="contact-row-container"
            useReactWindow={false}
            tReady={tReady}
          >
            {contactsList.map((contact) => (
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
  ) : (
    <EmptyScreen />
  );
};

export default inject(({ contactsStore, crmStore }) => {
  const { contactsList } = contactsStore;
  const { isLoaded } = crmStore;

  return {
    contactsList,
    isLoaded,
  };
})(observer(SectionBodyContent));
