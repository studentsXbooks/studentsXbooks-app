import React from 'react';
import styled from 'styled-components';
import { Form, Field, Formik } from 'formik';
// import { navigate } from '@reach/router';
// import Topography from '../../images/topography.svg';
import { callApi, ensureResponseCode, unwrapToJSON } from '../utils';
import { Header, Input, Button } from '../ui';
import useLoading from '../hooks/useLoading';

const postToAuthApi = callApi(`user/login`, 'POST');

const Login = () => {
  const [loading, startLoading, doneLoading] = useLoading();

  return (
    <CenterComponent>
        <Header align="center">txtX Login</Header>
        <Formik
          initialValues={{ email: '', password: '' }}
          onSubmit={async (values, { setSubmitting, setStatus }) => {
            startLoading();
            postToAuthApi(values)
              .then(ensureResponseCode(200))
              .then(unwrapToJSON)
              .then(user => {
                // // put user in local storage
                // localStorage.setItem(
                //   `${process.env.REACT_APP_TOKEN}`,
                //   user.token
                // );
                localStorage.setItem('email', user.email);
              })
              .catch(setStatus)
              .finally(() => {
                setSubmitting(false);
                doneLoading();
              });
          }}
        >
          {({ status, values, isSubmitting, handleSubmit }) => (
            <Form onSubmit={handleSubmit}>
              {status && status.message && (
                <div style={{ color: 'red' }}>{status.message}</div>
              )}
              <Field
                id="email"
                name="email"
                value={values.email}
                component={Input}
                label="Email"
              />
              <Field
                id="password"
                type="password"
                name="password"
                value={values.password}
                component={Input}
                label="Password"
              />
              <br />
              <Button
                type="submit"
                display="block"
                align="right"
                disabled={isSubmitting}
              >
                Submit
              </Button>
            </Form>
          )}
        </Formik>
    </CenterComponent>
  );
};

const CenterComponent = styled.div`
  height: 100vh;
  display: grid;
  align-items: center;
  justify-content: center;
  background-color: #dfdbe5;
`;

export default Login;
