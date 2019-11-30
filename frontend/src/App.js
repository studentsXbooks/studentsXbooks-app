import React from "react";
import { Router } from "@reach/router";
import {
  Home,
  Login,
  Register,
  VerifyEmail,
  UserListing,
  CreateListing,
  ListingDetails,
  ConfirmEmail,
  Search
} from "./pages";
import Layout from "./Layout";

export default () => (
  <Router>
    <Layout path="/">
      <Home path="/" />
      <Login path="login" />
      <Register path="register" />
      <VerifyEmail path="verify-email" />
      <UserListing path="/user/listings" />
      <UserListing path="/user/listings/:pageId" />
      {/* $FlowFixMe */}
      <CreateListing path="/listing/new" />
      <ListingDetails path="/listing/:id" />
      <ConfirmEmail path="/confirm-email" />
      <Search path="/search/:term" />
      <Search path="/search/:term/:pageId" />
    </Layout>
  </Router>
);
