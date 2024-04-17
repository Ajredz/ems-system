var objApplicantTaggingEditJS;

$(document).ready(function () {
    objApplicantTaggingEditJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divApplicantTaggingBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#ddlDesiredPosition, #ddlDesiredOrgGroup, #txtReferredBy").attr("readonly", false);
            $("#btnSave, #btnBack").show();
        },

        EditSuccessFunction: function () {
          $("#btnSearch").click();
          $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;

            $("#divApplicantTaggingModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmApplicantTagging", "#divApplicantTaggingErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                            , ApplicantTaggingEditPostURL \
                            , new FormData($('#frmApplicantTagging').get(0)) \
                            , '#divApplicantTaggingErrorMessage' \
                            , '#divApplicantTaggingModal #btnSave' \
                            , objApplicantTaggingEditJS.EditSuccessFunction);", "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(ApplicantTaggingViewURL + "?ID=" + objApplicantTaggingEditJS.ID, "divApplicantTaggingBodyModal");
            });
        },
    };

    objApplicantTaggingEditJS.Initialize();
});