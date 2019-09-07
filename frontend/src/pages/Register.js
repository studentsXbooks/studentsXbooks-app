// @flow

import React, { useState } from "react";
import ApiPost from "../components/ApiPost";

const Register = () => {
  const [username, setUsername] = useState();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState();
  return (
    <div>
      <h1>Register</h1>

      <form
        onSubmit={e => {
          e.preventDefault();
          e.persist();
          ApiPost("users/new", false, { username, email, password }).then(
            res =>
              // After registering, show verify email page.
              (window.location.href = "/verify-email?email=" + email)
          );
        }}
      >
        <label htmlFor="Username">Username</label>
        <input
          type="text"
          onChange={e => {
            setUsername(e.target.value);
          }}
          id="Username"
          name="Username"
        />
        {/* TODO: Regex to validate email with edu address. */}
        <br />
        <label htmlFor="Email">Email</label>
        <input
          type="text"
          value={email}
          placeholder="must be '.edu' address"
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

export default Register;
