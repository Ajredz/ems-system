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

        ID: $("#divEmployeeModal #hdnID").val(),

        Initialize: function () {
            $("#divEmployeeBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divEmployeeModal .form-control:not(#divEmployeeModal #txtCode \
                , #divEmployeeModal #txtMonthlySalary \
                , #divEmployeeModal #txtDailySalary \
                , #divEmployeeModal #txtHourlySalary \
            )").attr("readonly", false);
            $('#divEmployeeBodyModal input[type="checkbox"]').prop('disabled', false);
            $("#divEmployeeModal #btnEdit").hide();
            $("#divEmployeeModal #btnAddPreAccountability, #divEmployeeModal #btnAddAccountability").show();
            
            $("#divEmployeeModal #btnSave, #divEmployeeModal #btnBack, \
                #divEmployeeModal #btnOnboardingWorkflowTransactionSave").show();

            s.GetSystemUserName();
            // Reset Tab First Load tags
            personalInfoTabFirstLoad = false;
            familyBackTabFirstLoad = false;
            educationTabFirstLoad = false;
            WorkHisTabFirstLoad = false;
            SecDesigTabFirstLoad = false;
            logActivityTabFirstLoad = false;
            OnboardTabFirstLoad = false;
            AccountTabFirstLoad = false;
            MovementTabFirstLoad = false;

            EmployeeDeletedMainAttachments = [];
            s.GetAttachment();

            //document.getElementById("txtOfficeMobile").value = $("#txtOfficeMobile").val().substr(3);

            //document.getElementById("txtCellphoneNumber").value = $("#txtCellphoneNumber").val().substr(3);

            $("#txtOfficeMobile").val() == (null || "") ? "" : $("#txtOfficeMobile").val($("#txtOfficeMobile").val().slice(0, 4) + "-" + $("#txtOfficeMobile").val().substr(4));

            $("#txtCellphoneNumber").val() == (null || "") ? "" : $("#txtCellphoneNumber").val($("#txtCellphoneNumber").val().slice(0, 4) + "-" + $("#txtCellphoneNumber").val().substr(4));

            $("#txtContactPersonNumber").val() == (null || "") ? "" : $("#txtContactPersonNumber").val($("#txtContactPersonNumber").val().slice(0, 4) + "-" + $("#txtContactPersonNumber").val().substr(4));

            formatSSSNumber($("#txtSSSNumber"));
            addHyphen($("#txtPagibigNumber"), 4);
            addHyphen($("#txtPhilhealthNumber"), 4);
            addHyphen($("#txtTIN"), 3);
            $('#ddlCitizenshipCode option[value="FILIPINO"]').insertAfter('#ddlCitizenshipCode option[value=""]');
            $('#ddlNationalityCode option[value="FILIPINO"]').insertAfter('#ddlNationalityCode option[value=""]');
            $('#ddlReligionCode option[value="ROMCAT"]').insertAfter('#ddlReligionCode option[value=""]');
            $('#ddlCivilStatusCode option[value="MARRIED"]').insertAfter('#ddlCivilStatusCode option[value=""]');
            $('#ddlCivilStatusCode option[value="SINGLE"]').insertAfter('#ddlCivilStatusCode option[value=""]');

            $("#txtMonthlySalary").val($("#txtMonthlySalary").val().commaOnAmount());
            $("#txtDailySalary").val($("#txtDailySalary").val().commaOnAmount());
            $("#txtHourlySalary").val($("#txtHourlySalary").val().commaOnAmount());

            if ($("#txtCode").val() == "") {
                $("#divEmployeeModal .tablinks").find("span:contains('Background')").parent("button").click();
                $("#divEmployeeModal .tablinks").find("span:contains('Education')").parent("button").click();
                $("#divEmployeeModal .tablinks").find("span:contains('Working History')").parent("button").click();
            }

            if ($("#divEmployeeModal #ddlPSGCProvince").val() != "" & $("#divEmployeeModal #ddlPSGCCityMunicipality").val() == "") {
                $("#divEmployeeModal #ddlPSGCProvince").trigger("change");
            }

            objEMSCommonJS.ChangeTab($("#divEmployeeModal .tablinks").first(), "tabPersonalInformation", "#divEmployeeModal");
            $("#divEmployeeModal .tablinks").first().click();

        },

        ElementBinding: function () {
            var s = this;

            Code($("#divEmployeeModal #txtCode"));
            PreventSpace($("#divEmployeeModal #txtCode"));
            NumberOnly($("#divEmployeeModal #txtOldEmployeeID,#divEmployeeModal #txtOfficeMobile"));
            NumberDashOnly($("#divEmployeeModal #txtPagibigNumber"));
            NumberDashOnly($("#divEmployeeModal #txtPhilhealthNumber"));
            NumberDashOnly($("#divEmployeeModal #txtSSSNumber"));
            NumberDashOnly($("#divEmployeeModal #txtTIN"));

            $('#divEmployeeModal #txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });

            $("#divEmployeeModal #dpDateHired, #divEmployeeModal #dpBirthDate").datetimepicker({
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

            //    if (!ValidEmail($("#divEmployeeModal #txtEmail"))) {
            //        $("#divEmployeeModal #txtEmail").addClass("errMessage");
            //        $("#divEmployeeModal #txtEmail").focus();
            //        $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Email Address is invalid</li></label><br />");
            //    }
            //    return false;
            //});

            $("#divEmployeeModal #txtCorporateEmail").on("keyup", function () {
                $("#divEmployeeErrorMessage").html("");

                if (!ValidEmail($("#divEmployeeModal #txtCorporateEmail"))) {
                    $("#divEmployeeModal #txtCorporateEmail").addClass("errMessage");
                    $("#divEmployeeModal #txtCorporateEmail").focus();
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
                formatSSSNumber($("#divEmployeeModal #txtSSSNumber"));
            });

            $("#divEmployeeModal #txtPagibigNumber").on("keyup, keydown, keypress", function () {
                addHyphen($("#divEmployeeModal #txtPagibigNumber"), 4);
            });

            $("#divEmployeeModal #txtPhilhealthNumber").on("keyup, keydown, keypress", function () {
                addHyphen($("#divEmployeeModal #txtPhilhealthNumber"), 4);
            });

            $("#divEmployeeModal #txtTIN").on("keyup, keydown, keypress", function () {
                addHyphen($("#divEmployeeModal #txtTIN"), 3);
			});

            $("#divEmployeeModal #dpDateScheduled, #divEmployeeModal #dpDateCompleted").datetimepicker({
                useCurrent: true,
                format: 'MM/DD/YYYY',
            });

            $("#divEmployeeModal #btnBack").click(function () {
                LoadPartial(EmployeeViewURL + "?ID=" + objEmployeeEditJS.ID, "divEmployeeBodyModal");
            });

            $("#divEmployeeModal #btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , EmployeeDeleteURL + '?ID=' + objEmployeeEditJS.ID\
                    , {} \
                    , '#divEmployeeErrorMessage' \
                    , '#btnDelete' \
                    , objEmployeeEditJS.DeleteSuccessFunction);",
                    "function");
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
                    //else if (!validMobileNo($("#divEmployeeModal #txtOfficeMobile"))) {
                    //    $("#divEmployeeModal #txtOfficeMobile").addClass("errMessage");
                    //    $("#divEmployeeModal #txtOfficeMobile").focus();
                    //    $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Office Mobile Number is invalid</li></label><br />");
                    //    return false;
                    //}
                    //else if ($('#DivFamilyDynamicFields').children().length != 0) {
                    //    var isInvalid = 0;
                    //    $(".FamilyNumberDynamicFields").each(function (index) {
                    //        var ctr = $(this).prop("id").replace("txtNumber_", "");
                    //        if (!validMobileNo($(this))) {
                    //            $("#txtNumber_" + ctr).addClass("errMessage");
                    //            isInvalid++;
                    //        }
                    //    });
                    //    if (isInvalid > 0) {
                    //        $("#divEmployeeErrorMessage").append("<label class=\"errMessage\"><li>Contact Number is invalid</li></label><br />");
                    //        return false;
                    //    }
                    //    else {
                    //        $("#divEmployeeErrorMessage").html("");
                    //        return true;
                    //    }
                    //}
                    else {
                        return true;
                    }
                };

                $("#divEmployeeModal #tabOnboarding .form-control").removeClass("required-field");

                if ($("#hdnHasDataPrivacy").val() == "false") {
                    $("#divEmployeeModal #tabPersonalInformation .form-control").removeClass("required-field");
                    $("#divEmployeeModal #tabCompensation .form-control").removeClass("required-field");
                    $("#divEmployeeModal #tabFamilyBackground .form-control").removeClass("required-field");
                }

                if (objEMSCommonJS.ValidateBlankFields("#frmEmployee", "#divEmployeeErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeEditURL \
                        , objEmployeeEditJS.GetFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#divEmployeeModal #btnSave' \
                        , objEmployeeEditJS.EditSuccessFunction);",
                        "function");
                }
                else {
                    if ($("#divEmployeeModal #tabPersonalInformation .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Personal')").parent("button"), 'divEmployeeModal #tabPersonalInformation');
                    }
                    else if ($("#divEmployeeModal #tabCompensation .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Compensation')").parent("button"), 'divEmployeeModal #tabCompensation');
                    }
                    else if ($("#divEmployeeModal #tabFamilyBackground .errMessage").length > 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks").find("span:contains('Background')").parent("button"), 'divEmployeeModal #tabFamilyBackground');
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

            $("#divEmployeeModal #btnOnboardingWorkflowTransactionSave").click(function () {

                if (objEMSCommonJS.ValidateBlankFields("#tabOnboarding", "#divEmployeeErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SaveOnboardingWorkflowURL \
                        , objEmployeeEditJS.GetWorkflowTransactionFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#divEmployeeModal #btnOnboardingWorkflowTransactionSave' \
                        , objEmployeeEditJS.OnboardingWorkflowEditSuccessFunction);", "function");
                }
            });

            $("#divEmployeeModal #ddlStep").change(function () {
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

            $("#divEmployeeModal #btnAddWorkingHistoryFields").click(function () {
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

            $("#divEmployeeModal .tablinks").find("span:contains('Background')").parent("button").click(function () {
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

            $("#divEmployeeModal .tablinks").find("span:contains('Education')").parent("button").click(function () {
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

            $("#divEmployeeModal .tablinks").find("span:contains('Working History')").parent("button").click(function () {
                if (!WorkHisTabFirstLoad) {
                    s.LoadWorkingHistory();
                    if ($('#DivWorkingHistoryDynamicFields').children().length == 0) {
                        $("#btnAddWorkingHistoryFields").click();
                    }
                    WorkHisTabFirstLoad = true;
                    $("#hdnIsViewedWorkingHistory").val(true);
                }
            });

            $("#divEmployeeModal .tablinks").find("span:contains('Secondary Designation')").parent("button").click(function () {
                if (!SecDesigTabFirstLoad) {
                    s.GetOrgGroupDropDown();
                    s.GetPositionDropDown();
                    s.LoadEmployeeRoving();
                    SecDesigTabFirstLoad = true;
                    $("#hdnIsViewedSecondaryDesignation").val(true);
                    $("#divEmployeeModal #tabEmployeeRoving .form-control").prop("disabled", true);
                }
            });

            $("#divEmployeeModal .tablinks").find("span:contains('Task Checklist')").parent("button").click(function () {
                if (!logActivityTabFirstLoad) {
                    var isSuccessFunction = function () {
                        $("#divEmployeeModal #tabLogActivity #btnAddPreloaded, \
                            #divEmployeeModal #tabLogActivity #btnAddActivity,   \
                            #divEmployeeModal #tabLogActivity #btnBatchAssign, \
                            #divEmployeeModal #tabLogActivity #btnUploadActivity").show();
                    };
                    LoadPartialSuccessFunction(EmployeeLogActivityURL, "divEmployeeModal #tabLogActivity", isSuccessFunction);
                    logActivityTabFirstLoad = true;
                    return false;
                }
            });

            $("#divEmployeeModal .tablinks").find("span:contains('Onboarding')").parent("button").click(function () {
                if (!OnboardTabFirstLoad) {
                    objEmployeeListJS.OnboardingWorkflowTransactionJQGrid({
                        WorkflowID: $("#hdnOnboardingWorkflowID").val(),
                        EmployeeID: $("#divEmployeeModal #hdnID").val()
                    });

                    GenerateDropdownValues(GetOnboardingWorkflowStepDropDownURL + "&WorkflowCode=" + $("#hdnWorkflowCode").val(), "ddlStep", "Value", "Text", "", "", false);
                    OnboardTabFirstLoad = true;
                }
            });

            $("#divEmployeeModal .tablinks").find("span:contains('Accountability')").parent("button").click(function () {
                if (!AccountTabFirstLoad) {
                    objAccountabilityJS.LoadAccountabilityJQGrid({
                        EmployeeID: $("#divEmployeeModal #hdnID").val()
                    });
                    AccountTabFirstLoad = true;
                }
            });

            $("#divEmployeeModal .tablinks").find("span:contains('Movement')").parent("button").click(function () {
                if (!MovementTabFirstLoad) {
                    LoadPartial(EmployeeMovementURL, "tabMovement");
                    MovementTabFirstLoad = true;
                    return false;
                }
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

            $("#divEmployeeModal #ddlOrgGroup").change(function () {
                if ($(this).val() != "") {
                    var GetSuccessFunction = function (data) {
                        $("#lblRegion").text(data.Result.Region);
                        $("#hdnRegionCode").val(data.Result.RegionCode);
                        if (($("#btnEmployeeCompensationEdit").prop("id") || "") != "") {
                            $("#txtMonthlySalary").val(data.Result.MonthlyRate);
                            computeDailyHourly();
                            $("#txtMonthlySalary").val($("#txtMonthlySalary").val().commaOnAmount());
                        }
                    };

                    objEMSCommonJS.GetAjax(GetRegionByOrgGroupIDURL + "&OrgGroupID=" + $(this).val(), {}, "", GetSuccessFunction);
                }
                else {
                    $("#lblRegion").text("");
                    $("#hdnRegionCode").val("");
                }
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

            $("#divEmployeeModal #btnEmployeeCompensationEdit").click(function () {
                $("#tabCompensation #txtMonthlySalary, #tabCompensation #txtDailySalary, #tabCompensation #txtHourlySalary").attr("readonly", false);
                $("#tabCompensation #btnEmployeeCompensationSave, #tabCompensation #btnEmployeeCompensationBack").show();
                $("#tabCompensation #btnEmployeeCompensationEdit").hide();
            });

            $("#divEmployeeModal #btnEmployeeCompensationBack").click(function () {
                $("#tabCompensation #txtMonthlySalary, #tabCompensation #txtDailySalary, #tabCompensation #txtHourlySalary").attr("readonly", true);
                $("#tabCompensation #btnEmployeeCompensationSave, #tabCompensation #btnEmployeeCompensationBack").hide();
                $("#tabCompensation #btnEmployeeCompensationEdit").show();
            });

            $("#divEmployeeModal #btnEmployeeCompensationSave").click(function () {

                if (objEMSCommonJS.ValidateBlankFields("#tabCompensation", "#divEmployeeErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SaveEmployeeCompensationURL \
                        , objEmployeeEditJS.GetEmployeeCompensationFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#divEmployeeModal #btnEmployeeCompensationSave' \
                        , objEmployeeEditJS.CompensationEditSuccessFunction);", "function");
                }
            });

            $("#divEmployeeModal #btnAddMainAttachmentFields").click(function () {
                var fields = $("#DivUploadMainAttachmentDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddAttachmentDynamicFields();
            });

            $("#divEmployeeModal #btnSaveMainAttachment").click(function () {
                var isNoBlankFunction = function () {
                    if ($(".MainAttachment_Description").length == 0 && EmployeeDeletedMainAttachments == 0) {
                        $("#frmEmployee #divEmployeeErrorMessage").html(
                            "<label class=\"errMessage\"><li>" + "Please click plus icon to add attachment" + "</li></label><br />");
                        return false;

                    }
                    else {
                        return true;
                    }
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmEmployeeAttachment #DivUploadMainAttachmentDynamicFields", "#divEmployeeErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeSaveMainAttachmentURL \
                        , objEmployeeEditJS.GetAttachmentSectionFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#divEmployeeModal #btnSaveMainAttachment' \
                        , objEmployeeEditJS.AttachmentUpdateSuccessFunction);",
                        "function");
                }
            });

            objEMSCommonJS.BindAutoComplete("divEmployeeModal #txtHomeBranch"
                , OrgGroupAutoCompleteURL
                , 20, "divEmployeeModal #hdnHomeBranch", "ID", "Description");
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $("#divEmployeeModal").modal('hide');
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

        GetWorkflowTransactionFormData: function () {
            var formData = new FormData($('#frmEmployee').get(0));
            formData.append("Workflow.RecordID", $("#divEmployeeModal #hdnID").val());
            formData.append("Workflow.WorkflowID", $("#divEmployeeModal #hdnOnboardingWorkflowID").val());
            formData.append("Workflow.WorkflowCode", $("#divEmployeeModal #hdnWorkflowCode").val());
            formData.append("Workflow.CurrentStepCode", $("#divEmployeeModal #ddlStep").val());
            formData.append("Workflow.Result", $("#divEmployeeModal #ddlResult").val());
            formData.append("Workflow.DateScheduled", $("#divEmployeeModal #dpDateScheduled").val());
            formData.append("Workflow.DateCompleted", $("#divEmployeeModal #dpDateCompleted").val());
            formData.append("Workflow.Remarks", $("#divEmployeeModal #txtRemarks").val());
            return formData;
        },

        GetEmployeeCompensationFormData: function () {
            var formData = new FormData($('#frmEmployeeCompensation').get(0));
            formData.append("EmployeeCompensation.EmployeeID", $("#divEmployeeModal #hdnID").val());
            formData.append("EmployeeCompensation.MonthlySalary", $("#tabCompensation #txtMonthlySalary").val());
            formData.append("EmployeeCompensation.DailySalary", $("#tabCompensation #txtDailySalary").val());
            formData.append("EmployeeCompensation.HourlySalary", $("#tabCompensation #txtHourlySalary").val());
            return formData;
        },

        AddEmployeeRovingDynamicFields: function () {
            var s = this;
            var htmlId = $(".EmployeeRovingDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("EmployeeRovingDynamicFields_", "")) + 1;

            //$("#DivEmployeeRovingDynamicFields").append(
            //    "<div class=\"form-group form-fields EmployeeRovingDynamicFields\" id=\"EmployeeRovingDynamicFields_" + ctr + "\"> \
            //       <div class=\"col-md-3 text-align-center no-padding\"> \
            //           <select id=\"ddlRovingOrgGroup_" + ctr + "\" title=\"Org Group\" class=\"form-control RovingOrgGroupDynamicFields\"></select> \
            //       </div> \
            //       <div class=\"col-md-3 text-align-center no-padding\"> \
            //           <select id=\"ddlRovingPosition_" + ctr + "\" title=\"Position\" class=\"form-control RovingPositionDynamicFields\"></select> \
            //       </div> \
            //       <div class=\"col-md-0-5 text-align-center no-padding\"> \
            //           <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
            //            onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeEditJS.RemoveDynamicFields('#EmployeeRovingDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
            //       </div> \
            //     </div>"
            //);

            $("#DivEmployeeRovingDynamicFields").append(
                "<div class=\"form-group form-fields EmployeeRovingDynamicFields\" id=\"EmployeeRovingDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-6 text-align-center no-padding\"> \
                       <select id=\"ddlRovingOrgGroup_" + ctr + "\" title=\"Org Group\" class=\"form-control RovingOrgGroupDynamicFields\"></select> \
                   </div> \
                   <div class=\"col-md-6 text-align-center no-padding\"> \
                       <select id=\"ddlRovingPosition_" + ctr + "\" title=\"Position\" class=\"form-control RovingPositionDynamicFields\"></select> \
                   </div> \
                 </div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlRovingOrgGroup_" + ctr, OrgGroupDropDown);
            objEMSCommonJS.PopulateDropDown("#ddlRovingPosition_" + ctr, PositionDropDown);
        },

        RemoveDynamicFields: function (id) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
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
                            Value: item.Value,
                            Text: item.Text
                        });
                });
            };

            objEMSCommonJS.GetAjax(PositionDropDownURL, {}, "", GetSuccessFunction);
        },

        LoadEmployeeRoving: function () {
            var s = this;
            $("#DivEmployeeRovingDynamicFields").html("");

            var input = { EmployeeID: objEmployeeEditJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    $("#DivEmployeeRovingDynamicFields").append(
                        "<div class=\"form-group form-fields EmployeeRovingDynamicFields\" id=\"EmployeeRovingDynamicFields_" + idCtr + "\"> \
                           <div class=\"col-md-6 text-align-left dynamic-field\"> \
                               <label id=\"ddlRovingOrgGroup_" + idCtr + "\" title=\"Org Group\" class=\"form-control RovingOrgGroupDynamicFields\"></label> \
                           </div> \
                           <div class=\"col-md-6 text-align-left dynamic-field\"> \
                               <label id=\"ddlRovingPosition_" + idCtr + "\" title=\"Position\" class=\"form-control RovingPositionDynamicFields\"></label> \
                           </div> \
                         </div>"
                    );

                    $('#ddlOrgGroup option').each(function () {
                        if ($(this).val() == item.OrgGroupID) {
                            $("#ddlRovingOrgGroup_" + idCtr).append($('<option/>', {
                                value: $(this).val(),
                                text: $(this).text()
                            }));
                        }
                    });

                    $('#ddlPosition option').each(function () {
                        if ($(this).val() == item.PositionID) {
                            $("#ddlRovingPosition_" + idCtr).append($('<option/>', {
                                value: $(this).val(),
                                text: $(this).text()
                            }));
                        }
                    });
                };

                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetEmployeeRovingURL, input, "", GetSuccessFunction);
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
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeEditJS.RemoveDynamicFields('#FamilyDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeEditJS.RemoveDynamicFields('#EducationDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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
                        onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeEditJS.RemoveDynamicFields('#WorkingHistoryDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
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

            var input = { EmployeeID: objEmployeeEditJS.ID };

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

            var input = { EmployeeID: objEmployeeEditJS.ID };

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

            var input = { EmployeeID: objEmployeeEditJS.ID };

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

        GetAttachment: function () {
            var s = this;
            var input = {
                ID: $("#divEmployeeModal #hdnID").val()
            };

            var GetSuccessFunction = function (data) {
                $("#DivDownloadMainAttachmentDynamicFields").html("");
                var populateFields = function (item, idCtr) {
                    var htmlId = $(".MainAttachmentDynamicFields:last").prop("id");
                    var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("MainAttachmentDynamicFields_", "")) + 1;

                    $("#DivDownloadMainAttachmentDynamicFields").append(
                        "<div class=\"form-group form-fields MainAttachmentDynamicFields\" id=\"MainAttachmentDynamicFields_" + ctr + "\">"
                        + "    <div class=\"col-md-3-5 no-padding\">"
                        + "        <input type=\"text\" id=\"txtDescription_" + ctr + "\" maxlength=\"255\" class=\"form-control MainAttachment_Description\" title=\"Description\">"
                        + "    </div>"
                        + "    <div class=\"col-md-4 no-padding\">"
                        + "        <label class=\"control-label block-label MainAttachment_File\"> <span class=\"unreqField\">* </span>" + item.SourceFile + "</label>"
                        + "        <input type=\"hidden\" id=\"txtServerFile_ " + ctr + "\" value=\"" + item.ServerFile + "\" class=\"form-control MainAttachment_ServerFile\">"
                        + "        <input type=\"hidden\" id=\"txtSourceFile_ " + ctr + "\" value=\"" + item.SourceFile + "\" class=\"form-control MainAttachment_SourceFile\">"
                        + "    </div>"
                        + "    <div class=\"col-md-3-5 no-padding\">"
                        + "        <label class=\"control-label block-label\"> <span class=\"unreqField\">* </span>" + item.UploadedBy + "<br> Timestamp: " + item.Timestamp + "</label>"
                        + "    </div>"
                        + "    <div class=\"col-md-0-5 no-padding\">"
                        + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeListJS.RemoveDynamicFields('#MainAttachmentDynamicFields_" + ctr + "',objEmployeeEditJS.DeleteAttachmentFunction(&#39;" + item.ServerFile + "&#39;))&quot;, &quot;function&quot;);\"></span>"
                        + "    </div>"
                        + "     <div class=\"col-md-0-5 no-padding\">"
                        + "         <span class=\"btn-glyph-dynamic glyphicon glyphicon-download-alt\" onclick=\"objEMSCommonJS.DownloadAttachment(CheckFileIfExistsURL, 'PlantillaService_Employee_Attachment_Path','" + item.ServerFile + "', '" + item.SourceFile + "')\"></span>"
                        + "     </div>"
                        + "</div>"
                    );

                    $("#DivDownloadMainAttachmentDynamicFields #txtDescription_" + idCtr).val(item.Description);
                };

                if (data.IsSuccess == true) {
                    //$("#divAttachmentContainer").html("");
                    if (data.Result.length > 0) {
                        var ctr = 1;
                        $(data.Result).each(function (index, item) {
                            populateFields(item, ctr); ctr++;
                        });
                    }
                }
            };

            objEMSCommonJS.GetAjax(EmployeeGetMainAttachmentURL, input, "", GetSuccessFunction);
        },

        DeleteAttachmentFunction: function (serverFile) {
            EmployeeDeletedMainAttachments.push(serverFile);
        },

        AddAttachmentDynamicFields: function () {
            var s = this;
            var htmlId = $(".MainAttachmentDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("MainAttachmentDynamicFields_", "")) + 1;

            $("#DivUploadMainAttachmentDynamicFields").append(
                "<div class=\"form-group form-fields MainAttachmentDynamicFields\" id=\"MainAttachmentDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeListJS.RemoveDynamicFields('#MainAttachmentDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
                + "    </div>"
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtDescription_" + ctr + "\" maxlength=\"255\" class=\"form-control MainAttachment_Description\" title=\"Description\">"
                + "    </div>"
                + "    <div class=\"col-md-4 no-padding\">"
                + "        <input type=\"file\" id=\"fileAttachment_" + ctr + "\" class=\"form-control required-field MainAttachment_File\" title=\"Attachment\" accept=\".pdf,.doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document\">"
                + "    </div>"
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "    </div>"
                + "</div>"
            );

        },

        GetAttachmentSectionFormData: function () {
            $(".MainAttachment_Description").each(function (index, item) {
                $(this).prop("name", "EmployeeAttachmentForm.AddAttachmentForm[" + index + "].Description");
            });

            $(".MainAttachment_ServerFile").each(function (index, item) {
                $(this).prop("name", "EmployeeAttachmentForm.AddAttachmentForm[" + index + "].ServerFile");
            });

            $(".MainAttachment_SourceFile").each(function (index, item) {
                $(this).prop("name", "EmployeeAttachmentForm.AddAttachmentForm[" + index + "].SourceFile");
            });

            $(".MainAttachment_File").each(function (index, item) {
                $(this).prop("name", "EmployeeAttachmentForm.AddAttachmentForm[" + index + "].File");
            });

            var formData = new FormData($('#frmEmployeeAttachment').get(0));
            formData.append("EmployeeAttachmentForm.EmployeeID", $("#divEmployeeModal #hdnID").val());

            $(EmployeeDeletedMainAttachments).each(function (index, item) {
                formData.append("EmployeeAttachmentForm.DeleteAttachmentForm[" + index + "].ServerFile", item);
            });
            return formData;
        },

        AttachmentUpdateSuccessFunction: function () {
            $("#btnBack").click();
        },
    };

    objEmployeeEditJS.Initialize();
});