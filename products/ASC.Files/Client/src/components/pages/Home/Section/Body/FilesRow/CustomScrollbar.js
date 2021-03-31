import React, { Component } from "react";
import { Scrollbars } from "react-custom-scrollbars";
import { observer, inject } from "mobx-react";

class CustomScrollbar extends Component {
  constructor(props, ...rest) {
    super(props, ...rest);
    this.state = { top: 0 };
    this.handleUpdate = this.handleUpdate.bind(this);
  }

  handleUpdate(values) {
    const customScrollElm = document.querySelector(
      "#customScrollBar > .scroll-body"
    );
    const { top } = values;
    const { setHeaderVisible } = this.props;
    if (top > this.state.top) {
      console.log("Scrolling down");
      setHeaderVisible(true);
      customScrollElm.scrollTo(0, top);
    }

    if (top < this.state.top) {
      console.log("Scrolling up");
      setHeaderVisible(false);
      customScrollElm.scrollTo(0, top);
    }

    this.setState({ top });
  }

  render() {
    const { setHeaderVisible, ...rest } = this.props;
    return (
      <Scrollbars
        onUpdate={this.handleUpdate}
        style={{ overflow: "hidden" }}
        {...rest}
      />
    );
  }
}

export default inject(({ auth }) => {
  return {
    setHeaderVisible: auth.settingsStore.setHeaderVisible,
  };
})(observer(CustomScrollbar));
