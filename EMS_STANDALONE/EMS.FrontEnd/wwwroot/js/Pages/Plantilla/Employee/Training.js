var objEmployeeTrainingListJS;

var TrainingTemplate = [];

$(document).ready(function () {
    objEmployeeTrainingListJS = {

        EmployeeID: $("#hdnID").val(),

        Initialize: function () {
            $("#divTrainingTemplateBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            s.ElementBinding();

            var param = {
                UnderAccess: objEmployeeTrainingListJS.EmployeeID,
            };
            s.LoadJQGrid(param);
        },

        ElementBinding: function () {
            var s = this;

            $("#tabTraining #txtFilterCreatedDateFrom,#tabTraining #txtFilterCreatedDateTo \
                ,#tabTraining #txtFilterModifiedDateFrom,#tabTraining #txtFilterModifiedDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
                });

            $("#tabTraining #btnReset").click(function () {
                $("#tabTraining div.filterFields input[type='search']").val("");
                $("#tabTraining #multiSelectedCreatedBy").html("");
                $("#tabTraining #multiSelectedTypeOption label, #multiSelectedTypeOption input").prop("title", "add");
                $("#tabTraining #multiSelectedStatus").html("");
                $("#tabTraining #multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#tabTraining #multiSelectedType").html("");
                $("#tabTraining #multiSelectedModifiedBy").html("");
                $("#tabTraining #btnSearch").click();
            });

            $("#tabTraining #btnSearch").click(function () {
                var param = {
                    ID: $("#tabTraining #txtFilterID").val(),
                    UnderAccess: objEmployeeTrainingListJS.EmployeeID,
                    TypeDelimited: objEMSCommonJS.GetMultiSelectList("tabTraining #multiSelectedType").value,
                    Title: $("#tabTraining #txtFilterTitle").val(),
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("tabTraining #multiSelectedStatus").value,
                    CreatedBy: objEMSCommonJS.GetMultiSelectList("tabTraining #multiSelectedCreatedBy").value,
                    CreatedDateFrom: $("#tabTraining #txtFilterCreatedDateFrom").val(),
                    CreatedDateTo: $("#tabTraining #txtFilterCreatedDateTo").val(),
                    ModifiedBy: objEMSCommonJS.GetMultiSelectList("tabTraining #multiSelectedModifiedBy").value,
                    ModifiedDateFrom: $("#tabTraining #txtFilterModifiedDateFrom").val(),
                    ModifiedDateTo: $("#tabTraining #txtFilterModifiedDateTo").val(),
                };
                objEmployeeTrainingListJS.LoadJQGrid(param);
            });

            $("#tabTraining #btnAdd").click(function () {
                var SuccessFunction = function () {
                    $("#divTrainingModal").modal("show");
                };
                LoadPartialSuccessFunction(TrainingAddURL + "?EmployeeID=" + objEmployeeTrainingListJS.EmployeeID, "divTrainingBodyModal", SuccessFunction);
            });

            $("#tabTraining #btnAddTemplate").click(function () {
                $("#divTrainingTemplateModal").modal("show");

                var GetSuccessFunction = function (data) {
                    $("#divTrainingTemplateModal #ddlTrainingTemplate").html("");
                    TrainingTemplate = [];
                    $("#tblTrainingTemplateList").jqGrid("GridUnload");
                    $("#tblTrainingTemplateList").jqGrid("GridDestroy");
                    $(data.Result).each(function (index, item) {
                        TrainingTemplate.push(
                            {
                                Value: item.Value,
                                Text: item.Text
                            });
                    });
                    objEMSCommonJS.PopulateDropDown("#divTrainingTemplateModal #ddlTrainingTemplate", TrainingTemplate);
                };

                objEMSCommonJS.GetAjax(GetTrainingTemplateDropdownURL, {}, "", GetSuccessFunction);
            });
            $("#divTrainingTemplateModal #ddlTrainingTemplate").change(function () {
                objEmployeeTrainingListJS.LoadTrainingTemplateJQGrid({
                    TrainingTemplateID: $(this).val()
                });
            });
            $("#divTrainingTemplateModal #btnSaveTemplate").click(function () {
                $("#divTrainingTemplateModal .errMessage").removeClass("errMessage");
                $("#divTrainingTemplateErrorMessage").html("");
                if ($("#divTrainingTemplateModal #ddlTrainingTemplate").val() != "") {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                        "objEMSCommonJS.PostAjax(true \
                        , '" + AddEmployeeTrainingTemplateURL
                        + ("&EmployeeID=" + objEmployeeTrainingListJS.EmployeeID
                        + "&TrainingTemplateID=" + $("#divTrainingTemplateModal #ddlTrainingTemplate :selected").val() + "'") + " \
                        ,  { } \
                        , '#divTrainingTemplateModal #divTrainingTemplateErrorMessage' \
                        , '#divTrainingTemplateModal #btnSaveTemplate' \
                        , objEmployeeTrainingListJS.SuccessTrainingTemplateFunction); ",
                        "function");
                }
                else {
                    $("#divTrainingTemplateModal #ddlTrainingTemplate").addClass("errMessage");
                    $("#divTrainingTemplateModal #ddlTrainingTemplate").focus();
                    $("#divTrainingTemplateModal #divTrainingTemplateErrorMessage").append("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                }
            });

            $("#tabTraining #btnCancelDynamicChangeStatus").on("click", function () {
                $("#tabTraining #ChangeStatusModal").hide();
            });

            $("#tabTraining #btnTrainingChangeStatus").click(function () {
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
                        if ($("#tabTraining #ChangeStatusModal").is(":visible")) {
                            $("#tabTraining #ChangeStatusModal").hide();
                            return;
                        }
                        else
                            $("#tabTraining #ChangeStatusModal").show();

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

            $("#tabTraining #btnSaveDynamicChangeStatus").on("click", function () {
                $("#tabTraining #divDynamicChangeStatusErrorMessage").html("");

                if ($("#tabTraining #ddlDynamicChangeStatus :selected").val() == "") {
                    $("#tabTraining #divDynamicChangeStatusErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + " Training</li></label><br />");
                    return;
                }

                var isNoBlankFunction = function () {
                    return true;
                };
                if (objEMSCommonJS.ValidateBlankFields("#tabTraining #DynamicChangeStatusForm", "#tabTraining #divDynamicChangeStatusErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , TrainingChangeStatus + "+ ("'&ID=" + $("#tabTraining #hdnDynamicChangeStatusID").val() + "'") + " \
                        , objEmployeeTrainingListJS.ChangeStatusFormData() \
                        , '#tabTraining #divDynamicChangeStatusErrorMessage' \
                        , '#tabTraining #btnSaveDynamicChangeStatus' \
                        , objEmployeeTrainingListJS.ChangeStatusSuccessFunction);",
                        "function");
                }
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("tabTraining #multiSelectedType", GetTrainingTypeURL);
            objEMSCommonJS.BindFilterMultiSelectEnum("tabTraining #multiSelectedStatus", GetStatusURL);
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterCreatedBy"
                , ReferredByAutoComplete, 20, "multiSelectedCreatedBy");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterModifiedBy"
                , ReferredByAutoComplete, 20, "multiSelectedModifiedBy");
        },
        ChangeStatusSuccessFunction: function () {
            $("#tabTraining #ChangeStatusModal").hide();
            $("#tabTraining #btnSearch").click();
        },
        ChangeStatusFormData: function () {
            var formData = new FormData($('#tabTraining #DynamicChangeStatusForm').get(0));

            formData.append("ChangeStatus.Status", $("#tabTraining #ddlDynamicChangeStatus :selected").val());
            formData.append("ChangeStatus.Remarks", $("#tabTraining #txtDynamicChangeStatusRemarks").val());
            return formData;
        },
        SuccessTrainingTemplateFunction: function () {
            $("#divTrainingTemplateModal").modal("hide");
            var param = {
                ID: localStorage["TrainingListID"],
                UnderAccess: objEmployeeTrainingListJS.EmployeeID,
                PreloadName: localStorage["TrainingListTemplateName"],
                CreatedDateFrom: localStorage["TrainingListCreatedDateFrom"],
                CreatedDateTo: localStorage["TrainingListCreatedDateTo"]
            };
            objEmployeeTrainingListJS.LoadJQGrid(param);
        },
        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);

            var tableInfo = localStorage.getItem("tblEmployeeTrainingList") == "" ? "" : $.parseJSON(localStorage.getItem("tblEmployeeTrainingList"));
            var moveFilterFields = function () {
                var intialHeight = $("#tabTraining .jqgfirstrow").height();
                $("#tabTraining .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#tabTraining #filterFieldsContainer");
                });

                $("#tabTraining .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#tabTraining div.filterFields").unbind("keyup");
                $("#tabTraining div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#tabTraining #btnSearch").click();
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
                colNames: ["ID", "Type", "Title", "Status", "Created By", "Created Date", "Modified By", "Modified Date", ""],
                colModel: [
                    { width: 30, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objEmployeeTrainingListJS.ViewID },
                    { name: "Type", index: "Type", editable: true, align: "left", sortable: true },
                    { name: "Title", index: "Title", editable: true, align: "left", sortable: true },
                    {
                        name: "StatusDescription", index: "StatusDescription", editable: true, align: "left", sortable: true, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="background-color:' + rowObject.StatusColor + '"';
                        }
                    },
                    { name: "CreatedByName", index: "CreatedByName", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                    { name: "ModifiedByName", index: "ModifiedByName", editable: true, align: "left", sortable: true },
                    { name: "ModifiedDate", index: "ModifiedDate", editable: true, align: "left", sortable: true },
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

                        $("#tabTraining #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#tabTraining .jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                        $("#tabTraining .jqgrid-id-link").click(function () {
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

                    $("#tabTraining .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#tabTraining table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#tabTraining table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#tabTraining table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#tabTraining table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#tabTraining table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#tabTraining table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#tabTraining table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabTraining .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
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
            jQuery("#divEmployeeTrainingPager_custom_block_right .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divEmployeeTrainingPager_custom_block_right").appendTo("#divEmployeeTrainingPager_left");
            $("#divEmployeeTrainingPager_center .ui-pg-table").appendTo("#divEmployeeTrainingPager_right");
        },
        ViewID: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + TrainingViewURL + "?ID=" + rowObject.ID + "', 'divTrainingBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },
        ShowHideFilter: function () {
            if ($("#tabTraining #chkTrainingFilter").is(":checked")) {
                $("#tabTraining .jqgfirstrow .filterFields").show();
            }
            else if ($("#tabTraining #chkTrainingFilter").is(":not(:checked)")) {
                $("#tabTraining .jqgfirstrow .filterFields").hide();
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
                        name: "TakeExamID", index: "TakeExamID", align: "left", sortable: false, formatter: function (cellvalue, options, rowObject) {
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
    }

    objEmployeeTrainingListJS.Initialize();
});