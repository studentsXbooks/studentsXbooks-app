import React, { useState } from "react";
import { Button, Typography } from "@material-ui/core";

import { apiFetch } from "../utils/fetchLight";
import * as Yup from "yup";
import styled from "styled-components";

import { Field, Formik, Form } from "formik";
import Input from "../ui/Input";
import SiteMargin from "../ui/SiteMargin";
import Stack from "../ui/Stack";

const Login = ({ navigate }: Object) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const loginSchema = Yup.object().shape({
    email: Yup.string()
      .email("Invalid email address")
      .required("Email Required"),
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
  return (
    <SiteMargin>
      <Formik
        validateOnChange
        validationSchema={loginSchema}
        initialValues={{
          email: "",
          password: ""
        }}
        onSubmit={(formValues, formikBag) => {
          apiFetch("users", "POST", formValues)
            .then(() => {
              navigate(`/`);
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
        {({ isSubmitting, isValid, status }) => (
          <StyledForm>
            <Form>
              <Typography variant="h1">Login</Typography>
              <Stack>
                {status && <h4 style={{ color: "red" }}>{status}</h4>}
                <Field
                  id="email"
                  name="email"
                  label="Email"
                  component={Input}
                  variant="outlined"
                  placeholder="Email"
                  fullWidth
                />
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
                <Button
                  type="submit"
                  fullWidth
                  color="primary"
                  variant="contained"
                  align="right"
                  disabled={isSubmitting || !isValid}
                >
                  Login
                </Button>
              </Stack>
            </Form>
          </StyledForm>
        )}
      </Formik>
    </SiteMargin>
  );
};

export default Login;
