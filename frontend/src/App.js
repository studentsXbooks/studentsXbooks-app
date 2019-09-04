// @flow

//import logo from "./logo.svg";
import "./App.css";

// Added by dotnet-react support for authentication
import React, { Component } from 'react';

import { Route } from 'react-router';
import { Home, Login } from './pages';
 
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <div>
        <Route exact path='/' component={Home} />
        {/* <Route path='/register' component={Register} /> */}
        <Route path='/login' component={Login} />
      </div>
    );
  }
}