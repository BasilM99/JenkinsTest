import React, { Component } from 'react';
import PropTypes from 'prop-types';
import SidebarLink from './SidebarLink';
import SidebarCategory from './SidebarCategory';
import { UserProps } from '../../../shared/prop-types/ReducerProps';

const SideAdFalconMenu = ({ menu ,hideSidebar,sidebarCollapse}) => {

  const menuData= { ...menu };

  const renderMenu = (menu, level = 1) => {

      if (!menu || menu.length <= 0)
          return;

      var a = [] ;
     return  menu.map(item => {



         { if(item.items && item.items.length>0)
          {
 
            return  <SidebarCategory title={item.label} icon={item.icon} sidebarCollapse={sidebarCollapse}>
            {  renderMenu(item.items, level + 1)}
            </SidebarCategory>
          
          }
            else
            {

              return <SidebarLink key={item.id}  title={item.label}
              icon={item.icon}
              //route={item.href}
              route="/booking_dashboard"
              onClick={hideSidebar}>
            
              </SidebarLink>

            }

          }
                 
                     
                
         



              
          
         

      });
   
  };

  return <>


           


       

        


              {
                   renderMenu(menuData.items, 1)
              }
  
    
  </>;
};


class SidebarContent extends Component {
  static propTypes = {
    changeToDark: PropTypes.func.isRequired,
    changeToLight: PropTypes.func.isRequired,
    onClick: PropTypes.func.isRequired,
    sidebarCollapse: PropTypes.bool,
    user: UserProps.isRequired,
  };

  static defaultProps = {
    sidebarCollapse: false,
  }

  hideSidebar = () => {
    const { onClick } = this.props;
    onClick();
  };

  render() {
    const { changeToLight, changeToDark, sidebarCollapse ,user} = this.props;

    return (
      <div className="sidebar__content">
        <ul className="sidebar__block">

        <SideAdFalconMenu menu={user.SideMenu}   hideSidebar={this.hideSidebar}  sidebarCollapse={sidebarCollapse}>  </SideAdFalconMenu>
         {/*   <SidebarLink
            title="Dashboard"
            icon="home"
            route="/default_dashboard"
            onClick={this.hideSidebar}
          />
          <SidebarLink
            title="Account Information"
            icon="store"
            route="/e_commerce_dashboard"
            onClick={this.hideSidebar}
          />
          <SidebarLink
            title="ADM Account Settings"
            icon="smartphone"
            route="/app_dashboard"
            onClick={this.hideSidebar}
          />
          <SidebarLink
            title="Account Balance"
            icon="apartment"
            route="/booking_dashboard"
            onClick={this.hideSidebar}
          />
          <SidebarLink
            title="General Settings"
            icon="menu"
            route="/finance_dashboard"
            onClick={this.hideSidebar}
          />
          <SidebarLink
            title="Audit Trail"
            icon="heart-pulse"
            route="/fitness_dashboard"
            onClick={this.hideSidebar}
          />  */}

          <SidebarCategory title="Layout" icon="layers" sidebarCollapse={sidebarCollapse}>
            <button className="sidebar__link" type="button" onClick={changeToLight}>
              <p className="sidebar__link-title">Light Theme</p>
            </button>
            <button className="sidebar__link" type="button" onClick={changeToDark}>
              <p className="sidebar__link-title">Dark Theme</p>
            </button>
          </SidebarCategory>
          <SidebarLink
            title="Log In"
            route="/log_in"
            icon="user"
            onClick={this.hideSidebar}
          />
        </ul>
      </div>
    );
  }
}

export default SidebarContent;
