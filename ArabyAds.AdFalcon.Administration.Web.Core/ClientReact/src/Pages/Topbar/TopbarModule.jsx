import * as React from 'react';
import * as ReactDOM from 'react-dom';
import  TopbarComponent from './TopbarComponent';
// import asyncComponent from '../hoc/AsyncComponent/AsyncComponent';


// const AsyncMenu = asyncComponent(() => {
//     return import('./Menu');
//   });

ReactDOM.render(<TopbarComponent />,document.getElementById("page-topbar"))
