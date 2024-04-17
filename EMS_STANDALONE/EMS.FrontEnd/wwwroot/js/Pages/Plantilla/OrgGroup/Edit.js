var objOrgGroupEditJS;
var OrgGroupListBoxOptions = [];
var PositionLevelDropDownOptions = [];
var PositionPlannedOld = [];
var PositionPlannedNew = [];
//var ParentOrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgGroupAutoComplete";
var CheckFileIfExistsURL = window.location.pathname + "?handler=CheckFileIfExists";
var DownloadFileURL = window.location.pathname + "?handler=DownloadFile";


$(document).ready(function () {
    objOrgGroupEditJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divOrgGroupBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divOrgGroupModal .form-control").attr("readonly", false);
            $("#divOrgGroupModal #btnEdit").hide();
            $("#divOrgGroupModal #btnSave, #divOrgGroupModal #btnBack").show();
            s.GetOrgGroupListBoxOptions();
            //s.GetPositionLevelDropDownOptions();
            s.LoadChildrenOrgGroup();
            s.LoadOrgGroupPosition();
            s.LoadOrgGroupNPRF();
            $("#txtParentOrgGroup").change();
            //$("#ddlOrgType").change();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabOrgGroupPosition');
            /*if ($('#divOrgGroupTags').children().length == 0) {
                $(".tablinks:nth-child(3)").hide();
            }*/
            /*if ($('#ddlOrgType :selected').val() != "BRN" && $('#ddlOrgType :selected').val() != "DEPT") {
                $(".tablinks:nth-child(3)").hide();
            }*/
            if (parseInt($("#hdnOrgGroupTagCount").val()) == 0) {
                $("#ddlOrgType").change();
            }

            if ($('#ddlOrgType :selected').val() == "BRN")
                $(".isbranch").show();
            else
                $(".isbranch").hide();

            if ($('#ddlOrgType :selected').val() == "REG")
                $(".isregion").show();
            else
                $(".isregion").hide();
            /*if ($('#ddlOrgType :selected').val() == "BRN")
                $("#divBOMDAM").show();*/

            s.AddNPRFDynamicFields();
        },

        DeleteSuccessFunction: function () {
            $("#divOrgGroupTableList #btnSearch").click();
            $('#divOrgGroupModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#divOrgGroupTableList #btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;
            Code($("#txtCode"));
            NumberOnly($("#txtServiceBayCount"));
            PreventSpace($("#txtCode"));
            $('#txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });


            $("#txtCode").keyup(function () {
                $("#txtCode").val($("#txtCode").val().toUpperCase());
            });

            $("#divOrgGroupModal #txtBranchEmail").on("keyup", function () {
                $("#divOrgGroupErrorMessage").html("");

                if (!ValidEmail($("#divBranchInfoModal #txtBranchEmail"))) {
                    $("#divOrgGroupModal #txtBranchEmail").addClass("errMessage");
                    $("#divOrgGroupModal #txtBranchEmail").focus();
                    $("#divOrgGroupErrorMessage").append("<label class=\"errMessage\"><li>Branch Email is invalid</li></label><br />");
                }
                return false;
            });

            $('#ddlOrgType').change(function () {
                $("#divOrgGroupTags").html("");
                if ($(this).val() == "TOP") {
                    $('#reqParentGroup').hide();
                    $("#txtParentOrgGroup").removeClass("required-field");
                }
                else {
                    $('#reqParentGroup').show();
                    $("#txtParentOrgGroup").addClass("required-field");
                }


                if ($('#ddlOrgType :selected').val() == "BRN")
                    $(".isbranch").show();
                else
                    $(".isbranch").hide();

                if ($('#ddlOrgType :selected').val() == "REG")
                    $(".isregion").show();
                else
                    $(".isregion").hide();

                var GetSuccessFunction = function (data) {
                    if (data.Result.length != 0) {
                        $(data.Result).each(function (index, item) {
                            $("#divOrgGroupTags").append(
                                "<div class=\"form-group form-fields\"> \
                                  <div class=\"col-md-2-5 col-label\"> \
                                      <label class=\"control-label block-label\" for=\"ddl"+ item.Value + "\"> <span class=\"reqField\">* </span>" + item.Description + "</label> \
                                  </div> \
                                  <div class=\"col-md-4\"> \
                                      <input type=\"hidden\" id=\"hdnCode_"+ item.Value + "\" value=\"" + item.Value + "\" class=\"OrgGroupTagList_Code\"/> \
                                      <select id=\"ddl"+ item.Value + "\" title=\"" + item.Description + "\" class=\"form-control required-field OrgGroupTagList_Value\"> \
                                          <option value=\"\">- Select an item -</option> \
                                      </select> \
                                  </div> \
                               </div>");
                            $(item.DropDownOptions).each(function (index, itemOption) {
                                $("#ddl" + item.Value).append("<option value=\"" + itemOption.Value + "\">" + itemOption.Description + "</option>");
                            });
                        });
                    }
                };

                objEMSCommonJS.GetAjax(GetOrgGroupTagsByOrgGroupTypeURL + "ORG_BRN_TAGS", {}, "", GetSuccessFunction);
            });

            var GetAutoCompleteSuccessFunction = function (data) {
                if (data != "") {

                    var GetSuccessFunction = function (data) {

                        var length = data.Result.length;
                        var codeValue = $("#txtCode").val();
                        var descValue = $("#txtDescription").val();

                        $("#divHierarchyUpward").html("");

                        $(data.Result).each(function (idx, item) {
                            $("#divHierarchyUpward").append(item.Code + " - " + item.Description + " > ");
                        });

                        if (codeValue != "")
                            $("#divHierarchyUpward").append(codeValue + " - " + descValue);
                    };

                    objEMSCommonJS.GetAjax(OrgTypeHierarchyUpwardURL
                        + "&ID=" + data
                        , {}
                        , ""
                        , GetSuccessFunction, null, false);
                }

            };

            objEMSCommonJS.BindAutoComplete("divOrgGroupModal #txtParentOrgGroup"
                , ParentOrgGroupAutoCompleteURL
                , 20, "divOrgGroupModal #hdnParentOrgID", "ID", "Description", GetAutoCompleteSuccessFunction);
            
            objEMSCommonJS.BindAutoComplete("txtCsodAm"
                , ParentOrgGroupAutoCompleteURL
                , 20, "hdnCsodAm", "ID");

            objEMSCommonJS.BindAutoComplete("txtHRBP"
                , ParentOrgGroupAutoCompleteURL
                , 20, "hdnHRBP", "ID");

            objEMSCommonJS.BindAutoComplete("txtRRT"
                , ParentOrgGroupAutoCompleteURL
                , 20, "hdnRRT", "ID");

            $("#txtParentOrgGroup").change(function () {
                if ($("#txtParentOrgGroup").val() == "") {
                    $("#divHierarchyUpward").html("");
                }
                else {
                    var GetSuccessFunction = function (data) {

                        var length = data.Result.length;
                        var codeValue = $("#txtCode").val();
                        var descValue = $("#txtDescription").val();

                        $("#divHierarchyUpward").html("");

                        $(data.Result).each(function (idx, item) {
                            $("#divHierarchyUpward").append(item.Code + " - " + item.Description + " > ");
                        });

                        if (codeValue != "")
                            $("#divHierarchyUpward").append(codeValue + " - " + descValue);
                    };

                    objEMSCommonJS.GetAjax(OrgTypeHierarchyUpwardURL
                        + "&ID=" + $("#hdnParentOrgID").val()
                        , {}
                        , ""
                        , GetSuccessFunction, null, true);
                }
            });

            //$("#ddlParentOrgGroup").change(function () {
            //    if ($("#ddlParentOrgGroup").val() != "") {

            //        var GetSuccessFunction = function (data) {

            //            var length = data.Result.length;
            //            var codeValue = $("#txtCode").val();
            //            var descValue = $("#txtDescription").val();

            //            $("#divHierarchyUpward").html("");

            //            $(data.Result).each(function (idx, item) {
            //                $("#divHierarchyUpward").append(item.Code + " - " + item.Description + " > ");
            //            });

            //            if (codeValue != "")
            //                $("#divHierarchyUpward").append(codeValue + " - " + descValue);
            //        };

            //        objEMSCommonJS.GetAjax(OrgTypeHierarchyUpwardURL
            //            + "&ID=" + $("#ddlParentOrgGroup").val()
            //            , {}
            //            , ""
            //            , GetSuccessFunction);
            //    }
            //});

            //$("#lnkCreateRegion").click(function () {
            //    var isSuccessFunction = function () {
            //        $("#divRegionModal .close").click(function () {
            //            GenerateDropdownValues(EditRegionDropDownURL, "ddlRegion", "Value", "Text", "", "", false);
            //        });
            //    };
            //    LoadPartialSuccessFunction(RegionAddURL, "divRegionBodyModal", isSuccessFunction);
            //    $("#divRegionModal").modal("show");
            //});

            //$("#lnkCreatePosition").click(function () {
            //    LoadPartial(PositionAddURL, "divPositionBodyModal");
            //    $("#divPositionModal").modal("show");
            //});

            $("#btnMoveToListTwo").click(function () {
                ListBoxMove("ddlListOneOrgGroup", "ddlListTwoOrgGroup", false, true);
            });

            $("#btnMoveToListOne").click(function () {
                ListBoxMove("ddlListTwoOrgGroup", "ddlListOneOrgGroup", false, true);
            });

            $("#btnMoveToListTwoAll").click(function () {
                ListBoxMove("ddlListOneOrgGroup", "ddlListTwoOrgGroup", true, true);
            });

            $("#btnMoveToListOneAll").click(function () {
                ListBoxMove("ddlListTwoOrgGroup", "ddlListOneOrgGroup", true, true);
            });

            $("#txtSearchBoxOne").on("change keyup keydown", function () {
                ListBoxSearch("ddlListOneOrgGroup", $(this).val(), false);
            });

            $("#txtSearchBoxTwo").on("change keyup keydown", function () {
                ListBoxSearch("ddlListTwoOrgGroup", $(this).val(), false);
            });

            $("#btnAddChildrenManPowerFields").click(function () {
                //$(".PositionLevelDynamicFields").addClass("required-field");
                $(".PositionTextDynamicFields").addClass("required-field");
                $(".PlannedDynamicFields").addClass("required-field");
                $(".ActiveDynamicFields").addClass("required-field");
                $(".InactiveDynamicFields").addClass("required-field");
                var fields = $("#DivChildrenManPowerDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddChildrenManPowerDynamicFields(0, 0);
            });

            $("#btnAddNPRFFields").click(function () {
                $(".NPRFNumberDynamicFields").addClass("required-field");
                $(".DateApprovedDynamicFields").addClass("required-field");
                var fields = $("#DivNPRFDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddNPRFDynamicFields(0, 0);
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , OrgGroupDeleteURL + '?ID=' + objOrgGroupEditJS.ID\
                    , {} \
                    , '#divOrgGroupErrorMessage' \
                    , '#btnDelete' \
                    , objOrgGroupEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnSave").click(function () {

                if (objEMSCommonJS.ValidateBlankFields("#frmOrgGroup", "#divOrgGroupErrorMessage", objOrgGroupEditJS.ValidateDuplicateFields)) {

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , OrgGroupEditURL \
                        , objOrgGroupEditJS.GetFormData() \
                        , '#divOrgGroupErrorMessage' \
                        , '#btnSave' \
                        , objOrgGroupEditJS.EditSuccessFunction);", "function");
                }
                else {
                    if ($("#tabOrgGroupPosition .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabOrgGroupPosition');
                    }
                    else if ($("#tabOrgGroupTag .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks:nth-child(3)"), 'tabOrgGroupTag');
                    }
                    else if ($("#tabOrgGroupChildren .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks:nth-child(4)"), 'tabOrgGroupChildren');
                    }
                    else if ($("#tabOrgGroupNPRF .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks:nth-child(5)"), 'tabOrgGroupNPRF');
                    }
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(OrgGroupViewURL + "?ID=" + objOrgGroupEditJS.ID, "divOrgGroupBodyModal");
            });
        },

        GetFormData: function () {

            $(".Attachment_File").each(function (index) {
                $(this).prop("name", "OrgGroup.OrgGroupNPRFList[" + index + "].File");
            });

            var formData = new FormData($('#frmOrgGroup').get(0));

            $("#ddlListOneOrgGroup option").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.ChildrenOrgIDList[" + index + "]", $(this).val());
                }
            });

            $(".PositionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupPositionList[" + index + "].PositionID", $(this).val());
                }
            });

            $(".PlannedDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupPositionList[" + index + "].PlannedCount", $(this).val());
                }
            });
            $(".ActiveDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupPositionList[" + index + "].ActiveCount", $(this).val());
                }
            });
            $(".InactiveDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupPositionList[" + index + "].InactiveCount", $(this).val());
                }
            });
            $(".IsHeadDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupPositionList[" + index + "].IsHead", $(this).prop("checked"));
                }
            });

            $(".OrgGroupTagList_Code").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupTagList[" + index + "].Code", $(this).val());
                }
            });

            $(".OrgGroupTagList_Value").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupTagList[" + index + "].Value", $(this).val());
                }
            });

            $(".NPRFNumberDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupNPRFList[" + index + "].NPRFNumber", $(this).val());
                }
            });

            //$(".DateApprovedDynamicFields").each(function (index) {
            //    if ($(this).val() != "") {
            //        formData.append("OrgGroup.OrgGroupNPRFList[" + index + "].ApprovedDate", $(this).val());
            //    }
            //});

            $(".ReportingPositionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupPositionList[" + index + "].ReportingPositionID", $(this).val());
                }
            });

            return formData;

        },

        GetPositionLevelDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    PositionLevelDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(ViewPositionLevelDropDownURL, {}, "", GetSuccessFunction);
        },

        AddChildrenManPowerDynamicFields: function (PositionLevelID, PositionID) {
            var htmlId = $(".ChildrenManPowerDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenManPowerDynamicFields_", "")) + 1;
            //var PositionId = $('#ddlPosition_' + ctr).val();



            /*<td class=\"dynamic-field-header-custom text-align-right no-padding\"> \
                       <label id=\"txtInactive_" + ctr + "\" class=\"control-label text-amount total-label InactiveDynamicFields\" title=\"Inactive\" readonly></label>  \
                   </td> \*/

            $("#DivChildrenManPowerDynamicFields").append(
                "<tr class=\"form-group form-fields ChildrenManPowerDynamicFields\" id=\"ChildrenManPowerDynamicFields_" + ctr + "\"> \
                   <td class=\"dynamic-field-header-custom-header-no-border text-align-center no-padding\" style=\"width: 50px;\"> \
                       <span style=\"margin: auto;\" class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objOrgGroupEditJS.RemoveDynamicFields('#ChildrenManPowerDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-center no-padding\" style=\"width: 50px;\"> \
                       <input style=\"margin: auto;\" type=\"checkbox\" id=\"chkIsHead_" + ctr + "\" class=\"IsHeadDynamicFields\" title=\"Head\" readonly> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-center no-padding\" style=\"width: 300px;\"> \
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtPosition_" + ctr + "\" class=\"form-control PositionTextDynamicFields\" title=\"Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnPositionID_" + ctr + "\" class=\"form-control PositionDynamicFields\"> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-center no-padding\" style=\"width: 300px;\"> \
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtReportingPosition_" + ctr + "\" class=\"form-control ReportingPositionTextDynamicFields\" title=\"Reporting Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnReportingPositionID_" + ctr + "\" class=\"form-control ReportingPositionDynamicFields\"> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-right no-padding\"> \
                       <input type=\"text\" id=\"txtPlanned_" + ctr + "\" class=\"form-control text-amount PlannedDynamicFields\" title=\"Planned\"> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-right no-padding\"> \
                       <label id=\"txtActiveReg_" + ctr + "\" class=\"control-label text-amount total-label ActiveRegDynamicFields\" title=\"Active\" readonly></label>  \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-right no-padding\"> \
                       <label id=\"txtActiveProb_" + ctr + "\" class=\"control-label text-amount total-label ActiveProbDynamicFields\" title=\"Active\" readonly></label>  \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-right no-padding\"> \
                       <label id=\"txtActiveOut_" + ctr + "\" class=\"control-label text-amount total-label ActiveOutDynamicFields\" title=\"Active\" readonly></label>  \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-right no-padding\"> \
                       <label id=\"txtActiveTotal_" + ctr + "\" class=\"control-label text-amount total-label ActiveTotalDynamicFields\" title=\"Active\" readonly></label>  \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-right no-padding\"> \
                       <label id=\"txtVariance_" + ctr + "\" class=\"control-label text-amount total-label VarianceDynamicFields\"></label> \
                   </td> \
               </tr>"
            );

            NumberOnly($(".PlannedDynamicFields"));
            NumberOnly($(".ActiveDynamicFields"));
            NumberOnly($(".InactiveDynamicFields"));

            $("#txtActive_" + ctr + ", #txtInactive_" + ctr).val("0");

            $("#txtPlanned_" + ctr + ", " + "#txtActive_" + ctr + ", " + "#txtInactive_" + ctr).keyup(function () {
                var Variance = (parseInt($("#txtActiveReg_" + ctr).val() || 0) + parseInt($("#txtActiveProb_" + ctr).val() || 0)) - parseInt($("#txtPlanned_" + ctr).val() || 0);
                $("#txtVariance_" + ctr).val(Variance);
                $("#txtVariance_" + ctr).text(Variance);
                objOrgGroupListJS.CalculateTotal("VarianceDynamicFields", "txtTotalVariance");
            });

            $('.IsHeadDynamicFields').on('change', function () {
                $('.IsHeadDynamicFields').not(this).prop('checked', false);
                //$(this).val($(this).prop("checked") ? "true" : "false");
            });

            //$('#ddlPositionLevel_' + ctr).change(function () {
            //    //GenerateDropdownValues(EditPositionDropDownURL, "ddlPosition_" + ctr, "Value", "Text", "", "", false);
            //    GenerateDropdownValues(EditPositionByIDURL + "&PositionLevelID=" + $(this).val() +
            //        "&ID=0", "ddlPosition_" + ctr, "Value", "Text", "", "", false);
            //});

            $("#txtPlanned_" + ctr).keyup(function () {
                objOrgGroupListJS.CalculateTotal("PlannedDynamicFields", "txtTotalPlanned");
            });

            $("#txtActive_" + ctr).keyup(function () {
                objOrgGroupListJS.CalculateTotal("ActiveDynamicFields", "txtTotalActive");
            });

            $("#txtInactive_" + ctr).keyup(function () {
                objOrgGroupListJS.CalculateTotal("InactiveDynamicFields", "txtTotalInactive");
            });

            //objEMSCommonJS.PopulateDropDown("#ddlPositionLevel_" + ctr, PositionLevelDropDownOptions);
            //GenerateDropdownValues(EditPositionLevelDropDownURL, "ddlPositionLevel_" + ctr, "Value", "Text", "", "", false);
            //GenerateDropdownValues(EditPositionByIDURL + "&PositionLevelID=" + PositionLevelID +
            //    "&ID=" + PositionID, "ddlPosition_" + ctr, "Value", "Text", "", "", false);

            objEMSCommonJS.BindAutoComplete("txtPosition_" + ctr
                , PositionWithLevelAutoCompleteURL
                , 20, "hdnPositionID_" + ctr, "ID", "Description");

            objEMSCommonJS.BindAutoComplete("txtReportingPosition_" + ctr
                , PositionWithLevelAutoCompleteURL
                , 20, "hdnReportingPositionID_" + ctr, "ID", "Description");
        },

        RemoveDynamicFields: function (id, deleteAttachmentFunction) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
            if (deleteAttachmentFunction != null)
                deleteAttachmentFunction();
        },

        ValidateDuplicateFields: function () {
            var isValid = true;
            var isDuplicate = 0;
            var isParentDuplicate = 0;
            var parentValue = $("#hdnParentOrgID").val();
            var errCount = 0;
            var IsHead = 0;
            PositionPlannedNew = [];

            //BEFORE SAVE REQUIRED TO HAVE HEAD
            $("#divOrgGroupModal .IsHeadDynamicFields").each(function (num, index) {
                ctr = $(this).prop("id");
                if ($("#" + ctr).prop('checked') == true) {
                    IsHead++;
                }
            });
            if (IsHead != 1) {
                $("#divOrgGroupManpowerErrorMessage").html("<label class=\"errMessage\"><li>" + MSG_REQUIRED_ISHEAD + "</li></label><br />");
                isValid = false;
            }
            else
                $("#divOrgGroupManpowerErrorMessage").html('');

            $("#ddlListOneOrgGroup option").each(function (n1, x1) {
                if (this.value == parentValue && this.value != "") {
                    $("#txtParentOrgGroup").addClass("errMessage");
                    $("#ddlListOneOrgGroup").addClass("errMessage");
                    isParentDuplicate++;
                }
            });

            $("#divOrgGroupModal .PositionDynamicFields").each(function (n1, x1) {
                if (this.value != "") {
                    var currentVal = this.value;
                    var ctr = $(this).prop("id").replace("hdnPositionID_", "");
                    //$("#ChildrenManPowerDynamicFields_" + ctr + " #ddlPositionLevel_" + ctr).removeClass("errMessage");
                    $("#ChildrenManPowerDynamicFields_" + ctr + " #txtPosition_" + ctr).removeClass("errMessage");
                    $("#ChildrenManPowerDynamicFields_" + ctr + " #txtReportingPosition_" + ctr).removeClass("errMessage");
                    $("#ChildrenManPowerDynamicFields_" + ctr + " #txtPlanned_" + ctr).removeClass("errMessage");
                    $("#ChildrenManPowerDynamicFields_" + ctr + " #txtActive_" + ctr).removeClass("errMessage");
                    $("#ChildrenManPowerDynamicFields_" + ctr + " #txtInactive_" + ctr).removeClass("errMessage");
                    $(".PositionDynamicFields").not(this).filter(function () {
                        if (this.value == currentVal) {
                            //$("#ChildrenManPowerDynamicFields_" + ctr + " #ddlPositionLevel_" + ctr).addClass("errMessage");
                            $("#ChildrenManPowerDynamicFields_" + ctr + " #txtPosition_" + ctr).addClass("errMessage");
                            $("#ChildrenManPowerDynamicFields_" + ctr + " #txtReportingPosition_" + ctr).addClass("errMessage");
                            $("#ChildrenManPowerDynamicFields_" + ctr + " #txtPlanned_" + ctr).addClass("errMessage");
                            $("#ChildrenManPowerDynamicFields_" + ctr + " #txtActive_" + ctr).addClass("errMessage");
                            $("#ChildrenManPowerDynamicFields_" + ctr + " #txtInactive_" + ctr).addClass("errMessage");
                            isDuplicate++;
                        }
                    });

                    var plannedVal = $("#ChildrenManPowerDynamicFields_" + ctr + " #txtPlanned_" + ctr).val();
                    PositionPlannedNew.push(this.value + '-' + plannedVal);
                }
            });

            if (isParentDuplicate > 0) {
                $("#divOrgGroupErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_PARENT_ORG_GROUP + "</li></label><br />");
                isValid = false;
            }

            if (isDuplicate > 0) {
                $("#divOrgGroupErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }

            var plannedOld = PositionPlannedOld.sort().toString();
            var plannedNew = PositionPlannedNew.sort().toString();

            if (plannedOld != plannedNew) {
                $("#divOrgGroupNPRFErrorMessage").html("<label class=\"errMessage\"><li>" + MSG_REQUIRED_NPRF + "</li></label><br />");
                $("#divOrgGroupModal .NPRFNumberDynamicFields, .DateApprovedDynamicFields").addClass("errMessage");
                isValid = false;

                $("#divOrgGroupModal .NPRFNumberDynamicFields").each(function () {
                    var ctr = $(this).prop("id").replace("txtNPRFNumber_", "");
                    if (this.value != "")
                        $("#txtNPRFNumber_" + ctr).removeClass("errMessage");
                    else {
                        $("#txtNPRFNumber_" + ctr).addClass("errMessage");
                        errCount++;
                    }
                });

                //$("#divOrgGroupModal .DateApprovedDynamicFields").each(function () {
                //    var ctr = $(this).prop("id").replace("txtDateApproved_", "");
                //    if (this.value != "")
                //        $("#txtDateApproved_" + ctr).removeClass("errMessage");
                //    else {
                //        $("#txtDateApproved_" + ctr).addClass("errMessage"); 
                //        errCount++;
                //    } 
                //});

                $("#divOrgGroupModal .Attachment_File").each(function () {
                    var ctr = $(this).prop("id").replace("fileAttachment_", "");
                    if (this.value != "")
                        $("#fileAttachment_" + ctr).removeClass("errMessage");
                    else {
                        $("#fileAttachment_" + ctr).addClass("errMessage");
                        errCount++;
                    }
                });

                if (errCount == 0 && $('#divOrgGroupModal .NPRFNumberDynamicFields').length > 0) {
                    $("#divOrgGroupNPRFErrorMessage").html('');
                    isValid = true;
                }
            }
            else {
                $("#divOrgGroupNPRFErrorMessage").html('');
                $("#divOrgGroupModal .NPRFNumberDynamicFields, .DateApprovedDynamicFields").removeClass("errMessage");
            }


            if (!isValid)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return isValid;
        },

        GetOrgGroupListBoxOptions: function () {
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    OrgGroupListBoxOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(ViewOrgGroupDropDownURL, {}, "", GetSuccessFunction);
        },

        PopulateListBox: function (id, collection) {
            $(collection).each(function (index, item) {
                $(id).append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
        },

        LoadChildrenOrgGroup: function () {
            $('.listboxbutton, #txtSearchBoxTwo, #ddlListTwoOrgGroup').hide();
            $('#ddlListOneOrgGroup').prop('disabled', true);
            var s = this;
            var input = { ID: objOrgGroupEditJS.ID };

            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    //$('#ddlParentOrgGroup option').each(function () {
                    //    if ($(this).val() == item.Value) {
                    //        $("#ddlListTwoOrgGroup").append($('<option/>', {
                    //            value: $(this).val(),
                    //            text: $(this).text()
                    //        }));
                    //    }
                    //});
                    //$("#ddlListTwoOrgGroup").append($('<option/>', {
                    //    value: item.Value,
                    //    text: item.Text
                    //}));
                    $("#ddlListOneOrgGroup").append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });
                //s.PopulateListBox("#ddlListOneOrgGroup", OrgGroupListBoxOptions);
                //$("#ddlListTwoOrgGroup option").each(function () {
                //    $("#ddlListOneOrgGroup option[value='" + $(this).val() + "']").remove();
                //});
            };

            objEMSCommonJS.GetAjax(GetOrgGroupChildrenURL, input, "", GetSuccessFunction);
        },

        LoadOrgGroupPosition: function () {
            var s = this;
            $("#DivChildrenManPowerDynamicFields").html("");

            var input = { ID: objOrgGroupEditJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddChildrenManPowerDynamicFields(item.PositionLevelID, item.PositionID);

                    var Variance = (parseInt(item.ActiveCount) + parseInt(item.ActiveProbCount)) - parseInt(item.PlannedCount);
                    var VarianceLabel = (Variance > 0 ? "+" + Variance : Variance);

                    $("#txtInactive_" + idCtr).text(item.InactiveCount);
                    $("#txtInactive_" + idCtr).val(item.InactiveCount);
                    $("#txtPlanned_" + idCtr).val(item.PlannedCount);
                    $("#txtActiveReg_" + idCtr).text(item.ActiveCount);
                    $("#txtActiveReg_" + idCtr).val(item.ActiveCount);
                    $("#txtActiveProb_" + idCtr).text(item.ActiveProbCount);
                    $("#txtActiveProb_" + idCtr).val(item.ActiveProbCount);
                    $("#txtActiveOut_" + idCtr).text(item.OutgoingCount);
                    $("#txtActiveOut_" + idCtr).val(item.OutgoingCount);
                    $("#txtActiveTotal_" + idCtr).text(item.TotalActiveCount);
                    $("#txtActiveTotal_" + idCtr).val(item.TotalActiveCount);
                    $("#txtVariance_" + ctr).text(VarianceLabel);
                    $("#txtVariance_" + ctr).val(Variance);

                    if (Variance == 0)
                        $("#txtVariance_" + ctr).css("color", "#395B77");
                    else
                        $("#txtVariance_" + ctr).css("color", "red");

                    //$("#ddlPositionLevel_" + idCtr).val(item.PositionLevelID);
                    //$("#ddlPosition_" + idCtr).val(item.PositionID);
                    $("#chkIsHead_" + idCtr).attr('checked', item.IsHead);
                    $('#chkIsHead_' + ctr).val(item.IsHead);
                    $("#txtPosition_" + idCtr).val(item.PositionDescription);
                    $("#hdnPositionID_" + idCtr).val(item.PositionID);
                    $("#txtReportingPosition_" + idCtr).val(item.ReportingPositionDescription);
                    $("#hdnReportingPositionID_" + idCtr).val(item.ReportingPositionID);

                    objOrgGroupListJS.CalculateTotal("InactiveDynamicFields", "txtTotalInactive");
                    objOrgGroupListJS.CalculateTotal("PlannedDynamicFields", "txtTotalPlanned");
                    objOrgGroupListJS.CalculateTotal("ActiveRegDynamicFields", "txtTotalActiveReg");
                    objOrgGroupListJS.CalculateTotal("ActiveProbDynamicFields", "txtTotalActiveProb");
                    objOrgGroupListJS.CalculateTotal("ActiveOutDynamicFields", "txtTotalActiveOut");
                    objOrgGroupListJS.CalculateTotal("ActiveTotalDynamicFields", "txtTotalActive");
                    objOrgGroupListJS.CalculateTotal("VarianceDynamicFields", "txtTotalVariance");
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                    PositionPlannedOld.push(item.PositionID + '-' + item.PlannedCount);
                });
            };

            objEMSCommonJS.GetAjax(GetOrgGroupPositionURL, input, "", GetSuccessFunction);
        },

        LoadOrgGroupNPRF: function () {
            var s = this;
            $("#DivNPRFDynamicFields").html("");

            var input = { OrgGroupID: objOrgGroupEditJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {

                    $("#DivNPRFDynamicFields").append(
                        "<div class=\"form-group form-fields NPRFDynamicFields\" id=\"NPRFDynamicFields_" + idCtr + "\"> \
                           <div class=\"col-md-2-5 text-align-center no-padding\"> \
                               <input type=\"text\" id=\"txtNPRFNumber_" + idCtr + "\" value=\"" + item.NPRFNumber + "\" maxlength=\"50\" class=\"form-control\" title=\"NPRFNumber\" readonly> \
                           </div> \
                           <div class=\"col-md-3 no-padding\"> \
                               <label class=\"control-label block-label\">" + item.SourceFile + "</label> \
                               <input type=\"hidden\" id=\"txtServerFile_ " + idCtr + "\" value=\"" + item.ServerFile + "\" class=\"form-control\"> \
                               <input type=\"hidden\" id=\"txtSourceFile_ " + idCtr + "\" value=\"" + item.SourceFile + "\" class=\"form-control\"> \
                           </div> \
                           <div class=\"col-md-2-5 no-padding\"> \
                               <label class=\"control-label block-label\">" + item.UploadedBy + "<br> Timestamp: " + item.Timestamp + "</label> \
                           </div> \
                           <div class=\"col-md-0-5 no-padding\"> \
                               <span class=\"btn-glyph-dynamic glyphicon glyphicon-download-alt\" onclick=\"objEMSCommonJS.DownloadAttachment(CheckFileIfExistsURL, 'PlantillaService_OrgGroup_Attachment_Path','" + item.ServerFile + "', '" + item.SourceFile + "')\"></span> \
                           </div> \
                         </div>"
                    );

                    //var d = new Date(item.ApprovedDate);
                    //$("#txtNPRFNumber_" + idCtr).val(item.NPRFNumber);
                    //$("#txtDateApproved_" + idCtr).val(d.toLocaleString("en-US"));

                    //$("#txtDateApproved_" + idCtr).datetimepicker({
                    //    useCurrent: false,
                    //    format: 'MM/DD/YYYY'
                    //});
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetOrgGroupNPRFURL, input, "", GetSuccessFunction);
        },

        AddNPRFDynamicFields: function () {
            var htmlId = $(".NPRFDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("NPRFDynamicFields_", "")) + 1;

            $("#DivNPRFDynamicFields").append(
                "<div class=\"form-group form-fields NPRFDynamicFields\" id=\"NPRFDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-2-5 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtNPRFNumber_" + ctr + "\" maxlength=\"50\" class=\"form-control NPRFNumberDynamicFields\" title=\"NPRFNumber\"> \
                   </div> \
                   <div class=\"col-md-3 no-padding\"> \
                       <input type=\"file\" id=\"fileAttachment_" + ctr + "\" class=\"form-control Attachment_File\" title=\"Attachment\" accept=\".pdf,.doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document\"> \
                   </div> \
                   <div class=\"col-md-2-5 no-padding\"> \
                   </div> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                       <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objOrgGroupEditJS.RemoveDynamicFields('#NPRFDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                   </div> \
                 </div>"
            );

            //$(".DateApprovedDynamicFields").datetimepicker({
            //  useCurrent: false,
            //  format: 'MM/DD/YYYY'
            //});
        },

    };

    objOrgGroupEditJS.Initialize();
});