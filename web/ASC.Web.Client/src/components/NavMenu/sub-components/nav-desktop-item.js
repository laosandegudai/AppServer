import React, { useState } from "react";
import { ReactSVG } from "react-svg";
import Badge from "@appserver/components/badge";
import DropDown from "@appserver/components/drop-down";
import Text from "@appserver/components/text";
import Link from "@appserver/components/link";
import HelpButton from "@appserver/components/help-button";
import styled from "styled-components";
import { ReactSortable } from "react-sortablejs";

const separatorColor = "#3E668D";

const StyledNavDesktopItemSeparator = styled.div`
  height: 20px;
  margin: 0px 2px 0 20px;
  width: 1px;
  background: #3e668d;
  border-left: 1px solid ${separatorColor};
`;

const StyledNavDesktopItem = styled(Link)`
  cursor: pointer;
  display: flex;
  margin-left: 16px;

  .popup-icon {
    svg {
      height: 20px;
      width: 20px;
      path {
        fill: #fff;
      }
    }

    :hover {
      svg {
        path {
          fill: #657077;
        }
      }
    }
  }

  .popup-service-block {
    position: fixed;
    background: rgba(255, 255, 255, 0.94);
    z-index: 200;
    width: 100vw;
    height: calc(100vh - 56px);
    top: 56px;
    left: 0;

    .popup-service-block-wrapper {
      margin: 100px auto 0 auto;
      max-width: 944px;
      display: flex;
      flex-direction: column;
      justify-content: center;

      .popup-title {
        padding-bottom: 24px;
        margin-left: 10px;
        display: inline-flex;

        svg {
          width: 16px;
          height: 16px;
          padding-left: 8px;

          path {
            fill: #657077;
          }
        }
      }
    }
    .popup-service-close-icon {
      position: absolute;
      right: 20px;
      top: 20px;
      svg {
        height: 16px;
        width: 16px;
      }
    }
  }

  .popup-sortable-service-items {
    display: flex;
    flex-direction: row;

    .popup-sortable-service-item {
      height: 128px;
      width: 104px;
      border: 1px dashed #d0d5da;
      margin: 10px;

      display: flex;
      justify-content: center;
      flex-direction: column;
      align-items: center;
      padding: 24px 8px 13px;

      svg {
        height: 64px;
        width: 64px;
        path {
          fill: #3b72a7;
        }
      }

      .popup-sortable-pin-icon {
        position: absolute;
        margin: 0px 0px 130px 85px;
        svg {
          height: 16px;
          width: 16px;
          path {
            fill: #657077;
          }
        }
      }
    }
  }
`;

const StyledBadge = styled(Badge)`
  position: absolute;
  height: 16px;
  margin-top: -8px;
  margin-left: 8px;
`;

const StyledDropDown = styled.div`
  position: relative;

  .popup-drop-down {
    padding: 0;
  }

  .popup-header {
    width: 440px;
    height: 40px;
    border-radius: 6px 6px 0px 0px;
    background: linear-gradient(225deg, #2274aa 0%, #0f4071 100%);

    display: flex;
    align-items: center;

    .popup-mark-as-read {
      margin-left: 20px;
    }
    .popup-see-all {
      margin-left: auto;
      margin-right: 20px;
    }
  }

  .popup-body {
    width: 100%;
  }

  .popup-body-content {
    margin: 16px;
    height: 220px;
    border: 1px dashed #2da7db;

    display: flex;
    align-items: center;
    justify-content: center;
  }
`;

const NavDesktopItem = ({
  iconUrl,
  badgeNumber,
  url,
  separator,
  provider,
  onClick,
  modules,
  providers,
  ...rest
}) => {
  const [showPopup, setShowPopup] = useState(false);

  const onBadgeClick = () => setShowPopup(!showPopup);
  const onMarkAsRead = () => {};
  const onSeeAll = () => {};

  const getProviderIcon = (key) => {
    switch (key) {
      case "Google":
        return "/products/files/images/services/google_drive.svg";
      case "Facebook":
        return "/products/files/images/services/google_drive.svg";
      case "LinkedIn":
        return "/products/files/images/services/google_drive.svg";
      case "Mailru":
        return "/products/files/images/services/google_drive.svg";
      case "Vk":
        return "/products/files/images/services/google_drive.svg";
      case "Yandex":
        return "/products/files/images/services/google_drive.svg";
      default:
        return "";
    }
  };

  const clearModules = modules ? modules.filter((x) => !x.separator) : [];

  const fakeProviders = [];
  if (providers) {
    for (let item of providers) {
      const iconSrc = getProviderIcon(item);
      fakeProviders.push({ id: item, title: item, iconSrc, pinned: true });
    }
  }

  const [listModules, setListModules] = useState(clearModules);
  const [integrationsList, setIntegrationsList] = useState(fakeProviders);

  const [showServicePopup, setShowServicePopup] = useState(false);
  const onShowServicePanel = () => setShowServicePopup(!showServicePopup);

  const moduleTranslation = "Portal module";
  const integrationsTranslation = "Integrations";
  const tooltipContent =
    "Here are listed portal modules, pinned modules appear in the main menu bar. Also, you can rearrange all modules by dragging them.";

  const onPinModulesItem = (e) => {
    const id = e.currentTarget.dataset.id;
    const index = listModules.findIndex((x) => x.id === id);
    if (index !== -1) {
      listModules[index].pinned = !listModules[index].pinned;
      setListModules(listModules);
    }
  };

  const onPinIntegrationsItem = (e) => {
    const id = e.currentTarget.dataset.id;
    const index = integrationsList.findIndex((x) => x.id === id);
    if (index !== -1) {
      integrationsList[index].pinned = !integrationsList[index].pinned;
      setIntegrationsList(integrationsList);
    }
  };

  return separator ? (
    <StyledNavDesktopItemSeparator {...rest} />
  ) : (
    <StyledNavDesktopItem href={url} {...rest}>
      {listModules.length ? (
        <>
          <ReactSVG
            onClick={onShowServicePanel}
            className="popup-icon"
            src={iconUrl}
          />
          {showServicePopup && (
            <div className="popup-service-block">
              <ReactSVG
                onClick={onShowServicePanel}
                src="/static/images/cross.react.svg"
                className="popup-service-close-icon"
              />
              <div className="popup-service-block-wrapper">
                <div className="popup-title">
                  <Text fontSize="23px" fontWeight={600}>
                    {moduleTranslation}
                  </Text>
                  <HelpButton
                    iconName="/static/images/question.react.svg"
                    displayType="dropdown"
                    tooltipContent={tooltipContent}
                  />
                </div>

                <ReactSortable
                  className="popup-sortable-service-items"
                  list={listModules}
                  setList={setListModules}
                >
                  {listModules.map((item) => (
                    <div className="popup-sortable-service-item" key={item.id}>
                      <ReactSVG src={item.iconUrl} />
                      {item.pinned ? (
                        <div
                          className="popup-sortable-pin-icon"
                          data-id={item.id}
                          onClick={onPinModulesItem}
                        >
                          <ReactSVG src="/static/images/catalog.unpin.react.svg" />
                        </div>
                      ) : (
                        <div
                          className="popup-sortable-pin-icon"
                          data-id={item.id}
                          onClick={onPinModulesItem}
                        >
                          <ReactSVG src="/static/images/catalog.pin.react.svg" />
                        </div>
                      )}
                      {item.title}
                    </div>
                  ))}
                </ReactSortable>
                <br />
                <div className="popup-title">
                  <Text fontSize="23px" fontWeight={600}>
                    {integrationsTranslation}
                  </Text>
                  <HelpButton
                    iconName="/static/images/question.react.svg"
                    displayType="dropdown"
                    tooltipContent={tooltipContent}
                  />
                </div>
                <ReactSortable
                  className="popup-sortable-service-items"
                  list={integrationsList}
                  setList={setIntegrationsList}
                >
                  {integrationsList.map(({ id, title, iconSrc, pinned }) => (
                    <div className="popup-sortable-service-item" key={id}>
                      <ReactSVG src={iconSrc} />
                      {pinned ? (
                        <div
                          className="popup-sortable-pin-icon"
                          data-id={id}
                          onClick={onPinIntegrationsItem}
                        >
                          <ReactSVG src="/static/images/catalog.unpin.react.svg" />
                        </div>
                      ) : (
                        <div
                          className="popup-sortable-pin-icon"
                          data-id={id}
                          onClick={onPinIntegrationsItem}
                        >
                          <ReactSVG src="/static/images/catalog.pin.react.svg" />
                        </div>
                      )}
                      {title}
                    </div>
                  ))}
                </ReactSortable>
              </div>
            </div>
          )}
        </>
      ) : provider ? (
        <ReactSVG className="popup-icon" src={iconUrl} />
      ) : (
        <StyledDropDown>
          <ReactSVG className="popup-icon" src={iconUrl} />
          <StyledBadge label={badgeNumber} onClick={onBadgeClick} />
          {showPopup && (
            <StyledDropDown>
              <DropDown
                clickOutsideAction={onBadgeClick}
                className="popup-drop-down"
                open={showPopup}
              >
                <div className="popup-header">
                  <Link
                    className="popup-mark-as-read"
                    isHovered
                    type="action"
                    color="#fff"
                    fontSize="14px"
                    onClick={onMarkAsRead}
                  >
                    Mark as read
                  </Link>

                  <Link
                    className="popup-see-all"
                    isHovered
                    color="#7A95B0"
                    fontSize="14px"
                    onClick={onSeeAll}
                  >
                    See all
                  </Link>
                </div>
                <div className="popup-body">
                  <div className="popup-body-content">
                    <Text color="#2DA7DB" fontSize="21px">
                      Content
                    </Text>
                  </div>
                </div>
              </DropDown>
            </StyledDropDown>
          )}
        </StyledDropDown>
      )}
    </StyledNavDesktopItem>
  );
};

export default NavDesktopItem;
