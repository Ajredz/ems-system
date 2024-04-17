var objMyKPIScoreListJS;
const MyKPIScoreListURL = "/IPM/MyKPIScores?handler=List";
const MyKPIScoreViewURL = "/IPM/MyKPIScores/View";
const GetCheckExportListURL = "/IPM/MyKPIScores?handler=CheckExportList";
const EmployeeKPIScoreListURL = "/IPM/MyKPIScores?handler=EmployeeKPIScoreGetList";

const CheckExportEmployeeURL = "/IPM/MyKPIScores?handler=CheckExportEmployee";
const DownloadExportEmployeeURL = "/IPM/MyKPIScores?handler=DownloadExportEmployee";

const GetOrgGroupAutoCompleteURL = "/IPM/MyKPIScores?handler=OrgTypeAutoComplete";
const GetOrgGroupFilteredAutoCompleteURL = "/IPM/MyKPIScores?handler=OrgTypeFilteredAutoComplete";
const GetEmployeeFilteredAutoCompleteURL = "/IPM/MyKPIScores?handler=EmployeeFilteredAutoComplete";
const GetPositionAutoCompleteURL = "/IPM/MyKPIScores?handler=PositionAutoComplete";
const GetRunIDAutoCompleteURL = "/IPM/MyKPIScores?handler=RunIDAutoComplete";
const GetRatingGradesDropdownURL = "/IPM/MyKPIScores?handler=RatingGrades";


$(document).ready(function () {
    objMyKPIScoreListJS = {
        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["IPMMyKPIScoreListID"],
                TransSummaryIDDelimited: localStorage["IPMMyKPIScoreListIDDelimited"],
                Description: localStorage["IPMMyKPIScoreListDescription"],
                OrgGroupDelimited: localStorage["IPMMyKPIScoreListOrgGroupDelimited"],
                PositionDelimited: localStorage["IPMMyKPIScoreListPositionDelimited"],
                ScoreFrom: localStorage["IPMMyKPIScoreListScoreFrom"],
                ScoreTo: localStorage["IPMMyKPIScoreListScoreTo"],
                DateFromFrom: localStorage["IPMMyKPIScoreListDateFromFrom"],
                DateFromTo: localStorage["IPMMyKPIScoreListDateFromTo"],
                DateToFrom: localStorage["IPMMyKPIScoreListDateToFrom"],
                DateToTo: localStorage["IPMMyKPIScoreListDateToTo"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            $("#txtFilterID").removeClass("errMessage");
            $("#divMyKPIScoreListErrorMessage").html("");
            //if (objEMSCommonJS.GetMultiSelectList("multiSelectedID").value == "") {
            //    $("#divMyKPIScoreListErrorMessage").html("<label class=\"errMessage\"><li>Run ID is required.</li></label><br />");
            //    $("#txtFilterID").addClass("errMessage");
            //}
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterRecordID"));
            AmountOnly($("#txtFilterScoreFrom"));
            AmountOnly($("#txtFilterScoreTo"));

            $("#txtFilterDateFromFrom, #txtFilterDateFromTo, \
            #txtFilterDateToFrom, #txtFilterDateToTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterRecordID").val(),
                    TransSummaryIDDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedID").value,
                    Description: $("#txtFilterDescription").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    ScoreFrom: $("#txtFilterScoreFrom").val(),
                    ScoreTo: $("#txtFilterScoreTo").val(),
                    DateFromFrom: $("#txtFilterDateFromFrom").val(),
                    DateFromTo: $("#txtFilterDateFromTo").val(),
                    DateToFrom: $("#txtFilterDateToFrom").val(),
                    DateToTo: $("#txtFilterDateToTo").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblIPMMyKPIScoreList");
                s.LoadJQGrid(param);

                $("#txtFilterID").removeClass("errMessage");
                $("#divMyKPIScoreListErrorMessage").html("");
                //if (objEMSCommonJS.GetMultiSelectList("multiSelectedID").value == "") {
                //    $("#divMyKPIScoreListErrorMessage").html("<label class=\"errMessage\"><li>Run ID is required.</li></label><br />");
                //    $("#txtFilterID").addClass("errMessage");
                //}
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedID").html("");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedPosition").html("");
                $("#btnSearch").click();
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterID"
                , GetRunIDAutoCompleteURL, 20, "multiSelectedID");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , GetOrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , GetPositionAutoCompleteURL, 20, "multiSelectedPosition");

            $("#btnExport").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objMyKPIScoreListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblIPMMyKPIScoreList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblIPMMyKPIScoreList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterRecordID").val()
                + "&TransSummaryIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedID").value
                + "&Description=" + $("#txtFilterDescription").val()
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                + "&ScoreFrom=" + $("#txtFilterScoreFrom").val()
                + "&ScoreTo=" + $("#txtFilterScoreTo").val()
                + "&DateFromFrom=" + $("#txtFilterDateFromFrom").val()
                + "&DateFromTo=" + $("#txtFilterDateFromTo").val()
                + "&DateToFrom=" + $("#txtFilterDateToFrom").val()
                + "&DateToTo=" + $("#txtFilterDateToTo").val()

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
            var tableInfo = localStorage.getItem("tblIPMMyKPIScoreList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMMyKPIScoreList"));
            var moveFilterFields = function () {
                var intialHeight = $("#divMyKPIScoreList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divMyKPIScoreList  .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divMyKPIScoreList #filterFieldsContainer");
                });
                $("#divMyKPIScoreList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divMyKPIScoreList div.filterFields").unbind("keyup");
                $("#divMyKPIScoreList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divMyKPIScoreList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMMyKPIScoreList").jqGrid("GridUnload");
            $("#tblIPMMyKPIScoreList").jqGrid("GridDestroy");
            $("#tblIPMMyKPIScoreList").jqGrid({
                url: MyKPIScoreListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Run ID", "Report Name", "Run Date From", "Run Date To", "Score", "Org. Group", "Position", "KPI Position Date From", "KPI Position Date To"],
                colModel: [
                    { hidden: true },
                    { name: "ID", index: "ID", align: "center", sortable: true, formatter: objMyKPIScoreListJS.ViewLink },
                    { hidden: true, width: 150, key: false, name: "TransSummaryID", index: "TransSummaryID", align: "center", sortable: true },
                    { width: 200, name: "Description", index: "Description", align: "left", sortable: true },
                    { width: 100, name: "TDateFrom", index: "TDateFrom", editable: true, align: "center" },
                    { width: 100, name: "TDateTo", index: "TDateTo", editable: true, align: "center" },
                    { width: 100, name: "Score", Score: "Score", editable: true, align: "right" },
                    { width: 150, name: "OrgGroup", index: "OrgGroup", editable: true, align: "left" },
                    { width: 150, name: "Position", index: "Position", editable: true, align: "left" },
                    { width: 150, name: "PDateFrom", index: "PDateFrom", editable: true, align: "center",sortable: false },
                    { width: 150, name: "PDateTo", index: "PDateTo", editable: true, align: "center", sortable: false  }

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
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblIPMMyKPIScoreList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        //AutoSizeColumnJQGrid("tblIPMMyKPIScoreList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divMyKPIScoreList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divMyKPIScoreList .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblIPMMyKPIScoreList .jqgrid-id-link").click(function () {
                            $('#divMyKPIScoreModal').modal('show');
                        });
                    }

                    if (localStorage["MyKPIScoreListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["MyKPIScoreListFilterOption"]));
                    }
                    objMyKPIScoreListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objMyKPIScoreListJS.ShowHideFilter();
                        localStorage["MyKPIScoreListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divMyKPIScoreList #gview_tblIPMMyKPIScoreList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
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
                    GetJQGridState("tblIPMMyKPIScoreList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage: function () {
            localStorage["IPMMyKPIScoreListID"] = $("#txtFilterRecordID").val();
            //localStorage["IPMMyKPIScoreListTransID"] = $("#txtFilterTransID").val();
            localStorage["IPMMyKPIScoreListIDDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedID").value;
            localStorage["IPMMyKPIScoreListIDDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedID").text;

            localStorage["IPMMyKPIScoreListDescription"] = $("#txtFilterDescription").val();
            localStorage["IPMMyKPIScoreListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["IPMMyKPIScoreListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;
            localStorage["IPMMyKPIScoreListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["IPMMyKPIScoreListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;
            localStorage["IPMMyKPIScoreListScoreFrom"] = $("#txtFilterScoreFrom").val();
            localStorage["IPMMyKPIScoreListScoreTo"] = $("#txtFilterScoreTo").val();
            localStorage["IPMMyKPIScoreListDateFromFrom"] = $("#txtFilterDateFromFrom").val();
            localStorage["IPMMyKPIScoreListDateFromTo"] = $("#txtFilterDateFromTo").val();
            localStorage["IPMMyKPIScoreListDateToFrom"] = $("#txtFilterDateToFrom").val();
            localStorage["IPMMyKPIScoreListDateToTo"] = $("#txtFilterDateToTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterRecordID").val(localStorage["IPMMyKPIScoreListID"]);
            //$("#txtFilterTransID").val(localStorage["IPMMyKPIScoreListTransID"]);
            objEMSCommonJS.SetMultiSelectList("multiSelectedID"
                , "IPMMyKPIScoreListIDDelimited"
                , "IPMMyKPIScoreListIDDelimitedText");

            $("#txtFilterDescription").val(localStorage["IPMMyKPIScoreListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "IPMMyKPIScoreListOrgGroupDelimited"
                , "IPMMyKPIScoreListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "IPMMyKPIScoreListPositionDelimited"
                , "IPMMyKPIScoreListPositionDelimitedText");

            $("#txtFilterScoreFrom").val(localStorage["IPMMyKPIScoreListScoreFrom"]);
            $("#txtFilterScoreTo").val(localStorage["IPMMyKPIScoreListScoreTo"]);

            $("#txtFilterDateFromFrom").val(localStorage["IPMMyKPIScoreListDateFromFrom"]);
            $("#txtFilterDateFromTo").val(localStorage["IPMMyKPIScoreListDateFromTo"]);
            $("#txtFilterDateToFrom").val(localStorage["IPMMyKPIScoreListDateToFrom"]);
            $("#txtFilterDateToTo").val(localStorage["IPMMyKPIScoreListDateToTo"]);
        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + MyKPIScoreViewURL +
                "?ID=" + rowObject.ID + "', 'divMyKPIScoreBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
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

    objMyKPIScoreListJS.Initialize();
});