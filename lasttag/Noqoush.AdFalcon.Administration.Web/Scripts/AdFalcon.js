var app;
var queryBuilder;
var AccountRole = "Normal";
var GlobalAdvertiserId;
var GlobalAdvertiserAccountId;
var $window = $(window), previousScrollTop = 0, scrollLock = false;

$window.scroll(function (event) {

    if (scrollLock) {

        $window.scrollTop(previousScrollTop);

    }

    previousScrollTop = $window.scrollTop();

});
(function () {
    'use strict';

    app = angular.module('app',
        [
            'ngAnimate',                // support for CSS-based animations
            'ngTouch',                  //for touch-enabled devices
            'ui.grid',                  //data grid for AngularJS
            'ui.grid.pagination',       //data grid Pagination
            'ui.grid.resizeColumns',    //data grid Resize column
            'ui.grid.moveColumns',      //data grid Move column
            'ui.grid.pinning',          //data grid Pin column Left/Right
            'ui.grid.selection',        //data grid Select Rows
            'ui.grid.autoResize',       //data grid Enabled auto column Size
            'ui.grid.exporter',          //data grid Export Data
            'ui.grid.infiniteScroll',
            'ngSanitize', 'queryBuilder'

        ]);

    queryBuilder = angular.module('queryBuilder', []);
})();
var SSPImageSelectedList = [];

$(document).ready(function () {
    //compileAngularElement('#SSPGridAPP');
    if (typeof (uRlRequiredMsg) != "undefined") {
        jQuery.validator.addMethod("urlAutoComplete",
            function (value, element) {
                var result = ValidateUrl($(element));

                return result;
            }, uRlRequiredMsg);
    }

    setTimeout(function () {
        if (!(typeof (keepSuccessMessages) != "undefined" && keepSuccessMessages == true)) {
            hideSuccessfullyMessage();
        }
	}, parseInt(MessagesTime))

	if ($("#ImpressionTrackerTextDialog").length > 0 && (typeof (impressionDialogUpdate) != "undefined") &&  impressionDialogUpdate) {

		$("#ImpressionTrackerTextDialog").dialog({
			autoOpen: false,
			width: 490,
			height: 300,
			modal: true,
			resizable: false,
			draggable: false,

			buttons: [
				{
					text: "Save",
					click: function () {
						//SavePerm();

						$("#" + ImpressionTrackerJSRedirectName).val($("#ImpressionTrackerTextBox").val());
						$("#ImpressionTrackerTextDialog").dialog("close");


					},
					"class": 'primary-btn',

				}



			],
			open: function () {
				//  clearErrorMessage("party-divErrorMessages");
				// countcallAdv = 0;
				//Radio_check("UseHttpFormatRadio");
				//$('#ContentListItems').val("");
				//$('#tagsDDL').val("");
			}//,
			//position: { my: "center top", at: "center top"},
			//buttons: [{
			//
			//        text: '@*@Html.GetResource("Save", "Commands")*@',
			//        "class": 'primary-btn',
			//
			//        click: function () {
			//            SaveTrackingPixel();
			//        }
			//    }
			//]
		});
	}

	else if ($("#ImpressionTrackerTextDialog").length > 0) {


		$("#ImpressionTrackerTextDialog").dialog({
			autoOpen: false,
			width: 490,
			height: 300,
			modal: true,
			resizable: false,
			draggable: false,
			buttons: [
			

				//{
				//	text: "Close",
				//	click: function () {
				//		$(this).dialog('close');

				//	}
				//}
			],
			
			open: function () {
				//  clearErrorMessage("party-divErrorMessages");
				// countcallAdv = 0;
				//Radio_check("UseHttpFormatRadio");
				//$('#ContentListItems').val("");
				//$('#tagsDDL').val("");
			}//,
			//position: { my: "center top", at: "center top"},
			//buttons: [{
			//
			//        text: '@*@Html.GetResource("Save", "Commands")*@',
			//        "class": 'primary-btn',
			//
			//        click: function () {
			//            SaveTrackingPixel();
			//        }
			//    }
			//]
		});

	}


});



var ImpressionTrackerJSRedirectName = "";
function OpenImpressionTrackerDialog(a) {
	//debugger;
	ImpressionTrackerJSRedirectName = $(a).attr("ImpressionTrackerJSRedirectName");


    var opt = {
        autoOpen: false,
        modal: true,
        width: 490,
        height: 300
       
    };
	
    $("#ImpressionTrackerTextDialog").dialog(opt).dialog("open");;
	//$('#tagsTrackingPixelEventDialog').dialog('option', 'title', "test");
	$("#ImpressionTrackerTextBox").val($("#" + ImpressionTrackerJSRedirectName).val());

}

function OnChangeCommissionValue(self) {



	if ($('#' + self.id).val() == '' || parseFloat($('#' + self.id).val()) <= 0) {
		$("#AgencyCommissionValueErorMassege").show();

		return false;
	}
	else {

		$("#AgencyCommissionValueErorMassege").hide();
		//return true;
	}


	//check if  url is valid
	var cost_element_type = $("#textForAgencyCommissionValue").text();
	switch (cost_element_type) {
		case '%': //percentage
			var REGULAR = new RegExp("^\\d{1,3}(\\.\\d{1,3})?$");
			break;
		case '$': //fixed
			var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");
			break;
		default:
			var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");
	}


	var value = $('#' + self.id).val();


	if (!REGULAR.test(value) || parseFloat(value > 100 && cost_element_type == '%')) {
		$('#' + self.id).val("");
		$('#AgencyCommissionValueErorMassege').show();
		return false;
	}


	return true;
}
function OnChangeAgencyCommissionChanged() {

	$("#AgencyCommissionValue").val("0.00");
	$("#AgencyCommissionValueSec").show();
	//var Value = getCheckRadioValue($("#AgencyCommissionRadio"));
	
	var Value = $("#selectAgencyCommission").val();

	if (Value == '1') {
		$("#textForAgencyCommissionValue").text("$");

	} else {
		$("#textForAgencyCommissionValue").text("%");

	}
}




function OnKeyPressCommissionValue(e, textbox) {


	var cost_element_type = $("#textForAgencyCommissionValue").text();
	switch (cost_element_type) {
		case '%': //percentage
			var REGULAR = new RegExp("^\\d{1,3}(\\.\\d{1,3})?$");
			break;
		case '$': //fixed
			var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");
			break;
		default:
			var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");
	}
	var valStrLabe = "AgencyCommissionValueErorMassege";
	$("#" + valStrLabe).hide();
	var key = e.keyCode || e.charCode;

	if (cost_element_type == 1) {
		if (parseFloat(textbox.value) >= 100) {
			e.preventDefault();
		}
	}

	if (!(String.fromCharCode(key) == '.' && textbox.value.length > 0 && textbox.value.indexOf('.') < 0) && key != 8) {

		if (!REGULAR.test((textbox.value + String.fromCharCode(key))) || (parseFloat(textbox.value + String.fromCharCode(key)) > 100 && cost_element_type == '%')) {
			e.preventDefault();
		}
	}
}


function SetSelectedTap(selectedTap) {
    $("#" + selectedTap).find("a").eq(0).addClass("activeIcon");
}

function compileAngularElement(elSelector) {

    var elSelector = (typeof elSelector == 'string') ? elSelector : null;
    // The new element to be added
    if (elSelector != null) {
        var $div = $(elSelector);

        // The parent of the new element
        var $target = $("[ng-app]");

        angular.element($target).injector().invoke(['$compile', function ($compile) {
            var $scope = angular.element($target).scope();
            $compile($div)($scope);
            // Finally, refresh the watch expressions in the new element
            $scope.$apply();
        }]);
    }

}
function getSettings() {


}

String.prototype.format = function () {
    //alert(s);
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }

    return s;
};

var appSiteIdForAppReport;
var CampaignIdForCampaignpReport;

var CampaignBudget;
var CostElementsValuesSum = 0;
var httpsCount = 0;
var httpCount = 0;
var urlCount = 0;
var adgroupid;
var campaign_id;
var GetCostElementsValuesUrl;
var CampaignBidConfigNotCompleted = true;
var DashBoardChartGoogle;
var ReportChartGoogle;
var checked_items = new Array();

function TelerikgridInitilizeForMigration(jqelem) {
    if (typeof (jqelem.data().tGrid) === "undefined") {
        jqelem.tGrid();

    }

}
function fillselect2multipe(values, nameoflist) {
    //return;
    //
    var arrpush = [];
    $.each(values.split(","), function (i, e) {
        if (e != "Not Set" && e != "" && e != null && e != undefined) {
            //if (!/[0-9]+$/.test(e)) {
            //    //var newList = $.merge($(nameoflist).select2('data'), [{
            //    //    id: e,
            //    //    tag: e
            //    //}]);
            //    //$(nameoflist).select2('data', newList);
            //    //$(nameoflist).append('<option value="' + e + '">' + e + '</option>');

            //    $(nameoflist).select2().val(e).trigger("change");
            //}
            //else {
            //    $(nameoflist).select2().val(e).trigger("change");
            //}

            arrpush.push(e);
            $(nameoflist + " option[value='" + e + "']").prop("selected", true);

        }
    });

    if (arrpush.length > 0)
        $(nameoflist).val(arrpush).trigger("change");
}
function fillAutocomplete(value, AutocompleteName) {
    $("[id='" + AutocompleteName + "']").val(text);
}
function select_app_site(item) {
    if ($(item).is(':checked')) {
        if (checked_items.indexOf($(item).val().toString()) < 0) {
            checked_items.push($(item).val());
        }
    } else {
        var index = checked_items.indexOf($(item).val().toString());
        if (index > -1) {
            checked_items.splice(index, 1);
        }
    }

    $('[name="DestinationAppSites"]').val(checked_items.join(","));
}
function clear_selected_apps(e) {
    checked_items = new Array();
    $('[name="DestinationAppSites"]').val(null);
};
function onAppSiteRowDataBound(e) {


    var row = jQuery(e.row);
    if (row.find('td:eq(1)').children()[0].innerText.length > 40) {


        row.find('td:eq(1)').children()[0].title = row.find('td:eq(1)').children()[0].innerText;

        var name = row.find('td:eq(1)').children()[0].innerText.substring(0, 31) + "..";



        row.find('td:eq(1)').children()[0].childNodes[0].innerText = name;
    }
    if (checked_items.indexOf(e.dataItem.Id.toString()) > -1) {
        row.find("[name='selected_item_id']").attr('checked', "true");
    }
};
/////////////////////////////Time Control Start /////////////////////////////
function update_time_from_control(name) {
    var elem = $('[name="' + name + '"]');
    var hours = parseInt($('[name="' + name + '_hour"]').val());
    var mins = parseInt($('[name="' + name + '_min"]').val());
    if (isNaN(hours) && isNaN(mins)) {
        elem.val('');
        $("#" + name + "-time-control-clear-td").hide();
        return null;
    } else {
        if (isNaN(hours))
            hours = 0;
        if (isNaN(mins))
            mins = 0;
        var date = new Date();
        date.setHours(hours, mins, 0);
        elem.val(date.toString(timeFormat));
        $("#" + name + "-time-control-clear-td").show();
        return date;
    }
}
function Get_time_from_control(name) {
    return update_time_from_control(name);
}
function reset_time_control(name) {
    $('[name="' + name + '_hour"]').val('');
    $('[name="' + name + '_min"]').val('');
    $("#" + name + "-time-control-clear-td").hide();

    update_time_from_control(name);
}
/////////////////////////////Time Control End /////////////////////////////
function hasValue(obj) {
    return ((typeof (obj) != "undefined") && (obj != null) && (obj != ''));
}
function rebindGrid(gridName) {
    var grid = $("#" + gridName).data("tGrid");
    if (grid)
        grid.rebind();
}


function TelerikgridInitilizeForMigration(jqelem) {
    if (typeof (jqelem.data().tGrid) === "undefined") {
        jqelem.tGrid();

    }

}
function gridDataBound(e) {
    var grid = $(this).data("tGrid");
    //if not first page and the no data then try load first page
    if ((grid.data.length == 0) && (grid.currentPage > 1)) {
        e.preventDefault();
        grid.pageTo(1);
    }
};
function RefrashGrid(gridName) {

    var grid = $("#" + gridName).data("tGrid");
    grid.pageTo(grid.currentPage);
}
function showDialog(name) {

    jQuery('#' + name).dialog('open');
}
function showDialog(name, settings) {
    jQuery('#' + name).dialog(settings);
    jQuery('#' + name).dialog('open');
}
function closeDialog(name) {
    jQuery('#' + name).dialog('close');
}
function redirect(url) {
    document.location = url;
}
String.prototype.format = function () {
    var formatted = this;
    for (arg in arguments) {
        formatted = formatted.replace("{" + arg + "}", arguments[arg]);
    }
    return formatted;
};
var testPattern = function (value, pattern) { // Private Method

    var regExp = new RegExp(pattern, "");
    return regExp.test(value);
};
function ipIsInvaildDialog(msg) {
    if (typeof (msg) == "undefined" || msg == "") {
        msg = ip_not_valid_msg;
    }
    $('<div id = "ipIsInvaildDialog" title =' + warning + '></div>').dialog({

        open: function (event, ui) {
            $(this).html(msg);
        },
        close: function () {
            $(this).remove();
        },
        resizable: false,
        draggable: false,
        //height: 140,
        modal: true,
    });


}
function isSingleIP(elem) {
    //$(".t-grid-content").find("table tbody tr td").attr('title', value);
    elemjq = $(elem);

    if (!validate_IP_Single(elemjq.val())) {
        //  jQuery(elem).parents('td').find('#display_ip_msg').css('display', 'block')();// 
        // StartRange.val('');
        ipIsInvaildDialog();
        return false;
    }


    var grid = $("#IPRangesGrid").data("tGrid");
    var tr = $(elem).parents('td');
    //tr.append(elem.value);;

    elem.value = elemjq.val();

    //  jQuery(elem).parents('td').find('#display_ip_msg').hide();
    return true;
}
function isIP(elem, value) {
    var IpEndElem = jQuery(elem).parents('td').parent('tr').find('#EndIPRange');

    if (!validate_IP_Range(value, IpEndElem.text())) {
        ipIsInvaildDialog();
        deleteIPTargeting(elem);
        return false;
    } else if (!checkDuplicatedIpRange(IpEndElem.text(), value)) {

        deleteIPTargeting(elem);
        ipIsInvaildDialog(DuplicatedEnitiy);
        jQuery(elem).parents('td').find('#display_ip_msg').hide();

        return false;
    }

    jQuery(elem).parents('td').find('#display_ip_msg').hide();
    return true;
}
function isEndIP(elem, value) {

    var IpStartElem = jQuery(elem).parents('td').parent('tr').find('#StartIPRange');

    if (!validate_IP_Range(IpStartElem.text(), value)) {
        ipIsInvaildDialog();
        deleteIPTargeting(elem);
        return false;

    } else if (!checkDuplicatedIpRange(value, IpStartElem.text())) {
        deleteIPTargeting(elem);
        ipIsInvaildDialog(DuplicatedEnitiy);
        jQuery(elem).parents('td').find('#display_ip_msg').hide();

        return false;
    }
    jQuery(elem).parents('td').find('#display_ip_msg').hide();

    return true;

}
function validate_IP_Single(start) {

    var ipRE = new RegExp('^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$');
    var ipv6Req = new RegExp('(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))');



    if ((ipRE.test(start) && start != null)) {
        return true;
    }


    if ((ipv6Req.test(start) && start != null)) {
        return true;
    }
    return false;
}
function validate_IP_Range(start, end) {

    var ipRE = new RegExp('^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$');
    var ipv6Req = new RegExp('(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))');

    //if (start.indexOf("/") < 0 && end == "") {
    //    return false;
    //}
    start = start.trim();
    end = end.trim();

    if ((ipRE.test(start) && start != null) && (ipRE.test(end) || end == "")) {
        return true;
    }
    //if (end.indexOf("/") > 0) {
    //    //   alert(ip_not_valid_msg)
    //    return false;
    //}
    //if (start.indexOf("/") > 0 && end != "") {
    //    return false;
    //}
    if ((ipv6Req.test(start) && start != null) && (ipv6Req.test(end) || end == "")) {
        return true;
    }
    return false;
}
//function isURL(val, elem) {

//    //var val2 = $.trim(val);
//    var val2 = val.replace(/\s/g, '');
//    $(elem).val(val2);
//    // if no url, don't do anything
//    if (val2.length == 0) { return true; }
//    // if user has not entered http:// https:// or ftp:// assume they mean http://
//    if (!/^(https?):\/\//i.test(val2) && isMatchUrlExpressionWithoutProtocol(val2)) {
//        val2 = 'http://' + val2; // set both the value
//        $(elem).val(val2); // also update the form element
//    }
//    var result = isMatchUrlExpression(val2);
//    if(result)
//    {

//        //var regExp2 = new RegExp(/^(?:(?:https?|ftp):\/\/)(?:\S+(?::\S*)?@)?(?:(?!10(?:\.\d{1,3}){3})(?!127(?:\.\d{1,3}){3})(?!169\.254(?:\.\d{1,3}){2})(?!192\.168(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/[^\s]*)?$/);


//        var regExp2 = new RegExp(/^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?/);

//        var result2 = regExp2.test(val2);
//        return result && result2;
//    }

//    return result;

//    //return /^[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*$/i.test(val);
//};


function isURL(val, elem) {

    //var val2 = $.trim(val);
    var val2 = val.replace(/\s/g, '');
    $(elem).val(val2);
    // if no url, don't do anything
    if (val2.length == 0) {

        if (urlCount >= 1)
            urlCount--;
        return true;

    }

    // if user has not entered http:// https:// or ftp:// assume they mean http://
    if (!/^(https?):\/\//i.test(val2) && isMatchUrlExpressionWithoutProtocol(val2)) {
        val2 = 'http://' + val2; // set both the value
        $(elem).val(val2); // also update the form element
    }
    //
    val0 = val2.toLowerCase();
    var result = isMatchUrlExpression(val0);

    if (result)
        urlCount++;

    if (/^(https):\/\//i.test(val2) && result) {
        httpsCount++;
        if (httpCount >= 1)
            httpCount--;


    }
    else {

        if (result)
            httpsCount--;
        if (/^(http):\/\//i.test(val2) && result) {
            httpCount++;
        }
    }

    //if ($("#AdCreativeDto_IsSecureCompliant").length && httpCount == 0 && urlCount >= 1) {
    //    $("#AdCreativeDto_IsSecureCompliant").val("True");
    //}
    //else {

    //    $("#AdCreativeDto_IsSecureCompliant").val("False");
    //}

    //if (result) {

    //    //var regExp2 = new RegExp(/^(?:(?:https?|ftp):\/\/)(?:\S+(?::\S*)?@)?(?:(?!10(?:\.\d{1,3}){3})(?!127(?:\.\d{1,3}){3})(?!169\.254(?:\.\d{1,3}){2})(?!192\.168(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/[^\s]*)?$/);



    //    var regExp2 = new RegExp(/^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?/);
    //    val3 = val2.toLowerCase();
    //    var result2 = regExp2.test(val3);
    //    if (result2) {
    //        var indexof2dots = val2.indexOf("..");
    //        var indexof2hashes = val2.indexOf(":/", 7);
    //        var indexof2hashes = val2.indexOf(".-");
    //        var indexofEndDomaindots = val2.indexOf("/", 7);
    //        var indexofquerydots = val2.indexOf("?", 7);
    //        var indexofHashdots = val2.indexOf("#", 7);
    //        if (indexof2dots > 1) {
    //            if (indexof2dots < indexofquerydots || indexof2dots < indexofEndDomaindots || indexof2dots < indexofHashdots) {
    //                return false;
    //            }

    //        }
    //        if (indexof2hashes > 7) {
    //            if (indexofquerydots != -1 || indexofHashdots != -1) {
    //                if (((indexof2hashes < indexofquerydots) && (indexof2hashes < indexofEndDomaindots)) || (indexof2hashes < indexofEndDomaindots && (indexof2hashes < indexofquerydots || indexof2hashes < indexofHashdots)) || ((indexof2hashes < indexofHashdots) && (indexof2hashes < indexofEndDomaindots))) {
    //                    return false;
    //                }
    //            }

    //            else {
    //                if (indexof2hashes < indexofEndDomaindots) {
    //                    return false;
    //                }

    //            }


    //        }


    //    }

    //    return result && result2;
    //}

    return result;

    //return /^[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*$/i.test(val);
};

function isMatchUrlExpression(value) {
    ///^[w-]+(.[w-]+)+([w.,@?^=%&amp;:/~+#-]*[w@?^=%&amp;/~+#-])?i/

    // /^(?:(?!PATTERN).)*$
    //  return /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?/i.test(value);
    //var regExp = new RegExp(/(http|ftp|https):\/\/[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:\/~+#-]*[\w@?^=%&amp;\/~+#-])?/);
    //  var regExp = new RegExp(/(http|ftp|https):\/\/[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:\/~+#-]*[\w@?^=%&amp;\/~+#-])?/);
    var re_weburl = new RegExp(
        "^" +
        // protocol identifier 
        "(?:(?:http|ftp|https)://)" +
        // user:pass authentication 
        "(?:\\S+(?::\\S*)?@)?" +
        "(?:" +
        // IP address exclusion 
        // private & local networks 
        "(?!(?:10|127)(?:\\.\\d{1,3}){3})" +
        "(?!(?:169\\.254|192\\.168)(?:\\.\\d{1,3}){2})" +
        "(?!172\\.(?:1[6-9]|2\\d|3[0-1])(?:\\.\\d{1,3}){2})" +
        // IP address dotted notation octets 
        // excludes loopback network 0.0.0.0 
        // excludes reserved space >= 224.0.0.0 
        // excludes network & broacast addresses 
        // (first & last IP address of each class) 
        "(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])" +
        "(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}" +
        "(?:\\.(?:[1-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))" +
        "|" +
        // host name 
        "(?:(?:[a-z\\u00a1-\\uffff0-9]-*)*[a-z\\u00a1-\\uffff0-9]+)" +
        // domain name 
        "(?:\\.(?:[a-z\\u00a1-\\uffff0-9]-*)*[a-z\\u00a1-\\uffff0-9]+)*" +
        // TLD identifier 
        "(?:\\.(?:[a-z\\u00a1-\\uffff]{2,}))" +
        // TLD may end with dot 
        "\\.?" +
        ")" +
        // port number 
        "(?::\\d{2,5})?" +
        // resource path 
        "(?:[/?#]\\S*)?" +
        "$", "i"
    );
    if (AllowPrivateIPValidation == "True") {
        re_weburl = new RegExp(
            "^" +
            // protocol identifier 
            "(?:(?:http|ftp|https)://)" +
            // user:pass authentication 
            "(?:\\S+(?::\\S*)?@)?" +
            "(?:" +
            // IP address exclusion 
            // private & local networks 
            "(?!(?:10|127)(?:\\.\\d{1,3}){3})" +
            "(?!(?:169\\.254)(?:\\.\\d{1,3}){2})" +
            "(?!172\\.(?:1[6-9]|2\\d|3[0-1])(?:\\.\\d{1,3}){2})" +
            // IP address dotted notation octets 
            // excludes loopback network 0.0.0.0 
            // excludes reserved space >= 224.0.0.0 
            // excludes network & broacast addresses 
            // (first & last IP address of each class) 
            "(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])" +
            "(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}" +
            "(?:\\.(?:[1-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))" +
            "|" +
            // host name 
            "(?:(?:[a-z\\u00a1-\\uffff0-9]-*)*[a-z\\u00a1-\\uffff0-9]+)" +
            // domain name 
            "(?:\\.(?:[a-z\\u00a1-\\uffff0-9]-*)*[a-z\\u00a1-\\uffff0-9]+)*" +
            // TLD identifier 
            "(?:\\.(?:[a-z\\u00a1-\\uffff]{2,}))" +
            // TLD may end with dot 
            "\\.?" +
            ")" +
            // port number 
            "(?::\\d{2,5})?" +
            // resource path 
            "(?:[/?#]\\S*)?" +
            "$", "i"
        );
    }
    return re_weburl.test(value);
};
function isMatchUrlExpressionWithoutProtocol(value) {
    //  return /^(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?/i.test(value);
    var regExp = new RegExp(/[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:/~+#-]*[\w@?^=%&amp;/~+#-])?/);
    return regExp.test(value);
};

function isEmail(elem, error_msg_id) {

    if (!hasValue(error_msg_id)) {
        error_msg_id = "displayURLErrorMsg";
    }
    $('#' + error_msg_id).hide();
    var val = elem.val();
    var reg = new RegExp(/[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i);
    var result = reg.test(val);
    if ((val != null) && $.trim(val) != '') {
        if (!result) {
            $('#' + error_msg_id).show();
        }
        else {
            $('#' + error_msg_id).hide();
        }
    }

    return result;
};
function IsBidValidDecimalExpression(value) {

    var isBid = new RegExp('^\\$?\\d{1,5}(\\.(\\d{1,3}))?$');
    return isBid.test(value);
}

function isPhone(val) {
    val = $.trim(val);
    if (val.length == 0) {
        return true;
    }
    var phoneNumberPattern = /^[#+*]?[\d\-\s]{0,}$/;
    return phoneNumberPattern.test(val);
};
function checkBox(elem) {

    var elemObj = jQuery(elem);
    var chbspan = elemObj.find('#chbspan');
    var ckeckbox = elemObj.find(':checkbox');
    if (elemObj.attr('disabled')) {
        return false;
    }
    if (ckeckbox.attr('checked')) {
        ckeckbox.attr('checked', false);

        ckeckbox[0].checked = false;
        chbspan.removeClass("check-box-checked");
        chbspan.addClass("check-box-uncheck");
    }
    else {
        ckeckbox.attr('checked', true);

        ckeckbox[0].checked = true;
        chbspan.addClass("check-box-checked");
        chbspan.removeClass("check-box-uncheck");
    }

    var customOnChange = elemObj.attr('customOnChange');
    if (typeof (customOnChange) != "undefined") {
        var customOnChangeCall = customOnChange + '(elemObj,"' + elemObj.attr('customValue') + '");';

        eval(customOnChangeCall);
    }
}
function radioBox(elem, source, reset) {
    if (source == elem)
        return;
    var elemObj = jQuery(elem);
    var rbspan = elemObj.find('#rbspan');
    var customOnChange = elemObj.attr('customOnChange');
    var radiobutton = elemObj.find(':radio');
    //if (radiobutton.attr('checked') || ((typeof (reset) != "undefined") && !reset)) {
    if (radiobutton.attr('checked') && (typeof (reset) == "undefined")) {
        //        radiobutton.attr('checked', false);
        //        rbspan.removeClass("radio-button-checked");
        //        rbspan.addClass("radio-button-uncheck");
        return;
    }
    if (radiobutton.attr('checked') || ((typeof (reset) != "undefined") && !reset)) {
        radiobutton.attr('checked', false);
        radiobutton[0].checked = false;
        rbspan.removeClass("radio-button-checked");
        rbspan.addClass("radio-button-uncheck");
    }
    else {
        radiobutton.attr('checked', true);

        radiobutton[0].checked = true;
        rbspan.addClass("radio-button-checked");
        rbspan.removeClass("radio-button-uncheck");
    }
    if ((typeof (reset) == "undefined") || (reset)) {
        jQuery('div[name = "' + elemObj.attr('name') + '"]').each(function () {
            radioBox(this, elem, false);
        });
        if (typeof (customOnChange) != "undefined") {

            var customOnChangeCall = customOnChange + '(radiobutton,"' + radiobutton.attr('customValue') + '");';
            eval(customOnChangeCall);
        }
    }
};

function cloneElement(itemToCopyName, cloneCallBackFunction, removeCallBackFunction, igonreClickFunction, ignoreKeyUp, trackerContainerId) {
    var trackerContainerElementId = typeof (trackerContainerId) == 'undefined' || trackerContainerId == "" ? "trackerContainer" : trackerContainerId

    var randonNumber = Math.floor(Math.random() * 1000);
    var itemToCopy = $(itemToCopyName);

    var parentItem = $(itemToCopy[0]).parent();
    var clonedItem = itemToCopy.clone(false);
    var clonedItemId = trackerContainerElementId + randonNumber;
    clonedItem.attr("id", clonedItemId);

    var clonedItemTextBoxId = "clickTracker" + randonNumber;
    var textBox = clonedItem.find(":text");
    textBox.attr("id", clonedItemTextBoxId);
    textBox.val("");
    textBox.attr("onkeyup", "");
    textBox.unbind("autocompleteselect");
    if (null != ignoreKeyUp && undefined != ignoreKeyUp && ignoreKeyUp == true) {
    } else {
        textBox.keyup(function () {
            managePlusIcon(clonedItemId);
        });
    }
    var plusIcon = clonedItem.find(".plusicon");
    plusIcon.attr("onclick", "");
    if (null != igonreClickFunction && undefined != igonreClickFunction && igonreClickFunction == true) {
        //igonre Click Function
    } else {
        plusIcon.click(function () {
            cloneElement("#" + clonedItemId, onCopyTracker);
        });
    }

    if (clonedItem.find(".minusicon").length == 0) {
        var minusIcon = plusIcon.clone(false);
        minusIcon.removeClass("plusicon").addClass("minusicon");
        minusIcon.attr("onclick", "");
        minusIcon.click(function () {
            removeClonedElement(clonedItemId, removeCallBackFunction);
        });
        minusIcon.show();
        plusIcon.before(minusIcon);
    } else {
        var minusIcon = clonedItem.find(".minusicon");
        minusIcon.attr("onclick", "");
        minusIcon.click(function () {
            removeClonedElement(clonedItemId, removeCallBackFunction);
        });
    }

    plusIcon.hide();

    clonedItem.appendTo(parentItem);
    itemToCopy.find("img.plusicon").hide();
    if (cloneCallBackFunction != undefined && cloneCallBackFunction != null) {
        cloneCallBackFunction(itemToCopy, clonedItem);
    }
}
function removeClonedElement(itemToDelete, removeCallBackFunction) {
    var previousElement = $("#" + itemToDelete).prev();

    $("#" + itemToDelete).remove();
    previousElement.find(".plusicon").show();


    if (removeCallBackFunction != undefined && removeCallBackFunction != null) {
        removeCallBackFunction(previousElement);
    }
}
function radioBoxChange(elem) {
    radioBox(elem);

};

function checkBoxChange(elem) {
    checkBox(elem);

};

function getPosition(elemnt) {

    if (currentDirection == 'rtl') {
        var elemObj = jQuery(elemnt);
        var offset = jQuery(elemnt).position();
        var leftvalue = elemObj.offsetParent().width() - offset.left + elemObj.width();
        return {
            top: offset.top, left: leftvalue
        };
    }
    else {
        return jQuery(elemnt).position();
    }
}
function getPositionProperty() {
    if (currentDirection == 'rtl') {
        return "right";
    }
    else {
        return "left";
    }
};
function getSpinner(container) {

    var containerObj = jQuery("#" + container);
    var spinner = containerObj.find(".spinner-container");
    var spinnerTemplate = jQuery(".spinner-container");

    if (spinner.length < 1) {
        spinnerTemplate.clone().appendTo(containerObj);
        spinner.css({
            position: "relative"
        });
        spinner = containerObj.find(".spinner-container").attr("id", "spinnerCriteria");
        spinner.showSpinner = function () {

            spinner.show();
            var offset = getPosition(containerObj);
            spinner.width(containerObj.width());
            spinner.height(containerObj.height());
            spinner.css("top", offset.top);

            spinner.css(getPositionProperty(), offset.left);
            spinner.find("img").css({
                marginTop: "17%", marginRight: "45%"
            });
        };
        spinner.hideSpinner = function () {
            spinner.hide();
        };
    }
    return spinner;
};

function getSpinnerType(container) {

    var containerObj = jQuery("#" + container);
    var spinner = containerObj.find(".spinner-container");
    var spinnerTemplate = jQuery(".spinner-container");
    if (spinner.length < 1) {
        spinnerTemplate.clone().appendTo(containerObj);
        spinner.css({
            position: "relative"
        });
        spinner = containerObj.find(".spinner-container").attr("id", "spinnerCriteria");

    }

    spinner.showSpinner = function () {

        spinner.show();
        var offset = getPosition(containerObj);
        spinner.width(containerObj.width());
        spinner.height(containerObj.height());
        spinner.css("top", offset.top);
        //spinner.css("z-index", 100000000000000000);
        spinner.css(getPositionProperty(), offset.left);
        spinner.find("img").css({
            marginTop: "17%", marginRight: "45%"
        });
    };
    spinner.hideSpinner = function () {
        spinner.hide();
    };

    return spinner;
};
// this function create an Array that contains the JS code of every <script> tag in parameter
// then apply the eval() to execute the code in every script collected
function parseScript(strcode) {
    var scripts = new Array();         // Array which will store the script's code

    // Strip out tags
    while (strcode.indexOf("<script") > -1 || strcode.indexOf("</script") > -1) {
        var s = strcode.indexOf("<script");
        var s_e = strcode.indexOf(">", s);
        var e = strcode.indexOf("</script", s);
        var e_e = strcode.indexOf(">", e);

        // Add to scripts array
        scripts.push(strcode.substring(s_e + 1, e));
        // Strip from strcode
        strcode = strcode.substring(0, s) + strcode.substring(e_e + 1);
    }

    // Loop through every script collected and eval it
    for (var i = 0; i < scripts.length; i++) {
        try {
            jQuery.globalEval(scripts[i]);
            //eval(scripts[i]);
        }
        catch (ex) {
            // do what you want here when a script fails
        }
    }
};
var divErrorMsgTimer = null;
//var divSuccessfullyMsgTimer = null;
function hideSuccessfullyMessage() {


    if (divErrorMsgTimer != null)
		clearTimeout(divErrorMsgTimer);
	//clearAllSuccessMsg();
    $("#divSuccessMessages").hide("fast");
};

function showErrorMessage(errorMessage, keep, div) {

    clearErrorMessage(div);
    hideSuccessfullyMessage();
    if (divErrorMsgTimer != null)
        clearTimeout(divErrorMsgTimer);
    var divErrorMsg = "";

    if (typeof (div) != "undefined" && div != "") {
		divErrorMsg = $("#" + div);


    } else {
        divErrorMsg = $("#divErrorMessages");
		clearAllWarningMsg();
		clearAllSuccessMsg();
		//clearAllErrorMsg();
		clearAllInfoMsg();
		if (keep)
			showNotfy(errorMessage, "error", '', toastroptionsWithNoTime)
		else
			showNotfy(errorMessage, "error", '', toastroptionsWithTime)

		return;
    }
    var html = divErrorMsg.html();
    html += "<div class='data-row'><span class='msg-img'></span><span>" + errorMessage + "</span></div>";
    divErrorMsg.html(html);
    divErrorMsg.show();
    $('html, body').animate({ scrollTop: 0 }, 0);
    if ((typeof (keep) == "undefined") || (keep == false)) {
        divErrorMsgTimer = setTimeout("hideErrorMessage()", parseInt(3600000));
	}






};

function hideErrorMessage() {
    if (divErrorMsgTimer != null)
		clearTimeout(divErrorMsgTimer);
	//clearAllErrorMsg();
	$("#divErrorMessages").hide("fast");

	//toastr.clear();
};
function clearErrorMessage(div) {

    if (divErrorMsgTimer != null)
        clearTimeout(divErrorMsgTimer);
    var divErrorMsg = "";

    if (typeof (div) == "undefined") {
		divErrorMsg = $("#divErrorMessages");
		//toastr.clear();
		//clearAllErrorMsg();
		//return;
    } else {
        divErrorMsg = $("#" + div);
    }
    divErrorMsg.hide().html(''); //.hide("fast");
};
function showSuccessfullyMessage(message, keep) {

    hideErrorMessage();
	clearSuccessfullyMessage();
	//clearAllWarningMsg();
	clearAllSuccessMsg();
	clearAllErrorMsg();
	//clearAllInfoMsg();

	if (keep)
		showNotfy(message, "success", '', toastroptionsWithNoTime)
	else
		showNotfy(message, "success", '', toastroptionsWithTime)

	return;
    /*if (divErrorMsgTimer != null)
        clearTimeout(divErrorMsgTimer);*/
    var divMsg = $("#divSuccessMessages");
    var html = divMsg.html();
    html += "<div class='data-row'><span class='msg-img'></span><span>" + message + "</span></div>";
    divMsg.html(html);
    divMsg.show();
    $('html, body').animate({ scrollTop: 0 }, 0);

    if ((typeof (keep) == "undefined") || (keep == false)) {
        divErrorMsgTimer = setTimeout("hideSuccessfullyMessage()", parseInt(MessagesTime));
    }
};




function hideSuccessfullyMessage() {


    /*if (divErrorMsgTimer != null)
        clearTimeout(divErrorMsgTimer);*/

	//clearAllSuccessMsg();
    $("#divSuccessMessages").hide("fast");
};
function clearSuccessfullyMessage() {

	
    /*if (divErrorMsgTimer != null)
        clearTimeout(divErrorMsgTimer);*/
    $("#divSuccessMessages").hide().html(''); //.hide("fast");
};


function showWarningMessage(message, keep) {
    hideWarningMessage();
	var divMsg = $("#divWarnMessages");
	clearAllWarningMsg();
	//clearAllSuccessMsg();
	//clearAllErrorMsg();
	//clearAllInfoMsg();
	if (keep)
		showNotfy(message, "warning", '', toastroptionsWithNoTime)
	else
		showNotfy(message, "warning", '', toastroptionsWithTime)

	return;

    var html = divMsg.html();
    html += "<div class='data-row'><span class='msg-img'></span><span>" + message + "</span></div>";
    divMsg.html(html);
    divMsg.show();
    $('html, body').animate({ scrollTop: 0 }, 0);
    if ((typeof (keep) == "undefined") || (keep == false)) {
        divErrorMsgTimer = setTimeout("hideWarningMessage()", parseInt(3600000));
    }
};
function hideWarningMessage() {
	//clearAllWarningMsg();

	$("#divWarnMessages").hide("fast");

};
function clearWarningMessage() {

    $("#divWarnMessages").hide().html(''); //.hide("fast");
};

function enlargeImage(imageUrl) {

    $("#enlargeDialog-form").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        draggable: false,
        close: function () {
        }
    });

    $("#largeImage").one('load', function () {
        var imgWidth = $(this).width();
        var imgHeight = $(this).height();

        $("#enlargeDialog-form").dialog("option", "width", imgWidth + 30);
        $("#enlargeDialog-form").dialog("option", "height", imgHeight + 70);
        $("#enlargeDialog-form").dialog("option", "position", {
            my: "center center"
        });


    });

    $("#largeImage").attr("src", imageUrl);
    $('#enlargeDialog-form').dialog('open');
}

(function ($) {
    $.validator.unobtrusive.parseDynamicContent = function (selector) {

        var len = $(selector).length;

        //alert('got length');
        if ($(selector).length == 0) {
            alert('The selector (usually a div) passed in as the root level to start validation parsing at (rather than parsing the whole document again) could not be found in the DOM. Validation on this form will not likely continue. The selector parameter is:' + selector);
            return;
        }
        //use the normal unobstrusive.parse method
        $.validator.unobtrusive.parse(selector);

        //get the relevant form
        var form = $(selector).first().closest('form');
        if (form.length == 0) {
            alert('Could not find a form that was a parent of selector:' + selector + '\nValidation may not work properly');
            return;
        }


        //get the collections of unobstrusive validators, and jquery validators
        //and compare the two
        var unobtrusiveValidation = form.data('unobtrusiveValidation');
        //alert(unobtrusiveValidation.length);
        var validator = form.validate();

        $.each(unobtrusiveValidation.options.rules, function (elname, elrules) {

            if ($('[name="' + elname + '"]').hasClass("ValdationIgnored")) {
                $('[name="' + elname + '"]').removeAttr("data-val");
                $('[name="' + elname + '"]').removeClass("required");

                return;
            }
            if (validator.settings.rules[elname] == undefined) {
                var args = {
                };
                $.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                //$('[name=' + elname + ']').rules("add", args);
                $('[name="' + elname + '"]').rules("add", args);
            } else {
                $.each(elrules, function (rulename, data) {
                    if (validator.settings.rules[elname][rulename] == undefined) {
                        var args = {
                        };
                        args[rulename] = data;
                        args.messages = unobtrusiveValidation.options.messages[elname][rulename];

                        $('[name="' + elname + '"]').rules("add", args);
                    }
                });
            }
        });

    }
})($);


/**
* A class to parse color values
* @author Stoyan Stefanov <sstoo@gmail.com>
* @link   http://www.phpied.com/rgb-color-parser-in-javascript/
* @license Use it if you like it
*/
function RGBColor(color_string) {
    this.ok = false;
    if (color_string.length != 7) {
        this.ok = false;
        return;
    }
    // strip any leading #
    if (color_string.charAt(0) == '#') { // remove # if any
        color_string = color_string.substr(1, 6);
    }
    else {
        this.ok = false;
        return;
    }

    color_string = color_string.replace(/ /g, '');
    color_string = color_string.toLowerCase();
    // array of color definition objects
    var color_defs = [
        {
            re: /^rgb\((\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3})\)$/,
            example: ['rgb(123, 234, 45)', 'rgb(255,234,245)'],
            process: function (bits) {
                return [
                    parseInt(bits[1]),
                    parseInt(bits[2]),
                    parseInt(bits[3])
                ];
            }
        },
        {
            re: /^(\w{2})(\w{2})(\w{2})$/,
            example: ['#00ff00', '336699'],
            process: function (bits) {
                return [
                    parseInt(bits[1], 16),
                    parseInt(bits[2], 16),
                    parseInt(bits[3], 16)
                ];
            }
        }

    ];

    // search through the definitions to find a match
    for (var i = 0; i < color_defs.length; i++) {
        var re = color_defs[i].re;
        var processor = color_defs[i].process;
        var bits = re.exec(color_string);
        if (bits) {
            channels = processor(bits);
            this.r = channels[0];
            this.g = channels[1];
            this.b = channels[2];
            this.ok = true;
        }

    }

    // validate/cleanup values
    this.r = (this.r < 0 || isNaN(this.r)) ? 0 : ((this.r > 255) ? 255 : this.r);
    this.g = (this.g < 0 || isNaN(this.g)) ? 0 : ((this.g > 255) ? 255 : this.g);
    this.b = (this.b < 0 || isNaN(this.b)) ? 0 : ((this.b > 255) ? 255 : this.b);

    // some getters
    this.toRGB = function () {
        return 'rgb(' + this.r + ', ' + this.g + ', ' + this.b + ')';
    }
    this.toHex = function () {
        var r = this.r.toString(16);
        var g = this.g.toString(16);
        var b = this.b.toString(16);
        if (r.length == 1) r = '0' + r;
        if (g.length == 1) g = '0' + g;
        if (b.length == 1) b = '0' + b;
        return '#' + r + g + b;
    }
}

function downlaodByPostAction(path, params) {
    method = "post"; // Set method to post by default if not specified.

    // The rest of this code assumes you are not using a library.
    // It can be made less wordy if you use one.
    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);

    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            var hiddenField = document.createElement("input");
            hiddenField.setAttribute("type", "hidden");
            hiddenField.setAttribute("name", key);
            hiddenField.setAttribute("value", params[key]);

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
}

function changeReportViewTab(containerId) {

    var change = '';
    var returnToOriginal = '';

    if ($("#" + containerId + "AnchorGrid").hasClass("selected")) {
        returnToOriginal = 'Grid';
        change = 'Chart';
    } else {
        returnToOriginal = 'Chart';
        change = 'Grid';
    }

    $("#" + containerId + change).show();
    $("#" + containerId + returnToOriginal).hide();

    $("#" + containerId + "Anchor" + change).addClass("selected");
    $("#" + containerId + "Anchor" + returnToOriginal).removeClass("selected");
}

/////////////////////////////Common End /////////////////////////////

/////////////////////////////Tree Start /////////////////////////////
function updateTreeFlag(treeName, status) {
    isLoading = true;
    var isfound = false;
    for (var i = 0; i < treeFalgs.length; i++) {
        var item = treeFalgs[i];
        if (item.name == treeName) {
            item.status = status;
            isfound = true;
            break;
        }
    }
    if (!isfound) {
        var item = new Object();
        item.name = treeName;
        item.status = status;
        treeFalgs[treeFalgs.length] = item;
    }
    if (status) {
        checkTreeStatus();
    }
}
function checkTreeStatus() {
    var completed = true;
    for (var i = 0; i < treeFalgs.length; i++) {
        if (treeFalgs[i].status == false) {
            completed = false;
            break;
        }
    }
    if (completed) {
        if ((isLoading == true) && (typeof (treeLoadedHandler) != "undefined")) {
            isLoading = false;
            treeLoadedHandler();
        }
    }
}
function InitTree(treeName, TreeURL, callback, isSubLevel, prams) {
    //    var index = jQuery.inArray(, treeFalgs);
    //    if (index == -1) {
    //        treeFalgs[treeFalgs.length] = treeName;
    //    }

    //var spinner = getSpinner(treeName + 'TreeContainer');
    // spinner.showSpinner();
    var checkStatus = false;
    if (typeof (isSubLevel) == 'undefined')
        isSubLevel = false;
    if (typeof (prams) == 'undefined') {
        prams = new Object();
        updateTreeFlag(treeName, false);
        checkStatus = true;
    }

    /*prams = jQuery.toJSON(prams);*/
    if (TreeURL == null || TreeURL == "")
        return;
    jQuery.ajax(
        {
            dataType: "text json",
            contentType: 'application/json; charset=utf-8',
            type: "GET",
            data: prams,
            url: TreeURL,
            cache: false,
            success: function (result) {
                //eval(treeName + "Items=result;")
                initTree(treeName, result);
                initAutoComplete(treeName, result);
                eval('setSelected' + treeName + "();");
                if (typeof (callback) != 'undefined') {

                    callback();
                }
                //spinner.hideSpinner();
                if (checkStatus) {
                    updateTreeFlag(treeName, true);
                }
            },
            error: function (error) {
                //alert(error.responseText);
                //spinner.hideSpinner();
                if (checkStatus) {
                    updateTreeFlag(treeName, true);
                }
            }
        });
    jQuery('form').submit(function () {
        CheckTree('#' + treeName + 'Tree');
        var Ids = jQuery('#' + treeName);
        //Ids.val('');
        var items = null;
        if (isSubLevel) {
            items = jQuery.tree.plugins.checkbox.get_checked($.tree.reference('#' + treeName + 'Tree')).filter(":not([parentSelect=true])");
        }
        else {
            items = jQuery.tree.plugins.checkbox.get_checked($.tree.reference('#' + treeName + 'Tree')).filter(".leaf"); //.filter(":not([parentSelect=true])");
        }
        items.each(function ()//.filter(".leaf").each(function ()
        {
            var checkItem = jQuery(this);
            var checkedId = this.id;
            if (checkItem.attr('Key') != '') {
                var tempIds = jQuery('#' + checkItem.attr('Key'));
                tempIds.val(tempIds.val() + '&' + checkedId);
            }
            else {
                if (checkItem.hasClass('leaf')) {
                    Ids.val(Ids.val() + '&' + checkedId);
                }
            }
        });
    });



};
function GetItemsAuto(items, nodes, treeName) {
    if (typeof (nodes) == "undefined")
        nodes = new Array();
    if (typeof (items) == "undefined")
        items = eval(treeName + 'Items');

    for (var i = 0; i < items.length; i++) {
        if ((typeof (items[i].children) != "undefined") && (items[i].children)) {
            GetItemsAuto(items[i].children, nodes, treeName);
            var obj = new Object();
            obj.label = items[i].data;
            obj.value = items[i].attributes.id + "#" + items[i].attributes["Key"];
            nodes[nodes.length] = obj;
        }
        else {
            var obj = new Object();
            obj.label = items[i].data;
            obj.value = items[i].attributes.id + "#" + items[i].attributes["Key"];
            nodes[nodes.length] = obj;
        }
    }
    return nodes;
};

function getTreeText(Name) {
    var returnItems = new Array();
    if (typeof (key) == 'undefined') {
        key = Name;
    }
    if (typeof (isSubLevel) == 'undefined') {
        isSubLevel = false;
    }
    if (jQuery('#' + Name + 'Tree').length != 0) {

        CheckTree('#' + Name + 'Tree');
        var treeObj = jQuery.tree.reference('#' + Name + 'Tree');
        if (treeObj) {
            var items = null;
            if (isSubLevel) {
                items = jQuery.tree.plugins.checkbox.get_checked(treeObj).filter(":not([parentSelect=true])");
            }
            else {
                items = jQuery.tree.plugins.checkbox.get_checked(treeObj).filter(".leaf"); //.filter(":not([parentSelect=true])");
            }

            items.each(function () {//.filter(":not([parentSelect=true])").each(function () {
                var checkItem = jQuery(this);
                var checkedId = this.id;
                var checkedText = checkItem.text();
                var newitem = new Object();
                newitem.id = checkedId;
                newitem.dispalValue = checkedText;
                returnItems[returnItems.length] = newitem;
            });
        }
    }
    return returnItems;
};

function initilizeFormValidationForDynamicContent(formname) {


    var $forms = $('#' + formname);
    $.each($forms, function (key, value) {
        // enable validation when an input loses focus.
        var validator = $.data(value, 'validator');
        if (validator) {
            return validator;
        }

        $(value).attr('novalidate', 'novalidate');
        validator = new $.validator({

        }, value);
        var settings = validator.settings;

        settings.onfocusout = function (element) { $(element).valid(); };
        settings.onfocusin = function (element) { $(element).valid(); };

        $.data(value, 'validator', validator);
    });
}
function getTreeData(Name, key, isSubLevel) {
    //
    if (typeof (key) == 'undefined') {
        key = Name;
    }
    if (typeof (isSubLevel) == 'undefined') {
        isSubLevel = false;
    }
    var Ids = "";

    if (jQuery('#' + Name + 'Tree').length != 0) {

        CheckTree('#' + Name + 'Tree');

        var treeObj = jQuery.tree.reference('#' + Name + 'Tree');
        if (treeObj) {
            var items = null;
            if (isSubLevel) {
                items = jQuery.tree.plugins.checkbox.get_checkedOrUndeterminded(treeObj).filter("[Key='" + key + "']");
            }
            else {
                items = jQuery.tree.plugins.checkbox.get_checked(treeObj).filter(".leaf"); //.filter(":not([parentSelect=true])");
            }

            items.each(function () {//.filter(":not([parentSelect=true])").each(function () {
                var checkItem = jQuery(this);
                var checkedId = this.id;
                if ((!checkItem.attr('Key')) || (checkItem.attr('Key') == key)) {
                    Ids += ',' + checkedId;
                }
            });

        }
    }

    return Ids;
};
function initTree(treeName, treeItems) {
    var flag = true;
    var treeElm = jQuery("#" + treeName + "Tree");
    try {
        var _treeitemjQuery = jQuery.tree.reference('#' + treeName + 'Tree');
        if (_treeitemjQuery != null) {
            _treeitemjQuery.destroy();
        }
        treeElm.html('');
    }
    catch (ex) {
    }
    if (flag) {
        treeElm.tree({
            ui: {
                theme_name: "checkbox"
            },
            data: {
                type: "json",
                opts:
                    {
                        static: treeItems
                    }
            },
            plugins: {
                checkbox: {

                }
            },
            callback: {
                onload: function (tree) {
                    //tree.open_all();
                    jQuery("#" + treeName + "Tree").find('li[selected=true]').each(function () {
                        jQuery.tree.plugins.checkbox.check(this);
                    });
                }
            }
        });
    }

    if (appSiteIdForAppReport != "")
        $("#AdsListTree").find("#" + appSiteIdForAppReport).children().attr("class", "clicked checked");

    if (CampaignIdForCampaignpReport != "")
        $("#AdsListTree").find("#" + CampaignIdForCampaignpReport).children().attr("class", "clicked checked");

    if (treeName == "advancedCriteria") {
        if ($("#tabId").val() == "Operator" || $("#tabId").val() == "DeviceModel" || $("#tabId").val() == "GeoLocation" || $("#tabId").val() == "AudianceSegmentForAdvertiser") {
            autoChangedForTreeAdvanceCriteria("loading");
        }
    }
};
function autoChanged(event, item, treeName) {
    var values = item.value.split("#");
    var id = values[0];
    var key = values[1];
    var match = jQuery("#" + treeName + "Tree").find('li[id=' + id + '][key="' + key + '"]');
    match.each(function () {
        jQuery.tree.plugins.checkbox.check(this);
        if (jQuery(this).hasClass("closed")) {

            jQuery(this).removeClass("closed");

            jQuery(this).addClass("open");
        }
    });
    return false;
}
function initAutoComplete(treeName, items) {
    var obj = jQuery("#" + treeName + "treeSearch");
    var data_source = GetItemsAuto(items);
    var auto = obj.autocomplete(
        {
            //minLength: 0,
            delay: 0,
            source: data_source,
            open: function () {
                /* 
                   jQuery(this).autocomplete("widget").css("width", 400).css("position", "absolute"); //.find('li').css("width", 400);*/
            },
            focus: function (event, ui) {
                $("#" + treeName + "treeSearch").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                jQuery("#" + treeName + "treeSearch").val(ui.item.label);
                if (ui.item != null) {
                    autoChanged(event, ui.item, treeName);
                }
                return false;
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            if (falg) {
                falg = false;
                var returnval = jQuery("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + item.label + "</a>");
                return returnval.appendTo(ul);
            }
            else {
                falg = true;
                return jQuery("<li class='alter-item'></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + item.label + "</a>")
                    .appendTo(ul);
            }
        };

};

function CheckTree(id) {
    var tree = jQuery(id);
    tree.find("a").each(function () {
        jQuery(this).parent().attr('parentSelect', false);
    });
    checkchildsStatus(tree);
    tree.find("[isroot=true][parentselect=true]").attr('parentSelect', false);
}
function checkchildsStatus(treeNode) {
    var items = treeNode.children("ul");
    if (items.length == 0) {
        items = treeNode.children("li").children("ul");
    }
    items.each(function () {
        var item = jQuery(this);
        var itemChilds = item.children();

        if (item.find("a:not(.checked):eq(0)").length == 0) {
            //all Childs are selected
            itemChilds.find("a").each(function () {
                jQuery(this).parent().attr('parentSelect', true);
                //jQuery(this).attr('parentSelect', true);
            });
        }
        checkchildsStatus(item);
    });
}
/////////////////////////////Tree End /////////////////////////////////
/////////////////////////////Campaign Start ///////////////////////////
function calculate_discounted_value(bidValue, selector, updateMinBid) {
    var discounted_bid = bidValue;
    if ((has_discount) && (discount_value > 0)) {
        switch (discount_type) {
            case 1://Percentage
                {
                    discounted_bid = bidValue * (1.0 - discount_value);
                    break;
                }
            case 2://Fixed
                {
                    discounted_bid = bidValue - discount_value > 0 ? bidValue - discount_value : 0;
                    break;
                }
            default:
        }
        $(selector).text('(' + $.validator.format(discountedBidDesc, discounted_bid.toFixed(2)) + ')');
    } else {
        $(selector).text('');
    }
    if (hasValue(updateMinBid) && updateMinBid === true) {
        $('#minBid').text(discounted_bid.toFixed(2));
    }
    return discounted_bid;
}
/////////////////////////////Campaign End /////////////////////////////


/******************* DashBoard Page *********************************/
var sp;
var CurrentMonthRevenue;
var LastMonthRevenue;
function getChartImage() {
    // 
    var Id = $("#list").val();
    var period = $("#period").val();
    var metric = $("input[name=metric]:checked").val();

    if (sp == undefined) {
        sp = getSpinner("chartContainer");
    }

    sp.showSpinner();


    $.ajax({
        type: 'POST',
        data: {
            periodOption: period,
            metricCode: metric,
            Id: Id
        },
        url: chartControlImageSrc,
        success: function (data) {
            $('#chartImage').attr("src", "data:image/png;base64," + data);
            sp.hideSpinner();
        }
    });


}

function getGoogleChartImage() {


    var subId = '';
	var secondsubId = '';
	var AdvertiserAccountId = '';
    if ($("#campaigns").val() != '') {
        subId = $("#campaigns").val();
	}

	if ($("#advertisers").val() != '') {
		AdvertiserAccountId = $("#advertisers").val();
	}

    if ($("#adgruops").val() != '') {
        secondsubId = $("#adgruops").val();

    }
    var Id = $("#list").val();
    var period = $("#period").val();
    var metric = $("input[name=metric][checked]").val();
    var metricName = $("input[name=metric][checked]").attr("customtext");

    if ((typeof metric === 'undefined')) {


        return;

    }
    if ((typeof google === 'undefined') || (typeof google.visualization === 'undefined')) {
        return;
    }

    if (typeof google.visualization.DataTable === 'undefined') {
        return;
    }

    var chartControlImageSrcTemp = '';
    if ($("#AdvertiserAccountId").length > 0 && $("#AdvertiserAccountId").val() != '' && $("#AdvertiserAccountId").val() != '0') {
        chartControlImageSrcTemp = chartControlImageSrc + "&id=" + $("#AdvertiserAccountId").val();

	}

    else {

        chartControlImageSrcTemp = chartControlImageSrc;
	}



	var CompanyName = 0;
	if ($("#agenListDll").length > 0 && $("#agenListDll").val() != '') {
		CompanyName = $("#agenListDll").val();

	}
	var CampaignName = 0;
	if ($("#CampaignListDllDP").length > 0 && $("#CampaignListDllDP").val() != '') {
		CampaignName = $("#CampaignListDllDP").val();

	}
	var AdvertiserId = 0;
	if ($("#AdvListDllDP").length > 0 && $("#AdvListDllDP").val() != '') {
		AdvertiserId = $("#AdvListDllDP").val();

	}
	if (AdvertiserAccountId && AdvertiserAccountId.length > 0 && AdvertiserAccountId != '' && AdvertiserAccountId != '0') {


		AdvertiserId = AdvertiserAccountId;
	}
    var formatString = 'short';
    var data = new google.visualization.DataTable();

    if ((typeof DashBoardChartGoogle === 'undefined')) {


        DashBoardChartGoogle = new google.visualization.LineChart(document.getElementById('gchartContainer'));

    }
    DashBoardChartGoogle.clearChart();
    //$("#gchartContainer")[0].hide();

    //DashBoardChartGoogle.destroy();
    if (sp == undefined) {
        sp = getSpinner("gchartContainer");

    }

    sp.showSpinner();
    //
    var defaultHeight = "70%";

    if (period == "1" || period == "0") {
        defaultHeight = "80%";

    }

    $(function () {
        $.ajax({
            type: 'POST',
            data: {
                periodOption: period,
                metricCode: metric,
                Id: Id,
                subId: subId,
				secondsubId: secondsubId,
					CompanyName: CompanyName,
				CampName: CampaignName,

				AdvertiserId: AdvertiserId
            },
            url: chartControlImageSrcTemp,
            success: function (chartsdata) {
                if (typeof (chartsdata.OptionalParameter) != "undefined" && chartsdata.OptionalParameter != "") {
                    if (typeof ($("#TodaySpend")) != "undefined") {
                        $("#TodaySpend").text(chartsdata.OptionalParameter);
                    }
                }

                if (period == "3") {
                    $("#MonthRevenue").text(CurrentMonthRevenue);
                }
                else if (period == "4") {
                    $("#MonthRevenue").text(LastMonthRevenue);
				}
				else if (period == "5") {
					$("#MonthRevenue").text(Last3MonthRevenue);
				}
				else if (period == "6") {
					$("#MonthRevenue").text(Last6MonthRevenue);
				}
                // Callback that creates and populates a data table,    
                // instantiates the pie chart, passes in the data and    
                // draws it.    
                var hxticks = [];
                data.addColumn('number', 'period');
                data.addColumn('number', metricName);

                for (var i = 0; i < chartsdata.ChartDtoList.length; i++) {

                    if (chartsdata.ChartDtoList[i].Yaxis == -1) {

                        data.addRow([{ v: i, f: chartsdata.ChartDtoList[i].XaxisString }, null]);
                    }
                    else {

                        data.addRow([{ v: i, f: chartsdata.ChartDtoList[i].XaxisString }, chartsdata.ChartDtoList[i].Yaxis]);
                    }
                    // if ((i + 2) % 2 == 0) {
                    hxticks.push({
                        v: i, f: chartsdata.ChartDtoList[i].XaxisString
                    });
                    //}
                    if (chartsdata.ChartDtoList[i].Yaxis != -1 & chartsdata.ChartDtoList[i].Yaxis < 0.01 & chartsdata.ChartDtoList[i].Yaxis != 0 & chartsdata.ChartDtoList[i].Yaxis != null) {
                        formatString = 'decimal';
                    }

                }


                // Instantiate and draw our chart, passing in some options    
                //var chart = new google.visualization.PieChart(document.getElementById('gchartContainer'));
                var options = {
                    legend: 'none',
                    chartArea: {
                        left: 70, top: 20, width: '88%', height: defaultHeight
                    },
                    colors: [chartsdata.Color],
                    curveType: 'function',
                    pointSize: 5,
                    hAxis: {
                        title: chartsdata.HAxisText,
                        slantedText: chartsdata.slantedText,
                        // ticks: [{ v: 0, f: '23' }, { v: 1, f: '01' }, { v: 2, f: '10' }, { v: 3, f: '20' }],
                        viewWindow: {
                            min: 0
                        },
                        showTextEvery: 2,
                        slantedTextAngle: chartsdata.slantedTextAngle,
                        ticks: hxticks,
                        minTextSpacing: 0,
                        maxTextLines: 1,
                        textStyle: {

                            fontName: 'Verdana',
                            fontSize: 7,
                            bold: true,
                            italic: false
                        },
                        titleTextStyle: {
                            fontName: 'Verdana',
                            fontSize: 10,
                            bold: true,
                        },
                    },

                    backgroundColor: '#f3f3f7',

                    width: '100%', height: 350,
                    tite: "sfsdf",
                    reverseCategories: chartsdata.isRightToLeft,
                    vAxis: {
                        title: metricName,
                        logscale: false,
                        viewWindow: {
                            min: 0
                        },
                        format: formatString,
                        textStyle: {
                            color: 'Red',
                            fontName: 'Verdana',
                            fontSize: 7,
                            bold: true,

                            italic: false
                        },
                        titleTextStyle: {
                            fontName: 'Verdana',
                            fontSize: 10,
                            bold: true,
                        }
                    }

                };


                DashBoardChartGoogle.draw(data, options);

                sp.hideSpinner();
                sp = null;
            },
            error: function () {
                alert("Error loading data! Please try again.");
            }
        });
    })
}

function fillGrid() {

    var grid = $("#GeoLocationGrid").data("tGrid");

    grid.rebind();
}



function geoLocationDataBinding(args) {
    args.data = $.extend(args.data,
        {
            country: $('#country').val(),
            list: $('#list').val(),
            period: $('#period').val(),
            AdvertiserId: $("#AdvertiserId").val(),
            AdvertiserAccountId: $("#AdvertiserAccountId").val()
        });
}




function geoLocationExport(type) {

    var grid = $("#GeoLocationGrid").data("tGrid");

    //var country = $('#country').val();
    //var list = $('#list').val();
    //var period = $('#period').val();

    //window.location.href = exportUrl + "?type=" + type + "&country=" + country + "&list=" + list + "&period=" + period + "&orderby=" + grid.orderBy;


    var geoLocationParams = {
        country: $('#country').val(),
        list: $('#list').val(),
        period: $('#period').val(),
        orderby: grid.orderBy,
        type: type,
        AdvertiserId: $("#AdvertiserId").val(),
        AdvertiserAccountId: $("#AdvertiserAccountId").val()
    };

    downlaodByPostAction(exportUrl, geoLocationParams);
}

function performanceExport(type) {
    var grid = $("#Grid").data("tGrid");

    var performanceParams = {
        type: type,
        orderby: grid.orderBy
    };

    downlaodByPostAction(performanceExportUrl, performanceParams);

    //window.location.href = performanceExportUrl + "?type=" + type + "&orderby=" + grid.orderBy;
}

/************** End Dashboard Page **********************************/

/************* Start Report Page ***********************************/

var adsListTreeURL;
function getFilters(tabId, filterUrl, advancedFilterUrl, customLabel, showGroubNameFlag, groupByNameLabel) {
    $("[id^=tab]").removeClass("selected");
    $("#tab" + tabId).addClass("selected");
    $("#tabId").val(tabId);

    if (typeof (showGroupByName) != "undefined") {
        showGroupByName = showGroubNameFlag;
        showGrouByNameOption(null, $("input[name='layout']:checked").val(), groupByNameLabel);
    }

    if (advancedFilterUrl != null && advancedFilterUrl != "") {

        InitTree("advancedCriteria", advancedFilterUrl);
        $("#advancedCriteriaContainer").show();
    }
    else {
        $("#advancedCriteriaContainer").hide();
    }

    if (tabId == "DeviceModel") {
        $("#deviceTabs").show();
    }
    else {
        $("#deviceTabs").hide();
    }
    if (tabId == "SubAppSite") {
        $("#advancedCriteriaContainer").hide();

        var UniqueImpId = $("[customvalue='UniqueImp']").attr("id");
        var UniqueClicksId = $("[customvalue='UniqueClicks']").attr("id");
        uncheck(UniqueImpId);
        uncheck(UniqueClicksId);
        $("#Div_UniqueClicks").hide();
        $("#Div_UniqueImp").hide();
        ReportSchdulingmetricColumn = true;

    } else {

        var UniqueImpId = $("[customvalue='UniqueImp']").attr("id");
        var UniqueClicksId = $("[customvalue='UniqueClicks']").attr("id");
        check(UniqueImpId);
        check(UniqueClicksId);
        $("#Div_UniqueClicks").show();
        $("#Div_UniqueImp").show();
        ReportSchdulingmetricColumn = true;

    }
    if (tabId == "AudianceSegmentForAdvertiser") {

        var RequestsByTypeId = $("[customvalue='RequestsByType']").attr("id");
        var WonImpressionsId = $("[customvalue='WonImpressions']").attr("id");
        var DisplayRateId = $("[customvalue='DisplayRate']").attr("id");
        var WinRateId = $("[customvalue='WinRate']").attr("id");
        var DataProviderId = $("[customvalue='DataProvider']").attr("id");

        uncheck(RequestsByTypeId);
        uncheck(WonImpressionsId);
        uncheck(DisplayRateId);
        uncheck(WinRateId);
        check(DataProviderId);

        $("#Div_RequestsByType").hide();
        $("#Div_WonImpressions").hide();
        $("#Div_DisplayRate").hide();
        $("#Div_WinRate").hide();
        $("#Div_DataProvider").show();

        $("#Div_RequestsByType").parent().parent().hide();

    } else {

        var RequestsByTypeId = $("[customvalue='RequestsByType']").attr("id");
        var WonImpressionsId = $("[customvalue='WonImpressions']").attr("id");
        var DisplayRateId = $("[customvalue='DisplayRate']").attr("id");
        var WinRateId = $("[customvalue='WinRate']").attr("id");
        var DataProviderId = $("[customvalue='DataProvider']").attr("id");

        uncheck(RequestsByTypeId);
        uncheck(WonImpressionsId);
        check(DisplayRateId);
        check(WinRateId);
        uncheck(DataProviderId);

        $("#Div_RequestsByType").show();
        $("#Div_WonImpressions").show();
        $("#Div_DisplayRate").show();
        $("#Div_WinRate").show();
        $("#Div_DataProvider").hide();

        $("#Div_RequestsByType").parent().parent().show();


    }


    if ($("#AdvertiserAccountId") != null && $("#AdvertiserAccountId").length > 0) {

        var advid = ''
        if (tabId == 'campaign' || tabId.toLowerCase() == 'adgroup' || tabId.toLowerCase == 'ad' || tabId.toLowerCase == 'operator' || tabId.toLowerCase == 'geolocation' || tabId.toLowerCase == 'audiancesegmentforadvertiser' || tabId.toLowerCase == 'subappsite' || tabId.toLowerCase == 'devicemodel') {
            if ($("#AdvertiserAccountId").val() != null && $("#AccountAdvertiserId").val() != '') {

                advid = "&AdvertiserId=" + $("#AdvertiserAccountId").val();
            }
        }
        InitTree("AdsList", filterUrl + advid);

        adsListTreeURL = filterUrl;
        if (tabId == 'campaign' || tabId.toLowerCase() == 'adgroup' || tabId.toLowerCase == 'ad')
            $(".specificName").find(".check-box-text").html(customLabel);
        else
            $(".specificName").find(".check-box-text").html(MainLabel);
    }
	else {
		if (tabId.toLowerCase()  == 'campaign' || tabId.toLowerCase() == 'app' )
			InitTree("AdsList", MainFilterUrl);
		else
			InitTree("AdsList", filterUrl);
        $(".specificName").find(".check-box-text").html(MainLabel);
    }

    resetReport();
    showHideColmns();

}

function generateReport() {

    $("#reportForm").validate();
    if ($("#reportForm").validate().form()) {
        resetReport();
        showReportSpinner();
        generateChartImage();
        generateGrid();
    }
    else {
        clearErrorMessage();
        showErrorMessage(datesErrorMessage);
    }

    return false;
}
function ChangedColumnsMetric() {
    ReportSchdulingmetricColumn = true;

}
function UpdateGridLayout() {

    if (ReportSchdulingmetricColumn) {
        //showHideColmns();
	
        $.ajax(
            {
                url: reportRefrshGridURL + "?ColumnIds=" + getCheckmetriceColumn(),

                cache: false,
                type: 'POST',


                success: function (html) {

                    html = html.toString();
                    $("#GridLayoutDetails").html('');
                    var newelem = jQuery(html);
                    $("#GridLayoutDetails").append(newelem);
                    parseScript(html);
                    initilize();
					ReportSchdulingmetricColumn = false;
                    showReportSpinner();
                    generateGoogleChartImage();
                    //generateGrid();
				

                }
            });
        
    }
    else {

        showReportSpinner();
        generateGoogleChartImage();
        generateGrid();

	}

	return false;
}
function generateGoogleReport() {

    $("#ReportFormDetails").validate();
    var uncheckColumns = parseInt($("#metriceColumnsDataDiv").find(".check-box-uncheck").length);
    getCheckmetriceColumn();
    if (!(SelectableColumns - HiddenSelectableColumns >= 1)) {
        $("#ColumnsWarnMessages").show();
    } else {
        $("#ColumnsWarnMessages").hide();

        if ($("#ReportFormDetails").validate()) {
            //resetReport();
            UpdateGridLayout();

        }
        else {
            clearErrorMessage();
            showErrorMessage(datesErrorMessage);
        }
    }

    return false;
}
function resetReport() {

    $("#reportViewArea").hide();
    //

    // $.extend(true, $("#ReportGrid").tGrid, $.telerik.grid);

    //if (typeof($("#ReportGrid").data("tGrid") !='undefined' ))
    //{
    //TelerikgridInitilizeForMigration($("#ReportGrid"));


    //jQuery('#ReportGrid').tGrid({columns:[{"title":"Date Range","member":"DateRange","type":"String"},{"title":"Name","member":"Name","type":"String"},{"title":"Campaign Name","member":"SubName","type":"String"},{"title":"Impressions","member":"Impress","type":"Number"},{"title":"Clicks","member":"Clicks","type":"Number"},{"title":"CTR","member":"CtrText","type":"String"},{"title":"Avg.CPC","member":"AvgCPCText","type":"String"},{"title":"Spend","member":"SpendText","type":"String"}], total:0, currentPage:1, pageSizesInDropDown:["5","10","20","50"], pageOnScroll:false, ajax:{"selectUrl":"/Noqoush.AdFalcon.Administration.Web/en/reports/CampaignReport?CampaignId=\u0026058a8f9c-e9a4-445a-a093-0f2ed7313f78=System.Web.Mvc.DictionaryValueProvider%601%5BSystem.Object%5D\u0026ReportGrid-size=10\u0026reportType=ad"}, onDataBinding:reportDataBinding, onDataBound:gridReportSuccess, onError:gridReportError, noRecordsTemplate:'No records to display.'});
    //jQuery('#Metrics').tGrid({columns:[{"title":"Name","member":"Name","type":"Object"},{"title":"","member":"","type":"Object"}], urlFormat:'/Noqoush.AdFalcon.Administration.Web/en/reports/gcampaignchart?CampaignId=&058a8f9c-e9a4-445a-a093-0f2ed7313f78=System.Web.Mvc.DictionaryValueProvider%601%5BSystem.Object%5D&reportType=ad', pageSize:0, noRecordsTemplate:'No records to display.'});
    $("#ReportGrid").data("tGrid").showColumn(1);
    $("#ReportGrid").data("tGrid").showColumn(2);
    $("#ReportGrid").data("tGrid").showColumn(0);

}

function generateGoogleChartImage() {

    var metricName = $("input[name=metric]:checked").attr("customtext");
    if ((typeof google === 'undefined') || (typeof google.visualization === 'undefined')) {
        return;
    }
    if (typeof google.visualization.DataTable === 'undefined') {
        return;
    }
    if ((typeof ReportChartGoogle === 'undefined')) {
        if ((typeof google.visualization.LineChart === 'undefined')) {
            return;
        }
        else {

            ReportChartGoogle = new google.visualization.LineChart(document.getElementById('gchartImage'));
        }
    }
    ReportChartGoogle.clearChart();
    var data = new google.visualization.DataTable();
    if (sp == undefined) {
        sp = getSpinner("gchartImage");

    }
    var formatString = 'short';
    sp.showSpinner();
    $.ajax({
        type: 'POST',
        data: {
            tabId: $('#tabId').val(),
            fromDate: $('[name=FromDate]').val(),
            toDate: $('[name=ToDate]').val(),
            criteriaOpt: $('input[name=criteriaOpt]:checked').val(),
            AdsList: getTreeData("AdsList", "Ads"),
            advancedCriteria: getTreeData("advancedCriteria"),
            metricCode: $("input[name=metric]:checked").val(),
            deviceCategory: $("#deviceCategory").val(),
            groupByName: ($("#GroupByName:checked").length > 0 ? true : false),
            AdvertiserId: $('#AdvertiserId').val(),
            AccountAdvertiserId: $('#AdvertiserAccountId').val()
        },
        url: chartImageUrl,
        success: function (chartsdata) {
            // $('#chartImage').attr("src", "data:image/png;base64," + data);

            if (chartsdata) {

                var hxticks = [];
                data.addColumn('number', 'period');
                data.addColumn('number', metricName);
                //
                for (var i = 0; i < chartsdata.ChartDtoList.length; i++) {

                    if (chartsdata.ChartDtoList[i].Yaxis == -1) {



                        //data.addRow([{ v: i, f: chartsdata.ChartDtoList[i].XaxisString }, null]);
                        data.addRow([{ v: i, f: chartsdata.ChartDtoList[i].XaxisString }, null]);
                    }
                    else {

                        data.addRow([{ v: i, f: chartsdata.ChartDtoList[i].XaxisString }, chartsdata.ChartDtoList[i].Yaxis]);
                    }

                    //if ((i + 2) % 2 == 0) {
                    hxticks.push({
                        v: i, f: chartsdata.ChartDtoList[i].XaxisString
                    });
                    //}
                    if (chartsdata.ChartDtoList[i].Yaxis != -1 & chartsdata.ChartDtoList[i].Yaxis < 0.01 & chartsdata.ChartDtoList[i].Yaxis != 0 & chartsdata.ChartDtoList[i].Yaxis != null) {

                        formatString = 'decimal';
                    }
                }


                var options = {
                    legend: 'none',
                    chartArea: {
                        left: 70, top: 20, width: '88%', height: '70%'
                    },
                    colors: [chartsdata.Color],
                    minTextSpacing: 0,
                    maxTextLines: 1,
                    curveType: 'function',
                    pointSize: 5,
                    hAxis: {
                        title: chartsdata.HAxisText,
                        slantedText: chartsdata.slantedText,
                        // ticks: [{ v: 0, f: '23' }, { v: 1, f: '01' }, { v: 2, f: '10' }, { v: 3, f: '20' }],
                        viewWindow: {
                            min: 0
                        },
                        showTextEvery: 2,
                        slantedTextAngle: chartsdata.slantedTextAngle,
                        ticks: hxticks,
                        textStyle: {

                            fontName: 'Verdana',
                            fontSize: 7,
                            bold: true,
                            italic: false
                        },
                        titleTextStyle: {
                            fontName: 'Verdana',
                            fontSize: 10,
                            bold: true,
                        },
                    },

                    backgroundColor: '#f3f3f7',

                    width: '100%', height: 300,
                    reverseCategories: chartsdata.isRightToLeft,
                    vAxis: {
                        title: metricName,
                        logscale: true,
                        viewWindow: {
                            min: 0
                        },
                        format: formatString,
                        textStyle: {
                            color: 'Red',
                            fontName: 'Verdana',
                            fontSize: 7,
                            bold: true,
                            italic: false
                        },
                        titleTextStyle: {
                            fontName: 'Verdana',
                            fontSize: 10,
                            bold: true,
                        }
                    }

                };


                ReportChartGoogle.draw(data, options);
                sp.hideSpinner();
                sp = null;
            }
        }
    });

    //$("#chartImage").attr("src", chartImageUrl + "?tabId=" + $('#tabId').val() + "&fromDate=" + $('[name=FromDate]').val() + "&toDate=" + $('[name=ToDate]').val() +
    //                     "&criteriaOpt=" + $('input[name=criteriaOpt]:checked').val()
    //                        + "&AdsList=" + getTreeData("AdsList") + "&advancedCriteria=" + getTreeData("advancedCriteria") + "&metricCode=" + $("input[name=metric]:checked").val() + "&deviceCategory=" + $("#deviceCategory").val() + "&groupByName=" + ($("#GroupByName:checked").length > 0 ? true : false));
}
var CampaignReportSchedulinRecurrenceType;
var ReportSectionType;
var ReportSchdulingmetricColumn = true;
function ReportdataCollector() {

    //  
    var date_now = new Date();
    var startTime = Get_time_from_control('StartTime');


    if (!hasValue(startTime)) {
        startTime = new Date(date_now.getFullYear(), date_now.getMonth(), date_now.getDate(), 0, 0, 0);
    }

    var dateFull = new Date(Date.UTC(startTime.getFullYear(), startTime.getMonth(), startTime.getDate(), startTime.getHours(), startTime.getMinutes(), 0));
    var CampaignReportSchedulingViewModel = new Object();
    // ReportCriteriaDto
    CampaignReportSchedulingViewModel.AccountAdvertiserId = $('#AdvertiserAccountId').val();
    CampaignReportSchedulingViewModel.AdvertiserId = $('#AdvertiserId').val();
    CampaignReportSchedulingViewModel.TabId = $('#tabId').val();
    CampaignReportSchedulingViewModel.FromDate = $("#FromDate").val();
    CampaignReportSchedulingViewModel.ToDate = $("#ToDate").val();
    CampaignReportSchedulingViewModel.Layout = $("input[name=layout][checked]").val();
    CampaignReportSchedulingViewModel.SummaryBy = $("#SummaryBy").val();
    CampaignReportSchedulingViewModel.GroupByName = $("#GroupByName").is(":checked");

    CampaignReportSchedulingViewModel.IsActive = $("#IsActive").is(":checked");
    CampaignReportSchedulingViewModel.PreferedName = $("#ReportSchedulerDto_PreferedName").val();
    CampaignReportSchedulingViewModel.CriteriaOpt = $('input[name=criteriaOpt][checked]').val();
    CampaignReportSchedulingViewModel.ItemsList = getTreeData("AdsList", "Ads");
    CampaignReportSchedulingViewModel.AdvancedCriteria = getTreeData("advancedCriteria");
    CampaignReportSchedulingViewModel.DeviceCategory = $("#deviceCategory").val();
    CampaignReportSchedulingViewModel.CampaignType = 1;

    //ReportSchedulerDtok
    CampaignReportSchedulingViewModel.AllReportRecipient = SendEmails();
    CampaignReportSchedulingViewModel.Name = $("#ReportTempName").val();
    CampaignReportSchedulingViewModel.EmailSubject = $("#ReportSchedulerDto_EmailSubject").val();
    CampaignReportSchedulingViewModel.EmailIntroduction = $("#ReportSchedulerDto_EmailIntroduction").val();
    CampaignReportSchedulingViewModel.SchedulingEndtDate = $("#SchedulingEndDate").val();
    CampaignReportSchedulingViewModel.SchedulingStartDate = $("#SchedulingStartDate").val();
    CampaignReportSchedulingViewModel.TimeSentAt = dateFull.toUTCString();
    CampaignReportSchedulingViewModel.RecurrenceType = CampaignReportSchedulinRecurrenceType;
    CampaignReportSchedulingViewModel.ReportSectionType = ReportSectionType;
    CampaignReportSchedulingViewModel.DateRecurrenceType = $("#DateRecurrenceType").val();
    CampaignReportSchedulingViewModel.WeekDay = $("#Weeks").val();
    CampaignReportSchedulingViewModel.MonthDay = $("#Months").val();
    CampaignReportSchedulingViewModel.ID = $("#ReportSchedulerHId").val();
    CampaignReportSchedulingViewModel.IsSunday = $("#IsSunday").is(":checked");
    CampaignReportSchedulingViewModel.IsMonday = $("#IsMonday").is(":checked");
    CampaignReportSchedulingViewModel.IsTuesday = $("#IsTuesday").is(":checked");
    CampaignReportSchedulingViewModel.IsWednesday = $("#IsWednesday").is(":checked");
    CampaignReportSchedulingViewModel.IsThursday = $("#IsThursday").is(":checked");
    CampaignReportSchedulingViewModel.IsFriday = $("#IsFriday").is(":checked");
    CampaignReportSchedulingViewModel.IsSaturday = $("#IsSaturday").is(":checked");
    CampaignReportSchedulingViewModel.metriceColumns = getCheckmetriceColumn();

    return CampaignReportSchedulingViewModel;
}

function getCheckmetriceColumn() {

    var things = "";

    var Checks = $('*[id^="CheckmetriceColumn"]');
    SelectableColumns = 0;

    for (var i = 0; i < Checks.length; i++) {
        var id = $(Checks.eq(i)).attr("id");
        if (typeof (Checks.eq(i)) != "undefined" && Radio_IsChecked("#" + id)) {
            SelectableColumns++;
            id = id.replace("CheckmetriceColumn", "");
            things = things + "," + parseInt(id);
        }
    }


    return things;
}
function generateChartImage() {

    $.ajax({
        type: 'POST',
        data: {
            tabId: $('#tabId').val(),
            fromDate: $('[name=FromDate]').val(),
            toDate: $('[name=ToDate]').val(),
            criteriaOpt: $('input[name=criteriaOpt]:checked').val(),
            AdsList: getTreeData("AdsList", "Ads"),
            advancedCriteria: getTreeData("advancedCriteria"),
            metricCode: $("input[name=metric]:checked").val(),
            deviceCategory: $("#deviceCategory").val(),
            groupByName: ($("#GroupByName:checked").length > 0 ? true : false),
            AdvertiserId: $('#AdvertiserId').val(),
            AccountAdvertiserId: $('#AdvertiserAccountId').val()
        },
        url: chartImageUrl,
        success: function (data) {
            $('#chartImage').attr("src", "data:image/png;base64," + data);
        }
    });

    //$("#chartImage").attr("src", chartImageUrl + "?tabId=" + $('#tabId').val() + "&fromDate=" + $('[name=FromDate]').val() + "&toDate=" + $('[name=ToDate]').val() +
    //                     "&criteriaOpt=" + $('input[name=criteriaOpt]:checked').val()
    //                        + "&AdsList=" + getTreeData("AdsList") + "&advancedCriteria=" + getTreeData("advancedCriteria") + "&metricCode=" + $("input[name=metric]:checked").val() + "&deviceCategory=" + $("#deviceCategory").val() + "&groupByName=" + ($("#GroupByName:checked").length > 0 ? true : false));
}

function generateGrid() {

    var grid = $("#ReportGrid").data("tGrid");
    grid.rebind();

}

var gridFlag = 0;
function reportDataBinding(args) {

    if (gridFlag == 0) {
        args.preventDefault();
        gridFlag = 1;
        return;
    }
    args.data =
        {
            fromDate: $('[name=FromDate]').val(),
            toDate: $('[name=ToDate]').val(),
            summaryBy: $('#SummaryBy').val(),
            layout: $("input[name=layout][checked]").val(),
            criteriaOpt: $('input[name=criteriaOpt][checked]').val(),
            AdsList: getTreeData("AdsList", "Ads"),
            advancedCriteria: getTreeData("advancedCriteria"),
            tabId: $('#tabId').val(),
            deviceCategory: $("#deviceCategory").val(),
            groupByName: $("#GroupByName:checked").length > 0 ? true : false,
            AdvertiserId: $('#AdvertiserId').val(),
            AccountAdvertiserId: $('#AdvertiserAccountId').val()

        };


}

function gridReportError(e) {
    if (e.XMLHttpRequest.status == "500") {
        clearErrorMessage();
        showErrorMessage(e.XMLHttpRequest.responseText);
        e.preventDefault();
        $("#reportViewArea").hide();
        $("[id^=spinner]").hide();
    }
}

function gridReportSuccess() {

    var grid = $("#ReportGrid").data("tGrid");

    $("#ReportGrid").find(".t-last").removeClass("t-last");
    $("#reportViewArea").show();
    $("[id^=spinner]").hide();
}

function reportExport(exportType) {

    var grid = $("#ReportGrid").data("tGrid");

    var reportParams = {
        exportType: exportType,
        tabId: $('#tabId').val(),
        fromDate: $('[name=FromDate]').val(),
        toDate: $('[name=ToDate]').val(),
        summaryBy: $('#SummaryBy').val(),
        layout: $("input[name=layout][checked]").val(),
        criteriaOpt: $('input[name=criteriaOpt]:checked').val(),
        AdsList: getTreeData("AdsList", "Ads"),
        advancedCriteria: getTreeData("advancedCriteria"),
        deviceCategory: $("#deviceCategory").val(),
        orderby: grid.orderBy,
        groupByName: ($("#GroupByName:checked").length > 0 ? true : false),
        metriceColumns: getCheckmetriceColumn(),

        AdvertiserId: $('#AdvertiserId').val(),
        AccountAdvertiserId: $('#AdvertiserAccountId').val()


    };

    downlaodByPostAction(reportExportUrl, reportParams);
}

function TransactionVATExport(exportType) {

    var reportParams = {
        exportType: exportType,
        fromDate: $('[name=FromDate]').val(),
        toDate: $('[name=ToDate]').val(),
        AccountId: $("#AccountId").val(),
        Details: $("#Details").val(),
        FilterType: $("#FilterType").val()
    };

    downlaodByPostAction(reportExportUrl, reportParams);
}

function downloadImpLog(element) {
    var Id = $(element).parent().parent().find("#RecordId").text();
    var day = $(element).parent().parent().find("#LogDay").text();
    var ProviderName = $(element).parent().parent().find("#ProviderName").text();
    var reportParams = {
        Id: Id,
        name: day + "_" + ProviderName
    };

    downlaodByPostAction(downloadUrl, reportParams);
}

function changeReportView(area) {
    $("[id^=area]").hide();
    $("#" + area).show();
    $("[id^=subTabarea]").removeClass("selected");
    $("#subTab" + area).addClass("selected");

}

function changeAdvancedCriteria(item, value) {
    if (value == "all")
        $("#advancedCriteria").hide(); else $("#advancedCriteria").show();

}

function showGrouByNameOption(item, value, groupByNameLabel) {
    if (value == "detailed" && showGroupByName == "true") {
        $("#divGrouByName").show();
        // $("#divIsAccumulated").hide();
        if (groupByNameLabel != null && typeof (groupByNameLabel) != "undefined") {
            $("#groupbyNameLabel").html(groupByNameLabelTemplate.replace("{Name}", groupByNameLabel));
        }
        if ($("#IsAccumulated:checked").length > 0) {
            checkBox($("#IsAccumulated").closest("div")[0]);
        }
    }
    else {
        $("#divGrouByName").hide();
        //$("#divIsAccumulated").show();
        if ($("#GroupByName:checked").length > 0) {
            checkBox($("#GroupByName").closest("div")[0]);
        }
    }
    showHideColmns();
}

function showGrouByNameOptionForApp(item, value, groupByNameLabel) {

    if (value == "detailed") {

        // $("#divIsAccumulated").hide();

        if ($("#IsAccumulated:checked").length > 0) {
            checkBox($("#IsAccumulated").closest("div")[0]);
        }
    }
    else {

        // $("#divIsAccumulated").show();

    }
    showHideColmns();

}
function changeDeviceTab(e, treeUrl, value) {
    $("[id^=deviceTab]").removeClass("selected");
    $(e).addClass("selected");
    $("#deviceCategory").val(value);
    InitTree("advancedCriteria", treeUrl);
}

function showReportSpinner() {

    if ($("#criteriaContainer").find(".spinner-container").length < 1) {
        $(".spinner-container").clone().appendTo($("#criteriaContainer"));
        $("#criteriaContainer").css({
            position: "relative"
        });
        $("#criteriaContainer .spinner-container").attr("id", "spinnerCriteria");
    }
    if ($("#advancedCriteriaContainer").length > 0 && $("#advancedCriteriaContainer").find(".spinner-container").length < 1) {
        $($(".spinner-container")[0]).clone().appendTo($($("#advancedCriteriaContainer .section-form-container")[0]));
        $("#advancedCriteriaContainer .section-form-container").css({
            position: "relative"
        });
        $("#advancedCriteriaContainer .spinner-container").attr("id", "spinnerAdvancedCriteria");
    }
    $("[id^=spinner]").show();
    $("[id^=spinner] img").css({
        marginTop: "17%", marginRight: "45%"
    });


}

/*********** Ebd Report Page **************************************/
/************** Start Tab **********************************/
function changeHistoryTab(tab) {
    var tabLink = jQuery(tab);
    var tabindex = tabLink.attr('tabindex');
    $('[id^=aTab]').removeClass("selected");
    $("#aTab" + tabindex).addClass("selected");
    $("[id^=tabContainer]").hide();
    $("#tabContainer" + tabindex).show();

}


function changeTab(tab, suffix, hfId, extraInfo, isAll) {
    var isSubTab = true;
    if (typeof (suffix) == "undefined") {
        suffix = '';
        isSubTab = false;
    }
    var tabLink = jQuery(tab);
    var tabindex = tabLink.attr('tabindex' + suffix);
    if (typeof (hfId) != "undefined") {
        hfId = '#' + hfId;
        jQuery(hfId).val(tabindex);
        if ((typeof (clearTreeSummary) != "undefined") && (typeof (extraInfo) != "undefined")) {
            if (extraInfo == "Geographies") {
                filterOperaters();
            }
            clearTreeSummary(extraInfo, isAll);
        }
    }
    jQuery('div[tabsection' + suffix + ']').each(function () {
        var item = jQuery(this);
        if (tabindex == item.attr('tabsection' + suffix)) {
            item.show();
        }
        else {
            item.hide();
        }
        item.removeClass("selected");
    }
    );
    jQuery('a[tabindex' + suffix + ']').each(function () {
        var item = jQuery(this);
        item.removeClass("selected");
    }
    );
    tabLink.addClass('selected');
    if (isSubTab) {
        if (typeof (getBid) != "undefined") {
            getBid();
        }
    }
}
/************** End Tab **********************************/
/************** Start Upload **********************************/
var ErrorMessagesId = "";
function onFileUploadBulkSuccess(e) {
	if (e.operation == "remove")
		return true;
	if (e.response.status == "OK") {

		var docId = e.response.DocumentId;
		//TODO:Osaleh to search for better way
		var parent = jQuery("#CreativeUnit" + e.response.CreativeUnitId);
		changeImage(parent, docId);
	}
	/*else {
		clearErrorMessage();
		showErrorMessage(e.response.status);
	}*/
};
function onFileUploadSuccess(e) {
    if (e.operation == "remove")
        return true;
    if (e.response.status == "OK") {
        var docId = e.response.DocumentId;
        //TODO:Osaleh to search for better way
        var parent = jQuery(e.target).parent().parent().parent();
        changeImage(parent, docId);

        if (e.response.onchange != "") {
            onchangeImageFunc = e.response.onchange;
            if (typeof (onchangeImageFunc) != "undefined")
                executeFunctionByName(onchangeImageFunc, window, "");
        }
    }
    else {

        clearErrorMessage(ErrorMessagesId);

        showErrorMessage(e.response.status, false, ErrorMessagesId);
    }
};

function onFileUploadSuccessTitle(e) {
	if (e.operation == "remove")
		return true;
	if (e.response.status == "OK") {
		var docId = e.response.DocumentId;
		//TODO:Osaleh to search for better way
		var parent = jQuery(e.target).parent().parent();
		changeImage(parent, docId);

		if (e.response.onchange != "") {
			onchangeImageFunc = e.response.onchange;
			if (typeof (onchangeImageFunc) != "undefined")
				executeFunctionByName(onchangeImageFunc, window, "");
		}
	}
	else {

		clearErrorMessage(ErrorMessagesId);

		showErrorMessage(e.response.status, false, ErrorMessagesId);
	}
};
function onFileUploadZipSuccess(e) {

	clearErrorMessage();
	if (e.operation == "remove")
		return true;
	if (e.response.status == "OK") {
		
		var docId = e.response.DocumentId;
		var creativeid = e.response.CreativeUnitId;
	
		var FileName = e.response.FileName;

		
		//TODO:Osaleh to search for better way
		var parent = jQuery(e.target).parent().parent();
		changeZip(parent, docId, creativeid, FileName);

		if (e.response.onchange != "") {
			onchangeImageFunc = e.response.onchange;
			if (typeof (onchangeImageFunc) != "undefined")
				executeFunctionByName(onchangeImageFunc, window, "");
		}


		$("#ClickTagsDiv").empty();
		if (e.response.clickTags && e.response.clickTags.length>0) {
			for (var i = 0; i < e.response.clickTags.length;i++) {
				var divto = PrepareHTMLForClickTags(e.response.clickTags[i].TrackingUrl, e.response.clickTags[i].VariableName, i);
				$("#ClickTagsDiv").append(divto);
				$("#ClickTags_" + i + "__TrackingUrl").val(e.response.clickTags[i].TrackingUrl).trigger("onchange");
			}

		}
	}
	else {

		clearErrorMessage(ErrorMessagesId);

		showErrorMessage(e.response.status, false, ErrorMessagesId);
	}
};
function changeZip(elem, docId, creativeid, FileName) {
	
	var src = baseUrl + 'Downloader.ashx?docId=' + docId;
	elem.find('#NofileZip').hide();
	elem.find('#fileZip').show().attr('onclick', "downlaodByPostAction('" + src + "')");

	elem.find('#FileNameTxt').text(FileName);

	elem.find('#clearlnk').show();

	var docIdElm = elem.find('#SelectedHTML5DocumentId');
	docIdElm.val(docId);
	$("#displayUploadZipFileRequiredMsg").hide();
	elem = elem.parent().parent();
	var creativeidint = parseInt(creativeid);
	if (creativeidint>0)
	elem.find('#SelectedHTML5CreativeId').val(creativeid);
	elem.find('#clearlnk').show();
	elem.find('#copylnk').show();
	elem.find('#previewlnk').show();
    elem.find('#dialoglnk').show();
	// For Impression tracker
	elem.find('[id^=impressionTrackerRedirectContainer]').show();
	//jQuery('#fileImage').attr('src', src + docId);  
};
function clearZip(elem) {
	elem.find('#fileZip').hide().attr('onclick', "return false;");
	elem.find('#NofileZip').show();
	elem.find('#clearlnk').hide();
	elem.find('#copylnk').hide();
	elem.find('#previewlnk').hide();
    elem.find('#dialoglnk').hide();
	// For Impression tracker
	elem.find('[id^=impressionTrackerRedirectContainer]').hide({
		duration: 'slow'
	});
	var docIdElm = elem.find('#SelectedHTML5DocumentId');
	docIdElm.val('');
};
function clearZipUploadFile(link) {
	var elm = jQuery(link).parent().parent();
	clearZip(elm);

	//if (onchangeImageFunc != "")
	//    executeFunctionByName(onchangeImageFunc, window, "");
};
function executeFunctionByName(functionName, context, args) {
    var args = [].slice.call(arguments).splice(2);
    var namespaces = functionName.split(".");
    var func = namespaces.pop();
    for (var i = 0; i < namespaces.length; i++) {
        context = context[namespaces[i]];
    }
    return context[func].apply(context, args);
}
function changeImage(elem, docId) {
    var src = baseUrl + 'Downloader.ashx?docId=';
    elem.find('#fileImage').show().attr('src', src + docId);
    var docIdElm = elem.find('#docId');
    docIdElm.val(docId);
    elem = elem.parent();
    elem.find('#clearlnk').show();
    elem.find('#copylnk').show();
	elem.find('#previewlnk').show();
    elem.find('#dialoglnk').show();

	// For Impression tracker
	elem.find('[id^=impressionTrackerRedirectContainer]').parent().show();
	elem.find('[id^=impressionTrackerRedirectContainer]').show();
    //jQuery('#fileImage').attr('src', src + docId);  
};
function clearTileImage(elem) {
    elem.find('#fileImage').hide().attr('src', defualtImageUrl);
    elem.find('#clearlnk').hide();
    elem.find('#copylnk').hide();
    elem.find('#previewlnk').hide();
    elem.find('#dialoglnk').hide();
    // For Impression tracker
    elem.find('[id^=impressionTrackerRedirectContainer]').hide({
        duration: 'slow'
    });
    var docIdElm = elem.find('#docId');
    docIdElm.val('');
};

function clearUploadFile(link) {
    var elm = jQuery(link).parent().parent();
    clearTileImage(elm);

    //if (onchangeImageFunc != "")
    //    executeFunctionByName(onchangeImageFunc, window, "");
};
var dialogWidth = 0;
var dialogHeight = 0;
function previewUploadFile(link) {
    var elm = jQuery(link).parent().parent();
    var fileImage = elm.find('#fileImage');
    var imgPreview = jQuery('#imgPreview');
    var dialogForm = jQuery("#dialog-form");
    var src = fileImage.attr('src');
    if ((typeof (src) != 'undefined') && (src != '')) {
        imgPreview.attr('src', src);
        dialogWidth = fileImage.width();
        dialogHeight = fileImage.height();
        imgPreview.css('height', dialogHeight);
        imgPreview.css('width', dialogWidth);
        dialogForm.dialog("open");
    }
};
function previewDilaogOpen(event, ui) {
    jQuery(event.target).dialog('option', 'width', (dialogWidth + 50) + 'px');
    jQuery(event.target).dialog('option', 'position', 'center');
};
function TileImageChanged(e) { //(event, item) 
    {
        var value = jQuery('#tileImage').val();

        if (value != "-1") {
            var info = value.split('#');
            jQuery('#TileImage').val(info[0]);
            for (var i = 1; i < info.length; i++) {
                var tileImages = info[i].split('&');
                var imgId = "#tileImage_" + tileImages[0];
                var elem = jQuery(imgId);
                changeImage(elem, tileImages[1]);
                elem.parent().parent().find('[id^="uploadBtn_"]').hide();
            }
        }
        else {
            jQuery('#TileImage').val("-1");
            jQuery('[name^="tileImage_"]').each(
                function () {
                    var elem = jQuery(this);
                    clearTileImage(elem);
                    elem.parent().parent().find('[id^="uploadBtn_"]').show();
                }
            );
        }
    }
};
function checkFile(fileInfo, items) {
    var returnValue = false;
    var isError = false;
    jQuery(items).each(function () {

        if (this.extension.toLowerCase() == fileInfo.extension.toLowerCase()) {
            if (fileInfo.size < this.maxSize) {
                returnValue = true;
                return false;
            }
            else {

                clearErrorMessage();
                showErrorMessage(fileUploadSizeMsg);
                isError = true;
            }
        }
    });


    if ((!returnValue) && (!isError)) {
        clearErrorMessage();
        showErrorMessage(fileUploadTypeMsg);
    }
    return returnValue;
};
function copyFile(link, typeId) {
	var elm = jQuery(link).parent().parent().parent();
    var docIdElm = elm.find('#docId');
    var sendData = new Object();
    sendData.documentId = docIdElm.val();
    sendData.parentId = docIdElm.attr("parentId");
    sendData.typeId = docIdElm.attr("TypeId");
    sendData.adtypeid = docIdElm.attr("adtypeid");
    var prams = jQuery.toJSON(sendData);
    jQuery.ajax(
        {
            url: baseUrl + '/Document/Copy',
            dataType: "text json",
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: prams,
            success: function (copiedFiles) {
                if (copiedFiles != null) {
                    for (var i = 0; i < copiedFiles.length; i++) {
                        var item = copiedFiles[i];
                        //get the element using type and parent id
                        var container = jQuery('#banner_' + item.TypeId + '_' + item.ParentId);
                        if (container.length > 0) {
                            changeImage(container, item.DocId);
                        }
                    }
                }
            },
            error: function (error) {
                clearErrorMessage();
                showErrorMessage(error.responseText);
            }
        });
}
/************** End Upload **********************************/

/************** Start App/Site  **********************************/
function onRowDataBound(e) {
    //    var row = jQuery(e.row);
    //    row.mouseover(function (event) {
    //        showGridToolTip(e.dataItem.Id, event);
    //    });
};
var toolTipTimeout = null;
var gridToolTip = null;
var toolTipFlag = true;
function clearToolTipTimeout() {
    if (toolTipTimeout) {
        clearTimeout(toolTipTimeout);
    }
}
function startToolTipTimeout() {
    toolTipTimeout = setTimeout(function () { gridToolTip.hide(); }, 1100);
}
function mouseX(evt) {
    if (evt.pageX) return evt.pageX;
    else if (evt.clientX)
        return evt.clientX + (document.documentElement.scrollLeft ?
            document.documentElement.scrollLeft :
            document.body.scrollLeft);
    else return null;
}
function mouseY(evt) {
    if (evt.pageY) return evt.pageY;
    else if (evt.clientY)
        return evt.clientY + (document.documentElement.scrollTop ?
            document.documentElement.scrollTop :
            document.body.scrollTop);
    else return null;
}
function showGridToolTip(id, elem, item_extra_info, ExtraPrams) {

    gridItemId = id;
    gridToolTip = jQuery('#gridToolTip');
    if (typeof (id) == "undefined")
        return;

    var sourceElem = jQuery(elem); //eventObj.srcElement ? eventObj.srcElement : eventObj.originalTarget;
    clearToolTipTimeout();
    if (toolTipFlag) {
        gridToolTip.mouseleave(startToolTipTimeout);
        gridToolTip.mouseenter(clearToolTipTimeout);
        toolTipFlag = false;
    }
    attext = jQuery(elem).attr("item_extra_info");
    if (hasValue(attext)) {
        item_extra_info = attext;
    }
  
    var srcDownload = baseUrl + 'Downloader.ashx?docId=';


    jQuery('[customType="GridToolTip"]').each(function () {

        var item = jQuery(this);
        var basicurl = item.attr("basicurl");

        if (hasValue(item_extra_info)) { item.attr("item_extra_info", item_extra_info); }
        else {
            item.attr("item_extra_info", '');
        }

        if (item.attr("type") == "Download") {
            item.hide();
            if (hasValue(item_extra_info)) {
                if (item_extra_info != "0") {
                    srcDownload = srcDownload + item_extra_info;
                    //item.href = item
                    item.attr("href", srcDownload);
                    item.show();
                }

            }

        }
        if (item.attr("type") == "unArchive") {

            if (typeof (ExtraPrams) != "undefined" && ExtraPrams == "False") {
                $(item).hide();
            } else {
                $(item).show();
            }
        }

        if (item.attr("type") == "rename") {

            basicurl = decodeURIComponent(basicurl) + id;

        }
        else {
            if (basicurl.indexOf("?") != -1) {
                if (basicurl.indexOf("Action=scm") != -1) {
                    basicurl = basicurl.replace("Action=scm&", "");
                    basicurl += "&id=" + id;
                }
                else {

                    basicurl += "&itemId=" + id;
                }
            }
            else {
                if (item.attr("type") == "dashboard") {
                    basicurl += "/" + id + "?chartType=ad";
                } else if (item.attr("type") == "AccountAdvertisers") {
                    basicurl += "?AdvertiseraccId=" + id;

                }
                else {
                    basicurl += "/" + id;
                }
            }
            if (typeof (ExtraPrams) != "undefined" && ExtraPrams != "" && item.attr("type") != "unArchive" && ExtraPrams != "True" && ExtraPrams != "False") {
                basicurl += ExtraPrams;
            }

        }
        if ((typeof (item.attr("no-href")) == "undefined") || (item.attr("no-href") == false)) {
            item.attr("href", basicurl);
        } else {
            item.attr("href2", basicurl);
        }

        if (!(typeof (item.attr("ValItem")) == "undefined")) {

            item.attr("ValItem", id);
        }
    });
    var margin = 18;
    var xVal = sourceElem.offset().left + margin;
    var yVal = sourceElem.offset().top;  // eventObj.pageY ? eventObj.pageY : eventObj.clientY;
    gridToolTip.css('top', yVal);
    if (currentDirection == 'rtl') {
        //    if (jQuery.browser.msie) {
        //        margin = 0;
        //        if (jQuery.browser.version.slice(0, 3) == '7.0') {
        //            margin = 15;
        //    }
        //}

        var xVlaue = jQuery(document).width() - xVal + margin;
        gridToolTip.css('right', xVlaue);
    }
    else {
        gridToolTip.css('left', (xVal));
    }
    //    height: 60px;

    // gridToolTip.css('height', (height - heightToRemove) + heightToAdd);
    gridToolTip.slideDown("slow");
    // gridToolTip.animate({ left: left + 'px', top: top + 'px' }, 1000); ; //.fadeIn('slow');
    toolTipTimeout = setTimeout(function () { gridToolTip.hide(); }, 1300);
}
function showGeneralGridToolTip(elem, htmlItem, callBackFunction) {

    gridToolTip = jQuery('#gridToolTip');

    var sourceElem = jQuery(elem); //eventObj.srcElement ? eventObj.srcElement : eventObj.originalTarget;
    clearToolTipTimeout();
    if (toolTipFlag) {
        gridToolTip.mouseleave(startToolTipTimeout);
        gridToolTip.mouseenter(clearToolTipTimeout);
        toolTipFlag = false;
    }

    // Fill html content
    gridToolTip.html($("#" + htmlItem).html());

    var margin = 18;
    var xVal = sourceElem.offset().left + margin + sourceElem.width();
    var yVal = sourceElem.offset().top;  // eventObj.pageY ? eventObj.pageY : eventObj.clientY;
    gridToolTip.css('top', yVal);
    if (currentDirection == 'rtl') {
        //    if (jQuery.browser.msie) {
        //        margin = 0;
        //        if (jQuery.browser.version.slice(0, 3) == '7.0') {
        //            margin = 15;
        //    }
        //}
        xVal = sourceElem.offset().left

        var xVlaue = jQuery(document).width() - xVal + margin;
        gridToolTip.css('right', xVlaue);
    }
    else {
        gridToolTip.css('left', (xVal));
    }
    gridToolTip.slideDown("slow");

    if (callBackFunction != undefined) {
        eval(callBackFunction);
    }
    // gridToolTip.animate({ left: left + 'px', top: top + 'px' }, 1000); ; //.fadeIn('slow');
    toolTipTimeout = setTimeout(function () { gridToolTip.hide(); }, 2000);

}
function closeGridToolTip() {
    gridToolTip = jQuery('#gridToolTip');
    gridToolTip.hide();
}
var textColor = null;
var backgroundColor = null;
var themeSample = null;

function ThemeChanged(e) {//(event, item) {
    var value = jQuery('#themeList').val();
    if (value != "") {
        var info = value.toString().split("!");
        jQuery("#AppSiteDto_Theme_Id").val(info[0]);
        textColor.val(info[2]);
        backgroundColor.val(info[1]);
        //        backgroundColor.removeClass('required').removeAttribute('data-val-required');
        //        backgroundColor.val('');
        //        textColor.val('');
        themeSample.css("background-color", info[1]).css("color", info[2]);
    }
    else {
        clearTheme();
    }
}
function clearTheme(item, value) {
    var themeList_Obj = jQuery("#themeList");
    if (typeof (value) != "undefined") {
        if (value == "true") {
            themeList_Obj.attr("disabled", value);
        }
        else {
            themeList_Obj.removeAttr('disabled');
            jQuery("#themeList").val("");
        }
    }

    themeList_Obj[0].selectedIndex = 0;
    //jQuery('#themeList').val('');
    backgroundColor.attr('readonly', !backgroundColor.attr('readonly'));
    textColor.attr('readonly', !textColor.attr('readonly'));
    jQuery("#AppSiteDto_Theme_Name").val('');
    jQuery("#AppSiteDto_Theme_Id").val('-1');
    textColor.val('');
    backgroundColor.val('');
    themeSample.css("background-color", '').css("color", '');
}

function initAppSitePage(isCustom) {
    var themListElm = jQuery('#themeList');
    textColor = jQuery("#AppSiteDto_Theme_TextColor");
    backgroundColor = jQuery("#AppSiteDto_Theme_BackgroundColor");
    themeSample = jQuery("#themeSample");

    themListElm.change(ThemeChanged);
    if (!isCustom) {
        backgroundColor.attr('readonly', true);
        textColor.attr('readonly', true);
    }
    textColor.bind("change", function () {
        var color = new RGBColor(textColor.val());
        if (!color.ok) {

            themeSample.css("color", '');
            textColor.val('');
            //TODO: Osaleh to use the Alert Dialog
        }
        themeSample.css("color", '').css("color", textColor.val());
    });

    backgroundColor.bind("change", function () {
        var color = new RGBColor(backgroundColor.val());
        if (!color.ok) {
            themeSample.css("background-color", '');
            backgroundColor.val('');
            //TODO: Osaleh to use the Alert Dialog
        }
        themeSample.css("background-color", '').css("background-color", backgroundColor.val());
    });
}

//function generateAppSiteGrid() {
//    var grid = $("#Grid").data("tGrid");
//    grid.rebind();
//}

//function appSiteGridDataBinding(args) {
//    args.data = $.extend(args.data,
//            {
//                FromDate: $('#FromDate').val(),
//                ToDate: $('#ToDate').val(),
//                StatusId: $('#StatusId').val(),
//                CampaignId: '@Html.ViewContext.RouteData.Values["CampaignId"]'
//            });
//};


function onAppTypeChange(elem, value) {
    spinner.showSpinner();
    jQuery("#AppDetails").html('');
    var urll;
    var VAL = value.toString();
    if ((appSiteViewUrl.toString()).indexOf("ApprovalView") > 0) {
        if (VAL.indexOf("iOS") > -1)
            value = "ApprovalIos";
        else if (value.indexOf("Android") > -1) {
            value = "ApprovalAndroid";
        } else if (VAL.indexOf("Site") > -1) {
            value = "ApprovalAppSite";
        }

        urll = appSiteViewUrl + '/' + appSiteId + "?ViewName=" + value;
    }
    else {
        urll = appSiteViewUrl + '/' + value + '/' + appSiteId;
    }
    jQuery.ajax(
        {

            url: urll,

            cache: false,
            success: function (html) {
                html = html.toString();
                var newelem = jQuery(html);
                jQuery("#AppDetails").append(newelem);
                parseScript(html);
                initilize();
                if (typeof (localInitilize) != "undefined") {
                    localInitilize();
                }
                jQuery.validator.unobtrusive.parseDynamicContent("#AppDetails");

                spinner.hideSpinner();

                //  AppSiteLabel();
            },
            error: function (error) {
                spinner.hideSpinner();
            }
        });
};


function addValidationRules_IP() {
    jQuery.validator.addMethod("checkIP",
        function (value, element) {
            var result = false;
            return result;
        }, 'IP Not Valid');
};
function addIPRule(name) {
    var args = {
    };
    args['checkIP'] = true;
    jQuery('[name="' + name + '"]').rules("add", args);
};

function onPublishedChangde(item) {
    var isChecked = document.getElementById('AppSiteDto.IsPublished').checked;
    if (isChecked) {
        addUrlRule();
    }
    else {
        removeUrlRule();
    }

};
function addUrlRule() {
    var args = {
    };
    args['required'] = true;
    $('[name="AppSiteDto.URL"]').rules("add", args);
};
function removeUrlRule() {
    var args = {
    };
    args['required'] = false;
    $('[name="AppSiteDto.URL"]').rules("add", args);
};
/************** End   App/Site  **********************************/

/******************* Register *********************************/
function ValidateUrl(elem, error_msg_id) {

    //  
    if (!hasValue(error_msg_id)) {
        error_msg_id = "displayURLErrorMsg";
    }
    $('#' + error_msg_id).hide();
    var returnvalue = true;
    //check if display url is empty
    var val = elem.val();
    if ((val != null) && $.trim(val) != '') {
        if (!isURL(val, elem)) {
            returnvalue = false;
            $('#' + error_msg_id).show();
        }
        else {
            $('#' + error_msg_id).hide();
        }
    }
    return returnvalue;
};
function activateButton() {

    if ($("#acceptTerms:checked").length != 0) {
        $("#saveBtn").removeAttr("disabled").removeClass('disabled');
    }
    else {
        $("#saveBtn").attr("disabled", "disabled").addClass('disabled');
    }
}

/****************** End Register ******************************/

/************** Start More Info  **********************************/
function closeMoreInfo() {
    jQuery('.info-popup').each(function () {
        var item = jQuery(this);
        if (item.is(':visible')) {
            item.hide();
        }
    });
}
function showMoreInfo(elem) {

    var elemObje = jQuery(elem);
    var p = getPosition(elemObje); //elemObje.offset();
    var infoDialog = elemObje.find('#infoDialog');
    infoDialog.css('top', p.top - 13);
    if (currentDirection == 'rtl') {
        infoDialog.css(getPositionProperty(), p.left - 3);
    }
    else {
        infoDialog.css(getPositionProperty(), p.left + 22);
    }
    infoDialog.toggle('fast');

    var evt = window.event || arguments.callee.caller.arguments[0];
    evt.stopPropagation();
    return false;

}
/************** End   More Info  **********************************/
/************** Approve Ad       *********************************/


/************** End Approve Ad       *********************************/

/**************** Performance Dashboard ******************************/

function drawChart(chart, data, options) {

    chart.draw(data, google.charts.Bar.convertOptions(options));
}

function drawPieChart(containerId, jsonData, width, height, options, title) {
    var remainingFromWidth = (.15 * width);
    var remainingHeight = (.15 * height);

    if (typeof (options) == 'undefined') {
        options = {
            'width': width,
            'height': height,
            'legend': {
                //'position': 'left'
            },
            title: title !== 'undefined' ? title : "",

            'chartArea': {
                'width': width - remainingFromWidth,
                'height': height - remainingHeight,
                'left': 30
            }

        };
    }

    // Create our data table.
    var data = new google.visualization.DataTable(jsonData);

    var chart = new google.visualization.PieChart(document.getElementById(containerId));

    drawChart(chart, data, options);

    return chart;
}


function drawBarChart(containerId, jsonData, width, height, options) {

    if (typeof (options) == 'undefined') {

        var remainingFromWidth = (.50 * width);
        var remainingHeight = (.20 * height);

        options = {
            'chartArea': {
                'top': '10',
                'width': width - remainingFromWidth,
                'height': height - remainingHeight
            },
            'width': width,
            'height': height,
            'legend': {
                'position': 'none'
            },
            'bar': {
                'groupWidth': '25%'
            },
            'hAxis': {
                'textPosition': 'out',
                'textStyle': {
                    'fontSize': '10'
                }
            }
        };
    }

    // Create our data table.
    var data = new google.visualization.DataTable(jsonData);

    var chart = new google.charts.Bar(document.getElementById(containerId));

    var hideContainer = false;
    var container = $("#" + containerId);
    if (container.css("display") == 'none') {
        container.show();

        hideContainer = true;
    }

    google.visualization.events.addListener(chart, 'ready', function () {
        if (hideContainer) {
            container.hide();
        }
    });

    drawChart(chart, data, options);


    return chart;
}



function loadEntireAppSiteDashboardReport() {

    if (performanceDashboardSpinner == null) {
        performanceDashboardSpinner = getSpinner("bigContainer");
    }

    performanceDashboardSpinner.showSpinner();

    $("#accountsPerformanceLink").attr("href", accountsPerformanceUrl + decodeURIComponent("datefrom=" + $("#DateFrom").val() + "&dateto=" + $("#DateTo").val()));
    $("#appsitesPerformanceLink").attr("href", appsitesPerformanceUrl + decodeURIComponent("datefrom=" + $("#DateFrom").val() + "&dateto=" + $("#DateTo").val()));

    rebindGrid('TopAccountsGrid');
    rebindGrid('TopAppSitesGrid');

    var opts = {
        FromDate: $('#DateFrom').val(),
        ToDate: $('#DateTo').val(),
        orderColumn: $('#metric').val()
    };

    $.when(
        $.ajax({
            type: 'post',
            url: topAccountsUrl + "?" + decodeURIComponent($.param(opts)),
            success: function (data) {
                drawBarChart('topAccountsChartContainer', data, 540, 350);
            }
        }),

        $.ajax({
            type: 'post',
            url: topAppSitesUrl + "?" + decodeURIComponent($.param(opts)),
            success: function (data) {
                drawBarChart('topAppSitesChartContainer', data, 540, 350);
            }
        }),

        $.ajax({
            type: 'post',
            url: platformsUrl + "?" + decodeURIComponent($.param(opts)),
            success: function (data) {
                drawPieChart('platformsPerformanceContainer', data, 420, 350);
            }
        })).done(function () {
            performanceDashboardSpinner.hideSpinner();
        }).fail(function () {
            performanceDashboardSpinner.hideSpinner();
        });


}

function searchAppSitesPerformance() {
    var opts = {
        FromDate: $('#DateFrom').val(),
        ToDate: $('#DateTo').val(),
        AccountName: $('#AccountName').length > 0 ? $('#AccountName').val() : '',
        AppSiteName: $("#AppSiteName").val(),
        CountryIds: $("#country").val(),
        MetricValue: $("#metric").val()
    };

    if (topAppSitesPerformanceSpinner == null) {
        topAppSitesPerformanceSpinner = getSpinner("appSitesResultContainer");
    }

    topAppSitesPerformanceSpinner.showSpinner();

    rebindGrid('AppSitesGrid');

    $.when(
        $.ajax({
            type: 'post',
            url: appSitesChartUrl,
            data: $.param(opts, true),
            success: function (data) {
                drawBarChart('topAppSitesChart', data, 900, 450);

            }
        })).done(function () {
            topAppSitesPerformanceSpinner.hideSpinner();
        }).fail(function () {
            topAppSitesPerformanceSpinner.hideSpinner();
        });

}


function searchAppSitesAccountsPerformance() {
    var opts = {
        FromDate: $('#DateFrom').val(),
        ToDate: $('#DateTo').val(),
        AccountName: $("#AccountName").val(),
        AppSiteName: $("#AppSiteName").val(),
        CountryIds: $("#country").val(),
        MetricValue: $("#metric").val()
    };

    if (topAppSitesAccountsPerformanceSpinner == null) {
        topAppSitesAccountsPerformanceSpinner = getSpinner("appSitesResultContainer");
    }

    topAppSitesAccountsPerformanceSpinner.showSpinner();

    rebindGrid('AccountsGrid');

    $.when(
        $.ajax({
            type: 'post',
            url: appSitesAccountsChartUrl,
            data: $.param(opts, true),
            success: function (data) {
                drawBarChart('topAccountsChart', data, 900, 450);
            }
        })).done(function () {
            topAppSitesAccountsPerformanceSpinner.hideSpinner();
        }).fail(function () {
            topAppSitesAccountsPerformanceSpinner.hideSpinner();
        });

}


function loadAppSiteBasicInformation(appSiteId) {


    var container = $('#tooltipContainer' + appSiteId);

    if (container.find("[name=tableBasicInformation]").length == 0) {
        console.log("found");
        $.ajax({
            type: 'post',
            url: appsiteBasicInformation,
            data: {
                appsiteId: appSiteId
            },
            success: function (data) {
                container.html(data);
            }
        });
    }
}


var count = 0;
function disable(e, func) {


    var isvalid = eval(func);

    if (typeof isvalid == 'undefined')
        isvalid = true;

    if (count == 0 && isvalid) {

        $(e).toggleClass("disabled");
        count++;
        return true;
    }


    if (!isvalid) {
        count = 0;
    }

    return false;
}
function checkDeal() {

    return true;
}
function fillkeywords() {
    $("#Keywords").find("a").each(function () {
        var e = this;
        $("#keywordTags").find("a").each(function () {
            var x = this;
            if (e.innerText.replace(/\s/g, '') == x.innerText.replace(/\s/g, '')) {
                jQuery(x).hide();
            }
        });
    });

}


function approval(approvalStaus) {



    var sendData = new Object();
    sendData.NewKeywords = getKeywords();
    sendData.Comments = $("#Comments").val();
    sendData.Type = new Object();
    sendData.Type.Id = $("#Type_Id").val();
    sendData.Type.ViewName = $("#Type_ViewName").val();
    sendData.AppSiteId = $("#AppSiteId").val();
    sendData.approveStatus = approvalStaus;
    sendData.DeletedKeywords = getDeletedKeywords();


    prams = $.toJSON(sendData);
    $.ajax({
        url: ApprovalActionsUrl,

        dataType: "text json",
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        data: prams,

        success: function (data) {
            if (data.status) {

                window.location = ApprovalActionsUrl + "?successfulMassage=" + data.SuccessfulMassage + "&errorMassge=" + data.ErrorMassge + "&status=" + data.status;

            } else {

                showErrorMessage(data.ErrorMassge, true);
            }

        }

    });

}






function CommoDialogInitilize() {
    $("#party-add-dialog-form").dialog({
        autoOpen: false,
        //  height: 490,
        width: 446,

        modal: true,
        resizable: false,
        draggable: false
    });
}



function getCostElementsValues() {

    if (ad_group_id != "") {

        var sendData = new Object();
        sendData.campaignId = campaign_id;
        sendData.GroupId = adgroupid;
        var result;
        prams = $.toJSON(sendData);

        $.ajax({
            url: GetCostElementsValuesUrl,

            dataType: "text json",
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: prams,

            success: function (data) {

                CostElementsValuesSum = data.Sum;
                CampaignBudget = data.Budget;
            },

            error: function (error) {

            }

        });


    }
}

function onbidpress(e, bidTextBox) {


    var key = e.keyCode || e.charCode;

    if ($(bidTextBox).attr("isSelectContent") == 'true') {
        $(bidTextBox).val('');
        $(bidTextBox).attr("isSelectContent", "false");
    }
    if (key == 13) {
        onenterkeybidpress(e);
    }

    var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");


    if (!(String.fromCharCode(key) == '.' && bidTextBox.value.length > 0 && bidTextBox.value.indexOf('.') < 0) && key != 8) {

        if (!REGULAR.test((bidTextBox.value + String.fromCharCode(key)))) {
            e.preventDefault();
        }
    }

}


function onbidpress6(e, bidTextBox) {


    var key = e.keyCode || e.charCode;

    if ($(bidTextBox).attr("isSelectContent") == 'true') {
        $(bidTextBox).val('');
        $(bidTextBox).attr("isSelectContent", "false");
    }
    if (key == 13) {
        onenterkeybidpress(e);
    }

    var REGULAR = new RegExp("^\\d{1,6}(\\.\\d{1,3})?$");


    if (!(String.fromCharCode(key) == '.' && bidTextBox.value.length > 0 && bidTextBox.value.indexOf('.') < 0) && key != 8) {

        if (!REGULAR.test((bidTextBox.value + String.fromCharCode(key)))) {
            e.preventDefault();
        }
    }

}
function onbidchange6(item) {

    var value = $(item).val();
    var REGULAR = new RegExp("^\\d{1,6}(\\.\\d{1,3})?$");

    if (!REGULAR.test(value)) {
        $(item).val("");
        return false;

    }
    return true;
}

function onenterkeybidpress(e) {
    var key = e.keyCode || e.charCode;

    if (key == 13) {
        if (onbidchange("#Bid")) {
            if ($("[Name='Continue']").length) {
                if (checkStatus(true))
                    $("#targetingForm").submit();

            }
            else {
                if (checkStatus(false))
                    $("#targetingForm").submit();

            }
        }
    }


}


function AccountIdKeyPress(event) {
    var key = event.keyCode || event.charCode;
    if (!isFinite(event.key) && isNaN(String.fromCharCode(event.key))) {
        if (key != 13) {

            event.preventDefault();
        }
    }
}

function stopSubmit(e) {
    e.preventDefault();

}


function onbidchange(item) {

    var value = $(item).val();
    var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");

    if (!REGULAR.test(value)) {
        $(item).val("");
        return false;

    }
    return true;
}



//function validateGridBidsValuesInBidTap(gridName) {

//    var grid = $("#" + gridName).data("tGrid");
//    array = $("#" + gridName + " tbody tr");
//    var isValid = true;
//    for (var i = 0; i < array.length; i++) {
//        dataItem = grid.dataItem(array[i]);
//        var bidTextBox = "";
//        if (dataItem != undefined && dataItem.ID != undefined) {
//            bidTextBox = $(array[i]).find("#Bid" + dataItem.ID);
//        }


//        if (bidTextBox.length == 0)
//        { bidTextBox = $(array[i]).find("#Bid"); }


//        if (bidTextBox.length > 0 && !IsBidValidDecimalExpression(bidTextBox.val()) || bidTextBox.val() == "" || bidTextBox.val() == parseBidValue(0)) {
//            bidTextBox.addClass("input-validation-error");
//            isValid = false;
//        }

//        if (bidTextBox.val().length == 0) {
//            isValid = false;
//        }
//    }
//    return isValid;
//}

function isdeletedBid(gridName, dataItem) {

    var grid = $("#" + gridName).data("tGrid");
    for (var i = 0; i < grid.changeLog.deleted.length; i++) {
        if (typeof (grid.changeLog.deleted[i]) != "undefined")
            if (dataItem.Appsite.ID == grid.changeLog.deleted[i].Appsite.ID && dataItem.AccountId == grid.changeLog.deleted[i].AccountId && dataItem.SubPublisherId == grid.changeLog.deleted[i].SubPublisherId)
                return true;
    }
    return false;

}

function validateDialogGridBidsValues(gridName) {

    var grid = $("#" + gridName).data("tGrid");
    array = $("#" + gridName + " tbody tr");
    var isValid = true;
    if (grid.data.length > 0) {
        for (var i = 0; i < array.length; i++) {
            dataItem = grid.dataItem(array[i]);
            var bidTextBox = "";
            var minbidTextBox = "";
            if (dataItem != undefined && dataItem.ID != undefined) {
                bidTextBox = $(array[i]).find("#Bid" + dataItem.ID);

            }

            if (bidTextBox.length == 0) {
                bidTextBox = $(array[i]).find("#Bid");
            }


            minbidTextBox = $(array[i]).find("#MinBid");


            if ((bidTextBox.length > 0 && !IsBidValidDecimalExpression(bidTextBox.val()) || bidTextBox.val() == "" || bidTextBox.val() == parseBidValue(0)) || parseFloat(bidTextBox.val()) > parseFloat(minbidTextBox.text())) {

                bidTextBox.addClass("input-validation-error");
                isValid = false;
            }

            if (bidTextBox.val().length == 0) {
                isValid = false;
            }

        }
    }
    return isValid;
}

function getBidCampaignBidConfigList() {

    var grid = $('#CampaignBidConfigList').data("tGrid");
    var campaignBidConfigList = [];
    if (grid == null)
        return null;

    for (var i = 0; i < grid.changeLog.inserted.length; i++)
        if (typeof (grid.changeLog.inserted[i]) != "undefined")
            campaignBidConfigList.push(grid.changeLog.inserted[i].Bid);

    var updated = [];

    if ($('#UpdatedCampaignBidConfiges').val().length > 0)
        updated = JSON.parse($('#UpdatedCampaignBidConfiges').val());

    for (var y = 0; y < updated.length; y++)
        if (typeof (updated[y]) != "undefined")
            campaignBidConfigList.push(updated[y].Bid);


    return campaignBidConfigList;
}

function validateGridBidsValues(gridName) {


    var isValid = true;
    minbid = $("#bidSection").find("#Bid").val();
    var arrayOfBids = getBidCampaignBidConfigList();
    for (var i = 0; i < arrayOfBids.length; i++) {
        if (eval(arrayOfBids[i]) < eval(minbid))
            return false;
    }
    return isValid;
}


// Bid Config Dialog 
function DialogshowErrorMessage(id) {
    $("#" + id).attr("style", "display:block;");

};


function DialogclearErrorMessage(id) {
    $("#" + id).attr("style", "display:none;");
};


function onBid_select(elem) {

    $(elem).select();
    $(elem).attr("isSelectContent", true);
}

function validateBidValue(elem, bidValue) {

    isValidBid = IsBidValidDecimalExpression(bidValue);

    return isValidBid;
}

function CampaignBidConfigs_BidValue_Changed(elem) {

    onbidchange(elem);
    var grid = $("#CampaignBidConfigs").data("tGrid");
    var tr = $(elem).parents('tr');
    isvalidBid = validateBidValue(elem, elem.value);
    if (isvalidBid) {
        elem.value = parseFloat(elem.value).toFixed(3);
    }

}

function getNotCompatableItems() {

    var grid = $('#CampaignBidConfigs').data("tGrid");
    var campaignBidConfigsGridRowsArray = $("#CampaignBidConfigs tbody tr");
    var notCompatableItems = new Array();
    if (grid == null)
        return;

    for (var i = 0; i < campaignBidConfigsGridRowsArray.length; i++) {

        var campaignBidConfigsDataItem = grid.dataItem(campaignBidConfigsGridRowsArray[i]);

        if (!isdeletedBid("CampaignBidConfigs", campaignBidConfigsDataItem)) {
            campaignBidConfigsDataItem.Bid = campaignBidConfigsGridRowsArray.eq(i).find("[name='Bid']").val();
            notCompatableItems.push(campaignBidConfigsDataItem);
        }
    }
    return notCompatableItems;
}

function IsValidBidConfigDialog() {

    DialogclearErrorMessage("divErrorMessagesForBidConfigsDialog");

    if (validateDialogGridBidsValues("CampaignBidConfigs")) {
        CampaignBidConfigNotCompleted = false;
        return true;

    } else {
        CampaignBidConfigNotCompleted = true;
        return false;
    }
}

function parseBidValue(value) {
    return parseFloat(value).toFixed(3);
}

function CampaignBidConfigs_OnDataBound(sender, args) {

    var grid = $("#CampaignBidConfigs").data("tGrid");
    array = $("#CampaignBidConfigs tbody tr");
    for (var i = 0; i < array.length; i++) {
        dataItem = grid.dataItem(array[i]);
        if (dataItem != undefined) {
            if (dataItem.Bid != "") {
                $(array[i]).find("#Bid")[0].value = parseBidValue(dataItem.Bid);
            }
        }
    }
}

var cost_element_type = 2;

function onValuepress(e, textbox) {
    switch (cost_element_type) {
        case 1: //percentage
            var REGULAR = new RegExp("^\\d{1,3}(\\.\\d{1,3})?$");
            break;
        case 2: //fixed
            var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");
            break;
        default:
            var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");
    }

    $("#required_signValue").hide();
    var key = e.keyCode || e.charCode;

    if (cost_element_type == 1) {
        if (parseFloat(textbox.value) >= 100) {
            e.preventDefault();
        }
    }

    if (!(String.fromCharCode(key) == '.' && textbox.value.length > 0 && textbox.value.indexOf('.') < 0) && key != 8) {

        if (!REGULAR.test((textbox.value + String.fromCharCode(key))) || (parseFloat(textbox.value + String.fromCharCode(key)) > 100 && cost_element_type == 1)) {
            e.preventDefault();
        }
    }
}

function ZeroOneRegexPress(e, textbox) {

    var REGULAR = new RegExp("^(0+\\.?|0*\\.\\d+|0*1(\\.0*)?)$");


    $(textbox).parent().parent().find("#required_signValue").hide();
    var key = e.keyCode || e.charCode;

    if (!(String.fromCharCode(key) == '.' && textbox.value.length > 0 && textbox.value.indexOf('.') < 0) && key != 8) {
        var value = textbox.value + String.fromCharCode(key);
        if (!REGULAR.test(parseFloat(value))) {
            e.preventDefault();
        }
    }
    // || (parseFloat(textbox.value + String.fromCharCode(key)) > 1 || parseFloat(textbox.value + String.fromCharCode(key)) < 0)
}

function PercentageKeyPres(e, textbox) {

    var REGULAR = new RegExp("^(0+\\.?|0*\\.\\d+|0*1(\\.0*)?)$");


    $(textbox).parent().parent().find("#required_signValue").hide();
    $("#displayValueErrorMsgPercentage").hide();
    var key = e.keyCode || e.charCode;

    if (String.fromCharCode(key) == '.') {

        e.preventDefault();

    }

}
// || (parseFloat(textbox.value + String.fromCharCode(key)) > 1 || parseFloat(textbox.value + String.fromCharCode(key)) < 0)


function PercentageRegexPress(sender) {
    $(sender).parent().parent().find("#required_signValue").hide();
    $("#displayValueErrorMsgPercentage").hide();
    var REGULAR = new RegExp("/^\d+?$/");

    var value = $(sender).val();

    var x = parseFloat(value);
    if (x > 100) {
        //  $(sender).val("");
        $("#displayValueErrorMsgPercentage").show();
    }





    // || (parseFloat(textbox.value + String.fromCharCode(key)) > 1 || parseFloat(textbox.value + String.fromCharCode(key)) < 0)
}

function onbidPrecentagepress(e, o) {

	var key = e.keyCode || e.charCode;

	if (key == 13) {
		onenterkeybidpress(e);
	}
	var REGULAR = new RegExp("^\\d{1,3}(\\.\\d{1,2})?$");


	if (!(String.fromCharCode(key) == '.' && o.value.length > 0 && o.value.indexOf('.') < 0) && key != 8) {

		if (!REGULAR.test((o.value + String.fromCharCode(key))))
			e.preventDefault();
	}

}
function PercentageRegexPressById(sender) {
    $("#" + sender).parent().parent().find("#required_signValue").hide();
    $("#displayValueErrorMsgPercentage").hide();
    var REGULAR = new RegExp("/^\d+?$/");

    var value = $("#" + sender).val();

    var x = parseFloat(value);
    if (x > 100) {
        // $(sender).val("");
        $("#displayValueErrorMsgPercentage").show();
        return false;
    }

    return true;



    // || (parseFloat(textbox.value + String.fromCharCode(key)) > 1 || parseFloat(textbox.value + String.fromCharCode(key)) < 0)
}


function onValueChange(sender) {
    switch (cost_element_type) {
        case 1: //percentage
            var REGULAR = new RegExp("^\\d{1,3}(\\.\\d{1,3})?$");
            break;
        case 2: //fixed
            var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");
            break;
        default:
            var REGULAR = new RegExp("^\\d{1,5}(\\.\\d{1,3})?$");
    }

    $("#required_signValue").hide();
    var value = $(sender).val();


    if (!REGULAR.test(value) || parseFloat(value > 100 && cost_element_type == 1)) {
        $(sender).val("");

    }

}
//Floating number regex between 0 and 1
function ZeroOneRegex(sender) {
    var REGULAR = new RegExp("^(0+\\.?|0*\\.\\d+|0*1(\\.0*)?)$");

    $(sender).parent().parent().find("#required_signValue").hide();
    var value = $(sender).val();


    if (!REGULAR.test(parseFloat(value)) || parseFloat(value) > 1 || parseFloat(value) < 0) {
        $(sender).val("");

    }

}
function clearSelect2(id) {
    $("#" + id).select2("val", "()");

}
function uncheck(element) {
    $("#" + element).removeAttr("checked");
    $("#" + element).parent().attr("class", "check-box-uncheck");
}
function check(element) {
    $("#" + element).attr("checked", "checked");
    $("#" + element).parent().attr("class", "check-box-checked");
}
function Radio_uncheck(element) {
    $("#" + element).removeAttr("checked");
    $("#" + element).parent().attr("class", "radio-button-uncheck");

}
function Radio_check(element) {
    $("#" + element).attr("checked", "checked");
    $("#" + element).parent().attr("class", "radio-button-checked");
}

function Radio_IsChecked(id) {

    var checked = $(id).attr("checked");
    if (typeof (checked) == "undefined" || checked != "checked") {
        return false;
    }
    return true;
}

function clearValidation(formElement) {
    formElement = "#" + formElement;
    //Internal $.validator is exposed through $(form).validate()
    var validator = $(formElement).validate();
    //Iterate through named elements inside of the form, and mark them as error free
    $('[name]', formElement).each(function () {
        validator.successList.push(this);//mark as error free
        validator.showErrors();//remove error messages if present
    });
    validator.resetForm();//remove error class on name elements and clear history
    validator.reset();//remove all error and success data
}

function clearDialog(name) {

    var id = "#" + name;
    $(id + ' input[type="text"]').val('');
    $(id + ' input[type="hidden"]').val('');
    $(id + ' input[type="number"]').val('');
    $(id + ' textarea').val('');
    $(id + ' input[type="text"]').text('');
    $(id + ' input[type="hidden"]').text('');
    $(id + ' input[type="number"]').text('');
    $(id + ' textarea').text('');

    $(id + ' input[class="check-box"]').each(function () {

        var id = $(this).attr("id");
        uncheck(id);

    });

}



function tree_CollectSelected(treeName) {
    var treeObj = jQuery.tree.reference('#' + treeName);

    var isDefult = true;
    if ((treeObj == null) || (treeObj.container == null))
        return;
    var container = treeObj.container;
    var id = container.attr("id") + "Info";
    if ((container.attr("id") == "GeographiesTree") || (container.attr("id") == "GeographiesTreeInfo")) {
        loadOperatorsFlag = true;
    }
    var elem = jQuery("#" + id);
    if ((container.attr("id") == "OperatorsTree") || (container.attr("id") == "OperatorsInfo")) {
        //check is all flag
        //if is all then don't change the text
        if (jQuery('#OperatorTargetingIsAll').val() == '1') {
            return;
        }
        //if is WIFI Only then update the text 
        if (jQuery('#OperatorTargetingIsAll').val() == '3') {
            elem.removeClass("default-values").text(wifiOnlyText).attr('title', wifiOnlyText);
            return;
        }
        //if is specific IP Ranges  then update the text 
        if (jQuery('#OperatorTargetingIsAll').val() == '4') {
            //jQuery('#OperatorsTreeInfo').removeClass("default-values").text(specificIPRangesText).attr('title', specificIPRangesText);
            elem.removeClass("default-values").text(specificIPRangesText).attr('title', specificIPRangesText);
            return;
        }
        // }
    }
    if (container.attr("id") == "DevicesTree" && typeof deviceTreeChanged != "undefined" && deviceTreeChanged != null) {
        deviceTreeChanged = true;
    }
    if ((id == 'ManufacturersTreeInfo') || (id == "DevicesTreeInfo")) {
        id = 'deviceTargetingInfo';
    }
    elem = jQuery("#" + id);

    if (id == "GeographiesTreeInfo") {

        var notFilteredItemsList = jQuery.tree.plugins.checkbox.get_checked(treeObj);

        var finalItemList = new Array();

        notFilteredItemsList.each(function () {
            var item = jQuery(this);
            var itemParentId = item.parents('li').attr("id");

            if (itemParentId == "undefined") {
                finalItemList.push(item.attr("id"));
            } else {
                if (notFilteredItemsList.filter("[id='" + itemParentId + "']").length == 0) {
                    finalItemList.push(item.attr("id"));
                }
            }
        });

        var items = notFilteredItemsList.clone();

        notFilteredItemsList.each(function () {
            var item = jQuery(this);
            var itemId = item.attr("id");
            if (finalItemList.indexOf(itemId) == -1) {
                items = items.filter("[id!='" + itemId + "']");
            }

        });


    } else {
        var items = jQuery.tree.plugins.checkbox.get_checked(treeObj).filter(".leaf");
    }

    if (id == "ProviderTreeListTreeInfo") {


        var notFilteredItemsList = jQuery.tree.plugins.checkbox.get_checked(treeObj);

        var finalItemList = new Array();

        notFilteredItemsList.each(function () {
            var item = jQuery(this);
            var itemParentId = item.parents('li').attr("id");

            if (itemParentId == "undefined") {
                finalItemList.push(item.attr("id"));
            } else {
                if (notFilteredItemsList.filter("[id='" + itemParentId + "']").length == 0) {
                    finalItemList.push(item.attr("id"));
                }
            }
        });

        var items = notFilteredItemsList.clone();

        notFilteredItemsList.each(function () {
            var item = jQuery(this);
            var itemId = item.attr("id");
            if (finalItemList.indexOf(itemId) == -1) {
                items = items.filter("[id!='" + itemId + "']");
            }

        });


        Audiences = items;

    }
    // else {
    //    var items = jQuery.tree.plugins.checkbox.get_checked(treeObj).filter(".leaf");
    //    Audiences = items;
    //}


    var text = "";
    var title = "";
    var count = 0;
    items.each(function () {
        isDefult = false;
        var item = jQuery(this);
        var itemText = item.children().filter("a").text();
        if (text != "") {
            if (count < 3) {
                text += "," + itemText.trim();
            }
            title += "," + itemText.trim();
        }
        else {
            if (count < 3) {
                text = itemText.trim();
            }
            title = itemText.trim();
        }
        count++;
    });
    if (isDefult) {
        text = "";
        elem.addClass("default-values").text(text).attr('title', title);
    }
    else {
        elem.removeClass("default-values").text(text).attr('title', title);
    }

}

function alternateGrid(name) {
    var array = $("#" + name + " tbody tr");
    for (var i = 0; i < array.length; i++) {
        if ($(array[i]).is(":visible")) {
            if (i % 2 == 0) {
                $(array[i]).addClass("t-alt");
            } else {
                $(array[i]).removeClass("t-alt");

            }
        }
    }

}

function addToMVCGrid(gridName) {
    var grid = $("#" + gridName).data("tGrid");
    grid.addRow();

}

function clearValidations(name) {
    $("#" + name).find(".field-validation-error").hide();
}


function clone(obj) {
    if (obj == null || typeof (obj) != 'object')
        return obj;

    var temp = new obj.constructor();
    for (var key in obj)
        temp[key] = clone(obj[key]);

    return temp;
}

// Dialog region 

// store old method for later use
var oldcr = $.ui.dialog.prototype._create;
// add the two new options with default values
$.ui.dialog.prototype.options.showCloseButton = true;

// override the original _create method
$.ui.dialog.prototype._create = function () {
    oldcr.apply(this, arguments);

    try {
        if (typeof (this.options) != 'undefined' && typeof (this.options.buttons) != 'undefined' && this.options.showCloseButton) {
            var id = "#" + $(this).attr("element").attr("id");

            if (typeof ($(id) != 'undefined')) {
                var buttons = $(id).dialog("option", "buttons");
                if (typeof ($(buttons) != 'undefined')) {
                    if (!checkButtonDuplicate(buttons)) {
                        buttons[buttons.length] = {
                            text: close,
                            click: function () {
                                $(this).dialog('close');
                            },
                            "class": 'delete-button',
                            id: "close"
                        };
                        $(id).dialog("option", "buttons", buttons);
                    }
                }
            }
        }

    } catch (e) {
        //  

    }

};

function checkButtonDuplicate(buttons) {
    for (var i = 0; i < buttons.length; i++) {
        if (typeof (buttons[i].click) != 'undefined') {
            if (buttons[i].id == "close") {
                return true;
            }
        }

    }
    return false;
}

// End region 



function collapse(elem, divtog) {

    var containerElement = $("#" + divtog);

    containerElement.toggle();
    var headerElement = $(elem);

    if (headerElement.hasClass("close")) {
        headerElement.removeClass("close");
        headerElement.addClass("open");
    } else {
        headerElement.addClass("close");
        headerElement.removeClass("open");
    }
}


function getAmountVAT(amount, fieldValueId, VatAmountField, force) {
    var amountVal = parseInt(amount);
    var Vatamount = parseFloat(VatAmountPercentage) * parseInt(amount);
    var totalValue = 0;
    if (!force) {
        totalValue = Vatamount + amountVal;
        if (typeof ($("#" + VatAmountField)) != "undefined") {
            $("#" + VatAmountField).val(Vatamount.toFixed(2));
        }
    } else {
        totalValue = amountVal + parseInt($("#" + VatAmountField).val());
    }
    $('#' + fieldValueId).val(parseFloat(totalValue).toFixed(2));
}
function getAmountVATPayment(amount, fieldValueId, VatAmountField, force) {

    var amountVal = parseInt(amount);
    var Vatamount = amountVal / (1 + parseFloat(VatAmountPercentage));
    var theChange = amountVal - Vatamount;
    var totalValue = 0;
    if (!force) {
        totalValue = Vatamount;
        if (typeof ($("#" + VatAmountField)) != "undefined") {
            $("#" + VatAmountField).val(theChange.toFixed(2));
        }
    } else {
        totalValue = amountVal + parseInt($("#" + VatAmountField).val());
    }
    $('#' + fieldValueId).val(parseFloat(totalValue).toFixed(2));
}
function percentageCalculator(totalNumber, number) {

    return ((parseFloat(number) * 100) / parseFloat(totalNumber)).toFixed(0);
}

function ValidateRegex(Regex, value) {
    var regExp = new RegExp(Regex);
    return regExp.test(value);
}



$(function () {
    $('#adfMenuButton').click(function () {
        $('#adfMenu').fadeToggle('fast');
        setCookie('advMenu', 'adfMenu', 1);

        if ($('#sideBar').is(':visible')) {
            $('#sideBar').fadeOut();
        }

    })
    $('#adfMenu').mouseleave(function () {
        $(this).delay(250).fadeOut('fast')
    })
    $('.adfMenuGrid').mouseover(function () {
        $(this).css({
            'transition': '0.15s ease-out',
            'background-size': '120%'
        })
    })
    $('.adfMenuGrid').mouseleave(function () {
        $(this).css({
            'transition': '0.25s ease-in',
            'background-size': '100%'
        })
    })

    $('#adfMenuPin').click(function () {

        $('#adfMenu').fadeToggle();
        $('#sideBar').fadeToggle();

        if ($('#sideBar').is(':visible')) {
            setCookie('advMenu', 'sideBar', 1);
        } else {
            setCookie('advMenu', 'adfMenu', 1);
        }
    })

    $('#ADFalconuserName').click(function () {
        $('#ADFalconuserMenu').fadeToggle('fast')
    })
    $('#ADFalconuserMenu').mouseleave(function () {
        $(this).delay(250).fadeOut('fast')
    })

    $('#ADFalconuserMenu li').mouseover(function () {
        $(this).css({
            'transition': 'transform 0.15s ease-out',
            'transform': 'scale(1.05)'
        })
    })

    $('#ADFalconuserMenu li').mouseleave(function () {
        $(this).css({
            'transition': 'transform 0.15s ease-out',
            'transform': 'scale(1)'
        })
    })

    $('#sideBar').mouseover(function () {
        $(this).css({
            'transition': 'transform 0.15s ease-out',
            'transform': 'scale(1)',
            'opacity': '1',
            'filter': 'grayscale(0)',
            'box-shadow': '1px 0px 4px 2px rgba(0,0,0,0.2)'
        })
    })
    $('#sideBar').mouseleave(function () {
        $(this).css({
            'transition': 'transform 0.25s ease-in',
            'transform': 'scale(0.6)',
            'opacity': '0.4',
            'filter': 'grayscale(1)',
            'box-shadow': 'none'
        })
    })
    $('.sideBarGrid').mouseover(function () {
        $(this).css({
            'transition': '0.15s ease-out',
            'background-size': '115%'
        })
    })
    $('.sideBarGrid').mouseleave(function () {
        $(this).css({
            'transition': '0.25s ease-in',
            'background-size': '100%'
        })
    })
})


function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function ShowAdvSideMenu() {
    if ($("#adfMenuButton").length > 0) {
        var x = getCookie('advMenu');
        if (typeof (x) != "undefined" && x == "sideBar") {
            $('#sideBar').fadeIn();
            $('#adfMenu').fadeOut();
        }
    }
}

function ClickBtn(id) {
    if (typeof (id) != "undefined") {
        $(id).click();
    }
}


function CustomTimePeriod(days, hours, minutes, seconds, milliseconds) {
    this.Days = parseInt(days);
    this.Hours = parseInt(hours);
    this.Minutes = parseInt(minutes);
    this.Seconds = parseInt(seconds);
    this.Milliseconds = parseInt(milliseconds);
}

CustomTimePeriod.prototype.ToSeconds = function () {
    var TotalSeconds = this.Days;
    TotalSeconds = this.Hours + TotalSeconds * 24;
    TotalSeconds = this.Minutes + TotalSeconds * 60;
    TotalSeconds = this.Seconds + TotalSeconds * 60;

    return isNaN(TotalSeconds) ? 0 : TotalSeconds;
}


function stringToBoolean(string) {
    switch (string.toLowerCase().trim()) {
        case "true": case "yes": case "1": return true;
        case "false": case "no": case "0": case null: return false;
        default: return Boolean(string);
    }
}


function ToNumberArry(list) {

    if (list === null) {
        return null;
    }
    if (list === 'undefined') {
        return null;
    }
    if (list.length == 1) {
        return parseInt(list, 10);
    }
    return list.map(function (item) {
        return parseInt(item, 10);
    });
}



function Select2LimitSelection(id, limit) {

    $("#" + id).select2({
        maximumSelectionLength: limit
    });
}

function getCheckRadioValue(group) {
    var elements = $(group);
    for (var i = 0; i < elements.length; i++) {

        if (elements.eq(i).attr("checked") == "checked") {
            return $(elements.eq(i)).val();
        }
    }
}

