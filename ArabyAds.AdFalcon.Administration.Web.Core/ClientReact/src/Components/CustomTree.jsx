import * as React from "react";
import  { useContext,useState,useEffect,useRef} from 'react';
import $ from 'jquery';

import {
  TreeView,
  processTreeViewItems,
  handleTreeViewCheckChange
} from "@progress/kendo-react-treeview";

const CustomTree = (props) => {
  const treed = [
    {
      text: "Item1",
      expanded: true,
      items: [
        { text: "Item1.1" },
        {
          text: "Item1.2",
          expanded: true,
          items: [{ text: "Item1.2.1" }, { text: "Item1.2.2" }]
        }
      ]
    },
    { text: "Item2" },
    { text: "Item3" }
  ];
  const [state,setState] = useState({
    items:[],
    singleMode: false,
    checkChildren: true,
    checkParents: false,
    check: { ids: [], idField:'attributes.id', applyCheckIndeterminate: true }
  });
  useEffect(() => {

    //#region :: Load Advertisers List
   // convertTreeToArray(props.data, treeArrayData);

   
   //state.items=props.data;
   const newFormData = Object.assign({}, state);
   newFormData.items=props.data;

   let tempIds =[];
						
   props.check.forEach((x,index) => tempIds.push(x+""));

   newFormData.check.ids=tempIds;
  setState(newFormData);

 
  // setState(state);
  
},[props.data,props.check]);
 

  const handleSearch = () => {
    let value = document.querySelector(".k-textbox").value;
    let newData = search(props.data, value);
    //setState({ items: newData });
    const newFormData = Object.assign({}, state);
    newFormData.items=newData;
   setState(newFormData);
  };

 const search = (items, term) => {
    return items.reduce((acc, item) => {
      if (item.text.toLowerCase().indexOf(term.toLowerCase()) >= 0) {
        acc.push(item);
      } else if (item.items && item.items.length > 0) {
        let newItems = search(item.items, term);
        if (newItems && newItems.length > 0) {
          acc.push({
            text: item.text,
            items: newItems,
            expanded: item.expanded
          });
        }
      }
      return acc;
    }, []);
  };
 const contains = (text, term) => {
    return text.toLowerCase().indexOf(term.toLowerCase()) >= 0;
  };

  const onCheckChange = event => {
    if(event.item.attributes.state=='locked')
    return;

    const { singleMode, checkChildren, checkParents } = state;
    const settings = { singleMode, checkChildren, checkParents };
    const newFormData = Object.assign({}, state);
    newFormData.check=handleTreeViewCheckChange(
      event,
      newFormData.check,
      newFormData.items,
      settings
    )
    
     let newcheckSIngle=false;
    
    if (
      props.singleCheckElem &&
      props.singleCheckElem.length > 0 &&
      props.singleCheckElem.includes(event.item.attributes.id + "")
    ) {
      // console.log(event);
     
      if ( newFormData.check &&  newFormData.check.ids &&  newFormData.check.ids.length > 0) {
        let arrnew =  newFormData.check.ids.filter(function(item) {
          let value = props.singleCheckElem.includes(item+"");
          return !value;
        });

        if (event.item && !event.item.checked) {
          arrnew.push(event.item.attributes.id + "");
          newcheckSIngle=true;
        }
        newFormData.check.ids = arrnew;
      }
    }


    if (
      props.singleCheckElemParent &&
      props.singleCheckElemParent.length > 0 
      )
     {
    
      if ( newFormData.check &&  newFormData.check.ids &&  newFormData.check.ids.length > 0) {
        let arrnew =  newFormData.check.ids.filter(function(item) {
          let value = props.singleCheckElemParent.includes(item+"");
          return !value;
        });

       /* if (event.item && !event.item.checked) {
          arrnew.push(event.item.id + "");
        }*/
        if(newcheckSIngle)
        newFormData.check.ids = arrnew;
      }

    }
    //  newFormData.items=props.data;
      //state.items=;
    setState(newFormData);
      if(!(typeof  props.CallBackParent  === 'undefined') )
    props.CallBackParent(newFormData.check.ids);
  };
 const  onExpandChange = (event) => {
    event.item.expanded = !event.item.expanded;
   // this.forceUpdate();
}
 const treeContainerClick = () =>{
   //debugger;
   setTimeout(() => {
     
     $('#dropdownMenuTree_menuContainer').addClass("show");
   }, 100);
 }
   
 
 const treeContainerMouseLeave = () =>{
  //debugger;
    
    $('#dropdownMenuTree_menuContainer').removeClass("show");
  
}
   
  return <>
  

    <div className="form-group">
        <div class="col-md-12">
          <input className=" form-control k-textbox dropdown-toggle"  id="dropdownMenuTree" onChange={handleSearch} />
        </div>
        <div className="" >
          
          <TreeView   

          //focusIdField="attributes.id"

          //expandField: PropTypes.Requireable<string>;
          //selectField: PropTypes.Requireable<string>;
          //hasChildrenField="children" 


          //disableField="children


              data={processTreeViewItems(state.items, {
                check: state.check
              })}
              expandIcons={true}
              checkboxes={true}
              onCheckChange={onCheckChange}
              
              onExpandChange={onExpandChange}
            />
          </div>
    </div> 
    
  </>
}
export default CustomTree;