import React from "react";
import styled from "styled-components";

const ScrollButton = styled.div`
  cursor: ${(props) => (props.inactive ? "default" : "pointer")};
  opacity: ${(props) => (props.inactive ? "0.2" : "1")};
  z-index: 305;
  position: fixed;
  top: calc(50% - 20px);

  background: none;

  ${(props) => (props.orientation != "left" ? "left: 20px;" : "right: 20px;")}

  width: 40px;
  height: 40px;
  background-color: rgba(11, 11, 11, 0.7);
  border-radius: 50%;

  &:hover {
    background-color: ${(props) =>
      !props.inactive && "rgba(200, 200, 200, 0.2)"};
  }

  &:before {
    content: "";
    top: 12px;
    left: ${(props) => (props.orientation == "left" ? "9px;" : "15px;")};
    position: absolute;
    border: solid #fff;
    border-width: 0 2px 2px 0;
    display: inline-block;
    padding: 7px;
    transform: ${(props) =>
      props.orientation == "left" ? "rotate(-45deg)" : "rotate(135deg)"};
    -webkit-transform: ${(props) =>
      props.orientation == "left" ? "rotate(-45deg)" : "rotate(135deg)"};
  }
`;

const MediaScrollButton = (props) => {
  return <ScrollButton {...props} />;
};

export default MediaScrollButton;
