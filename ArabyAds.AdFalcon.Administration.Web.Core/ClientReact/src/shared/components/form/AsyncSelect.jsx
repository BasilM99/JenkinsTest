import React from 'react';
import PropTypes from 'prop-types';
import AsyncReactSelect from 'react-select/async';

export const AsyncSelectField = ({
  input, handleChange, value, name, placeholder, key, getloadOptions,handleInputChange
}) => {
  

  return (

    <AsyncReactSelect
										className="react-select"
                    classNamePrefix="react-select" 
                    key={key}
									 name={input.name}  value={input.value}
										cacheOptions
										defaultOptions
										autoload
										getOptionLabel={e => e.label}
										getOptionValue={e => e.value}
										loadOptions={getloadOptions}
										onInputChange={handleInputChange}
                   // onChange={input.onChange}
                   onChange= {(e) => {
                      handleChange(e);
                      input.onChange(e)
                  }}
										placeholder={placeholder}	  onBlurResetsInput={false}
                    onCloseResetsInput={false}
										/>

  );
};



AsyncSelectField.defaultProps = {
  placeholder: '',

};

const renderAsyncSelectField = ({
  input, meta,  placeholder, className,  handleChange, key, getloadOptions,handleInputChange

}) => (
  <div className={`form__form-group-input-wrap ${className}`}>
    <AsyncSelectField
      {...input}  
      handleChange={handleChange}
       key={key}
        getloadOptions={getloadOptions}
        handleInputChange={handleInputChange}
      placeholder={placeholder}
    />
    {meta.touched && meta.error && <span className="form__form-group-error">{meta.error}</span>}
  </div>
);



renderAsyncSelectField.defaultProps = {
  meta: null,
  options: [],
  placeholder: '',
  className: '',
};

export default renderAsyncSelectField;
