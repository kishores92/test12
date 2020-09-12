var pageUrl = "/Role/";
var ScreenPermissionsData;
var roleId;
$(document).ready(function () {
    BindPermissionsData();
    //$("#PermissionRoleIds").change(function () {
    //    if ($(this).val() != "" && $(this).val() != "0") {
    //        var data = $(this).val();
    //        var permissionIds = "";
    //        $.each(function (data,i) {
    //            permissionIds += data[i];
    //        });

    //        $("#PermissionIds").val();
    //    }
    //});
});

function BindPermissionsData() {
    var ajaxUrl = "/Role/GetScreenPermissionsAsync";
    $.ajax({
        type: "GET",
        url: ajaxUrl,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var ddlPermissions = $("#PermissionRoleIds");
            ddlPermissions.empty();
            if (response.length > 0) {
                BindSelectPicker(response, "#PermissionRoleIds");
            }
            else
                ddlPermissions.html('<option selected="selected" value="0">Select</option>');
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function DeleteRole(id) {
    var ajaxUrl = "/Role/DeleteRoleAsync/" + id;
    $.ajax({
        type: "Get",
        url: ajaxUrl,
        //beforeSend: function (xhr) {
        //    xhr.setRequestHeader("XSRF-TOKEN",
        //        $('input:hidden[name="__RequestVerificationToken"]').val());
        //},
        contentType: "application/json; charset=utf-8",
        success: function (response) {
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function UpDateRoles(id) {
    var ajaxUrl = "/Role/GetRoleByIdAsync/" + id;

    alert("editrol");
    $.ajax({
        type: "Get",
        url: ajaxUrl,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response != null) {
                BindPermissionsData();
                BindRolesData(response);
            }
            else
                ddlPermissions.html('<option selected="selected" value="0">Select</option>');
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function SaveRole() {
    var Role = {
        Name: $("#RoleName").val(),
        PermissionIds: $("#ddlPermissions").val()
    };
    var ajaxUrl = "/Role/SaveRoleAsync";
    $.ajax({
        type: "POST",
        url: ajaxUrl,
        data: JSON.stringify(Role),
        contentType: "application/json; charset=utf-8",
        success: function (response) {
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function BindRolesData(data) {
    if (data !== null) {
        $("#Id").val(data.id);
        $("#Name").val(data.name);
        $("#PermissionRoleIds").val(data.permissionRoleIds);
    }
}