import { Col, Container, Row, Button ,Card ,CardBody , Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, Form, FormGroup,ButtonToolbar} from 'reactstrap';
import { Field, reduxForm ,change} from 'redux-form';
import React, { useCallback, useMemo ,useState,useEffect} from 'react';
import { useForm,Controller } from "react-hook-form";
import  yup from "../../Config/YUP"
import { yupResolver } from '@hookform/resolvers/yup';
import  reacthookformValues from "../../Config/reacthookform"
import axios from 'axios';
import { useSelector, useDispatch } from 'react-redux';
import {  userInitialized, userRoleInitialized,ChangeUserRole ,UserSelectors } from '../../Store/ReducerSlices/User';
import Constants from '../../Config/Constants';
import { showBasicNotificationForAdFalcon } from '../../Components/Notification';
import { Upload } from '@progress/kendo-react-upload';
import {RadioButtonField} from '../../shared/components/form/RadioButton';

const validationSchemayup = 
yup.object().shape({
        Name: yup.string(),
        AppType: yup.string(),
      });
let UploaddevicesId = (props) => {
  
  const stateStore = useSelector(state => state);
  const UserGlobal = UserSelectors.getUser(stateStore);
  
  const [radioButtonValue , setradioButtonValue] = useState(1);
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
  },[props.collapseUploaddevicesId]);

  const SaveUploaddevicesId = (data, e) =>{
      
    //Id: props.AudienceLiestid,
    let params = {en: data.Name,  AdvertiserId: props.AdvertiserId, Description: data.Description };
    if(props.toEditObject){
      params = Object.assign(params,{Id:props.toEditObject.id})
    }
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
  const onChange = (e)=>{
    setradioButtonValue(e);
  }
    
  return (
    <form  className="form" onSubmit={handleSubmit(SaveUploaddevicesId)}>
       
              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Global:Name')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Input
                name="Name"
                type="input"
                placeholder="Name"
                innerRef={register}
                disabled 
                //validate={[ required() ]}
                />
    {errors.Name  && <span className="form__form-group-error">{errors.Name.message}</span>}  </div>
                </div>
              </div>


              <Row>
                <Col md={6}>
                  <div className="form__form-group">
                      <div className="form__form-group-field">
                        <div className="form__form-group-input-wrap">
                          <RadioButtonField
                              onChange={onChange}
                              name="AppType"
                              value={radioButtonValue}
                              
                              radioValue={1}
                              innerRef={register}
                              label={props.t('AppType:Android')}
                                />
                        </div>
                      </div>
                  </div>
                </Col>
                <Col md={6}>
                  <div className="form__form-group">
                      <div className="form__form-group-field">
                        <div className="form__form-group-input-wrap">
                          <RadioButtonField
                              onChange={onChange}
                              name="AppType"
                              value={radioButtonValue}
                              
                              radioValue={2}
                              innerRef={register}
                              label={props.t('AppType:IOS')}
                                />
                        </div>
                      </div>
                  </div>
                </Col>
              </Row>

              <div className="form__form-group">
                <span className="form__form-group-label">{props.t('AudienceList:SelectFile')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                  <Upload
                  showFileList={false}
                  batch={false}
                  restrictions={{
                    allowedExtensions: [ '.csv', '.txt' ],
                    maxFileSize: 100000000
                  }}
                  multiple={false}
                  defaultFiles={[]}
                  withCredentials={true}
                  saveUrl={ Constants.backend.UploaddevicesIdUrl+radioButtonValue + "&DeviceTypeId="+props.toEditObject.id}
                />
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


export default UploaddevicesId;