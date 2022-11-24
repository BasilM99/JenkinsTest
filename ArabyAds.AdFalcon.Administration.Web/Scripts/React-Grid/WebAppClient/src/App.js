import React, { Component } from "react";
import { Route, Switch, Redirect } from "react-router-dom";

import BaseComponent from "./components/common/BaseComponent";

import OuterAppGrid from "./components/OuterAppGrid";


import LoadingSpinner from "./components/common/LoadingSpinner";


class App extends BaseComponent {
  state = {
    title: "empty",
    Autenticated: true,
    loadingData: false,
    showLogin: false
  };
  handlelChangeTitle = title => {
    this.setState({ title: title });

  
    console.log(location.pathname);
  };
  setLoadingDataState = stat => {
    this.setState({ loadingData: stat });
  };

 
  render() {
    return (
      <div>
               <LoadingSpinner loadingData={this.state.loadingData}  />
        <OuterAppGrid
         
          setLoadingDataState={this.setLoadingDataState}
          onChangeTitle={this.handlelChangeTitle}
          gridConfigItem={this.props.gridConfigItem}
        />
      </div>
    );
  }
}
export default App;
