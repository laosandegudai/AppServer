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

const StyledForm = styled(Box)`
  margin: 63px auto auto 216px;
  width: 570px;
  display: flex;
  flex-direction: column;

  .phone-edit-text {
    margin-bottom: 14px;
  }
`;

const PhoneForm = (props) => {
  const { t, currentPhone, greetingTitle } = props;

  const [phone, setPhone] = useState(currentPhone);
  const [isLoading, setIsLoading] = useState(false);

  const onSubmit = () => {
    console.log("Request the code");
  };

  const onKeyPress = (target) => {
    if (target.code === "Enter") onSubmit();
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
      <Box displayProp="flex">
        <Box className="phone-edit-input">
          {/* <TextInput
            id="phone"
            name="phone"
            type="text"
            size="base"
            scale={true}
            isAutoFocussed={true}
            tabIndex={1}
            autocomple="off"
            placeholder={phonePlaceholder}
            onChange={(event) => {
              setPhone(event.target.value);
              onKeyPress(event.target);
            }}
            value={phone}
            hasError={false}
            isDisabled={isLoading}
            onKeyDown={(event) => onKeyPress(event.target)}
            guide={false}
            mask={simplePhoneMask}
            className="edit-input"
          /> */}
          <PhoneInput
            locale="BY"
            searchEmptyMessage={t("SearchEmptyMessage")}
            searchPlaceholderText={t("SearchPlaceholderText")}
          />
        </Box>

        <Box className="phone-edit-btn" marginProp="0 0 0 8px">
          <Button
            primary
            size="medium"
            tabIndex={3}
            label={isLoading ? t("LoadingProcessing") : t("GetCode")}
            isDisabled={isLoading}
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
}))(withRouter(withTranslation("Confirm")(observer(ChangePhoneForm))));
