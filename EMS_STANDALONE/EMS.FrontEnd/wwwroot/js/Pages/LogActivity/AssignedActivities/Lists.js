var objAssignedActivitiesListJS;
const AssignedActivitiesListURL = window.location.pathname + "?handler=List";
const GetApplicantLogActivitiesURL = window.location.pathname + "?handler=ApplicantLogActivities";
const GetCurrentStatusURL = window.location.pathname + "?handler=ReferenceValue&RefCode=ACTIVITY_STAT_FILTER";
const GetTypeURL = window.location.pathname + "?handler=ReferenceValue&RefCode=ACTIVITY_TYPE";
const GetSubTypeURL = window.location.pathname + "?handler=LogActivitySubType";
const UpdateLogActivityURL = window.location.pathname + "/Edit";
const UpdateApplicantLogActivityPostURL = window.location.pathname + "/Edit?handler=Applicant";
const UpdateEmployeeLogActivityPostURL = window.location.pathname + "/Edit?handler=Employee";

const ReferredByAutoComplete = window.location.pathname + "?handler=ReferredBy";
const GetApplicantLogActivityStatusHistoryURL = window.location.pathname + "?handler=ApplicantLogActivityStatusHistory";
const GetEmployeeLogActivityStatusHistoryURL = window.location.pathname + "?handler=EmployeeLogActivityStatusHistory";
const GetApplicantCommentsURL = window.location.pathname + "/Edit?handler=ApplicantComments";
const SaveApplicantCommentsURL = window.location.pathname + "/Edit?handler=SaveApplicantComments";
const GetEmployeeCommentsURL = window.location.pathname + "/Edit?handler=EmployeeComments";
const SaveEmployeeCommentsURL = window.location.pathname + "/Edit?handler=SaveEmployeeComments";
const GetApplicantAttachmentURL = window.location.pathname + "/Edit?handler=ApplicantAttachment";
const SaveApplicantAttachmentURL = window.location.pathname + "/Edit?handler=SaveApplicantAttachment";
const GetEmployeeAttachmentURL = window.location.pathname + "/Edit?handler=EmployeeAttachment";
const SaveEmployeeAttachmentURL = window.location.pathname + "/Edit?handler=SaveEmployeeAttachment";
const CheckFileIfExistsURL = window.location.pathname + "/Edit?handler=CheckFileIfExists";
const DownloadFileURL = window.location.pathname + "/Edit?handler=DownloadFile";
const AttachmentTypeDropDown = window.location.pathname + "/Edit?handler=ReferenceValue&RefCode=ATTACHMENT_TYPE";
const TaskBatchUpdateURL = window.location.pathname + "/BatchUpdate";
const TaskBatchUpdatePostURL = window.location.pathname + "/BatchUpdate";
const EmployeeAutoCompleteURL = window.location.pathname + "?handler=EmployeeAutoComplete";
const ApplicantAutoCompleteURL = window.location.pathname + "?handler=ApplicantAutoComplete";
const OrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgGroupAutoComplete";
const BatchStatusDropDownURL = window.location.pathname + "/BatchUpdate?handler=BatchStatusDropDown";
const GetCheckExportListURL = window.location.pathname + "?handler=CheckExportList";
const DownloadExportListURL = window.location.pathname + "?handler=DownloadExportList";


$(document).ready(function () {
    objAssignedActivitiesListJS = {

        Initialize: function () {
            var s = this;
            
            s.ElementBinding();
            var param = {
                ID: localStorage["AssignedActivitiesListID"],
                EmployeeDelimited: localStorage["AssignedActivitiesListEmployee"],
                ApplicantDelimited: localStorage["AssignedActivitiesListApplicant"],
                TypeDelimited: localStorage["AssignedActivitiesListType"],
                //SubTypeDelimited: localStorage["AssignedActivitiesListSubType"],
                Title: localStorage["AssignedActivitiesListTitle"],
                //Description: localStorage["AssignedActivitiesListDescription"],
                CurrentStatusDelimited: localStorage["AssignedActivitiesListCurrentStatus"],
                CurrentTimestampFrom: localStorage["AssignedActivitiesListCurrentTimestampFrom"],
                CurrentTimestampTo: localStorage["AssignedActivitiesListCurrentTimestampTo"],
                DueDateFrom: localStorage["AssignedActivitiesListDueDateFrom"],
                DueDateTo: localStorage["AssignedActivitiesListDueDateTo"],
                AssignedByDelimited: localStorage["AssignedActivitiesListAssignedBy"],
                OrgGroupDelimited: localStorage["AssignedActivitiesListOrgGroup"],
                //Remarks: localStorage["AssignedActivitiesListRemarks"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabHistory', '#divLogActivityModal');

            if (window.location.pathname.indexOf("all") >= 0) {
                $(".list-title").text("All Tasks");
            }
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

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterEmployee"
                , EmployeeAutoCompleteURL, 20, "multiSelectedEmployee");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterApplicant"
                , ApplicantAutoCompleteURL, 20, "multiSelectedApplicant");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterAssignedTaskID").val(),
                    EmployeeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").value,
                    ApplicantDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedApplicant").value,
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
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    //Remarks: $("#txtFilterRemarks").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAssignedActivitiesList");
                s.LoadJQGrid(param);
                $("#divAssignedActivitiesListErrorMessage").html("");
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
                $("#multiSelectedEmployee").html("");
                $("#multiSelectedEmployeeOption label, #multiSelectedEmployeeOption input").prop("title", "add");
                $("#multiSelectedApplicant").html("");
                $("#multiSelectedApplicantOption label, #multiSelectedApplicantOption input").prop("title", "add");
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedOrgGroupOption label, #multiSelectedOrgGroupOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnBatchUpdate").click(function () {

                var input = $("#tblAssignedActivitiesList").jqGrid("getGridParam", "selarrrow");
                var statusList = [];

                for (var x = 0; x < input.length; x++) {
                    var rowdata = $("#tblAssignedActivitiesList").jqGrid("getRowData", input[x]);
                    statusList.push(rowdata["CurrentStatus"]);
                }

                if (input.length == 0) {
                    //ModalAlert(MODAL_HEADER, "Please select atleast one task.");
                    $("#divAssignedActivitiesListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select atleast one task" + "</li></label><br />");
                }
                else if (objAssignedActivitiesListJS.ArrayUnique(statusList))
                {
                    //ModalAlert(MODAL_HEADER, "Please select tasks with same status.");
                    $("#divAssignedActivitiesListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select tasks with same current status only" + "</li></label><br />");
                }
                else {
                    $("#divAssignedActivitiesListErrorMessage").html("");
                    LoadPartial(TaskBatchUpdateURL, "divTaskBatchUpdateBodyModal");
                    $("#divTaskBatchUpdateModal").modal("show");
                }
            });

            $("#btnExport").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objAssignedActivitiesListJS.ExportFunction()",
                    "function");
            });

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedType", GetTypeURL, "Value", "Description");
            //objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedSubType", GetSubTypeURL, "Value", "Description");
            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedCurrentStatus", GetCurrentStatusURL, "Value", "Description");

        },

        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblAssignedActivitiesList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblAssignedActivitiesList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterAssignedTaskID").val()
                + "&EmployeeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").value
                + "&ApplicantDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedApplicant").value
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
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
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
            var tableInfo = localStorage.getItem("tblAssignedActivitiesList") == "" ? "" : $.parseJSON(localStorage.getItem("tblAssignedActivitiesList"));

            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
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
            $("#tblAssignedActivitiesList").jqGrid("GridUnload");
            $("#tblAssignedActivitiesList").jqGrid("GridDestroy");
            $("#tblAssignedActivitiesList").jqGrid({
                url: AssignedActivitiesListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Type", "Sub-Type", "Title", "Description", "Assigned By", "Due Date",
                        "Current Status", "Employee Name", "Applicant Name", "Org Group", "Timestamp", "Remarks", "", "", ""
                ],
                colModel: [
                    { width: 100, key: true, name: "ID", index: "ID", align: "center", sortable: false, formatter: objAssignedActivitiesListJS.UpdateLink },
                    { name: "Type", index: "Type", align: "left", sortable: false },
                    { hidden: true, name: "SubType", index: "SubType", align: "left", sortable: false },
                    { name: "Title", index: "Title", align: "left", sortable: false },
                    { hidden: true, name: "Description", index: "Description", align: "left", sortable: false },
                    { name: "AssignedBy", index: "AssignedBy", align: "left", sortable: false },
                    { name: "DueDate", index: "DueDate", align: "left", sortable: false },
                    { name: "CurrentStatus", index: "CurrentStatus", align: "left", sortable: false },
                    { width: 200, name: "EmployeeName", index: "EmployeeName", align: "left", sortable: false },
                    { width: 200, name: "ApplicantName", index: "ApplicantName", align: "left", sortable: false },
                    { name: "OrgGroupName", index: "OrgGroupName", align: "left", sortable: false },
                    { name: "CurrentTimestamp", index: "CurrentTimestamp", align: "left", sortable: false },
                    { hidden: true, name: "Remarks", index: "Remarks", align: "left", sortable: false },
                    { hidden: true, name: "ApplicantID", index: "ApplicantID", align: "left", sortable: false },
                    { hidden: true, name: "EmployeeID", index: "EmployeeID", align: "left", sortable: false },
                    { hidden: true, name: "CurrentStatusCode", index: "CurrentStatusCode", align: "left", sortable: false },
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
                    $("#tblAssignedActivitiesList tr:nth-child(" + (iRow + 1) + ") .activity-id-link").click();
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
                                if (data.rows[i].CurrentStatus == "DONE" || data.rows[i].CurrentStatus == "CANCELLED") {
                                    $("#jqg_tblAssignedActivitiesList_" + data.rows[i].ID).attr("disabled", true);
                                }
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblAssignedActivitiesList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblAssignedActivitiesList .jqgrid-id-link").click(function () {
                            $('#divLogActivityModal').modal('show');
                        });

                    }

                    if (localStorage["AssignedActivitiesListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["AssignedActivitiesListFilterOption"]));
                    }
                    objAssignedActivitiesListJS.ShowHideFilter();
                    
                    $("#chkFilter").on('change', function () {
                        objAssignedActivitiesListJS.ShowHideFilter();
                        localStorage["AssignedActivitiesListFilterOption"] = $("#chkFilter").is(":checked");
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
                onSelectAll: function (aRowids, status) {
                    if (status)
                    {
                        // uncheck "protected" rows
                        var cbs = $("tr.jqgrow > td > input.cbox:disabled", $(this));
                        cbs.removeAttr("checked");

                        //modify the selarrrow parameter 
                        $(this)[0].p.selarrrow = $(this).find("tr.jqgrow:has(td > input.cbox:checked)")
                            .map(function () { return this.id; }) // convert to set of ids 
                            .get(); // convert to instance of Array 
                    }
                },
                beforeSelectRow: function (id, e) {
                    var cbsdis = $("tr#" + id + ".jqgrow > td > input.cbox:disabled", $(this));
                        if (cbsdis.length == 0) {
                            return true; // allow select the row 
                        }
                        else {
                            return false; // not allow select the row 
                        } 
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblAssignedActivitiesList");
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
            localStorage["AssignedActivitiesListID"] = $("#txtFilterAssignedTaskID").val();
            localStorage["AssignedActivitiesListType"] = objEMSCommonJS.GetMultiSelectList("multiSelectedType").value;
            localStorage["AssignedActivitiesListTypeText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedType").text;
            //localStorage["AssignedActivitiesListSubType"] = objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").value;
            //localStorage["AssignedActivitiesListSubTypeText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").text;

            localStorage["AssignedActivitiesListTitle"] = $("#txtFilterTitle").val();
            //localStorage["AssignedActivitiesListDescription"] = $("#txtFilterDescription").val();

            localStorage["AssignedActivitiesListCurrentStatus"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").value;
            localStorage["AssignedActivitiesListCurrentStatusText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").text;

            localStorage["AssignedActivitiesListCurrentTimestampFrom"] = $("#txtFilterCurrentTimestampFrom").val();
            localStorage["AssignedActivitiesListCurrentTimestampTo"] = $("#txtFilterCurrentTimestampTo").val();

            localStorage["AssignedActivitiesListDueDateFrom"] = $("#txtFilterDueDateFrom").val();
            localStorage["AssignedActivitiesListDueDateTo"] = $("#txtFilterDueDateTo").val();

            localStorage["AssignedActivitiesListAssignedBy"] = objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedBy").value;
            localStorage["AssignedActivitiesListAssignedByText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedAssignedBy").text;

            localStorage["AssignedActivitiesListEmployee"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").value;
            localStorage["AssignedActivitiesListEmployeeText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").text;

            localStorage["AssignedActivitiesListApplicant"] = objEMSCommonJS.GetMultiSelectList("multiSelectedApplicant").value;
            localStorage["AssignedActivitiesListApplicantText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedApplicant").text;

            localStorage["AssignedActivitiesListOrgGroup"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["AssignedActivitiesListOrgGroupText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;

            //localStorage["AssignedActivitiesListRemarks"] = $("#txtFilterRemarks").val();
        },

        GetLocalStorage: function () {

            $("#txtFilterAssignedTaskID").val(localStorage["AssignedActivitiesListID"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedType"
                , "AssignedActivitiesListType"
                , "AssignedActivitiesListTypeText");
            //objEMSCommonJS.SetMultiSelectList("multiSelectedSubType"
            //    , "AssignedActivitiesListSubType"
            //    , "AssignedActivitiesListSubTypeText");

            $("#txtFilterTitle").val(localStorage["AssignedActivitiesListTitle"]);
            //$("#txtFilterDescription").val(localStorage["AssignedActivitiesListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStatus"
                , "AssignedActivitiesListCurrentStatus"
                , "AssignedActivitiesListCurrentStatusText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedAssignedBy"
                , "AssignedActivitiesListAssignedBy"
                , "AssignedActivitiesListAssignedByText");

            $("#txtFilterCurrentTimestampFrom").val(localStorage["AssignedActivitiesListCurrentTimestampFrom"]);
            $("#txtFilterCurrentTimestampTo").val(localStorage["AssignedActivitiesListCurrentTimestampTo"]);

            $("#txtFilterDueDateFrom").val(localStorage["AssignedActivitiesListDueDateFrom"]);
            $("#txtFilterDueDateTo").val(localStorage["AssignedActivitiesListDueDateTo"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedEmployee"
                , "AssignedActivitiesListEmployee"
                , "AssignedActivitiesListEmployeeText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedApplicant"
                , "AssignedActivitiesListApplicant"
                , "AssignedActivitiesListApplicantText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "AssignedActivitiesListOrgGroup"
                , "AssignedActivitiesListOrgGroupText");

            //$("#txtFilterRemarks").val(localStorage["AssignedActivitiesListRemarks"]);
        },

        UpdateLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return objAssignedActivitiesListJS.ViewUpdateStatusModal('" + UpdateLogActivityURL
                + "?ID=" + rowObject.ID + "&ApplicantID=" + rowObject.ApplicantID + "&EmployeeID=" + rowObject.EmployeeID + "');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
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
        },

        ArrayUnique: function (data) {
            return data.length !== 0 && new Set(data).size !== 1;
        },
    };

    objAssignedActivitiesListJS.Initialize();
});