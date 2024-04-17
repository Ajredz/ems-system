var objLoginJS;
var PostURL = "/Login";
var ListURL = "/ClaimsInformationLists";
$(document).ready(function () {
    objLoginJS = {
        Initialize: function () {
            $(".form-control").keypress(function (e) {
                if (e.which == 13) {
                    $("#btnLogin").click();
                }
            });
            $("#txtUsername").focus();

            localStorage["OurAgreementResult"] = "false";

            /* FOR DEVELOPMENT ONLY */
            //$("#txtUsername").val("admin");
            //$("#txtPassword").val("");
            //setTimeout(function() { $("#btnLogin").click(); }, 500);
            /* FOR DEVELOPMENT ONLY */
        },
        Submit: function () {
            var s = this;
            if (s.ValidateCredentials()) {
                $("#btnLogin").prop("disabled", true);
                Loading(true);
                var input = new FormData($("#frmLogin").get(0));
                $.ajax({
                    url: PostURL,
                    type: "POST",
                    data: input,
                    dataType: "json",
                    headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        if (data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            /* FOR DEVELOPMENT ONLY */
                            localStorage["OurAgreementResult"] = "false";
                            //localStorage["OurAgreementResult"] = "true";
                            window.location = data.Result;
                            //window.location = "/plantilla/employee";
                            //window.location = "/ipm/employeescore";
                            /* FOR DEVELOPMENT ONLY */
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
                            $("#divErrorMessage").html(msg);
                        }
                        $("#btnLogin").prop("disabled", false);
                        Loading(false);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        Loading(false);
                        $("#btnLogin").prop("disabled", false);
                        $("#divErrorMessage").html(jqXHR.responseText);
                    },
                });
            }
        },
        ValidateCredentials: function (additionalFunction) {
            var isValid = true;
            var fields = $(".required-field");
            var blankFields = 0;
            $("#divErrorMessage").html("");
            $(".errMessage").removeClass("errMessage");
            fields.each(function (n, element) {
                if ($(this).val() == "") {
                    $(this).addClass("errMessage");
                    blankFields++;
                }
            });

            if (blankFields == 1) {
                $(".required-field.errMessage:first").focus();
                $("#divErrorMessage").html("<label class=\"errMessage\"><li>" + $(".required-field.errMessage")[0].title + SUFF_REQUIRED + "</li></label><br />");
                isValid = false;
            }
            else if (blankFields > 1) {
                $(".required-field.errMessage:first").focus();
                $("#divErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }

            return isValid;
        },
    };

    objLoginJS.Initialize();
});