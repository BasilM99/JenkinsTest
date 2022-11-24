import React,{useState ,useEffect} from 'react';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import TrendingUpIcon from 'mdi-react/TrendingUpIcon';
import TrendingDownIcon from 'mdi-react/TrendingDownIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import LayersPlus from 'mdi-react/LayersPlusIcon';
import { Col, Container, Row, Button ,Card ,Progress,CardBody , Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, Form, FormGroup,ButtonToolbar} from 'reactstrap';
import LoadingSpinner ,{ Spinner} from './LoadingSpinner';


import ReactSelect from 'react-select';

import {
  BarChart, Bar, Cell, ResponsiveContainer,
} from 'recharts';
 const DashBoardCardCompnent = ({cardItem,newCard, KPILookupList,CallParent,t}) =>{
 
 let [CardsList, SetCardsList]=useState([]);
 let [KPIList, SetKPIList]=useState([]);
 const [KPISelected, SetKPISelected]=useState(null);
 const [DefaultSize, SetDefaultSize]=useState(4);
 const [MaxSize, SetMaxSize]=useState(10);
 const [UpdatedKPIList, SetUpdatedKPIList]=useState([]);
 const onDismiss =(e,id)=>{
    let  newCardList = CardsList.filter(x=> x.Id != id);
    CardsList=newCardList;
    SetCardsList(newCardList);
    filterKPIList();

 }
useEffect(

    ()=>{
        if(typeof(cardItem) != "undefined")
       {
           let newItems= cardItem.slice(0, DefaultSize);
           CardsList=newItems;
           SetCardsList(newItems);
    
    }

    debugger;
    if(typeof(KPILookupList) != "undefined")
    {KPIList=KPILookupList;
        SetKPIList(KPILookupList);
        filterKPIList();
    }


    },[]

);

useEffect(

    ()=>{

        debugger;
        if(typeof(KPILookupList) != "undefined")
    {KPIList=KPILookupList;
        SetKPIList(KPILookupList);
        filterKPIList();
    }
    


    },[KPILookupList]

);


useEffect(

    ()=>{
        if(typeof(cardItem) != "undefined")
       {
           let newItems= cardItem.slice(0, MaxSize);
           CardsList=newItems;
           SetCardsList(newItems);
           filterKPIList();
    }
    


    },[cardItem]

);
useEffect(

    ()=>{
        if(typeof(newCard) != "undefined" && !!newCard )
       {

        let newarr = [];
						for(var i=0; i<CardsList.length; i++)
						{
							newarr.push(CardsList[i]);
                        }
                        let newObj= Object.assign({}, newCard)
                         newarr.push(newObj);
           let newItems= newarr.slice(0, MaxSize);
           CardsList=newItems;
           SetCardsList(newItems);
           filterKPIList();
    }
    


    },[newCard]

);
const filterKPIList=()=>{

    let  newKPIList=  KPIList.filter(item  => !CardsList.some(val => parseInt(val.Id )=== parseInt(item.ID)));

	let  List = [];
debugger;
    newKPIList.map(x => {
        List.push({
							label: x.TextDesc,
							value: x.ID
						});
					})


    SetUpdatedKPIList(List);

}

    const CardShort = ( ) =>{

        var returnedList = [];
        //debugger;
        
        var percentValueStateClass = '';
        if(typeof(CardsList) != "undefined")
        CardsList.map(_props => {
           
            
            returnedList.push(

                <Col md={12} xl={3} lg={6} xs={12}>
                <Card>
                  <CardBody>

                  <div className="panel__btns">
             
        
             <button className="panel__btn" type="button"  disabled={CardsList.length==MaxSize || UpdatedKPIList.length ==0 }  onClick={(e) => { e.preventDefault();
    onDismiss(e, _props.Id);
}}   ><CloseIcon /></button>
           </div>

                    <div className="card__title">
                      <i class={_props.Icon}></i> 
                      <h5>{_props.Title}</h5>
                    </div>
                    <div className="dashboard__total">
                      <p className="dashboard__total-stat" style={{fontSize : 20}}>
                        {_props.MainValue}
                      </p>
                      {(_props.PercentValueState == 'up')?<TrendingUpIcon className="dashboard__trend-icon" />
                        :<TrendingDownIcon className="dashboard__trend-icon" />}
                      <div className="dashboard__chart-container">
                        <div className="progress-wrap progress-wrap--small progress-wrap--pink-gradient progress-wrap--label-top">
                            <Progress value={_props.PercentValue  && _props.PercentValue.length > 0? _props.PercentValue.substring(0,_props.PercentValue.indexOf('%')) :0} ><p className="progress__label">{_props.PercentValue}</p></Progress>
                        </div>
                      </div>
                    </div>
                  </CardBody>
                </Card>
              </Col>
            );
        });

        return returnedList;
    } 
    const handleChangeForKPIList=(Value) =>{
    
     
        // setValue('Country', Value.label, { shouldValidate: true })
        SetKPISelected(Value);
     
     };
const CallKPISelection= ()=>{
CallParent(KPISelected.value);
SetKPISelected(null);
}
    const KPICardAdd = ( ) =>{

        var returnedList = [];
        //debugger;
        
        var percentValueStateClass = '';
        if(typeof(CardsList) != "undefined")
     
{
              return <>  <Col md={12} xl={3} lg={6} xs={12}>
                <Card>
                  <CardBody>

    

                    {/* <div className="card__title">
                      <i class={_props.icon}></i> 
                      <h5>{_props.title}</h5>
                    </div> */}
                    <div className="dashboard__total">
                     
                    <ReactSelect
                  // key={KPISelected}
                   placeholder={t("Global:KPISelect")}
                   onChange={(e)=>{handleChangeForKPIList(e); }}
                 value={KPISelected} 
                   className="react-select"
                   classNamePrefix="react-select"
                  // defaultValue={colourOptions[0]}
                  // isDisabled={isDisabled}
                  // isLoading={isLoading}
                  //isLoading
                   isClearable={true}
                  // isRtl={isRtl}
                   isSearchable={true}
                   name="KPIList"
                   options={UpdatedKPIList}
                  
                  
                   /> 
 <Button
                  color="primary" className="rounded"
                   sm
                    type="button"
                    onClick={(e) => {
                      e.preventDefault();
                      if(KPISelected&& KPISelected.value )
                      CallKPISelection();
                    }}
                  >
                    <LayersPlus />
                  </Button>
                    </div>
                  </CardBody>
                </Card>
              </Col>
              </>
}
else
{
return '';

}
          }
        

   
    
 
    
    return <>
   <Spinner area="CardsListKPI" > </Spinner>
     {CardShort()} 
     {
     KPICardAdd()
     }
    </>
}


export default DashBoardCardCompnent;