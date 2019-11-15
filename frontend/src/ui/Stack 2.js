// $FlowFixMe
import styled from "styled-components";

const Stack = styled.div`
  & > * + * {
    display: block !important;
    margin-bottom: 1rem !important;
  }
`;

export default Stack;
