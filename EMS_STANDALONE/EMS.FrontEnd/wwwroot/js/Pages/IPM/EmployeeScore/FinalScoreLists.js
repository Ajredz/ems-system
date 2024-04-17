var objEmployeeFinalScoreListJS;

const EmployeeFinalScoreListURL = "/IPM/EmployeeScore?handler=FinalScoreList";
const GetRunIDDropDownURL = "/IPM/EmployeeScore?handler=RunIDDropDown";
const EmployeeFinalScoreAddPostURL = "/IPM/EmployeeScore?handler=RunFinalScore";
const GetFinalScoreCheckExportListURL = "/IPM/EmployeeScore?handler=FinalScoreCheckExportList";
const DownloadFinalScoreExportListURL = "/IPM/EmployeeScore?handler=DownloadFinalScoreExportList";


$(document).ready(function () {
    objEmployeeFinalScoreListJS = {
        Initialize: function () {
            $("#divRunEmployeeFinalScoreModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();

        },
        ElementBinding: function () {
            var s = this;

            AmountOnly($("#txtFinalScoreID"));
            AmountOnly($("#txtFinalScoreIpmCount"));
            AmountOnly($("#txtFinalScoreFrom"));
            AmountOnly($("#txtFinalScoreTo"));

            $("#btnRunFinalScore").click(function () {
                GenerateDropdownValues(GetRunIDDropDownURL,
                    "ddlRunID", "Value", "Text", "", "", false);
                $("#divRunEmployeeFinalScoreModal").modal("show");
            });

            $("#btnRunFinalScoreAdd").on("click", function () {
                ModalConfirmation(MODAL_HEADER,
                    MSG_CONFIRM,
                    "objEMSCommonJS.PostAjaxWithBeforeSend(true \
                        , '"+ EmployeeFinalScoreAddPostURL + "&RunID=" + $("#ddlRunID").val() + "' \
                        , '' \
                        , '' \
                        , '#divRunEmployeeFinalScoreModal #btnRunFinalScoreAdd' \
                        , objEmployeeFinalScoreListJS.AddSuccessFunctionFinalScore \
                        , null \
                        , objEmployeeFinalScoreListJS.BeforeSendFunctionFinalScore \
                        , true \
                        , true);",
                    "function");
            });

            $("#btnResetFinalScore").on("click", function () {
                $("#divEmployeeFinalScoreList div.filterFields input[type='search']").val("");
                $("#multiSelectedFinalScoreRun").html("");
                $("#multiSelectedFinalScoreRunOption label, #multiSelectedFinalScoreRunOption input").prop("title", "add");
                $("#multiSelectedFinalScoreEmployee").html("");
                $("#multiSelectedFinalScoreEmployeeOption label, #multiSelectedFinalScoreEmployeeOption input").prop("title", "add");
                $("#multiSelectedFinalScoreCreatedBy").html("");
                $("#multiSelectedFinalScoreCreatedByOption label, #multiSelectedFinalScoreCreatedByOption input").prop("title", "add");

                $("#btnSearchFinalScore").click();
            });
            $("#btnSearchFinalScore").on("click", function () {
                objEmployeeFinalScoreListJS.LoadFinalScore({
                    ID: $("#txtFinalScoreID").val(),
                    RunIDDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedFinalScoreRun").value,
                    EmployeeIDDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedFinalScoreEmployee").value,
                    IPMCount: $("#txtFinalScoreIpmCount").val(),
                    IPMMonths: $("#txtFinalScoreIpmMonths").val(),
                    FinalScoreFrom: $("#txtFinalScoreFrom").val(),
                    FinalScoreTo: $("#txtFinalScoreTo").val(),
                    CreatedBy: objEMSCommonJS.GetMultiSelectList("multiSelectedFinalScoreCreatedBy").value,
                });
            });

            $("#btnExportFinalScore").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeFinalScoreListJS.ExportFunction()",
                    "function");
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFinalScoreRun"
                , GetRunIDAutoCompleteURL, 20, "multiSelectedFinalScoreRun");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFinalScoreEmployee"
                , GetEmployeeAutoCompleteURL, 20, "multiSelectedFinalScoreEmployee");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFinalScoreCreatedBy"
                , GetEmployeeSystemUserAutoCompleteURL, 20, "multiSelectedFinalScoreCreatedBy");
        },
        AddSuccessFunctionFinalScore: function (data) {
            ModalAlert(MODAL_HEADER, data.Result);
            $("#btnSearchFinalScore").click();
            $("#divRunEmployeeFinalScoreModal").modal("hide");
        },
        BeforeSendFunctionFinalScore: function () {
            $("#divModal").modal("hide");
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

            objEMSCommonJS.GetAjax(GetRaterByTransIDURL + "&ID=" + ID, "", "", GetSuccessFunction);
        },

        LoadFinalScore: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMEmployeeFinalScoreList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMEmployeeFinalScoreList"));
            var moveFilterFields = function () {
                var intialHeight = $("#divEmployeeFinalScoreList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divEmployeeFinalScoreList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divEmployeeFinalScoreList #filterFieldsContainer");
                });
                $("#divEmployeeFinalScoreList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divEmployeeFinalScoreList div.filterFields").unbind("keyup");
                $("#divEmployeeFinalScoreList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divEmployeeFinalScoreList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMEmployeeFinalScoreList").jqGrid("GridUnload");
            $("#tblIPMEmployeeFinalScoreList").jqGrid("GridDestroy");
            $("#tblIPMEmployeeFinalScoreList").jqGrid({
                url: EmployeeFinalScoreListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Run Title", "Employee Name", "IPM Count", "IPM Months", "Final Score", "Created By", "Created Date"],
                colModel: [
                    {
                        name: "ID", index: "ID", editable: true, align: "center", sortable: true, formatter: function (rowId, val, rowObject, cm, rdata) {
                            return objEMSCommonJS.JQGridIDFormat(rowObject.ID);
                        }
                    },
                    { name: "RunTitle", index: "RunTitle", editable: true, align: "left", sortable: true },
                    { name: "EmployeeName", index: "EmployeeName", editable: true, align: "left", sortable: true },
                    { name: "IPMCount", index: "IPMCount", editable: true, align: "right", sortable: true },
                    { name: "IPMMonths", index: "IPMMonths", editable: true, align: "right", sortable: true },
                    { name: "FinalScore", index: "FinalScore", editable: true, align: "right", sortable: true },
                    { name: "CreatedName", index: "CreatedName", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                ],
                toppager: $("#divFinalScorePager"),
                pager: $("#divFinalScorePager"),
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
                    $("#tblIPMEmployeeFinalScoreList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblIPMEmployeeFinalScoreList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divEmployeeFinalScoreList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divEmployeeFinalScoreList .jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                    }

                    if (localStorage["EmployeeFinalScoreListFilterOption"] != undefined) {
                        $("#chkFinalScoreFilter").prop('checked', JSON.parse(localStorage["EmployeeFinalScoreListFilterOption"]));
                    }
                    objEmployeeFinalScoreListJS.ShowHideFilterFinalScore();

                    $("#chkFinalScoreFilter").on('change', function () {
                        objEmployeeFinalScoreListJS.ShowHideFilterFinalScore();
                        localStorage["EmployeeFinalScoreListFilterOption"] = $("#chkFinalScoreFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divEmployeeFinalScoreList #gview_tblIPMEmployeeFinalScoreList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#divEmployeeFinalScoreList table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#divEmployeeFinalScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#divEmployeeFinalScoreList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeFinalScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#divEmployeeFinalScoreList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeFinalScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#divEmployeeFinalScoreList table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeFinalScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
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
                    GetJQGridState("tblIPMEmployeeFinalScoreList");
                    moveFilterFields();
                },
            }).navGrid("#divEmployeeFinalScoreList #divFinalScorePager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#divFinalScorePager").css("width", "100%");
            $("#divFinalScorePager").css("height", "100%");

            $("#tblIPMEmployeeFinalScoreList_toppager_center").hide();
            $("#tblIPMEmployeeFinalScoreList_toppager_right").hide();
            $("#tblIPMEmployeeFinalScoreList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblFilterFinalScore\">Show Filters</label>");
            jQuery("#lblFilterFinalScore").after("<input type=\"checkbox\" id=\"chkFinalScoreFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery("#divFinalScorePager_custom_block_right .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divFinalScorePager_custom_block_right").appendTo("#divFinalScorePager_left");
            $("#divFinalScorePager_center .ui-pg-table").appendTo("#divFinalScorePager_right");
        },
        ShowHideFilterFinalScore: function () {
            if ($("#chkFinalScoreFilter").is(":checked")) {
                $("#divEmployeeFinalScoreList .jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFinalScoreFilter").is(":not(:checked)")) {
                $("#divEmployeeFinalScoreList .jqgfirstrow .filterFields").hide();
            }
        },

        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblIPMEmployeeScoreList").jqGrid("getGridParam", "sortorder")

                + "&ID=" + $("#txtFinalScoreID").val()
                + "&RunIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedFinalScoreRun").value
                + "&EmployeeIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedFinalScoreEmployee").value
                + "&IPMCount=" + $("#txtFinalScoreIpmCount").val()
                + "&IPMMonths=" + $("#txtFinalScoreIpmMonths").val()
                + "&FinalScoreFrom=" + $("#txtFinalScoreFrom").val()
                + "&FinalScoreTo=" + $("#txtFinalScoreTo").val()
                + "&CreatedBy=" + objEMSCommonJS.GetMultiSelectList("multiSelectedFinalScoreCreatedBy").value
                + "&IsExport=true";

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadFinalScoreExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetFinalScoreCheckExportListURL + parameters, {}, "#btnExportFinalScore", GetSuccessFunction, null, true);
        },
    };

    objEmployeeFinalScoreListJS.Initialize();
});