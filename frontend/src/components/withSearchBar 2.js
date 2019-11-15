import React, { useState } from "react";
import type { ComponentType } from "react";
import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";
import Button from "@material-ui/core/Button";
import { navigate } from "@reach/router";
// $FlowFixMe
import styled from "styled-components";

const SearchBox = styled.div`
  margin-bottom: 2rem;
  padding: 1rem 2rem;
  display: grid;
  flex-flow: row nowrap;
  grid-template: "logo search" auto / 20% 80%;
  align-items: center;
`;

const SearchArea = styled.div`
  grid-area: search;
`;

const LogoArea = styled.div`
  grid-area: logo;
`;

const SearchForm = styled.form`
  border: 2px solid #3f51b5;
  border-radius: 8px;
  border-color: #707070;
  height: 50px;
  display: grid;
  grid-template-columns: 1fr auto;
  align-items: center;
  justify-content: space-between;
  & > button {
    align-self: stretch;
    width: 150px;
    background-color: #32a8b3;
  }
  & > div {
    padding-left: 1rem;
  }
`;

function withSearchBar<C: ComponentType<any>>(WrapComponent: C) {
  return (props: {}) => {
    const [search, setSearch] = useState("");
    return (
      <>
        <SearchBox>
          <LogoArea>
            <img src="https://via.placeholder.com/250x100" alt="logo" />
          </LogoArea>
          <SearchArea>
            <SearchForm
              method="POST"
              onSubmit={e => {
                e.preventDefault();
                navigate(`/search/${search}`);
              }}
            >
              <InputBase
                placeholder="Title, Author, ISBN..."
                onChange={e => setSearch(e.target.value)}
                inputProps={{ "aria-label": "search" }}
              />
              <Button
                type="submit"
                startIcon={<SearchIcon />}
                color="primary"
                variant="contained"
              >
                Search
              </Button>
            </SearchForm>
          </SearchArea>
        </SearchBox>
        <WrapComponent {...props} />
      </>
    );
  };
}

export default withSearchBar;
