// @flow

import React from "react";
import { Router } from '@reach/router';
import { ThemeProvider } from 'styled-components';

import Theme from './theme.json';

function App() {
  return (
    <>
    <h1>hey</h1>
       <ThemeProvider theme={Theme}>
       
      </ThemeProvider> 
    </>
  );
}

export default App;
