import { createSlice } from '@reduxjs/toolkit';



const entities = createSlice({
    name: 'entities',
    initialState: {
        advertisersList: [],
        campainsList: []
    },
    reducers: {
        advertisersListFetched: (entities, action) => {
            entities.advertisersList = action.payload;
        },
        campainsListFetched: (entities, action) => {
            entities.campainsList = action.payload;
        }
    }
});

export const { advertisersListFetched, campainsListFetched } = entities.actions;
export default entities.reducer;




//#region :: Selector

export const getAdvertisersList = (state) => {
    return state.entities.advertisersList || [];
};

export const getAdvertiserById = (state, id) => {
    return state.entities?.advertisersList?.find(x => x.Id === id);
};

export const getCampainsList = (state) => {
    return state.entities.campainsList || [];
};



//#endregion