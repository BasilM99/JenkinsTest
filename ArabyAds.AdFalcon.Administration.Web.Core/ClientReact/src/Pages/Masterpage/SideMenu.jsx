import React, { useState } from 'react';
import styled from 'styled-components';

const SideAdFalconMenu = ({ menu }) => {

  const [menuData, setMenuData] = useState({ ...menu })

  const renderMenu = (menu, level = 1) => {

      if (!menu || menu.length <= 0)
          return;

      var a = [] ;
       menu.map(item => {

        a.push(

            <li key={item.id}>
                <a href={item.href} className={item.className}>
                  <i className={item.icon}></i>
                  <span>{item.label}</span>
                </a>

               {/*  <ul className='sub-menu' aria-expanded='false'>
                  <li><a href='#'>Setting 1</a></li>
                  <li><a href='#'>Setting 2</a></li>
                </ul> */}
                {
                      item.items && <ul  className='sub-menu'  aria-expanded={item.showBranchesLine}  level={level + 1} active={item.active} showBranchLine={item.showBranchesLine}>
                          {
                              renderMenu(item.items, level + 1)
                          }
                      </ul>
                }
            </li>);



              
          
         

      });
      return a;
  };

  return <>


           


       

        
<div data-simplebar class='h-100'>
      <ul className='metismenu list-unstyled' id='side-menu'>
              <li className='menu-title'>Menu</li>
       {/*    <Menu level={1} active={menuData.active} showBranchLine={menuData.showBranchesLine}> */}
              {
                  renderMenu(menuData.items, 1)
              }
          </ul>
      </div>
  </>;
};



export default SideAdFalconMenu 