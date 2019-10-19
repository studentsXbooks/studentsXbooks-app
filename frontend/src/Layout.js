import React, { useState, useEffect } from "react";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import AppBar from "@material-ui/core/AppBar";
import IconButton from "@material-ui/core/IconButton";
import MenuIcon from "@material-ui/icons/Menu";
import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";
import { Link } from "@reach/router";
import styled from "@emotion/styled/macro";
import { isNil } from "ramda";
import { ApiGet } from "./utils";

import { fade, makeStyles } from "@material-ui/core/styles";

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

// Search Input in underneath nav
// Whenever something is typed in and then submitted that should take you to a search result page
// Search Result Page component should take in a term
// Send that term to the api then display results
// Throw in Paging component in order to have paging.

export default ({ children, navigate }) => {
  const classes = useStyles();
  const [search, setSearch] = useState("");
  return (
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
      <div className={classes.search}>
        <div className={classes.searchIcon}>
          <SearchIcon />
        </div>
        <form
          method="POST"
          onSubmit={e => {
            e.preventDefault();
            navigate(`/search-filter/${search}/1`);
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
      <Link to="/user/listings"> My Listings</Link>
      <Link to="/listing/new"> New Listing</Link>
    </div>
  );
};
