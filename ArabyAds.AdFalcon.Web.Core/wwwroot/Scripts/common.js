
$(document).ready(function () {
    if ($("#formApply").length > 0) {
        $("#formApply").validate({
            errorPlacement: function (error, element) {
                error.prependTo(element.parent("div"));
            },
            errorElement: "div",
            errorClass: "errorValidation",
            highlight: function (element, errorClass) {
                $(element).addClass("errorField");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass("errorField").addClass(validClass);
            },
            rules:
        {
            firstName:
            {
                required: true
            },
            secondName:
            {
                required: true
            },
            email:
            {
                email: true,
                required: true
            },
            message:
            {
                required: true
            },
            name:
            {
                required: true
            },
            subject:
            {
                required: true
            }
        },
            messages:
        {
            firstName:
            {
                required: ""
            },
            secondName:
            {
                required: ""
            },
            email:
            {
                email: "",
                required: ""
            },
            message:
            {
                required: ""
            },
            name:
            {
                required: ""
            },
            subject:
            {
                required: ""
            }
        }
        });

    }

    initilize();
});

var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-25905897-1']);
_gaq.push(['_setDomainName', 'adfalcon.com']);
_gaq.push(['_trackPageLoadTime']);
_gaq.push(['_trackPageview']);

(function () {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();


function contains(str, value) {
    return str.indexOf(value) > -1;
}
function getLang() {
    var url = window.location.href.toLowerCase();
    if (contains(url, '/en/')) {
        return "en";
    }
    if (contains(url, '/ar/')) {
        return "ar";
    }
};
function getRandom() {
    //return Math.floor(Math.random() * 9999999);
    var date = new Date();
    return date.getTime();
};

function initilize() {
    //
    var action = "PublicInfo";
    if (window.location.protocol == "https:") {
        action = "PublicInfoHttps";
    }
    jQuery.get("../" + getLang() + '/user/' + action + '/?r=' + getRandom(), function (data) {
        jQuery('#loginInfoContainer').html(data); //replaceWith(data);
    });
};
function logout() {
    var action = "logout";
    if (window.location.protocol == "https:") {
        action = "LogoutHttps";
    }
    jQuery.get("../" + getLang() + '/user/' + action + '?r=' + getRandom(), function (data) {
        jQuery('#loginInfoContainer').html(data);
    });
};

function postRequest() {

    if ($("#formApply").validate().form()) {
        var url = "../misc/AgencyRequest";

        if (window.location.protocol == "https:") {
            url = "../misc/AgencyHttpsRequest";
        }
        $.post(url, jQuery("#formApply").serialize(), function (data) {
            if (data.status == "success") {
                successSendRequest();
            }
        });
        return true;
    }
    else {
        return false;
    }
}
function postContactUsRequest() {

    if (jQuery("#formApply").validate().form()) {
        var url = "../misc/ContactUsRequest";

        if (window.location.protocol == "https:") {
            url = "../misc/ContactUsHttpsRequest";
        }

        jQuery.post(url, jQuery("#formApply").serialize(), function (data) {
            if (data.status == "success") {
                successSendRequest();
            }
        });
        return true;
    }
    return false;
}
function successSendRequest() {
    $("#formApply").find(":input[type='text'],textarea").val("");
    $("#formContainer").hide();
    $("#successMessage").show();
}