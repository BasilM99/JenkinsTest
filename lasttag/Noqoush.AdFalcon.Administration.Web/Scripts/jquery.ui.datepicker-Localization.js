/* English/UK initialisation for the jQuery UI date picker plugin. */
/* Written by Stuart. */
jQuery(function ($) {
    $.datepicker.regional['en'] = {
        closeText: 'Done',
        prevText: 'Prev',
        nextText: 'Next',
        currentText: 'Today',
        monthNames: ['January', 'February', 'March', 'April', 'May', 'June',
		'July', 'August', 'September', 'October', 'November', 'December'],
        monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
		'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
        weekHeader: 'Wk',
        dateFormat: 'dd/mm/yy',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
});

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
        firstDay: 0,
        isRTL: true,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
});

/*$(function ($) {
    $.timepicker.regional['ar'] = { // Default regional settings
        currentText: 'الآن',
        closeText: 'إغلاق',
        ampm: false,
        amNames: ['AM', 'A'],
        pmNames: ['PM', 'P'],
        timeFormat: 'hh:mm tt',
        timeSuffix: '',
        timeOnlyTitle: 'اختر الوقت',
        timeText: 'الوقت',
        hourText: 'الساعة',
        minuteText: 'الدقيقة',
        secondText: 'الثانية',
        millisecText: 'ميلي ثانية',
        timezoneText: 'منطقة زمنية'
    };
    if (currentLang == 'ar') {
    $.timepicker.setDefaults($.timepicker.regional['ar']);
    }
});*/