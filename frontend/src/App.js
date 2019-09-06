// @flow

import React, { useState, useEffect } from "react";
import { Router, Link } from "@reach/router";

import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";

export default () => (
  <Router>
    <Layout path="/">
      <Home default />
      <Login path="login" email={"null"}></Login>
      <Register path="register" />
    </Layout>
  </Router>
);

const Layout = ({ children }) => (
  <div>
    <Link to="/">Home</Link>
    <Link to="/login">Login</Link>
    <Link to="/register">Register</Link>
    <Username />
    {children}
  </div>
);

const Username = () => {
  const [username, setUsername] = useState();
  useEffect(() => {
    fetch("http://sXb-front.com:5000/api/user/name", {
      credentials: "include"
    })
      .then(res => res.json())
      .then(json => {
        const { username } = json;
        setUsername(username);
        console.log(username);
      });
  });

  return <span>{username}</span>;
};
