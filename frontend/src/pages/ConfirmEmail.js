<<<<<<< HEAD
import React, { useEffect } from "react";
=======
import React, { useEffect, useState } from "react";
>>>>>>> 293f73cea067b88fe516e01c2d002b793ec192f2
import { ApiGet } from "../utils";

const ConfirmEmail = () => {
  const [accountConfirmed, setAccountConfirmed] = useState();
  const id = new URL(window.location).searchParams.get("id");
  const code = new URL(window.location).searchParams.get("code");
<<<<<<< HEAD
  var successCode = "";
  useEffect(() => {
    ApiGet("users/confirm-email?id=" + id + "&code=" + code, true).then(res => {
      successCode = res.status;
      console.log(successCode);
      setTimeout(() => {
        window.location.href = "/login";
      }, 2000);
    });
  });

  if (successCode === 200) {
    return <h1>Email Confirmed!</h1>;
  } else {
    return <h1>Email not confirmed!</h1>;
  }
=======

  useEffect(() => {
    ApiGet(
      `users/confirm-email?id=${id}&code=${encodeURIComponent(code)}`
    ).then(() => {
      setAccountConfirmed(true);
    });
  }, []);

  return (
    <div>
      {accountConfirmed && <h1>Account Confirmed</h1>}
      {!accountConfirmed && <h1>Error confirming account</h1>}
    </div>
  );
>>>>>>> 293f73cea067b88fe516e01c2d002b793ec192f2
};
export default ConfirmEmail;
