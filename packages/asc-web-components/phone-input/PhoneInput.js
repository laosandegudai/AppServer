import React, { useState, useCallback, memo } from "react";
import PropTypes from "prop-types";
import DropDown from "./DropDown";
import {
  StyledPhoneInput,
  StyledDialCode,
  StyledFlagBoxWrapper,
} from "./StyledPhoneInput";
import Box from "../box";
import Text from "../text";
import { Base } from "../themes";
import { options } from "./options";

const PhoneInput = memo(
  ({ searchEmptyMessage, searchPlaceholderText, onChange, ...props }) => {
    const [locale, setCountry] = useState(props.locale || options[0].code);

    const getCountry = (locale) => {
      return options.find((o) => o.code === locale) || options[0];
    };

    const country = getCountry(locale);

    const placeholder =
      country.mask === null
        ? "Enter phone number"
        : country.mask
            .join("")
            .replace(/[\/|\\]/g, "")
            .replace(/[d]/gi, "X");

    const onChangeCountry = useCallback((country) => setCountry(country), [
      locale,
    ]);

    const onChangeWrapper = (e) => {
      const value = e.target.value.trim();
      const dialCode = country.dialCode;
      const fullNumber = dialCode.concat(value);
      const locale = country;
      const mask = country.mask;
      const isValid = mask
        ? !!value.length && !Array.isArray(value.match(/_/gm))
        : !!value.length && Array.isArray(value.match(/^[0-9]+$/));

      onChange &&
        onChange({
          value,
          dialCode,
          fullNumber,
          locale,
          isValid,
        });
    };

    return (
      <Box displayProp="flex" className="input-container">
        <StyledFlagBoxWrapper>
          <DropDown
            value={country.code}
            onChange={onChangeCountry}
            options={options}
            theme={props.theme}
            searchPlaceholderText={searchPlaceholderText}
            searchEmptyMessage={searchEmptyMessage}
            size={props.size}
          />
        </StyledFlagBoxWrapper>
        <StyledDialCode size={props.size}>
          <Text className="dial-code-text">{country.dialCode}</Text>
        </StyledDialCode>
        <StyledPhoneInput
          mask={country.mask}
          placeholder={placeholder}
          onChange={onChangeWrapper}
          {...props}
        />
      </Box>
    );
  }
);

PhoneInput.propTypes = {
  locale: PropTypes.string,
  getLocaleCode: PropTypes.func,
  getMask: PropTypes.func,
  getPlaceholder: PropTypes.func,
  onChange: PropTypes.func,
  value: PropTypes.string,
  theme: PropTypes.object,
  searchPlaceholderText: PropTypes.string,
  searchEmptyMessage: PropTypes.string,
  size: PropTypes.string,
};

PhoneInput.defaultProps = {
  type: "text",
  value: "",
  theme: Base,
  searchPlaceholderText: "Type to search country",
  searchEmptyMessage: "Nothing found",
};

PhoneInput.displayName = "PhoneInput";

export default PhoneInput;
