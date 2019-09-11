import React, { useEffect, useState } from "react";
import { ApiGet } from "../utils";

const UserListing = () => {
  const [listings, setListings] = useState();

  useEffect(() => {
    ApiGet("listings/user", true).then(setListings);
  }, []);

  return (
    <div>{listings && listings.map(({ title }) => <div>{title}</div>)}</div>
  );
};

export default UserListing;
