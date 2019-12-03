import React from "react";
import { Router, navigate } from "@reach/router";
import {
  render,
  fireEvent,
  wait,
  cleanup,
  waitForElement
} from "@testing-library/react";
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

const customFetchReturn = makeFetchReturn({ status: 200 });

afterEach(cleanup);

it("Renders when passed required props", () => {
  customFetchReturn(fakeBooks);

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
  customFetchReturn(fakeBooks);
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

it("Pass in term, call api, show book", async () => {
  customFetchReturn({ data: fakeBooks });

  const { getByLabelText, getByText } = render(
    <Router>
      <FindBook navigate={navigate} location={{ search: "" }} default />
      <FindBook
        navigate={navigate}
        location={{ search: "" }}
        path="/listing/findbook/:term"
      />
    </Router>
  );

  fireEvent.change(getByLabelText(/Search/), { target: { value: "Book" } });

  fireEvent.submit(getByText(/submit/i));

  const book = await waitForElement(() => getByText(/New book/));
  expect(book).toBeDefined();
});

it("Passed in page and term, renders with current page shown", async () => {
  customFetchReturn({ currentPage: 2, totalPages: 2, data: fakeBooks });
  const wrappedFakeNavigate = jest.fn();
  const term = "mybook";

  const { getAllByText } = render(
    <FindBook
      navigate={wrappedFakeNavigate}
      location={{ search: "" }}
      term={term}
      pageId="2"
    />
  );

  await wait(() => {
    const page = getAllByText(/2 of 2/);
    expect(page).toBeDefined();
    expect(page).toHaveLength(2);
  });
});
