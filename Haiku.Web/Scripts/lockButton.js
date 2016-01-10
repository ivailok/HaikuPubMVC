$(document).ready(function () {
    $("#authentication-form").data("validator").settings.submitHandler = function (form) {
        $("#submit-button").attr("disabled", true).val("Working");
        form.submit();
    };
});