import React from "react";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router-dom";
import Loaders from "@appserver/common/components/Loaders";
import TreeFolders from "./TreeFolders";

const ArticleBodyContent = (props) => {
  const { isLoaded } = props;

  return !isLoaded ? <Loaders.TreeFolders /> : <TreeFolders />;
};

export default inject(({ crmStore }) => {
  const { isLoaded } = crmStore;

  return {
    isLoaded,
  };
})(observer(withRouter(ArticleBodyContent)));
