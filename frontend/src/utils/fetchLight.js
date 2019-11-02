const BaseUrl: string = process.env.REACT_APP_BACKEND || "";

type Response = {
  status: number
};

type Method = "POST" | "GET" | "PUT" | "DELETE";

class FailedRequestError extends Error {
  response: Response;

  constructor(response: Response, ...params: any) {
    super(...params);
    if (Error.captureStackTrace) {
      Error.captureStackTrace(this, FailedRequestError);
    }
    this.name = "FailedRequestError";
    this.response = response;
    this.message = `Failed Request | status code: ${response.status}`;
  }
}

/*
  Custom Fetch Wrapper, throws FailedRequestError if status code != 2##
*/
const fetchLight = (url: string, method: Method, json: Object) =>
  fetch(url, {
    method,
    credentials: "include",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(json)
  }).then(res => {
    if (res.status < 200 || res.status >= 300) {
      return Promise.reject(new FailedRequestError(res));
    }
    return res;
  });

/*
  Wrapper around fetchLight to use our Backend
*/
const apiFetch = (url: string, method: Method, json: Object) =>
  fetchLight(BaseUrl + url, method, json);

export { FailedRequestError, apiFetch };
export default fetchLight;
