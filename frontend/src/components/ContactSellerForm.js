import React from "react";
import { Formik, Form, Field } from "formik";
import { Button, Typography } from "@material-ui/core";
import * as Yup from "yup";
import styled from "styled-components";
import { apiFetch } from "../utils/fetchLight";
import Input from "../ui/Input";
import Stack from "../ui/Stack";
import useUserInfo from "../hooks/useUserInfo";

const CustomForm = styled(Form)`
  background: #fff;
  padding: 2rem;
  border-radius: 5px;
`;

const contactSellerSchema = Yup.object().shape({
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
  const { loading, userInfo } = useUserInfo();

  if (listing.contactOption !== 0) {
    return <div>Seller has chosen to not to be contacted</div>;
  }

  return (
    <Formik
      validationSchema={contactSellerSchema}
      enableReinitialize
      initialValues={{
        body: "",
        email: (userInfo && userInfo.email) || "",
        subject: ""
      }}
      onSubmit={async (values, { setSubmitting, setStatus }) => {
        const contactObject = {
          listingId: listing.id,
          ...values
        };

        apiFetch("listings/contact", "POST", contactObject)
          .then(() => {
            onComplete();
          })
          .catch(async e => {
            const errorMessage = await e.response.json();
            setStatus(errorMessage.message);
          })
          .finally(() => setSubmitting(false));
      }}
    >
      {({ isSubmitting, isValid, status }) => (
        <CustomForm>
          <Stack>
            <Typography variant="h6" paragraph>
              Contact Seller: {listing.title}
            </Typography>
            <Typography variant="body1" paragraph>
              Send a message to the Seller. You can tell them about a book you
              have to trade or tell them you have the cash to buy their book.
            </Typography>
            <div>{status}</div>
            <Field
              name="email"
              id="email"
              type="text"
              label="Email"
              fullWidth
              variant="outlined"
              component={Input}
              helperText={<div>{loading && "loading user info..."}</div>}
            />
            <Field
              name="body"
              id="body"
              type="text"
              fullWidth
              variant="outlined"
              multiline
              rows={5}
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
