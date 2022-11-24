import React from 'react';

const BreadCrumb = ({BreadCrumbModel}) =>{
    var _JbreadCrumb = BreadCrumbModel;
    var renderdList = [];
    
    if(_JbreadCrumb && _JbreadCrumb.items)
    {
    _JbreadCrumb.items.map(x => {
      var tempClass = "breadcrumb-item"
      if(x.isActive)
        tempClass += " active "
      if(x.Url != null)
        renderdList.push(<li className={tempClass}><a href={x.Url}>{x.Text}</a></li>)
      else
      renderdList.push(<li className={tempClass}>{x.Text}</li>)
    })
}
    return renderdList;
  }

  export default BreadCrumb 