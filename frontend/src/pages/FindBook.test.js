import React from "react";
import { render, fireEvent, wait, cleanup } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import FindBook from "./FindBook";
import makeFetchReturn from "../test-utils/makeFetchReturn";

const fakeBooks = [
  {
    title: "New book",
    description: "a new book",
    isbn10: "2342342342",
    isbn13: "978-2342342342",
    thumbnail: "",
    authors: "Book B. Author"
  },
  {
    title: "Old book",
    description: "an old book",
    isbn10: "23333342342",
    isbn13: "978-2333342342342",
    thumbnail: "",
    authors: "Book C. Author"
  }
];

const customFetchReturn = makeFetchReturn({});
customFetchReturn(fakeBooks);

afterEach(cleanup);

it("Renders when passed required props", () => {
  const { container } = render(
    <FindBook
      navigate={jest.fn()}
      location={{ search: "" }}
      term="myBook"
      pageId="1"
    />
  );
  expect(container).toBeDefined();
});

it("Enter Valid Search Term and Click submit", async () => {
  const wrappedFakeNavigate = jest.fn();
  const term = "mybook";

  const { getByLabelText, getByText } = render(
    <FindBook
      navigate={wrappedFakeNavigate}
      location={{ search: "" }}
      term={term}
      pageId="1"
    />
  );

  fireEvent.change(getByLabelText(/Search/), { target: { value: term } });

  fireEvent.submit(getByText(/Submit/));

  await wait(() =>
    expect(wrappedFakeNavigate).toHaveBeenCalledWith(
      `/listing/findbook/${term}`
    )
  );
});
