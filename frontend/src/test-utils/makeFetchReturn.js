import { curry } from "ramda";

const makeFetchReturn = (props: {}, toReturn: any | any[]) => {
  global.fetch = () =>
    Promise.resolve({
      json: () => Promise.resolve(toReturn),
      ...props
    });
};

// $FlowFixMe
export default curry(makeFetchReturn);
