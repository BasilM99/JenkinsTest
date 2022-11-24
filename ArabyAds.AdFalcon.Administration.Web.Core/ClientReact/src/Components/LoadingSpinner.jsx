	
import React, {Component} from 'react';

import { usePromiseTracker } from "react-promise-tracker";
import Loader from "react-loader-spinner";


const  LoadingSpinner = (props) =>{

  





	
	    return (props.loadingData ? 
			<div className="sweet-loading">
				<Loader  type="Circles"  height={80} width={80}
				
				color={"#123abc"}
				//loading={props.loadingData}


				/>
			</div>
            : <div></div>)
	
	
   
};

export default  LoadingSpinner;
 



export const Spinner = (props) => {
  const { promiseInProgress } = usePromiseTracker({area: props.area, delay: 0});

  return (
    promiseInProgress && (
      <div className="spinner">
        <Loader type="ThreeDots" color="#123abc" height="100" width="100" />
      </div>
    )
  );
};
