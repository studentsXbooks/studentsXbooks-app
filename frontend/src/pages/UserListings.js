import React, { useEffect, useState } from "react";
import { ApiGet } from "../utils";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography
} from "@material-ui/core";

const listingList = [
  {
    title: "Harry Potter",
    description: "Harry potter is a wizard",
    price: "5.99",
    id: 0
  },
  {
    title: "Garfield",
    description: "Garfield is a wizard",
    price: "5.99",
    id: 1
  },
  {
    title: "Thing here",
    description: "Garfield is a wizard",
    price: "5.99",
    id: 2
  },
  {
    title: "Thing here",
    description: "Garfield is a wizard",
    price: "5.99",
    id: 3
  }
];

// Need paging, and need listing details page
const UserListing = () => {
  const [listings, setListings] = useState(listingList);

  useEffect(() => {
    ApiGet("listings/user", true).then(setListings);
  }, []);

  return (
    <Grid container spacing={3}>
      {listings &&
        listings.map(listing => (
          <ListingCard listing={listing} key={listing.id} />
        ))}
    </Grid>
  );
};

const ListingCard = ({ listing: { title, description, price } }) => (
  <Grid item xs={12} sm={6} md={3}>
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
  </Grid>
);

export default UserListing;
