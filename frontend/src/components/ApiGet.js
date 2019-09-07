import BaseUrl from "./BaseUrl";

function ApiGet(url: string, creds: boolean) {
  return fetch(BaseUrl + url, {
    method: "GET",
    credentials: creds ? "include" : "omit"
  }).then(res => res.json());
}

export default ApiGet;
