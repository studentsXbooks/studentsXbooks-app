import { useEffect, useReducer } from "react";
import { ApiGet } from "../utils";

const loadingStates = {
  loading: "loading",
  done: "done",
  error: "error"
};

type State = {
  loading: boolean,
  data?: {} | [] | null,
  error?: {} | null
};

type DispatchObject = {
  type: string,
  data?: {} | [] | null,
  error?: {} | null
};

const loadingReducer = (
  currState: State,
  { type, data, error }: DispatchObject
) => {
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

const useApi = (url: string) => {
  const [{ loading, data, error }: State, dispatch] = useReducer(
    loadingReducer,
    {
      loading: true,
      data: null,
      error: null
    }
  );
  useEffect(() => {
    let cancelled = false;
    dispatch({ type: loadingStates.loading });
    ApiGet(url, true)
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
