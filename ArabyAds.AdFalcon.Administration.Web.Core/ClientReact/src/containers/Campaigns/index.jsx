import { Col, Container, Row, Button ,Card ,CardBody ,ButtonGroup , Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, Form,ButtonToolbar ,FormGroup} from 'reactstrap';
import { Grid, GridColumn as Column,  GridDetailRow  } from "@progress/kendo-react-grid";
import {CheckBoxField} from '../../shared/components/form/CheckBox';
import { useSelector, useDispatch } from 'react-redux';
import  { useContext,useState,useEffect} from 'react';
import { withTranslation } from 'react-i18next';
import CloseIcon from 'mdi-react/CloseIcon';
const queryString = require('query-string');
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { compose } from 'redux';
import moment from 'moment';
import React from 'react';
import axios from 'axios';
import { BasicNotification,showBasicNotificationForAdFalcon } from '../../Components/Notification';
import {  userInitialized, userRoleInitialized,ChangeUserRole ,UserSelectors } from '../../Store/ReducerSlices/User';
import CustomDateRangePicker from '../../Components/CustomDateRangePicker';
import { RTLProps } from '../../shared/prop-types/ReducerProps';
import { UISelectors } from '../../Store/ReducerSlices/UI';
import Constants from '../../Config/Constants';
// import SimpleForm from './AddAdvertiserModal';
//import {loadAdv} from './AddAdvertiserModal';
import Collapse from '../../Components/CustomCollapse';
import CloneComponent from '../../Components/CloneComponent';
import CustomConfirmationModal from '../../Components/CustomConfirmationModal';
import AdFalconGrid, {AdFalconColumn}  from '../../Components/Grids';
import useCurentUser from '../../Hooks/useCurrentUser'
import useCurrentUI from '../../Hooks/useMyCurrentUI'
import {UserRoles,UserPermissionCode,SystemRoles} from '../../Config/Enums';
import { SubmissionError } from 'redux-form';  // ES6
import Panel from '../../Components/Panel';

const Campaigns = ({ t, rtl }) => {
	const dispatch = useDispatch();
    useEffect(() => {

       // pageChange();

    },[]);
    
    const useuseCurrentUIHook = useCurrentUI();
    const UI =useuseCurrentUIHook.UI;

    var buttonLabel= "test";
    var className = "";


    const [collapseAddAdvertiser, setcollapseAddAdvertiser] = useState(false);

    const toggleAddAdvertiser = () =>{
        //showBasicNotificationForAdFalcon("ff","ff","primary","right-up");
        setcollapseAddAdvertiser(! collapseAddAdvertiser);    
           /*  dispatch(loadAdv({
                Name: "hfhfgh"
           
         }))*/
    
    };

    const [stateGridData , setstateGridData] = useState(	 {
        skip: 0, take: 10,data:[],Cols:[],checkedRecords:[],Count:0

    });
    const useCurrentUserHook = useCurentUser();
    const currentAccountId = useCurrentUserHook.UserGlobal.AccountId;
    const [state , setLocalState] = useState({
        categories: [],
        isRefresh:true,
        pending:"",
        color:'danger',
        lastSuccess:"",
        show: true ,
        ConfirmTitle:t('Global:Confirm'),
        ConfirmMessage:t("Campaign:SelectConfirmation"),
        selectedIds:[],
        filters:{
            FromDate:moment(new Date()),
            ToDate:moment(new Date()),
            name:"",Size:10,
            Page:1,keyGrid:0,
        }
    });
    var lastSelectedIndex = 0;
    const handleParentDateChange = (start , end ) => {
        state.filters.FromDate=start;
        state.filters.ToDate=end;
        state.filters.keyGrid=state.filters.keyGrid+1;
        setLocalState({...state});

       // pageChange();
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
    
   
    //checkbox end





    const onGridNameSearchChange = (e) =>{
     
        const newFormData = Object.assign({}, state);
        newFormData.filters.name = e.target.value;
        newFormData.filters.keyGrid=newFormData.filters.keyGrid+1;
		setLocalState({...newFormData});
        //state.filters.name = e.target.value;
        //setLocalState({...state});
        console.log(e.target.value);
        //pageChange();
    }



    const actoinButtonClick = (type) => {
        console.log("archive");
        //getAdvertizersDataandArchive

        var baseUrl = Constants.backend.getCampaignsActions;

        var querystring = require('querystring')
        var data = querystring.stringify(Object.assign({checkedRecords:state.selectedIds}, type));

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
    const onRunButtonClick = () => {
        actoinButtonClick({run: "Run"});
    }
    const onPauseButtonClick = () => {
        actoinButtonClick({pause: "Pause"});
    }
    const onDeleteButtonClick = () => {
        actoinButtonClick({Delete: "Delete"});
    }
    const collectSelectedCampaigns = (items) =>{
        if(items.length > 0)
        {
            let tempCampaigns = items.map(x => {
                return x.Name 
             }).join(" \n ");
             let tempMess = t('Confirmation:Run');
             tempMess = tempMess.replace("{0}",t('Titles:Campaign')) ;
             state.ConfirmMessage = tempMess+"?\n  "+tempCampaigns;
             state.selectedIds = items.map(x => x.Id)
             state.color='warning';
             setLocalState({...state});
        }else{
            
            state.ConfirmMessage =t("Campaign:SelectConfirmation");
            state.selectedIds = [];
            state.color='danger';
            setLocalState({...state});
        }
    }
    const contextMenu =(props) =>{
        let EditUrl = "/campaign/CreateRD/" + props.dataItem.Id;
        let ReportsUrl = "/Reports?reportType=ad&itemId=" + props.dataItem.Id;
        let AuditTrailUrl = "/Campaign/RedirectToAuditTrial/" + props.dataItem.Id ;
        let DealsUrl = "/Deals/" + props.dataItem.AdvertiserAccId;
        
        return <>

            <button className="btn-outline-primary btn"  title={t('Commands:Edit')}><a href={EditUrl} code="2"><span className="ri-pencil-line p-2" /></a></button>
            <button className="btn-outline-primary btn " title={t('Commands:Reports')}><a href={ReportsUrl} code="6"><span className="ri-file-chart-line p-2" /></a></button>
            <CloneComponent title={t('Commands:Copy')}  onCloneClick={onCloneClick} name={props.dataItem.Name} t={t} cloneUrl={Constants.backend.cloneCampaign+"?id="+props.dataItem.Id}  cloneSuccessMessage={t("Clone:Campaign")} cloneErrorMessage={t("Errors:CloneCampaignError")}/>
            <button className="btn-outline-primary btn" title={t('Audittrial:tooltpi')}><a href={AuditTrailUrl} code="0"><span className="ri-search-eye-line p-2" /></a></button>
            
        </>
    }

    const onCloneClick = () =>{
        console.log("clone");
    }
    const SelectedIdsGrid=(items)=>{
      
        collectSelectedCampaigns(items)
    
    }


    const renderImpUniqueImp = (props) =>{
        let tempCellText = props.dataItem.Performance.ImpUniqueImp;
        let tmpArray1 = tempCellText.split('</span>');
        let tmpArray2 = tmpArray1[0].split('/');
        return(
            <td className="text-center k-command-cell lead">
                <span className="text-secondary">{tmpArray2[0]}</span>
                <span className="text-secondary">/</span>
                <span className="text-danger">{tmpArray2[1]}</span>
                
                
            </td>
        )
    }
    const renderClicksUniqueClicks = (props) =>{
        let tempCellText = props.dataItem.Performance.ClicksUniqueClicks;
        let tmpArray1 = tempCellText.split('</span>');
        let tmpArray2 = tmpArray1[0].split('/');
        return(
            <td className="text-center k-command-cell lead">
                <span className="text-secondary">{tmpArray2[0]}</span>
                <span className="text-secondary">/</span>
                <span className="text-danger">{tmpArray2[1]}</span>
                
                
            </td>
        )
    }
    const renderCampaignNameLink = (props) =>{
        return(
            <td className="text-center k-command-cell">
                <a href={Constants.backend.getGroupsByCampaignId +props.dataItem.Id}  >{props.dataItem.Name}</a>
            </td>
        )
    }
    const renderCampaignsComponent = () =>{
        
		const queryValues = queryString.parse(location.search);
        let url  =Constants.backend.getCampaignsData;
        
		if (queryValues.chartType && typeof (queryValues.AdvertiseraccId !== "undefined"))
            url += "?AdvertiseraccId="+ queryValues.chartType;

        //pageChange();
        return(
            <Container className="dashboard">
                <Row>
                <Col md={12}>
                    {/* <h3 className="page-title">{t('default_dashboard.page_title')}</h3> */}
                    <h3 className="page-title">{t('Titles:Campaign')}</h3>
                </Col>
                </Row>
                <form  className="form"  onSubmit={e => { e.preventDefault(); }}  >
       
                                        <Row  className='mb-2'>
                                            <Col md={12} lg={6} xl={6}>
                                                <div className="form__form-group">
                                                    <span className="form__form-group-label">{t('Commands:Search')}</span>
                                                   
                                                    <div className="form__form-group-field">
                                                        <form className="topbar__search"  onSubmit={e => { e.preventDefault(); }} >
                                                            <input placeholder={t('Commands:Search')} onChange={onGridNameSearchChange} className=" form-control k-textbox dropdown-toggle" />
                                                         
                                                            <button className="topbar__btn topbar__search-btn " dir={rtl.direction} type="reset">
                                                                <CloseIcon />
                                                          </button>
                                                        </form>
                                                    </div>
                                                </div>
                                            </Col>
                                                
                                            <Col md={12} lg={6} xl={6}>
                                                <div className="form-group" >
                                                    <span className="form__form-group-label">{t('Report:DateRange')}</span>
                                                    <CustomDateRangePicker   CallBackParent={handleParentDateChange} id='testDateRange'  Tovalue={state.filters.ToDate}    Fromvalue={state.filters.FromDate }    nameTo='To' nameFrom='From' titleMsgFrom={UI.direction=='rtl'?'????':'From'} titleMsgTo={UI.direction=='rtl'?'??????':'To'} ></CustomDateRangePicker>
                                                </div>
                                            </Col>
                                        </Row>
                              
                               
                       
                     
            </form>
                <Row>
                    <Card>
                        <CardBody  >
                            <div className="card__title">
                                <i class="ri-table-2"></i>
                                <h5 className="bold-text">{t('Titles:Campaign')}</h5>
                            </div>
                        

                            {/* <h4 className='card-title mb-4 font-size-18'></h4> */}
                           <Container className='table-responsive'>

                             
                                <AdFalconGrid

                                    //pageable={true}
                                    key={state.filters.keyGrid}
                                    pageable={ true  }
                                    scrollable={ true  }
                                    reorderable={true}
                                    filters={state.filters}
                                    style={{ height: '400px', fontSize: '13px' }}
                                    sortable={false}
                                    APIurl={url}
                                    keyName={'Id'}
                                    EnableContextMenu={true}
                                    EnableSelection={true}
                                    ContentType={'application/x-www-form-urlencoded'}

                                    ContextMenu={contextMenu}
                                    QuickActionTitle={"QuickAction"}
                                    QuickActionWidth={"200px"}
                                    //expandField="expanded"
                                    //onExpandChange={expandChange}
                                    EnableExpand={true}
                                    CallBackParentSelected={SelectedIdsGrid}
                                    APIurldetails={Constants.backend.getCampaignsByAdvertiserAccountId}
                                    >
                                        

                                    <AdFalconColumn cell={renderCampaignNameLink}    title={t('Campaign:CampaignName')}     />
                                    <AdFalconColumn field="CreationDate" title={ t('Campaign:CreationDate')}   />
                                    <AdFalconColumn field="Status" title={ t('Global:Status')}     />
                                    <AdFalconColumn field="BudgetText"  title={ t('Campaign:Budget')}   />
                                    <AdFalconColumn cell={renderImpUniqueImp} title={ t('Campaign:ImpressionsUniqueImp')}    />
                                    <AdFalconColumn cell={renderClicksUniqueClicks} title={ t('Campaign:ClicksUniqueClicks')}  />
                                    <AdFalconColumn field="Performance.CtrText" title={ t('Campaign:CTR')}  width="80px" />
                                    <AdFalconColumn field="Performance.AvgCPCText" title={ t('Campaign:AvgCPC')}   />
                            
                                
                                    {useCurrentUserHook.IsUserHasRole(UserRoles.DSPRole)?
                                                    <React.Fragment>
                                                    <AdFalconColumn field="Performance.AdjustedNetCostText" title={ t('Global:NetCost')}/>
                                    <AdFalconColumn field="Performance.GrossCostText" title={ t('Global:GrossCost')} />
                                    <AdFalconColumn field="Performance.BillableCostText" title={ t('Global:BillableCost')}    />
                                                
                                        </React.Fragment>

                                        :
                                        <AdFalconColumn field="Performance.BillableCostText" title={ t('Global:BillableCost')}    />
                                        
                                        
                                        }
                                </AdFalconGrid>


                            </Container>
                          
                            
                        </CardBody>
                    </Card>
                </Row>

                <Row>
                                <Col  md={12} lg={6} xl={1} >
                                    <CustomConfirmationModal
                                                outBtnColor="danger"
                                                color={state.color}
                                                title={state.ConfirmTitle}
                                                header
                                                btn={t('Commands:Run')}
                                                message={state.ConfirmMessage}
                                                onConfirm={onRunButtonClick}
                                            />

                                </Col>
                                <Col  md={12} lg={6} xl={1}>
                                    <CustomConfirmationModal
                                                    outBtnColor="danger"
                                                    color={state.color}
                                                    title={state.ConfirmTitle}
                                                    header
                                                    btn={t('Commands:Pause')}
                                                    message={state.ConfirmMessage}
                                                    onConfirm={onPauseButtonClick}
                                                />
                                </Col>
                                <Col  md={12} lg={6} xl={1}>
                                    <CustomConfirmationModal
                                                    outBtnColor="danger"
                                                    color={state.color}
                                                    title={state.ConfirmTitle}
                                                    header
                                                    btn={t('Commands:Delete')}
                                                    message={state.ConfirmMessage}
                                                    onConfirm={onDeleteButtonClick}
                                                />
                                </Col>
                               
                            </Row>
                      
                <Row>



                    <Col md={12} >

                        {collapseAddAdvertiser && 
                        <Panel CallBackToggle={toggleAddAdvertiser} md={12} lg={6} xl={3} color="Primary" divider title={"Add Advertiser"} icon="chart-bars">
                            {/* <SimpleForm  resetForm={collapseAddAdvertiser} CallBackToggle={toggleAddAdvertiser} t={t} /> */}
                        </Panel>
                            }
                    </Col>
                </Row>
                  
        </Container>
        )
    }

    return <>
    {renderCampaignsComponent()}
    </>


};




Campaigns.propTypes = {
    t: PropTypes.func.isRequired,
    rtl: RTLProps.isRequired,
  };



  export default compose(withTranslation('common'), connect(state => ({
    rtl: state.rtl,
  })))(Campaigns);