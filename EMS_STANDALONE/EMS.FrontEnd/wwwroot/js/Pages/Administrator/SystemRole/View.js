var objSystemRoleViewJS;

$(document).ready(function () {
    objSystemRoleViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divSystemRoleBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");

            $("#divSystemRoleBodyModal input[type='checkbox']").prop("disabled", true);

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

        ElementBinding: function () {
            var s = this;

            $("#txtSearchBoxOne").on("change keyup keydown", function () {
                ListBoxSearch("ddlListOneOrgGroup", $(this).val(), false);
            });

            $("#txtSearchBoxTwo").on("change keyup keydown", function () {
                ListBoxSearch("ddlListTwoOrgGroup", $(this).val(), false);
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , SystemRoleDeleteURL + '?ID=' + objSystemRoleViewJS.ID \
                    , {} \
                    , '#divSystemRoleErrorMessage' \
                    , '#btnDelete' \
                    , objSystemRoleViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(SystemRoleEditURL + "?ID=" + objSystemRoleViewJS.ID, "divSystemRoleBodyModal");
            });

        },

    };

    objSystemRoleViewJS.Initialize();
});