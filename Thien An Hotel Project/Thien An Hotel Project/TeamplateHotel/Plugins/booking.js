var getCheckOut = false;

var checkIn = new Date();
var checkOut = new Date();
var termDate = new Date();
checkOut.setDate(checkIn.getDate() + 1);

var arrayDate = new Array();
arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkIn));
arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkOut));

$(document).ready(function () {

    $("#s-arrive").val($.datepicker.formatDate('mm/dd/yy', checkIn));
    $("#s-depart").val($.datepicker.formatDate('mm/dd/yy', checkOut));

    $("#s-arrive").datepicker({
        showAnim: 'show',
        numberOfMonths: 1,
        duration: 600,
        minDate: new Date(),
        onSelect: function (dataText, inst) {
            getCheckOut = false;
            checkIn = $(this).datepicker("getDate");
            termDate = $(this).datepicker("getDate");
            termDate.setDate(checkIn.getDate() + 1);
            checkOut = termDate;
            console.log("checkIn: " + checkIn + ", CheckOut: " + checkOut);
            $("#s-depart").val($.datepicker.formatDate('mm/dd/yy', checkOut));
            $('#s-depart').datepicker('option', { minDate: checkIn });

            arrayDate = new Array();
            arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkIn));
            arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkOut));

        },
        beforeShowDay: function (date) {
            var getDay = parseInt(date.getDate()) < 10 ? "0" + date.getDate() : date.getDate();
            var getMonth = parseInt(date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var theday = getMonth + '/' + getDay + '/' + date.getFullYear();
            return [true, $.datepicker.formatDate('mm/dd/yy', checkIn) == theday ? "first-specialDate" : $.datepicker.formatDate('mm/dd/yy', checkOut) == theday ? "last-specialDate" : $.inArray(theday, arrayDate) >= 0 ? "specialDate" : ""];
        },
        beforeShow: function () {
            $("#s-arrive").val("Select a date");
        },
        onClose: function () {
            $("#s-arrive").val($.datepicker.formatDate('mm/dd/yy', checkIn));
        }
    });

    $("#s-depart").datepicker({
        showAnim: 'show',
        numberOfMonths: 1,
        duration: 600,
        minDate: checkOut,
        onSelect: function (dataText, inst) {
            getCheckOut = true;
            checkIn = new Date($("#s-arrive").val());
            checkOut = $(this).datepicker("getDate");

            if (checkOut <= checkIn) {
                checkOut.setDate(checkIn.getDate() + 1);
                $("#s-depart").val($.datepicker.formatDate('mm/dd/yy', checkOut));
            }

            termDate = new Date($("#s-arrive").val());
            arrayDate = new Array();
            while (termDate <= checkOut) {
                arrayDate.push($.datepicker.formatDate('mm/dd/yy', termDate));
                termDate.setDate(termDate.getDate() + 1);
            }
        },
        beforeShowDay: function (date) {

            var getDay = parseInt(date.getDate()) < 10 ? "0" + date.getDate() : date.getDate();
            var getMonth = parseInt(date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var theday = getMonth + '/' + getDay + '/' + date.getFullYear();
            return [true, $.datepicker.formatDate('mm/dd/yy', checkIn) == theday ? "first-specialDate" : $.datepicker.formatDate('mm/dd/yy', checkOut) == theday ? "last-specialDate" : $.inArray(theday, arrayDate) >= 0 ? "specialDate" : ""];
        },
        beforeShow: function () {
            $("#s-depart").datepicker('setDate', new Date($("#s-arrive").val()));
            $("#s-depart").val("Select a date");
        },
        onClose: function () {
            $("#s-depart").val($.datepicker.formatDate('mm/dd/yy', checkOut));
        }
    });


});

