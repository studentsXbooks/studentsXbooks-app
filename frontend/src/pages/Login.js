// @flow

import React, { useState } from "react";
import BaseUrl from "../components/BaseUrl";

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
          fetch(BaseUrl + "user", {
            method: "POST",
            credentials: "include",
            headers: {
              "Content-Type": "application/json"
            },
            body: JSON.stringify({ email, password })
          }).then(res => console.log(res));
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
