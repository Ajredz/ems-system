var objEmployeeViewJS;
var RelationshipDropDown = [];
var SchoolLevelDropdown = [];
var EducAttDegreeDropdown = [];
var EducAttStatusDropDown = [];

$(document).ready(function () {
    objEmployeeViewJS = {

        ID: $("#divEmployeeModal #hdnID").val(),

        Initialize: function () {
            $("#divEmployeeBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $('#divEmployeeBodyModal input[type="checkbox"]').prop('disabled', true);
            $("#btnAddEmployeeRovingFields, #btnAddFamilyFields, #btnAddEducationFields, #btnAddWorkingHistoryFields, #btnEmployeeCompensationSave").hide();
			$("#btnOnboardingWorkflowTransactionSave").hide();
            $("#btnSaveMainAttachment, #frmEmployeeAttachment .glyphicon-trash, #frmEmployeeAttachment .glyphicon-plus-sign").remove();
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
            TrainingTabFirstLoad = false;
            MovementTabFirstLoad = false;
            skillsTabFirstLoad = false;

            objEMSCommonJS.ChangeTab($("#divEmployeeModal .tablinks").first(), "tabPersonalInformation", "#divEmployeeModal");
            $("#divEmployeeModal .tablinks").first().click();   

            EmployeeDeletedMainAttachments = [];
            s.GetAttachment();

            //WIP
            //setTimeout(function () {
            //    $(".tablinks").find("span:contains('Accountability')").parent("button").click();
            //}, 500);
            //setTimeout(function () {
            //    $("#btnPrintCOE").click();
            //}, 800);
            //WIP

            //s.LoadEmployeeRoving();


            //objLogActivityJS.LoadLogActivityJQGrid({
            //    EmployeeID: $("#hdnID").val()
            //});

            //objEmployeeListJS.OnboardingWorkflowTransactionJQGrid({
            //    WorkflowID: $("#hdnOnboardingWorkflowID").val(),
            //    EmployeeID: $("#hdnID").val()
            //}); 

            //objAccountabilityJS.LoadAccountabilityJQGrid({
            //    EmployeeID: $("#hdnID").val()
            //});

            //s.GetRelationshipDropDown();

            //s.LoadFamilyBackground();

            //s.LoadWorkingHistory();

            //objEmployeeListJS.LoadEmploymentStatusJQGrid({
            //    EmployeeID: $("#hdnID").val(),
            //});

            //$("#tblLogActivityList").jqGrid('hideCol', ["ID"]);
            //$("#tblAccountabilityList").jqGrid('hideCol', ["ID"]);

            $("#txtOfficeMobile").val() == (null || "") ? "" : $("#txtOfficeMobile").val($("#txtOfficeMobile").val().slice(0, 4) + "-" + $("#txtOfficeMobile").val().substr(4));

            $("#txtCellphoneNumber").val() == (null || "") ? "" : $("#txtCellphoneNumber").val($("#txtCellphoneNumber").val().slice(0, 4) + "-" + $("#txtCellphoneNumber").val().substr(4));

            $("#txtContactPersonNumber").val() == (null || "") ? "" : $("#txtContactPersonNumber").val($("#txtContactPersonNumber").val().slice(0, 4) + "-" + $("#txtContactPersonNumber").val().substr(4));

            $("#divEmployeeModal .form-control").prop("disabled", true);
            //document.getElementById("txtCellphoneNumber").value = $("#txtCellphoneNumber").val().substr(3);
            formatSSSNumber($("#txtSSSNumber"));
            addHyphen($("#txtPagibigNumber"), 4);
            addHyphen($("#txtPhilhealthNumber"), 4);
            addHyphen($("#txtTIN"), 3);

            $("#txtMonthlySalary").val($("#txtMonthlySalary").val().commaOnAmount());
            $("#txtDailySalary").val($("#txtDailySalary").val().commaOnAmount());
            $("#txtHourlySalary").val($("#txtHourlySalary").val().commaOnAmount());

        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $("#divEmployeeModal").modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#dpDateHired").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#dpBirthDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , EmployeeDeleteURL + '?ID=' + objEmployeeViewJS.ID\
                    , {} \
                    , '#divEmployeeErrorMessage' \
                    , '#btnDelete' \
                    , objEmployeeViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(EmployeeEditURL + "?ID=" + objEmployeeViewJS.ID, "divEmployeeBodyModal");
            });

            $(".tablinks").find("span:contains('Background')").parent("button").click(function () {
                if (!familyBackTabFirstLoad) {
                    s.GetRelationshipDropDown();
                    s.LoadFamilyBackground();
                    familyBackTabFirstLoad = true;
                    $("#divEmployeeModal #tabFamilyBackground .form-control").prop("disabled", true);
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
                    $("#divEmployeeModal #tabEducation .form-control").prop("disabled", true);
                }
            });

            $(".tablinks").find("span:contains('Working History')").parent("button").click(function () {
                if (!WorkHisTabFirstLoad) {
                    s.LoadWorkingHistory();
                    WorkHisTabFirstLoad = true;
                    $("#divEmployeeModal #tabWorkingHistory .form-control").prop("disabled", true);
                }
            });

            //ORIG
            $(".tablinks").find("span:contains('Secondary Designation')").parent("button").click(function () {
                if (!SecDesigTabFirstLoad) {
                    s.LoadEmployeeRoving();
                    SecDesigTabFirstLoad = true;
                    $("#divEmployeeModal #tabEmployeeRoving .form-control").prop("disabled", true);
                }
            });

            //$(".tablinks").find("span:contains('Secondary Designation')").parent("button").click(function () {
            //    if (!SecDesigTabFirstLoad) {
            //        LoadPartial(tabEmployeeRovingURL, "tabRoving");
            //        SecDesigTabFirstLoad = true;
            //        return false;
            //    }
            //});


            $(".tablinks").find("span:contains('Task Checklist')").parent("button").click(function () {
                if (!logActivityTabFirstLoad) {
                    var isSuccessFunction = function () {
                        objLogActivityJS.IsViewMode = true;
                    };
                    LoadPartialSuccessFunction(EmployeeLogActivityURL, "divEmployeeModal #tabLogActivity", isSuccessFunction);
                    logActivityTabFirstLoad = true;
                    return false;
                }
            });

            $(".tablinks").find("span:contains('Onboarding')").parent("button").click(function () {
                if (!OnboardTabFirstLoad) {
                    objEmployeeListJS.OnboardingWorkflowTransactionJQGrid({
                        WorkflowID: $("#hdnOnboardingWorkflowID").val(),
                        EmployeeID: $("#hdnID").val()
                    }); 
                    OnboardTabFirstLoad = true;
                }
            });

            $(".tablinks").find("span:contains('Accountability')").parent("button").click(function () {
                if (!AccountTabFirstLoad) {
                     objAccountabilityJS.LoadAccountabilityJQGrid({
                        EmployeeID: $("#hdnID").val()
                     });
                    //$("#tblAccountabilityList").jqGrid('hideCol', ["ID"]);
                    AccountTabFirstLoad = true;
                }
            });

            $(".tablinks").find("span:contains('Training')").parent("button").click(function () {
                if (!TrainingTabFirstLoad) {
                    LoadPartial(EmployeeTrainingURL, "tabTraining");
                    TrainingTabFirstLoad = true;
                    return false;
                }
            });

            $(".tablinks").find("span:contains('Movement')").parent("button").click(function () {
                if (!MovementTabFirstLoad) {
                    LoadPartial(EmployeeMovementURL, "tabMovement");
                    MovementTabFirstLoad = true;
                    return false;
                }
            });

            $(".tablinks").find("span:contains('Skills')").parent("button").click(function () {
                if (!skillsTabFirstLoad) {
                    var isSuccessFunction = function () {
                        objSkillsJS.IsViewMode = true;
                    };
                    //LoadPartialSuccessFunction(EmployeeSkillsURL, "divEmployeeModal #tabSkills", isSuccessFunction);
                    skillsTabFirstLoad = true;
                    return false;
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
                        , objEmployeeViewJS.GetAttachmentSectionFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#divEmployeeModal #btnSaveMainAttachment' \
                        , objEmployeeViewJS.AttachmentUpdateSuccessFunction);",
                        "function");
                }
            });
        },

        LoadEmployeeRoving: function () {
        //    ORIG
            var s = this;
            $("#DivEmployeeRovingDynamicFields").html("");

            //Orig
            var input = { EmployeeID: objEmployeeViewJS.ID };

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
        

        GetSystemUserName: function () {
            var s = this;

            if ($("#hdnSystemUserID").val() > 0) {
                var GetSuccessFunction = function (data) {
                    $("#txtSystemUserName").val(data.Result.Username);
                };

                objEMSCommonJS.GetAjax(GetSystemUserNameURL + "&ID=" + $("#hdnSystemUserID").val(), {}, "", GetSuccessFunction);
            }
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

        LoadFamilyBackground: function () {
            var s = this;
            $("#DivFamilyDynamicFields").html("");

            var input = { EmployeeID: objEmployeeViewJS.ID };

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
                    //document.getElementById("txtNumber_" + idCtr).value = $("#txtNumber_" + idCtr).val().substr(3);
                    $("#txtNumber_"+idCtr).val() == (null || "") ? "" : $("#txtNumber_" + idCtr).val($("#txtNumber_" + idCtr).val().slice(0, 4) + "-" + $("#txtNumber_" + idCtr).val().substr(4));
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetFamilyBackgroundURL, input, "", GetSuccessFunction);
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

        LoadEducationBackground: function () {
            var s = this;
            $("#DivEducationDynamicFields").html("");

            var input = { EmployeeID: objEmployeeViewJS.ID };

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

            var input = { EmployeeID: objEmployeeViewJS.ID };

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

        GetAttachment: function () {
            var s = this;
            var input = {
                ID: $("#hdnID").val()
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
                        //+ "    <div class=\"col-md-0-5 no-padding\">"
                        //+ "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeListJS.RemoveDynamicFields('#MainAttachmentDynamicFields_" + ctr + "',objEmployeeViewJS.DeleteAttachmentFunction(&#39;" + item.ServerFile + "&#39;))&quot;, &quot;function&quot;);\"></span>"
                        //+ "    </div>"
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
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtDescription_" + ctr + "\" maxlength=\"255\" class=\"form-control MainAttachment_Description\" title=\"Description\">"
                + "    </div>"
                + "    <div class=\"col-md-4 no-padding\">"
                + "        <input type=\"file\" id=\"fileAttachment_" + ctr + "\" class=\"form-control required-field MainAttachment_File\" title=\"Attachment\" accept=\".pdf,.doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document\">"
                + "    </div>"
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "    </div>"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objEmployeeListJS.RemoveDynamicFields('#MainAttachmentDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
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
            formData.append("EmployeeAttachmentForm.EmployeeID", $("#hdnID").val());

            $(EmployeeDeletedMainAttachments).each(function (index, item) {
                formData.append("EmployeeAttachmentForm.DeleteAttachmentForm[" + index + "].ServerFile", item.value);
            });
            return formData;
        },

        AttachmentUpdateSuccessFunction: function () {
            LoadPartial(EmployeeViewURL + "?ID=" + $("#hdnID").val(), 'divEmployeeBodyModal');
        },
    };

    objEmployeeViewJS.Initialize();
});