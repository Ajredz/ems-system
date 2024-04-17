var objEmployeeScoreApprovalListJS;
const EmployeeScoreApprovalListURL = "/IPM/EmployeeScoreApproval?handler=List";
const EmployeeScoreApprovalViewURL = "/IPM/EmployeeScoreApproval/View";
const EmployeeKPIScoreListURL = "/IPM/EmployeeScoreApproval?handler=EmployeeKPIScoreGetList";
const EmployeeScoreApprovalViewSummaryURL = "/IPM/EmployeeScoreApproval?handler=ViewSummary";
const EmployeeScoreApprovalEditURL = "/IPM/EmployeeScoreApproval/Edit";
const EmployeeScoreApprovalEditPostURL = "/IPM/EmployeeScoreApproval/Edit";
const GetCheckExportListURL = "/IPM/EmployeeScoreApproval?handler=CheckExportList";
const DownloadExportListURL = "/IPM/EmployeeScoreApproval?handler=DownloadExportList";
const GetEmployeeScoreStatusHistoryURL = "/IPM/EmployeeScoreApproval?handler=EmployeeScoreStatusHistory";
const GetRunIDAutoCompleteURL = "/IPM/EmployeeScoreApproval?handler=RunIDAutoComplete";
const EmployeeScoreBatchUpdateURL = "/IPM/EmployeeScoreApproval/BatchUpdate";
const BatchStatusDropDownURL = "/IPM/EmployeeScoreApproval/BatchUpdate?handler=BatchStatusDropDown";

const CheckExportEmployeeURL = "/IPM/EmployeeScoreApproval?handler=CheckExportEmployee";
const DownloadExportEmployeeURL = "/IPM/EmployeeScoreApproval?handler=DownloadExportEmployee";

const GetEmployeeAutoCompleteURL = "/IPM/EmployeeScoreApproval?handler=EmployeeAutoComplete";
const GetOrgGroupAutoCompleteURL = "/IPM/EmployeeScoreApproval?handler=OrgTypeAutoComplete";
const GetOrgGroupFilteredAutoCompleteURL = "/IPM/EmployeeScoreApproval?handler=OrgTypeFilteredAutoComplete";
const GetEmployeeFilteredAutoCompleteURL = "/IPM/EmployeeScoreApproval?handler=EmployeeFilteredAutoComplete";
const GetPositionAutoCompleteURL = "/IPM/EmployeeScoreApproval?handler=PositionAutoComplete";
const EmployeeScoreStatusDropDownURL = "/IPM/EmployeeScoreApproval?handler=ReferenceValue&RefCode=EMP_SCORE_STATUS";
const GetDefaultPassingScoreURL = "/IPM/EmployeeScore?handler=ReferenceValue&RefCode=DEFAULT_PASS_SCORE";
const GetRatingGradesDropdownURL = "/IPM/EmployeeScoreApproval?handler=RatingGrades";


//var deleteForm = new FormData();

$(document).ready(function () {
    objEmployeeScoreApprovalListJS = {
        Initialize: function () {
            $("#divEmployeeScoreSummaryBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            var param = {
                TransSummaryIDDelimited: localStorage["IPMEmployeeScoreApprovalListIDDelimited"],
                Description: localStorage["IPMEmployeeScoreApprovalListDescription"],
                NameDelimited: localStorage["IPMEmployeeScoreApprovalListNameDelimited"],
                ParentOrgGroup: localStorage["IPMEmployeeScoreApprovalListParentOrgGroup"],
                OrgGroupDelimited: localStorage["IPMEmployeeScoreApprovalListOrgGroupDelimited"],
                PositionDelimited: localStorage["IPMEmployeeScoreApprovalListPositionDelimited"],
                ScoreFrom: localStorage["IPMEmployeeScoreApprovalListFrom"],
                ScoreTo: localStorage["IPMEmployeeScoreApprovalListTo"],
                StatusDelimited: localStorage["IPMEmployeeScoreApprovalListStatusDelimited"],
                DateFromFrom: localStorage["IPMEmployeeScoreApprovalListDateFromFrom"],
                DateFromTo: localStorage["IPMEmployeeScoreApprovalListDateFromTo"],
                DateToFrom: localStorage["IPMEmployeeScoreApprovalListDateToFrom"],
                DateToTo: localStorage["IPMEmployeeScoreApprovalListDateToTo"],
                ShowForEvaluation: $("#cbShowForEvaluation").prop("checked"),
                ShowNoScore: $("#cbShowNoScore").prop("checked")
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            $(".multiselect-item:contains('NEW')").remove();
            $(".multiselect-item:contains('WIP')").remove();

            $("#txtFilterID").removeClass("errMessage");
            $("#divEmployeeScoreApprovalListErrorMessage").html("");
            //if (objEMSCommonJS.GetMultiSelectList("multiSelectedID").value == "") {
            //    $("#divEmployeeScoreApprovalListErrorMessage").html("<label class=\"errMessage\"><li>Run ID is required.</li></label><br />");
            //    $("#txtFilterID").addClass("errMessage");
            //}
        },

        DeleteSuccessFunction: function () {
            $("#divEmployeeScoreApprovalList #btnSearch").click();
        },

        ElementBinding: function () {
            var s = this;

            //NumberOnly($("#txtFilterID"));

            $("#txtFilterDateFromFrom, #txtFilterDateFromTo, \
            #txtFilterDateToFrom, #txtFilterDateToTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#divEmployeeScoreApprovalList #btnSearch").click(function () {
                var param = {
                    TransSummaryIDDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedID").value,
                    Description: $("#txtFilterDescription").val(),
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
                    ShowForEvaluation: $("#cbShowForEvaluation").prop("checked"),
                    ShowNoScore: $("#cbShowNoScore").prop("checked")
                };
                s.SetLocalStorage();
                ResetJQGridState("tblIPMEmployeeScoreList");
                s.LoadJQGrid(param);

                $("#txtFilterID").removeClass("errMessage");
                $("#divEmployeeScoreApprovalListErrorMessage").html("");
                //if (objEMSCommonJS.GetMultiSelectList("multiSelectedID").value == "") {
                //    $("#divEmployeeScoreApprovalListErrorMessage").html("<label class=\"errMessage\"><li>Run ID is required.</li></label><br />");
                //    $("#txtFilterID").addClass("errMessage");
                //}
            });

            $("#divEmployeeScoreApprovalList #btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedID").html("");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#multiSelectedName").html("");
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedPosition").html("");
                $("#divEmployeeScoreApprovalList #btnSearch").click();
            });

            $("#btnBatchUpdate").click(function () {

                var input = $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "selarrrow");
                var statusList = [];

                for (var x = 0; x < input.length; x++) {
                    var rowdata = $("#tblIPMEmployeeScoreList").jqGrid("getRowData", input[x]);
                    statusList.push(rowdata["Status"]);
                }

                if (input.length == 0) {
                    //ModalAlert(MODAL_HEADER, "Please select atleast one task.");
                    $("#divEmployeeScoreApprovalListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select atleast one record" + "</li></label><br />");
                }
                else if (objEmployeeScoreApprovalListJS.ArrayUnique(statusList)) {
                    //ModalAlert(MODAL_HEADER, "Please select tasks with same status.");
                    $("#divEmployeeScoreApprovalListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select records with same current status only" + "</li></label><br />");
                }
                else if (rowdata["Status"] == 'FINALIZED') {
                    $("#divEmployeeScoreApprovalListErrorMessage").html("<label class=\"errMessage\"><li>" + "Employee score record/s already finalized." + "</li></label><br />");
                }
                else if (rowdata["Status"] == 'FOR_REVISION') {
                    $("#divEmployeeScoreApprovalListErrorMessage").html("<label class=\"errMessage\"><li>" + "Employee score record/s not yet revised by the evaluator." + "</li></label><br />");
                }
                else {
                    $("#divEmployeeScoreApprovalListErrorMessage").html("");
                    LoadPartial(EmployeeScoreBatchUpdateURL, "divEmployeeScoreApprovalBatchUpdateBodyModal");
                    $("#divEmployeeScoreApprovalBatchUpdateModal").modal("show");
                }
            });

            $("#cbShowForEvaluation, #cbShowNoScore").click(function () {
                $("#divEmployeeScoreApprovalList #btnSearch").click();
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

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeScoreApprovalListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "sortorder")
                + "&TransSummaryIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedID").value
                + "&Description=" + $("#txtFilterDescription").val()
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


        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMEmployeeScoreList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMEmployeeScoreList"));
            var moveFilterFields = function () {
                var intialHeight = $("#divEmployeeScoreApprovalList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divEmployeeScoreApprovalList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divEmployeeScoreApprovalList #filterFieldsContainer");
                });
                $("#divEmployeeScoreApprovalList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divEmployeeScoreApprovalList div.filterFields").unbind("keyup");
                $("#divEmployeeScoreApprovalList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divEmployeeScoreApprovalList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMEmployeeScoreList").jqGrid("GridUnload");
            $("#tblIPMEmployeeScoreList").jqGrid("GridDestroy");
            $("#tblIPMEmployeeScoreList").jqGrid({
                url: EmployeeScoreApprovalListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Run ID", "Report Name", "Run Date From", "Run Date To", "Employee", "Score"
                    , "Status", "Org. Group", "Position", "Parent Org. Group", "KPI Position Date From"
                    , "KPI Position Date To"],
                colModel: [
                    { hidden: true },
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center", sortable: true },
                    { hidden: true, width: 150, key: false, name: "TransSummaryID", index: "TransSummaryID", align: "center", sortable: true, formatter: objEmployeeScoreApprovalListJS.SummaryLink },
                    { width: 200, name: "Description", index: "Description", align: "left", sortable: true },
                    { width: 100, name: "TDateFrom", index: "TDateFrom", editable: true, align: "center" },
                    { width: 100, name: "TDateTo", index: "TDateTo", editable: true, align: "center" },
                    { width: 300, name: "Employee", index: "Employee", editable: true, align: "left", formatter: objEmployeeScoreApprovalListJS.ViewLink },
                    { width: 100, name: "Score", Score: "Score", editable: true, align: "right" },
                    { width: 120, name: "Status", index: "Status", editable: true, align: "center" },
                    { width: 150, name: "OrgGroup", index: "OrgGroup", editable: true, align: "left" },
                    { width: 150, name: "Position", index: "Position", editable: true, align: "left" },
                    { width: 350, name: "ParentOrgGroup", index: "ParentOrgGroup", editable: true, align: "left" },
                    { width: 150, name: "PDateFrom", index: "PDateFrom", editable: true, align: "center", sortable: false },
                    { width: 150, name: "PDateTo", index: "PDateTo", editable: true, align: "center", sortable: false }

                ],
                toppager: $("#divEmployeeScoreApprovalList #divPager"),
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

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMEmployeeScoreList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divEmployeeScoreApprovalList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divEmployeeScoreApprovalList .jqgfirstrow td:nth-child(" + (n + 3) + ")");
                        });

                        $("#divEmployeeScoreApprovalList #tblIPMEmployeeScoreList .jqgrid-id-link").click(function () {
                            $('#divEmployeeScoreApprovalModal').modal('show');
                        });
                    }

                    if (localStorage["EmployeeScoreListFilterOption"] != undefined) {
                        $("#divEmployeeScoreApprovalList #chkFilter").prop('checked', JSON.parse(localStorage["EmployeeScoreListFilterOption"]));
                    }
                    objEmployeeScoreApprovalListJS.ShowHideFilter();

                    $("#divEmployeeScoreApprovalList #chkFilter").on('change', function () {
                        objEmployeeScoreApprovalListJS.ShowHideFilter();
                        localStorage["EmployeeScoreListFilterOption"] = $("#divEmployeeScoreApprovalList #chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#gview_tblIPMEmployeeScoreList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#divEmployeeScoreApprovalList table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#divEmployeeScoreApprovalList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#divEmployeeScoreApprovalList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeScoreApprovalList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#divEmployeeScoreApprovalList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeScoreApprovalList table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#divEmployeeScoreApprovalList table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreApprovalList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
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
            }).navGrid("#divEmployeeScoreApprovalList #divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery("#divEmployeeScoreApprovalList .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery("#divEmployeeScoreApprovalList .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#divEmployeeScoreApprovalList #lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage: function () {
            //localStorage["IPMEmployeeScoreApprovalListID"] = $("#txtFilterID").val();

            localStorage["IPMEmployeeScoreApprovalListIDDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedID").value;
            localStorage["IPMEmployeeScoreApprovalListIDDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedID").text;

            localStorage["IPMEmployeeScoreApprovalListDescription"] = $("#txtFilterDescription").val();
            localStorage["IPMEmployeeScoreApprovalListNameDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedName").value;
            localStorage["IPMEmployeeScoreApprovalListNameDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedName").text;
            localStorage["IPMEmployeeScoreApprovalListParentOrgGroup"] = $("#txtFilterParentOrgGroup").val();
            localStorage["IPMEmployeeScoreApprovalListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["IPMEmployeeScoreApprovalListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;
            localStorage["IPMEmployeeScoreApprovalListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["IPMEmployeeScoreApprovalListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;
            localStorage["IPMEmployeeScoreApprovalListScoreFrom"] = $("#txtFilterScoreFrom").val();
            localStorage["IPMEmployeeScoreApprovalListScoreTo"] = $("#txtFilterScoreTo").val();
            localStorage["IPMEmployeeScoreApprovalListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value;
            localStorage["IPMEmployeeScoreApprovalListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").text;
            localStorage["IPMEmployeeScoreApprovalListDateFromFrom"] = $("#txtFilterDateFromFrom").val();
            localStorage["IPMEmployeeScoreApprovalListDateFromTo"] = $("#txtFilterDateFromTo").val();
            localStorage["IPMEmployeeScoreApprovalListDateToFrom"] = $("#txtFilterDateToFrom").val();
            localStorage["IPMEmployeeScoreApprovalListDateToTo"] = $("#txtFilterDateToTo").val();
        },

        GetLocalStorage: function () {
            //$("#txtFilterID").val(localStorage["IPMEmployeeScoreApprovalListID"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedID"
                , "IPMEmployeeScoreApprovalListIDDelimited"
                , "IPMEmployeeScoreApprovalListIDDelimitedText");

            $("#txtFilterDescription").val(localStorage["IPMEmployeeScoreApprovalListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedName"
                , "IPMEmployeeScoreApprovalListNameDelimited"
                , "IPMEmployeeScoreApprovalListNameDelimitedText");
            
            $("#txtFilterParentOrgGroup").val(localStorage["IPMEmployeeScoreApprovalListParentOrgGroup"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "IPMEmployeeScoreApprovalListOrgGroupDelimited"
                , "IPMEmployeeScoreApprovalListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "IPMEmployeeScoreApprovalListPositionDelimited"
                , "IPMEmployeeScoreApprovalListPositionDelimitedText");

            $("#txtFilterScoreFrom").val(localStorage["IPMEmployeeScoreApprovalListScoreFrom"]);
            $("#txtFilterScoreTo").val(localStorage["IPMEmployeeScoreApprovalListScoreTo"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedStatus"
                , "IPMEmployeeScoreApprovalListStatusDelimited"
                , "IPMEmployeeScoreApprovalListStatusDelimitedText");

            $("#txtFilterDateFromFrom").val(localStorage["IPMEmployeeScoreApprovalListDateFromFrom"]);
            $("#txtFilterDateFromTo").val(localStorage["IPMEmployeeScoreApprovalListDateFromTo"]);
            $("#txtFilterDateToFrom").val(localStorage["IPMEmployeeScoreApprovalListDateToFrom"]);
            $("#txtFilterDateToTo").val(localStorage["IPMEmployeeScoreApprovalListDateToTo"]);
        },

        SummaryLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='' onclick=\"return objEmployeeScoreApprovalListJS.ViewSummary(" + rowObject.TransSummaryID + ");" +
                "\">" + objEMSCommonJS.JQGridIDFormat(rowObject.TransSummaryID) + "</a>";
        },

        ViewSummary: function (TransSummaryID) {
            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    $("#lblRunID").text(objEMSCommonJS.JQGridIDFormat(data.Result.ID));
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
                }
            };
            objEMSCommonJS.GetAjax(EmployeeScoreApprovalViewSummaryURL + "&ID=" + TransSummaryID, {}, "", GetSuccessFunction);
            return false;
        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + EmployeeScoreApprovalViewURL +
                "?ID=" + rowObject.ID + "', 'divEmployeeScoreApprovalBodyModal');\">" + rowObject.Employee + "</a>";
        },

        ShowHideFilter: function () {
            if ($("#divEmployeeScoreApprovalList #chkFilter").is(":checked")) {
                $("#divEmployeeScoreApprovalList .jqgfirstrow .filterFields").show();
            }
            else if ($("#divEmployeeScoreApprovalList #chkFilter").is(":not(:checked)")) {
                $("#divEmployeeScoreApprovalList .jqgfirstrow .filterFields").hide();
            }
        },
        
        ArrayUnique: function (data) {
            return data.length !== 0 && new Set(data).size !== 1;
        },
    };

    objEmployeeScoreApprovalListJS.Initialize();
});