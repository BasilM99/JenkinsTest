import React from 'react';
import {
  Card, CardBody, Col, Badge, Table,
} from 'reactstrap';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import BasicTableData from './BasicTableData';

const { tableHeaderData, tableRowsData } = BasicTableData();

const BasicTable = ({ t }) => (
  <Col md={12} lg={12} xl={6}>
    <Card>
      <CardBody>
        <div className="card__title">
          <h5 className="bold-text">{t('tables.basic_tables.basic_table')}</h5>
          <h5 className="subhead">Use default table</h5>
        </div>
        <Table responsive hover>
          <thead>
            <tr>
              {tableHeaderData.map(item => (
                <th key={item.id}>{item.title}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {tableRowsData.map(item => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.first}</td>
                <td>{item.last}</td>
                <td>{item.username}</td>
                <td><Badge color={item.status}>{item.badge}</Badge></td>
              </tr>
            ))}
          </tbody>
        </Table>
      </CardBody>
    </Card>
  </Col>
);

BasicTable.propTypes = {
  t: PropTypes.func.isRequired,
};

export default withTranslation('common')(BasicTable);
