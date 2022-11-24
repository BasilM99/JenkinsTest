import * as React from "react";
import * as ReactDOM from "react-dom";
import  {useState} from "react";

import { Grid, GridColumn as Column } from "@progress/kendo-react-grid";
import { DataLoader } from "./Data-loader.jsx";
import Constants from '../../Config/Constants';
import axios from 'axios';

export const AdPerformanceGrid = ({}) => {


  // const _state = {
  //       products: { data: [], total: 0 },
  //       dataState: { 
  //         sort: null,
  //         page: 1,
  //         pageSize: 10,
  //         group: null,
  //         filter: null,
  //       },
  //       isRefresh : false
  //     };


  
  const [_state,setLocalState] = useState( {
    List: { data: [], total: 0 },
    dataState: { 
      sort: null,
      page: 1,
      pageSize: 10,
      group: null,
      filter: null,
    },
    isRefresh:true,
    pending:"",
    lastSuccess : ""
  }
);

      const dataStateChange = e => {
      setLocalState({
      ...state,
      dataState: e.dataState
    });
  };
  const dataStateRefresh = e => {
    setLocalState({
      ...state,
      dataState: _state.dataState,
      isRefresh:true
    });
  };

  const dataRecieved = products => {
    setLocalState({
      ...state,
      products: products
    });
  };



  const callData = () =>{
    let headers= {};
    
    var endPointData = {
      httpMethod : 'post',
      url: Constants.backend.getAdPerformanceGrid,
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
          
          style={{ height: '400px', fontSize: '13px' }}
          {..._state.dataState}
          {..._state.products}
          onDataStateChange={dataStateChange}
        >
          <Column field="CampaignName" title="Campaign Name" />
          <Column field="ProductName" title="Impressions" />
          <Column field="Impress" title="Clicks" />
          <Column field="CtrText"  title="CTR" />
          <Column field="AvgCPCText" title="Avg.CPC" />
          <Column field="BillableCostText" filter="numeric" title="Billable Cost" />
        </Grid>
        <a onClick={dataStateRefresh}  className="k-pager-refresh k-link" title="Refresh" aria-label="Refresh"><span className="k-icon k-i-reload"></span></a>
								
      </div>
    );
  }

  return<>
  {GridRender()}
</>

}
export default AdPerformanceGrid;
