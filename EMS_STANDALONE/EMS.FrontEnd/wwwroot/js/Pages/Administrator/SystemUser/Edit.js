var objSystemUserEditJS;

$(document).ready(function () {
    objSystemUserEditJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divSystemUserBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $('#txtUsername').prop('disabled', true);
            $("#divSystemUserModal .form-control").attr("readonly", false);
            $("#divSystemUserModal #btnEdit").hide();
            $("#divSystemUserModal #btnSave, #divSystemUserModal #btnBack").show();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabMembers');
            s.LoadSystemRole();
            s.LoadSystemUserRole();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divSystemUserModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnMoveToListTwo").click(function () {
                ListBoxMove("ddlListOneOrgGroup", "ddlListTwoOrgGroup", false, true);
            });

            $("#btnMoveToListOne").click(function () {
                ListBoxMove("ddlListTwoOrgGroup", "ddlListOneOrgGroup", false, true);
            });

            $("#btnMoveToListTwoAll").click(function () {
                ListBoxMove("ddlListOneOrgGroup", "ddlListTwoOrgGroup", true, true);
            });

            $("#btnMoveToListOneAll").click(function () {
                ListBoxMove("ddlListTwoOrgGroup", "ddlListOneOrgGroup", true, true);
            });

            $("#txtSearchBoxOne").on("change keyup keydown", function () {
                ListBoxSearch("ddlListOneOrgGroup", $(this).val(), false);
            });

            $("#txtSearchBoxTwo").on("change keyup keydown", function () {
                ListBoxSearch("ddlListTwoOrgGroup", $(this).val(), false);
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , SystemUserDeleteURL + '?ID=' + objSystemUserEditJS.ID \
                    , {} \
                    , '#divSystemUserErrorMessage' \
                    , '#btnDelete' \
                    , objSystemUserEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnSave").click(function () {
                var isNoBlankFunction = function () {

                    if ($('#ddlListTwoOrgGroup option').length == 0) {
                        $("#ddlListTwoOrgGroup").addClass("errMessage");
                        $("#ddlListTwoOrgGroup").focus();
                        $("#divSystemUserErrorMessage").append("<label class=\"errMessage\"><li>Please select at least one (1) role</li></label><br />");
                    }
                    else
                        return true;
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmSystemUser", "#divSystemUserErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SystemUserEditURL \
                        , objSystemUserEditJS.GetFormData() \
                        ,'#divSystemUserErrorMessage' \
                        , '#btnSave' \
                        , objSystemUserEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(SystemUserViewURL + "?ID=" + objSystemUserEditJS.ID, "divSystemUserBodyModal");
            });

        },

        GetFormData: function () {

            var formData = new FormData($('#frmSystemUser').get(0));

            formData.append("SystemUser.Username", $("#txtUsername").val());
            formData.append("SystemUser.FirstName", $("#txtFirstName").val());
            formData.append("SystemUser.MiddleName", $("#txtMiddleName").val());
            formData.append("SystemUser.LastName", $("#txtLastName").val());

            $("#ddlListTwoOrgGroup option").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("SystemUser.RoleIDs[" + index + "]", $(this).val());
                }
            });

            return formData;

        },

        LoadSystemRole: function () {

            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    $("#ddlListOneOrgGroup").append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });
            };

            objEMSCommonJS.GetAjax(GetSystemRoleDropDownURL, {}, "", GetSuccessFunction);
        },

        LoadSystemUserRole: function () {

            var input = { ID: objSystemUserEditJS.ID };

            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    $('#ddlListOneOrgGroup option').each(function () {
                        if ($(this).val() == item.Value) {
                            $("#ddlListTwoOrgGroup").append($('<option/>', {
                                value: $(this).val(),
                                text: $(this).text()
                            }));
                        }
                    });
                });
                $("#ddlListTwoOrgGroup option").each(function () {
                    $("#ddlListOneOrgGroup option[value='" + $(this).val() + "']").remove();
                });
            };

            objEMSCommonJS.GetAjax(GetSystemUserRoleDropDownURL, input, "", GetSuccessFunction);
        },

    };

    objSystemUserEditJS.Initialize();
});