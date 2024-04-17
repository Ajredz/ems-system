var ObjEmployeeDetails;

const EmployeeEditURL = "/EmployeeProfile/EmployeeDetails";
const GetProvinceDropDownByRegionURL = "/EmployeeProfile/EmployeeDetails?handler=ProvinceDropDownByRegion";
const GetCityMunicipalityDropDownByProvinceURL = "/EmployeeProfile/EmployeeDetails?handler=CityMunicipalityDropDownByProvince";
const GetBarangayDropDownByCityMunicipalityURL = "/EmployeeProfile/EmployeeDetails?handler=BarangayDropDownByCityMunicipality";
const RelationshipDropDownURL = "/EmployeeProfile/EmployeeDetails?handler=RelationshipDropDown";
const GetFamilyBackgroundURL = "/EmployeeProfile/EmployeeDetails?handler=FamilyByEmployeeID";
const SchoolLevelDropDownURL = "/EmployeeProfile/EmployeeDetails?handler=ReferenceValue&RefCode=EMP_SCHOOL_LEVEL";
const EducationalAttainmentDegreeDropDownURL = "/EmployeeProfile/EmployeeDetails?handler=ReferenceValue&RefCode=EMP_ED_ATT_DEG";
const EducationalAttainmentStatusDropDownURL = "/EmployeeProfile/EmployeeDetails?handler=ReferenceValue&RefCode=EMP_ED_ATT_STAT";
const GetEducationURL = "/EmployeeProfile/EmployeeDetails?handler=EducationByEmployeeID";
const GetWorkingHistoryURL = "/EmployeeProfile/EmployeeDetails?handler=WorkingHistoryByEmployeeID";

var RelationshipDropDown = [];
var SchoolLevelDropdown = [];
var EducAttDegreeDropdown = [];
var EducAttStatusDropDown = [];

var personalInfoTabFirstLoad = false;
var educationTabFirstLoad = false;
var familyBackTabFirstLoad = false;
var WorkHisTabFirstLoad = false;

$(document).ready(function () {
    ObjEmployeeDetails = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            var s = this;
            s.ElementBinding();

            personalInfoTabFirstLoad = false;
            familyBackTabFirstLoad = false;
            educationTabFirstLoad = false;
            WorkHisTabFirstLoad = false;

            $("#txtOfficeMobile").val() == (null || "") ? "" : $("#txtOfficeMobile").val($("#txtOfficeMobile").val().slice(0, 4) + "-" + $("#txtOfficeMobile").val().substr(4));
            $("#txtCellphoneNumber").val() == (null || "") ? "" : $("#txtCellphoneNumber").val($("#txtCellphoneNumber").val().slice(0, 4) + "-" + $("#txtCellphoneNumber").val().substr(4));
            $("#txtContactPersonNumber").val() == (null || "") ? "" : $("#txtContactPersonNumber").val($("#txtContactPersonNumber").val().slice(0, 4) + "-" + $("#txtContactPersonNumber").val().substr(4));

            $("#btnAddFamilyFields,#btnAddEducationFields,#btnAddWorkingHistoryFields,.reqField").hide();
            
            formatSSSNumber($("#txtSSSNumber"));
            addHyphen($("#txtPagibigNumber"), 4);
            addHyphen($("#txtPhilhealthNumber"), 4);
            addHyphen($("#txtTIN"), 3);
            $('#ddlCitizenshipCode option[value="FILIPINO"]').insertAfter('#ddlCitizenshipCode option[value=""]');
            $('#ddlNationalityCode option[value="FILIPINO"]').insertAfter('#ddlNationalityCode option[value=""]');
            $('#ddlReligionCode option[value="ROMCAT"]').insertAfter('#ddlReligionCode option[value=""]');
            $('#ddlCivilStatusCode option[value="MARRIED"]').insertAfter('#ddlCivilStatusCode option[value=""]');
            $('#ddlCivilStatusCode option[value="SINGLE"]').insertAfter('#ddlCivilStatusCode option[value=""]');

            $(".tablinks").find("span:contains('Background')").parent("button").click();
            $(".tablinks").find("span:contains('Education')").parent("button").click();
            $(".tablinks").find("span:contains('Working History')").parent("button").click();

            objEMSCommonJS.ChangeTab($(".tablinks").first(), "tabPersonalInformation", "");
            $(".tablinks").first().click();
            
        },
        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtContactPersonNumber,#txtWeightLBS,#txtHeightCM"));
            NumberDashOnly($("#txtPagibigNumber"));
            NumberDashOnly($("#txtPhilhealthNumber"));
            NumberDashOnly($("#txtSSSNumber"));
            NumberDashOnly($("#txtTIN"));
            SetToUpper("txtLastName");
            SetToUpper("txtFirstName");
            SetToUpper("txtMiddleName");
            SetToUpper("txtSuffix");
            SetToUpper("txtNickname");
            SetToUpper("txtAddressLine1");
            SetToUpper("txtAddressLine2");
            SetToUpper("txtBirthPlace");
            SetToUpper("txtContactPersonName");
            SetToUpper("txtContactPersonAddress");

            $("#dpBirthDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#btnEdit").on("click", function () {
                $(".form-control").attr("readonly", false);
                $('input[type="checkbox"]').prop('disabled', false);
                $('select').prop('disabled', false);
                $("#btnAddFamilyFields,#btnAddEducationFields,#btnAddWorkingHistoryFields,#btnSave,#btnCancel,.reqField").show();
                $("#btnEdit").hide();
                objEMSCommonJS.ChangeTab($(".tablinks").first(), "tabPersonalInformation", "");
            });

            $("#btnCancel").on("click", function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_TO_CLOSE,
                    "ObjEmployeeDetails.CancelEdit()",
                    "function");
            });

            $("#btnSave").on("click", function () {
                var isNoBlankFunction = function () {
                    
                    return true;
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmEmployeeDetails", "#divEmployeeErrorMessage", isNoBlankFunction)) {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                        , EmployeeEditURL \
                        , ObjEmployeeDetails.GetFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#btnSave' \
                        , ObjEmployeeDetails.EditSuccessFunction);",
                    "function");
                }
                else {
                    if ($("#tabPersonalInformation .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Personal')").parent("button"), 'tabPersonalInformation').click();
                    }
                    else if ($("#tabFamilyBackground .errMessage").length > 0) {
                        $(".tablinks").find("span:contains('Background')").parent("button").click();
                    }
                    else if ($("#tabEducation .errMessage").length > 0) {
                        $(".tablinks").find("span:contains('Education')").parent("button").click();
                    }
                    else if ($("#tabWorkingHistory .errMessage").length > 0) {
                        $(".tablinks").find("span:contains('Working History')").parent("button").click();
                    }
                }
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

            $(".tablinks").find("span:contains('Background')").parent("button").click(function () {
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
            
        },
        EditSuccessFunction: function () {
            $("#btnSave,#btnCancel").hide();
            location.reload();
        },
        CancelEdit: function () {
            location.reload();
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
        AddFamilyDynamicFields: function () {
            var s = this;
            var htmlId = $(".FamilyDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("FamilyDynamicFields_", "")) + 1;

            /*<label class=\"control-label block-label lblPhoneNum\">+63</label> \*/

            $("#DivFamilyDynamicFields").append(
                "<div class=\"form-group form-fields FamilyDynamicFields\" id=\"FamilyDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                       <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;ObjEmployeeDetails.RemoveDynamicFields('#FamilyDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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
                       <input id=\"txtSpouseEmployer_" + ctr + "\" maxlength=\"255\" class=\"form-control FamilySpouseEmployerDynamicFields\" title=\"Spouse Employer\"> \
                   </div> \
                 </div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlRelationship_" + ctr, RelationshipDropDown);

            SetToUpper("txtName_" + ctr);
            SetToUpper("txtOccupation_" + ctr);

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
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;ObjEmployeeDetails.RemoveDynamicFields('#EducationDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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

            SetToUpper("txtSchool_" + ctr);
            SetToUpper("txtAddress_" + ctr);
            SetToUpper("txtCourse_" + ctr);

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
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;ObjEmployeeDetails.RemoveDynamicFields('#WorkingHistoryDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <input id=\"txtCompanyName_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field WorkingCompanyNameDynamicFields\" title=\"Company Name\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <input id=\"dpFrom_" + ctr + "\" class=\"form-control required-field WorkingFromDynamicFields\" title=\"From\"> \
                   </div> \
                   <div class=\"col-md-1-5 text-align-center no-padding\"> \
                       <input id=\"dpTo_" + ctr + "\" class=\"form-control required-field WorkingToDynamicFields\" title=\"To\"> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input id=\"txtPosition_" + ctr + "\" maxlength=\"100\" class=\"form-control required-field WorkingPositionDynamicFields\" title=\"Position\"> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <input id=\"txtReason_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field WorkingReasonDynamicFields\" title=\"Reason for Leaving\"> \
                   </div> \
                 </div>"
            );

            SetToUpper("txtCompanyName_" + ctr);
            SetToUpper("txtPosition_" + ctr);
            SetToUpper("txtReason_" + ctr);

            $("#dpFrom_" + ctr).datetimepicker({
                useCurrent: false,
                format: 'MM/YYYY'
            });

            $("#dpTo_" + ctr).datetimepicker({
                useCurrent: false,
                format: 'MM/YYYY'
            });

            $("#dpFrom_" + ctr + ", #dpTo_" + ctr).datetimepicker().on('dp.show', function () {
                $('.modal-body').css({ 'overflow': 'visible' });
                $('#divEmployeeModal.modal').css({ 'overflow': 'visible' });
            }).on('dp.hide', function () {
                $('.modal-body').css({ 'overflow': 'auto' });
                $('#divEmployeeModal.modal').css({ 'overflow': 'auto' });
            });
        },
        LoadFamilyBackground: function () {
            var s = this;
            $("#DivFamilyDynamicFields").html("");

            var input = { EmployeeID: ObjEmployeeDetails.ID };

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

                    if ($("#btnSave").is(":hidden")) {
                        $("#FamilyDynamicFields_" + idCtr + " span.glyphicon-trash").hide();
                        $("#txtName_" + idCtr).attr("readonly", true);
                        $("#ddlRelationship_" + idCtr).attr("disabled", true);
                        $("#txtNumber_" + idCtr).attr("readonly", true);
                        $("#dpBirthDate_" + idCtr).attr("readonly", true);
                        $("#txtOccupation_" + idCtr).attr("readonly", true);
                        $("#txtSpouseEmployer_" + idCtr).attr("readonly", true);
                    }

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

            var input = { EmployeeID: ObjEmployeeDetails.ID };

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

                    if ($("#btnSave").is(":hidden")) {
                        $("#EducationDynamicFields_" + idCtr + " span.glyphicon-trash").hide();
                        $("#txtSchool_" + idCtr).attr("readonly", true);
                        $("#txtAddress_" + idCtr).attr("readonly", true);
                        $("#ddlLevel_" + idCtr).attr("disabled", true);
                        $("#txtCourse_" + idCtr).attr("readonly", true);
                        $("#dpYearFrom_" + idCtr).attr("readonly", true);
                        $("#dpYearTo_" + idCtr).attr("readonly", true);
                        $("#ddlEducationalAttainmentDegree_" + idCtr).attr("disabled", true);
                        $("#ddlEducationalAttainmentStatus_" + idCtr).attr("disabled", true);
                    }
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

            var input = { EmployeeID: ObjEmployeeDetails.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddWorkingHistoryDynamicFields();
                    $("#txtCompanyName_" + idCtr).val(item.CompanyName);
                    $("#dpFrom_" + idCtr).val(item.From);
                    $("#dpTo_" + idCtr).val(item.To);
                    $("#txtPosition_" + idCtr).val(item.Position);
                    $("#txtReason_" + idCtr).val(item.ReasonForLeaving);

                    if ($("#btnSave").is(":hidden")) {
                        $("#WorkingHistoryDynamicFields_" + idCtr + " span.glyphicon-trash").hide();
                        $("#txtCompanyName_" + idCtr).attr("readonly", true);
                        $("#dpFrom_" + idCtr).attr("readonly", true);
                        $("#dpTo_" + idCtr).attr("readonly", true);
                        $("#txtPosition_" + idCtr).attr("readonly", true);
                        $("#txtReason_" + idCtr).attr("readonly", true);
                    }
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
            var formData = new FormData($('#frmEmployeeDetails').get(0));


            //OFFICEMOBILE
            formData.append("Employee.OfficeMobile", $("#txtOfficeMobile").val().replace(/-/g, ""));
            //CONTACT PERSON NUMBER
            formData.append("Employee.PersonalInformation.ContactPersonNumber", /*"+63" +*/ $("#txtContactPersonNumber").val().replace(/-/g, ""));


            formData.append("Employee.PersonalInformation.CellphoneNumber", /*"+63" + */$("#txtCellphoneNumber").val().replace(/-/g, ""));
            formData.append("Employee.PersonalInformation.SSSNumber", $("#txtSSSNumber").val().replace(/-/g, ""));
            formData.append("Employee.PersonalInformation.TIN", $("#txtTIN").val().replace(/-/g, ""));
            formData.append("Employee.PersonalInformation.PagibigNumber", $("#txtPagibigNumber").val().replace(/-/g, ""));
            formData.append("Employee.PersonalInformation.PhilhealthNumber", $("#txtPhilhealthNumber").val().replace(/-/g, ""));

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
    }
    ObjEmployeeDetails.Initialize();
});