import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Button, ButtonToolbar, Modal, Input } from 'reactstrap';
import classNames from 'classnames';
import { RTLProps } from '../shared/prop-types/ReducerProps';
import axios from 'axios';
import { useForm,Controller } from "react-hook-form";
import  yup from "../Config/YUP"
import  reacthookformValues from "../Config/reacthookform"
import { yupResolver } from '@hookform/resolvers/yup';
import { BasicNotification,showBasicNotificationForAdFalcon } from '../Components/Notification';

const validationSchemayup = 
yup.object().shape({
        Name: yup.string().required(),
      });
const RenameComponent = (props) =>{
  

    reacthookformValues.resolver=yupResolver(validationSchemayup);
    const { handleSubmit, errors, register,control,setValue, reset,setError,   formState: {isValid , isDirty, isSubmitting, touched, submitCount }} = useForm({...reacthookformValues   });
    
    const [modal, setModal] = useState(false);

    const toggle = () => {
      setModal(prevState => !prevState);
    };
    const submitForm = (data, e) =>{
        
        axios({
            method: 'get',
            url:props.renameUrl +"&name="+data.Name,
            //data:Jdata,
            headers: {
                    "Accept": "application/json"
                }
            })
            .then(success => {
                if(success.data && success.data.success==false){
              
                    setError('serverError', {message:success.data.Message});
       
                }
                else if(success.data && success.data.success==true)
                {
                    let message = props.renameSuccessMessage.replace("{0}",props.name);
                    toggle();
                    showBasicNotificationForAdFalcon(t("Commands:Clone"),message,"success","right-up");
                }
        }).catch(err => console.log('error', err));
    }

    const modalClass = classNames({
    'modal-dialog--header': true,
    });
    // const handleChangeForName = () =>{

    // }
    return (
    <React.Fragment>
        <button className="btn-outline-primary btn"  onClick={toggle}  title={props.title}><a  ><span className={"ri-file-edit-line p-2"} /></a></button>
    
        <Modal
            isOpen={modal}
            toggle={toggle}
            modalClassName={`${props.rtl.direction}-support`}
            className={`modal-dialog--danger ${modalClass}`}
        >
       

            <div className="modal__header">
            <button className="lnr lnr-cross modal__close-btn" type="button" onClick={toggle} />
            <span className="lnr lnr-cross-circle modal__title-icon" />
            <h4 className="text-modal  modal__title">Clone</h4>
            </div>
            <form  onSubmit={handleSubmit(submitForm)}>
            <div className="modal__body">
                <div className="form__form-group">
                    <span className="form__form-group-label">{props.t('Global:Name')}</span>
                    <div className="form__form-group-field">
                        <div className="form__form-group-input-wrap">
                            <Input
                            name="Name"
                            type="input"
                            placeholder="Name"
                            defaultValue={props.name}
                            innerRef={register}
                            //onChange={(e)=>{handleChangeForName(e); }}
                            />
                {errors.Name  && <span className="form__form-group-error">{errors.Name.message}</span>}
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
            </div>
            <ButtonToolbar className="modal__footer">
            <Button className="modal_cancel" onClick={toggle}>Cancel</Button>{' '}
            <Button className="modal_ok" color="danger" type="submit"   disabled={isSubmitting}  >Yes</Button>
            </ButtonToolbar>
            </form>
        </Modal>
    </React.Fragment>
     )
} 


RenameComponent.propTypes = {
    title: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
  };
  
  RenameComponent.defaultProps = {
    title: '',
    href: '',
    name: '',
  };
  
  export default connect(state => ({
    rtl: state.rtl,
  }))(RenameComponent);