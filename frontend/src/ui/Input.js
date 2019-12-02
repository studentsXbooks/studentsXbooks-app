import React from "react";
import TextField from "@material-ui/core/TextField";

type Props = {
  field: {},
  props: {}
};
const Input = ({ field, ...props }: Props) => (
  <TextField {...field} {...props} />
);
export default Input;
