import React from "react";
import withSearchBar from "../components/withSearchBar";

const EmailConfirmed = () => {
  setTimeout(() => {
    window.location.href = "/login";
  }, 1500);

  return (
    <div>
      <h1 style={{ color: "blue" }}>Account successfully confirmed!</h1>
      <h4>Please login.</h4>
    </div>
  );
};

export default withSearchBar(EmailConfirmed);
