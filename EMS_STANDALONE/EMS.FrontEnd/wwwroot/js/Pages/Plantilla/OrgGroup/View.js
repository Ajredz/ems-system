var objOrgGroupViewJS;
var PositionLevelDropDownOptions = [];
var PositionDropDownOptions = [];
var ParentOrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgGroupAutoComplete";
var CheckFileIfExistsURL = window.location.pathname + "?handler=CheckFileIfExists";
var DownloadFileURL = window.location.pathname + "?handler=DownloadFile";


$(document).ready(function () {
    objOrgGroupViewJS = {

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
            $('#divOrgGroupModal input[type="checkbox"], #divOrgGroupModal select').prop('disabled', true);


            s.LoadChildrenOrgGroup();
            s.GetPositionLevelDropDownOptions();
            s.GetPositionDropDownOptions();
            s.LoadOrgGroupPosition();
            s.LoadHierarchyUpward();
            s.LoadOrgGroupNPRF();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabOrgGroupPosition');
            /*if ($('#divOrgGroupTags').children().length <= 2) {
                $(".tablinks:nth-child(3)").hide();
            }*/
            /*if ($('#ddlOrgType :selected').val() != "BRN" && $('#ddlOrgType :selected').val() != "DEPT") {
                $(".tablinks:nth-child(3)").hide();
            }*/
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
                    , OrgGroupDeleteURL + '?ID=' + objOrgGroupViewJS.ID\
                    , {} \
                    , '#divOrgGroupErrorMessage' \
                    , '#btnDelete' \
                    , objOrgGroupViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(OrgGroupEditURL + "?ID=" + objOrgGroupViewJS.ID, "divOrgGroupBodyModal");
            });

            $("#btnEditDetails").click(function () {
                LoadPartial(OrgGroupEditDetailsURL + "?ID=" + objOrgGroupViewJS.ID, "divOrgGroupBodyModal");
            });

            $("#txtSearchBoxOne").on("change keyup keydown", function () {
                ListBoxSearch("ddlListOneOrgGroup", $(this).val(), false);
            });
        },

        AddChildrenManPowerDynamicFields: function () {
            var htmlId = $(".ChildrenManPowerDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenManPowerDynamicFields_", "")) + 1;
            var PositionId = $('#ddlPosition_' + ctr).val();

            /*$("#DivChildrenManPowerDynamicFields").append(
                "<div class=\"form-group form-fields ChildrenManPowerDynamicFields\" id=\"ChildrenManPowerDynamicFields_" + ctr + "\"> \
                  <div class=\"col-md-0-5\"> \
                   </div> \
                  <div class=\"col-md-1 text-align-center no-padding\" style=\"width: 50px;\"> \
                       <input type=\"checkbox\" id=\"chkIsHead_" + ctr + "\" class=\"IsHeadDynamicFields\" title=\"Head\" readonly> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtPosition_" + ctr + "\" class=\"form-control PositionTextDynamicFields\" title=\"Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnPositionID_" + ctr + "\" class=\"form-control PositionDynamicFields\"> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtReportingPosition_" + ctr + "\" class=\"form-control ReportingPositionTextDynamicFields\" title=\"Reporting Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnReportingPositionID_" + ctr + "\" class=\"form-control ReportingPositionDynamicFields\"> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-right no-padding\"> \
                       <label id=\"txtInactive_" + ctr + "\" class=\"control-label block-label text-amount total-label InactiveDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-right no-padding\"> \
                       <label id=\"txtPlanned_" + ctr + "\" class=\"control-label block-label text-amount total-label PlannedDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-1 text-align-right no-padding\"> \
                       <label id=\"txtActiveReg_" + ctr + "\" class=\"control-label block-label text-amount total-label ActiveRegDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-1 text-align-right no-padding\"> \
                       <label id=\"txtActiveProb_" + ctr + "\" class=\"control-label block-label text-amount total-label ActiveProbDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-1 text-align-right no-padding\"> \
                       <label id=\"txtActiveOut_" + ctr + "\" class=\"control-label block-label text-amount total-label ActiveOutDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-1 text-align-right no-padding\"> \
                       <label id=\"txtActiveTotal_" + ctr + "\" class=\"control-label block-label text-amount total-label ActiveTotalDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-right no-padding\"> \
                       <label id=\"txtVariance_" + ctr + "\" class=\"control-label block-label remarks-label text-amount total-label VarianceDynamicFields\"></label> \
                   </div> \
               </div>"
            );*/


            /*<td class=\"dynamic-field-header-custom\">\
                    <label id=\"txtInactive_" + ctr + "\" class=\"control-label text-amount total-label InactiveDynamicFields\"></label> \
                  </td>\*/

            $("#DivChildrenManPowerDynamicFields").append(
                "<tr class=\"ChildrenManPowerDynamicFields\" id=\"ChildrenManPowerDynamicFields_" + ctr + "\"> \
                  <td class=\"dynamic-field-header-custom-header-no-border\" style=\"width:50px;\">\
                  </td>\
                  <td class=\"dynamic-field-header-custom\" style=\"width:50px;\">\
                       <input style=\"margin: auto;\" type=\"checkbox\" id=\"chkIsHead_" + ctr + "\" class=\"IsHeadDynamicFields\" title=\"Head\" readonly> \
                  </td>\
                  <td class=\"dynamic-field-header-custom\" style=\"width:300px;\">\
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtPosition_" + ctr + "\" class=\"form-control PositionTextDynamicFields\" title=\"Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnPositionID_" + ctr + "\" class=\"form-control PositionDynamicFields\"> \
                  </td>\
                  <td class=\"dynamic-field-header-custom\" style=\"width:300px;\">\
                       <input type =\"search\" placeholder=\"Search..\" id=\"txtReportingPosition_" + ctr + "\" class=\"form-control ReportingPositionTextDynamicFields\" title=\"Reporting Position\" maxlength=\"100\"> \
                       <input type=\"hidden\" id=\"hdnReportingPositionID_" + ctr + "\" class=\"form-control ReportingPositionDynamicFields\"> \
                  </td>\
                  <td class=\"dynamic-field-header-custom\">\
                       <label id=\"txtPlanned_" + ctr + "\" class=\"control-label text-amount total-label PlannedDynamicFields\"></label> \
                  </td>\
                  <td class=\"dynamic-field-header-custom\">\
                       <label id=\"txtActiveReg_" + ctr + "\" class=\"control-label text-amount total-label ActiveRegDynamicFields\"></label> \
                  </td>\
                  <td class=\"dynamic-field-header-custom\">\
                       <label id=\"txtActiveProb_" + ctr + "\" class=\"control-label text-amount total-label ActiveProbDynamicFields\"></label> \
                  </td>\
                  <td class=\"dynamic-field-header-custom\">\
                       <label id=\"txtActiveOut_" + ctr + "\" class=\"control-label text-amount total-label ActiveOutDynamicFields\"></label> \
                  </td>\
                  <td class=\"dynamic-field-header-custom\">\
                       <label id=\"txtActiveTotal_" + ctr + "\" class=\"control-label text-amount total-label ActiveTotalDynamicFields\"></label> \
                  </td>\
                  <td class=\"dynamic-field-header-custom\">\
                       <label id=\"txtVariance_" + ctr + "\" class=\"control-label text-amount total-label VarianceDynamicFields\"></label> \
                  </td>\
               </tr>"
            );

            $('#divOrgGroupModal input[type="checkbox"],#divOrgGroupModal select').prop('disabled', true);
            
            //GenerateDropdownValues(ViewPositionLevelDropDownURL, "ddlPositionLevel_" + ctr, "Value", "Text", "", "", false);
            //GenerateDropdownValues(ViewPositionByIDURL, "ddlPosition_" + ctr, "Value", "Text", "", "", false);
            //objEMSCommonJS.PopulateDropDown("#ddlPositionLevel_" + ctr, PositionLevelDropDownOptions);
            //objEMSCommonJS.PopulateDropDown("#ddlPosition_" + ctr, PositionDropDownOptions);
        },

        LoadChildrenOrgGroup: function () {
            $('.listboxbutton, #txtSearchBoxTwo, #ddlListTwoOrgGroup').hide();

            var input = { ID: objOrgGroupViewJS.ID };

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

            var input = { ID: objOrgGroupViewJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddChildrenManPowerDynamicFields();

                    var Variance = (parseInt(item.ActiveCount) + parseInt(item.ActiveProbCount)) - parseInt(item.PlannedCount);
                    var VarianceLabel = (Variance > 0 ? "+" + Variance : Variance);

                    $("#txtInactive_" + idCtr).text(item.InactiveCount);
                    $("#txtInactive_" + idCtr).val(item.InactiveCount);
                    $("#txtPlanned_" + idCtr).text(item.PlannedCount);
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

                    //$("#txtVariance_" + idCtr).val(parseInt(item.ActiveCount) - parseInt(item.PlannedCount));
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
                });
            };

            objEMSCommonJS.GetAjax(GetOrgGroupPositionURL, input, "", GetSuccessFunction);
        },

        LoadHierarchyUpward: function () {

            var GetSuccessFunction = function (data) {

                var length = data.Result.length;
                var codeValue = $("#txtCode").val();
                var descValue = $("#txtDescription").val();
                var parentValue = "";

                $("#divHierarchyUpward").html("");

                $(data.Result).each(function (idx, item) {                    
                    $("#divHierarchyUpward").append(item.Code + " - " + item.Description + " > ");
                    parentValue = item.Code + " - " + item.Description;
                });

                $("#txtParentOrgGroup").val(parentValue);

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

            var input = { OrgGroupID: objOrgGroupViewJS.ID };

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

    objOrgGroupViewJS.Initialize();
});