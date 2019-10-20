import React, { useEffect, useState } from "react";
import { Grid, Typography } from "@material-ui/core";
import { ApiGet } from "../utils";

type Props = {
  id: string
};

const ListingDetails = ({ id }: Props) => {
  const [listing, setListing] = useState();

  useEffect(() => {
    ApiGet(`listings/${id}`, true).then(setListing);
  }, [id]);

  return (
    <Grid container spacing={3}>
      {listing && (
        <>
          <Grid item xs={6}>
            <img src="https://www.fillmurray.com/300/300" alt="Book" />
          </Grid>
          <Grid item xs={6}>
            <Typography variant="h2">{listing.title}</Typography>
            <Typography variant="h4">{listing.description}</Typography>
            <Typography variant="h4">{listing.authors}</Typography>
            <Typography variant="h4">ISBN: {listing.isbn10}</Typography>
            <Typography variant="h4">{listing.condition}</Typography>
            <Typography variant="h4">{listing.price}</Typography>
          </Grid>
        </>
      )}
    </Grid>
  );
};

export default ListingDetails;
