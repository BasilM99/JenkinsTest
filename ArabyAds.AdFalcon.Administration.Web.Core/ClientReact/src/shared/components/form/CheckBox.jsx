import React, { useEffect } from 'react';
import CheckIcon from 'mdi-react/CheckIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import PropTypes from 'prop-types';
import classNames from 'classnames';

export const CheckBoxField = ({
  onChange,
  defaultChecked,
  disabled,
  className,
  name,
  value,
  label,
  color,innerRef
}) => {
  useEffect(() => {
    onChange(defaultChecked);
  }, [onChange, defaultChecked]);

  const CheckboxClass = classNames({
    'checkbox-btn': true,
    disabled,
  });
  return (
    <label
      className={`${CheckboxClass} ${className ? ` checkbox-btn--${className}` : ''}`}
      htmlFor={name}
    >
      <input
        className="checkbox-btn__checkbox"
        type="checkbox"
        id={name}
        name={name}
      
        onChange={onChange}
        checked={value} ref={innerRef}
        disabled={disabled}
      />
      <span
        className="checkbox-btn__checkbox-custom"
        style={color ? { background: color, borderColor: color } : {}}
      >
        <CheckIcon />
      </span>
      {className === 'button'
        ? (
          <span className="checkbox-btn__label-svg">
            <CheckIcon className="checkbox-btn__label-check" />
            <CloseIcon className="checkbox-btn__label-uncheck" />
          </span>
        ) : ''}
      <span className="checkbox-btn__label">
        {label}
      </span>
    </label>
  );
};

CheckBoxField.propTypes = {
  onChange: PropTypes.func.isRequired,
  name: PropTypes.string.isRequired,
  value: PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.bool,
  ]).isRequired,
  label: PropTypes.string,
  defaultChecked: PropTypes.bool,
  disabled: PropTypes.bool,
  className: PropTypes.string,
  color: PropTypes.string,
};

CheckBoxField.defaultProps = {
  label: '',
  defaultChecked: false,
  disabled: false,
  className: '',
  color: '',
};

const renderCheckBoxField = ({
  input, label, defaultChecked, disabled, className, color,
}) => (
  <CheckBoxField
    {...input}
    label={label}
    defaultChecked={defaultChecked}
    disabled={disabled}
    className={className}
    color={color}
  />
);

renderCheckBoxField.propTypes = {
  input: PropTypes.shape({
    onChange: PropTypes.func,
    name: PropTypes.string,
    value: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.bool,
    ]),
  }).isRequired,
  label: PropTypes.string,
  defaultChecked: PropTypes.bool,
  disabled: PropTypes.bool,
  className: PropTypes.string,
  color: PropTypes.string,
};

renderCheckBoxField.defaultProps = {
  label: '',
  defaultChecked: false,
  disabled: false,
  className: '',
  color: '',
};

export default renderCheckBoxField;