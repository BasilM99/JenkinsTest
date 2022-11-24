import { useEffect } from 'react';
import { withRouter } from 'react-router-dom';
import PropTypes from 'prop-types';

const ScrollToTop = ({ children, location }) => {
  useEffect(() => {
    if (location && location.pathname) {
      window.scrollTo(0, 0);
    }
  }, [location]);
  return children;
};

ScrollToTop.propTypes = {
  location: PropTypes.shape({
    pathname: PropTypes.string,
  }).isRequired,
  children: PropTypes.element.isRequired,
};

export default ScrollToTop;
