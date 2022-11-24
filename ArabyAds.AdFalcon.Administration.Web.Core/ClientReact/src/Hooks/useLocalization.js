import { AppResources } from '../Localization/Resources';

import { useTranslation } from "react-i18next";
const useLocalization = () => {
    const { t, i18n } = useTranslation();

    const T = (id, options = {}) => {
        return t(id,options);
    };

    return {
        T,
        Resources: {
            AppResources
        }
    };
};

export default useLocalization;