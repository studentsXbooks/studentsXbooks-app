// @flow

import React from "react";
import { Router, Link } from "@reach/router";

export default () => (
  <Router>
    <Home default />
    <Login path="/Login" />
  </Router>
);

const Home = () => (
  <div>
    <h1>Home</h1>
    <Link to="/Login">Login</Link>
  </div>
);

const Login = () => <div>Login</div>;
