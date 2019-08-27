import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
        <div>
            <a href="Identity/Account/Register?returnUrl=/authentication/login" ><h1>Sign up!</h1></a>
        </div>
    );
  }
}
