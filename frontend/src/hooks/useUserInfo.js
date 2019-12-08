import { useState, useEffect } from "react";
import useApi from "./useApi";

const useLocation = () => {
  const [location, setLocation] = useState({});

  useEffect(() => {
    setLocation(window.location);
  }, []);
  return { location };
};

const useUserInfo = () => {
  const { loading, data: userInfo, error, retry } = useApi("users/info");
  const { location } = useLocation();

  useEffect(() => {
    if (userInfo == null) retry();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location.href, userInfo]);

  return { loading, userInfo, error };
};

export default useUserInfo;
