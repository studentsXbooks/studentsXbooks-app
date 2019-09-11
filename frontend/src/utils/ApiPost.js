// @flow

const BaseUrl = process.env.REACT_APP_BACKEND;

// TODO: Should this be stateless?
function ApiPost(url, creds, json) {
  return fetch(BaseUrl + url, {
    method: "POST",
    credentials: creds ? "include" : "omit",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(json)
  });
}
export default ApiPost;
