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
import UploaddevicesId from './UploaddevicesId';
import AddNewAudienceList from './AddNewAudienceList';
//import {loadAdv} from './AddAdvertiserModal';
import LoadingSpinner from '../../Components/LoadingSpinner';
import Collapse from '../../Components/CustomCollapse';
import CustomConfirmationModal from '../../Components/CustomConfirmationModal';
import AdFalconGrid, {AdFalconColumn}  from '../../Components/Grids';
import useCurentUser from '../../Hooks/useCurrentUser'
import useCurrentUI from '../../Hooks/useMyCurrentUI'
import {UserRoles,UserPermissionCode,SystemRoles} from '../../Config/Enums';
import { SubmissionError } from 'redux-form';  // ES6
import Panel from '../../Components/Panel';


//export const Extended;

const AdFalconAudienceLists = ({ t, rtl }) => {
	const dispatch = useDispatch();
    useEffect(() => {

       // pageChange();

    },[]);
    
    const useuseCurrentUIHook = useCurrentUI();
    const UI =useuseCurrentUIHook.UI;

    var buttonLabel= "test";
    var className = "";


    const [collapseAddAudienceList, setcollapseAddAudienceList] = useState(false);

    const toggleAddAudienceList = () =>{
        //showBasicNotificationForAdFalcon("ff","ff","primary","right-up");
        setcollapseAddAudienceList(! collapseAddAudienceList);    
           /*  dispatch(loadAdv({
                Name: "hfhfgh"
           
         }))*/
    
    };

    const [collapseUploaddevicesId, setcollapseUploaddevicesId] = useState(false);
    const toggleUploaddevicesId = () =>{
        //showBasicNotificationForAdFalcon("ff","ff","primary","right-up");
        setcollapseUploaddevicesId(! collapseUploaddevicesId);    
           /*  dispatch(loadAdv({
                Name: "hfhfgh"
           
         }))*/
    
    };
    const [toEditObject , setToEditObject] = useState({});
    const useCurrentUserHook = useCurentUser();
    const currentAccountId = useCurrentUserHook.UserGlobal.AccountId;
    const tempUrlParts = location.pathname.split('/');
    const AdvertiserId = tempUrlParts[tempUrlParts.length - 1];
    
    const [state , setLocalState] = useState({
        categories: [],
        isRefresh:true,
        pending:"",
        color:'danger',
        lastSuccess:"",
        show: true ,
        ConfirmTitle:t('Global:Confirm'),
        ConfirmMessage:t('AudienceList:SelectConfirmation'),
        idsToArchive:[],
        currentAudienceListId:0,
        filters:{
            FromDate:moment(new Date()),
            ToDate:moment(new Date()),
            name:"",Size:10,
            Page:1,keyGrid:0,
            showArchived:false,
        }
    });
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

    const collectAudienceListToArchive = (items) =>{
        if(items.length > 0)
        {
            let tempAudienceLists = items.map(x => {
                return x.Name 
             }).join(" \n ");
             let tempMess = t('Campaign:DeleteAudienceLists');
             state.ConfirmMessage = tempMess+"\n  "+tempAudienceLists;
             state.idsToArchive = items.map(x => x.Id)
             state.color='warning';
             setLocalState({...state});
        }else{
            
            state.ConfirmMessage = t('AudienceList:SelectConfirmation');
            state.idsToArchive = [];
            state.color='danger';
            setLocalState({...state});
        }
    }
    const contextMenu =(props) =>{
        return <>
                <button className="btn-outline-primary btn"  onClick={() => onEditAudienceListClick(props)} title={t('Commands:Edit')}><span className="ri-pencil-line p-2" /></button>
                <button className="btn-outline-primary btn " onClick={()=> onUploadDeviceClick(props)} title={t('AudienceList:UploadDevices')}><span className="ri-file-upload-line p-2" /></button>               
            </>
        

    }

    const SelectedIdsGrid=(items)=>{
      
        collectAudienceListToArchive(items)
    
    }

    const onEditAudienceListClick = (props) =>{
        setToEditObject({
            name:props.dataItem.Name.Value,
            Description:props.dataItem.Description,
            id:props.dataItem.ID
        });
        toggleAddAudienceList();
    }

    const onAddAudienceListClick = () =>{
        
        setToEditObject(false);
        toggleAddAudienceList();
    }

    const onUploadDeviceClick =(props)=>{
        setToEditObject({
            name:props.dataItem.Name.Value,
            Description:props.dataItem.Description,
            id:props.dataItem.ID
        });
        toggleUploaddevicesId();
    }

    const renderAudienceListComponent = () =>{
        //pageChange();
        return(
            <Container className="dashboard">
                <Row>
                <Col md={12}>
                    {/* <h3 className="page-title">{t('default_dashboard.page_title')}</h3> */}
                    <h3 className="page-title">{t('Global:AudienceList')}</h3>
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
                        <CardBody  >
                            <div className="card__title">
                                <i class="ri-table-2"></i>
                                <h5 className="bold-text">{t('Global:AudienceList')}</h5>
                            </div>
                        

                            {/* <h4 className='card-title mb-4 font-size-18'></h4> */}
                           <Container className='table-responsive'>
                                {/* <LoadingSpinner loadingData={true} /> */}
                             
                                <AdFalconGrid

                                    //pageable={true}
                                    key={state.filters.keyGrid}
                                    pageable={ true  }
                                    scrollable={ true  }
                                    reorderable={true}
                                    filters={state.filters}
                                    style={{ height: '400px', fontSize: '13px' }}
                                    sortable={false}
                                    APIurl={Constants.backend.getAudienceListIdUrl+"?Id="+AdvertiserId}
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
                                    >

                                    <AdFalconColumn field="en"  title={t('Global:Name')} />
                                    <AdFalconColumn field="Description" title={t('Campaign:Description')}  width="130px"/>
                                    <AdFalconColumn field="Performance.NoOfHits"  title={t('AppSite:Impressions')} />
                                    <AdFalconColumn field="Performance.UniqueUsers"  title={t('Campaign:Clicks')} width="70px"/>
                                    <AdFalconColumn cell={renderStatusMode} title={t('Global:Status')} width="70px"/>
          

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
                                        btn={t('Commands:Delete')}
                                        message={state.ConfirmMessage}
                                        onConfirm={onArchiveButtonClick}
                                    />

                              </Col>
                                <Col  md={12} lg={6} xl={3}>
                                    <Button color="success"  onClick={onAddAudienceListClick}>{t("Global:AddNewAudienceList")}</Button>
                                </Col>
                               
                            </Row>
                      
                <Row>

                    {collapseAddAudienceList && 
                    <Panel CallBackToggle={toggleAddAudienceList} md={6} lg={6} xl={3} color="Primary" divider title={t("Global:AddNewAudienceList")} icon="chart-bars">
                        <AddNewAudienceList  resetForm={collapseAddAudienceList} CallBackToggle={toggleAddAudienceList} t={t}  AdvertiserId={AdvertiserId} toEditObject={toEditObject}/>
                    </Panel>
                    }
                    {collapseUploaddevicesId && 
                    <Panel CallBackToggle={toggleUploaddevicesId} md={6} lg={6} xl={3} color="Primary" divider title={t("AudienceList:UploadDevices")} icon="chart-bars">
                        <UploaddevicesId  resetForm={collapseUploaddevicesId} CallBackToggle={toggleUploaddevicesId} t={t}  AdvertiserId={AdvertiserId} toEditObject={toEditObject}/>
                    </Panel>
                    }
                </Row>


                
                              
                               
        </Container>
        )
    }

    return <>
    {renderAudienceListComponent()}
    </>


};




AdFalconAudienceLists.propTypes = {
    t: PropTypes.func.isRequired,
    rtl: RTLProps.isRequired,
  };



  export default compose(withTranslation('common'), connect(state => ({
    rtl: state.rtl,
  })))(AdFalconAudienceLists);