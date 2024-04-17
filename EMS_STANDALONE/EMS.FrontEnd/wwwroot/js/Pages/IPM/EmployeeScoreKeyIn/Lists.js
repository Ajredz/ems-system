var objEmployeeScoreKeyInListJS;
const EmployeeScoreKeyInListURL = "/IPM/EmployeeScoreKeyIn?handler=List";
const EmployeeKPIScoreListURL = "/IPM/EmployeeScoreKeyIn?handler=EmployeeKPIScoreGetList";
const EmployeeScoreKeyInViewURL = "/IPM/EmployeeScoreKeyIn/View";
const EmployeeScoreKeyInViewSummaryURL = "/IPM/EmployeeScoreKeyIn?handler=ViewSummary";
const EmployeeScoreKeyInEditURL = "/IPM/EmployeeScoreKeyIn/Edit";
const EmployeeScoreKeyInEditPostURL = "/IPM/EmployeeScoreKeyIn/Edit";
const GetCheckExportListURL = "/IPM/EmployeeScoreKeyIn?handler=CheckExportList";
const DownloadExportListURL = "/IPM/EmployeeScoreKeyIn?handler=DownloadExportList";
const GetEmployeeScoreStatusHistoryURL = "/IPM/EmployeeScoreKeyIn?handler=EmployeeScoreStatusHistory";
const GetRunIDAutoCompleteURL = "/IPM/EmployeeScoreKeyIn?handler=RunIDAutoComplete";

const CheckExportEmployeeURL = "/IPM/EmployeeScoreKeyIn?handler=CheckExportEmployee";
const DownloadExportEmployeeURL = "/IPM/EmployeeScoreKeyIn?handler=DownloadExportEmployee";

const GetEmployeeAutoCompleteURL = "/IPM/EmployeeScoreKeyIn?handler=EmployeeAutoComplete";
const GetOrgGroupAutoCompleteURL = "/IPM/EmployeeScoreKeyIn?handler=OrgTypeAutoComplete";
const GetOrgGroupFilteredAutoCompleteURL = "/IPM/EmployeeScoreKeyIn?handler=OrgTypeFilteredAutoComplete";
const GetEmployeeFilteredAutoCompleteURL = "/IPM/EmployeeScoreKeyIn?handler=EmployeeFilteredAutoComplete";
const GetPositionAutoCompleteURL = "/IPM/EmployeeScoreKeyIn?handler=PositionAutoComplete";
const EmployeeScoreStatusDropDownURL = "/IPM/EmployeeScoreKeyIn?handler=ReferenceValue&RefCode=EMP_SCORE_STATUS";
const GetDefaultPassingScoreURL = "/IPM/EmployeeScore?handler=ReferenceValue&RefCode=DEFAULT_PASS_SCORE";
const GetRatingGradesDropdownURL = "/IPM/EmployeeScoreKeyIn?handler=RatingGrades";

//var deleteForm = new FormData();
const _MaxScore = $("#hdnMaxValue").val();

$(document).ready(function () {
    objEmployeeScoreKeyInListJS = {
        Initialize: function () {
            $("#divEmployeeScoreSummaryBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            var param = {
                TransSummaryIDDelimited: localStorage["IPMEmployeeScoreKeyInListIDDelimited"],
                Description: localStorage["IPMEmployeeScoreKeyInListDescription"],
                NameDelimited: localStorage["IPMEmployeeScoreKeyInListNameDelimited"],
                ParentOrgGroup: localStorage["IPMEmployeeScoreKeyInListParentOrgGroup"],
                OrgGroupDelimited: localStorage["IPMEmployeeScoreKeyInListOrgGroupDelimited"],
                PositionDelimited: localStorage["IPMEmployeeScoreKeyInListPositionDelimited"],
                ScoreFrom: localStorage["IPMEmployeeScoreKeyInListScoreFrom"],
                ScoreTo: localStorage["IPMEmployeeScoreKeyInListScoreTo"],
                StatusDelimited: localStorage["IPMEmployeeScoreKeyInListStatusDelimited"],
                DateFromFrom: localStorage["IPMEmployeeScoreKeyInListDateFromFrom"],
                DateFromTo: localStorage["IPMEmployeeScoreKeyInListDateFromTo"],
                DateToFrom: localStorage["IPMEmployeeScoreKeyInListDateToFrom"],
                DateToTo: localStorage["IPMEmployeeScoreKeyInListDateToTo"],
                ShowForEvaluation: $("#cbShowForEvaluation").prop("checked"),
                ShowNoScore: $("#cbShowNoScore").prop("checked")
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            $("#txtFilterID").removeClass("errMessage");
            $("#divEmployeeScoreListErrorMessage").html("");
            //if (objEMSCommonJS.GetMultiSelectList("multiSelectedID").value == "") {
            //    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>Run ID is required.</li></label><br />");
            //    $("#txtFilterID").addClass("errMessage");
            //}
        },

        DeleteSuccessFunction: function () {
            $("#divEmployeeScoreKeyInList #btnSearch").click();
        },

        ElementBinding: function () {
            var s = this;

            //NumberOnly($("#txtFilterID"));

            $("#txtFilterDateFromFrom, #txtFilterDateFromTo, \
            #txtFilterDateToFrom, #txtFilterDateToTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#divEmployeeScoreKeyInList #btnSearch").click(function () {
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
                $("#divEmployeeScoreListErrorMessage").html("");
                //if (objEMSCommonJS.GetMultiSelectList("multiSelectedID").value == "") {
                //    $("#divEmployeeScoreListErrorMessage").html("<label class=\"errMessage\"><li>Run ID is required.</li></label><br />");
                //    $("#txtFilterID").addClass("errMessage");
                //}

            });

            $("#divEmployeeScoreKeyInList #btnReset").click(function () {
                $("#divEmployeeScoreKeyInList div.filterFields input[type='search']").val("");
                $("#divEmployeeScoreKeyInList div.filterFields select").val("");
                $("#divEmployeeScoreKeyInList div.filterFields input[type='checkbox']").prop("checked", true);
                $("#divEmployeeScoreKeyInList #multiSelectedID").html("");
                $("#divEmployeeScoreKeyInList #multiSelectedStatus").html("");
                $("#divEmployeeScoreKeyInList #multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#divEmployeeScoreKeyInList #multiSelectedName").html("");
                $("#divEmployeeScoreKeyInList #multiSelectedOrgGroup").html("");
                $("#divEmployeeScoreKeyInList #multiSelectedPosition").html("");
                $("#divEmployeeScoreKeyInList #btnSearch").click();
            });

            $("#cbShowForEvaluation, #cbShowNoScore").click(function () {
                $("#divEmployeeScoreKeyInList #btnSearch").click();
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

            $("#divEmployeeScoreKeyInList #btnExport").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeScoreKeyInListJS.ExportFunction()",
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
                var intialHeight = $("#divEmployeeScoreKeyInList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divEmployeeScoreKeyInList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divEmployeeScoreKeyInList #filterFieldsContainer");
                });
                $("#divEmployeeScoreKeyInList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divEmployeeScoreKeyInList div.filterFields").unbind("keyup");
                $("#divEmployeeScoreKeyInList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divEmployeeScoreKeyInList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMEmployeeScoreList").jqGrid("GridUnload");
            $("#tblIPMEmployeeScoreList").jqGrid("GridDestroy");
            $("#tblIPMEmployeeScoreList").jqGrid({
                url: EmployeeScoreKeyInListURL,
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
                    { hidden: true, width: 150, key: false, name: "TransSummaryID", index: "TransSummaryID", align: "center", sortable: true, formatter: objEmployeeScoreKeyInListJS.SummaryLink },
                    { width: 200, name: "Description", index: "Description", align: "left", sortable: true },
                    { width: 100, name: "TDateFrom", index: "TDateFrom", editable: true, align: "center" },
                    { width: 100, name: "TDateTo", index: "TDateTo", editable: true, align: "center" },
                    { width: 300, name: "Employee", index: "Employee", editable: true, align: "left", formatter: objEmployeeScoreKeyInListJS.ViewLink },
                    { width: 100, name: "Score", Score: "Score", editable: true, align: "right" },
                    { width: 120, name: "Status", index: "Status", editable: true, align: "center" },
                    { width: 150, name: "OrgGroup", index: "OrgGroup", editable: true, align: "left" },
                    { width: 150, name: "Position", index: "Position", editable: true, align: "left" },
                    { width: 350, name: "ParentOrgGroup", index: "ParentOrgGroup", editable: true, align: "left" },
                    { width: 150, name: "PDateFrom", index: "PDateFrom", editable: true, align: "center", sortable: false },
                    { width: 150, name: "PDateTo", index: "PDateTo", editable: true, align: "center", sortable: false },
                ],
                toppager: $("#divEmployeeScoreKeyInList #divPager"),
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
                multiselect: false,
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
                        $("#divEmployeeScoreKeyInList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divEmployeeScoreKeyInList .jqgfirstrow td:nth-child(" + (n + 3) + ")");
                        });

                        $("#divEmployeeScoreKeyInList #tblIPMEmployeeScoreList .jqgrid-id-link").click(function () {
                            $('#divEmployeeScoreKeyInModal').modal('show');
                        });
                    }

                    if (localStorage["EmployeeScoreListFilterOption"] != undefined) {
                        $("#divEmployeeScoreKeyInList #chkFilter").prop('checked', JSON.parse(localStorage["EmployeeScoreListFilterOption"]));
                    }
                    objEmployeeScoreKeyInListJS.ShowHideFilter();

                    $("#divEmployeeScoreKeyInList #chkFilter").on('change', function () {
                        objEmployeeScoreKeyInListJS.ShowHideFilter();
                        localStorage["EmployeeScoreListFilterOption"] = $("#divEmployeeScoreKeyInList #chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divEmployeeScoreKeyInList #gview_tblIPMEmployeeScoreList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#divEmployeeScoreKeyInList table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#divEmployeeScoreKeyInList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#divEmployeeScoreKeyInList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeScoreKeyInList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#divEmployeeScoreKeyInList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeScoreKeyInList table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#divEmployeeScoreKeyInList table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreKeyInList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
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
            }).navGrid("#divEmployeeScoreKeyInList #divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery("#divEmployeeScoreKeyInList .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery("#divEmployeeScoreKeyInList .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#divEmployeeScoreKeyInList #lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage: function () {
            //localStorage["IPMEmployeeScoreKeyInListID"] = $("#txtFilterID").val();
            localStorage["IPMEmployeeScoreKeyInListIDDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedID").value;
            localStorage["IPMEmployeeScoreKeyInListIDDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedID").text;

            localStorage["IPMEmployeeScoreKeyInListDescription"] = $("#txtFilterDescription").val();
            localStorage["IPMEmployeeScoreKeyInListNameDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedName").value;
            localStorage["IPMEmployeeScoreKeyInListNameDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedName").text;
            localStorage["IPMEmployeeScoreKeyInListParentOrgGroup"] = $("#txtFilterParentOrgGroup").val();
            localStorage["IPMEmployeeScoreKeyInListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["IPMEmployeeScoreKeyInListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;
            localStorage["IPMEmployeeScoreKeyInListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["IPMEmployeeScoreKeyInListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;
            localStorage["IPMEmployeeScoreKeyInListScoreFrom"] = $("#txtFilterScoreFrom").val();
            localStorage["IPMEmployeeScoreKeyInListScoreTo"] = $("#txtFilterScoreTo").val();
            localStorage["IPMEmployeeScoreKeyInListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value;
            localStorage["IPMEmployeeScoreKeyInListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").text;
            localStorage["IPMEmployeeScoreKeyInListDateFromFrom"] = $("#txtFilterDateFromFrom").val();
            localStorage["IPMEmployeeScoreKeyInListDateFromTo"] = $("#txtFilterDateFromTo").val();
            localStorage["IPMEmployeeScoreKeyInListDateToFrom"] = $("#txtFilterDateToFrom").val();
            localStorage["IPMEmployeeScoreKeyInListDateToTo"] = $("#txtFilterDateToTo").val();
        },

        GetLocalStorage: function () {
            //$("#txtFilterID").val(localStorage["IPMEmployeeScoreKeyInListID"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedID"
                , "IPMEmployeeScoreKeyInListIDDelimited"
                , "IPMEmployeeScoreKeyInListIDDelimitedText");

            $("#txtFilterDescription").val(localStorage["IPMEmployeeScoreKeyInListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedName"
                , "IPMEmployeeScoreKeyInListNameDelimited"
                , "IPMEmployeeScoreKeyInListNameDelimitedText");

            $("#txtFilterParentOrgGroup").val(localStorage["IPMEmployeeScoreKeyInListParentOrgGroup"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "IPMEmployeeScoreKeyInListOrgGroupDelimited"
                , "IPMEmployeeScoreKeyInListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "IPMEmployeeScoreKeyInListPositionDelimited"
                , "IPMEmployeeScoreKeyInListPositionDelimitedText");

            $("#txtFilterScoreFrom").val(localStorage["IPMEmployeeScoreKeyInListScoreFrom"]);
            $("#txtFilterScoreTo").val(localStorage["IPMEmployeeScoreKeyInListScoreTo"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedStatus"
                , "IPMEmployeeScoreKeyInListStatusDelimited"
                , "IPMEmployeeScoreKeyInListStatusDelimitedText");

            $("#txtFilterDateFromFrom").val(localStorage["IPMEmployeeScoreKeyInListDateFromFrom"]);
            $("#txtFilterDateFromTo").val(localStorage["IPMEmployeeScoreKeyInListDateFromTo"]);
            $("#txtFilterDateToFrom").val(localStorage["IPMEmployeeScoreKeyInListDateToFrom"]);
            $("#txtFilterDateToTo").val(localStorage["IPMEmployeeScoreKeyInListDateToTo"]);
        },

        SummaryLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='' onclick=\"return objEmployeeScoreKeyInListJS.ViewSummary(" + rowObject.TransSummaryID + ");" +
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
            objEMSCommonJS.GetAjax(EmployeeScoreKeyInViewSummaryURL + "&ID=" + TransSummaryID, {}, "", GetSuccessFunction);
            return false;
        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + EmployeeScoreKeyInViewURL +
                "?ID=" + rowObject.ID + "', 'divEmployeeScoreKeyInBodyModal');\">" + rowObject.Employee + "</a>";
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        }
    };

    objEmployeeScoreKeyInListJS.Initialize();
});