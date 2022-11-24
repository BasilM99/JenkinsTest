import React from 'react'
import styled, { css } from 'styled-components';


export const Container = styled.div``;

export const Section = styled.section``;

export const Group = styled.div`
display:flex;
flex-direction:${({ dir = 'row' }) => dir};
align-items: center;

>*{
    margin: 0 ${({ space = '5px' }) => space};
}
>*:first-child{
    ${({ theme }) => theme.dir === 'ltr' ? css`margin-left: 0;` : css`margin-right: 0;`}
}
>*:last-child{
    ${({ theme }) => theme.dir === 'ltr' ? css`margin-right: 0;` : css`margin-left: 0;`}
}
`;