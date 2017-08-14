$(function () {
    $(document).on('change', ':file', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });
    $(document).ready(function () {
        $(':file').on('fileselect', function (event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = label;

            if (input.length) {
                input.val(log);
            } else {
                if (log) alert(log);
            }

        });
    });
    tinymce.init({
        selector: 'textarea',
        height: 300,
        menubar: false,
        plugins: 'paste lists',
        paste_as_text: true,
        toolbar: 'undo redo | bold italic | bullist numlist outdent indent',
        content_css: '//www.tinymce.com/css/codepen.min.css'
    });
});