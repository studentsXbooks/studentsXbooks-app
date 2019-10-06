import React, { useEffect, useState } from "react";
import { ApiGet } from "../utils";

const ConfirmEmail = () => {
  const [accountConfirmed, setAccountConfirmed] = useState();
  const id = new URL(window.location).searchParams.get("id");
  const code = new URL(window.location).searchParams.get("code");
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

};
export default ConfirmEmail;
