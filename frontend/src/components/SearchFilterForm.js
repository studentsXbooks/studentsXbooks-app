import React, { useState, useEffect } from "react";
import {
  TextField,
  FormControl,
  FormLabel,
  FormGroup,
  FormControlLabel,
  Checkbox,
  Grid,
  Button
} from "@material-ui/core";
import useApi from "../hooks/useApi";

type Props = {
  basePath: string,
  navigate: string => any,
  location: { search: string }
};

const getQuery = queries => key => new URLSearchParams(queries).get(key) || "";

const extractConditions = queries => {
  const conditions = getQuery(queries)("conditions");
  return conditions ? conditions.split(",") : [];
};

const buildQuery = (filterObj: {}) => {
  const keys = Object.keys(filterObj);
  return keys
    .filter(x => filterObj[x] !== "")
    .reduce((acc, key) => `${acc}${key}=${filterObj[key]}&`, "?")
    .slice(0, -1);
};

const SearchFilterForm = ({ basePath, navigate, location }: Props) => {
  // prettier-ignore
  const { loading, data: conditions } = useApi("Conditions");
  const [min, setMin] = useState(getQuery(location.search)("min"));
  const [max, setMax] = useState(getQuery(location.search)("max"));
  const [selectedConditions, setSelectedConditions] = useState(
    extractConditions(location.search)
  );

  useEffect(() => {
    const getFromSearch = getQuery(location.search);
    setMin(getFromSearch("min"));
    setMax(getFromSearch("max"));
    setSelectedConditions(extractConditions(location.search));
  }, [location.search]);

  const submitPriceRange = e => {
    e.preventDefault();
    navigate(
      `${basePath}${buildQuery({
        min,
        max,
        conditions: selectedConditions.filter(x => x !== "").join(",")
      })}`
    );
  };

  const handleCondition = e => {
    const { value, checked } = e.target;
    const newConditions = [...selectedConditions, value]
      .filter(x => checked || x !== value)
      .filter(x => x !== "")
      .join(",");
    navigate(
      `${basePath}${buildQuery({ min, max, conditions: newConditions })}`
    );
  };

  return (
    <div>
      <form method="POST">
        <FormControl component="fieldset">
          <FormLabel component="legend">Conditions</FormLabel>
          <FormGroup>
            {!loading &&
              conditions &&
              Array.isArray(conditions) &&
              conditions.map(({ value, name }) => (
                <FormControlLabel
                  key={name}
                  control={
                    <Checkbox
                      value={value}
                      checked={selectedConditions.some(
                        x => x.toString() === value.toString()
                      )}
                      inputProps={{ "aria-label": `condition-${value}` }}
                    />
                  }
                  onChange={handleCondition}
                  label={name}
                  id={`condition-${value}`}
                />
              ))}
          </FormGroup>
        </FormControl>
      </form>
      <br />
      <form method="POST" onSubmit={submitPriceRange}>
        <Grid container>
          <Grid item xs={12} md={4}>
            <TextField
              type="number"
              label="Min"
              id="min"
              value={min}
              onChange={e => setMin(e.target.value)}
            />
          </Grid>
          <Grid item xs={12} md={4}>
            <TextField
              type="number"
              label="Max"
              id="max"
              value={max}
              onChange={e => setMax(e.target.value)}
            />
          </Grid>
          <Grid item xs={12} md={4}>
            <Button type="submit" color="primary" fullWidth variant="contained">
              Submit
            </Button>
          </Grid>
        </Grid>
      </form>
    </div>
  );
};

export default SearchFilterForm;
