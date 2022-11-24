import React from 'react'
import styled, { css } from 'styled-components';
import IcSeach from '../Assets/img/search.svg';

export const TexBox = ({ placeholder, searchBox }) => {

    return <>
        <TextBoxWrapper searchBox={searchBox}>
            <Textbox type={'text'} placeholder={placeholder} /></TextBoxWrapper>
    </>;

};




//#region :: Style

const TextBoxWrapper = styled.div`
&:before{
    ${({ searchBox }) => searchBox && css`
     content:'';
     position:absolute;
     z-index: 1;
     display:block;
     background-image:url(${IcSeach});
    height: 20px;
    width: 20px;
   
    ${({ theme }) => theme.dir === 'ltr' ? css`left: 10px;` : css`right: 10px; transform: scaleX(-1);`};
    top: 6px;
    background-repeat: no-repeat;
    background-size: 100%;
    `}
 };
 position:relative;

 &>input{
        ${({ theme, searchBox }) => searchBox && (theme.dir === 'ltr' ? css`padding-left: 40px;` : css`padding-right: 40px;`)};
    }
`;

const Textbox = styled.input`
position:relative;
border-radius:${({ theme }) => theme.borderRadius};
padding:5px 10px;
background-color:${({ theme }) => theme.infoSecondary};
outline:none;
border:none;
color:${({ theme }) => theme.color};
font-size:${({ theme }) => theme.smFontSize};
&::placeholder{
color:${({ theme }) => theme.mutedColor};
 };


`;

//#endregion