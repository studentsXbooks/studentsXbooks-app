// @flow

import React from "react";
import { Button, Typography } from "@material-ui/core";
import * as Yup from "yup";
import { Field, Formik, Form } from "formik";
import Input from "../ui/Input";
import { apiFetch } from "../utils/fetchLight";
import SiteMargin from "../ui/SiteMargin";
import Stack from "../ui/Stack";
// $FlowFixMe
import styled from "styled-components";

const registerSchema = Yup.object().shape({
  username: Yup.string()
    .min(1, "Must be at least 1 character long.")
    .max(32, "Must be at most 32 characters.")
    .required("Username required"),
  email: Yup.string()
    .min(1, "Must be at least 1 character long.")
    .matches(/.+@.+[.]edu/, "Must be a .edu address.")
    .required("Email required"),
  password: Yup.string()
    .min(8, "Password must be at least 8 characters long.")
    .required("Password required.")
});
const StyledForm = styled.div`
  & > form {
    width: 750px;
    margin: auto;
    border: 3px solid #ccc;
    border-radius: 5px;
    padding: 2rem;
  }
`;
const ErrorMsg = styled.div`
  color: red;
  font-size: 1em;
`;

const Register = ({ navigate }: Object) => {
  return (
    <SiteMargin>
      <Formik
        validateOnChange
        validationSchema={registerSchema}
        initialValues={{
          username: "",
          email: "",
          password: ""
        }}
        onSubmit={(formValues, formikBag) => {
          apiFetch("users/register", "POST", formValues)
            .then(async res => {
              await res.json();
              navigate(`/verify-email?email=${formValues.email}`);
            })
            .catch(async error => {
              const body = await error.response.json();
              if (body.message) {
                formikBag.setStatus(body.message);
              }
            })
            .finally(() => formikBag.setSubmitting(false));
        }}
      >
        {({ isSubmitting, isValid, errors, touched, status }) => (
          <StyledForm>
            <Form>
              <Typography variant="h1">Register</Typography>
              <Stack>
                {status && <h4 style={{ color: "red" }}>{status}</h4>}
                <Field
                  name="username"
                  id="userName"
                  placeholder="Username"
                  component={Input}
                  label="Username"
                  variant="outlined"
                  fullWidth
                />
                {errors.username && touched.username ? (
                  <ErrorMsg>{errors.username}</ErrorMsg>
                ) : null}

                <br />
                <Field
                  id="email"
                  name="email"
                  label="Email"
                  component={Input}
                  variant="outlined"
                  placeholder="Email"
                  fullWidth
                />
                {errors.email && touched.email ? (
                  <ErrorMsg>{errors.email}</ErrorMsg>
                ) : null}

                <Field
                  id="password"
                  name="password"
                  label="Password"
                  component={Input}
                  variant="outlined"
                  placeholder="Password"
                  type="Password"
                  fullWidth
                />
                {errors.password && touched.password ? (
                  <ErrorMsg>{errors.password}</ErrorMsg>
                ) : null}

                <Button
                  type="submit"
                  variant="contained"
                  color="primary"
                  align="right"
                  fullWidth
                  disabled={isSubmitting || !isValid}
                >
                  Submit
                </Button>
              </Stack>
            </Form>
          </StyledForm>
        )}
      </Formik>
    </SiteMargin>
  );
};

export default Register;
