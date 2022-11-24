import React from "react";
//import { Link } from 'react-router-dom';
import BaseComponent from "./BaseComponent"; //<-
import AppGrid from "./AppGrid"; //<-
import Loading from "./Loading";
import Alert from "./Alert"; //<-

import AlertWhiteSpace from "./AlertWhiteSpace";

import ApiManager from "./ApiManager";
import LoadingSpinner from "./LoadingSpinner";

//import Relogin from '../ReLogin';
import { default as OverflowEllipsis } from "react-overflow-ellipsis";
import { object } from "prop-types";


var jQuery = require("./jquery");
let list = [];
let select = [];

var ReactOverflowTooltip = require("react-overflow-tooltip");
class InnerAppGrid extends BaseComponent {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      showConfirmDeleteAlert: false,
      showLogin: false,
      selectedItems: [],
      source: {
        data_list: [],
        paging: {
          countItems: 0,
        },
        isThereSubGrids: false,
      },
      toBeDeleted: -1,
      ArlangVal: "",
      GridData: [],
    };
  }


  componentDidMount() {

    this.props.onRef(this);
    this.callGridData({}, this.state.source, 0);
  }

  componentWillUnmount() {
    this.props.onRef(undefined);
  }

  CallRefreshGrid = () => {
    this.getDataList(1, null);
  };

  CallRefreshGrid = (page) => {
    this.getDataList(page, null);
  };

  componentDidUpdate() {
    jQuery.InitTooltip();
  }

  checkIfThereSubGrids = (parentsArray) => {
    let val = false;
    for (let i = 0; i < parentsArray.length; i++) {
      let parent = parentsArray[i];
      if (typeof parent.children != "undefined") {
        if (parent.children.length > 0) {
          val = true;
          break;
        } else {
          val = false;
        }
      }
    }
    return val;
  };

  callGridData = (fillters, result, page) => {

      //debugger;
      if (fillters && (Object.keys(fillters).length === 0 && fillters.constructor === Object))
{
  if(this.props.gridConfigItem.Dynamic)
  {
    return;
  }
} 
      let data = fillters;
      if (!data) {
          data = { page: 0, size: 10 };
      }
	  
				   let headers = {};
	    if(!this.props.gridConfigItem.Dynamic)
              {
     headers = {
        "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
        "X-Requested-With": "XMLHttpRequest"
    };
			  }
			  else
			  {
				  
				    headers = {
      "Content-Type": "application/json",
    };
				  
			  }
      this.props.setLoadingDataState(true);
      if (!page) { page = 0; }
	   if(!this.props.gridConfigItem.Dynamic)
              {
				data.page=page+1;
			  }
			  else{
				  
				data.page=page;  
				  
			  }
    data.size=this.props.gridConfigItem.pageSize;
    var self = this;
    if (this.props.gridConfigItem.gridDataUrl != "") {
      ApiManager.callwebservice(
        "post",
        this.props.gridConfigItem.gridDataUrl,
        data,
        headers,
        (success) => {
          if (success.data) {
            //Ajax Success
            //-----------------------------------
            //debugger;
            if((success.data.Data != null && success.data.Total > 0) || (success.data.data != null && success.data.data.Count > 0) )
            {
              let  rowsmod =[];

              if (success.data.Massage && success.data.Massage != "")
              {showErrorMessage(success.data.Massage, true);

                this.props.setLoadingDataState(false);
              }
                if (success.data.warnings  && success.data.warnings != "")
              showWarningMessage(success.data.warnings, true);


              if(!this.props.gridConfigItem.Dynamic)
              {
              this.state.GridData = success.data.Data;
              result.data_list = this.state.GridData;
              result.paging.countItems =success.data.Total;
              
              result.paging.pageIndex = page;
              result.paging.pageSize = this.props.gridConfigItem.pageSize;
              result.paging.countPages =
                result.paging.countItems > 0
                  ? Math.ceil(result.paging.countItems / result.paging.pageSize)
                  : 0;
              }
              else
              {

                this.props.gridConfigItem.headers = [];


                this.props.gridConfigItem.colDef =[];

                
                if (success.data.data.Columns.length > 0) {
                  let colNomber = success.data.data.Columns.length;
                  let colWidth = 100 / colNomber;


                  if (this.props.gridConfigItem != null && typeof (this.props.gridConfigItem) != "undefined") {
                  
                  
                    this.props.gridConfigItem.headers = success.data.data.Columns.map(
                      function (i) {
                        return {
                          style: {
                            width: ((i.length * colWidth)+4)>75?(i.length * colWidth)+4:75,
                            minWidth :75,
                            padding: "0 10px",
                          },
                          value: i,
                        };
                      }
                    );
                    this.props.gridConfigItem.colDef = success.data.data.Columns.map(
                      (i) => i
                    );

                    this.props.gridConfigItem.colDef = success.data.data.Columns.map(i => i);
                  
                    this.props.gridConfigItem.haveFooter=true;
                  let rows=  success.data.data.Rows;
				  let rowsModf=[];
                    rowsmod =rows;
					if(rows.length>0)
					{
						for(var cr=0; cr< rows.length; cr++)
						{
							rowsModf.push(rows[cr].Value);
							
							
						}
						
					}
					rowsmod=rowsModf;
                  /* rows.map(function (i) {
                        //debugger;
                        let index = 0;
                        for (var obj in i) {
                            try {
                                if (success.data.data.Columns[index].search(" ") != -1) {
                                    if (typeof (i[success.data.data.Columns[index]]) != "undefined") {
                                        i[obj.replace(/ /g, "")] = i[success.data.data.Columns[index]];
                                    }
                                    delete i[success.data.data.Columns[index]];
                                }
                                index++;
                            } catch (e) {
                                console.log("1");
                            }
                        }
                    
                        return i
                    });*/
                    

                  }
              }
            


                this.state.GridData = rowsmod;
                result.data_list = this.state.GridData;
                result.paging.countItems =success.data.data.Count;
                
                result.paging.pageIndex = page;
                result.paging.pageSize = this.props.gridConfigItem.pageSize;
                result.paging.countPages =
                  result.paging.countItems > 0
                    ? Math.ceil(result.paging.countItems / result.paging.pageSize)
                    : 0;


              }
              /*result.data_list = this.paginate(
                result.data_list,
                result.paging.pageSize,
                page
              );*/
              let _isThereSubGrids = this.checkIfThereSubGrids(result.data_list); // result.data_list.length > 0 ? true : false;
              this.setState({
                source: result,
                loading: false,
                isThereSubGrids: _isThereSubGrids,
              });
              this.props.setLoadingDataState(false);
            }
            else{
              this.props.setLoadingDataState(false);

              this.state.GridData = [];
              result.data_list = this.state.GridData;
              result.paging.countItems =success.data.Total;
              
              result.paging.pageIndex = page;
              result.paging.pageSize = this.props.gridConfigItem.pageSize;
              result.paging.countPages =
                result.paging.countItems > 0
                  ? Math.ceil(result.paging.countItems / result.paging.pageSize)
                  : 0;

            //this.state.GridData =[];
            this.setState({
              source: result,
              loading: false
            });
            }
          } else {
            showErrorMessage(success.data.msg,true);
            this.props.setLoadingDataState(false);
          }
        },
        (error) => {
          //debugger;
          showErrorMessage(error.data.msg,true);
          this.props.setLoadingDataState(false);
        },
        1
      );
    } else {
      this.state.GridData = this.props.gridConfigItem.RowsToFill;
      result.data_list = this.state.GridData;
      result.paging.countPages = result.paging.countItems =
        result.data_list.length;
      result.paging.pageIndex = page;
      result.paging.pageSize = this.props.gridConfigItem.pageSize;
      result.paging.countPages =
        result.paging.countItems > 0
          ? Math.ceil(result.paging.countItems / result.paging.pageSize)
          : 0;
      result.data_list = this.paginate(
        result.data_list,
        result.paging.pageSize,
        page
      );
      let _isThereSubGrids = this.checkIfThereSubGrids(result.data_list); // result.data_list.length > 0 ? true : false;
      this.setState({
        source: result,
        loading: false,
        isThereSubGrids: _isThereSubGrids,
      });
      this.props.setLoadingDataState(false);
    }
  };

  render() {
    let _isThereSubGrids = false;
    const { source } = this.state;
    console.log("JSON.stringify", list);
    const { T } = this;
    return (
      <div>
        <div>
          <div className="pull-left"></div>
        </div>
        <div>
        <LoadingSpinner loadingData={this.state.loadingData}  />

          <AppGrid
            headers={this.props.gridConfigItem.headers}
            data={this.state.source.data_list}
            filters={this.props.gridConfigItem.filters}
            paging={source.paging}
            haveFooter={this.props.gridConfigItem.haveFooter}
            haveSecondFooter={false}
            dataStyle={this.props.gridConfigItem.dataStyle}
            gridConfigItem={this.props.gridConfigItem}
            onPageChange={this.handelPageChange}
            loading={this.state.loading}
            onFilter={this.handelFilter}
            showGrid={"block"}
            isThereSubGrids={this.state.isThereSubGrids}
          ></AppGrid>
        </div>
        <Alert
          confirmButtonText={T("yes")}
          cancelButtonText={T("no")}
          show={this.state.showConfirmDeleteAlert}
          title={T("confirmDeletAud")}
          text={T("alert.deleteConfirmationAudL.msg")}
          showCancelButton
          onConfirm={this.handelConfirmDelete}
          onCancel={() => {
            this.setState({ showConfirmDeleteAlert: false });
          }}
          onEscapeKey={() => this.setState({ showConfirmDeleteAlert: false })}
        />
      </div>
    );
  }
  getId() {
    return localStorage.getItem("userdetail");
    //    console.log("userdetail is", )
  }

  
  getDataList = (page, filters) => {
    var self = this;
    let userid = self.getId();
    let result = {
      data_list: [],
      paging: {
        countItems: 0,
      },
    };

    let selectedaudiancelist = "";
    let advertiserId = "";

    let qry_param = this.props.querystring_param;
  
    let headers = {
      "Content-Type": "application/json",
    };
    let priceFromFilter = "";
    let nameFilter = "";
    let priceToFilter = "";


    this.props.setLoadingDataState(true);

    if (!Number.isInteger(page)) {
      page = 0;
    }



    
    this.setState({ loading: true });
    result = {
      data_list: [],
      paging: {
        countItems: 28,
      },
    };
    //     //simulate dat
    this.callGridData(filters, result, page);
 
  };


  handelPageChange = (page, filter) => {
   
    this.getDataList(page, filter);
  };
  handelFilter = (filter) => {
    this.getDataList(1, filter);
  };
  hanelDelete = (e) => {
    this.setState({
      toBeDeleted: e.target.getAttribute("dataid"),
      showConfirmDeleteAlert: true,
    });
  };
  handelConfirmDelete = () => {
    let newSource = { ...this.state.source };
    let toBeDeleted = this.state.toBeDeleted;
    //newSource.data = newSource.data.filter(x => x.id != toBeDeleted);
    //this.setState({ source: newSource, toBeDeleted: -1, showConfirmDeleteAlert: false });

    var rowdata = [toBeDeleted];
    // rowdata.push(row_id)
    console.log("rowdata", rowdata);
    let data = {
      audience_list_ids: rowdata,
    };
    let headers = {
      "Content-Type": "application/json",
    };
    var self = this;
    this.callapi(
      "delete",
      `/api/audience-list`,
      data,
      headers,
      (success) => {
        // this.RedirectToGeneric("/audience-list", "audience-list");

        self.props.onForTargetDelete({
          id: toBeDeleted,
        });
        self.setState({ toBeDeleted: "", showConfirmDeleteAlert: false });
        self.getDataList(1, null);
        Toast.success(success.data.msg);
      },
      (error) => {
        console.log("error", error);
        // this.RedirectToGeneric("/audience-list", "audience-list");
      }
    );
    //(@Anas)Pereforme Server Delete Here:
    //.......
  };
}
export default InnerAppGrid;
