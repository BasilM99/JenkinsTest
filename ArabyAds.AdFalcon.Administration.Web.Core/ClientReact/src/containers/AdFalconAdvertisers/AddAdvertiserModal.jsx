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
// const renderField = ({ input, label, type, meta: { touched, error, warning } }) => (
//   <div>
//     {/* <label>{label}</label> */}
//     <div>
//       <Input {...input} placeholder={label} type={type}/>
//       {touched && ((error && <span>{error}</span>) || (warning && <span>{warning}</span>))}
//     </div>
//   </div>
// )
/*const useYupValidationResolver = validationSchema =>
useCallback(
  async data => {
    try {
      const values = await validationSchema.validate(data, {
        abortEarly: false
      });

      return {
        values,
        errors: {}
      };
    } catch (errors) {
      return {
        values: {},
        errors: errors.inner.reduce(
          (allErrors, currentError) => ({
            ...allErrors,
            [currentError.path]: {
              type: currentError.type ?? "validation",
              message: currentError.message
            }
          }),
          {}
        )
      };
    }
  },
  [validationSchema]
);*/
const validationSchemayup = 
yup.object().shape({
        Name: yup.string().required(),
        Advertiser: yup.object().nullable().required(),
      });
let AddAdvertiser = (props) => {
  
  const stateStore = useSelector(state => state);
  const UserGlobal = UserSelectors.getUser(stateStore);
  const onBasicFieldChange = (event, newValue, previousValue, name) => {
    console.log(newValue)
  }
  let  buttonLabel= "test";
    let className = "";
   
    const handleInputChangeForAdvertiser=(inputValue) =>{
     
    };
    const [AdvList,setAdvList]= useState([]);
    reacthookformValues.resolver=yupResolver(validationSchemayup);
    const { handleSubmit, errors, register,control,setValue, getValues,reset,setError,   formState: {isValid , isDirty, isSubmitting, touched, submitCount }} = useForm({...reacthookformValues   });
    
    useEffect(() => {
		//getAdvListForDP(0,10);
    reset();
    },[props.collapseAddAdvertiser]);

    
 
      
    //const resolver = useYupValidationResolver();
    
 
    
  
    //const dispatch = useDispatch();
  
    const handleChangeForAdvertiser=(Value) =>{
    
      //dispatch(change('addAdvertiser','Name',Value.label))
        setValue('Name', Value.label, { shouldValidate: true })

    
    };
    const SaveAddAdvertiser = (data, e) =>{
       

      return axios
       .request({
         url:  Constants.backend.SaveAddAdvertiser+"?AdvirtiserId="+data.Advertiser.value+"&name="+data.Name,
         method: 'GET',
       })
       .then(success => {
         if(success.data && success.data.status=="businessException"){
        debugger
       

       /* throw new SubmissionError({
           Advertiser: 'Wrong password',
           _error: success.data.Message 
         });*/
         setError('serverError', {message:success.data.Message});

        // showBasicNotificationForAdFalcon("ff","ff","primary","right-up");
       //  showBasicNotificationForAdFalcon("ff","ff","primary","right-up");

             //console.log(response);
         }
         else if(success.data && success.data.status=="success")
         {

           showBasicNotificationForAdFalcon("Advertiser Add","you have succsfully added an Advertiser","success","right-up");
           toggle();
         }
       })
       .catch(err => 
          { 
           throw err;
          /*throw new SubmissionError({
           Advertiser: 'Wrong password',
           _error: 'vvg'
         });*/

         //setError('serverError', {message:err.response});
           console.log('error', err)
          }
       );
     
       //e.preventDefault();
   }
    const getAdvListForDP = (inputValue,callback) =>{
     //debugger
        var tempList= [];
      if(!inputValue)
      {inputValue='';}
      
      axios
      .request({
        url:  Constants.backend.GetLookupAdvertiser+"?q="+inputValue,
        method: 'GET',
      })
      .then(success => {
        if(success.data){
          tempList =  success.data.map( x => {
            return {
              label: x.Name.Value,
              value: x.ID
            }
          });
          //setAdvList(tempList);
            //state.advertiserList = tempList;
            //setLocalState(state);
            callback(tempList);
            //console.log(response);
        }
      })
      .catch(err => console.log('error', err));
    
    }
const toggle =()=>{


  reset();
  props.CallBackToggle()
}
    
  return (
    <form  className="form" onSubmit={handleSubmit(SaveAddAdvertiser)}>
       

            <div className="form__form-group">
                <span className="form__form-group-label">{props.t('Global:Advertiser')}</span>
                <div className="form__form-group-field">
                <div className="form__form-group-input-wrap">
                <Controller
                    name="Advertiser"
                    control={control}
                    defaultValue=""
                    render={({ onChange, value }) => 
                    
                    <AsyncReactSelect
                    cacheOptions
                    defaultOptions
                    autoload
                    className="react-select"
                    classNamePrefix="react-select" 
                    name="Advertiser"
                    onChange={(e)=>{handleChangeForAdvertiser(e);onChange(e); }}
                    key={null}
                    loadOptions={getAdvListForDP}
                    handleInputChange={handleInputChangeForAdvertiser}
                    placeholder={props.t('Advertiser:SelectAdvertiserRequired')}
                    value={value} 
                    />  }
                    />
    {touched && errors && errors.Advertiser  && <span className="form__form-group-error">{errors.Advertiser.message}</span>}  </div>
                </div>
              </div>
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

export default AddAdvertiser;