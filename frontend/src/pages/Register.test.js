import React from "react";
import { render, fireEvent, wait } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import Register from "./Register";

test("Renders with required props", () => {
  const { container } = render(<Register />);
  expect(container).toBeDefined();
});

test("Submit button disabled on mount", () => {
  const { getByText } = render(<Register />);
  expect(getByText(/submit/i)).toBeDisabled();
});

test("Can't submit with invalid email and other fields valid, can submit after valid email entered", async () => {
  const { getByText, getByLabelText, findByText } = render(<Register />);

  fireEvent.change(getByLabelText(/email/i), {
    target: { value: "testing" }
  });
  fireEvent.blur(getByLabelText(/email/i));

  const emailError = await findByText(/Invalid Email/i);
  expect(emailError).toBeDefined();

  await wait(() => {
    expect(getByText(/submit/i)).toBeDisabled();
  });

  fireEvent.change(getByLabelText(/username/i), {
    target: { value: "newuser" }
  });

  fireEvent.change(getByLabelText(/email/i), {
    target: { value: "test@wvup.edu" }
  });

  fireEvent.change(getByLabelText(/password/i), {
    target: { value: "Develop@90" }
  });

  fireEvent.blur(getByLabelText(/email/i));

  await wait(() => {
    expect(getByText(/submit/i)).not.toBeDisabled();
  });
});

test("Can't submit with invalid password and other fields valid, can submit after valid password entered", async () => {
  const { getByText, getByLabelText, findByText } = render(<Register />);

  fireEvent.change(getByLabelText(/password/i), {
    target: { value: "Deve0" }
  });

  fireEvent.blur(getByLabelText(/password/i));

  const passwordError = await findByText(/must/i);
  expect(passwordError).toBeDefined();

  await wait(() => {
    expect(getByText(/submit/i)).toBeDisabled();
  });

  fireEvent.change(getByLabelText(/username/i), {
    target: { value: "newuser" }
  });

  fireEvent.change(getByLabelText(/email/i), {
    target: { value: "testing@wvup.edu" }
  });

  fireEvent.change(getByLabelText(/password/i), {
    target: { value: "Develop@90" }
  });

  fireEvent.blur(getByLabelText(/password/i));

  await wait(() => {
    expect(getByText(/submit/i)).not.toBeDisabled();
  });
});
