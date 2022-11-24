import React, { Component } from 'react';
import moment from 'moment';

import localeEn from '../Localization/LocaleResources/DateRangepickerEn.json';
import localeAr from '../Localization/LocaleResources/DateRangepickerEn.json';
import { useContext, useState, useEffect, useRef } from 'react';
import $ from 'jquery';
import DateRangePicker2 from 'bootstrap-daterangepicker';


const CustomDateRangePicker = (props) => {
    const ranges = {
        today: [moment(), moment()],
        yesterday: [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        last7Days: [moment().subtract(6, 'days'), moment()],
        last30Days: [moment().subtract(29, 'days'), moment()],
        thisMonth: [moment().startOf('month'), moment().endOf('month')],
        lastMonth: [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    }
    const rangesEn = {
        'Today': ranges.today,
        'Yesterday': ranges.yesterday,
        'Last 7 Days': ranges.last7Days,
        'Last 30 Days': ranges.last30Days,
        'This Month': ranges.thisMonth,
        'Last Month': ranges.lastMonth
    }
    const rangesAr = {
        'اليوم': ranges.today,
        'امس': ranges.yesterday,
        'آخر سبع ايام': ranges.last7Days,
        'آخر 30 يوم': ranges.last30Days,
        'هذا الشهر': ranges.thisMonth,
        'الشهر الماضي': ranges.lastMonth
    }
    const [StartDate, setSartDate] = useState(moment(new Date()));
    const [EndDate, setEndDate] = useState(moment(new Date()));

    let customRangeRef = useRef();


    useEffect(() => {

        //#region :: Load Advertisers List
        // convertTreeToArray(props.data, treeArrayData);



        $(customRangeRef).daterangepicker(
            {
                showCustomRangeLabel: true, autoUpdateInput: true,
                maxSpan: true,
                linkedCalendars: false,
                locale: props.IsArabic ? localeAr : localeEn,
                startDate: props.Fromvalue || moment(new Date()),
                endDate: props.Tovalue || moment(new Date()),
                ranges: props.IsArabic ? rangesAr : rangesEn,
                opens: 'center'
            },
            handleCallback
        )
        setSartDate(props.Fromvalue || moment(new Date()));
        setEndDate(props.Tovalue || moment(new Date()));

    }, [customRangeRef]);

    const handleCallback = (start, end, label) => {
        setSartDate(start);
        setEndDate(end);
        props.CallBackParent(start, end);
    }
    return <>


        <div className="form__form-group-field" id={props.id} ref={ref => customRangeRef = ref} >
            {/* <div style={{display:'none'}}>
<DateRangePicker2><button></button></DateRangePicker2>


</div> */}
            <span className="form__form-group-label p-1" >{props.titleMsgFrom}</span>
            {/* <label className="control-label" style={{ display: 'inline-block', margin: '5px' }} >{props.titleMsgFrom}</label> */}
            
            <div class="topbar__search">
                <input value={StartDate.format("DD-MM-YYYY")} name={props.nameFrom}
                    type={'text'}
                    className="form-control"
                //onChange={props.onChange}
                />
                {/* <input placeholder="Search..." class=" form-control k-textbox dropdown-toggle"/> */}
                <div class="topbar__btn topbar__search-btn " >
                    <i class="ri-calendar-event-fill"></i>
                </div>
            </div>

            <span className="form__form-group-label p-1" >{props.titleMsgTo}</span>
            {/* <label className="control-label" style={{ display: 'inline-block', margin: '5px' }} >{props.titleMsgTo}</label> */}
            <div class="topbar__search">
               
                <input value={EndDate.format("DD-MM-YYYY")} name={props.nameTo}
                    type={'text'}
                    className="form-control"
                //onChange={props.onChange}

                />
                {/* <input placeholder="Search..." class=" form-control k-textbox dropdown-toggle"/> */}
                <div class="topbar__btn topbar__search-btn " >
                    <i class="ri-calendar-event-fill"></i>
                </div>
            </div>

        </div>

        {/* 
            <DateRangePicker
                containerClass="react-bootstrap-daterangepicker-container"
                containerStyles={{ display: "inline-block" }}
                startDate={props.from || moment(new Date())}
                endDate={props.to || moment(new Date())}
                
                opens={IsArabic ? "right" : "left"}
                showDropdowns
			
                locale={IsArabic ? localeAr : localeEn}
                showCustomRangeLabel autoUpdateInput
				maxSpan
				linkedCalendars={false}
                ranges={IsArabic ? rangesAr : rangesEn}
                onApply={props.onApply}>
                <label className="control-label" style={{ display: 'inline-block', margin: '5px' }} >{props.titleMsg}</label>
                <input value={props.value} name={props.name}
                    type={'text'}
                    className="form-control"
                    style={{ display: 'inline-block', width: "auto", maxWidth: '130px', margin: '5px' }}
                    onChange={props.onChange} />
            </DateRangePicker> */}
    </>

};



export const CustomSingleDatePicker = (props) => {
    const ranges = {
        today: [moment(), moment()],
        yesterday: [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        last7Days: [moment().subtract(6, 'days'), moment()],
        last30Days: [moment().subtract(29, 'days'), moment()],
        thisMonth: [moment().startOf('month'), moment().endOf('month')],
        lastMonth: [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    }
    const rangesEn = {
        'Today': ranges.today,
        'Yesterday': ranges.yesterday,
        'Last 7 Days': ranges.last7Days,
        'Last 30 Days': ranges.last30Days,
        'This Month': ranges.thisMonth,
        'Last Month': ranges.lastMonth
    }
    const rangesAr = {
        'اليوم': ranges.today,
        'امس': ranges.yesterday,
        'آخر سبع ايام': ranges.last7Days,
        'آخر 30 يوم': ranges.last30Days,
        'هذا الشهر': ranges.thisMonth,
        'الشهر الماضي': ranges.lastMonth
    }
    const [StartDate, setSartDate] = useState(null);
    const [EndDate, setEndDate] = useState(null);

    let customRangeRef = useRef();


    useEffect(() => {

        //#region :: Load Advertisers List
        // convertTreeToArray(props.data, treeArrayData);



        $(customRangeRef).daterangepicker(
            {
               autoUpdateInput: true,
                maxSpan: true,
                linkedCalendars: false,
                singleDatePicker: true,
    showDropdowns: true,

                locale: props.IsArabic ? localeAr : localeEn,
                startDate: props.Datevalue || moment(new Date()),
               // endDate: props.Datevalue || moment(new Date()),
                
                opens: 'center'
            },
            handleCallback
        )
        setSartDate(props.Datevalue );
       // setEndDate(props.Datevalue || moment(new Date()));

    }, [customRangeRef]);


    useEffect(() => {

       
        setSartDate(props.Datevalue );
       // setEndDate(props.Datevalue || moment(new Date()));

    }, [props.Datevalue ]);

    const handleCallback = (start, end, label) => {
        setSartDate(start);
        setEndDate(end);
        props.CallBackParent(start);
    }
    return <>


        <div className="form__form-group-field" id={props.id} ref={ref => customRangeRef = ref} >
            {/* <div style={{display:'none'}}>
<DateRangePicker2><button></button></DateRangePicker2>


</div> */}
            <span className="form__form-group-label p-1" >{props.titleMsgDate}</span>
            {/* <label className="control-label" style={{ display: 'inline-block', margin: '5px' }} >{props.titleMsgFrom}</label> */}
            
            <div class="topbar__search">
                <input value={ !!StartDate ?  StartDate.format("DD-MM-YYYY") : null} name={props.nameDate}
                    type={'text'} placeholder={props.placeholder}
                    className="form-control"
                //onChange={props.onChange}
                />
                {/* <input placeholder="Search..." class=" form-control k-textbox dropdown-toggle"/> */}
                <div class="topbar__btn topbar__search-btn " >
                    <i class="ri-calendar-event-fill"></i>
                </div>
            </div>

          

        </div>

      
    </>

};
export default CustomDateRangePicker;