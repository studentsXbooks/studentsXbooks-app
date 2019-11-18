import React from "react";
import useApi from "../hooks/useApi";
import withSearchBar from "../components/withSearchBar";

type ReturnedData = {
  accountConfirm: boolean
};

const ConfirmEmail = () => {
  const id = new URL(window.location).searchParams.get("id") || "";
  const code = new URL(window.location).searchParams.get("code") || "";
  // prettier-ignore
  const { loading, data, error } = useApi<ReturnedData>(
    `users/confirm-email?id=${id}&code=${encodeURIComponent(code)}`
  );

  if (loading === true)
    return (
      <div>
        <h1>Confirming Account now...</h1>
      </div>
    );

  if (data && data.accountConfirm)
    return (
      <div>
        {!loading && data && data.accountConfirm && <h1>Account Confirmed</h1>}
      </div>
    );

  if (error)
    return (
      <div>
        <h1>Error confirming account</h1>
      </div>
    );

  return <div>Bad stuff happened</div>;
};
export default withSearchBar(ConfirmEmail);
