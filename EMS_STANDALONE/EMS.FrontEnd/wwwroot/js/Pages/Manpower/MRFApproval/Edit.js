var objMRFApprovalEditJS;

$(document).ready(function () {
    objMRFApprovalEditJS = {
        
        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divMRFApprovalBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");

            $("#divMRFApprovalModal .form-control").prop("disabled", true);
            //$("#txtRemarks").prop("disabled", false);

            //var GetSuccessFunction = function(data) {
            //    s.AddSignatoriesDynamicFields(data);
            //};
            //objEMSCommonJS.GetAjax(GetSignatoriesURL + "&PositionID=" + $("#ddlPosition").val() + "&RecordID=" + s.ID, {}, "", GetSuccessFunction);
            objMRFApprovalListJS.LoadApprovalHistoryJQGrid(
                {
                    PositionID: $("#ddlPosition").val(),
                    RequestingPositionID: $("#hdnRequestingPositionID").val(),
                    RequestingOrgGroupID: $("#hdnRequestingOrgGroupID").val(),
                    MRFID: $("#hdnID").val()
                });

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApprovalHistory');
            objMRFApprovalListJS.GetComment();
        },

        ElementBinding: function () {
            var s = this;
            NumberOnly($("#divMRFApprovalModal #txtTurnaroundTime"));

            $("#btnApprove, #btnReject").click(function () {
                $("#btnProceed").val($(this).val());
                $("#divMRFRemarksModal").modal("show");
            });

            $("#btnProceed").click(function() {

                var isNoBlankFunction = function () {

                    if ($("#btnProceed").val() == "REJECTED" && $("#txtApprovalRemarks").val() == "") {
                        $("#divMRFRemarksErrorMessage").html("<label class=\"errMessage\"><li>Remarks is required</li></label><br />");
                        $("#txtApprovalRemarks").addClass("errMessage")
                    }
                    else
                        return true;
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmMRFApproval", "#divMRFApprovalErrorMessage", isNoBlankFunction)) {
                    //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    //    "objEMSCommonJS.PostAjax(true \
                    //    , MRFApprovalEditPostURL + '&RecordID=' + objMRFApprovalEditJS.ID \
                    //    + '&Result="+ $(this).val() +"' \
                    //    + '&LevelOfApproval="+ $("#hdnLevelOfApproval").val() +"' \
                    //    + '&NextApproverPositionID="+ $("#hdnNextApproverPositionID").val() +"' \
                    //    + '&NextApproverOrgGroupID="+ $("#hdnNextApproverOrgGroupID").val() +"' \
                    //    , {} \
                    //    , '#divMRFApprovalErrorMessage' \
                    //    , '#btnSave' \
                    //    , objMRFApprovalEditJS.EditSuccessFunction);", "function");

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                      "objEMSCommonJS.PostAjax(true \
                          , MRFApprovalEditPostURL \
                          , objMRFApprovalEditJS.GetFormData() \
                          , '#divMRFApprovalErrorMessage' \
                          , '#btnSave' \
                          , objMRFApprovalEditJS.EditSuccessFunction);", "function");
                }
            });

            $("#divMRFRemarksHeaderModal .close").click(function () {
                $("#divMRFRemarksErrorMessage").html("");
                $("#txtApprovalRemarks").removeClass("errMessage");
                $("#txtApprovalRemarks").val();
			});
			
            $("#btnSaveComment").click(function () {
                if ($("#txtAreaComments").val() != "") {
                    //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    //    "objEMSCommonJS.PostAjax(true \
                    //    , SaveCommentsURL \
                    //    , objMRFApprovalListJS.GetCommentSectionFormData() \
                    //    , '#divMRFErrorMessage' \
                    //    , '#btnSaveComment' \
                    //    , objMRFApprovalListJS.GetComment);", "function");

                    objEMSCommonJS.PostAjax(true
                        , SaveCommentsURL
                        , objMRFApprovalListJS.GetCommentSectionFormData()
                        , '#divMRFErrorMessage'
                        , '#btnSaveComment'
                        , objMRFApprovalListJS.GetComment, null, true);
                }
                else {
                    $("#txtAreaComments").focus();
                }
            });

            //$("#btnReject").click(function () {
            //    if (objEMSCommonJS.ValidateBlankFields("#frmMRFApproval", "#divMRFApprovalErrorMessage")) {
            //        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
            //            "objEMSCommonJS.PostAjax(true \
            //            , MRFApprovalEditPostURL + '&ID=' + objMRFApprovalEditJS.ID + '&Result="+ $(this).val() +"' \
            //            , {} \
            //            , '#divMRFApprovalErrorMessage' \
            //            , '#btnSave' \
            //            , objMRFApprovalEditJS.EditSuccessFunction);", "function");
            //    }
            //});
        },

        GetFormData: function () {

            var formData = new FormData($('#frmMRFApproval').get(0));

            formData.append("RecordID", objMRFApprovalEditJS.ID); 
            formData.append("LevelOfApproval", $("#hdnLevelOfApproval").val());
            formData.append("Result", $("#btnProceed").val());
            formData.append("NextApproverPositionID", $("#hdnNextApproverPositionID").val());
            formData.append("NextApproverOrgGroupID", $("#hdnNextApproverOrgGroupID").val());
            formData.append("NextAltApproverPositionID", $("#hdnNextAltApproverPositionID").val());
            formData.append("NextAltApproverOrgGroupID", $("#hdnNextAltApproverOrgGroupID").val());
            formData.append("Remarks", $("#txtApprovalRemarks").val());

            return formData;
        },

        EditSuccessFunction: function () {
            //$("#tblMRFFormApprovalHistoryList").jqGrid("GridUnload");
            //$("#tblMRFFormApprovalHistoryList").jqGrid("GridDestroy");
            $("#txtApprovalRemarks").val("");
            $("#divMRFApprovalFilter #btnSearch").click();
            $("#divMRFRemarksModal .close").click();
            LoadPartial("" + MRFApprovalEditURL + "?ID=" + objMRFApprovalEditJS.ID + "&RecordStatus=" + $("#lblMRFStatus").text() + ""
                , "divMRFApprovalBodyModal");
            //$("#divMRFApprovalModal .close").click();
        },

        AddSignatoriesDynamicFields: function (data) {
            $("#divSignatories").html("");
            $(data.Result).each(function (index, item) {
                var fontColor = item.ApprovalStatusCode == "PENDING" ?
                    "#FF6D0D" :
                    item.ApprovalStatusCode == "FOR_APPROVAL" ?
                        "#4949FF" :
                        item.ApprovalStatusCode == "APPROVED" ?
                            "#029902" :
                            item.ApprovalStatusCode == "REJECTED" ?
                                "#FF0000" : "";
                $("#divSignatories").append(
                    "<div class=\"form-group form-fields\">"
                    + " 	<div class=\"col-md-2-5 col-label\">"
                    + " 		<label class=\"control-label block-label\"> <span class=\"unreqField\">* </span>" + item.ApproverDescription + "</label>"
                    + " 	</div>"
                    + " 	<div class=\"col-md-4-5\">"
                    + " 		<input type=\"text\" class=\"form-control\" value=\"" + item.ApproverName + "\" title=\"Approver Description\" readonly>"
                    + " 	</div>"
                    + " 	<div class=\"col-md-1 col-label\">"
                    + " 		<label class=\"control-label block-label\">" + item.ApprovalTAT + " day(s)</label>"
                    + " 	</div>"
                    + " 	<div class=\"col-md-1-5 col-label\">"
                    + " 		<label class=\"control-label block-label\" style=\"color:" + fontColor + "\">" + item.ApprovalStatus + "</label>"
                    + " 	</div>"
                    + " 	<div class=\"col-md-2 col-label\">"
                    + " 		<label class=\"control-label block-label\">" + item.ApprovedDate + "</label>"
                    + " 	</div>"
                    + " </div>"
                );
            });
        },

    };
    
     objMRFApprovalEditJS.Initialize();
});