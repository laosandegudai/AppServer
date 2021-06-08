import React, { useState } from "react";
import { withRouter } from "react-router";
import { withTranslation } from "react-i18next";
import styled from "styled-components";
import Button from "@appserver/components/button";
import TextInput from "@appserver/components/text-input";
import PhoneInput from "@appserver/components/phone-input";
import Text from "@appserver/components/text";
import PageLayout from "@appserver/common/components/PageLayout";
import { inject, observer } from "mobx-react";
import Box from "@appserver/components/box";
import { mobile, tablet } from "@appserver/components/utils/device";

const StyledForm = styled(Box)`
  margin: 120px auto auto 216px;
  width: 570px;
  display: flex;
  flex-direction: column;

  @media ${tablet} {
    margin: 120px auto;
    width: 480px;
  }

  @media ${mobile} {
    margin: 72px 16px auto 8px;
    width: 311px;
  }

  .phone-edit-wrapper {
    @media ${tablet} {
      flex-direction: column;
    }
  }

  .phone-edit-text {
    margin-bottom: 14px;
  }

  .phone-edit-btn {
    @media ${tablet} {
      margin: 32px 0 0 0;
    }
  }
`;

const PhoneForm = (props) => {
  const { t, currentPhone, greetingTitle } = props;

  const [phone, setPhone] = useState("");
  const [isValid, setIsValid] = useState(false);
  const [fullNumber, setFullNumber] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [width, setWidth] = useState(window.innerWidth);

  React.useEffect(() => {
    window.addEventListener("resize", () => setWidth(window.innerWidth));
  }, []);

  const onChangePhone = (obj) => {
    setPhone(obj.value);
    setIsValid(obj.isValid);
    obj.isValid && setFullNumber(obj.fullNumber);
  };

  const onSubmit = () => {
    console.log(`Request the code for ${fullNumber}`);
  };

  const onKeyPress = (e) => {
    console.log(phone);
    if (e.keyCode === 13 && isValid) return onSubmit();
    return;
  };

  return (
    <StyledForm className="phone-edit-container">
      <Box className="phone-edit-title" marginProp="0 0 24px 0">
        <Text fontSize="32px" fontWeight="600">
          {greetingTitle}
        </Text>
      </Box>
      <Box className="phone-edit-description" marginProp="0 0 32px 0">
        <Text isBold fontSize="14px" className="phone-edit-text">
          {t("EnterNewMobile")}
        </Text>
        {currentPhone && (
          <Text className="phone-edit-text">
            {t("CurrentMobile")} <b>+{currentPhone}</b>
          </Text>
        )}
        <Text>{t("MobileChangeDescription")}</Text>
      </Box>
      <Box displayProp="flex" className="phone-edit-wrapper">
        <Box className="phone-edit-input">
          <PhoneInput
            size={width <= 1024 ? "large" : "base"}
            isAutoFocussed
            tabIndex={1}
            locale="AG"
            searchEmptyMessage={t("SearchEmptyMessage")}
            searchPlaceholderText={t("SearchPlaceholderText")}
            onChange={onChangePhone}
            value={phone}
            onKeyDown={onKeyPress}
          />
        </Box>
        <Box className="phone-edit-btn" marginProp="0 0 0 8px">
          <Button
            primary
            scale
            size={width <= 1024 ? "large" : "medium"}
            tabIndex={3}
            label={isLoading ? t("Common:LoadingProcessing") : t("GetCode")}
            isDisabled={!isValid || isLoading}
            isLoading={isLoading}
            onClick={onSubmit}
          />
        </Box>
      </Box>
    </StyledForm>
  );
};

const ChangePhoneForm = (props) => {
  return (
    <PageLayout>
      <PageLayout.SectionBody>
        <PhoneForm {...props} />
      </PageLayout.SectionBody>
    </PageLayout>
  );
};

export default inject(({ auth }) => ({
  isLoaded: auth.isLoaded,
  currentPhone: auth.userStore.mobilePhone,
  greetingTitle: auth.settingsStore.greetingSettings,
}))(
  withRouter(withTranslation(["Confirm", "Common"])(observer(ChangePhoneForm)))
);
