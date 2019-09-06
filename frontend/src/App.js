// @flow

import React, { useState, useEffect } from "react";
import { Router, Link } from "@reach/router";

import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import LoginSuccess from "./pages/LoginSuccess";
import EmailConfirmed from "./pages/EmailConfirmed";
import VerifyEmail from "./pages/VerifyEmail";

import ApiGet from "./components/ApiGet";
import ApiPost from "./components/ApiPost";

export default () => (
  // TODO: Clean up Router structure by groups.
  <Router>
    <Layout path="/">
      <Home default />
      <Login path="login" email={"null"}></Login>
      <Register path="register" />
      <Logout path="logout" />
      <LoginSuccess path="login-success" />
      <EmailConfirmed path="email-confirmed" />
      <VerifyEmail path="verify-email" />
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

  return <span>{username}</span>;
};

const Logout = () => {
  ApiPost("users/logout", true, {})
    .then(res => console.log(res))
    .then(redirec => (window.location.href = "/Home"));
};
