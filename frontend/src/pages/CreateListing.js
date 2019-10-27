import React, { useEffect, useState } from "react";
import { Formik, Form, Field } from "formik";
import { Icon, Button, Grid, Typography } from "@material-ui/core";
import * as Yup from "yup";
import { ApiPost, ApiGet } from "../utils";
import SiteMargin from "../ui/SiteMargin";

const listingSchema = Yup.object().shape({
  condition: Yup.number().required()
});

const Conditions = () => {
  const [conditions, setConditions] = useState();
  useEffect(() => {
    ApiGet(`conditions`, true).then(setConditions);
  }, []);
  return (
    <>
      <option style={{ display: "none" }}>Book Condition</option>
      {conditions &&
        conditions.map(({ value, name }) => (
          <option value={value} key={value}>
            {name}
          </option>
        ))}
    </>
  );
};

type Props = {
  navigate: string => any
};

const CreateListing = ({ navigate }: Props) => {
  return (
    <SiteMargin>
      <Typography variant="h1">New Listing</Typography>
      <Typography variant="p">
        Enter book details to create new listing
      </Typography>
      <Formik
        validationSchema={listingSchema}
        initialValues={{
          title: "",
          description: "",
          isbn10: "",
          price: "",
          firstName: "",
          middleName: "",
          lastName: ""
        }}
        onSubmit={(formValues, formikBag) => {
          ApiPost("listings", true, formValues)
            .then(async res => {
              const body = await res.json();
              navigate(`/listing/${body.id}`);
            })
            .finally(() => formikBag.setSubmitting(false));
        }}
      >
        {({ isSubmitting }) => (
          <Form method="POST">
            <Grid container spacing={3}>
              <Grid item sm={12} md={6}>
                <Typography variant="h3">Book Details</Typography>
                <Field
                  name="title"
                  id="title"
                  type="string"
                  placeholder="title"
                  label="Title"
                  required
                />
                <Field
                  id="description"
                  type="string"
                  name="description"
                  label="Description"
                  placeholder="Description"
                  required
                />
                <Field
                  id="isbn10"
                  type="string"
                  name="isbn10"
                  label="ISBN 10"
                  placeholder="ISBN 10"
                  required
                />
                <Typography variant="h3">Author</Typography>
                <Field
                  id="firstName"
                  type="string"
                  name="firstName"
                  label="First Name"
                  placeholder="First Name"
                  required
                />
                <Field
                  id="middleName"
                  type="string"
                  name="middleName"
                  label="Middle Name"
                  placeholder="Middle Name"
                  required
                />
                <Field
                  id="lastName"
                  type="string"
                  name="lastName"
                  label="Last Name"
                  placeholder="Last Name"
                  required
                />
              </Grid>
              <Grid item sm={12} md={6}>
                <Typography variant="h3">About Your Book</Typography>
                <Field
                  id="price"
                  type="number"
                  name="price"
                  label="Price"
                  placeholder="Price"
                  required
                />
                <Field id="condition" name="condition" component="select">
                  <Conditions />
                </Field>
              </Grid>
            </Grid>
            <br />
            <Button
              variant="contained"
              color="primary"
              endicon={<Icon>send</Icon>}
              type="submit"
              disabled={isSubmitting}
            >
              Submit
            </Button>
          </Form>
        )}
      </Formik>
    </SiteMargin>
  );
};

export default CreateListing;
