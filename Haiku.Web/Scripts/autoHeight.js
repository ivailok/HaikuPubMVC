$(document).ready(function () {
    var txtArea = $("#expanding-textarea");
    txtArea[0].focus();
    var resize = function () {
        txtArea[0].scrollTop = txtArea[0].scrollHeight;
        if (txtArea[0].scrollHeight > 100) {
            txtArea[0].style.height = "" + txtArea[0].scrollHeight + "px";
        }
    };
    txtArea.on("input change", resize);
    resize();
});