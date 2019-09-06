import React from "react";

const LoginSuccess = () => {
  setTimeout(() => {
    window.location.href = "/home";
  }, 750);

  return <h1 style={{ color: "blue" }}>Welcome!</h1>;
};
export default LoginSuccess;
