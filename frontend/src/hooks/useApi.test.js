import { renderHook } from "@testing-library/react-hooks";
import useApi from "./useApi";
import makeFetchReturn from "../test-utils/makeFetchReturn";
import { FailedRequestError } from "../utils/fetchLight";

// Excludes properties passed in
const everythingExcept = (excludeProps: [string]) => (a: {}) => {
  const arrayObject = Object.entries(a);
  return Object.fromEntries(
    arrayObject.filter(([key]) => excludeProps.some(t => t !== key))
  );
};

test("data = null, errors = null, and loading is true when first rendered", () => {
  const { result } = renderHook(() => useApi("/api/conditions"));

  const { loading, data, error } = result.current;
  expect(loading).toBe(true);
  expect(data).toBe(null);
  expect(error).toBe(null);
});

test("Fetch GET is called with url and sets json response to data and loading becomes false", async () => {
  const fakeData: {} = { title: "Death Stranding" };
  makeFetchReturn({ status: 200 })(fakeData);
  const { result, waitForNextUpdate } = renderHook(() =>
    useApi("/api/conditions")
  );

  await waitForNextUpdate();

  const { loading, data, error } = result.current;
  expect(loading).toBe(false);
  expect(data).toEqual(fakeData);
  expect(error).toBe(null);
});

test("Fetch Returns Bad Response, Loading is False, Data is Null, and error is Response", async () => {
  const fakeData: {} = { message: "Not a valid ISBN" };
  makeFetchReturn({ status: 404 })(fakeData);
  const { result, waitForNextUpdate } = renderHook(() =>
    useApi("/api/conditions")
  );

  await waitForNextUpdate();

  const { loading, error, data } = result.current;
  expect(loading).toBe(false);
  expect(data).toBeNull();
  expect(error).toBeInstanceOf(FailedRequestError);
});

test("Fetch return OK response, call retry, expect loading=true with error and data null, then next update loading = false with data and error = null", async () => {
  const groundhogDay: {} = { title: "Bill Freakin Murray" };
  makeFetchReturn({ status: 200 })(groundhogDay);
  const { result, waitForNextUpdate } = renderHook(() =>
    useApi("/api/conditions")
  );
  const excludeRetry = everythingExcept(["retry"]);

  await waitForNextUpdate();

  expect(excludeRetry(result.current)).toEqual({
    loading: false,
    data: groundhogDay,
    error: null
  });

  const zombieLand: {} = { title: "ZombieLand" };
  makeFetchReturn({ status: 200 })(zombieLand);

  result.current.retry();

  await waitForNextUpdate();

  expect(excludeRetry(result.current)).toEqual({
    loading: false,
    data: zombieLand,
    error: null
  });
});
