var objMRFEditJS;
var originalForm;
var MRFRevisePostURL = window.location.pathname + "/Edit?handler=Revise";

$(document).ready(function () {
    objMRFEditJS = {
        
        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divMRFBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass(".unreqField");
            $(".reqField").removeClass(".reqField");
            $("#btnEdit, #btnRevise").hide();
            if ($("#hdnStatusCode").val() == "REJECTED") {
                $("#btnReviseSave, #btnReviseBack").show();
            }
            else {
                $("#btnSave, #btnBack").show();
                $("#btnDelete").hide();
            }
            $("#divMRFModal .form-control").attr("readonly", false);
            $("#txtVacancy, #ddlOrgGroup, #ddlPositionLevel, #ddlPosition").attr("readonly", true);
            $("#ddlPosition").change();
            objMRFListJS.LoadApprovalHistoryJQGrid({
                RequestingPositionID: $("#ddlPosition").val(),
                RequestingOrgGroupID: $("#ddlOrgGroup").val(),
                PositionID: $("#ddlPosition").val(),
                MRFID: $("#hdnID").val()
            });
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApprovalHistory');
            objMRFListJS.GetComment();

            //, \#ddlNatureOfEmployment option[value = 'PROJ_BASED']
            // Temporarily disable other options, in preparation for next Phase
            $("#ddlNatureOfEmployment option[value='PART_TIME']").prop('disabled', true);

            originalForm = $('#frmMRF').serialize();

            if (!$("#cbIsAvailableOnline").is(":checked")) {
                $("#divMRFModal .tablinks").find("span:contains('Online Job Post')").parent("button").hide();
            }

            $('#SummerNoteJobDescription').summernote({
                toolbar: [
                    // [groupName, [list of button]]
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'style', 'paragraph']],
                    ['height', ['height']]
                ]
            });
            $('#SummerNoteJobQualification').summernote({
                toolbar: [
                    // [groupName, [list of button]]
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph', 'style']],
                    ['height', ['height']]
                ]
            });
        },

        ElementBinding: function () {
            var s = this;
            NumberOnly($("#divMRFModal #txtTurnaroundTime"));

            $("#txtJobPosition").keyup(function () {
                /*function toTitleCase(str) {
                    return str.replace(/(?:^|\s)\w/g, function (match) {
                        return match.toUpperCase();
                    });
                }*/
                $("#txtJobPosition").val($("#txtJobPosition").val().toUpperCase());
            });
            $("#txtJobLocation").keyup(function () {
                /*function toTitleCase(str) {
                    return str.replace(/(?:^|\s)\w/g, function (match) {
                        return match.toUpperCase();
                    });
                }*/
                $("#txtJobLocation").val($("#txtJobLocation").val().toUpperCase());
            });

            $("#ddlOrgGroup").change(function () {
                if ($("#ddlOrgGroup").val() != "") {
                    //var orgGroup = JSON.parse($("#ddlOrgGroup").val());
                    //GenerateDropdownValues(GetPositionLevelDropDownByOrgGroupIDURL + "&OrgGroupID=" + $("#ddlOrgGroup").val(), "ddlPositionLevel", "Value", "Text", "", "", false);

                    $("input[name='MRF.OrgGroupID']").val($("#ddlOrgGroup").val());
                    
                    //var GetSuccessFunction = function (data) {
                    //    $("input[name='MRF.Region.ID']").val(data.Result.ID);
                    //    $("input[name='MRF.Region.Code']").val(data.Result.Code);
                    //    $("input[name='MRF.Region.Description']").val(data.Result.Description);
                    //};
                    //objEMSCommonJS.GetAjax(GetRegionByIDURL + "&RegionID=" + orgGroup.RegionID, {}, "", GetSuccessFunction);
                }

            });

            $("#btnSaveComment").click(function () {
                if ($("#txtAreaComments").val() != "") {
                    //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    //    "objEMSCommonJS.PostAjax(true \
                    //    , SaveMRFCommentsURL \
                    //    , objMRFListJS.GetCommentSectionFormData() \
                    //    , '#divMRFErrorMessage' \
                    //    , '#btnSaveComment' \
                    //    , objMRFListJS.GetComment);", "function");

                    objEMSCommonJS.PostAjax(true
                        , SaveMRFCommentsURL 
                        , objMRFListJS.GetCommentSectionFormData()
                        , '#divMRFErrorMessage'
                        , '#btnSaveComment'
                        , objMRFListJS.GetComment, null, true);
                }
                else {
                    $("#txtAreaComments").focus();
                }

            });

            //$("#ddlPositionLevel").change(function () {
            //    GenerateDropdownValues(GetPositionDropDownURL +
            //        "&OrgGroupID=" + $("#ddlOrgGroup").val() +
            //        "&PositionLevelID=" + $("#ddlPositionLevel").val(),
            //        "ddlPosition", "Value", "Text", "", "", false);
            //    $("input[name='MRF.PositionLevel.ID']").val($("#ddlPositionLevel").val());
            //    $("input[name='MRF.PositionLevel.Description']").val($("#ddlPositionLevel option:selected").text());

            //});

            $("#ddlPosition").change(function () {
                if ($("#ddlPosition").val() != "") {
                    //var position = JSON.parse($("#ddlPosition").val());
                    //var GetSuccessFunction = function (data) {
                    //    s.AddSignatoriesDynamicFields(data);
                    //};
                    //objEMSCommonJS.GetAjax(GetSignatoriesURL + "&PositionID=" + $("#ddlPosition").val() + "&RecordID=" + s.ID, {}, "", GetSuccessFunction);
                    objMRFListJS.LoadApprovalHistoryJQGrid({ PositionID: $("#ddlPosition").val(), MRFID: $("#hdnID").val() });

                    $("input[name='MRF.PositionID']").val($("#ddlPosition").val());

                }
                else {
                    $("#divSignatories").html("");
                }

            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , MRFDeleteURL + '?ID=' + objMRFEditJS.ID \
                    , {} \
                    , '#divMRFErrorMessage' \
                    , '#btnDelete' \
                    , objMRFEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmMRF", "#divMRFErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , MRFEditPostURL \
                        , new FormData($('#frmMRF').get(0)) \
                        , '#divMRFErrorMessage' \
                        , '#btnSave' \
                        , objMRFEditJS.EditSuccessFunction);", "function");
                }
            });

            $("#btnReviseSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmMRF", "#divMRFErrorMessage")) {
                    if (originalForm != $('#frmMRF').serialize()) {
                        if ($("#hdnStatusCode").val() == "REJECTED") {
                            ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                                "objEMSCommonJS.PostAjax(true \
                                , MRFRevisePostURL \
                                , new FormData($('#frmMRF').get(0)) \
                                , '#divMRFErrorMessage' \
                                , '#btnReviseSave' \
                                , objMRFEditJS.EditSuccessFunction);", "function");
                        }
                    }
                    else {
                        $("#frmMRF #divMRFErrorMessage").html("<label class=\"errMessage\"><li>" + NO_CHANGE_FORM + "</li></label><br />");
                    }
                }
            });

            $("#btnBack, #btnReviseBack").click(function () {
                LoadPartial(MRFViewURL + "?ID=" + objMRFEditJS.ID, "divMRFBodyModal");
            });

            $("#cbIsAvailableOnline").change(function () {
                if ($("#cbIsAvailableOnline").prop("checked") == true) {
                    $("#tabJobAppLink").show();


                    var GetOnlineJobPost = function (data) {
                        $('#SummerNoteJobDescription').summernote('reset');
                        $('#SummerNoteJobQualification').summernote('reset');

                        $("#txtJobPosition").val(data.Result["OnlinePosition"]);
                        $("#txtJobLocation").val(data.Result["OnlineLocation"]);
                        $('#SummerNoteJobDescription').summernote('code', data.Result["OnlineJobDescription"]);
                        $('#SummerNoteJobQualification').summernote('code', data.Result["OnlineJobQualification"]);
                    };

                    if ($("#ddlPosition :selected").val() != "") {
                        objEMSCommonJS.GetAjax(GetOnlineJobPositionURL
                            + "&ID=" + $("#ddlPosition").val()
                            , {}
                            , ""
                            , GetOnlineJobPost);
                    }

                    $(".addReq").addClass("required-field");
                }
                else {
                    $("#tabJobAppLink").hide();
                    $(".addReq").removeClass("required-field");
                }
            });
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divMRFModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#tblMRFFormApprovalHistoryList").jqGrid("GridUnload");
            $("#tblMRFFormApprovalHistoryList").jqGrid("GridDestroy");
            $("#btnSearch").click();
            $("#btnBack").click();
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
    
     objMRFEditJS.Initialize();
});