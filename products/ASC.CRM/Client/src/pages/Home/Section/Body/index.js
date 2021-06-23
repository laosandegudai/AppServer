import React, { useEffect } from "react";
import { inject, observer } from "mobx-react";
import RowContainer from "@appserver/components/row-container";
import { Consumer } from "@appserver/components/utils/context";
import SimpleContactRow from "./SimpleContactRow";
import { isMobile } from "react-device-detect";
import Loaders from "@appserver/common/components/Loaders";
import RowContent from "@appserver/components/row-content";
import Row from "@appserver/components/row";
import Avatar from "@appserver/components/avatar";
import Link from "@appserver/components/link";
import Checkbox from "@appserver/components/checkbox";

const SectionBodyContent = ({ isLoaded, contacts }) => {
  return !isLoaded ? (
    <Loaders.Rows isRectangle={false} />
  ) : (
    <>
      {/* <Consumer>
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
      </Consumer> */}

      <RowContainer className='people-row-container' useReactWindow={false}>
        <Row
          data={{
            avatar: "",
            department: "Administration",
            email: "percival1979@yahoo.com",
            id: "1",
            isHead: false,
            mobilePhone: "+5 104 6473420",
            role: "owner",
            status: "normal",
            userName: "Jane Doe",
          }}
          contextOptions={[
            {
              key: "key1",
              label: "Send e-mail",
              onClick: function noRefCheck() {},
            },
            {
              key: "key2",
              label: "Send message",
              onClick: function noRefCheck() {},
            },
          ]}
          element={
            <>
              <Checkbox onChange={function noRefCheck() {}} />
              <Avatar role='owner' size='min' source='' userName='Jane Doe' />
            </>
          }
          status='normal'
        >
          <RowContent>
            <Link
              color='#333333'
              fontSize='15px'
              isBold
              isTextOverflow
              title='Jane Doe'
              type='page'
            >
              Jane Doe
            </Link>
            <React.Fragment key='.1' />
            <div
              style={{
                width: "110px",
              }}
            />
            <Link
              color='#A3A9AE'
              fontSize='12px'
              isTextOverflow
              title='Administration'
              type='action'
            >
              Administration
            </Link>
            <Link
              color='#A3A9AE'
              fontSize='12px'
              isTextOverflow
              title='+5 104 6473420'
              type='page'
            >
              +5 104 6473420
            </Link>
            <Link
              color='#A3A9AE'
              containerWidth='220px'
              fontSize='12px'
              isTextOverflow
              title='percival1979@yahoo.com'
              type='page'
            >
              percival1979@yahoo.com
            </Link>
          </RowContent>
        </Row>
      </RowContainer>
    </>
  );
};

export default inject(({ contactsStore, crmStore }) => {
  const { contacts } = contactsStore;
  const { isLoaded } = crmStore;

  return {
    contacts,
    isLoaded,
  };
})(observer(SectionBodyContent));
