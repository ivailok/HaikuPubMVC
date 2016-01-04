$(document).ready(function () {
    $('#authentication-form').on('submit', function () {
        var hash = CryptoJS.SHA3($('#Password').val(), { outputLength: 512 });
        $('#PublishCode').val(hash.toString());
        //$('#Password').remove();
    });
});