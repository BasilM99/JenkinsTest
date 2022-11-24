import * as React from 'react';
import  { useContext} from 'react';
import BaseComponent from '../../Components/Base/BaseComponent';
import {ApiManager} from "../../Communication/Axios";
//import Relogin from '../Base/ReLogin';




export default class MenuComponent extends BaseComponent{
	constructor(props){
		super(props);
		
		this.state = {
			externalList : null
        };
	};



	componentWillMount = () => {
		//var externalList = null;
        let self=this;
        ApiManager.callwebservice(
            "get",
            "/en/User/GetExternalDataProviderQueryResultAllResultActionResult",
            null,
            null,
            (success) => {
                if(success.data){
                    
                    self.setState({ externalList : success.data});
                    console.log(response);
                }
            },
            (error) => {
              //debugger;
             // showErrorMessage(error.data.msg,true);
             // this.props.setLoadingDataState(false);
                console.log(error);
            },
            1
          );

		// axios.get("/en/User/GetExternalDataProviderQueryResultAllResultActionResult")
		// .then(function(response ){
		//    // handle success
		//    //debugger;
		//    //externalList = response.data;
		// self.setState({ externalList : response.data});
		//    console.log(response);
		// });
	}
	callfunct = (funcName) => {
		console.log(funcName);
		eval(funcName);
	}

	getTranslations = ()  => {
		const element = window.document.querySelector('[data-translations]');
		if (element) {
			return JSON.parse(element.getAttribute('data-translations'));
		}
		
		return {} ;
	};

	showReLogin = () => {
		const  ReLoginObj  = this.getReloginComp();
        ApiManager.callwebservice(
            "get",
            //"/en/dashboard/GetExternalDataProviderQueryResultAllResultActionResult",
            "/en/user/GetExternalDataProviderQueryResultAllResultActionResult",
            null,
            null,
            (success) => {
                if(success.data){
					if( success.request.responseURL.includes("User/Login/?")){
						ReLoginObj.handelShow(true);
					}else{
						self.setState({ externalList : success.data});
					}
					
                    //
                    console.log(response);
                }
            },
            (error) => {
              //debugger;
             // showErrorMessage(error.data.msg,true);
             // this.props.setLoadingDataState(false);
                console.log(error);
			},
			this.getReloginComp
            //1
          );
	}

		
    getReloginComp = () => {
		return this.ReloginComp;
		
    }
	
	reunderMenu = () =>{
		
		return(
			<div data-simplebar className='h-100'>

            {/* <!--- Sidemenu --> */}
            <div id='sidebar-menu'>
                {/* <!-- Left Menu Start --> */}
                <ul className='metismenu list-unstyled' id='side-menu'>
                    <li className='menu-title'>Menu</li>

                    <li>
                        <a href='index.html' className='waves-effect'>
                            <i className='ri-dashboard-line'></i><span className='badge badge-pill badge-success float-right p-1'>3</span>
                            <span>Dashboard</span>
                        </a>
                    </li>

                    <li>
                        <a href='#' className=' waves-effect'>
                            <i className='ri-briefcase-line'></i>
                            <span>Account Information</span>
                        </a>
                    </li>

                    <li>
                        <a href='#' className=' waves-effect'>
                            <i className='ri-file-chart-line'></i>
                            <span>ADM Account Settings</span>
                        </a>
                    </li>

                    <li>
                        <a href='#' className='waves-effect'>
                            <i className='ri-money-dollar-box-line'></i>
                            <span>Account Balance</span>
                        </a>
                    </li>

                    <li>
                        <a href='javascript: void(0);' className='has-arrow waves-effect'>
                            <i className='ri-equalizer-line'></i>
                            <span>General Settings</span>
                        </a>
                        <ul className='sub-menu' aria-expanded='false'>
                            <li><a href='#'>Setting 1</a></li>
                            <li><a href='#'>Setting 2</a></li>
                        </ul>
                    </li>

                    <li>
                        <a href='apps-kanban-board.html' className=' waves-effect'>
                            <i className='ri-eye-line'></i>
                            <span>Audit Trail</span>
                        </a>
                    </li>

                    <li>
                        <a href='#' className='waves-effect'>
                            <i className='ri-exchange-dollar-line'></i>
                            <span>Transaction History</span>
                        </a>
                    </li>

                    <li>
                        <a href='#' className='waves-effect'>
                            <i className='ri-chat-new-line'></i>
                            <span>Invitations</span>
                        </a>
                    </li>

                    <li>
                        <a href='#' className='waves-effect'>
                            <i className='ri-user-3-line'></i>
                            <span>User Management</span>
                        </a>
                    </li>

                </ul>
            </div>
            {/* <!-- Sidebar --> */}
        </div>

		);
	}


	render(){
		
		return (
			<React.Fragment>
{/* 
				<Relogin  	onRef={ref => (this.ReloginComp = ref)}  /> */}
				{this.reunderMenu()}
			</React.Fragment>
		)
	}
}