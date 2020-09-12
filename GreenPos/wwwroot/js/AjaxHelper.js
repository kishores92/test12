var AjaxC = function () {
    var getJsonAsync = function (url) {
        var returnObject = $.ajax({
            type: "GET",
            url: url,
            //dataType: "json",
            async: true,
            //beforeSend: function (xhr) {
            //    xhr.setRequestHeader("XSRF-TOKEN",
            //        $('input:hidden[name="__RequestVerificationToken"]').val());
            //},
            //contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data) {
                    returnObject = data;
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                errorLogging(xmlHttpRequest, textStatus, errorThrown);
            }
        });
        return returnObject;
    };

    var getJson2Async = function (url, inputDataObject) {
        //var url = pageUrl;
        //var start = new Date();
        var returnObject = null;
        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            async: true,
            data: JSON.stringify(inputDataObject),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                //var end = new Date();
                //var seconds = "True >" + url + " : " + start + " - " + end + " -> " + ((end - start) / 1000);
                //console.log(seconds);
                if (data) {
                    returnObject = data;
                    //var end = new Date();
                    //var seconds = "False >" + url + " : " + start + " - " + end + " -> " + ((end - start) / 1000);
                    //console.log(seconds);
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                errorLogging(xmlHttpRequest, textStatus, errorThrown);
            }
        });
        return returnObject;
    };

    var getJson = function (url, withCache) {
        if (withCache == null)
            withCache = false;
        //var url = window.siteUrl + "/" + pageUrl;// "CommonAjaxCalls.aspx/" + "GetSelectedUsersSession";
        //var start = new Date();
        var returnObject = null;
        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            async: false,
            cache: withCache,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                returnObject = data;
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                errorLogging(xmlHttpRequest, textStatus, errorThrown);
            }
        });
        return returnObject;
    };

    var getWithParams = function (url, inputDataObject) {

        //var url = window.siteUrl + "/" + pageUrl;// "CommonAjaxCalls.aspx/" + "GetSelectedUsersSession";
        //var start = new Date();
        var returnObject = null;
        $.ajax({
            type: "POST",
            url: url,
            header: 'Content-Type: application/json',
            data: JSON.stringify(inputDataObject),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data) {
                    returnObject = data;
                    //var end = new Date();
                    //var seconds = "False >" + url + " : " + start + " - " + end + " -> " + ((end - start) / 1000);
                    //console.log(seconds);
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                errorLogging(xmlHttpRequest, textStatus, errorThrown);
            }
        });
        return returnObject;
    };

    var getHtml2 = function (url, jsonData) {
        //var url = window.siteUrl + "/" + pageUrl;// "CommonAjaxCalls.aspx/" + "GetSelectedUsersSession";
        //var start = new Date();
        var returnObject = null;
        $.ajax({
            type: "POST",
            header: 'Content-Type: application/json',
            data: jsonData,//JSON.stringify(inputDataObject),
            dataType: "html",
            async: false,
            url: url,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                returnObject = data;
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                errorLogging(xmlHttpRequest, textStatus, errorThrown);
            }
        });
        return returnObject;
    };

    var getHtml = function (url) {
        //var url = window.siteUrl + "/" + pageUrl;// "CommonAjaxCalls.aspx/" + "GetSelectedUsersSession";
        //var start = new Date();
        var returnObject = null;
        $.ajax({
            type: "POST",
            header: 'Content-Type: application/json',
            dataType: "html",
            async: false,
            url: url,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                returnObject = data;
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                errorLogging(xmlHttpRequest, textStatus, errorThrown);
            }
        });
        return returnObject;
    };

    var getHtml2Async = function (url, jsonData) {
        //var url = window.siteUrl + "/" + pageUrl;// "CommonAjaxCalls.aspx/" + "GetSelectedUsersSession";
        //var start = new Date();
        var returnObject = null;
        $.ajax({
            type: "POST",
            header: 'Content-Type: application/json',
            data: jsonData,//JSON.stringify(inputDataObject),
            dataType: "html",
            async: true,
            url: url,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                returnObject = data;
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                errorLogging(xmlHttpRequest, textStatus, errorThrown);
            }
        });
        return returnObject;
    };

    var getHtmlAsync = function (url) {
        //var url = window.siteUrl + "/" + pageUrl;// "CommonAjaxCalls.aspx/" + "GetSelectedUsersSession";
        //var start = new Date();
        var returnObject = null;
        $.ajax({
            type: "POST",
            header: 'Content-Type: application/json',
            dataType: "html",
            async: true,
            url: url,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                returnObject = data;
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                errorLogging(xmlHttpRequest, textStatus, errorThrown);
            }
        });
        return returnObject;
    };


    var errorLogging = function (xmlHttpRequest, textStatus, errorThrown) {
        // var pageUrl = "CommonAjaxCalls.aspx/" + "LogException";
        if (xmlHttpRequest.responseText != undefined) {
            var isSessionNull = (xmlHttpRequest.responseText.indexOf("SetWebMethodSession()") != -1)
            if (isSessionNull) { // if isSessionNull true then error is raised from the session method
                // show popup , here
            }
        }
        var pageUrl = window.siteUrl + "/" + "CommonAjaxCalls.aspx/" + "LogException";// "CommonAjaxCalls.aspx/" + "GetSelectedUsersSession";

        var edata = { "message": textStatus, "exception": xmlHttpRequest };
        $.ajax({
            type: "POST",
            url: pageUrl,
            dataType: "json",
            async: false,
            data: JSON.stringify(edata),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
            },
            error: function (xmlHttpRequest1, textStatus1, errorThrown1) {

            }
        });
    };

    return {
        GetJson: getJson,
        Post: getWithParams,
        GetJsonAsync: getJsonAsync,
        GetJson2Async: getJson2Async,
        GetHtml: getHtml,
        GetHtml2: getHtml2,
        GetHtmlAsync: getHtmlAsync,
        GetHtml2Async: getHtml2Async,
        errorLogging: errorLogging
    };
}();