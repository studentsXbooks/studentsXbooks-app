import React, { useEffect, useState } from "react";
import useApi from "../hooks/useApi";
import withSearchBar from "../components/withSearchBar";

const ConfirmEmail = () => {
  const id = new URL(window.location).searchParams.get("id") || "";
  const code = new URL(window.location).searchParams.get("code") || "";
  const [errorMessage, setErrorMessage] = useState("");

  const { loading, data, error } = useApi(
    `users/confirm-email?id=${id}&code=${encodeURIComponent(code)}`
  );

  useEffect(() => {
    if (error)
      error.response.json().then(json => setErrorMessage(json.message));
  }, [error]);

  if (loading === true)
    return (
      <div>
        <h1>Confirming Account now...</h1>
      </div>
    );

  if (!loading && data && data.accountConfirm)
    return (
      <div>
        <h1>Account Confirmed</h1>
      </div>
    );

  if (error) {
    return (
      <div>
        <h1>Error confirming account</h1>
        <h2>{errorMessage}</h2>
      </div>
    );
  }

  return <div>Bad stuff happened</div>;
};
export default withSearchBar(ConfirmEmail);
