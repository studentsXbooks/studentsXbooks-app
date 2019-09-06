import BaseUrl from "./BaseUrl";

const ApiGet = (url: string, creds: boolean) => {
  fetch(BaseUrl + url, {
    method: "GET",
    credentials: creds ? "include" : "omit"
  }).then(res => res.json());
};

export default ApiGet;
