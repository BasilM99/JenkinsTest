import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { apiCallBegan } from './Store/Actions/api';
import { advertisersListFetched, campainsListFetched, getCampainsList, getAdvertisersList, getAdvertiserById } from './Store/ReducerSlices/Entities';
import Constants from './Config/Constants';
import TestPage from './Pages/Test';


const App = ({ }) => {


    const dispatch = useDispatch();

    //on load
    useEffect(() => {

        //#region :: Load Advertisers List

        dispatch(apiCallBegan({
            url: Constants.backend.advertisorsListUrl,
            method: 'GET',
            onSuccess: advertisersListFetched.type
        }))

        //#endregion

        //#region :: Load Campains List

        dispatch(apiCallBegan({
            url: Constants.backend.campainsListUrl,
            method: 'GET',
            onSuccess: campainsListFetched.type
        }))

        //#endregion



    }, []);



    return <>
        <TestPage />
    </>;


};


export default App;



