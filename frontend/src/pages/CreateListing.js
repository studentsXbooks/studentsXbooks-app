import React from "react";
import { Field } from "formik";
import { Grid, Typography } from "@material-ui/core";
import * as Yup from "yup";
import { ApiPost } from "../utils";
import useApi from "../hooks/useApi";
import SiteMargin from "../ui/SiteMargin";
import MultiStepForm from "../components/MultiStepForm";
import RadioButton from "../components/RadioButton";

const listingSchema = Yup.object().shape({});

const Conditions = () => {
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

const ListingDetailsInputs = (
  <Grid container spacing={3}>
    <Grid item sm={12} md={6}>
      <Typography variant="h3">Book Details</Typography>
      <Field
        name="title"
        id="title"
        type="string"
        placeholder="title"
        label="Title"
      />
      <Field
        id="description"
        type="string"
        name="description"
        label="Description"
        placeholder="Description"
      />
      <Field
        id="isbn10"
        type="string"
        name="isbn10"
        label="ISBN 10"
        placeholder="ISBN 10"
      />
      <Typography variant="h3">Author</Typography>
      <Field
        id="firstName"
        type="string"
        name="firstName"
        label="First Name"
        placeholder="First Name"
      />
      <Field
        id="middleName"
        type="string"
        name="middleName"
        label="Middle Name"
        placeholder="Middle Name"
      />
      <Field
        id="lastName"
        type="string"
        name="lastName"
        label="Last Name"
        placeholder="Last Name"
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
      />
      <Conditions />
    </Grid>
  </Grid>
);

const getAboutText = contactEnumValue => {
  switch (contactEnumValue) {
    case 0:
      return "Seller will receive an email with the Buyer's contact information and an additional message. Buyer will not see any information about the Seller unless the Seller replies to the email";
    default:
      return "Missing Enum Match";
  }
};

const MethodOfContactInputs = () => {
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

const ReviewListing = ({
  values: { title, description, price, contactOption, condition }
}) => {
  return (
    <div>
      <Typography variant="h4">Title: {title}</Typography>
      <Typography variant="h4">Description: {description}</Typography>
      <Typography variant="h4">Price: {price}</Typography>
      <Typography variant="h4">Condition: {condition}</Typography>
      <Typography variant="h4">ContactOption: {contactOption}</Typography>
    </div>
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
      <MultiStepForm
        steps={[
          { name: "Listing Details", inputGroup: ListingDetailsInputs },
          { name: "Method of Contact", inputGroup: MethodOfContactInputs() }
        ]}
        onComplete={(formValues, formikBag) => {
          ApiPost("listings", true, formValues)
            .then(async res => {
              const body = await res.json();
              navigate(`/listing/${body.id}`);
            })
            .finally(() => formikBag.setSubmitting(false));
        }}
        reviewComponent={values => <ReviewListing values={values} />}
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
      />
    </SiteMargin>
  );
};

export default CreateListing;
