import React, {Component} from 'react';
import ReactDom from 'react-dom';
import BaseFormComponent from "./BaseFormComponent";
import {ApiManager} from "../../_helpers/ApiManager";
import SessionLogOuters from "./SessionLogOuter";
import { toast as Toast, ToastContainer } from 'react-toastify';
//import { Button, Header, Icon, Modal } from 'semantic-ui-react';

import "../../../../contents/styles/popup.css";
import 'react-responsive-modal/styles.css';
import { Modal } from 'react-responsive-modal';

const style = <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/semantic-ui@2.4.1/dist/semantic.min.css'/>


export default class Relogin extends BaseFormComponent {

  state = {
	  	show:false,
      form: {
          email: {
            readonly: false, //this.checkIfthereisIntegration(),
              value: '',
              validation: {
                  required: true,
              },
              validate: false,
              valid: false,
              error: ''
          },
          password: {
              value: '',
              validation: {
                  required: true
              },
              validate: false,
              valid: false,
              error: '',
          },
          rememberMe: {
              value: false,
              validation: {},
              validate: false,
              valid: true,
              error: '',
          }
      }
}



	

  componentWillUnmount() {
    this.props.onRef(undefined);
  }
 
 
 
 componentDidMount() {
    

		
			this.props.onRef(this);
	

		  
		  	 this.handleLoadPageIntegration();
    }

	handleLoadPageIntegration=()=>
	{
				var self= this;
	
		  window.$(document).ready(function () {
			

	  
		
		
		
		
		var userObj= localStorage.getItem('CurrentUserAdFalcon');
			
			if(userObj){
					userObj=JSON.parse(userObj);
	
			self.state.form.email.value=userObj.Email;
			//self.setState({name:{value: userObj.Name}});
			self.setState(self.state.form);
			}
			
		
		
		  });
		
	}


  handelShow = (open) => {
	  
	  	 this.handleLoadPageIntegration();
	  	this.setState({show:open});
  }
  handelSucess = (open) => {
	  
	   this.loginSuccess=open;
	   
	
  }
   handelFail= (open) => {
	  
	  	   this.LoginFail=open;
	  	
  }
  handleClose = (event) => {
	  	this.handelShow(false);
	    event.preventDefault();
		this.RedirectToLogin();
  }
  
  
handleRegister =  (event) => {
	  
	  
	  	this.handelShow(false);
	    event.preventDefault();
		this.RedirectToGeneric("/register",this.TString("Register"));
  }
  
  
  
  handelLogin = (event) => {
    event.preventDefault();
    // if (!this.isValidForm()) return;
	
	 if (window.currentCall > window.maxRetry) {
            window.currentCall = 1;
			this.handelShow(false);
			 // this.LoginFail();
            this.RedirectToLogin();
        }
        else {
            window.currentCall++;
        }
    console.log('called .. ');
    let { form } = this.state;
    let data = {
        username: form.email.value,
        password: form.password.value
    }
    let headers={
    };
    var self = this;
    console.log('data  in relogin ',data);
    ApiManager.callwebservice("post", '/login', data, headers, (success) => {
        if (success.data.code == '200') {
			    // localStorage.setItem("passwordEx", "true");
			     // self.RedirectToGeneric("/change-password","ChangePassword");
            localStorage.setItem("userdetail", success.data.user_id);
            if (form.rememberMe.value) {
                localStorage.setItem("email", form.email.value);
                localStorage.setItem("password", form.password.value);
            }
            else {
                localStorage.setItem("email", "");
                localStorage.setItem("password", "");
            }
			self.NotifyAuthincation();
            console.log('success is 111',success);
			this.handelShow(false);
        
			
				if(success.data.session_data && success.data.session_data.length>0)
				{
				self.ShowSessionsList(success.data.session_data);
				
				 this.loginSuccess(); 
				}
				else
				{
					
					    this.loginSuccess();   
					
				}
				
        }
		else if(success.data.code == '1011')
			{
					//localStorage.setItem("pw_change_token", success.data.pw_change_token);
				     localStorage.setItem("passwordEx", "true");
					     localStorage.setItem("userdetail", success.data.user_id);
				 self.RedirectToGeneric("/change-password","ChangePassword");
			}
        else {
            Toast.error(success.data.msg);
        }
    }, (error) => {
        Toast.error(error.data.msg);
    },1)

};


onKeyPress = e => {
  if (e.key === "Enter") {
    console.log("Enter key pressed");
    // write your functionality here
    this.handelLogin(e);
  }
};
ShowSessionsList = (Sessions) => {
  
this.SesssionLogOut.handelShow(true,Sessions);
};

showHideModal = (flag)=>{
  this.setState({show : flag});
}
  render() {
    console.log('relogin  component called ',this.props)
    let {form} = this.state;
    const onOpenModal = () => this.showHideModal(true);
    const onCloseModal = () => this.showHideModal(false);
       
    const show = this.state.show;
    return (
    <div>
      <Modal open={show} onClose={() => onCloseModal(false)} classNames={{modal: 'customModal'}}>
                <form action="">
                    <div className="section-form-inner-container last-container">
                        <div className="data-row login-area-container">
                            <div className="box-content">
                                <h3>Sign in</h3>
                                <div className="login-form-container">
                                    <div className="text-boxes-pass-container">
                                        <div className="form-group">
                                            <label><span className="field-title required-field">Username</span></label>
                                            <div>
                                                <input
                                                    className="form-control" 
                                                    autoFocus
                                                    margin="dense"
                                                    id="namelog"
                                                    label=  {this.isArabic?"اسم المستخدم":"User Name"}
                                                    type="email"
                                                    onChange={this.handelInputChange}
                                                    />
                                                <br />
                                                <br />
                                            </div>
                                        </div>
                                        <div className="form-group">
                                            <label style={{width: 95+"%"}}>
                                                <span className="field-title required-field">Password</span>
                                                <span className="forgot-password floating-right reverse-link"><a href="/en/user/forgetpassword/forgetpassword">Forgot your password?</a></span>
                                            </label>
                                            <div>
                                                <input
                                                    className="form-control"
                                                    autoFocus
                                                    margin="dense"
                                                    id="passlog"
                                                    label=  {this.isArabic?"كلمة السر":"Password"}
                                                    type="password"
                                                    onChange={this.handelInputChange}
                                        
                                                    name="password" 
                                                    onChange={this.handelInputChange}

                                                    onKeyPress={this.onKeyPress}
                                                />
                                                <br />
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>
                                <div className="data-row go-bold new-account"><input type="submit" id="LoginButton" className="primary-btn login-button floating-right" value="Login" /></div>
                            </div>
                            
                        </div>
                    </div>
                </form>
            </Modal>
      {/* {style}
        <Modal
          open={this.state.show}
          onClose={this.handleClose}

        >
          
          <Header icon='archive' content={this.isArabic?"دخول":"Login"} />
          <Modal.Content>
                    <input
                    autoFocus
                    margin="dense"
                    id="namelog"
                    label=  {this.isArabic?"اسم المستخدم":"User Name"}
                    type="email"
                    value={form.email.value}
                     onChange={this.handelInputChange}
                    />
                    <input
                    autoFocus
                    margin="dense"
                    id="passlog"
                    label=  {this.isArabic?"كلمة السر":"Password"}
                    type="password"
                    onChange={this.handelInputChange}
          
                    name="password" 
                     value={form.password.value} 
                     onChange={this.handelInputChange}

                    onKeyPress={this.onKeyPress}
                    />
          </Modal.Content>
          <Modal.Actions>
            <Button onClick={this.handleClose} color="green">
    {this.isArabic?"إغلاق":"Close"}
            </Button>
			
			            <Button onClick={this.handleRegister} color="green">
    {this.isArabic?"تسجيل":"Register"}
            </Button>
            <Button onClick={this.handelLogin} color="green">
            {this.IsArabic?"دخول":"Login"}
            </Button>
          </Modal.Actions>
        </Modal> */}
		</div>
    );
  }
}