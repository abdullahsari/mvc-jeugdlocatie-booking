$(function () {
    $('button').on('click', function (e) {
        alert($(e.target).data('message'));
    });
});