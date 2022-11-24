import React from 'react';
import { Route, Switch } from 'react-router-dom';
import MainWrapper from '../MainWrapper';
import Landing from '../../Landing/index';
import NotFound404 from '../../DefaultPage/404/index';
import LockScreen from '../../Account/LockScreen/index';
import LogIn from '../../Account/LogIn/index';
import LogInPhoto from '../../Account/LogInPhoto/index';
import Register from '../../Account/Register/index';
import RegisterPhoto from '../../Account/RegisterPhoto/index';
import ResetPassword from '../../Account/ResetPassword/index';
import ResetPasswordPhoto from '../../Account/ResetPasswordPhoto';
import WrappedRoutes from './WrappedRoutes';
import DefaultDashboard from '../../Dashboards/AdFalconDefaultDashboard/index';

const Router = () => (
  <MainWrapper>
    <main>
      <Switch>
        {/* <Route exact path="/" component={Landing} /> */}
        

        <Route path="/:lang" >
 
      
          <Route path="/:lang" component={WrappedRoutes}     />
      
{/*       
          <Route path="/:lang/Campaign" component={WrappedRoutes}     >
            <Route path="/:lang/Campaign/AccountAdvertisers" component={WrappedRoutes}     />
          </Route> */}
        </Route>

        <Route path="/404" component={NotFound404} />
        <Route path="/lock_screen" component={LockScreen} />
        <Route path="/log_in" component={LogIn} />
        <Route path="/log_in_photo" component={LogInPhoto} />
        <Route path="/register" component={Register} />
        <Route path="/register_photo" component={RegisterPhoto} />
        <Route path="/reset_password" component={ResetPassword} />
        <Route path="/reset_password_photo" component={ResetPasswordPhoto} />
      </Switch>
    </main>
  </MainWrapper>
);

export default Router;
