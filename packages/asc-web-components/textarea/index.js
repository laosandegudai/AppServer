import React from "react";
import PropTypes from "prop-types";
import { StyledTextarea, StyledScrollbar } from "./styled-textarea";

// eslint-disable-next-line react/prop-types, no-unused-vars

class Textarea extends React.PureComponent {
  render() {
    // console.log('Textarea render');
    const {
      className,
      id,
      isDisabled,
      isReadOnly,
      hasError,
      heightScale,
      maxLength,
      name,
      onChange,
      placeholder,
      style,
      tabIndex,
      value,
      fontSize,
      heightTextArea,
      color,
    } = this.props;

    return (
      <StyledScrollbar
        className={className}
        style={style}
        stype="preMediumBlack"
        isDisabled={isDisabled}
        hasError={hasError}
        heightScale={heightScale}
        heighttextarea={heightTextArea}
      >
        <StyledTextarea
          id={id}
          placeholder={placeholder}
          onChange={(e) => onChange && onChange(e)}
          maxLength={maxLength}
          name={name}
          tabIndex={tabIndex}
          isDisabled={isDisabled}
          disabled={isDisabled}
          readOnly={isReadOnly}
          value={value}
          fontSize={fontSize}
          color={color}
        />
      </StyledScrollbar>
    );
  }
}

Textarea.propTypes = {
  /** Class name */
  className: PropTypes.string,
  /** Used as HTML `id` property  */
  id: PropTypes.string,
  /** Indicates that the field cannot be used */
  isDisabled: PropTypes.bool,
  /** Indicates that the field is displaying read-only content */
  isReadOnly: PropTypes.bool,
  /** Indicates the input field has an error  */
  hasError: PropTypes.bool,
  /** Indicates the input field has scale */
  heightScale: PropTypes.bool,
  /** Max Length of value */
  maxLength: PropTypes.number,
  /** Used as HTML `name` property  */
  name: PropTypes.string,
  /** Allow you to handle changing events of component */
  onChange: PropTypes.func,
  /** Placeholder for Textarea  */
  placeholder: PropTypes.string,
  /** Accepts css style */
  style: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
  /** Used as HTML `tabindex` property */
  tabIndex: PropTypes.number,
  /** Value for Textarea */
  value: PropTypes.string,
  /** Value for font-size */
  fontSize: PropTypes.number,
  /** Value for height text-area */
  heightTextArea: PropTypes.number,
  /** Specifies the text color */
  color: PropTypes.string,
};

Textarea.defaultProps = {
  className: "",
  isDisabled: false,
  isReadOnly: false,
  hasError: false,
  heightScale: false,
  placeholder: "",
  tabIndex: -1,
  value: "",
  fontSize: 13,
  color: "#333333",
};

export default Textarea;
