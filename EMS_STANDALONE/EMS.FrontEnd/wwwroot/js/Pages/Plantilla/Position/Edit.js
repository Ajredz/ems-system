var objPositionEditJS;

$(document).ready(function () {
    objPositionEditJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divPositionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass(".unreqField");
            $(".reqField").removeClass(".reqField");
            $("#btnEdit").hide();
            $("#btnSave, #btnBack").show();
            $("#divPositionModal .form-control").attr("readonly", false);

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

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divPositionModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
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
            $("#ddlOrgGroup").change(function () {
                if ($("#ddlOrgGroup").val() == "" /*On Hold*/) {
                    $("#hdnOrgGroupID").val(0);
                }
                else {
                    $("#hdnOrgGroupID").val($("#ddlOrgGroup").val());
                }

            });

            $("#ddlPosition").change(function () {
                if ($("#ddlPosition").val() == "" /*On Hold*/) {
                    $("#hdnPositionLevelID").val(0);
                }
                else {
                    $("#hdnPositionLevelID").val($("#ddlPosition").val());
                }

            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , PositionDeleteURL + '?ID=' + objPositionEditJS.ID \
                    , {} \
                    , '#divPositionErrorMessage' \
                    , '#btnDelete' \
                    , objPositionEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmPosition", "#divPositionErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , PositionEditPostURL \
                        , new FormData($('#frmPosition').get(0)) \
                        , '#divPositionErrorMessage' \
                        , '#btnSave' \
                        , objPositionEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(PositionViewURL + "?ID=" + objPositionEditJS.ID, "divPositionBodyModal");
            });

        },

    };

    objPositionEditJS.Initialize();
});