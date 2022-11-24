import * as yup from "yup";
import LocalStorage from '../Utils/LocalStorage';
import Constants from '../Config/Constants';
import arLocal from './YUP-Local/arLocal';
import enLocal from './YUP-Local/enLocal';
if ( window.AdFalconUserLoggedInUserObject.CurrentLanguage
  == "ar" ||  window.AdFalconUserLoggedInUserObject.CurrentLanguage
  == "ar-jo"  ) {

    yup.setLocale(arLocal);
  } else {

    yup.setLocale(enLocal);
  }


  yup.addMethod(yup.number, 'limitdecimalto3', function(anyArgsYouNeed) {
    const { message } = anyArgsYouNeed;
    return this.test(`test-limitdecimalto3`, message, function(value) {
        const { path, createError } = this;
        const { some, more, args } = anyArgsYouNeed;
       return  (value + "").match(/^\d*\.{3}\d*$/)
        //return someCondition || conditionTwo || createError(...);
    });

    });



export default yup;