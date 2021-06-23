import React, { useState, useEffect } from "react";
import { withRouter } from "react-router";
import { withTranslation } from "react-i18next";
import styled from "styled-components";
import { inject, observer } from "mobx-react";

import Button from "@appserver/components/button";
import PhoneInput from "@appserver/components/phone-input";
import Text from "@appserver/components/text";
import PageLayout from "@appserver/common/components/PageLayout";
import Box from "@appserver/components/box";
import toastr from "@appserver/components/toast/toastr";
import { mobile, tablet } from "@appserver/components/utils/device";
import PhoneAuth from "./phoneAuth";
import withLoader from "../withLoader";

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

const ChangePhone = withLoader((props) => {
  const {
    t,
    currentPhone,
    greetingTitle,
    onSubmit,
    onChangePhone,
    isValid,
    phone,
    loading,
  } = props;

  const [width, setWidth] = useState(window.innerWidth);

  const isTabletWidth = width <= 1024;

  React.useEffect(() => {
    window.addEventListener("resize", () => setWidth(window.innerWidth));

    return () => {
      window.removeEventListener("resize", () => setWidth(window.innerWidth));
    };
  }, []);

  const onKeyPress = (e) => {
    if (e.keyCode === 13 && isValid) return onSubmit();
  };

  const splitted = navigator.language.split("-");
  const locale = splitted[splitted.length - 1];

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
            size={isTabletWidth ? "large" : "base"}
            isAutoFocussed
            tabIndex={1}
            locale={locale}
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
            size={isTabletWidth ? "large" : "medium"}
            tabIndex={3}
            label={loading ? t("Common:LoadingProcessing") : t("GetCode")}
            isDisabled={loading || !isValid}
            isLoading={loading}
            onClick={onSubmit}
          />
        </Box>
      </Box>
    </StyledForm>
  );
});

const ChangePhoneWrapper = (props) => {
  const [showPhoneAuth, setShowPhoneAuth] = useState(false);
  const [phone, setPhone] = useState("");
  const [isValid, setIsValid] = useState(false);
  const [fullNumber, setFullNumber] = useState(null);
  const [loading, setLoading] = useState(false);

  const {
    setAuthPhone,
    setPhoneNoise,
    location,
    setIsLoaded,
    setIsLoading,
  } = props;

  const onChangePhone = (obj) => {
    const { isValid, fullNumber, value } = obj;

    setPhone(value);
    setIsValid(isValid);
    if (isValid) setFullNumber(fullNumber);
  };

  const onSubmit = async () => {
    const { user, hash } = (location && location.state) || {};

    try {
      setLoading(true);
      const response = await setAuthPhone(user, hash, fullNumber);
      setPhoneNoise(response.phoneNoise);
      setLoading(false);
      setShowPhoneAuth(true);
    } catch (e) {
      setLoading(false);
      toastr.error(e);
    }
  };

  useEffect(() => {
    setIsLoaded(true);
    setIsLoading(false);
  }, []);

  return showPhoneAuth ? (
    <PhoneAuth />
  ) : (
    <PageLayout>
      <PageLayout.SectionBody>
        <ChangePhone
          onSubmit={onSubmit}
          onChangePhone={onChangePhone}
          isValid={isValid}
          phone={phone}
          loading={loading}
          {...props}
        />
      </PageLayout.SectionBody>
    </PageLayout>
  );
};

export default inject(({ auth, confirm }) => {
  const { currentPhone, greetingTitle } = auth;
  const { setAuthPhone, setPhoneNoise } = auth.tfaStore;
  const { setIsLoaded, setIsLoading } = confirm;

  return {
    setIsLoaded,
    setIsLoading,
    currentPhone,
    greetingTitle,
    setAuthPhone,
    setPhoneNoise,
  };
})(
  withRouter(
    withTranslation(["Confirm", "Common"])(observer(ChangePhoneWrapper))
  )
);
