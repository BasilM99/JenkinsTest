// Auto-filling and validation script for leads generation forms
// made by AdFalcon in compatibility with Adobe Business Catalyst

// form parameters
var formLang = "en",
	emailRequired = true,
	emailVisible = true,
	emailDomain = "@adfalcon.com",

// form field names
	logo = document.getElementById("logo"),
	formHeading = document.getElementById("formHeading"),
//
	fullName = document.getElementById("FullName"),
	fullNameLabel = document.getElementById("FullNameLabel"),
	emailAddress = document.getElementById("EmailAddress"),
	emailAddressLabel = document.getElementById("EmailAddressLabel"),
	cellPhone = document.getElementById("CellPhone"),
	cellPhoneLabel = document.getElementById("CellPhoneLabel"),
//
	companyName = document.getElementById("CompanyName"),
	companyLabel = document.getElementById("CompanyLabel"),
//
	submitButton = document.getElementById("catwebformbutton"),

// regex patterns
	phonePattern = /^[0-9-\u0660-\u0669]+$/,
	emailPattern = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i,

// validation keys
	validName, validEmail, validPhone, validCompany;

// set visibility of the email field
if (emailVisible === false) {
	emailAddress.style.display = "none";
}

// set language vars
if (formLang == "en") {
// set client logo language
	logo.src = "images/logo-en.png";

// set form heading
	formHeading.innerHTML = "Please fill in the form below";

// set placeholders for text fields
	fullName.placeholder = "Full Name";
	emailAddress.placeholder = "Email Address";
	cellPhone.placeholder = "Cell Phone";
	companyName.placeholder = "Company";
	submitButton.value = "Submit information";

// set label error messages
	var nameErrorMsg = "Enter your full name",
		emailErrorMsg = "Enter a valid email address",
		phoneErrorMsg = "Enter a valid phone number",
		companyErrorMsg = "Enter your company name",
		validateErrorMsg = "Please make sure you have filled all the required fields";
} else {
// set client logo language
	logo.src = "images/logo-ar.png";

// set form heading
	formHeading.innerHTML = "يرجى تعبئة النموذج التالي وسنقوم بالإتصال بك لاحقا";

// set placeholders for text fields
	fullName.placeholder = "الإسم الكامل";
	emailAddress.placeholder = "العنوان الإلكتروني";
	cellPhone.placeholder = "رقم الهاتف";
	companyName.placeholder = "المدينة";
	submitButton.value = "أرسل المعلومات";

// set label error messages
	var nameErrorMsg = "أدخل الإسم الكامل",
		emailErrorMsg = "أدخل العنوان الإلكتروني",
		phoneErrorMsg = "أدخل رقم هاتف صحيح",
		companyErrorMsg = "أدخل إسم المدينة",
		validateErrorMsg = "يرجى التأكد من تعبئة جميع الحقول المطلوبة بشكل صحيح";
}

// set focus to the name field
fullName.focus();

// generate a random string for optional email address field
function randomString(length, chars) {
	var result = '';
	for (var i = length; i > 0; --i) result += chars[Math.round(Math.random() * (chars.length - 1))];
	return result;
}
var rString = randomString(10, '0123456789abcdefghijklmnopqrstuvwxyz');

// generate a full email address using the random string
function fillEmailField() {
	if (emailRequired === false) {
		emailAddress.value = rString + emailDomain;
	}
}

// check for input onBlur
// check full name
function checkName() {
	if (fullName.value.length < 2 || !isNaN(fullName.value)) {
		fullNameLabel.innerHTML = nameErrorMsg;
		fullName.className = "invalidInput";
		validName = false;
	} else {
		fullNameLabel.innerHTML = "";
		fullName.className = "validInput";
		validName = true;
	}
}

// check email address
function checkEmail() {
	if (emailAddress.value.match(emailPattern)) {
		emailAddressLabel.innerHTML = "";
		emailAddress.className = "validInput";
		validEmail = true;
	} else {
		emailAddressLabel.innerHTML = emailErrorMsg;
		emailAddress.className = "invalidInput";
		validEmail = false;
	}
	if (emailRequired === false) {
		emailAddressLabel.innerHTML = "";
		emailAddress.className = "validInput";
		validEmail = true;
	}
}

// check cell phone
function checkPhone() {
	if (cellPhone.value.match(phonePattern) && cellPhone.value.length > 8) {
		cellPhoneLabel.innerHTML = "";
		cellPhone.className = "validInput";
		validPhone = true;
	} else {
		cellPhoneLabel.innerHTML = phoneErrorMsg;
		cellPhone.className = "invalidInput";
		validPhone = false;
	}
}

// check company name
function checkCompany() {
	if (companyName.value.length < 2 || !isNaN(companyName.value)) {
		companyLabel.innerHTML = companyErrorMsg;
		companyName.className = "invalidInput";
		validCompany = false;
	} else {
		companyLabel.innerHTML = "";
		companyName.className = "validInput";
		validCompany = true;
	}
}

// check form validation
function validate() {
	checkName(); checkEmail(); checkPhone(); checkCompany();

	if (emailRequired === false) {
		validEmail = true;
	}
	if (validName !== true || validEmail !== true || validPhone !== true || validCompany !== true) {
		alert(validateErrorMsg);
		return false;
	} else {
		fillEmailField();
		return true;
	}
}
