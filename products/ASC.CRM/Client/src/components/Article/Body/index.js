import React from "react";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router-dom";

class ArticleBodyContent extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const { isLoaded, isVisitor } = this.props;

    return !isVisitor && (!isLoaded ? <div>test0</div> : <div>test1</div>);
  }
}

export default inject(({ auth, crmStore }) => {
  const { isLoaded, isLoading } = crmStore;
  return {
    isLoaded,
    isLoading,
    isVisitor: auth.userStore.user.isVisitor,
  };
})(observer(withRouter(ArticleBodyContent)));
