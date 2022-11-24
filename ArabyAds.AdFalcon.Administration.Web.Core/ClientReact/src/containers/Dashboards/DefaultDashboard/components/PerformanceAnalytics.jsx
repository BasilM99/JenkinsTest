import React from 'react';
import {
  LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer,
} from 'recharts';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import { Card, Col, CardBody } from 'reactstrap';

const DashboardPerformanceAnalyticsData = [
  {
    name: 'June', Clicks: 2100, Impressions: 3000, Conversions: 1000,
  },
  {
    name: 'July', Clicks: 2800, Impressions: 4598, Conversions: 1710,
  },
  {
    name: 'August', Clicks: 4800, Impressions: 9800, Conversions: 2290,
  },
  {
    name: 'September', Clicks: 2780, Impressions: 7608, Conversions: 2000,
  },
  {
    name: 'October', Clicks: 3090, Impressions: 5800, Conversions: 2181,
  },
  {
    name: 'November', Clicks: 3000, Impressions: 7000, Conversions: 1500,
  },
  {
    name: 'December', Clicks: 3490, Impressions: 6500, Conversions: 2100,
  },
];

const PerformanceAnalytics = ({ t, dir, themeName }) => (
  <Col md={9}>
    <Card>
      <CardBody>
        <div className="card__title">
          <i class="ri-line-chart-line"></i>
          <h5 className="bold-text">{t('default_dashboard.performance_analytics')}</h5>
        </div>

        <ResponsiveContainer height={350}>
          <LineChart
            data={DashboardPerformanceAnalyticsData}
            margin={{
              top: 30, right: 0, left: -15, bottom: 0,
            }}
          >
            <XAxis dataKey="name" reversed={dir === 'rtl'} />
            <YAxis orientation={dir === 'rtl' ? 'right' : 'left'} />
            <CartesianGrid strokeDasharray="3 3" />
            <Tooltip />
            <Legend />
            <Line type="monotone" strokeWidth={3} dataKey="Impressions" stroke="#cd0000" activeDot={{ r: 8 }} />
            <Line type="monotone" strokeWidth={3} dataKey="Clicks" stroke="#ff9000" />
            <Line type="monotone" strokeWidth={3} dataKey="Conversions" stroke="#999" activeDot={{ r: 6 }} />
          </LineChart>
        </ResponsiveContainer>
      </CardBody>
    </Card>
  </Col>
);

PerformanceAnalytics.propTypes = {
  t: PropTypes.func.isRequired,
  dir: PropTypes.string.isRequired,
};

export default withTranslation('common')(PerformanceAnalytics);
