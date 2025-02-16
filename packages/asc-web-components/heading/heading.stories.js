import React from "react";
import Heading from ".";

export default {
  title: "Components/Heading",
  component: Heading,
  argTypes: {
    color: { control: "color" },
    headerText: { control: "text", description: "Header text" },
  },
  parameters: {
    docs: {
      description: {
        component: "Heading text structured in levels",
      },
    },
  },
};

const Template = ({ headerText, ...args }) => {
  return <Heading {...args}>{headerText}</Heading>;
};

export const Default = Template.bind({});
Default.args = {
  color: "#333333",
  level: 1,
  title: "",
  truncate: false,
  isInline: false,
  size: "large",
  headerText: "Sample text Heading",
};
