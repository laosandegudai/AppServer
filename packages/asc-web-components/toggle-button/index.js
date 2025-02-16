import React, { Component } from "react";
import PropTypes from "prop-types";
import { ToggleButtonContainer, HiddenInput } from "./styled-toggle-button";
import Text from "../text";
import globalColors from "../utils/globalColors";
import { motion } from "framer-motion";
import Base from "../themes/base";

const ToggleIcon = ({ isChecked, isLoading }) => {
  return (
    <motion.svg
      animate={[
        isChecked ? "checked" : "notChecked",
        isLoading ? "isLoading" : "",
      ]}
      width="28"
      height="16"
      viewBox="0 0 28 16"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <motion.rect
        width="28"
        height="16"
        rx="8"
        variants={{
          checked: { fill: Base.toggleButton.fillColor },
          notChecked: { fill: Base.toggleButton.fillColorOff },
        }}
      />
      <motion.circle
        fill-rule="evenodd"
        clip-rule="evenodd"
        cy="8"
        fill="white"
        variants={{
          isLoading: {
            r: [5, 6, 6],
            transition: {
              r: {
                yoyo: Infinity,
                duration: 0.6,
                ease: "easeOut",
              },
            },
          },
          checked: { cx: 20, r: 6 },
          notChecked: { cx: 8, r: 6 },
        }}
      />
    </motion.svg>
  );
};

class ToggleButton extends Component {
  constructor(props) {
    super(props);
    this.state = {
      checked: props.isChecked,
    };
  }

  componentDidUpdate(prevProps) {
    if (this.props.isChecked !== prevProps.isChecked) {
      this.setState({ checked: this.props.isChecked });
    }
  }

  render() {
    const {
      isDisabled,
      label,
      onChange,
      id,
      className,
      style,
      isLoading,
    } = this.props;
    const { gray } = globalColors;
    const colorProps = isDisabled ? { color: gray } : {};

    //console.log("ToggleButton render");

    return (
      <ToggleButtonContainer
        id={id}
        className={className}
        style={style}
        isDisabled={isDisabled}
      >
        <HiddenInput
          type="checkbox"
          checked={this.state.checked}
          disabled={isDisabled}
          onChange={onChange}
        />
        <ToggleIcon isChecked={this.state.checked} isLoading={isLoading} />
        {label && (
          <Text className="toggle-button-text" as="span" {...colorProps}>
            {label}
          </Text>
        )}
      </ToggleButtonContainer>
    );
  }
}

ToggleButton.propTypes = {
  /** The checked property sets the checked state of a ToggleButton. */
  isChecked: PropTypes.bool.isRequired,
  /** Disables the ToggleButton */
  isDisabled: PropTypes.bool,
  /** Will be triggered whenever an ToggleButton is clicked */
  onChange: PropTypes.func.isRequired,
  /** Label of the input  */
  label: PropTypes.string,
  /** Set component id */
  id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  /** Class name */
  className: PropTypes.string,
  /** Accepts css style */
  style: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
};

ToggleIcon.propTypes = {
  isChecked: PropTypes.bool,
};

export default ToggleButton;
