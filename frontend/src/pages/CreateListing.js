import React from "react";
import { Field, Formik, Form } from "formik";
import { Grid, Typography, Button } from "@material-ui/core";
import * as Yup from "yup";
import useApi from "../hooks/useApi";
import SiteMargin from "../ui/SiteMargin";
import RadioButton from "../ui/RadioButton";
import { apiFetch } from "../utils/fetchLight";
import Input from "../ui/Input";

const listingSchema = Yup.object().shape({});

const ConditionRadios = () => {
  const { data: conditions } = useApi("conditions");
  return (
    <fieldset>
      <legend>condition</legend>
      {conditions &&
        conditions.map(({ value, name }) => (
          <Field
            key={name}
            id={value.toString()}
            name="condition"
            label={name}
            component={RadioButton}
          />
        ))}
    </fieldset>
  );
};

const getAboutText = contactEnumValue => {
  switch (contactEnumValue) {
    case 0:
      return "Seller will receive an email with the Buyer's contact information and an additional message. Buyer will not see any information about the Seller unless the Seller replies to the email";
    default:
      return "Missing Enum Match";
  }
};

const MethodOfContactRadios = () => {
  const { data } = useApi("listings/contact");

  return (
    <fieldset>
      <legend>Contact Option</legend>
      {data &&
        data
          .filter(x => x.value === 0) // Match for now until additional options are available
          .map(x => (
            <Field
              id={x.value.toString()}
              key={x.name}
              name="contactOption"
              label={x.name}
              component={RadioButton}
              about={getAboutText(x.value)}
            />
          ))}
    </fieldset>
  );
};

type Props = {
  navigate: string => any
};

const CreateListing = ({ navigate }: Props) => {
  return (
    <SiteMargin>
      <Typography variant="h1">New Listing</Typography>
      <Typography variant="subtitle1">
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
          apiFetch("listings", "POST", formValues)
            .then(async res => {
              const body = await res.json();
              navigate(`/listing/${body.id}`);
            })
            .finally(() => formikBag.setSubmitting(false));
        }}
      >
        {() => (
          <Form>
            <Grid container spacing={3}>
              <Grid item sm={12} md={6}>
                <Typography variant="h3">Book Details</Typography>
                <Field
                  name="title"
                  id="title"
                  placeholder="Title"
                  component={Input}
                  label="Title"
                  variant="outlined"
                />
                <Field
                  id="description"
                  name="description"
                  label="Description"
                  component={Input}
                  variant="outlined"
                  placeholder="Description"
                />
                <Field
                  id="isbn10"
                  name="isbn10"
                  label="ISBN 10"
                  component={Input}
                  variant="outlined"
                  placeholder="ISBN 10"
                />
                <Typography variant="h3">Author</Typography>
                <Field
                  id="firstName"
                  name="firstName"
                  label="First Name"
                  component={Input}
                  variant="outlined"
                  placeholder="First Name"
                />
                <Field
                  id="middleName"
                  name="middleName"
                  label="Middle Name"
                  component={Input}
                  variant="outlined"
                  placeholder="Middle Name"
                />
                <Field
                  id="lastName"
                  name="lastName"
                  component={Input}
                  label="Last Name"
                  variant="outlined"
                  placeholder="Last Name"
                />
              </Grid>
              <Grid item sm={12} md={6}>
                <Typography variant="h3">About Your Book</Typography>
                <Field
                  id="price"
                  type="number"
                  name="price"
                  component={Input}
                  variant="outlined"
                  label="Price"
                  placeholder="Price"
                />
                <ConditionRadios />
              </Grid>
            </Grid>
            <br />
            <br />
            <Typography variant="h6">
              How do you want to be contacted?
            </Typography>
            <Grid container>
              <Grid>
                <MethodOfContactRadios />
              </Grid>
            </Grid>
            <Button
              type="Submit"
              variant="contained"
              color="primary"
              align="right"
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
