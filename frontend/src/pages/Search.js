import React, { useState, useEffect } from "react";
import { Link } from "@reach/router";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography,
  Button,
  List,
  ListItem,
  Checkbox,
  FormControl,
  FormControlLabel,
  FormGroup,
  FormLabel,
  FormHelperText
} from "@material-ui/core";
import { ApiPost } from "../utils";
import useApi from "../hooks/useApi";

// Need paging, and need listing details page
const Search = ({ pageId = 1, term, navigate, location }) => {
  const urlParams = new URLSearchParams(location.search);
  const [page, setPage] = useState();
  const [MinP, setMinP] = useState(
    urlParams.has("MinPrice") ? urlParams.get("MinPrice") : null
  );
  const [MaxP, setMaxP] = useState(
    urlParams.has("MaxPrice") ? urlParams.get("MaxPrice") : null
  );
  const [loading, conditions] = useApi("conditions");
  const [selectedConditions, setSelectedConditions] = useState(
    urlParams.has("Conditions") ? urlParams.get("Conditions").split(",") : []
  );
  console.log({ selectedConditions });

  useEffect(() => {
    setSelectedConditions(
      urlParams.has("Conditions") ? urlParams.get("Conditions").split(",") : []
    );
  }, [location.search]);

  useEffect(() => {
    ApiPost(`listings/search/${term}/${pageId}`, true, {
      Conditions: selectedConditions,
      MinPrice: MinP,
      MaxPrice: MaxP
    })
      .then(res => res.json())
      .then(setPage);
  }, [MinP, MaxP, selectedConditions, pageId, term]);

  const conditionChange = e => {
    e.preventDefault();
    const { value, checked } = e.target;

    if (checked) {
      navigate(
        `/search-filter/${term}/${pageId}?MaxPrice=${MaxP}&MinPrice=${MinP}&Conditions=${selectedConditions.join(
          ","
        )},${value}`
      );
    } else {
      const newFilteredConditions = selectedConditions.filter(x => x !== value);
      navigate(
        `/search-filter/${term}/${pageId}?MaxPrice=${MaxP}&MinPrice=${MinP}&Conditions=${newFilteredConditions.join(
          ","
        )}`
      );
    }
  };

  const submitPriceRange = e => {
    e.preventDefault();
    navigate(
      `/search-filter/${term}/${pageId}?MaxPrice=${MaxP}&MinPrice=${MinP}&Conditions=${selectedConditions.join(
        ","
      )}`
    );
  };
  return (
    <div>
      <FormControl component="fieldset">
        <FormLabel component="legend">Condition</FormLabel>
        <FormGroup>
          {!loading &&
            conditions &&
            conditions.map(({ value, name }) => (
              <FormControlLabel
                control={<Checkbox value={value} onChange={conditionChange} />}
                label={name}
              />
            ))}
        </FormGroup>
        <FormHelperText>Be careful</FormHelperText>
      </FormControl>
      <label>Price Range</label>
      <form method="POST" onSubmit={submitPriceRange}>
        <input
          type="text"
          placeholder="Min"
          onChange={e => setMinP(e.target.value)}
        ></input>
        <input
          type="text"
          placeholder="Max"
          onChange={e => setMaxP(e.target.value)}
        ></input>
        <button type="submit">Go</button>
      </form>
      <Grid container spacing={3}>
        {page &&
          page.data &&
          page.data.map(listing => (
            <ListingCard listing={listing} key={listing.id} />
          ))}
        <List>
          <ListItem>
            <Button
              onClick={() => navigate(`/search/${term}/${Number(pageId) - 1} `)}
              disabled={page && !page.hasPrev}
            >
              Prev
            </Button>
          </ListItem>
          <ListItem>
            <Typography>{page && page.currentPage}</Typography>
          </ListItem>
          <ListItem>
            <Button
              onClick={() => navigate(`/search/${term}/${Number(pageId) + 1} `)}
              disabled={page && !page.hasNext}
            >
              Next
            </Button>
          </ListItem>
        </List>
      </Grid>
    </div>
  );
};

const ListingCard = ({ listing: { title, description, price, id } }) => (
  <Grid item xs={12} sm={6} md={3}>
    <Link to={`/listing/${id}`}>
      <Card raised>
        <CardHeader
          title={
            <Typography variant="h3">
              {title} {price}
            </Typography>
          }
        />
        <CardContent>{description}</CardContent>
      </Card>
    </Link>
  </Grid>
);

export default Search;
