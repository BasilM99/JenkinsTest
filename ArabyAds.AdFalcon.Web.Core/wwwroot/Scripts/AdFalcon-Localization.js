/* English/UK initialisation for the jQuery UI date picker plugin. */
/* Written by Stuart. */


/* Arabic Translation for jQuery UI date picker plugin. */
/* Khaled Alhourani -- me@khaledalhourani.com */
/* NOTE: monthNames are the original months names and they are the Arabic names, not the new months name فبراير - يناير and there isn't any Arabic roots for these months */
jQuery(function ($) {
    $.datepicker.regional['ar'] = {
        closeText: 'إغلاق',
        prevText: '&#x3c;السابق',
        nextText: 'التالي&#x3e;',
        currentText: 'اليوم',
        monthNames: ['كانون الثاني', 'شباط', 'آذار', 'نيسان', 'مايو', 'حزيران',
		'تموز', 'آب', 'أيلول', 'تشرين الأول', 'تشرين الثاني', 'كانون الأول'],
        monthNamesShort: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
        dayNames: ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'],
        dayNamesShort: ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'],
        dayNamesMin: ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'],
        weekHeader: 'أسبوع',
        dateFormat: 'dd/mm/yy',
        firstDay: 6,
        isRTL: true,
        showMonthAfterYear: false,
        yearSuffix: '',
		currentText: 'الآن'
    };
});
/*
$(function($){
$.datepicker.regional['ar'] = { // Default regional settings
		currentText: 'الآن',
		closeText: 'Done',
		ampm: false,
		amNames: ['AM', 'A'],
		pmNames: ['PM', 'P'],
		timeFormat: 'hh:mm tt',
		timeSuffix: '',
		timeOnlyTitle: 'Choose Time',
		timeText: 'Time',
		hourText: 'Hour',
		minuteText: 'Minute',
		secondText: 'Second',
		millisecText: 'Millisecond',
		timezoneText: 'Time Zone'
	};
    
    $.timepicker.setDefaults($.timepicker.regional['ar']);
});*/

