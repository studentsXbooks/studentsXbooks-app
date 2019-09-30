// @flow

import React from "react";
import { Router } from "@reach/router";
import {
  Home,
  Login,
  Register,
  LoginSuccess,
  EmailConfirmed,
  VerifyEmail,
  UserListing
} from "./pages";
import { ApiGet } from "./utils";

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
