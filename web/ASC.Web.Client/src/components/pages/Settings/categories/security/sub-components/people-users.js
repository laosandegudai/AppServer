import React, { Component } from "react";
import { withRouter } from "react-router";
import { withTranslation } from "react-i18next";
import styled from "styled-components";
import Text from "@appserver/components/text";
import RadioButtonGroup from "@appserver/components/radio-button-group";
import Button from "@appserver/components/button";
import ToggleButton from "@appserver/components/toggle-button";
import toastr from "@appserver/components/toast/toastr";
import Loader from "@appserver/components/loader";
import { showLoader, hideLoader } from "@appserver/common/utils";

import { setDocumentTitle } from "../../../../../../helpers/utils";
import { inject } from "mobx-react";

const MainContainer = styled.div`
  width: 100%;

  .access-for-all-wrapper {
    background-color: #f8f9f9;
    padding: 14px;
  }

  .text-wrapper {
    margin-left: 48px;
  }

  .page_loader {
    position: fixed;
    left: 50%;
  }
`;

class PeopleUsers extends Component {
  constructor(props) {
    super(props);

    const { t } = props;
    this.state = {
      isLoaded: false,
      accessForAll: false,
    };
  }

  async componentDidMount() {
    showLoader();
    hideLoader();
    this.setState({ isLoaded: true });
  }

  onAccessForAllClick() {
    console.log("Access for all users");

    const { accessForAll } = this.state;
    this.setState({ accessForAll: !accessForAll });
  }

  render() {
    const { isLoaded, accessForAll } = this.state;
    const { t } = this.props;

    return !isLoaded ? (
      <Loader className="pageLoader" type="rombs" size="40px" />
    ) : (
      <MainContainer>
        <div className="access-for-all-wrapper">
          <div>
            <ToggleButton
              //className="toggle-btn"
              isChecked={accessForAll}
              onChange={() => this.onAccessForAllClick()}
              isDisabled={false}
            />
          </div>
          <div className="text-wrapper">
            <Text isBold={true} fontWeight="600">
              {t("AccessForAllUsers")}
            </Text>
            <Text fontSize="12">{t("AccessForAllUsersDescription")}</Text>
          </div>
        </div>
      </MainContainer>
    );
  }
}

export default inject(({ auth }) => ({
  organizationName: auth.settingsStore.organizationName,
}))(withTranslation(["Settings", "Common"])(withRouter(PeopleUsers)));
