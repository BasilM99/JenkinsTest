import React, { Component } from 'react';
import BaseComponent from './BaseComponent';

class AlertWhiteSpace extends BaseComponent {
    constructor(props) {
        super(props);
    }
    componentDidMount() {
        document.addEventListener("keydown", this.onEscapeKey, false);
    }
    componentWillUnmount() {
        document.removeEventListener("keydown", this.onEscapeKey, false);
    }

    onEscapeKey = (e) => {
        if (e.keyCode === 27) {
            this.props.onEscapeKey(e);
        }
    }

    render() {
        return (
            <div className={"alert-container " + (this.props.show && " show-alert")} >
                <div className="alert-backdrop"></div>

                <div className="alert-content">
                    <div className="alert-title-bar">
                        <span className="alert-title">{this.props.title}</span>
                        <button type="button" className="btnT-close-alert" onClick={this.props.onCancel}><i className="fa fa-times"></i></button>
                    </div>

                    <div className="alert-dialog"  style={{whiteSpace:'pre-wrap'}}>
                        {this.props.text}
                    </div>


                    <div className="alert-buttons">
                        <div className="pull-right">
                            <button type="button" className="btnT-alert pull-right" onClick={this.props.onConfirm}>{this.props.confirmButtonText}</button>

                            {this.props.showCancelButton && <button type="button" onClick={this.props.onCancel} className="btnT-alert">{this.props.cancelButtonText}</button>}
                        </div>
                    </div>
                </div>



            </div>
        );
    }
}

export default AlertWhiteSpace;