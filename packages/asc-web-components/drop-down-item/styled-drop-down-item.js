import styled, { css } from "styled-components";
import Base from "../themes/base";
import { tablet } from "../utils/device";

const itemTruncate = css`
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
`;

const fontStyle = css`
  font-family: ${(props) => props.theme.fontFamily};
  font-style: normal;
`;

const disabledAndHeaderStyle = css`
  color: ${(props) => props.theme.dropDownItem.disableColor};

  &:hover {
    cursor: default;
    background-color: ${(props) =>
      props.theme.dropDownItem.hoverDisabledBackgroundColor};
  }
`;

const StyledDropdownItem = styled.div`
  display: ${(props) => (props.textOverflow ? "block" : "flex")};
  width: ${(props) => props.theme.dropDownItem.width};
  max-width: ${(props) => props.theme.dropDownItem.maxWidth};
  border: ${(props) => props.theme.dropDownItem.border};
  cursor: pointer;
  margin: ${(props) => props.theme.dropDownItem.margin};
  padding: ${(props) => props.theme.dropDownItem.padding};
  line-height: ${(props) => props.theme.dropDownItem.lineHeight};
  box-sizing: border-box;
  text-align: left;
  background: none;
  text-decoration: none;
  user-select: none;
  outline: 0 !important;
  -webkit-tap-highlight-color: rgba(0, 0, 0, 0);

  .drop-down-item_icon {
    path {
      fill: ${(props) =>
        props.disabled
          ? props.theme.dropDownItem.icon.disableColor
          : props.theme.dropDownItem.icon.color};
    }
  }

  ${fontStyle}

  font-weight: ${(props) => props.theme.dropDownItem.fontWeight};
  font-size: ${(props) => props.theme.dropDownItem.fontSize};
  color: ${(props) => props.theme.dropDownItem.color};
  text-transform: none;

  ${itemTruncate}

  &:hover {
    background-color: ${(props) =>
      props.noHover
        ? props.theme.dropDownItem.backgroundColor
        : props.theme.dropDownItem.hoverBackgroundColor};
    text-align: left;
  }

  ${(props) =>
    props.isSeparator &&
    css`
      padding: ${props.theme.dropDownItem.separator.padding};
      border-bottom: ${props.theme.dropDownItem.separator.borderBottom};
      cursor: default;
      margin: ${props.theme.dropDownItem.separator.margin};
      line-height: ${props.theme.dropDownItem.separator.lineHeight};
      height: ${props.theme.dropDownItem.separator.height};
      width: ${props.theme.dropDownItem.separator.width};

      &:hover {
        cursor: default;
      }
    `}

  ${(props) =>
    props.isHeader &&
    css`
      ${disabledAndHeaderStyle}

      text-transform: uppercase;
      break-before: column;
    `}

    @media ${tablet} {
    line-height: ${(props) => props.theme.dropDownItem.tabletLineHeight};
  }

  ${(props) => props.disabled && disabledAndHeaderStyle}
`;
StyledDropdownItem.defaultProps = { theme: Base };

const IconWrapper = styled.div`
  display: flex;
  align-items: center;
  width: ${(props) => props.theme.dropDownItem.icon.width};
  margin-right: ${(props) => props.theme.dropDownItem.icon.marginRight};
  line-height: ${(props) => props.theme.dropDownItem.icon.lineHeight};
  svg {
    &:not(:root) {
      width: 100%;
      height: 100%;
    }
  }
`;
IconWrapper.defaultProps = { theme: Base };

export { StyledDropdownItem, IconWrapper };
