var pageUrl = "/Contact/";
var StaticNoteid = 0;

$(document).ready(function () {
    $(document).on("click", ".deleteNote", function () {
        $(this).parents("tr").remove();
    });
    $('[data-toggle="tooltip"]').tooltip();
    var actions = $("table td:last-child").html();

    // Append table with add row form on add new button click
    $("#add-new").click(function () {
        StaticNoteid = StaticNoteid + 1;
        var row = '<tr>' +
            '<td>' + $("#NotesTitle").val() + '</td>' +
            '<td>' + $("#Description").val() + '</td>' +
            '<td><a class="deleteNote" title="Delete" id="' + StaticNoteid + '" data-toggle="tooltip" onclick="DeleteNote(' + StaticNoteid + ')"><i class="material-icons">&#xE872;</i></a></td>' +
            '<td>' + actions + '</td>' +
            '</tr>';
        $("#tableNotes").append(row);
    });

    if ($("#Id").val() > 0) {
        $("#btnSaveContact").text('Update');
    }
});

function DeleteContact(id) {
    var ajaxUrl = "/Contact/DeleteContactAsync/" + id;
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