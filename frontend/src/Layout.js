import React, { useState } from "react";
import type { Node } from "react";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import AppBar from "@material-ui/core/AppBar";
import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";
import { Link } from "@reach/router";
// $FlowFixMe
import styled from "styled-components";
import { isNil } from "ramda";
import { fade, makeStyles } from "@material-ui/core/styles";
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
const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1
  },
  menuButton: {
    marginRight: theme.spacing(2)
  },
  title: {
    flexGrow: 1,
    display: "none",
    [theme.breakpoints.up("sm")]: {
      display: "block"
    }
  },
  search: {
    position: "relative",
    borderRadius: theme.shape.borderRadius,
    backgroundColor: fade(theme.palette.common.white, 0.15),
    "&:hover": {
      backgroundColor: fade(theme.palette.common.white, 0.25)
    },
    marginLeft: 0,
    width: "100%",
    [theme.breakpoints.up("sm")]: {
      marginLeft: theme.spacing(1),
      width: "auto"
    }
  },
  searchIcon: {
    width: theme.spacing(7),
    height: "100%",
    position: "absolute",
    pointerEvents: "none",
    display: "flex",
    alignItems: "center",
    justifyContent: "center"
  },
  inputRoot: {
    color: "inherit"
  },
  inputInput: {
    padding: theme.spacing(1, 1, 1, 7),
    transition: theme.transitions.create("width"),
    width: "100%",
    [theme.breakpoints.up("sm")]: {
      width: 120,
      "&:focus": {
        width: 200
      }
    }
  }
}));

type Props = {
  children: Node,
  navigate: string => any
};

export default ({ children, navigate }: Props) => {
  const classes = useStyles();
  const [search, setSearch] = useState("");
  return (
    <div>
      <AppBar position="static">
        <CustomToolBar>
          <Typography variant="h6">
            {/* $FlowFixMe */}
            <Link to="/">Home</Link>
          </Typography>
          <UserInfoOrLoginRegistration />
        </CustomToolBar>
      </AppBar>
      <div className={classes.search}>
        <div className={classes.searchIcon}>
          <SearchIcon />
        </div>
        <form
          method="POST"
          onSubmit={e => {
            e.preventDefault();
            navigate(`/search/${search}`);
          }}
        >
          <InputBase
            placeholder="Title, Author, ISBN..."
            classes={{
              root: classes.inputRoot,
              input: classes.inputInput
            }}
            onChange={e => setSearch(e.target.value)}
            inputProps={{ "aria-label": "search" }}
          />
        </form>
      </div>
      <main>{children}</main>
    </div>
  );
};

const UserInfoOrLoginRegistration = () => {
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
    <div>
      <span>{userInfo.username}</span>
      {/* $FlowFixMe */}
      <Link to="/user/listings"> My Listings</Link>
      {/* $FlowFixMe */}
      <Link to="/listing/new"> New Listing</Link>
    </div>
  );
};
