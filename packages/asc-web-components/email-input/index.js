import React from "react";
import PropTypes from "prop-types";
import TextInput from "../text-input";
import { EmailSettings, parseAddress } from "../utils/email/";
import equal from "fast-deep-equal/react";

/* eslint-disable no-unused-vars */
/* eslint-disable react/prop-types */
const TextInputWrapper = ({
  onValidateInput,
  isValidEmail,
  emailSettings,
  customValidate,
  ...props
}) => <TextInput {...props}></TextInput>;
/* eslint-enable react/prop-types */
/* eslint-enable no-unused-vars */

class EmailInput extends React.Component {
  constructor(props) {
    super(props);

    const { value, emailSettings } = this.props;
    const validatedSettings = EmailSettings.parse(emailSettings);
    const isValidEmail = this.checkEmail(value, validatedSettings);

    this.state = {
      isValidEmail,
      emailSettings: validatedSettings,
      inputValue: value,
    };
  }

  shouldComponentUpdate(nextProps, nextState) {
    return !equal(this.props, nextProps) || !equal(this.state, nextState);
  }

  componentDidUpdate(prevProps) {
    const { emailSettings, value } = this.props;

    if (!EmailSettings.equals(emailSettings, prevProps.emailSettings)) {
      const validatedSettings = EmailSettings.parse(emailSettings);

      this.setState({ emailSettings: validatedSettings }, () => {
        this.validate(this.state.inputValue);
      });
    }

    if (value !== prevProps.value) {
      this.validate(value);
    }
  }

  validate = (value) => {
    const { onValidateInput } = this.props;
    const isValidEmail = this.checkEmail(value);
    this.setState({
      inputValue: value,
      isValidEmail,
    });
    onValidateInput && onValidateInput(isValidEmail);
  };

  checkEmail = (value, emailSettings = this.state.emailSettings) => {
    const { customValidate } = this.props;
    if (customValidate) {
      return customValidate(value);
    } else {
      const emailObj = parseAddress(value, emailSettings);
      const isValidEmail = emailObj.isValid();
      const parsedErrors = emailObj.parseErrors;
      const errors = parsedErrors
        ? parsedErrors.map((error) => error.errorKey)
        : [];
      return {
        value,
        isValid: isValidEmail,
        errors,
      };
    }
  };

  onChange = (e) => {
    const { onChange, onValidateInput } = this.props;
    onChange ? onChange(e) : this.setState({ inputValue: e.target.value });

    const isValidEmail = this.checkEmail(e.target.value);
    this.setState({ isValidEmail });

    onValidateInput && onValidateInput(isValidEmail);
  };

  render() {
    //console.log('EmailInput render()');
    // eslint-disable-next-line no-unused-vars
    const { onValidateInput, hasError } = this.props;

    const { isValidEmail, inputValue } = this.state;
    const isError =
      typeof hasError === "boolean"
        ? hasError
        : Boolean(inputValue && !isValidEmail.isValid);

    return (
      <TextInputWrapper
        {...this.props}
        hasError={isError}
        value={inputValue}
        onChange={this.onChange}
        type="text"
        onValidateInput={onValidateInput}
      />
    );
  }
}

EmailInput.propTypes = {
  /** Accepts class */
  className: PropTypes.string,
  /** Function for your custom validation input value. Function must return object with following parameters: `value`: string value of input, `isValid`: boolean result of validating, `errors`(optional): array of errors */
  customValidate: PropTypes.func,
  /** { allowDomainPunycode: false, allowLocalPartPunycode: false, allowDomainIp: false, allowStrictLocalPart: true, allowSpaces: false, allowName: false, allowLocalDomainName: false } | Settings for validating email  */
  emailSettings: PropTypes.oneOfType([
    PropTypes.instanceOf(EmailSettings),
    PropTypes.objectOf(PropTypes.bool),
  ]),
  /** Used in your custom validation  */
  hasError: PropTypes.bool,
  /** Supported size of the input fields.  */
  size: PropTypes.oneOf(["base", "middle", "big", "huge", "large"]),
  /** Accepts id  */
  id: PropTypes.string,
  /** Function for your custom handling changes in input */
  onChange: PropTypes.func,
  /** Will be validate our value, return object with following parameters: `isValid`: boolean result of validating, `errors`: array of errors */
  onValidateInput: PropTypes.func,
  /** Accepts css style */
  style: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
  /** Value of the input */
  value: PropTypes.string,
  /** Used as HTML `autocomplete` property */
  autoComplete: PropTypes.string,
  /** Indicates that the field cannot be used (e.g not authorised, or changes not saved)  */
  isDisabled: PropTypes.bool,
  /** Indicates that the field is displaying read-only content */
  isReadOnly: PropTypes.bool,
  /** Used as HTML `name` property  */
  name: PropTypes.string,
  /** Placeholder text for the input  */
  placeholder: PropTypes.string,
  /** Indicates the input field has scale */
  scale: PropTypes.bool,
};

EmailInput.defaultProps = {
  autoComplete: "email",
  className: "",
  hasError: undefined,
  id: "",
  /** kkkk;kk */
  isDisabled: false,
  isReadOnly: false,
  maxLength: 255,
  name: "",
  placeholder: "",
  scale: false,
  size: "base",
  title: "",
  value: "",
  /** dfkasdfklasd;flkajs; */
  withBorder: true,
  /** Some text */
  emailSettings: new EmailSettings(),
};

export default EmailInput;
