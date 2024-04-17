var objSystemRoleEditJS;

$(document).ready(function () {
    objSystemRoleEditJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divSystemRoleBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divSystemRoleModal .form-control").attr("readonly", false);
            $("#divSystemRoleModal #btnEdit").hide();
            $("#divSystemRoleModal #btnSave, #divSystemRoleModal #btnBack").show();

            $(".div-page").each(function () {
                if ($(this).find(".cb-function-type:checked").length > 0) {
                    $(this).find(".cb-page").prop("checked", true);
                }
                else {
                    $(this).find(".cb-page").prop("checked", false);
                }
            });

            $(".div-module").each(function (index, item) {
                if ($(this).find("div").find(".cb-function-type:checked").length > 0) {
                    $("#cb" + item.id.substring(3, item.id.length)).prop("checked", true);
                    $(this).show();
                }
                else {
                    $("#cb" + item.id.substring(3, item.id.length)).prop("checked", false);
                    $(this).hide();
                }
            });
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divSystemRoleModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , SystemRoleDeleteURL + '?ID=' + objSystemRoleEditJS.ID \
                    , {} \
                    , '#divSystemRoleErrorMessage' \
                    , '#btnDelete' \
                    , objSystemRoleEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmSystemRole", "#divSystemRoleErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SystemRoleEditURL \
                        , objSystemRoleEditJS.GetFormData() \
                        ,'#divSystemRoleErrorMessage' \
                        , '#btnSave' \
                        , objSystemRoleEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(SystemRoleViewURL + "?ID=" + objSystemRoleEditJS.ID, "divSystemRoleBodyModal");
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

    objSystemRoleEditJS.Initialize();
});