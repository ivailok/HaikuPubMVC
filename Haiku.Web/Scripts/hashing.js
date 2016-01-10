$(document).ready(function () {
    $('#authentication-form').on('submit', function () {
        var hash = CryptoJS.SHA3($('#OriginalPassword').val(), { outputLength: 512 });
        $('#Password').val(hash.toString());
        //$('#Password').remove();
    });
});