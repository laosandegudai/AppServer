import React, { useState } from "react";

import InputBlock from ".";
import Button from "../button";
import IconButton from "../icon-button";

export default {
  title: "Components/InputBlock",
  component: InputBlock,
  argTypes: {
    iconColor: { control: "color" },
    hoverColor: { control: "color" },
    onChange: { action: "onChange" },
    onBlur: { action: "onBlur" },
    onFocus: { action: "onFocus" },
    onIconClick: { action: "onIconClick" },
    optionsMultiSelect: {
      control: {
        type: "multi-select",
        options: ["button", "icon"],
      },
    },
  },
};

const Template = ({ optionsMultiSelect, onChange, ...args }) => {
  const [value, setValue] = useState("");

  const children = [];

  if (optionsMultiSelect) {
    optionsMultiSelect.forEach(function (item, i) {
      switch (item) {
        case "button":
          children.push(<Button label="OK" key={i} />);
          break;
        case "icon":
          children.push(
            <IconButton
              size={16}
              color=""
              key={i}
              color="#d0d5da"
              iconName="static/images/settings.react.svg"
            />
          );
          break;
        default:
          break;
      }
    });
  }

  return (
    <InputBlock
      {...args}
      value={value}
      onChange={(e) => {
        setValue(e.target.value), onChange(e.target.value);
      }}
    >
      {children}
    </InputBlock>
  );
};

export const Default = Template.bind({});
Default.args = {
  id: "",
  name: "",
  placeholder: "This is placeholder",
  maxLength: 255,
  size: "base",
  isAutoFocussed: false,
  isReadOnly: false,
  hasError: false,
  hasWarning: false,
  scale: false,
  autoComplete: "off",
  tabIndex: 1,
  iconSize: 0,
  mask: null,
  isDisabled: false,
  iconName: "static/images/search.react.svg",
  iconColor: "#D0D5DA",
  isIconFill: false,
};
