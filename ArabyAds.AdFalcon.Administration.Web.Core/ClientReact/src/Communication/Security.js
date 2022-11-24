import axios, { ResponseStatusCodes } from './Axios';
import Constants from '../Config/Constants';
import { setCookie, getCookie } from '../Utils/Cookies';


export const Authorize = async ({ username }, successCallback, errorCallback) => {

    try {
        setCookie(Constants.keys.cookies.user, username);

        const token = await setNewToken();

        successCallback(token.data);
    } catch (error) {
        errorCallback(error);
    }
};

export const setNewToken = async () => {

    const response = await axios.request({
        url: Constants.backend.tokenUrl,
        method: 'POST',
        data: {
            userName: '',
            password: ''
        }
    });

    setCookie(Constants.keys.cookies.token, response.data.access_token);
    setCookie(Constants.keys.cookies.refreshToken, response.data.refresh_token);

    return response;
};

export const refreshToken = async () => {
    try {
        //#region :: REFRESH
        const refreshTokenResponse = await axios.request({
            url: Constants.backend.refreshTokenUrl,
            method: 'POST',
            data: {
                userName: '',
                password: ''
            }
        });

        setCookie(Constants.keys.cookies.token, refreshTokenResponse.data.access_token);
        setCookie(Constants.keys.cookies.refreshToken, refreshTokenResponse.data.refresh_token);

        return refreshTokenResponse;
        //#endregion
    }
    catch (error) {
        //#region :: GET NEW TOKEN
        if (error?.response?.status == ResponseStatusCodes.BAD_REQUEST &&
            error?.response?.data?.error?.toLowerCase() == 'invalid_grant'.toLowerCase() &&
            error?.response?.data?.error_description?.toLowerCase() == 'Token is not active'.toLowerCase()) {
            //Authorize Again:
            const token = await setNewToken();
        } else {
            //throw error;
        }
        //#endregion
    }

}


