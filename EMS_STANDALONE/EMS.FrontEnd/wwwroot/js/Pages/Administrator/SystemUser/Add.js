var objSystemUserAddJS;

$(document).ready(function () {
    objSystemUserAddJS = {

        Initialize: function () {
            $("#divSystemUserBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#btnSave").show();
            $("#divSystemUserModal .form-control").attr("readonly", false);
            $("#btnDelete, #btnBack").remove();
            $("#cbIsActive").attr('checked', true);
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabMembers');
            s.LoadSystemUsers();
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmSystemUser").trigger("reset");
            $('#ddlListTwoOrgGroup').empty();
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

            $("#btnSave").click(function () {
                var isNoBlankFunction = function () {

                    if ($('#ddlListTwoOrgGroup option').length == 0) {
                        $("#ddlListTwoOrgGroup").addClass("errMessage");
                        $("#divSystemUserErrorMessage").append("<label class=\"errMessage\"><li>Please select at least one (1) role</li></label><br />");
                    }
                    else
                        return true;
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmSystemUser", "#divSystemUserErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SystemUserAddPostURL \
                        , objSystemUserAddJS.GetFormData() \
                        ,'#divSystemUserErrorMessage' \
                        , '#btnSave' \
                        , objSystemUserAddJS.AddSuccessFunction);",
                        "function");
                }
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

        LoadSystemUsers: function () {

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

    };
    
     objSystemUserAddJS.Initialize();
});