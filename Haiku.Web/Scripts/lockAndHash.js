$(document).ready(function () {
    $("#authentication-form").data("validator").settings.submitHandler = function (form) {
        $("#submit-button").attr("disabled", true).val("Working");
        var hash = CryptoJS.SHA3($('#OriginalPassword').val(), { outputLength: 512 });
        $('#Password').val(hash.toString());
        form.submit();
    };
});