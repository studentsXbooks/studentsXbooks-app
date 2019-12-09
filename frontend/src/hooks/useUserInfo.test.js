import React from "react";
import { renderHook } from "@testing-library/react-hooks";
import { render, waitForElement } from "@testing-library/react";
import { navigate, Router } from "@reach/router";
import useUserInfo from "./useUserInfo";
import makeFetchReturn from "../test-utils/makeFetchReturn";

const FakeInfoComponent = ({ children }) => {
  const { loading, userInfo } = useUserInfo();
  let name = <div>Not Logged In</div>;

  if (userInfo) {
    name = <div>{userInfo.name}</div>;
  }
  if (loading) {
    name = <div>loading...</div>;
  }

  return (
    <div>
      {name}
      {children}
    </div>
  );
};

const FakePage = () => {
  return <div>Awesome</div>;
};

test("userInfo = null, errors = null, and loading is true when first rendered", () => {
  const { result } = renderHook(() => useUserInfo());

  const { loading, userInfo, error } = result.current;
  expect(loading).toBe(true);
  expect(userInfo).toBe(null);
  expect(error).toBe(null);
});

test("userInfo is on next update", async () => {
  makeFetchReturn({ status: 200 })({ name: "Billy Bob" });
  const { result, waitForNextUpdate } = renderHook(() => useUserInfo());

  await waitForNextUpdate();

  const { userInfo } = result.current;
  expect(userInfo).toBeDefined();
});

test("Should retry getting userInfo when changing pages", async () => {
  makeFetchReturn({ status: 400 })({});
  const { getByText } = render(
    <Router>
      <FakeInfoComponent path="/">
        <FakePage path="/*" />
      </FakeInfoComponent>
    </Router>
  );

  expect(getByText(/loading/i)).toBeDefined();
  await waitForElement(() => getByText(/Not Logged In/i));
  makeFetchReturn({ status: 200 })({ name: "Bob Billy" });
  navigate("/listings");
  const userInfo = await waitForElement(() => getByText(/Bob/i));
  expect(userInfo).toBeDefined();
  navigate("/search/harry");
  const userInfoAfterNavigate = getByText(/Bob/i);
  expect(userInfoAfterNavigate).toBeDefined();
});
