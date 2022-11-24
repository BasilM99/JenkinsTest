

const Logger = state => next => action => {
    console.log('STORE-ACTION', action);
    return next(action);
};

export default Logger;