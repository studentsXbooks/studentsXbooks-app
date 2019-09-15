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
import { ApiPost } from "../utils";
import FullHeightGrid from "../ui/FullHeightGrid";

const Login = ({ navigate }: Object) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  return (
    <FullHeightGrid container alignItems="center" justify="center">
      <Grid item xs={9} sm={6} lg={3}>
        <Card component="article" raised>
          <form
            onSubmit={e => {
              e.preventDefault();
              ApiPost("users", true, { email, password })
                .then(() => {
                  navigate("/login-success");
                })
                .catch(console.log);
            }}
            method="POST"
          >
            <CardHeader
              title={
                <Typography variant="h3" align="center">
                  Login
                </Typography>
              }
            />
            <CardContent>
              <TextField
                id="email"
                label="Email"
                value={email}
                onChange={e => setEmail(e.target.value)}
                fullWidth
              />
              <br />
              <TextField
                id="password"
                label="Password"
                type="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
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

export default Login;
