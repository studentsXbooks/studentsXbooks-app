import React, { useState, useEffect } from "react";
import { Grid, Typography, Button, List, ListItem } from "@material-ui/core";
import { ApiPost } from "../utils";
import SearchFilterForm from "../components/SearchFilterForm";
import ListingCard from "../components/ListingCard";
import SiteMargin from "../ui/SiteMargin";

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
    const urlConditions = urlParams.get("conditions");
    const conditions = urlConditions ? urlConditions.split(",") : [];
    ApiPost(`listings/search/${term}/${pageId}`, true, {
      minPrice: urlParams.get("min"),
      maxPrice: urlParams.get("max"),
      conditions
    })
      .then(res => res.json())
      .then(setPage);
  }, [pageId, term, location.search]);

  return (
    <SiteMargin>
      <Grid container spacing={3}>
        <Grid item xs={12} sm={3}>
          <SearchFilterForm
            basePath={`/search/${term}/${pageId}`}
            {...{ navigate, location }}
          />
        </Grid>
        <Grid item xs={12} sm={9}>
          <Grid container spacing={3}>
            {page &&
              page.data &&
              page.data.map(listing => (
                <ListingCard listing={listing} key={listing.id} />
              ))}
            <List>
              <ListItem>
                <Button
                  onClick={() =>
                    navigate(`/search/${term}/${Number(pageId) - 1} `)
                  }
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
                  onClick={() =>
                    navigate(`/search/${term}/${Number(pageId) + 1} `)
                  }
                  disabled={page && !page.hasNext}
                >
                  Next
                </Button>
              </ListItem>
            </List>
          </Grid>
        </Grid>
      </Grid>
    </SiteMargin>
  );
};

export default Search;
