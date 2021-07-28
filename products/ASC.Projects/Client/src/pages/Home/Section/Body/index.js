import { inject, observer } from "mobx-react";
import React from "react";
import { withTranslation } from "react-i18next";
import RowContainer from "@appserver/components/row-container";
import { Consumer } from "@appserver/components/utils/context";
import SimpleProjectsRow from "./SimpleProjectsRow";
import { isMobile } from "react-device-detect";
import EmptyScreen from "./EmptyScreen";
import withLoader from "../../../../HOCs/withLoader";
import Loaders from "@appserver/common/components/Loaders";

const PureSectionBodyContent = ({ items, tReady }) => {
  return items.length > 0 ? (
    <>
      <Consumer>
        {(context) => (
          <RowContainer
            className="people-row-container"
            useReactWindow={false}
            tReady={tReady}
          >
            {items.map((list) => (
              <SimpleProjectsRow
                isMobile={isMobile}
                key={list.id}
                list={list}
                sectionWidth={context.sectionWidth}
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

export default inject(({ projectsStore, projectsFilterStore }) => {
  const { items } = projectsStore;
  return {
    projectsStore,
    items,
  };
})(
  withTranslation(["Home", "Common"])(
    withLoader(observer(PureSectionBodyContent))(
      <Loaders.Rows isRectangle={false} />
    )
  )
);
