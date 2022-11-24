function getQueryStrings() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = location.search.substring(1);
    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }

    return assoc;
};

$(document).ready(function () {
    var query = getQueryStrings();
    var request_id = query["rid"];
    if ((typeof (request_id) != "undefined") && request_id != null) {
        //call the ad server
        var ad_unique_id = query["aduid"];
        var user_id = query["uid"];
        var tracking_type = query["trt"];
        var url = "http://192.168.2.21/AdServer1.0/AdConvTracker/Track/" + ad_unique_id + "/" + user_id + "/" + request_id + "/" + tracking_type + "?callback=?";
        $.getJSON(url, function (data) {
            // can use 'data' in here...
        });
    }
});