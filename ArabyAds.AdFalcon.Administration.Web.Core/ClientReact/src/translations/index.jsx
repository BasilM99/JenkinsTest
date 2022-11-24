import resources from './resources';
import Constants from '../Config/Constants';
import Locals from '../Localization/Locales';

export const config = {
  lng: window.AdFalconUserLoggedInUserObject.CurrentLanguage, load: 'currentOnly',
  ns:["Titles","Common","Campaign","Global"],
  partialBundledLanguages :true,saveMissing:true,
  backend:{ loadPath: Constants.backend.GetResourcesURL,





  allowMultiLoading: false},
  react: {
    wait: true,
    useSuspense: true,
 },

defaultNS:'Titles',
resources:  resources,

 
  supportedLngs: ["en", "ar"],
  // Allows "en-US" and "en-UK" to be implcitly supported when "en"
  // is supported
  nonExplicitSupportedLngs: true,
  fallbackLng: "en",
  interpolation: {
    escapeValue: false,
  },
  debug:true,
};

export { resources };
