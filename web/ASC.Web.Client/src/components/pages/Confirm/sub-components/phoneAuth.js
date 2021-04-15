import React, { useEffect, useState } from "react";
import { withRouter } from "react-router";
import { withTranslation } from "react-i18next";
import styled from "styled-components";
import Button from "@appserver/components/button";
import TextInput from "@appserver/components/text-input";
import Text from "@appserver/components/text";
import PageLayout from "@appserver/common/components/PageLayout";
import { inject, observer } from "mobx-react";
import Box from "@appserver/components/box";
import Link from "@appserver/components/link";

const StyledForm = styled(Box)`
  margin: 63px auto auto 216px;
  width: 570px;
  display: flex;
  flex-direction: column;

  .phone-code-text {
    margin-bottom: 14px;
  }
`;

const PhoneAuthForm = (props) => {
  const { t } = props;

  const [code, setCode] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [timer, setTimer] = useState(30);

  const onSubmit = () => {
    console.log("POST api/2.0/authentication/{code}"); // https://api.onlyoffice.com/portals/method/authentication/post/api/2.0/authentication/%7bcode%7d
  };

  const onKeyPress = (target) => {
    if (target.code === "Enter") onSubmit();
  };

  useEffect(() => {
    if (timer > 0) {
      setTimeout(() => setTimer(timer - 1), 1000);
    }
  });

  return (
    <StyledForm className="phone-code-container">
      <Box className="phone-code-description" marginProp="0 0 32px 0">
        <Text isBold fontSize="14px" className="phone-code-text">
          {t("EnterCodeTitle")}
        </Text>
        <Text>{t("EnterCodeDescription")}</Text>
      </Box>
      <Box displayProp="flex">
        <Box className="phone-code-input">
          <TextInput
            id="code"
            name="code"
            type="text"
            size="base"
            scale
            isAutoFocussed
            tabIndex={1}
            placeholder={t("EnterCodePlaceholder")}
            isDisabled={isLoading}
            onChange={(e) => setCode(e.target.value)}
            value={code}
          />
        </Box>
        <Box className="phone-code-continue-btn" marginProp="0 0 0 8px">
          <Button
            primary
            size="medium"
            tabIndex={3}
            label={isLoading ? t("LoadingProcessing") : t("Continue")}
            isDisabled={!code.length || isLoading}
            isLoading={isLoading}
            onClick={onSubmit}
          />
        </Box>
      </Box>
      <Box marginProp="32px 0 0 0">
        {timer > 0 ? (
          <Text color="#D0D5DA" fontWeight="600">
            {t("SendCodeAgain")} ({timer} {t("Second")})
          </Text>
        ) : (
          <Link
            fontWeight="600"
            color="#316DAA"
            onClick={() => {
              console.log("POST api/2.0/authentication/sendsms"); // https://api.onlyoffice.com/portals/method/authentication/post/api/2.0/authentication/sendsms
              setTimer(30);
            }}
          >
            {t("SendCodeAgain")}
          </Link>
        )}
      </Box>
    </StyledForm>
  );
};

const PhoneAuthFormWrapper = (props) => {
  return (
    <PageLayout>
      <PageLayout.SectionBody>
        <PhoneAuthForm {...props} />
      </PageLayout.SectionBody>
    </PageLayout>
  );
};

export default inject(({ auth }) => ({
  isLoaded: auth.isLoaded,
}))(withRouter(withTranslation("Confirm")(observer(PhoneAuthFormWrapper))));
