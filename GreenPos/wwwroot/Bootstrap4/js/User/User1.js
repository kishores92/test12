var pageUrl = "/User/";
var userId;

function BindRolesData() {
    var ajaxUrl = "/User/GetRolesAsync";
    $.ajax({
        type: "GET",
        url: ajaxUrl,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var ddlRoles = $("#ddlRoles");
            ddlRoles.empty();
            if (response.length > 0) {
                BindDropdown(response, "#ddlRoles");
            }
            else
                ddlPermissions.html('<option selected="selected" value="0">Select</option>');
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function DelUser(id) {
    var ajaxUrl = "/User/DeleteUserAsync/" + id;
    $.ajax({
        type: "Get",
        url: ajaxUrl,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            location.reload();
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function SaveUser() {
    var vm = JSON.stringify({
        UserName: $("#UserName").val(),
        CompanyName: $("#CompanyName").val(),
        Email: $("#Email").val(),
        Password: $("#Password").val(),
        RoleId: $("#RoleId").val(),
        Id: $("#Id").val()
    });

    var ajaxUrl = pageUrl + "SaveUserAsync";
    $.ajax({
        type: "POST",
        url: ajaxUrl,
        async: true,
        //beforeSend: function (xhr) {
        //    xhr.setRequestHeader("XSRF-TOKEN",
        //        $('input:hidden[name="__RequestVerificationToken"]').val());
        //},
        data: vm,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            console.log(1);
            location.reload();
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}


function EdUser(id) {
    var ajaxUrl = "/User/GetUserAsync/" + id;
    $.ajax({
        type: "Get",
        url: ajaxUrl,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response != null) {
                BindUserDetails(response);
            }
            location.reload();
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function BindUserDetails(data) {
    if (data !== null) {
        $("#Id").val(data.id);
        $("#UserName").val(data.userName);
        $("#CompanyName").val(data.companyName);
        $("#Email").val(data.email);
        $("#Password").val(data.password);
        $("#RoleId").val(data.roleId);
        $("#btnSave").text("Update");
    }
}

function closeForm() {
    $("#btnSave").text("Save");
    $("#myModal").close();
}