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
import styled from "styled-components";
import useApi from "../hooks/useApi";
import buildQuery from "../utils/buildQuery";

const SearchFilterLayout = styled.div`
  display: grid;
  grid-template-columns: 20% 20% 20% 20% 20%;
  grid-template-rows: 20% 20% 20% 20% 20%;
  grid-row-gap: 65px;
  justify-items: center;
  align-items: center;
`;

const SubmitPosition = styled.div`
  display: grid;
  grid-row-start: 1;
  grid-column-end: 5;
  align-items: center;
`;

const ConditionCheckBoxForm = styled.form`
  label[id="condition-0"] {
    border-bottom: solid 3px #07e000;
  }

  label[id="condition-1"] {
    border-bottom: solid 3px #a6ff00;
  }

  label[id="condition-2"] {
    border-bottom: solid 3px #ffbf00;
  }

  label[id="condition-3"] {
    border-bottom: solid 3px #cc3703;
  }

  label[id="condition-4"] {
    border-bottom: solid 3px #ff1a00;
  }
`;

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

const SearchFilterForm = ({ basePath, navigate, location }: Props) => {
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
      <ConditionCheckBoxForm method="POST">
        <FormControl component="fieldset">
          <FormLabel component="legend">Conditions</FormLabel>
          <FormGroup style={{ width: "100px", paddingLeft: "13px" }}>
            {!loading &&
              conditions &&
              Array.isArray(conditions) &&
              conditions.map(({ value, name }) => (
                <FormControlLabel
                  key={name}
                  control={
                    <Checkbox
                      value={value.toString()}
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
      </ConditionCheckBoxForm>
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
          <SearchFilterLayout>
            <SubmitPosition>
              <Grid item xs={12} md={4}>
                <Button
                  type="submit"
                  color="primary"
                  fullWidth
                  variant="contained"
                >
                  Submit
                </Button>
              </Grid>
            </SubmitPosition>
          </SearchFilterLayout>
        </Grid>
      </form>
    </div>
  );
};

export default SearchFilterForm;
