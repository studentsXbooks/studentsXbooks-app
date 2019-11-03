import React from "react";
import type { Node } from "react";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import AppBar from "@material-ui/core/AppBar";
import { Link } from "@reach/router";
// $FlowFixMe
import styled from "styled-components";
import { isNil } from "ramda";
import useApi from "./hooks/useApi";

const CustomToolBar = styled(Toolbar)`
  & > h6 {
    margin-right: 1rem;
  }
  & a {
    color: white;
    text-decoration: none;
  }
`;
type Props = {
  children: Node
};

export default ({ children }: Props) => {
  return (
    <div>
      <AppBar position="static">
        <CustomToolBar>
          <Typography variant="h6">
            {/* $FlowFixMe */}
            <Link to="/">Home</Link>
          </Typography>
          <UserNavOrDefault />
        </CustomToolBar>
      </AppBar>
      <main>{children}</main>
    </div>
  );
};

const UserNavOrDefault = () => {
  const { data: userInfo } = useApi("users/name");

  if (isNil(userInfo))
    return (
      <>
        <Typography variant="h6" style={{ marginLeft: "auto" }}>
          {/* $FlowFixMe */}
          <Link to="/register">Register</Link>
        </Typography>
        <Typography variant="h6">
          {/* $FlowFixMe */}
          <Link to="/login">Login</Link>
        </Typography>
      </>
    );
  return (
    <>
      <Typography variant="h6">{userInfo.username}</Typography>
      <Typography variant="h6">
        {/* $FlowFixMe */}
        <Link to="/user/listings"> My Listings</Link>
      </Typography>
      <Typography variant="h6">
        {/* $FlowFixMe */}
        <Link to="/listing/new"> New Listing</Link>
      </Typography>
    </>
  );
};
