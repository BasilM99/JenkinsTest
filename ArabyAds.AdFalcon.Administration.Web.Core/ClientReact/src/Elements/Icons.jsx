
import React from 'react';
import styled from 'styled-components';


export const IcHome = ({ size = 24, color }) => {
    return <>
        <svg  xmlns="http://www.w3.org/2000/svg" width={size} height={size} viewBox="0 0 24 24"  >
            <path d="M0,0H24V24H0Z" fill="none" />
            <Path d="M21,20a1,1,0,0,1-1,1H4a1,1,0,0,1-1-1V9.49a1,1,0,0,1,.386-.79l8-6.222a1,1,0,0,1,1.228,0l8,6.222A1,1,0,0,1,21,9.49V20Zm-2-1V9.978L12,4.534,5,9.978V19Z" fillColor={color} />
        </svg>
    </>;
};






//#region :: Styles

const Path = styled.path`
    fill:${({ theme, fillColor }) => fillColor || theme.iconColor};
`;



//#endregion