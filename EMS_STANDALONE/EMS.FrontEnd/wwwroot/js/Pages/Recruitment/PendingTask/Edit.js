var objPendingTaskEditJS;

$(document).ready(function () {
    objPendingTaskEditJS = {

        ID: $("#hdnID").val(),
        ApplicantID: $("#hdnApplicantID").val(),

        Initialize: function () {
            $("#divPendingTaskBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#btnSave").show();
            $("#divPendingTaskModal #ddlStatus, #divPendingTaskModal #txtRemarks").attr("readonly", false);
            $("#btnEdit").hide();
            $("#btnSave, #btnBack").show();
            s.LoadApplicant();
        },

        DeleteSuccessFunction: function () {
          $("#btnSearch").click();
          $('#divPendingTaskModal').modal('hide');
        },

        EditSuccessFunction: function () {
          $("#btnSearch").click();
          $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;

            $("#divPendingTaskModal #btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                      "objEMSCommonJS.PostAjax(false \
                      , PendingTaskDeleteURL + '?ID=' + objPendingTaskEditJS.ID\
                      , {} \
                      , '#divPendingTaskErrorMessage' \
                      , '#btnDelete' \
                      , objPendingTaskEditJS.DeleteSuccessFunction);",
                      "function");
            });

            $("#divPendingTaskModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmPendingTask", "#divPendingTaskErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                            , PendingTaskEditPostURL \
                            , new FormData($('#frmPendingTask').get(0)) \
                            , '#divPendingTaskErrorMessage' \
                            , '#divPendingTaskModal #btnSave' \
                            , objPendingTaskEditJS.EditSuccessFunction);", "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(PendingTaskViewURL + "?ID=" + objPendingTaskEditJS.ID, "divPendingTaskBodyModal");
            });
        },

        LoadApplicant: function () {
            var GetSuccessFunction = function (data) {
                $("#txtApplicant").val(data.Result.Description);
            };

            objEMSCommonJS.GetAjax(GetApplicantURL + "&ID=" + objPendingTaskViewJS.ApplicantID, {}, "", GetSuccessFunction);
        },

    };

    objPendingTaskEditJS.Initialize();
});