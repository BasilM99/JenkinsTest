
import React from 'react';
import {useState} from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { 	DropdownItem, DropdownMenu, DropdownToggle, UncontrolledDropdown, Table,
    UncontrolledPopover,Tooltip ,Popover, PopoverHeader, PopoverBody,Col, Container, Row, ButtonToolbar  ,ButtonGroup, Button ,Card ,CardBody , Modal, ModalHeader, ModalBody, ModalFooter, Input, Label, Form, FormGroup} from 'reactstrap';

import useLocalization from '../Hooks/useLocalization';


import { changeLanguage, changeTheme, UISelectors } from '../Store/ReducerSlices/UI';
import {  userInitialized, userRoleInitialized,ChangeUserRole ,UserSelectors } from '../Store/ReducerSlices/User';



const GridToolbar = ({idTab,id,dataItem, dataIndex , children }) => {
    const { T, Resources } = useLocalization();

    const dispatch = useDispatch();

    const state = useSelector(state => state);

    const currentLanguage = UISelectors.getCurrentLanguage(state);
    const currentTheme = UISelectors.getCurrentTheme(state);

    
    const currentUserRole = UserSelectors.getCurrentUserRole(state);
    const currentUserName = UserSelectors.getCurrentUserName(state);
    const UserGlobal = UserSelectors.getUser(state);
    const [XP, setXP] = useState(0);
    const CurrentDirection= UISelectors.getCurrentDirection(state);
    const [YP, setYP] = useState(0);

  const [openToolbarAction, setopenToolbarAction] = useState(false);
  const toggleToolbarAction= () => setopenToolbarAction(!openToolbarAction);


const handleElementClick=(e,id,idTab,dataIndex)=>
{

const component = document.getElementById(id);
const componentTab = document.getElementById(idTab);
if (!component) {
  return {};
}
if (!componentTab) {
    return {};
  }
const rect = component.getBoundingClientRect();

const topToggle=document.getElementById(idTab).offsetTop;
const topIcon=document.getElementById(id).offsetTop;
if(CurrentDirection=="ltr")
setXP(-1*rect.width-document.getElementById(id).offsetLeft);
else
setXP(1*rect.width);
setYP(topToggle +topIcon );
}

    return <>
    <div    id={"QuickAction"+dataItem.Id} onClick={e =>{handleElementClick(e, id, idTab, dataIndex);toggleToolbarAction()} }>
                                        <ButtonToolbar>
    <ButtonGroup className="btn-group--icons" dir={CurrentDirection}>
          <Button size="sm" color="primary"   id={"PopoverLegacy"} type="button" ><span className="lnr lnr-pushpin" /></Button>    </ButtonGroup></ButtonToolbar>
         
            
                                    
    
                                    
              
            </div>
    {openToolbarAction && CurrentDirection=='ltr' &&
            <div id={"QuickActionToolbar"+dataItem.Id}  dir={CurrentDirection} style={{visibility: 'visible', margin: '0px', position: 'absolute', inset: '0px 0px auto auto', transform: `translate(${XP}px,${YP}px)`}}>     
      {/*       <ButtonToolbar>
    
              <ButtonGroup className="btn-group--icons" dir="ltr">
                <Button outline><span className="lnr lnr-pushpin" /></Button>
                <Button outline><span className="lnr lnr-heart-pulse" /></Button>
                <Button outline><span className="lnr lnr-cog" /></Button>
                <Button outline><span className="lnr lnr-magic-wand" /></Button>
              </ButtonGroup>
            </ButtonToolbar> */}
            {children}
            
            </div>}
          
            {openToolbarAction && CurrentDirection=='rtl' &&
            <div id={"QuickActionToolbar"+dataItem.Id}  dir={CurrentDirection} style={{visibility: 'visible', margin: '0px', position: 'absolute', inset: '0px auto auto 0px', transform: `translate(${XP}px,${YP}px)`}}>     
      {/*       <ButtonToolbar>
    
              <ButtonGroup className="btn-group--icons" dir="ltr">
                <Button outline><span className="lnr lnr-pushpin" /></Button>
                <Button outline><span className="lnr lnr-heart-pulse" /></Button>
                <Button outline><span className="lnr lnr-cog" /></Button>
                <Button outline><span className="lnr lnr-magic-wand" /></Button>
              </ButtonGroup>
            </ButtonToolbar> */}
            {children}
            
            </div>}   
                    
          </>
               
};


export default GridToolbar;