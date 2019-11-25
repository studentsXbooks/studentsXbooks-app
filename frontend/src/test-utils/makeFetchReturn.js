import { curry } from "ramda";

const makeFetchReturn = (props: { status: string }, toReturn: any) => {
  global.fetch = () =>
    Promise.resolve({
      json: () => Promise.resolve(toReturn),
      ...props
    });
};

// $FlowFixMe
export default curry(makeFetchReturn);
