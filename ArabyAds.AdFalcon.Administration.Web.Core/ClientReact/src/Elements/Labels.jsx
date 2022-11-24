import React from 'react'
import styled, { css } from 'styled-components';


export const Label = ({ children, type, bold }) => {

    return <>
        <Text type={type} bold={bold}>
            {children}
        </Text>
    </>;

};



//#region :: Style

const Text = styled.label`
color:${({ theme, type }) => theme[type]};
font-size:${({ theme }) => theme.xsmFontSize};
font-weight:${({ bold }) => bold && 'bold'};
margin-bottom: 0;
`;

//#endregion