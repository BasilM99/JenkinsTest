import React from 'react';
import { Col,Input ,Container, Row, Label, Card, CardBody, Button, ButtonToolbar, ButtonGroup } from 'reactstrap';
import {
	BarChart, Bar, Cell, ResponsiveContainer,
} from 'recharts';
import PropTypes from 'prop-types';
import { RTLProps } from '../../../shared/prop-types/ReducerProps';
import { useContext, useState, useEffect } from 'react';
import { useLocation } from "react-router-dom";
import DashBoardCardCompnent from '../../../Components/DashBoardCardList';
import ChartContainer from '../../../Pages/DashBoard/DashBoardChartComponent';
import AdGeoLocationGridComponent from '../../../Pages/DashBoard/AdGeoLocationGridComponent';
import MetricComponent from '../../../Pages/DashBoard/MetricComponent';
import { trackPromise} from 'react-promise-tracker';
const queryString = require('query-string');

import axios from 'axios';
import CustomTree from '../../../Components/CustomTree';
import CustomDateRangePicker from '../../../Components/CustomDateRangePicker';
import { useSelector, useDispatch } from 'react-redux';
import useLocalization from '../../../Hooks/useLocalization';
import { changeLanguage, changeTheme, UISelectors } from '../../../Store/ReducerSlices/UI';
import Locales from '../../../Localization/Locales';
import { ThemeTypes } from '../../../Config/Themes';
import { getCampainsList, getAdvertisersList, getAdvertiserById } from '../../../Store/ReducerSlices/Entities';
import Constants from '../../../Config/Constants';
import { UserPermissionCode } from '../../../Config/Enums';
import { userInitialized, userRoleInitialized, ChangeUserRole, UserSelectors, IsUserPermitted } from '../../../Store/ReducerSlices/User';
import { Grid, GridColumn } from "@progress/kendo-react-grid";
import { DimensionsSearchCode } from "../../../Config/Enums";
import { KPIScope } from "../../../Config/Enums";

import {
	DropdownItem, DropdownMenu, DropdownToggle, UncontrolledDropdown, Table,
} from 'reactstrap';
import { BasicNotification,showBasicNotificationForAdFalcon } from '../../../Components/Notification';
import ReactSelect from 'react-select';
import AsyncReactSelect, { Async } from 'react-select/async';
import moment from 'moment';
import { compose } from 'redux';
import { connect } from 'react-redux';
import { withTranslation } from 'react-i18next';
import { CustomColumnMenu } from '../../../Components/ColumnMenu';
import useCurrentUser from '../../../Hooks/useCurrentUser';

import LayersPlus from 'mdi-react/LayersPlusIcon';
 
import useMyCurrentUI from '../../../Hooks/useMyCurrentUI';


import { useForm,Controller } from "react-hook-form";
import  yup from "../../../Config/YUP"

import { yupResolver } from '@hookform/resolvers/yup';
import  reacthookformValues from "../../../Config/reacthookform"
import { getFormValues } from 'redux-form';
import LoadingSpinner ,{ Spinner} from '../../../Components/LoadingSpinner';


const DefaultDashboard = ({ t, rtl }) => {
	const validationSchemayup = 
	yup.object().shape({
			Name: yup.string().required(),
			
		  });
	reacthookformValues.resolver=yupResolver(validationSchemayup);
    const { handleSubmit, errors, register,control,setValue, reset,setError,  getValues, formState: {isValid , isDirty, isSubmitting, touched, submitCount }} = useForm({...reacthookformValues   });
    

    
  const [showAddRow,setshowAddRow] =useState(false);
const toggle =()=>{
	setshowAddRow(false);

  //reset();
  //props.CallBackToggle()
}
	const { IsUserPermitted, IsAdmin ,UserGlobal} = useCurrentUser();
	const {  getDirection, getLanguage, getTheme ,UI} = useMyCurrentUI();

	const { T,Resources} = useLocalization();
	//on load
	useEffect(() => {

		//#region :: Load Advertisers List
		const queryValues = queryString.parse(location.search);
		let type = "ad";
		state.filters.dashboardType = queryValues.type;

		if (queryValues.chartType && typeof (queryValues.chartType !== "undefined"))
			type = queryValues.chartType;
		if (queryValues.ChartType && typeof (queryValues.ChartType !== "undefined"))
			type = queryValues.ChartType;

		if (queryValues.charttype && typeof (queryValues.charttype !== "undefined"))
			type = queryValues.charttype;
		state.ChartType = type;
		setLocalState(state);
		getDefaulKPISelectedList();
		Promise.allSettled([ getMetricsByTypeRequestAsync(), getMeasureTreeAsync(), getDimensTreeAsync()]).then(RES => {

			getGoogleChartImage();
			//FillDataGrid(0,10);
		}
		)

	}, []);

	const [campKeyFilter,setcampKeyFilter] = useState(-1);
	const [treeMeasuresData, setMeasuresTreeState] = useState(
		[]
	);
	const [treeDimensionsData, setDimensionsTreeState] = useState(
		[]
	);
	const [activeCode, setactiveCode] = useState('');
	const [_metricItems, setMetricItems] = useState([]);
	const [state, setLocalState] = useState({
		cardList: null,
		advertiserList: [],
		FirstLoading: false,
		campaignsList: [],
		appSiteList: [],
		CriteriaList: [],
		activeCode: null,
		dealList: [],
		ReportCriteriaList: [],
		cardsList: [],
		CanViewDeals: IsUserPermitted( UserPermissionCode.PMPDeal) ||IsAdmin(),
		CountryOptions: [],
		ChartType: 'ad',
		AdvertiserId: null,
		DealId: null,
		CampaignId: null,
		AppSiteId: null,

		filters: {
			period: 1,
			metric: 'Impress',
			metricName: '  Impressions ',
			categoriesTitle: '',
			mainTitle: '',
			AdvertiserId: null,
			DealId: null,
			CampaignId: null,
			AppSiteId: null,
			CriteriaId: null,
			selectedCountryOption: null,
			countryId: 0,
			categories: [],
			chartData: [],
			step: 1,
			rotation: 0,
			dashboardType: "",
			ColumnsIds: [],
			MeasuresIds: [],
			FromDate: moment().subtract(6, 'days')  ,
			ToDate:moment(new Date()) ,
		}
	});
	const [stateGridData, setstateGridData] = useState({
		skip: 0, take: 10, data: [], Cols: []

	});

	const [ReportCriteriaListAll,setReportCriteriaListAll] = useState([]);
	const [CriteriaSelected,setCriteriaSelected] = useState(null);
	const [CriteriaSelectedToBeSelected,setCriteriaSelectedToBeSelected] = useState(null);
	
	const [TypeSave,setTypeSave] = useState('');
	
	const handleParentDateChange = (start, end) => {

		const newFormData = Object.assign({}, state);
		state.filters.FromDate = start;
		state.filters.ToDate = end;
		setLocalState(state);
		getGoogleChartImage();
		FillDataGrid(0, 10);
	};
	const handleParentChangeDim = (check) => {

		const newFormData = Object.assign({}, state);
		state.filters.ColumnsIds = check;
		setLocalState(state);

		FillDataGrid(0, 10);
	};
	const handleParentChangeMeasure = (check) => {

		const newFormData = Object.assign({}, state);
		state.filters.MeasuresIds = check;
		setLocalState(state);
		FillDataGrid(0, 10);
	};

	//const dispatch = useDispatch();
	const getMetricsByTypeRequest = () => {
		try {
			axios
				.request({
					url: Constants.backend.getMetricsByType + "?type=" + state.ChartType,
					method: 'GET',
				}).then(response => { getMetricsByType(response.data); });
		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};

	const getDashboardCardDataLis = () => {
		try {
			axios
				.request({
					url: Constants.backend.getDashboardCardDataList + "?chartType=" + state.ChartType,
					method: 'GET',
				}).then(response => {
					let cardsList = response.data.data;


					const newFormData = Object.assign({}, state);
					state.cardsList = cardsList;
					//newFormData.filters.ToDate=end;
					setLocalState(state);
					//setLocalState({...state,cardsList:cardsList});
				});

		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};
	const getDimensTree = () => {
		try {

			axios
				.request({
					url: Constants.backend.getDimTree,
					method: 'GET',
				}).then(response => {
					let treeDimensionsData = response.data;
					setDimensionsTreeState(treeDimensionsData);
				});
		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};
	const getMeasureTree = () => {
		try {
			axios
				.request({
					url: Constants.backend.getMeasureTree,
					method: 'GET',
				}).then(response => {
					const treeMeasuresData = response.data;
					setMeasuresTreeState(treeMeasuresData);
				});

		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};

	const getMetricsByTypeRequestAsync = async() => {
		try {
		 let response = await	axios.request({
					url: Constants.backend.getMetricsByType + "?type=" + state.ChartType,
					method: 'GET',
				});
				await getMetricsByType(response.data);
				return response;
		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};
const [KPIList,setKPIList ] = useState([]);
	const getDashboardCardDataLisAsync = async () => {
		try {
		let response=	await axios
				.request({
					url: Constants.backend.getDashboardCardDataList + "?chartType=" + state.ChartType,
					method: 'GET',
				});
				let cardsList = response.data.data;


					const newFormData = Object.assign({}, state);
					state.cardsList = cardsList;
				
					debugger;
					 setKPIList(  response.data.KPIConfig);
					//newFormData.filters.ToDate=end;
				await	setLocalState(state);
				return response;

		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};
	const getDimensTreeAsync = async() => {
		try {

			let response = await axios
				.request({
					url: Constants.backend.getDimTree,
					method: 'GET',
				});
				let treeDimensionsData = response.data;
				await setDimensionsTreeState(treeDimensionsData);
				return response;
		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};
	const getMeasureTreeAsync = async() => {
		try {
			let response = await axios
				.request({
					url: Constants.backend.getMeasureTree,
					method: 'GET',
				});
				const treeMeasuresData = response.data;
				await setMeasuresTreeState(treeMeasuresData);
				return response;

		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};

	const getCountryList = () => {

		try {
			axios
				.request({
					url: Constants.backend.getCountres,
					method: 'GET',
				}).then(response => {
					let countryList = [];

					response.data.map(x => {
						countryList.push({
							label: x.Text,
							value: x.Value
						});
					})
					state.CountryOptions = countryList;
					setLocalState(state);
					//return response ;
				});
		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	}

	const getMetricsByType = (data) => {
		console.log(data);
		if (typeof (data) != "undefined") {
			data[0].active = true;
			state.activeCode = data[0].Code;
			//activeCode = data[0].Code;
			setLocalState(state);
			setactiveCode( data[0].Code);
			setMetricItems(data);
		}
	}
	const callFunc = (e) => {
		state.filters.period = e.target.value
		//this.setState({filters.metric:e.target.value});
		getGoogleChartImage();
		fillGrid();
	}

	const getGoogleChartImage = () => {

		const queryValues = queryString.parse(location.search);
		let type = "ad";

		if (queryValues.chartType)
			type = queryValues.chartType;

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
		if (type == "deal") {
			Id = state.filters.DealId;
			subId = state.filters.CampaignId;
		}
		else if (type == "appsite" || type == "app") {
			Id = state.filters.AppSiteId;
		}
		else if (type == "ad" || type == "campaign") {
			Id = state.filters.CampaignId;
		}
		if (typeof Id === 'undefined') {

			Id = '';
		}

		if (Id == null) {

			Id = '';
		}
		var newDataJson = {
			periodOption: period,
			metricCode: metric,
			id: Id + "",
			subId: subId,
			secondsubId: secondsubId,
			//CompanyName: CompanyName,
			//	CampName: CampaignName,

			type: state.ChartType,
			from: state.filters.FromDate.format("DD-MM-YYYY"),
			to: state.filters.ToDate.format("DD-MM-YYYY")
		}
		if (state.filters.AdvertiserId && typeof state.filters.AdvertiserId !== 'undefined') {
			newDataJson.AdvertiserId = state.filters.AdvertiserId;

		}

		//old_Filters = state.filters;
		axios.post(Constants.backend.ChartControlPostUrl,
			newDataJson)
			.then(function (success) {
				if (success.data) {
					const newFormData = Object.assign({}, state);

					//setLocalState(newFormData);

					state.filters.mainTitle = metricName;
					state.filters.categoriesTitle = success.data.HAxisText;
					state.filters.categories = success.data.ChartDtoList.map(x => x.XaxisString);
					state.filters.chartData = success.data.ChartDtoList.map(x => {
						if (x.Yaxis)
							return x.Yaxis
						else
							return 0;
					}
					);

					state.filters.step = success.data.showTextEvery;
					state.filters.rotation = success.data.slantedTextAngle;
					setLocalState({ ...state });
					console.log("showTextEvery:" + success.data.showTextEvery);
					console.log("slantedTextAngle:" + success.data.slantedTextAngle);
				}

			})
			.catch(function (error) {
				//alert("Error loading data! Please try again.");
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
	const getCampListForDP = (inputValue, callback) => {
		var tempList = [];
		if (!inputValue) { inputValue = ''; }

		let urlCamp = Constants.backend.getCampList + "?q=" + inputValue + "&UserId=" + UserGlobal.UserId
		var advertiserid = '';
		if (!!(state.filters.AdvertiserId)) {
			advertiserid = state.filters.AdvertiserId;
			urlCamp = urlCamp + "&AdvertiserAccountId=" + advertiserid;
		}
		var dealId = '';
		if (!!(state.filters.DealId)) {
			dealId = state.filters.DealId;
			urlCamp = urlCamp + "&dealId=" + dealId;
		}

		axios
			.request({
				url: urlCamp,
				method: 'GET',
			})
			.then(success => {
				if (success.data) {
					tempList = success.data.map(x => {
						return {
							label: x.Name,
							value: x.Id
						}
					});
					state.campaignsList = tempList;
					setLocalState(state);
					callback(tempList);
				}
			})
			.catch(err => console.log('error', err));

	}
	const handleInputChangeForDeal = (inputValue) => {
		//debugger;
		//getGoogleChartImage();
		//fillGrid();
	};

	const handleChangeForDeal = (Value) => {
		const newFormData = Object.assign({}, state);
		state.filters.DealI =null;
		if(state.filters.DealId !=null)
		state.filters.DealId = Value.value;
		setcampKeyFilter(		state.filters.DealId );
		state.filters.CampaignId = null;
		setLocalState(state);
		FillDataGrid(0, 10);
		getGoogleChartImage();
	};

	const handleInputChangeForAppSite = (inputValue) => {
		//debugger;
		//getGoogleChartImage();
		//fillGrid();
	};

	const handleChangeForAppSite = (Value) => {
		const newFormData = Object.assign({}, state);
		state.filters.AppSiteId =null;
		if(Value!=null)
		state.filters.AppSiteId = Value.value;
		setLocalState(state);
		FillDataGrid(0, 10);
		getGoogleChartImage();
	};

	const handleInputChangeForCampaign = (inputValue) => {
		//debugger;
		//getGoogleChartImage();
		//fillGrid();
	};

	const handleChangeForCampaign = (Value) => {
		const newFormData = Object.assign({}, state);
		state.filters.CampaignId =null;
		if(Value!=null)
		state.filters.CampaignId = Value.value;
		setLocalState(state);
		FillDataGrid(0, 10);
		getGoogleChartImage();
	};

	const handleInputChangeForAdvertiser = (inputValue) => {
		//debugger;
		//getGoogleChartImage();
		//fillGrid();
	};

	const handleChangeForAdvertiser = (Value) => {
		const newFormData = Object.assign({}, state);
		state.filters.AdvertiserId =null;
		if(Value!=null)
		state.filters.AdvertiserId = Value.value;
		setcampKeyFilter(		state.filters.AdvertiserId );
		state.filters.CampaignId = null;
		state.filters.DealId = null;
		setLocalState(state);
		FillDataGrid(0, 10);
		getGoogleChartImage();
	};

	const handleInputChangeForCriteria = (inputValue) => {
		//debugger;
		//getGoogleChartImage();
		//fillGrid();
	};

	const handleChangeForCriteria = (Value) => {
		//debugger;
		const newFormData = Object.assign({}, state);
		state.filters.CriteriaId = Value.value;
		state.filters.ColumnsIds = [];
		state.filters.MeasuresIds = [];
		setLocalState(state);
		setCriteriaSelected(Value);
		setCriteriaSelectedToBeSelected(null);
		setshowAddRow(false);

	let object=	ReportCriteriaListAll.find(x=>x.ID == parseInt(Value.value) );
	//setValue("Name", tempList[0].label, { shouldValidate: true });
						state.filters.MeasuresIds = object.MeasuresIds.map(el => el);
						state.filters.ColumnsIds = object.ColumnsIds.map(el => el);
					//	state.ReportCriteriaList = tempList;
						state.filters.CriteriaId = object.ID;
			setLocalState({...state});
		//newFormData.CriteriaSelected=null;

		/* 	axios
			.request({
				url:  Constants.backend.Ge+"?q="+inputValue+"&UserId="+UserGlobal.UserId,
				method: 'GET',
			})
			.then(success => {
				
				
				newFormData.filters.ColumnsIds=success.data[0].ColumnsIds
			newFormData.filters.MeasuresIds=	success.data[0].MeasuresIds;
			setLocalState(newFormData);
				FillDataGrid(0,10);
			getGoogleChartImage();
				
			})
			.catch(err => console.log('error', err)); */

		//debugger;
		//getGoogleChartImage();
		//fillGrid();

	};
	const getAdvListForDP = (inputValue, callback) => {
		var tempList = [];
		if (!inputValue) { inputValue = ''; }

		axios
			.request({
				url: Constants.backend.getAdvList + "?q=" + inputValue + "&UserId=" + UserGlobal.UserId,
				method: 'GET',
			})
			.then(success => {
				if (success.data) {
					tempList = success.data.map(x => {
						return {
							label: x.Name,
							value: x.Id
						}
					});
					//state.advertiserList =tempList;
					//this.setState({advertiserList :tempList});
					//setadvertiserList( tempList);
					state.advertiserList = tempList;
					setLocalState(state);
					callback(tempList);
					//console.log(response);
				}
			})
			.catch(err => console.log('error', err));
	}

	const getCountryListForDP = (inputValue, callback) => {
		var tempList = [];
		if (!inputValue) { inputValue = ''; }

		if (state.CountryOptions.length == 0) {
			axios
				.request({
					url: Constants.backend.getCountres,
					method: 'GET',
				}).then(response => {
					let countryList = [];

					response.data.map(x => {
						countryList.push({
							label: x.Text,
							value: x.Value
						});
					})

					//return response ;
					callback(countryList);
					state.CountryOptions = countryList;
					setLocalState({ ...state, CountryOptions: countryList });
				});
		}
		else {

			let newitems = state.CountryOptions.reduce((acc, item) => {
				if (item.label.toLowerCase().indexOf(inputValue.toLowerCase()) >= 0 || (inputValue === undefined || inputValue === null || inputValue === '')) {
					acc.push(item);
				}

				return acc;
			}, []);
			callback(newitems);

		}
	}
	const getAppSiteListForDP = (inputValue, callback) => {
		var tempList = [];
		if (!inputValue) { inputValue = ''; }

		axios
			.request({
				url: Constants.backend.getAppSiteList + "?q=" + inputValue + "&UserId=" + UserGlobal.UserId,
				method: 'GET',
			})
			.then(success => {
				if (success.data) {
					tempList = success.data.map(x => {
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
					setLocalState(state);
					callback(tempList);
					//console.log(response);
				}
			})
			.catch(err => console.log('error', err));

	}
	const DealListForDP = (inputValue, callback) => {
		var tempList = [];
		if (!inputValue) { inputValue = ''; }
		let urlDeal = Constants.backend.getDealList + "?q=" + inputValue + "&UserId=" + UserGlobal.UserId
		var advertiserid = '';
		if (!!(state.filters.AdvertiserId)) {
			advertiserid = state.filters.AdvertiserId;
			urlDeal = urlDeal + "&AdvertiserAccountId=" + advertiserid;
		}

		axios
			.request({
				url: urlDeal,
				method: 'GET',
				data: {
					q: '',//params.term,
					period: state.filters.period// $("#period").val()
				}
			})
			.then(success => {
				if (success.data) {
					tempList = success.data.map(x => {
						return {
							label: x.Name,
							value: x.Id
						}
					});
					//state.advertiserList =tempList;
					//this.setState({advertiserList :tempList});
					//setadvertiserList( tempList);
					state.dealList = tempList;
					setLocalState(state);
					callback(tempList);
					//console.log(response);
				}
			})
			.catch(err => console.log('error', err));

	}

	const onChangeMetricValue = (event) => {
		console.log(event.target.value);
		//state.filters.metric = event.target.value;
		//state.filters.metricName = event.target.getAttribute("customtext");

		const newFormData = Object.assign({}, state);
		state.filters.metric = event.target.value;
		state.filters.metricName = event.target.getAttribute("customtext");
		//newFormData.filters.mainTitle=customtext;
		setLocalState(state);

		getGoogleChartImage();
	}
	const refreshGrid = () => {
		var gridCom = this.AdGeoLocationGrid;
		return gridCom;
	}

	const onClickMetricValue = (value, customtext) => {
		console.log(value);

		const newFormData = Object.assign({}, state);
		state.filters.metric = value;
		state.filters.metricName = customtext;
		state.filters.mainTitle = customtext;
		state.FirstLoading = true;
		state.activeCode = value;
		//activeCode=value;

		setLocalState(state);
setactiveCode(value);

		getGoogleChartImage();
	}

	const getReportCriteriaForDP = (inputValue, callback) => {
		var tempList = [];
		if (!inputValue) { inputValue = ''; }
		//debugger
		if (state.ReportCriteriaList.length == 0  ||CriteriaSelected==null) {
			axios
				.request({
					url: Constants.backend.getReportCriteriaForDashboardList + "?chartType=" + state.ChartType + "&UserId=" + UserGlobal.UserId,
					method: 'get',

				})
				.then(success => {
					if (success.data) {
						tempList = success.data.map(x => {
							return {
								label: x.Name,
								value: x.ID
							}
						});

						//	setLocalState({...state , ReportCriteriaList: tempList});
					
						callback(tempList);
						const newFormData = Object.assign({}, state);

						//CriteriaSelected = tempList[0];
						setReportCriteriaListAll(success.data);
					

						//setValue("Name", tempList[0].label, { shouldValidate: true });
						state.filters.MeasuresIds = success.data[0].MeasuresIds.map(el => el);
						state.filters.ColumnsIds = success.data[0].ColumnsIds.map(el => el);
						state.ReportCriteriaList = tempList;
						state.filters.CriteriaId = success.data[0].ID;
						if(CriteriaSelectedToBeSelected==null)
						{
							setCriteriaSelected(tempList[0]);
							CriteriaSelected=tempList[0];
						}
						else{
							let object= tempList
							.find((e) => e.value === CriteriaSelectedToBeSelected)
						
							setCriteriaSelected(object);
							CriteriaSelected=object;
							state.filters.MeasuresIds = object.MeasuresIds.map(el => el);
							state.filters.ColumnsIds = object.ColumnsIds.map(el => el);
						
							state.filters.CriteriaId = object.ID;

						}
					

						setLocalState(state);
						
						FillDataGrid(0,10);
						//console.log(response);
					}
				})
				.catch(err => console.log('error', err));
		}
		else {

			let newitems = state.ReportCriteriaList.reduce((acc, item) => {
				if (item.label.toLowerCase().indexOf(inputValue.toLowerCase()) >= 0 || (inputValue === undefined || inputValue === null || inputValue === '')) {
					acc.push(item);
				}

				return acc;
			}, []);
			callback(newitems);

		}
	}
	const handleCountryChange = (e) => {

		const newFormData = Object.assign({}, state);
		state.filters.countryId =null;
		if(e!=null)
		state.filters.countryId = e.value;
		state.selectedCountryOption = e;
		setLocalState(state);


		FillDataGrid(0, 10);
	}
	const pageChange = (event) => {

		FillDataGrid(event.skip, event.take);
	}

	const  KPICardSelectedId=(val)=>{

		let data =dataTobeSentToKPICards();
		data.KpiConfigId=val;
		getDashboardCardDataListPostAsync(data);
	}
	const getDefaulKPISelectedList =()=>{

		let data =dataTobeSentToKPICards();
		//data.KPIId=val;
		getDashboardCardDataListPostAsync(data);

	}
	const [CardsList,setCardsList] =useState([]);
	const [newCard,setnewCard] =useState(null);
	
	const getDashboardCardDataListPostAsync = async (dataToBeSent) => {
		try {

		let urlApi=	Constants.backend.GetDefaultKPIs;
			if(dataToBeSent.KpiConfigId && parseInt(dataToBeSent.KpiConfigId) )
			urlApi=	Constants.backend.GetKPI;
			dataToBeSent = JSON.stringify(dataToBeSent);
			debugger;


		let response=	await  trackPromise( axios
				.request({
					url:urlApi,
					method: 'POST',
			data: dataToBeSent
			,headers: {
				'Content-Type': 'application/json',
				"Accept": "application/json"
			}
				}
				
				
				),"CardsListKPI")
				
				
				;
				


					//const newFormData = Object.assign({}, state);
					//state.cardsList = cardsList;
					if(!!response.data.defaultKPIs)
					  { let cardsList = response.data.defaultKPIs;
						  
						setCardsList(cardsList);
					
					}
					else
					{


						let singleCard = response.data;
						//let newarr = [];
						//for(var i=0; i<CardsList.length; i++)
						//{
					//		newarr.push(CardsList[i]);
						//}
						//let newFormData = Object.assign({}, CardsList);
						
						//newarr.push(singleCard);
						setnewCard(singleCard);

					}

					   if( !!response.data.AllKPIConfigs)
					await setKPIList(  response.data.AllKPIConfigs);
				
					
					//newFormData.filters.ToDate=end;
			//	await	setLocalState(state);
				return response;

		} catch (err) {
			// Handle Error Here
			console.error(err);
		}
	};
	const dataTobeSentToKPICards =()=>{


		let dataToBeSent = {
			
		
			"FromDate": state.filters.FromDate.format("DD-MM-YYYY"),
			

			"ToDate": state.filters.ToDate.format("DD-MM-YYYY"),
			
			Ids:[],

			KPIScope:KPIScope.undefined,
			KpiConfigId:null


		}


		if (state.ChartType.toLowerCase() == "campaign" ||  state.ChartType.toLowerCase() == "ad" )
		{
			
			dataToBeSent.KPIScope =KPIScope.Advertiser;

		}

		if (state.ChartType.toLowerCase() == "appsite" || state.ChartType.toLowerCase() == "appsite")
		{
		
			dataToBeSent.KPIScope =KPIScope.Publisher;

		}
		if (state.ChartType.toLowerCase() == "deal" || state.ChartType.toLowerCase() == "deals")
		{
			
			dataToBeSent.KPIScope =KPIScope.Deals;

		}

		if (state.ChartType.toLowerCase() == "impressionlog" || state.ChartType.toLowerCase() == "impressionlog")
		{
			
			dataToBeSent.KPIScope =KPIScope.DataProvider;

		}


		if (state.filters.DealId != null && typeof state.filters.DealId !== 'undefined'  &&  parseInt( state.filters.DealId ) >0)
		dataToBeSent.Ids.push(state.filters.DealId);

		if (state.filters.AdvertiserId != null && typeof state.filters.AdvertiserId !== 'undefined'   &&  parseInt( state.filters.AdvertiserId ) >0)
		dataToBeSent.Ids.push(state.filters.AdvertiserId);
		if (state.filters.CampaignId != null && typeof state.filters.CampaignId !== 'undefined'   &&  parseInt( state.filters.CampaignId ) >0)
		dataToBeSent.Ids.push(state.filters.CampaignId);

		
		if (state.filters.AppSiteId != null && typeof state.filters.AppSiteId !== 'undefined'  &&  parseInt( state.filters.DealId ) >0)
		dataToBeSent.Ids.push(state.filters.AppSiteId);



		//dataToBeSent = JSON.stringify(dataToBeSent);
		return dataToBeSent;

	}


	const dataTobeSentToReportCriteria =()=>{


		let dataToBeSent = {
			
		

				
			ColumnsIds: state.filters.ColumnsIds,


			MeasuresIds: state.filters.MeasuresIds,

			Name:getValues("Name"),
			ID:CriteriaSelected.value,
			TypeSave:TypeSave,
			chartType:state.ChartType,
			
			


		}

		dataToBeSent = JSON.stringify(dataToBeSent);
		return dataToBeSent;

	}

	const SaveAddCriteria = (data, e) =>{
       

		return axios
		.request({	url: Constants.backend.DashboardCriteriaSaveUrl,
			method: 'POST',
			data: dataTobeSentToReportCriteria()
			,headers: {
				'Content-Type': 'application/json',
				"Accept": "application/json"
			}
		
		})
		.then(success => {
		  if(success.data && success.data.status=="businessException"){
		 
		
 
	
		  setError('serverError', {message:success.data.Message});
 
	
		  }
		  else if(success.data && success.data.Value>0)
		  {
			setTypeSave('');
			showBasicNotificationForAdFalcon("Dashboard Criteria","you have succsfully added/updated new Criteria","success","right-up");
			setshowAddRow(false);
	
			if(success.data.Value != parseInt(CriteriaSelected.value))
			{		//CriteriaSelectedToBeSelected=success.data.Value;
				setCriteriaSelectedToBeSelected(success.data.Value );
				setshowAddRow(false);
				setCriteriaSelected(null);
		
			
			

		
				//setLocalState({...state, CriteriaSelected:null});

			}
		  }
		  if(success.data && success.data.status=="faild"){
			//debugger
		   
	
	   
			 setError('serverError', {message:success.data.status});
	
	   
			 }
		})
		.catch(err => 
		   { 
			throw err;

			console.log('error', err)
		   }
		);
     
   }
	const FillDataGrid = (skip, take) => {
		let dataToBeSent = {

			ColumnsIdsString: state.filters.ColumnsIds.join(","),


			"MeasuresIdsString": state.filters.MeasuresIds.join(","),
			"QueryJsonData": {},
			"fact": 1,
			"from": state.filters.FromDate.format("DD-MM-YYYY"),
			"function": 1,
			"page": (skip / take),
			//pageNumber: 0,
			"size": take,

			"to": state.filters.ToDate.format("DD-MM-YYYY")
		};
		if (state.filters.DealId != null && typeof state.filters.DealId !== 'undefined'  &&  parseInt( state.filters.DealId ) >0)
			dataToBeSent.QueryJsonData[DimensionsSearchCode.Deal + ""] = state.filters.DealId;

		if (state.filters.AdvertiserId != null && typeof state.filters.AdvertiserId !== 'undefined'   &&  parseInt( state.filters.AdvertiserId ) >0)
			dataToBeSent.QueryJsonData[DimensionsSearchCode.Advertiser + ""] = state.filters.AdvertiserId;
		if (state.filters.CampaignId != null && typeof state.filters.CampaignId !== 'undefined'   &&  parseInt( state.filters.CampaignId ) >0)
			dataToBeSent.QueryJsonData[DimensionsSearchCode.Campaign + ""] = state.filters.CampaignId;

		if (state.filters.countryId != null && typeof state.filters.countryId !== 'undefined'  &&  parseInt( state.filters.countryId ) >0 )
			dataToBeSent.QueryJsonData[DimensionsSearchCode.Country + ""] = state.filters.countryId;
		if (state.filters.AppSiteId != null && typeof state.filters.AppSiteId !== 'undefined'  &&  parseInt( state.filters.DealId ) >0)
			dataToBeSent.QueryJsonData[DimensionsSearchCode.AppSite + ""] = state.filters.AppSiteId;

		dataToBeSent.QueryJsonData = JSON.stringify(dataToBeSent.QueryJsonData);

		if (state.ChartType == 'app' || state.ChartType == 'appsite') {

			dataToBeSent.ForPublisher = true;
		}

		if(state.filters.MeasuresIds.length==0 || state.filters.ColumnsIds.length==0 || CriteriaSelected==null)
		{

			setstateGridData({ ...stateGridData, data: [], Count: 0, Cols: [], skip: 0 });
			return ;
		}
		/* if(state.filters.MeasuresIds!=null && typeof state.filters.MeasuresIds!== 'undefined')
			  dataToBeSent.QueryJsonDataobject[DimensionsSearchCode.MeasuresIds+""] = state.filters.MeasuresIds;
		 if(state.filters.ColumnsIds!=null && typeof state.filters.ColumnsIds!== 'undefined')
			  dataToBeSent.QueryJsonDataobject[DimensionsSearchCode.ColumnsIds+""] = state.filters.ColumnsIds;
		 if(state.filters.ColumnsIds!=null && typeof state.filters.ColumnsIds!== 'undefined')
			  dataToBeSent.QueryJsonDataobject[DimensionsSearchCode.ColumnsIds+""] = state.filters.ColumnsIds;
		if(state.filters.FromDate!=null && typeof state.filters.FromDate!== 'undefined')
			dataToBeSent.QueryJsonDataobject[DimensionsSearchCode.FromDate+""] = state.filters.FromDate.format("DD-MM-YYYY");
			if(state.filters.ToDate!=null && typeof state.filters.ToDate!== 'undefined')
				dataToBeSent.QueryJsonDataobject[DimensionsSearchCode.ToDate+""] = state.filters.ToDate.format("DD-MM-YYYY"); */
		axios
			.request({
				url: Constants.backend.DashboardGridPostUrl,
				method: 'POST',
				data: dataToBeSent
			})
			.then(res => {

				let c = 0;
				if (res && res.data && res.data.data && res.data.data.Rows) {
					let tempColumns = [];

					res.data.data.Columns.forEach((x, index) => tempColumns.push({
						field: x.replaceAll(" ", ""),
						title: x,
						id: index, locked: false,
						width: (x.length > 15 ? (x.length * 8 + 10) : x.length > 5 ? (x.length * 11 + 10) : (x.length * 16 + 10))
					}));

					let tempRows = [];
					for (var r = 0; r < res.data.data.Rows.length; r++) {
						var d = "";
						for (var i = 0; i < res.data.data.Columns.length; i++) {
							//debugger;
							var a, b;
							a = res.data.data.Columns[i].replaceAll(" ", "");
							b = res.data.data.Rows[r].Value[res.data.data.Columns[i]];
							d += '"' + a + '":"' + b + '",';

						}
						d = d.substring(0, d.length - 1)
						tempRows.push(JSON.parse("{" + d + "}"));
					}

					/* 	 const Rows=res.data.data.Rows.reduce((acc, item) => {
						 if (item.Value) {
						   acc.push(item.Value);
						 } 
						 return acc;
					   }, []);  */
					setstateGridData({ ...stateGridData, data: tempRows, Count: res.data.data.Count, Cols: tempColumns, skip: skip });

				}
				else {

					setstateGridData({ ...stateGridData, data: [], Count: 0, Cols: [], skip: 0 });

				}
			}

			).catch(err => console.log('error', err));

	}
	const SaveColumns = (id, item) => {

		const newFormData = Object.assign({}, stateGridData);
		stateGridData.Cols[id].locked = item.locked;
		setstateGridData(stateGridData);
	}
	const [dropdownOpen, setDropdownOpen] = useState(false);

	const toggleDropDown = () =>{ 	setshowAddRow(false);setDropdownOpen(prevState => !prevState)

	
	};
	const reunderDashboardContents = () => {
		return (

			<Container className='dashboard'>
				
				<Row className='mb-4'>
					<Col md={12}>
						<h4>Dashboard</h4>
					</Col>
				</Row>
				<Row>
					<DashBoardCardCompnent cardItem={CardsList}  newCard={newCard} KPILookupList={KPIList} t={t}  CallParent={KPICardSelectedId}  />
					
					{/* <!-- info cards end --> */}

				</Row>
				<Row className='mb-4'>
					<Col md={12} lg={6} xl={3} className={state.ChartType.toLowerCase() == "app" || state.ChartType.toLowerCase() == "appsite" || state.ChartType.toLowerCase() == "lmpressionlog" || state.AdvertiserId > 0 ? 'd-none' : ''} >
						<div className="form-group">
							<span className="form__form-group-label">{T("Global:Advertiser")}</span>
							<AsyncReactSelect
								className="react-select"
								classNamePrefix="react-select"
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
								isClearable

								onInputChange={handleInputChangeForAdvertiser}
								onChange={handleChangeForAdvertiser}
								placeholder={T("Advertiser:Select Advertiser")}
							/>
						</div>
					</Col>

					<Col md={12} lg={6} xl={3} className={state.ChartType.toLowerCase() == "app" || state.ChartType.toLowerCase() == "appsite" || state.ChartType.toLowerCase() == "lmpressionlog" || state.CampaignId > 0 || !state.CanViewDeals ? 'd-none' : ''}>
						<div className="form-group">
							<span className="form__form-group-label">{T("PMPDeal:Deal")}</span>

							<AsyncReactSelect

								className="react-select"
								classNamePrefix="react-select"
								//value={state.filters.DealId}
								key={state.filters.AdvertiserId}
isClearable
								cacheOptions
								defaultOptions

								getOptionLabel={e => e.label}
								getOptionValue={e => e.value}
								loadOptions={DealListForDP}
								onInputChange={handleInputChangeForDeal}
								onChange={handleChangeForDeal}
								placeholder='Select Deal'
							/>
						</div>
					</Col>
					<Col md={12} lg={6} xl={3} className={state.ChartType.toLowerCase() == "app" || state.ChartType.toLowerCase() == "appsite" || state.ChartType.toLowerCase() == "lmpressionlog" || state.DealId > 0 ? 'd-none' : ''} >
						<div className="form-group">
							<span className="form__form-group-label">{T("Global:Campaign")}</span>

							<AsyncReactSelect

								className="react-select"
								classNamePrefix="react-select"
								key={campKeyFilter}
								//value={state.filters.CampaignId}
								isClearable
								placeholder={T("Campaign:Select Campaign")}
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
					</Col>

					<Col md={12} lg={6} xl={3} className={state.ChartType.toLowerCase() == "campaign" || state.ChartType.toLowerCase() == "ad" || state.ChartType.toLowerCase() == "deal" || state.ChartType.toLowerCase() == "lmpressionlog" || state.AppSiteId > 0 ? 'd-none' : ''} >
						<div className="form-group">
							<span className="form__form-group-label">{T("SiteMapLocalizations:AppSite")}</span>

							<AsyncReactSelect

								className="react-select"
								classNamePrefix="react-select"
								//value={state.filters.AppSiteId}
								isClearable
								cacheOptions
								defaultOptions

								getOptionLabel={e => e.label}
								getOptionValue={e => e.value}
								loadOptions={getAppSiteListForDP}
								onInputChange={handleInputChangeForAppSite}
								onChange={handleChangeForAppSite}
								placeholder={T("SiteMapLocalizations:AppSite")}
							/>
						</div>
					</Col>


					<Col md={12} lg={6} xl={3} >
						
						<div className="form-group">
							<span className="form__form-group-label">{T("Report:Date Range")}</span>
							<CustomDateRangePicker CallBackParent={handleParentDateChange} id='testDateRange' Tovalue={state.filters.ToDate} Fromvalue={state.filters.FromDate} nameTo='To' nameFrom='From' titleMsgFrom={getDirection() == 'rtl' ? 'من' : 'From'} titleMsgTo={getDirection() == 'rtl' ? 'الى' : 'To'} >
							</CustomDateRangePicker>
						</div>

					</Col>
				</Row>


				<Row>
					<Col md={12}>
						<Card>
							<CardBody>

								<div className="card__title">
									<Row>
										<i class="ri-line-chart-line"></i>
										<h5>Performance Anayltics</h5>
										<Col md={12} lg={12} xl={6} className='mb-2'>&nbsp;</Col>
										<Col md={12} lg={12} xl={6} className='mb-2'>
											<ButtonToolbar className='btn-toolbar--center'>
												<ButtonGroup className="btn-group--justified">
													<MetricComponent metricItems={_metricItems} activeCode={activeCode} FirstLoading={state.FirstLoading} CallBackParent={onClickMetricValue} />

												</ButtonGroup>
											</ButtonToolbar>
										</Col>
									</Row>
								</div>


								<ResponsiveContainer height={430}>

									<ChartContainer
										props={state.filters}
									/>

								</ResponsiveContainer>


							</CardBody>
						</Card>
					</Col>
				</Row>
				{/* <!-- end row --> */}

				<Row className='mb-2'>

					<Col md={12} lg={6} xl={3} >
						<div className="form-group">
							<span className="form__form-group-label">{T("Campaign:Select Campaign")}</span>

							<AsyncReactSelect
								className="react-select"
								classNamePrefix="react-select"
								cacheOptions
								defaultOptions
								autoload
								key={CriteriaSelected}
								value={CriteriaSelected}
								//value={selectedValue}
								getOptionLabel={e => e.label}
								getOptionValue={e => e.value}
								loadOptions={getReportCriteriaForDP}
								onInputChange={handleInputChangeForCriteria}
								onChange={handleChangeForCriteria}
								placeholder='Select Criteria'
							/>
						</div>
					</Col>

					<Col md={12} lg={6} xl={3} >
						{/* // Another approach and appearance */}
						<UncontrolledDropdown  isOpen={dropdownOpen} toggle={toggleDropDown}>
							<div className="form-group">
								<span className="form__form-group-label">Dimensions &amp; Measures</span>
								<DropdownToggle caret={true} id="dropdown-basic" style={{ width: '100%' }}>
									Show / Hide Columns
									</DropdownToggle>
							</div>

							<DropdownMenu className="dropdown__menu " style={{ width: '210%' }}>
								<div className="row">
									<div className="col-md-6">
										<CustomTree
											data={treeDimensionsData} check={state.filters.ColumnsIds}
											CallBackParent={handleParentChangeDim} singleCheckElem={["29021", "29020", "29023", "29022"]} singleCheckElemParent={["29019"]}
										/>
									</div>
									<div className="col-md-6">

										<CustomTree
											data={treeMeasuresData} check={state.filters.MeasuresIds}
											CallBackParent={handleParentChangeMeasure}
										/>

									</div>
								</div>
								<Row>
								<Col  md={12} lg={6} xl={6} className="pl-4 pr-4" >
								<Button
					
					
					color="primary" className="rounded"
					sm
active={TypeSave=='Append'}
                    type="button"
                    onClick={(e) => {
                     // e.preventDefault();
                    //  if(KPISelected&& KPISelected.value )
					 // CallKPISelection();
					 setshowAddRow(true);
					 setTypeSave('Append');
					 setValue("Name",CriteriaSelected.label, { shouldValidate: true });
					 reset({Name:CriteriaSelected.label});
                    }}
                  >
                    <LayersPlus />Append
                  </Button>

				  <Button

color="primary" className="rounded"
sm
active={TypeSave=='Save'}
                    type="button"
                    onClick={(e) => {
                     // e.preventDefault();
                      //if(KPISelected&& KPISelected.value )
					  //CallKPISelection();
					  setshowAddRow(true);
					  setTypeSave('Save');
					  setValue("Name",CriteriaSelected.label, { shouldValidate: true });
					  reset({Name:CriteriaSelected.label});
                    }}
                  >
                    <LayersPlus /> Save
                  </Button>
								 </Col>



							
								</Row>
							{ showAddRow&&	<Row>
								


								<form  className="form" onSubmit={handleSubmit(SaveAddCriteria)}>
       

								<Col  md={12} lg={6} xl={6} className="pl-4 pr-4"  >
		 <div className="form__form-group">
		   <span className="form__form-group-label">{t('Global:Name')}</span>
		   <div className="form__form-group-field">
		   <div className="form__form-group-input-wrap">
		   <Input
		   name="Name"
		   type="input"
		   placeholder="Name"
		   innerRef={register}
		   //validate={[ required() ]}
		   />
{errors.Name  && <span className="form__form-group-error">{errors.Name.message}</span>}
{ errors.serverError  && <span className="form__form-group-error">{errors.serverError.message}</span>}

  </div>
		   </div>
		 </div>


 </Col>
 <Col  md={12} lg={6} xl={6} className="mt-4 " >
   <ButtonToolbar className="form__button-toolbar">
   <Button color="primary" type="submit"   disabled={isSubmitting}   >{  TypeSave=="Append"? t('Global:Update'): t('Global:New')}</Button>{' '}
   <Button color="secondary"   className="pr-4 pl-4" disabled={isSubmitting}      type="button"  onClick={toggle}>{t('Global:Cancel')}</Button>
   </ButtonToolbar>
   </Col>
</form>

								</Row>}
							</DropdownMenu>
						</UncontrolledDropdown>
					</Col>
				
					<Col md={12} lg={6} xl={2} >
						<div className="form-group">
							<span className="form__form-group-label">Country</span>

							<AsyncReactSelect
								className="react-select"
								classNamePrefix="react-select"
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
								isClearable
								//value={selectedValue}
								getOptionLabel={e => e.label}
								getOptionValue={e => e.value}
								loadOptions={getCountryListForDP}
								//onInputChange={handleInputChangeForAdvertiser}
								onChange={handleCountryChange}
								placeholder={t("Global:CountrySelect")}
							/>
							{/* 
									<ReactSelect
										className="react-select"
										classNamePrefix="react-select"
										onChange={handleCountryChange}
										options={state.CountryOptions}
										value={state.selectedCountryOption }
										clearable={false}
									/> */}
						</div>
					</Col>

				</Row>



				<Row>

					<Col md={12} >
						<Card>
							<CardBody>
								<div className="card__title">
									<i class="ri-table-2"></i>
									<h5>Dashboard Report</h5>
								</div>
								<Container className='table-responsive'>


									<Grid
										scrollable
										reorderable={true}
										style={{ height: '500px' }}
										data={stateGridData.data}
										skip={stateGridData.skip}
										take={stateGridData.take}
										total={stateGridData.Count}
										filterable={false}
										sortable={false}
										pageable={true}

										onPageChange={pageChange}
									>
										{stateGridData.Cols.map(item => {
											return <GridColumn columnData={item} locked={item.locked} columnMenu={props => (
												<CustomColumnMenu
													{...props} columnData={item} column={item}
													columns={stateGridData.Cols}
													onGridMenuCallBack={SaveColumns}
												/>
											)} title={item.title} width={item.width} field={item.field} />


										})}
									</Grid>

								</Container>


							</CardBody>
						</Card>
					</Col>


				</Row>
			</Container>
			//  <!-- container-fluid -->

		)
	}


	return <>


		{reunderDashboardContents()}

	</>
};

DefaultDashboard.propTypes = {
	t: PropTypes.func.isRequired,
	rtl: RTLProps.isRequired,
};

export default compose(withTranslation('common'), connect(state => ({
	rtl: state.rtl,
})))(DefaultDashboard);
