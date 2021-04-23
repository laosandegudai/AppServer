import styled from "styled-components";
import { tablet } from "@appserver/components/utils/device";

const StyledHeader = styled.div`
  height: 56px;
  align-items: center;
  display: grid;
  padding-left: 16px;
  padding-right: 16px;
  grid-template-rows: 1fr;
  grid-column-gap: 16px;
  grid-template-columns: 256px repeat(auto-fill, 24px);

  @media ${tablet} {
    grid-template-columns: 24px 168px 1fr 36px;
  }
`;

const StyledSpacer = styled.div``;

export { StyledHeader, StyledSpacer };
