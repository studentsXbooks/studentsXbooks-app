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
import { apiFetch } from "../utils/fetchLight";
import FullHeightGrid from "../ui/FullHeightGrid";

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

  return (
    <FullHeightGrid container alignItems="center" justify="center">
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
                  navigate("/");
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
                  Login
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
