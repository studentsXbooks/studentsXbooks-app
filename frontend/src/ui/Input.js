import React from "react";
import TextField from "@material-ui/core/TextField";
import { ErrorMessage } from "formik";
import styled from "styled-components";
import type { FieldProps } from "formik";

const ErrorText = styled.span`
  color: red;
`;

const Input = ({ field, helperText, ...props }: FieldProps) => (
  <TextField
    {...field}
    {...props}
    helperText={
      helperText || (
        <ErrorMessage
          name={field.name}
          render={msg => <ErrorText>{msg}</ErrorText>}
        />
      )
    }
  />
);
export default Input;
