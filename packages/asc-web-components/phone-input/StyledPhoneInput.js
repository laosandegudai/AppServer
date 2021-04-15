import styled from "styled-components";
import TextInput from "../text-input";
import Box from "../box";
import { Base } from "../themes";

// base
// large

const StyledFlagBoxWrapper = styled(Box)`
  border: 1px solid #d1d1d1;
  border-radius: 3px 0px 0px 3px;
`;

const StyledFlagBox = styled(Box)`
  cursor: pointer;
  font-size: ${(props) =>
    (props.size === "base" && "20px") || (props.size === "large" && "27px")};
  padding: ${(props) =>
    (props.size === "base" && "0 4px 0 8px") ||
    (props.size === "large" && " 0 4px 0 16px")};
`;

const StyledDialCode = styled(Box)`
  border-top: 1px solid #d1d1d1;
  border-bottom: 1px solid #d1d1d1;
  padding: ${(props) =>
    (props.size === "base" && "7px 0 7px 8px") ||
    (props.size === "large" && "11px 0 11px 16px")};
  .dial-code-text {
    font-size: ${(props) =>
      (props.size === "base" && "13px") || (props.size === "large" && "16px")};
    line-height: ${(props) =>
      (props.size === "base" && "16px") || (props.size === "large" && "21px")};
  }
`;

const StyledPhoneInput = styled(TextInput)`
  border-left: 0;
  border-radius: 0px 3px 3px 0px;
  width: ${(props) =>
    (props.size === "base" && "142px") || (props.size === "large" && "180px")};
  padding: ${(props) =>
    (props.size === "base" && "7px 8px 7px 4px") ||
    (props.size === "large" && "11px 16px 11px 6px")};
  font-size: ${(props) =>
    (props.size === "base" && "13px") || (props.size === "large" && "16px")};
  line-height: ${(props) =>
    (props.size === "base" && "16px") || (props.size === "large" && "21px")};

  :hover {
    border-color: #d1d1d1;
  }
  :focus {
    border-color: #d1d1d1;
  }
`;

const StyledTriangle = styled.div`
  border-left: 3px solid transparent;
  border-right: 3px solid transparent;
  border-top: 3px solid #a3a9ae;
  cursor: pointer;
  margin: ${(props) =>
    (props.size === "base" && "14px 8px 0 0") ||
    (props.size === "large" && "21px 8px 0 0")};
`;

const StyledDropDownWrapper = styled(Box)`
  position: relative;
`;

const StyledDropDown = styled.div`
  position: absolute;
  top: ${(props) =>
    (props.size === "base" && "30px") || (props.size === "large" && "43px")};
  left: -1px;
  width: ${(props) =>
    (props.size === "base" && "229px") || (props.size === "large" && "300px")};
  height: 260px;
  z-index: 1999;
  background-color: #fff;
  border-radius: 4px;
  border: 1px solid #d1d1d1;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
`;

const StyledSearchPanel = styled.div`
  background-color: #fff;
  padding: 8px 8px 0 8px;
  .phone-input-searcher {
    background-color: #fff;
    ::placeholder {
      color: ${(props) => props.theme.phoneInput.placeholderColor};
    }
  }
`;

const StyledCountryItem = styled(Box)`
  cursor: pointer;
  &:hover {
    background-color: ${(props) => props.theme.phoneInput.itemHoverColor};
  }
  background-color: ${(props) => props.theme.phoneInput.itemBackgroundColor};
`;

StyledDropDown.defaultProps = { theme: Base };
StyledCountryItem.defaultProps = { theme: Base };
StyledSearchPanel.defaultProps = { theme: Base };

export {
  StyledPhoneInput,
  StyledTriangle,
  StyledDropDown,
  StyledCountryItem,
  StyledFlagBox,
  StyledSearchPanel,
  StyledDropDownWrapper,
  StyledDialCode,
  StyledFlagBoxWrapper,
};
