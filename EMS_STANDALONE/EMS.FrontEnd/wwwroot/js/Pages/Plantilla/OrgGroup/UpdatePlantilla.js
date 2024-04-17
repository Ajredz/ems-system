var objOrgGroupUpdatePlantillaJS;
var OrgGroupListBoxOptions = [];
var PositionLevelDropDownOptions = [];
var PositionPlannedOld = [];
var PositionPlannedNew = [];
var CheckFileIfExistsURL = window.location.pathname + "?handler=CheckFileIfExists";
var DownloadFileURL = window.location.pathname + "?handler=DownloadFile";

$(document).ready(function () {
    objOrgGroupUpdatePlantillaJS = {

        Initialize: function () {
            $("#divUpdatePlantillaBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();

            s.LoadOrgGroupNPRF();
            s.AddNPRFDynamicFields();
            objEMSCommonJS.ChangeTab($("#divUpdatePlantillaModal .tablinks:nth-child(1)"), 'divUpdatePlantillaModal #tabOrgGroupPosition');
            $("#divUpdatePlantillaModal .UpdatePlannedDynamicFields").each(function (index, item) {
                PositionPlannedOld.push($("#divUpdatePlantillaModal #hdnUpdatePositionID_"+ index).val() + '-' + $(this).val());
            });
            $("#divUpdatePlantillaModal .IsHeadDynamicFields").attr("disabled", true);

            objOrgGroupListJS.CalculateTotal("UpdatePlannedDynamicFields", "txtUpdateTotalPlanned");
            objOrgGroupListJS.CalculateTotal("UpdateActiveDynamicFields", "txtUpdateTotalActive");
            objOrgGroupListJS.CalculateTotal("UpdateInactiveDynamicFields", "txtUpdateTotalInactive");
            //objOrgGroupListJS.CalculateTotal("UpdateVarianceDynamicFields", "txtUpdateTotalVariance");

            $("#txtUpdateTotalVariance").html(($("#txtUpdateTotalActive").text() - $("#txtUpdateTotalPlanned").text()));
        },

        EditSuccessFunction: function () {
            objOrgGroupUpdatePlantillaJS.LoadOrgGroupNPRF();
            objOrgGroupUpdatePlantillaJS.AddNPRFDynamicFields();
            $("#btnSearch").click();
            $("#divUpdatePlantillaModal .IsHeadDynamicFields").attr("disabled", true);
        },

        ElementBinding: function () {
            var s = this;
            NumberOnly($(".UpdatePlannedDynamicFields"));
            NumberOnly($(".UpdateActiveDynamicFields"));
            NumberOnly($(".UpdateInactiveDynamicFields"));
                
            $(".UpdatePlannedDynamicFields").keyup(function () {
                var index = $(this).prop("id").replace("txtUpdatePlannedCount_", "");
                objOrgGroupListJS.CalculateTotal("UpdatePlannedDynamicFields", "txtUpdateTotalPlanned");
                $("#lblUpdateVarianceCount_" + index).text(parseInt($("#txtUpdateActiveCount_" + index).val() || 0) - parseInt($("#txtUpdatePlannedCount_" + index).val() || 0));
            });
            $(".UpdateActiveDynamicFields").keyup(function () {
                var index = $(this).prop("id").replace("txtUpdateActiveCount_", "");
                objOrgGroupListJS.CalculateTotal("UpdateActiveDynamicFields", "txtUpdateTotalActive");
                $("#lblUpdateVarianceCount_" + index).text(parseInt($("#txtUpdateActiveCount_" + index).val() || 0) - parseInt($("#txtUpdatePlannedCount_" + index).val() || 0));
            });
            $(".UpdateInactiveDynamicFields").keyup(function () {
                var index = $(this).prop("id").replace("txtUpdateInactiveCount_", "");
                objOrgGroupListJS.CalculateTotal("UpdateInactiveDynamicFields", "txtUpdateTotalInactive");
                $("#lblUpdateVarianceCount_" + index).text(parseInt($("#txtUpdateActiveCount_" + index).val() || 0) - parseInt($("#txtUpdatePlannedCount_" + index).val() || 0));
            });
            $(".UpdateVarianceDynamicFields").keyup(function () {
                var index = $(this).prop("id").replace("lblUpdateVarianceCount_", "");
                objOrgGroupListJS.CalculateTotal("UpdateVarianceDynamicFields", "txtUpdateTotalVariance");
                $("#lblUpdateVarianceCount_" + index).text(parseInt($("#txtUpdateActiveCount_" + index).val() || 0) - parseInt($("#txtUpdatePlannedCount_" + index).val() || 0));
            });

            $("#divUpdatePlantillaModal #btnAddNPRFFields").click(function () {
                $("#divUpdatePlantillaModal .NPRFNumberDynamicFields").addClass("required-field");
                $("#divUpdatePlantillaModal .DateApprovedDynamicFields").addClass("required-field");
                var fields = $("#divUpdatePlantillaModal #DivNPRFDynamicFields .required-field");
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

            $("#divUpdatePlantillaModal #btnSave").click(function () {
                var isValid = true;
                var errCount = 0;
                var PositionPlannedNew = [];

                //$("#divUpdatePlantillaModal .IsHeadDynamicFields").attr("disabled", false);

                $("#divUpdatePlantillaModal .UpdatePlannedDynamicFields").each(function (index, item) {
                    PositionPlannedNew.push($("#divUpdatePlantillaModal #hdnUpdatePositionID_" + index).val() + '-' + $(this).val());
                });

                var plannedOld = PositionPlannedOld.sort().toString();
                var plannedNew = PositionPlannedNew.sort().toString();

                if (plannedOld != plannedNew) {
                    $("#divUpdatePlantillaModal #divOrgGroupNPRFErrorMessage").html("<label class=\"errMessage\"><li>" + MSG_REQUIRED_NPRF + "</li></label><br />");
                    $("#divUpdatePlantillaModal .NPRFNumberDynamicFields, .DateApprovedDynamicFields").addClass("errMessage");
                    isValid = false;

                    $("#divUpdatePlantillaModal .NPRFNumberDynamicFields").each(function () {
                        var ctr = $(this).prop("id").replace("txtNPRFNumber_", "");
                        if (this.value != "")
                            $("#txtNPRFNumber_" + ctr).removeClass("errMessage");
                        else {
                            $("#txtNPRFNumber_" + ctr).addClass("errMessage");
                            errCount++;
                        }
                    });

                    //$("#divUpdatePlantillaModal .DateApprovedDynamicFields").each(function () {
                    //    var ctr = $(this).prop("id").replace("txtDateApproved_", "");
                    //    if (this.value != "")
                    //        $("#txtDateApproved_" + ctr).removeClass("errMessage");
                    //    else {
                    //        $("#txtDateApproved_" + ctr).addClass("errMessage");
                    //        errCount++;
                    //    }
                    //});

                    $("#divUpdatePlantillaModal .Attachment_File").each(function () {
                        var ctr = $(this).prop("id").replace("fileAttachment_", "");
                        if (this.value != "")
                            $("#fileAttachment_" + ctr).removeClass("errMessage");
                        else {
                            $("#fileAttachment_" + ctr).addClass("errMessage");
                            errCount++;
                        }
                    });

                    if (errCount == 0 && $("#divUpdatePlantillaModal .NPRFNumberDynamicFields").length > 0) {
                        $("#divUpdatePlantillaModal #divOrgGroupNPRFErrorMessage").html('');
                        isValid = true;
                    }
                }
                else {
                    $("#divUpdatePlantillaModal #divOrgGroupNPRFErrorMessage").html('');
                    $("#divUpdatePlantillaModal .NPRFNumberDynamicFields, .DateApprovedDynamicFields").removeClass("errMessage");
                }

                if (isValid & $("#divUpdatePlantillaModal .UpdatePlannedDynamicFields").length > 0) {

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , OrgGroupPlantillaCountUpdateURL \
                        , objOrgGroupUpdatePlantillaJS.GetFormData() \
                        , '#divUpdatePlantillaModal #divOrgGroupErrorMessage' \
                        , '#divUpdatePlantillaModal #btnSave' \
                        , objOrgGroupUpdatePlantillaJS.EditSuccessFunction);", "function");
                }
                else {
                    objEMSCommonJS.ChangeTab($(".tablinks:nth-child(2)"), 'divUpdatePlantillaModal #tabOrgGroupNPRF');
                }
            });

        },

        GetFormData: function () {

            $("#divUpdatePlantillaModal .Attachment_File").each(function (index) {
                $(this).prop("name", "NPRFList[" + index + "].File");
            });

            var formData = new FormData($('#frmOrgGroupPlantillaCountUpdate').get(0));

            $("#divUpdatePlantillaModal .NPRFNumberDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("NPRFList[" + index + "].OrgGroupID", $("#hdnUpdateOrgGroupID_0").val());
                    formData.append("NPRFList[" + index + "].NPRFNumber", $(this).val());
                }
            });

            //$("#divUpdatePlantillaModal .DateApprovedDynamicFields").each(function (index) {
            //    if ($(this).val() != "") {
            //        formData.append("NPRFList[" + index + "].OrgGroupID", $("#hdnUpdateOrgGroupID_0").val());
            //        formData.append("NPRFList[" + index + "].ApprovedDate", $(this).val());
            //    }
            //});

            return formData;

        },


        AddNPRFDynamicFields: function () {
            var htmlId = $("#divUpdatePlantillaModal .NPRFDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("NPRFDynamicFields_", "")) + 1;

            $("#divUpdatePlantillaModal #DivNPRFDynamicFields").append(
                "<div class=\"form-group form-fields NPRFDynamicFields\" id=\"NPRFDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-2-5 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtNPRFNumber_" + ctr + "\" class=\"form-control NPRFNumberDynamicFields\" title=\"NPRFNumber\"> \
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

            //$("#divUpdatePlantillaModal #txtDateApproved_" + ctr).datetimepicker({
            //    useCurrent: false,
            //    format: 'MM/DD/YYYY'
            //});
        },

        LoadOrgGroupNPRF: function () {
            var s = this;
            $("#divUpdatePlantillaModal #DivNPRFList").html("");
            $("#divUpdatePlantillaModal #DivNPRFDynamicFields").html("");

            var input = { OrgGroupID: $("#hdnUpdateOrgGroupID_0").val() };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {

                    $("#divUpdatePlantillaModal #DivNPRFList").append(
                        "<div class=\"form-group form-fields NPRFListFields\" id=\"NPRFListFields" + idCtr + "\"> \
                           <div class=\"col-md-2-5 text-align-center no-padding\"> \
                               <input type=\"text\" id=\"lblNPRFNumber_" + idCtr + "\" value=\"" + item.NPRFNumber + "\" class=\"form-control\" title=\"NPRFNumber\" readonly> \
                           </div> \
                           <div class=\"col-md-3 no-padding\"> \
                               <label class=\"control-label block-label\">" + item.SourceFile + "</label> \
                               <input type=\"hidden\" id=\"lblServerFile_ " + idCtr + "\" value=\"" + item.ServerFile + "\" class=\"form-control\"> \
                               <input type=\"hidden\" id=\"lblSourceFile_ " + idCtr + "\" value=\"" + item.SourceFile + "\" class=\"form-control\"> \
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
                    //$("#divUpdatePlantillaModal #lblNPRFNumber_" + idCtr).val(item.NPRFNumber);
                    //$("#divUpdatePlantillaModal #lblDateApproved_" + idCtr).val(d.toLocaleString("en-US"));

                    //$("#divUpdatePlantillaModal #lblDateApproved_" + idCtr).datetimepicker({
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

        RemoveDynamicFields: function (id) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },


    };

    objOrgGroupUpdatePlantillaJS.Initialize();
});