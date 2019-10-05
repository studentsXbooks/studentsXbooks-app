import React, { useEffect, useState } from "react";
import { Link } from "@reach/router";
import { ApiGet } from "../utils";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography
} from "@material-ui/core";

// Need paging, and need listing details page
const UserListing = () => {
  const [page, setPage] = useState();

  useEffect(() => {
    ApiGet("listings/user", true).then(setPage);
  }, []);

  return (
    <Grid container spacing={3}>
      {page &&
        page.data &&
        page.data.map(listing => (
          <ListingCard listing={listing} key={listing.id} />
        ))}
    </Grid>
  );
};

const ListingCard = ({ listing: { title, description, price, id } }) => (
  <Grid item xs={12} sm={6} md={3}>
    <Link to={`/listing/${id}`}>
      <Card raised>
        <CardHeader
          title={
            <Typography variant="h3">
              {title} {price}
            </Typography>
          }
        />
        <CardContent>{description}</CardContent>
      </Card>
    </Link>
  </Grid>
);

export default UserListing;
