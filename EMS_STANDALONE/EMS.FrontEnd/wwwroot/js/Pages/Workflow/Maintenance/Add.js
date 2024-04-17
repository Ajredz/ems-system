var objWorkflowAddJS;
var ResultTypeDropDownOptions = [];

$(document).ready(function () {
    objWorkflowAddJS = {

        Initialize: function () {
            $("#divWorkflowBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#btnSave").show();
            $("#divWorkflowModal .form-control").attr("readonly", false);
            $("#btnDelete #btnBack").remove();
            $("#DivApproverRoleDynamicFields").html("");
            $("#divWorkflowApprovalModal .modal-dialog").attr("style", "width: 50%");
            s.GetResultTypeDropDownOptions();
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmWorkflow").trigger("reset");
            $("#DivApproverRoleDynamicFields").html("");
            $("#DivWorkflowStepDynamicFields").html("");
            objWorkflowAddJS.AddWorkflowStepDynamicFields();
        },

        ElementBinding: function () {
            var s = this;
            Code($("#txtCode"));
            PreventSpace($("#txtCode"));
            $('#txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });

            $("#divWorkflowBodyModal .close").click(function () {
                $("#DivApproverRoleDynamicFields").html("");
            });

            $("#btnAddWorkflowStepFields").click(function () {
                var fields = $("#DivWorkflowStepDynamicFields .required-field");
                var addNewFields = true;
                var addNewApprover = true;

                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });

                $(".OrderDynamicFields").each(function () {
                    var id = $(this).prop("id").replace("txtOrder_","");
                    var hasNoApprover = s.ValidateApproverRole(id);

                    if (hasNoApprover > 0) {
                        addNewApprover = false;
                        return false;
                    }
                });

                if (addNewFields)
                {
                    $(".required-field").removeClass("errMessage");

                    if (addNewApprover) {
                        $("#divWorkflowErrorMessage").html("");
                        s.AddWorkflowStepDynamicFields();
                    }
                    else {
                        $("#divWorkflowErrorMessage").html("<label class=\"errMessage\"><li>" + "Approver" + SUFF_REQUIRED + "</li></label><br />");
                    }
                }
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmWorkflow", "#divWorkflowErrorMessage", objWorkflowAddJS.ValidateDuplicateFields)) {
                    
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        " objEMSCommonJS.PostAjax(true \
                        , WorkflowAddPostURL \
                        , objWorkflowAddJS.GetFormData() \
                        , '#divWorkflowErrorMessage' \
                        , '#btnSave' \
                        , objWorkflowAddJS.AddSuccessFunction); ",
                        "function");
                }
            });
        },

        GetFormData: function () {

            var formData = new FormData($('#frmWorkflow').get(0));

            // Append dynamic fields into form data
            $(".OrderDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Workflow.WorkflowStepList[" + index + "].Order", $(this).val()); 
                }
            });

            $(".StepCodeDynamicFields").each(function (index1) {
                if ($(this).val() != "") {
                    var stepCodeValue = $(this).val();
                    formData.append("Workflow.WorkflowStepList[" + index1 + "].StepCode", stepCodeValue);
                }

                var id = $(this).prop("id").replace("txtStepCode_","");

                $(".RoleIDDynamicFields_" + id + " option").each(function (index2) {
                    if ($(this).val() != "") {
                        formData.append("Workflow.WorkflowStepList[" + index1 + "].WorkflowStepApproverList[" + index2 + "].StepCode", stepCodeValue); 
                        formData.append("Workflow.WorkflowStepList[" + index1 + "].WorkflowStepApproverList[" + index2 + "].RoleID", $(this).val()); 
                    }
                });
            });

            $(".StepDescriptionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Workflow.WorkflowStepList[" + index + "].StepDescription", $(this).val());
                }
            });

            $(".StepColorDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Workflow.WorkflowStepList[" + index + "].StatusColor", $(this).val());
                }
            });

            $(".ResultTypeDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Workflow.WorkflowStepList[" + index + "].ResultType", $(this).val());
                }
            });

            $(".IsRequiredDynamicFields").each(function (index) {
                var isChecked = $(this).is(':checked') ? true : false;
                formData.append("Workflow.WorkflowStepList[" + index + "].IsRequired", isChecked);
            });

            $(".AllowBackflowDynamicFields").each(function (index) {
                var isChecked = $(this).is(':checked') ? true : false;
                formData.append("Workflow.WorkflowStepList[" + index + "].AllowBackflow", isChecked);
            });

            $(".SendEmailToRequesterDynamicFields").each(function (index) {
                var isChecked = $(this).is(':checked') ? true : false;
                formData.append("Workflow.WorkflowStepList[" + index + "].SendEmailToRequester", isChecked);
            });

            $(".SendEmailToApproverDynamicFields").each(function (index) {
                var isChecked = $(this).is(':checked') ? true : false;
                formData.append("Workflow.WorkflowStepList[" + index + "].SendEmailToApprover", isChecked);
            });

            $(".TATDaysDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Workflow.WorkflowStepList[" + index + "].TATDays", $(this).val());
                }
            });

            return formData;
        },

        GetResultTypeDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    ResultTypeDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
                s.AddWorkflowStepDynamicFields();
            };

            objEMSCommonJS.GetAjax(AddResultTypeDropDownURL, {}, "", GetSuccessFunction);
        },

        PopulateDropDown: function (id, collection) {
            $(id).append($('<option/>', {
                value: "",
                text: "- Select an Item -"
            }));

            $(collection).each(function (index, item) {
                $(id).append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
        },

        AddWorkflowStepDynamicFields: function () {
            var s = this;
            var htmlId = $(".WorkflowStepDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("WorkflowStepDynamicFields_", "")) + 1;
            var order = ctr != 1 ? parseInt($("#txtOrder_" + (ctr - 1)).val()) + 1 : 1;

            $("#DivWorkflowStepDynamicFields").append(
            "<div class=\"form-group form-fields WorkflowStepDynamicFields\" id=\"WorkflowStepDynamicFields_" + ctr + "\">"
            + "    <div class=\"col-md-0-5 text-align-center no-padding\">"
            + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objWorkflowAddJS.RemoveDynamicFields('#WorkflowStepDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
            + "    </div>"
            + "    <div class=\"col-md-0-75 no-padding\">"
            + "        <input id=\"txtOrder_" + ctr + "\" value=\"" + order + "\" class=\"form-control OrderDynamicFields required-field text-amount\" maxlength=\"17\" title=\"Order\">"
            + "    </div>"
            + "    <div class=\"col-md-1-5 no-padding\">"
            + "        <input type=\"text\" id=\"txtStepCode_" + ctr + "\" class=\"form-control required-field StepCodeDynamicFields\" title=\"Step Code\" maxlength=\"50\">"
            + "    </div>"
            + "    <div class=\"col-md-2 no-padding\">"
            + "        <input type=\"text\" id=\"txtStepDescription_" + ctr + "\" class=\"form-control required-field StepDescriptionDynamicFields\" title=\"Step Description\" maxlength=\"255\">"
            + "    </div>"
            + "    <div class=\"col-md-1 no-padding\">"
            + "        <input type=\"text\" id=\"txtStepColor_" + ctr + "\" class=\"form-control StepColorDynamicFields\" title=\"Step Color\" maxlength=\"255\">"
            + "    </div>"
            + "    <div class=\"col-md-1-5 no-padding\">"
            + "        <select id=\"ddlResultType_" + ctr + "\" title=\"Result Type\" class=\"form-control required-field ResultTypeDynamicFields\"></select>"
            + "    </div>"
            + "    <div class=\"col-md-0-75 text-align-center no-padding\">"
            + "        <input type=\"checkbox\" id=\"chkIsRequired_" + ctr + "\" class=\"IsRequiredDynamicFields\" title=\"IsRequired\">"
            + "    </div>"
            + "    <div class=\"col-md-0-75 text-align-center no-padding\">"
            + "        <input type=\"checkbox\" id=\"chkAllowBackflow_" + ctr + "\" class=\"AllowBackflowDynamicFields\" title=\"Allow Back Flow\">"
            + "    </div>"
            + "    <div class=\"col-md-0-75 text-align-center no-padding\">"
            + "        <input type=\"checkbox\" id=\"chkSendEmailToRequester_" + ctr + "\" class=\"SendEmailToRequesterDynamicFields\" title=\"Send Email to Requester\">"
            + "    </div>"
            + "    <div class=\"col-md-0-75 text-align-center no-padding\">"
            + "        <input type=\"checkbox\" id=\"chkSendEmailToApprover_" + ctr + "\" class=\"SendEmailToApproverDynamicFields\" title=\"Send Email to Approver\">"
            + "    </div>"
            + "    <div class=\"col-md-0-75 no-padding\">"
            + "        <input type=\"text\" id=\"txtTATDays_" + ctr + "\" class=\"form-control text-amount required-field TATDaysDynamicFields\" title=\"TAT Days\">"
            + "    </div>"
            + "    <div class=\"col-md-0-75 text-align-center no-padding\">"
            + "        <label id=\"btnApprover_" + ctr + "\">"
            + "           <span class=\"btn-glyph-dynamic glyphicon glyphicon-list\" title=\"Click to add approver role\"></span>"
            + "        </label>"
            + "    </div>"
            + "</div>"
            );

            $("#btnApprover_" + ctr).click(function () {
                s.AddApproverRoleDynamicFields(ctr);
            });

            s.PopulateDropDown("#ddlResultType_" + ctr, ResultTypeDropDownOptions);

            NumberOnly($("#txtTATDays_" + ctr));
            NumberOnly($("#txtOrder_" + ctr));
        },

        AddApproverRoleDynamicFields: function (id) {
            var htmlId = $(".ApproverRoleDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ApproverRoleDynamicFields_", "")) + 1;

            //Append the Approver Role Modal every first click of Approver button
            if (id == ctr)
            {
                $("#DivApproverRoleDynamicFields").append(
                    "<div class=\"form-group form-fields ApproverRoleDynamicFields\" id=\"ApproverRoleDynamicFields_" + id + "\">"
                    + "   <div class=\"col-md-5\">"
                    + "       <input type=\"text\" id=\"\" class=\"form-control listbox txtSearchBoxOne\" placeholder=\"Search here...\"/>"
                    + "       <select id=\"ddlListOne_" + id + "\" class=\"form-control listbox\" size=\"10\"></select>"
                    + "   </div>"
                    + "   <div class=\"col-md-2 text-align-center listboxbutton\">"
                    + "       <br/>"
                    + "       <br/>"
                    + "       <button type=\"button\" id=\"btnMoveToListTwoAll_" + id + "\" class=\"btnBlue formbtn btn-transfer\">>></button>"
                    + "       <button type=\"button\" id=\"btnMoveToListTwo_" + id + "\" class=\"btnBlue formbtn btn-transfer\">></button>"
                    + "       <button type=\"button\" id=\"btnMoveToListOne_" + id + "\" class=\"btnBlue formbtn btn-transfer\"><</button>"
                    + "       <button type=\"button\" id=\"btnMoveToListOneAll_" + id + "\" class=\"btnBlue formbtn btn-transfer\"><<</button>"
                    + "   </div>"
                    + "   <div class=\"col-md-5 RoleIDList\">"
                    + "       <input type=\"text\" id=\"\" class=\"form-control listbox txtSearchBoxTwo\" placeholder=\"Search here...\"/>"
                    + "       <select id=\"ddlListTwo_" + id + "\" class=\"form-control listbox required-field RoleIDDynamicFields_" + id + "\" size=\"10\"></select>"
                    + "   </div>"
                    + "</div>"
                );

                ListBoxGenerateValues(AddApproverRoleDropDownURL, {}, "ddlListOne_" + id, "Value", "Text");

                $("#btnMoveToListTwo_" + id).click(function () {
                    ListBoxMove("ddlListOne_" + id, "ddlListTwo_" + id, false, true);
                });

                $("#btnMoveToListOne_" + id).click(function () {
                    ListBoxMove("ddlListTwo_" + id, "ddlListOne_" + id, false, true);
                });

                $("#btnMoveToListTwoAll_" + id).click(function () {
                    ListBoxMove("ddlListOne_" + id, "ddlListTwo_" + id, true, true);
                });

                $("#btnMoveToListOneAll_" + id).click(function () {
                    ListBoxMove("ddlListTwo_" + id, "ddlListOne_" + id, true, true);
                });

                $(".txtSearchBoxOne").on("change keyup keydown", function () {
                    ListBoxSearch("ddlListOne_" + id, $(this).val(), false);
                });

                $(".txtSearchBoxTwo").on("change keyup keydown", function () {
                    ListBoxSearch("ddlListTwo_" + id, $(this).val(), false);
                });

                $("#divWorkflowApprovalHeaderModal .close").click(function () {
                    $("#ApproverRoleDynamicFields_" + id).hide();
                });
            }
            
            $("#ApproverRoleDynamicFields_" + id).show();
            $("#divWorkflowApprovalModal").modal("show");
        },

        RemoveDynamicFields: function (id) {
            var ctrToDelete = parseInt(id == undefined ? 0 : id.replace("#WorkflowStepDynamicFields_", ""));
            $(".WorkflowStepDynamicFields").each(function () {
                var ctr = $(this).prop("id").replace("WorkflowStepDynamicFields_", "");
                if (ctrToDelete < ctr)
                    $("#txtOrder_" + ctr).val($("#txtOrder_" + ctr).val() - 1);
            });

            $(id).remove();
            $("#divWorkflowApprovalModal #divWorkflowApprovalBodyModal #DivApproverRoleDynamicFields #ApproverRoleDynamicFields_" + ctrToDelete).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },

        ValidateApproverRole: function (ctr) {
            var hasNoApprover = 0;
            var approverLength = $("#ddlListTwo_" + ctr + " option").length;

            if (approverLength == 0) {
                hasNoApprover++;
                $("#btnApprover_" + ctr).attr('style', 'color: #c40000');
            }
            else {
                $("#ddlListTwo_" + ctr + " option").each(function (n1, x1) {
                    if (this.value == "") {
                        hasNoApprover++;
                        $("#btnApprover_" + ctr).attr('style', 'color: #c40000');
                    }
                    else {
                        $("#btnApprover_" + ctr).attr('style', 'color: black');
                    }
                });
            }

            return hasNoApprover;
        },

        ValidateDuplicateFields: function () {
            var isValid = true;
            var isDuplicate = 0;
            var isDuplicateOrder = 0;
            var orderIncorrectSequence = 0;
            var OrderArray = [];

            $(".OrderDynamicFields").each(function (n1, x1) {
                if (this.value != "") {
                    //var currentVal = this.value;
                    //var ctr = $(this).prop("id").replace("txtOrder_", "");
                    //$("#WorkflowStepDynamicFields_" + ctr + " #txtOrder_" + ctr).removeClass("errMessage");

                    //$(".OrderDynamicFields").not(this).filter(function () {
                    //    if (this.value == currentVal) {
                    //        $("#WorkflowStepDynamicFields_" + ctr + " #txtOrder_" + ctr).addClass("errMessage");
                    //        isDuplicateOrder++;
                    //    }
                    //});

                    /* Include order 0, to always show as an option */
                    if (parseInt(this.value) > 0) {
                        OrderArray.push(this.value);
                    }
                }
            });

            //OrderArray.sort();
            $(".OrderDynamicFields").removeClass("errMessage");
            $($.unique(OrderArray).sort()).each(function (index, item) {
                if ((parseInt(index) + 1) != item) {
                    orderIncorrectSequence++;
                }
            });

            $(".StepCodeDynamicFields").each(function (n1, x1) {
                if (this.value != "") {
                    var currentVal = this.value;
                    var ctr = $(this).prop("id").replace("txtStepCode_", "");
                    $("#WorkflowStepDynamicFields_" + ctr + " #txtStepCode_" + ctr).removeClass("errMessage");
                    $("#WorkflowStepDynamicFields_" + ctr + " #txtStepDescription_" + ctr).removeClass("errMessage");
                    $("#WorkflowStepDynamicFields_" + ctr + " #txtStepColor_" + ctr).removeClass("errMessage");
                    $("#WorkflowStepDynamicFields_" + ctr + " #ddlResultType_" + ctr).removeClass("errMessage");
                    $("#WorkflowStepDynamicFields_" + ctr + " #txtTATDays_" + ctr).removeClass("errMessage");

                    $(".StepCodeDynamicFields").not(this).filter(function () {
                        if (this.value == currentVal) {
                            $("#WorkflowStepDynamicFields_" + ctr + " #txtStepCode_" + ctr).addClass("errMessage");
                            $("#WorkflowStepDynamicFields_" + ctr + " #txtStepDescription_" + ctr).addClass("errMessage");
                            $("#WorkflowStepDynamicFields_" + ctr + " #txtStepColor_" + ctr).addClass("errMessage");
                            $("#WorkflowStepDynamicFields_" + ctr + " #ddlResultType_" + ctr).addClass("errMessage");
                            $("#WorkflowStepDynamicFields_" + ctr + " #txtTATDays_" + ctr).addClass("errMessage");
                            isDuplicate++;
                        }
                    });

                    var hasNoApprover = objWorkflowAddJS.ValidateApproverRole(ctr);
                    if (hasNoApprover > 0) {
                        $("#divWorkflowErrorMessage").html("<label class=\"errMessage\"><li>" + "Approver" + SUFF_REQUIRED + "</li></label><br />");
                        isValid = false;
                    }
                }
            });

            if (isDuplicate > 0) {
                $(".StepCodeDynamicFields").addClass("errMessage");
                $("#divWorkflowErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }
            //if (isDuplicateOrder > 0) {
            //    $("#divWorkflowErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
            //    isValid = false;
            //}
            if (orderIncorrectSequence > 0) {
                $(".OrderDynamicFields").addClass("errMessage");
                $("#divWorkflowErrorMessage").html("<label class=\"errMessage\"><li>" + INCORRECT_ORDER_SEQUENCE + "</li></label><br />");
                isValid = false;
            }

            if (!isValid)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return isValid;
        },

    };

    objWorkflowAddJS.Initialize();
});