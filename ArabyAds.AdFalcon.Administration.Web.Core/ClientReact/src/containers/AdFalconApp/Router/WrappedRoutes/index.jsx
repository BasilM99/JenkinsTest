import React from 'react';
import { Route, Switch } from 'react-router-dom';
import Layout from '../../../Layout/index';
/* import Commerce from './Commerce';
import Finance from './Finance';
import OnLineMarketingDashboard from '../../../Dashboards/OnLineMarketing/index';
import AppDashboard from '../../../Dashboards/App/index';
import BookingDashboard from '../../../Dashboards/Booking/index';
import FitnessDashboard from '../../../Dashboards/Fitness/index';
import UI from './UI';
import Mail from '../../../Mail/index';
import Chat from '../../../Chat/index';
import Todo from '../../../Todo/index';
import Forms from './Forms';
import Tables from './Tables';
import Charts from './Charts';
import Maps from './Maps';
import Account from './Account';
import ECommerce from './ECommerce';
import DefaultPages from './DefaultPages';
import Documentation from './Documentation'; */
import DefaultDashboard from '../../../Dashboards/AdFalconDefaultDashboard/index';
import AdFalconAdvertisers from '../../../AdFalconAdvertisers/index'
import AdFalconCreateCampaign from '../../../AdFalconCampaign/index';
import deals from '../../../Deals/index'
import Ads from '../../../Ads/index'
import Groups from '../../../Groups/index'
import AudienceList from '../../../AudienceList/index'
import Campaigns from '../../../Campaigns/index'




export default () => (
  <div>
    <Layout />
    <div className="container__wrap">
     {/*  <Route path="/e_commerce_dashboard" component={Commerce} />
      <Route path="/online_marketing_dashboard" component={OnLineMarketingDashboard} /> */}
      <Route path="/:lang/dashboard"   component={DefaultDashboard} />
      <Route exact path="/:lang/campaign/AccountAdvertisers"   component={AdFalconAdvertisers} />
      <Route exact path="/:lang/campaign/AudienceList/:accountAdvertiserId"   component={AudienceList} />
      <Route exact path="/:lang/Campaign/Ads/:campaignId/:groupId"   component={Ads} />/
      <Route exact path="/:lang/Campaign/Groups/:campaignId"   component={Groups} />
      <Route path="/:lang/deals"   component={deals} />
      <Route exact path="/:lang/Campaign"   component={Campaigns} />
      <Route exact path="/:lang/campaign/Create/:accountAdvertiserId/:campaignId" component={AdFalconCreateCampaign} />
      <Route exact path="/:lang/campaign/Create/:accountAdvertiserId" component={AdFalconCreateCampaign} />
     
     
     {/*  <Route exact path="/app_dashboard" component={AppDashboard} />
      <Route path="/booking_dashboard" component={BookingDashboard} />
      <Route path="/finance_dashboard" component={Finance} />
      <Route path="/fitness_dashboard" component={FitnessDashboard} />
      <Route path="/ui" component={UI} />
      <Route path="/mail" component={Mail} />
      <Route path="/chat" component={Chat} />
      <Route path="/todo" component={Todo} />
      <Route path="/forms" component={Forms} />
      <Route path="/tables" component={Tables} />
      <Route path="/charts" component={Charts} />
      <Route path="/maps" component={Maps} />
      <Route path="/account" component={Account} />
      <Route path="/e-commerce" component={ECommerce} />
      <Route path="/default_pages" component={DefaultPages} />
      <Route path="/documentation" component={Documentation} /> */}
    </div>
  </div>
);
