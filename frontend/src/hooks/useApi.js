import { useEffect, useReducer } from "react";
import { ApiGet } from "../utils";

const loadingStates = {
  loading: "loading",
  done: "done",
  error: "error"
};

type State<T> = {
  loading: boolean,
  data: T | null,
  error: string | null
};

type DispatchObject<T> = {
  type: string,
  data?: T | null,
  error?: string | null
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

function useApi<T>(url: string): State<T> {
  const [{ loading, data, error }, dispatch] = useReducer(loadingReducer, {
    loading: true,
    data: null,
    error: null
  });

  useEffect(() => {
    let cancelled = false;
    dispatch({
      type: loadingStates.loading
    });
    ApiGet(url, true)
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
            error: e.message
          });
      });

    return () => {
      cancelled = true;
    };
  }, [url]);

  return { loading, data, error };
}

export default useApi;
