import React from "react";
import { render, fireEvent, wait, cleanup } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import FindBook from "./FindBook";
import makeFetchReturn from "../test-utils/makeFetchReturn";

const validSearch = [{ term: "book", value: 1 }, { pageId: "1", value: -1 }];

const customFetchReturn = makeFetchReturn({});
customFetchReturn(validSearch);

afterEach(cleanup);

it("Renders when passed required props", () => {
  const { container } = render(
    <FindBook navigate={jest.fn()} location={{ search: "" }} />
  );
  expect(container).toBeDefined();
});

it("Enter Valid Search Term and Click submit", async () => {
  const wrappedFakeNavigate = jest.fn();
  const term = "mybook";

  const { getByLabelText, getByText } = render(
    <FindBook navigate={wrappedFakeNavigate} location={{ search: "" }} />
  );

  fireEvent.change(getByLabelText(/Search/), { target: { value: term } });

  fireEvent.submit(getByText(/Submit/));

  await wait(() =>
    expect(wrappedFakeNavigate).toHaveBeenCalledWith(
      `/listing/findbook/${term}`
    )
  );
});
