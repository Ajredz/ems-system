var objEmployeeScoreListJS;
const EmployeeScoreListURL = "/IPM/EmployeeScore?handler=List";
const EmployeeKPIScoreListURL = "/IPM/EmployeeScore?handler=EmployeeKPIScoreGetList";
const EmployeeScoreUpdateDescriptionPostURL = "/IPM/EmployeeScore?handler=UpdatedRunDescription";
const EmployeeScoreRerunPostURL = "/IPM/EmployeeScore?handler=ReRun";
const EmployeeScoreAddURL = "/IPM/EmployeeScore/Add";
const EmployeeScoreViewURL = "/IPM/EmployeeScore/View";
const EmployeeScoreViewSummaryURL = "/IPM/EmployeeScore?handler=ViewSummary";
const EmployeeScoreEditURL = "/IPM/EmployeeScore/Edit";
const EmployeeScoreDeleteURL = "/IPM/EmployeeScore/Delete";
//const EmployeeBatchUpdateURL = "/IPM/EmployeeScore/Update";
const EmployeeScoreAddPostURL = "/IPM/EmployeeScore/Add";
const EmployeeScoreEditPostURL = "/IPM/EmployeeScore/Edit";
const GetCheckExportListURL = "/IPM/EmployeeScore?handler=CheckExportList";
const DownloadExportListURL = "/IPM/EmployeeScore?handler=DownloadExportList";
const EmployeeScoreBulkDeleteListURL = "/IPM/EmployeeScore?handler=BulkDelete";
const GetEmployeeScoreStatusHistoryURL = "/IPM/EmployeeScore?handler=EmployeeScoreStatusHistory";
const GetRunIDAutoCompleteURL = "/IPM/EmployeeScore?handler=RunIDAutoComplete";

const EmployeeScoreBulkVoidListURL = "/IPM/EmployeeScore?handler=BulkVoid";
const EmployeeScoreBatchUpdatesURL = "/IPM/EmployeeScore/BatchUpdates";
const BatchStatusDropDownURL = "/IPM/EmployeeScore/BatchUpdates?handler=BatchStatusDropDown";

const EmployeeScoreBulkApprovedListURL = "/IPM/EmployeeScore?handler=BulkApproved";
const ProcessTransNewRunURL = "/IPM/EmployeeScore?handler=ProcessTransNewRun";

const CheckExportEmployeeURL = "/IPM/EmployeeScore?handler=CheckExportEmployee";
const DownloadExportEmployeeURL = "/IPM/EmployeeScore?handler=DownloadExportEmployee";
//const DownloadExportEmployeeURL = "/IPM/EmployeeScore?handler=PrintDownload";

const GetEmployeeScoreRunURL = "/IPM/EmployeeScore?handler=GetScores";
const EmployeeScoreRunURL = "/IPM/EmployeeScore?handler=RunScores";
const SaveEmployeeScoreURL = "/IPM/EmployeeScore?handler=SaveScores";

const GetEmployeeAutoCompleteURL = "/IPM/EmployeeScore?handler=EmployeeAutoComplete";
const GetEmployeeSystemUserAutoCompleteURL = "/IPM/EmployeeScore?handler=EmployeeSystemUserAutoComplete";
const GetOrgGroupAutoCompleteURL = "/IPM/EmployeeScore?handler=OrgTypeAutoComplete";
const GetOrgGroupFilteredAutoCompleteURL = "/IPM/EmployeeScore?handler=OrgTypeFilteredAutoComplete";
const GetEmployeeFilteredAutoCompleteURL = "/IPM/EmployeeScore?handler=EmployeeFilteredAutoComplete";
const GetPositionAutoCompleteURL = "/IPM/EmployeeScore?handler=PositionAutoComplete";
const EmployeeScoreStatusDropDownURL = "/IPM/EmployeeScore?handler=ReferenceValue&RefCode=EMP_SCORE_STATUS";

const GetTransProgressURL = "/IPM/EmployeeScore/Add?handler=TransProgress";
const GetRatingGradesDropdownURL = "/IPM/EmployeeScore?handler=RatingGrades";

const GetRaterByTransIDURL = "/IPM/EmployeeScore?handler=IPMRaterByTransID";

const _MaxScore = $("#hdnMaxValue").val();

var transProgress = 0;
var transTotal = 0;

var isRunOnGoing = false;
var isCheckOnLoad = false;
var timer;
var isEnd = 0;
var TransSumID = 0;

var Interval = null;

$(document).ready(function () {
    objEmployeeScoreListJS = {
        Initialize: function () {
            $("#divEmployeeScoreSummaryBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divRunEmployeeFinalScoreModal .modal-header").mousedown(handle_mousedown);
            $("#divEmployeeScoreDescriptionErrorMessage").css({ "display": "none" });
            var s = this;
            s.ElementBinding();
            var param = {
                TransSummaryIDDelimited: localStorage["IPMEmployeeScoreListIDDelimited"],
                Description: localStorage["IPMEmployeeScoreListDescription"],
                IsActiveDelimited: localStorage["IPMEmployeeScoreListIsActiveDelimited"],
                NameDelimited: localStorage["IPMEmployeeScoreListNameDelimited"],
                ParentOrgGroup: localStorage["IPMEmployeeScoreListParentOrgGroup"],
                OrgGroupDelimited: localStorage["IPMEmployeeScoreListOrgGroupDelimited"],
                PositionDelimited: localStorage["IPMEmployeeScoreListPositionDelimited"],
                ScoreFrom: localStorage["IPMEmployeeScoreListScoreFrom"],
                ScoreTo: localStorage["IPMEmployeeScoreListScoreTo"],
                StatusDelimited: localStorage["IPMEmployeeScoreListStatusDelimited"],
                //RunDate//
                DateFromFrom: localStorage["IPMEmployeeScoreListDateFromFrom"],
                DateFromTo: localStorage["IPMEmployeeScoreListDateFromTo"],
                DateToFrom: localStorage["IPMEmployeeScoreListDateToFrom"],
                DateToTo: localStorage["IPMEmployeeScoreListDateToTo"],
                //DateEffective//
                DateEffectiveFromFrom: localStorage["MovementListDateEffectiveFromFrom"],
                DateEffectiveFromTo: localStorage["MovementListDateEffectiveFromTo"],
                DateEffectiveToFrom: localStorage["MovementListDateEffectiveToFrom"],
                DateEffectiveToTo: localStorage["MovementListDateEffectiveToTo"],
                //
                ShowForEvaluation: $("#cbShowForEvaluation").prop("checked"),
                ShowNoScore: $("#cbShowNoScore").prop("checked"),
                isShowAll: $("#cbShowAllEmployeeScore").prop("checked")
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabScoreList');
            EmployeeScoreLoadOnce = true;
            EmployeeFinalScoreLoadOnce = false;
        },

        DeleteSuccessFunction: function () {
            $("#divEmployeeScoreList #btnSearch").click();
        },

        //added
        VoidSuccessFunction: function () {
            $("#divEmployeeScoreList #btnSearch").click();
        },

        ApprovedSuccessFunction: function () {
            $("#divEmployeeScoreList #btnSearch").click();
        },

        ElementBinding: function () {
            var s = this;

            //NumberOnly($("#txtFilterID"));
            AmountOnly($("#txtFilterScoreFrom"));
            AmountOnly($("#txtFilterScoreTo"));

            $("#txtFilterDateFromFrom, #txtFilterDateFromTo, \
            #txtFilterDateToFrom, #txtFilterDateToTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#txtRerunDateFrom, #txtRerunDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $('#txtRerunDateFrom, #txtRerunDateTo').datetimepicker().on('dp.show', function () {
                $('#divEmployeeScoreSummaryRerunBodyModal .modal-body').css({ 'overflow': 'visible' });
                $('#divEmployeeScoreSummaryRerunModal.modal').css({ 'overflow': 'visible' });
            }).on('dp.hide', function () {
                $('#divEmployeeScoreSummaryRerunBodyModal .modal-body').css({ 'overflow': 'auto' });
                $('#divEmployeeScoreSummaryRerunModal.modal').css({ 'overflow': 'auto' });
            })

            $("#divEmployeeScoreList #btnSearch").click(function () {
                var param = {
                    TransSummaryIDDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedID").value,
                    Description: $("#txtFilterDescription").val(),
                    IsActiveDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedIsActive").value,
                    NameDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedName").value,
                    ParentOrgGroup: $("#txtFilterParentOrgGroup").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    ScoreFrom: $("#txtFilterScoreFrom").val(),
                    ScoreTo: $("#txtFilterScoreTo").val(),
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value,
                    DateFromFrom: $("#txtFilterDateFromFrom").val(),
                    DateFromTo: $("#txtFilterDateFromTo").val(),
                    DateToFrom: $("#txtFilterDateToFrom").val(),
                    DateToTo: $("#txtFilterDateToTo").val(),
                    DateEffectiveFromFrom: $("#txtFilterDateFromFrom").val(),
                    DateEffectiveFromTo: $("#txtFilterDateFromTo").val(),
                    DateEffectiveToFrom: $("#txtFilterDateToFrom").val(),
                    DateEffectiveToTo: $("#txtFilterDateToTo").val(),
                    ShowForEvaluation: $("#cbShowForEvaluation").prop("checked"),
                    ShowNoScore: $("#cbShowNoScore").prop("checked"),
                    isShowAll: $("#cbShowAllEmployeeScore").prop("checked")
                };
                s.SetLocalStorage();
                ResetJQGridState("tblIPMEmployeeScoreList");
                s.LoadJQGrid(param);
            });

            $("#divEmployeeScoreList #btnReset").click(function () {
                $("#divEmployeeScoreList div.filterFields input[type='search']").val("");
                $("#divEmployeeScoreList div.filterFields select").val("");
                $("#divEmployeeScoreList div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedID").html("");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#multiSelectedIsActive").html("");
                $("#multiSelectedIsActiveOption label, #multiSelectedIsActiveOption input").prop("title", "add");
                $("#multiSelectedName").html("");
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedPosition").html("");
                $("#divEmployeeScoreList #btnSearch").click();
            });

            $("#btnRun").click(function () {
                LoadPartial(EmployeeScoreAddURL, "divRunEmployeeScoreBodyModal");
                $("#divRunEmployeeScoreModal").modal("show");
            });

            $("#btnBulkDelete").click(function () {

                var input = $('#tblIPMEmployeeScoreList').jqGrid('getGridParam', 'selarrrow');
                var statusList = [];

                for (var x = 0; x < input.length; x++) {
                    var rowdata = $("#tblIPMEmployeeScoreList").jqGrid("getRowData", input[x]);
                    statusList.push(rowdata["Status"]);
                }

                if (input.length == 0) {
                    //ModalAlert(MODAL_HEADER, "Please select atleast one task.");
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select atleast one record." + "</li></label><br />");
                }

                else if (rowdata["Status"] != 'NEW') {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Only newly created employee score record/s can be deleted." + "</li></label><br />");
                }

                else {
                    $("#divEmployeeScoreListErrorMessage").html("");
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE,
                            "objEMSCommonJS.PostAjax(true \
                            , EmployeeScoreBulkDeleteListURL \
                            , objEmployeeScoreListJS.DeleteFormData() \
                            , '#divEmployeeScoreErrorMessage' \
                            , '#divEmployeeScoreModal #btnBulkDelete' \
                            , objEmployeeScoreListJS.DeleteSuccessFunction);",
                        "function");
                }
            });

            //added
            $("#btnBatchUpdates").click(function () {

                var input = $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "selarrrow");
                var statusList = [];

                for (var x = 0; x < input.length; x++) {
                    var rowdata = $("#tblIPMEmployeeScoreList").jqGrid("getRowData", input[x]);
                    statusList.push(rowdata["Status"]);
                }

                if (input.length == 0) {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select atleast one record" + "</li></label><br />");
                }

                else {
                    $("#divEmployeeScoreListErrorMessage").html("");
                    LoadPartial(EmployeeScoreBatchUpdatesURL, "divEmployeeScoreBatchUpdatesBodyModal");
                    $("#divEmployeeScoreBatchUpdatesModal").modal("show");
                }
            });


            
            //added
            $("#btnBulkVoid").click(function () {
                var input = $('#tblIPMEmployeeScoreList').jqGrid('getGridParam', 'selarrrow');
                var statusList = [];

                for (var x = 0; x < input.length; x++) {
                    var rowdata = $("#tblIPMEmployeeScoreList").jqGrid("getRowData", input[x]);
                    statusList.push(rowdata["Status"]);
                }

                if (input.length == 0) {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select atleast one record." + "</li></label><br />");
                }
                else if (objEmployeeScoreListJS.ArrayUnique(statusList)) {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select records with same current status only." + "</li></label><br />");
                }
                else if (rowdata["Status"] == 'VOID') {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select newly added Employees only. Current score status already set to VOID." + "</li></label><br />");
                }
                else if (rowdata["Status"] == 'FINALIZED') {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select newly added Employees only. Employee/s score record/s already finalized." + "</li></label><br />");
                }
                else if (rowdata["Status"] == 'FOR_REVISION') {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select newly added Employees only. Employee/s score record/s not yet revised by the evaluator." + "</li></label><br />");
                }
                else {
                    $("#divEmployeeScoreListErrorMessage").html("");
                    ModalConfirmation(MODAL_HEADER, "Do you want to set the status of this record/s to VOID?",
                        "objEMSCommonJS.PostAjax(true \
                            , EmployeeScoreBulkVoidListURL \
                            , objEmployeeScoreListJS.VoidFormData() \
                            , '#divEmployeeScoreErrorMessage' \
                            , '#divEmployeeScoreModal #btnBulkVoid' \
                            , objEmployeeScoreListJS.VoidSuccessFunction); ",
                        "function");
                }

            });

            //added
            $("#btnBulkApproved").click(function () {
                var input = $('#tblIPMEmployeeScoreList').jqGrid('getGridParam', 'selarrrow');
                var statusList = [];

                for (var x = 0; x < input.length; x++) {
                    var rowdata = $("#tblIPMEmployeeScoreList").jqGrid("getRowData", input[x]);
                    statusList.push(rowdata["Status"]);
                }

                if (input.length == 0) {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select atleast one record." + "</li></label><br />");
                }
                else if (objEmployeeScoreListJS.ArrayUnique(statusList)) {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select records with same current status only." + "</li></label><br />");
                }
                else if (rowdata["Status"] == 'VOID') {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Employee/s score record/s was set to VOID. Score cannot be approve." + "</li></label><br />");
                }
                else if (rowdata["Status"] == 'FINALIZED') {
                    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>" + "Employee/s score record has been finalized." + "</li></label><br />");
                }
                else {
                    $("#divEmployeeScoreListErrorMessage").html("");
                    ModalConfirmation(MODAL_HEADER, "Do you want to APPROVE this score?",
                        "objEMSCommonJS.PostAjax(true \
                            , EmployeeScoreBulkApprovedListURL \
                            , objEmployeeScoreListJS.ApprovedFormData() \
                            , '#divEmployeeScoreErrorMessage' \
                            , '#divEmployeeScoreModal #btnBulkApproved' \
                            , objEmployeeScoreListJS.ApprovedSuccessFunction); ",
                        "function");
                }

            });

            $("#cbShowForEvaluation, #cbShowNoScore, #cbShowAllEmployeeScore").click(function () {
                $("#divEmployeeScoreList #btnSearch").click();
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterID"
                , GetRunIDAutoCompleteURL, 20, "multiSelectedID");

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedStatus", EmployeeScoreStatusDropDownURL);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterName"
                , GetEmployeeAutoCompleteURL, 20, "multiSelectedName");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , GetOrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , GetPositionAutoCompleteURL, 20, "multiSelectedPosition");

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeScoreListJS.ExportFunction()",
                    "function");
            });

            $("#btnEditDescription").click(function () {
                $("#divEmployeeScoreDescriptionErrorMessage").html("");
                $("#divEmployeeScoreDescriptionErrorMessage").css({"display": "none"});
                $("#txtDescription").removeClass("errMessage");
                if ($("#txtDescription").val() != "") {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeScoreUpdateDescriptionPostURL \
                        , objEmployeeScoreListJS.GetFormData() \
                        , '' \
                        , '#divEmployeeScoreSummaryModal #btnEditDescription' \
                        , objEmployeeScoreListJS.EditSuccessFunction, null, null, false);",
                        "function");
                }
                else {
                    $("#divEmployeeScoreDescriptionErrorMessage").html("<label class=\"errMessage\"><li>Report Name" + SUFF_REQUIRED + "</li></label><br />");
                    $("#divEmployeeScoreDescriptionErrorMessage").css({ "display": "block" });
                    $("#txtDescription").addClass("errMessage");
                }
            });

            $("#btnRerun").click(function () {
                $("#txtRerunDateFrom").val($("#lblRunDateFrom").text());
                $("#txtRerunDateTo").val($("#lblRunDateTo").text());
                $("#divEmployeeScoreSummaryRerunModal").modal("show");
            });
            
            $("#btnRerunSave").click(function () {
                $("#divRerunErrorMessage").html("");
                $("#divRerunErrorMessage").css({ "display": "none" });
                $("#txtRerunDateFrom, #txtRerunDateTo").removeClass("errMessage");
                if ($("#txtRerunDateFrom").val() != "" && $("#txtRerunDateTo").val() != "") {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                        "objEMSCommonJS.PostAjaxWithBeforeSend(true \
                        , EmployeeScoreRerunPostURL \
                        , objEmployeeScoreListJS.RerunGetFormData() \
                        , '' \
                        , '#divEmployeeScoreSummaryRerunModal #btnRerunSave' \
                        , objEmployeeScoreListJS.RerunSuccessFunction \
                        , null \
                        , objEmployeeScoreListJS.BeforeSendFunction \
                        , true \
                        , false );",
                        "function");
                }
                else {
                    $("#divRerunErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                    $("#divRerunErrorMessage").css({ "display": "block" });
                    if ($("#txtRerunDateFrom").val() == "")
                        $("#txtRerunDateFrom").addClass("errMessage");
                    if ($("#txtRerunDateTo").val() == "")
                        $("#txtRerunDateTo").addClass("errMessage");
                }
            });


            var enumData = [];
            enumData.push({ ID: "YES", Description: "YES" });
            enumData.push({ ID: "NO", Description: "NO" });
            objEMSCommonJS.BindFilterMultiSelectEnumLocalData("multiSelectedIsActive", enumData);


            //LIST OF FINAL SCORE
            $(".tablinks").find("span:contains('IPM Consolidated Score')").parent("button").click(function () {
                if (!EmployeeFinalScoreLoadOnce) {
                    objEmployeeFinalScoreListJS.LoadFinalScore({
                    });
                    EmployeeFinalScoreLoadOnce = true;
                }
            });

        },

        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "sortorder")
                + "&TransSummaryIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedID").value
                + "&Description=" + $("#txtFilterDescription").val()
                + "&IsActiveDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedIsActive").value
                + "&NameDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedName").value
                + "&ParentOrgGroup=" + $("#txtFilterParentOrgGroup").val()
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                + "&ScoreFrom=" + $("#txtFilterScoreFrom").val()
                + "&ScoreTo=" + $("#txtFilterScoreTo").val()
                + "&StatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value
                + "&DateFromFrom=" + $("#txtFilterDateFromFrom").val()
                + "&DateFromTo=" + $("#txtFilterDateFromTo").val()
                + "&DateToFrom=" + $("#txtFilterDateToFrom").val()
                + "&DateToTo=" + $("#txtFilterDateToTo").val()
                + "&ShowForEvaluation=" + $("#cbShowForEvaluation").prop("checked")
                + "&ShowNoScore=" + $("#cbShowNoScore").prop("checked")

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },


        CheckOnGoingTransaction: function () {
            isRunOnGoing = true;
            isCheckOnLoad = true;
            clearTimeout(timer);
            objEmployeeScoreListJS.DisplayProgressBar("#divEmployeeScoreSummaryModal", "#frmRerunEmployeeScore");
        },

        EditSuccessFunction: function () {
            $("#divEmployeeScoreList #btnSearch").click();
        },

        GetFormData: function () {
            var formData = new FormData($('#frmEmployeeScore').get(0));
            formData.append("Description.RunID", $("#hdnRunID").val());
            formData.append("Description.Description", $("#txtDescription").val());
            formData.append("Description.IsTransActive", $("#cbIsTransActive").prop("checked"));
            return formData;
        },

        RerunGetFormData: function () {
            var formData = new FormData($('#frmEmployeeScore').get(0));
            formData.append("Rerun.RunID", $("#hdnRunID").val());
            formData.append("Rerun.DateFrom", $("#txtRerunDateFrom").val());
            formData.append("Rerun.DateTo", $("#txtRerunDateTo").val());
            return formData;
        },

        RerunSuccessFunction: function (data) {
            isRunOnGoing = false;
            if (data.Result.TransSummaryID != 0) {
                $("#btnSearch").click();
                $("#divRunEmployeeScoreModal").modal("hide");
                objEmployeeScoreListJS.ViewSummary(data.Result.TransSummaryID, false);
            }
            else
                ModalAlert(MODAL_HEADER, data.Result.Message);
        },

        DeleteFormData: function () {

            var input = $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "selarrrow");
            var formData = new FormData($('#frmEmployeeScore').get(0));
            var ctr1 = 0;

            for (var x = 0; x < input.length; x++) {
                formData.append("BulkDelete.IDs[" + ctr1 + "]", input[x]);
                ctr1++;
            }

            return formData;
        },

        VoidFormData: function () {

            var input = $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "selarrrow");
            var formData = new FormData($('#frmEmployeeScore').get(0));
            var ctr1 = 0;

            for (var x = 0; x < input.length; x++) {
                formData.append("BulkVoid.IDs[" + ctr1 + "]", input[x]);
                ctr1++;
            }

            formData.append("EmployeeScore.Status", "VOID");
            return formData;
        },

        ApprovedFormData: function () {

            var input = $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "selarrrow");
            var formData = new FormData($('#frmEmployeeScore').get(0));
            var ctr1 = 0;

            for (var x = 0; x < input.length; x++) {
                formData.append("BulkApproved.IDs[" + ctr1 + "]", input[x]);
                ctr1++;
            }

            formData.append("EmployeeScore.Status", "APPROVED");
            return formData;
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMEmployeeScoreList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMEmployeeScoreList"));
            var moveFilterFields = function () {
                var intialHeight = $("#divEmployeeScoreList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divEmployeeScoreList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divEmployeeScoreList #filterFieldsContainer");
                });
                $("#divEmployeeScoreList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divEmployeeScoreList div.filterFields").unbind("keyup");
                $("#divEmployeeScoreList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divEmployeeScoreList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMEmployeeScoreList").jqGrid("GridUnload");
            $("#tblIPMEmployeeScoreList").jqGrid("GridDestroy");
            $("#tblIPMEmployeeScoreList").jqGrid({
                url: EmployeeScoreListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Run ID","Is Active", "Report Name", "Run Date From", "Run Date To", "Employee", "Score"
                    , "Status", "Org. Group", "Position", "Parent Org. Group", "IPM Date From", "IPM Date To", "IPM Month/s", "Movement Date From", "Movement Date To"],
                colModel: [
                    { hidden: true },
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center", sortable: true },
                    { width: 150, key: false, name: "TransSummaryID", index: "TransSummaryID", align: "center", sortable: true, formatter: objEmployeeScoreListJS.SummaryLink },
                    { width: 100, name: "IsActive", index: "IsActive", align: "center", sortable: true },
                    { width: 200, name: "Description", index: "Description", align: "left", sortable: true },
                    { width: 100, name: "TDateFrom", index: "TDateFrom", editable: true, align: "center" },
                    { width: 100, name: "TDateTo", index: "TDateTo", editable: true, align: "center" },
                    { width: 300, name: "Employee", index: "Employee", editable: true, align: "left", formatter: objEmployeeScoreListJS.ViewLink },
                    { width: 100, name: "Score", Score: "Score", editable: true, align: "right" },
                    { width: 120, name: "Status", index: "Status", editable: true, align: "center" },
                    { width: 150, name: "OrgGroup", index: "OrgGroup", editable: true, align: "left" },
                    { width: 150, name: "Position", index: "Position", editable: true, align: "left" },
                    { width: 350, name: "ParentOrgGroup", index: "ParentOrgGroup", editable: true, align: "left" },
                    { width: 150, name: "PDateFrom", index: "PDateFrom", editable: true, align: "center", sortable: false },
                    { width: 150, name: "PDateTo", index: "PDateTo", editable: true, align: "center", sortable: false },
                    { width: 150, name: "IPMMonths", index: "IPMMonths", editable: true, align: "center", sortable: false },
                    { width: 150, name: "DateEffectiveFrom", index: "DateEffectiveFrom", editable: true, align: "center" },
                    { width: 150, name: "DateEffectiveTo", index: "DateEffectiveTo", editable: true, align: "center" },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false
                },
                emptyrecords: "No records to display",
                multiselect: true,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblIPMEmployeeScoreList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    //WIP
                    //setTimeout(function () { $(".jqgrid-id-link:first").click(); }, 500);

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMEmployeeScoreList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divEmployeeScoreList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divEmployeeScoreList .jqgfirstrow td:nth-child(" + (n + 3) + ")");
                        });

                        $("#tblIPMEmployeeScoreList .jqgrid-id-link").click(function () {
                            $('#divEmployeeScoreModal').modal('show');
                        });
                    }

                    if (localStorage["EmployeeScoreListFilterOption"] != undefined) {
                        $("#divEmployeeScoreList #chkFilter").prop('checked', JSON.parse(localStorage["EmployeeScoreListFilterOption"]));
                    }
                    objEmployeeScoreListJS.ShowHideFilter();

                    $("#divEmployeeScoreList #chkFilter").on('change', function () {
                        objEmployeeScoreListJS.ShowHideFilter();
                        localStorage["EmployeeScoreListFilterOption"] = $("#divEmployeeScoreList #chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divEmployeeScoreList #gview_tblIPMEmployeeScoreList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#divEmployeeScoreList table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#divEmployeeScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#divEmployeeScoreList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#divEmployeeScoreList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#divEmployeeScoreList table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                    });
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblIPMEmployeeScoreList");
                    moveFilterFields();
                },
            }).navGrid("#divEmployeeScoreList #divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            //$("#tblIPMEmployeeScoreList_toppager_center").css('padding-left', '150px');
            jQuery("#divEmployeeScoreList .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery("#divEmployeeScoreList .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#divEmployeeScoreList #lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },
            

        SetLocalStorage: function () {
            //localStorage["IPMEmployeeScoreListID"] = $("#txtFilterID").val();
            localStorage["IPMEmployeeScoreListIDDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedID").value;
            localStorage["IPMEmployeeScoreListIDDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedID").text;

            localStorage["IPMEmployeeScoreListIsActiveDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedIsActive").value;
            localStorage["IPMEmployeeScoreListIsActiveDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedIsActive").text;

            localStorage["IPMEmployeeScoreListDescription"] = $("#txtFilterDescription").val();

            localStorage["IPMEmployeeScoreListNameDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedName").value;
            localStorage["IPMEmployeeScoreListNameDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedName").text;

            localStorage["IPMEmployeeScoreListParentOrgGroup"] = $("#txtFilterParentOrgGroup").val();

            localStorage["IPMEmployeeScoreListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["IPMEmployeeScoreListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;

            localStorage["IPMEmployeeScoreListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["IPMEmployeeScoreListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;

            localStorage["IPMEmployeeScoreListScoreFrom"] = $("#txtFilterScoreFrom").val();
            localStorage["IPMEmployeeScoreListScoreTo"] = $("#txtFilterScoreTo").val();

            localStorage["IPMEmployeeScoreListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value;
            localStorage["IPMEmployeeScoreListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").text;

            localStorage["IPMEmployeeScoreListDateFromFrom"] = $("#txtFilterDateFromFrom").val();
            localStorage["IPMEmployeeScoreListDateFromTo"] = $("#txtFilterDateFromTo").val();

            localStorage["IPMEmployeeScoreListDateToFrom"] = $("#txtFilterDateToFrom").val();
            localStorage["IPMEmployeeScoreListDateToTo"] = $("#txtFilterDateToTo").val();
        },

        GetLocalStorage: function () {

            //$("#txtFilterID").val(localStorage["IPMEmployeeScoreListID"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedID"
                , "IPMEmployeeScoreListIDDelimited"
                , "IPMEmployeeScoreListIDDelimitedText");

            
            $("#txtFilterDescription").val(localStorage["IPMEmployeeScoreListDescription"]);
            
            objEMSCommonJS.SetMultiSelectList("multiSelectedIsActive"
                            , "IPMEmployeeScoreListIsActiveDelimited"
                            , "IPMEmployeeScoreListIsActiveDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedName"
                            , "IPMEmployeeScoreListNameDelimited"
                            , "IPMEmployeeScoreListNameDelimitedText");
            
            $("#txtFilterParentOrgGroup").val(localStorage["IPMEmployeeScoreListParentOrgGroup"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "IPMEmployeeScoreListOrgGroupDelimited"
                , "IPMEmployeeScoreListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "IPMEmployeeScoreListPositionDelimited"
                , "IPMEmployeeScoreListPositionDelimitedText");

            $("#txtFilterScoreFrom").val(localStorage["IPMEmployeeScoreListScoreFrom"]);
            $("#txtFilterScoreTo").val(localStorage["IPMEmployeeScoreListScoreTo"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedStatus"
                , "IPMEmployeeScoreListStatusDelimited"
                , "IPMEmployeeScoreListStatusDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedSourceType"
                , "IPMKPIListSourceTypeDelimited"
                , "IPMKPIListSourceTypeDelimitedText");

            $("#txtFilterDateFromFrom").val(localStorage["IPMEmployeeScoreListDateFromFrom"]);
            $("#txtFilterDateFromTo").val(localStorage["IPMEmployeeScoreListDateFromTo"]);
            $("#txtFilterDateToFrom").val(localStorage["IPMEmployeeScoreListDateToFrom"]);
            $("#txtFilterDateToTo").val(localStorage["IPMEmployeeScoreListDateToTo"]);
        },

        SummaryLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='' onclick=\"return objEmployeeScoreListJS.ViewSummary(" + rowObject.TransSummaryID + "," + false + ");" +
                "\">" + objEMSCommonJS.JQGridIDFormat(rowObject.TransSummaryID) + "</a>";
        },


        ViewSummary: function (TransSummaryID, ReDisplay) {
            isEnd = 0;
            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    if (ReDisplay == false) {
                        objEmployeeScoreListJS.CheckOnGoingTransaction();
                    }

                    $("#lblRunID").text(objEMSCommonJS.JQGridIDFormat(data.Result.ID));
                    $("#hdnRunID").val(data.Result.ID);
                    $("#lblIsTransActive").text(data.Result.IsTransActive);
                    $("#cbIsTransActive").prop("checked", data.Result.IsTransActive);
                    $("#txtDescription").val(data.Result.Description);
                    $("#lblDescription").text(data.Result.Description);
                    $("#lblCreatedDate").text(data.Result.CreatedDate);
                    $("#lblTotalEmployees").text(data.Result.TotalNumOfEmployees);
                    $("#lblEmployeesWithIPM").text(data.Result.EmployeesWithIPM);
                    $("#lblRatingEEMin").text(data.Result.RatingEEMin);
                    $("#lblRatingEEMax").text(data.Result.RatingEEMax);
                    $("#lblRatingEEEmployees").text(data.Result.RatingEEEmployees);
                    $("#lblRatingMEMin").text(data.Result.RatingMEMin);
                    $("#lblRatingMEMax").text(data.Result.RatingMEMax);
                    $("#lblRatingMEEmployees").text(data.Result.RatingMEEmployees);
                    $("#lblRatingSBEMin").text(data.Result.RatingSBEMin);
                    $("#lblRatingSBEMax").text(data.Result.RatingSBEMax);
                    $("#lblRatingSBEEmployees").text(data.Result.RatingSBEEmployees);
                    $("#lblRatingBEMin").text(data.Result.RatingBEMin);
                    $("#lblRatingBEMax").text(data.Result.RatingBEMax);
                    $("#lblRatingBEEmployees").text(data.Result.RatingBEEmployees);

                    $("#lblRunDateFrom").text(data.Result.TDateFrom);
                    $("#lblRunDateTo").text(data.Result.TDateTo);

                    $("#lblFilterBy").text(data.Result.FilterBy);
                    $("#lblFilterOrgGroup").text(data.Result.FilterOrgGroup);
                    $("#lblFilterPosition").text(data.Result.FilterPosition);
                    $("#lblFilterEmployee").text(data.Result.FilterEmployee);
                    $("#lblUseRecent").text(data.Result.FilterUseCurrent);
                    $("#lblOverride").text(data.Result.FilterOverride);
                    $("#lblLevelsBelow").text(data.Result.FilterIncludeLevelBelow);
                    $("#divEmployeeScoreSummaryModal").modal("show");

                    //NEW CHANGES 11-7-2022 BRITANICO - DEV
                    $("#lblTotalEmployeesWithIPM").text(data.Result.TotalEmployeesWithIPM);
                    $("#lblEmployeesWithMultiple").text(data.Result.EmployeesWithMultiple);
                    $("#lblTotalIPMResult").text(data.Result.TotalIPMResult);

                    clearInterval(Interval);
                    if (data.Result.RunStart != null && data.Result.RunEnd == null) {
                        Interval = setInterval(myTimer, 1000);
                        function myTimer() {
                            const d = new Date();
                            $("#lblRuntime").text(objEmployeeScoreListJS.TimeDuration(data.Result.RunStart, d));
                        }
                    }
                    else if (data.Result.RunStart != null && data.Result.RunEnd != null) {
                        $("#lblRuntime").text(objEmployeeScoreListJS.TimeDuration(data.Result.RunStart, data.Result.RunEnd));
                    }
                    else {
                        $("#lblRuntime").text("No Data");
                    }

                }
            };
            objEMSCommonJS.GetAjax(EmployeeScoreViewSummaryURL + "&ID=" + TransSummaryID, {}, "", GetSuccessFunction);
            
            return false;
        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + EmployeeScoreViewURL +
                "?ID=" + rowObject.ID + "', 'divEmployeeScoreBodyModal');\">" + rowObject.Employee + "</a>";
        },

        ShowHideFilter: function () {
            if ($("#divEmployeeScoreList #chkFilter").is(":checked")) {
                $("#divEmployeeScoreList .jqgfirstrow .filterFields").show();
            }
            else if ($("#divEmployeeScoreList #chkFilter").is(":not(:checked)")) {
                $("#divEmployeeScoreList .jqgfirstrow .filterFields").hide();
            }
        },

        ArrayUnique: function (data) {
                return data.length !== 0 && new Set(data).size !== 1;
        },

        DisplayProgressBar: function (modal, form) {
            transProgress = 0;
            transTotal = 0;

            var loopFunction = function () {
                objEMSCommonJS.GetAjaxNoLoading(GetTransProgressURL, {}, "", GetSuccessFunction, null, false);
            };

            var GetSuccessFunction = function (data) {
                var percentage = Math.round(data.Result.ProcessedEmployees / data.Result.EmployeesWithIPM * 100);
                transProgress = data.Result.ProcessedEmployees;
                transTotal = data.Result.EmployeesWithIPM != 0 ? data.Result.EmployeesWithIPM : transTotal;

                
                if (!data.Result.IsDone) {
                    
                    if (data.Result.EmployeesWithIPM == 0 && data.Result.ProcessedEmployees == 0) {
                        $(modal + " #lblProgressDetails").text("Initializing...");
                        $(modal + " #progressbar").css("width", 0 + "%");

                        $(modal + " #divTransProgress").show();
                        //$("#divOnGoingOverlay").show();
                        $(modal + " #divOnGoingOverlay").fadeIn();
                        $(modal + " #divOnGoingOverlay").width($(form).width());
                        $(modal + " #divOnGoingOverlay").height($(modal + " .modal-body").height() - $(modal + " #divTransProgress").height() - 20);
                        if (!isCheckOnLoad) {
                            Loading(false);
                        }
                        isEnd = 1;
                    }
                    else {
                        $(modal + " #lblProgressDetails").text("Processing " + transProgress + " out of " + transTotal + " Employees."
                            + (transProgress == transTotal ? " (Finalizing...)" : ""));
                        $(modal + " #progressbar").css("width", percentage + "%");

                        if (data.Result.EmployeesWithIPM != 0) {
                            $(modal + " #divTransProgress").show();
                            //$("#divOnGoingOverlay").show();
                            $(modal + " #divOnGoingOverlay").fadeIn();
                            $(modal + " #divOnGoingOverlay").width($(form).width());
                            $(modal + " #divOnGoingOverlay").height($(modal + " .modal-body").height() - $(modal + " #divTransProgress").height() - 20);
                            if (!isCheckOnLoad) {
                                Loading(false);
                            }
                        }
                        else {
                            if (isCheckOnLoad) {
                                $("#btnSearch").click();
                                isRunOnGoing = false;
                            }
                            $(modal + " #divTransProgress").hide();
                        }

                        isEnd = 2;
                        TransSumID = data.Result.TransSummaryID;
                      
                    }
                }
                else
                {
                    if (isCheckOnLoad) {
                        $("#btnSearch").click();
                        isRunOnGoing = false;
                    }
                    $(modal + " #divTransProgress").hide();

                    if (isEnd == 1) {
                        if (data.Result.ProcessedEmployees == 0 && data.Result.EmployeesWithIPM == 0) {

                            ModalAlert(MODAL_HEADER, "No employee score generated");

                        }
                        isEnd = 0;
                    }

                    if (isEnd == 2)
                    {

                        if ($('#divEmployeeScoreSummaryModal').is(':visible')) {
                            
                            $('#divEmployeeScoreSummaryModal').modal('hide');
                        }

                        if ($('#divRunEmployeeScoreModal').is(':visible')) {

                            $('#divRunEmployeeScoreModal').modal('hide');
                        }

                        ModalAlert(MODAL_HEADER, "IPM RUN ID: " + TransSumID + " Completed");

                        isEnd = 0;
                    }
                  
                }

                if (isRunOnGoing) {
                    timer = setTimeout(function () {
                        loopFunction();
                    }, 1000);
                }
                else {
                    $(modal + " #lblProgressDetails").text(transTotal + " out of " + transTotal + " Employees.");
                    $(modal + " #progressbar").css("width", 100 + "%");
                }

            };

            objEMSCommonJS.GetAjaxNoLoading(GetTransProgressURL, {}, "", GetSuccessFunction, null, false);
        },

        BeforeSendFunction: function () {
            $("#divEmployeeScoreSummaryRerunModal").modal("hide");
            $("#divModal").modal("hide");
            $("#lblRunDateFrom").text($("#txtRerunDateFrom").val());
            $("#lblRunDateTo").text($("#txtRerunDateTo").val());
            isRunOnGoing = true;
            isCheckOnLoad = false;
            objEmployeeScoreListJS.DisplayProgressBar('#divEmployeeScoreSummaryModal', '#frmRerunEmployeeScore');
        },
        LoadRater: function (ID = "") {
            $("#raterListview").html("");
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    $("#raterListview").append($('<option/>', {
                        value: data.Result[index].Result,
                        text: data.Result[index].Result
                    }));
                });
            };

            objEMSCommonJS.GetAjax(GetRaterByTransIDURL + "&ID="+ID, "", "", GetSuccessFunction);
        },

        TimeDuration: function(t0, t1) {
            let d = (new Date(t1)) - (new Date(t0));
            let weekdays = Math.floor(d / 1000 / 60 / 60 / 24 / 7);
            let days = Math.floor(d / 1000 / 60 / 60 / 24 - weekdays * 7);
            let hours = Math.floor(d / 1000 / 60 / 60 - weekdays * 7 * 24 - days * 24);
            let minutes = Math.floor(d / 1000 / 60 - weekdays * 7 * 24 * 60 - days * 24 * 60 - hours * 60);
            let seconds = Math.floor(d / 1000 - weekdays * 7 * 24 * 60 * 60 - days * 24 * 60 * 60 - hours * 60 * 60 - minutes * 60);
            let milliseconds = Math.floor(d - weekdays * 7 * 24 * 60 * 60 * 1000 - days * 24 * 60 * 60 * 1000 - hours * 60 * 60 * 1000 - minutes * 60 * 1000 - seconds * 1000);
            let t = {};
            ['weekdays', 'days', 'hours', 'minutes', 'seconds', 'milliseconds'].forEach(q => { if (eval(q) > 0) { t[q] = eval(q); } });


            if (hours.toString().length == 1)
                hours = "0" + hours;
            if (minutes.toString().length == 1)
                minutes = "0" + minutes;
            if (seconds.toString().length == 1)
                seconds = "0" + seconds;

            t = hours + ":" + minutes + ":" + seconds;
            return t;
        },
    };

    objEmployeeScoreListJS.Initialize();
});