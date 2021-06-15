import React from "react";
import styled, { css } from "styled-components";
import Loaders from "@appserver/common/components/Loaders";
import Headline from "@appserver/common/components/Headline";
import ContextMenuButton from "@appserver/components/context-menu-button";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import { tablet, desktop } from "@appserver/components/utils/device";
import { isMobile } from "react-device-detect";
import { Consumer } from "@appserver/components/utils/context";
import DropDownItem from "@appserver/components/drop-down-item";

const StyledContainer = styled.div`
  .header-container {
    position: relative;
    ${(props) =>
      props.title &&
      css`
        display: grid;
        grid-template-columns: ${(props) =>
          props.isRootFolder
            ? "auto auto 1fr"
            : props.canCreate
            ? "auto auto auto auto 1fr"
            : "auto auto auto 1fr"};

        @media ${tablet} {
          grid-template-columns: ${(props) =>
            props.isRootFolder
              ? "1fr auto"
              : props.canCreate
              ? "auto 1fr auto auto"
              : "auto 1fr auto"};
        }
      `}
    align-items: center;
    max-width: calc(100vw - 32px);

    @media ${tablet} {
      .headline-header {
        margin-left: -1px;
      }
    }
    .arrow-button {
      margin-right: 15px;
      min-width: 17px;

      @media ${tablet} {
        padding: 8px 0 8px 8px;
        margin-left: -8px;
        margin-right: 16px;
      }
    }

    .add-button {
      margin-bottom: -1px;
      margin-left: 16px;

      @media ${tablet} {
        margin-left: auto;

        & > div:first-child {
          padding: 8px 8px 8px 8px;
          margin-right: -8px;
        }
      }
    }

    .option-button {
      margin-bottom: -1px;

      @media (min-width: 1024px) {
        margin-left: 8px;
      }

      @media ${tablet} {
        & > div:first-child {
          padding: 8px 8px 8px 8px;
          margin-right: -8px;
        }
      }
    }
  }

  .group-button-menu-container {
    margin: 0 -16px;
    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
    padding-bottom: 56px;

    ${isMobile &&
    css`
      position: sticky;
    `}

    ${(props) =>
      !props.isTabletView
        ? props.width &&
          isMobile &&
          css`
            width: ${props.width + 40 + "px"};
          `
        : props.width &&
          isMobile &&
          css`
            width: ${props.width + 32 + "px"};
          `}

    @media ${tablet} {
      padding-bottom: 0;
      ${!isMobile &&
      css`
        height: 56px;
      `}
      & > div:first-child {
        ${(props) =>
          !isMobile &&
          props.width &&
          css`
            width: ${props.width + 16 + "px"};
          `}

        position: absolute;
        /* ${(props) =>
          !props.isDesktop &&
          css`
            top: 56px;
          `} */
        z-index: 180;
      }
    }

    @media ${desktop} {
      margin: 0 -24px;
    }
  }
`;

const PureSectionHeaderContent = ({ t, title }) => {
  const createNewCompany = () => {
    console.log("New Company clicked");
  };

  const createNewPerson = () => {
    console.log("New Person clicked");
  };

  const importContacts = () => {
    console.log("Import Contacts clicked");
  };

  const exportContacts = () => {
    console.log("Export Contacts clicked");
  };

  const getContextOptionsPlus = () => {
    return [
      {
        key: "new-company",
        label: t("NewCompany"),
        onClick: createNewCompany,
      },
      {
        key: "new-person",
        label: t("NewPerson"),
        onClick: createNewPerson,
      },
      { key: "separator", isSeparator: true },
      {
        key: "import-contacts",
        label: t("ImportContacts"),
        onClick: importContacts,
      },
      {
        key: "export-contacts",
        label: t("ExportContacts"),
        onClick: exportContacts,
      },
    ];
  };

  return (
    <Consumer>
      {(context) => (
        <StyledContainer width={context.sectionWidth} title={title}>
          <div className='header-container'>
            {!title ? (
              <Loaders.SectionHeader />
            ) : (
              <>
                <Headline
                  className='headline-header'
                  type='content'
                  truncate={true}
                >
                  {t(title)}
                </Headline>
                <ContextMenuButton
                  className='add-button'
                  directionX='right'
                  iconName='images/plus.svg'
                  size={17}
                  color='#A3A9AE'
                  hoverColor='#657077'
                  isFill
                  getData={getContextOptionsPlus}
                  isDisabled={false}
                />
              </>
            )}
          </div>
        </StyledContainer>
      )}
    </Consumer>
  );
};

const SectionHeaderContent = withTranslation("Home")(PureSectionHeaderContent);

export default inject(({ treeFoldersStore }) => {
  return {
    title: treeFoldersStore.title,
  };
})(observer(SectionHeaderContent));
