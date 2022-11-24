import React from 'react';
import { Grid as KendoGrid, GridColumn as Column } from '@progress/kendo-react-grid';
import  { useContext,useState,useEffect} from 'react';
import { CustomColumnMenu } from '../ColumnMenu';
import useMyCurrentUI from '../../Hooks/useMyCurrentUI';
import axios from 'axios';
import { Col, Container, Row, Label, Card, CardBody, Button, ButtonToolbar, ButtonGroup } from 'reactstrap';

const AdFalconGrid = props  => {
   
   /* let childrenWithProps = React.Children.map(props.children, child => {
        // checking isValidElement is the safe way and avoids a typescript error too
        if (React.isValidElement(child)) {
           // if(child.props.RenderContextMenu)
            return React.cloneElement(child, { ColumnMenu: ColumnMenu });
           // return React.cloneElement(child, { ColumnMenu: ColumnMenu });
        }
        return child;
    });*/

          

       // debugger;
      

       // return 

      

 //checkbox column code start
 const selectionChange = (event) => {
   // debugger;
    const data = stateGridData.data.map(item=>{
        if(item[GridProps.keyName]=== event.dataItem[GridProps.keyName]){
            item.selected = !event.dataItem.selected;
        }
        return item;
    });
    stateGridData.data = data;
    setstateGridData({...stateGridData });

    if(props.CallBackParentSelected)
    {   let  selectedList= data.reduce((selectedListV, item) => {
     if (  item.selected ) {
         selectedListV.push(item);
     } 
     return selectedListV;
   }, []);
        
     props.CallBackParentSelected(selectedList);
    }
    //this.setState({ data });
}
const rowClick = event => {
   // debugger
    let last = lastSelectedIndex;
    const data = [...stateGridData.data];
    const current = data.findIndex(dataItem => dataItem === event.dataItem);

    if (!event.nativeEvent.shiftKey) {
        lastSelectedIndex = last = current;
    }

    if (!event.nativeEvent.ctrlKey) {
        data.forEach(item => (item.selected = false));
    }
    const select = !event.dataItem.selected;
    for (let i = Math.min(last, current); i <= Math.max(last, current); i++) {
        data[i].selected = select;
    }
    stateGridData.data = data;
    setstateGridData({...stateGridData });
    if(props.CallBackParentSelected)
   {   let  selectedList= data.reduce((selectedListV, item) => {
    if (  item.selected ) {
        selectedListV.push(item);
    } 
    return selectedListV;
  }, []);
       
    props.CallBackParentSelected(selectedList);
   }
};
const expandChange =(event)=>{

    event.dataItem.expanded = event.value;
    let recordId =  event.dataItem[GridProps.keyName];

    let data = stateGridData.data.slice();
    let index = data.findIndex(d => d[GridProps.keyName] === recordId);
    data[index].expanded =  event.value;
    setstateGridData({...stateGridData , data: data });

    if (!event.value || event.dataItem.details) {
        return;
    }


    
    axios({
        method: 'get',
        url: GridProps.APIurldetails+"?Id="+recordId ,
        headers: {
                "Accept": "application/json"
            }
        })
    .then(res => {

     
        let data = stateGridData.data.slice();
            let newData = res.data.Data.map(x => {
                return {
                   ...x,
                    selected:false,

                };
            })
            let index = data.findIndex(d => d[GridProps.keyName] === recordId);
            data[index].details = newData;
            setstateGridData({...stateGridData, data: data });
    }).catch(err => console.log('error', err));



};
const headerSelectionChange = (event) => {
    //debugger
    const checked = event.syntheticEvent.target.checked;
    const data = stateGridData.data.map(item=>{
        item.selected = checked;
        return item;
    });
    stateGridData.data = data;
    setstateGridData({...stateGridData });
    //this.setState({ data });

    if(props.CallBackParentSelected)
    {   let  selectedList= data.reduce((selectedListV, item) => {
     if (  item.selected ) {
         selectedListV.push(item);
     } 
     return selectedListV;
   }, []);
        
     props.CallBackParentSelected(selectedList);
    }
}

    const [GridProps, setGridProps] = useState({
        scrollable:false,
        reorderable:false,
        style:null,
         filters:null,
        filterable:false,
        sortable:false,
        pageable:false,
        APIurl:'',
        ContentType:'',
        keyName:'',
        EnableContextMenu:false,
        QuickActionTitle:"",
        QuickActionWidth:"200"
    });
    useEffect(() => {
       
        // debugger;
              GridProps.filters=props.filters;
              setGridProps({...GridProps});
              FillDataGrid(0,10);
      },[props.filters]);
      
    let index=0;
    let colsAdded=  props.children.reduce((cols, item) => {
      cols.push( {...item.props, id:index++}   
          );
      
          return cols;
       
         // 
        }, []);
    
    
    GridProps.EnableSelection=props.EnableSelection;
    GridProps.scrollable=props.scrollable;
    GridProps.reorderable=props.reorderable;
    GridProps.style=props.style;
    GridProps.filters=props.filters;
    GridProps.filterable=props.filterable;
    GridProps.sortable=props.sortable;
    GridProps.pageable=props.pageable;
    GridProps.APIurl=props.APIurl;
    GridProps.ContentType=props.ContentType;
    GridProps.keyName=props.keyName;
    GridProps.EnableContextMenu=props.EnableContextMenu;

    GridProps.QuickActionTitle=props.QuickActionTitle;
    GridProps.QuickActionWidth=props.QuickActionWidth;
 

    if(props.EnableSelection)
    {GridProps.selectedField="selected"
    GridProps.keyName =props.keyName ;  
    GridProps.onSelectionChange=selectionChange;
    GridProps.onHeaderSelectionChange=headerSelectionChange;
    GridProps.onRowClick=rowClick;
    }
    if(props.EnableExpand)
    {
        GridProps.detail=props.DetailComponent;
        GridProps.expandField="expanded";
        GridProps.onExpandChange=expandChange;
        GridProps.APIurldetails=props.APIurldetails;
    }
    if(props.EnableContextMenu)
    {
        GridProps.EnableContextMenu=true;
        GridProps.ContextMenu=props.ContextMenu;
     
    }

    GridProps.filters=props.filters;
    //setGridProps({...GridProps});

    
  
    const [XP, setXP] = useState(0);



    const [YP, setYP] = useState(0);
  
    const [openToolbarAction, setopenToolbarAction] = useState(false);
    const toggleToolbarAction= (index) =>{ setopenToolbarAction(!openToolbarAction);
        if(CurrentIndex!=index)
        setCurrentIndex(index) ;
        else
        setCurrentIndex(-1) ;
    
    }
    const {  getDirection, getLanguage, getTheme ,UI} = useMyCurrentUI();
    const [CurrentIndex, setCurrentIndex] = useState(-1);
    const RenderColumnAsDate=(props)=>{
        let newDateString =props.dataItem[props.field];
        if(newDateString && typeof (newDateString) !='undefined'  && newDateString.length>0)
        {
        let  newDate= new Date(props.dataItem[props.field]);
         newDateString = ("0" + newDate.getDate()).slice(-2) + "-" + ("0"+(newDate.getMonth()+1)).slice(-2) + "-" +
        newDate.getFullYear() ;
        }
    return(
        <td   className={props.className } style={{...props.style}}  >
             
             {newDateString}
               </td>
            
            )
     


}
const renderContextColumn = (props,contenxtMenu) =>{

        const id="QuickAction"+props.dataItem[GridProps.keyName];
        const idTab="QuickActionTab"+props.dataItem[GridProps.keyName];
        const idToolbarTab="QuickActiontoolbar"+props.dataItem[GridProps.keyName];
        return(
            <td id={idTab}  className={props.className + " k-command-cell"} style={{...props.style, overflow:'visible'}}  >

<div id={"QuickAction"+props.dataItem[GridProps.keyName]} onClick={e =>{  toggleToolbarAction(props.dataIndex) ; handleElementClick(e, id, idTab,idToolbarTab, props.dataIndex);} } style={{overflow:'visible'} }>
<ButtonToolbar>
<ButtonGroup className="btn-group--icons" dir="ltr">
<Button   color="primary"  outline id={"PopoverLegacy"} type="button" ><span className="lnr lnr-pushpin" /></Button> </ButtonGroup></ButtonToolbar>






</div>
{CurrentIndex== props.dataIndex  && getDirection()==="ltr" &&
( <div  id={"QuickActiontoolbar"+props.dataItem[GridProps.keyName]}  style={{visibility:  CurrentIndex== props.dataIndex  ? 'visible' :  'hidden' ,zIndex:43434334, margin: '0px', position: 'absolute', inset: '0px 0px auto auto', transform: `translate(${XP}px,${YP}px)`}}>
<ButtonToolbar>

<ButtonGroup className="btn-group--icons" dir="ltr">
    {contenxtMenu(props)}
{/* <Button outline><span className="lnr lnr-pushpin" /></Button>
<Button outline><span className="lnr lnr-heart-pulse" /></Button>
<Button outline><span className="lnr lnr-cog" /></Button>
<Button outline><span className="lnr lnr-magic-wand" /></Button>
 */}

</ButtonGroup>
</ButtonToolbar>
</div> )
}

{CurrentIndex== props.dataIndex  && getDirection()==="rtl" &&
( <div id={"QuickActiontoolbar"+props.dataItem[GridProps.keyName]} style={{visibility: CurrentIndex== props.dataIndex  ? 'visible' :  'hidden',zIndex:43434334, margin: '0px', position: 'absolute', inset: '0px auto auto 0px', transform: `translate(${XP}px,${YP}px)`}}>
<ButtonToolbar>

<ButtonGroup className="btn-group--icons" dir="rtl">

{contenxtMenu(props)}
{/* <Button outline><span className="lnr lnr-pushpin" /></Button>
<Button outline><span className="lnr lnr-heart-pulse" /></Button>
<Button outline><span className="lnr lnr-cog" /></Button>
<Button outline><span className="lnr lnr-magic-wand" /></Button> */}
</ButtonGroup>
</ButtonToolbar>
</div> )
}
</td>
        );
     };

const handleElementClick=(e,id,idTab,idtoolbar,dataIndex)=>
{


const component = document.getElementById(id);
const componentTab = document.getElementById(idTab);
if (!component) {
return {};
}
const rect = component.getBoundingClientRect();
const rect1 = componentTab.getBoundingClientRect();
const top2=document.getElementById(idTab).offsetTop;
const top1=document.getElementById(id).offsetTop;
if(getDirection()=="ltr")
setXP(-1*rect.width- document.getElementById(id).offsetLeft);
else
setXP(1*rect.width);
setYP(top1 );

//document.getElementById(idtoolbar).style.visibility = "visible";


}
   
let  lastSelectedIndex = 0;

const [stateGridData, setstateGridData] = useState({
    skip: 0, take: 10, data: [], Cols:colsAdded,Page:0

});

const pageChange = (event) => {
    if(event && event.page)
FillDataGrid(event.page.skip,event.page.take);
else
FillDataGrid(0,10);


}

const FillDataGrid =(skip ,take )=>{


//state.pending = JSON.stringify({skip:stateGridData.skip,take : stateGridData.take,Cols:stateGridData.Cols,checkedRecords:stateGridData.checkedRecords});
let axioConfig={};
GridProps.filters.Page=skip/take+1;
GridProps.filters.Size=take;

GridProps.filters.PageNumber=skip/take+1;
GridProps.filters.PagSize=take;
    if(GridProps.ContentType==='application/x-www-form-urlencoded')
    {
        var querystring = require('querystring')
   
        var data = querystring.stringify({...stateGridData, ...GridProps.filters});
        

        axioConfig={
            method: 'post',
            url:GridProps.APIurl,
            data: data,
            headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    "Accept": "application/json"
                }
            };
    }
    else
    {

        var querystring = require('querystring')
        state.filters.Page=skip/take+1;
       // var data = querystring.stringify({...stateGridData, ...GridProps.filters});
        
         axioConfig={
            method: 'post',
            url:GridProps.APIurl,
            data: data,
          
            };
    }

axios(axioConfig)
.then(res => {

    //state.lastSuccess = state.pending;
    //state.pending = '';

    //state.isRefresh = true;
    //debugger;
    stateGridData.data = res.data.Data.map(x => {
        return {...x,
            selected:false,
            
            expanded:false
        };
    })
    setstateGridData({...stateGridData ,Count:res.data.Total,skip:skip});


}
).catch(err => console.log('error', err));

}


const SaveColumns = (id, item) => {

    const newFormData = Object.assign({}, stateGridData);
    newFormData.Cols[id].locked = item.locked;
    setstateGridData(newFormData);
}
const ColumnMenu=(props)=>{


    return (
        <CustomColumnMenu
         {...pros} 
            columns={stateGridData.Cols}
            onGridMenuCallBack={SaveColumns}
        />
    )
}
    return <>
        <KendoGrid 
        

       

    
      
       
        
         data={stateGridData.data}
         skip={stateGridData.skip}
         take={stateGridData.take}
         total={stateGridData.Count}
      

         onPageChange={pageChange}

{...GridProps}
        >
                  { GridProps.EnableSelection&&  <Column
                                field="selected"
                                width="50px"
                                headerSelectionValue={
                                    stateGridData.data.findIndex(dataItem => dataItem.selected === false) === -1
                                } /> }

                                {
                
                stateGridData.Cols.map(item => {
                    //debugger;
                      if( item.renderAsDate ){
                        
                    



                    
                    return <Column  {...item}  cell={e=>RenderColumnAsDate(e)}  columnMenu={props => (
                        <CustomColumnMenu
                            {...props}  column={item}
                            columns={stateGridData.Cols}
                            onGridMenuCallBack={SaveColumns}
                        />

                    )}  />

                
                
                }
                    else

                    {

                        
                        return <Column  {...item}   columnMenu={props => (
                            <CustomColumnMenu
                                {...props}  column={item}
                                columns={stateGridData.Cols}
                                onGridMenuCallBack={SaveColumns}
                            />
    
                        )}  />

                    }

                    

                })

                                        
                                        
                                        
                                        }

{ GridProps.EnableContextMenu&&   

<Column locked cell={e=>renderContextColumn(e,GridProps.ContextMenu)} title={GridProps.QuickActionTitle} width={GridProps.QuickActionWidth} />
}
        </KendoGrid>


    </>;
};



  export function AdFalconColumn( props
 ){

    return( <Column {...props} />);


  };
export default AdFalconGrid;

    /*
                    stateGridData.Cols.map(item => {
											return <GridColumn  title={item.title} width={item.width} field={item.field}   locked={item.locked} columnMenu={props => (
												<CustomColumnMenu
													{...props}  column={item}
													columns={stateGridData.Cols}
													onGridMenuCallBack={SaveColumns}
												/>
											)}  />


										})*/