$(document).ready(function () {
    var txtArea = $("textarea");
    var resize = function () {
        txtArea.scrollTop = txtArea.scrollHeight;
        txtArea.style.height = "" + txtArea.scrollHeight + "px";
    };
    txtArea.on("input change", resize);
    $timeout(resize, 0);
});