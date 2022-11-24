

/////////////////////////////Targeting Start ///////////////////////////
var firstTimeLoadedTargeting = false;
var spinner = null;
var deviceIsExist = null;
var deviceTreeChanged = null;
var costModelWrapperBaseValue = null;
//var inserted = [];
//var deleted = [];
var CheckAppSiteCompatibleWithCampaignUrl;
var CampaignBidConfigNotCompleted = true;
var BiddingStrategyFixedValue = true;



function getSpinnerObj() {
    if (spinner == null)
        spinner = getSpinner('ContentOfTabTargeting');
    return spinner;
}
function show_change_cost_model_warning() {
    $("#GeneralDialogText").text(change_cost_model_warning);
    $("#resultGeneralDialog").dialog("open");
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
        //height: 150,
        width: 400,
        showCloseButton: false,

        modal: true,
        resizable: false,
        draggable: false,
        buttons: confirmationDialogButtons,
        close: function () {
        }
    });



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
                    var sendData = new Object();
                    sendData.bid = currentbidValue.toString();
                    sendData.campaignId = campaign_id;
                    sendData.adGroupId = ad_group_id;
                    var prams = $.toJSON(sendData);


           


                    if (targetingContinueFlagUpper) {
                       // targetingContinueFlagUpper = false;



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
                                    return false;
                                }
                                else {
                                    targetingContinueSubmit();


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
                                    return false;
                                }
                                else {
                                    targetingContinueSubmit();


                                }
                            },
                            error: function (error) {
                                alert(error.responseText);
                            }
                        });
                    }



                }
                

                else {
                    targetingContinueSubmit();

                }
            }


            else {


       

               // else {
                    if (!costElementContinueFlag) {

                        if (costModelWrapperBaseValue != costModelArapperValue || isPricingModelChangedFromPublic == "true" || isNew) {
                            e.preventDefault();
                            showCostElementDialog();
                            return false;
                        }
                    }

                    this;


                    if (CampaignBidConfigNotCompleted) {
                        e.preventDefault();
                        CheckAppSiteNotCompatibleWithCampaign();

                    }

                //}

            }
        } else {
            e.preventDefault();
        }

    });



    function CheckAppSiteNotCompatibleWithCampaign() {

        var grid = $("#CampaignBidConfigList").data("tGrid");
        var Data = new Object();
        Data.CampaignId = $("#CampaignId").val();
		Data.adGroupId = $("#AdGroupId").val();
		if (grid)
        Data.InsertedItems = jQuery.toJSON(grid.changeLog.inserted);
        Data.UpdateItems = $('#UpdatedCampaignBidConfiges').val();
        Data.DeletedItems = $('#DeletedCampaignBidConfigs').val();
        Data.PricingModel = $("#CostModelWrapper").val();
        Data.OldPriceModel = $("#OldPriceModel").val();


        $.ajax({
            type: 'POST',
            data: Data,
            url: CheckAppSiteCompatibleWithCampaignUrl,

            success: function (data) {
                if (!data.status) {

                    var grid = $("#CampaignBidConfigs").data("tGrid");
                    $("#CampaignBidConfigs tbody").find(".t-no-data").remove();
                    grid.dataBind(data.List);

                    $("#CampaignBidConfigDialog").dialog("open");
                    targetingContinueFlag = false;

                } else {
                    CampaignBidConfigNotCompleted = false;
                    targetingContinueSubmit();

                }

            }
        });


    }



    $('#AgeGroupId').change(updateDemographicsInfo);
    costModelWrapperBaseValue = $('#CostModelWrapper').val();
    $('#CostModelWrapper').change(function () {
        $("#confirmDeleteAllEvents").dialog("open");
        //  updateAudianceSeqmenttab();

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
        // height: 400,
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
                dialog.dialog("close");
                targetingContinueSubmit();

            },
            error: function (error) {
                alert(error.responseText);
            }
        });


    };
    bidDialogButtons[resource_no_command] = function () {

        targetingContinueFlag = false;
        //targetingContinueFlagUpper = false;
        
        count = 0;
        $(this).dialog("close");
        if (targetingContinueFlagUpper) {
            targetingContinueSubmit();
        }
    };
    $("#bidDialog-form").dialog({
        autoOpen: false,
        // height: 400,
        width: 450,
        modal: true,
        resizable: false,
        draggable: false,
        showCloseButton: false,
        buttons: bidDialogButtons,
        close: function () {
        }
    });

    var costelementDialogButtons = {};
    costelementDialogButtons[okCommand] =
          {
              text: SaveButtun,
              click: function () {
                  var valicationResult = updateCostElements();

                  if (valicationResult) {
                      DialogclearErrorMessage("divErrorMessagesForcostElementsDialog");
                      $("#costElementsDialog-Form").dialog("close");
                      costElementContinueSubmit();
                      isCostElementsValid = true;
                  } else {
                      DialogshowErrorMessage("divErrorMessagesForcostElementsDialog");
                  }
              },
              "class": 'primary-btn',

          };

    //    function () {
    //    var valicationResult = updateCostElements();

    //    if (valicationResult) {
    //        DialogclearErrorMessage("divErrorMessagesForcostElementsDialog");
    //        $("#costElementsDialog-Form").dialog("close");
    //        costElementContinueSubmit();
    //        isCostElementsValid = true;
    //    } else {
    //        DialogshowErrorMessage("divErrorMessagesForcostElementsDialog");
    //    }
    //};



    $("#costElementsDialog-Form").dialog({
        autoOpen: false,
        // height: 450,
        width: 600,
        modal: true,
        resizable: false,
        draggable: false,
        buttons: costelementDialogButtons,
        close: function () {
        }
    });
    $("#costElementsDialog-Form").dialog({
        close: function (event, ui) {
            $("#Save").removeClass("disabled");
            $("#Continue").removeClass("disabled");
            count = 0;
            return false;

        }
    });
    if (typeof (localInitilize_CostElements) != "undefined" && localInitilize_CostElements != null) {
        localInitilize_CostElements();
    }
    if (typeof (localInitilize_TrackingEvents) != "undefined" && localInitilize_TrackingEvents != null) {
        localInitilize_TrackingEvents();
    }
    if (typeof (localInitilize_AdRequests) != "undefined" && localInitilize_AdRequests != null) {
        localInitilize_AdRequests();
    }

    updateTabs();
    updateDemographicsInfo();

    setTimeout(function () {
        firstTimeLoadedTargeting = true;
        getBid();
    }, 2000);

    //updateDeviceCapabilitiesInfo();
    $('#minBid').text(discount_value_string);
    $('#Bid').val(bid_value_string);


};
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
function updateAudianceSeqmenttab() {

    if (AudianceSegmentCostModelTypeAllowed.indexOf(document.getElementById("CostModelWrapper").value)<0) {
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
	if (loadDefaults==true || loadDefaults=="True") {
		$("#tab-padding").attr("style", "pointer-events:none;color:silver;");
		$("#ConversionSubItemMenu").addClass("disabledCustom");
	}
	else {
		$("#tab-padding").attr("style", "pointer-events:all;color:'';");
		$("#ConversionSubItemMenu").removeClass("disabledCustom");

	}*/
}
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
function targetingContinueSubmit() {
    targetingContinueFlag = true;
    targetingContinueFlagUpper = false;
    $("#AllowOpenAuction.check-box").removeAttr('disabled');
    $('#targetingForm').submit();
};

function costElementContinueSubmit() {
    costElementContinueFlag = true;
    $('#targetingForm').submit();
}

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
function changeLanguageIcons(img, text, value) {




    if ($("input:checkbox[name=LanguageType]:checked").length == 1) {
        if ($("input:checkbox[name=LanguageType][value=" + value + "]:checked").length == 1) {

            return;
        }
    }




    var lowerText = text.toLowerCase();

    if (!$(img).hasClass("clicked")) {
        $(img).addClass("clicked");
    }
    if ($("input:checkbox[name=LanguageType][value=" + value + "]:checked").length == 1) {
        $("input:checkbox[name=LanguageType][value=" + value + "]").attr("checked", false);
        $(img).removeClass(lowerText + "enabled");
        $(img).addClass(lowerText + "disabled");

    } else {

        $("input:checkbox[name=LanguageType][value=" + value + "]").attr("checked", true);
        $(img).addClass(lowerText + "enabled");
        $(img).removeClass(lowerText + "disabled");

    }


    return;
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
    if (typeof (showSpinner) == "undefined") {
        getSpinnerObj().hideSpinner();
    }

    if (typeof (isLoadHandler) == "undefined" || isLoadHandler == false) {
        checkTreeStatus();
    }
    if (isLoading)
        return;
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


        },
        error: function (error) {
            // alert(error.responseText);

        }
    });
}

function getDevicetargeting() {
    var platforms = $("#DevicesTree").find("[key='Platforms']");
    var chosenplatforms = [];
    if (typeof platforms != "undefined") {

        for (var i = 0 ; i < platforms.length; i++) {
            var platFormIdClass = platforms.eq(i).children().eq(0).attr("class");;
            var platFormId = platforms.eq(i).attr("id");
            if (platFormIdClass) {
                if ((platFormIdClass.indexOf("checked") > -1 || platFormIdClass == 'undetermined') && platFormIdClass.indexOf("unchecked") < 0) {

                    var platForm = getPlatForm(platFormId);

                    if (platFormIdClass.indexOf("checked") > -1 && platFormIdClass.indexOf("unchecked") < 0 && ad_action_type_id != 3 && ad_action_type_id != 4)
                        platForm.isAll = true;

                    var allManufacturers = platforms.eq(i).children().eq(1).find("[Key='Manufacturers']")

                    if (typeof allManufacturers != "undefined")
                        for (var j = 0 ; j < allManufacturers.length; j++) {

                            manufacturerCalss = allManufacturers.eq(j).children().eq(0).attr("class");
                            if (manufacturerCalss) {
                                if ((manufacturerCalss.indexOf("checked") > -1 || manufacturerCalss == 'undetermined') && manufacturerCalss.indexOf("unchecked") < 0) {

                                    var Manufacturer = getManu(allManufacturers.eq(j).attr("id"));

                                    if (manufacturerCalss.indexOf("checked") > -1 && manufacturerCalss.indexOf("unchecked") < 0 && ad_action_type_id != 3 && ad_action_type_id != 4)
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
    getTrackingEvents();
    getDevicetargeting();
    //getCampaignBidConfigLists();
    var Orgbuttons = $("#CampaignBidConfigDialog").dialog("option", "buttons");

    $("#CampaignBidConfigDialog").dialog({
        maxHeight: 700,
        minHeight: 0,
        buttons: [
        {
            text: SaveButtun,
            "class": 'primary-btn',
            click: function () {

                if (IsValidBidConfigDialog()) {

                    var grid = $("#CampaignBidConfigs").data("tGrid");
                    $('#UpdatedNotCompatableCampaignBidConfiges').val(jQuery.toJSON(getNotCompatableItems()));

                    $(this).dialog('close');
                    targetingContinueSubmit();

                }
                else {

                    DialogshowErrorMessage("divErrorMessagesForBidConfigsDialog", true);
                    return false;
                }


            }
        }, Orgbuttons[Orgbuttons.length - 1]]
    });
    

    $("#CampaignBidConfigDialog").dialog({
        close: function (event, ui) {
            DialogclearErrorMessage("divErrorMessagesForBidConfigsDialog");
            if (CampaignBidConfigNotCompleted) {
                $("#Save").removeClass("disabled");
                $("#Continue").removeClass("disabled");
                count = 0;

                return false;
            }
        }
    });

    var result = true;
    clearErrorMessage();

    if (loadOperatorsFlag) {
        filterOperaters();
    }
    

    result = validateDialogGridBidsValues("CampaignBidConfigList");

    if (!result) {
        showErrorMessage(MinBidErrMsginBidConfigTap, true);
        return result;
    }
	
	result = checkBid() && result && !disableSaveButtonForAjax && ValidatedIPRanges() && BudgetChanged() && validateConversions() ;
	
    if (has_device_targeting) {
        result = checkDevices() && result;
	}

	

	result &= $("form:not(#SwitchAccountForm)").valid();

    getJsonRules();
    
	if (result) {
		count = 0;
	}

    return result;
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
		showErrorMessage(CountConversionEveryMessageText , true);
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


	if ((countPrimary == 0 && !$('#conversions').is(':visible')) ) {
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


function checkBid() {

	if (BiddingStrategyFixedValue) {
		$("#BidOptimizationValue").val("0.01");
		$("#MaxBidPrice").val("0.01");
		if (!minBid) {
			if (!IsAdminUserLogged) {
				showErrorMessage(minBidErrMsg, true);
				return false;
			}
			else {
				showWarningMessage(minBidLessErrMsg, true);
				var millisecondsToWait = 3000;
				setTimeout(function () {
					// Whatever you want to do after the wait
				}, millisecondsToWait);

				var bidElemB = jQuery('#Bid');
				var minBidElmValueb = parseFloat(bidElemB.val());
				if (isNaN(minBidElmValueb)) {
					$("#Bid").val("0.0");
				}
				return true;
			}
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
		if (!IsAdminUserLogged) {
			if (minBidElmValue < minBidValue || minBidElmValue <= 0) {

				if (minBidElmValue <= 0) {
					showErrorMessage(minBidMsg, true);
					return false;
				}
				showErrorMessage(minBidErrMsg, true);
				return false;
			}
		}
		else {

			if (minBidElmValue < minBidValue || minBidElmValue <= 0) {

				if (minBidElmValue <= 0) {
					showWarningMessage(minBidMsg, true);
					var millisecondsToWait = 3000;
					setTimeout(function () {
						// Whatever you want to do after the wait
					}, millisecondsToWait);
					return true;
				}
				showWarningMessage(minBidLessErrMsg, true);
				var millisecondsToWait = 3000;
				setTimeout(function () {
					// Whatever you want to do after the wait
				}, millisecondsToWait);
				return true;
			}
		}

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

    if (parseFloat($("#Bid").val()) >= parseFloat($("#minBidValue").text())) {
        $("#tab-padding").attr("style", "pointer-events:all;color:'';");

    }
    else {
        $("#tab-padding").attr("style", "pointer-events:none;color:silver;");

    }

    if ((minBidValue.toFixed(2)) <= 0) {

        $("#minBidValueDiv").hide();
    }
    else {
		if (BiddingStrategyFixedValue)
		$("#minBidValueDiv").show();

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


function getTrackingEvents() {
    var grid = $('#TrackingEventsGrid').data("tGrid");
    if (grid == null)
        return;

    if (grid.changeLog.dirty()) {
        var inserted = new Array();
		var deletedTrackingEvents = '';
		var deletedTrackingCodeEvents = '';
        for (var i = 0; i < grid.changeLog.inserted.length; i++) {
            inserted[inserted.length] = grid.changeLog.inserted[i];
        }

        for (i = 0; i < grid.changeLog.deleted.length; i++) {
            if (typeof (grid.changeLog.deleted[i]) != "undefined") {
				deletedTrackingEvents += '&' + grid.changeLog.deleted[i].Id;
				deletedTrackingCodeEvents += '&' + grid.changeLog.deleted[i].Code;
            }
        }
        $('#InsertedTrackingEvents').val(jQuery.toJSON(inserted));//;JSON.stringify(inserted));
		$('#DeletedTrackingEvents').val(deletedTrackingEvents);
		$('#DeletedTrackingCodeEvents').val(deletedTrackingCodeEvents);
    } else {
        $('#InsertedTrackingEvents').val("");
		$('#DeletedTrackingEvents').val("");
		$('#DeletedTrackingCodeEvents').val("");
    }
}

function onIPRangeRowDataBound(e) {
    $('#IPRangesGrid').find("td:not(.t-last)").click(function (e) {
        e.stopPropagation();
    });
}

function onGeoFencingDataBound(e) {
    $('#GeoFencingsGrid').find("td:not(.t-last)").click(function (e) {
        e.stopPropagation();
    });
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

function deleteGeoFencingTargeting(elem) {
    var grid = $("#GeoFencingsGrid").data("tGrid");
    var tr = $(elem).parents('tr');
    // delete the row
    grid.deleteRow(tr);

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

function updateCostElements() {


    var costElementConflictDialog = $("#costElementConflictDialogTable tbody");
    var validationResult = true;

    var costElementsArray = new Array();
    $.each(costElementConflictDialog.find("tr"), function () {
        var currentRow = $(this);
        var textBox = currentRow.find("input");

        var controlValidationResult = {
            validationResult: false
        };
        textBox.trigger("change", [controlValidationResult]);

        if (!controlValidationResult.validationResult) {
            validationResult = controlValidationResult.validationResult;
        } else {
            var costElementId = currentRow.find("td:eq(0)").text();
            var rowDataItem = {
                ID: costElementId,
                Value: textBox.val()
            };

            costElementsArray.push(rowDataItem);
        }

        if (textBox.val() == "") validationResult = false;

    });

    if (validationResult) {
        $("#UpdatedCostElements").val($.toJSON(costElementsArray));
    } else {
        $("#UpdatedCostElements").val();
    }

    return validationResult;
}

function showCostElementDialog() {
	if (!($('#CostElementsGrid').length>0)) {
		costElementContinueSubmit();
	}
    var grid = $('#CostElementsGrid').data("tGrid");
    if (grid == null)
        return;

    var filteredRows = $("#CostElementsGrid tbody tr");

    var costElementConflictDialog = $("#costElementConflictDialogTable tbody");
    costElementConflictDialog.find("tr").remove();

    $.each(filteredRows, function () {

        var dataItem = grid.dataItem($(this));

        if (dataItem != null && dataItem != undefined) {
            if (dataItem["Stoped"] == false) {
                var value = dataItem["Value"];
                var costElement = dataItem["CostElement"];
                var costElementId = dataItem.ID

                var rowBuilder = $("<tr></tr>");

                var costElemetIdTD = $("<td style='display:none'></td>");
                var costElementTD = $("<td width='35%'></td>");
                var valueTD = $("<td width='65%'></td>");
                var valueType = dataItem.TypeName;

                var valueTextBox = $("<input type='text' class='text-box'  />");
                valueTextBox.change(function (e, data) { validateCostElementValue(this, dataItem.Type, data); });
                valueTextBox.val(value);
                valueTextBox.keypress(function (e) { return validateDecimal(e); });

                var validationSpan = $("<span  class='validation-arrow field-validation-error' style='display: none;'><span class=''>" + currencyValidationMessage + "</span> </span>");

                var valueSpan = $("<span class='small-big-field' style='float:left'></span>");
                valueSpan.append(valueTextBox);
                valueSpan.append("&nbsp;" + valueType);
                costElemetIdTD.text(costElementId);
                costElementTD.text(costElement);
                valueTD.append(valueSpan);
                valueTD.append(validationSpan);
                rowBuilder.append(costElemetIdTD);
                rowBuilder.append(costElementTD);
                rowBuilder.append(valueTD);

                costElementConflictDialog.append(rowBuilder);
            }
        }
    });

    if (costElementConflictDialog.find("tr").length != 0) {
        $("#costElementsDialog-Form").dialog("open");
    } else {
        costElementContinueSubmit();
    }
}

function validateCostElementValue(element, costElementType, data) {

    var jelement = $(element);
    var elementValue = jelement.val();
    var validateResult = false;
    if (isNaN(elementValue) || parseFloat(elementValue) <= 0.0 ||
                        (costElementType == 1 && (parseFloat(elementValue) < 0.0 || parseFloat(elementValue) > 100))) {
        jelement.parent().closest("tr").find("span:eq(1)").show();
    } else {
        jelement.parent().closest("tr").find("span:eq(1)").hide();
        validateResult = true;
    }

    if (data != undefined) {
        data.validationResult = validateResult;
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

function ValidatedIPRanges() {

    var result = true;

    var array = $("#IPRangesGrid tbody tr");

    for (var i = 0; i < array.length; i++) {

        var StartIPRange = $(array[i]).find("#StartIPRange").text();
        var EndIPRange = $(array[i]).find("#EndIPRange").text();

        if (EndIPRange != "" || StartIPRange != "")
            result &= validate_IP_Range(StartIPRange, EndIPRange);
    }

    if (!result)
        showErrorMessage(IPRangeValidation, true);

    return result;

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

                    newText += ", " + itemText;

                    newTitle += ", " + itemText;
                }
                else {

                    newText = itemText;

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

    var rows = $("#TrackingEventsGrid tbody tr:visible");
    var grid = $("#TrackingEventsGrid").data("tGrid");

    var codesArray = new Array();

    $.each(rows, function () {
        var dataItem = grid.dataItem(this);
        if (dataItem != null) {
            codesArray.push($($(this).children("td")[1]).html());
        }
    });

    $.ajax({
        type: 'POST',
        url: checkDeleteTrackingEventUrl,
        cache: false,
        data: { adGroupTrackingEventCodes: codesArray, checkStandards: false, costModelWrapperId: $("#CostModelWrapper").val() },
        success: function (data) {

            show_change_cost_model_warning();

            updateBid();

            costModelWrapperBaseValue = $("#CostModelWrapper").val();

			loadDefaults = costModelWrapperBaseValue == costModelArapperValue ? false : true;
			updateConversiontab();
            refreshTrackingEventsGrid();


        },
        error: function (data) {
            $("#GeneralDialogText").text(data.responseText);
            $("#resultGeneralDialog").dialog("open");
            $("#CostModelWrapper").val(costModelWrapperBaseValue);
        },
        traditional: true

    });
}


function getCampaignBidConfigLists() {
    var grid = $("#CampaignBidConfigList").data("tGrid");
    $('#InsertedItems').val($('#InserteCampaignBidConfigs').val());
    $('#DeletedCampaignBidConfigs').val($('#DeletedCampaignBidConfigs').val());
    $('#UpdatedItems').val($('#UpdatedCampaignBidConfiges').val());
}


function getElem(tr) {


    array = $("#CampaignBidConfigList tbody tr");

    var AppsiteID = $(tr).find("td").eq(2).text();
    var AccountId = $(tr).find("td").eq(3).text();

    for (var i = 0 ; i < array.length; i++) {
        if ($(array[i]).find("td").eq(1).text() == AppsiteID && $(array[i]).find("td").eq(2).text() == AccountId) {
            return $(array[i]).find("a");

        }
    }

}


function deleteAssignedAppsitesBidConfigDialog(elem) {

    var grid = $("#CampaignBidConfigs").data("tGrid");
    var tr = $(elem).parents('tr');
    grid.deleteRow(tr);
    deleteAssignedAppsites(getElem(tr));

}

function CampaignBidConfigs_OnDataBound(sender, args) {

    var grid = $("#CampaignBidConfigs").data("tGrid");
    array = $("#CampaignBidConfigs tbody tr");

    $(this).find("tr").each(function () {

        var row = $(this);
        var Item = grid.dataItem(row);
        if (typeof (Item) != "undefined")
            $(row).find("#AppsiteName").attr("title", Item.Appsite.Name);
    })
    for (var i = 0; i < array.length; i++) {

        dataItem = grid.dataItem(array[i]);
        if (dataItem != undefined) {

            if (dataItem.Bid != "") {
                $(array[i]).find("#Bid")[0].value = parseBidValue(dataItem.Bid);
            }
            if (typeof ($("#BidTap")) != "undefined")
                $(array[i]).find("#MinBid")[0].innerText = $("#bidSection").find("#Bid").val();

            if (!dataItem.HideDeleteButton) {
                $(array[i]).find("#DeleteButton").attr("style", "display:block");
            }
        }
    }
}



function validateDialogGridBidsValues(gridName) {
    var grid = $("#" + gridName).data("tGrid");
    array = $("#" + gridName + " tbody tr");
    var isValid = true;
    for (var i = 0; i < array.length; i++) {
        dataItem = grid.dataItem(array[i]);
        var bidTextBox = "";
        var minbidTextBox = "";
        if (dataItem != undefined && dataItem.ID != undefined) {
            bidTextBox = $(array[i]).find("#Bid" + dataItem.ID);

        }
        if (typeof (bidTextBox) != "undefined" && typeof ($(bidTextBox).val()) != "undefined") {

            if (bidTextBox.length == 0)
            { bidTextBox = $(array[i]).find("#Bid"); }

            if (gridName != "CampaignBidConfigList")
                minbidTextBox = $(array[i]).find("#MinBid").text();
            else
                minbidTextBox = $("#Bid").val();
            if (!isdeletedBid(gridName, dataItem)) {
                if ((bidTextBox.length > 0 && !IsBidValidDecimalExpression(bidTextBox.val()) || bidTextBox.val() == "" || bidTextBox.val() == parseBidValue(0)) || parseFloat(bidTextBox.val()) > parseFloat(minbidTextBox) || bidTextBox.val().length == 0) {

                    bidTextBox.addClass("input-validation-error");
                    isValid = false;
                }

                if (bidTextBox.val().length == 0) {
                    isValid = false;
                }
            }
        }
    }

    return isValid;
}
//




/////////////////////////////Campaign Info End /////////////////////////////