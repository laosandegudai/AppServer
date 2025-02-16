import React from "react";
import PropTypes from "prop-types";

import StyledButton from "./styled-selector-add-button";
import IconButton from "../icon-button";

const SelectorAddButton = (props) => {
  const { isDisabled, title, className, id, style } = props;

  const onClick = (e) => {
    !isDisabled && props.onClick && props.onClick(e);
  };

  return (
    <StyledButton
      isDisabled={isDisabled}
      title={title}
      onClick={onClick}
      className={className}
      id={id}
      style={style}
    >
      <IconButton
        color="#979797"
        size={14}
        iconName="/static/images/actions.header.touch.react.svg"
        isFill={true}
        isDisabled={isDisabled}
        isClickable={!isDisabled}
      />
    </StyledButton>
  );
};

SelectorAddButton.propTypes = {
  /** Title text */
  title: PropTypes.string,
  /** What the button will trigger when clicked */
  onClick: PropTypes.func,
  /** Tells when the button should present a disabled state */
  isDisabled: PropTypes.bool,
  /** Attribute className  */
  className: PropTypes.string,
  /** Accepts id */
  id: PropTypes.string,
  /** Accepts css style */
  style: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
};

SelectorAddButton.defaultProps = {
  isDisabled: false,
};

export default SelectorAddButton;
