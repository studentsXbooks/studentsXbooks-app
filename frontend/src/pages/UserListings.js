import React, { useEffect, useState } from "react";
import { Link } from "@reach/router";
import { ApiGet } from "../utils";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography,
  Button,
  List,
  ListItem
} from "@material-ui/core";

// Need paging, and need listing details page
const UserListing = ({ pageId = 1, navigate }) => {
  const [page, setPage] = useState();

  useEffect(() => {
    ApiGet(`listings/user/${pageId}`, true).then(setPage);
  }, [pageId]);

  return (
    <Grid container spacing={3}>
      {page &&
        page.data &&
        page.data.map(listing => (
          <ListingCard listing={listing} key={listing.id} />
        ))}
      <List>
        <ListItem>
          <Button
            onClick={() => navigate(`/user/listings/${Number(pageId) - 1} `)}
            disabled={page && !page.hasPrev}
          >
            Prev
          </Button>
        </ListItem>
        <ListItem>
          <Typography>{page && page.currentPage}</Typography>
        </ListItem>
        <ListItem>
          <Button
            onClick={() => navigate(`/user/listings/${Number(pageId) + 1} `)}
            disabled={page && !page.hasNext}
          >
            Next
          </Button>
        </ListItem>
      </List>
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
