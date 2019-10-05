import React, { useEffect, useState } from "react";
import { ApiGet } from "../utils";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography
} from "@material-ui/core";

const ListingDetails = ({ id }) => {
  const [listing, setListing] = useState();

  useEffect(() => {
    ApiGet(`listings/${id}`, true).then(setListing);
  }, []);

  return (
    <Grid container spacing={3}>
      {listing && (
        <>
          <Grid item xs={6}>
            <img src="https://www.fillmurray.com/300/300" />
          </Grid>
          <Grid item xs={6}>
            <Typography variant="h2">{listing.book.title}</Typography>
            <Typography variant="h4">{listing.book.description}</Typography>
          </Grid>
        </>
      )}
    </Grid>
  );
};

export default ListingDetails;
