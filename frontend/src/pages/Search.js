import React, { useState, useEffect } from "react";
import { Link } from "@reach/router";
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
import { ApiPost } from "../utils";
import SearchFilterForm from "../components/SearchFilterForm";

type Props = {
  pageId: string,
  term: string,
  navigate: string => any,
  location: { search: string }
};

// Need paging, and need listing details page
const Search = ({ pageId = "1", term, navigate, location }: Props) => {
  const [page, setPage] = useState();

  useEffect(() => {
    const urlParams = new URLSearchParams(location.search);
    ApiPost(`listings/search/${term}/${pageId}`, true, {
      minPrice: urlParams.get("min"),
      maxPrice: urlParams.get("max"),
      conditions: urlParams.get("conditions")
    })
      .then(res => res.json())
      .then(setPage);
  }, [pageId, term, location.search]);

  return (
    <div>
      <SearchFilterForm
        basePath={`/search/${term}/${pageId}`}
        {...{ navigate, location }}
      />
      <Grid container spacing={3}>
        {page &&
          page.data &&
          page.data.map(listing => (
            <ListingCard listing={listing} key={listing.id} />
          ))}
        <List>
          <ListItem>
            <Button
              onClick={() => navigate(`/search/${term}/${Number(pageId) - 1} `)}
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
              onClick={() => navigate(`/search/${term}/${Number(pageId) + 1} `)}
              disabled={page && !page.hasNext}
            >
              Next
            </Button>
          </ListItem>
        </List>
      </Grid>
    </div>
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

export default Search;
