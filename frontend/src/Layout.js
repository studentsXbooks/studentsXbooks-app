import React, { useState, useEffect } from "react";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import AppBar from "@material-ui/core/AppBar";
import IconButton from "@material-ui/core/IconButton";
import MenuIcon from "@material-ui/icons/Menu";
import { Link } from "@reach/router";
import styled from "@emotion/styled/macro";
import { isNil } from "ramda";
import { ApiGet } from "./utils";

const CustomToolBar = styled(Toolbar)`
  & > h6 {
    margin-right: 1rem;
  }
  & a {
    color: white;
    text-decoration: none;
  }
`;

export default ({ children }) => (
  <div>
    <AppBar position="static">
      <CustomToolBar>
        <IconButton edge="start" color="inherit" aria-label="menu">
          <MenuIcon />
        </IconButton>
        <Typography variant="h6">
          <Link to="/">Home</Link>
        </Typography>
        <UserInfoOrLoginRegistration />
      </CustomToolBar>
    </AppBar>
    <main>{children}</main>
  </div>
);

const UserInfoOrLoginRegistration = () => {
  const [username, setUsername] = useState();
  useEffect(() => {
    ApiGet("users/name", true)
      .then(json => {
        const { username: returnedUserName } = json;
        setUsername(returnedUserName);
      })
      .catch(console.log);
  });

  if (isNil(username))
    return (
      <>
        <Typography variant="h6" style={{ marginLeft: "auto" }}>
          <Link to="/register">Register</Link>
        </Typography>
        <Typography variant="h6">
          <Link to="/login">Login</Link>
        </Typography>
      </>
    );
  return (
    <div>
      <span>{username}</span>
      <Link to="/user/listings">My Listings</Link>
    </div>
  );
};
