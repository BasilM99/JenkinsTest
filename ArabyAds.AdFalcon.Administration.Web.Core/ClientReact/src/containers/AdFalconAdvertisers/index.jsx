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
import SimpleForm from './AddAdvertiserModal';
//import {loadAdv} from './AddAdvertiserModal';
import Collapse from '../../Components/CustomCollapse';
import CustomConfirmationModal from '../../Components/CustomConfirmationModal';
import AdFalconGrid, {AdFalconColumn}  from '../../Components/Grids';
import useCurentUser from '../../Hooks/useCurrentUser'
import useCurrentUI from '../../Hooks/useMyCurrentUI'
import {UserRoles,UserPermissionCode,SystemRoles} from '../../Config/Enums';
import { SubmissionError } from 'redux-form';  // ES6
import Panel from '../../Components/Panel';

class DetailComponent extends GridDetailRow {

    renderImpUniqueImp = (props) =>{
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
    renderClicksUniqueClicks = (props) =>{
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
    render() {
        const data = this.props.dataItem.details;
        const UserRole = this.props.dataItem.UserRole;
      let url  =Constants.backend.getCampaignsByAdvertiserAccountId+"?Id="+this.props.dataItem.Id;
        if (data) {
            return (
                <div>  
              <Row>    
                  
              <AdFalconGrid

//pageable={true}
pageable={ true  }
scrollable={ true  }
reorderable={true}

style={{ height: '400px', fontSize: '13px' }}
sortable={false}
APIurl={url}
keyName={'Id'}
EnableContextMenu={false}
EnableSelection={false}
ContentType={'application/x-www-form-urlencoded'}

filters={{ Page:0,Size:10,keyGrid:1}}
QuickActionTitle={"QuickAction"}
QuickActionWidth={"200px"}


// detail={DetailComponent}
//expandField="expanded"
//onExpandChange={expandChange}
>

                    <AdFalconColumn field="Name"  title={this.props.t('Campaign:CampaignName')}     />
                    <AdFalconColumn field="CreationDate" title={this.props.t('Campaign:CreationDate')}    renderAsDate />
                    <AdFalconColumn field="Status" title={this.props.t('Global:Status')}     />
                    <AdFalconColumn field="BudgetText"  title={this.props.t('Campaign:Budget')}   />
                    <AdFalconColumn cell={this.renderImpUniqueImp} title={this.props.t('Campaign:ImpressionsUniqueImp')}    />
                    <AdFalconColumn cell={this.renderClicksUniqueClicks} title={this.props.t('Campaign:ClicksUniqueClicks')}  />
                    <AdFalconColumn field="Performance.CtrText" title={this.props.t('Campaign:CTR')}  width="80px" />
                    <AdFalconColumn field="Performance.AvgCPCText" title={this.props.t('Campaign:CTR')}   />
              
                   
                    {UserRole ==UserRoles.DSPRole?
                                    <React.Fragment>
                                    <AdFalconColumn field="Performance.AdjustedNetCostText" title={this.props.t('Global:NetCost')}/>
                    <AdFalconColumn field="Performance.GrossCostText" title={this.props.t('Global:GrossCost')} />
                    <AdFalconColumn field="Performance.BillableCostText" title={this.props.t('Global:BillableCost')}    />
                                   
                                    </React.Fragment>

                                    :
                                    <AdFalconColumn field="Performance.BillableCostText" title={this.props.t('Global:BillableCost')}    />
                                    
                                    
                                    }

                </AdFalconGrid></Row>
                <Row>
                    <Button color="success" >{this.props.t('AdChart:AllCampaigns')}</Button>
                </Row>
            </div>
            );
        }
        return (
            <div style={{ height: "50px", width: '100%' }}>
                <div style={{ position: 'absolute', width: '100%' }}>
                    <div className="k-loading-image" />
                </div>
            </div>
        );
    }
}

const ExtendedDetailComponent = withTranslation()(DetailComponent);
//Extended.static = DetailComponent.static;

//export const Extended;

const AdFalconAdvertisers = ({ t, rtl }) => {
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
        ConfirmMessage:t('Advertiser:SelectConfirmation'),
        idsToArchive:[],
        filters:{
            FromDate:moment(new Date()),
            ToDate:moment(new Date()),
            name:"",Size:10,
            Page:1,keyGrid:0,
            showArchived:false,
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



    const onArchiveCheckChange = (e) =>{
        if(typeof(e.target) != "undefined")
            {
                state.filters.showArchived = e.target.checked;
                state.filters.keyGrid=state.filters.keyGrid+1;
                setLocalState({...state});
                //setLocalState({...state});
                //pageChange();
            }
        
        //console.log(e);

        //pageChange();
    }

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



    const onArchiveButtonClick = () => {
        console.log("archive");
        //getAdvertizersDataandArchive

        var baseUrl = Constants.backend.getAdvertizersDataandArchive;

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

    const collectAdvertiserToArchive = (items) =>{
        if(items.length > 0)
        {
            let tempAdvertisers = items.map(x => {
                return x.Name 
             }).join(" \n ");
             let tempMess = t('Confirmation:Archive');
             tempMess = tempMess.replace("{0}",t('Global:Advertisers')) ;
             state.ConfirmMessage = tempMess+"\n  "+tempAdvertisers;
             state.idsToArchive = items.map(x => x.Id)
             state.color='warning';
             setLocalState({...state});
        }else{
            
            state.ConfirmMessage = t('Confirmation:SelectConfirmation');
            state.idsToArchive = [];
            state.color='danger';
            setLocalState({...state});
        }
    }
    const contextMenu =(props) =>{
        let CampaignUrl = "/Campaign?AdvertiseraccId=" + props.dataItem. Id;
        let AudienceListUrl = "/Campaign/AudienceList/" + props.dataItem. Id;
        let DashboardUrl = "/Dashboard/" + props.dataItem. Id + "?chartType=ad";
        let DealsUrl = "/Deals/" + props.dataItem. Id;
        if(useCurrentUserHook.IsUserPermitted(UserPermissionCode.PMPDeal)){

            return <>

                <button className="btn-outline-primary btn"  title={t('Campains:Menu')}><a href={CampaignUrl} code="2"><span className="lnr lnr-pushpin p-2" /></a></button>
                <button className="btn-outline-primary btn " title={t('Global:AudienceList')}><a href={AudienceListUrl} code="6"><span className="lnr lnr-heart-pulse p-2" /></a></button>
                <button className="btn-outline-primary btn" title={t('Menu:Dashboard')}><a href={DashboardUrl} code="1"><span className="lnr lnr-cog p-2" /></a></button>
                <button className="btn-outline-primary btn" title={t('PMPDeal:Deals')}><a href={DealsUrl} code="0"><span className="lnr lnr-magic-wand p-2" /></a></button>
                {useCurrentUserHook.IsUserPrimary?
                    <React.Fragment>
                        <button className="btn-outline-primary btn" title={t('Global:ContentLists')}><a href={"/Campaign/MasterAppSites/" + props.dataItem. Id} code="5"><span className="lnr lnr-magic-wand p-2" /></a></button>
                        <button className="btn-outline-primary btn" title={t('Targeting:Pixels')}><a href={"/Campaign/TrackingPixel/" + props.dataItem. Id} code="7"><span className="lnr lnr-magic-wand p-2" /></a></button>
                        <button className="btn-outline-primary btn" title={t('AppSite:Settings')}><a href={"/Campaign/AccountAdvertiserSettings/"+ props.dataItem. Id} code="3"><span className="lnr lnr-magic-wand p-2" /></a></button>
                    </React.Fragment>
                : <React.Fragment>
                    <button className="btn-outline-primary btn" title={t('Global:ContentLists')}><a href={"/Campaign/MasterAppSites/"+ props.dataItem. Id} code="5"><span className="lnr lnr-magic-wand p-2" /></a></button>
                    <button className="btn-outline-primary btn" title={t('Targeting:Pixels')}><a href={"/Campaign/TrackingPixel/"+ props.dataItem. Id} code="7"><span className="lnr lnr-magic-wand p-2" /></a></button>
                  </React.Fragment>}
                {useCurrentUserHook.IsAdmin() && useCurrentUserHook.IsUserHasRole(SystemRoles.AdOps)?
                <React.Fragment>
                    <button className="btn-outline-primary btn" title={t('PMPDeals:unArchive')}><a href="/Campaign/unArchiveAdvertiser/" code="4" extraPrams = "unArchive"><span className="lnr lnr-magic-wand p-2" /></a></button>
                    <button className="btn-outline-primary btn" title="Lookalike"><a href={"/Campaign/AudienceListForAdmin/"+props.dataItem. Id} code="6"><span className="lnr lnr-magic-wand p-2" /></a></button>
                </React.Fragment>:
                <React.Fragment>

                </React.Fragment>
                }
        
            </>
        }else{
            
            return <>

                <button className="btn-outline-primary btn" title={t('Campains:Menu')}><a href={CampaignUrl} code="2"><span className="lnr lnr-pushpin p-2" /></a></button>
                <button className="btn-outline-primary btn" title={t('Global:AudienceList')}><a href={AudienceListUrl} code="6"><span className="lnr lnr-heart-pulse p-2" /></a></button>
                <button className="btn-outline-primary btn" title={t('Menu:Dashboard')}><a href={DashboardUrl} code="1"  extraPrams2="ad"><span className="lnr lnr-cog p-2" /></a></button>
                {useCurrentUserHook.IsUserPrimary?
                    <React.Fragment>
                        <button className="btn-outline-primary btn" title={t('Global:ContentLists')}><a  href={"/Campaign/MasterAppSites/" + props.dataItem. Id} code="5"><span className="lnr lnr-magic-wand p-2" /></a></button>
                        <button className="btn-outline-primary btn" title={t('Targeting:Pixels')}><a  href={"/Campaign/TrackingPixel/" + props.dataItem. Id} code="7"><span className="lnr lnr-magic-wand p-2" /></a></button>
                        <button className="btn-outline-primary btn" title={t('AppSite:Settings')}><a  href={"/Campaign/AccountAdvertiserSettings/"+ props.dataItem. Id} code="3"><span className="lnr lnr-magic-wand p-2" /></a></button>
                    </React.Fragment>
                : <React.Fragment>
                    <button className="btn-outline-primary btn" title={t('Global:ContentLists')}><a href={"/Campaign/MasterAppSites/" + props.dataItem. Id} code="5"><span className="lnr lnr-magic-wand p-2" /></a></button>
                    <button className="btn-outline-primary btn" title={t('Targeting:Pixels')}><a href={"/Campaign/TrackingPixel/"+ props.dataItem. Id} code="7"><span className="lnr lnr-magic-wand p-2" /></a></button>
                  </React.Fragment>}
                {useCurrentUserHook.IsAdmin() && useCurrentUserHook.IsUserHasRole(SystemRoles.AdOps)?
                <React.Fragment>
                    <button className="btn-outline-primary btn" title={t('PMPDeals:unArchive')}><a href="/Campaign/unArchiveAdvertiser/" code="4" extraPrams = "unArchive"><span className="lnr lnr-magic-wand p-2" /></a></button>
                    <button className="btn-outline-primary btn" title="Lookalike"><a href="/Campaign/AudienceListForAdmin/" code="6"><span className="lnr lnr-magic-wand p-2" /></a></button>
                </React.Fragment>:
                <React.Fragment>

                </React.Fragment>
                }
        
            </>
        }


    }

    const SelectedIdsGrid=(items)=>{
      
        collectAdvertiserToArchive(items)
    
    }
    const renderAdvertizersComponent = () =>{
        //pageChange();
        return(
            <Container className="dashboard">
                <Row>
                <Col md={12}>
                    {/* <h3 className="page-title">{t('default_dashboard.page_title')}</h3> */}
                    <h3 className="page-title">{t('Global:Advertisers')}</h3>
                </Col>
                </Row>
                <form  className="form"  onSubmit={e => { e.preventDefault(); }}  >
       
                                        <Row  className='mb-2'>
                                            <Col md={12} lg={6} xl={3}>
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
                                                    <CustomDateRangePicker   CallBackParent={handleParentDateChange} id='testDateRange'  Tovalue={state.filters.ToDate}    Fromvalue={state.filters.FromDate }    nameTo='To' nameFrom='From' titleMsgFrom={UI.direction=='rtl'?'من':'From'} titleMsgTo={UI.direction=='rtl'?'الى':'To'} ></CustomDateRangePicker>
                                                </div>
                                            </Col>
                                            <Col md={12} lg={6} xl={3}>
                                                <div className="form__form-group mt-5">
                                                    <div className="form__form-group-field">
                                                        <CheckBoxField
                                                            onChange={onArchiveCheckChange}
                                                            name="test"
                                                            value={state.filters.showArchived}
                                                            label={t('Global:ShowArchived')}
                                                            defaultChecked />
                                                            {/* <span className="form__form-group-label"></span> */}
                                                    </div>
                                                </div>
                                                
                                            </Col>
                                        </Row>
                              
                               
                       
                     
            </form>
                <Row>   
                    <Card>
                        <CardBody>
                     
                 
                            <div className="card__title">
                                <i class="ri-table-2"></i>
                                <h5 className="bold-text">{t('Global:Advertisers')}</h5>
                            </div>
                        

                            {/* <h4 className='card-title mb-4 font-size-18'></h4> */}
                           <Container >

                             
                                <AdFalconGrid

                                    //pageable={true}
                                    key={state.filters.keyGrid}
                                    pageable={ true  }
                                    scrollable={ true  }
                                    reorderable={true}
                                    filters={state.filters}
                                    style={{ height: '400px', fontSize: '13px' }}
                                    sortable={false}
                                    APIurl={Constants.backend.getAdvertizersData+"?Id="+currentAccountId}
                                    keyName={'Id'}
                                    EnableContextMenu={true}
                                    EnableSelection={true}
                                    ContentType={'application/x-www-form-urlencoded'}

                                    ContextMenu={contextMenu}
                                    QuickActionTitle={"QuickAction"}
                                    QuickActionWidth={"200px"}
                                    DetailComponent={ExtendedDetailComponent}
                                    //expandField="expanded"
                                    //onExpandChange={expandChange}
                                    EnableExpand={true}
                                    CallBackParentSelected={SelectedIdsGrid}
                                    APIurldetails={Constants.backend.getCampaignsByAdvertiserAccountId}
                                    >

                                    <AdFalconColumn field="Name"  title={t('Global:Name')} />
                                    <AdFalconColumn field="AdvertiserItem.Name.Value" title={t('Menu:Advertiser') + " " + t('Global:Name')}  width="130px"/>
                                    <AdFalconColumn cell={renderStatusMode} title={t('Global:Status')} width="70px"/>
                                    <AdFalconColumn field="Performance.Impress"  title={t('AppSite:Impressions')} />
                                    <AdFalconColumn field="Performance.Clicks"  title={t('Campaign:Clicks')} width="70px"/>
                                    <AdFalconColumn field="Performance.CtrText"  title={t('Campaign:CTR')} width="170px" />
                                    <AdFalconColumn field="Performance.AvgCPCText" title={t('Campaign:AvgCPC')}  width="180px" />
                                    {useCurrentUserHook.IsUserHasRole(UserRoles.DSPRole)?
                                    <React.Fragment>
                                        <AdFalconColumn field="Performance.AdjustedNetCostText" title={t('Global:NetCost')}   width="80px"/>
                                        <AdFalconColumn field="Performance.GrossCostText" title={t('Global:GrossCost')}  />
                                        <AdFalconColumn field="Performance.BillableCostText" title={t('Global:BillableCost')}   />
                                    </React.Fragment>

                                    :
                                    <AdFalconColumn field="Performance.BillableCostText" title={t('Global:BillableCost')}   />}




                                </AdFalconGrid>


                            </Container>
                          
                            
                        </CardBody>
                    </Card>
                </Row>

                <Row>
                              <Col  md={12} lg={6} xl={1} >
                              <CustomConfirmationModal
                                        outBtnColor="secondary"
                                        color={state.color}
                                        title={state.ConfirmTitle}
                                        header
                                        btn={t('PMPDeal:Archive')}
                                        message={state.ConfirmMessage}
                                        onConfirm={onArchiveButtonClick}
                                    />

                              </Col>
                                <Col  md={12} lg={6} xl={2}>
                                    <Button color="success" >{t('AdChart:AllCampaigns')}</Button>
                                </Col>
                                <Col  md={12} lg={6} xl={2}>
                                  
                                        
                                        <Button color="success"  onClick={toggleAddAdvertiser}>{t('Advertiser:AddNewAdvertiser')}</Button>
                                       
                                       
                                 
                                </Col>
                               
                            </Row>
                      
                <Row>


      




{/* 
                <Collapse collapseFromOut={ collapseAddAdvertiser} toggleFromOut={toggleAddAdvertiser} title="Add new Advertiser" className="with-shadow">
                                        
                                     
                                            
                                    </Collapse> */}

<Col md={12} >

{collapseAddAdvertiser && 
                                    <Panel CallBackToggle={toggleAddAdvertiser} md={12} lg={6} xl={3} color="Primary" divider title={"Add Advertiser"} icon="chart-bars">
                                    <SimpleForm  resetForm={collapseAddAdvertiser} CallBackToggle={toggleAddAdvertiser} t={t} />
  </Panel>
    }
</Col>
                </Row>


                
                              
                               
        </Container>
        )
    }

    return <>
    {renderAdvertizersComponent()}
    </>


};




AdFalconAdvertisers.propTypes = {
    t: PropTypes.func.isRequired,
    rtl: RTLProps.isRequired,
  };



  export default compose(withTranslation('common'), connect(state => ({
    rtl: state.rtl,
  })))(AdFalconAdvertisers);