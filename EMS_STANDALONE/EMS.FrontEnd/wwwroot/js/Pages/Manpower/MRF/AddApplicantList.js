var objMRFAddApplicantListJS;
var CompanyDropDown = [];

$(document).ready(function () {
    objMRFAddApplicantListJS = {

        Initialize: function () {
            $("#divMRFAddApplicantBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divWorkflowTransactionBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divCloseInternalModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            workflowStepDropdownOptions = [];
            s.ElementBinding();
            var param = {
                MRFID: $("#divMRFAddApplicantModal #hdnID").val(),
                IDDelimited: $("#hdnIDDelimited").val(),
                ID: localStorage["AddApplicantListID"],
                ForHiringID: localStorage["AddApplicantListForHiringID"],
                ApplicantName: localStorage["AddApplicantListApplicantName"],
                CurrentStepDelimited: localStorage["AddApplicantListCurrentStepDelimited"],
                //WorkflowDelimited: localStorage["AddApplicantListWorkflowDelimited"],
                StatusDelimited: localStorage["AddApplicantListStatusDelimited"],
                DateScheduledFrom: localStorage["AddApplicantListDateScheduledFrom"],
                DateScheduledTo: localStorage["AddApplicantListDateScheduledTo"],
                DateCompletedFrom: localStorage["AddApplicantListDateCompletedFrom"],
                DateCompletedTo: localStorage["AddApplicantListDateCompletedTo"],
                ApproverRemarks: localStorage["AddApplicantListApproverRemarks"],
            };

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApplicants', "#divMRFAddApplicantModal");
            objMRFAddApplicantListJS.GetApplicantComment();

            $("#dpDateScheduled, #dpDateCompleted").datetimepicker({
                useCurrent: true,
                format: 'MM/DD/YYYY',
            });

            $("#dpDateScheduled, #dpDateCompleted").datetimepicker().on('dp.show', function () {
                $('#divWorkflowTransactionModal .modal-body').css({ 'overflow': 'visible' });
                $('#divWorkflowTransactionModal.modal').css({ 'overflow': 'visible' });
            }).on('dp.hide', function () {
                $('#divWorkflowTransactionModal .modal-body').css({ 'overflow': 'auto' });
                $('#divWorkflowTransactionModal.modal').css({ 'overflow': 'auto' });
            });

            $("#txtFilterDateScheduledFrom, #txtFilterDateScheduledTo, \
                #txtFilterDateCompletedFrom, #txtFilterDateCompletedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY',
            });

            $("#hdnIsHiredApplicantID").val("");
            s.GetCompanyDropDown();
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;
            NumberOnly($("#divMRFAddApplicantModal #txtFilterApplicantID"));
            NumberOnly($("#divMRFAddApplicantModal #txtFilterExpectedSalaryFrom"));
            NumberOnly($("#divMRFAddApplicantModal #txtFilterExpectedSalaryTo"));

            $("#divMRFAddApplicantModal #btnSearch").click(function () {
                var param = {
                    MRFID: $("#divMRFAddApplicantModal #hdnID").val(),
                    IDDelimited: $("#hdnIDDelimited").val(),
                    ID: $("#divMRFAddApplicantModal #txtFilterApplicantID").val(),
                    ForHiringID: $("#hdnIDForHiring").val(),
                    ApplicantName: $("#divMRFAddApplicantModal #txtFilterApplicantName").val(),
                    CurrentStepDelimited: objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedCurrentStep").value,
                    //WorkflowDelimited: objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedWorkflow").value,
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedStatus").value,
                    DateScheduledFrom: $("#txtFilterDateScheduledFrom").val(),
                    DateScheduledTo: $("#txtFilterDateScheduledTo").val(),
                    DateCompletedFrom: $("#txtFilterDateCompletedFrom").val(),
                    DateCompletedTo: $("#txtFilterDateCompletedTo").val(),
                    ApproverRemarks: $("#txtFilterApproverRemarks").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblManpowerAddApplicantList");
                s.LoadJQGrid(param);
            });

            $("#divMRFAddApplicantModal #btnReset").click(function () {
                $("#divMRFAddApplicantModal div.filterFields input[type='search']").val("");
                $("#divMRFAddApplicantModal div.filterFields select").val("");
                $("#divMRFAddApplicantModal div.filterFields input[type='checkbox']").prop("checked", true);

                $("#divMRFAddApplicantModal #multiSelectedCurrentStep").html("");
                //$("#divMRFAddApplicantModal #multiSelectedWorkflow").html("");
                $("#divMRFAddApplicantModal #multiSelectedStatus").html("");
                $("#divMRFAddApplicantModal #multiSelectedStatusOption label, #divMRFAddApplicantModal #multiSelectedStatusOption input").prop("title", "add");
                $("#divMRFAddApplicantModal #btnSearch").click();
            });

            $("#divMRFAddApplicantModal #btnAddApplicant").click(function () {
                LoadPartial(ApplicantPickerURL, "divMRFApplicantPickerBodyModal");
                $("#divMRFAddApplicantModal").modal("hide");
                $("#divMRFApplicantPickerModal").modal("show");
            });

            $("#divMRFAddApplicantModal #btnCloseRequest").click(function () {
                if ($("#hdnIsHiredApplicantID").val() != "0" & $("#hdnIsHiredApplicantID").val() != "") {
                    objMRFAddApplicantListJS.ConvertConfirmation
                    ( 
                        MODAL_HEADER,
                        MSG_CONFIRM,
                        "objMRFAddApplicantListJS.UpdateStatusMRFRequest(" + $('#hdnIsHiredApplicantID').val() + ", '" + $(this).val() + "')",
                        "objMRFAddApplicantListJS.ConvertApplicant(" + $('#hdnIsHiredApplicantID').val() + "," + $('#hdnOrgGroupID').val() + "," + $('#hdnPositionID').val() + ")"
                    );
                }
                else {
                    $("#divMRFAddApplicantErrorMessage").html("<label class=\"errMessage\"><li>" + MSG_MRF_REQUEST_FOR_HIRING + "</li></label><br />");
                }
            });


            $("#divWorkflowTransactionModal #btnWorkflowTransactionSave").click(function () {
                $("#divMRFAddApplicantErrorMessage").hide();
                $("#divMRFAddApplicantErrorMessage").html("");

                if ($("#hdnLastStepCode").val() == $("#ddlStep").val() & ($("#hdnIsHiredApplicantID").val() || "") != "") {

                    $("#divMRFAddApplicantErrorMessage").show();
                    $("#divMRFAddApplicantErrorMessage").html("<label class=\"errMessage\"><li>" + ONE_MRF_HIRED + "</li></label><br />");
                }
                else {
                    if (objEMSCommonJS.ValidateBlankFields("#frmTransactionAdd", "#divMRFErrorMessage")) {
                        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                            "objEMSCommonJS.PostAjax(true \
                        , MRFAddApplicantModalSaveWorkflowURL \
                        , objMRFAddApplicantListJS.GetFormData() \
                        , '#divMRFAddApplicantErrorMessage' \
                        , '#divWorkflowTransactionModal #btnWorkflowTransactionSave' \
                        , objMRFAddApplicantListJS.EditSuccessFunction);", "function");
                    }

                }
                //if (currentStepBlanks > 0 || resultBlanks > 0) {
                //    $("#divMRFAddApplicantErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                //}
                //else {
                //    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                //        "objEMSCommonJS.PostAjax(true \
                //        , MRFAddApplicantModalSaveWorkflowURL \
                //        ,  objMRFAddApplicantListJS.GetFormData() \
                //        , '#divMRFAddApplicantErrorMessage' \
                //        , '#divApplicantModal #btnSave' \
                //        , objMRFAddApplicantListJS.EditSuccessFunction); ",
                //        "function");
                //}

            });

            $("#divMRFAddApplicantModal #btnSaveComment").click(function () {
                if ($("#divMRFAddApplicantModal #txtAreaComments").val() != "") {
                    //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    //    "objEMSCommonJS.PostAjax(true \
                    //    , SaveApplicantCommentsURL \
                    //    , objMRFAddApplicantListJS.GetCommentSectionFormData() \
                    //    , '#divMRFAddApplicantModal #divMRFErrorMessage' \
                    //    , '#divMRFAddApplicantModal #btnSaveComment' \
                    //    , objMRFAddApplicantListJS.GetApplicantComment);", "function");

                    objEMSCommonJS.PostAjax(true
                        , SaveApplicantCommentsURL
                        , objMRFAddApplicantListJS.GetCommentSectionFormData()
                        , '#divMRFAddApplicantModal #divMRFErrorMessage'
                        , '#divMRFAddApplicantModal #btnSaveComment'
                        , objMRFAddApplicantListJS.GetApplicantComment, null, true);

                }
                else {
                    $("#divMRFAddApplicantModal #txtAreaComments").focus();
                }

            });

            $("#btnBatchUpdate").click(function () {
                $('#divMRFAddApplicantErrorMessage').html('');
                var selRow = $("#tblManpowerAddApplicantList").jqGrid("getGridParam", "selarrrow");
                var firstValue = "";
                var isValid = true;
                if (selRow.length > 0) {
                    $(selRow).each(function (index, item) {
                        if (firstValue == "")
                            firstValue = $("#tblManpowerAddApplicantList").getRowData(item).CurrentStep;
                        else if (firstValue != $("#tblManpowerAddApplicantList").getRowData(item).CurrentStep)
                            isValid = false;
                    });
                    if (isValid) {
                        //$('#divMRFAddApplicantModal').modal('hide');
                        $('#divWorkflowTransactionModal').modal('show');
                        $("#ddlResult option").remove();
                        $("#frmTransactionAdd .form-control").val("");
                        //$("#hdnWorkflowID").val(WorkflowID);
                        //$("#hdnMRFApplicantID").val(MRFApplicantID);

                        var WorkflowStepOptions = [];
                        var GetSuccessFunction = function (data) {
                            $("#ddlStep option").remove();
                            $(data.Result).each(function (index, item) {
                                WorkflowStepOptions.push(
                                    {
                                        Value: item.Value,
                                        Text: item.Text,
                                    });
                            });
                            objEMSCommonJS.PopulateDropDown("#ddlStep", WorkflowStepOptions);

                            if ($("#lblIsConfidential").text() == "Yes") {
                                $("#ddlStep option[value='4A-JOB OFFER']").remove();
                            }
                            else {
                                $("#ddlStep option[value='4B-JOB OFFER HR HEAD']").remove();
                            }

                            $("#ddlStep option[value='0-NEW']").remove();
                        };
                        objEMSCommonJS.GetAjax(GetWorkflowStepDropDownURL + "&WorkflowCode=" + $("#hdnWorkflowCode").val() + "&CurrentStatus=" + firstValue, {}, "", GetSuccessFunction);

                        //GenerateDropdownValues(GetWorkflowStepDropDownURL + "&WorkflowCode=" + $("#hdnWorkflowCode").val(), "ddlStep", "Value", "Text", "", "", false);


                        //$("#divWorkflowTransactionModal .close").click(function () {
                        //    $("#divMRFAddApplicantModal").modal("show");
                        //    $("#divWorkflowTransactionModal").modal("hide");
                        //});

                        $("#ddlStep").unbind("change");
                        $("#ddlStep").change(function () {
                            if ($(this).val() != "") {
                                var GetResultTypeFunction = function (data) {
                                    var GetSuccessFunction = function (data) {
                                        $("#ddlResult option").remove();
                                        objEMSCommonJS.PopulateDropDown("#ddlResult", data.Result);
                                    };
                                    objEMSCommonJS.GetAjax(MRFAddApplicantReferenceValueURL + "&RefCode=" + data.Result.ResultType, {}, "", GetSuccessFunction);
                                };

                                objEMSCommonJS.GetAjax(MRFAddApplicantWorkflowStepURL + "&WorkflowCode=" + $("#hdnWorkflowCode").val() + "&StepCode=" + $(this).val()
                                    , {}, "", GetResultTypeFunction);
                            }
                            else {
                                $("#ddlResult option").remove();
                            }

                        });
                    }
                    else
                        $("#divMRFAddApplicantErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_DIFF_STATUS + " </li></label><br />");
                }
                else {
                    $("#divMRFAddApplicantErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Item</li></label><br />");
                }


/*
                if ($("#tblManpowerAddApplicantList").jqGrid("getGridParam", "selarrrow").length == 0) {
                    $("#divMRFAddApplicantErrorMessage").show();
                    $("#divMRFAddApplicantErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "applicant.</li></label><br />");
                }
                else {
                    

                    //objMRFAddApplicantListJS.WorkflowTransactionJQGrid({
                    //    WorkflowID: $("#hdnWorkflowID").val(),
                    //    MRFApplicantID: $("#hdnMRFApplicantID").val()
                    //});   
                }*/
            });

            $("#btnPrint").click(function () {

                var mrfID = $("#MRFTransactionID").val().substring(9, 16);
                $("#PrintMRFTransactionID").text(mrfID + "-")

                var GetSuccessFunction = function (data) {
                    if (data.rows.length >= 2) {
                        $("#PrintApprover1").text(data.rows[0].ApproverName);
                        $("#PrintApprover2").text(data.rows[1].ApproverName);
                        $("#PrintApprover1Date").text(data.rows[0].ApprovedDate);
                        $("#PrintApprover2Date").text(data.rows[1].ApprovedDate);
                        $("#PrintApprover1Remarks").text(data.rows[0].ApprovalRemarks);
                        $("#PrintApprover2Remarks").text(data.rows[1].ApprovalRemarks);


                        //var frontIndex = $("#hdnNameForHiring").val().indexOf(">");
                        //var backIndex = $("#hdnNameForHiring").val().indexOf("</");
                        //$("#PrintEmployeeName").text($("#hdnNameForHiring").val().substring(frontIndex + 1, backIndex));
                    }

                    objEMSCommonJS.ShowPrintModal("divPrintMRF");
                    //$("#divPrintMRF").show();
                    //var divToPrint = document.getElementById("divPrintMRF");
                    //newWin = window.open("");
                    //newWin.document.write(divToPrint.outerHTML);
                    //$("#divPrintMRF").hide();
                    //newWin.print();
                    //newWin.close();
                };
                objEMSCommonJS.GetAjax(GetApprovalHistoryURL
                    + "&RequestingPositionID=" + $("#divMRFAddApplicantModal #hdnPositionID").val()
                    + "&RequestingOrgGroupID=" + $("#divMRFAddApplicantModal #hdnOrgGroupID").val()
                    + "&PositionID=" + $("#divMRFAddApplicantModal #hdnPositionID").val()
                    + "&MRFID=" + $("#divMRFAddApplicantModal #hdnID").val()
                    , {}, "", GetSuccessFunction);

                //var GetSuccessFunction = function (data) {
                //    if ((data.Result || "") != "") {
                //        //if (data.Result.CurrentStepCode == "") {
                //        $("#PrintHiredRemarks").val(data.Result.ApproverRemarks);
                //        //}
                //    }

                //    var GetSuccessFunctionNested = function (data) {
                //        if (data.rows.length >= 2) {
                //            $("#PrintApprover1").text(data.rows[0].ApproverName);
                //            $("#PrintApprover2").text(data.rows[1].ApproverName);
                //            $("#PrintApprover1Date").text(data.rows[0].ApprovedDate);
                //            $("#PrintApprover2Date").text(data.rows[1].ApprovedDate);
                //            $("#PrintApprover1Remarks").text(data.rows[0].ApprovalRemarks);
                //            $("#PrintApprover2Remarks").text(data.rows[1].ApprovalRemarks);


                //            var frontIndex = $("#hdnNameForHiring").val().indexOf(">");
                //            var backIndex = $("#hdnNameForHiring").val().indexOf("</");
                //            $("#PrintServeDate").text($("#hdnNameForHiring").val().substring(frontIndex + 1, backIndex));
                //        }

                //        $("#divPrintMRF").show();
                //        var divToPrint = document.getElementById("divPrintMRF");
                //        newWin = window.open("");
                //        newWin.document.write(divToPrint.outerHTML);
                //        $("#divPrintMRF").hide();
                //        newWin.print();
                //        newWin.close();
                //    };
                //    objEMSCommonJS.GetAjax(GetApprovalHistoryURL
                //        + "&RequestingPositionID=" + $("#divMRFAddApplicantModal #hdnPositionID").val()
                //        + "&RequestingOrgGroupID=" + $("#divMRFAddApplicantModal #hdnOrgGroupID").val()
                //        + "&PositionID=" + $("#divMRFAddApplicantModal #hdnPositionID").val()
                //        + "&MRFID=" + $("#divMRFAddApplicantModal #hdnID").val()
                //        , {}, "", GetSuccessFunctionNested);
                //};

                //objEMSCommonJS.GetAjax(GetApplicantByMRFIDAndIDURL
                //    + "&MRFID=" + $("#divMRFAddApplicantModal #hdnID").val()
                //    + "&ApplicantID=" + $("#hdnIsHiredApplicantID").val()
                //    , {}, "", GetSuccessFunction);

                
            });

            $("#btnSendEmail").click(function () {
                $("#divMRFAddApplicantErrorMessage").hide();
                $("#divMRFAddApplicantErrorMessage").html("");

                var tableGrid = $("#tblManpowerAddApplicantList");
                var selRow = tableGrid.jqGrid("getGridParam", "selarrrow");

                if (selRow.length == 0) {
                    $("#divMRFAddApplicantErrorMessage").show();
                    $("#divMRFAddApplicantErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "applicant.</li></label><br />");
                }
                else if (selRow.length > 1) {
                    $("#divMRFAddApplicantErrorMessage").show();
                    $("#divMRFAddApplicantErrorMessage").html("<label class=\"errMessage\"><li>" + MSG_SELECT_ONE_ONLY + "</li></label><br />");
                }
                else {
                    var ID;
                    $(selRow).each(function (index, item) {
                        cellValue = tableGrid.jqGrid("getCell", item, "ID");
                        ID = cellValue;
                    });

                    $('#divSendEmailModal').modal('show');
                    LoadPartial(SendEmailURL + "?ID=" + ID + "&Position=" + $("#lblPosition").text() + "&Mrf=" + $("#MRFTransactionID").val(), "divSendEmailBodyModal");
                }
            });

            $("#btnCloseInternal").on("click", function () {
                $("#txtCloseInternalStatus").val(this.value);
                $("#divCloseInternalModal").modal("show");
            });

            $("#btnCloseInternalProceed").on("click", function () {
                var IsSuccessFunction = function () {
                    $('#divCloseInternalModal').modal('hide');
                    $('#divMRFList #btnSearch').click();
                };
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                        , PostCloseInternalMRFURL \
                        , objMRFAddApplicantListJS.GetCloseInternalFormData() \
                        , '#divMRFAddApplicantErrorMessage' \
                        , '#btnCloseInternalProceed' \
                        , "+IsSuccessFunction+");", "function");
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFAddApplicantModal #txtFilterCurrentStep"
                , RecruitmentCurrentStepAutoCompleteURL, 20, "divMRFAddApplicantModal #multiSelectedCurrentStep");

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFAddApplicantModal #txtFilterWorkflow"
            //    , RecruitmentWorkflowAutoCompleteURL, 20, "divMRFAddApplicantModal #multiSelectedWorkflow");

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("divMRFAddApplicantModal #multiSelectedStatus", GetApplicantStatusURL, "Value", "Description");
        },

        GetCloseInternalFormData: function () {
            var formData = new FormData($('#frmTransactionAdd').get(0));
            formData.append("MrfID", $('#hdnID').val());
            formData.append("Status", $("#txtCloseInternalStatus").val());
            formData.append("Remarks", $("#txtCloseInternalRemarks").val());
            return formData;
        },

        GetApplicantComment: function () {
            var input = {
                MRFID: $("#divMRFAddApplicantModal #hdnID").val()
            };

            $("#divMRFAddApplicantModal #txtAreaComments").val("");

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    $("#divMRFAddApplicantModal #divCommentsContainer").html("");
                    if (data.Result.length > 0) {
                        $("#divMRFAddApplicantModal #divCommentsContainer").append("<span class='comment-header'>-- Start --</span>");
                        $(data.Result).each(function (index, item) {
                            $("#divMRFAddApplicantModal #divCommentsContainer").append("<span class='comment-details'>" + item.Timestamp
                                + " " + item.Sender + ": </span><span class='comment'> " + item.Comments + "</span><br>");
                            if (data.Result.length <= (index + 1)) {
                                setTimeout(function () { $('#divMRFAddApplicantModal #divCommentsContainer').scrollTop($('#divMRFAddApplicantModal #divCommentsContainer')[0].scrollHeight) }, 300);
                            }
                        });
                    }
                    else {
                        $("#divMRFAddApplicantModal #divCommentsContainer").append("<span class='comment-header'>-- No comments found. --</span>");
                    }
                    $("#divMRFAddApplicantModal #txtAreaComments").attr("readonly", false);
                    $("#divMRFAddApplicantModal #txtAreaComments").prop("disabled", false);
                }
            };

            objEMSCommonJS.GetAjax(GetApplicantCommentsURL, input, "", GetSuccessFunction);
        },

        GetCommentSectionFormData: function () {
            var formData = new FormData($('#frmTransactionAdd').get(0));
            formData.append("CommentsForm.MRFID", $("#divMRFAddApplicantModal #hdnID").val());
            formData.append("CommentsForm.Comments", $("#divMRFAddApplicantModal #txtAreaComments").val());
            return formData;
        },

        EditSuccessFunction: function () {
            $("#divWorkflowTransactionModal").modal("hide");
            $("#frmTransactionAdd .form-control").val("");
            objMRFAddApplicantListJS.WorkflowTransactionJQGrid({
                WorkflowID: $("#hdnWorkflowID").val(),
                MRFApplicantID: $("#hdnMRFApplicantID").val()
            }); 
            LoadPartial(MRFAddApplicantModalURL + "?ID=" + $("#divMRFAddApplicantModal #hdnID").val(), "divMRFAddApplicantBodyModal");
        },

        GetFormData: function () {
            var formData = new FormData($('#frmTransactionAdd').get(0));
            //$(".BatchCurrentStep").each(function (index) {
            //    formData.append("Workflow[" + index + "].ApplicantID", $("#hdnID").val());
            //    formData.append("Workflow[" + index + "].CurrentStep", $(this).val());
            //});
            //$(".BatchResult").each(function (index) {
            //    formData.append("Workflow[" + index + "].Result", $(this).val());
            //});
            formData.append("Workflow.MRFID", $("#divMRFAddApplicantModal #hdnID").val());
            $($("#tblManpowerAddApplicantList").jqGrid("getGridParam", "selarrrow")).each(function (index, item) {
                formData.append("Workflow.BatchUpdateRecordIDs["+index+"]", item);
            });
            $($("#tblManpowerAddApplicantList").jqGrid("getGridParam", "selarrrow")).each(function (index, item) {
                formData.append("Workflow.BatchUpdateApplicantIDs[" + index + "]", $("#tblManpowerAddApplicantList").jqGrid("getCell", item, "ID"));
            });
            return formData;
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblManpowerAddApplicantList") == "" ? "" : $.parseJSON(localStorage.getItem("tblManpowerAddApplicantList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divMRFAddApplicantModal .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divMRFAddApplicantModal .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divMRFAddApplicantModal #filterFieldsContainer");
                });

                $("#divMRFAddApplicantModal .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divMRFAddApplicantModal div.filterFields").unbind("keyup");
                $("#divMRFAddApplicantModal div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divMRFAddApplicantModal #btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#divMRFAddApplicantModal #tblManpowerAddApplicantList").jqGrid("GridUnload");
            $("#divMRFAddApplicantModal #tblManpowerAddApplicantList").jqGrid("GridDestroy");
            $("#divMRFAddApplicantModal #tblManpowerAddApplicantList").jqGrid({
                url: MRFAddApplicantModalListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "", "", "For Hiring", "", "Applicant Name", "Application Status", "Status Update Date", "Status Update By",
                    "Date Scheduled", "Date Completed", "Approver Remarks", "Kick-out Points",
                    "Status", "", "", "", "", ""
                ],
                colModel: [
                    { hidden: true },
                    { width: 25, key: true, hidden: true, name: "MRFApplicantID", index: "MRFApplicantID", align: "center", sortable: true },
                    { width: 35, name: "", formatter: objMRFAddApplicantListJS.RemoveRecordLink, align: "center" },
                    { width: 35, name: "ForHiring", formatter: objMRFAddApplicantListJS.ForHiringCheckBox, align: "center", hidden: true },
                    { width: 35, name: "", align: "center", sortable: true, hidden: true, formatter: objMRFAddApplicantListJS.WorkflowTransactionLink },
                    { name: "ApplicantName", index: "ApplicantName", align: "left", sortable: true, formatter: objMRFAddApplicantListJS.ViewApplicantLink},
                    { name: "CurrentStep", index: "CurrentStep", align: "left", sortable: true },
                    { name: "LastUpdateDate", index: "LastUpdateDate", align: "left", sortable: true },
                    { name: "UpdateByName", index: "UpdateByName", align: "left", sortable: true },
                    { name: "DateScheduled", index: "DateScheduled", align: "left", sortable: true },
                    { name: "DateCompleted", index: "DateCompleted", align: "left", sortable: true },
                    { name: "ApproverRemarks", index: "ApproverRemarks", align: "left", sortable: true },
                    { name: "Points", index: "Points", align: "right", sortable: true, formatter: objMRFAddApplicantListJS.KickoutPoints },
                    { name: "Status", index: "Status", align: "left", sortable: true, hidden: true },
                    { name: "WorkflowID", index: "WorkflowID", hidden: true },
                    { name: "CurrentStepCode", index: "CurrentStepCode", hidden: true },
                    { name: "CurrentResult", index: "CurrentResult", hidden: true },
                    { name: "ResultType", index: "ResultType", hidden: true },
                    { name: "ID", index: "ID", hidden: true }
                ],
                toppager: $("#divMRFAddApplicantModal #divManpowerAddApplicantPager"),
                pager: $("#divMRFAddApplicantModal #divManpowerAddApplicantPager"),
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
                            $("#divMRFAddApplicantModal #divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        var IsCompletedCtr = 0;
                        if (data.rows.length > 0) {
                            var approverRemarksHired = "";
                            var approverRemarksDeployed = "";
                            var employeeHired = "";
                            var employeeDeployed = "";
                            for (var i = 0; i < data.rows.length; i++) {
                                if ($("#hdnIDForHiring").val() == data.rows[i].ID) {
                                    $("#chkForHiring_" + data.rows[i].ID).prop("checked", true);
                                    
                                }

                                if (data.rows[i].Status == "COMPLETED") {
                                    IsCompletedCtr++;
                                    $("#" + data.rows[i].MRFApplicantID).css({ "background": "#EEFFEE" });
                                    $("#hdnIsHiredApplicantID").val(data.rows[i].ID);
                                    $("#hdnNameForHiring").val($('#tblManpowerAddApplicantList').jqGrid('getCell', data.rows[i].MRFApplicantID, 'ApplicantName')
                                        .replace("<a", "<b").replace("</a", "</b"));
                                    employeeDeployed = data.rows[i].ApplicantName;
                                    approverRemarksDeployed = data.rows[i].ApproverRemarks;

                                }

                                if (data.rows[i].CurrentStep == "6-HIRED") {
                                    employeeHired = data.rows[i].ApplicantName;
                                    approverRemarksHired = data.rows[i].ApproverRemarks;
                                }

                                if (data.rows[i].CurrentStep == "7-DEPLOYED") {
                                    $("#hdnDeployedDateHired").val(data.rows[i].DateCompleted);
                                }

                            }

                            if (employeeHired != "" || employeeDeployed != "" ) {

                                //var frontIndex = $("#hdnNameForHiring").val().indexOf(">");
                                //var backIndex = $("#hdnNameForHiring").val().indexOf("</");
                                //$("#PrintEmployeeName").text($("#hdnNameForHiring").val().substring(frontIndex + 1, backIndex));
                                $("#PrintEmployeeName").text(employeeDeployed != "" ? employeeDeployed : employeeHired);
                                $("#PrintHiredRemarks").text(approverRemarksDeployed != "" ? approverRemarksDeployed : approverRemarksHired);
                            }

                            if (IsCompletedCtr > 0) {
                                $("#btnCloseRequest").show();
                            }

                            if ($("#hdnMRFStatus").val() == "CLOSED") {
                                $(".for-hiring-checkboxes").prop("disabled", true);
                                $("#divMRFAddApplicantModal #tblManpowerAddApplicantList").jqGrid('hideCol', [""]);
                            }

                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblManpowerAddApplicantList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divMRFAddApplicantModal #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divMRFAddApplicantModal .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $(".for-hiring-checkboxes").click(function (event) {
                            if ($(this).prop("checked")) {
                                $(".for-hiring-checkboxes").prop("checked", false);
                                $(this).prop("checked", true);
                                $("#divMRFAddApplicantErrorMessage").html("");
                            }

                            ModalConfirmation(MODAL_HEADER,
                                MSG_CONFIRM_UPDATE_FOR_HIRING_MRF
                                , "objMRFAddApplicantListJS.UpdateForHiringApplicant(" + $(this).prop("id") + ","
                                + $("#divMRFAddApplicantModal #hdnID").val() + "," + $(this).prop("id").replace("chkForHiring_", "") + "," + $(this).prop("checked") + ")",
                                "function", "objMRFAddApplicantListJS.CancelUpdateForHiringApplicant()");

                        });

                        if ($("#hdnHasRemoveApplicantFeature").val() == "false") {
                            $("#divMRFAddApplicantModal #tblManpowerAddApplicantList").jqGrid('hideCol', ["ID"]);
                            $("#divMRFAddApplicantModal #tblManpowerAddApplicantList").jqGrid('hideCol', [""]);
                        }

                    }

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divMRFAddApplicantModal .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    if (localStorage["MRFListFilterShowOption"] != undefined) {
                        $("#chkFilterShow").prop('checked', JSON.parse(localStorage["MRFListFilterShowOption"]));
                    }
                    objMRFAddApplicantListJS.ShowHideFilter();

                    $("#chkFilterShow").on('change', function () {
                        objMRFAddApplicantListJS.ShowHideFilter();
                        localStorage["MRFListFilterShowOption"] = $("#chkFilterShow").is(":checked");
                    });

                    $("#divMRFAddApplicantModal table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#divMRFAddApplicantModal table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#divMRFAddApplicantModal table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divMRFAddApplicantModal table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#divMRFAddApplicantModal table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divMRFAddApplicantModal table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#divMRFAddApplicantModal table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divMRFAddApplicantModal .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                    });
                },
                onSelectRow: function (ids) {
                    $("#divMRFAddApplicantErrorMessage").html("");
                    //$("#divMRFAddApplicantErrorMessage").hide();
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblManpowerAddApplicantList");
                    moveFilterFields();
                },
            }).navGrid("#divMRFAddApplicantModal #divMRFAddApplicantModal",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            //jQuery("#divMRFAddApplicantModal .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

           /* jQuery("#divMRFAddApplicantModal .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery("#divMRFAddApplicantModal .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#divMRFAddApplicantModal #lblFilter").after("<input type=\"checkbox\" id=\"chkFilterShow\" style=\"margin-right:15px; margin-top: 5px;\"></div>");*/

            $("#divManpowerAddApplicantPager").css("width", "100%");
            $("#divManpowerAddApplicantPager").css("height", "100%");

            $("#tblManpowerAddApplicantList_toppager_center").hide();
            $("#tblManpowerAddApplicantList_toppager_right").hide();
            $("#tblManpowerAddApplicantList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblFilter\" style=\"margin-left: 20%\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilterShow\" style=\"margin-right:15px; margin-top: 5px;\" checked=\"checked\"></div>");
            jQuery("#divManpowerAddApplicantPager .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divManpowerAddApplicantPager_custom_block_right").appendTo("#divManpowerAddApplicantPager_left");
            $("#divManpowerAddApplicantPager_center .ui-pg-table").appendTo("#divManpowerAddApplicantPager_right");
        },
        KickoutPoints: function (cellvalue, options, rowObject) {
            return (rowObject.Points + " / " + rowObject.TotalPoints);
        },

        SetLocalStorage: function () {

            localStorage["AddApplicantListID"] = $("#divMRFAddApplicantModal #txtFilterApplicantID").val();
            localStorage["AddApplicantListForHiringID"] = $("#hdnIDForHiring").val();
            localStorage["AddApplicantListApplicantName"] = $("#divMRFAddApplicantModal #txtFilterApplicantName").val();
            localStorage["AddApplicantListCurrentStepDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedCurrentStep").value;
            localStorage["AddApplicantListCurrentStepDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedCurrentStep").text;

            //localStorage["AddApplicantListWorkflowDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedWorkflow").value;
            //localStorage["AddApplicantListWorkflowDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedWorkflow").text;

            localStorage["AddApplicantListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedStatus").value;
            localStorage["AddApplicantListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFAddApplicantModal #multiSelectedStatus").text;


            localStorage["AddApplicantListScheduledFrom"] = $("#divMRFAddApplicantModal #txtFilterScheduledFrom").val();
            localStorage["AddApplicantListScheduledTo"] = $("#divMRFAddApplicantModal #txtFilterScheduledTo").val();
            localStorage["AddApplicantListCompletedFrom"] = $("#divMRFAddApplicantModal #txtFilterCompletedFrom").val();
            localStorage["AddApplicantListCompletedTo"] = $("#divMRFAddApplicantModal #txtFilterCompletedTo").val();
            localStorage["AddApplicantListApproverRemarks"] = $("#divMRFAddApplicantModal #txtFilterApproverRemarks").val();

        },

        GetLocalStorage: function () {
            $("#divMRFAddApplicantModal #txtFilterApplicantID").val(localStorage["AddApplicantListID"]);
            $("#divMRFAddApplicantModal #txtFilterApplicantName").val(localStorage["AddApplicantListApplicantName"]);

            objEMSCommonJS.SetMultiSelectList("divMRFAddApplicantModal #multiSelectedCurrentStep"
                , "AddApplicantListCurrentStepDelimited"
                , "AddApplicantListCurrentStepDelimitedText");

            //objEMSCommonJS.SetMultiSelectList("divMRFAddApplicantModal #multiSelectedWorkflow"
            //    , "AddApplicantListWorkflowDelimited"
            //    , "AddApplicantListWorkflowDelimitedText");

            objEMSCommonJS.SetMultiSelectList("divMRFAddApplicantModal #multiSelectedStatus"
                , "AddApplicantListStatusDelimited"
                , "AddApplicantListStatusDelimitedText");


             $("#divMRFAddApplicantModal #txtFilterScheduledFrom").val(localStorage["AddApplicantListScheduledFrom"]);
             $("#divMRFAddApplicantModal #txtFilterScheduledTo").val(localStorage["AddApplicantListScheduledTo"]);
             $("#divMRFAddApplicantModal #txtFilterCompletedFrom").val(localStorage["AddApplicantListCompletedFrom"]);
             $("#divMRFAddApplicantModal #txtFilterCompletedTo").val(localStorage["AddApplicantListCompletedTo"]);
             $("#divMRFAddApplicantModal #txtFilterApproverRemarks").val(localStorage["AddApplicantListApproverRemarks"]);
        },

        FormatAmount: function (cellvalue, options, rowObject) {
            return AddZeroes(rowObject.ExpectedSalary + "").withComma();
        },

        RemoveRecordLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, 'objMRFAddApplicantListJS.RemoveApplicant(" + rowObject.ID + ")', 'function'); return false;\"><span class='btn-glyph-dynamic glyphicon glyphicon-trash'></span></a>";
        },

        DeleteSuccessFunction: function (id) {
            LoadPartial(MRFAddApplicantModalURL + '?ID=' + $("#divMRFAddApplicantModal #hdnID").val() + '', 'divMRFAddApplicantBodyModal');
        },

        RemoveApplicant: function (id) {
            objEMSCommonJS.PostAjax(false
                , MRFAddApplicantModalRemoveApplicantURL + "?MRFID=" + $("#divMRFAddApplicantModal #hdnID").val() + '&MRFApplicantID=' + id
                , {}
                , '#divMRFAddApplicantModal #divErrorMessage'
                , ''
                , objMRFAddApplicantListJS.DeleteSuccessFunction);
        },

        ViewApplicantLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" onclick=\"return objMRFAddApplicantListJS.ShowApplicantDetails(" + rowObject.ID + ",'" + RecruitmentApplicantViewURL + "?ID=" + rowObject.ID + "', 'divApplicantBodyModal' , '" + rowObject.CurrentStep + "', '" + rowObject.MRFApplicantID +"');\">" + rowObject.ApplicantName + "</a>";
        },

        WorkflowTransactionLink: function (cellvalue, options, rowObject) {

            return "<a href=\"\" onclick=\"return objMRFAddApplicantListJS.ShowWorkflowTransaction(" + rowObject.ID + ", " + rowObject.WorkflowID + ", " + rowObject.MRFApplicantID + ", 'divWorkflowTransactionBodyModal');\"><span class=\"btn-glyph-dynamic\"><span class=\"glyphicon glyphicon-pencil\"></span></span></a>";

            //var populateWorkflowDropdown = function () {
            //    var GetSuccessFunction = function (data) {
            //        var options = [];
            //        $(data.Result).each(function (index, item) {
            //            options.push({
            //                Value: item.Value,
            //                Text: item.Text
            //            });
            //        });
            //        workflowStepDropdownOptions.push(
            //            {
            //                WorkflowID: rowObject.WorkflowID,
            //                options: options
            //            }
            //        );
            //    };
            //    objEMSCommonJS.GetAjax(GetWorkflowStepDropDownURL + "&WorkflowID=" + rowObject.WorkflowID, {}, "", GetSuccessFunction);
            //};

            //var populateWorkflowResultDropdown = function () {
            //    var GetSuccessFunction = function (data) {
            //        var options = [];
            //        $(data.Result).each(function (index, item) {
            //            options.push({
            //                Value: item.Value,
            //                Text: item.Description
            //            });
            //        });
            //        workflowStepResultDropdownOptions.push(
            //            {
            //                CurrentStepCode: rowObject.CurrentStepCode,
            //                options: options
            //            }
            //        );
            //    };
            //    objEMSCommonJS.GetAjax(MRFAddApplicantReferenceValueURL + "&RefCode=" + rowObject.ResultType, {}, "", GetSuccessFunction);
            //};

            //// Get Current Step Dropdown options per Workflow ID
            //if (workflowStepDropdownOptions.length == 0) {
            //    populateWorkflowDropdown();
            //}
            //else
            //{
            //    var IsNew = true;
            //    $(workflowStepDropdownOptions).each(function (index, item) {
            //        if (item.WorkflowID == rowObject.WorkflowID) {
            //            IsNew = false;
            //        }
            //    });

            //    if (IsNew)
            //        populateWorkflowDropdown();
            //}

            //// Get Dropdown options per Workflow Step
            //if (workflowStepResultDropdownOptions.length == 0) {
            //    populateWorkflowResultDropdown();
            //}
            //else {
            //    var IsNew = true;
            //    $(workflowStepResultDropdownOptions).each(function (index, item) {
            //        if (item.CurrentStepCode == rowObject.CurrentStepCode) {
            //            IsNew = false;
            //        }
            //    });

            //    if (IsNew)
            //        populateWorkflowResultDropdown();
            //}

            //return "<select class=\"BatchCurrentStep\" id=\"ddlCurrentStep_" + rowObject.ID + "\"  style=\"width:65%\" \
            // title=\"Current Workflow Step\"> \
            //        </select>\
            //<select class=\"BatchResult\" id=\"ddlResult_" + rowObject.ID + "\"  style=\"width:30%\" \
            // title=\"Result\"> \
            //        </select>";


        },

        ForHiringCheckBox: function (cellvalue, options, rowObject) {
            return "<input type=\"checkbox\" class=\"for-hiring-checkboxes\" id=\"chkForHiring_" + rowObject.ID + "\" title=\"For Hiring\">";
            //return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + RecruitmentApplicantViewURL + "?ID=" + rowObject.ID + "', 'divApplicantBodyModal');\">" + rowObject.ID + "</a>";
        },

        UpdateStatusMRFRequest: function (ID, Status,EmployeeID) {

            var successFunction = function (data) {
                $("#divMRFList #btnSearch").click();
                LoadPartial(MRFAddApplicantModalURL + '?ID=' + $("#divMRFAddApplicantModal #hdnID").val() + '', 'divMRFAddApplicantBodyModal');
            };

            objEMSCommonJS.PostAjax(true
                , MRFUpdateStatusURL + "?ID=" + ID + "&Status=" + Status
                + "&MRFTransactionID=" + $("#MRFTransactionID").val()
                + "&ApplicantID=" + $("#hdnIsHiredApplicantID").val()
                + "&IsExist=true"
                + "&EmployeeID=" + EmployeeID
                , objMRFAddApplicantListJS.GetCloseMRFFormData()
                , '#divMRFAddApplicantModal #divErrorMessage'
                , ''
                , successFunction);
        },

        GetCloseMRFFormData: function () {
            var formData = new FormData($('#frmTransactionAdd').get(0));
            formData.append("MRFStatus.ID", $("#divMRFAddApplicantModal #hdnID").val());
            formData.append("MRFStatus.Status", $("#btnCloseRequest").val());

            formData.append("Applicants.MRFTransactionID", $("#MRFTransactionID").val());
            formData.append("Applicants.HiredApplicantID", $("#hdnIsHiredApplicantID").val());

            //$($("#tblManpowerAddApplicantList").jqGrid("getGridParam", "selarrrow")).each(function (index, item) {
            //    formData.append("Applicants.ApplicantIDs[" + index + "]", item);
            //});

            $($("#tblManpowerAddApplicantList").jqGrid('getRowData')).each(function (index, item) {
                if ($("#hdnIsHiredApplicantID").val() != item.ID)
                    formData.append("Applicants.ApplicantIDs[" + index + "]", item.ID);
            });

            return formData;
        },

        ShowApplicantDetails: function (ApplicantID, URL, DivModal, CurrentStep, MRFApplicantID) {
            $('#tblManpowerAddApplicantList').jqGrid('resetSelection');
            $('#tblManpowerAddApplicantList').jqGrid('setSelection', MRFApplicantID);
            //$('#divMRFAddApplicantModal').modal('hide');
            $('#divApplicantModal').modal('show');
            $("#hdnMRFApplicantID").val(ApplicantID);
            var successFunction = function () {
                $("#divApplicantModal #btnDelete").remove();
                $("#divApplicantModal .close").click(function () {
                    $('#divApplicantModal').modal('hide');
                    objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApplicants', "#divMRFAddApplicantModal");
                    //$('#divMRFAddApplicantModal').modal('show');
                });


                $("#btnApplicantChangeStatus").show();
                $("#divApplicantBodyModal #btnEdit").hide();

                $("#btnApplicantChangeStatus").on("click", function () {
                    $('#divWorkflowTransactionModal').modal('show');
                    $("#ddlResult option").remove();
                    $("#frmTransactionAdd .form-control").val("");
                    //$("#hdnWorkflowID").val(WorkflowID);
                    //$("#hdnMRFApplicantID").val(MRFApplicantID);

                    var WorkflowStepOptions = [];
                    var GetSuccessFunction = function (data) {
                        $("#ddlStep option").remove();
                        $(data.Result).each(function (index, item) {
                            WorkflowStepOptions.push(
                                {
                                    Value: item.Value,
                                    Text: item.Text,
                                });
                        });
                        objEMSCommonJS.PopulateDropDown("#ddlStep", WorkflowStepOptions);

                        if ($("#lblIsConfidential").text() == "Yes") {
                            $("#ddlStep option[value='4A-JOB OFFER']").remove();
                        }
                        else {
                            $("#ddlStep option[value='4B-JOB OFFER HR HEAD']").remove();
                        }

                        $("#ddlStep option[value='0-NEW']").remove();
                    };
                    objEMSCommonJS.GetAjax(GetWorkflowStepDropDownURL + "&WorkflowCode=" + $("#hdnWorkflowCode").val() + "&CurrentStatus=" + CurrentStep, {}, "", GetSuccessFunction);

                    //GenerateDropdownValues(GetWorkflowStepDropDownURL + "&WorkflowCode=" + $("#hdnWorkflowCode").val(), "ddlStep", "Value", "Text", "", "", false);


                    //$("#divWorkflowTransactionModal .close").click(function () {
                    //    $("#divMRFAddApplicantModal").modal("show");
                    //    $("#divWorkflowTransactionModal").modal("hide");
                    //});

                    $("#ddlStep").unbind("change");
                    $("#ddlStep").change(function () {
                        if ($(this).val() != "") {
                            var GetResultTypeFunction = function (data) {
                                var GetSuccessFunction = function (data) {
                                    $("#ddlResult option").remove();
                                    objEMSCommonJS.PopulateDropDown("#ddlResult", data.Result);
                                };
                                objEMSCommonJS.GetAjax(MRFAddApplicantReferenceValueURL + "&RefCode=" + data.Result.ResultType, {}, "", GetSuccessFunction);
                            };

                            objEMSCommonJS.GetAjax(MRFAddApplicantWorkflowStepURL + "&WorkflowCode=" + $("#hdnWorkflowCode").val() + "&StepCode=" + $(this).val()
                                , {}, "", GetResultTypeFunction);
                        }
                        else {
                            $("#ddlResult option").remove();
                        }

                    });

                });

                //var MRFIDDropdownOptions = [];
                //var GetSuccessFunction = function (data) {
                //    $(data.Result).each(function (index, item) {
                //        MRFIDDropdownOptions.push(
                //            {
                //                Value: item.Value,
                //                Text: item.Text,
                //            });
                //    });
                //    objEMSCommonJS.PopulateDropDown("#ddlMRF", MRFIDDropdownOptions);
                //    $('#ddlMRF option').filter(function () {
                //        return ($(this).text() == $("#hdnMRFTransactionID").val());
                //    }).prop('selected', true);
                //    $('#ddlMRF').change();
                //};
                //objEMSCommonJS.GetAjax(MRFIDDropdownURL + "&ApplicantID=" + $("#hdnID").val(), {}, "", GetSuccessFunction);

                //GenerateDropdownValues(MRFIDDropdownURL + "&ApplicantID=" + $("#hdnMRFApplicantID").val(), "ddlMRF", "Value", "Text", "", "", false);
            };
            LoadPartialSuccessFunction(URL, DivModal, successFunction);


            return false;
        },

        ShowWorkflowTransaction: function (ApplicantID, WorkflowID, MRFApplicantID, DivModal) {
            $('#divMRFAddApplicantModal').modal('hide');
            $('#divWorkflowTransactionModal').modal('show');
            $("#ddlResult option").remove();
            $("#frmTransactionAdd .form-control").val("");
            $("#hdnWorkflowID").val(WorkflowID);
            $("#hdnMRFApplicantID").val(MRFApplicantID);

            GenerateDropdownValues(GetWorkflowStepDropDownURL + "&WorkflowID=" + WorkflowID, "ddlStep", "Value", "Text", "", "", false);
            $("#divWorkflowTransactionModal .close").click(function () {
                $("#divMRFAddApplicantModal").modal("show");
                $("#divWorkflowTransactionModal").modal("hide");
            });

            $("#ddlStep").unbind("change");
            $("#ddlStep").change(function () {
                if ($(this).val() != "") {
                    var GetResultTypeFunction = function (data) {
                        var GetSuccessFunction = function (data) {
                            $("#ddlResult option").remove();
                            objEMSCommonJS.PopulateDropDown("#ddlResult", data.Result);
                        };
                        objEMSCommonJS.GetAjax(MRFAddApplicantReferenceValueURL + "&RefCode=" + data.Result.ResultType, {}, "", GetSuccessFunction);
                    };

                    objEMSCommonJS.GetAjax(MRFAddApplicantWorkflowStepURL + "&WorkflowID=" + WorkflowID + "&StepCode=" + $(this).val()
                        , {}, "", GetResultTypeFunction);
                }
                else {
                    $("#ddlResult option").remove();
                }

            });

            objMRFAddApplicantListJS.WorkflowTransactionJQGrid({
                WorkflowID: $("#hdnWorkflowID").val(),
                MRFApplicantID: $("#hdnMRFApplicantID").val()
            });   

            return false;
        },

        WorkflowTransactionJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblWorkflowTransactionList").jqGrid("GridUnload");
            $("#tblWorkflowTransactionList").jqGrid("GridDestroy");
            $("#tblWorkflowTransactionList").jqGrid({
                url: MRFAddApplicantWorkflowTransactionURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Order", "Step", "Status", "Date Scheduled", "Date Completed",
                    "Timestamp", "Remarks"],
                colModel: [
                    { name: "", hidden: true },
                    { name: "Order", index: "Type", align: "left", sortable: false, hidden: true },
                    { name: "Step", index: "Title", align: "left", sortable: false },
                    { name: "Status", index: "Status", align: "left", sortable: false },
                    { name: "DateScheduled", index: "DateScheduled", align: "left", sortable: false },
                    { name: "DateCompleted", index: "DateCompleted", align: "left", sortable: false },
                    { name: "Timestamp", index: "Timestamp", align: "left", sortable: false },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false }
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
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblWorkflowTransactionList", data);
                        
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },


        UpdateForHiringApplicant: function (elementID, MRFID, ApplicantID, ForHiring) {
            var successFunction = function () {
                //$("#divMRFList #btnSearch").click();
                LoadPartial(MRFAddApplicantModalURL + '?ID=' + $("#divMRFAddApplicantModal #hdnID").val() + '', 'divMRFAddApplicantBodyModal');
            };
            
            objEMSCommonJS.PostAjax(false
                , MRFUpdateForHiringURL + "?MRFID=" + MRFID + "&MRFApplicantID=" + ApplicantID + "&ForHiring=" + ForHiring
                , {}
                , '#divMRFAddApplicantModal #divErrorMessage'
                , ''
                , successFunction);

        },

        CancelUpdateForHiringApplicant: function () {
            $(".for-hiring-checkboxes").prop("checked", false);
            $("#chkForHiring_" + $("#hdnIDForHiring").val()).prop("checked", true); 
        },

        EncodeEmployee: function (ID) {
            $("#divModal").modal("hide");
            $("#divMRFAddApplicantModal").modal("hide");
            $("#divEmployeeModal").modal("show");

            var isSuccessFunction = function () {
                var GetSuccessFunction = function (data) {
                    var d = new Date(data.Result.PersonalInformation.BirthDate);
                    var month = d.getMonth() + 1;
                    var day = d.getDate();
                    var year = d.getFullYear();
                    var OrgGroupValue = $("#divMRFAddApplicantModal #lblOrgGroup").text();
                    var PositionValue = $("#divMRFAddApplicantModal #lblPosition").text();
                    $("#divEmployeeBodyModal #txtLastName").val(data.Result.PersonalInformation.LastName);
                    $("#divEmployeeBodyModal #txtFirstName").val(data.Result.PersonalInformation.FirstName);
                    $("#divEmployeeBodyModal #txtMiddleName").val(data.Result.PersonalInformation.MiddleName);
                    $("#divEmployeeBodyModal #dpBirthDate").val(month + "/" + day + "/" + year);
                    $("#divEmployeeBodyModal #txtAddressLine1").val(data.Result.PersonalInformation.AddressLine1);
                    $("#divEmployeeBodyModal #txtAddressLine2").val(data.Result.PersonalInformation.AddressLine2);
                    $("#divEmployeeBodyModal #ddlPSGCRegion").val(data.Result.PersonalInformation.PSGCRegion);
                    $("#divEmployeeBodyModal #PSGCRegion").val(data.Result.PersonalInformation.PSGCRegion);
                    GenerateDropdownValues(GetCityDropDownByRegionURL + "&RegionID=" + $("#divEmployeeBodyModal #ddlPSGCRegion").val(), "divEmployeeBodyModal #ddlPSGCCity", "Value", "Text", "", "", false);
                    $("#divEmployeeBodyModal #ddlPSGCCity").val(data.Result.PersonalInformation.PSGCCity);
                    $("#divEmployeeBodyModal #PSGCCity").val(data.Result.PersonalInformation.PSGCCity);
                    $("#divEmployeeBodyModal #txtEmail").val(data.Result.PersonalInformation.Email);
                    $("#divEmployeeBodyModal #txtCellphoneNumber").val(data.Result.PersonalInformation.CellphoneNumber);
                    document.getElementById("txtCellphoneNumber").value = $("#divEmployeeBodyModal #txtCellphoneNumber").val().substr(3);
                    $('label[for="txtSystemUserName"]').hide();
                    $("#divEmployeeBodyModal #txtSystemUserName").hide();
                    $("#divEmployeeBodyModal #btnSave").click(function () {
                        objEmployeeAddJS.AddSuccessFunction = function () {
                            $("#divEmployeeModal").modal("hide");
                            //objMRFAddApplicantListJS.AddSystemUser
                            //    (
                            //        data.Result.PersonalInformation.FirstName,
                            //        data.Result.PersonalInformation.MiddleName,
                            //        data.Result.PersonalInformation.LastName,
                            //        data.Result.ID,
                            //        $("#MRFTransactionID").val()
                            //    );
                            $("#divMRFList #btnSearch").click();
                            //objMRFAddApplicantListJS.UpdateStatusMRFRequest($('#hdnIsHiredApplicantID').val(), 'CLOSED');
                            //$("#divApplicantFilter #btnSearch").click();
                        };
                    });
                    $("#divEmployeeModal #ddlStatus").val("PROBATIONARY");
                    $("#divEmployeeModal #ddlOrgGroup option:contains(" + OrgGroupValue + ")").attr('selected', 'selected');
                    $("#divEmployeeModal #ddlPosition option:contains(" + PositionValue + ")").attr('selected', 'selected');
                    $("#divEmployeeModal .close").click(function () {
                        $("#divEmployeeModal").modal("hide");
                        $("#divMRFAddApplicantModal").modal("show");
                        objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApplicants', "#divMRFAddApplicantModal");
                        //objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), "tab" + $(".tablinks:nth-child(1)").text().trim().replace(" ", ""), "#divMRFAddApplicantModal");
                    });

                };

                objEMSCommonJS.GetAjax(GetApplicantInfoURL + "&ID=" + ID, {}, "", GetSuccessFunction);
            };
            LoadPartialSuccessFunction(EmployeeAddURL, "divEmployeeBodyModal", isSuccessFunction);
        },

        ConvertApplicant: function (ApplicantID, OrgGroupID, PositionID) {

            var GetSuccessFunction = function (data) {
                if (data.IsExist) {
                    objMRFAddApplicantListJS.ExistConfirmation
                        (
                            MODAL_HEADER,
                            "Applicant already exist on Employee List",
                            "objMRFAddApplicantListJS.UpdateStatusMRFRequest(" + $('#hdnIsHiredApplicantID').val() + ", '" + $("#divMRFAddApplicantModal #btnCloseRequest").val() + "'," + data.EmployeeID +")"
                        );
                }
                else {
                    var Status = "CLOSED";
                    var DateHired = $("#hdnDeployedDateHired").val();

                    var successFunction = function (data) {
                        $("#divMRFList #btnSearch").click();
                        LoadPartial(MRFAddApplicantModalURL + '?ID=' + $("#divMRFAddApplicantModal #hdnID").val() + '', 'divMRFAddApplicantBodyModal');

                        var GetSuccessFunction = function (data) {
                            var button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objMRFAddApplicantListJS.LoadEmployeeDetails('" + NewEmployeeDetailsURL + "?ApplicantID=" + data.Result["ApplicantID"] + "&EmployeeID=" + data.Result["EmployeeID"] + "', 'divNewEmployeeBodyModal');\">Edit Employee</button>";
                            button += "&nbsp <button type=\"button\" id=\"btnModalClose\" class=\"btnBlue formbtn\" data-dismiss=\"modal\">Close</button>";

                            //var button = "<button type=\"button\" id=\"btnModalClose\" class=\"btnBlue formbtn\" data-dismiss=\"modal\">Close</button>";

                            $("#divHeaderModal").html(MODAL_HEADER);
                            $("#divBodyModal").html(MSG_SUCCESS_SAVED);
                            $("#divFooterModal").html(button);
                        };

                        objEMSCommonJS.GetAjax(ConvertApplicantURL
                            + "&ApplicantID=" + ApplicantID
                            + "&OrgGroupID=" + OrgGroupID
                            + "&PositionID=" + PositionID
                            + "&DateHired=" + DateHired, {}, "", GetSuccessFunction);
                    };

                    objEMSCommonJS.PostAjax(true
                        , MRFUpdateStatusURL + "?ID=" + ApplicantID + "&Status=" + Status
                        + "&MRFTransactionID=" + $("#MRFTransactionID").val()
                        + "&ApplicantID=" + $("#hdnIsHiredApplicantID").val()
                        + "&IsExist=false"
                        , objMRFAddApplicantListJS.GetCloseMRFFormData()
                        , '#divMRFAddApplicantModal #divErrorMessage'
                        , ''
                        , successFunction
                        , null
                        , true);
                }
            };

            objEMSCommonJS.GetAjax(EmployeeIfExistURL + "&ApplicantID=" + $('#hdnIsHiredApplicantID').val(), {}, "", GetSuccessFunction);
        },

        LoadEmployeeDetails: function (URL, DivModal) {
            $("#divModal").modal("hide");
            $("#divMRFAddApplicantModal").modal("hide");
            $('#divNewEmployeeModal').modal('show');
            var successFunction = function () {
                $("#divMRFList #btnSearch").click();
            };
            LoadPartialSuccessFunction(URL, DivModal, successFunction);
            return false;
        },

        AddSystemUser: function (FirstName, MiddleName, LastName, ApplicantID, MRFTransactionID) {
            var parameters = "&FirstName=" + FirstName
                + "&MiddleName=" + MiddleName
                + "&LastName=" + LastName
                + "&ApplicantID=" + ApplicantID
                + "&MRFTransactionID=" + MRFTransactionID;

            var GetSuccessFunction = function () {};

            objEMSCommonJS.GetAjax(AddSystemUserURL + parameters, {}, "", GetSuccessFunction);
        },

        GetCompanyDropDown: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    CompanyDropDown.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(CompanyDropDownURL, {}, "", GetSuccessFunction);
        },

        ExistConfirmation: function (header, body, closeFunction) {
            var button = "";

            button += "<button type=\"button\" id=\"btnCloseMRF\" class=\"btnBlue formbtn button-width-auto\" onclick=\"" + closeFunction + "\">Proceed to Close only</button>&nbsp;";
            button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnBlue formbtn button-width-auto\" data-dismiss=\"modal\">Cancel</button>";

            $("#divHeaderModal").html(header);
            $("#divBodyModal").html(body);
            $("#divFooterModal").html(button);
            $("#divModal").modal({
                backdrop: 'static',
                keyboard: false,
            });
        },
        ConvertConfirmation: function (header, body, closeFunction, convertFunction) {
            var button = "";

            //if (closeFunction != "")
                //button += "<button type=\"button\" id=\"btnCloseMRF\" class=\"btnBlue formbtn button-width-auto\" onclick=\"" + closeFunction + "\">Close Only</button>&nbsp;";

            if (convertFunction != "")
                button += "<button type=\"button\" id=\"btnConvertApplicant\" class=\"btnBlue formbtn button-width-auto\" onclick=\"" + convertFunction + "\">Close and Convert</button>&nbsp;";

            button += "<button type=\"button\" id=\"btnCloseConfirmationModal\" class=\"btnBlue formbtn button-width-auto\" data-dismiss=\"modal\">Cancel</button>";

            $("#divHeaderModal").html(header);
            $("#divBodyModal").html(body);
            $("#divFooterModal").html(button);
            $("#divModal").modal({
                backdrop: 'static',
                keyboard: false,
            });
        },

        ShowHideFilter: function () {
            if ($("#chkFilterShow").is(":checked")) {
                $("#divMRFAddApplicantModal .jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilterShow").is(":not(:checked)")) {
                $("#divMRFAddApplicantModal .jqgfirstrow .filterFields").hide();
            }
        }
    };
    
    objMRFAddApplicantListJS.Initialize();
});