import React from "react";
import { render, waitForElement, cleanup } from "@testing-library/react";
import { Router, navigate } from "@reach/router";
import ConfirmEmail from "./ConfirmEmail";
import makeFetchReturn from "../test-utils/makeFetchReturn";

const backendURL = process.env.REACT_APP_BACKEND || "";

afterEach(cleanup);

test("Renders with required props", () => {
  const { container } = render(<ConfirmEmail term="" />);
  expect(container).toBeDefined();
});

test("Calls api to confirm account when rendered, display loading during call, then displays account confirm fetch return", async () => {
  const fakeFetch = makeFetchReturn({ status: 200 })({ accountConfirm: true });
  const id = 13456;
  const code = 1654987961324;
  navigate(`/confirm-account?id=${id}&code=${code}`);
  const { getByText } = render(
    <Router>
      <ConfirmEmail path="*" term="" />
    </Router>
  );

  expect(getByText(/confirming/i)).toBeDefined();
  const successMessage = waitForElement(() => getByText(/confirmed/i));
  expect(successMessage).toBeDefined();
  expect(fakeFetch).toHaveBeenCalledTimes(1);
  expect(fakeFetch).toHaveBeenCalledWith(
    `${backendURL}users/confirm-email?id=${id}&code=${code}`,
    expect.anything()
  );
});

test("Calls api to confirm account when rendered, fetch return non 2XX, displays failure text to user", async () => {
  const message = "failure";
  makeFetchReturn({ status: 400 })({
    message
  });
  const id = 13456;
  const code = 1654987961324;
  navigate(`/confirm-account?id=${id}&code=${code}`);
  const { getByText } = render(
    <Router>
      <ConfirmEmail path="*" term="" />
    </Router>
  );

  const failureMessage = await waitForElement(() => getByText(/error/i));
  expect(failureMessage).toBeDefined();
  const fetchMessage = await waitForElement(() => getByText(message));
  expect(fetchMessage).toBeDefined();
});
