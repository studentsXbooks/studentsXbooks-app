import BaseUrl from "./BaseUrl";

async function ApiPost(url: string, creds: boolean, json) {
  return await fetch(BaseUrl + url, {
    method: "POST",
    credentials: creds ? "include" : "omit",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(json)
  });
}
export default ApiPost;
