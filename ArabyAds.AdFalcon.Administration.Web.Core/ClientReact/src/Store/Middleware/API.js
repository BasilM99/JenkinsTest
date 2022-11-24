//import axios from 'axios';
import Constants from '../../Config/Constants';
import * as actions from '../Actions/API';
import axios from '../../Communication/Axios';


let requestsCancelations = [];

const API = ({ dispatch }) => next => async action => {
    if (action.type !== actions.apiCallBegan.type) return next(action);

    next(action);

    const { url, method, data, onSuccess, onError, args } = action.payload;

    try {

        //Cancel Repetitive Requests
        let currentUrlRequests = requestsCancelations.filter(x => x.url === url);
        if (currentUrlRequests) {
            currentUrlRequests.map(request => request.cancelRequest());
            requestsCancelations = requestsCancelations.filter(x => x.url !== url);
        }
        const response = await axios.request({
            baseURL: Constants.backend.baseUrl,
            url,
            method,
            data,
            cancelToken: new axios.CancelToken(c => requestsCancelations.push({ url, cancelRequest: c }))
        });

        requestsCancelations = requestsCancelations.filter(x => x.url !== url);

        //General:
        dispatch(actions.apiCallSuccess(response.data));

        //Specifc:
        if (onSuccess) {

            dispatch({ type: onSuccess, payload: response.data, args });
        }

    } catch (error) {
        if (axios.isCancel(error)) {
            return;
        }
        console.log('CALL_API_ERROR', error);
        //General:
        dispatch(actions.apiCallFailed(error.message));

        //Specifc:
        if (onError) {
            dispatch({ type: onError, payload: error, args });
        }

    }
};

export default API;