// @flow

import React, { useState } from "react";
// $FlowFixMe
import styled from "styled-components";
import type { ComponentType } from "react";
import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";
import Button from "@material-ui/core/Button";
import { navigate } from "@reach/router";
import { TextField } from "@material-ui/core";
import { makeStyles } from "@material-ui/core/styles";
import { MuiThemeProvider, createMuiTheme } from "@material-ui/core";

const SearchBox = styled.div`
  margin-bottom: 2rem;
  padding: 1rem 2rem;
  display: grid;
  flex-flow: row nowrap;
  grid-template: "logo search" auto / 20% 80%;
  align-items: center;
  color: white;
`;

const SearchArea = styled.div`
  grid-area: search;
  position: absolute;
  width: 500px;
  color: white;
`;

const LogoArea = styled.div`
  grid-area: logo;
`;

const SearchLayout = styled.div`
  display: grid;
  /* grid-template-rows: auto;
  grid-template-columns: 50% 50%; */
  grid-template-columns: 20% 20% 20% 20% 20%;
  grid-template-rows: 20% 20% 20% 20% 20%;
  grid-row-gap: 60px;
  justify-items: center;
  align-items: center;
`;

const SearchPosition = styled.div`
  display: grid;
  grid-row-start: 3;
  grid-column-end: 3;
  grid-gap: -10px;
  align-items: center;
`;

const LogoPosition = styled.div`
  display: grid;
  grid-row-start: 2;
  grid-column-end: 4;
  grid-gap: -10px;
  align-items: center;
`;

const SearchForm = styled.form`
  border: 2px solid #3f51b5;
  border-radius: 25px;
  border-color: white;
  height: 43px;
  width: 500px;
  display: grid;
  grid-template-columns: 1fr auto;
  align-items: center;
  justify-content: space-between;
  position: absolute;
  color: white;
  position: relative;
  z-index: 2;
  background: white;

  & > button {
    align-self: stretch;
    width: 150px;
    border-radius: 25px;
    background-color: #33578c;
  }
  & > div {
    padding-left: 1rem;
  }
  & input {
    color: gray !important;
    background: white;
  }
  & input::placeholder {
    color: gray !important;
    opacity: 1;
    background: white;
  }
`;
const BkgOverlay = styled.div`
  background-color: rgba(0, 0, 0, 0.5);
  height: 100%;
  position: relative;
  z-index: 1;
`;
const MainContent = styled.div`
  background-image: url("./BookShelf2.JPG");
  background-size: 100vw;
  background-repeat: no-repeat;
  height: 90.3vh;
`;

const fyt = styled.div`
  color: white;
`;

const Home = () => {
  const [search, setSearch] = useState("");

  return (
    <MainContent>
      <BkgOverlay>
        <SearchLayout>
          <LogoPosition>
            <img src={"./Mock_Logo_LargeV2.png"}></img>
          </LogoPosition>
          <SearchPosition>
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
          </SearchPosition>
        </SearchLayout>
      </BkgOverlay>
    </MainContent>
  );
};

export default Home;

{
  /* <MuiThemeProvider theme={input}>
                <InputBase
                  placeholder="Title, Author, ISBN..."
                  onChange={e => setSearch(e.target.value)}
                  inputProps={{ "aria-label": "search" }}
                />
              </MuiThemeProvider> */
}

// style={{ backgroundImage: "url(${./BookShelf.JPG})" }}
