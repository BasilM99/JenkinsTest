
continueFlag = false;
var subType = null;


function CreativeVendorChanged(event, item) {
    if ((typeof (item) != "undefined") && (item != null)) {

        $('[name="CreativeVendorId"]').val(item.ID);
    }
    else {
        $('[name="CreativeVendorId"]').val('');
    }


}
function localInitilize() {
    $('input[id*=ImpressionTrackerRedirect]').change(validateImpressionTrackerRedirect);
    $('input[name*=ClickTrackers]').change(validateClickTrackers);
    $('input[name*=EndCardClickTracker]').change(validateClickTrackers);
    $('input[name*=ImpressionClickTracker]').change(validateClickTrackers);

    $('[name="EnvironmentType"]').change(environmentTypeChange);
    $('[name="OrientationType"]').change(orientationTypeChange);
    $('[name="AdType"]').change(adTypeChange);
    $('[name="AdBannerType"]').change(deviceTypeChange);
    $('[name="AdSubType"]').change(adSubTypeChange);


    $('#AdCreativeDto_Bid').change(function () {
        // update discount text
        var bidElem = $('#AdCreativeDto_Bid');
        //var minBidElm = $('#minBid');
        var bid_value = parseFloat(bidElem.val());
        if (isNaN(bid_value)) {
            bid_value = 0.0;
        }
        calculate_discounted_value(bid_value, '#bidDiscounted');
    });
    $('#AdCreativeDto_AdText').change(textAdChanged).keyup(textAdChanged).attr('maxlength', maxAdTextLength);

    if ($("#AdCreativeDto_ActionText").length != 0) {
        $("#AdCreativeDto_ActionText").attr("maxlength", maxAdActionTextLenth);
    }
    $('#AdCreativeDto_Description').change(descriptionAdChanged).keyup(descriptionAdChanged);
    $('#AdCreativeDto_ActionText').change(actionTextAdChanged).keyup(actionTextAdChanged);


    if (typeof initiateNativeAd == 'function') {
        initiateNativeAd();
    }

    if (typeof VideoEndCardimage == 'function') {
        VideoEndCardimage();
    }

    textAdChanged();
    descriptionAdChanged();
    actionTextAdChanged();
    var dialogButtons = {};
    dialogButtons[close_text] = function () {
        $(this).dialog("close");
    };

    $("#dialog-form").dialog({
        autoOpen: false,
        open: previewDilaogOpen,
        modal: true,
        closeText: '',
        //buttons: dialogButtons,
        resizable: false,
        draggable: false,
        close: function () {
        }
    });

    var statusDialogButtons = {};
    statusDialogButtons[yes_text] = function () {
        $('[name="AdCreativeDto.IsAdPaused"]').val("True");
        continueSubmit();
        $(this).dialog("close");
    };
    statusDialogButtons[no_text] = function () {
        $('[name="AdCreativeDto.IsAdPaused"]').val("False");
        continueSubmit();
        $(this).dialog("close");
    };
    $("#statusDialog-form").dialog({
        autoOpen: false,
        // height: 150,
        width: 300,
        modal: true,
        resizable: false,
        draggable: false,
        buttons: statusDialogButtons,
        close: function () {
        }
    });

    $('form').submit(function (e) {
        if ((!continueFlag) && (isGroupPaused) && (isNew)) {
            e.preventDefault();
            $('#statusDialog-form').dialog('open');
        }

    });
  
    //call ad type custom init code
    var adTypeIdElem = $('#AdTypeId');
    var adTypeId = parseInt(adTypeIdElem.val());
    switch (adTypeId) {
        case 1://banner
        case 2://text
        case 3://Plain html
            {

                adTypeChange();
                if (adTypeId == 3)
                { $("#IsSecureCompliantDiv").show(); }

                environmentTypeChange();
                break;
            }
        case 4://rich media
            {
                subType = parseInt($('[name=AdSubType]').val());
                updateURLDetails();

                adSubTypeChange();
                environmentTypeChange();
                orientationTypeChange();

                break;
            }
    }
    if (is_client_locked) {

        showWarningMessage(locked_warning, true);
	}
	if (is_client_readOnly) {

		showWarningMessage(readOnly_warning, true);
	}


    var validator = $("form:not(#SwitchAccountForm)").data('validator');
    if (validator != null)
        validator.settings.ignore = ":not(:visible)";

    if (adTypeId == 2) {
        $('#DisplayURLSPan').addClass('required-field');
        $('#AdTextSpan').addClass('required-field');
        $('#AdCreativeDto_AdText').addClass('required');
        $('#AdCreativeDto_AdActionRichMediaValue_Value2').addClass('required');
    }
    else {

        $('#DisplayURLSPan').removeClass('required-field');
        $('#AdTextSpan').removeClass('required-field');
        $('#AdCreativeDto_AdText').removeClass('required');
        $('#AdCreativeDto_AdActionRichMediaValue_Value2').removeClass('required');
    }
}
function continueSubmit() {
    continueFlag = true;
    $('form').submit();
};

function ValidateBid() {

    if (!ishouseadd) {
        var bid = parseFloat($('#AdCreativeDto_Bid').val());
        if (((!isNaN(bid)) && (bid < minBid)) || isNaN(bid)) {
            showErrorMessage(adsMoreThanMinBidDMsg, true);
            return false;
        }
    }
    return true;
}

function validateImpressionTrackerRedirect(inputIdValue) {
    var fieldObject = $(this);

    if (typeof inputIdValue == "string") {
        fieldObject = $('#' + inputIdValue);
    } else {
        fieldObject = $(this);
    }


    var returnValue = ValidateUrl(fieldObject, fieldObject.attr("id") + '-URLErrorMsg2');
    return returnValue;
}
function validateClickTrackers(inputIdValue, isValidDublicatedClickTrackersUrl, elmentsId) {
    //  
    var returnValue;
    var fieldObject;
    var isDublicatedValidation = true;
    if (typeof inputIdValue == "string") {
        fieldObject = $('#' + inputIdValue);
    } else {
        fieldObject = $(this);
    }
    error_msg_id = fieldObject.attr("id") + '-URLErrorMsg2';
    dublicatedUrl_Error_msg_id = fieldObject.attr("id") + '-URLErrorMsg3';

    if (isValidDublicatedClickTrackersUrl != false) {


        var isDublicatedClick = validateIsDublicateClickTrackersUrl(fieldObject.val(), elmentsId);
        if (isDublicatedClick) {
            returnValue = false;
            isDublicatedValidation = false;
            $('#' + dublicatedUrl_Error_msg_id).show();
        } else {
            isDublicatedValidation = true;
            $('#' + dublicatedUrl_Error_msg_id).hide();
        }
    }
    returnValue = ValidateUrl(fieldObject, error_msg_id);
    if (!returnValue) {
        return returnValue;
    }


    return returnValue && isDublicatedValidation;
}

function validateIsDublicateClickTrackersUrl(clickTrackerValue, elmentsId) {
    //  
    var ClickTrackersElementId = typeof (elmentsId) == 'undefined' || elmentsId == "" ? "ClickTrackers" : elmentsId

    var isValidDublicatedClickTrackerUrl = false;
    var count = 0;
    $('input[name*=' + ClickTrackersElementId + ']').each(function () {
        if ($(this).val() == clickTrackerValue && $(this).val() != '') {
            count++;
        }
    });
    if (count > 1) {//if found more than one  
        // dublicated click tracker url
        isValidDublicatedClickTrackerUrl = true;
    }
    return isValidDublicatedClickTrackerUrl;
}

function creativeUnitValidate() {

    if (!$("form:not(#SwitchAccountForm)").valid() && jQuery('.field-validation-error:first').position() != "undefined") {
        if (jQuery('.field-validation-error:first').position().top != "undefined") {
            jQuery(window).scrollTop(jQuery('.field-validation-error:first').position().top - 250);
        }
    }

    if (typeof (ShowUrlsErrors) == 'function') {
        ShowUrlsErrors();
    }
    clearErrorMessage();
    $("form:not(#SwitchAccountForm)").validate({
        //other options
        ignore: ':hidden'
    });
    $("form:not(#SwitchAccountForm)").valid();
    //:visible
    var adTypeId = parseInt($('#AdTypeId').val());

    var returnvalue = true;
    var isRequiredNotfound = false;
    var isPartialFound = false;
    var HasPartialFound = false;
    var custom_name = null;
    var showErrorFormatMessage = false;
    switch (adTypeId) {
        case 5:
            {
                returnvalue = validateNativeAdIcons() && validateNativeAdImages();
                if (jQuery('#AdCreativeDto_AppUrl').length) {
                    returnvalue = returnvalue && ValidateAdActionAppUrl();

                }
            }
            break;
        case 1: //banner
        case 3: //Plain HTML
        case 4: //Rich Media

            {
                var adSubTypeId = parseInt($('[name="AdSubType"]').val());
                var adBannerTypeId = parseInt($('#AdBannerTypeId').val());
				if (adSubTypeId == 7 || adSubTypeId == 8) {
                    $('[name="AdSubType"]').val()


                    var SelectedHTML5CreativeId = parseInt($('[name="SelectedHTML5CreativeId"]').val());
                    var SelectedHTML5DocumentId = parseInt($('[name="SelectedHTML5DocumentId"]').val());
                    if (!(SelectedHTML5CreativeId > 0)) {
                        returnvalue = false;
                        $("#displaySelectedHTML5CreativeIdRequiredMsg").show();
                    }
                    if (!(SelectedHTML5DocumentId > 0)) {
                        returnvalue = false;
                        $("#displayUploadZipFileRequiredMsg").show();
                    } else {

                        $("#displayUploadZipFileRequiredMsg").hide();
                    }
					
						$('.errorURLClickTags').each(function () {
							if ($(this).css('display') != 'none') {
								// element is not hidden

								returnvalue = false;

								return;
							}
						});
					
				


                    if (returnvalue == false) {

                        jQuery(window).scrollTop(jQuery('.field-validation-error:first').position().top - 250);
                    }
                }
                custom_name = "CreativeUnit_" + adTypeId;
                if (isNaN(adSubTypeId)) {
                    custom_name += "_0_" + adBannerTypeId;
                } else {
                    custom_name += "_" + adSubTypeId + "_" + adBannerTypeId;
                }
                $('div[custom_name^=' + custom_name + ']:visible').each(function () {

                    var item = $(this);

                    var requiredType = parseInt(item.attr('RequiredType'));
                    var isHasContent = hasContent(item);

                    if (isHasContent && item.attr("ishtml") != null && item.attr("ishtml") == "true") {
                        var formattedValue = isValidformatContent(item);
                        if (formattedValue && ('' + formattedValue).toLowerCase() == "false") {
                            showErrorFormatMessage = true;
                        }
                    }

                    switch (requiredType) {
                        case 2:
                            {
                                if (!isHasContent) {
                                    isRequiredNotfound = true;
                                    return;
                                }
                                break;
                            }
                        case 3:
                            {
                                HasPartialFound = true;
                                if (isHasContent) {
                                    isPartialFound = true;
                                    return;
                                }
                                break;
                            }
                    }
                });


                if (isRequiredNotfound) {
                    showErrorMessage(RequiredBannerMsg, true);
                    returnvalue = false;
                }
                else {
                    if ((HasPartialFound) && (!isPartialFound)) {
                        showErrorMessage(PartialBannerMsg, true);
                        returnvalue = false;
                    }
                }
                break;
            }
        case 2: //Creative Text
            {
                $('div[tileImage=true]').each(function () {
                    var item = $(this);
                    var isHasContent = hasContent(item);
                    if (!isHasContent) {
                        returnvalue = false;
                        return;
                    }
                });
                if (!returnvalue) {
                    showErrorMessage(tileImageMissingBRMsg, true);
                    returnvalue = false;
                }
                break;
            }
    }

    if (showErrorFormatMessage) {
        showErrorMessage(invalidFormatMessage, true);
        returnvalue = false;
    }

    var subValidate = ValidateBid();
    returnvalue = returnvalue && subValidate;

    if ((typeof (ValidateAdAction) != "undefined") && (ValidateAdAction != null) && adTypeId != 3) {
        subValidate = ValidateAdAction();
        returnvalue = returnvalue && subValidate;
    }

    //validate ImpressionTracker
    var impressionTrackerReturnValue = true;
    var validateClickTrackersValue = true;

    switch (adTypeId) {
        case 1:
		case 7:
		case 8:
        case 2:
            {
                //  var isDublicatedClickTrackersUrl = false;
                $('input[name*=ClickTrackers]').each(function () {
                    var validateUrlValue = validateClickTrackers($(this).attr('id'), false);
                    if (validateUrlValue == false) {
                        validateClickTrackersValue = validateUrlValue;
                        return;
                    }

                });
                $('input[id*=ImpressionTrackerRedirect]').each(function () {
                    var validateUrlValue = validateImpressionTrackerRedirect($(this).attr('id'));
                    if (validateUrlValue == false) {
                        impressionTrackerReturnValue = validateUrlValue;
                        return;
                    }
                });

                break;
            }
        case 3:
            {
                //nothing
                break;
            }
        case 4: //Rich Media
            {
                switch (subType) {
                    case 1:
                        $('input[name*=ClickTrackers]').each(function () {
                            var validateUrlValue = validateClickTrackers($(this).attr('id'));

                            if (validateUrlValue == false) {
                                validateClickTrackersValue = validateUrlValue;
                                return;
                            }
                        });
                    case 2:
                    case 3:
                        {
                            //nothing
                            break;
                        }
                    case 4:
                        {
                            //External Url Interstitial
                            //get all visible creative units and check if it contains valid url
                            $('input[name^=' + custom_name + ']').each(function () {
                                if ($(this).parents().find('div[custom_name^=' + custom_name + ']').is(":visible")) {
                                    subValidate = ValidateUrl($(this), "displayURLErrorMsg" + this.name);
                                    returnvalue = returnvalue && subValidate;
                                }
                            });
                            break;
                        }
                }
                break;
            }
        case 5: //Native Ads
            {
                $('input[name*=ClickTrackers]').each(function () {
                    var validateUrl = validateClickTrackers($(this).attr('id'));

                    if (validateUrl == false) {
                        validateClickTrackersValue = validateUrl;
                        return;
                    }
                });

                var validateUrlValue = validateImpressionTrackerRedirect($('#ImpressionTrackerRedirect').attr('id'));

                if (validateUrlValue == false) {
                    impressionTrackerReturnValue = validateUrlValue;
                }
            }
        case 6: //Instream Video
            {
                $('input[name*=ClickTrackers]').each(function () {
                    var validateUrl = validateClickTrackers($(this).attr('id'));

                    if (validateUrl == false) {
                        validateClickTrackersValue = validateUrl;
                        return;
                    }
                });
                $('input[name*=EndCardClickTracker]').each(function () {
                    var validateUrl = validateClickTrackers($(this).attr('id'), false, "EndCardClickTracker");

                    if (validateUrl == false) {
                        validateClickTrackersValue = validateUrl;
                        return;
                    }
                });

                $('input[name*=ImpressionClickTracker]').each(function () {
                    var validateUrl = validateClickTrackers($(this).attr('id'), false, "ImpressionClickTracker");

                    if (validateUrl == false) {
                        validateClickTrackersValue = validateUrl;
                        return;
                    }
                });
                var instramVideoCustomname = $('[name="InstramVideoCustomname"]');//parseInt($('[name="InstramVideoCustomname"]').val());
                $('div[custom_name^=' + instramVideoCustomname.val() + ']:visible').each(function () {

                    var item = $(this);

                    var requiredType = parseInt(item.attr('RequiredType'));
                    var parent_id = item.attr('parent_id');
                    var docContent = $('#CreativeUnit_VidoeDocId');// + parent_id);

                    if (docContent.val() != null && docContent.val() != '') {
                        isHasContent = true;
                    } else {
                        isHasContent = false;
                    }

                    if (isHasContent && item.attr("ishtml") != null && item.attr("ishtml") == "true") {
                        var formattedValue = isValidformatContent(item);
                        if (formattedValue && ('' + formattedValue).toLowerCase() == "false") {
                            showErrorFormatMessage = true;
                        }
                    }

                    switch (requiredType) {
                        case 0:
                            {
                                if (!isHasContent && isVideo == "True") {
                                    isRequiredNotfound = true;
                                    returnvalue = false;
                                }
                                break;
                            }
                        case 1:
                            {
                                if (!isHasContent) {
                                    isRequiredNotfound = true;
                                    returnvalue = false;
                                }
                                break;
                            }
                        case 2:
                            {
                                if (!isHasContent) {
                                    isRequiredNotfound = true;
                                    return;
                                }
                                break;
                            }
                        case 3:
                            {
                                HasPartialFound = true;
                                if (isHasContent) {
                                    isPartialFound = true;
                                    return;
                                }
                                break;
                            }
                    }
                });


                if (isRequiredNotfound) {
                    showErrorMessage(RequiredInstreamVideorMsg, true);
                    returnvalue = false;
                }
                if (Radio_IsChecked("#UploadXml")) {
                    if (!($("#AdCreativeDto_XMlUrl").val() || $("#AdCreativeDto_Xml").val())) {

                        showErrorMessage(RequiredVASTInstreamVideorMsg, true);
                        returnvalue = false;

                    }


                }
                $('.errorURLEnteredImages').each(function () {
                    if ($(this).css('display') != 'none') {
                        // element is not hidden

                        returnvalue = false;

                        return;
                    }
                });
               
                if (typeof (FinalCheckforSubmitting) == 'function') {
                    FinalCheckforSubmitting();
                }


                if (typeof (EventsVendorsSaveWhenSubmit) == 'function') {
                    var resultValVendor = EventsVendorsSaveWhenSubmit();

                    returnvalue = resultValVendor;
                }
                $('.errorURLEnteredCTA').each(function () {
                    if ($(this).css('display') != 'none') {
                        // element is not hidden

                        returnvalue = false;

                        return;
                    }
                });

                $('.errorURLEnteredFluid').each(function () {
                    if ($(this).css('display') != 'none') {
                        // element is not hidden

                        returnvalue = false;

                        return;
                    }
                });
                $('.errorXMlUrl').each(function () {
                    if ($(this).css('display') != 'none') {
                        // element is not hidden

                        returnvalue = false;

                        return;
                    }
                });

                break;
            }

            break;
    }
    if (impressionTrackerReturnValue == false || validateClickTrackersValue == false) {
        returnvalue = false;
    }
    if (returnvalue) {
        hideErrorMessage();
    }
    validatePause();
	return returnvalue && $("form:not(#SwitchAccountForm)").valid();
}

function validateNativeAdIcons() {
    var returnvalue = true;

    for (var i = 0; i < requiredIconsArray.length; i++) {
        if (creativesUnitsIds.indexOf(requiredIconsArray[i]) == -1) {
            showErrorMessage(RequiredIconMsg, true);
            returnvalue = false;
            break;
        }
    }

    return returnvalue;
}


function validateNativeAdImages() {
    var returnvalue = true;

    for (var i = 0; i < requiredIconsArray.length; i++) {
        if (creativesUnitsIds.indexOf(requiredImagesArray[i]) == -1) {
            showErrorMessage(RequiredImageMsg, true);
            returnvalue = false;
            break;
        }
    }

    return returnvalue;
}

function isValidformatContent(item) {

    var content = getContent(item);
    var isFormatted = true;
    if ((content != null) && (content != '')) {
        $.ajax({
            type: "POST", async: false, url: formatContentUrl, data: { content: content }, success: function (data) {
                isFormatted = data;
            }
        });

        return isFormatted;
    }

}

function hasContent(item) {
    var content = getContent(item);
    return ((content != null) && (content != ''));
}

function getContent(elem) {

    var inputElem = elem.find('[custom_name="content"]');
    if (inputElem.length < 1) {
        inputElem = elem.find('[custom-name="content"]');
    }
    var content = inputElem.val();

    return content;
}

function validatePause() {

}

function textAdChanged() {
    var elem = $('#AdCreativeDto_AdText');
    var value = elem.val();
    var rem = maxAdTextLength - value.length;
    var text = rem + remainingCharactersMsg + maxAdTextLength;
    $('#remainingCharactersDesc').text(text);
    $('[isSampleText]').text(value);

}

function descriptionAdChanged() {
    var elem = $('#AdCreativeDto_Description');

    if (elem.length != 0) {
        var value = elem.val();

        if (maxAdDesriptionLength >= value.length) {

            var rem = maxAdDesriptionLength - value.length;
            var text = rem + remainingCharactersMsg + maxAdDesriptionLength;
            $('#remainingDescriptionCharactersDesc').text(text);
        } else {
            elem.val(value.substring(0, maxAdDesriptionLength));
        }
    }
}


function actionTextAdChanged() {
    var elem = $('#AdCreativeDto_ActionText');

    if (elem.length != 0) {
        var value = elem.val();

        if (maxAdActionTextLenth >= value.length) {

            var rem = maxAdActionTextLenth - value.length;
            var text = rem + remainingCharactersMsg + maxAdActionTextLenth;
            $('#remainingActionTextCharactersDesc').text(text);
        } else {
            elem.val(value.substring(0, maxAdActionTextLenth));
        }
    }
}


function fireDeviceChange() {
    deviceTypeChange();
}
function fireDeviceChangePublicWEbSite() {
    var found = false;
    var elem = $('[name="AdBannerType"]');
    if (elem.length > 0) {
        for (var i = 0; i < elem.length; i++) {
            var ctx = $(elem[i]);
            if (ctx.attr('checked')) {
                ctx.attr('checked', false).first().click();
                found = true;
            }
        }
        if (!found) {
            $('[name="AdBannerType"]').first().click();
        }
    } else {
        $('[name="AdBannerType"]').first().click();
    }
}
function deviceTypeChange() {

    var adBannerTypeIdElem = $('#AdBannerTypeId');
    var value = $('#AdBannerType').val();
    if (!hasValue(value)) {
        value = $('input[name=AdBannerType][checked]').val();;
    }
    if (isNaN(parseInt(value))) {
        if (value == "Phone") {
            adBannerTypeIdElem.val('1');
            $('div[name="PhoneBannersCreativeUnitContainer"]').show();
            $('div[name="TabletBannersCreativeUnitContainer"]').hide();
        } else //Tablet
        {
            adBannerTypeIdElem.val('2');
            $('div[name="PhoneBannersCreativeUnitContainer"]').hide();
            $('div[name="TabletBannersCreativeUnitContainer"]').show();
        }
    }
}

function adTypeChange() {



    var adTypeIdElem = $('#AdTypeId');
    var value = $('#AdType').val();

    if (!hasValue(value)) {
        value = $('input[name=AdType][checked]').val();
    }
    var adBannerTypeIdElem = $('#AdBannerTypeId');
    hideErrorMessage();

    if (value == "TextCreative") {
        $("#urls").show();
        $('#deviceTypeDiv').hide('fast');
        $('#CreativeTextContainer').show();
        $('#CreativeUnitContainer_1').hide();
        $('#CreativeUnitContainer_3').hide();
        adTypeIdElem.val('2');
        adBannerTypeIdElem.val('');
        $('#environmentTypeContiner').hide();
        $('#bannerDiv').hide();
        updateURLDetails(1);
        $('#DisplayURLSPan').addClass('required-field');
        $('#AdTextSpan').addClass('required-field');
        $('#AdCreativeDto_AdText').addClass('required');
        $('#AdCreativeDto_AdActionRichMediaValue_Value2').addClass('required');
        $('#AdCreativeDto_Name-error').remove();
        $('#displayURLRequiredMsg').hide();
        $('#AdvanceSettingdivContainer').hide();
    }

    if (value == "BannerCreative") {
        $("#urls").show();
        $('#deviceTypeDiv').show('fast');
        $('#CreativeTextContainer').hide();
        $('#CreativeUnitContainer_1').show();
        $('#CreativeUnitContainer_3').hide();
        $('#environmentTypeContiner').show();
        adTypeIdElem.val('1');
        $('#bannerDiv').show();
        fireDeviceChange();
        //fireDeviceChangePublicWEbSite();
        updateURLDetails(1);
        $('#AdCreativeDto_Name-error').remove();
        $('#displayURLRequiredMsg').hide();
        $('#AdvanceSettingdivContainer').show();
    }
    if (value == "HTMLCreative") {
        $("#urls").hide();
        $('#deviceTypeDiv').show('fast');
        $('#CreativeTextContainer').hide();
        $('#CreativeUnitContainer_1').hide();
        $('#CreativeUnitContainer_3').show();
        $('#environmentTypeContiner').show();

        adTypeIdElem.val('3');
        fireDeviceChange();
        updateURLDetails(2);
        $('#AdCreativeDto_Name-error').remove();
        $('#displayURLRequiredMsg').hide();
        $('#AdvanceSettingdivContainer').hide();
    }
}

var updateURLDetails = function (checkType) {

    $('#DisplayURLSPan').removeClass('required-field');
    $('#AdTextSpan').removeClass('required-field');
    $('#AdCreativeDto_AdText').removeClass('required');
    $('#AdCreativeDto_AdActionRichMediaValue_Value2').removeClass('required');
    $('#AdCreativeDto_AdText-error').remove();
    $('#displayURLRequiredMsg2').hide();
    switch (checkType) {
    case 1:
    {
        $("#IsSecureCompliantDiv").hide();
        $("#urlDetailsContainer").show();

        ValidateAdAction = validateRM_AdAction;
        //  $('#AdCreativeDto_AdActionRichMediaValue_Value2').addClass('required');
        $('#AdCreativeDto_AdActionRichMediaValue_Value').addClass('required');

        break;
    }
    case 7:
    {
        $("#IsSecureCompliantDiv").hide();
        //$("#urlDetailsContainer").show();

        $("#urlDetailsContainer").hide();
        $('#AdCreativeDto_AdActionRichMediaValue_Value2').removeClass('required');
        $('#AdCreativeDto_AdActionRichMediaValue_Value').removeClass('required');
        ValidateAdAction = function () { return true; };

        break;
			}
		case 8:
			{
				$("#IsSecureCompliantDiv").hide();
				//$("#urlDetailsContainer").show();

				$("#urlDetailsContainer").hide();
				$('#AdCreativeDto_AdActionRichMediaValue_Value2').removeClass('required');
				$('#AdCreativeDto_AdActionRichMediaValue_Value').removeClass('required');
				ValidateAdAction = function () { return true; };

				break;
			}
    case 2:
    {
        $("#IsSecureCompliantDiv").show();
        $("#urlDetailsContainer").hide();
        $('#AdCreativeDto_AdActionRichMediaValue_Value2').removeClass('required');
        $('#AdCreativeDto_AdActionRichMediaValue_Value').removeClass('required');
        ValidateAdAction = function () { return true; };
        break;
    }
    }
}
function onIsSecureCompliantChangde(item) {
    //
    var isChecked = document.getElementById('IsSecureCompliantRich').checked;
    if (isChecked) {

        $("#AdCreativeDto_IsSecureCompliant").val("True");

    }
    else {
        $("#AdCreativeDto_IsSecureCompliant").val("False");
    }

};
function adSubTypeChange() {
    var value = $('#AdSubType').val();
    subType = parseInt(value);
    switch (subType) {
		case 1:
			{
				//Expandable Rich Media
				$('#CreativeUnitContainer_1').show();
				$('#CreativeUnitContainer_2').hide();
				$('#CreativeUnitContainer_7').hide();
				$('#ClickMethodHTML5').hide();
				updateURLDetails(1);
				$("#HTML5ToolTip").hide();
				break;
			}
		case 7:
			{
				//Expandable Rich Media
				$('#CreativeUnitContainer_1').hide();
				$('#ClickMethodHTML5').show();
				$('#CreativeUnitContainer_7').show();
				$('#CreativeUnitContainer_2').hide();


				$('#CreativeUnitContainer_3').hide();
				$('#CreativeUnitContainer_4').hide();
				updateURLDetails(7);
				$("#HTML5ToolTip").show();
				break;
			}

		case 8:
			{
				//Expandable Rich Media
				$('#CreativeUnitContainer_1').hide();
				$('#ClickMethodHTML5').show();
				$('#CreativeUnitContainer_8').show();
				$('#CreativeUnitContainer_2').hide();


				$('#CreativeUnitContainer_3').hide();
				$('#CreativeUnitContainer_4').hide();
				updateURLDetails(8);
				$("#HTML5ToolTip").show();
				break;
			}
		case 2:
			{
				//JavaScript Rich Media
				$('#CreativeUnitContainer_1').hide();
				$('#CreativeUnitContainer_2').show();
				$('#CreativeUnitContainer_7').hide();
				$('#ClickMethodHTML5').hide();
				updateURLDetails(2);
				$("#HTML5ToolTip").hide();
				break;
			}
		case 3:
			{
				//JavaScript Interstitial
				$("#IsSecureCompliantDiv").show();
				$('#CreativeUnitContainer_3').show();
				$('#CreativeUnitContainer_4').hide();
				$('#CreativeUnitContainer_7').hide();
				$('#CreativeUnitContainer_8').hide();
				$('#ClickMethodHTML5').hide();
				$("#HTML5ToolTip").hide();
				break;
			}
		case 4:
			{
				//External Url Interstitial
				$("#IsSecureCompliantDiv").hide();
				$('#CreativeUnitContainer_3').hide();
				$('#CreativeUnitContainer_4').show();
				$('#CreativeUnitContainer_7').hide();
				$('#CreativeUnitContainer_8').hide();
				$('#ClickMethodHTML5').hide();
				$("#HTML5ToolTip").hide();
				break;
			}
    }
    fireDeviceChange();
}

function environmentTypeChange() {
    onRadioEnvironmentTypeChanged();
    filterDevices();
}
var webEnvironmentTypeId;
function onRadioEnvironmentTypeChanged() {

    var environmentType = 0;
    if ($("#EnvironmentType").val() != '') {
        environmentType = $("#EnvironmentType").val();
    }
    if ($('input[name=EnvironmentType][checked]').length > 0) {
        environmentType = $('input[name=EnvironmentType][checked]').val();
    }
    if (environmentType == webEnvironmentTypeId) {
        $("#divRichMediaProtocol").hide();

    } else {
        $("#divRichMediaProtocol").show();
    }
    //if ($('input[name=EnvironmentType]:checked').length > 0)
    //    if ($('input[name=EnvironmentType]:checked').val() == webEnvironmentTypeId) {
    //        $("#divRichMediaProtocol").hide();

    //    } else {

    //        $("#divRichMediaProtocol").show();
    //    }
}

function orientationTypeChange() {
    filterDevices();
}

var filterDevices = function () {
    var environment_type = parseInt($('[name="EnvironmentType"]:checked').val());
    var orientation_type = parseInt($('[name="OrientationType"]:checked').val());
    var creativeUnits = $("div[environmenttype]");


    if (!hasValue($('[name="OrientationType"]:checked').val())) {
        orientation_type = parseInt($('[name="OrientationType"]').val());
    }

    if (!hasValue($('[name="EnvironmentType"]:checked').val())) {
        environment_type = parseInt($('[name="EnvironmentType"]').val());
    }

    switch (orientation_type) {
        case 0:
            {
                creativeUnits.hide();

                if (environment_type == 0) {
                    $('div[orientationType="0"][orientationtypeid="0"]').show();
                    $('div[orientationType="1"][orientationtypeid="0"]').show();
                    $('div[orientationType="2"][orientationtypeid="0"]').show();
                }
                else {
                    $('div[orientationType="0"][environmenttype="' + environment_type + '"]').show();
                    $('div[orientationType="1"][environmenttype="' + environment_type + '"]').show();
                    $('div[orientationType="2"][environmenttype="' + environment_type + '"]').show();
                    $('div[orientationType="0"][environmenttype="0"]').show();
                    $('div[orientationType="1"][environmenttype="0"]').show();
                    $('div[orientationType="2"][environmenttype="0"]').show();
                }
                break;
            }
        case 1:
            //Portrait
            {
                creativeUnits.hide();
                if (environment_type == 0) {
                    $('div[orientationType="0"]').show();
                    $('div[orientationType="1"]').show();
                } else {
                    $('div[orientationType="0"][environmenttype="' + environment_type + '"]').show();
                    $('div[orientationType="1"][environmenttype="' + environment_type + '"]').show();
                    $('div[orientationType="0"][environmenttype="0"]').show();
                    $('div[orientationType="1"][environmenttype="0"]').show();
                }
                break;
            }
        case 2:
            //Landscape
            {
                creativeUnits.hide();
                if (environment_type == 0) {
                    $('div[orientationType="0"]').show();
                    $('div[orientationType="2"]').show();
                } else {
                    $('div[orientationType="0"][environmenttype="' + environment_type + '"]').show();
                    $('div[orientationType="2"][environmenttype="' + environment_type + '"]').show();
                    $('div[orientationType="0"][environmenttype="0"]').show();
                    $('div[orientationType="2"][environmenttype="0"]').show();
                }
                break;
            }
    }
}
var formatContent = function (content_elem) {
    var array = content_elem.split('_');
    var creativeId = array[array.length - 1];
    //call the function and return the result
    var content = $('[name="' + content_elem + '"]').val();
    $.ajax(
        {
            url: baseUrl + "/Campaign/FormatContent?creativeId=" + creativeId,
            //dataType: "text text",
            //contentType: 'application/text; charset=utf-8',
            type: "POST",
            data: content,
            success: function (formattedContent) {
                $('[name="' + content_elem + '"]').val(formattedContent.Content);
                clearErrorMessage();
                if (formattedContent.IsValid == false) {
                    showErrorMessage(invalidFormatMessage, true);
                }
                else {
                    clearSuccessfullyMessage();
                    showSuccessfullyMessage(succcessFormatMessage, false);
                }
            },
            error: function (error) {
                clearErrorMessage();
                showErrorMessage(error.responseText);
            }
        });
}


/*** For Native Ads *******/
function rateAnchor(anchor) {
    var anchorobj = $(anchor);
    var ratingValue = anchorobj.attr("rating");
    setRating(ratingValue);

}

function returnRate(event) {
    var event = $.Event(event);
    //if (!$(event.originalEvent.target).is("div")) {
    //    return;
    //}

    var adRating = $("#AdCreativeDto_StarRating").val();

    setRating(adRating);
}

function setRating(ratingValue) {
    for (var i = 0; i <= ratingValue; i++) {
        var itemAnchor = $("#ratingDiv a[rating=" + i + "]");
        itemAnchor.addClass("filled");
        itemAnchor.removeClass("outline");
    }

    for (var i = 5; i > ratingValue; i--) {
        var itemAnchor = $("#ratingDiv a[rating=" + i + "]");
        itemAnchor.addClass("outline");
        itemAnchor.removeClass("filled");
    }
}

function rateNativeAd(ratingValue) {
    $("#AdCreativeDto_StarRating").val(ratingValue);
    setRating(ratingValue);
    return false;
}