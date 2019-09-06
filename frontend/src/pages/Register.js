// @flow

import React, { useState } from "react";
import ApiPost from "../components/ApiPost";

type Props = {
  email: string,
  children: any
};

const Register = (props: Props) => {
  const [email, setEmail] = useState("@wvup.edu");
  const [username, setUsername] = useState();
  const [password, setPassword] = useState();
  return (
    <div>
      <h1>Register</h1>

      <form
        onSubmit={e => {
          e.preventDefault();
          e.persist();
          ApiPost("users/new", false, { email, password }).then(res =>
            // After registering, log user in,
            ApiPost("users", true, { email, password }).then(
              // ...Then, refresh to show username.
              res => (window.location.href = "/Home")
            )
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

        <br />
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

export default Register;
