import React from 'react';
import BaseComponent from './BaseComponent';
class Loading extends BaseComponent {
    render() {
        return (<div>{this.T("loading")}</div>);
    }
}
export default Loading;