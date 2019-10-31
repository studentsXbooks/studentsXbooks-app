const BaseUrl: string = process.env.REACT_APP_BACKEND || "";

function ApiPost(url: string, creds: boolean, json: Object) {
  return fetch(BaseUrl + url, {
    method: "POST",
    credentials: creds ? "include" : "omit",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(json)
  }).then(res => {
    if (res.status <= 200 || res.status >= 299)
      throw Error(`Response of ${res.status}`);
    return res;
  });
}
export default ApiPost;
