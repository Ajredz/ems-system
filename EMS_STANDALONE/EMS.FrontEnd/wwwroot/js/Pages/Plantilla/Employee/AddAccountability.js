var objAddAccountabilityJS;

$(document).ready(function () {
    objAddAccountabilityJS = {

        Initialize: function () {
            $("#divAccountabilityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            $("#divAccountabilityModal #btnSaveAccountability").show();
            $("#divAccountabilityModal .form-control").attr("readonly", false);
            s.ElementBinding();
        },

        ElementBinding: function () {
            var s = this;

            $("#ddlType").on("change", function () {
                if ($("#ddlType").val() == "ACCOUNTABILITY_INTANGIBLE")
                    $(".ShowIntangible").prop("hidden", false);
                else
                    $(".ShowIntangible").prop("hidden", true);
            });

            $("#btnSaveAccountability").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmAccountability", "#divAccountabilityErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , AddAccountabilityPostURL \
                        , objAddAccountabilityJS.GetFormData() \
                        , '#divAccountabilityErrorMessage' \
                        , '#btnSaveAccountability' \
                        , objAddAccountabilityJS.AddSuccessFunction);",
                        "function");
                }
            });

            objEMSCommonJS.BindAutoComplete("txtOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "hdnOrgGroup", "ID", "Description");
            objEMSCommonJS.BindAutoComplete("txtPosition"
                , PositionAutoCompleteURL, 20, "hdnPosition", "ID", "Description");
            objEMSCommonJS.BindAutoComplete("txtEmployee"
                , EmployeeAutoCompleteURL, 20, "hdnEmployee", "ID", "Description");
        },

        GetFormData: function () {
            var formData = new FormData($('#frmAccountability').get(0));
            formData.append("TagToEmployeeForm.EmployeeID", $("#divEmployeeModal #hdnID").val());
            return formData;
        },

        AddSuccessFunction: function () {
            objAccountabilityJS.LoadAccountabilityJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
            $("#frmAccountability").trigger("reset");
        },
    };

    objAddAccountabilityJS.Initialize();
});