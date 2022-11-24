import React from 'react';
import {
    Card, CardBody, Col, Badge, Table,
} from 'reactstrap';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import BasicTableData from './BasicTableData';

const { tableHeaderData, tableRowsData } = BasicTableData();

const PerformanceReport = ({ t }) => (
    <Col md={12} >
        <Card>
            <CardBody>
                <div className="card__title">
                    <i class="ri-table-2"></i>
                    <h5 className="bold-text">{t('default_dashboard.performance_report')}</h5>
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

PerformanceReport.propTypes = {
    t: PropTypes.func.isRequired,
};

export default withTranslation('common')(PerformanceReport);
