// @flow

import React, { useState } from "react";
import {
  TextField,
  Button,
  Card,
  CardHeader,
  CardContent,
  CardActions,
  Grid,
  Typography,
  MuiThemeProvider,
  createMuiTheme
} from "@material-ui/core";
// $FlowFixMe
import styled from "styled-components";
import FullHeightGrid from "../ui/FullHeightGrid";
import { apiFetch } from "../utils/fetchLight";

const Register = ({ navigate }: Object) => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword] = useState("");

  const submitButton = createMuiTheme({
    palette: {
      primary: {
        main: "#33578c"
      }
    }
  });

  const LoginInfo = styled.div`
  text-align: center;
  background-color: #ffffff;
  color: #707070;
  padding 50px;
  margin: auto;
  box-shadow: inset 0 0 2px 2px;
`;
  return (
    <FullHeightGrid container alignItems="center" justify="center">
      <LoginInfo>
        <h2>Create a new account today using a valid '.edu' email.</h2>
      </LoginInfo>
      <Grid item>
        <Card
          style={{ width: "25rem", padding: 20, marginRight: 25 }}
          component="article"
          raised
        >
          <form
            onSubmit={e => {
              e.preventDefault();
              apiFetch("users/register", "POST", { username, email, password })
                .then(() => {
                  navigate(`/verify-email?email=${email}`);
                })
                .catch(console.log);
            }}
            method="POST"
          >
            <CardHeader
              title={
                <Typography variant="h4" align="center">
                  Register!
                </Typography>
              }
            />
            <CardContent>
              <TextField
                id="userName"
                label="UserName"
                value={username}
                onChange={e => {
                  setUsername(e.target.value);
                }}
                fullWidth
              />
              <br />
              <TextField
                id="email"
                label="Email"
                value={email}
                onChange={e => {
                  setEmail(e.target.value);
                }}
                fullWidth
              />
              <br />
              <TextField
                id="password"
                label="Password"
                type="password"
                value={password}
                onChange={e => {
                  setPassword(e.target.value);
                }}
                fullWidth
              />
              <br />
              <TextField
                id="confirmPassword"
                label="Confirm Password"
                type="password"
                value={confirmPassword}
                fullWidth
              />
              <br />
            </CardContent>
            <CardActions>
              <MuiThemeProvider theme={submitButton}>
                <Button
                  variant="contained"
                  color="primary"
                  fullWidth
                  type="Submit"
                >
                  Sign Up
                </Button>
              </MuiThemeProvider>
            </CardActions>
          </form>
        </Card>
      </Grid>
    </FullHeightGrid>
  );
};

export default Register;
