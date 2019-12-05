import { useEffect, useReducer } from "react";
import { apiFetch } from "../utils/fetchLight";
import useToggle from "./useToggle";

const loadingStates = {
  loading: "loading",
  done: "done",
  error: "error"
};

type State<T> = {
  loading: boolean,
  data: T | null,
  error: any | null
};

type DispatchObject<T> = {
  type: string,
  data?: T | null,
  error?: any | null
};

function loadingReducer<T>(
  currState: State<T>,
  { type, data = null, error = null }: DispatchObject<T>
): State<T> {
  switch (type) {
    case loadingStates.loading:
      return { loading: true, data: null, error: null };
    case loadingStates.done:
      return { loading: false, data, error: null };
    case loadingStates.error:
      return { loading: false, error, data: null };
    default:
      return currState;
  }
}

type UseApiReturns<T> = {
  ...State<T>,
  retry: () => mixed
};

function useApi<T>(url: string): UseApiReturns<T> {
  const [{ loading, data, error }, dispatch] = useReducer(loadingReducer, {
    loading: true,
    data: null,
    error: null
  });
  const [toggle, on, off] = useToggle(false);
  const retry = () => {
    if (toggle === true) {
      off();
    } else {
      on();
    }
  };

  useEffect(() => {
    let cancelled = false;
    dispatch({
      type: loadingStates.loading
    });
    apiFetch(url, "GET", {})
      .then(res => res.json())
      .then(json => {
        if (!cancelled)
          dispatch({
            type: loadingStates.done,
            data: json
          });
      })
      .catch(e => {
        if (!cancelled)
          dispatch({
            type: loadingStates.error,
            error: e
          });
      });

    return () => {
      cancelled = true;
    };
  }, [url, toggle]);

  return { loading, data, error, retry };
}

export default useApi;
