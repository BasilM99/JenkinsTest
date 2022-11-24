
const set = (key, value) => {
    localStorage.setItem(key, value);
};

const remove = (key) => {
    localStorage.removeItem(key);
};

const get = (key, defVal = null) => {
    return localStorage.getItem(key) || defVal;
};

export default {
    set,
    remove,
    get
};