import React from 'react';
import { Col, Container, Row } from 'reactstrap';
import { withTranslation } from 'react-i18next';
import PropTypes from 'prop-types';
import DnDTable from './components/DnDReactTable';
import CreateTableData from '../CreateData';

const DragAndDropTable = ({ t }) => {
  const reactTableData = CreateTableData();
  return (
    <Container>
      <Row>
        <Col md={12}>
          <h3 className="page-title">{t('tables.dnd_table.title')}</h3>
          <h3 className="page-subhead subhead">Use this elements, if you want to show some hints or additional
            information
          </h3>
        </Col>
      </Row>
      <Row>
        <DnDTable reactTableData={reactTableData} />
      </Row>
    </Container>
  );
};

DragAndDropTable.propTypes = {
  t: PropTypes.func.isRequired,
};

export default withTranslation('common')(DragAndDropTable);
