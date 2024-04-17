var objPendingTaskBatchEditJS;

$(document).ready(function () {
    objPendingTaskBatchEditJS = {

        Initialize: function () {
            $("#divPendingTaskBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
            $("#divPendingTaskModal").modal("hide");
        },

        ElementBinding: function () {
            var s = this;

            $("#frmBatchTask #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmBatchTask", "#divBatchTaskErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                            , BatchTaskEditPostURL \
                            , objPendingTaskBatchEditJS.GetFormData() \
                            , '#divBatchTaskErrorMessage' \
                            , '#divBatchTaskModal #btnSave' \
                            , objPendingTaskBatchEditJS.EditSuccessFunction);", "function");
                }
            });
        },

        GetFormData: function () {

            var input = $("#tblPendingTaskList").jqGrid("getGridParam", "selarrrow");
            var formData = new FormData($('#frmBatchTask').get(0));

            for (var x = 0; x < input.length; x++) {
               formData.append("BatchTask.IDs[" + x + "]", input[x]);
            }

            formData.append("BatchTask.Status", $('#ddlStatus').val());
            formData.append("BatchTask.Remarks", $('#txtRemarks').val());

            return formData;
        }

    };
    
     objPendingTaskBatchEditJS.Initialize();
});