import React from "react";

// TODO: Consider changing this page to self-dismissing modal popup.

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
export default EmailConfirmed;
