import React from "react";
import TextField from "@material-ui/core/TextField";
import { ErrorMessage } from "formik";
import styled from "styled-components";
import type { FieldProps } from "formik";

const ErrorText = styled.span`
  color: red;
`;

const Input = ({ field, ...props }: FieldProps) => (
  <TextField
    {...field}
    {...props}
    helperText={
      <ErrorMessage
        name={field.name}
        render={msg => <ErrorText>{msg}</ErrorText>}
      />
    }
  />
);
export default Input;
