import React from "react";
import styled, { css } from "styled-components";
import Loaders from "@appserver/common/components/Loaders";
import Headline from "@appserver/common/components/Headline";
import { inject, observer } from "mobx-react";

const SectionHeaderContent = ({ title }) => {
  return (
    <div>
      <Headline className='headline-header' type='content' truncate={true}>
        {title}
      </Headline>
    </div>
  );
};

export default inject(({ treeFoldersStore }) => {
  return {
    title: treeFoldersStore.title,
  };
})(observer(SectionHeaderContent));
