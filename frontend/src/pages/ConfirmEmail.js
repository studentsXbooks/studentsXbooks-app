import React from "react";
import useApi from "../hooks/useApi";

const ConfirmEmail = () => {
  const id = new URL(window.location).searchParams.get("id") || "";
  const code = new URL(window.location).searchParams.get("code") || "";
  const [loading, data, error] = useApi(
    `users/confirm-email?id=${id}&code=${encodeURIComponent(code)}`
  );

  return (
    <div>
      {loading && <h1>Confirming Account now...</h1>}
      {!loading && data && data.accountConfirm && <h1>Account Confirmed</h1>}
      {!loading && error && <h1>Error confirming account</h1>}
    </div>
  );
};
export default ConfirmEmail;
