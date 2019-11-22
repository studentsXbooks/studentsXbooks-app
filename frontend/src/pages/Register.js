// @flow

import React, { useState } from "react";
import { navigate } from "@reach/router";
import {
  TextField,
  Button,
  Card,
  CardHeader,
  CardContent,
  CardActions,
  Grid,
  Typography
} from "@material-ui/core";
import FullHeightGrid from "../ui/FullHeightGrid";
import { apiFetch } from "../utils/fetchLight";
// $FlowFixMe
import styled from "styled-components";
import { MuiThemeProvider, createMuiTheme } from "@material-ui/core";

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
  color: #000000;
  padding 10px;
  margin: auto;
`;
  return (
    <FullHeightGrid container alignItems="center" justify="center">
      <Grid item>
        <Card
          style={{ width: "50rem", padding: 20 }}
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
                  Submit
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
