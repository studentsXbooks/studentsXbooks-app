import React, { useState, useEffect } from "react";
import { Grid } from "@material-ui/core";
import { ApiGet } from "../utils";
import ListingCard from "../components/ListingCard";
import SiteMargin from "../ui/SiteMargin";
import Paging from "../components/Paging";

type Props = {
  pageId: string,
  term: string,
  navigate: string => any,
  location: { search: string }
};

const FindBook = ({ pageId = "1", term, navigate, location }: Props) => {
  const [page, setPage] = useState();

  useEffect(() => {
    ApiGet(`listings/findBook/${term}/${pageId}`, true).then(setPage);
  }, [pageId]);

  return (
    <SiteMargin>
      <Grid container spacing={3}>
        <Grid item xs={12} sm={3}>
          {" "}
        </Grid>
        <Grid item xs={12} sm={9}>
          <Paging
            basePath={`/findBook/${term}`}
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
            basePath={`/findBook/${term}`}
            currentPage={page ? page.currentPage : "1"}
            totalPages={page ? page.totalPages : "1"}
          />
        </Grid>
      </Grid>
    </SiteMargin>
  );
};

export default FindBook;
