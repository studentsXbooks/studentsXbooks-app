import React, { useState, useEffect } from "react";
import { Grid, Button } from "@material-ui/core";
import { Formik, Form, Field } from "formik";
import Input from "../ui/Input";
import ListingCard from "../components/ListingCard";
import SiteMargin from "../ui/SiteMargin";
import Paging from "../components/Paging";
import { apiFetch } from "../utils/fetchLight";

type Props = {
  pageId: string,
  term: string,
  navigate: string => any,
  location: { search: string }
};

const FindBook = ({ pageId = "1", term, navigate, location }: Props) => {
  const [page, setPage] = useState();

  useEffect(() => {
    apiFetch(`listings/find/${term}?page=${pageId}`, "GET", {})
      .then(res => res.json())
      .then(setPage);
  }, [pageId, term]);

  return (
    <SiteMargin>
      <Formik
        initialValues={{ search: "" }}
        onSubmit={({ search }, { setSubmitting }) => {
          navigate(`/listing/findbook/${search}`);
          setSubmitting(false);
        }}
      >
        {() => (
          <Form>
            <Field
              label="Search"
              id="search"
              name="search"
              type="text"
              variant="outlined"
              component={Input}
            />
            <Button type="submit">Submit</Button>
          </Form>
        )}
      </Formik>
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
    </SiteMargin>
  );
};

export default FindBook;
