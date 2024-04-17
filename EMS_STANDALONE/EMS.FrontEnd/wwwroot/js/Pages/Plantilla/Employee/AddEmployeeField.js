var objEmployeeFieldAddJS;

$(document).ready(function () {

    objEmployeeFieldAddJS = {

        Initialize: function () {
            $("#divEmployeeFieldBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divEmployeeFieldModal .form-control").attr("readonly", false);
            $("#frmEmployeeField option[value='SECONDARY_DESIG']").remove();
        },

        AddSuccessFunction: function () {
            //$("#frmEmployeeField").trigger("reset");
            $("#divEmployeeFieldModal").modal("hide");
            //var param = {
            //    EmployeeField: localStorage["PlantillaEmployeeField"]
            //};
            //objEmployeeMovementAddListJS.LoadJQGrid(param);
            objEmployeeMovementAddListJS.LoadJQGrid();
            //$("#divMovementEmployeeFieldErrorMessage").html("");
            //$("#divMovementEmployeeFieldErrorMessage").css("display", "none");
        },

        ElementBinding: function () {
            var s = this;   

            $("#divEmployeeFieldModal #btnSaveField").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmEmployeeField", "#divEmployeeFieldErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeFieldAddPostURL \
                        , objEmployeeFieldAddJS.GetFormData() \
                        , '#divEmployeeFieldErrorMessage' \
                        , '#divEmployeeFieldModal #btnSaveField' \
                        , objEmployeeFieldAddJS.AddSuccessFunction);",
                        "function");
                }
            });
        },

        GetFormData: function () {

            var formData = new FormData($('#frmEmployeeField').get(0));

            formData.append("MovementEmployeeField.MovementType", $("#ddlMovementType").val());
            formData.append("MovementEmployeeField.EmployeeField", $("#ddlMovementEmployeeField option:selected").html());
            formData.append("MovementEmployeeField.EmployeeFieldCode", $("#ddlMovementEmployeeField").val());

            return formData;

        },

    };

    objEmployeeFieldAddJS.Initialize();
});