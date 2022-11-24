/////////////////////////////Targeting Start ///////////////////////////
var firstTimeLoadedTargeting = false;
var spinner = null;
var deviceIsExist = null;
var deviceTreeChanged = null;
var costModelWrapperBaseValue = null;
var BiddingStrategyFixedValue = true;
function getSpinnerObj() {
    if (spinner == null)
        spinner = getSpinner('ContentOfTabTargeting');
    return spinner;
}


function deleteGeoFencingTargeting(elem) {
	var grid = $("#GeoFencingsGrid").data("tGrid");
	var tr = $(elem).parents('tr');
	// delete the row
	grid.deleteRow(tr);

}

function onGeoFencingDataBound(e) {
	$('#GeoFencingsGrid').find("td:not(.t-last)").click(function (e) {
		e.stopPropagation();
	});
}
function validateConversions() {



	var returnerrorFee = true;
	$('.CountConversionEveryMessageText').each(function () {
		if ($(this).css('display') != 'none') {
			// element is not hidden

			returnerrorFee = false;

			return;
		}
	});


	if (!returnerrorFee) {
		showErrorMessage(CountConversionEveryMessageText, true);
		return false;
	}

	returnerrorFee = true;
	$('.ViewAttributionWindowMessageText').each(function () {
		if ($(this).css('display') != 'none') {
			// element is not hidden

			returnerrorFee = false;

			return;
		}
	});


	if (!returnerrorFee) {
		showErrorMessage(ViewAttributionWindowMessageText, true);
		return false;
	}






	returnerrorFee = true;

	countPrimary = CheckIsPimaryUniqValidation();

	if (countPrimary != -1 && (countPrimary >= 1) && $("#PrimaryConv").is(':checked')) {

		returnerrorFee = false;
	}


	if ((countPrimary == 0 && !$('#conversions').is(':visible'))) {
		showErrorMessage(PrimaryValidationWindowMessageOneText, true);
		returnerrorFee = true;
		return false;
	}
	if (!returnerrorFee) {
		showErrorMessage(PrimaryValidationWindowMessageText, true);
		return false;
	}









	return true;



}


function validationDynamic() {

	if (BiddingStrategyFixedValue) {
		return true;
	}
	var result = true;
	var result_CostElementDD = true;
	var result_Value = true;


	if ($("#BidOptimizationType").val() == '0') {
		$("#required_sign_BidType").show();
		result_CostElementDD = false;
	}


	if ($("#BidOptimizationValue").val() == '' || parseFloat($("#BidOptimizationValue").val()) <= 0) {
		$("#required_sign_BidOptimizationValue").show();
		result_Value = false;
	}


	if ($("#MaxBidPrice").val() == '' || parseFloat($("#MaxBidPrice").val()) <= 0) {
		$("#required_sign_MaxBidPrice").show();
		result_Value = false;
	}


	if (result_Value && result_CostElementDD) {
		result = true;
	} else {
		result = false;
	}

	return result;
}
function show_change_cost_model_warning() {
    $("#GeneralDialogText").text(change_cost_model_warning);
    $("#resultGeneralDialog").dialog("open");
}
function collapseLanguageTypes() {
    $("#LanguageTypes").toggle();

    var headerElement = $("#languageTypeTargeting .header");

    if (headerElement.hasClass("close")) {
        headerElement.removeClass("close");
        headerElement.addClass("open");
    } else {
        headerElement.addClass("close");
        headerElement.removeClass("open");
    }
}

function updateAudianceSeqmenttab() {

    if (AudianceSegmentCostModelTypeAllowed.indexOf(document.getElementById("CostModelWrapper").value) < 0) {
        $("#tab-padding").attr("style", "pointer-events:none;color:silver;");
        $("#AudienceSegmentSubItemMenu").addClass("disabledCustom");
    }
    else {
        $("#tab-padding").attr("style", "pointer-events:all;color:'';");
        $("#AudienceSegmentSubItemMenu").removeClass("disabledCustom");

    }
}


function updateConversiontab() {
/*
	if (loadDefaults == true || loadDefaults == "True") {
		$("#tab-padding").attr("style", "pointer-events:none;color:silver;");
		$("#ConversionSubItemMenu").addClass("disabledCustom");
	}
	else {
		$("#tab-padding").attr("style", "pointer-events:all;color:'';");
		$("#ConversionSubItemMenu").removeClass("disabledCustom");

	}*/
}

function localInitilize() {

    var confirmationDialogButtons = {};
    confirmationDialogButtons[yes_text] = function () {
        var container = $("#ModelInfo");
        container.empty();
        $(this).dialog("close");

        if (typeof deviceIsExist != "undefined" && deviceIsExist != null && deviceIsExist == true) {
            deviceIsExist = false;
        }

        changeIcons(selectedDeviceType[0], selectedDeviceType[1], selectedDeviceType[2]);

        if (typeof initiateDeviceTree == 'function') {
            initiateDeviceTree();
        }

        if (typeof deviceIsExist != "undefined" && deviceIsExist != null) {
            deviceIsExist = true;
        }


    };
    confirmationDialogButtons[no_text] = function () {
        $(this).dialog("close");
    };
    $("#confirmationDialogForm").dialog({
        autoOpen: false,
        //  height: 150,
        width: 400,
        modal: true,
        resizable: false,
        draggable: false,
        showCloseButton: false,

        buttons: confirmationDialogButtons,
        close: function () {
        }
    });


    if (typeof (localInitilize_TrackingEvents) != "undefined" && localInitilize_TrackingEvents != null) {
        localInitilize_TrackingEvents();
    }

    //addIPRules();
    if (is_client_locked) {
        showWarningMessage(locked_warning, true);
	}


	if (is_client_readOnly) {

		showWarningMessage(readOnly_warning, true);
	}
	onIPRangeRowDataBound();
	onGeoFencingDataBound();
    onURLTargetingRowDataBound();
    getSpinnerObj().showSpinner();

    $('#targetingForm').submit(function (e) {

        if (EventsConversionsSaveWhenSubmit()) {

            if (!targetingContinueFlag) {
                e.preventDefault();
                if (!isNew) {
               
                    var currentbidValue = parseFloat($('#Bid').val());
                    if (isNaN(currentbidValue)) {
                        currentbidValue = 0.0;
                    }
                    e.preventDefault();
                    var sendData = new Object();
                    sendData.bid = currentbidValue.toString();
                    sendData.campaignId = campaign_id;
                    sendData.adGroupId = ad_group_id;
                    var prams = $.toJSON(sendData);






                    if (targetingContinueFlagUpper) {
                      
                        //targetingContinueFlag = !continueFlag;


                        $.ajax({
                            url: ad_more_url,
                            dataType: "text json",
                            contentType: 'application/json; charset=utf-8',
                            type: "POST",
                            data: prams,
                            success: function (data) {
                                if (data.length > 0) {
                                    var html = "<table width='100%' cellspacing='0' cellpadding='4'><th>" + resource_Ads + "</th><th>" + resource_old_bid + "</th>";
                                    for (var i = 0; i < data.length; i++) {
                                        var item = data[i];
                                        html += "<tr><td> <input name='adLessBid' value='" + item.Id + "' type='hidden' /> " + item.Name + "</td><td>" + item.Bid + "</td></tr>";
                                    }
                                    html += "</table>";
                                    $('#moreThanBidList').html(html);
                                    //$('#bidDialogValue').text(sendData.bid + "$");
                                    $('#adsBid').val(sendData.bid);
                                    $('#minBidDescAdg1').html(resource_minBidDesc2);
                                    $('#bidDialog-form').dialog('open');
                                }
                                else {
                                    continueSubmit();
                                }
                            },
                            error: function (error) {
                                alert(error.responseText);
                            }
                        });
                    }

                    else {
                        $.ajax({
                            url: ad_less_url,
                            dataType: "text json",
                            contentType: 'application/json; charset=utf-8',
                            type: "POST",
                            data: prams,
                            success: function (data) {
                                if (data.length > 0) {
                                    var html = "<table width='100%' cellspacing='0' cellpadding='4'><th>" + resource_Ads + "</th><th>" + resource_old_bid + "</th>";
                                    for (var i = 0; i < data.length; i++) {
                                        var item = data[i];
                                        html += "<tr><td> <input name='adLessBid' value='" + item.Id + "' type='hidden' /> " + item.Name + "</td><td>" + item.Bid + "</td></tr>";
                                    }
                                    html += "</table>";
                                    $('#moreThanBidList').html(html);
                                    //$('#bidDialogValue').text(sendData.bid + "$");
                                    $('#adsBid').val(sendData.bid);

                                    $('#minBidDescAdg1').html(resource_minBidDesc1);
                                    $('#bidDialog-form').dialog('open');
                                }
                                else {
                                    continueSubmit();



                                }
                            },
                            error: function (error) {
                                alert(error.responseText);
                            }
                        });



                    }

                }
                else {

                    continueSubmit();
                }

            }
       
            else {
                targetingContinueFlag = !continueFlag;
            }

        } else {
            e.preventDefault();
        }
    });


    $('#AgeGroupId').change(updateDemographicsInfo);
    costModelWrapperBaseValue = $('#CostModelWrapper').val();
    $('#CostModelWrapper').change(function () {
        $("#confirmDeleteAllEvents").dialog("open");
    }
    );
    $('#Bid').change(function () {
        //call update bid
        updateBid(true);
        // update discount text
        var bidElem = $('#Bid');
        //var minBidElm = $('#minBid');
        var bid_value = parseFloat(bidElem.val());
        if (isNaN(bid_value)) {
            bid_value = 0.0;
        }
        var discounted_bid = calculate_discounted_value(bid_value, '#bidDiscounted', true);
    });
    getKeywordInfo();
    getDeviceTypeInfo();
    $('input[cancelEnter=true]').keypress(function (e) {
        return e.keyCode != 13;
    });

    $("#modelSearchInput").keyup(function (event) {
        if (event.keyCode == 13) {
            modelSearch();
        }
        return event.keyCode != 13;
    });
    var dialogButtons = {};
    dialogButtons[resource_select_command] = function () {
        var modelsInfo = getTreeText(device_tree_name);
        for (var i = 0; i < modelsInfo.length; i++) {
            var obj = modelsInfo[i];
            AddCustomTargetingId(obj.id, obj.dispalValue);
        }
        getmodelsInfo();
        getBid();
        $(this).dialog("close");
    };
    dialogButtons[resource_cancel_command] = function () {
        getmodelsInfo();
        $(this).dialog("close");
    };

    $("#dialog-form").dialog({
        autoOpen: false,
        //height: 400,
        width: 490,
        modal: true,
        resizable: false,
        draggable: false,
        showCloseButton: false,

        buttons: dialogButtons,
        close: function () {
            getmodelsInfo();
        }
    });
    var bidDialogButtons = {};
    bidDialogButtons[resource_yes_command] = function () {
        var isvalid = checkAdBidVlaue();
        if (!isvalid) {
            return;
        }
        var bidvalue = parseFloat($('#adsBid').val());
        var dialog = $(this);
        var sendData = new Object();
        var adIds = new Array();
        sendData.bid = bidvalue;
        sendData.campaignId = campaign_id;
        sendData.adGroupId = ad_group_id;
        $('[name=adLessBid]').each(function () { adIds[adIds.length] = $(this).val(); });
        sendData.adIds = adIds;
        var prams = $.toJSON(sendData);
        $.ajax({
            url: set_ads_bid_url,
            dataType: "text json",
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            data: prams,
            success: function (data) {
                continueSubmit();
                dialog.dialog("close");
            },
            error: function (error) {
                alert(error.responseText);
            }
        });


    };
    bidDialogButtons[resource_no_command] = function () {
        count = 0;
        targetingContinueFlag = false;
       
        $(this).dialog("close");

        if (targetingContinueFlagUpper) {
            continueSubmit();
        }
    };
    $("#bidDialog-form").dialog({
        autoOpen: false,
        //  height: 400,
        width: 450,
        modal: true,
        resizable: false,
        draggable: false,
        showCloseButton: false,
        buttons: bidDialogButtons,
        close: function () {
        }
    });
    if (typeof (localInitilize_CostElements) != "undefined" && localInitilize_CostElements != null) {
        localInitilize_CostElements();
    }


    updateTabs();
    updateDemographicsInfo();
    //updateDeviceCapabilitiesInfo();
    //firstTimeLoadedTargeting = true;
    //getBid();

    setTimeout(function () {
        firstTimeLoadedTargeting = true;
        getBid();
    }, 2000);

    $('#minBid').text(discount_value_string);
    $('#Bid').val(bid_value_string);



};
function checkAdBidVlaue() {
    var bidvalue = parseFloat($('#adsBid').val());
    var adGroupBid = parseFloat($('#Bid').val());
    if (isNaN(bidvalue) || (bidvalue < adGroupBid)) {
        $('#adBidErrorMsg').show();
        return false;
    }
    else {
        $('#adBidErrorMsg').hide();
        return true;
    }
}
function continueSubmit() {
    targetingContinueFlag = true;
    targetingContinueFlagUpper = false;
    $('#targetingForm').submit();
};

function collapseDeviceTypes() {
    $("#deviceTypes").toggle();

    var headerElement = $("#deviceTypeTargeting .header");

    if (headerElement.hasClass("close")) {
        headerElement.removeClass("close");
        headerElement.addClass("open");
    } else {
        headerElement.addClass("close");
        headerElement.removeClass("open");
    }
}


function changeIcons(img, text, value) {


    if (typeof defineDeviceVariables == 'function') {
        if (deviceIsExist == null && deviceTreeChanged == null) {
            defineDeviceVariables(deviceTreeFirstValue);
        }
    }

    if ($("input:checkbox[name=DeviceType]:checked").length == 1) {
        if ($("input:checkbox[name=DeviceType][value=" + value + "]:checked").length == 1) {

            return;
        }
    }

    var initiate = false;
    var container = $("#ModelInfo");
    var proceedChange = true;
    if (container.find("input").length != 0 || (typeof deviceIsExist != "undefined" && deviceIsExist != null && deviceIsExist == true)) {
        if (typeof deviceTreeChanged != "undefined" && deviceTreeChanged != null && deviceTreeChanged == false) {
            proceedChange = true;
            if (typeof initiateDeviceTree == 'function') {
                initiate = true;
            }
        } else {
            selectedDeviceType[0] = img;
            selectedDeviceType[1] = text;
            selectedDeviceType[2] = value;
            $('#confirmationDialogForm').dialog('open');
            proceedChange = false;
        }
    }

    if (proceedChange) {
        var lowerText = text.toLowerCase();

        if (!$(img).hasClass("clicked")) {
            $(img).addClass("clicked");
        }
        if ($("input:checkbox[name=DeviceType][value=" + value + "]:checked").length == 1) {
            $("input:checkbox[name=DeviceType][value=" + value + "]").attr("checked", false);
            $(img).removeClass(lowerText + "enabled");
            $(img).addClass(lowerText + "disabled");

        } else {

            $("input:checkbox[name=DeviceType][value=" + value + "]").attr("checked", true);
            $(img).addClass(lowerText + "enabled");
            $(img).removeClass(lowerText + "disabled");

        }
        getDeviceTypeInfo();
        if (initiate) {
            initiateDeviceTree();
        }
    }
    return;
}

function removeClickedClass(img) {
    $(img).removeClass("clicked");
}

function changePlatformStatus(item) {
    var element = $(item);
    var platformId = element.attr("customvalue");

    var insElement = element.find("ins");

    if (insElement.hasClass("checked")) {
        insElement.removeClass("checked");
        insElement.addClass("unchecked");
        element.removeClass("checked");
        element.addClass("unchecked");
        $("#Platforms").val($("#Platforms").val().replace("&" + platformId, ""));

    }
    else {
        insElement.removeClass("unchecked");
        insElement.addClass("checked");
        element.removeClass("unchecked");
        element.addClass("checked");
        $("#Platforms").val($("#Platforms").val() + "&" + platformId);
    }

    updatePlatformInfo();
}

function AddCustomTargetingId(ids, dispalValue) {
    var idsArray = ids.split('#');
    var id = idsArray[0];
    var manufacturerId = idsArray[1];
    var platformId = idsArray[2];
    var container = $("#ModelInfo");
    if ((id == '0') || (!CheckTargetingFound(container, id))) {
        var html = "<a ModelId=" + id + " ManufacturerId=" + manufacturerId + " PlatformId=" + platformId + " href='javaScript:void(0);'>" + dispalValue + "<span  onclick='RemoveCustomTargeting(this);'></span>";
        html += "<input id='ModelId' name='ModelId' value='" + id + "' type='hidden' /></a>";
        container.append(html);
    }
};
function RemoveCustomTargeting(elemspan) {
    var elem = $(elemspan).parent();
    var id = elem.attr("ModelId");
    $("#ModelInfo").find('[ModelId="' + id + '"]').show();
    elem.remove();
    getmodelsInfo();
    if (typeof (getBid) != 'undefined') {
        getBid();
    }
}
function CheckTargetingFound(elem, id) {
    return elem.find('[ModelId="' + id + '"]').length > 0;
}


function modelSearch() {
    getSpinnerObj().showSpinner();
    var modelSearchQuery = $('#modelSearchInput').val();
    var final_search_url = search_device_url + modelSearchQuery + "&deviceTypeId=" + ($("input[name=DeviceType]:checked").length != 1 ? "" : $("input[name=DeviceType]:checked").val());

    InitTree(device_tree_name, final_search_url, function () {
        $('#' + device_tree_name + 'treeSearch').val("");
        getSpinnerObj().hideSpinner();
        var treeObj = $.tree.reference('#' + device_tree_name + 'Tree');
        treeObj.open_all();
        $("#" + device_tree_name + "Tree").find('li').each(function () {
            $.tree.plugins.checkbox.check(this);
        });
        $("#dialog-form").dialog("open");
    });
}


function filterOperaters() {

    loadOperatorsFlag = false;
    if (isLoading)
        return;
    if (!eval(geographics_tree_name + "Loaded"))
        return;

    var isAll = $('#GeographicTargetingIsAll').val();
    var geoLeafsData = '';
    var geoParameter = '';

    if (isAll != '1') {
        geoLeafsData = getTreeData('Geographies');
        geoParameter = "Geographies=" + geoLeafsData;
        if (geoParameter.Geographies == '') {
            geoParameter = "Geographies=null";
        }

        var geoSeletectedLocations = new Array();

        // To get selected locations including parents, not only leafs
        jQuery.tree.plugins.checkbox.get_checkedOrUndeterminded(jQuery.tree.reference('#GeographiesTree')).each(function (index) {
            geoSeletectedLocations.push(this.id);
        });

        if (geoSeletectedLocations.length != 0) {
            var currentSelectValues = $(getTreeData('Operators').split(','));
            var latestSelectedItems = '';
            var latestSelectedItemsArray = new Array();

            var tree = $("#OperatorsTree");
            for (var i = 0; i < currentSelectValues.length; i++) {
                var Idstr = currentSelectValues[i];
                if (Idstr != '') {
                    match = tree.find('li[id=' + currentSelectValues[i] + ']');
                    var countryId = match.parents('li').attr('id');

                    if (geoSeletectedLocations.indexOf(countryId) > -1) {
                        latestSelectedItemsArray.push(match.attr("id"));
                    }

                }
            }
        } else {
            var latestSelectedItemsArray = $(getTreeData('Operators').split(','));
        }
    } else {
        var latestSelectedItemsArray = $(getTreeData('Operators').split(','));
    }

    setSelectedOperators = function () {

    };

    disableSaveButtonForAjax = true;
    function reSelectItems() {
        var tree = $("#OperatorsTree");
        for (var i = 0; i < latestSelectedItemsArray.length; i++) {
            var id = latestSelectedItemsArray[i];
            if (id != '') {
                match = tree.find('li[id=' + latestSelectedItemsArray[i] + ']');
                match.each(function () { $.tree.plugins.checkbox.check(this, false); });
            }
        }
        disableSaveButtonForAjax = false;
    }

    InitTree('Operators', operater_tree_url, reSelectItems, undefined, geoParameter);
}



function getBid(isLoadHandler, showSpinner) {

    if (!firstTimeLoadedTargeting) {
        return;
    }
    if (typeof (isLoadHandler) == "undefined" || isLoadHandler == false) {
        checkTreeStatus();
    }
    if (typeof (showSpinner) == "undefined") {
        showSpinner = true;
    }
    if (isLoading)
        return;
    if (showSpinner) {
        getSpinnerObj().showSpinner();
    }
    var geographicTargetingIsAll = $('#GeographicTargetingIsAll').val();
    var operatorTargetingIsAll = $('#OperatorTargetingIsAll').val();
    var sendData = new Object();
    sendData.Keywords = getKeywords();
    if (operatorTargetingIsAll == '1' || operatorTargetingIsAll == '4') {
        sendData.Operators = '';
    }
    else {
        if (operatorTargetingIsAll == '3') {
            sendData.Operators = wifiOperatorId;
        }
        else {
            sendData.Operators = getTreeData('Operators');
        }
    }
    if (geographicTargetingIsAll == '1') {
        sendData.Geographies = '';
    }
    else {
        sendData.Geographies = getTreeData('Geographies');
    }
    sendData.Demographic = getdemographicsInfo();
    sendData.DeviceTargetingTypeId = $('#DeviceTargetingTypeId').val();
    sendData.ActionTypeId = ad_action_type_id;
    sendData.AdTypeId = adtypeId;


    if (!has_device_targeting) {
        if (sendData.DeviceTargetingTypeId == '3') {
            var container = $("#ModelInfo");
            sendData.Platforms = container.find('[PlatformId]').map(function () { return $(this).attr("PlatformId") }).get().join(',');
            sendData.Manufacturers = container.find('[ManufacturerId]').map(function () { return $(this).attr("ManufacturerId") }).get().join(',');
        } else {
            sendData.Manufacturers = getTreeData('Manufacturers');

            sendData.Platforms = $('#Platforms').val().replace(new RegExp("&", "g"), ",");
        }
        var DeviceCapabilities = "";
        $('[name=DeviceCapability]:checked').each(function () {
            DeviceCapabilities += ',' + ($(this).attr('customValue'));
        });
        sendData.DeviceCapabilities = DeviceCapabilities;

        var ExcludeDeviceCapability = "";
        $('[name=ExcludeDeviceCapability]:checked').each(function () {
            ExcludeDeviceCapability += ',' + ($(this).attr('customValue'));
        });
        sendData.ExcludeDeviceCapability = ExcludeDeviceCapability;
    } else {
        sendData.Manufacturers = getTreeData('Devices', 'Manufacturers', true);
        sendData.Platforms = getTreeData('Devices', 'Platforms', true);
        sendData.DeviceCapabilities = "";
    }
    //sendData.Operators = getTreeData('Operators');
    prams = $.toJSON(sendData);
    $.ajax({
        url: get_bid_url,
        //dataType: "json",
        dataType: "text json",
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        data: prams,
        success: function (data) {
            minBid = $.parseJSON(data);
            updateBid();
            if (showSpinner) {
                getSpinnerObj().hideSpinner();
            }

        },
        error: function (error) {
            // alert(error.responseText);
            if (showSpinner) {
                getSpinnerObj().hideSpinner();
            }
        }
    });
}

function checkStatus(IsContinue) {

    $("#IsContinue").val(IsContinue);
    var deviceCapabilitieValues = "";
    $('[name=DeviceCapability]:checked').each(function () {
        deviceCapabilitieValues += '&' + ($(this).attr('customValue'));
    });
    $('#DeviceCapabilities').val(deviceCapabilitieValues);


    var excludedeviceCapabilitieValues = "";
    $('[name=ExcludeDeviceCapability]:checked').each(function () {
        excludedeviceCapabilitieValues += '&' + ($(this).attr('customValue'));
    });
    $('#ExcludeDeviceCapability').val(excludedeviceCapabilitieValues);

    getIPRanges();
	getURLTargeting();
	getGeoFencingTargeting();
    getDevicetargeting();
    var result = true;
    clearErrorMessage();

    if (loadOperatorsFlag) {
        filterOperaters();
    }

	result = checkBid() && result && !disableSaveButtonForAjax && BudgetChanged() && validateConversions();
    if (has_device_targeting) {
        result = checkDevices() && result;
    }
    if (typeof getJsonRules === "function") {
        getJsonRules();
	}

	if (result)
	{
		count = 0;
	}

    return result;
}

function getDevicetargeting() {
    var platforms = $("#DevicesTree").find("[key='Platforms']");
    var chosenplatforms = [];
    if (typeof platforms != "undefined") {

        for (var i = 0 ; i < platforms.length; i++) {
            var platFormIdClass = platforms.eq(i).children().eq(0).attr("class");;
            var platFormId = platforms.eq(i).attr("id");
            //
            if (platFormIdClass) {
                if ((platFormIdClass.indexOf("checked") > -1 || platFormIdClass == 'undetermined') && platFormIdClass.indexOf("unchecked") < 0) {
                    //

                    var platForm = getPlatForm(platFormId);

                    if (platFormIdClass.indexOf("checked") > -1 && platFormIdClass.indexOf("unchecked") < 0)
                        platForm.isAll = true;

                    var allManufacturers = platforms.eq(i).children().eq(1).find("[Key='Manufacturers']")

                    if (typeof allManufacturers != "undefined")
                        for (var j = 0 ; j < allManufacturers.length; j++) {

                            manufacturerCalss = allManufacturers.eq(j).children().eq(0).attr("class");
                            if (manufacturerCalss) {
                                if ((manufacturerCalss.indexOf("checked") > -1 || manufacturerCalss == 'undetermined') && manufacturerCalss.indexOf("unchecked") < 0) {
                                    //
                                    var Manufacturer = getManu(allManufacturers.eq(j).attr("id"));

                                    if (manufacturerCalss.indexOf("checked") > -1 && manufacturerCalss.indexOf("unchecked") < 0)
                                        Manufacturer.isAll = true;

                                    var Devices = allManufacturers.eq(j).find("[Key='Models']")


                                    for (var v = 0 ; v < Devices.length; v++) {

                                        if (Devices.eq(v).children().eq(0).attr("class")) {
                                            if (Devices.eq(v).children().eq(0).attr("class").indexOf("checked") > -1 && Devices.eq(v).children().eq(0).attr("class").indexOf("unchecked") < 0) {

                                                var DeviceId = Devices.eq(v).attr("id");

                                                Manufacturer.Devices.push(DeviceId);

                                            }
                                        }

                                    }
                                    platForm.Manu.push(Manufacturer);

                                }
                            }

                        }

                    chosenplatforms.push(platForm);
                }
            }

        }



    }
    // 
    $('#platfromTree').val(jQuery.toJSON(chosenplatforms));
}
function getPlatForm(Id) {
    // 

    var platform = PlatForm();
    platform.Id = Id;
    return platform;

}

function getManu(Id) {
    // 

    var Manu = Manufacturer();
    Manu.Id = Id;
    return Manu;

}

function PlatForm() {
    var platform = new Object();

    platform.Id = -1;
    platform.isAll = false;
    platform.Manu = [];
    return platform;
}
function Manufacturer() {
    var manufacturer = new Object();
    manufacturer.Id = -1;
    manufacturer.isAll = false;
    manufacturer.Devices = [];
    return manufacturer;
}

function updateUrlInfo(isLoad) {
    var grid = $("#UrlGrid tbody");

    if (isLoad == true) {
        var rowsData = grid.find("tr:not(.t-no-data)");
    }
    else {
        var rowsData = grid.find("tr:visible:not(.t-no-data)");
    }

    if ((rowsData.length) > 0) {
        $("#urlInfo").attr("title", specificUrlText).text(specificUrlText).removeClass("default-values");
    }
    else {
        $("#urlInfo").attr("title", defaultText).text(defaultText).addClass("default-values");
    }
}
function updateTabs() {

    var geographicTab = $('a[tabindexgeographic="' + geographics_type_id + '"]');
    changeTab(geographicTab[0], 'geographic', 'GeographicTargetingIsAll', geographics_tree_name, false);

    var operaterTab = $('a[tabindexoperater="' + operater_type_id + '"]');
    changeTab(operaterTab[0], 'operater', 'OperatorTargetingIsAll', operater_tree_name, false);

    //if is WIFI Only then update the text 
    if ($('#OperatorTargetingIsAll').val() == '3') {
        $('#OperatorsTreeInfo').removeClass("default-values").text(wifiOnlyText).attr('title', wifiOnlyText);
    }
    //if is specific IP Ranges  then update the text 
    if ($('#OperatorTargetingIsAll').val() == '4') {
        $('#OperatorsTreeInfo').removeClass("default-values").text(specificIPRangesText).attr('title', specificIPRangesText);
    }
    if (has_device_targeting) {
        $('#DeviceTargetingTypeId').val('4');
    } else {
        if (device_type_id == 3) {
            var deviceTab = $('a[tabindexdevice="' + device_type_id + '"]');
            changeTab(deviceTab[0], 'Device', 'DeviceTargetingTypeId', "DevicesTreeInfo");
        } else {
            var deviceTab = $('a[tabindexdevice="' + device_type_id + '"]');
            changeTab(deviceTab[0], 'Device', 'DeviceTargetingTypeId');
            updateDeviceCapabilitiesInfo();
            updatePlatformInfo();
        }
    }
}


function updateDeviceInfo() {

};
function getdemographicsInfo() {
    var returnresult = null;
    var genderRbtn = jQuery("input[name='Gender']:checked");
    var ageGroup = jQuery('#AgeGroupId');
    var genderValue = genderRbtn.val();
    var ageGroupValue = ageGroup.val();
    if ((genderValue != '0') || (ageGroupValue != '0')) {
        returnresult = 1;
    }
    return returnresult;
}

function checkDevices() {
    var ids = getTreeData('Devices', 'Models');
    if (ids == "") {
        showErrorMessage(noDeviceErrMsg, true);
        return false;
    }
    return true;
}
function checkBid() {

	if (BiddingStrategyFixedValue) {
		$("#BidOptimizationValue").val("0.01");
		$("#MaxBidPrice").val("0.01");
		if (!minBid) {
			showErrorMessage(minBidErrMsg, true);
			return false;
		}
		var minBidValue = 0;
		var bidElem = jQuery('#Bid');
		var minBidElmValue = parseFloat(bidElem.val());
		if (isNaN(minBidElmValue)) {
			minBidElmValue = 0.0;
		}
		var costModel = parseInt(jQuery('#CostModelWrapper').val());
		if (isNaN(costModel)) {
			costModel = 1;
		}
		minBidValue = parseFloat(minBid[costModel]);
		if (minBidElmValue < minBidValue || minBidElmValue <= 0) {
			if (minBidElmValue <= 0) {
				showErrorMessage(minBidMsg, true);
				return false;
			}
			showErrorMessage(minBidErrMsg, true);
			return false;
		}

		return true;
	}
	else {
		$("#Bid").val("0.01");
		var result = validationDynamic();


		if (!result) {
			showErrorMessage(DynamicBiddingBiddingError, true);
			return result;
		}
	}

		var returnerrorFee = true;
		$('.errorEnteredFees').each(function () {
			if ($(this).css('display') != 'none') {
				// element is not hidden

				returnerrorFee = false;

				return;
			}
		});


		if (!returnerrorFee) {
			showErrorMessage(UpdateFeesErrorMsg, true);
			return false;
		}

		return true;

	
}
function updateBid(allowGreater) {
    if (typeof (allowGreater) == "undefined")
        allowGreater = false;
    var minBidValue = 0;
    var bidElem = jQuery('#Bid');
    var minBidElmValue = parseFloat(bidElem.val());
    if (isNaN(minBidElmValue)) {
        minBidElmValue = 0.0;
    }
    var costModel = parseInt(jQuery('#CostModelWrapper').val());
    if (isNaN(costModel)) {
        //toDO OSaleh to handle this issue
        costModel = 1;
    }
    minBidValue = minBid[costModel];
    $("#minBidValue").text(minBidValue.toFixed(2));
    if ((minBidValue.toFixed(2))<=0)
    {

    $("#minBidValueDiv").hide();
}
else
	{
		if (BiddingStrategyFixedValue)
$("#minBidValueDiv").show();

}
	var returnerrorFee = true;
	$('.errorEnteredFees').each(function () {
		if ($(this).css('display') != 'none') {
			// element is not hidden

			returnerrorFee = false;

			return;
		}
	});


	if (!returnerrorFee) {
		showErrorMessage(UpdateFeesErrorMsg, true);
		return false;
	}

    calculate_discounted_value(minBidValue, '#micBidDiscounted');
};
function treeLoadedHandler() {
    if (isLoading)
        return;


    if (treeFalgs.length > 0) {
        Allcompleted = true;
        for (var i = 0; i < treeFalgs.length; i++) {
            if (treeFalgs[i].status == false) {
                Allcompleted = false;
                break;
            }
        }

        if (Allcompleted) {
            filterOperaters();
            getBid(true);
        }
    }
}

function getIPRanges() {
    var grid = $('#IPRangesGrid').data("tGrid");
    if (grid == null)
        return;
    if (grid.changeLog.dirty()) {
        var inserted = new Array();
        var deletedIPRangeIds = '';
        for (var i = 0; i < grid.changeLog.inserted.length; i++) {
            inserted[inserted.length] = grid.changeLog.inserted[i];
        }

        for (i = 0; i < grid.changeLog.deleted.length; i++) {
            if (typeof (grid.changeLog.deleted[i]) != "undefined") {
                deletedIPRangeIds += '&' + grid.changeLog.deleted[i].ID;
            }
        }
        $('#DeletedIPRanges').val(deletedIPRangeIds);
        $('#InsertedIPRanges').val(jQuery.toJSON(inserted));//;JSON.stringify(inserted));
    } else {
        $('#DeletedIPRanges').val("");
        $('#InsertedIPRanges').val("");
    }
}
function onIPRangeRowDataBound(e) {
    $('#IPRangesGrid').find("td:not(.t-last)").click(function (e) {
        e.stopPropagation();
    });
}

function getGeoFencingTargeting() {
	var grid = $('#GeoFencingsGrid').data("tGrid");
	if (grid == null)
		return;

	if (grid.changeLog.dirty()) {
		var inserted = new Array();
		var deletedGeoFencingIds = '';
		for (var i = 0; i < grid.changeLog.inserted.length; i++) {
			inserted[inserted.length] = grid.changeLog.inserted[i];
		}

		for (i = 0; i < grid.changeLog.deleted.length; i++) {
			if (typeof (grid.changeLog.deleted[i]) != "undefined") {
				deletedGeoFencingIds += '&' + grid.changeLog.deleted[i].ID;
			}
		}

		$('#DeletedGeoFencing').val(deletedGeoFencingIds);
		$('#InsertedGeoFencing').val(jQuery.toJSON(inserted));//;JSON.stringify(inserted));
	} else {
		$('#DeletedGeoFencing').val("");
		$('#InsertedGeoFencing').val("");
	}
}

function Grid_onError(args) {
    if (args.textStatus == "modelstateerror" && args.modelState) {
        var message = "Errors:\n";
        $.each(args.modelState, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () { message += this + "\n"; });
            }
        });
        args.preventDefault();
        alert(message);
    }
}



function deleteIPTargeting(elem) {
    var grid = $("#IPRangesGrid").data("tGrid");
    var tr = $(elem).parents('tr');
    // delete the row
    grid.deleteRow(tr);
}

function addIPRules() {
    addValidationRules_IP();
    addIPRule('StartRange');
    addIPRule('EndRange');
}


function onURLTargetingRowDataBound(e) {
    $('#Grid').find("td:not(.t-last)").click(function (e) {
        e.stopPropagation();
    });
}


function getURLTargeting() {
    var grid = $('#UrlGrid').data("tGrid");
    if (grid == null)
        return;
    if (grid.changeLog.dirty()) {
        var inserted = new Array();
        var deletedURLTargetingIds = '';
        for (var i = 0; i < grid.changeLog.inserted.length; i++) {
            if (grid.changeLog.inserted[i].URL != null && grid.changeLog.inserted[i].URL != '') {
                inserted[inserted.length] = grid.changeLog.inserted[i];
            }
        }

        for (i = 0; i < grid.changeLog.deleted.length; i++) {
            if (typeof (grid.changeLog.deleted[i]) != "undefined") {
                deletedURLTargetingIds += '&' + grid.changeLog.deleted[i].ID;
            }
        }
        $('#DeletedURLTargeting').val(deletedURLTargetingIds);
        $('#InsertedURLTargeting').val(jQuery.toJSON(inserted));//;JSON.stringify(inserted));
    } else {
        $('#DeletedURLTargeting').val("");
        $('#InsertedURLTargeting').val("");
    }
}

/////////////////////////////Targeting End /////////////////////////////
/////////////////////////////Campaign Info Start /////////////////////////////

function getKeywordInfo() {
    var isDefult = true;
    var elem = jQuery("#keywordsInfo");
    var items = jQuery('#Keywords').find("[keywordId]");
    var text = "";
    var title = "";
    var count = 0;
    items.each(function () {
        isDefult = false;
        var item = jQuery(this);
        if (text != "") {
            if (count < maxCount) {
                text += "," + item.text();
            }
            title += "," + item.text();
        }
        else {
            if (count < maxCount) {
                text = item.text();
            }
            title = item.text();
        }
        count++;
    });
    if (isDefult) {
        text = defaultText;
        elem.addClass("default-values").text(text).attr('title', title);
    }
    else {
        elem.removeClass("default-values").text(text).attr('title', title);
    }
};
function getmodelsInfo() {
    var isDefult = true;
    var elem = jQuery("#deviceTargetingInfo");
    var items = jQuery('#ModelInfo').find("[ModelId]");
    var text = "";
    var title = "";
    var count = 0;
    items.each(function () {
        isDefult = false;
        var item = jQuery(this);
        if (text != "") {
            if (count < maxCount) {
                text += "," + item.text().trim();
            }
            title += "," + item.text().trim();
        }
        else {
            if (count < maxCount) {
                text = item.text().trim();
            }
            title = item.text().trim();
        }
        count++;
    });
    if (isDefult) {
        text = defaultText;
        elem.addClass("default-values").text(text).attr('title', title);
    }
    else {
        elem.removeClass("default-values").text(text).attr('title', title);
    }
}

function getDeviceTypeInfo() {
    var infoLabel = $("#deviceTypeInfo");

    if (infoLabel.length != 0) {
        var items = $("input:checkbox[name=DeviceType]:checked");

        var isDefult = true;
        var text = "";
        if (items.length != 2) {
            isDefult = false;
            items.each(function () {
                var item = jQuery(this);
                text += item.attr("customvalue");
            });
        }

        if (isDefult) {
            text = defaultText;
            infoLabel.addClass("default-values").text(text).attr('title', text);
        }
        else {
            infoLabel.removeClass("default-values").text(text).attr('title', text);
        }

    }
}

function clearTreeSummary(treeName, isAll) {
    if (treeName == "DevicesTreeInfo") {
        getmodelsInfo();
        return;
    }
    if (treeName == "deviceCapabilities") {
        updateDeviceCapabilitiesInfo();
        return;
    }
    if (treeName == "Platforms") {
        updatePlatformInfo();
        return;
    }

    id = treeName + "Tree";

    if (isAll) {
        if ((id == 'ManufacturersTree') || (id == 'DevicesTree') || (id == "deviceTargetingInfoTree")) {
            id = 'deviceTargeting';
        }
        var elem = jQuery("#" + id + "Info");
        elem.addClass("default-values").text(defaultText).attr('title', defaultText);
    }
    else {
        var treeObj = jQuery.tree.reference('#' + id);
        treeHnadler(treeObj);
    }
}
function treeHnadler(treeObj) {
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
    if (id == "AudiencesTreeInfo") {


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
    var text = "";
    var title = "";
    var count = 0;
    items.each(function () {
        isDefult = false;
        var item = jQuery(this);
        var itemText = item.children().filter("a").text();
        if (text != "") {
            if (count < maxCount) {
                text += "," + itemText;
            }
            title += "," + itemText;
        }
        else {
            if (count < maxCount) {
                text = itemText;
            }
            title = itemText;
        }
        count++;
    });
    if (isDefult) {
        text = defaultText;
        elem.addClass("default-values").text(text).attr('title', title);
    }
    else {
        elem.removeClass("default-values").text(text).attr('title', title);
    }
    if (id != "AudiencesTreeInfo") {
        getBid(undefined, false);
    }
}
function keywordHnadler(text, isNew) {
    getKeywordInfo();
}
function addCampaignInfo(container, newValue) {
    newValue = jQuery.trim(newValue);
    var newText = "";
    var oldText = jQuery.trim(container.text());
    if (typeof (oldText) == "undefined") {
        oldText = "";
    }
    if (oldText.toString() == defaultText.toString()) {
        oldText = '';
    }
    var strs = oldText.split(',');
    if (strs.length < 3) {
        if (oldText != "") {
            newText = oldText + "," + newValue;
        }
        else {
            newText = newValue;
        }
        container.removeClass("default-values").text(newText).attr('title', newText); //.attr("innerText", newText);
    }

}

function removeCampaignInfo(container, valueToRemoved) {
    valueToRemoved = jQuery.trim(valueToRemoved);
    var oldText = jQuery.trim(container.text());
    if (typeof (oldText) == "undefined") {
        oldText = "";
    }
    var strs = oldText.split(',');
    var newText = "";
    var isFirst = true;
    for (var i = 0; i < strs.length; i++) {
        if ((strs[i] != "") && (strs[i] != valueToRemoved)) {
            if (isFirst) {
                newText += strs[i];
                isFirst = false;
            }
            else {
                newText += "," + strs[i];
            }
        }
    }
    if (newText == "") {
        container.addClass("default-values").text(defaultText).attr('title', newText);
    }
    else {
        container.text(newText).attr('title', newText);
    }

}
function updateDemographicsInfo() {
    var isDefult = true;
    var newText = "";
    var demographicsInfoObj = jQuery('#demographicsInfo');
    var genderRbtn = jQuery("input[name='Gender']:checked");
    var ageGroup = jQuery('#AgeGroupId');
    var genderValue = genderRbtn.val();
    var ageGroupValue = ageGroup.val();
    if (genderValue != '0') {
        newText = genderRbtn.attr('customText');
        isDefult = false;
    }
    if (ageGroupValue != '0') {
        if (isDefult) {
            newText = ageGroup.find("option:selected").text();
        }
        else {
            newText += "," + ageGroup.find("option:selected").text();
        }
        isDefult = false;
    }
    if (isDefult) {
        newText = defaultText;
        demographicsInfoObj.addClass("default-values").text(defaultText).attr('title', newText);
    }
    else {
        demographicsInfoObj.removeClass("default-values").text(newText).attr('title', newText);
    }
    getBid();
}

function updatePlatformInfo() {

    var isPlatformTargeting = $('#DeviceTargetingTypeId').val() == "1" ? true : false;

    if (isPlatformTargeting) {
        var isDefult = true;
        var newText = "";
        var newTitle = "";
        var count = 0;
        var deviceTargetingInfoObj = jQuery('#deviceTargetingInfo');

        var platformContainer = $("#platformtargeting");
        var checkedItems = platformContainer.find("label[class='checked']");

        if (checkedItems.length != 0) {
            isDefult = false;

            checkedItems.each(function () {
                var item = $(this);
                var itemText = item.text().trim();
                if (newText != "") {
                    if (count < maxCount) {
                        newText += ", " + itemText;
                    }
                    newTitle += ", " + itemText;
                }
                else {
                    if (count < maxCount) {
                        newText = itemText;
                    }
                    newTitle += itemText;
                }
                count++;
            });
        }

        if (isDefult) {
            newText = defaultText;
            deviceTargetingInfoObj.addClass("default-values").text(newText).attr('title', newText);
        }
        else {
            deviceTargetingInfoObj.removeClass("default-values").text(newText).attr('title', newTitle);
        }
    }
    getBid();
}

function updateDeviceCapabilitiesInfo() {
    var isDefult = true;
    var newText = "";
    var newTitle = "";
    var count = 0;

    var deviceTargetingInfoObj = jQuery('#deviceTargetingInfo');
    jQuery('[name=DeviceCapability]:checked').each(function () {
        var item = jQuery(this);
        isDefult = false;
        if (newText != "") {
            if (count < maxCount) {
                newText += "," + item.parent().parent().find('.check-box-text').text().trim();
            }
            newTitle += "," + item.parent().parent().find('.check-box-text').text().trim();
        }
        else {
            if (count < maxCount) {
                newText = item.parent().parent().find('.check-box-text').text().trim();
            }
            newTitle += item.parent().parent().find('.check-box-text').text().trim();
        }
        count++;
    });
    if (isDefult) {
        newText = defaultText;
        deviceTargetingInfoObj.addClass("default-values").text(defaultText).attr('title', defaultText);
    } else {
        deviceTargetingInfoObj.removeClass("default-values").text(newText).attr('title', newTitle);
    }
    getBid(false, true);
}



function loadTrackingEvents() {

    $.ajax({
        type: 'POST',
        url: checkDeleteTrackingEventUrl,
        cache: false,
        data: { checkStandards: false, costModelWrapperId: $("#CostModelWrapper").val() },
        success: function (data) {

            show_change_cost_model_warning();
            updateBid();
            costModelWrapperBaseValue = $("#CostModelWrapper").val();
		

			loadDefaults = costModelWrapperBaseValue == costModelArapperValue ? false : true;
			updateConversiontab();
        },
        error: function (data) {
            $("#GeneralDialogText").text(data.responseText);
            $("#resultGeneralDialog").dialog("open");
            $("#CostModelWrapper").val(costModelWrapperBaseValue);
        },
        traditional: true

    });
}

/////////////////////////////Campaign Info End /////////////////////////////