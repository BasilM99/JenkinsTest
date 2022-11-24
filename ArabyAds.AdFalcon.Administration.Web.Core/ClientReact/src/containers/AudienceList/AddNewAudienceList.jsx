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
import { yupResolver } from '@hookform/resolvers/yup';
import  reacthookformValues from "../../Config/reacthookform"
import axios from 'axios';
import { useSelector, useDispatch } from 'react-redux';
import {  userInitialized, userRoleInitialized,ChangeUserRole ,UserSelectors } from '../../Store/ReducerSlices/User';
import Constants from '../../Config/Constants';
import { BasicNotification,showBasicNotificationForAdFalcon } from '../../Components/Notification';
import { withTranslation } from 'react-i18next';

const validationSchemayup = 
yup.object().shape({
        Name: yup.string().required(),
        Description: yup.string(),
      });
let AddAudienceList = (props) => {
  
  const stateStore = useSelector(state => state);
  const UserGlobal = UserSelectors.getUser(stateStore);
  const onBasicFieldChange = (event, newValue, previousValue, name) => {
    console.log(newValue)
  }
  let  buttonLabel= "test";
  let className = "";
  
  reacthookformValues.resolver=yupResolver(validationSchemayup);
  const { handleSubmit, errors, register,control,setValue, reset,setError,   formState: {isValid , isDirty, isSubmitting, touched, submitCount }} = useForm({...reacthookformValues   });
  useEffect(() => {
    //getAdvListForDP(0,10);
    //reset();
    setLocalValues();
  },[props.collapseAddAudienceList]);

  const SaveAddAudienceList = (data, e) =>{
      
    //Id: props.AudienceLiestid,
    let params = {en: data.Name,  AdvertiserId: props.AdvertiserId, Description: data.Description };
    if(props.toEditObject){
      params = Object.assign(params,{Id:props.toEditObject.id})
    }
    return axios
      .request({
        url:  Constants.backend.SaveAudienceListUrl,
        method: 'Post',
        data:params
      })
      .then(success => {
        if(success.data && success.data.status=="businessException"){
      debugger

        setError('serverError', {message:success.data.Message});

        }
        else if(success.data && success.data.status=="success")
        {

          showBasicNotificationForAdFalcon("AudienceList Add","you have succsfully added an AudienceList","success","right-up");
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
  
  const setLocalValues = () =>{
    if(props.toEditObject)
      {
        setValue('Name', props.toEditObject.name, { shouldValidate: true })
        setValue('Description', props.toEditObject.Description)
      }
      else
        reset();
  }

  const toggle =()=>{


    reset();
    props.CallBackToggle()
  }
    
  return (
    <form  className="form" onSubmit={handleSubmit(SaveAddAudienceList)}>
       
              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Global:Name')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input
                name="Name"
                type="input"
                placeholder="Name"
                innerRef={register}
                //validate={[ required() ]}
                />
    {errors.Name  && <span className="form__form-group-error">{errors.Name.message}</span>}  </div>
                </div>
              </div>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Campaign:Description')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input type="textarea" name="Description"  innerRef={register} />
               
                {/* {errors.Description  && <span className="form__form-group-error">{errors.Description.message}</span>} */}
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


export default AddAudienceList;