import axios from 'axios';
import Constants from '../Config/Constants';
import { getCookie } from '../Utils/Cookies';
import { refreshToken } from './Security';

export  const   ApiManager = {
    callwebservice
  };
  function callwebservice(
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
  
    });
  
  if(headers["Content-Type"]=="application/x-www-form-urlencoded; charset=UTF-8")
  {
        var querystring = require('querystring')
        data = querystring.stringify(data);
  }
  
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
        
        if ((error && error.response  && error.response.status && error.response.status === 401) || (error && error.response  && error.response.status && error.response.status === 422)) {
          xiosConfig.method = "post",
              xiosConfig.url = `/auth/refresh`;
          xiosConfig.headers = {
              "x-csrf-token": Cookie.getCookie("csrf_refresh_token"),
              "X-ADF-USERNAME":username
          };
          return axios(xiosConfig)
              .then(response2 => {
                  console.log("response refreshtoken", response2)
                  current.callwebservice(methodname, endpoint, data, headerconfig, fnSucess, fnErr,ReloginComp,currentCall,RootCall);
              })
              .catch(function (error2) {
              //debugger
                          // if (error2 && error2.response &&  error2.response.status === 401) 
              {
                                /*     this is the code for opening dialog for relogin  */
                if(ReloginComp && ReloginComp!=1)
                {
                
                      ReloginComp.handelSucess(RootCall);
                ReloginComp.handelFail(fnErr);
                  ReloginComp.handelShow(true);
                
                return;
                }
                else
                {
                                return fnErr({status:'failed', data:{ 
                                    methodname: methodname,  
                                    endpoint: endpoint,  
                                    data: data,  
                                    headerconfig: headerconfig
                                }})
                };
                            }
                        })
                }
  if(error && error.response && error.response.data)
  {
      console.log(error.response.data.msg);
      Toast.error(error.response.data.msg);
  }
      });
    //   return ;
  }
export const ResponseStatusCodes = {
    SUCCESS: 200,
    BAD_REQUEST: 400,
    UNAUTHORIZED: 401,
    UNPROCESSABLE: 422,
    INTERNAL_SERVER_ERROR: 500
};
axios.defaults.withCredentials = true;
axios.defaults.Credentials = 'include';
//#region :: Request Interceptor
axios.interceptors.request.use(
    async config => {
    config.withCredentials = true;
    config.Credentials = 'include';
        config.baseURL = Constants.backend.baseUrl;

        const token = getCookie(Constants.keys.cookies.token);
        if (token) {
             config.headers['Authorization'] = `Bearer ${token}`;
        }
        config.headers['X-ADFALCON-API'] ='True';
        return config;
    },
    error => {
        Promise.reject(error)
    });

//#endregion


//#region :: Response Interceptor
axios.interceptors.response.use((response) => {
    return response
}, async (error) => {
    const originalRequest = error.config;

    //#region :: Request UNAUTHORIZED
    if (error?.response?.status === ResponseStatusCodes.UNAUTHORIZED && originalRequest.url === Constants.backend.tokenUrl) {
        //TODO: Redirect to some login page. or Authorize again
        return Promise.reject(error);
    }
    //#endregion

    //#region :: Request UNAUTHORIZED -> Token Expired || Refresh Token Expired
    if (error?.response?.status === ResponseStatusCodes.UNAUTHORIZED ||
        error?.response?.status === ResponseStatusCodes.UNPROCESSABLE) {
        try {
            const token = await refreshToken();
            axios.defaults.headers.common['Authorization'] = 'Bearer ' + token.data.access_token;
            return axios(originalRequest);
        } catch (error) {
            console.log('refresh_token_error', error.response);
        }
    }
    //#endregion

    return Promise.reject(error);
});
//#endregion


export default axios;