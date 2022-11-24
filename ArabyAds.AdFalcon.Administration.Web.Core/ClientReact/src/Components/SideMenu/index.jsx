import React, { useState } from 'react';
import styled, { css } from 'styled-components';

import { IcHome } from '../../Elements/Icons';

const SideMenu = ({ menu }) => {

    const [menuData, setMenuData] = useState({ ...menu })

    const renderMenu = (menu, level = 1) => {

        if (!menu || menu.length <= 0)
            return;

        return menu.map(item => {

            return <MenuItem key={item.id}>{
                <>
                    <Anchor onClick={(e) => {
                        if (item.items) {
                            item.active = !item.active;
                            setMenuData({ ...menuData });
                        }
                        if (item.action) {
                            item.action(e, { ...item });
                        }
                    }}>
                        <Icon><IcHome size={20} /></Icon>
                        <Label>{item.label}</Label>
                    </Anchor>
                    {
                        item.items && <Menu level={level + 1} active={item.active} showBranchLine={item.showBranchesLine}>
                            {
                                renderMenu(item.items, level + 1)
                            }
                        </Menu>
                    }
                </>
            }
            </MenuItem >

        });

    };

    return <>
        <Container>
            <Menu level={1} active={menuData.active} showBranchLine={menuData.showBranchesLine}>
                {
                    renderMenu(menuData.items, 1)
                }
            </Menu>
        </Container>
    </>;
};

export default SideMenu;

//#region :: Styles

const Container = styled.div`
`;

const Menu = styled.div`
${({ theme, level, margin = level === 1 ? 0 : '30px' }) => theme.dir === 'rtl' && css`margin-right:${margin};` || css`margin-left:${margin};`};

${({ active }) => {
        if (!active)
            return css`
            margin-top:0;
            height:0;
            overflow:hidden;
`;
    }};

margin-top:0px;
position: relative;


${MenuItem}:before{
    ${({ showBranchLine }) => {
        return showBranchLine && css`
            position: absolute;
            top: 0;
            bottom: 0;
            height: calc(100% - 20px);
            ${({ theme }) => theme.dir === 'ltr' ?
                (css` left: -1.25em;`) :
                (css` right: -1.25em;`)}
            display: block;
            width: 0;
            ${({ theme }) => theme.dir === 'ltr' ?
                (css`border-left: 1px solid  ${({ theme }) => theme.accent};`) :
                (css`border-right: 1px solid  ${({ theme }) => theme.accent};`)}
            content: '';
`;
    }}
}

${MenuItem}:before{
    display:none;
}
 
${MenuItem}>${Anchor}:first-child:after{
    ${({ showBranchLine }) => {
        return showBranchLine && css`
    position: absolute;
    top: -.3em;
    ${({ theme }) => theme.dir === 'ltr' ?
                (css` left: -1.25em;`) :
                (css` right: -1.25em;`)};

    display: block;
    height: 1em;
    width: 0.9em;
    border-bottom: 1px solid ${({ theme }) => theme.accent};
    ${({ theme }) => theme.dir === 'ltr' ?
                (css`border-left: 1px solid  ${({ theme }) => theme.accent};`) :
                (css`border-right: 1px solid  ${({ theme }) => theme.accent};`)};
    border-radius: 0;
    content: '';
  `;
    }}
  
}

`;

const MenuItem = styled.div`
   position:relative;
   margin-top:10px;
   margin-bottom:10px;

  
`;

const Anchor = styled.a`
    display: flex;
    flex-grow:0;
    color:${({ theme }) => theme.color};
    flex-direction:row;
    align-items: center;
    position: relative;
    cursor:pointer;
    :hover&{
        opacity:.5;
        color:${({ theme }) => theme.color};
        text-decoration:none;
    }
    &>*{cursor:pointer;}
`;


const Icon = styled.i`
    display:block;
`;

const Label = styled.label`
    display:block;
    margin: 0 10px;
    font-size:14px;
`;
//#endregion