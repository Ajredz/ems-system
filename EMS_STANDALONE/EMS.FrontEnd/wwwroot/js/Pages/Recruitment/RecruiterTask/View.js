var objRecruiterTaskViewJS;

$(document).ready(function () {
    objRecruiterTaskViewJS = {

        ID: $("#hdnID").val(),
        RecruiterID: $("#hdnRecruiterID").val(),
        ApplicantID: $("#hdnApplicantID").val(),

        Initialize: function () {
            $("#divRecruiterTaskBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divRecruiterTaskModal .form-control").prop("disabled", true);
            s.LoadRecruiter();
            s.LoadApplicant();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divRecruiterTaskModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , RecruiterTaskDeleteURL + '?ID=' + objRecruiterTaskViewJS.ID\
                    , {} \
                    , '#divRecruiterTaskErrorMessage' \
                    , '#btnDelete' \
                    , objRecruiterTaskViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(RecruiterTaskEditURL + "?ID=" + objRecruiterTaskViewJS.ID, "divRecruiterTaskBodyModal");
            });

            //GenerateDropdownValues(ViewGetRecruiterURL + "&ID=" + objRecruiterTaskViewJS.RecruiterID, "txtRecruiter", "ID", "Name", "", "", false);
        },

        LoadRecruiter: function () {
            var GetSuccessFunction = function (data) {
                $("#txtRecruiter").val(data.Result.Description);
            };

          objEMSCommonJS.GetAjax(GetRecruiterURL + "&ID=" + objRecruiterTaskViewJS.RecruiterID, {}, "", GetSuccessFunction);
        },

        LoadApplicant: function () {
            var GetSuccessFunction = function (data) {
                $("#txtApplicant").val(data.Result.Description);
            };

            objEMSCommonJS.GetAjax(GetApplicantURL + "&ID=" + objRecruiterTaskViewJS.ApplicantID, {}, "", GetSuccessFunction);
        },

    };
    
     objRecruiterTaskViewJS.Initialize();
});