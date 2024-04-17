var objWorkflowTransactionAddJS;

$(document).ready(function () {
    objWorkflowTransactionAddJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            var s = this;
            s.ElementBinding();
        },

        ElementBinding: function () {
            $("#divUpdateStatusBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            $("#dpDateScheduled, #dpDateCompleted").datetimepicker({
                useCurrent: true,
                format: 'MM/DD/YYYY',
            });

            $("#divUpdateStatusModal #btnSave").click(function () {
                
                if (objEMSCommonJS.ValidateBlankFields("#frmTransactionAdd", "#divTransactionAddErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , UpdateStatusURL \
                        ,  objWorkflowTransactionAddJS.GetFormData() \
                        , '#divTransactionAddErrorMessage' \
                        , '#divUpdateStatusModal #btnSave' \
                        , objWorkflowTransactionAddJS.EditSuccessFunction); ",
                        "function");
                }
            });

        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divUpdateStatusModal').modal('hide');
        },

        GetFormData: function () {
            var formData = new FormData($('#frmTransactionAdd').get(0));
            return formData;
        },
    };
    
     objWorkflowTransactionAddJS.Initialize();
});