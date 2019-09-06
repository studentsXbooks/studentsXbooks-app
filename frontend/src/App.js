// @flow

import React, { useState, useEffect } from "react";
import { Router, Link } from "@reach/router";

import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";

import ApiGet from "./components/ApiGet";

export default () => (
  <Router>
    <Layout path="/">
      <Home default />
      <Login path="login" email={"null"}></Login>
      <Register path="register" />
      <Logout path="logout" />
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
  
}
