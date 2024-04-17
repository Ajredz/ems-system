var objAddLogActivityJS;

$(document).ready(function () {
    objAddLogActivityJS = {

        Initialize: function () {
            $("#divLogActivityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            $("#divLogActivityModal #btnSaveActivity").show();
            $("#divLogActivityModal .form-control").attr("readonly", false);
            $("#divAssignUser").show();
            $("#divIsVisible").show();
            s.ElementBinding();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSaveActivity").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmLogActivity", "#divLogActivityErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeAddLogActivityPostURL \
                        , objAddLogActivityJS.GetFormData() \
                        , '#divLogActivityErrorMessage' \
                        , '#btnSaveActivity' \
                        , objAddLogActivityJS.AddSuccessFunction);",
                        "function");
                }
            });

            $("#dpDueDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            //$("#ddlType").change(function () {
            //    GenerateDropdownValues(SubTypeDropdownChangeURL + "&Type=" + $("#ddlType").val(),
            //        "ddlSubType", "Value", "Description", "", "", false);
            //});

            //$("#ddlType").change(function () {
            //    // Get Tags from SubType Prefix Value, ex. 1_0_MEDICAL, IsWithPassFail = true, IsWithAssignment = false
            //    if (($(this).val()).substr(2,1) == "1") {
            //        $("#divAssignUser").show();
            //        $("#divLogActivityModal #divAssignUser .form-control").attr("readonly", false);
            //        $("#divLogActivityModal #txtAssignedUser").addClass("required-field");
            //        $("#divLogActivityModal label[for='txtAssignedUser'] span").addClass("reqField");
            //        $("#divLogActivityModal label[for='txtAssignedUser'] span").removeClass("unreqField");


            //    }
            //    else {
            //        $("#divAssignUser").hide();
                    $("#divLogActivityModal #txtAssignedUser").removeClass("required-field");
                    $("#divLogActivityModal label[for='txtAssignedUser'] span").removeClass("reqField");
                    $("#divLogActivityModal label[for='txtAssignedUser'] span").addClass("unreqField");
            //        $("#divLogActivityModal #divAssignUser .form-control").attr("readonly", true);
            //    }
            //});

            objEMSCommonJS.BindAutoComplete("divLogActivityModal #txtAssignedUser"
                , ReferredByAutoComplete
                , 20, "divLogActivityModal #hdnAssignedUserID", "ID", "Description");

            //$("#txtAssignedUser").autocomplete({
            //    source: function (request, response) {
            //        $.ajax({
            //            url: ReferredByAutoComplete, // URL 
            //            type: "GET",
            //            dataType: "json",
            //            data: {
            //                Term: $("#txtAssignedUser").val(),
            //                TopResults: 20
            //            },
            //            success: function (data) {
            //                if (data.IsSuccess) {
            //                    response($.map(data.Result, function (item) {
            //                        return {
            //                            label: item.Description,
            //                            value: item.ID
            //                        };
            //                    }))
            //                }
            //                else {
            //                    ModalAlert(MODAL_HEADER, data.Result);
            //                }
            //            },
            //            error: function (jqXHR, textStatus, errorThrown) {
            //                ModalAlert(MODAL_HEADER, jqXHR.responseText);
            //            }
            //        })
            //    },
            //    select: function (event, ui) { // Event - triggers after selection on list
            //        if (ui.item.label != null) {
            //        }
            //        return false;
            //    },
            //    change: function (event, ui) { // Event - triggers when the value of the textbox changed
            //        if (ui.item == null) {
            //            $("#hdnAssignedUserID").val(0);
            //            $(this).val("");
            //        } else {
            //            $("#hdnAssignedUserID").val(ui.item.value);
            //            $(this).val(ui.item.label);
            //        }
            //    },
            //    focus: function (event, ui) {
            //        $(this).val(ui.item.label);
            //        event.preventDefault(); // Prevent the default focus behavior.
            //    }
            //});

            $("#cbIsAssignToSelf").on("click", function () {
                if ($(this).is(':checked')) {
                    $("#txtAssignedUser").prop("disabled", true);
                } else {
                    $("#txtAssignedUser").prop("disabled", false);
                }
                $("#txtAssignedUser").val("");
                $("#hdnAssignedUserID").val("");
            });

            objEMSCommonJS.BindAutoComplete("txtAssignedOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "hdnAssignedOrgGroup", "ID", "Description");

        },

        GetFormData: function () {
            var formData = new FormData($('#frmLogActivity').get(0));
            formData.append("TagToEmployeeForm.EmployeeID", $("#divEmployeeModal #hdnID").val());
            return formData;
        },

        AddSuccessFunction: function () {
            objLogActivityJS.LoadLogActivityJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
            $("#frmLogActivity").trigger("reset");
            $("#divLogActivityModal .form-control").attr("disabled", false);
        },

        UpdateLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return LoadPartial('" + EmployeeViewURL + "?ID=" + rowObject.ID + "', 'divEmployeeBodyModal');\">Update</a>";
        },

        AddPreloadedSuccessFunction: function () {
            objAddLogActivityJS.LoadLogActivityJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
        },
    };

    objAddLogActivityJS.Initialize();
});