import React from 'react';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import Panel from '../../../../shared/components/Panel';
import BuyBTC from './BuyBTC';
import SellBTC from './SellBTC';

const PlaceOrder = ({ t }) => (
  <Panel
    xl={6}
    lg={12}
    title={t('finance_dashboard.trade_history')}
    subhead="You can buy or sell a financecurrency"
  >
    <div className="dashboard__place-order">
      <BuyBTC onSubmit />
      <SellBTC onSubmit />
    </div>
  </Panel>
);

PlaceOrder.propTypes = {
  t: PropTypes.func.isRequired,
};

export default withTranslation('common')(PlaceOrder);
