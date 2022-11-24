import * as React from "react";
import  {useState} from "react";
import * as ReactDOM from "react-dom";

import { Grid, GridColumn as Column } from "@progress/kendo-react-grid";
import { DataLoader } from "./Data-loader.jsx";
import {ApiManager} from "../../Communication/Axios";
import axios from 'axios';
import Constants from '../../Config/Constants';

export const AdGeoLocationGrid= ({props})=> {
  // constructor(props) {
  //   super(props);
  //   state = {
  //     products: { data: [], total: 0 },
  //     dataState: { 
  //       sort: null,
  //       page: 1,
  //       pageSize: 10,
  //       group: null,
  //       filter: null,
  //       country:'' ,
  //       list: null,
  //       period: 0,
  //       AdvertiserId: null,
  //       AdvertiserAccountId: null,
  //     },
  //     isRefresh:true
  //   };
  // }
  const componentDidMount = () =>{
    callData();
  }
  const dataStateChange = e => {
    setLocalState({
      ..._state,
      dataState: e.dataState
    });
  };

  const [_state,setLocalState] = useState( {
      List: { data: [], total: 0 },
      dataState: { 
        sort: null,
        page: 1,
        pageSize: 10,
        group: null,
        filter: null,
        country:'' ,
        list: null,
        period: 0,
        AdvertiserId: null,
        AdvertiserAccountId: null,
      },
      isRefresh:true,
      pending:"",
      lastSuccess : ""
    }
  );
  // const dataRecieved = (products) => {
  //   this.setState({
  //     ...state,
  //     products: products
  //   });
  // }

  const refreshTable = () =>{
    state.isRefresh = false;
    callData();
  }


  const callData = () =>{
    
    var endPointData = {
      httpMethod : 'post',
      url: Constants.backend.getAdGeoLocationGrid,
    }
    //var self=this;
    if ( (_state.pending || JSON.stringify(_state.dataState) === _state.lastSuccess )  ) {
      if( _state.isRefresh)
        return;
    }
  
    _state.pending = JSON.stringify(_state.dataState);
    axios.post( endPointData.url , 
      _state.dataState)
      .then(function (success) {
        _state.lastSuccess = _state.pending;
        _state.pending = '';
        _state.isRefresh = true;
          if(success.data){
            var rdata = {
              data: success.data.Data,
              total:  success.data.Total
            };
            setLocalState({
              ..._state,
              List: rdata,
            });
            
          }
        console.log(success);
      })
      .catch(function (error) {
        console.log(error);
      }
    );

  }

  const GridRender = () => {
 
    callData();
    return (
      <div>
        
        <Grid
          
          //pageable={true}
          pageable={ true  }
          refresh ={true}
          style={{ height: '400px', fontSize: '13px' }}
          {..._state.dataState}
          {..._state.List}
          onDataStateChange={dataStateChange}
        >
          <Column field="CountryName"  title="Country Name" />
          <Column field="CampaignName" title="Campaign Name" />
          <Column field="ProductName" title="Impressions" />
          <Column field="Clicks"  title="Clicks" />
          <Column field="CtrText"  title="CTR" />
          <Column field="AvgCPCText" title="Avg.CPC" />
          <Column field="BillableCostText" filter="numeric" title="Billable Cost" />
        </Grid>
        <a onClick={refreshTable}  className="k-pager-refresh k-link float-right mt-2" title="Refresh" aria-label="Refresh"><span className="k-icon k-i-reload"></span></a>
						
        {/* <DataLoader
          dataState={state.dataState}
          onDataRecieved={this.dataRecieved}
          endPointData= {endPointData}
        /> */}
      </div>
    )
  //}
}
return<>
  {GridRender()}
</>

}

export default AdGeoLocationGrid;
