/////////////////////////////Targeting Start ///////////////////////////

var spinner = null;
var deviceIsExist = null;
var deviceTreeChanged = null;

function getSpinnerObj() {
    if (spinner == null)
        spinner = getSpinner('ContentOfTabTargeting');
    return spinner;
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
        modal: true,
        resizable: false,
        draggable: false,
        showCloseButton:false,
        buttons: confirmationDialogButtons,
        close: function () {
        }
    });


    getDeviceTypeInfo();


    //addIPRules();
    if (is_client_locked) {
        showWarningMessage(locked_warning, true);
	}

	if (is_client_readOnly) {

		showWarningMessage(readOnly_warning, true);
	}
    getSpinnerObj().showSpinner();
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
        $(this).dialog("close");
    };
    dialogButtons[resource_cancel_command] = function () {
        getmodelsInfo();
        $(this).dialog("close");
    };
    
    $("#dialog-form").dialog({
        autoOpen: false,
        //height: 400,
        width: 450,
        modal: true,
        resizable: false,
        draggable: false,
        showCloseButton:false,
        buttons: dialogButtons,
        close: function () {
            getmodelsInfo();
        }
    });
    updateTabs();
   
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
        console.log("deviceIsExist=" + deviceIsExist);
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
function checkStatus() {
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
    var result = !disableSaveButtonForAjax;;
    clearErrorMessage();

    if (loadOperatorsFlag) {
        filterOperaters();
    }
    var result = !disableSaveButtonForAjax;
    if (has_device_targeting) {
        result = checkDevices() && result;
    }
    return result;
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

function checkDevices() {
    var ids = getTreeData('Devices', 'Models');
    if (ids == "") {
        showErrorMessage(noDeviceErrMsg, true);
        return false;
    }
    return true;
}
function treeLoadedHandler() {
    if (isLoading) {
        return;
    } else {
        getSpinnerObj().hideSpinner();
    }
    filterOperaters();
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
/////////////////////////////Targeting End /////////////////////////////
/////////////////////////////Campaign Info Start /////////////////////////////
function clearTreeSummary(treeName, isAll) {
    if (treeName == "DevicesTreeInfo") {
        getmodelsInfo();
        return;
    }
    if (treeName == "deviceCapabilities") {
        updateDeviceCapabilitiesInfo();
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
                newText += "," + item.parent().parent().find('.check-box-text').text();
            }
            newTitle += "," + item.parent().parent().find('.check-box-text').text();
        }
        else {
            if (count < maxCount) {
                newText = item.parent().parent().find('.check-box-text').text();
            }
            newTitle += item.parent().parent().find('.check-box-text').text();
        }
        count++;
    });
    if (isDefult) {
        newText = defaultText;
        deviceTargetingInfoObj.addClass("default-values").text(defaultText).attr('title', defaultText);
    } else {
        deviceTargetingInfoObj.removeClass("default-values").text(newText).attr('title', newTitle);
    }
}
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
/////////////////////////////Campaign Info End /////////////////////////////