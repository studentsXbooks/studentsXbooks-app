import React from "react";
import { render, fireEvent, wait, cleanup } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import BookCard from "./BookCard";
import makeFetchReturn from "../test-utils/makeFetchReturn";

afterEach(cleanup);

const validListing = {
  title: "sound",
  isbn10: "0679732241",
  isbn13: "978-0679732242",
  description: "novel",
  authors: "faulkner",
  thumbnail: "url"
};

it("Renders with required props", () => {
  const { container } = render(
    <BookCard listing={validListing} onComplete={jest.fn()} />
  );
  expect(container).toBeDefined();
});

it("Onclick navigate to /create with appropriate queries", async () => {
  const { getByText } = render(
    <BookCard listing={validListing} onComplete={jest.fn()} />
  );

  fireEvent.click(getByText("sound"));

  const url = `?title=sound&isbn10=0679732241&isbn13=978-0679732242&thumbnail=url&description=novel&authors=faulkner`;
  await wait(() => {
    expect(global.location.search).toBe(url);
  });
});
