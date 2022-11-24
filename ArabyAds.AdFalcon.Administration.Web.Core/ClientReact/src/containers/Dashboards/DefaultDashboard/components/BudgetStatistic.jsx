import React from 'react';
import { Pie, PieChart, ResponsiveContainer } from 'recharts';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import { Card, CardBody, Col, Progress } from 'reactstrap';
import { PanelTitle } from '../../../../shared/components/Panel';

const DashboardBudgetStatisticsData1 = [{ value: 78, fill: '#cd0000' },
{ value: 23, fill: '#999' }];

const DashboardBudgetStatisticsData2 = [{ value: 44, fill: '#ff9000' },
{ value: 75, fill: '#999' }];

const SocialScore = ({ children, progress }) => (
  <div className="dashboard__social-stat-item">
    <div className="dashboard__social-stat-title">
      {children}
    </div>
    <div className="dashboard__social-stat-progress">
      <div className="progress-wrap progress-wrap--small progress-wrap--red-gradient progress-wrap--rounded">
        <p className="progress__label">{progress}%</p>
        <Progress value={progress} />
      </div>
    </div>
  </div>
);

SocialScore.propTypes = {
  progress: PropTypes.number.isRequired,
  children: PropTypes.string.isRequired,
};

const BudgetStatistic = ({ t }) => (
  <Col md={3}>
    <Card>
      <CardBody>
        <div className="card__title">
          <i class="ri-bank-line"></i>
          <h5 className="bold-text">{t('default_dashboard.budget_statistic')}</h5>
        </div>
        <div className="dashboard__weekly-stat">
          <div className="dashboard__weekly-stat-chart">
            <div className="dashboard__weekly-stat-chart-item">
              <div className="dashboard__weekly-stat-chart-pie">
                <ResponsiveContainer>
                  <PieChart className="dashboard__weekly-stat-chart-pie-wrapper">
                    <Pie
                      data={DashboardBudgetStatisticsData1}
                      dataKey="value"
                      cx={50}
                      cy={50}
                      innerRadius={50}
                      outerRadius={55}
                    />
                  </PieChart>
                </ResponsiveContainer>
                <p className="dashboard__weekly-stat-label" style={{ color: '#eee' }}>78%</p>
              </div>
              <div className="dashboard__weekly-stat-info">
                <p>Customers satisfaction rate</p>
              </div>
            </div>
            <div className="dashboard__weekly-stat-chart-item">
              <div className="dashboard__weekly-stat-chart-pie">
                <ResponsiveContainer>
                  <PieChart className="dashboard__weekly-stat-chart-pie-wrapper">
                    <Pie
                      data={DashboardBudgetStatisticsData2}
                      dataKey="value"
                      cx={50}
                      cy={50}
                      innerRadius={50}
                      outerRadius={55}
                    />
                  </PieChart>
                </ResponsiveContainer>
                <p className="dashboard__weekly-stat-label" style={{ color: '#eee' }}>44%</p>
              </div>
              <div className="dashboard__weekly-stat-info">
                <p>Negative <br />feedback</p>
              </div>
            </div>
          </div>
          <hr />
          <PanelTitle title='Campaign Spend' />
          <div className="dashboard__social-stat">
            <SocialScore progress={75}>
              Etisalat
        </SocialScore>
            <SocialScore progress={65}>
              Range Rover
        </SocialScore>
            <SocialScore progress={92}>
              Tripadvisor
        </SocialScore>
            <SocialScore progress={61}>
              Coca Cola
        </SocialScore>
          </div>
        </div>
      </CardBody>
    </Card>
  </Col>
);

BudgetStatistic.propTypes = {
  t: PropTypes.func.isRequired,
};

export default withTranslation('common')(BudgetStatistic);
