import styled from "styled-components";
import React from "react";

import commonInputStyle from "../text-input/common-input-styles";
import Base from "../themes/base";

const StyledIconBlock = styled.div`
  display: ${(props) => props.theme.inputBlock.display};
  align-items: ${(props) => props.theme.inputBlock.alignItems};
  cursor: ${(props) =>
    props.isDisabled || !props.isClickable ? "default" : "pointer"};

  height: ${(props) => props.theme.inputBlock.height};
  padding-right: ${(props) => props.theme.inputBlock.paddingRight};
  padding-left: ${(props) => props.theme.inputBlock.paddingLeft};
  -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
`;
StyledIconBlock.defaultProps = { theme: Base };

const StyledChildrenBlock = styled.div`
  display: ${(props) => props.theme.inputBlock.display};
  align-items: ${(props) => props.theme.inputBlock.alignItems};
  padding: ${(props) => props.theme.inputBlock.padding};
`;
StyledChildrenBlock.defaultProps = { theme: Base };

/* eslint-disable react/prop-types, no-unused-vars */
const CustomInputGroup = ({
  isIconFill,
  hasError,
  hasWarning,
  isDisabled,
  scale,
  ...props
}) => <div {...props}></div>;
/* eslint-enable react/prop-types, no-unused-vars */
const StyledInputGroup = styled(CustomInputGroup)`
  display: ${(props) => props.theme.inputBlock.display};

  .prepend {
    display: ${(props) => props.theme.inputBlock.display};
    align-items: ${(props) => props.theme.inputBlock.alignItems};
  }

  .append {
    align-items: ${(props) => props.theme.inputBlock.alignItems};
    margin: ${(props) => props.theme.inputBlock.margin};
  }

  ${commonInputStyle} :focus-within {
    border-color: ${(props) => props.theme.inputBlock.borderColor};
  }
`;
StyledInputGroup.defaultProps = { theme: Base };

export { StyledInputGroup, StyledChildrenBlock, StyledIconBlock };
