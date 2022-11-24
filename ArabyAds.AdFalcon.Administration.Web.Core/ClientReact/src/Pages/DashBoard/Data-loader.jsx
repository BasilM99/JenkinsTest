import * as React from "react";
import * as ReactDOM from "react-dom";
import { toODataString } from "@progress/kendo-data-query";
import {ApiManager} from "../../Communication/Axios";

export class DataLoader extends React.Component {
 
   lastSuccess = "";
   pending = "";

  requestDataIfNeeded = () => {
    if ( (this.pending || JSON.stringify(this.props.dataState) === this.lastSuccess ) || this.props.isRefresh ) {
      return;
    }
    let headers= {};
    
    this.pending = JSON.stringify(this.props.dataState);
    ApiManager.callwebservice(
        this.props.endPointData.httpMethod,
        this.props.endPointData.url,
        this.props.dataState,
        headers,
        (success) => {
          this.lastSuccess = this.pending;
          this.pending = '';
            if(success.data){
              this.props.onDataRecieved.call(undefined, {
                data: success.data.Data,
                total:  success.data.Total
              });
                console.log(response);
            }
        },
        (error) => {
          //debugger;
            console.log(error);
        },
        1
      );

    // this.pending = toODataString(this.props.dataState);
    // fetch(this.baseUrl + this.pending, this.init)
    //   .then(response => response.json())
    //   .then(json => {
    //     this.lastSuccess = this.pending;
    //     this.pending = "";
    //     if (toODataString(this.props.dataState) === this.lastSuccess) {
    //       this.props.onDataRecieved.call(undefined, {
    //         data: json.value,
    //         total: json["@odata.count"]
    //       });
    //     } else {
    //       this.requestDataIfNeeded();
    //     }
    //   });
  };

  render() {
    this.requestDataIfNeeded();
    return this.pending && <LoadingPanel />;
  }
}

class LoadingPanel extends React.Component {
  render() {
    const loadingPanel = (
      <div className="k-loading-mask">
        <span className="k-loading-text">Loading</span>
        <div className="k-loading-image" />
        <div className="k-loading-color" />
      </div>
    );

    const gridContent = document && document.querySelector(".k-grid-content");
    return gridContent
      ? ReactDOM.createPortal(loadingPanel, gridContent)
      : loadingPanel;
  }
}
