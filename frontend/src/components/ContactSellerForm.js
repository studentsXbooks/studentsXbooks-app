import React from "react";
import { Formik, Form, Field } from "formik";
import { Button, Typography } from "@material-ui/core";
import * as Yup from "yup";
import styled from "@emotion/styled";
import { apiFetch } from "../utils/fetchLight";
import Input from "../ui/Input";

const CustomForm = styled(Form)`
  background: #fff;
  padding: 2rem;
  border-radius: 5px;
`;

const Stack = styled.div`
  & > * + * {
    display: block;
    margin-bottom: 1rem;
  }
`;

const contactSellerSchema = Yup.object().shape({
  subject: Yup.string().required,
  body: Yup.string().required,
  email: Yup.string().email.required
});

type ListingDetail = {
  contactOption: number,
  id: number,
  title: string
};

type Props = {
  listing: ListingDetail,
  onComplete: () => typeof undefined
};

const ContactSellerForm = ({ listing, onComplete }: Props) => {
  if (listing.contactOption !== 0) {
    return <div>Seller has chosen to not to be contacted</div>;
  }

  return (
    <Formik
      validationSchema={contactSellerSchema}
      initialValues={{ body: "", email: "", subject: "" }}
      onSubmit={async (values, { setSubmitting }) => {
        const contactObject = {
          listingId: listing.id,
          ...values
        };

        apiFetch("listings/contact", "POST", contactObject)
          .then(() => {
            onComplete();
          })
          .catch(e => console.log(e))
          .finally(() => setSubmitting(false));
      }}
    >
      {({ isSubmitting, isValid }) => (
        <CustomForm>
          <Stack>
            <Typography variant="h6" paragraph>
              Contact Seller: about {listing.title}
            </Typography>
            <Typography variant="body1" paragraph>
              Send a message to the Seller. You can tell them about a book you
              have to trade or tell them you have the cash to buy their book.
            </Typography>
            <Field
              name="email"
              id="email"
              type="text"
              label="Email"
              fullWidth
              variant="outlined"
              component={Input}
            />
            <Field
              name="subject"
              id="subject"
              type="text"
              label="Subject"
              fullWidth
              variant="outlined"
              component={Input}
            />
            <Field
              name="body"
              id="body"
              type="text"
              fullWidth
              variant="outlined"
              label="Body"
              component={Input}
            />
            <Button
              type="submit"
              variant="contained"
              color="primary"
              disabled={!isValid || isSubmitting}
              fullWidth
            >
              Send
            </Button>
          </Stack>
        </CustomForm>
      )}
    </Formik>
  );
};

export default ContactSellerForm;
