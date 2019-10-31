import React from "react";
import { Formik, Form, Field } from "formik";
import { Button } from "@material-ui/core";
import * as Yup from "yup";
import { apiFetch } from "../utils/fetchLight";

const contactSellerSchema = Yup.object().shape({
  subject: Yup.string().required,
  body: Yup.string().required
});

type ListingDetail = {
  contactOption: number
};

type Props = {
  listing: ListingDetail,
  onComplete: () => null
};

const ContactSellerForm = ({ listing, onComplete }: Props) => {
  if (listing.contactOption !== 0) {
    return <div>Seller has chosen to not to be contacted</div>;
  }

  return (
    <Formik
      validationSchema={contactSellerSchema}
      onSubmit={async (values, { setSubmitting }) => {
        apiFetch("/listings/contact", "POST", values)
          .then(() => {
            onComplete();
          })
          .catch(e => console.log(e))
          .finally(() => setSubmitting(false));
      }}
    >
      {({ isSubmitting, isValid }) => (
        <Form>
          <label htmlFor="subject">Subject</label>
          <Field name="subject" id="subject" type="text" />
          <label htmlFor="body">Body</label>
          <Field name="body" id="body" type="text" />
          <Button
            type="submit"
            variant="contained"
            disabled={!isValid || isSubmitting}
          >
            Send
          </Button>
        </Form>
      )}
    </Formik>
  );
};

export default ContactSellerForm;
