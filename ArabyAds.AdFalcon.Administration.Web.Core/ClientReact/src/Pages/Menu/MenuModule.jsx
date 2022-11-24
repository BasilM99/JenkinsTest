import * as React from 'react';
import * as ReactDOM from 'react-dom';
import  Menu from './Menu';
// import asyncComponent from '../hoc/AsyncComponent/AsyncComponent';


// const AsyncMenu = asyncComponent(() => {
//     return import('./Menu');
//   });

ReactDOM.render(<Menu />,document.getElementById("vertical-menu"))
