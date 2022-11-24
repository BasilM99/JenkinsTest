import React from 'react';
import PropTypes from 'prop-types';
import NotificationSystem from 'rc-notification';
import { ThemeProps } from '../prop-types/ReducerProps';
import { useSelector, useDispatch } from 'react-redux';
import { UISelectors } from '../../Store/ReducerSlices/UI';

import useCurrentUser from '../../Hooks/useCurrentUser';

import useMyCurrentUI from '../../Hooks/useMyCurrentUI';

const showNotification = (theme, rtl, title, message) => {
  let notification = null;
  // eslint-disable-next-line no-return-assign
  NotificationSystem.newInstance({ style: { top: 65 } }, n => notification = n);
  const notificationInitialProps = {
    content: <BasicNotification
      color="danger"
      title={title}
      message={message}
      theme={theme}
    />,
    closable: true,
    duration: 5,
    style: { top: 0, left: 'calc(100vw - 100%)' },
    className: `right-up ${rtl.direction}-support`,
  };
  notification.notice(notificationInitialProps);
};

const showBasicNotificationForAdFalcon = ( title, message,color,position) => {
  let notification = null;
  // eslint-disable-next-line no-return-assign
  //const stateStore = useSelector(state => state);
  //const {UI} = useMyCurrentUI();
  const rtl = {direction:window.AdFalconUserLoggedInUserObject.Direction};
  const theme ={
    className: window.AdFalconUserLoggedInUserObject.themeClass,
  }
  
  NotificationSystem.newInstance({ style: { top: 65 } }, n => notification = n);
 if(position=='right-up')
 {
  const notificationInitialProps = {
    content: <BasicNotification
    color={color}
      title={title}
      message={message}
      theme={theme}
    />,
    closable: true,
    duration: 5,
    style: { top: 0, left: 'calc(100vw - 100%)' },
    className: `${position} ${rtl.direction}-support`,
  };
  notification.notice(notificationInitialProps);
}
else if(position=='left-up')
{  const notificationInitialProps = {
  content: <BasicNotification
    //color={color}
    title={title}
    message={message}
    theme={theme}
  />,
  closable: true,
  duration: 5,
  style: { top: 0, left: 0 },
  className: `${position} ${rtl.direction}-support`,
};
notification.notice(notificationInitialProps);
  //notification.notice(notificationInitialProps);
}
else
{  const notificationInitialProps = {
  content: <BasicNotification
    //color={color}
    title={title}
    message={message}
    theme={theme}
  />,
  closable: true,
  duration: 5,
  style: { top: 0, left: 0 },
  className: `${position} ${rtl.direction}-support`,
};
notification.notice(notificationInitialProps);

  //notification.notice(notificationInitialProps);
}
  
};

const BasicNotification = ({
  color, title, message, theme,
}) => (
  <div className={`notification notification--${color} notification--${theme.className}`}>
    <h5 className="notification__title bold-text">{title}</h5>
    <p className="notification__message">{message}</p>
  </div>
);

BasicNotification.propTypes = {
  theme: ThemeProps.isRequired,
  color: PropTypes.string,
  title: PropTypes.string,
  message: PropTypes.string.isRequired,
};

BasicNotification.defaultProps = {
  color: '',
  title: '',
};

const ImageNotification = ({
  img, title, message, theme,
}) => (
  <div className={`notification notification--image notification--${theme.className}`}>
    <div className="notification__image">
      <img src={img} alt="" />
    </div>
    <h5 className="notification__title bold-text">{title}</h5>
    <p className="notification__message">{message}</p>
  </div>
);

ImageNotification.propTypes = {
  theme: ThemeProps.isRequired,
  img: PropTypes.string.isRequired,
  title: PropTypes.string,
  message: PropTypes.string.isRequired,
};

ImageNotification.defaultProps = {
  title: '',
};

const FullWideNotification = ({ color, message }) => (
  <div className={`notification notification--full-wide notification--${color}`}>
    <p className="notification__message">{message}</p>
  </div>
);

FullWideNotification.propTypes = {
  color: PropTypes.string,
  message: PropTypes.string.isRequired,
};

FullWideNotification.defaultProps = {
  color: '',
};

export {
  BasicNotification,
  ImageNotification,
  FullWideNotification,
  showNotification,showBasicNotificationForAdFalcon
};
