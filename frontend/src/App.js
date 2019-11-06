import React from "react";
import { Router } from "@reach/router";
import {
  Home,
  Login,
  Register,
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
    <Layout path="/">
      <Home path="/" />
      <Login path="login" />
      <Register path="register" />
      <EmailConfirmed path="email-confirmed" />
      <VerifyEmail path="verify-email" />
      {/* $FlowFixMe */}
      <UserListing path="/user/listings" />
      {/* $FlowFixMe */}
      <UserListing path="/user/listings/:pageId" />
      {/* $FlowFixMe */}
      <CreateListing path="/listing/new" />
      {/* $FlowFixMe */}
      <FindBook path="listing/findbook" />
      {/* $FlowFixMe */}
      <FindBook path="/listing/findbook/:term" />
      {/* $FlowFixMe */}
      <FindBook path="/listing/findbook/:term/:pageId" />
      <ListingDetails path="listing/:id" />
      <ConfirmEmail path="confirm-email" />
      {/* $FlowFixMe */}
      <Search path="/search/:term" />
      {/* $FlowFixMe */}
      <Search path="/search/:term/:pageId" />
    </Layout>
  </Router>
);
