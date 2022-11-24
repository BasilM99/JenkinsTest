import React, { Fragment } from 'react';
import { IntlProvider, FormattedMessage, useIntl } from 'react-intl';
import LocalesResources from './LocaleResources'
import Locales from './Locales'

//Provider:
const LocaleProvider = ({ children, locale = Locales.ENGLISH }) => (
    <IntlProvider
        locale={locale}
        textComponent={Fragment}
        messages={LocalesResources[locale]}
        onError={(error) => {
        }}>
        { children}
    </IntlProvider >

);
export default LocaleProvider;

//Translate:
export const Format = (id, value = {}) => <FormattedMessage tagName="" id={id} values={{ ...value }} />;





