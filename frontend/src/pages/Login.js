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
import { apiFetch } from "../utils/fetchLight";
import FullHeightGrid from "../ui/FullHeightGrid";

// $FlowFixMe
import styled from "styled-components";
import { MuiThemeProvider, createMuiTheme } from "@material-ui/core";

const Login = ({ navigate }: Object) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const submitButton = createMuiTheme({
    palette: {
      primary: {
        main: "#33578c"
      }
    }
  });

  const quote = styled.h2`
    text-align: center;
    color: #707070;
  `;

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
        <h2>You can login here using your email and password!</h2>
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
              apiFetch("users", "POST", { email, password })
                .then(() => {
                  navigate("/home");
                })
                .catch(console.log);
            }}
            method="POST"
          >
            <CardHeader
              title={
                <Typography variant="h4" align="center">
                  Login!
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

export default Login;
