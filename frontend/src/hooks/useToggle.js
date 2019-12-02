import { useState } from "react";

const useToggle = (startOpened: boolean = false) => {
  const [toggle, setToggle] = useState(startOpened);
  const open = () => setToggle(true);
  const close = () => setToggle(false);
  return [toggle, open, close];
};

export default useToggle;
