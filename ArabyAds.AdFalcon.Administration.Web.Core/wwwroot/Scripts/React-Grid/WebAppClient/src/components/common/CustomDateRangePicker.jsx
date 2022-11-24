import React, { Component } from "react";
import BaseFormComponent from "./BaseFormComponent";
import moment from "moment";
import DateRangePicker from "react-bootstrap-daterangepicker";
//import localeEn from '../../translations/DateRangePickerEn.json';
//import localeAr from '../../translations/DateRangePickerAr.json';

class CustomDateRangePicker extends BaseFormComponent {
  ranges = {
    today: [moment(), moment()],
    yesterday: [moment().subtract(1, "days"), moment().subtract(1, "days")],
    last7Days: [moment().subtract(6, "days"), moment()],
    last30Days: [moment().subtract(29, "days"), moment()],
    thisMonth: [moment().startOf("month"), moment().endOf("month")],
    lastMonth: [
      moment()
        .subtract(1, "month")
        .startOf("month"),
      moment()
        .subtract(1, "month")
        .endOf("month")
    ]
  };
  rangesEn = {
    Today: this.ranges.today,
    Yesterday: this.ranges.yesterday,
    "Last 7 Days": this.ranges.last7Days,
    "Last 30 Days": this.ranges.last30Days,
    "This Month": this.ranges.thisMonth,
    "Last Month": this.ranges.lastMonth
  };
  rangesAr = {
    اليوم: this.ranges.today,
    امس: this.ranges.yesterday,
    "آخر سبع ايام": this.ranges.last7Days,
    "آخر 30 يوم": this.ranges.last30Days,
    "هذا الشهر": this.ranges.thisMonth,
    "الشهر الماضي": this.ranges.lastMonth
  };
  render() {
    return (
      <DateRangePicker
        containerClass="react-bootstrap-daterangepicker-container"
        containerStyles={{ display: "inline-block" }}
        startDate={this.props.from || moment(new Date())}
        endDate={this.props.to || moment(new Date())}
        showDropdowns
        locale={this.IsArabic ? localeAr : localeEn}
        showCustomRangeLabel
        opens={this.IsArabic ? "right" : "left"}
        autoUpdateInput
        maxSpan
        linkedCalendars={false}
        ranges={this.IsArabic ? this.rangesAr : this.rangesEn}
        onApply={this.props.onApply}
      >
        <label
          className="control-label"
          style={{ display: "inline-block", margin: "5px" }}
        >
          {this.props.titleMsg}
        </label>
        <input
          value={this.props.value}
          name={this.props.name}
          type={"text"}
          className="form-control"
          style={{
            display: "inline-block",
            width: "auto",
            maxWidth: "130px",
            margin: "5px"
          }}
          onChange={this.props.onChange}
        />
      </DateRangePicker>
    );
  }
}

export default CustomDateRangePicker;
