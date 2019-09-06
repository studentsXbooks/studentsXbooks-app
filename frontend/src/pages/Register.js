// @flow

import React, { useState } from "react";

type Props = {
  email: string,
  children: any
};

const Register = (props: Props) => {
  const [email, setEmail] = useState("@wvup.edu");
  const [password, setPassword] = useState();
  <div>
    <h1>Register</h1>

    <form
        onSubmit={e => {
          e.preventDefault();
          e.persist();
          ApiPost("user/create", true, { email, password }).then(
            // Shows username after login.
            // I used a redirect to reload.
            res => (window.location.href = "/Home")
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
  </div>;
};

const Login = (prop: Props) => {
  return (
    <div>
      <h1>Login</h1>

      
    </div>
  );
};

export default Register;
