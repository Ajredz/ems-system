var objPendingTaskViewJS;

$(document).ready(function () {
    objPendingTaskViewJS = {

        ID: $("#hdnID").val(),
        ApplicantID: $("#hdnApplicantID").val(),

        Initialize: function () {
            $("#divPendingTaskBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divPendingTaskModal .form-control").prop("disabled", true);
            s.LoadApplicant();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divPendingTaskModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , PendingTaskDeleteURL + '?ID=' + objPendingTaskViewJS.ID\
                    , {} \
                    , '#divPendingTaskErrorMessage' \
                    , '#btnDelete' \
                    , objPendingTaskViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#frmPendingTask #btnEdit").click(function () {
                LoadPartial(PendingTaskEditURL + "?ID=" + objPendingTaskViewJS.ID, "divPendingTaskBodyModal");
            });
        },

        LoadApplicant: function () {
            var GetSuccessFunction = function (data) {
                $("#txtApplicant").val(data.Result.Description);
            };

            objEMSCommonJS.GetAjax(GetApplicantURL + "&ID=" + objPendingTaskViewJS.ApplicantID, {}, "", GetSuccessFunction);
        },

    };
    
     objPendingTaskViewJS.Initialize();
});