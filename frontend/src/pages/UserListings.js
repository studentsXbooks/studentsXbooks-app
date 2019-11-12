import React from "react";
import { Grid } from "@material-ui/core";
import ListingCard from "../components/ListingCard";
import useApi from "../hooks/useApi";
import Paging from "../components/Paging";
import withSearchBar from "../components/withSearchBar";
import SiteMargin from "../ui/SiteMargin";

type Props = {
  pageId: string
};

// Need paging, and need listing details page
const UserListing = ({ pageId = "1" }: Props) => {
  const { data: page } = useApi(`listings/user/${pageId}`);

  return (
    <>
      {page && (
        <>
          <Paging
            basePath="/user/listings"
            currentPage={page.currentPage}
            totalPages={page.totalPages}
          />
          <SiteMargin>
            <Grid container spacing={3} wrap="wrap" justify="space-around">
              {page.data &&
                page.data.map(listing => (
                  <ListingCard listing={listing} key={listing.id} />
                ))}
            </Grid>
          </SiteMargin>
          <Paging
            basePath="/user/listings"
            currentPage={page.currentPage}
            totalPages={page.totalPages}
          />
        </>
      )}
    </>
  );
};

export default withSearchBar(UserListing);
