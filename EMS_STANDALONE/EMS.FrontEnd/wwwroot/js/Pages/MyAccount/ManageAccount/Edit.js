var objIndexJS;
const EmployeeEditURL = "/MyAccount/ManageAccount/Edit";
const ChangePasswordURL = "/MyAccount/ManageAccount/Edit?handler=ChangePassword";
const GetFamilyBackgroundURL = "/MyAccount/ManageAccount/Edit?handler=FamilyByEmployeeID";
const RelationshipDropDownURL = "/MyAccount/ManageAccount/Edit?handler=RelationshipDropDown";
const SchoolLevelDropDownURL = "/MyAccount/ManageAccount/Edit?handler=ReferenceValue&RefCode=EMP_SCHOOL_LEVEL";
const EducationalAttainmentDegreeDropDownURL = "/MyAccount/ManageAccount/Edit?handler=ReferenceValue&RefCode=EMP_ED_ATT_DEG";
const EducationalAttainmentStatusDropDownURL = "/MyAccount/ManageAccount/Edit?handler=ReferenceValue&RefCode=EMP_ED_ATT_STAT";
const GetEducationURL = "/MyAccount/ManageAccount/Edit?handler=EducationByEmployeeID";
const GetWorkingHistoryURL = "/MyAccount/ManageAccount/Edit?handler=WorkingHistoryByEmployeeID";
const GetCityMunicipalityDropDownByProvinceURL = "/MyAccount/ManageAccount/Edit?handler=CityMunicipalityDropDownByProvince";
const GetProvinceDropDownByRegionURL = "/MyAccount/ManageAccount/Edit?handler=ProvinceDropDownByRegion";
const GetBarangayDropDownByCityMunicipalityURL = "/MyAccount/ManageAccount/Edit?handler=BarangayDropDownByCityMunicipality";
const FieldsDropDownURL = "/MyAccount/ManageAccount/Edit?handler=FieldsDropDown";
const GetFieldsValueURL = "/MyAccount/ManageAccount/Edit?handler=FieldsValue";
const GetUpdateProfileTagsURL = "/MyAccount/ManageAccount/Edit?handler=UpdateProfileTags";

var RelationshipDropDown = [];
var SchoolLevelDropdown = [];
var EducAttDegreeDropdown = [];
var EducAttStatusDropDown = [];
var FieldsDropDown = [];

var UpdateProfileTags = "";

$(document).ready(function () {
    objIndexJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            s.GetFieldsDropDown();

            $(".form-control").attr("readonly", false);
            $('input[type="checkbox"]').prop('disabled', false);
            

            $(".tablinks").first().click();

            // Reset Tab First Load tags
            personalInfoTabFirstLoad = false;
            familyBackTabFirstLoad = false;
            educationTabFirstLoad = false;
            WorkHisTabFirstLoad = false;

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabEditPersonalInformation');
            $(".tablinks").first().click();



            var GetSuccessFunction = function (data) {
                UpdateProfileTags = data.Result[0].Text;
            }
            objEMSCommonJS.GetAjax(GetUpdateProfileTagsURL, {}, "", GetSuccessFunction);
        },

        EditSuccessFunction: function () {
            console.log("done");
        },

        ElementBinding: function () {
            var s = this;

            NumberDashOnly($("#txtContactPersonNumber"));
            NumberDashOnly($("#txtPagibigNumber"));
            NumberDashOnly($("#txtPhilhealthNumber"));
            NumberDashOnly($("#txtSSSNumber"));
            NumberDashOnly($("#txtTIN"));

            $("#dpBirthDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });
            $("#ddlPSGCRegion").change(function () {
                GenerateDropdownValues(GetProvinceDropDownByRegionURL + "&Code=" + $("#ddlPSGCRegion").val(),
                    "ddlPSGCProvince", "Value", "Text", "", "", false);

                if ($("#ddlPSGCRegion").val() != "") {
                    $("input[name='Employee.PersonalInformation.PSGCRegionCode']").val($("#ddlPSGCRegion").val());
                }

                //reset dropdown values
                else {
                    //city/municipality
                    GenerateDropdownValues(GetCityMunicipalityDropDownByProvinceURL + "&Code=" + "",
                        "ddlPSGCCityMunicipality", "Value", "Text", "", "", false);
                    //barangay
                    GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + "",
                        "ddlPSGCBarangay", "Value", "Text", "", "", false);
                }
            });

            $("#ddlPSGCProvince").change(function () {
                GenerateDropdownValues(GetCityMunicipalityDropDownByProvinceURL + "&Code=" + $("#ddlPSGCProvince").val(),
                    "ddlPSGCCityMunicipality", "Value", "Text", "", "", false);

                if ($("#ddlPSGCProvince").val() != "") {
                    $("input[name='Employee.PersonalInformation.PSGCProvinceCode']").val($("#ddlPSGCProvince").val());
                }

                //reset dropdown values
                else {
                    //barangay
                    GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + "",
                        "ddlPSGCBarangay", "Value", "Text", "", "", false);
                }
            });

            $("#ddlPSGCCityMunicipality").change(function () {
                GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + $("#ddlPSGCCityMunicipality").val(),
                    "ddlPSGCBarangay", "Value", "Text", "", "", false);

                if ($("#ddlPSGCCityMunicipality").val() != "") {
                    $("input[name='Employee.PersonalInformation.PSGCCityMunicipalityCode']").val($("#ddlPSGCCityMunicipality").val());
                }
            });

            $("#ddlPSGCBarangay").change(function () {
                if ($("#ddlPSGCBarangay").val() != "") {
                    $("input[name='Employee.PersonalInformation.PSGCBarangayCode']").val($("#ddlPSGCBarangay").val());
                }
            });

            //$("#txtEmail").on("keyup", function () {
            //    $("#divEmployeeErrorMessage").html("");

            //    if (!ValidEmail($("#txtEmail"))) {
            //        $("#txtEmail").addClass("errMessage");
            //        $("#txtEmail").focus();
            //        $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Email Address is invalid</li></label><br />");
            //    }
            //    return false;
            //});

            $("#txtCorporateEmail").on("keyup", function () {
                $("#divEmployeeErrorMessage").html("");

                if (!ValidEmail($("#txtCorporateEmail"))) {
                    $("#txtCorporateEmail").addClass("errMessage");
                    $("#txtCorporateEmail").focus();
                    $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Corporate Email is invalid</li></label><br />");
                }
                return false;
            });

            $("#txtOfficeMobile").on("keyup", function () {
                $("#divEmployeeErrorMessage").html("");

                if (!validMobileNo($("#txtOfficeMobile"))) {
                    $("#txtOfficeMobile").addClass("errMessage");
                    $("#txtOfficeMobile").focus();
                    $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Office Mobile Number is invalid</li></label><br />");
                }
                else
                    $("#txtOfficeMobile").removeClass("errMessage");

                if ($("#txtOfficeMobile").val().length == 4)
                    $("#txtOfficeMobile").val($("#txtOfficeMobile").val() + "-");

                return false;
            });

            $("#txtCellphoneNumber").on("keyup", function () {
                $("#divEmployeeErrorMessage").html("");

                if (!validMobileNo($("#txtCellphoneNumber"))) {
                    $("#txtCellphoneNumber").addClass("errMessage");
                    $("#txtCellphoneNumber").focus();
                    $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Cellphone Number is invalid</li></label><br />");
                }
                else
                    $("#txtCellphoneNumber").removeClass("errMessage");

                if ($("#txtCellphoneNumber").val().length == 4)
                    $("#txtCellphoneNumber").val($("#txtCellphoneNumber").val() + "-");

                return false;
            });

            $("#txtContactPersonNumber").on("keyup", function () {
                $("#divEmployeeErrorMessage").html("");

                if (!validMobileNo($("#txtContactPersonNumber"))) {
                    $("#txtContactPersonNumber").addClass("errMessage");
                    $("#txtContactPersonNumber").focus();
                    $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Cellphone Number is invalid</li></label><br />");
                }
                else
                    $("#txtContactPersonNumber").removeClass("errMessage");

                if ($("#txtContactPersonNumber").val().length == 4)
                    $("#txtContactPersonNumber").val($("#txtContactPersonNumber").val() + "-");

                return false;
            });

            $("#txtSSSNumber").on("keyup, keydown, keypress", function () {
                formatSSSNumber($("#txtSSSNumber"));
            });

            $("#txtPagibigNumber").on("keyup, keydown, keypress", function () {
                addHyphen($("#txtPagibigNumber"), 4);
            });

            $("#txtPhilhealthNumber").on("keyup, keydown, keypress", function () {
                addHyphen($("#txtPhilhealthNumber"), 4);
            });

            $("#txtTIN").on("keyup, keydown, keypress", function () {
                addHyphen($("#txtTIN"), 3);
            });

            $("#dpDateScheduled, #dpDateCompleted").datetimepicker({
                useCurrent: true,
                format: 'MM/DD/YYYY',
            });

            $("#btnBack").click(function () {
                LoadPartial(EmployeeViewURL + "?ID=" + objIndexJS.ID, "divEmployeeBodyModal");
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , EmployeeDeleteURL + '?ID=' + objIndexJS.ID\
                    , {} \
                    , '#divEmployeeErrorMessage' \
                    , '#btnDelete' \
                    , objIndexJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnSave").click(function () {
                var isNoBlankFunction = function () {

                    return true;
                }
                if ($("#hdnHasDataPrivacy").val() == "false") {
                    $("#tabPersonalInformation .form-control").removeClass("required-field");
                    $("#tabCompensation .form-control").removeClass("required-field");
                    $("#tabFamilyBackground .form-control").removeClass("required-field");
                }

                if (objEMSCommonJS.ValidateBlankFields("#frmEmployee", "#divEmployeeErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeEditURL \
                        , objIndexJS.GetFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#btnSave' \
                        , objIndexJS.EditSuccessFunction);",
                        "function");
                }
                else {
                    if ($("#tabPersonalInformation .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Personal Information')").parent("button"), 'tabPersonalInformation');
                    }
                    else if ($("#tabFamilyBackground .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Family Background')").parent("button"), 'tabFamilyBackground');
                    }
                    else if ($("#tabEducation .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Education')").parent("button"), 'tabEducation');
                    }
                    else if ($("#tabWorkingHistory .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Working History')").parent("button"), 'tabWorkingHistory');
                    }
                }
            });

            $("#btnOnboardingWorkflowTransactionSave").click(function () {

                if (objEMSCommonJS.ValidateBlankFields("#tabOnboarding", "#divEmployeeErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SaveOnboardingWorkflowURL \
                        , objIndexJS.GetWorkflowTransactionFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#btnOnboardingWorkflowTransactionSave' \
                        , objIndexJS.OnboardingWorkflowEditSuccessFunction);", "function");
                }
            });

            $("#ddlStep").change(function () {
                if ($(this).val() != "") {
                    var GetResultTypeFunction = function (data) {
                        var GetSuccessFunction = function (data) {
                            $("#ddlResult option").remove();
                            objEMSCommonJS.PopulateDropDown("#ddlResult", data.Result);
                        };
                        objEMSCommonJS.GetAjax(ReferenceValueURL + "&RefCode=" + data.Result.ResultType, {}, "", GetSuccessFunction);
                    };

                    objEMSCommonJS.GetAjax(OnboardingWorkflowStepURL + "&WorkflowCode=" + $("#hdnWorkflowCode").val() + "&StepCode=" + $(this).val()
                        , {}, "", GetResultTypeFunction);
                }
                else {
                    $("#ddlResult option").remove();
                }

            });

            $("#btnAddEmployeeRovingFields").click(function () {
                $(".RovingOrgGroupDynamicFields").addClass("required-field");
                $(".RovingPositionDynamicFields").addClass("required-field");
                var fields = $("#DivEmployeeRovingDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddEmployeeRovingDynamicFields();
            });

            $("#btnAddFields").click(function () {
                var fields = $("#DivDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddDynamicFields();
            });

            $("#btnAddFamilyFields").click(function () {
                //$(".FamilyNameDynamicFields").addClass("required-field");
                //$(".FamilyRelationshipDynamicFields").addClass("required-field");

                var fields = $("#DivFamilyDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddFamilyDynamicFields();
            });

            $("#btnAddWorkingHistoryFields").click(function () {
                $(".WorkingCompanyNameDynamicFields").addClass("required-field");
                $(".WorkingFromDynamicFields").addClass("required-field");
                $(".WorkingToDynamicFields").addClass("required-field");
                $(".WorkingPositionDynamicFields").addClass("required-field");
                $(".WorkingReasonDynamicFields").addClass("required-field");
                var fields = $("#DivWorkingHistoryDynamicFields .required-field");
                var addNewFields = true;
                //fields.each(function (n, element) {
                //    if ($(this).val() == "") {
                //        $(this).focus();
                //        addNewFields = false;
                //        return false;
                //    }
                //});
                if (addNewFields)
                    s.AddWorkingHistoryDynamicFields();
            });
            $("#btnAddEducationFields").click(function () {
                var fields = $("#DivEducationDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddEducationDynamicFields();
            });

            $(".tablinks").find("span:contains('Family Background')").parent("button").click(function () {
                if (!familyBackTabFirstLoad) {
                    s.GetRelationshipDropDown();
                    s.LoadFamilyBackground();
                    if ($('#DivFamilyDynamicFields').children().length == 0) {
                        $("#btnAddFamilyFields").click();
                    }
                    familyBackTabFirstLoad = true;
                    $("#hdnIsViewedFamilyBackground").val(true);
                }
            });

            $(".tablinks").find("span:contains('Education')").parent("button").click(function () {
                if (!educationTabFirstLoad) {
                    s.GetSchoolLevelDropDown();
                    s.GetEducAttDegreeDropDown();
                    s.GetEducAttStatusDropDown();
                    s.LoadEducationBackground();
                    if ($('#DivEducationDynamicFields').children().length == 0) {
                        $("#btnAddEducationFields").click();
                    }
                    educationTabFirstLoad = true;
                    $("#hdnIsViewedEducation").val(true);
                }
            });

            $(".tablinks").find("span:contains('Working History')").parent("button").click(function () {
                if (!WorkHisTabFirstLoad) {
                    s.LoadWorkingHistory();
                    if ($('#DivWorkingHistoryDynamicFields').children().length == 0) {
                        $("#btnAddWorkingHistoryFields").click();
                    }
                    WorkHisTabFirstLoad = true;
                    $("#hdnIsViewedWorkingHistory").val(true);
                }
            });

            $("#btnCancel").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_TO_CLOSE,
                    "objIndexJS.CancelEditProfile()",
                    "function");
            });
        },
        CancelEditProfile: function () {
            window.location = "/myaccount/manageaccount";
        },
        ChangePasswordSuccessFunction: function (data) {
            ModalAlertRedirect(MODAL_HEADER, data.Result, '/Login?handler=Logout');
            $("#frmManageAcountChangePassword").trigger("reset");
        },

        GetFieldsDropDown: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    FieldsDropDown.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(FieldsDropDownURL, {}, "", GetSuccessFunction);
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

        AddDynamicFields: function () {
            var s = this;
            var htmlId = $(".DynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("DynamicFields_", "")) + 1;

            /*<label class=\"control-label block-label lblPhoneNum\">+63</label> \*/

            $("#DivDynamicFields").append(
                "<div class=\"form-group form-fields DynamicFields\" id=\"DynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                       <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objIndexJS.RemoveDynamicFields('#DynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                   </div> \
                   <div class=\"col-md-2-5 text-align-center no-padding\"> \
                       <select id=\"ddlFields_" + ctr + "\" title=\"Fields\" class=\"form-control required-field EditFieldsDynamicFields\"></select> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtOldValue_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field EditOldDynamicFields\" title=\"Old Value\" disabled> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\" id=\"DynamicNewValue_"+ctr+"\"> \
                   </div> \
                 </div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlFields_" + ctr, FieldsDropDown);

            $("#ddlFields_" + ctr).on("change", function () {
                var GetSuccessFunction = function (data) {
                    if (JSON.parse(UpdateProfileTags)["InputText"].includes($("#ddlFields_" + ctr).val())) {
                        $("#txtOldValue_" + ctr).val(data.Result["PersonalInformation"][$("#ddlFields_" + ctr).val()]);
                        $("#DynamicNewValue_" + ctr).html("<input id=\"txtNewValue_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field EditNewDynamicFields\" title=\"New Value\"> ");

                        $("#txtNewValue_" + ctr).focus();

                        $("#txtNewValue_" + ctr).keyup(function () {
                            $("#txtNewValue_" + ctr).val($("#txtNewValue_" + ctr).val().toUpperCase());
                        });
                    }
                    if (JSON.parse(UpdateProfileTags)["InputNumber"].includes($("#ddlFields_" + ctr).val())) {
                        $("#txtOldValue_" + ctr).val(data.Result["PersonalInformation"][$("#ddlFields_" + ctr).val()]);
                        $("#txtOldValue_" + ctr).val() == (null || "") ? "" : $("#txtOldValue_" + ctr).val($("#txtOldValue_" + ctr).val().slice(0, 4) + "-" + $("#txtOldValue_" + ctr).val().substr(4));

                        $("#DynamicNewValue_" + ctr).html("<input id=\"txtNewValue_" + ctr + "\" maxlength=\"12\" class=\"form-control required-field EditNewDynamicFields\" title=\"New Value\"> ");

                        $("#txtNewValue_" + ctr).focus();

                        NumberOnly($("#txtNewValue_" + ctr));

                        $("#txtNewValue_" + ctr).on("keyup", function () {
                            $("#divEmployeeErrorMessage").html("");

                            if (!validMobileNo($("#txtNewValue_" + ctr))) {
                                $("#txtNewValue_" + ctr).addClass("errMessage");
                                $("#txtNewValue_" + ctr).focus();
                                $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Cellphone Number is invalid</li></label><br />");
                            }
                            else
                                $("#txtNewValue_" + ctr).removeClass("errMessage");

                            if ($("#txtNewValue_" + ctr).val().length == 4)
                                $("#txtNewValue_" + ctr).val($("#txtNewValue_" + ctr).val() + "-");
                            return false;
                        });
                    }

                    if (JSON.parse(UpdateProfileTags)["Select"].includes($("#ddlFields_" + ctr).val())) {
                        $("#txtOldValue_" + ctr).val(data.Result["PersonalInformation"][$("#ddlFields_" + ctr).val()]);
                        $("#DynamicNewValue_" + ctr).html("<select id=\"txtNewValue_" + ctr + "\" title=\"New Value\" class=\"form-control required-field EditNewDynamicFields\"></select>");
                    };
                };
                objEMSCommonJS.GetAjax(GetFieldsValueURL, {}, "", GetSuccessFunction);
            });

        },

        AddFamilyDynamicFields: function () {
            var s = this;
            var htmlId = $(".FamilyDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("FamilyDynamicFields_", "")) + 1;

            /*<label class=\"control-label block-label lblPhoneNum\">+63</label> \*/

            $("#DivFamilyDynamicFields").append(
                "<div class=\"form-group form-fields FamilyDynamicFields\" id=\"FamilyDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                       <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objIndexJS.RemoveDynamicFields('#FamilyDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <input id=\"txtName_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field FamilyNameDynamicFields\" title=\"Name\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <select id=\"ddlRelationship_" + ctr + "\" title=\"Relationship\" class=\"form-control required-field FamilyRelationshipDynamicFields\"></select> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <div style = \"overflow:hidden;\" > \
                           <input type=\"tel\" id=\"txtNumber_" + ctr + "\" class=\"form-control FamilyNumberDynamicFields\" maxlength=\"12\" title=\"Cellphone Number\" placeholder=\"09xx-xxxxxxx\" /> \
                       </div> \
                   </div> \
                    <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <input id=\"dpBirthDate_" + ctr + "\" maxlength=\"10\" class=\"form-control required-field FamilyBirthDateDynamicFields\" title=\"Birth Date\"> \
                   </div> \
                    <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtOccupation_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field FamilyOccupationDynamicFields\" title=\"Occupation\"> \
                   </div> \
                    <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtSpouseEmployer_" + ctr + "\" maxlength=\"255\" class=\"form-control FamilySpouseEmployerDynamicFields\" title=\"Spouse Employer\"> \
                   </div> \
                 </div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlRelationship_" + ctr, RelationshipDropDown);

            $("#dpBirthDate_" + ctr).datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#ddlRelationship_" + ctr).change(function () {
                if ($("#ddlRelationship_" + ctr).val() == "SPOUSE") {
                    $("#txtSpouseEmployer_" + ctr).prop("disabled", false);
                    $("#txtSpouseEmployer_" + ctr).addClass("required-field", false);
                }
                else {
                    $("#txtSpouseEmployer_" + ctr).prop("disabled", true);
                    $("#txtSpouseEmployer_" + ctr).removeClass("required-field", false);
                }
            });

            //$("#txtNumber_" + ctr).on("keyup", function () {
            //    $("#divEmployeeErrorMessage").html("");

            //    if (!validMobileNo($("#txtNumber_" + ctr))) {
            //        $("#txtNumber_" + ctr).addClass("errMessage");
            //        $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Contact Number is invalid</li></label><br />");
            //    }
            //});

            NumberOnly($("#txtNumber_" + ctr));

            $("#txtNumber_" + ctr).on("keyup", function () {

                $("#divEmployeeErrorMessage").html("");

                if (!validMobileNo($("#txtNumber_" + ctr))) {
                    $("#txtNumber_" + ctr).addClass("errMessage");
                    $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Contact Number is invalid</li></label><br />");
                }
                else
                    $("#txtNumber_" + ctr).removeClass("errMessage");

                if ($("#txtNumber_" + ctr).val().length == 4)
                    $("#txtNumber_" + ctr).val($("#txtNumber_" + ctr).val() + "-");
            });
        },

        AddEducationDynamicFields: function () {
            var s = this;
            var htmlId = $(".EducationDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("EducationDynamicFields_", "")) + 1;

            $("#DivEducationDynamicFields").append(
                "<div class=\"form-group form-fields EducationDynamicFields\" id=\"EducationDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                       <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objIndexJS.RemoveDynamicFields('#EducationDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtSchool_" + ctr + "\" maxlength=\"255\" class=\"form-control EducationSchoolDynamicFields\" title=\"School\"> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtAddress_" + ctr + "\" maxlength=\"255\" class=\"form-control EducationAddressDynamicFields\" title=\"Address\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <select id=\"ddlLevel_" + ctr + "\" title=\"Level\" class=\"form-control EducationLevelDynamicFields\"></select> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <input id=\"txtCourse_" + ctr + "\" maxlength=\"255\" class=\"form-control EducationCourseDynamicFields\" title=\"Course\"> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input id=\"dpYearFrom_" + ctr + "\" maxlength=\"5\" class=\"form-control EducationFromDynamicFields\" title=\"From\"> \
                   </div> \
                    <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input id=\"dpYearTo_" + ctr + "\" maxlength=\"5\" class=\"form-control EducationToDynamicFields\" title=\"To\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <select id=\"ddlEducationalAttainmentDegree_" + ctr + "\" title=\"Educational Attainment Degree\" class=\"form-control EducationEducationalAttainmentDegreeDynamicFields\"></select> \
                   </div> \
                    <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <select id=\"ddlEducationalAttainmentStatus_" + ctr + "\" title=\"Educational Attainment Status\" class=\"form-control EducationEducationalAttainmentStatusDynamicFields\"></select> \
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
                       <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objIndexJS.RemoveDynamicFields('#WorkingHistoryDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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

            $("#dpFrom_" + ctr + ", #dpTo_" + ctr).datetimepicker().on('dp.show', function () {
                $('#divEmployeeModal .modal-body').css({ 'overflow': 'visible' });
                $('#divEmployeeModal.modal').css({ 'overflow': 'visible' });
            }).on('dp.hide', function () {
                $('#divEmployeeModal .modal-body').css({ 'overflow': 'auto' });
                $('#divEmployeeModal.modal').css({ 'overflow': 'auto' });
            });
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
                    //document.getElementById("txtNumber_" + idCtr).value = $("#txtNumber_" + idCtr).val().substr(3);

                    $("#txtNumber_" + idCtr).val() == (null || "") ? "" : $("#txtNumber_" + idCtr).val($("#txtNumber_" + idCtr).val().slice(0, 4) + "-" + $("#txtNumber_" + idCtr).val().substr(4));

                    $("#ddlRelationship_" + ctr).change();
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
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetWorkingHistoryURL, input, "", GetSuccessFunction);
        },
        RemoveDynamicFields: function (id) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },

        GetFormData: function () {

            var formData = new FormData($('#frmEmployee').get(0));
            //CONTACT PERSON NUMBER

            $(".EditFieldsDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("UpdateProfileForm.UpdateProfile[" + index + "].Fields", $(this).val());
                }
            });
            $(".EditOldDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("UpdateProfileForm.UpdateProfile[" + index + "].OldValue", $(this).val());
                }
            });
            $(".EditNewDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("UpdateProfileForm.UpdateProfile[" + index + "].NewValue", $(this).val());
                }
            });


            $(".FamilyNameDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeFamilyList[" + index + "].Name", $(this).val());
                }
            });

            $(".FamilyRelationshipDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeFamilyList[" + index + "].Relationship", $(this).val());
                }
            });

            $(".FamilyNumberDynamicFields").each(function (index) {
                formData.append("Employee.EmployeeFamilyList[" + index + "].ContactNumber", /*"+63" +*/ $(this).val().replace(/-/g, ""));
            });

            $(".FamilyBirthDateDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeFamilyList[" + index + "].BirthDate", $(this).val());
                }
            });
            $(".FamilyOccupationDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeFamilyList[" + index + "].Occupation", $(this).val());
                }
            });
            $(".FamilySpouseEmployerDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeFamilyList[" + index + "].SpouseEmployer", $(this).val());
                }
            });

            $(".EducationSchoolDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeEducationList[" + index + "].School", $(this).val());
                }
            });

            $(".EducationAddressDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeEducationList[" + index + "].SchoolAddress", $(this).val());
                }
            });
            $(".EducationLevelDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeEducationList[" + index + "].SchoolLevelCode", $(this).val());
                }
            });
            $(".EducationCourseDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeEducationList[" + index + "].Course", $(this).val());
                }
            });
            $(".EducationFromDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeEducationList[" + index + "].YearFrom", $(this).val());
                }
            });
            $(".EducationToDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeEducationList[" + index + "].YearTo", $(this).val());
                }
            });
            $(".EducationEducationalAttainmentDegreeDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeEducationList[" + index + "].EducationalAttainmentDegreeCode", $(this).val());
                }
            });
            $(".EducationEducationalAttainmentStatusDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeEducationList[" + index + "].EducationalAttainmentStatusCode", $(this).val());
                }
            });

            $(".WorkingCompanyNameDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeWorkingHistoryList[" + index + "].CompanyName", $(this).val());
                }
            });

            $(".WorkingFromDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeWorkingHistoryList[" + index + "].From", $(this).val());
                }
            });

            $(".WorkingToDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeWorkingHistoryList[" + index + "].To", $(this).val());
                }
            });

            $(".WorkingPositionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeWorkingHistoryList[" + index + "].Position", $(this).val());
                }
            });

            $(".WorkingReasonDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeWorkingHistoryList[" + index + "].ReasonForLeaving", $(this).val());
                }
            });



            return formData;
        },
    };

    objIndexJS.Initialize();
});