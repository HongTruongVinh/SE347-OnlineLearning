
//#region bắt đầu các hàm xử lý Datatable (phân trang trong trang quản lý user)
$.extend(true, $.fn.dataTable.defaults, {
    "lengthMenu": [10, 25, 50, 75, 100],
    "pageLength": 100
});

$(document).ready(function () {
    $('#datatable').DataTable();
});
//#endregion kết thúc

//#region bắt đầu các hàm xử lý particalview

$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {

        var url = $(this).data('url');
        var decodeUrl = decodeURIComponent(url);
        $.get(decodeUrl).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        })
    })

    PlaceHolderElement.on('click', '[data-save="modal_update"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();

        var id = $('input[name="ID"]').val();
        var username = $('input[name="UserName"]').val();
        var name = $('input[name="Name"]').val();
        var address = $('input[name="Address"]').val();
        var email = $('input[name="Email"]').val();
        var phone = $('input[name="Phone"]').val();

        $.post(actionUrl, sendData).done(function (dataResult) {
            PlaceHolderElement.find('.modal').modal('hide');
            $('#datatable').find('#row_' + id).find("td").eq(1).html(username);
            $('#datatable').find('#row_' + id).find("td").eq(2).html(name);
            $('#datatable').find('#row_' + id).find("td").eq(3).html(address);
            $('#datatable').find('#row_' + id).find("td").eq(4).html(email);
            $('#datatable').find('#row_' + id).find("td").eq(5).html(phone);
        })
    })

    PlaceHolderElement.on('click', '[data-save="modal_delete"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();

        var id = $('input[name="ID"]').val();

        $.post(actionUrl, sendData).done(function (dataResult) {
            PlaceHolderElement.find('.modal').modal('hide');
            $('#datatable').find('#row_' + id).remove();
        })
    })

    PlaceHolderElement.on('click', '[data-dismiss="modal"]', function (event) {
        event.preventDefault();
        PlaceHolderElement.find('.modal').modal('hide');
    })

})

//#endregion
