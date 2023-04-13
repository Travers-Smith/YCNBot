import React, { Component } from 'react';
import './custom.css';
import YCNBot from './YCNBot';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
        <YCNBot/>
    );
  }
}
