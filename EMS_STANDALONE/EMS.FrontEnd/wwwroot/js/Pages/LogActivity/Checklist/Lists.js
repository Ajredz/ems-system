var objChecklistListJS;
const ChecklistListURL = "/LogActivity/Checklist?handler=List";
const GetApplicantLogActivitiesURL = "/LogActivity/Checklist?handler=ApplicantLogActivities";
const GetCurrentStatusURL = "/LogActivity/Checklist?handler=ReferenceValue&RefCode=ACTIVITY_STAT_FILTER";
const GetTypeURL = "/LogActivity/Checklist?handler=ReferenceValue&RefCode=ACTIVITY_TYPE";
const GetSubTypeURL = "/LogActivity/Checklist?handler=LogActivitySubType";
const UpdateLogActivityURL = "/LogActivity/Checklist/Edit";
const UpdateApplicantLogActivityPostURL = "/LogActivity/Checklist/Edit?handler=Applicant";
const UpdateEmployeeLogActivityPostURL = "/LogActivity/Checklist/Edit?handler=Employee";

const ReferredByAutoComplete = "/LogActivity/Checklist?handler=ReferredBy";
const GetApplicantLogActivityStatusHistoryURL = "/LogActivity/Checklist?handler=ApplicantLogActivityStatusHistory";
const GetEmployeeLogActivityStatusHistoryURL = "/LogActivity/Checklist?handler=EmployeeLogActivityStatusHistory";
const GetApplicantCommentsURL = "/LogActivity/Checklist/Edit?handler=ApplicantComments";
const SaveApplicantCommentsURL = "/LogActivity/Checklist/Edit?handler=SaveApplicantComments";
const GetEmployeeCommentsURL = "/LogActivity/Checklist/Edit?handler=EmployeeComments";
const SaveEmployeeCommentsURL = "/LogActivity/Checklist/Edit?handler=SaveEmployeeComments";
const GetApplicantAttachmentURL = "/LogActivity/Checklist/Edit?handler=ApplicantAttachment";
const SaveApplicantAttachmentURL = "/LogActivity/Checklist/Edit?handler=SaveApplicantAttachment";
const GetEmployeeAttachmentURL = "/LogActivity/Checklist/Edit?handler=EmployeeAttachment";
const SaveEmployeeAttachmentURL = "/LogActivity/Checklist/Edit?handler=SaveEmployeeAttachment";
const CheckFileIfExistsURL = "/LogActivity/Checklist/Edit?handler=CheckFileIfExists";
const DownloadFileURL = "/LogActivity/Checklist/Edit?handler=DownloadFile";
const AttachmentTypeDropDown = "/LogActivity/Checklist/Edit?handler=ReferenceValue&RefCode=ATTACHMENT_TYPE";
const GetCheckExportListURL = "/LogActivity/Checklist?handler=CheckExportList";
const DownloadExportListURL = "/LogActivity/Checklist?handler=DownloadExportList";


$(document).ready(function () {
    objChecklistListJS = {

        Initialize: function () {
            var s = this;
            
            s.ElementBinding();
            var param = {
                TypeDelimited: localStorage["ChecklistListType"],
                //SubTypeDelimited: localStorage["ChecklistListSubType"],
                Title: localStorage["ChecklistListTitle"],
                //Description: localStorage["ChecklistListDescription"],
                CurrentStatusDelimited: localStorage["ChecklistListCurrentStatus"],
                CurrentTimestampFrom: localStorage["ChecklistListCurrentTimestampFrom"],
                CurrentTimestampTo: localStorage["ChecklistListCurrentTimestampTo"],
                DueDateFrom: localStorage["ChecklistListDueDateFrom"],
                DueDateTo: localStorage["ChecklistListDueDateTo"],
                AssignedByDelimited: localStorage["ChecklistListAssignedBy"],
                AssignedToDelimited: localStorage["ChecklistListAssignedTo"],
                //Remarks: localStorage["ChecklistListRemarks"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabHistory', '#divLogActivityModal');
        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterCurrentTimestampFrom, #txtFilterCurrentTimestampTo").datetimepicker({
                    useCurrent: false,
                    format: 'MM/DD/YYYY'
            });

            $("#txtFilterDueDateFrom, #txtFilterDueDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterAssignedBy"
                , ReferredByAutoComplete, 20, "multiSelectedAssignedBy");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterAssignedTo"
                , ReferredByAutoComplete, 20, "multiSelectedAssignedTo");

            $("#btnSearch").click(function () {
                var param = {
                    TypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedType").value,
                    //SubTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").value,
                    Title: $("#txtFilterTitle").val(),
                    //Description: $("#txtFilterDescription").val(),
                    CurrentStatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").value,
                    CurrentTimestampFrom: $("#txtFilterCurrentTimestampFrom").val(),
                    CurrentTimestampTo: $("#txtFilterCurrentTimestampTo").val(),
                    DueDateFrom: $("#txtFilterDueDateFrom").val(),
                    DueDateTo: $("#txtFilterDueDateTo").val(),
                    AssignedByDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedBy").value,
                    AssignedToDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedTo").value,
                    //Remarks: $("#txtFilterRemarks").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblChecklistList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedType").html("");
                $("#multiSelectedTypeOption label, #multiSelectedTypeOption input").prop("title", "add");
                //$("#multiSelectedSubType").html("");
                //$("#multiSelectedSubTypeOption label, #multiSelectedSubTypeOption input").prop("title", "add");
                $("#multiSelectedCurrentStatus").html("");
                $("#multiSelectedCurrentStatusOption label, #multiSelectedCurrentStatusOption input").prop("title", "add");
                $("#multiSelectedAssignedBy").html("");
                $("#multiSelectedAssignedByOption label, #multiSelectedAssignedByOption input").prop("title", "add");
                $("#multiSelectedAssignedTo").html("");
                $("#multiSelectedAssignedToOption label, #multiSelectedAssignedToOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnExport").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objChecklistListJS.ExportFunction()",
                    "function");
            });

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedType", GetTypeURL, "Value", "Description");
            //objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedSubType", GetSubTypeURL, "Value", "Description");
            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedCurrentStatus", GetCurrentStatusURL, "Value", "Description");

        },

        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblChecklistList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblChecklistList").jqGrid("getGridParam", "sortorder")
                + "&TypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedType").value
                //+ "&SubTypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").value
                + "&Title=" + $("#txtFilterTitle").val()
                //+ "&Description=" + $("#txtFilterDescription").val()
                + "&CurrentStatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").value
                + "&CurrentTimestampFrom=" + $("#txtFilterCurrentTimestampFrom").val()
                + "&CurrentTimestampTo=" + $("#txtFilterCurrentTimestampTo").val()
                + "&DueDateFrom=" + $("#txtFilterDueDateFrom").val()
                + "&DueDateTo=" + $("#txtFilterDueDateTo").val()
                + "&AssignedByDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedBy").value
                + "&AssignedToDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedTo").value
            //+ "&Remarks=" + $("#txtFilterRemarks").val()

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
            var tableInfo = localStorage.getItem("tblChecklistList") == "" ? "" : $.parseJSON(localStorage.getItem("tblChecklistList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divCheckList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divCheckList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divCheckList #filterFieldsContainer");
                });

                $("#divCheckList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divCheckList div.filterFields").unbind("keyup");
                $("#divCheckList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#tblChecklistList").jqGrid("GridUnload");
            $("#tblChecklistList").jqGrid("GridDestroy");
            $("#tblChecklistList").jqGrid({
                url: ChecklistListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Type", "Sub-Type", "Title", "Description", "Assigned By", "Assigned To",
                          "Due Date", "Current Status", "Timestamp", "Remarks", "", ""
                ],
                colModel: [
                    { key: true, name: "ID", index: "ID", align: "center", sortable: false, formatter: objChecklistListJS.UpdateLink },
                    { name: "Type", index: "Type", align: "left", sortable: false },
                    { hidden: true, name: "SubType", index: "SubType", align: "left", sortable: false },
                    { name: "Title", index: "Title", align: "left", sortable: false },
                    { hidden: true, name: "Description", index: "Description", align: "left", sortable: false },
                    { name: "AssignedBy", index: "AssignedBy", align: "left", sortable: false },
                    { name: "AssignedTo", index: "AssignedTo", align: "left", sortable: false },
                    { name: "DueDate", index: "DueDate", align: "left", sortable: false },
                    { name: "CurrentStatus", index: "CurrentStatus", align: "left", sortable: false },
                    { name: "CurrentTimestamp", index: "CurrentTimestamp", align: "left", sortable: false },
                    { hidden: true, name: "Remarks", index: "Remarks", align: "left", sortable: false },
                    { hidden: true, name: "ApplicantID", index: "ApplicantID", align: "left", sortable: false },
                    { hidden: true, name: "EmployeeID", index: "EmployeeID", align: "left", sortable: false },
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
                    $("#tblChecklistList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblChecklistList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divCheckList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#divCheckList #tblChecklistList .jqgrid-id-link").click(function () {
                            $('#divLogActivityModal').modal('show');
                        });

                    }

                    if (localStorage["ChecklistListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["ChecklistListFilterOption"]));
                    }
                    objChecklistListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objChecklistListJS.ShowHideFilter();
                        localStorage["ChecklistListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
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
                    GetJQGridState("tblChecklistList");
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
            localStorage["ChecklistListType"] = objEMSCommonJS.GetMultiSelectList("multiSelectedType").value;
            localStorage["ChecklistListTypeText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedType").text;
            //localStorage["ChecklistListSubType"] = objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").value;
            //localStorage["ChecklistListSubTypeText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").text;

            localStorage["ChecklistListTitle"] = $("#txtFilterTitle").val();
            //localStorage["ChecklistListDescription"] = $("#txtFilterDescription").val();

            localStorage["ChecklistListCurrentStatus"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").value;
            localStorage["ChecklistListCurrentStatusText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").text;

            localStorage["ChecklistListCurrentTimestampFrom"] = $("#txtFilterCurrentTimestampFrom").val();
            localStorage["ChecklistListCurrentTimestampTo"] = $("#txtFilterCurrentTimestampTo").val();

            localStorage["ChecklistListDueDateFrom"] = $("#txtFilterDueDateFrom").val();
            localStorage["ChecklistListDueDateTo"] = $("#txtFilterDueDateTo").val();

            localStorage["ChecklistListAssignedBy"] = objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedBy").value;
            localStorage["ChecklistListAssignedByText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedBy").text;

            localStorage["ChecklistListAssignedTo"] = objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedTo").value;
            localStorage["ChecklistListAssignedToText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedTo").text;

            //localStorage["ChecklistListRemarks"] = $("#txtFilterRemarks").val();
        },

        GetLocalStorage: function () {

            objEMSCommonJS.SetMultiSelectList("multiSelectedType"
                , "ChecklistListType"
                , "ChecklistListTypeText");
            //objEMSCommonJS.SetMultiSelectList("multiSelectedSubType"
            //    , "ChecklistListSubType"
            //    , "ChecklistListSubTypeText");

            $("#txtFilterTitle").val(localStorage["ChecklistListTitle"]);
            //$("#txtFilterDescription").val(localStorage["ChecklistListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStatus"
                , "ChecklistListCurrentStatus"
                , "ChecklistListCurrentStatusText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedAssignedBy"
                , "ChecklistListAssignedBy"
                , "ChecklistListAssignedByText");

            $("#txtFilterCurrentTimestampFrom").val(localStorage["ChecklistListCurrentTimestampFrom"]);
            $("#txtFilterCurrentTimestampTo").val(localStorage["ChecklistListCurrentTimestampTo"]);

            $("#txtFilterDueDateFrom").val(localStorage["ChecklistListDueDateFrom"]);
            $("#txtFilterDueDateTo").val(localStorage["ChecklistListDueDateTo"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedAssignedTo"
                , "ChecklistListAssignedTo"
                , "ChecklistListAssignedToText");

            //$("#txtFilterRemarks").val(localStorage["ChecklistListRemarks"]);
        },

        UpdateLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return objChecklistListJS.ViewUpdateStatusModal('" + UpdateLogActivityURL
                + "?ID=" + rowObject.ID + "&ApplicantID=" + rowObject.ApplicantID + "&EmployeeID=" + rowObject.EmployeeID + "');\">" +objEMSCommonJS.JQGridIDFormat(rowObject.ID)+ "</a>";
        },

        ViewUpdateStatusModal: function (url) {
            $('#divLogActivityModal').modal('show');
            LoadPartial(url, 'divLogActivityBodyModal');
            return false;
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

    objChecklistListJS.Initialize();
});