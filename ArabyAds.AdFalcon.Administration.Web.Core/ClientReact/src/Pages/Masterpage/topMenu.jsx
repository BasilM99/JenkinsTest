import React from 'react';



const AdFalconTopMenu = ({ MenuData}) => {
    const topMenuRender = () =>{
        var _JtopMenuList = MenuData;
        var renderdList = [];
        _JtopMenuList.items.map(x => {
        var tempClass = x.icon + " align-middle mr-2 font-size-18"
          renderdList.push(<button type='button' className='btn btn-primary mr-2 waves-effect waves-light'>
          <i className={tempClass}></i>
          {x.title}
        </button>)
        })
        return renderdList;
      }


    return<>
    {topMenuRender()}
    </>
}


export default AdFalconTopMenu 