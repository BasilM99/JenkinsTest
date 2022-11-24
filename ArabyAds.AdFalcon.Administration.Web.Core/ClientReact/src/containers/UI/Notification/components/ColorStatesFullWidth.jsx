import React from 'react';
import {
  Button, ButtonToolbar, Card, CardBody, Col,
} from 'reactstrap';
import PropTypes from 'prop-types';
import { withTranslation } from 'react-i18next';
import { FullWideNotification } from '../../../../shared/components/Notification';

const show = (showNotification, color) => showNotification({
  notification(theme) {
    return (
      <FullWideNotification
        color={color}
        message="Learning day desirous informed expenses material returned six the.
              She enabled invited exposed him another."
        theme={theme}
      />
    );
  },
  position: 'full',
});

const ColorStatesFullWidth = ({ t, showNotification }) => (
  <Col md={12} lg={6} xs={12}>
    <Card>
      <CardBody>
        <div className="card__title">
          <h5 className="bold-text">{t('ui_elements.notifications.color_states_full_width')}</h5>
          <h5 className="subhead">Use FullWideNotification with color: <span className="red-text">primary</span>,
            <span className="red-text"> success</span>, <span className="red-text">warning</span>,
            <span className="red-text"> danger</span>
          </h5>
        </div>
        <ButtonToolbar>
          <Button color="primary" onClick={() => show(showNotification, 'primary')}>Primary</Button>
          <Button color="success" onClick={() => show(showNotification, 'success')}>Success</Button>
          <Button color="warning" onClick={() => show(showNotification, 'warning')}>Warning</Button>
          <Button color="danger" onClick={() => show(showNotification, 'danger')}>Danger</Button>
        </ButtonToolbar>
      </CardBody>
    </Card>
  </Col>
);

ColorStatesFullWidth.propTypes = {
  showNotification: PropTypes.func.isRequired,
  t: PropTypes.func.isRequired,
};

export default withTranslation('common')(ColorStatesFullWidth);
