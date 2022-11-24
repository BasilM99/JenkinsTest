import * as React from 'react';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import TrendingUpIcon from 'mdi-react/TrendingUpIcon';
import TrendingDownIcon from 'mdi-react/TrendingDownIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import {
  Card, CardBody, Col, Progress,
} from 'reactstrap';
import {
  BarChart, Bar, Cell, ResponsiveContainer,
} from 'recharts';
 const DashBoardCardCompnent = ({cardItem}) =>{
    // constructor(props){
	// 	super(props);
        
        
	// 	this.state = {
	// 		externalList : null
    //     };
    // };
 

    const CardShort = ( ) =>{

        var returnedList = [];
        //debugger;
        
        var percentValueStateClass = '';
        if(typeof(cardItem) != "undefined")
        cardItem.map(_props => {
           
            
            returnedList.push(

                <Col md={12} xl={3} lg={6} xs={12}>
                <Card>
                  <CardBody>

                  <div className="panel__btns">
             
        
             <button className="panel__btn" type="button"  onClick={onDismiss}><CloseIcon /></button>
           </div>

                    <div className="card__title">
                      <i class={_props.icon}></i> 
                      <h5>{_props.title}</h5>
                    </div>
                    <div className="dashboard__total">
                      <p className="dashboard__total-stat" style={{fontSize : 20}}>
                        {_props.mainValue}
                      </p>
                      {(_props.percentValueState == 'up')?<TrendingUpIcon className="dashboard__trend-icon" />
                        :<TrendingDownIcon className="dashboard__trend-icon" />}
                      <div className="dashboard__chart-container">
                        <div className="progress-wrap progress-wrap--small progress-wrap--pink-gradient progress-wrap--label-top">
                            <Progress value={_props.percentValue.substring(0,_props.percentValue.indexOf('%'))} ><p className="progress__label">{_props.percentValue}</p></Progress>
                        </div>
                      </div>
                    </div>
                  </CardBody>
                </Card>
              </Col>
            );
        });

        return returnedList;
    } 
    


    const renderCards = () =>{
        
        //var _props = cardItem;
        var returnedList = [];
        
        
        if(typeof(cardItem) != "undefined")
        cardItem.map(_props => {
            if(_props.percentValueState == 'up')
            {
                percentValueStateClass = 'mdi mdi-menu-up';
            }else if(_props.percentValueState == 'down'){
                percentValueStateClass = 'mdi mdi-menu-down';
            }
            var percentValueStateClass = '';

            <Col md={12} xl={3} lg={6} xs={12}>
            <Card>
              <CardBody>
                <div className="card__title">
                  <i class="ri-drag-drop-line"></i> 
                  <h5>{t('online_marketing_dashboard.total_page_views')}</h5>
                </div>
                <div className="dashboard__total">
                  <TrendingUpIcon className="dashboard__trend-icon" />
                  <p className="dashboard__total-stat">
                    {activeItem.amt}
                  </p>
                  <div className="dashboard__chart-container">
                    <ResponsiveContainer height={70}>
                      <BarChart data={data}>
                        <Bar dataKey="amt" onClick={this.handleClick}>
                          {
                            data.map((entry, index) => (
                              <Cell
                                cursor="pointer"
                                fill={index === activeIndex ? '#cd0000' : '#999'}
                                key={`cell-${index}`}
                              />
                            ))
                          }
                        </Bar>
                      </BarChart>
                    </ResponsiveContainer>
                  </div>
                </div>
            
            
              </CardBody>
            </Card>
          </Col>

            returnedList.push(

              <Col md={12} xl={3} lg={6} xs={12}>
              <Card>
                <CardBody>
                  <div className="card__title">
                    <i className={_props.icon}></i> 
                    <h5>{_props.title}</h5>
                  </div>
                  <div className="dashboard__total">
                    <TrendingUpIcon className="dashboard__trend-icon" />
                    <p className="dashboard__total-stat">
                    {_props.mainValue}
                    </p>
                    <div className="dashboard__chart-container">
                      <ResponsiveContainer height={70}>

                      <h5 className="dashboard__booking-total-description">Total profit earned</h5>
        <div className="progress-wrap progress-wrap--small progress-wrap--pink-gradient progress-wrap--rounded">
          <p className="dashboard__booking-card-progress-label progress__label">87%</p>
          <Progress value={87} />
        </div>
                   {/*    <div className='col-md-3' key={_props.title}>
                    <div className='card'>
                        <div className='card-body'>
                            <div className='media'>
                                <div className='media-body overflow-hidden'>
                                    <p className='text-truncate font-size-18 mb-2'>{_props.title}</p>
                                    <h4 className='mb-0 font-size-32'>{_props.mainValue}</h4>
                                </div>
                                <div className='text-primary'>
                                    <i className={_props.icon}></i>
                                </div>
                            </div>
                        </div>
    
                        <div className='card-body border-top py-3'>
                            <div className='text-truncate'>
                                <span className='badge badge-soft-success font-size-12 p-2'><i className={percentValueStateClass}> </i> {_props.percentValue} </span>
                                <span className='text-muted ml-2 font-size-11'>{_props.percentValueDisc}</span>
                            </div>
                        </div>
                    </div>
                </div>
 */}
                        </ResponsiveContainer>

                        </div>
</div>
</CardBody>


</Card>
</Col>
            )
            
            //return( renderCards(cardItem) )
        })
    
        
        return returnedList;
    }
    
    return <>
   
     {CardShort()} 
    </>
}
export default DashBoardCardCompnent;