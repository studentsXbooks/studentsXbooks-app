import { render, fireEvent, wait } from "@testing-library/react";
import React from "react";
import { Router, navigate } from "@reach/router";
import SearchFilterForm from "./SearchFilterForm";
import makeFetchReturn from "../test-utils/makeFetchReturn";

const fakeConditions = [
  { name: "Like New", value: 1 },
  { name: "Old and Dirty", value: -1 }
];

const customFetchReturn = makeFetchReturn({});
customFetchReturn(fakeConditions);

it("Renders when passed required props", async () => {
  const { container } = render(
    <SearchFilterForm
      basePath="/search"
      navigate={jest.fn()}
      location={{ search: "" }}
    />
  );
  await wait(() => expect(container).toBeDefined());
});

it("Enter Valid Min and Max and Click submit, calls navigate with query params", async () => {
  const wrappedFakeNavigate = jest.fn();
  const min = 0;
  const max = 20;
  const { getByLabelText, getByText } = render(
    <SearchFilterForm
      basePath="/search/1"
      navigate={wrappedFakeNavigate}
      location={{ search: "" }}
    />
  );
  fireEvent.change(getByLabelText(/Min/), { target: { value: min } });
  fireEvent.change(getByLabelText(/Max/), { target: { value: max } });

  fireEvent.submit(getByText(/Submit/));

  await wait(() =>
    expect(wrappedFakeNavigate).toHaveBeenCalledWith(
      `/search/1?min=${min}&max=${max}`
    )
  );
});

it("Loads in conditions after being rendered", async () => {
  customFetchReturn([{ name: "Like New", value: 1 }]);
  const wrappedFakeNavigate = jest.fn();
  const { findByLabelText } = render(
    <SearchFilterForm
      basePath="/search/1"
      navigate={wrappedFakeNavigate}
      location={{ search: "" }}
    />
  );

  const condition = await findByLabelText("Like New");
  expect(condition).toBeDefined();
});

it("Clicking on a conditions calls navigate", async () => {
  const wrappedFakeNavigate = jest.fn();
  const { findByLabelText } = render(
    <SearchFilterForm
      basePath="/search/1"
      navigate={wrappedFakeNavigate}
      location={{ search: "" }}
    />
  );
  const condition = await findByLabelText("Like New");

  fireEvent.click(condition);

  expect(wrappedFakeNavigate).toHaveBeenCalledWith(
    `/search/1?conditions=${fakeConditions[0].value}`
  );
});

it("Selecting Multiple Conditions calls navigate with all in query params", async () => {
  const [cond1, cond2] = fakeConditions;
  customFetchReturn(fakeConditions);
  const { findByLabelText } = render(
    <Router>
      {/* $FlowFixMe */}
      <SearchFilterForm basePath="/search/1" path="/*" />
    </Router>
  );
  const condition = await findByLabelText(cond1.name);
  fireEvent.click(condition);
  expect(window.location.search).toEqual(`?conditions=${cond1.value}`);
  await wait(() => expect(condition).toBeChecked());

  const condition2 = await findByLabelText(cond2.name);
  fireEvent.click(condition2);
  expect(window.location.search).toEqual(
    `?conditions=${cond1.value},${cond2.value}`
  );
  await wait(() => expect(condition).toBeChecked());
  await wait(() => expect(condition2).toBeChecked());
});

it("Setting Min and Max then selecting condition calls navigate with all queries", async () => {
  const min = 0;
  const max = 20;
  const [cond1] = fakeConditions;
  navigate("/");
  customFetchReturn(fakeConditions);
  const { getByLabelText, getByText, findByLabelText } = render(
    <Router>
      {/* $FlowFixMe */}
      <SearchFilterForm basePath="/search/1" default />
    </Router>
  );
  fireEvent.change(getByLabelText(/Min/), { target: { value: min } });
  fireEvent.change(getByLabelText(/Max/), { target: { value: max } });

  fireEvent.submit(getByText(/Submit/));
  await wait(() =>
    expect(global.location.search).toEqual(`?min=${min}&max=${max}`)
  );

  const condition = await findByLabelText(cond1.name);
  expect(condition.checked).toEqual(false);
  fireEvent.click(condition);
  await wait(() => expect(condition.checked).toEqual(true));

  await wait(() =>
    expect(global.location.search).toEqual(
      `?min=${min}&max=${max}&conditions=${cond1.value}`
    )
  );

  fireEvent.change(getByLabelText(/Max/), { target: { value: 30 } });
  fireEvent.submit(getByText(/Submit/));

  await wait(() =>
    expect(global.location.search).toEqual(
      `?min=${min}&max=${30}&conditions=${cond1.value}`
    )
  );
});

it("Select Condition then unselect condition removes from query", async () => {
  const [cond1] = fakeConditions;
  navigate("/");

  customFetchReturn(fakeConditions);
  const { findByLabelText } = render(
    <Router>
      {/* $FlowFixMe */}
      <SearchFilterForm basePath="/search/1" default />
    </Router>
  );

  const condition = await findByLabelText(cond1.name);
  expect(condition.checked).toEqual(false);
  fireEvent.click(condition);
  await wait(() => expect(condition.checked).toEqual(true));

  fireEvent.click(condition);
  await wait(() => expect(condition.checked).toEqual(false));
  await wait(() => expect(global.location.search).toEqual(""));
});

it("Renders with minPrice and maxPrice inputs set to query params", async () => {
  const max = 50;
  const min = 30;
  navigate(`/?min=${min}&max=${max}`);
  customFetchReturn(fakeConditions);
  const { getByLabelText } = render(
    <Router>
      {/* $FlowFixMe */}
      <SearchFilterForm basePath="/search/1" default />
    </Router>
  );
  await wait(() => {
    expect(getByLabelText(/Min/).value).toEqual(min.toString());
    expect(getByLabelText(/Max/).value).toEqual(max.toString());
  });
});
