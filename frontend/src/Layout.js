import React, { useState } from "react";
import type { Node } from "react";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import AppBar from "@material-ui/core/AppBar";
import { Link } from "@reach/router";
import { Menu, MenuItem } from "@material-ui/core";
import styled from "styled-components";
import { isNil } from "ramda";
import useUserInfo from "./hooks/useUserInfo";
import { apiFetch } from "./utils/fetchLight";

const CustomToolBar = styled(Toolbar)`
  & > h6 {
    margin-right: 1rem;
  }
  & a {
    color: white;
    text-decoration: none;
  }
  background-color: #358;
`;
const Logo = styled.span`
  & > a {
    color: #f95;
    font-weight: bold;
    font-size: 1.25rem;
  }
  margin-right: 1rem;
`;

type Props = {
  children: Node
};

export default ({ children }: Props) => {
  return (
    <div>
      <AppBar position="static">
        <CustomToolBar>
          <Logo variant="h6">
            {/* $FlowFixMe */}
            <Link to="/">StudentsXbooks</Link>
          </Logo>
          <Typography variant="h6">
            {/* $FlowFixMe */}
            <Link to="/sell">Sell</Link>
          </Typography>
          <Typography variant="h6">
            {/* $FlowFixMe */}
            <Link to="/about">About</Link>
          </Typography>
          <Typography variant="h6">
            {/* $FlowFixMe */}
            <Link to="/help">Help</Link>
          </Typography>
          <UserNavOrDefault />
        </CustomToolBar>
      </AppBar>
      <main>{children}</main>
    </div>
  );
};

const UserNavOrDefault = () => {
  const { userInfo } = useUserInfo();
  const [anchorEl, setAnchorEl] = useState(null);

  const handleLogout = () => {
    apiFetch(`users/logout`, "POST", {}).then(() =>
      window.location.replace(`/login`)
    );
  };
  const handleClick = e => setAnchorEl(e.currentTarget);
  const handleClose = () => setAnchorEl(null);

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
      <Typography
        variant="h6"
        onClick={handleClick}
        style={{ marginLeft: "auto", cursor: "pointer" }}
      >
        {userInfo.userName}
      </Typography>
      <Menu
        id="user-menu"
        open={Boolean(anchorEl)}
        anchorEl={anchorEl}
        onClose={handleClose}
      >
        <MenuItem onClick={handleClose}>
          {/* $FlowFixMe */}
          <Link to="/user/listings"> My Listings</Link>
        </MenuItem>
        <MenuItem onClick={handleClose}>
          {/* $FlowFixMe */}
          <Link to="/listing/findbook"> New Listing</Link>
        </MenuItem>
      </Menu>
      <Typography
        variant="h6"
        onClick={handleLogout}
        style={{ marginLeft: "relative", cursor: "pointer" }}
      >
        Logout
      </Typography>
    </>
  );
};
