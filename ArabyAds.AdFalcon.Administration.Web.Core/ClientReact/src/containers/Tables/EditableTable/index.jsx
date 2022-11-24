import React from 'react';
import PropTypes from 'prop-types';
import { withTranslation } from 'react-i18next';
import { Col, Container, Row } from 'reactstrap';
import EditableReactTable from './components/EditableReactTable';
import CreateTableData from '../CreateData';

const EditableTable = ({ t }) => {
  const reactTableData = CreateTableData();
  return (
    <Container>
      <Row>
        <Col md={12}>
          <h3 className="page-title">{t('tables.editable_table.title')}</h3>
          <h3 className="page-subhead subhead">Use this elements, if you want to show some hints or additional
            information
          </h3>
        </Col>
      </Row>
      <Row>
        <EditableReactTable reactTableData={reactTableData} />
      </Row>
    </Container>
  );
};

EditableTable.propTypes = {
  t: PropTypes.func.isRequired,
};

export default withTranslation('common')(EditableTable);
