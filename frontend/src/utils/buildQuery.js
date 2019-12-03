const buildQuery = (filterObj: {}) => {
  const keys = Object.keys(filterObj);
  return keys
    .filter(x => filterObj[x] !== "")
    .reduce((acc, key) => `${acc}${key}=${filterObj[key]}&`, "?")
    .slice(0, -1);
};

export default buildQuery;
