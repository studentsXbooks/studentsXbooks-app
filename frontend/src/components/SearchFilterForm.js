import React, { useState, useEffect } from "react";
import {
  TextField,
  FormControl,
  FormLabel,
  FormGroup,
  FormControlLabel,
  Checkbox
} from "@material-ui/core";
import useApi from "../hooks/useApi";

type Props = {
  basePath: string,
  navigate: string => any,
  location: { search: string }
};

const extractConditions = queries => {
  const conditions = new URLSearchParams(queries).get("conditions");
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
  const [loading, conditions] = useApi("Conditions");
  const [min, setMin] = useState("");
  const [max, setMax] = useState("");
  const [selectedConditions, setSelectedConditions] = useState(
    extractConditions(location.search)
  );

  useEffect(() => {
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
      <form method="POST" onSubmit={submitPriceRange}>
        <TextField
          type="number"
          label="Min"
          id="min"
          value={min}
          onChange={e => setMin(e.target.value)}
        />
        <TextField
          type="number"
          label="Max"
          id="max"
          value={max}
          onChange={e => setMax(e.target.value)}
        />
        <button type="submit">Submit</button>
      </form>
      <form method="POST">
        <FormControl component="fieldset">
          <FormLabel component="legend">Conditions</FormLabel>
          <FormGroup>
            {!loading &&
              conditions &&
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
    </div>
  );
};

export default SearchFilterForm;
