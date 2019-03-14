
$(document).ready(function () {
    //get order size from image.
    $(".orderSize").click(function () {
        var orderSize = $(this).data("value")

        $.ajax({
            url: 'OrderTotals',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            type: 'POST',
            cache: false,
            data: JSON.stringify({
                OrderSize: orderSize
            })
        }).done(function (jsonResults) {

            var model = $.parseJSON(jsonResults)
            var orderTotal = (model.OrderTotal).formatMoney(2);
            var orderSize = model.OrderSize;

            $("#price").html('<strong>Total Amount: $' + orderTotal + '</strong>');
            $("#size").html('<strong>Number of goodies: ' + orderSize + '</strong>');

            $("#selectableItems").fadeIn('slow');
                $('html, body').animate({
                    scrollTop: $("#selectableItems").offset().top - 100
                }, 750);

                HandleCheckboxSelection("#selectableItems", orderSize);
        });

        $("#selectableItems input:checkbox").click(function () {
            HandleCheckboxSelection("#selectableItems", orderSize);
        });
    });



    $(document).scroll(function () {
        //stick nav to top of page
        var viewport = {
            width: $(window).width(),
            height: $(window).height()
        };
        var y = $(this).scrollTop();
        var navWrap = $('#assembleBox').offset().top;
        if (viewport.width > 780) {
            if (y > navWrap) {
                $('#orderDetails').hide().addClass('sticky').fadeIn();
            }
            else {
                $('#orderDetails').removeClass('sticky');
            }
        }
        else {
            //if (y > navWrap) {
            //    $('#orderDetails').hide().addClass('sticky').fadeIn();
            //}
            //else {
            //    $('#orderDetails').removeClass('sticky');
            //}
        }
    });
});

function GetSelectedCheckboxCount(selector) {
    var selectedCheckboxCount = 0;
    $(selector + " input:checkbox").each(function () {
        if ($(this).is(":checked")) {
            selectedCheckboxCount++;
        }
    });

    return selectedCheckboxCount;
}

function HandleCheckboxSelection(selector, selectionLimit) {

    if (GetSelectedCheckboxCount(selector) == selectionLimit) {
        $(selector + " .btnAssembleBox").removeAttr("disabled");
        $(selector + " input:checkbox").each(function () {
            if ($(this).is(":checked")) {
                $(this).removeAttr("disabled");
            }
            else {
                $(this).attr("disabled", "true");
            }
        });
        $('html, body').animate({
            scrollTop: $("#messageBox").offset().top - 100
        }, 750);
    }
    else {
        $(selector + " .btnAssembleBox").attr("disabled", "true");
        $(selector + " input:checkbox").each(function () {
            $(this).removeAttr("disabled");
        });
    }
}

new WOW().init();

Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};




//$("#AlternateShipping").click(function () {

//    if ($("#AlternateShipping").attr("checked")) {
//        $("#AlternateShippingAddress").fadeIn('slow');
//    }
//    else {
//        $("#AlternateShippingAddress").fadeOut('slow');
//    }
//});
