import React, { useEffect, useRef, useState } from 'react';
import { useForm, Controller } from "react-hook-form";
import { compose } from 'redux';
import { Container, Card, CardBody, Col, Row, Button, ButtonToolbar, Popover, PopoverBody, PopoverHeader } from 'reactstrap';
import useLocalization from '../../Hooks/useLocalization';
import { RTLProps } from '../../shared/prop-types/ReducerProps';
import { connect } from 'react-redux';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import { CustomUISingleDatePicker } from '../../Components/CustomUIDateRangePicker';
import createNumberMask from 'text-mask-addons/dist/createNumberMask';
import MaskedInput from 'react-text-mask';
import yup from "../../Config/YUP";
import { yupResolver } from '@hookform/resolvers/yup';
import reacthookformValues from "../../Config/reacthookform";
import TimePicker from '../../Components/CustomTimePicker';
import axios from "axios";
import Constants from "../../Config/Constants";
import moment from 'moment';
import { useParams } from 'react-router-dom';
import InformationOutlineIcon from "mdi-react/InformationOutlineIcon";



const schema = yup.object().shape({
  Name: yup.string().required(),
  StartDate: yup.string().required(),
  EndDate: yup.string().nullable(true),
  Budget: yup.number().required(),
  DailyBudget: yup.number().nullable(true),
  Note:  yup.string().nullable(true)
});

const AdFalconCreateCampaign = ({ t, rtl }) => {

  const { register, handleSubmit, control, errors, reset } = useForm({ ...reacthookformValues, resolver: yupResolver(schema) });
  const { T } = useLocalization();
  const { campaignId, accountAdvertiserId } = useParams();
  const numberMask = createNumberMask({
    prefix: '$ ',
    allowDecimal: true,
    allowLeadingZeroes: true,
    decimalLimit: 3
  });
  const unmask = (val) => Number(val.replace(/[$,/\s]/g, ''));
  const [showPopOver, togglePopOver] = useState(false);
  const model =  useRef();
  const setDefaultData = (data) =>{
    console.log(data);
    data.StartDate = moment(data.StartDate, true).isValid() ? moment(new Date(data.StartDate)).format("DD-MM-YYYY") : null;
    data.EndDate = moment(data.EndDate, true).isValid() ? moment(new Date(data.EndDate)).format("DD-MM-YYYY") : null;
    data.StartTime = moment(data.StartTime, true).isValid() ? new Date(data.StartTime) : null;
    data.EndTime = moment(data.EndTime, true).isValid() ? new Date(data.EndTime) : null;
    model.current = {...data};
    reset(data);

  };
  useEffect(async () => {
      try {
        const response = await axios.get(`${Constants.backend.GetCampaign}/${accountAdvertiserId}${campaignId> 0? "/"+campaignId : ""}`);
        setDefaultData(response.data);
      }
      catch (error) { }
   
  }, [reset]);

  const onSubmit = async data => {

    try {
      console.log({...model.current, ...data});
      const response = await axios.post(Constants.backend.SaveCampaign,{...model.current, ...data});
      console.log(response);
      if(!response.data.HasErrors)
        setDefaultData(response.data.Response);
    }
    catch (error) { }

   };

  return (
    <Container >
      <Row>
        <Col xs={10} md={10} lg={10} xl={5} >
          <Card>
            <CardBody>
              <form className="form form--horizontal" onSubmit={handleSubmit(onSubmit)}>
                <div className="form__form-group">
                  <span className="form__form-group-label">{T("Campaign:CampaignName")}</span>
                  <div className="form__form-group-field">
                    <div className="form__form-group-input-wrap" >
                      <input name="Name" type="text" ref={register} maxlength={255} />
                      <span className="form__form-group-error" hidden={!errors.Name}>{T("ResourceSet:CampaignRequiredMsg")}</span>
                    </div>
                  </div>
                </div>
                {/* <div className="form__form-group">
              <Controller
                 control={control}
                 name="dateRange"
                 render={({ onChange, onBlur, value }) => (
                  <CustomDateRangePicker   CallBackParent={(FromDate , ToDate ) => onChange({FromDate,ToDate})} id='customeDateRange'  
                  Tovalue={value?.ToDate}    Fromvalue={value?.FromDate }   
                   nameTo='StartDate' nameFrom='EndDate' titleMsgFrom={T("Campaign:StartDate")} titleMsgTo={T("Campaign:EndDate")} />
                 )}
               />
                </div> */}
                <CustomUISingleDatePicker pickerConfig={{minDate :new Date()}}  >{({ startDomReady, endDomReady }) => <>
                  <div className="form__form-group">
                    <span className="form__form-group-label">{T("Campaign:StartDate")}</span>
                    <div className="form__form-group-field" >
                      <div className="form__form-group-input-wrap" >
                        <input name="StartDate"  type="text" ref={ref => { startDomReady(ref); register(ref); }} />
                        <div class="topbar__btn topbar__search-btn " >
                          <i class="ri-calendar-event-fill"></i>

                        </div>
                        <span className="form__form-group-error" hidden={!errors.StartDate}>{T("ResourceSet:StartDateRequiredMsg")}</span>
                      </div>
                    </div>
                  </div> 
                  <div className="form__form-group">
                    <span className="form__form-group-label">{T("Campaign:EndDate")}</span>
                    <div className="form__form-group-field"  >
                      <input name="EndDate"  type="text" ref={ref => { endDomReady(ref); register(ref); }} />
                      <div class="topbar__btn topbar__search-btn " >
                        <i class="ri-calendar-event-fill"></i>
                      </div>

                    </div>
                  </div>
                </>
                }
                </CustomUISingleDatePicker>
                <div className="form__form-group">
                  <span className="form__form-group-label">{T("Campaign:StartTime")}</span>
                  <div className="form__form-group-field">
                    <Controller
                      control={control}
                      name="StartTime"
                      render={({ value, onChange }) => (
                        <TimePicker value={value} onChange={onChange} />
                      )}
                    />
                  </div>
                </div>
                <div className="form__form-group">
                  <span className="form__form-group-label">{T("Campaign:EndTime")}</span>
                  <div className="form__form-group-field">
                    <Controller
                      control={control}
                      name="EndTime"
                      render={({ value, onChange }) => (
                        <TimePicker value={value} onChange={onChange} />
                      )}
                    />
                  </div>
                </div>
                <div className="form__form-group">
                  <span className="form__form-group-label">{T("Campaign:Budget")}</span>
                  <div className="form__form-group-field">
                    <div className="form__form-group-input-wrap" >
                      <Controller
                        control={control}
                        name="Budget"
                        render={({ value, onChange, onBlur }) => (
                          <MaskedInput mask={numberMask} onChange={e => onChange(unmask(e.target.value))} onBlur={onBlur} value={value} />
                        )}
                      />
                        <span className="form__form-group-error" hidden={!errors.Budget}>{T("ResourceSet:BudgetRequiredMsg")}</span>
                        <span className="form__form-group-description">
                          ({T("Global:MinBudget")})
                      </span>
                    </div>


                  </div>
                </div>
                <div className="form__form-group">
                  <span className="form__form-group-label">{T("Campaign:DailyBudget")}</span>
                  <div className="form__form-group-field">
                    <Controller

                      control={control}
                      name="DailyBudget"
                      render={({ value, onChange, onBlur }) => (
                        <MaskedInput mask={numberMask} onChange={e => onChange(unmask(e.target.value))} onBlur={onBlur} value={value} />
                      )}
                    />

                      <button id="PopoverTop"
                    type="button"
                    className={"form__form-group-button"}
                    onClick={e => togglePopOver(old => !old)}
                  ><InformationOutlineIcon />
                  </button> 
                  <Popover
                placement="top"
                isOpen={showPopOver}
                target="PopoverTop"
              >
                <PopoverBody> <p dangerouslySetInnerHTML={ {__html:T("Global:BudgetMoreInfo")}}></p></PopoverBody>
                </Popover>
                 {/* <div className="form__form-group-icon">
                    <InformationOutlineIcon />
                    </div> */}
                  
                  </div>
                  <span className="form__form-group-description">
                    ({T("Global:MinDailyBudget")})
                </span>
                </div>
                <div className="form__form-group">
                  <span className="form__form-group-label">{T("Campaign:Note")}</span>
                  <div className="form__form-group-field">
                    <textarea name="Note" maxlength={1024} ref={register} rows={5} cols={5} />

                  </div>
                </div>
                {model.current?.UniqueId &&  <div className="form__form-group">
                  <span className="form__form-group-label">{T("Global:UniqueId")}</span>
                  <div className="form__form-group-field">
                   <input name="UniqueId" type="text" value={model.current?.UniqueId}   disabled />
                  </div>
                </div>}
                <ButtonToolbar className="form__button-toolbar">
                  <Button color="primary" type="submit">Save</Button>
                  <Button type="button" onClick={e => reset()}>
                    Cancel
                </Button>
                </ButtonToolbar>
              </form>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );

}

AdFalconCreateCampaign.propTypes = {
  t: PropTypes.func.isRequired,
  rtl: RTLProps.isRequired,
  advertiserAccId: PropTypes.number.isRequired

};


export default compose(withTranslation('common'), connect(state => ({
  rtl: state.rtl,
})))(AdFalconCreateCampaign);


