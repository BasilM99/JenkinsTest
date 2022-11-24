import React from 'react';
import { PieChart, Pie } from 'recharts';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import Panel from '../../../../shared/components/Panel';

const data01 = [{ value: 50, fill: '#cd0000' },
  { value: 50, fill: '#eeeeee' }];

const BudgetStatistic = ({ t }) => (
  <Panel md={4} lg={4} xl={4} title={t('online_marketing_dashboard.budget_statistic')}>
    <div className="dashboard__stat dashboard__stat--budget">
      <div className="dashboard__stat-main">
        <p className="dashboard__stat-main-title">Total Budget</p>
        <p className="dashboard__stat-main-number">$12,321</p>
        <hr />
      </div>
      <div className="dashboard__stat-chart">
        <PieChart height={120} width={120}>
          <Pie data={data01} dataKey="value" cx={55} cy={55} innerRadius={50} outerRadius={60} />
        </PieChart>
        <p className="dashboard__stat-label">$</p>
      </div>
      <div className="dashboard__stat-data">
        <div>
          <p className="dashboard__stat-data-number">$4,937</p>
          <p style={{ color: '#cd0000' }}>Completed</p>
        </div>
        <div>
          <p className="dashboard__stat-data-number">$7,566</p>
          <p style={{ color: '#999' }}>Remaining</p>
        </div>
      </div>
    </div>
  </Panel>
);

BudgetStatistic.propTypes = {
  t: PropTypes.func.isRequired,
};

export default withTranslation('common')(BudgetStatistic);
