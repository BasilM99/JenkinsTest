import { Col, Container, Row, Button ,Card ,CardBody , Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, Form} from 'reactstrap';
import { Grid, GridColumn as Column,  GridDetailRow  } from "@progress/kendo-react-grid";
import {CheckBoxField} from '../../shared/components/form/CheckBox';
import { useSelector, useDispatch } from 'react-redux';
import  { useContext,useState,useEffect} from 'react';
import AsyncReactSelect from 'react-select/async';
import { withTranslation } from 'react-i18next';
import CloseIcon from 'mdi-react/CloseIcon';
const queryString = require('query-string');
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { compose } from 'redux';
import moment from 'moment';
import React from 'react';
import axios from 'axios';


import {  userInitialized, userRoleInitialized,ChangeUserRole ,UserSelectors } from '../../Store/ReducerSlices/User';
import CustomDateRangePicker from '../../Components/CustomDateRangePicker';
import { RTLProps } from '../../shared/prop-types/ReducerProps';
import useLocalization from '../../Hooks/useLocalization';
import { UISelectors } from '../../Store/ReducerSlices/UI';
import Constants from '../../Config/Constants';
import AdFalconGrid, {AdFalconColumn}  from '../../Components/Grids';
import useCurentUser from '../../Hooks/useCurrentUser'
import useCurrentUI from '../../Hooks/useMyCurrentUI'
import CustomConfirmationModal from '../../Components/CustomConfirmationModal';
import {UserRoles,UserPermissionCode,SystemRoles} from '../../Config/Enums';



const Deals = ({ t, rtl }) => {

    useEffect(() => {
		//FillDataGrid(0,10);
       
    },[]);

    
    const useCurrentUserHook = useCurentUser();
    //const currentAccountId = useCurrentUserHook.UserGlobal.AccountId;
	const { T, Resources } = useLocalization();
    const stateStore = useSelector(state => state);
    const currentAccountId = UserSelectors.getCurrentAccountId(stateStore);
    const UI = UISelectors.getUI(stateStore);

    const [modal, setModal] = useState(false);

    const toggle = () => setModal(!modal);

  
    const [state , setLocalState] = useState({
        categories: [],
        isRefresh:true,
        pending:"",
        lastSuccess:"",
        CountryOptions:[],
        exchangeList:[],
        adSizeList:[],
        adSizeFormatTypes: [],
        adSizeIsDisabled:true,
        bannerFlag:false,
        inStreamVideoFlag:false,
        nativeAdFlag:false,
        color:'danger',
        ConfirmTitle:"Confirm",
        ConfirmMessage:"Please select at least one Advertiser !",
        idsToArchive:[],
        filters:{
            FromDate:moment(new Date()),
            ToDate:moment(new Date()),
            name:"",
            showArchived:false,
            ExchangeId:"",
            PublisherName:"",
            DealName:"",
            AdFormat:"",
            showArchived:"",
            IsGlobal:"",
            Countries:[],
            AdSize:"",
            name:"",
            Size:10,
            Page:1,
            keyGrid:0,
        }
    });
    const handleParentDateChange = (start , end ) => {
        setLocalState({filters:{	FromDate:start,
            ToDate:end


        }})

    };
 

    const renderStatusMode = (props) =>{
        return(
            <td className="text-center k-command-cell">

                {
                    props.dataItem.Status == "InActive" ?
                    <i class="ri-alert-line text-danger" style={{fontSize:21}}></i>
                :
                <i class="ri-heart-pulse-line text-success" style={{fontSize:21}}></i>

                }
            </td>
        )
    }
    const renderDealNameLink = (props) =>{
        return(
            <td className="text-center k-command-cell">
                <a href={"/Deals/Create?id="+props.dataItem.ID+"&AdvertiseraccId=" + props.dataItem.AdvertiserAccountId} code="2">{props.dataItem.Name}</a>
            </td>
        )
    }



    const onArchiveCheckChange = (e) =>{
        if(typeof(e.target) != "undefined"){
            state.filters.showArchived = e.target.checked;
            state.filters.keyGrid=state.filters.keyGrid+1;
            const newFormData = Object.assign({}, state);
            setLocalState({...newFormData});
            //setLocalState({...state});
		    //FillDataGrid(0,10);
        }
        
        //console.log(e);
    }

    const onGridNameSearchChange = (e) =>{
        state.filters.DealName = e.target.value;
        state.filters.keyGrid=state.filters.keyGrid+1;
        const newFormData = Object.assign({}, state);
		setLocalState({...newFormData});
        //console.log(e.target.value);
		//FillDataGrid(0,10);
    }

    const onGridPublisherSearchChange = (e) =>{
        state.filters.PublisherName = e.target.value;
        state.filters.keyGrid=state.filters.keyGrid+1;
        const newFormData = Object.assign({}, state);
		setLocalState({...newFormData});
        //console.log(e.target.value);
		//FillDataGrid(0,10);
    }


    const getExchangeList = (inputValue,callback) =>{
		var tempList= [];
		if(!inputValue)
		{inputValue='';}
        let data = {
            language: "en",
            SSpartyGrid_size: 10,
            Prefix: "",
            id: "ssppartner",
        }
        
        var querystring = require('querystring')
        data = querystring.stringify(data);
        axios({
            method: 'post',
            url: Constants.backend.getExchangeList ,
            data: data,
            headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    "Accept": "application/json"
                }
            })
		.then(success => {
			if(success.data){
				tempList =  success.data.map( x => {
					return {
						label: x.data,
						value: x.attributes.Id
					}
				});
					state.exchangeList = tempList;
                    state.filters.keyGrid=state.filters.keyGrid+1;
                    const newFormData = Object.assign({}, state);
                    setLocalState({...newFormData});
					//setLocalState(state);
					callback(tempList);
			}
		})
		.catch(err => console.log('error', err));
	
	}

	const handleInputChangeForExchangeList=(inputValue) =>{
		

    };
    const handleChangeForExchangeList=(Value) =>{
	
        state.filters.AdvertiserId= Value.value;
        state.filters.keyGrid=state.filters.keyGrid+1;
        const newFormData = Object.assign({}, state);
		setLocalState({...newFormData});
        
        //setLocalState(state);
		//FillDataGrid(0,10);
    };


    //ad sizes group code start 

    const getAdSizeList = (inputValue,callback) =>{
		var tempList= [];
        axios({
            method: 'post',
            url: Constants.backend.getAdSizeList + state.adSizeFormatTypes.join(',') ,
            headers: {
                    'Content-Type': 'application/json',
                    "Accept": "application/json"
                }
            })
		.then(success => {
			if(success.data){
                if(success.data.tree.length > 0){
                    tempList = success.data.tree.map(x => {
                        let tempObj = {};
                        tempObj.label = x.Name.Value;
                        tempObj.options = x.Childs.map(c => {
                            let tempSubOjb = {};
                            tempSubOjb.value= c.Id;
                            tempSubOjb.label= c.Name.Value;
                            return tempSubOjb;
                        })
                        return tempObj;
                    
                    })
                }
				
                state.adSizeList = tempList;
                state.filters.keyGrid=state.filters.keyGrid+1;
                const newFormData = Object.assign({}, state);
                setLocalState({...newFormData});
                //setLocalState({...state});
                //callback(tempList);
			}
		})
		.catch(err => console.log('error', err));
	
	}

    const handleChangeForAdSizeList=(Value) =>{
	
        state.filters.AdSize= Value.map(x => x.value).join(',');
        state.filters.keyGrid=state.filters.keyGrid+1;
        const newFormData = Object.assign({}, state);
		setLocalState({...newFormData});
        
        //setLocalState(state);
		//FillDataGrid(0,10);
    };
    const adSizeFormatGroupLabel = data => (
        <div style={groupStyles}>
          <span>{data.label}</span>
          <span style={groupBadgeStyles}>{data.options.length}</span>
        </div>
      );

    const groupStyles = {
    color: "#dee2e6;",
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    };
    const groupBadgeStyles = {
    backgroundColor: '#EBECF0',
    borderRadius: '2em',
    color: '#172B4D',
    display: 'inline-block',
    fontSize: 12,
    fontWeight: 'normal',
    lineHeight: '1',
    minWidth: 1,
    padding: '0.16666666666667em 0.5em',
    textAlign: 'center',
    };
    // const groupedOptions = [
    //     {
    //       label: 'Banner',
    //       options: colourOptions,
    //     },
    //     {
    //       label: 'In-Stream Video',
    //       options: flavourOptions,
    //     },
    //     {
    //       label: 'Native Ad',
    //       options: flavourOptions,
    //     },
    //   ];

    const onAdFormatChanged = () =>{

        //let adSizeFormatTypes =  [];
        let bannerType = state.bannerFlag ? "1":"";
        let nativeAdType = state.nativeAdFlag ? "2":"";
        let inStreamVideoType = state.inStreamVideoFlag ? "3":"";

        state.adSizeFormatTypes = [bannerType ,nativeAdType,inStreamVideoType  ]

        if(state.bannerFlag || state.inStreamVideoFlag || state.nativeAdFlag){
            state.adSizeIsDisabled = false ;
            getAdSizeList();
            setLocalState({...state})
        }else{
            state.adSizeIsDisabled = true ;
            getAdSizeList();
            setLocalState({...state})
        }
    }
    const onBannerFlagCheckChange = (e) =>{
        if(typeof(e.target) != "undefined")
            {
                state.bannerFlag = e.target.checked;
                //setLocalState({...state});
                //pageChange();
                onAdFormatChanged();
            }
        
    }
    
    const onNativeAdCheckChange = (e) =>{
        if(typeof(e.target) != "undefined")
            {
                state.nativeAdFlag = e.target.checked;
                //setLocalState({...state});
                //pageChange();
                onAdFormatChanged();
            }
        
    }
    
    const onInStreamVideoCheckChange = (e) =>{
        if(typeof(e.target) != "undefined")
            {
                state.inStreamVideoFlag = e.target.checked;
                //setLocalState({...state});
                //pageChange();
                onAdFormatChanged();
            }
        
    }
    //ad sizes group code end 

    const getCountryListFor = (inputValue,callback) =>{
		var tempList= [];
		if(!inputValue)
		{inputValue='';}
		
	
		if(state.CountryOptions.length==0)
		{

			axios
			.request({
				url:   Constants.backend.getCountres,
				method: 'GET',
			}).then(response=>{
			let countryList =[];
			
			response.data.map(x =>{
						countryList.push({
							label :x.Text,
							value :x.Value
						});	
					}) 
				
					//return response ;
					callback(countryList  );
					state.CountryOptions = countryList;
                    state.filters.keyGrid=state.filters.keyGrid+1;
					//setLocalState({...state,CountryOptions:countryList});
                    const newFormData = Object.assign({}, state);
                    setLocalState({...newFormData,CountryOptions:countryList});
				});


		}
		else

		{
		
		let newitems= state.CountryOptions.reduce((acc, item) => {
			if (item.label.toLowerCase().indexOf(inputValue.toLowerCase()) >= 0 || ( inputValue === undefined || inputValue === null || inputValue==='')) {
			  acc.push(item);
			} 
			
			return acc;
		  }, []);
     callback(newitems  );
		
		}
    }
    
    const handleCountryChange = (e) =>{

		const newFormData = Object.assign({}, state);
		state.filters.Countries= e.map(x => x.value).join(',');
		setLocalState(state);

		
		//FillDataGrid(0,10);
	}

    
    const contextMenu =(props) =>{
        
        //if(useCurrentUserHook.IsUserPermitted(UserPermissionCode.PMPDeal)){

            return <>

                <button className="btn-outline-primary btn"  title={t('Commands:Edit')}><a href={"/Deals/Create?id="+props.dataItem.ID+"&AdvertiseraccId=" + props.dataItem.AdvertiserAccountId} code="2"><span className="ri-pencil-line p-2" /></a></button>
            </>
        


    }
    const SelectedIdsGrid=(items)=>{
      
        collectAdvertiserToArchive(items)
    
    }

    const collectAdvertiserToArchive = (items) =>{
        if(items.length > 0)
        {
            let tempAdvertisers = items.map(x => {
                return x.Name 
             }).join(" \n ");
             state.ConfirmMessage = "Are you sure you want to Archive the below Advertisers?\n  "+tempAdvertisers;
             state.idsToArchive = items.map(x => x.Id)
             state.color='Primary';
             setLocalState({...state});
        }else{
            
            state.ConfirmMessage = "Please select at least one Advertiser !";
            state.idsToArchive = [];
            state.color='danger';
            setLocalState({...state});
        }
    }
    const onArchiveButtonClick = () => {
        console.log("archive");
        //getAdvertizersDataandArchive

        var baseUrl = Constants.backend.getDealsDataArchive;

        var querystring = require('querystring')
        var data = querystring.stringify({checkedRecords:state.idsToArchive});

        axios({
            method: 'post',
            url:baseUrl,
            data:data,
            headers: {
                    "Accept": "application/json"
                }
            })
        .then(res => {

        }).catch(err => console.log('error', err));

    }
    const renderDealsComponent = () =>{
        //pageChange();
        return(
            <Container className="dashboard">
                <Row>
                    <Col md={12}>
                        {/* <h3 className="page-title">{t('default_dashboard.page_title')}</h3> */}
                        <h3 className="page-title">{t("Titles:PMPDeals")}</h3>
                    </Col>
                </Row>
                <Row>
                    <Col md={12}  >
                        <Row>
                            <Col md={3}>
                                <div className="form__form-group  mt-3">
                                    <span className="form__form-group-label">{t('Commands:Search')}</span>
                                    <form className="topbar__search">
                                        <input placeholder="Name" onChange={onGridNameSearchChange} className=" form-control k-textbox dropdown-toggle" />
                                        <button className="topbar__btn topbar__search-btn " type="reset">
                                            <CloseIcon />
                                        </button>
                                    </form>
                                    
                                </div>
                            </Col>
                             
                            <Col md={3}>
                                <div  className="form__form-group mt-3">
                                    <span className="form__form-group-label">{t('Commands:Search')}</span>
                                    <form className="topbar__search">
                                        <input placeholder="Publisher" onChange={onGridPublisherSearchChange} className=" form-control k-textbox dropdown-toggle" />
                                        <button className="topbar__btn topbar__search-btn " type="reset">
                                            <CloseIcon />
                                        </button>
                                    </form>
                                    
                                </div>
                            </Col>   
                            <Col md={4}>
                                <div className="form__form-group  mt-3" >
                                    <span className="form__form-group-label">{t('Report:DateRange')}</span>
                                    <CustomDateRangePicker   CallBackParent={handleParentDateChange} id='customeDateRange'  Tovalue={state.filters.ToDate}    Fromvalue={state.filters.FromDate }    nameTo='To' nameFrom='From' titleMsgFrom={UI.direction=='rtl'?'من':'From'} titleMsgTo={UI.direction=='rtl'?'الى':'To'} ></CustomDateRangePicker>
                                </div>
                            </Col>
                            <Col md={2}>
                                <div className="form__form-group mt-4">
                                    <span className="form__form-group-label">{t('Commands:Search')}</span>
                                    
                                    <div className="form__form-group-field">
                                        <CheckBoxField
                                        onChange={onArchiveCheckChange}
                                        name="showArchived"
                                        value={state.filters.showArchived}
                                        label={t('Global:ShowArchived')}
                                        defaultChecked />
                                        {/* <span className="form__form-group-label"></span> */}
                                    </div>
                                </div>
                            </Col>
                        </Row>
                    </Col>
                </Row>
                <Row className="mb-2">
                    <Col md={6}>
                        <Row>
                        <Col md={4}>
                                <AsyncReactSelect
										className="react-select"
										classNamePrefix="react-select"
										cacheOptions
										defaultOptions
                                        isMulti
										autoload
										getOptionLabel={e => e.label}
										getOptionValue={e => e.value}
										loadOptions={getCountryListFor}
										onChange={handleCountryChange}
										placeholder={t('Global:CountrySelect')}
										/>
                           </Col>
                           <Col md={4}>
                               <AsyncReactSelect
                                           className="react-select"
                                           classNamePrefix="react-select"
                                           cacheOptions
                                           defaultOptions
                                           autoload
                                           //value={selectedValue}
                                           getOptionLabel={e => e.label}
                                           getOptionValue={e => e.value}
                                           loadOptions={getExchangeList}
                                           onInputChange={handleInputChangeForExchangeList}
                                           onChange={handleChangeForExchangeList}
                                           placeholder={T("PMPDeal:SelectExchangeT")}	
                                       />
                           </Col>
                           
                           <Col md={4}>
                               <AsyncReactSelect
                                           className="react-select"
                                           classNamePrefix="react-select"
                                           //cacheOptions
                                           defaultOptions={state.adSizeList}
                                           //autoload
                                           isMulti
                                           //value={selectedValue}
                                        //    getOptionLabel={e => e.label}
                                        //    getOptionValue={e => e.value}
                                           isDisabled={state.adSizeIsDisabled}
                                           //loadOptions={getAdSizeList}
                                           //options={state.adSizeList}
                                           formatGroupLabel={adSizeFormatGroupLabel}
                                           onChange={handleChangeForAdSizeList}
                                           placeholder={T("PMPDealTargetings:AdSize")}	
                                       />
                           </Col>
                        </Row>
                    </Col>
                    <Col md={6}>
                        <Row>
                            <Col md={12}>
                                <Row>
                                    <Col md={3}>
                                        <span className="form__form-group-label">{t('PMPDeal:AdFormat')}</span>
                                    </Col>
                                    <Col md={2}>
                                        <div  className="form__form-group ">
                                            <CheckBoxField
                                            onChange={onBannerFlagCheckChange}
                                            name="bannerFlag"
                                            value={state.filters.bannerFlag}
                                            label={t('PMPDeal:Banner')}
                                            defaultChecked />
                                        </div>
                                    </Col>
                                    <Col md={4}>
                                        <div  className="form__form-group ">
                                            <CheckBoxField
                                            onChange={onInStreamVideoCheckChange}
                                            name="inStreamVideoFlag"
                                            value={state.filters.inStreamVideoFlag}
                                            label={t('InstreamVideo:InstreamVideoName')}
                                            defaultChecked />
                                        </div>
                                    </Col>
                                    <Col md={3}>
                                        <div  className="form__form-group ">
                                            <CheckBoxField
                                                onChange={onNativeAdCheckChange}
                                                name="nativeAdFlag"
                                                value={state.filters.nativeAdFlag}
                                                label={t('NativeAd:NativeAdName')}
                                                defaultChecked />
                                        </div>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>
                    </Col>
                </Row>
                <Row>
                    <Card>
                        <CardBody  >
                            <Container className='dropdown float-right'>
                                {/* <a onClick={this.refreshGrid}  className="k-pager-refresh k-link" title="Refresh" aria-label="Refresh"><span className="k-icon k-i-reload"></span></a> */}
                            </Container>

                            <h4 className='card-title mb-4 font-size-18'>{t('Deal:GlobalDeals')}</h4>

                            <div className='table-responsive'>
                            <AdFalconGrid

                                //pageable={true}
                                key={state.filters.keyGrid}
                                pageable={ true  }
                                scrollable={ true  }
                                reorderable={true}
                                filters={state.filters}
                                style={{ height: '400px', fontSize: '13px' }}
                                sortable={false}
                                APIurl={Constants.backend.getDealsData }
                                keyName={'Id'}
                                EnableContextMenu={true}
                                EnableSelection={true}
                                ContentType={'application/x-www-form-urlencoded'}

                                ContextMenu={contextMenu}
                                QuickActionTitle={"QuickAction"}
                                QuickActionWidth={"200px"}
                                //DetailComponent={ExtendedDetailComponent}
                                //expandField="expanded"
                                //onExpandChange={expandChange}
                                EnableExpand={true}
                                CallBackParentSelected={SelectedIdsGrid}
                                //APIurldetails={Constants.backend.getCampaignsByAdvertiserAccountId}
                                >

                                    <AdFalconColumn cell={renderDealNameLink}   title={t('Global:Name')} />
                                    <AdFalconColumn field="StartDateString" title={T("Campaign:StartDate")}/>
                                    <AdFalconColumn field="EndDateString"  title={T("Campaign:EndDate")}/>
                                    <AdFalconColumn field="Price"  title={T("SSPFloorPrices:Price")}  />
                                    <AdFalconColumn field="ExchangeName"  title={T("PMPDeals:ExchangeName")} />
                                    <AdFalconColumn field="PublisherName" title={T("PMPDeals:PublisherName")} />
                                    <AdFalconColumn field="DealTypeString" title={T("Global:Type")} />
                                    <AdFalconColumn cell={renderStatusMode} title="Status" width="70px" />

                                </AdFalconGrid>

                            </div>
                            <Row className="mt-2">
                                <Col md={1}>
                                    <CustomConfirmationModal
                                            outBtnColor="secondary"
                                            color={state.color}
                                            title={state.ConfirmTitle}
                                            header
                                            btn={t('PMPDeal:Archive')}
                                            message={state.ConfirmMessage}
                                            onConfirm={onArchiveButtonClick}
                                        />
                                    {/* <Button color="secondary" className=" p-2">{T("PMPDeal:Archive")}</Button> */}
                                </Col>
                                <Col md={2}>
                                    <div>
                                        <Form inline onSubmit={(e) => e.preventDefault()}>

                                            <Button color="success" className=" p-2" onClick={toggle}><a className="text-white" href={"/Deals/Create/0"} code="2">{T("Commands:AddNewPMPDeals")}</a></Button>
                                        </Form>
                                        
                                        {/* <SimpleForm onSubmit={alertDataTest} isOpen={modal} toggle={toggle} /> */}
                                        
                                    </div>
                                </Col>
                                <Col md={9}></Col>
                            </Row>
                        </CardBody>
                    </Card>
                </Row>
        </Container>
        )
    }

    return <>
    {renderDealsComponent()}
    </>


};




Deals.propTypes = {
    t: PropTypes.func.isRequired,
    rtl: RTLProps.isRequired,
  };



  export default compose(withTranslation('common'), connect(state => ({
    rtl: state.rtl,
  })))(Deals);
