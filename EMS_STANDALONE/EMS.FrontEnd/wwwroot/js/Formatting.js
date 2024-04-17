///Allow alphabet and ('), (-) characters in 'Name' fields
function NameOnly(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^[a-zA-Z-.\']*$/;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 32) {
            e.preventDefault();
        }
    });
}

// Allows Number and + only
function TelNoOnly(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9]|[+])$/g;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        if (txtbox.val().indexOf('+') != -1) {
            if (e.shiftKey == 1 && e.keyCode == 187 || (e.keycode == 107))
                e.preventDefault();
        }
    });
}

///9 Digits with 2 Decimal Places 
///Formatting for Amount
function AmountDecimalFormat(FieldName) {
    FieldName.keypress(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{2})?$/g;
        var Amount = FieldName.val().replace(/,/g, "");

        if (e.keyCode == 32) {
            e.preventDefault();
        }
        if (FieldName.val() == "" && e.keyCode == 190) {
            FieldName.val("0");
        }
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false"
             && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 9 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}

///MM/dd/yyyy Formatting for date
function DateFormat(dtp) {
    var date = new Date(dtp.val());
    dtp.val(formatDateToString(date));
}

function formatDateToString(date) {
    // 01, 02, 03, ... 29, 30, 31
    var dd = (date.getDate() < 10 ? '0' : '') + date.getDate();
    // 01, 02, 03, ... 10, 11, 12
    var MM = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
    // 1970, 1971, ... 2015, 2016, ...
    var yyyy = date.getFullYear();

    // create the format you want
    return (MM + "/" + dd + "/" + yyyy);
}

function newformatDateToString(date) {
    const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    ];
    // 01, 02, 03, ... 29, 30, 31
    var dd = (date.getDate() < 10 ? '0' : '') + date.getDate();
    // 01, 02, 03, ... 10, 11, 12
    var MM = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth());
    // 1970, 1971, ... 2015, 2016, ...
    var yyyy = date.getFullYear();

    // create the format you want
    return (dd + "-" + monthNames[MM] + "-" + yyyy);
}

function Code(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^[a-zA-Z0-9_-]*$/; //added \-
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 32) {
            e.preventDefault();
        }
    });
}


///1 Digit with 4 Decimal Places for Percentage
function PercentDecimalFormat(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{4})?$/g;
        var Amount = txtbox.val().replace(/,/g, "");

        if (e.keyCode == 32) //Space
        {
            e.preventDefault();
        }
        
        if (txtbox.val() == "" && e.keyCode == 190) {
            txtbox.val("0");
        }

        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false"
             && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 1 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}

///Automatic put comma when textbox changed / remove focus
function AutoCommaAmount(txtbox) {
    txtbox.on('change', function () {
        if (txtbox.val() != "") {
            var amount = txtbox.val();
            txtbox.val(parseFloat(amount, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
        }
    });
}

///Automatic put comma in display, 2 decimal places
function AutoCommaAmount_showOnEdit(txtbox) {
    if (txtbox.val() != "") {
        var amount = txtbox.val();
        txtbox.val(parseFloat(amount, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
    }
}

///Automatic put comma in display, 4 decimal places
function AutoCommaAmount_showOnEdit4(txtbox) {
    if (txtbox.val() != "") {
        var amount = txtbox.val();
        txtbox.val(parseFloat(amount, 12).toFixed(4).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
    }
}

///Accepts Numbers Only
function NumericFormat(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9])$/g;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
    });
}

//Accepts Negative and positive whole number
function IntegerFormat(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9])$/g;
        var value = txtbox.val();
        var charvalue = String.fromCharCode(e.keyCode)

        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false"
            && e.keyCode != 8 && charvalue != "½"
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        if (charvalue == "½" && value.includes("-"))
        {
            e.preventDefault(); 
        }

        if (charvalue == "½" && value.length > 0) {
            e.preventDefault();
        }

        if (charvalue == "0" && value.length == 1 && value.substring(0, 1) == "-") {
            e.preventDefault();
        }

        if (charvalue == "0" && value.length == 0) {
            e.preventDefault();
        }

    });
}

//Accepts Negative and positive decimal
function DecimalsFormat(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^-?[0-9]\d*(\.\d+)?$/;
        var value = txtbox.val();
        var charvalue = String.fromCharCode(e.keyCode)

        if (("" + reg.test(e.keyCode)) == "false"
            && e.keyCode != 8 && charvalue != "½" && charvalue != "¾"
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
            ) {
            e.preventDefault();
        }

        if (charvalue == "½" && value.includes("-")) {
            e.preventDefault();
        }

        if (charvalue == "¾" && value.includes(".")) {
            e.preventDefault();
        }

        if (charvalue == "½" && value.length > 0) {
            e.preventDefault();
        }

        if (charvalue == "¾" && value.length == 1 && value.substring(0, 1) == "-") {
            e.preventDefault();
        }

    });
}

function Days3Format(txtbox) {
     txtbox.keypress(function (e) {
         if ($(this).val().length == 3
          && e.keyCode != 46 // delete
          && e.keyCode != 8 // backspace
         ) {
             e.preventDefault();
         }
    })
}

///no Space Allowed
function PreventSpace(txtbox) {
    txtbox.keypress(function (e) {
        if (e.keyCode == 32
            ) {
            e.preventDefault();
        }
    });
}

///1 Digit with 4 Decimal Places for Percentage
function PercentDecimalFormatPagibig(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{4})?$/g;
        var Amount = txtbox.val().replace(/,/g, "");

        if (e.keyCode == 32) //Space
        {
            e.preventDefault();
        }
        if (txtbox.val() == "" && e.keyCode != 49 && e.keyCode != 190 && e.keyCode != 48) {
            e.preventDefault();
        }
        if (txtbox.val() == "" && e.keyCode == 190) {
            txtbox.val("0");
        }
        if (txtbox.val() == 1 &&  e.keyCode != 8) {
            e.preventDefault();
        }
        
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false"
             && e.keyCode != 8
             && parseFloat(Amount) > 1
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9 
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 1 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}

///Remove value of fields when Key up and down.
function RemoveValue(txtbox)
{
    txtbox.keypress(function (e) {
        txtbox.val("");
    });

    txtbox.keyup(function (e) {
        txtbox.val("");
    });
}

///Accept alphanumeric only
function AlphaNumeric(txtbox)
{
    txtbox.bind('input', function () {
        $(this).val($(this).val().replace(/[^a-z0-9\s]/gi, ''));
    });
}

function PersonnelCode(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^[a-zA-Z0-9_-]*$/; //removed \-
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 32) {
            e.preventDefault();
        }
    });
}

///2 Digit with 4 Decimal Places for Percentage
function DaysDecimalFormat(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{4})?$/g;
        var Amount = txtbox.val().replace(/,/g, "");

        if (e.keyCode == 32) //Space
        {
            e.preventDefault();
        }

        if (txtbox.val() == "" && e.keyCode == 190 && e.keyCode == 110) {
            txtbox.val("0");
        }
		
		if (e.shiftKey) {
			e.preventDefault();
		}
        
		if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false"
             && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 110 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && (e.keyCode == 190 || e.keyCode == 110)) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 2 && e.keyCode != 190 && e.keyCode != 110 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}
///3 Digit with 4 Decimal Places for Percentage
function AnnualDaysDecimalFormat(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9])$/g;
        var decreg = /^\d+(\.\d{4})?$/g;
        var Amount = txtbox.val().replace(/,/g, "");

        if (e.keyCode == 32) //Space
        {
            e.preventDefault();
        }

        if (txtbox.val() == "" && e.keyCode == 190) {
            txtbox.val("0");
        }

        if (e.shiftKey) {
            e.preventDefault();
        }

        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false"
             && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 190 && e.keyCode != 9
            ) {
            e.preventDefault();
        }
        else if (Amount.includes(".") && e.keyCode == 190) {
            e.preventDefault();
        }

        else if (Amount.includes(".") && Amount.match(decreg) && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9) {
            e.preventDefault();
        }

        else if (!Amount.includes(".") && Amount.length == 3 && e.keyCode != 190 && e.keyCode != 8) {

            e.preventDefault();
        }

    });
}

//Validate if valid email
function ValidEmail(txtbox) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(txtbox.val());
}

// valid Moile No.
function validMobileNo(txtbox) {
    //txtbox.keydown(function (e) {
    //    var reg = /[0-9]$/;
    //    if ((("" + reg.test(String.fromCharCode(e.keyCode))) == "false") && e.keyCode != 8
    //        && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
    //    ) {
    //        e.preventDefault();
    //    }
    //    if (txtbox.val() == "00000000000") {
    //        txtbox.val('');
    //    }
    //});

    //txtbox.blur(function () {
    //    var reg = /^[0-9+]*$/;
    //    if ("" + reg.test(txtbox.val()) == "false") {
    //        txtbox.val('');
    //    }
    //});
    var re = /^(09)(\d){02}[-](\d){07}$/;
    return re.test(txtbox.val());
}

// Add Hyphen
function addHyphen(txtbox, count) {
    var res = txtbox.val().split("-").join(""); // remove hyphens
    if (res.length > 0) {
        res = res.match(new RegExp('.{1,'+count+'}', 'g')).join("-");
    }
    txtbox.val(res);
}

// Allows Number and - only
function NumberDashOnly(txtbox) {
    var re = /^[0-9\-]+$/;

    txtbox.on("keyup, keydown, keypress, input", function (e) {
        if (!re.test(txtbox.val())) {
            txtbox.val('');
        }
    });
}

function formatSSSNumber(txtbox) {
    var val = txtbox.val();
    txtbox.val(val.replace(/(\d{2})(\d{7})(\d{1})/, "$1-$2-$3"));
}

function AutoCapitalFirst(txtbox) {
    txtbox.keypress(function (e) {
        const arr = txtbox.val().split(" ");

        for (var i = 0; i < arr.length; i++) {
            arr[i] = arr[i].charAt(0).toUpperCase() + arr[i].slice(1);
        }

        const str2 = arr.join(" ");
        txtbox.val(str2);
    });
}