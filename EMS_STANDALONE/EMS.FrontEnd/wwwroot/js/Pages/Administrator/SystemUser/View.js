var objSystemUserViewJS;

$(document).ready(function () {
    objSystemUserViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divSystemUserBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $('.listboxbutton button').prop('disabled', true);
            $('#cbIsActive').prop('disabled', true);
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabMembers');
            s.LoadSystemRole();
            s.LoadSystemUserRole();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divSystemUserModal').modal('hide');
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
                    , SystemUserDeleteURL + '?ID=' + objSystemUserViewJS.ID \
                    , {} \
                    , '#divSystemUserErrorMessage' \
                    , '#btnDelete' \
                    , objSystemUserViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(SystemUserEditURL + "?ID=" + objSystemUserViewJS.ID, "divSystemUserBodyModal");
            });

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

            var input = { ID: objSystemUserViewJS.ID };

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

    objSystemUserViewJS.Initialize();
});