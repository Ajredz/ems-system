var sideMenuInitialHeight = 0;
$(document).ready(function () {

    sideMenuInitialHeight = $(".system-side-menu").height();

    $(".menu-item").mousedown(function () {
        $(this).next('ul').toggle(100);

        if ($(this).find("span.glyphicon").hasClass("glyphicon-chevron-down")) {
            $(this).find("span.glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            $(this).find("span.glyphicon-chevron-down").removeClass("glyphicon-chevron-down");
        }
        else {
            $(this).find("span.glyphicon-chevron-up").addClass("glyphicon-chevron-down");
            $(this).find("span.glyphicon-chevron-up").removeClass("glyphicon-chevron-up");
        }

        ///// move system-user-logs when menu length reached it
        //setTimeout(function () {
        //    var containerHeight = $(".system-side-menu").height() > $(".system-body-container").height() ?
        //        $(".system-side-menu").height() : $(".system-body-container").height()

        //    $(".system-body-container").css("height", containerHeight);
        //    if ($(".navbar-collapse.collapse").height() > (sideMenuInitialHeight - ($(".system-user-logs").height()))) {
        //        $(".system-user-logs").css({
        //            "bottom": $(".system-body-container").height() > sideMenuInitialHeight ? 0 : (sideMenuInitialHeight - ($(".system-user-logs").height())) - $(".navbar-collapse.collapse").height(),
        //            "position":
        //                $(".system-body-container").height() + $(".system-user-logs").height() > sideMenuInitialHeight ? "relative" : "relative"
        //        });
        //    }
        //    else {
        //        $(".system-user-logs").css({
        //            "bottom": 0,
        //            "position": "absolute"
        //        });
        //    }
        //}, 100);

    });

    /// set active menu-item
    if (!$("a[href='" + window.location.pathname + "']").parent('li').hasClass("first-level")) {

        // disable auto toggle for my account links
        if (window.location.pathname != "/myaccount/manageaccount") {
            $("a[href='" + window.location.pathname + "']").parent('li').parent('ul').toggle(100);
        }

        if ($("a[href='" + window.location.pathname + "']").parent('li').hasClass("third-level")) {

            // disable auto toggle for my account links
            if (window.location.pathname != "/myaccount/manageaccount") {
                $("a[href='" + window.location.pathname + "']").parent('li').parent('ul').parent('ul').toggle(100);
            }

            $("a[href='" + window.location.pathname + "']").parent('li').parent('ul').prev().find("span.glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            $("a[href='" + window.location.pathname + "']").parent('li').parent('ul').prev().find("span.glyphicon-chevron-down").removeClass("glyphicon-chevron-down");
        }
    }
    $(".menu-item").removeClass("active");
    $("a[href='" + window.location.pathname + "']").parent('li').addClass("active");
    $("a[href='" + window.location.pathname + "']").parent('li').parent('ul').prev().find("span.glyphicon-chevron-down").addClass("glyphicon-chevron-up");
    $("a[href='" + window.location.pathname + "']").parent('li').parent('ul').prev().find("span.glyphicon-chevron-down").removeClass("glyphicon-chevron-down");
    
    /// end

    $(".AccordionList .field-title").click(function () {
        if ($(".AccordionList").hasClass("off")) {
            $(".AccordionList").removeClass("off");
            $(".AccordionList").addClass("on");
        }
        else if ($(".AccordionList").hasClass("on")) {
            $(".AccordionList").removeClass("on");
            $(".AccordionList").addClass("off");
        }
    });

    if ($('#back-to-top').length) {
        $(window).on('scroll', function () {
            if ($(window).scrollTop() > 100) {
                $('#back-to-top').addClass('show');
            } else {
                $('#back-to-top').removeClass('show');
            }
        });

        $('#back-to-top').on('click', function (e) {
            $('html,body').animate({
                scrollTop: 0
            });
            return false;
        });
    }

   
});

function handle_mousedown(e) {
    window.my_dragging = {};
    my_dragging.pageX0 = e.pageX;
    my_dragging.pageY0 = e.pageY;
    my_dragging.elem = this;
    my_dragging.offset0 = $(this).offset();

    $(my_dragging.elem).css({ "cursor": "-webkit-grabbing" });

    function handle_dragging(e) {
        var left = my_dragging.offset0.left + (e.pageX - my_dragging.pageX0);
        var top = my_dragging.offset0.top + (e.pageY - my_dragging.pageY0);

        $($(my_dragging.elem).closest(".modal"))
            .offset({ top: top, left: left });

        $($(my_dragging.elem).closest(".modal-content"))
            .offset({ top: top, left: left });

        $(my_dragging.elem)
            .offset({ top: top, left: left });

    }
    function handle_mouseup(e) {
        $('body')
            .off('mousemove', handle_dragging)
            .off('mouseup', handle_mouseup);

        $(my_dragging.elem).css({ "cursor": "-webkit-grab" });

    }
    $('body')
        .on('mouseup', handle_mouseup)
        .on('mousemove', handle_dragging);
}

// Add Comma to numbers. To use :: varname.withComma
String.prototype.withComma = function (value) { return this.replace(/\B(?=(\d{3})+(?!\d))/g, ","); };

// Remove Comma to numbers. To use :: varname.noComma
String.prototype.noComma = function (value) { return this.replace(/,/g, ""); };

// Date converter of ASP.NET
String.prototype.convertDateFromASP = function () {
    var dte = eval("new " + this.replace(/\//g, '') + ";");
    dte = (dte.getDate()) + "-" + (dte.getMonth() + 1) + "-" + (dte.getFullYear());

    return dte;
}

// Add Comma with 2 decimal places
String.prototype.commaOnAmount = function (value) {
    var amountWithComma = parseFloat(parseFloat(this.noComma()).toFixed(2)).toLocaleString("en");
    if (amountWithComma.split(".").length === 1)
        amountWithComma = amountWithComma + ".00";
    return amountWithComma;
}

// Adds decimal zeroes to end of the number.
// 1 = 1.00 | 1.1 = 1.10 | 1.11 = 1.11 | 1.567 = 1.57
function AddZeroes(num) {
    num = num.noComma();
    var value = parseFloat(num).toFixed(2);
    var res = value.toString().split(".");
    if (num.indexOf('.') === -1) {
        num = value.toString();
    } else if (res[1].length < 3) {
        num = value.toString();
    }
    return num;
}

// Set text change for decimal number
function AmountTextChange(e) {
    //If text box is 0.00 set value to blank
    e.focus(function () {
        if (parseFloat($(this).val()) == 0) {
            $(this).val("");
        }

        $(this).val($(this).val().withComma());
    });
    //If text box is empty set value to 0.00
    e.blur(function () {
        if ($(this).val() != "" & $(this).val() != ".") {
            $(this).val(AddZeroes($(this).val()).withComma());
        }
    });
}

//Accept only numbers, commas and dots
function AmountOnly(txtbox) {
    txtbox.keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13/*, 188*/]) !== -1 ||
            (e.keyCode == 65 && e.ctrlKey === true) ||
            (e.keyCode == 67 && e.ctrlKey === true) ||
            (e.keyCode == 88 && e.ctrlKey === true) ||
            (e.keyCode == 90 && e.ctrlKey === true) ||
            (e.keyCode == 89 && e.ctrlKey === true) ||
            (e.keyCode >= 35 && e.keyCode <= 39) ||
            ((e.keyCode == 110 || e.keyCode == 190) && $(txtbox).val().split('.').length < 2)) {
            return;
        }
        if (((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) ||
            ((e.keyCode == 110 || e.keyCode == 190) && $(txtbox).val().split('.').length === 2)
        ) {
            e.preventDefault();
        }

    });
}

// Remove Space
function NoSpace(txtbox) {
    txtbox.keydown(function (e) {
        if (e.keyCode == 32) {
            e.preventDefault();
        }
    });
}

// Number only
function NumberOnly(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9])$/g;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9 || e.shiftKey
        ) {
            e.preventDefault();
        }
    });
}

// Number only
function NumberOnlyWithNegative(txtbox) {
    txtbox.keypress(function (e) {
        var reg = /^([0-9-])$/g;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9 || e.shiftKey
        ) {
            e.preventDefault();
        }
    });
}


// Letters, Numbers and Hyphen
function LetterNumberHyphenOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([a-z]|[0-9]|[-])/ig;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// Letters, Space, Hypen only (Added by Charles 03/16/2017)
function LetterHyphenSpaceOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([a-z]|[-\s.])/ig;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// No special characters (Added by Charles 03/16/2017)
function InvalidSpecialChar(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([@!#$%^&*|?"'])/ig;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "true" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

//Number and Hypen Only (Added by Charles 03/16/2017)
function NumberHyphenOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([-]|[0-9])/ig;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

//User Define Expression  
///txtbox  - (e.g. $('#txtItem15PrintName'))
// reg     - (e.g. /([a-z]|[-])/ig) //Regular Expression
//Example Usage:
//              UserDefineRegEx($('#txtItem15PrintName'), /([a-z]|[-])/ig);
function UserDefineRegEx(txtbox, reg) {
    txtbox.keydown(function (e) {
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
} //Added by Charles 03/10/2017

//Number and Hypen Only (Added by Charles 03/16/2017)
function NumberHypenOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /([-]|[0-9])/ig;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// Allows Number and + only
function TelNoOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^([0-9]|[+])$/g;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

// Submit action using Ajax call
// url - URL of the function (Required)
// type - Indicate if POST or GET (Required)
// param - Parameters to be pass (Optional)
// button - Name of button to be disabled while waiting for the server response (Optional)
function AjaxSubmit(url, type, param, button) {
    if (param == undefined)
        param = "";

    Loading(true);

    $("#" + button + "").prop("disabled", true);

    $.ajax({
        url: url,
        type: "POST",
        data: param,
        //dataType: "json",
        //contentType: "application/json; charset=utf-8",
        success: function (data) {
            var msg = "";
            if (data.IsListResult == true) {
                for (var i = 0; i < data.Result.length; i++) {
                    msg += data.Result[i] + "<br />";
                }
            } else {
                msg += data.Result;
            }

            Loading(false);
            ModalAlert(MODAL_HEADER, msg, "");

            $("#" + button + "").prop('disabled', false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
            $("#" + button + "").prop("disabled", false);
        },
    });
}

// Alert using modal
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
function ModalAlert(header, body) {

    if (body == "Your session has expired. Please login again.") {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" onclick=\"location.reload()\" data-dismiss=\"modal\">Close</button>";
    }
    else {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" data-dismiss=\"modal\">Close</button>";
    }

    $("#divHeaderModal").html(header);
    if (body.length > 0) {
        $("#divBodyModal").html(body);
    }
    else {
        $("#divBodyModal").html(MSG_A_PROBLEM_HAS_OCCURRED);
    }

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });

    //setTimeout(function () {
    //    $("#divModal").modal("hide");
    //}, 2000);
}

// Alert using modal, then execute a function after continue
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
function ModalAlertContinue(header, body, controlId) {

    if (body == "Your session has expired. Please login again.") {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" onclick=\"location.reload()\" data-dismiss=\"modal\">Close</button>";
    }
    else {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" onclick=\"" + controlId + "\" data-dismiss=\"modal\">Close</button>";
    }

    $("#divHeaderModal").html(header);
    if (body.length > 0) {
        $("#divBodyModal").html(body);
    }
    else {
        $("#divBodyModal").html('Oops something went wrong. Please contact your system administrator.');
    }

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

// Alert using modal with Reload after Close
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
function ModalAlertReload(header, body) {

    if (body == "Your session has expired. Please login again.") {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" onclick=\"location.reload()\" data-dismiss=\"modal\">Close</button>";
    }
    else {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" onclick=\"location.reload()\" data-dismiss=\"modal\">Close</button>";
    }

    $("#divHeaderModal").html(header);
    if (body.length > 0) {
        $("#divBodyModal").html(body);
    }
    else {
        $("#divBodyModal").html('Oops something went wrong. Please contact your system administrator.');
    }

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

// Alert using modal with Reload after Close
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
function ModalAlertRedirect(header, body, url) {

    if (body == "Your session has expired. Please login again.") {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" onclick=\"window.location.href ='" + url + "'\" data-dismiss=\"modal\">Close</button>";
    }
    else {
        var footer = "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" onclick=\"window.location.href ='" + url + "'\" data-dismiss=\"modal\">Close</button>";
    }

    $("#divHeaderModal").html(header);
    if (body.length > 0) {
        $("#divBodyModal").html(body);
    }
    else {
        $("#divBodyModal").html('Oops something went wrong. Please contact your system administrator.');
    }

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

// Alert using modal with Reload after Close
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
function ModalAlertRedirectToList(header, body, listURL) {
    var footer = "";
    if (body == "Your session has expired. Please login again.") {
        footer += "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn\" onclick=\"location.reload()\" data-dismiss=\"modal\">Close</button>";
    }
    else {
        footer += "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn btn-auto-width\" onclick=\"window.location.href ='" + listURL + "'\" data-dismiss=\"modal\">Go to List</button>&nbsp;";
        footer += "<button id = \"btnModalClose\" type=\"button\" class=\"btnBlue formbtn btn-auto-width\" onclick=\"location.reload()\" data-dismiss=\"modal\">Stay on Page</button>";
    }

    $("#divHeaderModal").html(header);
    if (body.length > 0) {
        $("#divBodyModal").html(body);
    }
    else {
        $("#divBodyModal").html('Oops something went wrong. Please contact your system administrator.');
    }

    $("#divFooterModal").html(footer);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}


// Confirmation using modal
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
// controlId - Id of the html control to execute the action (Required)
// action - Type of action to execute click, submit, change, or function (Required)
// cancelButton - Function to execute if button cancel is clicked (Optional)
function ModalConfirmation(header, body, controlId, action, cancelButton) {
    var button = "";

    if (action == "function")
        button += "<button type=\"button\" id=\"btnConfirmationModal\" class=\"btnBlue formbtn\" onclick=\"" + controlId + "\">Yes</button>&nbsp;";
    else
        button += "<button type=\"button\" id=\"btnConfirmationModal\" class=\"btnBlue formbtn\" onclick=\"ContinueConfirmation('" + controlId + "', '" + action + "');\">Yes</button>&nbsp;";

    button += "&nbsp;";

    if (cancelButton != "")
        button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnBlue formbtn\" onclick=\"" + cancelButton + "\" data-dismiss=\"modal\">No</button>";
    else
        button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnBlue formbtn\" data-dismiss=\"modal\">No</button>";

    $("#divHeaderModal").html(header);
    $("#divBodyModal").html(body);
    $("#divFooterModal").html(button);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

// Execute Confirmation modal command
function ContinueConfirmation(controlId, action) {
    $("#btnConfirmationModal").prop("disabled", true);

    if (action == "submit")
        $("#" + controlId + "").submit();
    else if (action == "change")
        $("#" + controlId + "").change();
    else
        $("#" + controlId + "").click();
}

// Confirmation using modal
// header - Header title of the modal (Optional)
// body - Message content of the modal (Optional)
// destinationURL - Destination URL (Required)
// isPartialLoad - Indicates if the page will be load to a HTML tag (Optional)
// div - HTML tags where partival view will be loaded (Required if isPartialLoad is true)
function ReturnConfirmation(header, body, destinationURL, isPartialLoad, div) {
    var button = "";

    button = "<button type=\"button\" id=\"btnConfirmationModal\" class=\"btnBlue formbtn\" onclick=\"ReturnPage('" + destinationURL + "', '" + isPartialLoad + "', '" + div + "');\">Yes</button>&nbsp;";
    button += "&nbsp;";
    button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnBlue formbtn\" data-dismiss=\"modal\">No</button>";

    $("#divHeaderModal").html(header);
    $("#divBodyModal").html(body);
    $("#divFooterModal").html(button);
    $("#divModal").modal({
        backdrop: 'static',
        keyboard: false,
    });
}

// Execute Confirmation modal command
function ReturnPage(destinationURL, isPartialLoad, div) {
    window.IsTransactionPage = false;
    $("#btnConfirmationModal").prop("disabled", true);
    $("#divModal").modal("hide");
    if (isPartialLoad == "true" || isPartialLoad == true)
        LoadPartial(destinationURL, div);
    else
        window.location = destinationURL;
}

// Upload using modal
// url - Upload URL (Optional)
// header - Header title of the modal (Optional)
// sampleFormURL - Form where to get the URL (Optional)
// customFunction - Local Function to execute (Optional but Required if url parameter is not provided). If this is provided you have to create your own upload function
function ModalUpload(url, header, sampleFormURL, customFunction) {
    var button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnBlue formbtn\" onclick=\"Upload('" + url + "');\">Upload</button>";
    var sampleFormLink = "";

    if (customFunction != "")
        button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnBlue formbtn\" onclick=\"" + customFunction + ";\">Upload</button>";


    if (sampleFormURL != "")
        sampleFormLink = "<a href=\"" + sampleFormURL + "\">Click to Download Form</a>";

    if (header != "")
        $("#pheader").html(header);

    button += "&nbsp <button type=\"button\" id=\"btnUploadFileClose\" class=\"btnBlue formbtn\" data-dismiss=\"modal\">Cancel</button>";

    $("#fileUpload").val("");
    $("#divUploadForm").html(sampleFormLink);
    $("#divFooterUploadModal").html(button);
    $("#divUploadModal").modal("show");
}

// Save Uploaded File
function Upload(url) {
    $("#btnUploadFile").prop("disabled", true);

    var file;
    file = $("#fileUpload")[0].files[0];

    $.ajax({
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Cache-Control", "no-cache");
            xhr.setRequestHeader("X-File-Name", file.name);
            xhr.setRequestHeader("X-File-Size", file.size);
            xhr.setRequestHeader("Content-Type", "multipart/form-data");
        },
        type: "POST",
        url: url,
        processData: false,
        cache: false,
        data: file,
        success: function (data, textStatus, xhr) {
            $("#divUploadModal").modal("hide");

            var msg = "";
            if (data.IsListResult) {
                for (var i = 0; i < data.Result.length; i++) {
                    msg += data.Result[i] + "<br />";
                }
            } else {
                msg += data.Result;
            }

            if (data.IsSuccess) {
                ModalAlert(MODAL_HEADER, msg);
            } else {
                ModalAlert(MODAL_HEADER, msg);
            }

            $("#btnUpload").prop("disabled", false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#divUploadModal").modal("hide");
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
            $("#btnUpload").prop("disabled", false);
        }
    });
}

// Hide Upload Modal
function HideUploadModal() {
    $("#fileUpload").val("");
    $("#divUploadForm").html("");
    $("#divFooterUploadModal").html("");
    $("#divUploadModal").modal("hide");
}

// Load partial view into a div
// url - URL Page to load (Required)
// div - HTML tags where partival view will be loaded (Required)
// urlRouteForError - URL page to be assign on the button to load if error is encountered (Optional)
function LoadPartial(url, div, urlRouteForError) {
    Loading(true);

    if (urlRouteForError != undefined && urlRouteForError != "")
        $("#" + div + "").html("");

    //Clear div contents
    $("#" + div + "").html("");

    $.ajax({
        url: url,
        success: function (data) {
            if (data.IsSuccess == undefined) {
                $("#" + div + "").html(data);
            }
            else {
                var msg = "";
                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }
                ModalAlert(MODAL_HEADER, msg);

                if (urlRouteForError != undefined && urlRouteForError != "")
                    $("#" + div + "").html("<button type=\"button\" class=\"btnBlue formbtn\" onclick=\"LoadPartial('" + urlRouteForError + "','" + div + "')\">Back</button>");
            }

            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, jqXHR.responseText);

            if (urlRouteForError != undefined && urlRouteForError != "")
                $("#" + div + "").html("<button type=\"button\" class=\"btnBlue formbtn\" onclick=\"LoadPartial('" + urlRouteForError + "','" + div + "')\">Back</button>");

            //ModalAlert("Session Expired", "Please try to Login again.");
            //window.location = window.loginPageURL;
        }
    });

    return false;
}

// Load partial view into a div with Function Parameter
// url - URL Page to load (Required)
// div - HTML tags where partival view will be loaded (Required)
// urlRouteForError - URL page to be assign on the button to load if error is encountered (Optional)
function LoadPartialSuccessFunction(url, div, isSuccessFunction, urlRouteForError) {
    Loading(true);

    if (urlRouteForError != undefined && urlRouteForError != "")
        $("#" + div + "").html("");

    //Clear div contents
    $("#" + div + "").html("");
    $.ajax({
        url: url,
        success: function (data) {
            if (data.IsSuccess == undefined) {
                $("#" + div + "").html(data);
                if (isSuccessFunction != null)
                    isSuccessFunction();
            }
            else {
                var msg = "";
                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }
                ModalAlert(MODAL_HEADER, msg);

                if (urlRouteForError != undefined && urlRouteForError != "")
                    $("#" + div + "").html("<button type=\"button\" class=\"btnBlue formbtn\" onclick=\"LoadPartial('" + urlRouteForError + "','" + div + "')\">Back</button>");
            }

            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, jqXHR.responseText);

            if (urlRouteForError != undefined && urlRouteForError != "")
                $("#" + div + "").html("<button type=\"button\" class=\"btnBlue formbtn\" onclick=\"LoadPartial('" + urlRouteForError + "','" + div + "')\">Back</button>");

        }
    });

    return false;
}

// Load page view into a div
// url - URL Page to load (Required)
// type - Indicate if POST or GET (Required)
// param - Parameters to be pass (Optional)
// traditional - Indicate if the parameter would be treated traditional or not (Optional) | Boolean
// div - HTML tags where partival view will be loaded (Required)
// urlRouteForError - URL page to be assign on the button to load if error is encountered (Optional)
function LoadPage(url, type, param, traditional, div, urlRouteForError) {
    if (param == undefined)
        param = "";

    if (traditional == undefined)
        param = false;

    Loading(true);

    $.ajax({
        url: url,
        type: type,
        data: param,
        traditional: traditional,
        success: function (data) {
            if (data.IsSuccess == undefined) {
                $("#" + div + "").html(data);
            }
            else {
                var msg = "";
                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }
                ModalAlert(MODAL_HEADER, msg);

                $("#" + div + "").html("<button type=\"button\" class=\"btnBlue formbtn\" onclick=\"LoadPartial('" + urlRouteForError + "','" + div + "')\">Back</button>");
            }

            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
        }
    });

    return false;
}

// Generate Dropdown
// url - URL where to get the data that will populate the dropdown (Required)
// ddlId - Id of the select tag (Required)
// valueColumn - Property name of the result list that would be use as a value on the option (Required)
// displayColumn - Property name of the result list that would be use as a display on the option (Required)
// defaulValue - Default value of the select (Optional)
// isFilter - To determine if the select is will be used on Form or Filter (Required) | Boolean
function GenerateDropdownValues(url, ddlId, valueColumn, displayColumn, defaulValue, defaulValueDisplay, isFilter) {
    $.ajax({
        url: url,
        type: "GET",
        data: "",
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var values = "";

            $("#" + ddlId + "").empty();

            if (isFilter) {
                $("#" + ddlId + "").append($('<option/>', {
                    value: defaulValue,
                    text: "- " + (defaulValueDisplay == "" ? "Any" : defaulValueDisplay) + " -"
                }));
            }
            else {
                $("#" + ddlId + "").append($('<option/>', {
                    value: defaulValue,
                    text: "- Select an " + (defaulValueDisplay == "" ? "Item" : defaulValueDisplay) + " -"
                }));
            }

            if (data.IsSuccess) {
                for (var i = 0; i < data.Result.length; i++) {
                    $("#" + ddlId + "").append($('<option/>', {
                        value: data.Result[i][valueColumn],
                        text: data.Result[i][displayColumn]
                    }));
                }
            } else {
                var msg = "";

                if (data.IsListResult) {
                    if (data.Result != null && data.Result != undefined) {
                        for (var i = 0; i < data.Result.length; i++) {
                            msg += data.Result[i] + "<br />";
                        }
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert(MODAL_HEADER, msg);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
        },
    });
}

// Generate Dropdown
// url - URL where to get the data that will populate the dropdown (Required)
// param - Paramenter
// ddlId - Id of the select tag (Required)
// valueColumn - Property name of the result list that would be use as a value on the option (Required)
// displayColumn - Property name of the result list that would be use as a display on the option (Required)
// defaulValue - Default value of the select (Optional)
// isFilter - To determine if the select is will be used on Form or Filter (Required) | Boolean
function GenerateDropdownValuesWithParam(url, param, ddlId, valueColumn, displayColumn, defaulValue, defaulValueDisplay, isFilter) {
    $.ajax({
        url: url,
        type: "GET",
        data: param,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var values = "";

            $("#" + ddlId + "").empty();

            if (isFilter) {
                $("#" + ddlId + "").append($('<option/>', {
                    value: defaulValue,
                    text: "- " + (defaulValueDisplay == "" ? "Any" : defaulValueDisplay) + " -"
                }));
            }
            else {
                $("#" + ddlId + "").append($('<option/>', {
                    value: defaulValue,
                    text: "- Select an " + (defaulValueDisplay == "" ? "Item" : defaulValueDisplay) + " -"
                }));
            }

            if (data.IsSuccess) {
                for (var i = 0; i < data.Result.length; i++) {
                    $("#" + ddlId + "").append($('<option/>', {
                        value: data.Result[i][valueColumn],
                        text: data.Result[i][displayColumn]
                    }));
                }
            } else {
                var msg = "";

                if (data.IsListResult) {
                    if (data.Result != null && data.Result != undefined) {
                        for (var i = 0; i < data.Result.length; i++) {
                            msg += data.Result[i] + "<br />";
                        }
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert(MODAL_HEADER, msg);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
        },
    });
}

// Download Report File (Look Tax Exemption file export for sample)
// url - Link where to download file (Required)
// param - Paremeter of link for downloading file (optional)
function DownloadReportFile(url, param) {

    Loading(true);
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {
                window.open(window.downloadReportURL + "?id=" + data.Result);

            }
            else {
                var msg = "";
                if (data.IsListResult == true) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                HideExportModal();
                ModalAlert(MODAL_HEADER, msg);
            }

            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HideExportModal();
            Loading(false);
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
        },
    });
}

// Download Text File 
// url - Link where to download file (Required)
// param - Parameter of link for downloading file (optional)
function DownloadTextFile(url, param) {
    Loading(true);

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {
                window.open(window.downloadTextFileURL + "?id=" + data.Result);
            }
            else {
                var msg = "";
                if (data.IsListResult == true) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                HideExportModal();
                ModalAlert(MODAL_HEADER, msg);
            }

            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HideExportModal();
            Loading(false);
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
        },
    });
}

// Build a table
// tableHeader - List of header columns of the table (Required)
// tableColumn - List of Column name or object name (Required)
// tableData - List of values to display (Required)
// Note: tableHeader and tableColumn must have the same length
function TableBuilder(tableHeader, tableColumn, tableData) {
    var headerLength = tableHeader.length;

    var result = "<table id=\"tblGenerated\">";

    result += "<thead>";
    result += "<tr>";
    for (var i = 0; i < headerLength; i++) {
        result += "<td>" + tableHeader[i] + "</td>";
    }
    result += "</tr>";
    result += "</thead>";

    result += "<tbody>";
    for (var i = 0; i < tableData.length; i++) {
        result += "<tr>";
        for (var j = 0; j < headerLength; j++) {
            result += "<td>" + tableData[i][tableColumn[j]] + "</td>";
        }
        result += "</tr>";
    }
    result += "</tbody>";

    result += "</table>";

    return result;
}

// Create table to be use for JQGrid
// Note: You can only initialize it once on the page
function InitializeTableForJQGrid() {
    var result = "<table id=\"tblJQGrid\" class=\"table\">";
    result += "</table><div id=\"divJQGridPager\"></div>";
    return result;
}

// Covert the InitializeTableForJQGrid to JQGrid
// tableHeader - List of header columns of the table (Required)
// tableColumn - List of Column name or object name (Required)
// tableData - List of values to display (Required)
// Note: tableHeader and tableColumn must have the same length
function ConvertTableBuilderToJQGrid(tableHeader, tableColumn, tableData) {
    $("#tblJQGrid").jqGrid("GridUnload");
    $("#tblJQGrid").jqGrid("GridDestroy");
    $("#tblJQGrid").jqGrid({
        data: tableData,
        datatype: "local",
        colNames: tableHeader,
        colModel: tableColumn,
        toppager: $("#divJQGridPager"),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        loadonce: true,
        viewrecords: true,
        emptyrecords: "No records to display",
        rowNumbers: true,
        width: "100%",
        height: "100%",
        sortable: true,
    }).navGrid("#divJQGridPager",
        { edit: false, add: false, del: false, search: false, refresh: false }
    );
}

// Convert the table build by the TableBuilder function to DataTable
// Note: You must TableBuilder function first before this 
// columnToFreeze - No of column to freeze on the left (Optional) (Don't forget to include the dataTables.fixedColumns extention)
function ConvertTableBuilderToDataTable(columnToFreeze) {

    var table = $("#tblGenerated").DataTable({
        bFilter: false,
        bInfo: false,
        bAutoWidth: false,
        sScrollX: true,
        sScrollY: true,
    });

    if (columnToFreeze > 0) {
        //new $.fn.dataTable.FixedColumns(table, {
        //    leftColumns: columnToFreeze,
        //});        
    }
}

// To show the export modal
function ShowExportModal() {
    $("#divExportModal").modal("show");
}

// To hide export modal
function HideExportModal() {
    $("#divExportModal").modal("hide");
}

// Generate Listbox values
// url - URL where to get the data that will populate the dropdown (Required)
// ddlId - Id of the select tag (Required)
// param - Parameter
// valueColumn - Property name of the result list that would be use as a value on the option (Required)
// displayColumn - Property name of the result list that would be use as a display on the option (Required)
// ddlFilterId - Id of the 2nd select tag, if the valueColumn of ddlId exist on ddlFilterId value it will not be shown (Optional)
function ListBoxGenerateValues(url, param, ddlId, valueColumn, displayColumn, ddlFilterId) {
    Loading(true);
    $.ajax({
        url: url,
        type: "GET",
        data: JSON.stringify(param),
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {
                $("#" + ddlId + "").empty();
                $("#" + ddlId + "").prop("multiple", "multiple");
                for (var i = 0; i < data.Result.length; i++) {
                    if ($("#" + ddlFilterId + " option[value='" + data.Result[i][valueColumn] + "']").length == 0) {
                        $("#" + ddlId + "").append($('<option/>', {
                            value: data.Result[i][valueColumn],
                            text: data.Result[i][displayColumn]
                        }));
                    }
                }
            } else {
                var msg = "";

                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert(MODAL_HEADER, msg);
            }
            Loading(false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Loading(false);
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
        },
    });
}

function ListBoxGenerateValues2(url, param, ddlId, valueColumn, displayColumn, button1, button2, button3, button4) {
    $.ajax({
        url: url,
        type: "GET",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {
                $("#" + ddlId + "").empty();
                $("#" + ddlId + "").prop("multiple", "multiple");
                for (var i = 0; i < data.Result.length; i++) {
                    $("#" + ddlId + "").append($('<option/>', {
                        value: data.Result[i][valueColumn],
                        text: data.Result[i][displayColumn]
                    }));
                }
            } else {
                var msg = "";

                if (data.IsListResult) {
                    for (var i = 0; i < data.Result.length; i++) {
                        msg += data.Result[i] + "<br />";
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert(MODAL_HEADER, msg);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
        },
        beforeSend: function () {
            $("#" + button1 + "").prop("disabled", true);
            $("#" + button2 + "").prop("disabled", true);
            $("#" + button3 + "").prop("disabled", true);
            $("#" + button4 + "").prop("disabled", true);
        },
        complete: function () {
            $("#" + button1 + "").prop("disabled", false);
            $("#" + button2 + "").prop("disabled", false);
            $("#" + button3 + "").prop("disabled", false);
            $("#" + button4 + "").prop("disabled", false);
        },
    });
}

// Move list box values from one to another
// sourceId - Id of the source list box (Required)
// destinationId - Id of the list box that will received the values (Required)
// isAll - Identifies if the values would be transfer is selected only or all values | Boolean (Optional)
// isSort - Identifies if the values would be sorted | Boolean (Optional)
function ListBoxMove(sourceId, destinationId, isAll, isSort) {
    if (isAll == undefined)
        isAll = false;

    $("#" + sourceId + (isAll ? " option" : " :selected") + "").each(function (i, value) {
        $("#" + destinationId + "").append($("<option>", {
            value: value.value,
            text: value.text
        }));

        $(this).remove();
    });

    if (isSort) {
        $("#" + destinationId + "").append($("#" + destinationId + " option").remove().sort(function (a, b) {
            return ($(a).text() > $(b).text()) ? 1 : (($(a).text() < $(b).text()) ? -1 : 0);
        }));
    }
}

// Search on the list box values
// id - Id of the list box to be searched (Required)
// searchValue - Value to be searched (Required)
// isCS - Identifies if the value to be search is case sensitive or not | Boolean (Optional)
function ListBoxSearch(id, searchValue, isCS) {
    searchValue = $.trim("" + searchValue);

    if (isCS == undefined) {
        isCS = false;
    }

    if (!isCS) {
        searchValue = searchValue.toUpperCase();

        // Overried the 'contains' function of jquery
        $.expr[":"].contains = $.expr.createPseudo(function (arg) {
            return function (elem) {
                return $(elem).text().toUpperCase().indexOf(arg.toUpperCase()) >= 0;
            };
        });
    }

    if (searchValue != "") {
        $("#" + id + " option:not(:contains('" + searchValue + "'))").hide();
        $("#" + id + " option:contains('" + searchValue + "')").show();
    } else {
        $("#" + id + " option").show();
    }
}

// Got to page
// link - URL where to go
function GoToPage(link) {
    window.location = link;
}

// Show the loading
// show - Indicates if the loading overlay will show | Boolean
function Loading(show) {
    //if (show == true)
    //    show = "show";
    //else if (show == false)
    //    show = "hide";
    //else
    //    show = "hide";

    //$("body").plainOverlay(show);

/*Motortrade Loading screen*/
    if (show) {
        $("#loading").show();
    }
    else if (!show) {
        $("#loading").hide();
        // $("body").css({ "overflow-y": "auto"});
    }


}

//Change the character into UPPERCASE
function SetToUpper(fieldID) {
    $("#" + fieldID).bind('keyup', function (e) {
        if (e.which >= 97 && e.which <= 122) {
            var newKey = e.which - 32;

            e.keyCode = newKey;
            e.charCode = newKey;
        }

        $("#" + fieldID).val(($("#" + fieldID).val()).toUpperCase());
    });
}

function SinglePeriodOnly(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^[0-9]*\.?[0-9]*$/;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9 ||
            ((e.keyCode == 110 || e.keyCode == 190) && $(txtbox).val().split('.').length === 2)
        ) {
            e.preventDefault();
        }
    });
}

/*
    Returning 0.00 when amount is empty for blur
*/

function AmountFieldBlur(txtbox) {
    txtbox.blur(function () {
        $(this).val($(this).val().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        FormatZeroAmount(txtbox);
    });
}

function FormatZeroAmount(textbox) {
    if (textbox.val() != "")
        textbox.val(AddZeroes(textbox.val()).withComma());
    else
        textbox.val("0.00");
}

function GetCurrentDate(fieldName, url) {
    $.ajax({
        url: url,
        type: "POST",
        data: "",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.IsSuccess) {

                $("#" + fieldName + "").text(data.Result);
            } else {
                var msg = "";

                if (data.IsListResult) {
                    if (data.Result != null && data.Result != undefined) {
                        for (var i = 0; i < data.Result.length; i++) {
                            msg += data.Result[i] + "<br />";
                        }
                    }
                } else {
                    msg += data.Result;
                }

                ModalAlert(MODAL_HEADER, msg);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ModalAlert(MODAL_HEADER, jqXHR.responseText);
        },
    });
}

function AlphanumericKeydown(textbox) {
    textbox.keydown(function (e) {
        var reg = /^[a-zA-Z0-9]*$/;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9
        ) {
            e.preventDefault();
        }
    });
}

function AlphanumericValid(textbox, fieldName) {
    var value = textbox.val();
    var regValidation0 = /^[a-zA-Z][0-9a-zA-Z]*$/;
    var regValidation1 = /^(?=.*\d)(?=.*[a-z])[a-zA-Z0-9]*$/;
    var isValid = false;
    var msg = '';
    if (value != '') {
        if (("" + regValidation0.test(value)) == "false") {
            msg = "First character must not be numeric for the field: " + fieldName + ".";
        }
        else {
            if (("" + regValidation1.test(value)) == "false") {
                msg = fieldName + " must have an alphanumeric value.";
            }
        }

    }

    if (msg.length > 0) {
        $("#divErrorMessage").html('');
        textbox.css({ 'border-color': '#c40000' });
        $('#btnSave').prop("disabled", true);

        $("#divErrorMessage").append("<label class='errMessage'><li>" + msg + "</li></label>");
        $("#divErrorMessage").append('<br />');
        isValid = false;
    }
    else {
        textbox.css({ 'border-color': '#d2d1d1' });
        $('#btnSave').prop("disabled", false);
        $("#divErrorMessage").html('');
        isValid = true;
    }

    return isValid;
}

//Number  of items to show in JQGrid
function SetRowList() {
    var TABLE_SHOW_LIST = [10, 20, 50, 100];
    return TABLE_SHOW_LIST;
}

function OneClickSelect(fieldName) {
    $(fieldName).mousedown(function (e) {
        e.preventDefault();

        var select = this;
        var scroll = select.scrollTop;

        e.target.selected = !e.target.selected;

        setTimeout(function () { select.scrollTop = scroll; }, 0);

        $(select).focus();
    }).mousemove(function (e) { e.preventDefault() });
}

function DoubleClickDeselectAll(fieldName, result) {

    $(fieldName).dblclick(function () {
        $(fieldName + " option:selected").prop("selected", false);
    });
}//Saving JQGrid parameters to localStorage

//table - JQGrid id
function GetJQGridState(table) {
    var tableInfo = new Object();
    tableInfo.url = $("#" + table).jqGrid('getGridParam', 'url');
    tableInfo.sortname = $("#" + table).jqGrid('getGridParam', 'sortname');
    tableInfo.sortorder = $("#" + table).jqGrid('getGridParam', 'sortorder');
    //tableInfo.selrow = $("#" + table).jqGrid('getGridParam', 'selrow');
    tableInfo.page = $("#" + table).jqGrid('getGridParam', 'page');
    tableInfo.rowNum = $("#" + table).jqGrid('getGridParam', 'rowNum');
    tableInfo.postData = $("#" + table).jqGrid('getGridParam', 'postData');
    localStorage.setItem(table, JSON.stringify(tableInfo));
}

//Reset JQGrid parameters from localStorage
//table - JQGrid id
function ResetJQGridState(table) {
    var tableInfo = new Object();
    tableInfo.url = "";
    tableInfo.sortname = "";
    tableInfo.sortorder = "";
    //tableInfo.selrow = "";
    tableInfo.page = 1;
    tableInfo.rowNum = 10;
    tableInfo.postData = "";
    localStorage.setItem(table, JSON.stringify(tableInfo));
}

//Set width size of columns depending on max size of its content
//table - JQGrid id
//data - JQGrid data
function AutoSizeColumnJQGrid(table, data) {
    var columnMultiplier = 8.2; // Size per character.
    var maxCharArr = [];
    var ctr = 0;

     getMax = function(length, max) { return length >= max ? length : max; };

    // Get all visible column headers text size.
    var columnModels = $("#" + table).jqGrid('getGridParam', 'colModel');
    for (var index in columnModels) {
        if (!columnModels[index].hidden) {
            maxCharArr[ctr] = $("#" + table + "_" + columnModels[index].name).text().trim().length;
            ctr++;
        }
    }
    ctr = 0;
    // Loop through all data and get the largest content size then set the width of the column.
    for (var index in columnModels) {
        if (!columnModels[index].hidden & columnModels[index].name != "") {
            for (var i = 0; i < data.rows.length; i++) {
                maxCharArr[ctr] = getMax((data.rows[i][columnModels[index].name] + "").trim().length, maxCharArr[ctr]);
            }
            var maxWidth = (maxCharArr[ctr] * columnMultiplier);
            // Add additional 10 padding for columns with width 100 or less.
            maxWidth = maxWidth <= 100 ? maxWidth + 50 : maxWidth;
            // Set 40 pixels for checkboxes.
            maxWidth = columnModels[index].name == "cb" ? 40 : maxWidth; 

            $("#" + table + "_" + columnModels[index].name).css("width", maxWidth + "px");

            if (data.rows.length) {
                $("#" + table + " tr")
                    .find("td:eq(" + ($("[aria-describedby='" + table + "_" + columnModels[index].name + "']").index()) + ")")
                    .each(function () { $(this).css("width", maxWidth + "px"); });
            }
            else {
                $("#" + table + " tr.jqgfirstrow")
                    .find("td:eq(" + ($("[id='" + table + "_" + columnModels[index].name + "']").index()) + ")")
                    .each(function () { $(this).css("width", maxWidth + "px"); });
            }

            ctr++;
        }
    }
}

function DecimalTwoPlaces(txtbox) {
    txtbox.keydown(function (e) {
        var reg = /^[0-9]*\.?[0-9]*$/;
        if (("" + reg.test(/*e.key --this syntax first supports chrome 51 and mozilla 23*/ String.fromCharCode(e.keyCode))) == "false" && e.keyCode != 8
            && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 9 ||
            ((e.keyCode == 110 || e.keyCode == 190) && $(txtbox).val().split('.').length === 2)
        ) {
            e.preventDefault();
        }
    });
}

function GetCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function SetClass() {
    //before assigning class check local storage if it has any value
    $('.toggle-arrow').addClass(sessionStorage.getItem('toggleClass'));
    $('.system-side-menu').addClass(sessionStorage.getItem('menuClass'));
    $('.system-body-container').addClass(sessionStorage.getItem('containerClass'));
}

/*Toggle Side Nav Bar*/
$(document).ready(function () {

    SetClass();

    $('.toggle-arrow').css('background-color', $('.menu-item.first-level').css('background-color'));
    $('.toggle-arrow.toggled').css('left', $('.system-side-menu').width() + 8);
    $('.system-side-menu.showMenu').css('left', '10px').css('opacity', '1');
    $('.system-body-container.resize').css('margin-left', $('.system-side-menu').width() + 10);
    $('.toggle-arrow').css('transition', 'none');
    $('.toggle-arrow span').css('transition', 'none');
    $('.system-side-menu').css('transition', 'none');
    $('.system-body-container').css('transition', 'none');

    $('.toggle-arrow').on('click', function () {
        $(this).css('transition', '.5s ease-in-out');
        $('.toggle-arrow span').css('transition', '.5s ease-in-out');
        $('.system-side-menu').css('transition', '.5s ease-in-out');
        $('.system-body-container').css('transition', '.5s ease-in-out');

        $(this).toggleClass('toggled');
        if ($(this).hasClass('toggled')) {
            $(this).css('left', $('.system-side-menu').width() + 8);
            sessionStorage.setItem('toggleClass', 'toggled');
        }
        else {
            $(this).css('left', '0');
            sessionStorage.setItem('toggleClass', 'stay');
        }

        $('.system-side-menu').toggleClass('showMenu');
        if ($('.system-side-menu').hasClass('showMenu')) {
            $('.system-side-menu').css('left', '10px').css('opacity', '1');
            sessionStorage.setItem('menuClass', 'showMenu');
        }
        else {
            $('.system-side-menu').css('left', '-170px').css('opacity', '0');
            sessionStorage.setItem('menuClass', 'stay');
        }

        $('.system-body-container').toggleClass('resize');
        if ($('.system-body-container').hasClass('resize')) {
            $('.system-body-container').css('margin-left', $('.system-side-menu').width() + 10);
            sessionStorage.setItem('containerClass', 'resize');
        }
        else {
            $('.system-body-container').css('margin-left', '0');
            sessionStorage.setItem('containerClass', 'stay');
        }


    });

    $(window).resize(function () {
        if ($('.toggle-arrow').hasClass('toggled')) {
            $('.toggle-arrow').css('left', $('.system-side-menu').width() + 8);
            $('.toggle-arrow').css('transition', 'none');
        }
        else {
            $('.toggle-arrow').css('left', '0');
        }

        if ($('.system-body-container').hasClass('resize')) {
            $('.system-body-container').css('margin-left', $('.system-side-menu').width() + 10);
            $('.system-body-container').css('transition', 'none');
        }
        else {
            $('.system-body-container').css('margin-left', '0');
        }
    });

});