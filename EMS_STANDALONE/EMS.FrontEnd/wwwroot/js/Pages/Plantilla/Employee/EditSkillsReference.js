var objEditSkillsReferenceJS;

$(document).ready(function () {
    objEditSkillsReferenceJS = {

        ID: $("#divSkillsReferenceModal #hdnID").val(),
        ReferenceCode: $("#divSkillsReferenceModal #hdnSkillsReferenceCode").val(),

        Initialize: function () {
            $("#divSkillsReferenceBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            objAddSkillsReferenceJS.LoadReferenceListJQGrid({
                RefCode: s.ReferenceCode
            });
        },

        ElementBinding: function () {
            var s = this;

            $("#divSkillsReferenceModal #btnSaveReference").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmAddReference", "#divReferenceErrorMessage", objEditSkillsReferenceJS.ValidateDuplicateFields)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SkillsReferenceEditPostURL + objEditSkillsReferenceJS.ReferenceCode \
                        , new FormData($('#frmAddReference').get(0)) \
                        , '#divReferenceErrorMessage' \
                        , '#btnSaveReference' \
                        , objEditSkillsReferenceJS.SaveSuccessFunction); ", "function");
                }
            });

        },

        SaveSuccessFunction: function () {
            var field = $("#divSkillsReferenceModal #hdnSkillsReferenceField").val();
            $("#" + field).click();
        },
    };

    objEditSkillsReferenceJS.Initialize();
});