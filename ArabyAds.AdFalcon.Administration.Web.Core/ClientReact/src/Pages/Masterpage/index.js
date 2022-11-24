
import React from 'react';
import styled from 'styled-components';
import { useSelector, useDispatch } from 'react-redux';


import LogoDarkFull from '../../Assets/img/logo-dark-full.svg';
import LogoDark from '../../Assets/img/logo-dark.svg';
import useLocalization from '../../Hooks/useLocalization';
import { Button } from '../../Elements/Buttons';
import { Group } from '../../Elements/Containers';
import { IcHome } from '../../Elements/Icons';
import { ElementStyle } from '../../Config/Enums';
import { Label } from '../../Elements/Labels';
import { TexBox } from '../../Elements/TextBoxes';
import AdFalconTopMenu from './TopMenu'
import BreadCrumb from './BreadCrumb'
import BreadCrumbData from '../../Components/MenuData/BreadCrumb'
import SideAdFalconMenu from './SideMenu'
import UserSettingsMenu from './UserSettingsMenu'
import { changeLanguage, changeTheme, UISelectors } from '../../Store/ReducerSlices/UI';
import {  userInitialized, userRoleInitialized,ChangeUserRole ,UserSelectors } from '../../Store/ReducerSlices/User';
import Themes from '../../Config/Themes';
import Locales from '../../Localization/Locales';
import { ThemeTypes } from '../../Config/Themes';
import moment from 'moment';




const Masterpage = ({ children }) => {
    const { T, Resources } = useLocalization();

    const dispatch = useDispatch();

    const state = useSelector(state => state);

    const currentLanguage = UISelectors.getCurrentLanguage(state);
    const currentTheme = UISelectors.getCurrentTheme(state);

    
    const currentUserRole = UserSelectors.getCurrentUserRole(state);
    const currentUserName = UserSelectors.getCurrentUserName(state);
    const UserGlobal = UserSelectors.getUser(state);
    
    const handleLanguageChange = (e) => {
        dispatch(changeLanguage({
            language: currentLanguage === Locales.ENGLISH ? Locales.ARABIC : Locales.ENGLISH,
            direction: currentLanguage === Locales.ENGLISH ? 'rtl' : 'ltr'
        }))
    };

    const handleChangeTheme = (e) => {
        dispatch(changeTheme({
            theme: currentTheme === ThemeTypes.LIGHT ? ThemeTypes.DARK : ThemeTypes.LIGHT
        }))
    };
    const handleChangeUserRole = (e) => {
        dispatch(ChangeUserRole({
            theme: e.UserRole
        }))
    };
    return <div   data-sidebar='dark'>

        <div id='layout-wrapper'>

         
            <header  id='page-topbar'>
     <div  className='navbar-header'>

              
          <div class='d-flex'>
          
            <div class='navbar-brand-box'>
              <a href='index.html' class='logo logo-dark'>
                <span class='logo-sm'>
                  <img src='assets/images/logo-sm-dark.png' alt='' height='22' />
                </span>
                <span class='logo-lg'>
                  <img src='assets/images/logo-dark.png' alt='' height='20' />
                </span>
              </a>

              <a href='index.html' class='logo logo-light'>
                <span class='logo-sm'>
                  <img src='assets/images/logo-sm-light.png' alt='' height='22' />
                </span>
                <span class='logo-lg'>
                  <img src='assets/images/logo-dark.png' alt='' height='20' />
                </span>
              </a>
            </div>

            <button type='button' class='btn btn-sm px-3 font-size-24 header-item waves-effect' id='vertical-menu-btn'>
              <i class='ri-menu-2-line align-middle'></i>
            </button>

            <div id='top-menu'>
              <AdFalconTopMenu MenuData={UserGlobal.TopMenu} />
            </div>

          </div>

         
  <div class='d-flex'>




            <div class=' d-sm-inline-block'>
  <button onClick={handleChangeTheme}    type='button' class='btn header-item waves-effect' >
    {/* <img class='' src='assets/images/flags/us.jpg' alt='Header Language' height='16'/>
    <h3> {currentTheme}</h3> */}
  </button>
  </div>
<div class=' d-sm-inline-block'>
  <button onClick={handleLanguageChange}    type='button' class='btn header-item waves-effect' >
   {/*  <img class='' src='assets/images/flags/us.jpg' alt='Header Language' height='16'/>
    <h3>{currentLanguage}</h3> */}
  </button>
{/*   <div class='dropdown-menu dropdown-menu-right'>

  
    <a href='javascript:void(0);' class='dropdown-item notify-item'>
      <img src='assets/images/flags/spain.jpg' alt='user-image' class='mr-1' height='12'/> <span class='align-middle'>Spanish</span>
    </a>

  
    <a href='javascript:void(0);' class='dropdown-item notify-item'>
      <img src='assets/images/flags/germany.jpg' alt='user-image' class='mr-1' height='12'/> <span class='align-middle'>German</span>
    </a>

 
  </div> */}
</div>


<div class='dropdown d-none d-sm-inline-block'>
  <button    type='button' class='btn header-item waves-effect' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>
    <img class='' src='assets/images/flags/us.jpg' alt='Header Language' height='16'/>
    <h3>{currentLanguage}</h3>
  </button>
   <div class='dropdown-menu dropdown-menu-right'>
   { window.AdFalconUserLoggedInUserObject.NormalAccId > 0 &&
        <a href='javascript:void(0);' class='dropdown-item notify-item'>
        <img src='assets/images/flags/spain.jpg' alt='user-image' class='mr-1' height='12'/> <span class='align-middle'>Normal</span>
      </a>
      }
   { window.AdFalconUserLoggedInUserObject.DSPAccId > 0 &&
        <a href='javascript:void(0);' class='dropdown-item notify-item'>
        <img src='assets/images/flags/spain.jpg' alt='user-image' class='mr-1' height='12'/> <span class='align-middle'>DSP</span>
      </a>
      }
    
    { window.AdFalconUserLoggedInUserObject.DataProviderAccId > 0 &&
        <a href='javascript:void(0);' class='dropdown-item notify-item'>
        <img src='assets/images/flags/spain.jpg' alt='user-image' class='mr-1' height='12'/> <span class='align-middle'>Data Provider</span>
      </a>
      }
    
  
   
 
  </div> 
</div>


<div class='dropdown d-none d-lg-inline-block ml-1'>
  <button type='button' class='btn header-item noti-icon waves-effect' data-toggle='fullscreen'>
    <i class='ri-fullscreen-line'></i>
  </button>
</div>



<div class='dropdown d-inline-block user-dropdown'>
  <button type='button' class='btn header-item waves-effect' id='page-header-user-dropdown' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>
    <img class='rounded-circle header-profile-user' src='assets/images/users/avatar-2.jpg' alt='Header Avatar' />
    <span class='d-none d-xl-inline-block ml-1'>{currentUserName}</span>
    <i class='mdi mdi-chevron-down d-none d-xl-inline-block'></i>
  </button>
  <div class='dropdown-menu dropdown-menu-right'>

  <UserSettingsMenu userSettingsList={UserGlobal.UserSettingsMenu} ></UserSettingsMenu>
  </div>
</div>

</div>

        

                        
                  {/*     <!--
                       <Button active type={ElementStyle.ACCENT}>
                            <ButtonIconWrapper><IcHome size={22} /></ButtonIconWrapper>
                            <span> Advertisers </span>
                        </Button>
                        <Button type={ElementStyle.ACCENT}>
                            <ButtonIconWrapper><IcHome size={22} /></ButtonIconWrapper>
                            <span> Reports </span>
                        </Button>
                        <Button type={ElementStyle.ACCENT}>
                            <ButtonIconWrapper><IcHome size={22} /></ButtonIconWrapper>
                            <span> Deals </span>
                        </Button>
                        <Button type={ElementStyle.ACCENT} >
                            <ButtonIconWrapper><IcHome size={22} /></ButtonIconWrapper>
                            <span> Data Provider </span>
                        </Button>
                        
                        --> */}
                    </div>

            {/*         <Group space="8px" dir="row">
                        <Label type={ElementStyle.INFO} bold>17-8-2020 09:46:17 Monday</Label>

                        <TexBox placeholder={'Search'} searchBox />


                        <Button noPadding>
                            <ButtonIconWrapper noMargin><IcHome size={25} /></ButtonIconWrapper>
                        </Button>

                        <Button noPadding>
                            <ButtonIconWrapper noMargin><IcHome size={25} /></ButtonIconWrapper>
                        </Button>

                        <Button noPadding>
                            <ButtonIconWrapper noMargin><IcHome size={25} /></ButtonIconWrapper>
                        </Button>
                    </Group> */}

                </header>

             

                
            <div   className='vertical-menu' >
             {/*    <LogoWrapper>
                    <Logo src={LogoDarkFull} />
                </LogoWrapper> */}

               
                    <SideAdFalconMenu menu={UserGlobal.SideMenu} />
                
            </div>
            <div  className='main-content'>
              <div className="row">
                <div className="col-12">
                  <div className="page-title-box d-flex align-items-center justify-content-between">

                    <div className="page-title-right">
                      <ol className="breadcrumb m-0">
                       
                        <BreadCrumb BreadCrumbModel={BreadCrumbData} ></BreadCrumb>
                      </ol>
                    </div>

                    
                  </div>
                </div>
				      </div>

                {/*[START] Render Page Content Here */}
                <div className='page-content'>
                    {children}
                </div>
                {/*[END] Render Page Content Here */}


                <footer className='footer'>
                <div className='container-fluid'>
               
                <div class='row'>

                    <div class='col-sm-6'>
                        <div class='d-none d-sm-block'>
                            <img src={LogoDark}  />
                        </div>
                    </div>
                    <div class='col-sm-6'>
                        <div class='text-right font-size-11'>
                            Copyright © AdFalcon Marketing &amp; Advertising Cloud
                            {moment().get('year')}
                            . All rights reserved.
                        </div>
                    </div>
                </div>
           
                </div>
               
                </footer>
            </div>

        </div>



    </div>;
};


export default Masterpage;


//#region :: Styles

const Body = styled.div`
color:${({ theme }) => theme.color};
background-color:${({ theme }) => theme.bodyBg};;
display:flex;
flex-direction:${({ flexDir = 'row' }) => flexDir};
direction:${({ theme }) => theme.dir};

`;

const Container = styled.div`
display:flex;
flex-direction:row;
margin:25px 35px;
min-height:700px;
margin-bottom:0;
margin-bottom:10px;
flex-grow:1;
`;

//#region :: SideColumn

const SideColumn = styled.div`
padding:0 20px;
`;

const MenuWrapper = styled.div`

`;

const LogoWrapper = styled.div`
margin-bottom:20px;
`;

const Logo = styled.img`
height:38px;
`;

//#endregion

//#region :: MainColumn
const MainColumn = styled.div`
display:flex;
flex-direction:column;
flex-grow:1;
padding: ${({ theme }) => theme.dir === 'ltr' ? '0 0 0 40px' : '0 40px 0 0'};
`;

const Header = styled.header`
padding-bottom: 15px;
border-bottom: 1px solid ${({ theme }) => theme.info};
display:flex;
justify-content:space-between;
flex-direction:row;
`;

const Placeholder = styled.div`
    flex-grow: 1;
`;

const ButtonIconWrapper = styled.i`
margin:${({ theme, noMargin = false }) => !noMargin && (theme.dir === 'ltr' ? '0 5px 0 0' : '0 0 0 5px')};
`;


//#endregion

//#region :: Footer
const Footer = styled.footer`
display:flex;
justify-content:space-between;
flex-direction:row;

`;

const FooterLogo = styled.img`
height:15px;
`;
//#endregion

//#endregion 