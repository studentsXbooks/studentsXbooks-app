import React, { useState, useEffect } from "react";
import { Grid, Button } from "@material-ui/core";
import { Formik, Form, Field } from "formik";
import Input from "../ui/Input";
import BookCard from "../components/BookCard";
import SiteMargin from "../ui/SiteMargin";
import Paging from "../components/Paging";
import { apiFetch } from "../utils/fetchLight";

type Props = {
  pageId?: string,
  term?: string,
  navigate: string => any,
  location: { search: string }
};

const FindBook = ({ pageId = "1", term = "", navigate, location }: Props) => {
  const [page, setPage] = useState();

  useEffect(() => {
    if (term !== "")
      apiFetch(`listings/find/${term}?page=${pageId}`, "GET", {})
        .then(res => res.json())
        .then(setPage);
  }, [pageId, term, location.search]);
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
              fullWidth
            />
            <Button type="submit">Submit</Button>
          </Form>
        )}
      </Formik>
      <Button onClick={() => navigate(`/listing/new`)}>
        Can't Find your book? Click here to enter info manually
      </Button>
      <Paging
        basePath={`/listing/findbook/${term}`}
        currentPage={page ? page.currentPage : "1"}
        totalPages={page ? page.totalPages : "1"}
      />
      <Grid container spacing={3}>
        {page &&
          page.data &&
          page.data.map(listing => (
            <BookCard listing={listing} key={listing.title + listing.isbn10} />
          ))}
      </Grid>
      <Paging
        basePath={`/listing/findbook/${term}`}
        currentPage={page ? page.currentPage : "1"}
        totalPages={page ? page.totalPages : "1"}
      />
    </SiteMargin>
  );
};

FindBook.defaultProps = {
  term: "",
  pageId: "1"
};

export default FindBook;
