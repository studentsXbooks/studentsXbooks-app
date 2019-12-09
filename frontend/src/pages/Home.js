import React, { useState } from "react";
import styled from "styled-components";
import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";
import Button from "@material-ui/core/Button";
import { navigate } from "@reach/router";
import MockLogo from "../images/Mock_Logo_Largev2.png";
import Books from "../images/homePageBooks.jpg";

const SearchBox = styled.div`
  margin-bottom: 2rem;
  padding: 1rem 2rem;
  display: grid;
  flex-flow: row nowrap;
  grid-template: "logo search" auto / 20% 80%;
  align-items: center;
  color: white;
`;

const SearchLayout = styled.div`
  display: grid;
  padding-top: 10rem;
  align-items: flex-start;
  justify-items: center;
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
    background-color: #32a8b3;
  }
  & > div {
    padding-left: 1rem;
  }
  & input {
    color: gray !important;
    background: white;
  }
  & input::placeholder {
    opacity: 0.75;
  }
`;

const MainContent = styled.div`
  background: rgba(0, 0, 0, 0.5) url(${Books}) center / cover no-repeat;
  background-blend-mode: darken;
  height: calc(
    100vh - 64px
  ); /* 64px is based off of material-ui navbar, Changes on smaller screens */
  text-align: center;
`;

const Home = () => {
  const [search, setSearch] = useState("");

  return (
    <MainContent>
      <SearchLayout>
        <div>
          <img src={MockLogo} alt="Very Big Mock Logo" />

          <SearchBox>
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
          </SearchBox>
        </div>
      </SearchLayout>
    </MainContent>
  );
};

export default Home;
