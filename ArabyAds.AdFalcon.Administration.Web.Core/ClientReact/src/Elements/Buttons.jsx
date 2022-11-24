import React, { useState } from 'react';
import styled, { css } from 'styled-components';



export const Button = ({ children, type, active = false, noPadding, onClick }) => {

    const handleClick = (e) => {
        if (onClick)
            onClick(e);
    };

    return <>
        <ButtonStyled noPadding={noPadding} onClick={handleClick} active={active} type={type}>
            {children}
        </ButtonStyled>
    </>;

};

export const ButtonsGroup = ({ children, vertical }) => {
    return <>
        <ButtonsGroupWrapper vertical={vertical}>
            {children}
        </ButtonsGroupWrapper>
    </>;
};


//#region :: Style

const ButtonStyled = styled.button`
display: flex;
align-items: center;
outline:none;
border:none;
padding:${({ noPadding = false }) => (!noPadding && '6px 10px') || 0};
background-color:${({ theme, active, type }) => active && theme[type] || 'transparent'};
border-radius:${({ theme }) => theme.borderRadiusX1};
color:${({ theme }) => theme.color};
font-size:${({ theme }) => theme.smFontSize};
cursor:pointer;

&:hover{
    opacity:${({ theme }) => theme.hoverOpacity};
}

&:focus{
    outline: none;
}
`;

const ButtonsGroupWrapper = styled.div`
display: flex;
flex-direction: ${({ vertical }) => vertical ? 'column' : 'row'};

> button{
    margin: 0 5px;
};

> button:first-child{
    ${({ theme }) => theme.dir === 'ltr' ? css`margin-left: 0;` : css`margin-right: 0;`}
};

> button:last-child{
    ${({ theme }) => theme.dir === 'ltr' ? css`margin-right: 0;` : css`margin-left: 0;`}
};


`;

//#endregion


