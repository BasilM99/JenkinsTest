import React from "react";
import {
  GridColumnMenuSort,
  GridColumnMenuFilter,
  GridColumnMenuItemGroup,
  GridColumnMenuItem,
  GridColumnMenuItemContent
} from "@progress/kendo-react-grid";
import { Checkbox } from "@progress/kendo-react-inputs";

export class CustomColumnMenu extends React.Component {
  state = {
    columns: this.props.columns,
    column: this.props.column
  };

  onToggleColumn = (item )=> {

    const newFormData = Object.assign({}, this.state);
    newFormData.column.locked= !item.locked
    this.setState(
        newFormData
    
      );
      this.props.onGridMenuCallBack(newFormData.column.id,newFormData.column);
   /* this.setState({
      columns: this.state.columns.map((column, idx) => {
        return idx === id ? { ...column, locked: !column.locked } : column;
      })
    });*/

    
  };

  onReset = event => {
     // debugger;
    event.preventDefault();
    const allColumns = this.props.columns.map(col => {
      return {
        ...col,
        locked: false
      };
    });

    this.setState({ columns: allColumns }, () => this.onSubmit());
  };

  onSubmit = event => {
    if (event) {
      event.preventDefault();
    }
    this.props.onColumnsSubmit(this.state.columns);
    if (this.props.onCloseMenu) {
      this.props.onCloseMenu();
    }
  };

  render() {
    return (
      <div>
  {/*       <GridColumnMenuSort {...this.props} />
        <GridColumnMenuFilter {...this.props} /> */}
        <GridColumnMenuItemGroup>
          <GridColumnMenuItem
            title={"Pin Column"}
            iconClass={"k-i-lock"}
            onClick={this.onMenuItemClick}
          />
          <GridColumnMenuItemContent show={true}>
            <div className={"k-column-list-wrapper"}>
            
                <div className={"k-column-list"}>
                
                    <div  className={"k-column-list-item"}>
                      <span>
                        <Checkbox
                          value={this.props.column.locked}
                          onChange={() => {
                            this.onToggleColumn(this.props.column);
                          }}
                          label={this.props.column.title}
                        />
                      </span>
                    </div>
               
                </div>
              
         
            </div>
          </GridColumnMenuItemContent>
        </GridColumnMenuItemGroup>
      </div>
    );
  }
}
