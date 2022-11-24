import React from "react";
import BaseFormComponent from "./BaseFormComponent";
import moment from "moment";
import DateRangePicker from "react-bootstrap-daterangepicker";
import CustomDateRangePicker from "./CustomDateRangePicker";
import LoadingSpinner from "./loadingSpinner";
// = require("./this.gridConfig").getAudienceGridConfig();
//var gridConfig =  eval("gridConfig" + document.getElementById("grid-div").dataset.gridname);

class AppGrid extends BaseFormComponent {
  constructor(props) {
    super(props);
    this.state = {
      form: {},
      selectedItems: [],
      filter: null,
    };
    //var gridConfig =  eval("gridConfig" + stringConfig);

    //Add Filters to state:
    const { filters } = this.props;
    filters.forEach((filter) => {
      switch (filter.condition) {
        case "between":
          this.state.form[filter.field + "From"] = {
            value: "",
            type: filter.type == "date" ? "text" : filter.type,
            label: "from",
          };
          this.state.form[filter.field + "To"] = {
            value: "",
            type: filter.type == "date" ? "text" : filter.type,
            label: "to",
          };

          if (filter.type == "date") {
            this.state.form[filter.field + "From"].date = new Date();
            this.state.form[filter.field + "To"].date = new Date();
          }

          break;

        case "equals":
          this.state.form[filter.field] = {
            value: "",
            type: filter.type,
            label: filter.display,
          };
          break;
        default:
          this.state.form[filter.field] = {
            value: "",
            type: filter.type,
            label: filter.display,
          };
      }
    });
  }
  propName = (prop, value) => {
    for (var i in prop) {
      if (prop[i] == value) {
        return i;
      }
    }
    return false;
  };

  renderArrowMenu = (obj, value) => {
    let tepmPropName = this.propName(obj, value);
    let tempArray = new Array();

    this.props.gridConfigItem.arrowMenu.forEach((element) => {
      //let tempAttributes = element;
      const tempAttributes = JSON.parse(JSON.stringify(element));

      if (tempAttributes.hasOwnProperty("attributes")) {
        if (tempAttributes.attributes.hasOwnProperty("valitem"))
          tempAttributes.attributes.valitem = element.attributes.valitem.replace(
            "{{id}}",
            eval("obj." + this.props.gridConfigItem.colDefObj.itemIds)
          );

        if (tempAttributes.attributes.hasOwnProperty("href2"))
          tempAttributes.attributes.href2 = element.attributes.href2.replace(
            "{{id}}",
            eval("obj." + this.props.gridConfigItem.colDefObj.itemIds)
          );

        if (tempAttributes.attributes.hasOwnProperty("item_extra_info"))
          tempAttributes.attributes.item_extra_info = element.attributes.item_extra_info.replace(
            "{{val}}",
            value
          );
      }

      if (tempAttributes.columnName == tepmPropName) {
        tempArray.push(tempAttributes);
      }
    });

    if (tepmPropName && tempArray.length > 0) {
      return (
        <div data-table-dropdown className="dropdown pull-right">
          <button
            type="button"
            className="btnT btnT-arrow dropdown-toggle"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          ></button>
          <ul className="dropdown-menu  dropdown-menu-right">
            <li></li>
            {tempArray.map((element) => {
              element.className += " grid-icon";
              return (
                <li key={element.actionName}>
                  <a
                    key={element.actionName}
                    dataid={value}
                    href={element.urlString}
                    onClick={eval(element.functionToRun)}
                    role="button"
                    {...element.attributes}
                  >
                    <i
                      key={element.actionName}
                      className={element.className}
                    ></i>
                    &nbsp;&nbsp;{element.textToShow}
                  </a>
                </li>
              );
            })}
          </ul>
        </div>
      );
    }
  };

  renderToolTip = (obj, value) => {
    let tepmPropName = this.propName(obj, value);
    let tempArray = new Array();

    this.props.gridConfigItem.toolTip.forEach((element) => {
      if (element.columnName == tepmPropName) {
        tempArray.push(element);
      }
    });

    if (tepmPropName && tempArray.length > 0) {
      return (
        <a
          title={value}
          data-toggle="tooltip"
          data-placement="top"
          className="btnT-empty-primary pull-right more-info fa fa-text-overflow"
        ></a>
      );
    }
  };

  renderButton = (obj, value) => {
    let tepmPropName = this.propName(obj, value);
    //let tempArray = new Array();
    let tmpGridConfigObj;
    this.props.gridConfigItem.buttons.forEach((element) => {
      if (element.columnName == tepmPropName) {
        tmpGridConfigObj = element;
      }
    });

    if (tepmPropName && tmpGridConfigObj) {
      return (
        <button
          // disabled={select[index] === row.id}
          //disabled={this.props.selectedItemsPr.some((x) => x.id == value)}
          style={{ margin: "5px 0" }}
          id={value}
          onClick={eval(tmpGridConfigObj.functionToRun)}
          className={tmpGridConfigObj.className}
        > 
          {tmpGridConfigObj.textToShow /*parentT("button.target")*/}
        </button>
      );
    }
  };

  getPagesbtnTs() {
    //console.log("this.props.paging", this.props.paging);
    //let counterpage=0;
    let pages = [];
    for (let page = 0; page < this.props.paging.countPages; page++) {
      pages.push(page);
    }

    return pages;
  }

  getCells = (row, _isThereSubGrids) => {
    let cells = [];

    for (
      let index = 0;
      index < this.props.gridConfigItem.colDef.length;
      index++
    ) {
      let element = this.props.gridConfigItem.colDef[index];
      if (typeof row[element] == "undefined") {
        row[element] = "";
      }

      if (index == 0) {
        if (this.props.gridConfigItem.isTherecheckboxCol) {
          cells.push(
            <React.Fragment>
              <td key={index}>
                <input
                  name="checkedRecords"
                  type="checkbox"
                  value={eval(
                    "row." + this.props.gridConfigItem.colDefObj.itemIds
                  )}
                  title="checkedRecords"
                  style={{ margin: "0 15px" ,  textAlign: "center" }}
                  key={index}
                ></input>
              </td>
            </React.Fragment>
          );
        } else if (_isThereSubGrids) {
          cells.push(
            <React.Fragment>
              {typeof row.children != "undefined" && row.children.length > 0 ? (
                <td
                 style={{ textAlign: "center" }}
                  key={index}
                  dangerouslySetInnerHTML={{
                    __html: this.ShowSubGridArrow(row.children.length),
                  }}
                  onClick={this.handelGenerateTable}
                  
                ></td>
              ) : (
                <td key={index} onClick={this.handelGenerateTable}></td>
              )}
            </React.Fragment>
          );
        }
        //continue;
      } else if (
        index == 1 &&
        _isThereSubGrids &&
        this.props.gridConfigItem.isTherecheckboxCol
      ) {
        cells.push(
          <React.Fragment>
            {typeof row.children != "undefined" && row.children.length > 0 ? (
              <td
              style={{ textAlign: "center" }}
              key={index}
                dangerouslySetInnerHTML={{
                  __html: this.ShowSubGridArrow(row.children.length),
                }}
                onClick={this.handelGenerateTable}
              ></td>
            ) : (
              <td key={index} onClick={this.handelGenerateTable}></td>
            )}
          </React.Fragment>
        );
      }

      cells.push(
        <React.Fragment>
          <td
            style={eval(this.props.gridConfigItem.dataStyle[element])}
            className={element}
            key={element}
            style={{ textAlign: "center" }}
          >
            {eval("row['" + element +"']" ) != "" ? eval("row['" + element +"']" ) : null}
            {eval("row['" + element+"']" ) != ""
              ? this.renderArrowMenu(row, eval("row['" + element +"']"))
              : null}
            {eval("row['" + element+"']") != ""
              ? this.renderToolTip(row, eval("row['" + element+"']"))
              : null}
            {eval("row['" + element+"']" ) != ""
              ? this.renderButton(row, eval("row['" + element+"']"))
              : null}
          </td>
        </React.Fragment>
      );
    }
    return cells;
  };

  renderFilterFields(filter) {
    const { T } = this;
    const { form } = this.state;
    switch (filter.condition) {
      case "between":
        return filter.type == "date" ? (
          <div id={form[filter.field]} key={filter.field}>
            <CustomDateRangePicker
              titleMsg={this.IsArabic ? "من" : "From"}
              title="From"
              name={form[filter.field + "From"]}
              value={form[filter.field + "From"].value}
              from={form[filter.field + "From"].date}
              to={form[filter.field + "To"].date}
              onChange={this.handelInputChange}
              onApply={(event, picker) => {
                this.handelApplyDateRangePicker(picker, filter.field);
              }}
            ></CustomDateRangePicker>
            <CustomDateRangePicker
              titleMsg={this.IsArabic ? "الى" : "To"}
              title="To"
              name={form[filter.field + "To"]}
              value={form[filter.field + "To"].value}
              from={form[filter.field + "From"].date}
              to={form[filter.field + "To"].date}
              onChange={this.handelInputChange}
              onApply={(event, picker) => {
                this.handelApplyDateRangePicker(picker, filter.field);
              }}
            ></CustomDateRangePicker>
          </div>
        ) : (
          <React.Fragment key={"inside" + filter.field}>
            <div className="form-group">
              <label className="control-label col-sm-3">
                <span>{T(filter.display)}</span>{" "}
                <span>{T(form[filter.field + "From"].label)}</span>
              </label>
              <div className={"col-sm-9"}>
                <input
                  value={form[filter.field + "From"].value}
                  name={filter.field + "From"}
                  type={form[filter.field + "From"].type}
                  className="form-control"
                  onChange={this.handelInputChange}
                />
              </div>
            </div>
            <div className="form-group">
              <label className="control-label col-sm-3">
                <span>{T(filter.display)}</span>{" "}
                <span>{T(form[filter.field + "To"].label)}</span>
              </label>
              <div className={"col-sm-9"}>
                <input
                  value={form[filter.field + "To"].value}
                  name={filter.field + "To"}
                  type={form[filter.field + "To"].type}
                  className="form-control"
                  onChange={this.handelInputChange}
                />
              </div>
            </div>
          </React.Fragment>
        );

      case "equals":
        switch (form[filter.field].type) {
          case "text":
            return (
              <div key={"inside" + filter.field} className="form-group">
                <label className="control-label col-sm-3">
                  {T(form[filter.field].label)}
                </label>
                <div className={"col-sm-9"}>
                  <input
                    value={form[filter.field].value}
                    name={filter.field}
                    type={form[filter.field].type}
                    className="form-control"
                    onChange={this.handelInputChange}
                  />
                </div>
              </div>
            );

          case "dropdown":
            return (
              <div key={"inside" + filter.field} className="form-group">
                <label className="control-label col-sm-3">
                  {T(form[filter.field].label)}
                </label>
                <div className={"col-sm-9"}>
                  <select
                    className="form-control"
                    name={filter.field}
                    value={form[filter.field].value}
                    onChange={this.handelInputChange}
                  >
                    {filter.options.map((item) => {
                      return this.TSelectOptions(item.value, item.text);
                    })}
                  </select>
                </div>
              </div>
            );
        }

      default:
        return (
          <div key={"inside" + filter.field} className="form-group">
            <label className="control-label col-sm-3">
              {T(form[filter.field].label)}
            </label>
            <div className={"col-sm-9"}>
              <input
                value={form[filter.field].value}
                name={filter.field}
                type={form[filter.field].type}
                className="form-control"
                onChange={this.handelInputChange}
              />
            </div>
          </div>
        );
    }
  }

  handelGenerateTable = (e) => {
    const elem = e.target.closest("tr").nextElementSibling.lastElementChild
      .firstElementChild;
    let elem2 = e._targetInst.stateNode.querySelectorAll("span")[0];

    if (elem.style.display == "none") {
      this.state.ArlangVal = this.IsArabic;

      elem2.closest("div").outerHTML =
        "<div class='arrowClass'><span>&#31;</span></div>";
      elem2.style.fontSize = "20px";
      elem.style.display = "block";
    } else if (elem.style.display == "block") {
      if (this.state.ArlangVal)
        elem2.closest("div").outerHTML =
          "<div class='arrowClass'><span>&#17;</span></div>";
      else
        elem2.closest("div").outerHTML =
          "<div class='arrowClass arrowRotate'><span>&#17;</span></div>";
      //elem2.innerHTML ='<span class="arrowClass">&#17;</span>'
      elem2.style.fontSize = "15px";
      elem.style.display = "none";
    }
    console.log;
  };
  // renderOutsideFilterFields(filter) {
  //     const { T } = this;
  //     const { form } = this.state;
  //     switch (filter.condition) {
  //         case 'between':
  //             return filter.type == 'date' ?
  //                 <div id={form[filter.field]} key={"outside" + filter.field}>
  //                     <CustomDateRangePicker
  //                         title="From"
  // 						     titleMsg={this.IsArabic ? "من": "From"}
  //                         name={form[filter.field + "From"]}
  //                         value={form[filter.field + "From"].value}
  //                         from={form[filter.field + "From"].date}
  //                         to={form[filter.field + "To"].date}
  //                         onChange={this.handelInputChange}
  //                         onApply={(event, picker) => { this.handelApplyDateRangePicker(picker, filter.field) }}>
  //                     </CustomDateRangePicker>
  //                     <CustomDateRangePicker
  //                         title="To"
  // 						     titleMsg={this.IsArabic ? "الى": "To"}
  //                         name={form[filter.field + "To"]}
  //                         value={form[filter.field + "To"].value}
  //                         from={form[filter.field + "From"].date}
  //                         to={form[filter.field + "To"].date}
  //                         onChange={this.handelInputChange}
  //                         onApply={(event, picker) => { this.handelApplyDateRangePicker(picker, filter.field) }}>
  //                     </CustomDateRangePicker>
  //                 </div>
  //                 : <React.Fragment key={"outside" + filter.field}>
  //                     <div className="form-group col-sm-6">
  //                         <label className="control-label col-sm-2" >{T(form[filter.field + "From"].label)}</label>
  //                         <div className={"col-sm-10"}>
  //                             <input value={form[filter.field + "From"].value} name={filter.field + "From"} type={form[filter.field + "From"].type} className="form-control" onChange={this.handleDateRefresh} />
  //                         </div>
  //                     </div>
  //                     <div className="form-group col-sm-6">
  //                         <label className="control-label col-sm-2" >{T(form[filter.field + "To"].label)}</label>
  //                         <div className={"col-sm-10"}>
  //                             <input value={form[filter.field + "To"].value} name={filter.field + "To"} type={form[filter.field + "To"].type} className="form-control" onChange={this.handleDateRefresh} />
  //                         </div>
  //                     </div>
  //                 </React.Fragment>

  //         case 'equals':
  //             return <div key={"outside" + filter.field} className="form-group">
  //                 <label className="control-label col-sm-3" >{T(form[filter.field].label)}</label>
  //                 <div className={"col-sm-9"}>
  //                     <input value={form[filter.field].value} name={filter.field} type={form[filter.field].type} className="form-control" onChange={this.handelInputChange} />
  //                 </div>
  //             </div>
  //         default:
  //             return <div key={"outside" + filter.field} className="form-group">
  //                 <label className="control-label col-sm-3" >{T(form[filter.field].label)}</label>
  //                 <div className={"col-sm-9"}>
  //                     <input value={form[filter.field].value} name={filter.field} type={form[filter.field].type} className="form-control" onChange={this.handelInputChange} />
  //                 </div>
  //             </div>
  //     }
  // }
  renderLoading() {
    return this.props.LoadingData ? <LoadingSpinner /> : <div></div>;
  }
  get PagingDisplayItems() {
    const { paging } = this.props;
    let from =
      paging.countItems == 0 ? 0 : paging.pageSize * paging.pageIndex + 1;
    let to =
      paging.countItems == 0
        ? 0
        : paging.pageSize * paging.pageIndex + paging.pageSize;
    to = (to > paging.countItems && paging.countItems) || to;
    return (
      <span>
        {this.TFormat("paging.dispalyingItems", [from, parseInt(to), paging.countItems])}
      </span>
    );
  }

  ShowSubGridArrow = (length) => {
    this.state.ArlangVal = this.IsArabic;

    if (length > 0) {
      if (this.state.ArlangVal)
        return "<div class='arrowClass '><span >\u0011</span></div>";
      else
        return "<div class='arrowClass arrowRotate'><span >\u0011</span></div>";
    } else {
      return "<span >\u0020</span>";
    }
  };
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

  renderNewGrid = (children, index) => {
    let _isThereSubGrids = this.checkIfThereSubGrids(children);
    let _isEven = index % 2 == 0 ? true : false;

    return (
      <tr className={_isEven ? "sub-even" : "sub-odd"}>
        <td></td>
        <td colSpan="4">
          <AppGrid
            headers={this.props.gridConfigItem.headers}
            data={children}
            filters={[]}
            paging={10}
            haveFooter={false}

            haveSecondFooter={true}
            dataStyle={this.props.gridConfigItem.dataStyle}
            onPageChange={this.handelPageChange}
            loading={this.state.loading}
            onFilter={this.handelFilter}
            showGrid={"none"}
            isThereSubGrids={_isThereSubGrids}
            gridConfigItem={this.props.gridConfigItem}
          ></AppGrid>
        </td>
      </tr>
    );
  };
  render() {
    if (typeof this.props.gridConfigItem.GlobalGridComponent != null)
      this.props.gridConfigItem.GlobalGridComponent = this;
    const { T } = this;
    const pages = this.getPagesbtnTs();
    let counterpage = 0;
    let counterpager = 0;
    const { paging, filters } = this.props;

    const _tableClass = !this.props.isThereSubGrids
      ? "table one-table"
      : "table ";

    return (
      <div style={{ display: this.props.showGrid + "" }}>
        {
          // filters && filters.length > 0 && <div className="pull-right " style={{ minWidth: this.props.dataStyle.maxWidth}} >
          //     <div className="form-horizontal col-sm-11">
          //         {
          //             filters.filter(x => x.place == 'outside').map(filter => {
          //                 return this.renderOutsideFilterFields(filter)
          //             })
          //         }
          //     </div>
          //     <div className="pull-right" >
          //         <div className="dropdown" id="ddFilterWrapper" style={{ top: "0px" }}>
          //             <a href="#" className="dropdown-toggle fa fa-filter"
          //                 style={{ fontSize: "22px", textDecoration: "none", color: "#666666" }}
          //                 id="ddFilter"
          //                 data-toggle="dropdown"
          //                 role="button"
          //                 aria-haspopup="true"
          //                 aria-expanded="false">  </a>
          //             <ul className="dropdown-menu dropdown-menu-right" id="popUpFilter" aria-labelledby="ddFilter" >
          //                 <li>
          //                     <form onSubmit={this.handelFilterClick} className="form-horizontal form-filter-dd" >
          //                         {
          //                             filters.filter(x => x.place == 'inside').map(filter => {
          //                                 return this.renderFilterFields(filter)
          //                             })
          //                         }
          //                         <div className="pull-right">
          //                             <button name="filter"
          //                                 type="submit"
          //                                 className="btnT btnT-primary"
          //                                 style={{ width: "auto", margin: "0 5px" }}>{T("filter")}</button>
          //                             <button
          //                                 name="clear"
          // 								onClick={this.handelResetFilterClick}
          //                                 type="button"
          //                                 className="btnT btnT-default"
          //                                 style={{ width: "auto", margin: "0 5px" }}>{T("clear")}</button>
          //                         </div>
          //                     </form>
          //                 </li>
          //             </ul>
          //         </div>
          //     </div>
          // </div>
        }
        <table className={_tableClass}        style={
            this.checkHeadersLength()
              ? { overflowX: "scroll", display: "block" }
              : {}
          }>
          <thead>
            <tr>
              {this.props.headers.map((header, index) => {
                if (!this.props.isThereSubGrids && header.value == "\u0011")
                  return null;

                return (
                  <th style={header.style} key={index}>
                    {T(header.value)}
                  </th>
                );
              })}
            </tr>
          </thead>
          <tbody>
            {
            
            this.props.data!=null &&  this.props.data.length>0?
            this.props.data.map((row, index) => {
              let _isEven = index % 2 == 0 ? true : false;
              return (
                <React.Fragment>
                  <tr className={_isEven ? "tr-even" : "tr-odd"} key={row.id}>
                    {this.getCells(row, this.props.isThereSubGrids).map(
                      (cell) => {
                        return cell;
                      }
                    )}
                  </tr>

                  {"children" in row && row.children.length > 0
                    ? this.renderNewGrid(row.children, index)
                    : null}
                </React.Fragment>
              );
            })
            :

            !(this.props.headers!=null && this.props.headers.length>0) ?  <React.Fragment></React.Fragment>:
            <tr>

<td colspan="1333">

{T("msg.noRecords")}
          </td>
            </tr>
     
            }



          </tbody>
        </table>

        {this.props.haveFooter ? (
          <div className="table-footer">
            <div
              style={{ height: "0px", background: "#fff", margin: "0 -7px" }}
            ></div>
            <div className="table-paging pull-left">
              <span className="btnT-refresh-container">
                <button
                  onClick={this.handelPageRefresh}
                  className={
                    "btnT-paging p-refresh " +
                    ((this.props.loading && "loading") || "")
                  }
                  type="button"
                ></button>
              </span>

              <button
                className={
                  "btnT-paging p-fisrt " +
                  ((this.props.paging.pageIndex === 0 && "disabled") || "")
                }
                onClick={this.handelPageFirst}
                type="button"
              ></button>

              <button
                className={
                  "btnT-paging p-previous " +
                  ((this.props.paging.pageIndex === 0 && "disabled") || "")
                }
                onClick={this.handelPagePrevious}
                type="button"
              ></button>

              {pages.map(
                (page) => {
                  if (paging.pageIndex <= page + 5 && counterpage < 5) {
                    counterpage = counterpage + 1;
                    return (
                      <button
                        type="button"
                        onClick={this.handelChangePage}
                        className={
                          "btnT btnT-primary  p-page " +
                          ((paging.pageIndex === page && "current") || "")
                        }
                        key={page}
                        dataid={page}
                      >
                        {page + 1}
                      </button>
                    );
                  } else if (
                    paging.pageIndex >= page - 5 &&
                    counterpager <= 5 &&
                    paging.pageIndex - page <= 5
                  ) {
                    counterpager = counterpager + 1;
                    return (
                      <button
                        type="button"
                        onClick={this.handelChangePage}
                        className={
                          "btnT btnT-primary  p-page " +
                          ((paging.pageIndex === page && "current") || "")
                        }
                        key={page}
                        dataid={page}
                      >
                        {page + 1}
                      </button>
                    );
                  }
                }
                // else {

                //}
              )}

              <button
                className={
                  "btnT-paging p-next " +
                  ((this.props.paging.pageIndex ===
                    this.props.paging.countPages - 1 &&
                    "disabled") ||
                    "")
                }
                onClick={this.handelPageNext}
                type="button"
              ></button>
              <button
                className={
                  "btnT-paging p-last " +
                  ((this.props.paging.pageIndex ===
                    this.props.paging.countPages - 1 &&
                    "disabled") ||
                    "")
                }
                onClick={this.handelPageLast}
                type="button"
              ></button>
            </div>
            <select
              onChange={this.gridDisplayingItemsCount}
              style={{
                width: "4%",
                margin: "2px 5px",
              }}
              ref={(node) => {
                if (node) {
                  node.style.setProperty("height", "20px", "important");
                }
              }}
            >
              <option value="10">10</option>
              <option value="20">20</option>
              <option value="50">50</option>
              <option value="100">100</option>
              <option value="200">200</option>
              <option value="500">500</option>
            </select>
            <div className="table-info pull-right">
              {this.PagingDisplayItems}
            </div>
          </div>
        ) :this.props.haveSecondFooter?   (
          <div className="table-footer"></div>
        ):
        (<div ></div>)
        
        }
      </div>
    );
  }

  handelApplyDateRangePicker = (picker, field) => {
    console.log(picker);
    let newForm = { ...this.state.form };
    newForm[field + "From"].value = moment(picker.startDate).format(
      "DD-MM-YYYY"
    );
    newForm[field + "To"].value = moment(picker.endDate).format("DD-MM-YYYY");

    newForm[field + "From"].date = picker.startDate;
    newForm[field + "To"].date = picker.endDate;

    this.setState({ form: newForm });

    //this.handelInputChange(e);
    this.handleDateRefresh();
  };

  handelPageRefresh = (flag) => {
  
  
    if (flag) {
     // this.props.gridConfigItem.gridDataFilter = {};
     this.props.paging.pageIndex=0;
     
    }

    
    const { paging } = this.props;
if(! (Object.keys(this.props.gridConfigItem.gridDataFilter).length === 0 && this.props.gridConfigItem.gridDataFilter.constructor === Object))
{
    this.state.filter=this.props.gridConfigItem.gridDataFilter;
    //this.props.haveFooter=this.props.gridConfigItem.haveFooter;
}

    this.props.onPageChange(
      paging.pageIndex,
       this.state.filter
    );
    return false;
  };

  handelChangePage = (e) => {
    const { paging } = this.props;
    if (paging.pageIndex !== parseInt(e.target.getAttribute("dataid")))
      this.props.onPageChange(
        parseInt(e.target.getAttribute("dataid")),
        this.state.filter
      );
  };

  handelPageNext = (e) => {
    const { paging } = this.props;
    if (paging.pageIndex < paging.countPages - 1)
      this.props.onPageChange(paging.pageIndex + 1, this.state.filter);
  };

  handelPagePrevious = (e) => {
    const { paging } = this.props;
    if (paging.pageIndex > 0)
      this.props.onPageChange(paging.pageIndex - 1, this.state.filter);
  };
  handelPageFirst = (e) => {
    const { paging } = this.props;
    if (paging.pageIndex !== 0) this.props.onPageChange(0, this.state.filter);
  };
  handelPageLast = (e) => {
    const { paging } = this.props;
    if (paging.pageIndex !== paging.countPages - 1)
      this.props.onPageChange(paging.countPages - 1, this.state.filter);
  };
  handleDateRefresh = (e) => {
    //this.handelInputChange(e);

    $("#ddFilterWrapper").removeClass("open");
    let filter = null;
    const { filters } = this.props;
    let form = { ...this.state.form };

    if (e && e.target.name !== "filter") {
      for (let item in form) {
        form[item].value = "";
      }

      this.setState({ filter: filter, form: form }, () =>
        this.props.onFilter(filter)
      );

      return;
    }

    filter = [];

    filters.forEach((item) => {
      switch (item.condition) {
        case "between":
          filter.push({
            condition: item.condition,
            column: item.field,
            from: form[item.field + "From"].value,
            to: form[item.field + "To"].value,
            type: item.type,
          });
          break;
        default:
          filter.push({
            condition: item.condition,
            column: item.field,
            value: form[item.field].value,
            type: item.type,
          });
      }
    });

    this.setState({ filter: filter }, () => this.props.onFilter(filter));
  };

  handelResetFilterClick = (e) => {
    $("#ddFilterWrapper").removeClass("open");

    event.preventDefault();
    let filter = null;
    const { filters } = this.props;
    let form = { ...this.state.form };

    if (e.target.name !== "filter") {
      for (let item in form) {
        form[item].value = "";
      }

      this.setState({ filter: filter, form: form }, () =>
        this.props.onFilter(filter)
      );

      return false;
    }
  };
  handelFilterClick = (e) => {
    $("#ddFilterWrapper").removeClass("open");

    event.preventDefault();
    let filter = null;
    const { filters } = this.props;
    let form = { ...this.state.form };

    filter = [];

    filters.forEach((item) => {
      switch (item.condition) {
        case "between":
          filter.push({
            condition: item.condition,
            column: item.field,
            from: form[item.field + "From"].value,
            to: form[item.field + "To"].value,
            type: item.type,
          });
          break;
        default:
          filter.push({
            condition: item.condition,
            column: item.field,
            value: form[item.field].value,
            type: item.type,
          });
      }
    });

    this.setState({ filter: filter }, () => this.props.onFilter(filter));

    return false;
  };

  gridDisplayingItemsCount = (e) => {
    //console.log(e);
    this.props.gridConfigItem.pageSize = e.currentTarget.value;
    this.props.onPageChange(0,       this.props.gridConfigItem.gridDataFilter
      );

    //this.handleLoadPageIntegration(0);
  };


  checkHeadersLength = () => {
    let sum = 0;
    this.props.headers.map((x) => (sum += x.value.length));
    if (sum >= 130) return true;
    else return false;
  };
}

export default AppGrid;
