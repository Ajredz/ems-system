var objForceChangePasswordJS;
var PostURL = "/ForceChangePassword";
var ListURL = "/ClaimsInformationLists";
$(document).ready(function () {
    objForceChangePasswordJS = {
        Initialize: function () {
            var s = this;
            $(".form-control").keypress(function (e) {
                if (e.which == 13) {
                    $("#btnSave").click();
                }
            });
            $("#txtNewPassword").focus();
            s.ElementBinding();

        },

        ElementBinding: function () {
            var s = this;
            $("#btnBack").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_TAB_BACK, 'window.location.href = \'/Login?handler=Logout\'', 'function');
            });

            $("#btnSave").click(function () {
                if (s.ValidateCredentials()) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                    "objForceChangePasswordJS.Submit()",
                    "function");
                }
            });
        },

        Submit: function () {
            var s = this;
            $("#btnSave").prop("disabled", true);
            Loading(true);
            var input = new FormData($("#frmManageAcountChangePassword").get(0));
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
                        ModalAlertRedirect(MODAL_HEADER, MSG_SUCCESSULLY_CHANGED_PASSWORD, '/Login?handler=Logout');
                        //window.location.href = '/Login?handler=Logout';
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
                    $("#btnCloseConfirmationModal").click();
                    $("#btnSave").prop("disabled", false);
                    Loading(false);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    $("#btnSave").prop("disabled", false);
                    $("#divErrorMessage").html(jqXHR.responseText);
                },
            });
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
            else {
                if ($("#txtNewPassword").val().length < 8 || $("#txtConfirmNewPassword").val() < 8) {
                    $("#divErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_MSG_NEW_PASS_MIN + "</li></label><br />");
                    isValid = false;
                }
                else if ($("#txtNewPassword").val() != $("#txtConfirmNewPassword").val()) {
                    $("#divErrorMessage").html("<label class=\"errMessage\"><li>New Password" + COMPARE_DOES_NOT_MATCH + "Confirm New Password.</li></label><br />");
                    isValid = false;
                }
            }

            return isValid;
        },
    };

    objForceChangePasswordJS.Initialize();
});