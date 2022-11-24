import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';
import reducer from './Reducer';
import api from './Middleware/API';
import logger from './Middleware/Logger';

export default () => {
    return configureStore({
        reducer,
        middleware: (getDefaultMiddleware) => {
            return getDefaultMiddleware()
                .concat(api)
                .concat(logger)
        }
    });
};