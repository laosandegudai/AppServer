import { inject, observer } from "mobx-react";
import React from "react";
import { withTranslation } from "react-i18next";
import RowContainer from "@appserver/components/row-container";
import { Consumer } from "@appserver/components/utils/context";
import SimpleProjectsRow from "./SimpleProjectsRow";
import { isMobile } from "react-device-detect";

const PureSectionBodyContent = ({ items }) => {
  return items.length > 0 ? (
    <>
      <Consumer>
        {(context) => (
          <RowContainer
            className="people-row-container"
            useReactWindow={false}
            // tReady={tReady}
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
    <div>empty...</div>
  );
};

const SectionBodyContent = withTranslation(["Home", "Common"])(
  observer(PureSectionBodyContent)
);

export default inject(({ projectsStore, projectsFilterStore }) => {
  const { items } = projectsStore;
  return {
    projectsStore,
    items,
  };
})(observer(SectionBodyContent));
