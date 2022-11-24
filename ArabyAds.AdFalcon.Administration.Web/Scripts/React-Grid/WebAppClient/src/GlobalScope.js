import React, { Component } from 'react';
const queryStringLib = require('query-string');
class GlobalScope extends Component {
	 
    shared = {
        //Control Language:
        Language: {
            ChangeLanguage: function () {
                if (localStorage.getItem('lang') === 'ar')
                    localStorage.setItem('lang', 'en');
                else
                    localStorage.setItem('lang', 'ar');
                location.reload(true);
            },
            SetLanguage: function (lang) {
                localStorage.setItem('lang', lang);
                location.reload(true);
            }
        }

    };
    state = {
       
      
		
		showItems:true
    };
	componentWillMount() {
	
					
			
				
	
	
}
componentDidMount() {
	
	this.setState({showItems:true});
}

	
    render() {
		if(this.state.showItems)
		{
        return (this.props.children);
		}
		else
		{
			return (
			<div />);
		}
    };
}
export default GlobalScope;


