$(document).ready(function () {
    var rating = $("#rating");
    rating.rateit();

    var tooltipvalues = ["Hate it", "Don't like it", "It's okay", "Like it", "Love it"];
    rating.bind('over', function (event, value) {
        $("#ratingHover").text(value === null ? "" : tooltipvalues[value - 1]);
    });

    rating.on('beforerated', function (e, value) {
        if (!confirm('Are you sure you want to rate this item: ' + value + ' stars?')) {
            e.preventDefault();
        }
    });

    rating.bind('rated', function (e, value) {
        var haikuId = $("#haikuId").val();

        $.ajax({
            url: '/Haikus/Rate/' + haikuId,
            data: { rating: value },
            type: 'POST'
        });
    });
});