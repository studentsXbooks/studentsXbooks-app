import React, { useState } from "react";
import type { Node } from "react";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import AppBar from "@material-ui/core/AppBar";
import { Link } from "@reach/router";
import { Menu, MenuItem } from "@material-ui/core";
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
  const [anchorEl, setAnchorEl] = useState(null);

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
        style={{ marginLeft: "auto" }}
      >
        {userInfo.username}
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
          <Link to="/listing/new"> New Listing</Link>
        </MenuItem>
      </Menu>
    </>
  );
};
