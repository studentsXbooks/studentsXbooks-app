import BaseUrl from "./BaseUrl";

// TODO: Should this be stateless?
function ApiPost(url: string, creds: boolean, json) {
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
