import * as React from 'react';
import  { useContext,useState,useEffect} from 'react';
import {useLocation} from "react-router-dom";
import BaseComponent from '../../Components/Base/BaseComponent';
import DashBoardCardCompnent from './DashBoardCardComponent';
import ChartContainer from './DashBoardChartComponent';
import AdGeoLocationGridComponent from './AdGeoLocationGridComponent';
import AdPerformanceGridComponent from './AdPerformanceGridComponent';
import MetricComponent from './MetricComponent';
import Select2 from 'react-select2-wrapper';
const queryString = require('query-string');
import { apiCallBegan } from '../../Store/Actions/api';
import axios from 'axios';
import CustomTree  from '../../Components/CustomTree';
import CustomTree2  from '../../Components/AudiencesTree';
import CustomDateRangePicker from '../../Components/CustomDateRangePicker';
import { useSelector, useDispatch } from 'react-redux';

import useLocalization from '../../Hooks/useLocalization';
import { changeLanguage, changeTheme, UISelectors } from '../../Store/ReducerSlices/UI';
import Locales from '../../Localization/Locales';
import { ThemeTypes } from '../../Config/Themes';
import { getCampainsList, getAdvertisersList, getAdvertiserById } from '../../Store/ReducerSlices/Entities';
import Constants from '../../Config/Constants';
import {  userInitialized, userRoleInitialized,ChangeUserRole ,UserSelectors } from '../../Store/ReducerSlices/User';
import { Grid, GridColumn as Column } from "@progress/kendo-react-grid";

import Masterpage from '../Masterpage';
import ReactSelect from 'react-select';
import AsyncReactSelect from 'react-select/async';
//import DateRangePicker2 from 'react-bootstrap-daterangepicker';
//import '@progress/kendo-theme-bootstrap/dist/bootstrap-3.css';
//import '../../Assets/KendoUI/variables.scss';
import '../../Assets/KendoUI/adfalcon.css';
//
import  'react-select2-wrapper/css/select2.css';
//mport  './select2-adminlte.css';
import moment from 'moment';

import { Col, Container, Row } from 'reactstrap';
import { compose } from 'redux';
import { connect } from 'react-redux';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import Impressions from '../../containers/Dashboards/DefaultDashboard/components/Impressions';
import Clicks from '../../containers/Dashboards/DefaultDashboard/components/Clicks';
import MainWrapper from '../../containers/App/MainWrapper';
/* import Conversions from '../../containers/Dashboards/DefaultDashboard/components/Conversions';
import TotalSpend from '../../containers/Dashboards/DefaultDashboard/components/TotalSpend';
import PerformanceAnalytics from '../../containers/Dashboards/DefaultDashboard/components/PerformanceAnalytics';
import BudgetStatistic from '../../containers/Dashboards/DefaultDashboard/components/BudgetStatistic';
import { RTLProps } from '../../shared/prop-types/ReducerProps';
import PerformanceReport from '../../containers/Dashboards/DefaultDashboard/components/PerformanceReport';
 */
// export const DashboardComponent = {
// 	Dashboard
// }

 const  Dashboard = ({}) => {


	const stateStore = useSelector(state => state);
	const UI = UISelectors.getUI(stateStore);
	var [treeData,setTreeState] =useState(
		[]
	  );
   
    const UserGlobal = UserSelectors.getUser(stateStore);
	// constructor(props){
	// 	super(props);

	// 	state = {
	// 		cardList : null,
	// 		advertiserList :[],
	// 		campaignsList :[],
	// 		filters:{
	// 			period:1,
	// 			metric:'Impress',
	// 			metricName:'  Impressions ',
	// 			categoriesTitle : '',
	// 			mainTitle : '',
	// 			categories: [],
	// 			chartData:[]
	// 		}
    //     };
	// };
	// componentDidMount() {
	// 	setTimeout(() => {
	// 		this.getGoogleChartImage();
	// 	}, 1000);
	// }

		const [_metricItems,setMetricItems] = useState([]);
		const [state , setLocalState] = useState(	 {
			cardList : null,
			advertiserList :[],
			campaignsList :[],
			appSiteList :[],
			dealList :[],
			ReportCriteriaList:[],
			filters:{
				period:1,
				metric:'Impress',
				metricName:'  Impressions ',
				categoriesTitle : '',
				mainTitle : '',
				AdvertiserId:null,
				DealId:null,
				CampaignId:null,
				AppSiteId:null,
				countryId:0,
				categories: [],
				chartData:[],
				step: 1,
				rotation:0,

				dashboardType:"",
				FromDate:moment(new Date()),
				ToDate:moment(new Date()),
			}
		});
		const [stateGridData , setstateGridData] = useState(	 {
			skip: 0, take: 10,data:[],Cols:[]
			
		});
		 const handleParentDateChange = (start , end ) => {
			setLocalState({filters:{	FromDate:start,
				ToDate:end 
			
			
			}})
     };
//ans

    // const state = useSelector(state => state);
    // const currentLanguage = UISelectors.getCurrentLanguage(state);
    // const currentTheme = UISelectors.getCurrentTheme(state);

    // const handleLanguageChange = (e) => {
    //     dispatch(changeLanguage({
    //         language: currentLanguage === Locales.ENGLISH ? Locales.ARABIC : Locales.ENGLISH,
    //         direction: currentLanguage === Locales.ENGLISH ? 'rtl' : 'ltr'
    //     }))
    // };

    // const handleChangeTheme = (e) => {
    //     dispatch(changeTheme({
    //         theme: currentTheme === ThemeTypes.LIGHT ? ThemeTypes.DARK : ThemeTypes.LIGHT
    //     }))
	// };
		
	const dispatch = useDispatch();
	
	
    //on load
    useEffect(() => {

        //#region :: Load Advertisers List
		const queryValues = queryString.parse(location.search);
		queryValues.type = "ad";
		state.filters.dashboardType = queryValues.type;

		if( queryValues.chartType)
		queryValues.type = queryValues.chartType;
        /* dispatch(apiCallBegan({
            url: Constants.backend.getMetricsByType+"?type="+queryValues.type,
            method: 'GET',
            onSuccess: getMetricsByType()
        })) */

       
		axios
		.request({
			url:  Constants.backend.getMetricsByType+"?type="+queryValues.type,
			method: 'GET',
		})
		.then(res => getMetricsByType(res))
		.catch(err => console.log('error', err));

       
		axios
		.request({
			url:  "/en/Tree/Get?type=2&factId=1&IncludeId=false&_=1606228546490",
			method: 'GET',
		})
		.then(res =>{
			//debugger;
			treeData = res.data;
			setTreeState(treeData);

		})
		.catch(err => console.log('error', err));

		setLocalState({...state});
    }, []);

	const getMetricsByType = (data) => {
		console.log(data);
		if(typeof(data) != "undefined")
			setMetricItems(data.data);
	}
	const callFunc = (e) =>{
		state.filters.period = e.target.value
		//this.setState({filters.metric:e.target.value});
		getGoogleChartImage();
		fillGrid();
	}
	
const getGoogleChartImage = () => {




	const queryValues = queryString.parse(location.search);
	queryValues.type = "ad";

	if( queryValues.chartType)
	queryValues.type = queryValues.chartType;

    var subId = '';
	var secondsubId = '';
	var AdvertiserAccountId = '';
	var old_Filters = {};
   	 var Id = '';//$("#list").val();
     var period = state.filters.period;
     var metric = state.filters.metric;
     var metricName = state.filters.metricName;



     var formatString = 'short';
    
    var defaultHeight = "70%";

    if (period == "1" || period == "0") {
        defaultHeight = "80%";

	}
	if(queryValues.type =="deal")
	{
		Id=state.filters.DealId;
		subId=state.filters.CampaignId;
	}
	else if(queryValues.type =="appsite" || queryValues.type =="app")
	{
		Id=state.filters.AppSiteId;
	}
	else if(queryValues.type =="ad" || queryValues.type =="campaign")
	{
		Id=state.filters.CampaignId;
	}
	var newDataJson = {
			periodOption: period,
			metricCode: metric,
			id: Id,
			subId: subId,
			secondsubId: secondsubId,
			//CompanyName: CompanyName,
		//	CampName: CampaignName,
			AdvertiserId: state.filters.AdvertiserId,
			type:queryValues.type
		}
	

	//old_Filters = state.filters;
	axios.post( Constants.backend.ChartControlPostUrl , 
		newDataJson)
		.then(function (success) {
			if(success.data){
				state.filters.mainTitle = metricName;
				state.filters.categoriesTitle = success.data.HAxisText;
				state.filters.categories = success.data.ChartDtoList.map(x => x.XaxisString);
				state.filters.chartData = success.data.ChartDtoList.map(x => x.Yaxis);
			}
			setLocalState({...state});
		})
		.catch(function (error) {
			alert("Error loading data! Please try again.");
			console.log(error);
		  }
		);

}

const fillGrid = () => {
    // if ($("#GeoLocationGrid").length>0) {
        // var grid = $("#GeoLocationGrid").data("kendoGrid");

        // grid.dataSource.read();
        // grid.refresh();
    // }
}
	const getCampListForDP = (inputValue,callback) =>{
		var tempList= [];
		if(!inputValue)
		{inputValue='';}
		var advertiserid='';
		if(!!(state.filters.AdvertiserId))
		{
			advertiserid=state.filters.AdvertiserId;
		}
		var dealId='';
		if(!!(state.filters.AdvertiserId))
		{
			dealId=state.filters.DealId;
		}
		
		axios
		.request({
			url:  Constants.backend.getCampList+"?q="+inputValue+"&UserId="+UserGlobal.UserId,
			method: 'GET',
		})
		.then(success => {
			if(success.data){
				tempList =  success.data.map( x => {
					return {
						label: x.Name,
						value: x.Id
					}
				});
				//setState({campaignsList :tempList});
				state.campaignsList = tempList;
				setLocalState({...state});
				callback(tempList);
				//console.log(response);
			
			}
		})
		.catch(err => console.log('error', err));
	
	}
	const handleInputChangeForDeal=(inputValue) =>{
		//debugger;
		//getGoogleChartImage();
		//fillGrid();

	};

	const handleChangeForDeal=(Value) =>{
		//debugger;
		state.filters.CampaignId=null;
		//state.filters.DealId=null;
		state.filters.DealId=Value.value;
			setLocalState({...state});
			getGoogleChartImage();

		//value.Id
		//getGoogleChartImage();
		//fillGrid();

	};

	const handleInputChangeForAppSite=(inputValue) =>{
		//debugger;
		//getGoogleChartImage();
		//fillGrid();

	};

	const handleChangeForAppSite=(Value) =>{
		//debugger;
		state.filters.AppSiteId=Value.value;
		getGoogleChartImage();
		//value.Id
		//getGoogleChartImage();
		//fillGrid();

	};

	
	const handleInputChangeForCampaign=(inputValue) =>{
		//debugger;
		//getGoogleChartImage();
		//fillGrid();

	};

	const handleChangeForCampaign=(Value) =>{
		//debugger;
		
		//value.Id
		//getGoogleChartImage();
		//fillGrid();
		state.filters.CampaignId=Value.value;
		getGoogleChartImage();

	};

	const handleInputChangeForAdvertiser=(inputValue) =>{
		//debugger;
		//getGoogleChartImage();
		//fillGrid();

	};



	const handleChangeForAdvertiser=(Value) =>{
	
		state.filters.AdvertiserId=Value.value;
		state.filters.CampaignId=null;
	state.filters.DealId=null;

		setLocalState({...state});
		//getGoogleChartImage();
		//debugger;
		//getGoogleChartImage();
		//fillGrid();

	};

	const handleInputChangeForCriteria=(inputValue) =>{
		//debugger;
		//getGoogleChartImage();
		//fillGrid();

	};

	const handleChangeForCriteria=(Value) =>{
	
		state.filters.AdvertiserId=Value.value;
		state.filters.CampaignId=null;
		state.filters.DealId=null;

		setLocalState({...state});
		//getGoogleChartImage();
		//debugger;
		//getGoogleChartImage();
		//fillGrid();

	};
	const getAdvListForDP = (inputValue,callback) =>{
		var tempList= [];
		if(!inputValue)
		{inputValue='';}
		
		axios
		.request({
			url:  Constants.backend.getAdvList+"?q="+inputValue+"&UserId="+UserGlobal.UserId,
			method: 'GET',
		})
		.then(success => {
			if(success.data){
				tempList =  success.data.map( x => {
					return {
						label: x.Name,
						value: x.Id
					}
				});
					//state.advertiserList =tempList;
					//this.setState({advertiserList :tempList});
					//setadvertiserList( tempList);
					state.advertiserList = tempList;
					setLocalState({...state});
					callback(tempList);
					//console.log(response);
			}
		})
		.catch(err => console.log('error', err));
	
	}
	const getAppSiteListForDP = (inputValue,callback) =>{
		var tempList= [];
		if(!inputValue)
		{inputValue='';}
		
		
		axios
		.request({
			url:  Constants.backend.getAppSiteList+"?q="+inputValue+"&UserId="+UserGlobal.UserId,
			method: 'GET',
		})
		.then(success => {
			if(success.data){
				tempList =  success.data.map( x => {
				  return {
					  label: x.Name,
					  value: x.Id
				  }
				}
				);
				  
				//state.advertiserList =tempList;
				//this.setState({advertiserList :tempList});
				//setadvertiserList( tempList);
				state.appSiteList = tempList;
				setLocalState({...state});
				callback(tempList);
				//console.log(response);
				  
			}
		})
		.catch(err => console.log('error', err));
	
	}
	const DealListForDP = (inputValue,callback) =>{
		var tempList= [];
		if(!inputValue)
		{inputValue='';}
		var advertiserid='';
		if(!!(state.filters.AdvertiserId))
		{
			advertiserid=state.filters.AdvertiserId;
		}
		
		axios
		.request({
			url:  Constants.backend.getDealList,
			method: 'GET',
			data: {
				q: '',//params.term,
				period:state.filters.period// $("#period").val()
			}
		})
		.then(success => {
			if(success.data){
				tempList =  success.data.map( x => {
					  return {
						  label: x.Name,
						  value: x.Id
					  }
				  });
				  //state.advertiserList =tempList;
				  //this.setState({advertiserList :tempList});
				  //setadvertiserList( tempList);
				  state.dealList = tempList;
				  setLocalState({...state});
				  callback(tempList);
				  //console.log(response);
				  
			}
		})
		.catch(err => console.log('error', err));
	
	}


	const onChangeMetricValue = (event) => {
		console.log(event.target.value);
		state.filters.metric = event.target.value;
		state.filters.metricName = event.target.getAttribute("customtext");
		getGoogleChartImage();
	  }
	  const refreshGrid = () => {
		var gridCom = this.AdGeoLocationGrid;
		return  gridCom;
	}

	const getReportCriteriaForDP = (inputValue,callback) =>{
		var tempList= [];
		if(!inputValue)
		{inputValue='';}
		
		axios
		.request({
			url:  Constants.backend.getReportCriteriaForDashboardList,
			method: 'post',
			data:{
				SectionType:0,
				q:''
			}
		})
		.then(success => {
			if(success.data){
				tempList =  success.data.map( x => {
					return {
						label: x.Name,
						value: x.Id
					}
				});
					//state.advertiserList =tempList;
					//this.setState({advertiserList :tempList});
					//setadvertiserList( tempList);
					state.ReportCriteriaList = tempList;
					setLocalState({...state});
					callback(tempList);
					//console.log(response);
			}
		})
		.catch(err => console.log('error', err));
	}
	const pageChange = (event) => {
        this.setState({
            skip: event.page.skip,
            take: event.page.take
		});
		
		if (item.text.toLowerCase().indexOf(term.toLowerCase()) >= 0) {
			acc.push(item);
		} else if (item.items && item.items.length > 0) {
			let newItems = search(item.items, term);
			if (newItems && newItems.length > 0) {
			  acc.push({
				text: item.text,
				items: newItems,
				expanded: item.expanded
			  });
			}
		}
		  return acc;
		//}, []);
    }
	const reunderDashboardContents = () =>{
		
		return(
			<div className='container-fluid'>

				{/* <!-- start page title --> */}
				<div className='row'>
					<div className='col-12'>
						<div className='page-title-box d-flex align-items-center justify-content-between'>

							<div className='page-title-right'>
								<ol className='breadcrumb m-0'>
									<li className='breadcrumb-item'><a href='javascript: void(0);'>AdFalcon</a></li>
									<li className='breadcrumb-item active'>Dashboard</li>
								</ol>
							</div>

							{/* <!-- App Search--> */}
							<form className='dashboard-search d-inline'>
								<div className='position-relative'>
									<input type='text' className='form-control' placeholder='Search...'/>
									<span className='ri-search-line'></span>
								</div>
							</form>

						</div>
					</div>
				</div>
				{/* <!-- end page title --> */}

				<div className='row'>
					<div className='col-xl-12'>

						{/* <!-- info cards start --> */}
						<div id='info-cards'>
							<div className='row'>
								
								<DashBoardCardCompnent  /> 
							</div>
						</div>
						{/* <!-- info cards end --> */}

								
						<div id="dashboardFilter">
							<div className="row">
								<div className="col-md-3">
									<div className="form-group">
									<label htmlFor="PeriodList">Advertiser</label>
										
										<AsyncReactSelect
										
										/*cacheOptions
										loadOptions={getAdvListForDP}
										defaultOptions
										onInputChange={handleInputChangeForAdvertiser}
										placeholder ="Select Advertiser"
										cacheOptions*/
										//value={setLocalState.filters.AdvertiserId}
										cacheOptions
										defaultOptions
										autoload
        //value={selectedValue}
        getOptionLabel={e => e.label}
        getOptionValue={e => e.value}
        loadOptions={getAdvListForDP}
        onInputChange={handleInputChangeForAdvertiser}
        onChange={handleChangeForAdvertiser}
		placeholder='select Advertiser'	
										/>
									</div>
								</div>
								<div className="col-md-3">
									<div className="form-group">
										<label htmlFor="PeriodList">Campaigns</label>
										
										<AsyncReactSelect
										    key={state.filters.AdvertiserId}
 
										//value={state.filters.CampaignId}

										placeholder='Select Campaign'
										cacheOptions
										defaultOptions
        //value={selectedValue}
        getOptionLabel={e => e.label}
        getOptionValue={e => e.value}
        loadOptions={getCampListForDP}
        onInputChange={handleInputChangeForCampaign}
        onChange={handleChangeForCampaign}

										/>
									</div>
								</div>
								<div className="col-md-3">
									<div className="form-group">
										<label htmlFor="PeriodList">Deals</label>
										
										<AsyncReactSelect

//value={state.filters.DealId}

key={state.filters.CampaignId}
									cacheOptions
										defaultOptions
       
        getOptionLabel={e => e.label}
        getOptionValue={e => e.value}
        loadOptions={DealListForDP}
        onInputChange={handleInputChangeForDeal}
		onChange={handleChangeForDeal}
		placeholder='select deal'
		


										
										/>
									</div>
								</div>
								<div className="col-md-3">
									<div className="form-group">
										<label htmlFor="PeriodList">App Sites</label>
										
										<AsyncReactSelect
										

										//value={state.filters.AppSiteId}

										cacheOptions
										defaultOptions
       
										getOptionLabel={e => e.label}
										getOptionValue={e => e.value}
										loadOptions={getAppSiteListForDP}
										onInputChange={handleInputChangeForAppSite}
										onChange={handleChangeForAppSite}
										placeholder='select AppSite'



										/>
									</div>
								</div>
								
							</div>
							<div className="row">
								<div className="col-md-3">
									<div className="form-group">
										<label htmlFor="PeriodList">Period</label>
										<select className="form-control" id="PeriodList" value={state.filters.period} style={{width:287}} onChange={callFunc}>
											<option value="1">Yesterday </option>
											<option value="2">Last 7 Days</option>
											<option value="3">This Month</option>
											<option value="4">Last Month</option>
										</select>
									</div>
								</div>
								<div className="col-md-9">
									<fieldset className="row form-group" onChange={onChangeMetricValue}>
										<MetricComponent props={_metricItems} />
									</fieldset>
								</div>
							</div>
								
						
						</div>

						<div id='dashboard-chart' className='row'>
							<div className='col-xl-9'>
								<chart>
									<ChartContainer 
										props = {state.filters}  
									  />
								</chart>
							</div>

							<div className='col-xl-3'>
								<div className='card'>
									<div className='card-body'>

										<h4 className='card-title mb-4 font-size-18'>Sales Analytics</h4>

										<div id='donut-chart' className='apex-charts'></div>

										<div className='row'>
											<div className='col-4'>
												<div className='text-center mt-4'>
													<p className='mb-2 text-truncate'><i className='mdi mdi-circle text-primary font-size-10 mr-1'></i> Product A</p>
													<p>42 %</p>
												</div>
											</div>
											<div className='col-4'>
												<div className='text-center mt-4'>
													<p className='mb-2 text-truncate'><i className='mdi mdi-circle text-success font-size-10 mr-1'></i> Product B</p>
													<p>26 %</p>
												</div>
											</div>
											<div className='col-4'>
												<div className='text-center mt-4'>
													<p className='mb-2 text-truncate'><i className='mdi mdi-circle text-warning font-size-10 mr-1'></i> Product C</p>
													<p>42 %</p>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>

						</div>

					</div>
				</div>
				{/* <!-- end row --> */}

				<div className='row mt-4'>
					<div className='col-lg-12'>
					<div className="row">
						<div className="col-md-3">
							<div className="form-group">
								<label htmlFor="PeriodList">Criteria</label>
									
									<AsyncReactSelect
									
									cacheOptions
									defaultOptions
									autoload
									//value={selectedValue}
									getOptionLabel={e => e.label}
									getOptionValue={e => e.value}
									loadOptions={getReportCriteriaForDP}
									onInputChange={handleInputChangeForCriteria}
									onChange={handleChangeForCriteria}
									placeholder='select Criteria'	
									/>
							</div>
						</div>
						<div className="col-md-3">
							
											<CustomTree
											data={treeData}
											//  onAddRuleTarget={getReportCriteriaForDP}
											//  onAddRuleExclude={handleInputChangeForCriteria}
										/>
				
						
						</div>
						<div className="col-md-4">
							 <div className="form-group" style={{marginTop: -4}}>
								<label htmlFor="testDateRange">Date Range</label>
								<CustomDateRangePicker   CallBackParent={handleParentDateChange} id='testDateRange'  Tovalue={state.filters.ToDate}    Fromvalue={state.filters.FromDate }    nameTo='To' nameFrom='From' titleMsgFrom={UI.direction=='rtl'?'من':'From'} titleMsgTo={UI.direction=='rtl'?'الى':'To'} ></CustomDateRangePicker>
							</div>
						</div>
						<div className="col-md-2">
						<div className="form-group">
							<label htmlFor="PeriodList">Country</label>
							<select className="form-control" id="country" name="country"  value={state.filters.countryId} defaultValue={-1} onChange={fillGrid} >
								<option  value="-1">Country</option>
								<option value="932">Afghanistan</option>
								<option value="863">Albania</option>
								<option value="883">Algeria</option>
								<option value="976">American Samoa</option>
								<option value="984">Andorra</option>
								<option value="884">Angola</option>
								<option value="813">Argentina</option>
								<option value="933">Armenia</option>
								<option value="814">Aruba</option>
								<option value="977">Australia</option>
								<option value="840">Austria</option>
								<option value="934">Azerbaijan</option>
								<option value="985">Bahamas</option>
								<option value="935">Bahrain</option>
								<option value="936">Bangladesh</option>
								<option value="815">Barbados</option>
								<option value="864">Belarus</option>
								<option value="841">Belgium</option>
								<option value="816">Belize</option>
								<option value="885">Benin</option>
								<option value="809">Bermuda</option>
								<option value="937">Bhutan</option>
								<option value="817">Bolivia</option>
								<option value="865">Bosnia and Herzegovina</option>
								<option value="886">Botswana</option>
								<option value="818">Brazil</option>
								<option value="938">Brunei Darussalam</option>
								<option value="866">Bulgaria</option>
								<option value="887">Burkina Faso</option>
								<option value="888">Burundi</option>
								<option value="939">Cambodia</option>
								<option value="889">Cameroon</option>
								<option value="810">Canada</option>
								<option value="890">Cape Verde</option>
								<option value="891">Central African Republic</option>
								<option value="892">Chad</option>
								<option value="819">Chile</option>
								<option value="940">China</option>
								<option value="820">Colombia</option>
								<option value="893">Congo</option>
								<option value="894">Congo, Democratic Republic</option>
								<option value="821">Costa Rica</option>
								<option value="895">Cote D'Ivoire</option>
								<option value="867">Croatia</option>
								<option value="986">Cuba</option>
								<option value="941">Cyprus</option>
								<option value="868">Czech Republic</option>
								<option value="842">Denmark</option>
								<option value="896">Djibouti</option>
								<option value="822">Dominican Republic</option>
								<option value="823">Ecuador</option>
								<option value="897">Egypt</option>
								<option value="824">El Salvador</option>
								<option value="898">Equatorial Guinea</option>
								<option value="869">Estonia</option>
								<option value="899">Ethiopia</option>
								<option value="843">Faroe Islands</option>
								<option value="978">Fiji</option>
								<option value="844">Finland</option>
								<option value="845">France</option>
								<option value="979">French Polynesia</option>
								<option value="900">Gabon</option>
								<option value="901">Gambia</option>
								<option value="942">Georgia</option>
								<option value="846">Germany</option>
								<option value="902">Ghana</option>
								<option value="847">Gibraltar</option>
								<option value="848">Greece</option>
								<option value="811">Greenland</option>
								<option value="825">Guadeloupe</option>
								<option value="980">Guam</option>
								<option value="826">Guatemala</option>
								<option value="849">Guernsey</option>
								<option value="903">Guinea</option>
								<option value="904">Guinea-Bissau</option>
								<option value="827">Guyana</option>
								<option value="828">Haiti</option>
								<option value="987">Honduras</option>
								<option value="943">Hong Kong</option>
								<option value="870">Hungary</option>
								<option value="850">Iceland</option>
								<option value="944">India</option>
								<option value="945">Indonesia</option>
								<option value="988">Iran</option>
								<option value="946">Iraq</option>
								<option value="851">Ireland</option>
								<option value="852">Italy</option>
								<option value="829">Jamaica</option>
								<option value="947">Japan</option>
								<option value="948">Jordan</option>
								<option value="949">Kazakhstan</option>
								<option value="905">Kenya</option>
								<option value="950">Korea, Republic of</option>
								<option value="951">Kuwait</option>
								<option value="952">Kyrgyzstan</option>
								<option value="953">Lao People's Democratic Republic</option>
								<option value="871">Latvia</option>
								<option value="954">Lebanon</option>
								<option value="906">Lesotho</option>
								<option value="907">Liberia</option>
								<option value="908">Libyan Arab Jamahiriya</option>
								<option value="872">Lithuania</option>
								<option value="853">Luxembourg</option>
								<option value="955">Macao</option>
								<option value="873">Macedonia, the Former Yugoslav Republic of</option>
								<option value="909">Madagascar</option>
								<option value="910">Malawi</option>
								<option value="956">Malaysia</option>
								<option value="957">Maldives</option>
								<option value="911">Mali</option>
								<option value="854">Malta</option>
								<option value="912">Mauritania</option>
								<option value="913">Mauritius</option>
								<option value="830">Mexico</option>
								<option value="874">Moldova, Republic of</option>
								<option value="958">Mongolia</option>
								<option value="875">Montenegro</option>
								<option value="914">Morocco</option>
								<option value="915">Mozambique</option>
								<option value="10070">Myanmar</option>
								<option value="916">Namibia</option>
								<option value="959">Nepal</option>
								<option value="855">Netherlands</option>
								<option value="989">New Caledonia</option>
								<option value="981">New Zealand</option>
								<option value="990">Nicaragua</option>
								<option value="917">Niger</option>
								<option value="918">Nigeria</option>
								<option value="982">Northern Mariana Islands</option>
								<option value="856">Norway</option>
								<option value="960">Oman</option>
								<option value="961">Pakistan</option>
								<option value="962">Palestinian Territory, Occupied</option>
								<option value="831">Panama</option>
								<option value="991">Papua New Guinea</option>
								<option value="832">Paraguay</option>
								<option value="833">Peru</option>
								<option value="963">Philippines</option>
								<option value="876">Poland</option>
								<option value="857">Portugal</option>
								<option value="834">Puerto Rico</option>
								<option value="964">Qatar</option>
								<option value="919">Reunion</option>
								<option value="877">Romania</option>
								<option value="878">Russian Federation</option>
								<option value="920">Rwanda</option>
								<option value="983">Samoa</option>
								<option value="858">San Marino</option>
								<option value="965">Saudi Arabia</option>
								<option value="921">Senegal</option>
								<option value="879">Serbia</option>
								<option value="922">Seychelles</option>
								<option value="993">Sierra Leone</option>
								<option value="966">Singapore</option>
								<option value="880">Slovakia</option>
								<option value="881">Slovenia</option>
								<option value="923">Somalia</option>
								<option value="924">South Africa</option>
								<option value="859">Spain</option>
								<option value="967">Sri Lanka</option>
								<option value="992">Sudan</option>
								<option value="835">Suriname</option>
								<option value="925">Swaziland</option>
								<option value="860">Sweden</option>
								<option value="861">Switzerland</option>
								<option value="994">Syria</option>
								<option value="968">Taiwan</option>
								<option value="969">Tajikistan</option>
								<option value="926">Tanzania, United Republic of</option>
								<option value="970">Thailand</option>
								<option value="927">Togo</option>
								<option value="836">Trinidad and Tobago</option>
								<option value="928">Tunisia</option>
								<option value="971">Turkey</option>
								<option value="929">Uganda</option>
								<option value="882">Ukraine</option>
								<option value="972">United Arab Emirates</option>
								<option value="862">United Kingdom</option>
								<option value="812">United States</option>
								<option value="837">Uruguay</option>
								<option value="973">Uzbekistan</option>
								<option value="838">Venezuela</option>
								<option value="974">Viet Nam</option>
								<option value="839">Virgin Islands, British</option>
								<option value="975">Yemen</option>
								<option value="930">Zambia</option>
								<option value="931">Zimbabwe</option>
							</select>
						</div>
					</div>
					</div>				
						<div className='row card'>
							<div className='card-body'>
								<div className='dropdown float-right'>
									{/* <a onClick={this.refreshGrid}  className="k-pager-refresh k-link" title="Refresh" aria-label="Refresh"><span className="k-icon k-i-reload"></span></a> */}
								</div>

								<h4 className='card-title mb-4 font-size-18'>By Geo Location</h4>

								<div className='table-responsive'>
									{/* <grid>
										<span className='k-icon k-i-loading'></span>
									</grid> */}
									{/* <GridComponent /> */}
									{/* <AdGeoLocationGridComponent onRef={ref => (this.AdGeoLocationGrid = ref)}/> */}
									<AdGeoLocationGridComponent props={state.filters}/> 
								</div>
							</div>
						</div>
					</div>
				</div>
				{/* <!-- end row --> */}

				<div className='row mt-4'>
					<div className='col-lg-12'>
						<div className='card'>
							<div className='card-body' >
					
									<a href='#' className='dropdown-toggle arrow-none card-drop' data-toggle='dropdown' aria-expanded='false'>
										<i className='mdi mdi-dots-vertical'></i>
									</a>
									<div className='dropdown-menu dropdown-menu-right'>
										{/* <!-- item--> */}
										<a href='javascript:void(0);' className='dropdown-item'>Sales Report</a>
										{/* <!-- item--> */}
										<a href='javascript:void(0);' className='dropdown-item'>Export Report</a>
										{/* <!-- item--> */}
										<a href='javascript:void(0);' className='dropdown-item'>Profit</a>
										{/* <!-- item--> */}
										<a href='javascript:void(0);' className='dropdown-item'>Action</a>
									</div>
								</div>

								
							
						</div>
					
					
						<h4 className='card-title mb-4 font-size-18'>Campaign Performance Details Today</h4>

<div className='table-responsive'>
	{/* <grid>
		<span className='k-icon k-i-loading'></span>
	</grid> */}
	{/* <GridComponent /> */}
	<Grid
                    style={{ height: '400px' }}
                    data={stateGridData.data}
                    skip={stateGridData.skip}
                    take={stateGridData.take}
                    total={stateGridData.Count}
                    pageable={true}
                    onPageChange={pageChange}
                >
{stateGridData.Cols.map(item => {
                   return <GridColumn field={item}    title={item} />
                  
					
})}
                </Grid>
</div>
					</div>
				</div>
				{/* <!-- end row --> */}
{/* 									
                <button onClick={this.handleChangeTheme}>{currentTheme}</button>
                <button onClick={this.handleLanguageChange}>{currentLanguage === Locales.ENGLISH ? Locales.ARABIC : Locales.ENGLISH}</button> */}
			</div>
			//  <!-- container-fluid -->

		)
	}


	return <>

<MainWrapper>
			<Masterpage>

				{reunderDashboardContents()}


				<Container className="dashboard">
    <Row>
      <Col md={12}>
        <h3 className="page-title">hhfgh</h3>
      </Col>
    </Row>
    <Row>
      <Impressions />
      <Clicks />
    
    </Row>
    <Row>
      
    </Row>
    <Row>
   
    </Row>
  </Container>
			</Masterpage>
	</MainWrapper>
	</> 


}

export default Dashboard;