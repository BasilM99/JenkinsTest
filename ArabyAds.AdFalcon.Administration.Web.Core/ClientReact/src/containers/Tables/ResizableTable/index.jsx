import React from 'react';
import PropTypes from 'prop-types';
import { withTranslation } from 'react-i18next';
import { Col, Container, Row } from 'reactstrap';
import ResizableDataTable from './components/ResizableReactTable';
import CreateTableData from '../CreateData';

const ResizableTable = ({ t }) => {
  const reactTableData = CreateTableData();

  return (
    <Container>
      <Row>
        <Col md={12}>
          <h3 className="page-title">{t('tables.resizable_table.title')}</h3>
          <h3 className="page-subhead subhead">Use this elements, if you want to show some hints or additional
            information
          </h3>
        </Col>
      </Row>
      <Row>
        <ResizableDataTable reactTableData={reactTableData} />
      </Row>
    </Container>
  );
};

ResizableTable.propTypes = {
  t: PropTypes.func.isRequired,
};

export default withTranslation('common')(ResizableTable);
