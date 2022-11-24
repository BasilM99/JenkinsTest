
import React, { Component } from 'react';
import { FormattedMessage, FormattedHTMLMessage } from 'react-intl';
import ApiManager from './ApiManager';
import localeMEn from '../../translations/en.json';
import localeMAr from '../../translations/ar.json';


//Toast notification
import { toast as Toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Cookie from './Cookie.jsx'

const queryString = require('query-string');
   window.maxRetry = 3;
    window.currentCall = 1;
class BaseComponent extends Component {
	
    Unique(value, index, self) {
        return self.indexOf(value) === index;
    }
	
    get IsArabic() {
			const queryValuesC = queryString.parse(location.search);
			var langVa='en';
        if(queryValuesC.lang !== undefined){
			
			langVa= queryValuesC.lang;
		}
        return (localStorage.getItem('lang') /*|| navigator.language.split(/[-_]/)[0] */|| langVa|| 'en') === 'ar';
    }

    T(x) {
        return <FormattedMessage id={x} defaultMessage={x} children={
            (msg) => msg
        } />;
    }
	TString(x){
		
		
		return x;
	}
	TMString(x){
		//debugger;
		if(this.IsArabic)
		{
		return localeMAr[x];
		}
		else
			return localeMEn[x];
	}
    TSelectOptions(key, value) {
        return <FormattedMessage id={value} defaultMessage={value} key={key} children={
            (msg) => <option key={key} value={key}>{msg}</option>
        } >
        </FormattedMessage>
    }
populateOptions(options) {
  return options.map((option, index) => (
  
  
      <option key={index}  value={option.Id} advertiser_name={option.Name}>{option.Name}</option>
  
 
	
	
	)
	
  );
}
	
    TFormat(x, args) {
        return <FormattedMessage id={x} defaultMessage={x}>

            {
                (msg) => {
                    let newMsg = msg;
                    args.forEach((arg, i) => {
                        newMsg = newMsg.replace("[" + i + "]", arg);
                    });
                    return newMsg;
                }

            }

        </FormattedMessage>;
    }

    THtml(x) {
        return <FormattedHTMLMessage id={x} defaultMessage={x} />;
    }
	

	
	CheckIsInIFrame()
	{
		
	
		return false;
		
	}

	GetTitle(Title)
	{
		var userObj= localStorage.getItem('CurrentUserAdFalcon');
			
			if(userObj&& window.AdFalconGlobal.IntegrationOn){
					userObj=JSON.parse(userObj);
		if(userObj.CurrentLanguage==='ar')
		{
			if(Title==='Audience')
			{
				return 'جمهور';
				
			}
			else if(Title==='Register')
			{
				
					return 'تسجيل';
			}
			else if(Title==='Audiences')
			{
				return 'جماهير';
				
			}
			else if(Title==='New Audience')
			{
					return 'جمهور جديد';
				
			}
				else if(Title==='Update Audience')
			{
				return 'تعديل جمهور';
				
			}	else if(Title==='Login')
			{
				return 'دخول';
				
			}
		}
		else
		{
			
			if(Title==='Audiences')
			{
				return 'Audience Lists';
				
			}
			
			
		}
		
		
			}
			
			return Title;
		
	}
	

    get Guid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    paginate(array, page_size, page_number) {
        page_number;
        return array.slice(page_number * page_size, (page_number + 1) * page_size);
    }


    random(min, max) {
        return Math.floor(Math.random() * (max - min + 1) + min);
	}
	

	
 callapi(methodname, endpoint, data, headerconfig, fnSucess, fnErr,ReloginCom){
	 

var RootCall=() => {
	
               ApiManager.callwebservice(methodname, endpoint, data, headerconfig, fnSucess, fnErr,1);
			   
            };
       return ApiManager.callwebservice(methodname, endpoint, data, headerconfig, fnSucess, fnErr,ReloginCom  ,1,RootCall)
   }




}

export default BaseComponent;