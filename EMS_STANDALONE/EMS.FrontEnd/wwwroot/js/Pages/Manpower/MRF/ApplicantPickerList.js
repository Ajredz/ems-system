var objApplicantPickerListJS;

$(document).ready(function () {
    objApplicantPickerListJS = {

        Initialize: function () {
            $("#divMRFApplicantPickerBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            
            s.ElementBinding();
            var param = {
                IsTaggedToMRF: localStorage["ApplicantPickerListIsTaggedToMRF"] != undefined ? localStorage["ApplicantPickerListIDIsTaggedToMRF"] : false,
                ID: localStorage["ApplicantPickerListID"],
                LastName: localStorage["ApplicantPickerListApplicantLastName"],
                FirstName: localStorage["ApplicantPickerListApplicantFirstName"],
                MiddleName: localStorage["ApplicantPickerListApplicantMiddleName"],
                Suffix: localStorage["ApplicantPickerListApplicantSuffix"],
                ApplicationSourceDelimited: localStorage["ApplicantPickerListApplicantSourceDelimited"],
                MRFTransactionID: localStorage["ApplicantPickerListMRFTransactionID"],
                CurrentStepDelimited: localStorage["ApplicantPickerListCurrentStepDelimited"],
                DateScheduledFrom: localStorage["ApplicantPickerListDateScheduledFrom"],
                DateScheduledTo: localStorage["ApplicantPickerListDateScheduledTo"],
                DateCompletedFrom: localStorage["ApplicantPickerListDateCompletedFrom"],
                DateCompletedTo: localStorage["ApplicantPickerListDateCompletedTo"],
                ApproverRemarks: localStorage["ApplicantPickerListApproverRemarks"],
                PositionRemarks: localStorage["ApplicantPickerListPositionRemarks"],
                Course: localStorage["ApplicantPickerListCourseDelimited"],
                CurrentPositionTitle: localStorage["ApplicantPickerListCurrentPositionTitle"],
                ExpectedSalaryFrom: localStorage["ApplicantPickerListExpectedSalaryFrom"],
                ExpectedSalaryTo: localStorage["ApplicantPickerListExpectedSalaryTo"],
                DateAppliedFrom: localStorage["ApplicantPickerListDateAppliedFrom"],
                DateAppliedTo: localStorage["ApplicantPickerListDateAppliedTo"],
                ScopeOrgGroupDelimited: localStorage["ApplicantPickerListScopeOrgGroupDelimited"],
                SelectedIDDelimited: $("#hdnIDDelimited").val(),

            };
            s.LoadJQGrid(param);

            GenerateDropdownValues(GetWorkflowDropDownURL, "ddlWorkflow", "Value", "Text", "", "", false);

            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterDateAppliedFrom, #txtFilterDateAppliedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            NumberOnly($("#divMRFApplicantPickerModal #txtFilterApplicantID"));
            NumberOnly($("#divMRFApplicantPickerModal #txtFilterExpectedSalaryFrom"));
            NumberOnly($("#divMRFApplicantPickerModal #txtFilterExpectedSalaryTo"));

            $("#divMRFApplicantPickerModal #btnSearch").click(function () {
                var param = {
                    IsTaggedToMRF: $("#divMRFApplicantPickerModal #cbIsTaggedToMRF").is(":checked"),
                    ID: $("#divMRFApplicantPickerModal #txtFilterApplicantID").val(),
                    LastName: $("#divMRFApplicantPickerModal #txtFilterApplicantLastName").val(),
                    FirstName: $("#divMRFApplicantPickerModal #txtFilterApplicantFirstName").val(),
                    MiddleName: $("#divMRFApplicantPickerModal #txtFilterApplicantMiddleName").val(),
                    Suffix: $("#divMRFApplicantPickerModal #txtFilterApplicantSuffix").val(),
                    ApplicationSourceDelimited: objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedApplicationSource").value,
                    MRFTransactionID: $("#txtFilterMRFTransactionID").val(),
                    CurrentStepDelimited: objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCurrentStep").value,
                    DateScheduledFrom: $("#divMRFApplicantPickerModal #txtFilterDateScheduledFrom").val(),
                    DateScheduledTo: $("#divMRFApplicantPickerModal #txtFilterDateScheduledTo").val(),
                    DateCompletedFrom: $("#divMRFApplicantPickerModal #txtFilterDateCompletedFrom").val(),
                    DateCompletedTo: $("#divMRFApplicantPickerModal #txtFilterDateCompletedTo").val(),
                    ApproverRemarks: $("#divMRFApplicantPickerModal #txtFilterApproverRemarks").val(),
                    PositionRemarks: $("#divMRFApplicantPickerModal #txtFilterPositionRemarks").val(),
                    Course: objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCourse").value,
                    CurrentPositionTitle: $("#divMRFApplicantPickerModal #txtFilterCurrentPositionTitle").val(),
                    ExpectedSalaryFrom: $("#divMRFApplicantPickerModal #txtFilterExpectedSalaryFrom").val(),
                    ExpectedSalaryTo: $("#divMRFApplicantPickerModal #txtFilterExpectedSalaryTo").val(),
                    DateAppliedFrom: $("#txtFilterDateAppliedFrom").val(),
                    DateAppliedTo: $("#txtFilterDateAppliedTo").val(),
                    SelectedIDDelimited: $("#hdnIDDelimited").val(),
                    ScopeOrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").value,
                };
                s.SetLocalStorage();
                ResetJQGridState("tblMRFApplicantPickerList");
                s.LoadJQGrid(param);
            });

            $("#cbIsTaggedToMRF").change(function () {
                $("#divMRFApplicantPickerModal #btnSearch").click();
            });

            $("#divMRFApplicantPickerModal .close").click(function () {
                $("#divMRFAddApplicantModal").modal("show");
                $("#divMRFApplicantPickerModal").modal("hide");
            });

            $("#divMRFApplicantPickerModal #btnReset").click(function () {
                $("#divMRFApplicantPickerModal div.filterFields input[type='search']").val("");
                $("#divMRFApplicantPickerModal div.filterFields select").val("");
                $("#divMRFApplicantPickerModal div.filterFields input[type='checkbox']").prop("checked", true);
                $("#divMRFApplicantPickerModal #multiSelectedApplicationSource").html("");
                $("#divMRFApplicantPickerModal #multiSelectedApplicationSourceOption label, #divMRFApplicantPickerModal #multiSelectedApplicationSourceOption input").prop("title", "add");
                $("#multiSelectedCurrentStep").html("");
                $("#multiSelectedScopeOrgGroup").html("");
                $("#divMRFApplicantPickerModal #multiSelectedCourse").html("");
                $("#divMRFApplicantPickerModal #multiSelectedCourseOption label, #divMRFApplicantPickerModal #multiSelectedCourseOption input").prop("title", "add");
                //$("#divMRFApplicantPickerModal #multiSelectedCurrentStep").html("");
                //$("#divMRFApplicantPickerModal #multiSelectedWorkflow").html("");
                //$("#divMRFApplicantPickerModal #multiSelectedDesiredOrgGroup").html("");
                //$("#divMRFApplicantPickerModal #multiSelectedDesiredPosition").html("");
                $("#divMRFApplicantPickerModal #btnSearch").click();
            });

            $("#divMRFApplicantPickerModal #btnPick").click(function () {

                $(".errMessage").removeClass("errMessage");
                $("#divApplicantPickerErrorMessage").hide();
                $("#divApplicantPickerErrorMessage").html("");

                $('#ddlWorkflow option').filter(function () {
                    return ($(this).text() == $("#hdnWorkflowCode").val());
                }).prop('selected', true);

                if ($("#ddlWorkflow").val() == "")
                {
                    $("#ddlWorkflow").addClass("errMessage");
                    $("#divApplicantPickerErrorMessage").show();
                    $("#divApplicantPickerErrorMessage").html("<label class=\"errMessage\"><li>Workflow" + SUFF_REQUIRED + ".</li></label><br />");
                }
                else if ($("#tblMRFApplicantPickerList").jqGrid("getGridParam", "selarrrow").length == 0) {
                    $("#divApplicantPickerErrorMessage").show();
                    $("#divApplicantPickerErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "applicant.</li></label><br />");
                }
                else {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED, 'objApplicantPickerListJS.SaveSelectedApplicants()', 'function');
                }

            });

            objEMSCommonJS.BindFilterMultiSelectEnum("divMRFApplicantPickerModal #multiSelectedApplicationSource", RecruitmentGetApplicationSourceURL);
            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("divMRFApplicantPickerModal #multiSelectedCourse", RecruitmentGetCourseURL, "Value", "Description");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFApplicantPickerModal #txtFilterCurrentStep"
                , RecruitmentCurrentStepAutoCompleteURL, 20, "multiSelectedCurrentStep");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterScopeOrgGroup"
                , RegionAutoCompleteURL, 20, "multiSelectedScopeOrgGroup");

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFApplicantPickerModal #txtFilterCurrentStep"
            //    , RecruitmentCurrentStepAutoCompleteURL, 20, "divMRFApplicantPickerModal #multiSelectedCurrentStep");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFApplicantPickerModal #txtFilterWorkflow"
            //    , RecruitmentWorkflowAutoCompleteURL, 20, "divMRFApplicantPickerModal #multiSelectedWorkflow");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFApplicantPickerModal #txtFilterDesiredOrgGroup"
            //    , RecruitmentOrgGroupAutoCompleteURL, 20, "divMRFApplicantPickerModal #multiSelectedDesiredOrgGroup");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFApplicantPickerModal #txtFilterDesiredPosition"
            //    , RecruitmentPositionAutoCompleteURL, 20, "divMRFApplicantPickerModal #multiSelectedDesiredPosition");

        },

        SaveSelectedApplicants: function () {
            if ($("#tblMRFApplicantPickerList").jqGrid("getGridParam", "selarrrow").length > 0) {
                objEMSCommonJS.PostAjax(true 
                    , ApplicantPickerPostURL 
                    , objApplicantPickerListJS.GetFormData() 
                    , '#divBatchTaskErrorMessage' 
                    , '#divBatchTaskModal #btnSave' 
                    , objApplicantPickerListJS.SaveSelectedSuccessFunction);
            }
        },

        GetFormData: function () {
            var input = $("#tblMRFApplicantPickerList").jqGrid("getGridParam", "selarrrow");
            var formData = new FormData($('#frmMRFAddApplicant').get(0));

            formData.append("Form.WorkflowID", $("#ddlWorkflow").val());
            formData.append("Form.MRFTransactionID", $("#MRFTransactionID").val());
            for (var x = 0; x < input.length; x++) {
                formData.append("Form.Applicants[" + x + "].ApplicantID", input[x]);
            }
            return formData;
        },


        SaveSelectedSuccessFunction: function () {
            LoadPartial(MRFAddApplicantModalURL + '?ID=' + $("#divMRFAddApplicantModal #hdnID").val() + '', 'divMRFAddApplicantBodyModal');
            $("#divMRFAddApplicantModal").modal("show");
            $("#divMRFApplicantPickerModal").modal("hide");
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblMRFApplicantPickerList") == "" ? "" : $.parseJSON(localStorage.getItem("tblMRFApplicantPickerList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divMRFApplicantPickerModal .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divMRFApplicantPickerModal .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divMRFApplicantPickerModal #filterFieldsContainer");
                });

                $("#divMRFApplicantPickerModal .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divMRFApplicantPickerModal div.filterFields").unbind("keyup");
                $("#divMRFApplicantPickerModal div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divMRFApplicantPickerModal #btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#tblMRFApplicantPickerList").jqGrid("GridUnload");
            $("#tblMRFApplicantPickerList").jqGrid("GridDestroy");
            $("#tblMRFApplicantPickerList").jqGrid({
                url: RecruitmentApplicantPickerListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Filter By Region", "Last Name", "First Name", "Middle Name", "Suffix", "Date Applied", "Application Source", /*"Current Step",*/
                    "MRF ID", "Application Status",
                    "Date Scheduled", "Date Completed", "Approver Remarks",
                    "Desired Position (Remarks)", "Course", "Current Position Title", "Expected Salary"
                ],
                colModel: [
                    { hidden: true },
                    { width: 25, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objApplicantPickerListJS.AddLink },
                    { name: "ScopeOrgGroup", index: "ScopeOrgGroup", editable: true, align: "left", sortable: false },
                    { name: "LastName", index: "LastName", align: "left", sortable: true },
                    { name: "FirstName", index: "FirstName", align: "left", sortable: true },
                    { name: "MiddleName", index: "MiddleName", align: "left", sortable: true },
                    { name: "Suffix", index: "Suffix", align: "left", sortable: true },
                    { name: "DateApplied", index: "DateApplied", align: "left", sortable: true },
                    { name: "ApplicationSource", index: "ApplicationSource", align: "left", sortable: true },
                    { name: "MRFTransactionID", index: "MRFTransactionID", align: "left", sortable: true },
                    { name: "CurrentStep", index: "CurrentStep", align: "left", sortable: true },
                    { name: "DateScheduled", index: "DateScheduled", align: "left", sortable: true },
                    { name: "DateCompleted", index: "DateCompleted", align: "left", sortable: true },
                    { name: "ApproverRemarks", index: "ApproverRemarks", align: "left", sortable: true },
                    //{ name: "CurrentStep", index: "CurrentStep", align: "left", sortable: true },
                    //{ name: "WorkflowDescription", index: "WorkflowDescription", align: "left", sortable: true },
                    //{ name: "OrgGroupRemarks", index: "OrgGroupRemarks", align: "left", sortable: true },
                    //{ name: "DesiredOrgGroup", index: "DesiredOrgGroup", align: "left", sortable: true },
                    { name: "PositionRemarks", index: "PositionRemarks", align: "left", sortable: true },
                    //{ name: "DesiredPosition", index: "DesiredPosition", align: "left", sortable: true },
                    { name: "Course", index: "Course", align: "left", sortable: true },
                    { name: "CurrentPositionTitle", index: "CurrentPositionTitle", align: "left", sortable: true },
                    { name: "ExpectedSalary", index: "ExpectedSalary", align: "right", sortable: true, formatter: objApplicantPickerListJS.FormatAmount },
                ],
                toppager: $("#tblMRFApplicantPickerPager"),
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
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divMRFApplicantPickerModal #divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        var hasDisabled = 0;
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                                if ($.inArray((data.rows[i].ID).toString(), $("#hdnIDDelimited").val().split(',')) >= 0) {
                                    $("#jqg_tblMRFApplicantPickerList_" + data.rows[i].ID).prop("disabled", true);
                                    hasDisabled++;
                                }
                            }
                            if (hasDisabled > 0) {
                                $("#divApplicantPickerInformationMessage").show();
                                $("#divApplicantPickerInformationMessage").text(ALREADY_SELECTED_ITEM);
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblMRFApplicantPickerList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divMRFApplicantPickerModal #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divMRFApplicantPickerModal .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblMRFApplicantPickerList .jqgrid-id-link").click(function () {
                          
                        });

                    }

                    if (localStorage["ApplicantPickerListFilterOption"] != undefined) {
                        $("#chkApplicantPickerFilter").prop('checked', JSON.parse(localStorage["ApplicantPickerListFilterOption"]));
                    }
                    objApplicantPickerListJS.ShowHideFilter();

                    $("#chkApplicantPickerFilter").on('change', function () {
                        objApplicantPickerListJS.ShowHideFilter();
                        localStorage["ApplicantPickerListFilterOption"] = $("#chkApplicantPickerFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divMRFApplicantPickerModal .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#divMRFApplicantPickerModal table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#divMRFApplicantPickerModal table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#divMRFApplicantPickerModal table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divMRFApplicantPickerModal table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#divMRFApplicantPickerModal table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divMRFApplicantPickerModal table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#divMRFApplicantPickerModal table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divMRFApplicantPickerModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                    });
                },
                onSelectRow: function (ids) {
                    $("#divApplicantPickerErrorMessage").html("");
                    $("#divApplicantPickerErrorMessage").hide();
                    $(".errMessage").removeClass("errMessage");
                },
                onSelectAll: function (aRowids, status) {
                    if (status) {
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
                        return true;    // allow select the row
                    } else {
                        return false;   // not allow select the row
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblMRFApplicantPickerList");
                    moveFilterFields();
                },
            }).navGrid("#tblMRFApplicantPickerPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery("#divMRFApplicantPickerModal .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery("#divMRFApplicantPickerModal .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblApplicantPickerFilter\">Show Filter</label>");
            jQuery("#divMRFApplicantPickerModal #lblApplicantPickerFilter").after("<input type=\"checkbox\" id=\"chkApplicantPickerFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage: function () {
            
            localStorage["ApplicantPickerListIsTaggedToMRF"] = $("#divMRFApplicantPickerModal #txtcbIsTaggedToMRFFilterApplicantID").is(":checked");
            localStorage["ApplicantPickerListID"] = $("#divMRFApplicantPickerModal #txtFilterApplicantID").val();
            localStorage["ApplicantPickerListApplicantLastName"] = $("#divMRFApplicantPickerModal #txtFilterApplicantLastName").val();
            localStorage["ApplicantPickerListApplicantFirstName"] = $("#divMRFApplicantPickerModal #txtFilterApplicantFirstName").val();
            localStorage["ApplicantPickerListApplicantMiddleName"] = $("#divMRFApplicantPickerModal #txtFilterApplicantMiddleName").val();
            localStorage["ApplicantPickerListApplicantSuffix"] = $("#divMRFApplicantPickerModal #txtFilterApplicantSuffix").val();

            localStorage["ApplicantPickerListApplicationSourceDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedApplicationSource").value;
            localStorage["ApplicantPickerListApplicationSourceDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedApplicationSource").text;

            localStorage["ApplicantPickerListMRFTransactionID"] = $("#divMRFApplicantPickerModal #txtFilterMRFTransactionID").val();

            localStorage["ApplicantPickerListCurrentStepDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCurrentStep").value;
            localStorage["ApplicantPickerListCurrentStepDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCurrentStep").text;

            localStorage["ApplicantPickerListScheduledFrom"] = $("#divMRFApplicantPickerModal #txtFilterScheduledFrom").val();
            localStorage["ApplicantPickerListScheduledTo"] = $("#divMRFApplicantPickerModal #txtFilterScheduledTo").val();
            localStorage["ApplicantPickerListCompletedFrom"] = $("#divMRFApplicantPickerModal #txtFilterCompletedFrom").val();
            localStorage["ApplicantPickerListCompletedTo"] = $("#divMRFApplicantPickerModal #txtFilterCompletedTo").val();
            localStorage["ApplicantPickerListApproverRemarks"] = $("#divMRFApplicantPickerModal #txtFilterApproverRemarks").val();


            localStorage["ApplicantPickerListCourseDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCourse").value;
            localStorage["ApplicantPickerListCourseDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCourse").text;


            localStorage["ApplicantPickerListPositionRemarks"] = $("#divMRFApplicantPickerModal #txtFilterPositionRemarks").val();

            localStorage["ApplicantPickerListCurrentPositionTitle"] = $("#divMRFApplicantPickerModal #txtFilterCurrentPositionTitle").val();
            localStorage["ApplicantPickerListExpectedSalaryFrom"] = $("#divMRFApplicantPickerModal #txtFilterSalaryFrom").val();
            localStorage["ApplicantPickerListExpectedSalaryTo"] = $("#divMRFApplicantPickerModal #txtFilterSalaryTo").val();
            localStorage["ApplicantPickerListDateAppliedFrom"] = $("#txtFilterDateAppliedFrom").val();
            localStorage["ApplicantPickerListDateAppliedTo"] = $("#txtFilterDateAppliedTo").val();

            localStorage["ApplicantPickerListScopeOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").value;
            localStorage["ApplicantPickerListScopeOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").text;
        },

        GetLocalStorage: function () {
            $("#divMRFApplicantPickerModal #cbIsTaggedToMRF").val(
                localStorage["ApplicantPickerListIsTaggedToMRF"] != undefined ? localStorage["ApplicantPickerListIDIsTaggedToMRF"] : false
            );

            objEMSCommonJS.SetMultiSelectList("divMRFApplicantPickerModal #multiSelectedOrgGroup"
                , "RecruitmentApplicantPickerListOrgGroupDelimited"
                , "RecruitmentApplicantPickerListOrgGroupDelimitedText");

            $("#divMRFApplicantPickerModal #txtFilterApplicantID").val(localStorage["ApplicantPickerListID"]);
            $("#divMRFApplicantPickerModal #txtFilterApplicantLastName").val(localStorage["ApplicantPickerListApplicantLastName"]);
            $("#divMRFApplicantPickerModal #txtFilterApplicantFirstName").val(localStorage["ApplicantPickerListApplicantFirstName"]);
            $("#divMRFApplicantPickerModal #txtFilterApplicantMiddleName").val(localStorage["ApplicantPickerListApplicantMiddleName"]);
            $("#divMRFApplicantPickerModal #txtFilterApplicantSuffix").val(localStorage["ApplicantPickerListApplicantSuffix"]);


            objEMSCommonJS.SetMultiSelectList("divMRFApplicantPickerModal #multiSelectedApplicationSource"
                , "ApplicantPickerListApplicationSourceDelimited"
                , "ApplicantPickerListApplicationSourceDelimitedText");

            $("#divMRFApplicantPickerModal #txtFilterMRFTransactionID").val(localStorage["ApplicantPickerListMRFTransactionID"]);

            objEMSCommonJS.SetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCurrentStep"
                , "ApplicantPickerListCurrentStepDelimited"
                , "ApplicantPickerListCurrentStepDelimitedText");

            $("#divMRFApplicantPickerModal #txtFilterScheduledFrom").val(localStorage["ApplicantPickerListScheduledFrom"]);
            $("#divMRFApplicantPickerModal #txtFilterScheduledTo").val(localStorage["ApplicantPickerListScheduledTo"]);
            $("#divMRFApplicantPickerModal #txtFilterCompletedFrom").val(localStorage["ApplicantPickerListCompletedFrom"]);
            $("#divMRFApplicantPickerModal #txtFilterCompletedTo").val(localStorage["ApplicantPickerListCompletedTo"]);
            $("#divMRFApplicantPickerModal #txtFilterApproverRemarks").val(localStorage["ApplicantPickerListApproverRemarks"]);



            objEMSCommonJS.SetMultiSelectList("multiSelectedScopeOrgGroup"
                , "ApplicantPickerListScopeOrgGroupDelimited"
                , "ApplicantPickerListScopeOrgGroupDelimitedText");

            //objEMSCommonJS.SetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCurrentStep"
            //    , "ApplicantPickerListCurrentStepDelimited"
            //    , "ApplicantPickerListCurrentStepDelimitedText");

            //objEMSCommonJS.SetMultiSelectList("divMRFApplicantPickerModal #multiSelectedWorkflow"
            //    , "ApplicantPickerListWorkflowDelimited"
            //    , "ApplicantPickerListWorkflowDelimitedText");

            //$("#divMRFApplicantPickerModal #txtFilterOrgGroupRemarks").val(localStorage["ApplicantPickerListOrgGroupRemarks"]);
            //objEMSCommonJS.SetMultiSelectList("divMRFApplicantPickerModal #multiSelectedDesiredOrgGroup"
            //    , "ApplicantPickerListDesiredOrgGroupDelimited"
            //    , "ApplicantPickerListDesiredOrgGroupDelimitedText");

            $("#divMRFApplicantPickerModal #txtFilterPositionRemarks").val(localStorage["ApplicantPickerListPositionRemarks"]);
            //objEMSCommonJS.SetMultiSelectList("divMRFApplicantPickerModal #multiSelectedDesiredPosition"
            //    , "ApplicantPickerListDesiredPositionDelimited"
            //    , "ApplicantPickerListDesiredPositionDelimitedText");

            //$("#divMRFApplicantPickerModal #txtFilterCourse").val(localStorage["ApplicantPickerListCourse"]);

            objEMSCommonJS.SetMultiSelectList("divMRFApplicantPickerModal #multiSelectedCourse"
                , "ApplicantPickerListCourseDelimited"
                , "ApplicantPickerListCourseDelimitedText");


            $("#divMRFApplicantPickerModal #txtFilterCurrentPositionTitle").val(localStorage["ApplicantPickerListCurrentPositionTitle"]);
            $("#divMRFApplicantPickerModal #txtFilterSalaryFrom").val(localStorage["ApplicantPickerListExpectedSalaryFrom"]);
            $("#divMRFApplicantPickerModal #txtFilterSalaryTo").val(localStorage["ApplicantPickerListExpectedSalaryTo"]);
            $("#txtFilterDateAppliedFrom").val(localStorage["ApplicantPickerListDateAppliedFrom"]);
            $("#txtFilterDateAppliedTo").val(localStorage["ApplicantPickerListDateAppliedTo"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return objApplicantPickerListJS.ViewApplicantModal(" + rowObject.ID + ", '" + RecruitmentApplicantViewURL + "?ID=" + rowObject.ID + "');\">" + rowObject.ID + "</a>";
        },

        ViewApplicantModal: function (ApplicantID, url) {

            $('#divMRFApplicantPickerModal').modal('hide');
            $('#divApplicantModal').modal('show');
            $("#hdnMRFApplicantID").val(ApplicantID);

            var successFunction = function () {
                $("#divApplicantModal #btnDelete, #divApplicantModal #btnEdit").remove();
                $("#divApplicantModal .close").click(function () {
                    objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApplicants', "#divMRFAddApplicantModal");
                    $('#divMRFApplicantPickerModal').modal('show');
                    $('#divApplicantModal').modal('hide');
                });

                GenerateDropdownValues(MRFIDDropdownURL + "&ApplicantID=" + $("#hdnMRFApplicantID").val(), "ddlMRF", "Value", "Text", "", "", false);
            };

            LoadPartialSuccessFunction(url, 'divApplicantBodyModal', successFunction);
            return false;
        },

        FormatAmount: function (cellvalue, options, rowObject) {
            return AddZeroes(rowObject.ExpectedSalary+"").withComma();
        },

        ShowHideFilter: function () {
            if ($("#chkApplicantPickerFilter").is(":checked")) {
                $("#divMRFApplicantPickerModal .jqgfirstrow .filterFields").show();
            }
            else if ($("#chkApplicantPickerFilter").is(":not(:checked)")) {
                $("#divMRFApplicantPickerModal .jqgfirstrow .filterFields").hide();
            }
        }
    };

    objApplicantPickerListJS.Initialize();
});