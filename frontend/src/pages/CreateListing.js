import React, { useEffect, useState } from "react";
import { ApiPost, ApiGet } from "../utils";
import { Formik, Form, Field } from "formik";
import { Icon, Button } from "@material-ui/core";
import * as Yup from "yup";

const listingSchema = Yup.object().shape({
  condition: Yup.number().required()
});

const Conditions = props => {
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

const CreateListing = ({ navigate }) => {
  return (
    <>
      <h1>New Listing</h1>
      <p>Enter book details to create new listing</p>

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
              navigate("/listing/" + body.id);
            })
            .finally(() => formikBag.setSubmitting(false));
        }}
      >
        {({ isSubmitting }) => (
          <Form method="POST">
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
            <h3>Author</h3>
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
    </>
  );
};

export default CreateListing;