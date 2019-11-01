import React from "react";
import TextField from "@material-ui/core/TextField";

// add error output

const Input = ({ field, form: { touched, errors }, ...props }) => (
  <TextField {...field} {...props} />
);
export default Input;
