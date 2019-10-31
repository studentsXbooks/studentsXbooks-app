import React from "react";
import { render, fireEvent, wait, cleanup } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import ContactSellerForm from "./ContactSellerForm";
import makeFetchReturn from "../test-utils/makeFetchReturn";

afterEach(cleanup);

const validListing = {
  contactOption: 0
};

const badListing = {
  contactOption: -1
};

it("Renders with required props", () => {
  const { container } = render(
    <ContactSellerForm listing={validListing} onComplete={jest.fn()} />
  );
  expect(container).toBeDefined();
});

it("Empty Message and Subject, Send is disabled", () => {
  const { getByText } = render(
    <ContactSellerForm listing={validListing} onComplete={jest.fn()} />
  );
  const sendButton = getByText(/Send/);
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
  fireEvent.change(getByLabelText(/subject/i), {
    target: { value: "Trade with Me <3" }
  });
  fireEvent.change(getByLabelText(/body/i), {
    target: { value: "Hey I really would like to trade" }
  });
  fireEvent.submit(getByText(/Send/));
  await wait(() => {
    expect(mockComplete).toBeCalledTimes(1);
  });
});
