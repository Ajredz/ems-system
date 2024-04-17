var objEmployeeAddJS;
var OrgGroupDropDown = [];
var PositionDropDown = [];
var RelationshipDropDown = [];
var SchoolLevelDropdown = [];
var EducAttDegreeDropdown = [];
var EducAttStatusDropDown = [];

$(document).ready(function () {
    objEmployeeAddJS = {

        Initialize: function () {
            $("#divEmployeeBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divEmployeeModal #btnSave").show();
            $("#divEmployeeModal .form-control:not(#divEmployeeModal #txtCode \
                , #divEmployeeModal #txtDailySalary \
                , #divEmployeeModal #txtHourlySalary \
            )").attr("readonly", false);
            $("#divEmployeeModal #btnDelete, #divEmployeeModal #btnBack").remove();
            objEMSCommonJS.ChangeTab($(".tablinks:visible").first(), "tabPersonalInformation", "#divEmployeeModal");
            $("#divEmployeeModal .tablinks").find("span:contains('Task Checklist')").parent("button").hide();
            $("#divEmployeeModal .tablinks").find("span:contains('Onboarding')").parent("button").hide();
            $("#divEmployeeModal .tablinks").find("span:contains('Accountability')").parent("button").hide();
            $("#divEmployeeModal .tablinks").find("span:contains('Secondary Designation')").parent("button").hide();
            $("#divEmployeeModal .tablinks").find("span:contains('Skills')").parent("button").hide();
            s.GetOrgGroupDropDown();
            s.GetPositionDropDown();
            s.AddEmployeeRovingDynamicFields();
            s.GetRelationshipDropDown();
            s.GetSchoolLevelDropDown();
            s.GetEducAttDegreeDropDown();
            s.GetEducAttStatusDropDown();
            s.AddFamilyDynamicFields();
            s.AddEducationDynamicFields();
            s.AddWorkingHistoryDynamicFields();

            $('#ddlCitizenshipCode option[value="FILIPINO"]').insertAfter('#ddlCitizenshipCode option[value=""]');
            $('#ddlNationalityCode option[value="FILIPINO"]').insertAfter('#ddlNationalityCode option[value=""]');
            $('#ddlReligionCode option[value="ROMCAT"]').insertAfter('#ddlReligionCode option[value=""]');
            $('#ddlCivilStatusCode option[value="MARRIED"]').insertAfter('#ddlCivilStatusCode option[value=""]');
            $('#ddlCivilStatusCode option[value="SINGLE"]').insertAfter('#ddlCivilStatusCode option[value=""]');
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmEmployee").trigger("reset");
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabPersonalInformation');
            $("#DivFamilyDynamicFields").html("");
            $("#DivEducationDynamicFields").html("");
            objEmployeeAddJS.AddFamilyDynamicFields();
            objEmployeeAddJS.AddEducationDynamicFields();
            $("#DivWorkingHistoryDynamicFields").html("");
            objEmployeeAddJS.AddWorkingHistoryDynamicFields();
        },

        ElementBinding: function () {
            var s = this;

            Code($("#txtCode"));
            PreventSpace($("#txtCode"));
            NumberOnly($("#txtOldEmployeeID"));
            NumberDashOnly($("#txtPagibigNumber"));
            NumberDashOnly($("#txtPhilhealthNumber"));
            NumberDashOnly($("#txtSSSNumber"));
            NumberDashOnly($("#txtTIN"));

            NumberOnly($("#txtOfficeMobile"));
            NumberOnly($("#txtCellphoneNumber"));
            NumberOnly($("#txtContactPersonNumber"));

            $('#txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });

            $("#dpDateHired, #dpBirthDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#divEmployeeModal #ddlPSGCRegion").change(function () {
                GenerateDropdownValues(GetProvinceDropDownByRegionURL + "&Code=" + $("#divEmployeeModal #ddlPSGCRegion").val(),
                    "divEmployeeModal #ddlPSGCProvince", "Value", "Text", "", "", false);

                if ($("#divEmployeeModal #ddlPSGCRegion").val() != "") {
                    $("input[name='Employee.PersonalInformation.PSGCRegionCode']").val($("#divEmployeeModal #ddlPSGCRegion").val());
                }

                //reset dropdown values
                else {
                    //city/municipality
                    GenerateDropdownValues(GetCityMunicipalityDropDownByProvinceURL + "&Code=" + "",
                        "divEmployeeModal #ddlPSGCCityMunicipality", "Value", "Text", "", "", false);
                    //barangay
                    GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + "",
                        "divEmployeeModal #ddlPSGCBarangay", "Value", "Text", "", "", false);
                }
            });

            $("#divEmployeeModal #ddlPSGCProvince").change(function () {
                GenerateDropdownValues(GetCityMunicipalityDropDownByProvinceURL + "&Code=" + $("#divEmployeeModal #ddlPSGCProvince").val(),
                    "divEmployeeModal #ddlPSGCCityMunicipality", "Value", "Text", "", "", false);

                if ($("#divEmployeeModal #ddlPSGCProvince").val() != "") {
                    $("input[name='Employee.PersonalInformation.PSGCProvinceCode']").val($("#divEmployeeModal #ddlPSGCProvince").val());
                }

                //reset dropdown values
                else {
                    //barangay
                    GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + "",
                        "divEmployeeModal #ddlPSGCBarangay", "Value", "Text", "", "", false);
                }
            });

            $("#divEmployeeModal #ddlPSGCCityMunicipality").change(function () {
                GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + $("#divEmployeeModal #ddlPSGCCityMunicipality").val(),
                    "divEmployeeModal #ddlPSGCBarangay", "Value", "Text", "", "", false);

                if ($("#divEmployeeModal #ddlPSGCCityMunicipality").val() != "") {
                    $("input[name='Employee.PersonalInformation.PSGCCityMunicipalityCode']").val($("#divEmployeeModal #ddlPSGCCityMunicipality").val());
                }
            });

            $("#divEmployeeModal #ddlPSGCBarangay").change(function () {
                if ($("#divEmployeeModal #ddlPSGCBarangay").val() != "") {
                    $("input[name='Employee.PersonalInformation.PSGCBarangayCode']").val($("#divEmployeeModal #ddlPSGCBarangay").val());
                }
            });

            //$("#divEmployeeModal #txtEmail").on("keyup", function () {
            //    $("#divEmployeeErrorMessage").html("");

            //    if (!ValidEmail($("#txtEmail"))) {
            //        $("#txtEmail").addClass("errMessage");
            //        $("#txtEmail").focus();
            //        $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Email Address is invalid</li></label><br />");
            //    }
            //    return false;
            //});

            $("#divEmployeeModal #txtCorporateEmail").on("keyup", function () {
                $("#divEmployeeErrorMessage").html("");

                if (!ValidEmail($("#txtCorporateEmail"))) {
                    $("#txtCorporateEmail").addClass("errMessage");
                    $("#txtCorporateEmail").focus();
                    $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Corporate Email is invalid</li></label><br />");
                }
                return false;
            });

            $("#divEmployeeModal #txtOfficeMobile").on("keyup", function () {
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

            $("#divEmployeeModal #txtCellphoneNumber").on("keyup", function () {
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

            $("#divEmployeeModal #txtContactPersonNumber").on("keyup", function () {
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

            $("#divEmployeeModal #txtSSSNumber").on("keyup, keydown, keypress", function () {
                formatSSSNumber($("#txtSSSNumber"));
            });

            $("#divEmployeeModal #txtPagibigNumber").on("keyup, keydown, keypress", function () {
                addHyphen($("#txtPagibigNumber"), 4);
            });

            $("#divEmployeeModal #txtPhilhealthNumber").on("keyup, keydown, keypress", function () {
                addHyphen($("#txtPhilhealthNumber"), 4);
            });

            $("#divEmployeeModal #txtTIN").on("keyup, keydown, keypress", function () {
                addHyphen($("#txtTIN"), 3);
            });

            $("#divEmployeeModal #btnSave").click(function () {

                var isNoBlankFunction = function () {
                    //if (!ValidEmail($("#divEmployeeModal #txtEmail"))) {
                    //    $("#divEmployeeModal #txtEmail").addClass("errMessage");
                    //    $("#divEmployeeModal #txtEmail").focus();
                    //    $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Email Address is invalid</li></label><br />");
                    //    return false;
                    //}
                    if ($("#divEmployeeModal #txtCorporateEmail").val() != "" & !ValidEmail($("#divEmployeeModal #txtCorporateEmail"))) {
                        $("#divEmployeeModal #txtCorporateEmail").addClass("errMessage");
                        $("#divEmployeeModal #txtCorporateEmail").focus();
                        $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Corporate Email is invalid</li></label><br />");
                        return false;
                    }
                    else if (!validMobileNo($("#divEmployeeModal #txtCellphoneNumber"))) {
                        $("#divEmployeeModal #txtCellphoneNumber").addClass("errMessage");
                        $("#divEmployeeModal #txtCellphoneNumber").focus();
                        $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Cellphone Number is invalid</li></label><br />");
                        return false;
                    }
                    else if ($('#DivFamilyDynamicFields').children().length != 0) {
                        var isInvalid = 0;
                        $(".FamilyNumberDynamicFields").each(function (index) {
                            var ctr = $(this).prop("id").replace("txtNumber_", "");
                            if (!validMobileNo($(this))) {
                                $("#txtNumber_" + ctr).addClass("errMessage");
                                isInvalid++;
                            }
                        });
                        if (isInvalid > 0) {
                            $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Contact Number is invalid</li></label><br />");
                            return false;
                        }
                        else {
                            $("#divEmployeeErrorMessage").html("");
                            return true;
                        }
                    }
                    else {
                        return true;
                    }

                    //$(".FamilyNumberDynamicFields").each(function (n1, x1) {
                    //    var ctr = $(this).prop("id").replace("txtNumber_", "");
                    //    if (this.value != "") {
                    //        if (!validMobileNo($("#FamilyDynamicFields_" + ctr + " #txtNumber_" + ctr))) {
                    //            $("#FamilyDynamicFields_" + ctr + " #txtNumber_" + ctr).addClass("errMessage");
                    //            $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Contact Number is invalid</li></label><br />");
                    //            return false;
                    //        }
                    //        else {
                    //            $("#FamilyDynamicFields_" + ctr + " #txtNumber_" + ctr).removeClass("errMessage");
                    //            $("#divEmployeeErrorMessage").html("");
                    //            return true;
                    //        }
                    //    }
                    //});
                };

                $("#divEmployeeModal #tabOnboarding .form-control").removeClass("required-field");

                if (objEMSCommonJS.ValidateBlankFields("#frmEmployee", "#divEmployeeErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeAddPostURL \
                        , objEmployeeAddJS.GetFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#divEmployeeModal #btnSave' \
                        , objEmployeeAddJS.AddSuccessFunction);",
                        "function");
                }
                else {
                    if ($("#divEmployeeModal #tabPersonalInformation .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Personal Information')").parent("button"), 'divEmployeeModal #tabPersonalInformation');
                    }
                    else if ($("#divEmployeeModal #tabCompensation .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Compensation')").parent("button"), 'divEmployeeModal #tabCompensation');
                    }
                    else if ($("#divEmployeeModal #tabFamilyBackground .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Family Background')").parent("button"), 'divEmployeeModal #tabFamilyBackground');
                    }
                    else if ($("#divEmployeeModal #tabEducation .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Education')").parent("button"), 'divEmployeeModal #tabEducation');
                    }
                    else if ($("#divEmployeeModal #tabWorkingHistory .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Working History')").parent("button"), 'divEmployeeModal #tabWorkingHistory');
                    }
                    else if ($("#divEmployeeModal #tabEmployeeRoving .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Secondary Designation')").parent("button"), 'divEmployeeModal #tabEmployeeRoving');
                    }
                }

                $("#divEmployeeModal #tabOnboarding .form-control").addClass("required-field");
            });

            $("#divEmployeeModal #btnAddEmployeeRovingFields").click(function () {
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

            $("#divEmployeeModal #btnAddFamilyFields").click(function () {
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

            $("#divEmployeeModal #btnAddEducationFields").click(function () {
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

            $("#divEmployeeModal #btnAddWorkingHistoryFields").click(function () {
                $(".WorkingCompanyNameDynamicFields").addClass("required-field");
                $(".WorkingFromDynamicFields").addClass("required-field");
                $(".WorkingToDynamicFields").addClass("required-field");
                $(".WorkingPositionDynamicFields").addClass("required-field");
                $(".WorkingReasonDynamicFields").addClass("required-field");
                var fields = $("#DivWorkingHistoryDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddWorkingHistoryDynamicFields();
            });

            $("#divEmployeeModal #txtSystemUserName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: SystemUserAutoCompleteURL, // URL 
                        type: "GET",
                        dataType: "json",
                        data: {
                            Term: $("#txtSystemUserName").val(),
                            TopResults: 20
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                response($.map(data.Result, function (item) {
                                    return {
                                        label: item.Description,
                                        value: item.ID
                                    };
                                }))
                            }
                            else {
                                ModalAlert(MODAL_HEADER, data.Result);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            ModalAlert(MODAL_HEADER, jqXHR.responseText);
                        }
                    })
                },
                select: function (event, ui) { // Event - triggers after selection on list
                    if (ui.item.label != null) {
                    }
                    return false;
                },
                change: function (event, ui) { // Event - triggers when the value of the textbox changed
                    if (ui.item == null) {
                        $("#hdnSystemUserID").val(0)
                        $(this).val("");
                    } else {
                        $("#hdnSystemUserID").val(ui.item.value)
                        $(this).val(ui.item.label)
                    }
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            $("#divEmployeeModal #ddlOrgGroup").change(function () {
                if ($(this).val() != "") {
                    var GetSuccessFunction = function (data) {
                        $("#lblRegion").text(data.Result.Region);
                        $("#hdnRegionCode").val(data.Result.RegionCode);
                        $("#txtMonthlySalary").val(data.Result.MonthlyRate);
                        computeDailyHourly();
                        $("#txtMonthlySalary").val($("#txtMonthlySalary").val().commaOnAmount());

                        // AUTOMATE CORPORATE EMAIL
                        $("#txtCorporateEmail").val(objEmployeeListJS.CorporateEmailAutomate($("#ddlOrgGroup :selected").val()));
                    };

                    objEMSCommonJS.GetAjax(GetRegionByOrgGroupIDURL + "&OrgGroupID=" + $(this).val(), {}, "", GetSuccessFunction);
                    s.GetOrgGroupPositionDropDown($(this).val());
                }
                else {
                    $("#lblRegion").text("");
                    $("#hdnRegionCode").val("");
                }

                $("#lblJobClass").text("");
            });

            $("#divEmployeeModal #ddlPosition").change(function () {
                if ($(this).val() != "") {
                    var GetSuccessFunction = function (data) {
                        $("#lblJobClass").text(data.Result.JobClassDescription);
                    };

                    objEMSCommonJS.GetAjax(GetJobClassByPositionIDURL + "&PositionID=" + $(this).val(), {}, "", GetSuccessFunction);
                }
                else {
                    $("#lblJobClass").text("");
                }
            });

            AmountOnly($("#txtMonthlySalary"));

            var computeDailyHourly = function () {
                var m = parseFloat($("#txtMonthlySalary").val().noComma());
                var ddiv = parseFloat($("#hdnWageDailyDivisor").val());
                var hdiv = parseFloat($("#hdnWageHourlyDivisor").val());
                $("#txtDailySalary").val((m / ddiv).toFixed(2).commaOnAmount());
                $("#txtHourlySalary").val((m / ddiv / hdiv).toFixed(2).commaOnAmount());
            };

            $("#txtMonthlySalary").keyup(function () {
                computeDailyHourly();
            });

            $("#txtMonthlySalary").blur(function () {
                if ($("#txtMonthlySalary").val() == "") {
                    $("#txtMonthlySalary").val("0");
                    $("#txtMonthlySalary").val($("#txtMonthlySalary").val().commaOnAmount());
                }
                computeDailyHourly();
            });

            objEMSCommonJS.BindAutoComplete("divEmployeeModal #txtHomeBranch"
                , OrgGroupAutoCompleteURL
                , 20, "divEmployeeModal #hdnHomeBranch", "ID", "Description");

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

            formData.append("Employee.EmployeeCompensation.EmployeeID", $("#divEmployeeModal #hdnID").val());
            formData.append("Employee.EmployeeCompensation.MonthlySalary", $("#tabCompensation #txtMonthlySalary").val());
            formData.append("Employee.EmployeeCompensation.DailySalary", $("#tabCompensation #txtDailySalary").val());
            formData.append("Employee.EmployeeCompensation.HourlySalary", $("#tabCompensation #txtHourlySalary").val());

            //OFFICEMOBILE
            formData.append("Employee.OfficeMobile", /*"+63" +*/ $("#divEmployeeModal #txtOfficeMobile").val().replace(/-/g, ""));
            //CONTACT PERSON NUMBER
            formData.append("Employee.PersonalInformation.ContactPersonNumber", /*"+63" +*/ $("#divEmployeeModal #txtContactPersonNumber").val().replace(/-/g, ""));

            formData.append("Employee.PersonalInformation.CellphoneNumber", /*"+63" + */$("#divEmployeeModal #txtCellphoneNumber").val().replace(/-/g, ""));
            formData.append("Employee.PersonalInformation.SSSNumber", $("#divEmployeeModal #txtSSSNumber").val().replace(/-/g, ""));
            formData.append("Employee.PersonalInformation.TIN", $("#divEmployeeModal #txtTIN").val().replace(/-/g, ""));
            formData.append("Employee.PersonalInformation.PagibigNumber", $("#divEmployeeModal #txtPagibigNumber").val().replace(/-/g, ""));
            formData.append("Employee.PersonalInformation.PhilhealthNumber", $("#divEmployeeModal #txtPhilhealthNumber").val().replace(/-/g, ""));

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
            $(".FamilyNumberDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Employee.EmployeeFamilyList[" + index + "].ContactNumber", /*"+63" +*/ $(this).val().replace(/-/g, ""));
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

        AddEmployeeRovingDynamicFields: function () {
            var s = this;
            var htmlId = $(".EmployeeRovingDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("EmployeeRovingDynamicFields_", "")) + 1;

            $("#DivEmployeeRovingDynamicFields").append(
                "<div class=\"form-group form-fields EmployeeRovingDynamicFields\" id=\"EmployeeRovingDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <label id=\"ddlRovingOrgGroup_" + ctr + "\" title=\"Org Group\" class=\"form-control RovingOrgGroupDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <label id=\"ddlRovingPosition_" + ctr + "\" title=\"Position\" class=\"form-control RovingPositionDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                       <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeAddJS.RemoveDynamicFields('#EmployeeRovingDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                   </div> \
                 </div>"
            );

            // objEMSCommonJS.PopulateDropDown("#ddlRovingOrgGroup_" + ctr, OrgGroupDropDown);
            // objEMSCommonJS.PopulateDropDown("#ddlRovingPosition_" + ctr, PositionDropDown);
        },

        RemoveDynamicFields: function (id) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },

        GetOrgGroupDropDown: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    OrgGroupDropDown.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(OrgGroupDropDownURL, {}, "", GetSuccessFunction);
        },

        GetPositionDropDown: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    PositionDropDown.push(
                        {
                            Value: item.PositionID,
                            Text: item.PositionDescription
                        });
                });
            };

            objEMSCommonJS.GetAjax(PositionDropDownURL, {}, "", GetSuccessFunction);
        },

        AddSystemUser: function (FirstName, MiddleName, LastName, ApplicantID) {
            var parameters = "&FirstName=" + FirstName
                + "&MiddleName=" + MiddleName   
                + "&LastName=" + LastName
                + "&ApplicantID=" + ApplicantID;

            var GetSuccessFunction = function () {
                
            };

            objEMSCommonJS.GetAjax(AddSystemUserURL + parameters, {}, "", GetSuccessFunction);
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
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeAddJS.RemoveDynamicFields('#FamilyDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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
                       <input id=\"txtSpouseEmployer_" + ctr + "\" maxlength=\"255\" class=\"form-control FamilySpouseEmployerDynamicFields\" title=\"Spouse Employer\" disabled> \
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
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeAddJS.RemoveDynamicFields('#EducationDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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
                       <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeAddJS.RemoveDynamicFields('#WorkingHistoryDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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


        GetOrgGroupPositionDropDown: function (ID) {
            var s = this;
            var GetSuccessFunction = function (data) {
                $("#ddlPosition").empty();
                $("#ddlPosition").append('<option value="">- Select an item -</option>');
                $(data).each(function (index, item) {
                    $("#ddlPosition").append($('<option/>', {
                        value: item.PositionID,
                        text: item.PositionDescription
                    }));
                });
            };

            objEMSCommonJS.GetAjax(PositionOrgGroupDropDownURL +"&ID="+ID, {}, "", GetSuccessFunction);
        },
    };

    objEmployeeAddJS.Initialize();
});