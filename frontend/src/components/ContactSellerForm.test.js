import React from "react";
import {
  render,
  fireEvent,
  wait,
  waitForElement
} from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import ContactSellerForm from "./ContactSellerForm";
import makeFetchReturn from "../test-utils/makeFetchReturn";

const validListing = {
  contactOption: 0,
  id: 0,
  title: "GOOD"
};

const badListing = {
  contactOption: -1,
  id: 0,
  title: "BAD"
};

it("Renders with required props", () => {
  const { container } = render(
    <ContactSellerForm listing={validListing} onComplete={jest.fn()} />
  );
  expect(container).toBeDefined();
});

it("Empty Message and Subject, Send is disabled", async () => {
  const { getByText } = render(
    <ContactSellerForm listing={validListing} onComplete={jest.fn()} />
  );
  const sendButton = getByText("Send");
  expect(sendButton).toBeDisabled();
});

it("Listing does not have contact option = 0, text saying seller has chosen not to be contacted is shown", () => {
  const { getByText } = render(
    <ContactSellerForm listing={badListing} onComplete={jest.fn()} />
  );
  const noContact = getByText(/not to be contacted/);
  expect(noContact).toBeDefined();
});

it("Calls onComplete after form is submitted successfully", async () => {
  makeFetchReturn({ status: 200 })({ spooky: true });
  const mockComplete = jest.fn();
  const { getByLabelText, getByText } = render(
    <ContactSellerForm listing={validListing} onComplete={mockComplete} />
  );
  fireEvent.change(getByLabelText(/body/i), {
    target: { value: "Hey I really would like to trade" }
  });
  fireEvent.submit(getByText("Send"));
  await wait(() => {
    expect(mockComplete).toBeCalledTimes(1);
  });
});

it("Should fill in email with the current user's email", async () => {
  const email = "buyer@gmail.com";
  makeFetchReturn({ status: 200 })({ email });
  const { getByLabelText, getByText } = render(
    <ContactSellerForm listing={validListing} onComplete={jest.fn()} />
  );
  const loading = getByText(/loading/i);
  expect(loading).toBeDefined();

  const emailInput = getByLabelText(/email/i);
  await wait(() => {
    expect(emailInput).toHaveValue(email);
  });
});

it("Should display error message when fetch returns back a non 2xx on submit", async () => {
  const { getByLabelText, getByText } = render(
    <ContactSellerForm listing={validListing} onComplete={jest.fn()} />
  );
  fireEvent.change(getByLabelText(/email/i), {
    target: { value: "buyer@gmail.com" }
  });
  fireEvent.change(getByLabelText(/body/i), {
    target: { value: "Hey I really would like to trade" }
  });
  const message = "Bad times";
  makeFetchReturn({ status: 400 })({ message });
  fireEvent.submit(getByText("Send"));

  const failedMessage = await waitForElement(() => getByText(message));
  expect(failedMessage).toBeDefined();
});
