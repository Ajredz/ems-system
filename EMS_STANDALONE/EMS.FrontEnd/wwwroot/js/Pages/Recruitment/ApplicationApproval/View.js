var objApplicantViewJS;

var AttachmentTypeDropDownOptions = [];
$(document).ready(function () {
    objApplicantViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divApplicationApprovalBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $("#btnAddAttachmentFields, .deleteAttachmentButtons").remove();
            $(".required-field").removeClass("required-field");
            $("#divApplicantModal .form-control").prop("disabled", true);
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabPersonalInformation');
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divApplicantModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;
            //$("#divApplicantModal #txtExpectedSalary").val(AddZeroes($("#divApplicantModal #txtExpectedSalary").val()).withComma());
            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , ApplicantDeleteURL + '?ID=' + objApplicantViewJS.ID\
                    , {} \
                    , '#divApplicantErrorMessage' \
                    , '#btnDelete' \
                    , objApplicantViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(ApplicantEditURL + "?ID=" + objApplicantViewJS.ID, "divApplicantBodyModal");
            });

        },

    };
    
     objApplicantViewJS.Initialize();
});