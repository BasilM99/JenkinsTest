//callwebserviceimport axios from "axios";
const axios = require("axios");
import { toast as Toast, ToastContainer } from "react-toastify";
import Cookie from "./Cookie";
let ApiManager = {
  callwebservice(
    methodname,
    endpoint,
    data,
    headerconfig,
    fnSucess,
    fnErr,
    ReloginComp,
    currentCall,
    RootCall
  ) {
    let maxRetry = 4;

    var userObj = localStorage.getItem("CurrentUserAdFalcon");
    var username = "";
    if (userObj) {
      userObj = JSON.parse(userObj);

      username = userObj.Email;
    } else {
      username = localStorage.getItem("emailtobelogin");
    }
    /* Here Im merging header  */
    var headers = Object.assign({}, headerconfig, {
      "X-ADF-USERNAME": username
    });

    var xiosConfig = {
      method: methodname,
      url: endpoint,
      data: data,
      headers: headers
    };

    var current = this;
    /* Here i am calling api endpoints  with data */

    return axios(xiosConfig)
      .then(response => {
        let { data } = response;
        /* Here i am handling success response  */
        if (response.status == "200") {
          return fnSucess(response);
        } else {
          return fnErr(response);
        }
      })
      .catch(function(error) {
        /* Here i am handling exceptions from api  */

        if (error && error.response && error.response.data) {
          console.log(error.response.data.msg);
          Toast.error(error.response.data.msg);
        }
      });
    //   return ;
  }
};
module.exports = ApiManager;
