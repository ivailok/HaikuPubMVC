$(document).ready(function () {
    var submitBtn = $('#submit-button');
    submitBtn.on('click', function () {
        submitBtn.prop('disabled', true);
    });
});