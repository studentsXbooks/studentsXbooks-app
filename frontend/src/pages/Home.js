// @flow

import React, { useState } from "react";
// $FlowFixMe
import styled from "styled-components";
import type { ComponentType } from "react";
import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";
import Button from "@material-ui/core/Button";
import { navigate } from "@reach/router";

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
  position: absolute;
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
  position: absolute;
  & > button {
    align-self: stretch;
    width: 150px;
    background-color: #33578c;
    position: absolute;
  }
  & > div {
    padding-left: 1rem;
  }
`;
const BkgOverlay = styled.div`
  background-color: rgba(0, 0, 0, 0.5);
  height: 100%;
`;
const MainContent = styled.div`
  background-image: url("./BookShelf2.JPG");
  background-size: 100vw;
  background-repeat: no-repeat;
  height: 90.3vh;
`;

const Home = () => {
  const [search, setSearch] = useState("");

  return (
    <MainContent>
      <BkgOverlay>
        <SearchBox>
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
      </BkgOverlay>
    </MainContent>
  );
};

export default Home;

// style={{ backgroundImage: "url(${./BookShelf.JPG})" }}
