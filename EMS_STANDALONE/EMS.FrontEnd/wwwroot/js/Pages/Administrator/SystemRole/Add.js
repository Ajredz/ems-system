var objSystemRoleAddJS;

$(document).ready(function () {
    objSystemRoleAddJS = {

        Initialize: function () {
            $("#divSystemRoleBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#btnSave").show();
            $("#divSystemRoleModal .form-control").attr("readonly", false);
            $("#btnDelete, #btnBack").remove();
            
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmSystemRole").trigger("reset");
            $(".cb-module").prop("checked", false);
            $(".cb-page").prop("checked", false);
            $(".cb-function-type").prop("checked", false);
            $(".div-module").hide();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmSystemRole", "#divSystemRoleErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SystemRoleAddPostURL \
                        , objSystemRoleAddJS.GetFormData() \
                        ,'#divSystemRoleErrorMessage' \
                        , '#btnSave' \
                        , objSystemRoleAddJS.AddSuccessFunction);",
                        "function");
                }
            });

            $(".cb-page").click(function () {
                var p = $(this).parent("div");
                if ($(this).prop("checked")) {
                    $(p).find(".cb-function-type").prop("checked", true);
                }
                else {
                    $(p).find(".cb-function-type").prop("checked", false);
                }
            });

            $(".cb-function-type").click(function () {
                var p = $(this).parent("div");
                if ($(p).find(".cb-function-type:checked").length > 0) {
                    $(p).find(".cb-page").prop("checked", true);
                }
                else {
                    $(p).find(".cb-page").prop("checked", false);
                }
            });


            $(".cb-module").click(function () {
                if ($(this).prop("checked")) {
                    $("#div" + this.id.substring(2, this.id.length)).show();
                }
                else {
                    $("#div" + this.id.substring(2, this.id.length)).hide();
                }
            });
        },

        GetFormData: function () {

            var formData = new FormData($('#frmSystemRole').get(0));

            formData.append("SystemRole.RoleName", $("#txtRoleName").val());

            var ctr = 0;
            $(".div-module:visible").each(function () {

                if ($(this).find(".cb-function-type:checked").length > 0) {
                    // Add Module
                    formData.append("SystemRole.SystemRoleAccessList[" + ctr + "].PageID", $("#cb" + this.id.substring(3, this.id.length)).val());
                    formData.append("SystemRole.SystemRoleAccessList[" + ctr + "].FunctionType", "VIEW");
                    formData.append("SystemRole.SystemRoleAccessList[" + ctr + "].HasAccess", true);
                    ctr++;

                    $(this).find(".cb-function-type:checked").each(function (index) {
                        formData.append("SystemRole.SystemRoleAccessList[" + ctr + "].PageID", $(this).parent("div").find(".cb-page").val());
                        formData.append("SystemRole.SystemRoleAccessList[" + ctr + "].FunctionType", $(this).val());
                        formData.append("SystemRole.SystemRoleAccessList[" + ctr + "].HasAccess", true);
                        ctr++;
                    });
                }
            });

            return formData;

        },

    };
    
     objSystemRoleAddJS.Initialize();
});