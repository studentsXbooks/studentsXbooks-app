// @flow

import React, { useState } from "react";
import ApiPost from "../components/ApiPost";

type Props = {
  email: string,
  children: any
};

const Login = (prop: Props) => {
  const [email, setEmail] = useState("@wvup.edu");
  const [password, setPassword] = useState();
  return (
    <div>
      <h1>Login</h1>

      <form
        onSubmit={e => {
          e.preventDefault();
          e.persist();
          ApiPost("users", true, { email, password }).then(
            // Redirect to Login success message window.
            res => (window.location.href = "/login-success")
          );
        }}
      >
        <label htmlFor="Email">Email</label>
        <input
          type="text"
          value={email}
          onChange={e => {
            setEmail(e.target.value);
          }}
          id="Email"
          name="Email"
        />
        <br />
        <label htmlFor="Password">Password</label>
        <input
          type="password"
          onChange={e => {
            setPassword(e.target.value);
          }}
          id="Password"
          name="Password"
        />
        <br />
        <input type="submit" value="Submit" />
      </form>
    </div>
  );
};

export default Login;
