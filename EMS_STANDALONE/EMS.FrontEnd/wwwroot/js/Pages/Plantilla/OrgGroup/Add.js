var objOrgGroupAddJS;
//var PositionLevelDropDownOptions = [];
//var ParentOrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgGroupAutoComplete";
//var PositionWithLevelAutoCompleteURL = window.location.pathname + "?handler=PositionWithLevelByAutoComplete";

$(document).ready(function () {
    objOrgGroupAddJS = {

        Initialize: function () {
            $("#divOrgGroupBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divOrgGroupModal #btnSave").show();
            $("#divOrgGroupModal .form-control:not(.TotalFields)").attr("readonly", false);
            $("#divOrgGroupModal #btnDelete, #divOrgGroupModal #btnBack").remove();
            $(".tablinks:nth-child(2)").hide();
            $(".tablinks:nth-child(5)").hide();

            //s.GetChildOrgGroupDropDownOptions();
            //s.GetPositionLevelDropDownOptions();
            s.AddChildrenManPowerDynamicFields();
            s.LoadChildrenOrgGroup();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabOrgGroupPosition');
        },

        AddSuccessFunction: function () {
            $("#divOrgGroupTableList #btnSearch").click();
            $("#frmOrgGroup").trigger("reset");
            $('#ddlListTwoOrgGroup').empty();
            $("#divOrgGroupTags").html("");
            $(".tablinks:nth-child(3)").hide();
            $("#DivChildrenManPowerDynamicFields").empty();
            objOrgGroupAddJS.LoadChildrenOrgGroup();
            objOrgGroupAddJS.AddChildrenManPowerDynamicFields();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabOrgGroupPosition');
            $("#divHierarchyUpward").html("");
            $(".TotalFields, .VarianceDynamicFields").empty();
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
                                    <label class=\"control-label block-label\" for=\"ddl"+ item.Value + "\"> <span class=\"reqField\">* </span>" + item.Description +"</label> \
                                </div> \
                                <div class=\"col-md-4\"> \
                                    <input type=\"hidden\" id=\"hdnCode_"+ item.Value + "\" value=\"" + item.Value + "\" class=\"OrgGroupTagList_Code\"/> \
                                    <select id=\"ddl"+ item.Value + "\" title=\"" + item.Description +"\" class=\"form-control required-field OrgGroupTagList_Value\"> \
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

            var GetAutoCompleteSuccessFunction = function (ParentID) {
                if (ParentID != "") {

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
                        + "&ID=" + ParentID
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

            $("#txtParentOrgGroup").change(function () {
                if ($("#txtParentOrgGroup").val() == "") {
                    $("#divHierarchyUpward").html("");
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
                    s.AddChildrenManPowerDynamicFields();
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmOrgGroup", "#divOrgGroupErrorMessage", objOrgGroupAddJS.ValidateDuplicateFields)) {

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , OrgGroupAddPostURL \
                        , objOrgGroupAddJS.GetFormData() \
                        , '#divOrgGroupErrorMessage' \
                        , '#btnSave' \
                        , objOrgGroupAddJS.AddSuccessFunction); ",
                        "function");
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
                }
            });
        },

        GetFormData: function () {

            var formData = new FormData($('#frmOrgGroup').get(0));

            $("#ddlListTwoOrgGroup option").each(function (index) {
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

            $(".ReportingPositionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.OrgGroupPositionList[" + index + "].ReportingPositionID", $(this).val());
                }
            });


            return formData;

        },

        //GetPositionLevelDropDownOptions: function () {
        //    var s = this;
        //    var GetSuccessFunction = function (data) {
        //        $(data.Result).each(function (index, item) {
        //            PositionLevelDropDownOptions.push(
        //                {
        //                    Value: item.Value,
        //                    Text: item.Text
        //                });
        //        });
        //        s.AddChildrenManPowerDynamicFields();
        //    };

        //    objEMSCommonJS.GetAjax(ViewPositionLevelDropDownURL, {}, "", GetSuccessFunction);
        //},

        AddChildrenManPowerDynamicFields: function () {
            var s = this;
            var htmlId = $(".ChildrenManPowerDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenManPowerDynamicFields_", "")) + 1;
            var PositionLevelId = 0;


            /*<td class=\"dynamic-field-header-custom text-align-right no-padding\"> \
                       <label id=\"txtInactive_" + ctr + "\" class=\"control-label text-amount total-label InactiveDynamicFields\" title=\"Inactive\" readonly></label>  \
                   </td> \*/

            $("#DivChildrenManPowerDynamicFields").append(
                "<tr class=\"form-group form-fields ChildrenManPowerDynamicFields\" id=\"ChildrenManPowerDynamicFields_" + ctr + "\"> \
                   <td class=\"dynamic-field-header-custom-header-no-border text-align-center no-padding\" style=\"width: 50px;\"> \
                       <span  style=\"margin: auto;\"class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objOrgGroupAddJS.RemoveDynamicFields('#ChildrenManPowerDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-center no-padding\"  style=\"width: 50px;\"> \
                       <input style=\"margin: auto;\" type=\"checkbox\" id=\"chkIsHead_" + ctr + "\" class=\"IsHeadDynamicFields\" title=\"Head\" readonly> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-center no-padding\" style=\"width: 150px;\"> \
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtPosition_" + ctr + "\" class=\"form-control PositionTextDynamicFields\" title=\"Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnPositionID_" + ctr + "\" class=\"form-control PositionDynamicFields\"> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-center no-padding\" style=\"width: 150px;\"> \
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtReportingPosition_" + ctr + "\" class=\"form-control ReportingPositionTextDynamicFields\" title=\"Reporting Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnReportingPositionID_" + ctr + "\" class=\"form-control ReportingPositionDynamicFields\"> \
                   </td> \
                   <td class=\"dynamic-field-header-custom text-align-center no-padding\"> \
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
                       <label id=\"txtVariance_" + ctr + "\" class=\"control-label remarks-label text-amount total-label VarianceDynamicFields\"></label> \
                   </td> \
               </tr>"
            );

            NumberOnly($(".PlannedDynamicFields"));
            NumberOnly($(".ActiveDynamicFields"));
            NumberOnly($(".InactiveDynamicFields"));

            $("#txtActive_" + ctr + ", #txtInactive_" + ctr).val("0");

            $("#txtPlanned_" + ctr + ", " + "#txtActive_" + ctr + ", " + "#txtInactive_" + ctr).keyup(function () {
                $("#txtVariance_" + ctr).val((parseInt($("#txtActive_" + ctr).val() || 0) + parseInt($("#txtInactive_" + ctr).val() || 0)) - parseInt($("#txtPlanned_" + ctr).val() || 0));
                $("#txtVariance_" + ctr).text((parseInt($("#txtActive_" + ctr).val() || 0) + parseInt($("#txtInactive_" + ctr).val() || 0)) - parseInt($("#txtPlanned_" + ctr).val() || 0));
                objOrgGroupListJS.CalculateTotal("VarianceDynamicFields", "txtTotalVariance");
            });

            $('.IsHeadDynamicFields').on('change', function () {
                $('.IsHeadDynamicFields').not(this).prop('checked', false);
                //$('#chkIsHead_' + ctr).val("true");
            });

            //$('#ddlPositionLevel_' + ctr).change(function () {
            //    PositionLevelId = $('#ddlPositionLevel_' + ctr).val();
            //    GenerateDropdownValues(PositionDropDownURL + "&PositionLevelID=" + PositionLevelId, "ddlPosition_" + ctr, "Value", "Text", "", "", false);
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

            objEMSCommonJS.BindAutoComplete("txtPosition_" + ctr
                , PositionWithLevelAutoCompleteURL
                , 20, "hdnPositionID_" + ctr, "ID", "Description");

            objEMSCommonJS.BindAutoComplete("txtReportingPosition_" + ctr
                , PositionWithLevelAutoCompleteURL
                , 20, "hdnReportingPositionID_" + ctr, "ID", "Description");
        },

        RemoveDynamicFields: function (id) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },

        ValidateDuplicateFields: function () {
            var isValid = true;
            var isDuplicate = 0;
            var isParentDuplicate = 0;
            var parentValue = $("#hdnParentOrgID").val();

            $("#ddlListTwoOrgGroup option").each(function (n1, x1) {
                if (this.value == parentValue && this.value != "") {
                    $("#txtParentOrgGroup").addClass("errMessage");
                    $("#ddlListTwoOrgGroup").addClass("errMessage");
                    isParentDuplicate++;
                }
            });

            $(".PositionDynamicFields").each(function (n1, x1) {
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

            if (!isValid)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return isValid;
        },

        LoadChildrenOrgGroup: function () {

            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    $("#ddlListOneOrgGroup").append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });
            };

            objEMSCommonJS.GetAjax(GetOrgGroupChildrenURL, {}, "", GetSuccessFunction);
        },

    };

    objOrgGroupAddJS.Initialize();
});