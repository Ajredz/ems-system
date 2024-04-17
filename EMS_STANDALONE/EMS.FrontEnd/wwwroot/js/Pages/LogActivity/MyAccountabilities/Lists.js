var objMyAccountabilitiesListJS;
const MyAccountabilitiesListURL = window.location.pathname + "?handler=List";
const GetApplicantLogActivitiesURL = window.location.pathname + "?handler=ApplicantLogActivities";
const GetCurrentStatusURL = window.location.pathname + "?handler=StatusFilter";
const GetTypeURL = window.location.pathname + "?handler=ReferenceValue&RefCode=ACCOUNTABILITY_TYPE";
const UpdateAccountabilityURL = window.location.pathname + "/Edit";
const UpdateAccountabilityPostURL = window.location.pathname + "/Edit?handler=Employee";

const ReferredByAutoComplete = window.location.pathname + "?handler=ReferredBy";
const GetAccountabilityStatusHistoryURL = window.location.pathname + "?handler=AccountabilityStatusHistory";
const GetAccountabilityCommentsURL = window.location.pathname + "/Edit?handler=EmployeeComments";
const SaveAccountabilityCommentsURL = window.location.pathname + "/Edit?handler=SaveEmployeeComments";
const GetAccountabilityAttachmentURL = window.location.pathname + "/Edit?handler=EmployeeAttachment";
const SaveAccountabilityAttachmentURL = window.location.pathname + "/Edit?handler=SaveEmployeeAttachment";
const CheckFileIfExistsURL = window.location.pathname + "/Edit?handler=CheckFileIfExists";
const DownloadFileURL = window.location.pathname + "/Edit?handler=DownloadFile";
const AttachmentTypeDropDown = window.location.pathname + "/Edit?handler=ReferenceValue&RefCode=ATTACHMENT_TYPE";
const OrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgTypeAutoComplete";
const OrgGroupByOrgTypeAutoCompleteURL = window.location.pathname + "?handler=OrgGroupByOrgTypeAutoComplete";
const EmployeeAutoCompleteURL = window.location.pathname + "?handler=EmployeeAutoComplete";
const GetCheckExportListURL = window.location.pathname + "?handler=CheckExportList";
const ExportMyAccountabilitiesListURL = window.location.pathname + "?handler=ExportMyAccountabilitiesList";
const ExportAllAccountabilitiesListURL = window.location.pathname + "?handler=ExportAllAccountabilitiesList";
const ExportClearanceListURL = window.location.pathname + "?handler=ExportClearanceList";
const OldEmployeeIDAutoCompleteURL = window.location.pathname + "?handler=OldEmployeeIDAutoComplete";
const UploadInsertURL = window.location.pathname + "?handler=UploadInsert";
const DownloadFormURL = window.location.pathname + "?handler=DownloadAccountabilityTemplate";

var AccountabilityDeleteURL = window.location.pathname + "?handler=DeleteAccountability";
var AccountabilityChangeStatus = window.location.pathname + "?handler=AccountabilityChangeStatus";

const PositionAutoCompleteURL = window.location.pathname + "?handler=PositionAutoComplete";

const GetQuestionByIDURL = window.location.pathname + "?handler=QuestionByCategory&Category=EXITINTERVIEW";

const GetEmployeeMovementByEmployeeIDsURL = window.location.pathname + "?handler=EmployeeMovementByEmployeeIDs";

const GetCheckQuestionEmployeeAnswersExportURL = window.location.pathname + "?handler=CheckQuestionEmployeeAnswersExport";
const GetQuestionEmployeeAnswersExportURL = window.location.pathname + "?handler=QuestionEmployeeAnswersExport";

$(document).ready(function () {
    objMyAccountabilitiesListJS = {

        Initialize: function () {
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            s.ElementBinding();
            var param = {
                TypeDelimited: localStorage["MyAccountabilitiesListType"],
                Title: localStorage["MyAccountabilitiesListTitle"],
                Description: localStorage["MyAccountabilitiesListDescription"],
                CurrentStatusDelimited: localStorage["MyAccountabilitiesListCurrentStatus"],
                CurrentTimestampFrom: localStorage["MyAccountabilitiesListCurrentTimestampFrom"],
                CurrentTimestampTo: localStorage["MyAccountabilitiesListCurrentTimestampTo"],
                OrgGroupIDDelimited: localStorage["MyAccountabilitiesListOrgGroup"],
                EmployeeIDDelimited: localStorage["MyAccountabilitiesListEmployee"],
                Remarks: localStorage["MyAccountabilitiesListRemarks"],
                ModifiedByDelimited: localStorage["MyAccountabilitiesListModifiedBy"],
                OldEmployeeID: localStorage["MyAccountabilitiesListOldEmployeeID"],
                OpenStatusOnly: localStorage["AccountabilityFilterOpenStatusOnly"] == undefined ? true : localStorage["AccountabilityFilterOpenStatusOnly"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            //objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabHistory', '#divLogActivityModal');

            if (window.location.pathname.indexOf("all") >= 0) {
                $(".list-title").text("All Accountabilities");
            }
            else if (window.location.pathname.indexOf("clearance") >= 0) {
                $(".list-title").text("Clearance List");
            }

            $("#btnBulkChangeStatus").show();
            $("#btnDeleteAccountability").show();

        },

        SuccessFunction: function () {
            $("#btnSearch").click();
        },

        AccountabilityChangeStatusSuccessFunction: function () {
            $("#divChangeStatusModal").modal("hide");
            $("#txtChangeStatusRemarks").val("");
            $("#btnSearch").click();
        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterCreatedDateFrom, #txtFilterCreatedDateTo, \
                #txtFilterSeparationFrom,#txtFilterSeparationTo, \
                #txtFilterDateHiredFrom,#txtFilterDateHiredTo, \
                #txtFilterStatusUpdatedDateFrom,#txtFilterStatusUpdatedDateTo, \
                #txtFilterLastCommentDateFrom, #txtFilterLastCommentDateTo").datetimepicker({
                    useCurrent: false,
                    format: 'MM/DD/YYYY'
            });


            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterEmployee"
                , EmployeeAutoCompleteURL, 20, "multiSelectedEmployee");

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedStatus", GetCurrentStatusURL, "ID", "Description");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterEmployeeOrg"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedEmployeeOrg");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterEmployeeOrgRegion"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=REG", 20, "multiSelectedEmployeeOrgRegion");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterEmployeePos"
                , PositionAutoCompleteURL, 20, "multiSelectedEmployeePos");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterClearingOrg"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedClearingOrg");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterStatusUpdatedBy"
                , ReferredByAutoComplete, 20, "multiSelectedStatusUpdatedBy");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOldEmployeeID"
                , OldEmployeeIDAutoCompleteURL, 20, "multiSelectedOldEmployeeID");

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterID").val(),
                    CreatedDateFrom: $("#txtFilterCreatedDateFrom").val(),
                    CreatedDateTo: $("#txtFilterCreatedDateTo").val(),
                    EmployeeIDDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").value,
                    SeparationDateFrom: $("#txtFilterSeparationFrom").val(),
                    SeparationDateTo: $("#txtFilterSeparationTo").val(),
                    HiredDateFrom: $("#txtFilterDateHiredFrom").val(),
                    HiredDateTo: $("#txtFilterDateHiredTo").val(),
                    Title: $("#txtFilterTitle").val(),
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value,
                    StatusUpdatedDateFrom: $("#txtFilterStatusUpdatedDateFrom").val(),
                    StatusUpdatedDateTo: $("#txtFilterStatusUpdatedDateTo").val(),
                    EmployeeOrgDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeOrg").value,
                    EmployeeOrgRegionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeOrgRegion").value,
                    EmployeePosDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeePos").value,
                    ClearingOrgDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedClearingOrg").value,
                    StatusUpdatedByDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedStatusUpdatedBy").value,
                    StatusRemarks: $("#txtFilterStatusRemarks").val(),
                    LastComment: $("#txtFilterLastComment").val(),
                    LastCommentDateFrom: $("#txtFilterLastCommentDateFrom").val(),
                    LastCommentDateTo: $("#txtFilterLastCommentDateTo").val(),

                    OpenStatusOnly: objMyAccountabilitiesListJS.FilterOpenStatusOnly(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblMyAccountabilitiesList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedEmployee").html("");
                $("#multiSelectedEmployeeOption label, #multiSelectedEmployeeOption input").prop("title", "add");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#multiSelectedEmployeeOrg").html("");
                $("#multiSelectedEmployeeOrgOption label, #multiSelectedEmployeeOrgOption input").prop("title", "add");
                $("#multiSelectedEmployeeOrgRegion").html("");
                $("#multiSelectedEmployeeOrgRegionOption label, #multiSelectedEmployeeOrgRegionOption input").prop("title", "add");
                $("#multiSelectedEmployeePos").html("");
                $("#multiSelectedEmployeePosOption label, #multiSelectedEmployeePosOption input").prop("title", "add");
                $("#multiSelectedClearingOrg").html("");
                $("#multiSelectedClearingOrgOption label, #multiSelectedClearingOrgOption input").prop("title", "add");
                $("#multiSelectedStatusUpdatedBy").html("");
                $("#multiSelectedStatusUpdatedByOption label, #multiSelectedStatusUpdatedByOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objMyAccountabilitiesListJS.ExportFunction()",
                    "function");
            });

            $("#btnUpload").click(function () {
                objEMSCommonJS.UploadModal(UploadInsertURL, "Upload", DownloadFormURL);
                $('#divModalErrorMessage').html('');
            });

            $("#btnDeleteAccountability").click(function () {
                $('#divAccountabilityErrorMessage').html('');
                var selRow = $("#tblMyAccountabilitiesList").jqGrid("getGridParam", "selarrrow");
                if (selRow.length > 0) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE + SPAN_DELETE_START + " ID : " + selRow + SPAN_END,
                        "objEMSCommonJS.PostAjax(true \
                        , '" + AccountabilityDeleteURL + "&ID=" + selRow + "' \
                        , {} \
                        , '#divAccountabilityErrorMessage' \
                        , '#btnDeleteAccountability' \
                        , objMyAccountabilitiesListJS.SuccessFunction);",
                        "function");
                }
                else {
                    $("#divAccountabilityErrorMessage").append("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Accountability</li></label><br />");
                }
            });

            $("#btnBulkChangeStatus").click(function () {
                $('#divAccountabilityErrorMessage').html('');
                var selRow = $("#tblMyAccountabilitiesList").jqGrid("getGridParam", "selarrrow");
                var firstValue = "";
                var isValid = true;
                if (selRow.length > 0) {
                    $(selRow).each(function (index, item) {
                        if (firstValue == "")
                            firstValue = $("#tblMyAccountabilitiesList").getRowData(item).Status;
                        else if (firstValue != $("#tblMyAccountabilitiesList").getRowData(item).Status)
                            isValid = false;
                    });
                    if (isValid) {
                        //firstValue = $(firstValue).text();

                        //$("#divtest").show();
                        $(".editRequired").addClass("reqField");
                        $("#divChangeStatusModal").modal("show");
                        $('#divAccountabilityChangeStatusErrorMessage').html('');
                        $("#txtChangeStatusRemarks").val("");
                        $("#AccountabilityChangeStatusID").val(selRow);
                        
                        var Clearance = "Clearance";
                        if (window.location.pathname.indexOf("myaccountabilities") >= 0)
                            Clearance = "Employee";
                        GenerateDropdownValues(AccountabilityChangeStatus + "&CurrentStatus=" + firstValue + "&Form=" + Clearance, "ddlAccountabilityChangeStatus", "Value", "Text", "", "", false);
                    }
                    else
                        $("#divAccountabilityErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_DIFF_STATUS + " </li></label><br />");
                }
                else {
                    $("#divAccountabilityErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Accountability</li></label><br />");
                }
            });

            $("#ddlAccountabilityChangeStatus").change(function () {
                if ($("#ddlAccountabilityChangeStatus").val() == "CANCELLED") {
                    $("#remarksRequired").addClass("reqField");
                    $("#remarksRequired").removeClass("unreqField");
                }
                else {
                    $("#remarksRequired").addClass("unreqField");
                    $("#remarksRequired").removeClass("reqField");
                }

            });

            $("#btnSaveAccountabilityChangeStatus").click(function () {
                $('#divAccountabilityChangeStatusErrorMessage').html('');
                if ($("#ddlAccountabilityChangeStatus").val() != "") {
                    if ($("#ddlAccountabilityChangeStatus").val() == "CANCELLED" && $("#txtChangeStatusRemarks").val().trim() == "") {
                        $("#divAccountabilityChangeStatusErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_INPUT_REMARKS + "</li></label><br />");
                        $("#txtChangeStatusRemarks").addClass("required-field errMessage");
                        return;
                    }
                    $("#txtChangeStatusRemarks").removeClass("required-field errMessage");
                    var ID = $("#AccountabilityChangeStatusID").val();
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_UPDATE + SPAN_DELETE_START + " ID : " + ID + SPAN_END,
                        "objEMSCommonJS.PostAjax(true \
                        , '" + AccountabilityChangeStatus + "&ID=" + ID + "&NewStatus=" + $("#ddlAccountabilityChangeStatus").val() + "&Remarks=" + $("#txtChangeStatusRemarks").val() + "' \
                        , {} \
                        , '#divAccountabilityChangeStatusErrorMessage' \
                        , '#btnSaveAccountabilityChangeStatus' \
                        , objMyAccountabilitiesListJS.AccountabilityChangeStatusSuccessFunction);",
                        "function");
                }
                else
                    $("#divAccountabilityChangeStatusErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + "</li></label><br />");
            });

            $("#btnExportExitInterview").on("click", function () {
                //console.log("sad");
                //$('#divExitInterviewModal').modal('show');

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objMyAccountabilitiesListJS.ExportQuestionEmployeeAnswerFunction()",
                    "function");
            });
        },

        ExportFunction: function () {
            var parameter1 = "&sidx=" + $("#tblMyAccountabilitiesList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblMyAccountabilitiesList").jqGrid("getGridParam", "sortorder")

                + "&ID=" + $("#txtFilterID").val()
                + "&CreatedDateFrom=" + $("#txtFilterCreatedDateFrom").val()
                + "&CreatedDateTo=" + $("#txtFilterCreatedDateTo").val()
                + "&EmployeeIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").value
                + "&Title=" + $("#txtFilterTitle").val()
                + "&StatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value
                + "&StatusUpdatedDateFrom=" + $("#txtFilterStatusUpdatedDateFrom").val()
                + "&StatusUpdatedDateTo=" + $("#txtFilterStatusUpdatedDateTo").val()
                + "&EmployeeOrgDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeOrg").value
                + "&EmployeeOrgRegionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeOrgRegion").value
                + "&EmployeePosDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeePos").value
                + "&ClearingOrgDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedClearingOrg").value
                + "&StatusUpdatedByDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedStatusUpdatedBy").value
                + "&StatusRemarks=" + $("#txtFilterStatusRemarks").val()
                + "&LastComment=" + $("#txtFilterLastComment").val()
                + "&LastCommentDateFrom=" + $("#txtFilterLastCommentDateFrom").val()
                + "&LastCommentDateTo=" + $("#txtFilterLastCommentDateTo").val()
                + "&OpenStatusOnly=" + objMyAccountabilitiesListJS.FilterOpenStatusOnly()

                + "&IsExport=true";

            /*var parameter2 = "&sidx=" + $("#tblMyAccountabilitiesList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblMyAccountabilitiesList").jqGrid("getGridParam", "sortorder")
                + "&TypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedType").value
                + "&Title=" + $("#txtFilterTitle").val()
                + "&Description=" + $("#txtFilterDescription").val()
                + "&CurrentStatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").value
                + "&CurrentTimestampFrom=" + $("#txtFilterCurrentTimestampFrom").val()
                + "&CurrentTimestampTo=" + $("#txtFilterCurrentTimestampTo").val()
                + "&EmployeeIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").value
                + "&Remarks=" + $("#txtFilterRemarks").val()
                + "&ModifiedByDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedModifiedBy").value
                + "&OldEmployeeID=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOldEmployeeID").value;

            var parameter3 = "&sidx=" + $("#tblMyAccountabilitiesList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblMyAccountabilitiesList").jqGrid("getGridParam", "sortorder")
                + "&TypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedType").value
                + "&Title=" + $("#txtFilterTitle").val()
                + "&Description=" + $("#txtFilterDescription").val()
                + "&CurrentStatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").value
                + "&CurrentTimestampFrom=" + $("#txtFilterCurrentTimestampFrom").val()
                + "&CurrentTimestampTo=" + $("#txtFilterCurrentTimestampTo").val()
                + "&OrgGroupIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                + "&Remarks=" + $("#txtFilterRemarks").val()
                + "&ModifiedByDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedModifiedBy").value
                + "&OldEmployeeID=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOldEmployeeID").value;*/

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {

                    window.location = ExportAllAccountabilitiesListURL + parameter1;
                    $("#divModal").modal("hide");

                    /*if (window.location.pathname.indexOf("all") >= 0) {
                        window.location = ExportAllAccountabilitiesListURL + parameter1;
                        $("#divModal").modal("hide");
                    }
                    else if (window.location.pathname.indexOf("clearance") >= 0) {
                        window.location = ExportClearanceListURL + parameter2;
                        $("#divModal").modal("hide");
                    }
                    else {
                        window.location = ExportMyAccountabilitiesListURL + parameter3;
                        $("#divModal").modal("hide");
                    }*/
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckExportListURL + parameter1, {}, "#btnExport", GetSuccessFunction, null, true);

            /*if (window.location.pathname.indexOf("all") >= 0) {
                objEMSCommonJS.GetAjax(GetCheckExportListURL + parameter1, {}, "#btnExport", GetSuccessFunction, null, true);
            }
            else if (window.location.pathname.indexOf("clearance") >= 0) {
                objEMSCommonJS.GetAjax(GetCheckExportListURL + parameter2, {}, "#btnExport", GetSuccessFunction, null, true);
            }
            else {
                objEMSCommonJS.GetAjax(GetCheckExportListURL + parameter3, {}, "#btnExport", GetSuccessFunction, null, true);
            }*/
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblMyAccountabilitiesList") == "" ? "" : $.parseJSON(localStorage.getItem("tblMyAccountabilitiesList"));

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
            $("#tblMyAccountabilitiesList").jqGrid("GridUnload");
            $("#tblMyAccountabilitiesList").jqGrid("GridDestroy");
            $("#tblMyAccountabilitiesList").jqGrid({
                url: MyAccountabilitiesListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Employee", "Date Separated", "Date Hired", "Title", "Status", "Status Updated Date", "Employee Branch / Department","Employee Region", "Employee Position",
                    "Clearing Department", "Status Updated By", "Status Remarks", "Last Comment", "Last Comment Date", "Created Date", ""
                ],
                colModel: [
                    {
                        key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objMyAccountabilitiesListJS.UpdateLink, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="border-left:10px solid ' + rowObject.StatusColor + ' !important;"';
                        }
                    },
                    { name: "EmployeeName", index: "EmployeeName", align: "left", sortable: true },
                    { name: "EmployeeStatusUpdatedDate", index: "EmployeeStatusUpdatedDate", align: "left", sortable: true },
                    { name: "EmployeeDatehired", index: "EmployeeDatehired", align: "left", sortable: true },
                    { name: "Title", index: "Title", align: "left", sortable: true },
                    {
                        name: "StatusDescription", index: "StatusDescription", align: "left", sortable: true, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="background-color:' + rowObject.StatusColor + '"';
                        }
                    },
                    { name: "StatusUpdateDate", index: "StatusUpdateDate", align: "left", sortable: true },
                    { name: "EmployeeOrg", index: "EmployeeOrg", align: "left", sortable: true },
                    { name: "EmployeeOrgRegion", index: "EmployeeOrgRegion", align: "left", sortable: true },
                    { name: "EmployeePos", index: "EmployeePos", align: "left", sortable: true },
                    { name: "ClearingOrg", index: "ClearingOrg", align: "left", sortable: true },
                    { name: "StatusUpdatedByName", index: "StatusUpdatedByName", align: "left", sortable: true },
                    { name: "StatusRemarks", index: "StatusRemarks", align: "left", sortable: true },
                    { name: "LastComment", index: "LastComment", align: "left", sortable: true },
                    { name: "LastCommentDate", index: "LastCommentDate", align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", align: "left", sortable: true },
                    { hidden: true, name: "Status", index: "Status", align: "left", sortable: true },
                ],
                toppager: $("#divPager"),
                pager: $("#divPager"),
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
                    $("#tblMyAccountabilitiesList tr:nth-child(" + (iRow + 1) + ") .activity-id-link").click();
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
                        // Show Filter By Region Column if user is non admin
                        if (window.location.pathname.indexOf("all") >= 0) {
                            //$("#tblMyAccountabilitiesList").jqGrid('showCol', ["Name"]);
                            //$("#tblMyAccountabilitiesList").jqGrid('showCol', ["OldEmployeeID"]);
                            //$("#divEmployeeFilter").show();
                            //$("#divOldEmployeeIDFilter").show();
                        }
                        else if (window.location.pathname.indexOf("clearance") >= 0)
                        {
                            //$("#tblMyAccountabilitiesList").jqGrid('showCol', ["Name"]);
                            //$("#tblMyAccountabilitiesList").jqGrid('showCol', ["OldEmployeeID"]);
                            //$("#divEmployeeFilter").show();
                            //$("#divOldEmployeeIDFilter").show();
                            //$("#tblMyAccountabilitiesList").jqGrid('hideCol', ["ClearingOrg"]);
                            //$("#divOrgGroupFilter").hide();
                        }

                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                                if (data.rows[i].Status == "CLEARED" || data.rows[i].Status == "CANCELLED") {
                                    $("#jqg_tblMyAccountabilitiesList_" + data.rows[i].ID).prop("disabled", true);
                                }
                                if ($("#hdnClearingHRBP").val() == "true" && !data.rows[i].Title.includes("HR – TALENT MANAGEMENT - Exit Interview")) {
                                    $("#jqg_tblMyAccountabilitiesList_" + data.rows[i].ID).prop("disabled", true);
                                }
                            }
                        }

                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblMyAccountabilitiesList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblMyAccountabilitiesList .jqgrid-id-link").click(function () {
                            $('#divAccountabilityModal').modal('show');
                        });

                    }

                    if (localStorage["MyAccountabilitiesListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["MyAccountabilitiesListFilterOption"]));
                    }
                    objMyAccountabilitiesListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objMyAccountabilitiesListJS.ShowHideFilter();
                        localStorage["MyAccountabilitiesListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    // FILTER OPEN STATUS ONLY
                    if (localStorage["AccountabilityFilterOpenStatusOnly"] != undefined) {
                        $("#chkOpenStatus").prop('checked', JSON.parse(localStorage["AccountabilityFilterOpenStatusOnly"]));
                    }
                    objMyAccountabilitiesListJS.FilterOpenStatusOnly();
                    $("#chkOpenStatus").on('change', function () {
                        objMyAccountabilitiesListJS.FilterOpenStatusOnly();
                        localStorage["AccountabilityFilterOpenStatusOnly"] = $("#chkOpenStatus").is(":checked");
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
                    $("#divPager").css("width", "100%");
                    $("#divPager").css("height", "100%");
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeSelectRow: function (id, e) {
                    var cbsdis = $("tr#" + id + ".jqgrow > td > input.cbox:disabled", $(this));

                    if (cbsdis.length == 0) {
                        return true;    // allow select the row
                    } else {
                        return false;   // not allow select the row
                    }
                },
                onSelectAll: function (aRowids, status) {
                    if (status) {
                        var grid = $("#tblMyAccountabilitiesList"), i;
                        // uncheck "protected" rows
                        var cbs = $("tr.jqgrow > td > input.cbox:disabled", grid[0]);
                        cbs.removeAttr("checked");

                        //modify the selarrrow parameter
                        grid[0].p.selarrrow = $(this).find("tr.jqgrow:has(td > input.cbox:checked)")
                            .map(function () { return this.id; }) // convert to set of ids
                            .get(); // convert to instance of Array

                        //deselect disabled rows
                        $(this).find("tr.jqgrow:has(td > input.cbox:disabled)")
                            .attr('aria-selected', 'false')
                            .removeClass('ui-state-highlight');
                    }
                },
                beforeRequest: function () {
                    GetJQGridState("tblMyAccountabilitiesList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#tblMyAccountabilitiesList_toppager_center").hide();
            $("#tblMyAccountabilitiesList_toppager_right").hide();
            $("#tblMyAccountabilitiesList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblShowOpenStatus\">Show Open Status Only</label>");
            jQuery("#lblShowOpenStatus").after("<input type=\"checkbox\" id=\"chkOpenStatus\" style=\"margin-right:15px; margin-top: 5px;\" checked>");
            jQuery("#chkOpenStatus").after("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filters</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\">");
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divPager_custom_block_right").appendTo("#divPager_left");
            $("#divPager_center .ui-pg-table").appendTo("#divPager_right");
        },

        SetLocalStorage: function () {
            localStorage["MyAccountabilitiesListType"] = objEMSCommonJS.GetMultiSelectList("multiSelectedType").value;
            localStorage["MyAccountabilitiesListTypeText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedType").text;

            localStorage["MyAccountabilitiesListTitle"] = $("#txtFilterTitle").val();
            localStorage["MyAccountabilitiesListDescription"] = $("#txtFilterDescription").val();

            localStorage["MyAccountabilitiesListCurrentStatus"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").value;
            localStorage["MyAccountabilitiesListCurrentStatusText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStatus").text;

            localStorage["MyAccountabilitiesListCurrentTimestampFrom"] = $("#txtFilterCurrentTimestampFrom").val();
            localStorage["MyAccountabilitiesListCurrentTimestampTo"] = $("#txtFilterCurrentTimestampTo").val();

            localStorage["MyAccountabilitiesListOrgGroup"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["MyAccountabilitiesListOrgGroupText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;

            localStorage["MyAccountabilitiesListEmployee"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").value;
            localStorage["MyAccountabilitiesListEmployeeText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").text;

            localStorage["MyAccountabilitiesListRemarks"] = $("#txtFilterRemarks").val();

            localStorage["MyAccountabilitiesListModifiedBy"] = objEMSCommonJS.GetMultiSelectList("multiSelectedModifiedBy").value;
            localStorage["MyAccountabilitiesListModifiedByText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedModifiedBy").text;

            localStorage["MyAccountabilitiesListOldEmployeeID"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOldEmployeeID").value;
            localStorage["MyAccountabilitiesListOldEmployeeIDText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOldEmployeeID").text;
        },

        GetLocalStorage: function () {

            objEMSCommonJS.SetMultiSelectList("multiSelectedType"
                , "MyAccountabilitiesListType"
                , "MyAccountabilitiesListTypeText");
            
            $("#txtFilterTitle").val(localStorage["MyAccountabilitiesListTitle"]);
            $("#txtFilterDescription").val(localStorage["MyAccountabilitiesListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStatus"
                , "MyAccountabilitiesListCurrentStatus"
                , "MyAccountabilitiesListCurrentStatusText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "MyAccountabilitiesListOrgGroup"
                , "MyAccountabilitiesListOrgGroupText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedEmployee"
                , "MyAccountabilitiesListEmployee"
                , "MyAccountabilitiesListEmployeeText");

            $("#txtFilterCurrentTimestampFrom").val(localStorage["MyAccountabilitiesListCurrentTimestampFrom"]);
            $("#txtFilterCurrentTimestampTo").val(localStorage["MyAccountabilitiesListCurrentTimestampTo"]);
            $("#txtFilterRemarks").val(localStorage["MyAccountabilitiesListRemarks"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedModifiedBy"
                , "MyAccountabilitiesListModifiedBy"
                , "MyAccountabilitiesListModifiedByText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedOldEmployeeID"
                , "MyAccountabilitiesListOldEmployeeID"
                , "MyAccountabilitiesListOldEmployeeIDText");
        },

        UpdateLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return objMyAccountabilitiesListJS.ViewUpdateStatusModal('" + UpdateAccountabilityURL
                + "?ID=" + rowObject.ID + "');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        ViewUpdateStatusModal: function (url) {
            $('#divAccountabilityModal').modal('show');
            LoadPartial(url, 'divAccountabilityBodyModal');
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

        FilterOpenStatusOnly: function () {
            if ($("#chkOpenStatus").is(":checked")) 
                return true;
            else if ($("#chkOpenStatus").is(":not(:checked)")) 
                return false;
        },

        LoadQuestion: function (EmployeeID) {
            var GetSuccessFunction = function (data) {
                var questionsContainer = $('#divDisplayQuestion').html("");;

                $.each(data.Result, function (index, question) {
                    var defaultCol = 12;
                    var questionCol = defaultCol - question.Tab;
                    var addQuestionType = "";

                    if (question.ParentQuestionID != 0)
                        addQuestionType = '<input type="' + question.QuestionType + '" id="' + question.QuestionID +'" name="' + question.ParentQuestionID + '" disabled >';

                    questionsContainer.append(
                        '<div class="form-group form-fields">' +
                        '<div class="col-md-' + question.Tab +'">' +
                        '</div>' +
                        '<div class="col-md-' + questionCol +'" style="display:inline-flex;">' +
                        addQuestionType + '<label class="control-label">' + question.Question + ' </label>' +
                        '</div>' +
                        '</div>'
                    );

                    if (question.EmployeeAnswerID != null)
                        $("#" + question.QuestionID + ",#" + question.ParentQuestionID).prop("checked", true);

                    if (question.AnswerID == null && question.AnswerType == "TEXTAREA") {
                        if (question.EmployeeAnswerDetails != null) {
                            questionsContainer.append(
                                '<div class="form-group form-fields">' +
                                '<div class="col-md-' + question.Tab + '">' +
                                '</div>' +
                                '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                                '<textarea class="form-control" disabled>' + question.EmployeeAnswerDetails+'</textarea>' +
                                '</div>' +
                                '</div>'
                            );
                        }
                    }
                    if (question.AnswerID == null && question.AnswerType == "NUMBER") {
                        if (question.EmployeeAnswerDetails != null) {
                            questionsContainer.append(
                                '<div class="form-group form-fields">' +
                                '<div class="col-md-' + question.Tab + '">' +
                                '</div>' +
                                '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                                '<input class="form-control" type="text" value="' + question.EmployeeAnswerDetails + '" disabled>' +
                                '</div>' +
                                '</div>'
                            );
                        }
                    }
                    if (question.AnswerID == null && question.AnswerType == "MULTIPLE") {
                        if (question.EmployeeAnswerDetails != null) {
                            $.each(question.EmployeeAnswerDetails.split('&&'), function (index, item) {
                                $.each(item.split('|'), function (index, item) {
                                    questionsContainer.append(
                                        '<div class="col-md-2 form-group form-fields">' +
                                        '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                                        '<input class="form-control" type="text" value="' + item + '" disabled>' +
                                        '</div>' +
                                        '</div>'
                                    );
                                });
                            });
                        }
                    }

                    if (question.Answer != null) {
                        var answerCol = question.Tab + 1;
                        questionCol = defaultCol - answerCol;

                        $.each(question.Answer.split('|'), function (index, answer) {
                            var IsChecked = "";
                            var EmployeeAnswerIndex = 99;

                            var AnswerIDArr = question.AnswerID.split('|');
                            if (AnswerIDArr[index] == question.EmployeeAnswerID) {
                                IsChecked = "checked";
                                EmployeeAnswerIndex = index;
                            }

                            questionsContainer.append(
                                '<div class="form-group form-fields">' +
                                '<div class="col-md-' + answerCol +'">' +
                                '</div>' +
                                '<div class="col-md-' + questionCol +'" style="display:inline-flex;">' +
                                '<input type="' + question.AnswerType + '" name="' + question.Code + '" disabled ' + IsChecked +'><label class="control-label">' + answer + ' </label>' +
                                '</div>' +
                                '</div>'
                            );

                            var AddReasonArr = question.AddReason.split('|');
                            var Answer = "";
                            if (AddReasonArr[index] == "INPUT") {
                                if (index == EmployeeAnswerIndex) {
                                    Answer = (question.EmployeeAnswerDetails == null ? "" : question.EmployeeAnswerDetails);
                                }
                                questionsContainer.append(
                                    '<div class="form-group form-fields">' +
                                    '<div class="col-md-' + answerCol + '">' +
                                    '</div>' +
                                    '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                                    '<input class="form-control" type="text" value="' + Answer +'" disabled>' +
                                    '</div>' +
                                    '</div>'
                                );
                            }
                        });
                    }

                });
            };
            objEMSCommonJS.GetAjax(GetQuestionByIDURL + "&EmployeeID=" + EmployeeID, {}, "", GetSuccessFunction, null, true);
        },

        ExportQuestionEmployeeAnswerFunction: function () {
            var parameter1 = "";

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {

                    window.location = GetQuestionEmployeeAnswersExportURL + parameter1;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckQuestionEmployeeAnswersExportURL + parameter1, {}, "#btnExportExitInterview", GetSuccessFunction, null, true);
        },
    };

    objMyAccountabilitiesListJS.Initialize();
});