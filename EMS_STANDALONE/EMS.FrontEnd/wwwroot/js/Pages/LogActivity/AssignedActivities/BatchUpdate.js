var objTaskBatchUpdateJS;
var StatusDropDownOptions = [];

$(document).ready(function () {
    objTaskBatchUpdateJS = {

        Initialize: function () {
            $("#divTaskBatchUpdateBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            s.GetStatusDropDownOptions();
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
            $("#divTaskBatchUpdateModal").modal("hide");
        },

        ElementBinding: function () {
            var s = this;

            $("#frmBatchTask #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmBatchTask", "#divBatchTaskErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                            , TaskBatchUpdatePostURL \
                            , objTaskBatchUpdateJS.GetFormData() \
                            , '#divBatchTaskErrorMessage' \
                            , '#divTaskBatchUpdateModal #btnSave' \
                            , objTaskBatchUpdateJS.EditSuccessFunction);", "function");
                }
            });
        },

        GetFormData: function () {

            var input = $("#tblAssignedActivitiesList").jqGrid("getGridParam", "selarrrow");
            var formData = new FormData($('#frmBatchTask').get(0));
            var ctr1 = 0;
            var ctr2 = 0;

            for (var x = 0; x < input.length; x++)
            {
                var rowdata = $("#tblAssignedActivitiesList").jqGrid("getRowData", input[x]);

                if (rowdata["ApplicantName"])
                {
                    formData.append("BatchTask.ApplicantIDs[" + ctr1 + "]", input[x]);
                    ctr1++;
                }
                else if (rowdata["EmployeeName"])
                {
                    formData.append("BatchTask.EmployeeIDs[" + ctr2 + "]", input[x]);
                    ctr2++;
                }
            }

            formData.append("BatchTask.Status", $('#ddlStatus').val());
            formData.append("BatchTask.Remarks", $('#txtRemarks').val());

            return formData;
        },

        GetStatusDropDownOptions: function () {

            var input = $("#tblAssignedActivitiesList").jqGrid("getGridParam", "selarrrow");
            var statusList = [];

            for (var x = 0; x < input.length; x++) {
                var rowdata = $("#tblAssignedActivitiesList").jqGrid("getRowData", input[x]);
                statusList.push(rowdata["CurrentStatusCode"]);
            }

            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    StatusDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
                objEMSCommonJS.PopulateDropDown("#divTaskBatchUpdateModal #ddlStatus", StatusDropDownOptions);
            };

            objEMSCommonJS.GetAjax(BatchStatusDropDownURL + "&CurrentStatus=" + statusList[0], {}, "", GetSuccessFunction);
        },

    };
    
     objTaskBatchUpdateJS.Initialize();
});