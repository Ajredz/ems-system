var objUpdateLogActivityJS;

$(document).ready(function () {
    objUpdateLogActivityJS = {

        Initialize: function () {
            $("#divLogActivityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            $("#divLogActivityModal label[for='ddlType'] span, \
                    #divLogActivityModal label[for='ddlTitle'] span, \
                    #divLogActivityModal label[for='ddlStatus'] span, \
                    #divLogActivityModal label[for='txtTitle'] span").removeClass("reqField");
            $("#divLogActivityModal label[for='ddlType'] span, \
                   #divLogActivityModal label[for='ddlTitle'] span, \
                   #divLogActivityModal label[for='ddlStatus'] span, \
                   #divLogActivityModal label[for='txtTitle'] span").addClass("unreqField");

            $("#divLogActivityModal #btnEditActivity").show();
            s.ElementBinding();

            s.GetAttachmentTypeDropDownOptions();
            logActivityDeletedAttachments = [];

            objUpdateLogActivityJS.LoadLogActivityStatusHistoryJQGrid({
                ID: $("#hdnApplicantLogActivityID").val(),
            });

            $("#lblHeaderType").text($("#ddlType option:selected").text());
            //$("#lblHeaderSubType").text($("#ddlSubType option:selected").text());

            s.GetComment();
            s.GetAttachment();

            objEMSCommonJS.ChangeTab($("#divLogActivityModal .tablinks:nth-child(1)"), 'tabHistory', '#divLogActivityModal');

        },

        ElementBinding: function () {
            var s = this;

            $("#btnSaveActivity").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmLogActivity", "#divLogActivityErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , UpdateLogActivityPostURL \
                        , objUpdateLogActivityJS.GetFormData() \
                        , '#divLogActivityErrorMessage' \
                        , '#btnSaveActivity' \
                        , objUpdateLogActivityJS.UpdateSuccessFunction);",
                        "function");
                }
            });

            $("#divLogActivityModal #btnEditActivity").click(function () {
                //$("#divLogActivityModal #ddlStatus, #divLogActivityModal #txtRemarks").attr("disabled", false);
                $("#divLogActivityModal #ddlStatus, #divLogActivityModal #txtRemarks").attr("readonly", false);
                $("#divLogActivityModal label[for='ddlStatus'] span").addClass("reqField");
                $("#divLogActivityModal label[for='ddlStatus'] span").removeClass("unreqField");

                $("#divLogActivityModal #btnEditActivity").hide();
                $("#divLogActivityModal #btnSaveActivity").show();
                $("#divLogActivityModal #btnBackActivity").show();
            });

            $("#divLogActivityModal #btnBackActivity").click(function () {
                //$("#divLogActivityModal #ddlStatus, #divLogActivityModal #txtRemarks").attr("disabled", true);
                $("#divLogActivityModal #ddlStatus, #divLogActivityModal #txtRemarks").attr("readonly", true);
                $("#divLogActivityModal label[for='ddlStatus'] span").addClass("reqField");
                $("#divLogActivityModal label[for='ddlStatus'] span").removeClass("unreqField");

                $("#divLogActivityModal #btnEditActivity").show();
                $("#divLogActivityModal #btnSaveActivity").hide();
                $("#divLogActivityModal #btnBackActivity").hide();
            });

            $("#btnSaveComment").click(function () {
                if ($("#txtAreaComments").val() != "") {
                    //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE,
                    //    "objEMSCommonJS.PostAjax(true \
                    //    , SaveCommentsURL \
                    //    , objUpdateLogActivityJS.GetCommentSectionFormData() \
                    //    , '#divLogActivityErrorMessage' \
                    //    , '#btnSaveComment' \
                    //    , objUpdateLogActivityJS.GetComment);", "function");

                    objEMSCommonJS.PostAjax(true
                        , SaveCommentsURL
                        , objUpdateLogActivityJS.GetCommentSectionFormData()
                        , '#divLogActivityErrorMessage'
                        , '#btnSaveComment'
                        , objUpdateLogActivityJS.GetComment, null, true);
                }
                else {
                    $("#txtAreaComments").focus();
                }
            });

            $("#txtAssignedUser").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: ReferredByAutoComplete, // URL 
                        type: "GET",
                        dataType: "json",
                        data: {
                            Term: $("#txtAssignedUser").val(),
                            TopResults: 20
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                response($.map(data.Result, function (item) {
                                    return {
                                        label: item.Description,
                                        value: item.ID
                                    };
                                }))
                            }
                            else {
                                ModalAlert(MODAL_HEADER, data.Result);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            ModalAlert(MODAL_HEADER, jqXHR.responseText);
                        }
                    })
                },
                select: function (event, ui) { // Event - triggers after selection on list
                    if (ui.item.label != null) {
                    }
                    return false;
                },
                change: function (event, ui) { // Event - triggers when the value of the textbox changed
                    if (ui.item == null) {
                        $("#hdnAssignedUserID").val(0);
                        $(this).val("");
                    } else {
                        $("#hdnAssignedUserID").val(ui.item.value);
                        $(this).val(ui.item.label);
                    }
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            $("#btnSaveAttachmentActivity").click(function () {
                var isNoBlankFunction = function () {
                    if ($(".LogActivity_Attachment_Type").length == 0) {
                        $("#frmLogActivity #divLogActivityErrorMessage").html(
                            "<label class=\"errMessage\"><li>" + "Please click plus icon to add attachment" + "</li></label><br />");
                        return false;

                    }
                    else {
                        return true;
                    }
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmLogActivity #DivLogActivityUploadAttachmentDynamicFields", "#divLogActivityErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SaveAttachmentURL \
                        , objUpdateLogActivityJS.GetAttachmentSectionFormData() \
                        , '#divLogActivityErrorMessage' \
                        , '#btnSaveAttachmentActivity' \
                        , objUpdateLogActivityJS.AttachmentUpdateSuccessFunction);",
                        "function");
                }
            });

            $("#btnLogActivityAddAttachmentFields").click(function () {
                var fields = $("#DivLogActivityUploadAttachmentDynamicFields .required-field");
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
        },

        GetFormData: function () {
            var formData = new FormData($('#frmLogActivity').get(0));
            formData.append("ApplicantLogActivityForm.ApplicantID", $("#hdnID").val());
            return formData;
        },

        UpdateSuccessFunction: function () {
            $('#divLogActivityModal').modal('hide');
            objAssignedActivitiesListJS.LoadJQGrid({
                TypeDelimited: localStorage["AssignedActivitiesListType"],
                Title: localStorage["AssignedActivitiesListTitle"],
                Description: localStorage["AssignedActivitiesListDescription"],
                CurrentStatusDelimited: localStorage["AssignedActivitiesListCurrentStatus"],
                CurrentTimestampFrom: localStorage["AssignedActivitiesListCurrentTimestampFrom"],
                CurrentTimestampTo: localStorage["AssignedActivitiesListCurrentTimestampTo"],
            });
        },

        AttachmentUpdateSuccessFunction: function () {
            LoadPartial(UpdateLogActivityURL + "?ID=" + $("#hdnApplicantLogActivityID").val(), 'divLogActivityBodyModal');
        },

        LoadLogActivityStatusHistoryJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblLogActivityStatusHistoryList").jqGrid("GridUnload");
            $("#tblLogActivityStatusHistoryList").jqGrid("GridDestroy");
            $("#tblLogActivityStatusHistoryList").jqGrid({
                url: GetLogActivityStatusHistoryURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Status", "Timestamp", "User", "Remarks"],
                colModel: [
                    { name: "ID", index: "", hidden: true },
                    { name: "Status", index: "Status", align: "left", sortable: false },
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
                        AutoSizeColumnJQGrid("tblLogActivityStatusHistoryList", data);
                        $("#tblLogActivityStatusHistoryList_subgrid").width(20);
                        $(".jqgfirstrow td:first").width(20);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        UpdateLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return LoadPartial('" + ApplicantViewURL + "?ID=" + rowObject.ID + "', 'divApplicantBodyModal');\">Update</a>";
        },

        AddPreloadedSuccessFunction: function () {
            objUpdateLogActivityJS.LoadLogActivityJQGrid({
                ApplicantID: $("#hdnID").val()
            });
        },

        GetComment: function () {
            var input = {
                ID: $("#hdnApplicantLogActivityID").val()
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

            objEMSCommonJS.GetAjax(GetCommentsURL, input, "", GetSuccessFunction);
        },

        GetAttachment: function () {
            var s = this;
            var input = {
                ID: $("#hdnApplicantLogActivityID").val()
            };

            var GetSuccessFunction = function (data) {
                $("#DivLogActivityDownloadAttachmentDynamicFields").html("");
                var populateFields = function (item, idCtr) {
                    var htmlId = $(".LogActivityAttachmentDynamicFields:last").prop("id");
                    var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("LogActivityAttachmentDynamicFields_", "")) + 1;

                    $("#DivLogActivityDownloadAttachmentDynamicFields").append(
                        "<div class=\"form-group form-fields LogActivityAttachmentDynamicFields\" id=\"LogActivityAttachmentDynamicFields_" + ctr + "\">"
                        + "    <div class=\"col-md-1-5 no-padding\">"
                        + "        <select id=\"ddlType_" + ctr + "\" class= \"form-control required-field LogActivity_Attachment_Type\" title=\"Attachment Type\"></select >"
                        + "    </div>"
                        + "    <div class=\"col-md-2-5 no-padding\">"
                        + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\" id=\"txtRemarks_" + ctr + "\" maxlength=\"255\" class=\"form-control LogActivity_Attachment_Remarks\" title=\"Remarks\"></textarea>"
                        + "    </div>"
                        + "    <div class=\"col-md-3-5 no-padding\">"
                        + "        <label class=\"control-label block-label LogActivity_Attachment_File\"> <span class=\"unreqField\">* </span>" + item.SourceFile + "</label>"
                        + "        <input type=\"hidden\" id=\"txtServerFile_ " + ctr + "\" value=\"" + item.ServerFile + "\" class=\"form-control LogActivity_Attachment_ServerFile\">"
                        + "        <input type=\"hidden\" id=\"txtSourceFile_ " + ctr + "\" value=\"" + item.SourceFile + "\" class=\"form-control LogActivity_Attachment_SourceFile\">"
                        + "    </div>"
                        + "    <div class=\"col-md-3-5 no-padding\">"
                        + "        <label class=\"control-label block-label\"> <span class=\"unreqField\">* </span>" + item.UploadedBy + "<br> Timestamp: " + item.Timestamp + "</label>"
                        + "    </div>"
                        + "    <div class=\"col-md-0-5 no-padding\">"
                        + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objApplicantListJS.RemoveDynamicFields('#LogActivityAttachmentDynamicFields_" + ctr + "',objUpdateLogActivityJS.DeleteAttachmentFunction(&#39;" + item.ServerFile + "&#39;))&quot;, &quot;function&quot;);\"></span>"
                        + "    </div>"
                        + "     <div class=\"col-md-0-5 no-padding\">"
                        + "         <span class=\"btn-glyph-dynamic glyphicon glyphicon-download-alt\" onclick=\"objEMSCommonJS.DownloadAttachment(CheckFileIfExistsURL, 'WorkflowService_LogActivity_Attachment_Path','" + item.ServerFile + "', '" + item.SourceFile + "')\"></span>"
                        + "     </div>"
                        + "</div>"
                    );

                    objEMSCommonJS.PopulateDropDown("#DivLogActivityDownloadAttachmentDynamicFields #ddlType_" + ctr, LogActivityAttachmentTypeDropDownOptions);

                    $("#DivLogActivityDownloadAttachmentDynamicFields #ddlType_" + idCtr).val(item.AttachmentType);
                    $("#DivLogActivityDownloadAttachmentDynamicFields #txtRemarks_" + idCtr).val(item.Remarks);
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

            objEMSCommonJS.GetAjax(GetAttachmentURL, input, "", GetSuccessFunction);
        },

        GetCommentSectionFormData: function () {
            var formData = new FormData($('#frmLogActivity').get(0));
            formData.append("CommentsForm.ApplicantLogActivityID", $("#hdnApplicantLogActivityID").val());
            formData.append("CommentsForm.Comments", $("#txtAreaComments").val());
            return formData;
        },

        GetAttachmentSectionFormData: function () {
            $(".LogActivity_Attachment_Type").each(function (index) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].AttachmentType");
            });

            $(".LogActivity_Attachment_Remarks").each(function (index, item) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].Remarks");
            });

            $(".LogActivity_Attachment_ServerFile").each(function (index, item) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].ServerFile");
            });

            $(".LogActivity_Attachment_SourceFile").each(function (index, item) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].SourceFile");
            });

            $(".LogActivity_Attachment_File").each(function (index, item) {
                $(this).prop("name", "AttachmentForm.AddAttachmentForm[" + index + "].File");
            });

            var formData = new FormData($('#frmLogActivity').get(0));
            formData.append("AttachmentForm.ApplicantLogActivityID", $("#hdnApplicantLogActivityID").val());

            $(logActivityDeletedAttachments).each(function (index, item) {
                formData.append("AttachmentForm.DeleteAttachmentForm[" + index + "].ServerFile", item.value);
            });
            return formData;
        },

        GetAttachmentTypeDropDownOptions: function () {
            var s = this;
            LogActivityAttachmentTypeDropDownOptions = [];
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    LogActivityAttachmentTypeDropDownOptions.push(
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
            logActivityDeletedAttachments.push(serverFile);
        },

        AddAttachmentDynamicFields: function () {
            var s = this;
            var htmlId = $(".LogActivityAttachmentDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("LogActivityAttachmentDynamicFields_", "")) + 1;

            $("#DivLogActivityUploadAttachmentDynamicFields").append(
                "<div class=\"form-group form-fields LogActivityAttachmentDynamicFields\" id=\"LogActivityAttachmentDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <select id=\"ddlType_" + ctr + "\" class= \"form-control required-field LogActivity_Attachment_Type\" title=\"Attachment Type\"></select >"
                + "    </div>"
                + "    <div class=\"col-md-2-5 no-padding\">"
                + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\" id=\"txtRemarks_" + ctr + "\" maxlength=\"255\" class=\"form-control LogActivity_Attachment_Remarks\" title=\"Remarks\"></textarea>"
                + "    </div>"
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "        <input type=\"file\" id=\"fileAttachment_" + ctr + "\" class=\"form-control required-field LogActivity_Attachment_File\" title=\"Attachment\" accept=\".pdf,.doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document\">"
                + "    </div>"
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "    </div>"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objApplicantListJS.RemoveDynamicFields('#LogActivityAttachmentDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
                + "    </div>"
                + "</div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlType_" + ctr, LogActivityAttachmentTypeDropDownOptions);
        },
    };

    objUpdateLogActivityJS.Initialize();
});