function BindSelectPicker(data, ddlSelector, hdSelector = "") {
    $(ddlSelector).empty();
    var items = '<option value="0">--Select--</option>';
    if (data !== null && data.length > 0) {

        var appenddata;
        $.each(data, function (key, obj) {
            appenddata += "<option value = '" + obj.value + "'>" + obj.text + " </option>";
        });
        $(ddlSelector).html(appenddata);


        var hdValue = !$.isNumeric(hdSelector) && hdSelector.indexOf('#') != -1 ? hdValue = $(hdSelector).val() : hdValue = hdSelector;

        if (hdValue != null && hdValue != '') {
            $(ddlSelector).val(hdValue);
            if ($(ddlSelector).val() == null || $(ddlSelector).val() == undefined) {
                $(ddlSelector + " option").filter(function (index) { return $(this).text() === "" + hdValue + ""; }).attr('selected', 'selected');
            }
        }
        else {
            if ($(ddlSelector).length > 0)
                $(ddlSelector)[0].selectedIndex = 0;
        }
    }
}



function BindDropdown(data, ddlSelector, hdSelector = "") {
    $(ddlSelector).empty();
    var items = '<option value="0">--Select--</option>';
    if (data !== null && data.length > 0) {
        $.each(data, function (i, obj) {
            var newItem = "<option id='" + obj.value + "' value='" + obj.value + "'>" + obj.text + "</option>";
            items += newItem;
        });

        $(ddlSelector).html(items);
        var hdValue = !$.isNumeric(hdSelector) && hdSelector.indexOf('#') != -1 ? hdValue = $(hdSelector).val() : hdValue = hdSelector;

        if (hdValue != null && hdValue != '') {
            $(ddlSelector).val(hdValue);
            if ($(ddlSelector).val() == null || $(ddlSelector).val() == undefined) {
                $(ddlSelector + " option").filter(function (index) { return $(this).text() === "" + hdValue + ""; }).attr('selected', 'selected');
            }
        }
        else {
            if ($(ddlSelector).length > 0)
                $(ddlSelector)[0].selectedIndex = 0;
        }
    }
}

