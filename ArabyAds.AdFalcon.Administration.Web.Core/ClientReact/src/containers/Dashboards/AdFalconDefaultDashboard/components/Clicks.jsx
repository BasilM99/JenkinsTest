/* eslint-disable react/no-array-index-key */
import React, { Component } from 'react';
import { Card, CardBody, Col } from 'reactstrap';
import {
  BarChart, Bar, Cell, ResponsiveContainer,
} from 'recharts';
import TrendingUpIcon from 'mdi-react/TrendingUpIcon';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';

const data = [
  { name: 'Page A', amt: 2400 },
  { name: 'Page B', amt: 2210 },
  { name: 'Page C', amt: 2290 },
  { name: 'Page D', amt: 2000 },
  { name: 'Page E', amt: 2181 },
  { name: 'Page F', amt: 2500 },
  { name: 'Page G', amt: 2100 },
  { name: 'Page H', amt: 2290 },
  { name: 'Page I', amt: 2000 },
  { name: 'Page J', amt: 2181 },
];

class Clicks extends Component {
  static propTypes = {
    t: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);
    this.state = {
      activeIndex: 0,
    };
  }

  handleClick = (item) => {
    const index = data.indexOf(item.payload);
    this.setState({
      activeIndex: index,
    });
  };

  render() {
    const { activeIndex } = this.state;
    const activeItem = data[activeIndex];
    const { t } = this.props;

    return (
      <Col md={12} xl={3} lg={6} xs={12}>
        <Card>
          <CardBody>
            <div className="card__title">
              <i class="ri-drag-drop-line"></i> 
              <h5 className="bold-text">{t('online_marketing_dashboard.total_page_views')}</h5>
            </div>
            <div className="dashboard__total">
              <TrendingUpIcon className="dashboard__trend-icon" />
              <p className="dashboard__total-stat">
                {activeItem.amt}
              </p>
              <div className="dashboard__chart-container">
                <ResponsiveContainer height={70}>
                  <BarChart data={data}>
                    <Bar dataKey="amt" onClick={this.handleClick}>
                      {
                        data.map((entry, index) => (
                          <Cell
                            cursor="pointer"
                            fill={index === activeIndex ? '#cd0000' : '#999'}
                            key={`cell-${index}`}
                          />
                        ))
                      }
                    </Bar>
                  </BarChart>
                </ResponsiveContainer>
              </div>
            </div>
          </CardBody>
        </Card>
      </Col>
    );
  }
}

export default withTranslation('common')(Clicks);
