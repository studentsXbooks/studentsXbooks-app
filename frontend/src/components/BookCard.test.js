import React from "react";
import { render, fireEvent, wait, cleanup } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import BookCard from "./BookCard";
import makeFetchReturn from "../test-utils/makeFetchReturn";

afterEach(cleanup);

const validListing = {
  title: "the sound and the fury",
  isbn10: "0679732241",
  isbn13: "978-0679732242",
  description: "A novel written by the American author William Faulkner.",
  authors: "William Faulkner",
  thumbnail:
    "https://upload.wikimedia.org/wikipedia/en/thumb/e/e3/TheSoundAndTheFuryCover.jpg/220px-TheSoundAndTheFuryCover.jpg"
};

it("Renders with required props", () => {
  const { container } = render(
    <BookCard listing={validListing} onComplete={jest.fn()} />
  );
  expect(container).toBeDefined();
});
