import React from "react";
import BaseComponent from "./common/BaseComponent";
import InnerAppGrid from "./common/InnerAppGrid";
import Cookie from "./common/Cookie.jsx";
import { toast as Toast, ToastContainer } from "react-toastify";
const queryString = require("query-string");

var selectedaudience = [];
class OuterAppGrid extends BaseComponent {
  constructor(props) {
    super(props);

    this.state = {
      list: {
        items: [],
      },
      templist: {
        items: [],
      },
      disableexport: false,
    };

  
  }

  componentDidMount() {
   


  }

 


  setLoadingDataStatePass = (data) => {
    this.props.setLoadingDataState(data);
  };
 

  render() {
    //let querystring_param = this.props.location.search.split("=")[1]

    const queryValues = queryString.parse(location.search);
    let querystring_param = queryValues;
    return (
      <React.Fragment>
        <InnerAppGrid
         
          onRef={(ref) => (this.child = ref)}
          setLoadingDataState={this.setLoadingDataStatePass}
       
       
      
         
          querystring_param={querystring_param}
          gridConfigItem={this.props.gridConfigItem}
        />
      </React.Fragment>
    );
  }



 
}

export default OuterAppGrid;
