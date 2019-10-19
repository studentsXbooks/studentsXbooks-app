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
  UserListing,
  CreateListing,
  ListingDetails,
  ConfirmEmail,
  Search,
  SearchFilter
} from "./pages";
import Layout from "./Layout";

export default () => (
  <Router>
    <Layout path="/">
      <Home default />
      <Login path="login" />
      <Register path="register" />
      <LoginSuccess path="login-success" />
      <EmailConfirmed path="email-confirmed" />
      <VerifyEmail path="verify-email" />
      <UserListing path="user/listings/" />
      <UserListing path="user/listings/:pageId" />
      <CreateListing path="listing/new" />
      <ListingDetails path="listing/:id" />
      <ConfirmEmail path="confirm-email" />
      <Search path="search/:term/:pageId" />
      <SearchFilter path="search-filter/:term/:pageId" />
    </Layout>
  </Router>
);
