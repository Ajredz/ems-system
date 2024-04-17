var objEmployeeTrainingAddJS;


$(document).ready(function () {
    objEmployeeTrainingAddJS = {

        EmployeeID: $("#hdnEmployeeID").val(),

        Initialize: function () {
            $("#divTrainingBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            $("#divTrainingModal #ddlType, \
               #divTrainingModal #txtTitle, \
               #divTrainingModal #txtDescription, \
               #divTrainingModal #txtDateSchedule").attr("disabled", false);

            s.ElementBinding();
        },

        ElementBinding: function () {
            var s = this;

            $("#txtDateSchedule").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#divTrainingModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#divTrainingModal #formEmployeeTraining", "#divTrainingModal #divTrainingErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , TrainingAddURL \
                        , objEmployeeTrainingAddJS.FormData() \
                        , '#divTrainingModal #divTrainingErrorMessage' \
                        , '#divTrainingModal #btnSave' \
                        , objEmployeeTrainingAddJS.SuccessAddFunction);",
                        "function");
                }
            });
        },

        FormData: function () {
            var formData = new FormData($('#formEmployeeTraining').get(0));
            return formData;
        },

        SuccessAddFunction: function () {
            $('#divTrainingModal').modal('hide');
            $("#tabTraining #btnSearch").click();
        },
    }

    objEmployeeTrainingAddJS.Initialize();
});