import React, { useEffect } from 'react';
import CheckIcon from 'mdi-react/CheckIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import * as PropTypes from 'prop-types';
import classNames from 'classnames';

export const RadioButtonField = ({
  defaultChecked,
  onChange,
  radioValue,
  className,
  disabled,
  label,
  name,
  value, innerRef
}) => {
  useEffect(() => {
    if (defaultChecked) {
      onChange(radioValue);
    }
  }, [defaultChecked, onChange, radioValue]);
  const RadioButtonClass = classNames({
    'radio-btn': true,
    disabled,
  });
  const handleChange = () => {
    onChange(radioValue);
  };
  return (
    // eslint-disable-next-line jsx-a11y/label-has-for
    <label
      className={`${RadioButtonClass}${className ? ` radio-btn--${className}` : ''}`}
    >
      <input
        className="radio-btn__radio"
        name={name}
        type="radio" ref={innerRef}
        onChange={handleChange}
        checked={value === radioValue}
        disabled={disabled}
      />
      <span className="radio-btn__radio-custom" />
      {className === 'button'
        ? (
          <span className="radio-btn__label-svg">
            <CheckIcon className="radio-btn__label-check" />
            <CloseIcon className="radio-btn__label-uncheck" />
          </span>
        ) : ''}
      <span className="radio-btn__label">{label}</span>
    </label>
  );
};

RadioButtonField.propTypes = {
  onChange: PropTypes.func,
  name: PropTypes.string.isRequired,
  value: PropTypes.string.isRequired,
  label: PropTypes.oneOfType([PropTypes.element, PropTypes.string]),
  defaultChecked: PropTypes.bool,
  disabled: PropTypes.bool,
  radioValue: PropTypes.string,
  className: PropTypes.string,
};

RadioButtonField.defaultProps = {
  label: '',
  defaultChecked: false,
  disabled: false,
  radioValue: '',
  className: '',
};

const renderRadioButtonField = ({
  input, label, defaultChecked, disabled, className, radioValue,
}) => (
  <RadioButtonField
    {...input}
    label={label}
    defaultChecked={defaultChecked}
    disabled={disabled}
    radioValue={radioValue}
    className={className}
  />
);

renderRadioButtonField.propTypes = {
  input: PropTypes.shape({
    onChange: PropTypes.func,
    name: PropTypes.string,
  }),
  label: PropTypes.oneOfType([PropTypes.element, PropTypes.string]),
  defaultChecked: PropTypes.bool,
  disabled: PropTypes.bool,
  radioValue: PropTypes.string,
  className: PropTypes.string,
};

renderRadioButtonField.defaultProps = {
  label: '',
  defaultChecked: false,
  disabled: false,
  radioValue: '',
  className: '',
};

export default renderRadioButtonField;
