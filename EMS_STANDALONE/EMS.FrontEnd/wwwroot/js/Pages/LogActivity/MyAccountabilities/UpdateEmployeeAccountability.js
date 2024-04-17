var objUpdateAccountabilityJS;

$(document).ready(function () {
    objUpdateAccountabilityJS = {

        Initialize: function () {
            $("#divAccountabilityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            //if (window.location.pathname.indexOf("all") >= 0) {
                $("#divAccountabilityModal .form-control").attr("disabled", true);
                $("#divAccountabilityModal label[for='ddlType'] span, \
                    #divAccountabilityModal label[for='ddlTitle'] span, \
                    #divAccountabilityModal label[for='ddlStatus'] span, \
                    #divAccountabilityModal label[for='txtTitle'] span").removeClass("reqField");
                $("#divAccountabilityModal label[for='ddlType'] span, \
                   #divAccountabilityModal label[for='ddlTitle'] span, \
                   #divAccountabilityModal label[for='ddlStatus'] span, \
                   #divAccountabilityModal label[for='txtTitle'] span").addClass("unreqField");
            //}

            $("#divAccountabilityModal #btnEditAccountability").show();
            s.ElementBinding();

            s.GetAttachmentTypeDropDownOptions();
            AccountabilityDeletedAttachments = [];

            objUpdateAccountabilityJS.LoadAccountabilityStatusHistoryJQGrid({
                ID: $("#hdnEmployeeAccountabilityID").val(),
            });

            //$("#lblHeaderType").text($("#ddlType option:selected").text());

            s.GetComment();
            s.GetAttachment();

            objEMSCommonJS.ChangeTabForm($(".tablinksform:nth-child(1)"), 'tabAccountability', '#divAccountabilityModal');
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabHistory', '#divAccountabilityModal');

            if ($("#ddlType").val() == "ACCOUNTABILITY_INTANGIBLE")
                $(".ShowIntangible").prop("hidden", false);
            else
                $(".ShowIntangible").prop("hidden", true);

            if (window.location.pathname.indexOf("all") <= 0) {
                $("#btnEditAccountability").hide();
            }

            if($("#hdnTitle").val().includes("INVENTORY"))
                $(".tablinks").find("span:contains('Movement')").parent("button").prop("hidden", false);

            if ($("#hdnClearingHRBP").val() == "true" && $("#hdnTitle").val() != "HR – TALENT MANAGEMENT - Exit Interview")
                    $("#btnChangeStatus").prop("hidden",true);
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSaveAccountability").click(function () {
                if (($("#ddlStatus").val() == "NOT_CLEARED" || $("#ddlStatus").val() == "CANCELLED") && $("#txtRemarks").val().trim() == "") {
                    $("#frmAccountability #divAccountabilityErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_INPUT_REMARKS + "</li></label><br />");
                    $("#txtRemarks").addClass("required-field errMessage");
                    return;
                }
                $("#txtRemarks").removeClass("required-field errMessage");

                $("#divAccountabilityModal .form-control").attr("disabled", false);

                if (objEMSCommonJS.ValidateBlankFields("#frmAccountability", "#frmAccountability #divAccountabilityErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , UpdateAccountabilityPostURL \
                        , objUpdateAccountabilityJS.GetFormData() \
                        , '#frmAccountability #divAccountabilityErrorMessage' \
                        , '#btnSaveAccountability' \
                        , objUpdateAccountabilityJS.UpdateSuccessFunction);",
                        "function");
                }
            });


            $("#ddlStatus").change(function () {
                if ($("#ddlStatus").val() == "CANCELLED") {
                    $("#remarksRequired").addClass("reqField");
                    $("#remarksRequired").removeClass("unreqField");
                }
                else {
                    $("#remarksRequired").addClass("unreqField");
                    $("#remarksRequired").removeClass("reqField");
                }

            });

            $("#divAccountabilityModal #btnEditAccountability").click(function () {
                if (window.location.pathname.indexOf("all") >= 0) {
                    $("#divAccountabilityModal .form-control").attr("disabled", false);
                    $("#divAccountabilityModal .form-control").attr("readonly", false);
                }
                else if (window.location.pathname.indexOf("my") >= 0) {
                    $("#divAccountabilityModal #ddlStatus, #divAccountabilityModal #txtRemarks").attr("disabled", false);
                    $("#divAccountabilityModal #ddlStatus, #divAccountabilityModal #txtRemarks").attr("readonly", false);
                    $("#divAccountabilityModal #ddlStatus option[value!='']option[value!='ACCEPTED']").remove();
                }
                else {
                    $("#divAccountabilityModal #ddlStatus, #divAccountabilityModal #txtRemarks").attr("disabled", false);
                    $("#divAccountabilityModal #ddlStatus, #divAccountabilityModal #txtRemarks").attr("readonly", false);
                }
                //$("#divAccountabilityModal label[for='ddlStatus'] span").addClass("reqField");
                //$("#divAccountabilityModal label[for='ddlStatus'] span").removeClass("unreqField");

                $("#divAccountabilityModal #btnEditAccountability,#btnChangeStatus").hide();
                $("#divAccountabilityModal #btnSaveAccountability").show();
                $("#divAccountabilityModal #btnBackAccountability").show();
            });

            $("#divAccountabilityModal #btnBackAccountability").click(function () {
                if (window.location.pathname.indexOf("all") >= 0) {
                    $("#divAccountabilityModal .form-control").attr("disabled", true);
                }

                $("#divAccountabilityModal #ddlStatus, #divAccountabilityModal #txtRemarks").attr("readonly", false);
                $("#divAccountabilityModal label[for='ddlStatus'] span").removeClass("reqField");
                $("#divAccountabilityModal label[for='ddlStatus'] span").addClass("unreqField");

                $("#divAccountabilityModal #btnEditAccountability,#btnChangeStatus").show();
                $("#divAccountabilityModal #btnSaveAccountability").hide();
                $("#divAccountabilityModal #btnBackAccountability").hide();
            });

            $("#btnSaveAccountabilityComment").click(function () {
                if ($("#divAccountabilityModal #txtAreaComments").val() != "") {
                    //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE,
                    //    "objEMSCommonJS.PostAjax(true \
                    //    , SaveAccountabilityCommentsURL \
                    //    , objUpdateAccountabilityJS.GetCommentSectionFormData() \
                    //    , '#divAccountabilityErrorMessage' \
                    //    , '#btnSaveAccountabilityComment' \
                    //    , objUpdateAccountabilityJS.GetComment);", "function");

                    objEMSCommonJS.PostAjax(true 
                        , SaveAccountabilityCommentsURL 
                        , objUpdateAccountabilityJS.GetCommentSectionFormData() 
                        , '#divAccountabilityErrorMessage' 
                        , '#btnSaveAccountabilityComment' 
                        , objUpdateAccountabilityJS.GetComment, null, true);
                }
                else {
                    $("#divAccountabilityModal #txtAreaComments").focus();
                }

            });

            $("#btnSaveAttachmentAccountability").click(function () {
                var isNoBlankFunction = function () {
                    if ($(".Accountability_Attachment_Type").length == 0) {
                        $("#frmAccountability #divAccountabilityErrorMessage").html(
                            "<label class=\"errMessage\"><li>" + "Please click plus icon to add attachment" + "</li></label><br />");
                        return false;

                    }
                    else {
                        return true;
                    }
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmAccountability #DivAccountabilityUploadAttachmentDynamicFields", "#frmAccountability #divAccountabilityErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SaveAccountabilityAttachmentURL \
                        , objUpdateAccountabilityJS.GetAttachmentSectionFormData() \
                        , '#frmAccountability #divAccountabilityErrorMessage' \
                        , '#btnSaveAttachmentAccountability' \
                        , objUpdateAccountabilityJS.AttachmentUpdateSuccessFunction);",
                        "function");
                }
            });

            $("#btnAccountabilityAddAttachmentFields").click(function () {
                var fields = $("#DivAccountabilityUploadAttachmentDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddAttachmentDynamicFields();
            });

            $(".tablinksform").find("span:contains('Exit Questionnaire')").parent("button").click(function () {
                objMyAccountabilitiesListJS.LoadQuestion($("#EmployeeID").val());
            });


            $("#btnChangeStatus").click(function () {
                if ($("#ChangeStatusModal").is(":visible"))
                    $("#ChangeStatusModal").hide();
                else {
                    $("#ddlDynamicChangeStatus,#txtDynamicChangeStatusRemarks").prop("disabled", false);
                    $("#ddlDynamicChangeStatus").addClass("required-field");
                    $("#ChangeStatusModal").show();

                    var Clearance = "Clearance";
                    if (window.location.pathname.indexOf("myaccountabilities") >= 0)
                        Clearance = "Employee";

                    GenerateDropdownValues(AccountabilityChangeStatus + "&CurrentStatus=" + $("#hdnStatus").val() + "&Form=" + Clearance, "divAccountabilityModal #ddlDynamicChangeStatus", "Value", "Text", "", "", false);
                }
            });
            $("#divAccountabilityModal #btnCancelDynamicChangeStatus").on("click", function () {
                $("#divAccountabilityModal #ChangeStatusModal").hide();
            });
            $("#divAccountabilityModal #ddlDynamicChangeStatus").on("change", function () {
                $("#divAccountabilityModal #divDynamicChangeStatusErrorMessage").html("");
                if ($("#divAccountabilityModal #ddlDynamicChangeStatus :selected").val() == "CANCELLED") {
                    $("#divAccountabilityModal #spnDynamicChangeStatus").addClass("reqField");
                    $("#divAccountabilityModal #spnDynamicChangeStatus").removeClass("unreqField");
                    $("#divAccountabilityModal #txtDynamicChangeStatusRemarks").addClass("required-field");
                }
                else {
                    $("#divAccountabilityModal #spnDynamicChangeStatus").addClass("unreqField");
                    $("#divAccountabilityModal #spnDynamicChangeStatus").removeClass("reqField");
                    $("#divAccountabilityModal #txtDynamicChangeStatusRemarks").removeClass("required-field");
                }
            });

            $("#btnSaveDynamicChangeStatus").click(function () {
                $("#divAccountabilityModal #divDynamicChangeStatusErrorMessage").html("");

                if ($("#divAccountabilityModal #ddlDynamicChangeStatus :selected").val() == "") {
                    $("#divAccountabilityModal #divDynamicChangeStatusErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + " Movement</li></label><br />");
                    return;
                }

                if ($("#ddlDynamicChangeStatus").val() == "CANCELLED" && $("#txtDynamicChangeStatusRemarks").val().trim() == "") {
                    $("#divDynamicChangeStatusErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_INPUT_REMARKS + "</li></label><br />");
                    $("#txtDynamicChangeStatusRemarks").addClass("required-field errMessage");
                    return;
                }

                var isNoBlankFunction = function () {
                    return true;
                };
                if (objEMSCommonJS.ValidateBlankFields("#divAccountabilityModal #DynamicChangeStatusForm", "#divAccountabilityModal #divDynamicChangeStatusErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , '" + AccountabilityChangeStatus + "&ID=" + $("#hdnEmployeeAccountabilityID").val() + "&NewStatus=" + $("#ddlDynamicChangeStatus :selected").val() + "&Remarks=" + encodeURIComponent($("#txtDynamicChangeStatusRemarks").val()) + "' \
                        , null \
                        , '#divAccountabilityModal #divDynamicChangeStatusErrorMessage' \
                        , '#divAccountabilityModal #btnSaveDynamicChangeStatus' \
                        , objUpdateAccountabilityJS.ChangeStatusSuccessFunction);",
                        "function");
                }
            });


            $("#btnDeleteThisAccountability").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE + SPAN_DELETE_START + " ID : " + $("#hdnEmployeeAccountabilityID").val() + SPAN_END,
                    "objEMSCommonJS.PostAjax(true \
                        , '" + AccountabilityDeleteURL + "&ID=" + $("#hdnEmployeeAccountabilityID").val() + "' \
                        , {} \
                        , '#divAccountabilityErrorMessage' \
                        , '#btnDeleteThisAccountability' \
                        , objUpdateAccountabilityJS.ChangeStatusSuccessFunction);",
                    "function");
            });

            $(".tablinks").find("span:contains('Movement')").parent("button").click(function () {
                objUpdateAccountabilityJS.LoadMovement({
                    EmployeeID: $("#EmployeeID").val(),
                });
            });

            objEMSCommonJS.BindAutoComplete("txtOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "hdnOrgGroup", "ID", "Description");
            objEMSCommonJS.BindAutoComplete("txtPosition"
                , PositionAutoCompleteURL, 20, "hdnPosition", "ID", "Description");
            objEMSCommonJS.BindAutoComplete("txtEmployee"
                , EmployeeAutoCompleteURL, 20, "hdnEmployee", "ID", "Description");
        },
        ChangeStatusSuccessFunction: function () {
            $("#ChangeStatusModal").hide();
            $('#divAccountabilityModal').modal('hide');
            $("#btnSearch").click();
        },

        GetFormData: function () {
            var formData = new FormData($('#frmAccountability').get(0));
            formData.append("EmployeeAccountabilityForm.EmployeeID", $("#divEmployeeModal #hdnID").val());
            return formData;
        },

        UpdateSuccessFunction: function () {
            LoadPartial(UpdateAccountabilityURL + "?ID=" + $("#hdnEmployeeAccountabilityID").val(), 'divAccountabilityBodyModal');
        },

        AttachmentUpdateSuccessFunction: function () {
            LoadPartial(UpdateAccountabilityURL + "?ID=" + $("#hdnEmployeeAccountabilityID").val(), 'divAccountabilityBodyModal');
        },
        // GPractice 317,320
        LoadAccountabilityStatusHistoryJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblAccountabilityStatusHistoryList").jqGrid("GridUnload");
            $("#tblAccountabilityStatusHistoryList").jqGrid("GridDestroy");
            $("#tblAccountabilityStatusHistoryList").jqGrid({
                url: GetAccountabilityStatusHistoryURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["Status", "TAT", "Updated Date", "Updated By", "Remarks"],
                colModel: [
                    { name: "Status", index: "Status", align: "left", sortable: false },
                    { name: "TAT", index: "TAT", align: "left", sortable: false },
                    { name: "Timestamp", index: "Timestamp", align: "left", sortable: false },
                    { name: "User", index: "User", align: "left", sortable: false },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false },
                ],
                //toppager: $("#divMRFFormApprovalHistoryPager"),   
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
                                //if (data.rows[i].ApprovalStatusCode == "FOR_APPROVAL") {
                                //    $("#hdnApproverPositionID").val(data.rows[i].PositionID);
                                //    $("#hdnApproverOrgGroupID").val(data.rows[i].OrgGroupID);
                                //}
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblAccountabilityStatusHistoryList", data);
                        //$("#tblAccountabilityStatusHistoryList_subgrid").width(20);
                        //$(".jqgfirstrow td:first").width(20);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        UpdateLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='Accountability-id-link' onclick=\"return LoadPartial('" + EmployeeViewURL + "?ID=" + rowObject.ID + "', 'divEmployeeBodyModal');\">Update</a>";
        },

        AddPreloadedSuccessFunction: function () {
            objUpdateAccountabilityJS.LoadAccountabilityJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
        },

        GetComment: function () {
            var input = {
                ID: $("#hdnEmployeeAccountabilityID").val()
            };

            $("#txtAreaComments").val("");

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    $("#divCommentsContainer").html("");
                    if (data.Result.length > 0) {
                        $("#divCommentsContainer").append("<span class='comment-header'>-- Start --</span>");
                        $(data.Result).each(function (index, item) {
                            $("#divCommentsContainer").append("<span class='comment-details'>" + item.Timestamp
                                + " " + item.Sender + ": </span><span class='comment'> " + item.Comments + "</span><br>");
                            if (data.Result.length <= (index + 1)) {
                                setTimeout(function () { $('#divCommentsContainer').scrollTop($('#divCommentsContainer')[0].scrollHeight) }, 300);
                            }
                        });
                    }
                    else {
                        $("#divCommentsContainer").append("<span class='comment-header'>-- No comments found. --</span>");
                    }
                    $("#txtAreaComments").attr("readonly", false);
                    $("#txtAreaComments").prop("disabled", false);
                }
            };

            objEMSCommonJS.GetAjax(GetAccountabilityCommentsURL, input, "", GetSuccessFunction);
        },

        GetAttachment: function () {
            var s = this;
            var input = {
                ID: $("#hdnEmployeeAccountabilityID").val()
            };

            var GetSuccessFunction = function (data) {
                $("#DivAccountabilityDownloadAttachmentDynamicFields").html("");
                var populateFields = function (item, idCtr) {
                    var htmlId = $(".AccountabilityAttachmentDynamicFields:last").prop("id");
                    var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("AccountabilityAttachmentDynamicFields_", "")) + 1;

                    $("#DivAccountabilityDownloadAttachmentDynamicFields").append(
                        "<div class=\"form-group form-fields AccountabilityAttachmentDynamicFields\" id=\"AccountabilityAttachmentDynamicFields_" + ctr + "\">"
                        + "    <div class=\"col-md-0-5 no-padding\">"
                        + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeListJS.RemoveDynamicFields('#AccountabilityAttachmentDynamicFields_" + ctr + "',objUpdateAccountabilityJS.DeleteAttachmentFunction(&#39;" + item.ServerFile + "&#39;))&quot;, &quot;function&quot;);\"></span>"
                        + "    </div>"
                        + "     <div class=\"col-md-0-5 no-padding\">"
                        + "         <span class=\"btn-glyph-dynamic glyphicon glyphicon-download-alt\" onclick=\"objEMSCommonJS.DownloadAttachment(CheckFileIfExistsURL, 'WorkflowService_Accountability_Attachment_Path','" + item.ServerFile + "', '" + item.SourceFile + "')\"></span>"
                        + "     </div>"
                        + "    <div class=\"col-md-1-5 no-padding\">"
                        + "        <select id=\"ddlType_" + ctr + "\" class= \"form-control required-field Accountability_Attachment_Type\" title=\"Attachment Type\"></select >"
                        + "    </div>"
                        + "    <div class=\"col-md-2-5 no-padding\">"
                        + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\" id=\"txtRemarks_" + ctr + "\" maxlength=\"255\" class=\"form-control Accountability_Attachment_Remarks\" title=\"Remarks\"></textarea>"
                        + "    </div>"
                        + "    <div class=\"col-md-3-5 no-padding\">"
                        + "        <label class=\"control-label block-label Accountability_Attachment_File\"> <span class=\"unreqField\">* </span>" + item.SourceFile + "</label>"
                        + "        <input type=\"hidden\" id=\"txtServerFile_ " + ctr + "\" value=\"" + item.ServerFile + "\" class=\"form-control Accountability_Attachment_ServerFile\">"
                        + "        <input type=\"hidden\" id=\"txtSourceFile_ " + ctr + "\" value=\"" + item.SourceFile + "\" class=\"form-control Accountability_Attachment_SourceFile\">"
                        + "    </div>"
                        + "    <div class=\"col-md-3-5 no-padding\">"
                        + "        <label class=\"control-label block-label\"> <span class=\"unreqField\">* </span>" + item.UploadedBy + "<br> Timestamp: " + item.Timestamp + "</label>"
                        + "    </div>"
                        + "</div>"
                    );

                    objEMSCommonJS.PopulateDropDown("#DivAccountabilityDownloadAttachmentDynamicFields #ddlType_" + ctr, AccountabilityAttachmentTypeDropDownOptions);

                    $("#DivAccountabilityDownloadAttachmentDynamicFields #ddlType_" + idCtr).val(item.AttachmentType);
                    $("#DivAccountabilityDownloadAttachmentDynamicFields #txtRemarks_" + idCtr).val(item.Remarks);
                };

                if (data.IsSuccess == true) {
                    $("#divAttachmentContainer").html("");
                    if (data.Result.length > 0) {
                        var ctr = 1;
                        $(data.Result).each(function (index, item) {
                            populateFields(item, ctr); ctr++;
                        });
                    }
                }
            };

            objEMSCommonJS.GetAjax(GetAccountabilityAttachmentURL, input, "", GetSuccessFunction);
        },

        GetCommentSectionFormData: function () {
            var formData = new FormData($('#frmAccountability').get(0));
            formData.append("CommentsForm.EmployeeAccountabilityID", $("#hdnEmployeeAccountabilityID").val());
            formData.append("CommentsForm.Comments", $("#txtAreaComments").val());
            return formData;
        },

        GetAttachmentSectionFormData: function () {
            $(".Accountability_Attachment_Type").each(function (index) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].AttachmentType");
            });

            $(".Accountability_Attachment_Remarks").each(function (index, item) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].Remarks");
            });

            $(".Accountability_Attachment_ServerFile").each(function (index, item) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].ServerFile");
            });

            $(".Accountability_Attachment_SourceFile").each(function (index, item) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].SourceFile");
            });

            $(".Accountability_Attachment_File").each(function (index, item) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].File");
            });

            var formData = new FormData($('#frmAccountability').get(0));
            formData.append("AttachmentForm.EmployeeAccountabilityID", $("#hdnEmployeeAccountabilityID").val());

            $(AccountabilityDeletedAttachments).each(function (index, item) {
                formData.append("AttachmentForm.DeleteAttachmentForm[" + index + "].ServerFile", item.value);
            });
            return formData;
        },

        GetAttachmentTypeDropDownOptions: function () {
            var s = this;
            AccountabilityAttachmentTypeDropDownOptions = [];
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    AccountabilityAttachmentTypeDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(AttachmentTypeDropDown, {}, "", GetSuccessFunction);
        },

        RemoveDynamicFields: function (id, deleteAttachmentFunction) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
            if (deleteAttachmentFunction != null)
                deleteAttachmentFunction();
        },

        DeleteAttachmentFunction: function (serverFile) {
            AccountabilityDeletedAttachments.push(serverFile);
        },

        AddAttachmentDynamicFields: function () {
            var s = this;
            var htmlId = $(".AccountabilityAttachmentDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("AccountabilityAttachmentDynamicFields_", "")) + 1;

            $("#DivAccountabilityUploadAttachmentDynamicFields").append(
                "<div class=\"form-group form-fields AccountabilityAttachmentDynamicFields\" id=\"AccountabilityAttachmentDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "    </div>"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeListJS.RemoveDynamicFields('#AccountabilityAttachmentDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
                + "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <select id=\"ddlType_" + ctr + "\" class= \"form-control required-field Accountability_Attachment_Type\" title=\"Attachment Type\"></select >"
                + "    </div>"
                + "    <div class=\"col-md-2-5 no-padding\">"
                + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\"  id=\"txtRemarks_" + ctr + "\" maxlength=\"255\" class=\"form-control Accountability_Attachment_Remarks\" title=\"Remarks\"></textarea>"
                + "    </div>"
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "        <input type=\"file\" id=\"fileAttachment_" + ctr + "\" class=\"form-control required-field Accountability_Attachment_File\" title=\"Attachment\" accept=\".pdf,.doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document\">"
                + "    </div>"
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "    </div>"
                + "</div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlType_" + ctr, AccountabilityAttachmentTypeDropDownOptions);
        },
        LoadMovement: function (param) {
            var s = this;
            Loading(true);
            $("#tblMovement").jqGrid("GridUnload");
            $("#tblMovement").jqGrid("GridDestroy");
            $("#tblMovement").jqGrid({
                url: GetEmployeeMovementByEmployeeIDsURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Movement Type", "Employee Field", "From", "To", "Date Effective From", "Date Effective To"],
                colModel: [
                    {
                        name: "ID", index: "ID", align: "center", sortable: false, formatter: function (cellvalue, options, rowObject) {
                            return objEMSCommonJS.JQGridIDFormat(rowObject.ID);
                        },
                    },
                    { name: "MovementType", index: "MovementType", align: "left", sortable: false },
                    { name: "EmployeeField", index: "EmployeeField", align: "left", sortable: false },
                    { name: "From", index: "From", align: "left", sortable: false },
                    { name: "To", index: "To", align: "left", sortable: false },
                    { name: "DateEffectiveFrom", index: "DateEffectiveFrom", align: "left", sortable: false },
                    { name: "DateEffectiveTo", index: "DateEffectiveTo", align: "left", sortable: false },
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

                        AutoSizeColumnJQGrid("tblMovement", data);
                        $("#tblMovement_subgrid").width(20);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });
        },
    };

    objUpdateAccountabilityJS.Initialize();
});