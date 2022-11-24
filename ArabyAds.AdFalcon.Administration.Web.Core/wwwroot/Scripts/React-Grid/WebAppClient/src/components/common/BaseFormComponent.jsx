import React, { Component } from 'react';
import BaseComponent from './BaseComponent';

class BaseFormComponent extends BaseComponent {
    validate(value, rules) {
        let isValid = true;
        let error = '';
		
		let tempval=''+value;
        if (rules.required) {
            isValid = value && tempval.trim() !== '';
            error = this.T("validation.required");
        }
        if (isValid && rules.match) {
            isValid = rules.match.regEx.test(value);
            error = rules.match.error;
        }
        if (isValid && rules.equals) {
            isValid = value === this.state.form[rules.equals.name].value;
            error = rules.equals.error;
        }
        if (isValid && rules.checked) {
            isValid = value;
            error = rules.checked.error;
        }
        return {
            error: error,
            valid: isValid
        };
    }

    isValidForm() {
	
        let isFormValid = true;
        let form = { ...this.state.form };
        for (let key in form) {
            const result = this.validate(form[key].value, form[key].validation);
			 form[key].valid = result.valid;
            form[key].error = result.error;
            isFormValid = isFormValid && form[key].valid;
            form[key].validate = true;
           
        }
        this.setState({ form: form });
        return isFormValid;
    }

    handelInputChange = (event) => {
		
        const target = event.target;
        const name = target.name;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        let form = { ...this.state.form };
        if (form[name].validation) {
            const checkValidity = this.validate(value, form[name].validation)
            form[name].validate = true;
            form[name].value = value;
            form[name].valid = checkValidity.valid;
            form[name].error = checkValidity.error;
        } else {
            form[name].value = value;
        }
        this.setState({ form: form });
    }
    GetFiledClass(shouldValidate, valid) {
        return shouldValidate && !valid ? "col-sm-6" : "col-sm-6";
    }
}
export default BaseFormComponent;