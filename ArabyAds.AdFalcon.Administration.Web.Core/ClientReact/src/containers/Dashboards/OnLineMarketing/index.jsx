import React from 'react';
import { Col, Container, Row } from 'reactstrap';
import { compose } from 'redux';
import { connect } from 'react-redux';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import Impressions from './components/Impressions';
import Clicks from './components/Clicks';
import Conversions from './components/Conversions';
import TotalSpend from './components/TotalSpend';
import PerformanceAnalytics from './components/PerformanceAnalytics';
import BudgetStatistic from './components/BudgetStatistic';
import { RTLProps } from '../../../shared/prop-types/ReducerProps';
import MatTable from './components/MatTable';

const OnLineMarketingDashboard = ({ t, rtl }) => (
  <Container className="dashboard">
    <Row>
      <Col md={12}>
        <h3 className="page-title">{t('online_marketing_dashboard.page_title')}</h3>
      </Col>
    </Row>
    <Row>
      <Impressions />
      <Clicks />
      <Conversions />
      <TotalSpend />
    </Row>
    <Row>
      <PerformanceAnalytics dir={rtl.direction} />
      <BudgetStatistic />
    </Row>
    <Row>
      <Col md={12}>
        <h3 className="page-title">Performance Report</h3>
        <h3 className="page-subhead subhead">Use this elements, if you want to show some hints or additional
          information
        </h3>
      </Col>
    </Row>
    <Row>
      <MatTable />
    </Row>
  </Container>
);

OnLineMarketingDashboard.propTypes = {
  t: PropTypes.func.isRequired,
  rtl: RTLProps.isRequired,
};

export default compose(withTranslation('common'), connect(state => ({
  rtl: state.rtl,
})))(OnLineMarketingDashboard);
