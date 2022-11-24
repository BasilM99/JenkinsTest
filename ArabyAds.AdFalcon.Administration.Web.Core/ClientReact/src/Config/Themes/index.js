import darkTheme from './dark';
import lightTheme from './light';


export default {
    light: { ...lightTheme },
    dark: { ...darkTheme }
};


export const ThemeTypes = {
    LIGHT: 'light',
    DARK: 'dark'
};