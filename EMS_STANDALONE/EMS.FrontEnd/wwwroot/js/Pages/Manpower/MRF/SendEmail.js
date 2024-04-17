var objMRFSendEmailJS;

$(document).ready(function () {
    objMRFSendEmailJS = {

        Initialize: function () {
            $("#divSendEmailBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();

            //$('#txtBody').val($('#txtBody').val().replace("&lt;Name&gt;", $("#txtNameOnly").val()));

            $('#txtBody').summernote({
                toolbar: [
                    // [groupName, [list of button]]
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    //['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol']],
                    ['height', ['height']]
                ]
            });
        },

        ElementBinding: function () {
            var s = this;

            $("#btnCancel").click(function () {
                $("#divSendEmailModal").modal("hide");
            });

            $("#btnSend").click(function () {
                $("#divSendEmailErrorMessage").hide();
                $("#divSendEmailErrorMessage").html("");

                if ($("#txtBody").val().includes("&lt;Insert Date&gt;")) {
                    $("#divSendEmailErrorMessage").show();
                    $("#divSendEmailErrorMessage").html("<label class=\"errMessage\"><li>" + MSG_INPUT_DATE + "</li></label><br />");
                }
                else if ($("#txtBody").val().includes("&lt;Insert Time&gt;")) {
                    $("#divSendEmailErrorMessage").show();
                    $("#divSendEmailErrorMessage").html("<label class=\"errMessage\"><li>" + MSG_INPUT_TIME + "</li></label><br />");
                }
                else {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_EMAIL_INVITE,
                        "objEMSCommonJS.PostAjax(true \
                        , SendEmailURL \
                        , new FormData($('#frmSendEmail').get(0)) \
                        , '#divSendEmailErrorMessage' \
                        , '#divSendEmailModal #btnSave' \
                        , objMRFSendEmailJS.AddSuccessFunction); ",
                        "function");
                }
            });
        },

        AddSuccessFunction: function () {
            $("#divSendEmailModal").modal("hide");
        },
    };
    
    objMRFSendEmailJS.Initialize();
});