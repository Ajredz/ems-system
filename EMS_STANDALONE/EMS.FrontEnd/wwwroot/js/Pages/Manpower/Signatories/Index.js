var objFormJS;

const UserNameAutoCompleteURL = "/ManPower/Signatories?handler=UserNameAutoComplete";
const SystemRoleDropDown = "/ManPower/Signatories?handler=SystemRoleDropDown";
const GetPositionLevelDropDownURL = "/ManPower/Signatories?handler=PositionLevelDropDown";
const GetPositionDropDownURL = "/ManPower/Signatories?handler=PositionDropDown";
const CommonPositionDropdownURL = "/ManPower/Signatories?handler=PositionDropdown";
const CommonApproversListURL = "/ManPower/Signatories?handler=Signatories";
const SignatoriesPostURL = "/ManPower/Signatories?handler=Submit";
var SystemRoleDropDownOptions  = [];

$(document).ready(function () {
    objFormJS = {

        Initialize: function () {
            var s = this;
            GenerateDropdownValues(GetPositionLevelDropDownURL, "ddlPositionLevel", "Value", "Text", "", "", false);
            s.GetSystemRoleDropDownOptions();
            s.ElementBinding();

        },

        ElementBinding: function () {
            var s = this;
            $("#txtRequesterUserName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: UserNameAutoCompleteURL, // URL 
                        type: "GET",
                        dataType: "json",
                        data: {
                            Term: $("#txtRequesterUserName").val(),
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
                        $("#hdnRequesterID").val(0)
                        $(this).val("");
                    } else {
                        $("#hdnRequesterID").val(ui.item.value)
                        $(this).val(ui.item.label)
                        if ($("#ddlPosition").val() != "") {
                            //get saved approvers
                            objFormJS.LoadApprovers();
                        }
                    }
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            $('#ddlPositionLevel').change(function () {
                GenerateDropdownValues(GetPositionDropDownURL + "&PositionLevelID=" + $('#ddlPositionLevel').val(), "ddlPosition", "Value", "Text", "", "", false);
            });

            $("#ddlPosition").change(function () {
                if ($("#ddlPosition").val() == "") {
                }
                else {
                    if ($("#hdnRequesterID").val() != 0) {
                        //get saved approvers
                        objFormJS.LoadApprovers();
                    }
                }

            });

            $("#btnAddApproversFields").click(function () {
                var fields = $("#DivApproverDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddApproverDynamicFields();
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmMRFSignatories", "#divMRFSignatoriesErrorMessage", objFormJS.ValidateDuplicateFields)) {

                    $(".Signatories_IDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].ID");
                    });
                    $(".Signatories_WorkflowIDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].WorkflowID");
                    });
                    $(".Signatories_WorkflowStepIDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].WorkflowStepID");
                    });
                    $(".Signatories_WorkflowStepCodeDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].WorkflowStepCode");
                    });
                    $(".Signatories_WorkflowStepApproverIDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].WorkflowStepApproverID");
                    });
                    $(".Signatories_RequesterIDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].RequesterID");
                    });
                    
                    $(".Signatories_ApproverRoleIDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].SystemRole.ID");
                    });
                    $(".Signatories_ApproverRoleNameDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].SystemRole.RoleName");
                    });

                    $(".Signatories_PositionIDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].PositionID");
                    });

                    $(".Signatories_Position_IDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].Position.ID");
                    });
                    $(".Signatories_Position_CodeDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].Position.Code");
                    });
                    $(".Signatories_Position_TitleDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].Position.Title");
                    });
                    $(".Signatories_Position_PositionLevelIDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].Position.PositionLevelID");
                    });

                    $(".Signatories_PositionLevel_IDDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].PositionLevel.ID");
                    });
                    $(".Signatories_PositionLevel_DescriptionDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].PositionLevel.Description");
                    });

                    $(".Signatories_ApproverRoleDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].ApproverRoleID");
                    });
                    $(".Signatories_ApproverDescriptionDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].ApproverDescription");
                    });
                    $(".Signatories_TATDaysDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].TATDays");
                    });
                    $(".Signatories_OrderDynamicFields").each(function (index) {
                        $(this).prop("name", "MRFSignatory[" + index + "].Order");
                    });

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SignatoriesPostURL \
                        , new FormData($('#frmMRFSignatories').get(0)) \
                        , '#divMRFSignatoriesErrorMessage' \
                        , '#btnSave' \
                        , objFormJS.SaveSuccessFunction);", "function");
                }
            });

        },

        GetSystemRoleDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    SystemRoleDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
                s.AddApproverDynamicFields();
            };

            objEMSCommonJS.GetAjax(SystemRoleDropDown, {}, "", GetSuccessFunction);
        },

        SaveSuccessFunction: function () {
            objFormJS.LoadApprovers();
        },

        AddApproverDynamicFields: function () {
            var s = this;
            var htmlId = $(".ApproverDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ApproverDynamicFields_", "")) + 1;

            //get order from last row
            var order = ctr != 1 ? parseInt($("#txtOrder_" + (ctr - 1)).val()) + 1 : 1;
            $("#DivApproverDynamicFields").append(
                "<div class=\"form-group form-fields ApproverDynamicFields\" id=\"ApproverDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-3 no-padding\">"
                + "        <input type=\"hidden\" id=\"hdnID_" + ctr + "\" value=\"" + $("#hdnID").val() + "\" class=\"Signatories_IDDynamicFields\">"
                + "        <input type=\"hidden\" id=\"hdnWorkflowID_" + ctr + "\" value=\"" + $("#hdnWorkflowID").val() + "\" class=\"Signatories_WorkflowIDDynamicFields\">"
                + "        <input type=\"hidden\" id=\"hdnRequesterID_" + ctr + "\" value=\"" + $("#hdnRequesterID").val() + "\" class=\"Signatories_RequesterIDDynamicFields\">"
                + "        <input type=\"hidden\" id=\"hdnPositionID_" + ctr + "\" value=\"" + $("#ddlPosition").val() + "\" class=\"Signatories_PositionIDDynamicFields\">"

                //+ "        <input type=\"hidden\" id=\"hdn_Position_ID_" + ctr + "\" value=\"" + (positionDetails  == "" ? "" : positionDetails.ID) + "\" class=\"Signatories_Position_IDDynamicFields\">"
                //+ "        <input type=\"hidden\" id=\"hdn_Position_Code_" + ctr + "\" value=\"" + (positionDetails  == "" ? "" : positionDetails.Code) + "\" class=\"Signatories_Position_CodeDynamicFields\">"
                //+ "        <input type=\"hidden\" id=\"hdn_Position_Title" + ctr + "\" value=\"" + (positionDetails == "" ? "" : positionDetails.Title) + "\" class=\"Signatories_Position_TitleDynamicFields\">"
                //+ "        <input type=\"hidden\" id=\"hdn_Position_PositionLevelID_" + ctr + "\" value=\"" + (positionDetails == "" ? "" : positionDetails.PositionLevelID) + "\" class=\"Signatories_Position_PositionLevelIDDynamicFields\">"

                //+ "        <input type=\"hidden\" id=\"hdn_PositionLevel_ID_" + ctr + "\" value=\"" + $("#ddlPositionLevel").val() + "\" class=\"Signatories_PositionLevel_IDDynamicFields\">"
                //+ "        <input type=\"hidden\" id=\"hdn_PositionLevel_Description_" + ctr + "\" value=\"" + $("#ddlPositionLevel option:selected").text() + "\" class=\"Signatories_PositionLevel_DescriptionDynamicFields\">"

                + "        <input type=\"hidden\" id=\"hdnWorkflowStepID_" + ctr + "\" class=\"Signatories_WorkflowStepIDDynamicFields\">"
                + "        <input type=\"hidden\" id=\"hdnWorkflowStepCode_" + ctr + "\" class=\"Signatories_WorkflowStepCodeDynamicFields\">"
                + "        <input type=\"hidden\" id=\"hdnWorkflowStepApproverID_" + ctr + "\" class=\"Signatories_WorkflowStepApproverIDDynamicFields\">"

                + "        <select id=\"ddlApproverRole_" + ctr + "\" class= \"form-control required-field Signatories_ApproverRoleDynamicFields\" title=\"Approver Role\"></select >"
                //+ "        <input type=\"hidden\" id=\"hdnRoleID_" + ctr + "\" class=\"Signatories_ApproverRoleIDDynamicFields\">"
                //+ "        <input type=\"hidden\" id=\"hdnRoleName_" + ctr + "\" class=\"Signatories_ApproverRoleNameDynamicFields\">"
                + "    </div>"
                + "    <div class=\"col-md-4 no-padding\">"
                + "        <input id=\"txtApproverDescription_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field Signatories_ApproverDescriptionDynamicFields\" title=\"Approver Description\">"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <input id=\"txtTATDays_" + ctr + "\" maxlength=\"15\" class=\"form-control required-field Signatories_TATDaysDynamicFields\" title=\"TAT Days\">"
                + "    </div>"
                + "    <div class=\"col-md-1 no-padding\">"
                + "        <input id=\"txtOrder_" + ctr + "\" value=\"" + order + "\" class=\"form-control Signatories_OrderDynamicFields text-amount\" maxlength=\"17\" title=\"Order\" readonly>"
                + "    </div>"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objFormJS.RemoveDynamicFields('#ApproverDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
                + "    </div>"
                + "</div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlApproverRole_" + ctr, SystemRoleDropDownOptions);

            $("#ddlApproverRole_" + ctr).change(function () {
                $("#hdnRoleID_" + ctr).val($("#ddlApproverRole_" + ctr).val());
                $("#hdnRoleName_" + ctr).val($("#ddlApproverRole_" + ctr + " option:selected").text());
            });

            NumberOnly($("#txtTATDays_" + ctr));
        },

        RemoveDynamicFields: function (id) {
            var ctrToDelete = parseInt(id == undefined ? 0 : id.replace("#ApproverDynamicFields_", ""));
            $(".ApproverDynamicFields").each(function () {
                var ctr = $(this).prop("id").replace("ApproverDynamicFields_", "");
                if (ctrToDelete < ctr)
                    $("#txtOrder_" + ctr).val($("#txtOrder_" + ctr).val() - 1);
            });

            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },

        ValidateDuplicateFields: function (additionalFunction) {
            var isValid = true;

            var isDuplicate = 0;

            $(".Signatories_ApproverRoleDynamicFields").each(function (n1, x1) {
                if (this.value != "") {
                    var currentVal = this.value;
                    var ctr = $(this).prop("id").replace("ddlApproverRole_", "");
                    $("#ApproverDynamicFields_" + ctr + " input").removeClass("errMessage");
                    $("#ApproverDynamicFields_" + ctr + " select").removeClass("errMessage");
                    $(".Signatories_ApproverRoleDynamicFields").not(this).filter(function () {
                        if (this.value == currentVal) {
                            $("#ApproverDynamicFields_" + ctr + " input").addClass("errMessage");
                            $("#ApproverDynamicFields_" + ctr + " select").addClass("errMessage");
                            isDuplicate++;
                        }
                    });
                }
            });

            if (isDuplicate > 0) {
                $("#divMRFSignatoriesErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }

            if (!isValid)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return isValid;
        },

        LoadApprovers: function () {
            Loading(true);
            var s = this;

            $("#DivApproverDynamicFields").html("");

            var input = {
                UserID: $("#hdnRequesterID").val()
                , PositionID: $("#ddlPosition").val()
            }

            var GetSuccessFunction = function (data) {
                var ctr = 1;
                var populateFields = function (item, idCtr) {
                    s.AddApproverDynamicFields();
                    if (idCtr == 1) {
                        $("#hdnID").val(item.ID);
                        $("#hdnWorkflowID").val(item.WorkflowID);
                    }

                    //$("#hdn_Position_ID_" + idCtr).val(item.Position.ID);
                    //$("#hdn_Position_Code_" + idCtr).val(item.Position.Code);
                    //$("#hdn_Position_Title_" + idCtr).val(item.Position.Title);
                    //$("#hdn_Position_PositionLevelID_" + idCtr).val(item.Position.PositionLevelID);

                    //$("#hdn_PositionLevel_ID_" + idCtr).val(item.PositionLevel.ID);
                    //$("#hdn_PositionLevel_Description_" + idCtr).val(item.PositionLevel.Description);

                    $("#hdnWorkflowStepID_" + idCtr).val(item.WorkflowStepID);
                    $("#hdnWorkflowStepCode_" + idCtr).val(item.WorkflowStepCode);
                    $("#hdnWorkflowStepApproverID_" + idCtr).val(item.WorkflowStepApproverID);
                    $("#ddlApproverRole_" + idCtr).val(item.ApproverRoleID);
                    //$("#hdnRoleID_" + idCtr).val(item.ApproverRoleID);
                    //$("#hdnRoleName_" + idCtr).val(item.ApproverRoleName);
                    $("#txtApproverDescription_" + idCtr).val(item.ApproverDescription);
                    $("#txtTATDays_" + idCtr).val(item.TATDays);
                    $("#txtOrder_" + idCtr).val(item.Order);
                };

                if (data.Result.length > 0) {
                    $(data.Result).each(function (index, item) {
                        populateFields(item, ctr); ctr++;
                    });
                }
                else {
                    s.AddApproverDynamicFields();
                }
            };

            objEMSCommonJS.GetAjax(CommonApproversListURL, input, "", GetSuccessFunction);
        },
    };
    
    objFormJS.Initialize();
});