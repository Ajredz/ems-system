var objApplicationApprovalListJS;
const ApplicationApprovalListURL = "/Recruitment/ApplicationApproval?handler=List";
const OrgGroupAutoCompleteURL = "/Recruitment/ApplicationApproval?handler=OrgTypeAutoComplete";
const PositionAutoCompleteURL = "/Recruitment/ApplicationApproval?handler=PositionAutoComplete";
const GetApplicationSourceURL = "/Recruitment/ApplicationApproval?handler=ReferenceValue&RefCode=APPLICATION_SOURCE";
//const CurrentStepAutoCompleteURL = "/Recruitment/ApplicationApproval?handler=CurrentStepAutoComplete";
//const WorkflowAutoCompleteURL = "/Recruitment/ApplicationApproval?handler=WorkflowAutoComplete";
const ReferredByAutoComplete = "/Recruitment/ApplicationApproval?handler=ReferredBy";
const ApplicantViewURL = "/Recruitment/ApplicationApproval/View";
const CheckFileIfExistsURL = "/Recruitment/ApplicationApproval?handler=CheckFileIfExists";
const UpdateStatusURL = "/Recruitment/ApplicationApproval/UpdateStatus";


$(document).ready(function () {
    objApplicationApprovalListJS = {

        Initialize: function () {
            var s = this;
            
            s.ElementBinding();
            var param = {
                ID: localStorage["ApplicationApprovalListID"],
                ApplicantName: localStorage["ApplicationApprovalListApplicantName"],
                ApplicantSourceDelimited: localStorage["ApplicationApprovalListApplicantSourceDelimited"],
                //CurrentStepDelimited: localStorage["ApplicationApprovalListCurrentStepDelimited"],
                //WorkflowDelimited: localStorage["ApplicationApprovalListWorkflowDelimited"],
                //OrgGroupRemarks: localStorage["ApplicationApprovalListOrgGroupRemarks"],
                //OrgGroupDelimited: localStorage["ApplicationApprovalListOrgGroupDelimited"],
                PositionRemarks: localStorage["ApplicationApprovalListPositionRemarks"],
                //PositionDelimited: localStorage["ApplicationApprovalListPositionDelimited"],
                Course: localStorage["ApplicationApprovalListCourse"],
                CurrentPositionTitle: localStorage["ApplicationApprovalListCurrentPositionTitle"],
                ExpectedSalaryFrom: localStorage["ApplicationApprovalListExpectedSalaryFrom"],
                ExpectedSalaryTo: localStorage["ApplicationApprovalListExpectedSalaryTo"],
                DateAppliedFrom: localStorage["ApplicationApprovalListDateAppliedFrom"],
                DateAppliedTo: localStorage["ApplicationApprovalListDateAppliedTo"],

            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterDateAppliedFrom, #txtFilterDateAppliedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            NumberOnly($("#txtFilterApplicantID"));
            NumberOnly($("#txtFilterExpectedSalaryFrom"));
            NumberOnly($("#txtFilterExpectedSalaryTo"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterApplicantID").val(),
                    ApplicantName: $("#txtFilterApplicantName").val(),
                    ApplicationSourceDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").value,
                    //CurrentStepDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value,
                    //WorkflowDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflow").value,
                    //OrgGroupRemarks: $("#txtFilterOrgGroupRemarks").val(),
                    //OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").value,
                    PositionRemarks: $("#txtFilterPositionRemarks").val(),
                    //PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").value,
                    Course: $("#txtFilterCourse").val(),
                    CurrentPositionTitle: $("#txtFilterCurrentPositionTitle").val(),
                    ExpectedSalaryFrom: $("#txtFilterExpectedSalaryFrom").val(),
                    ExpectedSalaryTo: $("#txtFilterExpectedSalaryTo").val(),
                    DateAppliedFrom: $("#txtFilterDateAppliedFrom").val(),
                    DateAppliedTo: $("#txtFilterDateAppliedTo").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblRecruitmentApplicationApprovalList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedApplicationSource").html("");
                //$("#multiSelectedCurrentStep").html("");
                //$("#multiSelectedWorkflow").html("");
                //$("#multiSelectedDesiredOrgGroup").html("");
                //$("#multiSelectedDesiredPosition").html("");
                $("#btnSearch").click();
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedApplicationSource", GetApplicationSourceURL);

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterCurrentStep"
            //    , CurrentStepAutoCompleteURL, 20, "multiSelectedCurrentStep");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterWorkflow"
            //    , WorkflowAutoCompleteURL, 20, "multiSelectedWorkflow");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterDesiredOrgGroup"
            //    , OrgGroupAutoCompleteURL, 20, "multiSelectedDesiredOrgGroup");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterDesiredPosition"
            //    , PositionAutoCompleteURL, 20, "multiSelectedDesiredPosition");

        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblRecruitmentApplicationApprovalList") == "" ? "" : $.parseJSON(localStorage.getItem("tblRecruitmentApplicationApprovalList"));

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
            $("#tblRecruitmentApplicationApprovalList").jqGrid("GridUnload");
            $("#tblRecruitmentApplicationApprovalList").jqGrid("GridDestroy");
            $("#tblRecruitmentApplicationApprovalList").jqGrid({
                url: ApplicationApprovalListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Application", "Applicant Name", "Date Applied", "Application Source", /*"Current Step",*/
                    "Desired Position (Remarks)", "Course", "Current Position Title", "Expected Salary", "", ""
                ],
                colModel: [
                    { hidden: true },
                    { width: 25, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objApplicationApprovalListJS.AddLink },
                    { width: 35, name: "Application", sortable: false, formatter: objApplicationApprovalListJS.UpdateStatusLink, align: "center" },
                    { name: "ApplicantName", index: "ApplicantName", align: "left", sortable: true },
                    { name: "DateApplied", index: "DateApplied", align: "left", sortable: true },
                    { name: "ApplicationSource", index: "ApplicationSource", align: "left", sortable: true },
                    //{ name: "CurrentStep", index: "CurrentStep", align: "left", sortable: true },
                    //{ name: "OrgGroupRemarks", index: "OrgGroupRemarks", align: "left", sortable: true },
                    //{ name: "DesiredOrgGroup", index: "DesiredOrgGroup", align: "left", sortable: true },
                    { name: "PositionRemarks", index: "PositionRemarks", align: "left", sortable: true },
                    //{ name: "DesiredPosition", index: "DesiredPosition", align: "left", sortable: true },
                    { name: "Course", index: "Course", align: "left", sortable: true },
                    { name: "CurrentPositionTitle", index: "CurrentPositionTitle", align: "left", sortable: true },
                    { name: "ExpectedSalary", index: "ExpectedSalary", align: "right", sortable: true, formatter: objApplicationApprovalListJS.FormatAmount },
                    { name: "HasApproval", index: "HasApproval", align: "left", sortable: false, hidden: true },
                    { name: "WorkflowID", index: "WorkflowID", align: "left", sortable: false, hidden: true  },
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
                    $("#tblRecruitmentApplicationApprovalList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblRecruitmentApplicationApprovalList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblRecruitmentApplicationApprovalList .jqgrid-id-link").click(function () {
                            $('#divApplicationApprovalModal').modal('show');
                        });
                    }

                    if (localStorage["ApplicationApprovalListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["ApplicationApprovalListFilterOption"]));
                    }
                    objApplicationApprovalListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objApplicationApprovalListJS.ShowHideFilter();
                        localStorage["ApplicationApprovalListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblRecruitmentApplicationApprovalList");
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
            localStorage["ApplicationApprovalListID"] = $("#txtFilterApplicantID").val();
            localStorage["ApplicationApprovalListApplicantName"] = $("#txtFilterApplicantName").val();
            localStorage["ApplicationApprovalListApplicationSourceDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").value;
            localStorage["ApplicationApprovalListApplicationSourceDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").text;

            //localStorage["ApplicationApprovalListCurrentStepDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value;
            //localStorage["ApplicationApprovalListCurrentStepDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").text;

            //localStorage["ApplicationApprovalListWorkflowDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflow").value;
            //localStorage["ApplicationApprovalListWorkflowDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflow").text;

            //localStorage["ApplicationApprovalListOrgGroupRemarks"] = $("#txtFilterOrgGroupRemarks").val();
            //localStorage["ApplicationApprovalListDesiredOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").value;
            //localStorage["ApplicationApprovalListDesiredOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").text;

            localStorage["ApplicationApprovalListPositionRemarks"] = $("#txtFilterPositionRemarks").val();
            //localStorage["ApplicationApprovalListDesiredPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").value;
            //localStorage["ApplicationApprovalListDesiredPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").text;
            
            localStorage["ApplicationApprovalListCourse"] = $("#txtFilterCourse").val();
            localStorage["ApplicationApprovalListCurrentPositionTitle"] = $("#txtFilterCurrentPositionTitle").val();
            localStorage["ApplicationApprovalListExpectedSalaryFrom"] = $("#txtFilterSalaryFrom").val();
            localStorage["ApplicationApprovalListExpectedSalaryTo"] = $("#txtFilterSalaryTo").val();
            localStorage["ApplicationApprovalListDateAppliedFrom"] = $("#txtFilterDateAppliedFrom").val();
            localStorage["ApplicationApprovalListDateAppliedTo"] = $("#txtFilterDateAppliedTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterApplicantID").val(localStorage["ApplicationApprovalListID"]);
            $("#txtFilterApplicantName").val(localStorage["ApplicationApprovalListApplicantName"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedApplicationSource"
                , "ApplicationApprovalListApplicationSourceDelimited"
                , "ApplicationApprovalListApplicationSourceDelimitedText");

            //objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStep"
            //    , "ApplicationApprovalListCurrentStepDelimited"
            //    , "ApplicationApprovalListCurrentStepDelimitedText");

            //objEMSCommonJS.SetMultiSelectList("multiSelectedWorkflow"
            //    , "ApplicationApprovalListWorkflowDelimited"
            //    , "ApplicationApprovalListWorkflowDelimitedText");

            //$("#txtFilterOrgGroupRemarks").val(localStorage["ApplicationApprovalListOrgGroupRemarks"]);
            //objEMSCommonJS.SetMultiSelectList("multiSelectedDesiredOrgGroup"
            //    , "ApplicationApprovalListDesiredOrgGroupDelimited"
            //    , "ApplicationApprovalListDesiredOrgGroupDelimitedText");

            $("#txtFilterPositionRemarks").val(localStorage["ApplicationApprovalListPositionRemarks"]);
            //objEMSCommonJS.SetMultiSelectList("multiSelectedDesiredPosition"
            //    , "ApplicationApprovalListDesiredPositionDelimited"
            //    , "ApplicationApprovalListDesiredPositionDelimitedText");

            $("#txtFilterCourse").val(localStorage["ApplicationApprovalListCourse"]);
            $("#txtFilterCurrentPositionTitle").val(localStorage["ApplicationApprovalListCurrentPositionTitle"]);
            $("#txtFilterSalaryFrom").val(localStorage["ApplicationApprovalListExpectedSalaryFrom"]);
            $("#txtFilterSalaryTo").val(localStorage["ApplicationApprovalListExpectedSalaryTo"]);
            $("#txtFilterDateAppliedFrom").val(localStorage["ApplicationApprovalListDateAppliedFrom"]);
            $("#txtFilterDateAppliedTo").val(localStorage["ApplicationApprovalListDateAppliedTo"]);
        },

        RemoveDynamicFields: function (id, deleteAttachmentFunction) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
            if (deleteAttachmentFunction != null)
                deleteAttachmentFunction();
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + ApplicantViewURL + "?ID=" + rowObject.ID + "', 'divApplicationApprovalBodyModal');\">" + rowObject.ID + "</a>";
        },

        FormatAmount: function (cellvalue, options, rowObject) {
            return AddZeroes(rowObject.ExpectedSalary+"").withComma();
        },

        UpdateStatusLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-update-link' onclick=\"return objApplicationApprovalListJS.OpenApplicationHistoryModal('"
                + UpdateStatusURL + "?ID=" + rowObject.ID + "&WorkflowID=" + rowObject.WorkflowID + "&HasApproval=" + rowObject.HasApproval + "', " + rowObject.ID+");\">" + (rowObject.HasApproval ? "Update" : "View") + "</a>";
        },

        OpenApplicationHistoryModal: function (url, ID) {
            $('#divUpdateStatusModal').modal('show');
            var successFunction = function () {
                $("#txtApplicantName").val($('#tblRecruitmentApplicationApprovalList').jqGrid('getCell', ID, 'ApplicantName'));
                $("#hdnID").val(ID);
            };
            LoadPartialSuccessFunction(url, 'divUpdateStatusBodyModal', successFunction);
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

    objApplicationApprovalListJS.Initialize();
});