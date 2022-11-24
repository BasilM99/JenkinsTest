import React, { Component } from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
const SecretRoute = ({ component: Component, ...rest }) => (
    <Route {...rest} render={(props) => (
        AuthService.isAuthenticated === true
            ? <Component {...props} />
            : <Redirect to='/login' />
    )} />
);
export default SecretRoute;