var objEmployeeTrainingListJS;

var TrainingTemplate = [];

const TrainingListURL = "/Training/AllTraining?handler=List";
const TrainingViewURL = "/Training/AllTraining/View";
const TrainingAddURL = "/Training/AllTraining/Add";
const TrainingEditURL = "/Training/AllTraining/Edit";
const GetStatusURL = "/Training/AllTraining?handler=StatusFilter";
const GetTrainingTypeURL = "/Training/AllTraining?handler=ReferenceValue&RefCode=TRAINING_TYPE";
const GetTrainingTemplateDropdownURL = "/Training/AllTraining?handler=TrainingTemplateDropdown";
const TrainingTemplateByIDURL = "/Training/AllTraining?handler=TrainingTemplateByID";
const AddEmployeeTrainingTemplateURL = "/Training/AllTraining?handler=AddEmployeeTrainingTemplate";
const UploadInsertURL = "/Training/AllTraining?handler=UploadInsert";
const DownloadFormURL = "/Training/AllTraining?handler=DownloadTrainingTemplate";
const TrainingChangeStatus = "/Training/AllTraining?handler=ChangeStatus";
const GetEmployeeAutoCompleteURL = "/Training/AllTraining?handler=EmployeeAutoComplete";
const GetSystemUserAutoCompleteURL = "/Training/AllTraining?handler=SystemUserAutoComplete";
const GetTrainingStatusHistoryURL = "/Training/AllTraining?handler=TrainingStatusHistory";
const GetTrainingScoreURL = "/Training/AllTraining?handler=TrainingScore";
const GetClassroomAutoCompleteURL = "/Training/AllTraining?handler=ClassroomNameAutoComplete";
const ExportListURL = "/Training/AllTraining?handler=ExportList";
const ExportDownloadListURL = "/Training/AllTraining?handler=ExportDownloadList";

$(document).ready(function () {
    objEmployeeTrainingListJS = {

        HasView: $("#HasView").val(),

        Initialize: function () {
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            s.ElementBinding();

            var param = {
            };
            s.LoadJQGrid(param);
        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterCreatedDateFrom,#txtFilterCreatedDateTo \
                ,#txtFilterModifiedDateFrom,#txtFilterModifiedDateTo \
                ,#txtFilterDateScheduleFrom,#txtFilterDateScheduleTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("#multiSelectedCreatedBy").html("");
                $("#multiSelectedEmployeeName").html("")
                $("#multiSelectedType").html("");
                $("#multiSelectedTypeOption label, #multiSelectedTypeOption input").prop("title", "add");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#multiSelectedModifiedBy").html("");
                $("#btnSearch").click();
            });

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterID").val(),
                    EmployeeID: objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeName").value,
                    TypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedType").value,
                    Title: $("#txtFilterTitle").val(),
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value,
                    CreatedBy: objEMSCommonJS.GetMultiSelectList("multiSelectedCreatedBy").value,
                    CreatedDateFrom: $("#txtFilterCreatedDateFrom").val(),
                    CreatedDateTo: $("#txtFilterCreatedDateTo").val(),
                    DateScheduleFrom: $("#txtFilterDateScheduleFrom").val(),
                    DateScheduleTo: $("#txtFilterDateScheduleTo").val(),
                    ModifiedBy: objEMSCommonJS.GetMultiSelectList("multiSelectedModifiedBy").value,
                    ModifiedDateFrom: $("#txtFilterModifiedDateFrom").val(),
                    ModifiedDateTo: $("#txtFilterModifiedDateTo").val(),
                };
                objEmployeeTrainingListJS.LoadJQGrid(param);
            });

            $("#btnUpload").click(function () {
                objEMSCommonJS.UploadModal(UploadInsertURL, "Upload", DownloadFormURL);
                $('#divModalErrorMessage').html('');
            });


            $("#btnCancelDynamicChangeStatus").on("click", function () {
                $("#ChangeStatusModal").hide();
            });

            $("#btnTrainingChangeStatus").click(function () {
                $('#divEmployeeTrainingErrorMessage').html('');
                var selRow = $("#tblEmployeeTrainingList").jqGrid("getGridParam", "selarrrow");
                var firstValue = "";
                var isValid = true;
                if (selRow.length > 0) {
                    $(selRow).each(function (index, item) {
                        if (firstValue == "")
                            firstValue = $("#tblEmployeeTrainingList").getRowData(item).Status;
                        else if (firstValue != $("#tblEmployeeTrainingList").getRowData(item).Status)
                            isValid = false;
                    });
                    if (isValid) {
                        if ($("#ChangeStatusModal").is(":visible")) {
                            $("#ChangeStatusModal").hide();
                            return;
                        }
                        else
                            $("#ChangeStatusModal").show();

                        $(".editRequired").addClass("reqField");
                        $('#divEmployeeMovementErrorMessage').html('');
                        $("#txtChangeStatusRemarks").val("");
                        $("#hdnDynamicChangeStatusID").val(selRow);
                        GenerateDropdownValues(TrainingChangeStatus + "&CurrentStatus=" + firstValue, "ddlDynamicChangeStatus", "Value", "Text", "", "", false);
                    }
                    else
                        $("#divEmployeeTrainingErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_DIFF_STATUS + " </li></label><br />");
                }
                else {
                    $("#divEmployeeTrainingErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Item</li></label><br />");
                }
            });

            $("#btnSaveDynamicChangeStatus").on("click", function () {
                $("#divDynamicChangeStatusErrorMessage").html("");

                if ($("#ddlDynamicChangeStatus :selected").val() == "") {
                    $("#divDynamicChangeStatusErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + " Training</li></label><br />");
                    return;
                }

                var isNoBlankFunction = function () {
                    return true;
                };
                if (objEMSCommonJS.ValidateBlankFields("#DynamicChangeStatusForm", "#divDynamicChangeStatusErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , TrainingChangeStatus + "+ ("'&ID=" + $("#hdnDynamicChangeStatusID").val() + "'") + " \
                        , objEmployeeTrainingListJS.ChangeStatusFormData() \
                        , '#divDynamicChangeStatusErrorMessage' \
                        , '#btnSaveDynamicChangeStatus' \
                        , objEmployeeTrainingListJS.ChangeStatusSuccessFunction);",
                        "function");
                }
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeTrainingListJS.ExportFunction()",
                    "function");
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedType", GetTrainingTypeURL);
            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedStatus", GetStatusURL);
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterEmployeeName"
                , GetEmployeeAutoCompleteURL, 20, "multiSelectedEmployeeName");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterCreatedBy"
                , GetSystemUserAutoCompleteURL, 20, "multiSelectedCreatedBy");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterModifiedBy"
                , GetSystemUserAutoCompleteURL, 20, "multiSelectedModifiedBy");
        },
        ChangeStatusSuccessFunction: function () {
            $("#ChangeStatusModal").hide();
            $("#btnSearch").click();
        },
        ChangeStatusFormData: function () {
            var formData = new FormData($('#DynamicChangeStatusForm').get(0));

            formData.append("ChangeStatus.Status", $("#ddlDynamicChangeStatus :selected").val());
            formData.append("ChangeStatus.Remarks", $("#txtDynamicChangeStatusRemarks").val());
            return formData;
        },
        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);

            var tableInfo = localStorage.getItem("tblEmployeeTrainingList") == "" ? "" : $.parseJSON(localStorage.getItem("tblEmployeeTrainingList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });

                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            };
            moveFilterFields();

            $("#tblEmployeeTrainingList").jqGrid("GridUnload");
            $("#tblEmployeeTrainingList").jqGrid("GridDestroy");
            $("#tblEmployeeTrainingList").jqGrid({
                url: TrainingListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Employee", "Type", "Title", "Status","Date Schedule", "Created By", "Created Date", "Modified By", "Modified Date", "", ""],
                colModel: [
                    {
                        width: 30, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objEmployeeTrainingListJS.ViewID, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="border-left:10px solid ' + rowObject.StatusColor + ' !important;"';
                        } },
                    { name: "EmployeeName", index: "EmployeeName", editable: true, align: "left", sortable: true },
                    { name: "Type", index: "Type", editable: true, align: "left", sortable: true },
                    { name: "Title", index: "Title", editable: true, align: "left", sortable: true },
                    {
                        name: "StatusDescription", index: "StatusDescription", editable: true, align: "left", sortable: true, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="background-color:' + rowObject.StatusColor + '"';
                        }
                    },
                    { name: "DateSchedule", index: "DateSchedule", editable: true, align: "left", sortable: true },
                    { name: "CreatedByName", index: "CreatedByName", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                    { name: "ModifiedByName", index: "ModifiedByName", editable: true, align: "left", sortable: true },
                    { name: "ModifiedDate", index: "ModifiedDate", editable: true, align: "left", sortable: true },
                    { hidden: true, name: "EmployeeID", index: "EmployeeID", editable: true, align: "left" },
                    { hidden: true, name: "Status", index: "Status", editable: true, align: "left", sortable: true }
                ],
                toppager: $("#divEmployeeTrainingPager"),
                pager: $("#divEmployeeTrainingPager"),
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
                    $("#tblEmployeeTrainingList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);

                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divEmployeeTrainingErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {

                            }
                        }
                        AutoSizeColumnJQGrid("tblEmployeeTrainingList", data);

                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                        $(".jqgrid-id-link").click(function () {
                            $('#divTrainingModal').modal('show');
                        });
                    }

                    if (localStorage["EmployeeTrainingListFilterOption"] != undefined) {
                        $("#chkTrainingFilter").prop('checked', JSON.parse(localStorage["EmployeeTrainingListFilterOption"]));
                    }
                    objEmployeeTrainingListJS.ShowHideFilter();

                    $("#chkTrainingFilter").on('change', function () {
                        objEmployeeTrainingListJS.ShowHideFilter();
                        localStorage["EmployeeTrainingListFilterOption"] = $("#chkTrainingFilter").is(":checked");
                    });

                    $(".ui-jqgrid-bdiv").css({ "min-height": "400px" });

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
                    GetJQGridState("tblEmployeeTrainingList");
                    moveFilterFields();
                },
            }).navGrid("#divEmployeeTrainingPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#divEmployeeTrainingPager").css("width", "100%");
            $("#divEmployeeTrainingPager").css("height", "100%");

            $("#tblEmployeeTrainingList_toppager_center").hide();
            $("#tblEmployeeTrainingList_toppager_right").hide();
            $("#tblEmployeeTrainingList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filters</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkTrainingFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divEmployeeTrainingPager_custom_block_right").appendTo("#divEmployeeTrainingPager_left");
            $("#divEmployeeTrainingPager_center .ui-pg-table").appendTo("#divEmployeeTrainingPager_right");
        },
        ViewID: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + TrainingViewURL + "?ID=" + rowObject.ID + "', 'divTrainingBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },
        ShowHideFilter: function () {
            if ($("#chkTrainingFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkTrainingFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },
        LoadTrainingTemplateJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblTrainingTemplateList").jqGrid("GridUnload");
            $("#tblTrainingTemplateList").jqGrid("GridDestroy");
            $("#tblTrainingTemplateList").jqGrid({
                url: TrainingTemplateByIDURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Type", "Title", "Description"],
                colModel: [
                    { name: "", hidden: true },
                    { name: "Type", index: "Type", align: "center", sortable: false },
                    { name: "Title", index: "Title", align: "left", sortable: false },
                    { name: "Description", index: "Description", align: "center", sortable: false },
                ],
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divTrainingTemplateErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {

                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblTrainingTemplateList", data);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },
        LoadStatusHistory: function (param) {
            var s = this;
            Loading(true);
            $("#tblStatusHistory").jqGrid("GridUnload");
            $("#tblStatusHistory").jqGrid("GridDestroy");
            $("#tblStatusHistory").jqGrid({
                url: GetTrainingStatusHistoryURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["Status", "Remarks", "Updated By", "Updated Date"],
                colModel: [
                    { name: "Status", index: "Status", align: "left", sortable: false },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false },
                    { name: "CreatedName", index: "CreatedName", align: "left", sortable: false },
                    { name: "CreatedDate", index: "CreatedDate", align: "left", sortable: false },
                ],  
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
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
                        }

                        AutoSizeColumnJQGrid("tblStatusHistory", data);
                        $("#tblStatusHistory_subgrid").width(20);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });
        },
        LoadScore: function (param) {
            var s = this;
            Loading(true);
            $("#tblScore").jqGrid("GridUnload");
            $("#tblScore").jqGrid("GridDestroy");
            $("#tblScore").jqGrid({
                url: GetTrainingScoreURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["Exam ID", "Exam Score", "Total Exam Score", "Average Score", "Question", "Completed Date"],
                colModel: [
                    {
                        name: "TakeExamID", index: "TakeExamID", align: "left", sortable: false, formatter: function(cellvalue, options, rowObject) {
                                return objEMSCommonJS.JQGridIDFormat(rowObject.TakeExamID);
                        },
                    },
                    { name: "ExamScore", index: "ExamScore", align: "center", sortable: false },
                    { name: "TotalExamScore", index: "TotalExamScore", align: "center", sortable: false },
                    {
                        name: "AverageScore", index: "AverageScore", align: "center", sortable: false, formatter: function (cellvalue, options, rowObject) {
                            return rowObject.AverageScore + "%";
                        },
                    },
                    { name: "TotalExamQuestion", index: "TotalExamQuestion", align: "center", sortable: false },
                    { name: "CompletedDate", index: "CompletedDate", align: "left", sortable: false },
                ],
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
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
                        }

                        AutoSizeColumnJQGrid("tblScore", data);
                        $("#tblScore_subgrid").width(20);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });
        },
        ExportFunction: function () {
            Loading(true);
            var parameters = "&sidx=" + $("#tblEmployeeTrainingList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblEmployeeTrainingList").jqGrid("getGridParam", "sortorder")
                + "&IsExport=" + "TRUE"
                + "&ID=" + $("#txtFilterID").val()
                + "&EmployeeID=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeName").value
                + "&TypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedType").value
                + "&Title=" + $("#txtFilterTitle").val()
                + "&StatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value
                + "&CreatedBy=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCreatedBy").value
                + "&CreatedDateFrom=" + $("#txtFilterCreatedDateFrom").val()
                + "&CreatedDateTo=" + $("#txtFilterCreatedDateTo").val()
                + "&DateScheduleFrom=" + $("#txtFilterDateScheduleFrom").val()
                + "&DateScheduleTo=" + $("#txtFilterDateScheduleTo").val()
                + "&ModifiedBy=" + objEMSCommonJS.GetMultiSelectList("multiSelectedModifiedBy").value
                + "&ModifiedDateFrom=" + $("#txtFilterModifiedDateFrom").val()
                + "&ModifiedDateTo=" + $("#txtFilterModifiedDateTo").val();

            var GetSuccessFunction = function (data) {
                Loading(false);
                if (data.IsSuccess == true) {
                    window.location = ExportDownloadListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };
            objEMSCommonJS.GetAjax(ExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },
    }

    objEmployeeTrainingListJS.Initialize();
});