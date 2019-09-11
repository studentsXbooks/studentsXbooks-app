// @flow

import React, { useState, useEffect } from "react";
import { Router, Link } from "@reach/router";
import {
  Home,
  Login,
  Register,
  LoginSuccess,
  EmailConfirmed,
  VerifyEmail,
  UserListing
} from "./pages";
import { ApiGet, ApiPost } from "./utils";

export default () => (
  // TODO: Clean up Router structure by groups.
  <Router>
    <Layout path="/">
      <Home default />
      <Login path="login" email={"null"}></Login>
      <Register path="register" />
      {/* <Logout path="logout" /> */}
      <LoginSuccess path="login-success" />
      <EmailConfirmed path="email-confirmed" />
      <VerifyEmail path="verify-email" />
      <UserListing path="user/listings" />
    </Layout>
  </Router>
);

const Layout = ({ children }) => (
  <div>
    <Link to="/">Home</Link>
    <Link to="/login">Login</Link>
    <Link to="/register">Register</Link>
    <Link to="/logout">Logout</Link>
    <Username />
    {children}
  </div>
);

// TODO: Only call api when cookie is present.
const Username = () => {
  const [username, setUsername] = useState();
  useEffect(() => {
    ApiGet("users/name", true).then(json => {
      const { username } = json;
      setUsername(username);
    });
  });

  return (
    <>
      {username && (
        <div>
          <span>{username}</span>
          <Link to="/user/listings">My Listings</Link>
        </div>
      )}
    </>
  );
};

const Logout = () => {
  ApiPost("users/logout", true, {})
    .then(res => console.log(res))
    .then(redirec => (window.location.href = "/Home"));
};
