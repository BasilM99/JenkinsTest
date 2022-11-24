import { Col, Container, Row, Button ,Card ,CardBody , Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, Form, FormGroup,ButtonToolbar} from 'reactstrap';
import { Field, reduxForm ,change} from 'redux-form';
import React, { useCallback, useMemo ,useState,useEffect} from 'react';
import { connect } from 'react-redux';
import { required, email } from 'redux-form-validators'
import renderAsyncSelectField from '../../shared/components/form/AsyncSelect';
import {AsyncSelectField} from '../../shared/components/form/AsyncSelect';
import { useForm,Controller } from "react-hook-form";
import  yup from "../../Config/YUP"
import AsyncReactSelect from 'react-select/async';
import ReactSelect from 'react-select';

import { yupResolver } from '@hookform/resolvers/yup';
import  reacthookformValues from "../../Config/reacthookform"
import axios from 'axios';
import { useSelector, useDispatch } from 'react-redux';
import {  userInitialized, userRoleInitialized,ChangeUserRole ,UserSelectors } from '../../Store/ReducerSlices/User';
import Constants from '../../Config/Constants';
import { BasicNotification,showBasicNotificationForAdFalcon } from '../../Components/Notification';
import { withTranslation } from 'react-i18next';
import {CheckBoxField} from '../../shared/components/form/CheckBox';

import {RadioButtonField} from '../../shared/components/form/RadioButton';
import AccountOutlineIcon from 'mdi-react/AccountOutlineIcon';
import mdiCurrencyUsdIcon  from 'mdi-react/CurrencyUsdIcon';
import WebIcon from 'mdi-react/WebIcon';
import KeyVariantIcon from 'mdi-react/KeyVariantIcon';
import {MessageType} from '../../Config/Enums';
import {CustomSingleDatePicker} from '../../Components/CustomDateRangePicker';

let AddEditDeal= (props) => {


  const validationSchemayup = 
  yup.object().shape({
          Name: yup.string().required(),
          DealID: yup.string().required(),
          Exchange: yup.object().nullable().required(),
          Price:   yup.number().limitdecimalto3("limited to 3 decimal places"),
          PublisherName:  yup.string().required(),
          StartDate :Yup.date()
    
        .format('DD-MM-YYYY', true),
        EndDate :Yup.date()
    
        .format('DD-MM-YYYY', true),
  
        });
  const [DealID, setDealID] =useState(0);
  const [AdvertiserId, setAdvertiserId] =useState(0);

  const [ExchangeList, setExchangeList] =useState([]);
  const [AdSizeList, setAdSizeList] =useState([]);
  const [CountryList, setCountryList] =useState([]);
  const [adSizeIsDisabled, setadSizeIsDisabled] =useState(true);
  const [adSizeFormatTypes, setadSizeFormatTypes] =useState(true);
 // const [CountryList, setCountryList] =useState([]);

  const getExchangeList = async () =>{
		var tempList= [];
	
        let data = {
            language: "en",
            SSpartyGrid_size: 20,
            Prefix: "",
            id: "ssppartner",
        }
        
        var querystring = require('querystring')
        data = querystring.stringify(data);
    let response =  await   axios({
            method: 'post',
            url: Constants.backend.getExchangeList ,
            data: data,
            headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    "Accept": "application/json"
                }
            });


            if(response.data){
              tempList =  response.data.map( x => {
                return {
                  label: x.data,
                  value: x.attributes.Id
                }
              });
        
                          setExchangeList(tempList);
              
            }
	
	
  }
  
useEffect(()=>{

setAdvertiserId(props.AdvertiserId);


setDealID(props.DealID);
DealID=props.DealID;
GetDealData();
},[props.DealID,props.AdvertiserId])


useEffect(()=>{

  Promise.allSettled([getAdSizeListAll(),getCountryList(),getExchangeList()]).then(Res=> {
  
    if(props.AdvertiserId>0)
    {
     await setAdvertiserId(props.AdvertiserId);

    }
    if(props.DealID>0)
    {
      await setDealID(props.DealID);
      GetDealData();

    }

  
  
  }
    )



  },[]);


  const adSizeFormatGroupLabel = data => (
    <div style={groupStyles} >
      <span>{data.label}</span>
      <span style={groupBadgeStyles}>{data.options.length}</span>
    </div>
  );
const [CountryValue,setCountryValue] = useState([]);
  const CountryGroupLabel = data => (
    <div style={groupStyles}  onClick={() => {
      setCountryValue((value) => {
        if (!value) value = [];


        let objectalreadyexists = value.find((item) => item.value == data.value);

              if (objectalreadyexists != null) {
                value = value.filter((item) => item.value != data.value);
                setValue('CountryList',value,{shouldValidate:true});
                return value;
              }  
        let filteredItems = value.filter(
          (item) => !data.options.includes(item)
        );

        filteredItems = filteredItems.filter(
          (item) => item.value !== data.value
          //!filteredItems.includes({ label: groupName, value: Val })
        );

        filteredItems = filteredItems.concat([
          { label: groupName, value: data.value }
        ]);
        // value.concat(options.filter((grpOpt) => !value.includes(grpOpt)));
        value = filteredItems;
        setValue('CountryList',value,{shouldValidate:true});
        return value;
      });
    }}   >
      <span>{data.label}</span>
      <span style={groupBadgeStyles}>{data.options.length}</span>
    </div>
  );
  const onAdFormatChanged = () =>{

    //let adSizeFormatTypes =  [];
    let bannerType = getValues("AdFormatBanner") ? "1":"";
    let nativeAdType = getValues("AdFormatInStream") ? "2":"";
    let inStreamVideoType =  getValues("AdFormatNative") ? "3":"";
    adSizeFormatTypes=[bannerType ,nativeAdType,inStreamVideoType  ];
    setadSizeFormatTypes([bannerType ,nativeAdType,inStreamVideoType  ]);

    if(getValues("AdFormatBanner")  ||  getValues("AdFormatInStream")  ||   getValues("AdFormatNative")){
      setadSizeIsDisabled(false) ;
        getAdSizeList();
       // setLocalState({...state})
    }else{
      setadSizeIsDisabled(true) ;
        getAdSizeList();
        //setLocalState({...state})
    }
}
  const getAdSizeList = () =>{
		var tempList= [];
        axios({
            method: 'post',
            url: Constants.backend.getAdSizeList + adSizeFormatTypes.join(',') ,
            headers: {
                    'Content-Type': 'application/json',
                    "Accept": "application/json"
                }
            })
		.then(success => {
			if(success.data){
                if(success.data.tree.length > 0){
                    tempList = success.data.tree.map(x => {
                        let tempObj = {};
                        tempObj.label = x.Name.Value;
                        tempObj.options = x.Childs.map(c => {
                            let tempSubOjb = {};
                            tempSubOjb.value= c.Id;
                            tempSubOjb.label= c.Name.Value;
                            return tempSubOjb;
                        })
                        return tempObj;
                    
                    })
                }
				
                //state.adSizeList = tempList;
               
                setAdSizeList(tempList);
               
			}
		})
		.catch(err => console.log('error', err));
	
  }

  const getAdSizeListAll = async () =>{
		var tempList= [];
        let suncess = await axios({
            method: 'post',
            url: Constants.backend.getAdSizeList + "1,2,3" ,
            headers: {
                    'Content-Type': 'application/json',
                    "Accept": "application/json"
                }
            });

    
    if(success.data){
      if(success.data.tree.length > 0){
          tempList = success.data.tree.map(x => {
              let tempObj = {};
              tempObj.label = x.Name.Value;
              tempObj.options = x.Childs.map(c => {
                  let tempSubOjb = {};
                  tempSubOjb.value= c.Id;
                  tempSubOjb.label= c.Name.Value;
                  return tempSubOjb;
              })
              return tempObj;
          
          })
      }

      //state.adSizeList = tempList;
     
      setAdSizeList(tempList);
     
}
  }
  const GetDealData =()=>{

    axios({
      method: 'get',
      url: Constants.backend.GetDealData +"?id"+DealId
    
    }
   )
.then(success => {
if(success.data)
{

 let AdFormatBanner=false;
    if(success.data.PMPTargetingSaveDto.AdFormats.includes(1))
    {
      AdFormatBanner=true;

    }
   let  AdFormatInStream=false;
    if(success.data.PMPTargetingSaveDto.AdFormats.includes(2))
    {
      AdFormatInStream=true;
      
    }

    let AdFormatNative=false;
    if(success.data.PMPTargetingSaveDto.AdFormats.includes(3))
    {
      AdFormatNative=true;
      
    }
    let countryListVar =[];
    if(!!PMPTargetingSaveDto.Geographies && PMPTargetingSaveDto.Geographies.length>0)
    {  
      
        countryListVar=  CountryList.filter(item  => !PMPTargetingSaveDto.Geographies.some(val => parseInt(val )=== parseInt(item.value)));


      
    
    }

    setCountryValue(countryListVar);

    let ExVar =null;
    if(!!PMPTargetingSaveDto.Geographies && PMPTargetingSaveDto.Geographies.length>0)
    {  
      
      ExVar=  ExchangeList.find(item  =>  parseInt(item.value)== parseInt(success.data.ExchangeId));


      
    
    }

    let AdSizesVar =[];

    if(!!PMPTargetingSaveDto.AdSizes && PMPTargetingSaveDto.AdSizes.length>0)
    {  
      
      let  AdSizesVar=  AdSizeList.filter(item  => !PMPTargetingSaveDto.AdSizes.some(val => parseInt(val )=== parseInt(item.value)));


      
    
    }
   // let datatoBeSent={ID:DealID,Name:dataInJson.Name,Price:dataInJson.Price,DealID:dataInJson.DealID,ExchangeId:dataInJson.Exchange.value,Note:dataInJson.Note,Description:dataInJson.Description,Type:dataInJson.Type,IsGlobal:dataInJson.IsGlobal,StartDate:dataInJson.StartDate,EndDate:dataInJson.EndDate,PublisherName:dataInJson.PublisherName,PMPTargetingSaveDto:{Geographies:CountryListArr,AdFormats:adSizeFormatTypesArr,AdSizes:AdSizeArr,RawAdFormats:adSIzeFormatType}};

      reset({PublisherName : success.data.PublisherName,  DealID:success.data.DealID,Note:success.data.Note,Type:success.data.Type,Name:success.data.Name,StartDate:new moment(new Date(success.data.StartDate)) ,EndDate:new moment(new Date(success.data.EndDate)),Price:success.data.Price,Description:success.data.Description,Exchange:ExVar,CountryList:countryListVar,AdSize:   AdSizesVar   , AdFormatBanner:AdFormatBanner,AdFormatNative:AdFormatNative,AdFormatInStream:AdFormatInStream ,IsGlobal:IsGlobal})  ;
         
}
})
.catch(err => console.log('error', err));

  }

  const getCountryList = async () =>{
		var tempList= [];
     let response = await   axios({
            method: 'post',
            url: Constants.backend.GetTreeDataForCountry ,
            headers: {
                    'Content-Type': 'application/json',
                    "Accept": "application/json"
                }
            });

    
    if (response.data)
      {
    
          if(response.data.tree.length > 0){
              tempList = response.data.tree.map(x => {
                  let tempObj = {};
                  tempObj.label = x.Name.Value;
                  tempObj.value=x.Id;
                  tempObj.options = x.Childs.map(c => {
                      let tempSubOjb = {};
                      tempSubOjb.value= c.Id;
                      tempSubOjb.label= c.Name.Value;
                      return tempSubOjb;
                  })
                  return tempObj;
              
              })
          }
  
          //state.adSizeList = tempList;
         
          setCountryList(tempList);
         
}

      
  }
  
   
   
    reacthookformValues.resolver=yupResolver(validationSchemayup);
    const { handleSubmit, errors, register,control,setValue, reset,setError,getValues  ,formState: {isValid , isDirty, isSubmitting, touched, submitCount }} = useForm({...reacthookformValues   });
    useEffect(() => {
		
    reset();
    },[props.collapseAddEditDeal]);

    
    const SaveAddEditDeal = (data, e) =>{
       

        baseUrl = Constants.backend.CreateDeal;

      var querystring = require('querystring');
      var dataInJson = getValues();
      
      let CountryListArr=[];
      if(!!dataInJson.CountryList && dataInJson.CountryList.length>0)
      {CountryListArr= dataInJson.CountryList.map(function(e){
        return (e.value);
      });
    }

    let AdSizeArr=[];
    if(!!dataInJson.AdSize && dataInJson.AdSize.length>0)
    {AdSizeArr= dataInJson.AdSize.map(function(e){
      return (e.value);
    });
  }

  let adSIzeFormatType= adSizeFormatTypes.join(',')

  let adSizeFormatTypesArr=adSizeFormatTypes;
     // var data = querystring.stringify({checkedRecords:state.idsToArchive});
     let datatoBeSent={ID:DealID,Name:dataInJson.Name,Price:dataInJson.Price,DealID:dataInJson.DealID,ExchangeId:dataInJson.Exchange.value,Note:dataInJson.Note,Description:dataInJson.Description,Type:dataInJson.Type,IsGlobal:dataInJson.IsGlobal,StartDate:dataInJson.StartDate,EndDate:dataInJson.EndDate,PublisherName:dataInJson.PublisherName,PMPTargetingSaveDto:{Geographies:CountryListArr,AdFormats:adSizeFormatTypesArr,AdSizes:AdSizeArr,RawAdFormats:adSIzeFormatType}};
     if(!!AdvertiserId && parseInt(AdvertiserId)>0)
     datatoBeSent.AdvertiserAccountId =AdvertiserId;
    
     axios({
          method: 'post',
          url:baseUrl,
          data:data,
          headers: {
                  "Accept": "application/json"
              }
          })
       .then(success => {
         if(success.data && success.data.status=="businessException"){
        
       

      
         setError('serverError', {message:success.data.Message});

 
         }
         else if(success.data.Mesessges && success.data.Mesessges.length>0)
         {
          success.data.Mesessges.forEach(function(entry) {
           // console.log(entry);

           if(entry.Type == MessageType.Success)
           {

         
         
            showBasicNotificationForAdFalcon("Deal Create",entry.Message,"success","right-up");
           }
           if(entry.Type == MessageType.Error)
           {

         
         
            showBasicNotificationForAdFalcon("Deal Create",entry.Message,"danger","right-up");
           }
           if(entry.Type == MessageType.Warning)
           {

         
         
            showBasicNotificationForAdFalcon("Deal Create",entry.Message,"warning","right-up");
           }

           if(entry.Type == MessageType.Information)
           {

         
         
            showBasicNotificationForAdFalcon("Deal Create",entry.Message,"","right-up");
           }

        });  

          if(!!success.data.Id && parseInt(success.data.Id)>0)
          {
            setDealID(parseInt(success.data.Id));

          }
         
           toggle();
         }
       })
       .catch(err => 
          { 
           throw err;
  
           console.log('error', err)
          }
       );
     
       
   }
   
const toggle =()=>{


  reset();
  props.CallBackToggle();
}
const onKeyDown = (e) => {
  const decimal_index = e.target.value.indexOf('.');
  if(decimal_index > -1){
      var decimals = e.target.value.substring(decimal_index, e.target.value.length+1);
      if(decimals.length > 4 && e.keyCode !== 8){
         e.preventDefault();
      }
      //this.props.onChange();
  }
}
  return (
    <form  className="form" onSubmit={handleSubmit(SaveAddEditDeal)}>
       

            
              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Global:Name')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input
                name="Name"
                type="input"
                placeholder={props.t('Global:Name')}
                innerRef={register}
                //validate={[ required() ]}
                />
    {errors.Name  && <span className="form__form-group-error">{errors.Name.message}</span>}  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('PMPDeal:DealID')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input
                name="DealID"
                type="input"
                placeholder={props.t('PMPDeal:DealID')}
                innerRef={register}
                //validate={[ required() ]}
                />
    {errors.DealID  && <span className="form__form-group-error">{errors.DealID.message}</span>}  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('SSPFloorPrices:Price')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input
                name="Price"
                type="number"
                onKeyDown={onKeyDown}

                placeholder={props.t('SSPFloorPrices:Price')}
                innerRef={register}
                //validate={[ required() ]}
                />     <div className="form__form-group-icon">
                <mdiCurrencyUsdIcon />
              </div>
    {errors.Price  && <span className="form__form-group-error">{errors.Price.message}</span>}  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('PMPDealTargetings:AdSize')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
            
                <RadioButtonField 
                                                          //  onChange={onArchiveCheckChange}
                                                            name="Type"
                                                            radioValue={1}
                                                            label={t('PMPDeal:PrivateAuction')}
                                                            innerRef={register}
                                                             />
                                                                 <RadioButtonField
                                                          //  onChange={onArchiveCheckChange}
                                                            name="Type"
                                                           // value={state.filters.showArchived}
                                                           
                                                           radioValue={2}
                                                           innerRef={register}
                                                            label={t('PMPDeal:Fixed')}
                                                             />
                                                        
                                                          
  </div>
                </div>
              </div>


      
              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('PMPDeal:Exchange')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Controller
                    name="Exchange"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                    
                    <ReactSelect
                   
                    placeholder={props.t('PMPDeal:SelectExchangeT')}
                    onChange={(e)=>{onChange(e); }}
                    value={value} 
                    className="react-select"
                    classNamePrefix="react-select"
                   // defaultValue={colourOptions[0]}
                   // isDisabled={isDisabled}
                   // isLoading={isLoading}
                   //isLoading
                    isClearable={true}
                   // isRtl={isRtl}
                    isSearchable={true}
                    name="Exchange"
                    options={ExchangeList}
                   
                   
                    />  }
                    />
    {touched && errors && errors.Exchange  && <span className="form__form-group-error">{errors.Exchange.message}</span>}  </div>
                </div>
              </div>



              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('PMPDeals:PublisherName')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input
                name="PublisherName"
                type="input"
                placeholder={props.t('PMPDeals:PublisherName')}
                innerRef={register}
                //validate={[ required() ]}
                />
    {errors.PublisherName  && <span className="form__form-group-error">{errors.PublisherName.message}</span>}  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Campaign:StartDate')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
              

<Controller
                    name="StartDate"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                   
                    <CustomSingleDatePicker                  placeholder={props.t('Campaign:StartDate')}
                     CallBackParent={onChange} id='StartDate'     Datevalue={value}    nameDate={props.t('Campaign:StartDate')} titleMsgDate={props.t('Campaign:StartDate')} ></CustomSingleDatePicker>
                  
                  }
                    />

    {errors.StartDate  && <span className="form__form-group-error">{errors.StartDate.message}</span>}  </div>
                </div>
              </div>
              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Campaign:EndDate')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">


                <Controller
                    name="EndDate"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                   
                    <CustomSingleDatePicker                  placeholder={props.t('Campaign:EndDate')}
                     CallBackParent={onChange} id='EndDate'     Datevalue={value}    nameDate={props.t('Campaign:EndDate')} titleMsgDate={props.t('Campaign:EndDate')} ></CustomSingleDatePicker>
                  
                  }
                    />


              
    {errors.EndDate  && <span className="form__form-group-error">{errors.EndDate.message}</span>}  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Campaign:Description')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input type="textarea"
                name="Description"
                
                placeholder={props.t('Campaign:Description')}
                innerRef={register}
                //validate={[ required() ]}
                />
  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Campaign:Notes')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input type="textarea"
                name="Notes"
                
                placeholder={props.t('Campaign:Notes')}
                innerRef={register}
                //validate={[ required() ]}
                />
  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Global:Country')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Controller
                    name="Country"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                    
                    <ReactSelect
                   
                    placeholder={props.t('Global:CountrySelect')}
                    onChange={(e)=>{onChange(e); setCountryValue(e);}}
                    value={value} 
                    className="react-select"
                    classNamePrefix="react-select"
                    formatGroupLabel={CountryGroupLabel}
                    isClearable={true}
                isMulti
                    isSearchable={true}
                    name="Country"
                    options={CountryList}
                   
                   
                    />  }    />       


  </div>
                </div>
              </div>



              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('PMPDealTargetings:AdSize')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
            
                <CheckBoxField 
                                                            onChange={onAdFormatChanged}
                                                            name="AdFormatBanner"
                                                           // value={state.AdFormatBanner} 
                                                           innerRef={register}
                                                            label={t('PMPDeals:Banner')}
                                                             />
                                                                 <CheckBoxField
                                                            onChange={onAdFormatChanged}
                                                            name="AdFormatInStream"
                                                           // value={state.filters.showArchived}     
                                                           
                                                           innerRef={register}
                                                            label={t('NativeAd:NativeAdName')}
                                                             />
                                                               <CheckBoxField
                                                            onChange={onAdFormatChanged}
                                                            name="AdFormatNative"     
                                                            innerRef={register}
                                                            //value={state.filters.showArchived}
                                                            label={t('InstreamVideo:InstreamVideoName')}
                                                             />
                                                          
  </div>
                </div>
              </div>



              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('PMPDealTargetings:AdSize')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Controller
                    name="AdSize"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                    
                    <ReactSelect
                   
                    placeholder={props.t('PMPDealTargetings:AdSize')}

                    value={value} 
                    className="react-select"
                    classNamePrefix="react-select"
                  
                   onChange={(e)=>{onChange(e); }}

                   formatGroupLabel={adSizeFormatGroupLabel}
                    isClearable={true}
               isDisabled={adSizeIsDisabled}
                    isMulti={true}
                    isSearchable={true}
                    name="AdSize"
                    options={AdSizeList}
                   
                   
                    />  }    />       


  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label"></span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
              
     { errors.serverError  && <span className="form__form-group-error">{errors.serverError.message}</span>} </div>
                </div>
              </div>
      
        <ButtonToolbar className="form__button-toolbar">
        <Button color="primary" type="submit"   disabled={isSubmitting}   >{props.t('Commands:Save')}</Button>{' '}
        <Button color="secondary"   disabled={isSubmitting}      type="button"  onClick={toggle}>{props.t('Global:Close')}</Button>
        </ButtonToolbar>
    </form>
    );
};


/*const LOADAddAdvertiser = 'LOADAddAdvertiser';

export const loadData=(state = {}, action) => {
  switch (action.type) {
    case LOADAddAdvertiser:
      return {
        data: state, type:LOADAddAdvertiser
      };
    default:
      return state;
  }
}*/

//export const loadAdv = data => loadData(data,{ type: LOADAddAdvertiser });

// You have to connect() to any reducers that you wish to connect to yourself
// AddAdvertiser= reduxForm({
//   form: 'addAdvertiser',enableReinitialize: true // a unique identifier for this form
// })(AddAdvertiser); 
// AddAdvertiser = connect(
//   state => ({
//    // initialValues: state.Entet// pull initial values from account reducer
//   })
  
//   //,  { load: loadAdv }, // bind account loading action creator
// )(AddAdvertiser);

export default AddEditDeal;