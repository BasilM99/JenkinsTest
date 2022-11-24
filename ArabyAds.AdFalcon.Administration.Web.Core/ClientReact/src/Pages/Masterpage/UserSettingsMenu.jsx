import React from 'react';


const UserSettingsMenu = ({ userSettingsList })=>{
  
  var _JuserSettingsMenuList = userSettingsList;
  var renderdList = [];
  _JuserSettingsMenuList.items.map(x => {
    if(!x.isdivider){
      var tempClass = x.icon + "align-middle mr-1"
      if(x.descNum == null)
      {renderdList.push(
      <a className='dropdown-item' href={x.href}><i className={tempClass}></i> {x.title}</a>
      )}
      else{
        renderdList.push(
        <a className='dropdown-item' href={x.href}><span className='badge badge-success float-right mt-1'>{x.descNum}</span><i className={tempClass}></i> {x.title}</a>
      )
    }
      
    }else{
      renderdList.push(<div className='dropdown-divider'></div>);
    }
  })
  return renderdList;
}



export default UserSettingsMenu