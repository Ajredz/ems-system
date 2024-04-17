var objMRFViewJS;

$(document).ready(function () {
    objMRFViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divMRFBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divMRFModal .form-control").prop("disabled", true);
            $('#cbIsConfidential').prop('disabled', true);
            $('#cbIsAvailableOnline').prop('disabled', true);
            //$("#ddlPosition").change();
            objMRFListJS.LoadApprovalHistoryJQGrid({
                RequestingPositionID: $("#ddlPosition").val(),
                RequestingOrgGroupID: $("#ddlOrgGroup").val(),
                PositionID: $("#ddlPosition").val(),
                MRFID: $("#hdnID").val()
            });
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApprovalHistory');
            objMRFListJS.GetComment();

            if (!$("#cbIsAvailableOnline").is(":checked")) {
                $("#divMRFModal .tablinks").find("span:contains('Online Job Post')").parent("button").hide();
            }
            else {
                $('#SummerNoteJobDescription').summernote({
                    toolbar: [
                    ]
                });
                $('#SummerNoteJobQualification').summernote({
                    toolbar: [
                    ]
                });
                $('#SummerNoteJobDescription').summernote('disable');
                $('#SummerNoteJobQualification').summernote('disable');
            }

            KickoutQuestionTabLoadOnce = false;
        },

        SuccessFunction: function () {
            $("#divMRFList #btnSearch").click();
            $('#divMRFModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , MRFDeleteURL + '?ID=' + objMRFViewJS.ID\
                    , {} \
                    , '#divMRFErrorMessage' \
                    , '#btnDelete' \
                    , objMRFViewJS.SuccessFunction);",
                    "function");
            });

            $("#btnCancel").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_CANCEL_REQUEST,
                    "objEMSCommonJS.PostAjax(false \
                    , MRFCancelURL + '?ID=' + objMRFViewJS.ID\
                    , {} \
                    , '#divMRFErrorMessage' \
                    , '#btnCancel' \
                    , objMRFViewJS.SuccessFunction);",
                    "function");
            });

            $("#btnEdit, #btnRevise").click(function () {
                LoadPartial(MRFEditURL + "?ID=" + objMRFViewJS.ID, "divMRFBodyModal");
            });

            $("#btnSaveComment").click(function () {
                if ($("#txtAreaComments").val() != "") {
                    //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE,
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

            $("#btnHRCancel").click(function () {
                //LoadPartial(AdminMRFCancelURL, "divMRFCancelBodyModal");
                $("#divMRFCancelModal").modal("show");
                $('#txtReason').val("");
            });

            $("#btnProceed").click(function () {
                if ($("#txtReason").val() != "") {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED, "objMRFViewJS.HRCancelMRFRequest(" + $('#hdnID').val() + ", '" + $('#txtReason').val() + "')", "function");
                }
                else {
                    $("#txtReason").focus();
                }
            });

            $(".tablinks").find("span:contains('Kickout Question')").parent("button").click(function () {
                if (!KickoutQuestionTabLoadOnce) {
                    objMRFListJS.LoadKickoutQuestion({
                        MRFID: objMRFViewJS.ID,
                    });
                    KickoutQuestionTabLoadOnce = true;
                }
            });

        },

        HRCancelMRFRequest: function (ID, Reason) {

            var successFunction = function () {
                $("#divMRFCancelModal").modal("hide");
                $("#divMRFModal").modal("hide");
                $("#divMRFList #btnSearch").click();
                //LoadPartial(MRFViewURL + "?ID=" + objMRFViewJS.ID, "divMRFBodyModal");
            };

            objEMSCommonJS.PostAjax(false
                , HRCancelMRFRequestURL + "?MRFID=" + ID + "&Reason=" + Reason
                , {}
                , '#divMRFCancelModal #divMRFCancelErrorMessage'
                , ''
                , successFunction);
        },

    };
    
     objMRFViewJS.Initialize();
});