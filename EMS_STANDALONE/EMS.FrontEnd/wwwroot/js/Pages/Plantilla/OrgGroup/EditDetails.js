var objOrgGroupEditDetailsJS;
var PositionLevelDropDownOptions = [];
var PositionDropDownOptions = [];
var ParentOrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgGroupAutoComplete";
var CheckFileIfExistsURL = window.location.pathname + "?handler=CheckFileIfExists";
var DownloadFileURL = window.location.pathname + "?handler=DownloadFile";


$(document).ready(function () {
    objOrgGroupEditDetailsJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divOrgGroupBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $('#lnkCreateRegion, #lnkCreatePosition, #btnAddChildrenManPowerFields, #btnAddNPRFFields').hide();
            $('.DivDeleteChildrenOrgGroup').attr("style", "display: none !important"); //to by pass "display: flex !important;"
            $("#divOrgGroupModal .form-control").not('#txtAddress,#txtBranchEmail,#ddlBranchCategory,#txtBranchNumber').attr("disabled", true);
            $("label[for='txtAddress']").addClass("reqfield");
            $("#txtAddress,#txtBranchEmail,#ddlBranchCategory,#txtBranchNumber").addClass('required-field').attr('readonly', false);
            $("#divOrgGroupModal #btnEdit").hide();
            $("#divOrgGroupModal #btnSave, #divOrgGroupModal #btnBack").show();
            s.LoadChildrenOrgGroup();
            s.GetPositionLevelDropDownOptions();
            s.GetPositionDropDownOptions();
            s.LoadOrgGroupPosition();
            s.LoadHierarchyUpward();
            s.LoadOrgGroupNPRF();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabOrgGroupPosition');
            if ($('#divOrgGroupTags').children().length == 0) {
                $(".tablinks:nth-child(3)").hide();
            }
            if ($('#ddlOrgType :selected').val() != "BRN")
                $(".isbranch").hide();
        },

        DeleteSuccessFunction: function () {
            $("#divOrgGroupTableList #btnSearch").click();
            $('#divOrgGroupModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , OrgGroupDeleteURL + '?ID=' + objOrgGroupEditDetailsJS.ID\
                    , {} \
                    , '#divOrgGroupErrorMessage' \
                    , '#btnDelete' \
                    , objOrgGroupEditDetailsJS.DeleteSuccessFunction);",
                    "function");
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

            $("#btnEdit").click(function () {
                LoadPartial(OrgGroupEditURL + "?ID=" + objOrgGroupEditDetailsJS.ID, "divOrgGroupBodyModal");
            });

            $("#btnEditDetails").click(function () {
                LoadPartial(OrgGroupEditDetailsURL + "?ID=" + objOrgGroupEditDetailsJS.ID, "divOrgGroupBodyModal");
            });

            $("#txtSearchBoxOne").on("change keyup keydown", function () {
                ListBoxSearch("ddlListOneOrgGroup", $(this).val(), false);
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmOrgGroup", "#divOrgGroupErrorMessage", objOrgGroupEditDetailsJS.ValidateDuplicateFields)) {

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , OrgGroupEditDetailsURL \
                        , objOrgGroupEditDetailsJS.GetFormData() \
                        , '#divOrgGroupErrorMessage' \
                        , '#btnSave' \
                        , objOrgGroupEditDetailsJS.EditSuccessFunction);", "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(OrgGroupViewURL + "?ID=" + objOrgGroupEditDetailsJS.ID, "divOrgGroupBodyModal");
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

        EditSuccessFunction: function () {
            $("#divOrgGroupTableList #btnSearch").click();
            $("#btnBack").click();
        },

        AddChildrenManPowerDynamicFields: function () {
            var htmlId = $(".ChildrenManPowerDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenManPowerDynamicFields_", "")) + 1;
            var PositionId = $('#ddlPosition_' + ctr).val();

            $("#DivChildrenManPowerDynamicFields").append(
                "<div class=\"form-group form-fields ChildrenManPowerDynamicFields\" id=\"ChildrenManPowerDynamicFields_" + ctr + "\"> \
                  <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input type=\"checkbox\" id=\"chkIsHead_" + ctr + "\" class=\"IsHeadDynamicFields\" title=\"Head\" readonly> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtPosition_" + ctr + "\" class=\"form-control PositionTextDynamicFields\" title=\"Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnPositionID_" + ctr + "\" class=\"form-control PositionDynamicFields\"> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtReportingPosition_" + ctr + "\" class=\"form-control ReportingPositionTextDynamicFields\" title=\"Reporting Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnReportingPositionID_" + ctr + "\" class=\"form-control ReportingPositionDynamicFields\"> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtPlanned_" + ctr + "\" class=\"form-control text-amount PlannedDynamicFields\" title=\"Planned\"> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtActive_" + ctr + "\" class=\"form-control text-amount ActiveDynamicFields\" title=\"Active\" readonly> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtInactive_" + ctr + "\" class=\"form-control text-amount InactiveDynamicFields\" title=\"Inactive\" readonly> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-right no-padding\"> \
                       <label id=\"txtVariance_" + ctr + "\" class=\"control-label block-label remarks-label text-amount total-label VarianceDynamicFields\"></label> \
                   </div> \
               </div>"
            );
            $('#DivChildrenManPowerDynamicFields input[type="checkbox"], select').prop('disabled', true);

            //GenerateDropdownValues(ViewPositionLevelDropDownURL, "ddlPositionLevel_" + ctr, "Value", "Text", "", "", false);
            //GenerateDropdownValues(ViewPositionByIDURL, "ddlPosition_" + ctr, "Value", "Text", "", "", false);
            //objEMSCommonJS.PopulateDropDown("#ddlPositionLevel_" + ctr, PositionLevelDropDownOptions);
            //objEMSCommonJS.PopulateDropDown("#ddlPosition_" + ctr, PositionDropDownOptions);
        },

        LoadChildrenOrgGroup: function () {
            $('.listboxbutton, #txtSearchBoxTwo, #ddlListTwoOrgGroup').hide();

            var input = { ID: objOrgGroupEditDetailsJS.ID };

            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    //$('#ddlParentOrgGroup option').each(function () {
                    //    if ($(this).val() == item.Value) {
                    //        $("#ddlListOneOrgGroup").append($('<option/>', {
                    //            value: $(this).val(),
                    //            text: $(this).text()
                    //        }));
                    //    }
                    //});
                    $("#ddlListOneOrgGroup").append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });
            };

            objEMSCommonJS.GetAjax(GetOrgGroupChildrenURL, input, "", GetSuccessFunction);
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

        GetPositionDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    PositionDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(ViewPositionByIDURL, {}, "", GetSuccessFunction);
        },

        LoadOrgGroupPosition: function () {
            var s = this;
            $("#DivChildrenManPowerDynamicFields").html("");

            var input = { ID: objOrgGroupEditDetailsJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddChildrenManPowerDynamicFields();
                    $("#txtPlanned_" + idCtr).val(item.PlannedCount);
                    $("#txtActive_" + idCtr).val(item.ActiveCount);
                    $("#txtVariance_" + idCtr).val((parseInt(item.ActiveCount) + parseInt(item.InactiveCount)) - parseInt(item.PlannedCount));
                    $("#txtVariance_" + ctr).text((parseInt(item.ActiveCount) + parseInt(item.InactiveCount)) - parseInt(item.PlannedCount));
                    $("#txtInactive_" + idCtr).val(item.InactiveCount);
                    //$("#ddlPositionLevel_" + idCtr).val(item.PositionLevelID);
                    $("#txtPosition_" + idCtr).val(item.PositionDescription);
                    $("#hdnPositionID_" + idCtr).val(item.PositionID);
                    $("#txtReportingPosition_" + idCtr).val(item.ReportingPositionDescription);
                    $("#hdnReportingPositionID_" + idCtr).val(item.ReportingPositionID);
                    $("#chkIsHead_" + idCtr).attr('checked', item.IsHead);

                    $("#txtPlanned_" + idCtr).attr("readonly", true);
                    $("#txtActive_" + idCtr).attr("readonly", true);
                    $("#txtInactive_" + idCtr).attr("readonly", true);
                    //$("#ddlPositionLevel_" + idCtr).attr("readonly", true);
                    //$("#ddlPosition_" + idCtr).attr("readonly", true);
                    $("#chkIsHead_" + idCtr).attr("readonly", true);
                    $("#txtPosition_" + idCtr).attr("readonly", true);
                    $("#txtReportingPosition_" + idCtr).attr("readonly", true);

                    objOrgGroupListJS.CalculateTotal("PlannedDynamicFields", "txtTotalPlanned");
                    objOrgGroupListJS.CalculateTotal("ActiveDynamicFields", "txtTotalActive");
                    objOrgGroupListJS.CalculateTotal("InactiveDynamicFields", "txtTotalInactive");
                    objOrgGroupListJS.CalculateTotal("VarianceDynamicFields", "txtTotalVariance");
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetOrgGroupPositionURL, input, "", GetSuccessFunction);
        },

        LoadHierarchyUpward: function () {

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
                , GetSuccessFunction);
        },

        LoadOrgGroupNPRF: function () {
            var s = this;
            $("#DivNPRFDynamicFields").html("");

            var input = { OrgGroupID: objOrgGroupEditDetailsJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {

                    $("#DivNPRFDynamicFields").append(
                        "<div class=\"form-group form-fields NPRFDynamicFields\" id=\"NPRFDynamicFields_" + idCtr + "\"> \
                           <div class=\"col-md-2-5 text-align-center no-padding\"> \
                               <input type=\"text\" id=\"txtNPRFNumber_" + idCtr + "\" value=\"" + item.NPRFNumber + "\" class=\"form-control\" title=\"NPRFNumber\" readonly> \
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

    };

    objOrgGroupEditDetailsJS.Initialize();
});