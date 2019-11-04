import React from "react";
import { Router } from "@reach/router";
import {
  Home,
  Login,
  Register,
  LoginSuccess,
  EmailConfirmed,
  VerifyEmail,
  UserListing,
  CreateListing,
  ListingDetails,
  ConfirmEmail,
  Search,
  FindBook
} from "./pages";
import Layout from "./Layout";

export default () => (
  <Router>
    {/* $FlowFixMe */}
    <Layout path="/">
      <Home default />
      <Login path="login" />
      <Register path="register" />
      <LoginSuccess path="login-success" />
      <EmailConfirmed path="email-confirmed" />
      <VerifyEmail path="verify-email" />
      {/* $FlowFixMe */}
      <UserListing path="user/listings/" />
      {/* $FlowFixMe */}
      <UserListing path="user/listings/:pageId" />
      {/* $FlowFixMe */}
      <CreateListing path="listing/new" />
      {/* $FlowFixMe */}
      <FindBook path="listing/findBook" />
      {/* $FlowFixMe */}
      <ListingDetails path="listing/:id" />
      <ConfirmEmail path="confirm-email" />
      {/* $FlowFixMe */}
      <Search path="search/:term" />
      {/* $FlowFixMe */}
      <Search path="search/:term/:pageId" />
    </Layout>
  </Router>
);
