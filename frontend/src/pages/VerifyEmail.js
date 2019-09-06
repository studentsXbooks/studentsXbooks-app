import React, { useState } from "react";

const VerifyEmail = () => {
  const email = new URL(window.location).searchParams.get("email");
  return (
    <div>
      <h3 style={{ color: "blue", width: "200px" }}>
        <center>
          A confirmation email has been sent to {email}, please check your
          inbox.
        </center>
      </h3>
    </div>
  );
};
export default VerifyEmail;
