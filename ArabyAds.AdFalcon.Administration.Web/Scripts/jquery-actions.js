var confirmationDilaog;
var currentSubmited = null;
var continueFlag = false;

$(window).on('load', function () {
  $.validator.methods.date = function (value, element) {

    var date = Globalize.parseDate(value, "dd-MM-yyyy", "en-GB");

    return this.optional(element) ||
      date;

  }
  attachResizeEvent();
  attachValidation();
  addClearToDatepicker();
  jQuery(document).click(documentClickHandler);
  jQuery('input[type=submit]').click(function (e) {
    currentSubmited = e.srcElement;
  });
  jQuery.datepicker.setDefaults(jQuery.datepicker.regional[currentLang]);

  if (typeof (localInitilize) != "undefined") {
    localInitilize();
  }
  // setTimeout(hideSuccessfullyMessage, 2000);
  if (isShowConfirmation) {
    //        jQuery('form').submit(function (e) {
    //            if (!continueFlag) {
    //                e.preventDefault();
    //                showConfirmation("message", "title");
    //            }
    //            else {
    //                continueFlag = !continueFlag;
    //            }
    //        });
  }
  $.datepicker._gotoToday = function (id) {

    $(id).datepicker('setDate', new Date()).blur();
  };
});
function documentClickHandler(e) {
  if (!jQuery(e.srcElement).hasClass('blue-info')) {
    if (typeof (closeMoreInfo) != "undefined") {
      closeMoreInfo();
    }
  }
};
function showConfirmation(msg, title) {
  //if (typeof (confirmationDilaog) == "undefined") {
  initilizeConfirmationDilaog();
  //}
  jQuery('#dialog-confirm').attr('title', title);
  jQuery('#dialog-confirm-Msg').text(msg);
  confirmationDilaog.show();
}
function initilizeConfirmationDilaog() {
  confirmationDilaog = jQuery("#dialog-confirm").dialog({
    resizable: false,
    height: 140,
    modal: true,
    buttons: {
      "Yes": function () {
        continueFlag = true;
        //jQuery('form').submit();
        jQuery(currentSubmited).click();
        jQuery(this).dialog("close");
      },
      "NO": function () {
        continueFlag = false;
        jQuery(this).dialog("close");
      }
    }
  });
}
function initilize() {
  //    $("select").selectbox();
}

function addClearToDatepicker() {
  //wrap up the redraw function with our new shiz
  var dpFunc = $.datepicker._generateHTML; //record the original
  $.datepicker._generateHTML = function (inst) {
    var thishtml = $(dpFunc.call($.datepicker, inst)); //call the original

    thishtml = $('<div />').append(thishtml); //add a wrapper div for jQuery context

    //locate the button panel and add our button - with a custom css class.
    $('.ui-datepicker-buttonpane', thishtml).append(
      $('<button class="ui-datepicker-clear ui-state-default ui-priority-primary ui-corner-all"\>' + clearText + '</button>').click(function () {
        inst.input.val('');
        inst.input.datepicker('hide');
      })
    );

    thishtml = thishtml.children(); //remove the wrapper div

    return thishtml; //assume okay to return a jQuery
  };
}

function attachResizeEvent() {
  $(window).resize(function () {
    var field = $(document.activeElement);
    if (field.is('.hasDatepicker')) {
      field.datepicker('hide').datepicker('show');
    }
    if (typeof (localResizeEvent) != "undefined") {
      localResizeEvent();
    }
  });
}
function attachValidation() {
  jQuery("input[maskType = 'decimal']").keypress(function (e) { return validateDecimal(e); });
  jQuery("input[maskType = 'percentage']").keypress(function (e) { return validatePercentage(e); });
  jQuery("input[maskType = 'Int']").keypress(function (e) { return validateInt(e); });
  jQuery("input[maskType = 'Int']").change(function (e) { return validateInt(e); });

  jQuery("input[maskType = 'Int']").on('focusout', function (e) {
    var $this = $(this);
     // $this.val($this.val().replace(/[^0-9]/g, ''));


      var resultsval = $this.val().replace(/[^0-9]/g, '');
      if (parseInt(resultsval) > 2147483647) {
          $this.val('');
          return false;
      }
      $this.val(resultsval);

      return validateInt(e);

  }).on('paste', function (e) {
      var $this = $(this);
      var eEve = e;
      setTimeout(function () {

          var resultsval = $this.val().replace(/[^0-9]/g, '');
          if (parseInt(resultsval) > 2147483647) {
              $this.val('');
              return false;
          }
          $this.val(resultsval);

        return validateInt(eEve);

    }, 5);
  });


  jQuery("input[maskType = 'String']").keypress(function (e) { return validateString(e); });
  jQuery("input[maskType = 'phone']").keypress(function (e) { return validatePhone(e); });
  if (currentDirection == 'rtl') {
    /*
    * Translated default messages for the jQuery validation plugin into arabic.
    * Locale: AR
    */
    jQuery.extend(jQuery.validator.messages, {
      required: "هذا الحقل إلزامي",
      remote: "يرجى تصحيح هذا الحقل للمتابعة",
      email: "رجاء إدخال عنوان بريد إلكتروني صحيح",
      url: "رجاء إدخال عنوان موقع إلكتروني صحيح",
      date: "رجاء إدخال تاريخ صحيح",
      dateISO: "رجاء إدخال تاريخ صحيح (ISO)",
      number: "رجاء إدخال عدد بطريقة صحيحة",
      digits: "رجاء إدخال أرقام فقط",
      creditcard: "رجاء إدخال رقم بطاقة ائتمان صحيح",
      equalTo: "رجاء إدخال نفس القيمة",
      accept: "رجاء إدخال ملف بامتداد موافق عليه",
      maxlength: jQuery.validator.format("الحد الأقصى لعدد الحروف هو {0}"),
      minlength: jQuery.validator.format("الحد الأدنى لعدد الحروف هو {0}"),
      rangelength: jQuery.validator.format("عدد الحروف يجب أن يكون بين {0} و {1}"),
      range: jQuery.validator.format("رجاء إدخال عدد قيمته بين {0} و {1}"),
      max: jQuery.validator.format("رجاء إدخال عدد أقل من أو يساوي (0}"),
      min: jQuery.validator.format("رجاء إدخال عدد أكبر من أو يساوي (0}")
    });
  }
}

function validateDecimal(e) {

  //var val = this.value + String.fromCharCode(51);
  //this.value = this.value.replace(/[^0-9\.]/g, '');
  var charCode = (e.which) ? e.which : event.keyCode;
  if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
    return false;
  return true;
}


function validateInt(e) {
  var charCode = (e.which) ? e.which : event.keyCode;
  if (charCode > 31 && (charCode < 48 || charCode > 57))
    return false;


  if (e.target.value > 247483647) {
     // e.target.value = "";
      return false;
  }

  return true;
}

function validatePhone(e) {
  var charCode = (e.which) ? e.which : event.keyCode;
  if (charCode != 43 && charCode != 35 && charCode != 42 && charCode > 31 && (charCode < 48 || charCode > 57))
    return false;
  return true;
}

function validateString(e) {
  var charCode = (e.which) ? e.which : event.keyCode;
  if (charCode > 31 && (charCode < 48 || charCode > 57))
    //if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8)
    return true;
  return false;
}

function validatePercentage(e) {
  var charCode = (e.which) ? e.which : event.keyCode;
  if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
    return false;
  return true;
}


function convertKeyValueArrayToJson(keyValueArray) {
  var jsonObject = {};
  $.each(keyValueArray, function () {
    if (jsonObject[this.name] !== undefined) {
      if (!jsonObject[this.name].push) {
        jsonObject[this.name] = [jsonObject[this.name]];
      }
      jsonObject[this.name].push(this.value || '');
    } else {
      jsonObject[this.name] = this.value || '';
    }
  });
  return jsonObject;
}
