import { curry } from "ramda";

const makeFetchReturn = (props: { status: string }, toReturn: {} | []) => {
  global.fetch = () =>
    Promise.resolve({
      json: () => Promise.resolve(toReturn),
      ...props
    });
};

// $FlowFixMe
export default curry(makeFetchReturn);
