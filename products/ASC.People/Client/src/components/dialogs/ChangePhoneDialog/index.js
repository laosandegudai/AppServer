import React from "react";
import PropTypes from "prop-types";
import ModalDialog from "@appserver/components/modal-dialog";
import Button from "@appserver/components/button";
import Text from "@appserver/components/text";
import { withTranslation } from "react-i18next";
import toastr from "studio/toastr";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router";

class ChangePhoneDialogComponent extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      isRequestRunning: false,
    };
  }

  onChangePhone = () => {
    const {
      onClose,
      t,
      resetMobilePhone,
      targetUser,
      isMe,
      logout,
      history,
    } = this.props;

    this.setState({ isRequestRunning: true }, () => {
      const data = isMe ? "" : targetUser.id;

      resetMobilePhone(data).then((url) => {
        const newUrl = url.replace(window.location.origin, "");
        if (isMe) {
          logout(true);
          history.push(newUrl);
        } else {
          toastr.success(t("ChangePhoneInstructionSent"));
        }
      });

      this.setState({ isRequestRunning: false }, () => onClose());
    });
  };

  render() {
    console.log("ChangePhoneDialog render");
    const { t, visible, onClose, isAdmin, isMe } = this.props;
    const { isRequestRunning } = this.state;

    return (
      <ModalDialog visible={visible} onClose={onClose}>
        <ModalDialog.Header>{t("MobilePhoneChangeTitle")}</ModalDialog.Header>
        <ModalDialog.Body>
          <Text>
            {isAdmin && !isMe
              ? t("MobilePhoneUserEraseDescription")
              : t("MobilePhoneEraseDescription")}
          </Text>
        </ModalDialog.Body>
        <ModalDialog.Footer>
          <Button
            key="SendBtn"
            label={t("Common:SendButton")}
            size="medium"
            primary
            onClick={this.onChangePhone}
            isLoading={isRequestRunning}
          />
        </ModalDialog.Footer>
      </ModalDialog>
    );
  }
}

const ChangePhoneDialog = withRouter(
  withTranslation(["ChangePhoneDialog", "Common"])(ChangePhoneDialogComponent)
);

ChangePhoneDialog.propTypes = {
  visible: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  user: PropTypes.object.isRequired,
};

export default inject(({ auth, peopleStore }) => ({
  isAdmin: auth.isAdmin,
  logout: auth.logout,
  isMe: peopleStore.targetUserStore.isMe,
  resetMobilePhone: peopleStore.targetUserStore.resetMobilePhone,
  targetUser: peopleStore.targetUserStore.targetUser,
}))(observer(ChangePhoneDialog));
