var objWorkflowViewJS;
var ApprovalRoleListBoxOptions = [];

$(document).ready(function () {
    objWorkflowViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divWorkflowBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#btnAddWorkflowStepFields").hide();
            s.GetApprovalRoleListBoxOptions();
            s.LoadWorkflowStep();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $("#divWorkflowModal").modal("hide");
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , WorkflowDeleteURL + '?ID=' + objWorkflowViewJS.ID\
                    , {} \
                    , '#divWorkflowErrorMessage' \
                    , '#btnDelete' \
                    , objWorkflowViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(WorkflowEditURL + "?ID=" + objWorkflowViewJS.ID, "divWorkflowBodyModal");
            });
        },

        GetApprovalRoleListBoxOptions: function () {
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    ApprovalRoleListBoxOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(ViewApproverRoleDropDownURL, {}, "", GetSuccessFunction);
        },

        AddWorkflowStepDynamicFields: function () {
            var s = this;
            var htmlId = $(".WorkflowStepDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("WorkflowStepDynamicFields_", "")) + 1;
            var order = ctr != 1 ? parseInt($("#txtOrder_" + (ctr - 1)).val()) + 1 : 1;

            $("#DivWorkflowStepDynamicFields").append(
                "<div class=\"form-group form-fields WorkflowStepDynamicFields\" id=\"WorkflowStepDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-1 no-padding\">"
                + "        <input id=\"txtOrder_" + ctr + "\" value=\"" + order + "\" class=\"form-control OrderDynamicFields text-amount\" maxlength=\"17\" title=\"Order\" readonly>"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <input type=\"text\" id=\"txtStepCode_" + ctr + "\" class=\"form-control StepCodeDynamicFields\" title=\"Step Code\">"
                + "    </div>"
                + "    <div class=\"col-md-2-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtStepDescription_" + ctr + "\" class=\"form-control StepDescriptionDynamicFields\" title=\"Step Description\">"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <select id=\"ddlResultType_" + ctr + "\" title=\"Result Type\" class=\"form-control ResultTypeDynamicFields\"><option value = \"\">- Select an item -</option></select>"
                + "    </div>"
                + "    <div class=\"col-md-1 text-align-center no-padding\">"
                + "        <input type=\"checkbox\" id=\"chkIsRequired_" + ctr + "\" class=\"IsRequiredDynamicFields\" title=\"IsRequired\">"
                + "    </div>"
                + "    <div class=\"col-md-1 text-align-center no-padding\">"
                + "        <input type=\"checkbox\" id=\"chkAllowBackflow_" + ctr + "\" class=\"AllowBackflowDynamicFields\" title=\"Allow Back Flow\">"
                + "    </div>"
                + "    <div class=\"col-md-1 no-padding\">"
                + "        <input type=\"text\" id=\"txtTATDays_" + ctr + "\" class=\"form-control TATDaysDynamicFields\" title=\"TAT Days\">"
                + "    </div>"
                + "    <div class=\"col-md-1 text-align-center no-padding\">"
                + "        <label id=\"btnApprover_" + ctr + "\">"
                + "           <span class=\"btn-glyph-dynamic glyphicon glyphicon-list\" title=\"Click to view approver role\"></span>"
                + "        </label>"
                + "    </div>"
                + "</div>"
            );

            $('input[type="checkbox"]').prop('disabled', true);
            $('.ResultTypeDynamicFields').prop('disabled', true);

            GenerateDropdownValues(ViewResultTypeDropDownURL, "ddlResultType_" + ctr, "Value", "Description", "", "", false);

            s.AddApproverRoleDynamicFields(ctr);

            $("#btnApprover_" + ctr).click(function () {
                $("#ApproverRoleDynamicFields_" + ctr).show();
                $("#divWorkflowApprovalModal").modal("show");
            });
        },

        AddApproverRoleDynamicFields: function (id) {

            $("#DivApproverRoleDynamicFields").append(
              "<div class=\"form-group form-fields ApproverRoleDynamicFields\" id=\"ApproverRoleDynamicFields_" + id + "\">"
              + "   <div class=\"col-md-5 listOne\">"
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
              + "   <div class=\"col-md-5 listTwo\">"
              + "       <input type=\"text\" id=\"\" class=\"form-control listbox txtSearchBoxTwo\" placeholder=\"Search here...\"/>"
              + "       <select id=\"ddlListTwo_" + id + "\" class=\"form-control listbox required-field RoleIDDynamicFields_" + id + "\" size=\"10\"></select>"
              + "   </div>"
              + "</div>"
            );

            $(".listOne, .listboxbutton").hide();
            $(".listTwo").removeClass("col-md-5");
            $(".listTwo").addClass("col-md-12");
            $("#divWorkflowApprovalModal .modal-dialog").attr("style", "width: 20%");
            $(".ApproverRoleDynamicFields").hide();

            $("#divWorkflowApprovalHeaderModal .close").click(function () {
                $("#ApproverRoleDynamicFields_" + id).hide();
            });
        },

        LoadWorkflowStep: function () {
            var s = this;
            $("#DivWorkflowStepDynamicFields").html("");

            var input = { handler: "Workflow", ID: objWorkflowViewJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddWorkflowStepDynamicFields();
                    s.AddApproverRoleDynamicFields(idCtr);
                    $("#txtOrder_" + idCtr).val(item.Order);
                    $("#txtStepCode_" + idCtr).val(item.StepCode);
                    $("#txtStepDescription_" + idCtr).val(item.StepDescription);
                    $("#ddlResultType_" + idCtr).val(item.ResultType);
                    $("#chkIsRequired_" + idCtr).val(item.IsRequired);
                    $("#chkIsRequired_" + idCtr).attr('checked', item.IsRequired);
                    $("#chkAllowBackflow_" + idCtr).val(item.AllowBackflow);
                    $("#chkAllowBackflow_" + idCtr).attr('checked', item.AllowBackflow);
                    $("#txtTATDays_" + idCtr).val(item.TATDays);

                    $("#txtOrder_" + idCtr).attr("readonly", true);
                    $("#txtStepCode_" + idCtr).attr("readonly", true);
                    $("#txtStepDescription_" + idCtr).attr("readonly", true);
                    $("#ddlResultType_" + idCtr).attr("readonly", true);
                    $("#chkIsRequired_" + idCtr).attr("readonly", true);
                    $("#chkAllowBackflow_" + idCtr).attr("readonly", true);
                    $("#txtTATDays_" + idCtr).attr("readonly", true);

                    //Populate selected Approver
                    $("#ddlListTwo_" + idCtr).empty();
                    $(item.WorkflowStepApproverList).each(function (idx1, subItem1) {
                        $(ApprovalRoleListBoxOptions).each(function (idx2, subItem2) {
                            if (subItem2.Value == subItem1.RoleID) {
                                $("#ddlListTwo_" + idCtr).append($('<option/>', {
                                  value: subItem2.Value,
                                  text: subItem2.Text
                                }));
                            }
                        });
                    });
                };
                var ctr = 1;
                $(data.Result.WorkflowStepList).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(WorkflowViewURL, input, "", GetSuccessFunction);
        },
    };

    objWorkflowViewJS.Initialize();
});