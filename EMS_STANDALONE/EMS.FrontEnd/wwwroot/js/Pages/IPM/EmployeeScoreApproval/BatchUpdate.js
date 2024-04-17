var objEmployeeScoreBatchUpdateJS;
var StatusDropDownOptions = [];

$(document).ready(function () {
    objEmployeeScoreBatchUpdateJS = {

        Initialize: function () {
            $("#divEmployeeScoreApprovalBatchUpdateBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            s.GetStatusDropDownOptions();

            $('#ddlUpdateStatus option[value="WIP"]').remove();
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
            $("#divEmployeeScoreApprovalBatchUpdateModal").modal("hide");
        },

        ElementBinding: function () {
            var s = this;

            $("#frmBatchEmployeeScore #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmBatchEmployeeScore", "#divBatchEmployeeScoreErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                            , EmployeeScoreBatchUpdateURL \
                            , objEmployeeScoreBatchUpdateJS.GetFormData() \
                            , '#divBatchEmployeeScoreErrorMessage' \
                            , '#divEmployeeScoreApprovalBatchUpdateModal #btnSave' \
                            , objEmployeeScoreBatchUpdateJS.EditSuccessFunction);", "function");
                }
            });
        },

        GetFormData: function () {

            var input = $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "selarrrow");
            var formData = new FormData($('#frmBatchEmployeeScore').get(0));
            var ctr1 = 0;

            for (var x = 0; x < input.length; x++)
            {
                formData.append("BatchEmployeeScore.IDs[" + ctr1 + "]", input[x]);
                ctr1++;
            }

            formData.append("BatchEmployeeScore.Status", $('#ddlUpdateStatus').val());
            formData.append("BatchEmployeeScore.Remarks", $('#txtRemarks').val());

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
                objEMSCommonJS.PopulateDropDown("#divEmployeeScoreApprovalBatchUpdateModal #ddlUpdateStatus", StatusDropDownOptions);
            };

            objEMSCommonJS.GetAjax(BatchStatusDropDownURL + "&CurrentStatus=" + statusList[0], {}, "", GetSuccessFunction);
        },

    };
    
     objEmployeeScoreBatchUpdateJS.Initialize();
});