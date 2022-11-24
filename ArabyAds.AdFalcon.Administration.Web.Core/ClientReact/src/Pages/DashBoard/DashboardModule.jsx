import * as React from 'react';
import * as ReactDOM from 'react-dom';
import  {DashboardComponent} from './DashboardComponent';
// import asyncComponent from '../hoc/AsyncComponent/AsyncComponent';


// const AsyncMenu = asyncComponent(() => {
//     return import('./Menu');
//   });

ReactDOM.render(<DashboardComponent.Dashboard />,document.getElementById("dashboard-page-content"))
