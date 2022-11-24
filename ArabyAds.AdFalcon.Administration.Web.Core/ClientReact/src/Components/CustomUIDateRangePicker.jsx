import React from 'react';
import moment from 'moment';
import localeEn from '../Localization/LocaleResources/DateRangepickerEn.json';
import localeAr from '../Localization/LocaleResources/DateRangepickerAr.json';
import { useState, useCallback, useEffect, useRef } from 'react';
import $ from 'jquery';


const CustomUIDateRangePicker = (props) => {
    const [StartDate, setStartDate] = useState(props.Fromvalue);
    const [EndDate, setEndDate] = useState(props.Tovalue);
    const datePicker= useRef();
    useEffect(()=>{
        
        if(StartDate && EndDate && datePicker.current)
        {
            if(datePicker.current.startDate !== StartDate)
                datePicker.current.setStartDate(StartDate);
           if(datePicker.current.endDate !== EndDate)
               datePicker.current.setEndDate(moment(EndDate));
        }
      
    },[StartDate,EndDate]);
   
    const domReady = useCallback(node => {
        if (node !== null) {
             $(node).daterangepicker(
                {
                    showCustomRangeLabel: true, autoUpdateInput: false,
                    maxSpan: true,
                    linkedCalendars: false,
                    locale: props.IsArabic ? localeAr : localeEn,
                    opens: 'center',
                    buttonClasses:"btn",
                    applyButtonClasses:"btn-primary",
                    cancelButtonClasses:"btn-secondary"

                }
            )
            datePicker.current =  $(node).data('daterangepicker');
            $(node).on('cancel.daterangepicker', function(ev, picker) {
                setStartDate(null);
                setEndDate(null);
            });
            $(node).on('apply.daterangepicker', function(ev, picker) {
                setStartDate(picker.startDate);
                setEndDate(picker.endDate);
            });
        }
      }, []);
 
     return (
         <div ref={domReady}>
             {props.children({StartDate,EndDate, setStartDate, setEndDate})}
         </div>
      );

};


export const CustomUISingleDatePicker = ({pickerConfig={},...props}) => {
   
    const startDomReady = useCallback(node => {
        if (node !== null) {
             $(node).daterangepicker(
                {
                    showCustomRangeLabel: true, autoUpdateInput: false,
                    maxSpan: true,
                    singleDatePicker: true,
                    linkedCalendars: false,
                    locale: props.IsArabic ? localeAr : localeEn,
                    opens: 'center',
                    buttonClasses:"btn",
                    applyButtonClasses:"btn-primary",
                    cancelButtonClasses:"btn-secondary",
                    ...pickerConfig
                }
            )
            $(node).on('cancel.daterangepicker', function(ev, picker) {
                $(node).val(null);
            });
            $(node).on('apply.daterangepicker', function(ev, picker) {
                $(node).val(picker.startDate.format('DD-MM-YYYY'));
            });
        }
      }, []);

      const endDomReady = useCallback(node => {
        if (node !== null) {
             $(node).daterangepicker(
                {
                    showCustomRangeLabel: true, autoUpdateInput: false,
                    maxSpan: true,
                    singleDatePicker: true,
                    linkedCalendars: false,
                    locale: props.IsArabic ? localeAr : localeEn,
                    opens: 'center',
                    buttonClasses:"btn",
                    applyButtonClasses:"btn-primary",
                    cancelButtonClasses:"btn-secondary",
                    ...pickerConfig
                }
            )
            $(node).on('cancel.daterangepicker', function(ev, picker) {
                $(node).val(null);
            });
            $(node).on('apply.daterangepicker', function(ev, picker) {
                $(node).val(picker.endDate.format('DD-MM-YYYY'));
            });
        }
      }, []);
 
     return (
         <div>
             {props.children({startDomReady, endDomReady})}
         </div>
      );

};


export default CustomUIDateRangePicker;