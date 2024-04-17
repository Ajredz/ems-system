var objEmployeeScoreBatchUpdateJS;
var StatusDropDownOptions = [];

$(document).ready(function () {
    objEmployeeScoreBatchUpdateJS = {

        Initialize: function () {
            $("#divEmployeeScoreBatchUpdatesBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
           s.GetStatusDropDownOptions();

            $('#ddlUpdateStatus option[value="WIP"]').remove();
        },

        EditSuccessFunction: function () {
            $("#divEmployeeScoreList #btnSearch").click();
            $("#divEmployeeScoreBatchUpdatesModal").modal("hide");
        },

        ElementBinding: function () {
            var s = this;

            $("#frmBatchEmployeeScore #btnSave").click(function () {
                console.log("test remarks :" + $("#txtRemarksMultiple").val() )
                if (objEMSCommonJS.ValidateBlankFields("#frmBatchEmployeeScore", "#divBatchEmployeeScoreErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                            , EmployeeScoreBatchUpdatesURL \
                            , objEmployeeScoreBatchUpdateJS.GetFormData() \
                            , '#divBatchEmployeeScoreErrorMessage' \
                            , '#divEmployeeScoreBatchUpdatesModal #btnSave' \
                            , objEmployeeScoreBatchUpdateJS.EditSuccessFunction);", "function");
                }
            });
        },


        GetFormData: function () {

            var input = $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "selarrrow");
            var formData = new FormData($('#frmBatchEmployeeScore').get(0));
            var ctr1 = 0;

            for (var x = 0; x < input.length; x++) {
                formData.append("BatchEmployeesScore.IDs[" + ctr1 + "]", input[x]);
                ctr1++;
            }

            formData.append("BatchEmployeesScore.Status", $("#ddlUpdateStatus").val());
            formData.append("BatchEmployeesScore.Remarks", $("#txtRemarksMultiple").val());

            return formData;
        },

        GetStatusDropDownOptions: function () {

            var input = $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "selarrrow");
            var statusList = [];

            for (var x = 0; x < input.length; x++) {
                var rowdata = $("#tblIPMEmployeeScoreList").jqGrid("getRowData", input[x]);
                statusList.push(rowdata["Status"]);
            }

            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    StatusDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
                objEMSCommonJS.PopulateDropDown("#divEmployeeScoreBatchUpdatesModal #ddlUpdateStatus", StatusDropDownOptions);
            };

            objEMSCommonJS.GetAjax(BatchStatusDropDownURL + "&CurrentStatus=" + statusList[0], {}, "", GetSuccessFunction);
        },

    };

    objEmployeeScoreBatchUpdateJS.Initialize();
});