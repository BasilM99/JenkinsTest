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

import mdiPercentIcon  from 'mdi-react/PercentIcon'; 
import WebIcon from 'mdi-react/WebIcon';
import KeyVariantIcon from 'mdi-react/KeyVariantIcon';
import {MessageType} from '../../Config/Enums';
import {CustomSingleDatePicker} from '../../Components/CustomDateRangePicker';
import AdFalconNotificationsUtil  from '../../Utils/AdFalconNotificationsUtil';
import LockOutlineIcon from 'mdi-react/LockOutlineIcon';
import LockOpenVariantOutlineIcon   from 'mdi-react/LockOpenVariantOutlineIcon';



let AdvertiserSettings= (props) => {


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

  const [AdvertiserId, setAdvertiserId] =useState(0);
  const [AdvertiserName, setAdvertiserName] =useState(null);

  const [UserList, setUserList] =useState([]);
  const [WriteUserList, setWriteUserList] =useState([]);
  const [ReadUserList, setReadUserList] =useState([]);
  const [OpenMessage, setOpenMessage] =useState(null);
  const [RestrictedMassage, setRestrictedMassage] =useState(null);
  const [AgencyCommissionList, setAgencyCommissionList] =useState([]);
  const [IsRestricted, setIsRestricted] =useState(false);
  const [AgencyCommission, setAgencyCommission] =useState(null);
 
  const getCommissionList=()=>{

 let newarr=[];
 newarr.push(new {value:0,label:props.t("Global:Select")});
 newarr.push(new {value:1,label:props.t("Global:FixedCPM")});
 newarr.push(new {value:2,label:props.t("Global:NetCostMargin")});

 newarr.push(new {value:3,label:props.t("Global:BillableCostMargin")});

 newarr.push(new {value:4,label:props.t("Global:GrossCostMargin")});

 return newarr;

  }

  const GetAdvertiserSettingsData =()=>{
setAgencyCommissionList(getCommissionList());
    axios({
      method: 'get',
      url: Constants.backend.GetAccountAdvertiserSettings +"?id"+AdvertiserId
    
    }
   )
.then(success => {
if(success.data)
{
  setAdvertiserName(success.data.AdvertiserName);
  setOpenMessage(props.t("AdvSecurity:Open").replace("{0}",success.data.AdvertiserName));
  setRestrictedMassage(props.t("AdvSecurity:Restricted").replace("{0}",success.data.AdvertiserName));

  
  let arrUser =[];
  let  WriteUsersVar= [];
  let ReadUsersVar= [];
    if(!!success.data.Users && success.data.Users.length>0)
    {   arrUser=success.data.Users.map((row, i) => ( { 
      label : x.Text, value:x.Value

    })) ;
    UserList=arrUser;
      setUserList(arrUser );
      if(!!success.data.ReadUsers && success.data.ReadUsers.length>0)
    { 
      var array = success.data.ReadUsers.split(',');

      let ReadUsersVar=  UserList.filter(item  => !array.some(val => parseInt(val )=== parseInt(item.value)));
      setReadUserList(ReadUsersVar);
    }   
     else
    {

      setReadUserList(arrUser);

    }
  
  if(!!success.data.WriteUsers && success.data.WriteUsers.length>0)
    { 

      var array = success.data.WriteUsers.split(',');

       WriteUsersVar=  UserList.filter(item  => !array.some(val => parseInt(val )=== parseInt(item.value)));
setWriteUserList(WriteUsersVar);

    }
    else
    {
      setWriteUserList(arrUser);

    }
      


      
    
    }

    setIsRestricted(success.data.IsRestricted);
   let listcommi= getCommissionList();
   if(!!success.data.AgencyCommission && parseInt(success.data.AgencyCommission)>0)
   {
    let itemfind = listcommi.find(e=>parseInt(e.value )== parseInt(success.data.AgencyCommission ) );
    setAgencyCommission(itemfind);


   }






      reset({IsRestricted : success.data.IsRestricted,  WriteUsers:WriteUsersVar,ReadUsers:ReadUsersVar,AgencyCommission:success.data.AgencyCommission,AgencyCommissionValue:success.data.AgencyCommissionValue})  ;
         
}
})
.catch(err => console.log('error', err));

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


   
   
    reacthookformValues.resolver=yupResolver(validationSchemayup);
    const { handleSubmit, errors, register,control,setValue, reset,setError,getValues  ,formState: {isValid , isDirty, isSubmitting, touched, submitCount }} = useForm({...reacthookformValues   });
    useEffect(() => {
		
    reset();
    },[props.collapseAddEditDeal]);

    
    const SaveAddEditAdvertiserSettings = (data, e) =>{
       

        baseUrl = Constants.backend.SaveAccountAdvertiserSettings;

      var querystring = require('querystring');
      var dataInJson = getValues();
      
    

    let ReadUsersArr=[];
    if(!!dataInJson.ReadUsers && dataInJson.ReadUsers.length>0)
    {ReadUsersArr= dataInJson.ReadUsers.map(function(e){
      return e.value;
    });
  

  let ReadUsersArrStr= ReadUsersArr.join(',')

  let WriteUsersArr=[];
  if(!!dataInJson.WriteUsers && dataInJson.WriteUsers.length>0)
  {WriteUsersArr= dataInJson.WriteUsers.map(function(e){
    return e.value;
  });


let WriteUsersArrStr= WriteUsersArr.join(',')

  //reset({IsRestricted : success.data.IsRestricted,  WriteUsers:WriteUsersArrStr,ReadUsers:ReadUsersArrStr,AgencyCommission:success.data.AgencyCommission,AgencyCommissionValue:success.data.AgencyCommissionValue})  ;
         
     // var data = querystring.stringify({checkedRecords:state.idsToArchive});
     let datatoBeSent={AdvertiserAccountId:AdvertiserId,IsRestricted:dataInJson.IsRestricted,AgencyCommission:dataInJson.AgencyCommission,AgencyCommissionValue:dataInJson.AgencyCommissionValue,ReadUsers:dataInJson.ReadUsers,WriteUsers:dataInJson.WriteUsers};
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

          /*
          success.data.Mesessges.forEach(function(entry) {
           // console.log(entry);

           if(entry.Type == MessageType.Success)
           {

         
         
            showBasicNotificationForAdFalcon("Advertiser Settings",entry.Message,"success","right-up");
           }
           if(entry.Type == MessageType.Error)
           {

         
         
            showBasicNotificationForAdFalcon("Advertiser Settings",entry.Message,"danger","right-up");
           }
           if(entry.Type == MessageType.Warning)
           {

         
         
            showBasicNotificationForAdFalcon("Advertiser Settings",entry.Message,"warning","right-up");
           }

           if(entry.Type == MessageType.Information)
           {

         
         
            showBasicNotificationForAdFalcon("Advertiser Settings",entry.Message,"","right-up");
           }

        });  
*/
        AdFalconNotificationsUtil(success.data.Mesessges,"Advertiser Settings");

          /*if(!!success.data.Id && parseInt(success.data.Id)>0)
          {
            setDealID(parseInt(success.data.Id));

          }*/
         
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

const onIsRestrictedCheckChange=(e) =>{

setIsRestricted(e);

}

const UpdateWriteUsers=(e)=>{
 let  WriteUsersVar=  WriteUserList.filter(item  => !ReadUserList.some(val => parseInt(val.value )=== parseInt(item.value)) && pareseInt(item.value )!==parseInt(e.value )  );

setWriteUserList(WriteUsersVar);

}
const UpdateReadUsers=(e)=>{
  

  
 
 let WriteUsersVar=  ReadUserList.filter(item  => !WriteUserList.some(val => parseInt(val.value )=== parseInt(item.value))   && pareseInt(item.value )!==parseInt(e.value )   );
setReadUserList(WriteUsersVar);


}
  return (
    <form  className="form" onSubmit={handleSubmit(SaveAddEditAdvertiserSettings)}>
       

            
              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Lookup:Type')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
               
                <RadioButtonField 
                                                           onChange={onIsRestrictedCheckChange}
                                                            name="IsRestricted"
                                                            radioValue={false}
                                                            label={<div><LockOutlineIcon></LockOutlineIcon><p className="payment__credit-name">{props.t('Global:Open')}</p></div>}
                                                            innerRef={register}
                                                             />

<span >{OpenMessage}</span>



    
    
    
     </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Lookup:Type')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
               
       


<RadioButtonField 
                                                          onChange={onIsRestrictedCheckChange}
                                                            name="IsRestricted"
                                                            radioValue={true}
                                                            label={<div><LockOpenVariantOutlineIcon></LockOpenVariantOutlineIcon> <p className="payment__credit-name">{props.t('Global:Restricted')}</p></div>}
                                                            innerRef={register}
                                                             />


<span >{RestrictedMassage}</span>

    
    
    
     </div>
                </div>
              </div>


      
              <div className="form__form-group" style={{ visibility: IsRestricted  ? 'visible' :'hidden'}}>
                <span className="form__form-group-label">{props.t('Global:Users')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Controller
                    name="WriteUsers"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                    
                    <ReactSelect
                   
                    //placeholder={props.t('PMPDeal:SelectExchangeT')}
                    onChange={(e)=>{onChange(e); UpdateReadUsers(e);}}
                    value={value} 
                    className="react-select"
                    classNamePrefix="react-select"
                
                    isClearable={true}
                   // isRtl={isRtl}
                   isMulti
                    isSearchable={true}
                    name="WriteUsers"
                    options={WriteUserList}
                   
                   
                    />  }
                    />

    
    
      </div>
                </div>
              </div>



              <div className="form__form-group" style={{ visibility: IsRestricted  ? 'visible' :'hidden'}}>
                <span className="form__form-group-label">{props.t('Global:Read')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Controller
                    name="ReadUsers"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                    
                    <ReactSelect
                   
                    //placeholder={props.t('PMPDeal:SelectExchangeT')}
                    onChange={(e)=>{onChange(e);  UpdateWriteUsers(e);}}
                    value={value} 
                    className="react-select"
                    classNamePrefix="react-select"
                
                    isClearable={true}
                  
                   isMulti
                    isSearchable={true}
                    name="ReadUsers"
                    options={ReadUserList}
                   
                   
                    />  }
                    />

    
    
      </div>
                </div>
              </div>



              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Global:Country')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Controller
                    name="AgencyCommission"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                    
                    <ReactSelect
                   
                   
                    onChange={(e)=>{onChange(e);setAgencyCommission(e); }}
                    value={value} 
                    className="react-select"
                    classNamePrefix="react-select"
                  
                    isClearable={true}
                 
           
                    name="AgencyCommission"
                    options={AgencyCommissionList}
                   
                   
                    />  }    />       


  </div>
                </div>
              </div>


              <div className="form__form-group"  style={{ visibility: parseInt(AgencyCommission)>0  ? 'visible' :'hidden'}}>
                <span className="form__form-group-label">{props.t('Global:AgencyCommissionValue')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input
                name="AgencyCommissionValue"
                type="number"
                placeholder={props.t('Global:AgencyCommissionValue')}
                innerRef={register} onKeyDown={onKeyDown}
                //validate={[ required() ]}
                />
               { !!AgencyCommission && parseInt(AgencyCommission.value)==1  && <mdiCurrencyUsdIcon>

                </mdiCurrencyUsdIcon>}

                { !!AgencyCommission && parseInt(AgencyCommission.value)>1  && <mdiPercentIcon>

</mdiPercentIcon>}
               
 
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





export default AdvertiserSettings;