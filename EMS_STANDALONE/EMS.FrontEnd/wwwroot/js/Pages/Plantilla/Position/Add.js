var objPositionAddJS;

$(document).ready(function () {
    objPositionAddJS = {

        Initialize: function () {
            $("#divPositionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divPositionModal #btnSave").show();
            $("#divPositionModal .form-control").attr("readonly", false);
            $("#divPositionModal #btnDelete, #divPositionModal #btnBack").remove();

            $('#SummerNoteJobDescription').summernote({
                toolbar: [
                    // [groupName, [list of button]]
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'style', 'paragraph']],
                    ['height', ['height']]
                ]
            });
            $('#SummerNoteJobQualification').summernote({
                toolbar: [
                    // [groupName, [list of button]]
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph', 'style']],
                    ['height', ['height']]
                ]
            });
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmPosition").trigger("reset");
        },

        ElementBinding: function () {
            var s = this;
            Code($("#txtCode"));
            PreventSpace($("#txtCode"));
            $('#txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });

            $("#txtCode").keyup(function () {
                $("#txtCode").val($("#txtCode").val().toUpperCase());
            });
            $("#txtJobPosition").keyup(function () {
                $("#txtJobPosition").val($("#txtJobPosition").val().toUpperCase());
            });
            $("#txtJobLocation").keyup(function () {
                $("#txtJobLocation").val($("#txtJobLocation").val().toUpperCase());
            });
            $("#divPositionModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmPosition", "#divPositionErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , PositionAddPostURL \
                        , new FormData($('#frmPosition').get(0)) \
                        , '#divPositionErrorMessage' \
                        , '#divPositionModal #btnSave' \
                        , objPositionAddJS.AddSuccessFunction);",
                        "function");
                }
            });
        },

    };

    objPositionAddJS.Initialize();
});