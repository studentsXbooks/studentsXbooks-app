import { useEffect, useReducer } from "react";
import { ApiGet } from "../utils";

const loadingStates = {
  loading: "loading",
  done: "done",
  error: "error"
};

const loadingReducer = (currState, { type, data, error }) => {
  switch (type) {
    case loadingStates.loading:
      return { loading: true };
    case loadingStates.done:
      return { loading: false, data };
    case loadingStates.error:
      return { loading: false, error };
    default:
      return currState;
  }
};

const useApi = url => {
  const [{ loading, data, error }, dispatch] = useReducer(loadingReducer, {
    loading: false
  });
  useEffect(() => {
    let cancelled = false;
    dispatch({ type: loadingStates.loading });
    ApiGet(url)
      .then(json => {
        if (!cancelled) dispatch({ type: loadingStates.done, data: json });
      })
      .catch(e => {
        if (!cancelled) dispatch({ type: loadingStates.error, error: e });
      });

    return () => {
      cancelled = true;
    };
  }, [url]);

  return [loading, data, error];
};

export default useApi;
