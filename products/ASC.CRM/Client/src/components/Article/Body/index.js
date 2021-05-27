import React from "react";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router-dom";
import Loaders from "@appserver/common/components/Loaders";
import TreeFolders from "./TreeFolders";

class ArticleBodyContent extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { isLoaded } = this.props;

    return !isLoaded ? <Loaders.TreeFolders /> : <TreeFolders />;
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
