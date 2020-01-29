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
  Search,
  About,
  Sell,
  Help,
  FindBook
} from "./pages";
import Layout from "./Layout";

export default () => (
  <Router>
    <Layout path="/">
      {/* $FlowFixMe */}
      <Home path="/" />
      <Login path="login" />
      <Register path="register" />
      {/* $FlowFixMe */}
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
      <About path="/about" />
      <Sell path="/sell" />
      <Help path="/help" />
      {/* $FlowFixMe */}
      <FindBook path="/listing/findbook/:term/:pageId" />
      {/* $FlowFixMe */}
      <ListingDetails path="listing/:id" />
      {/* $FlowFixMe */}
      <ConfirmEmail path="confirm-email" />
      {/* $FlowFixMe */}
      <Search path="/search/:term" />
      {/* $FlowFixMe */}
      <Search path="/search/:term/:pageId" />
    </Layout>
  </Router>
);
