import React, { useEffect, useState } from "react";
import { withRouter } from "react-router";
import { withTranslation, Trans } from "react-i18next";
import styled from "styled-components";
import { inject, observer } from "mobx-react";

import Button from "@appserver/components/button";
import TextInput from "@appserver/components/text-input";
import Text from "@appserver/components/text";
import PageLayout from "@appserver/common/components/PageLayout";
import Box from "@appserver/components/box";
import Link from "@appserver/components/link";
import toastr from "@appserver/components/toast/toastr";
import { mobile, tablet } from "@appserver/components/utils/device";
import withLoader from "../withLoader";

const StyledForm = styled(Box)`
  margin: 63px auto auto 216px;
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

  .phone-code-text {
    margin-bottom: 14px;
  }

  .phone-code-wrapper {
    @media ${tablet} {
      flex-direction: column;
    }
  }

  .phone-code-continue-btn {
    @media ${tablet} {
      margin: 32px 0 0 0;
    }
  }
`;

const PhoneAuth = withLoader((props) => {
  const {
    t,
    requestSmsCode,
    loginWithCode,
    phoneNoise,
    location,
    history,
  } = props;

  const [code, setCode] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [timer, setTimer] = useState(30);
  const [width, setWidth] = useState(window.innerWidth);

  const isValidLength = code.length === 6;
  const isTicking = timer > 0;
  const isTabletWidth = width <= 1024;

  React.useEffect(() => {
    window.addEventListener("resize", () => setWidth(window.innerWidth));

    return () => {
      window.removeEventListener("resize", () => setWidth(window.innerWidth));
    };
  }, []);

  const onChangeCode = (e) => setCode(e.target.value);

  const onSubmit = async () => {
    const { user, hash } = (location && location.state) || {};

    try {
      setIsLoading(true);
      const url = await loginWithCode(user, hash, code);
      setIsLoading(false);
      history.push(url || "/");
    } catch (e) {
      setIsLoading(false);
      toastr.error(e);
    }
  };

  const onKeyPress = (target) => {
    if (target.code === "Enter" && isValidLength) return onSubmit();
  };

  const sendSmsCodeAgain = () => {
    const { user, hash } = (location && location.state) || {};

    try {
      setTimer(30);
      requestSmsCode(user, hash);
    } catch (e) {
      toastr.error(e);
    }
  };

  useEffect(() => {
    if (isTicking) {
      setTimeout(() => setTimer(timer - 1), 1000);
    }
  });

  return (
    <StyledForm className="phone-code-container">
      <Box className="phone-code-description" marginProp="0 0 32px 0">
        <Text isBold fontSize="14px" className="phone-code-text">
          {t("EnterCodeTitle")}
        </Text>
        <Text>
          <Trans t={t} i18nKey="EnterCodeDescription" ns="Confirm">
            An SMS with portal access code has been sent to your
            <b>{phoneNoise ? { phoneNoise } : ""}</b>
            mobile number. Please enter the code and click the "Continue"
            button. If no message is received for more than three minutes, click
            the "Send code again" link.
          </Trans>
        </Text>
      </Box>
      <Box displayProp="flex" className="phone-code-wrapper">
        <Box className="phone-code-input">
          <TextInput
            id="code"
            name="code"
            type="text"
            size={isTabletWidth ? "large" : "base"}
            scale
            isAutoFocussed
            tabIndex={1}
            maxLength={6}
            placeholder={t("EnterCodePlaceholder")}
            isDisabled={isLoading}
            onChange={onChangeCode}
            value={code}
            onKeyDown={onKeyPress}
          />
        </Box>
        <Box className="phone-code-continue-btn" marginProp="0 0 0 8px">
          <Button
            scale
            primary
            size={isTabletWidth ? "large" : "medium"}
            tabIndex={3}
            label={isLoading ? t("Common:LoadingProcessing") : t("Continue")}
            isDisabled={!isValidLength || isLoading}
            isLoading={isLoading}
            onClick={onSubmit}
          />
        </Box>
      </Box>
      <Box marginProp="32px 0 0 0">
        <Text
          color={isTicking ? "#D0D5DA" : "#316DAA"}
          fontWeight="600"
          textAlign={isTabletWidth ? "center" : null}
        >
          {isTicking ? (
            `${t("SendCodeAgain")} (${timer} ${t("Second")})`
          ) : (
            <Link fontWeight="600" color="#316DAA" onClick={sendSmsCodeAgain}>
              {t("SendCodeAgain")}
            </Link>
          )}
        </Text>
      </Box>
    </StyledForm>
  );
});

const PhoneAuthWrapper = (props) => {
  const { setIsLoaded, setIsLoading } = props;

  useEffect(() => {
    setIsLoaded(true);
    setIsLoading(false);
  }, []);

  return (
    <PageLayout>
      <PageLayout.SectionBody>
        <PhoneAuth {...props} />
      </PageLayout.SectionBody>
    </PageLayout>
  );
};

export default inject(({ auth, confirm }) => {
  const { loginWithCode, isLoaded, login, loginWithSmsCode, smsLogin } = auth;
  const { requestSmsCode, loginWithCodeAndCookie, phoneNoise } = auth.tfaStore;
  const { setIsLoaded, setIsLoading } = confirm;

  return {
    loginWithCode,
    isLoaded,
    login,
    requestSmsCode,
    loginWithSmsCode,
    loginWithCodeAndCookie,
    smsLogin,
    phoneNoise,
    setIsLoaded,
    setIsLoading,
  };
})(
  withRouter(withTranslation(["Confirm", "Common"])(observer(PhoneAuthWrapper)))
);
