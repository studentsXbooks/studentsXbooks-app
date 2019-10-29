import React, { useEffect } from "react";

// TODO: Consider changing this page to self-dismissing modal popup.

const LoginSuccess = () => {
  useEffect(() => {
    setTimeout(() => {
      window.location.href = "/home";
    }, 1500);
  });

  return <h1 style={{ color: "blue" }}>Welcome!</h1>;
};
export default LoginSuccess;
