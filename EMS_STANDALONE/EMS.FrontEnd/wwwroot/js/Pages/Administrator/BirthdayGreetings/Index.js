var objIndexJS;
const BirthdateGreetingsPostURL = "/Administrator/BirthdayGreetings";
const SendEmailBirthdayURL = "/Administrator/BirthdayGreetings?handler=SendEmailBirthday";

$(document).ready(function () {
    objIndexJS = {
        Initialize: function () {
            var s = this;

            s.ElementBinding();
            $("#txtBody").on("summernote.enter", function (we, e) {
                $(this).summernote("pasteHTML", "<br>");
                e.preventDefault();
            });
            $('#txtBody').summernote({
                toolbar: [
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'style', 'paragraph']],
                    ['height', ['height']]
                ]
            });
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSave").on("click", function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmBirthdayGreetings", "#divBirthdayGreetingsErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , BirthdateGreetingsPostURL \
                        , objIndexJS.GetFormData() \
                        ,'#divBirthdayGreetingsErrorMessage' \
                        , '#btnSave' \
                        , objIndexJS.SuccessFunction);",
                        "function");
                }
            });

            $("#btnSendEmail").on("click", function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                        , SendEmailBirthdayURL \
                        , objIndexJS.GetFormData() \
                        ,'#divBirthdayGreetingsErrorMessage' \
                        , '#btnSave' \
                        , objIndexJS.SuccessFunction);",
                    "function");
            });
        },
        SuccessFunction: function () {
        },
        GetFormData: function () {
            var formData = new FormData($('#frmBirthdayGreetings').get(0));

            return formData;
        } 

    }
    objIndexJS.Initialize();
});
