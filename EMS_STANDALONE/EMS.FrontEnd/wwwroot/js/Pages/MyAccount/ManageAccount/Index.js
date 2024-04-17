var objIndexJS;
const ChangePasswordURL = "/MyAccount/ManageAccount?handler=ChangePassword";
const GetFamilyBackgroundURL = "/MyAccount/ManageAccount?handler=FamilyByEmployeeID";
const RelationshipDropDownURL = "/MyAccount/ManageAccount?handler=RelationshipDropDown";
const SchoolLevelDropDownURL = "/MyAccount/ManageAccount?handler=ReferenceValue&RefCode=EMP_SCHOOL_LEVEL";
const EducationalAttainmentDegreeDropDownURL = "/MyAccount/ManageAccount?handler=ReferenceValue&RefCode=EMP_ED_ATT_DEG";
const EducationalAttainmentStatusDropDownURL = "/MyAccount/ManageAccount?handler=ReferenceValue&RefCode=EMP_ED_ATT_STAT";
const GetEducationURL = "/MyAccount/ManageAccount?handler=EducationByEmployeeID";
const GetWorkingHistoryURL = "/MyAccount/ManageAccount?handler=WorkingHistoryByEmployeeID";
const GetDetailsURL = "/MyAccount/ManageAccount?handler=Details";

var RelationshipDropDown = [];
var SchoolLevelDropdown = [];
var EducAttDegreeDropdown = [];
var EducAttStatusDropDown = [];

$(document).ready(function () {
    objIndexJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            var s = this;
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $('input[type="checkbox"]').prop('disabled', true);
            $("#btnAddEmployeeRovingFields, #btnAddFamilyFields, #btnAddEducationFields, #btnAddWorkingHistoryFields, #btnEmployeeCompensationSave").hide();
            $("#btnSaveMainAttachment, #frmEmployeeAttachment .glyphicon-trash, #frmEmployeeAttachment .glyphicon-plus-sign").remove();
            $("#txtCellphoneNumber").val() == (null || "") ? "" : $("#txtCellphoneNumber").val($("#txtCellphoneNumber").val().slice(0, 4) + "-" + $("#txtCellphoneNumber").val().substr(4));
            $("#txtContactPersonNumber").val() == (null || "") ? "" : $("#txtContactPersonNumber").val($("#txtContactPersonNumber").val().slice(0, 4) + "-" + $("#txtContactPersonNumber").val().substr(4));
            $(".form-control").prop("disabled", true);
            $("#txtCurrentPassword, #txtNewPassword, #txtConfirmNewPassword").prop("readonly", false);
            $("#txtCurrentPassword, #txtNewPassword, #txtConfirmNewPassword").prop("disabled", false);
            $("#txtCurrentPassword, #txtNewPassword, #txtConfirmNewPassword").addClass("required-field");
            $("#divPasswordModal .unreqField").addClass("reqField");
            $("#divPasswordModal .reqField").removeClass("unreqField");

            formatSSSNumber($("#txtSSSNumber"));
            addHyphen($("#txtPagibigNumber"), 4);
            addHyphen($("#txtPhilhealthNumber"), 4);
            addHyphen($("#txtTIN"), 3);


            // Reset Tab First Load tags
            personalInfoTabFirstLoad = false;
            familyBackTabFirstLoad = false;
            educationTabFirstLoad = false;
            WorkHisTabFirstLoad = false;

            s.ElementBinding();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabPersonalInformation');
            $(".tablinks").first().click();  
        },

        ElementBinding: function () {
            var s = this;

            $("#dpBirthDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#btnEdit").click(function () {
                $("#btnChangePassword").show();
                $("#tabChangePassword .form-control").prop("readonly", false);
            });

            $("#btnBack").click(function () {
                $("#btnChangePassword").hide();
                $("#tabChangePassword .form-control").prop("readonly", true);
            });

            $("#btnChangePassword").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmManageAcountChangePassword", "#divChangePasswordErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_CHANGE_PASSWORD,
                        "objEMSCommonJS.PostAjax(true \
                        , ChangePasswordURL \
                        , new FormData($('#frmManageAcountChangePassword').get(0)) \
                        , '#divChangePasswordErrorMessage' \
                        , '#btnSave' \
                        , objIndexJS.ChangePasswordSuccessFunction, null, true);",
                        "function");
                }
            });

            $(".tablinks").find("span:contains('Family Background')").parent("button").click(function () {
                if (!familyBackTabFirstLoad) {
                    s.GetRelationshipDropDown();
                    s.LoadFamilyBackground();
                    familyBackTabFirstLoad = true;
                    $("#tabFamilyBackground .form-control").prop("disabled", true);
                }
            });
            $(".tablinks").find("span:contains('Education')").parent("button").click(function () {
                if (!educationTabFirstLoad) {
                    s.GetSchoolLevelDropDown();
                    s.GetEducAttDegreeDropDown();
                    s.GetEducAttStatusDropDown();
                    s.LoadEducationBackground();
                    educationTabFirstLoad = true;
                    $("#hdnIsViewedEducation").val(true);
                    $("#tabEducation .form-control").prop("disabled", true);
                }
            });

            $(".tablinks").find("span:contains('Working History')").parent("button").click(function () {
                if (!WorkHisTabFirstLoad) {
                    s.LoadWorkingHistory();
                    WorkHisTabFirstLoad = true;
                    $("#tabWorkingHistory .form-control").prop("disabled", true);
                }
            });

            $("#btnChangePass").click(function () {
                $('#divPasswordModal').modal('show');
            });

            $("#btnEditProfile").click(function () {
                window.location = "/myaccount/manageaccount/edit";
            });
        },
        ChangePasswordSuccessFunction: function (data) {
            ModalAlertRedirect(MODAL_HEADER, data.Result, '/Login?handler=Logout');
            $("#frmManageAcountChangePassword").trigger("reset");
        },
        LoadFamilyBackground: function () {
            var s = this;
            $("#DivFamilyDynamicFields").html("");

            var input = { EmployeeID: objIndexJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddFamilyDynamicFields();
                    $("#txtName_" + idCtr).val(item.Name);
                    $("#ddlRelationship_" + idCtr).val(item.Relationship);
                    $("#txtNumber_" + idCtr).val(item.ContactNumber);
                    $("#dpBirthDate_" + idCtr).val(item.BirthDate);
                    $("#txtOccupation_" + idCtr).val(item.Occupation);
                    $("#txtSpouseEmployer_" + idCtr).val(item.SpouseEmployer);

                    $("#txtName_" + idCtr).attr("readonly", true);
                    $("#ddlRelationship_" + idCtr).attr("readonly", true);
                    $("#txtNumber_" + idCtr).attr("readonly", true);
                    $("#dpBirthDate_" + idCtr).attr("readonly", true);
                    $("#txtOccupation_" + idCtr).attr("readonly", true);
                    $("#txtSpouseEmployer_" + idCtr).attr("readonly", true);
                    $("#txtNumber_" + idCtr).val() == (null || "") ? "" : $("#txtNumber_" + idCtr).val($("#txtNumber_" + idCtr).val().slice(0, 4) + "-" + $("#txtNumber_" + idCtr).val().substr(4));
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetFamilyBackgroundURL, input, "", GetSuccessFunction);
        },
        LoadEducationBackground: function () {
            var s = this;
            $("#DivEducationDynamicFields").html("");

            var input = { EmployeeID: objIndexJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddEducationDynamicFields();
                    $("#txtSchool_" + idCtr).val(item.School);
                    $("#txtAddress_" + idCtr).val(item.SchoolAddress);
                    $("#ddlLevel_" + idCtr).val(item.SchoolLevelCode);
                    $("#txtCourse_" + idCtr).val(item.Course);
                    $("#dpYearFrom_" + idCtr).val(item.YearFrom);
                    $("#dpYearTo_" + idCtr).val(item.YearTo);
                    $("#ddlEducationalAttainmentDegree_" + idCtr).val(item.EducationalAttainmentDegreeCode);
                    $("#ddlEducationalAttainmentStatus_" + idCtr).val(item.EducationalAttainmentStatusCode);

                    $("#txtSchool_" + idCtr).attr("readonly", true);
                    $("#txtAddress_" + idCtr).attr("readonly", true);
                    $("#ddlLevel_" + idCtr).attr("readonly", true);
                    $("#txtCourse_" + idCtr).attr("readonly", true);
                    $("#dpYearFrom_" + idCtr).attr("readonly", true);
                    $("#dpYearTo_" + idCtr).attr("readonly", true);
                    $("#ddlEducationalAttainmentDegree_" + idCtr).attr("readonly", true);
                    $("#ddlEducationalAttainmentStatus_" + idCtr).attr("readonly", true);
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetEducationURL, input, "", GetSuccessFunction);
        },

        LoadWorkingHistory: function () {
            var s = this;
            $("#DivWorkingHistoryDynamicFields").html("");

            var input = { EmployeeID: objIndexJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddWorkingHistoryDynamicFields();
                    $("#txtCompanyName_" + idCtr).val(item.CompanyName);
                    $("#dpFrom_" + idCtr).val(item.From);
                    $("#dpTo_" + idCtr).val(item.To);
                    $("#txtPosition_" + idCtr).val(item.Position);
                    $("#txtReason_" + idCtr).val(item.ReasonForLeaving);

                    $("#txtCompanyName_" + idCtr).attr("readonly", true);
                    $("#dpFrom_" + idCtr).attr("readonly", true);
                    $("#dpTo_" + idCtr).attr("readonly", true);
                    $("#txtPosition_" + idCtr).attr("readonly", true);
                    $("#txtReason_" + idCtr).attr("readonly", true);
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetWorkingHistoryURL, input, "", GetSuccessFunction);
        },
        GetSchoolLevelDropDown: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    SchoolLevelDropdown.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(SchoolLevelDropDownURL, {}, "", GetSuccessFunction);
        },

        GetEducAttDegreeDropDown: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    EducAttDegreeDropdown.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(EducationalAttainmentDegreeDropDownURL, {}, "", GetSuccessFunction);
        },

        GetEducAttStatusDropDown: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    EducAttStatusDropDown.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(EducationalAttainmentStatusDropDownURL, {}, "", GetSuccessFunction);
        },
        AddFamilyDynamicFields: function () {
            var s = this;
            var htmlId = $(".FamilyDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("FamilyDynamicFields_", "")) + 1;
            /*<label class=\"control-label block-label lblPhoneNum\">+63</label> \*/
            $("#DivFamilyDynamicFields").append(
                "<div class=\"form-group form-fields FamilyDynamicFields\" id=\"FamilyDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <input id=\"txtName_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field FamilyNameDynamicFields\" title=\"Name\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <select id=\"ddlRelationship_" + ctr + "\" title=\"Relationship\" class=\"form-control required-field FamilyRelationshipDynamicFields\"></select> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <div style = \"overflow:hidden;\" > \
                           <input type=\"tel\" id=\"txtNumber_" + ctr + "\" class=\"form-control required-field FamilyNumberDynamicFields\" maxlength=\"12\" title=\"Cellphone Number\" placeholder=\"09xx-xxxxxxx\" /> \
                       </div> \
                   </div> \
                    <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <input id=\"dpBirthDate_" + ctr + "\" maxlength=\"10\" class=\"form-control required-field FamilyBirthDateDynamicFields\" title=\"Birth Date\"> \
                   </div> \
                    <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtOccupation_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field FamilyOccupationDynamicFields\" title=\"Occupation\"> \
                   </div> \
                    <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtSpouseEmployer_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field FamilySpouseEmployerDynamicFields\" title=\"Spouse Employer\"> \
                   </div> \
                 </div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlRelationship_" + ctr, RelationshipDropDown);
        },

        AddEducationDynamicFields: function () {
            var s = this;
            var htmlId = $(".EducationDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("EducationDynamicFields_", "")) + 1;

            $("#DivEducationDynamicFields").append(
                "<div class=\"form-group form-fields EducationDynamicFields\" id=\"EducationDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtSchool_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field EducationSchoolDynamicFields\" title=\"School\"> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtAddress_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field EducationAddressDynamicFields\" title=\"Address\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <select id=\"ddlLevel_" + ctr + "\" title=\"Level\" class=\"form-control required-field EducationLevelDynamicFields\"></select> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <input id=\"txtCourse_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field EducationCourseDynamicFields\" title=\"Course\"> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input id=\"dpYearFrom_" + ctr + "\" maxlength=\"5\" class=\"form-control required-field EducationFromDynamicFields\" title=\"From\"> \
                   </div> \
                    <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input id=\"dpYearTo_" + ctr + "\" maxlength=\"5\" class=\"form-control required-field EducationToDynamicFields\" title=\"To\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <select id=\"ddlEducationalAttainmentDegree_" + ctr + "\" title=\"Educational Attainment Degree\" class=\"form-control required-field EducationEducationalAttainmentDegreeDynamicFields\"></select> \
                   </div> \
                    <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <select id=\"ddlEducationalAttainmentStatus_" + ctr + "\" title=\"Educational Attainment Status\" class=\"form-control required-field EducationEducationalAttainmentStatusDynamicFields\"></select> \
                   </div> \
                 </div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlLevel_" + ctr, SchoolLevelDropdown);
            objEMSCommonJS.PopulateDropDown("#ddlEducationalAttainmentDegree_" + ctr, EducAttDegreeDropdown);
            objEMSCommonJS.PopulateDropDown("#ddlEducationalAttainmentStatus_" + ctr, EducAttStatusDropDown);

            $("#dpYearFrom_" + ctr + ", #dpYearTo_" + ctr).datetimepicker({
                viewMode: 'years',
                format: 'YYYY'
            });

        },
        AddWorkingHistoryDynamicFields: function () {
            var s = this;
            var htmlId = $(".WorkingHistoryDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("WorkingHistoryDynamicFields_", "")) + 1;

            $("#DivWorkingHistoryDynamicFields").append(
                "<div class=\"form-group form-fields WorkingHistoryDynamicFields\" id=\"WorkingHistoryDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <input id=\"txtCompanyName_" + ctr + "\" maxlength=\"255\" class=\"form-control WorkingCompanyNameDynamicFields\" title=\"Company Name\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <input id=\"dpFrom_" + ctr + "\" class=\"form-control WorkingFromDynamicFields\" title=\"From\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <input id=\"dpTo_" + ctr + "\" class=\"form-control WorkingToDynamicFields\" title=\"To\"> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtPosition_" + ctr + "\" maxlength=\"100\" class=\"form-control WorkingPositionDynamicFields\" title=\"Position\"> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <input id=\"txtReason_" + ctr + "\" maxlength=\"255\" class=\"form-control WorkingReasonDynamicFields\" title=\"Reason for Leaving\"> \
                   </div> \
                 </div>"
            );

            $("#dpFrom_" + ctr).datetimepicker({
                useCurrent: false,
                format: 'MM/YYYY'
            });

            $("#dpTo_" + ctr).datetimepicker({
                useCurrent: false,
                format: 'MM/YYYY'
            });
        },

        GetRelationshipDropDown: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    RelationshipDropDown.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(RelationshipDropDownURL, {}, "", GetSuccessFunction);
        },

        GetDetails: function () {
            var GetSuccessFunction = function (data) {
                console.log(data);
            }
            objEMSCommonJS.GetAjax(GetDetailsURL, {}, "", GetSuccessFunction);
        }
    };
    
     objIndexJS.Initialize();
});