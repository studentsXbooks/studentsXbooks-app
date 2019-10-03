import React from "react";
import { ApiGet } from "../utils";

const ConfirmEmail = () => {
  const id = new URL(window.location).searchParams.get("id");
  const code = new URL(window.location).searchParams.get("code");
  var successCode = "";
  ApiGet("users/confirm-email?id=" + id + "&code=" + code, true).then(res => {
    successCode = res.status;
    console.log(successCode);
  });
  setTimeout(() => {
    window.location.href = "/login";
  }, 5000);
  if (successCode === 200) {
  }
  return <h1>Email Confirmed!</h1>;
};
export default ConfirmEmail;
