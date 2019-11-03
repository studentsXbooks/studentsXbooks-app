import React, { useState, useEffect } from "react";
import { Grid } from "@material-ui/core";
import { apiFetch } from "../utils/fetchLight";
import SearchFilterForm from "../components/SearchFilterForm";
import ListingCard from "../components/ListingCard";
import SiteMargin from "../ui/SiteMargin";
import Paging from "../components/Paging";

type Props = {
  pageId: string,
  term: string,
  navigate: string => any,
  location: { search: string }
};

const Search = ({ pageId = "1", term, navigate, location }: Props) => {
  const [page, setPage] = useState();

  useEffect(() => {
    const urlParams = new URLSearchParams(location.search);
    const urlConditions = urlParams.get("conditions");
    const conditions = urlConditions ? urlConditions.split(",") : [];
    apiFetch(`listings/search/${term}/${pageId}`, "POST", {
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
          <Paging
            basePath={`/search/${term}`}
            currentPage={page ? page.currentPage : "1"}
            totalPages={page ? page.totalPages : "1"}
          />
          <Grid container spacing={3}>
            {page &&
              page.data &&
              page.data.map(listing => (
                <ListingCard listing={listing} key={listing.id} />
              ))}
          </Grid>
          <Paging
            basePath={`/search/${term}`}
            currentPage={page ? page.currentPage : "1"}
            totalPages={page ? page.totalPages : "1"}
          />
        </Grid>
      </Grid>
    </SiteMargin>
  );
};

export default Search;
