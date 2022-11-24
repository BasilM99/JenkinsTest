import React from 'react';
import BaseComponent from './BaseComponent';
class ValidationError extends BaseComponent {
    render() {
        const ret = this.props.shouldValidate && !this.props.valid ? (
           
this.props.customsm?(
		   <div className="col-sm-3">
                <span className="validation-error">
                    {this.props.error}
                </span>
            </div>):(
			
			 <div className="col-sm-4">
                <span className="validation-error">
                    {this.props.error}
                </span>
            </div>)
			
			)
        : "";
        return ret;
    }
}
export default ValidationError;