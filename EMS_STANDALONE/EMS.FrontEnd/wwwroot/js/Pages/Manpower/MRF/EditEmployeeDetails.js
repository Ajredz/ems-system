var objEmployeeEditJS;
var SaveEmployeeCompensationURL = "/Plantilla/Employee/Edit?handler=SaveEmployeeCompensation";
var OrgGroupDropDown = [];
var PositionDropDown = [];
var RelationshipDropDown = [];
var SchoolLevelDropdown = [];
var EducAttDegreeDropdown = [];
var EducAttStatusDropDown = [];

$(document).ready(function () {
    objEmployeeEditJS = {

        ID: $("#divNewEmployeeModal #hdnID").val(),

        Initialize: function () {
            $("#divNewEmployeeBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divNewEmployeeModal .form-control:not(#divEmployeeModal #txtCode)").attr("readonly", false);
            $('#divNewEmployeeBodyModal input[type="checkbox"]').prop('disabled', false);
            $("#divNewEmployeeModal #btnEdit").hide();

            $("#divNewEmployeeModal #btnSave, #divEmployeeModal #btnBack, \
                #divNewEmployeeModal #btnOnboardingWorkflowTransactionSave").show();

            s.GetSystemUserName();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#divNewEmployeeModal #txtOldEmployeeID"));

            $("#btnSaveNewEmployee").on("click", function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                        , NewEmployeeDetailsURL \
                        , new FormData($('#frmNewEmployee').get(0)) \
                        , '#divEmployeeErrorMessage' \
                        , '#btnSaveNewEmployee' \
                        , objEmployeeEditJS.AddSuccessFunction); ",
                    "function");
            });
        },
        AddSuccessFunction: function () {
            $("#divNewEmployeeModal").modal("hide");
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        OnboardingWorkflowEditSuccessFunction: function () {
            $("#tabOnboarding .form-control").val("");
            objEmployeeListJS.OnboardingWorkflowTransactionJQGrid({
                WorkflowID: $("#hdnOnboardingWorkflowID").val(),
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
            $("#btnSearch").click();
        },

        CompensationEditSuccessFunction: function () {
            $("#tabCompensation #btnEmployeeCompensationBack").click();
        },

        GetFormData: function () {

            var formData = new FormData($('#frmEmployee').get(0));

            $(".RovingOrgGroupDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeRovingList[" + index + "].OrgGroupID", $(this).val());
                }
            });

            $(".RovingPositionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeRovingList[" + index + "].PositionID", $(this).val());
                }
            });

            //OFFICEMOBILE
            formData.append("Employee.OfficeMobile", $("#divEmployeeModal #txtOfficeMobile").val().replace(/-/g, ""));
            //CONTACT PERSON NUMBER


            return formData;

        },

        GetSystemUserName: function () {
            var s = this;

            if ($("#hdnSystemUserID").val() > 0) {
                var GetSuccessFunction = function (data) {
                    $("#txtSystemUserName").val(data.Result.Username);
                };

                objEMSCommonJS.GetAjax(GetSystemUserNameURL + "&ID=" + $("#hdnSystemUserID").val(), {}, "", GetSuccessFunction);

            }
        },

    };

    objEmployeeEditJS.Initialize();
});