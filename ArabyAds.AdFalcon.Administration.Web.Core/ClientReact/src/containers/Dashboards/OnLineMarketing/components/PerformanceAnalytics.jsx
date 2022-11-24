import React from 'react';
import { connect } from 'react-redux';
import {
  AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer,
} from 'recharts';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import Panel from '../../../../shared/components/Panel';

import getTooltipStyles from '../../../../shared/helpers';

const data = [{ name: 'Mon', a: 590, b: 60 },
  { name: 'Tue', a: 868, b: 106 },
  { name: 'Wed', a: 1397, b: 550 },
  { name: 'Thu', a: 1080, b: 360 },
  { name: 'Fri', a: 1420, b: 550 },
  { name: 'Sat', a: 1350, b: 450 },
  { name: 'Sun', a: 1300, b: 570 }];

const PerformanceAnalytics = ({ t, dir, themeName }) => (
  <Panel md={8} lg={8} xl={8} title={t('online_marketing_dashboard.performance_analytics')}>
    <div dir="ltr">
      <ResponsiveContainer height={350} className="dashboard__area">
        <AreaChart data={data} margin={{ top: 20, left: -15, bottom: 20 }}>
          <XAxis dataKey="name" tickLine={false} reversed={dir === 'rtl'} />
          <YAxis tickLine={false} orientation={dir === 'rtl' ? 'right' : 'left'} />
          <Tooltip {...getTooltipStyles(themeName, 'defaultItems')} />
          <Legend />
          <CartesianGrid />
          <Area name="Impressions" type="monotone" dataKey="a" fill="#cd0000" stroke="#cd0000" fillOpacity={0.5} />
          <Area name="Clicks" type="monotone" dataKey="b" fill="#999" stroke="#eee" fillOpacity={1} />
        </AreaChart>
      </ResponsiveContainer>
    </div>
  </Panel>
);

PerformanceAnalytics.propTypes = {
  t: PropTypes.func.isRequired,
  dir: PropTypes.string.isRequired,
  themeName: PropTypes.string.isRequired,
};

export default connect(state => ({ themeName: state.theme.className }))(withTranslation('common')(PerformanceAnalytics));
