import React, { useState, useCallback, memo } from "react";
import PropTypes from "prop-types";
import DropDown from "./DropDown";
import { StyledPhoneInput, StyledInputBox } from "./StyledPhoneInput";
import Box from "../box";
import Text from "../text";
import { Base } from "../themes";
import { options, countryCodes } from "./options";

const PhoneInput = memo(
  ({ searchEmptyMessage, searchPlaceholderText, ...props }) => {
    const [country, setCountry] = useState(props.locale);

    const onChangeCountry = useCallback((country) => setCountry(country), [
      country,
    ]);

    const getLocaleCode = (locale) =>
      options.find((o) => o.code === locale).dialCode;

    const getMask = (locale) => options.find((o) => o.code === locale).mask;

    const getPlaceholder = (locale) =>
      options.find((o) => o.code === locale).mask === null
        ? "Enter phone number"
        : options
            .find((o) => o.code === locale)
            .mask.join("")
            .replace(/[\/|\\]/g, "")
            .replace(/[d]/gi, "X");

    return (
      <StyledInputBox>
        <Box
          displayProp="flex"
          borderProp={{
            style: "solid",
            width: "0 1px 0 0",
            color: "#D0D5DA",
          }}
          paddingProp="8px"
        >
          <DropDown
            value={country}
            onChange={onChangeCountry}
            options={options}
            theme={props.theme}
            searchPlaceholderText={searchPlaceholderText}
            searchEmptyMessage={searchEmptyMessage}
          />
        </Box>
        <Box displayProp="flex">
          <Box paddingProp={"6px 0 11px 8px"}>
            <Text>{getLocaleCode(country)}</Text>
          </Box>
          <div>
            <StyledPhoneInput
              mask={getMask(country)}
              placeholder={getPlaceholder(country)}
              {...props}
            />
          </div>
        </Box>
      </StyledInputBox>
    );
  }
);

PhoneInput.propTypes = {
  locale: PropTypes.oneOf(countryCodes),
  getLocaleCode: PropTypes.func,
  getMask: PropTypes.func,
  getPlaceholder: PropTypes.func,
  onChange: PropTypes.func,
  value: PropTypes.string,
  theme: PropTypes.object,
  searchPlaceholderText: PropTypes.string,
  searchEmptyMessage: PropTypes.string,
};

PhoneInput.defaultProps = {
  locale: "RU",
  type: "text",
  value: "",
  theme: Base,
  searchPlaceholderText: "Type to search country",
  searchEmptyMessage: "Nothing found",
};

PhoneInput.displayName = "PhoneInput";

export default PhoneInput;
