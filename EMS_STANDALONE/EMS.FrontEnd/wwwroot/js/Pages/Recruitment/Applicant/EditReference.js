var objEditReferenceJS;

$(document).ready(function () {
    objEditReferenceJS = {

        ID: $("#divReferenceModal #hdnID").val(),
        ReferenceCode: $("#divReferenceModal #hdnReferenceCode").val(),

        Initialize: function () {
            $("#divReferenceBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            objAddReferenceJS.LoadReferenceListJQGrid({
                RefCode: s.ReferenceCode
            });
        },

        ElementBinding: function () {
            var s = this;

            $("#divReferenceModal #btnSaveReference").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmAddReference", "#divReferenceErrorMessage", objEditReferenceJS.ValidateDuplicateFields)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , ReferenceEditPostURL + objEditReferenceJS.ReferenceCode \
                        , new FormData($('#frmAddReference').get(0)) \
                        , '#divReferenceErrorMessage' \
                        , '#btnSaveReference' \
                        , objEditReferenceJS.SaveSuccessFunction); ", "function");
                }
            });

        },

        SaveSuccessFunction: function () {
            var field = $("#divReferenceModal #hdnReferenceField").val();
            $("#" + field).click();
        },
    };

    objEditReferenceJS.Initialize();
});