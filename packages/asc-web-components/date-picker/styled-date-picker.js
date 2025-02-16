import styled from "styled-components";
import Base from "../themes/base";

const DateInputStyle = styled.div`
  width: ${(props) => props.theme.datePicker.width};
`;
DateInputStyle.defaultProps = { theme: Base };

const DropDownStyle = styled.div`
  .drop-down {
    padding: ${(props) => props.theme.datePicker.dropDownPadding};
  }
  position: relative;
`;
DropDownStyle.defaultProps = { theme: Base };

const Content = styled.div`
  box-sizing: border-box;
  position: relative;
  width: 100%;
  background-color: #fff;
  padding: ${(props) => props.theme.datePicker.contentPadding};

  .header {
    max-width: ${(props) => props.theme.datePicker.contentMaxWidth};
    margin: 0;
    line-height: ${(props) => props.theme.datePicker.contentLineHeight};
    font-weight: ${(props) =>
      props.theme.datePicker.contentFontWeight} !important;
  }
`;
Content.defaultProps = { theme: Base };

const Header = styled.div`
  display: flex;
  align-items: center;
  border-bottom: ${(props) => props.theme.datePicker.borderBottom};
`;
Header.defaultProps = { theme: Base };

const Body = styled.div`
  position: relative;
  padding: ${(props) => props.theme.datePicker.bodyPadding};
`;
Body.defaultProps = { theme: Base };

export { Body, Header, Content, DropDownStyle, DateInputStyle };
