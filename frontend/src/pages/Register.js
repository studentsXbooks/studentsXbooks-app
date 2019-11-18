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
  Typography
} from "@material-ui/core";
import FullHeightGrid from "../ui/FullHeightGrid";
import { apiFetch } from "../utils/fetchLight";

const Register = ({ navigate }: Object) => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  return (
    <FullHeightGrid container alignItems="center" justify="center">
      <Grid item xs={9} sm={6} lg={3}>
        <Card component="article" raised>
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
                <Typography variant="h3" align="center">
                  Registration
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
            </CardContent>
            <CardActions>
              <Button
                variant="contained"
                color="primary"
                fullWidth
                type="submit"
              >
                Submit
              </Button>
            </CardActions>
          </form>
        </Card>
      </Grid>
    </FullHeightGrid>
  );
};

export default Register;
