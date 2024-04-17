var objEmployeeTrainingViewJS;

$(document).ready(function () {
    objEmployeeTrainingViewJS = {

        ID: $("#divTrainingModal #hdnID").val(),
        EmployeeID: $("#hdnEmployeeID").val(),

        Initialize: function () {
            $("#divTrainingBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;


            s.ElementBinding();

            objEmployeeTrainingListJS.LoadStatusHistory({
                ID: objEmployeeTrainingViewJS.ID,
            });
            objEMSCommonJS.ChangeTab($("#divTrainingModal .tablinks:nth-child(1)"), 'tabHistory', '#divTrainingModal');
            HistoryLoadOnce = true;
            ScoreLoadOnce = false;
        },

        ElementBinding: function () {
            var s = this;

            $("#txtDateSchedule").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $(".tablinks").find("span:contains('Score')").parent("button").click(function () {
                if (!ScoreLoadOnce) {
                    objEmployeeTrainingListJS.LoadScore({
                        ID: objEmployeeTrainingViewJS.ID,
                    });
                    ScoreLoadOnce = true;
                }
            });

            $("#divTrainingModal #btnEdit").click(function () {
                LoadPartial(TrainingEditURL + "?ID=" + objEmployeeTrainingViewJS.ID, "divTrainingBodyModal");
            });

            $("#divTrainingModal #btnChangeStatus").click(function () {
                if ($("#divTrainingModal #ChangeStatusModal").is(":visible"))
                    $("#divTrainingModal #ChangeStatusModal").hide();
                else {
                    $("#divTrainingModal #ChangeStatusModal").show();
                    GenerateDropdownValues(TrainingChangeStatus + "&CurrentStatus=" + $("#hdnStatus").val(), "divTrainingModal #ddlDynamicChangeStatus", "Value", "Text", "", "", false);
                }
            });
            $("#divTrainingModal #btnCancelDynamicChangeStatus").on("click", function () {
                $("#divTrainingModal #ChangeStatusModal").hide();
            });
            $("#divTrainingModal #ddlDynamicChangeStatus").on("change", function () {
                $("#divTrainingModal #divDynamicChangeStatusErrorMessage").html("");
                if ($("#divTrainingModal #ddlDynamicChangeStatus :selected").val() == "CANCELLED") {
                    $("#divTrainingModal #spnDynamicChangeStatus").addClass("reqField");
                    $("#divTrainingModal #spnDynamicChangeStatus").removeClass("unreqField");
                    $("#divTrainingModal #txtDynamicChangeStatusRemarks").addClass("required-field");
                }
                else {
                    $("#divTrainingModal #spnDynamicChangeStatus").addClass("unreqField");
                    $("#divTrainingModal #spnDynamicChangeStatus").removeClass("reqField");
                    $("#divTrainingModal #txtDynamicChangeStatusRemarks").removeClass("required-field");
                }
            });
            $("#divTrainingModal #btnSaveDynamicChangeStatus").on("click", function () {
                $("#divTrainingModal #divDynamicChangeStatusErrorMessage").html("");

                if ($("#divTrainingModal #ddlDynamicChangeStatus :selected").val() == "") {
                    $("#divTrainingModal #divDynamicChangeStatusErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + " Movement</li></label><br />");
                    return;
                }

                var isNoBlankFunction = function () {
                    return true;
                };
                if (objEMSCommonJS.ValidateBlankFields("#divTrainingModal #formEmployeeTraining", "#divTrainingModal #divDynamicChangeStatusErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , TrainingChangeStatus + "+ ("'&ID=" + objEmployeeTrainingViewJS.ID + "'") + " \
                        , objEmployeeTrainingViewJS.ChangeStatusFormData() \
                        , '#divTrainingModal #divDynamicChangeStatusErrorMessage' \
                        , '#divTrainingModal #btnSaveDynamicChangeStatus' \
                        , objEmployeeTrainingViewJS.ChangeStatusSuccessFunction);",
                        "function");
                }
            });
        },
        ChangeStatusFormData: function () {
            var formData = new FormData($('#divTrainingModal #DynamicChangeStatusForm').get(0));

            formData.append("ChangeStatus.Status", $("#divTrainingModal #ddlDynamicChangeStatus :selected").val());
            formData.append("ChangeStatus.Remarks", $("#divTrainingModal #txtDynamicChangeStatusRemarks").val());
            return formData;
        },
        ChangeStatusSuccessFunction: function () {
            $('#divTrainingModal').modal('hide');
            $("#tabTraining #btnSearch").click();
        },
    }

    objEmployeeTrainingViewJS.Initialize();
});