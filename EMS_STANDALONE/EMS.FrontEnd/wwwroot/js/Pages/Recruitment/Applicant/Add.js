var objApplicantAddJS;

var AttachmentTypeDropDownOptions = [];
$(document).ready(function () {
    objApplicantAddJS = {

        Initialize: function () {
            $("#divApplicantBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divApplicantModal #btnSave").show();
            $("#divApplicantModal .form-control").attr("readonly", false);
            $("#divApplicantModal #btnDelete, #divApplicantModal #btnBack").remove();
            $("#txtExpectedSalary").val("");
            $(".downloadColumn").remove();
            $(".tablinks:nth-child(4)").remove();
            $(".tablinks:nth-child(3)").remove();
            $("#tabIdLegalProfile").remove();
            s.GetAttachmentTypeDropDownOptions();
            s.AddAttachmentDynamicFields();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabPersonalInformation');
            $(".timestampColumn").remove();
        },

        ElementBinding: function () {
            var s = this;
            AmountTextChange($("#divApplicantModal #txtExpectedSalary"));
            AmountOnly($("#divApplicantModal #txtExpectedSalary"));
            NumberOnly($("#divApplicantModal #txtCellphoneNumber"));
            $("#dpBirthDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'});
            $("#dpDateApplied").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'});

            $("#btnAddAttachmentFields").click(function () {
                var fields = $("#DivUploadAttachmentDynamicFields .required-field");
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

            $("#divApplicantModal #ddlPSGCRegion").change(function () {
                GenerateDropdownValues(GetProvinceDropDownByRegionURL + "&Code=" + $("#divApplicantModal #ddlPSGCRegion").val(),
                    "divApplicantModal #ddlPSGCProvince", "Value", "Text", "", "", false);

                if ($("#divApplicantModal #ddlPSGCRegion").val() != "") {
                    $("input[name='Applicant.PersonalInformation.PSGCRegionCode']").val($("#divApplicantModal #ddlPSGCRegion").val());
                }

                //reset dropdown values
                else {
                    //city/municipality
                    GenerateDropdownValues(GetCityMunicipalityDropDownByProvinceURL + "&Code=" + "",
                        "divApplicantModal #ddlPSGCCityMunicipality", "Value", "Text", "", "", false);
                    //barangay
                    GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + "",
                        "divApplicantModal #ddlPSGCBarangay", "Value", "Text", "", "", false);
                }
            });

            $("#divApplicantModal #ddlPSGCProvince").change(function () {
                GenerateDropdownValues(GetCityMunicipalityDropDownByProvinceURL + "&Code=" + $("#divApplicantModal #ddlPSGCProvince").val(),
                    "divApplicantModal #ddlPSGCCityMunicipality", "Value", "Text", "", "", false);

                if ($("#divApplicantModal #ddlPSGCProvince").val() != "") {
                    $("input[name='Applicant.PersonalInformation.PSGCProvinceCode']").val($("#divApplicantModal #ddlPSGCProvince").val());
                }

                //reset dropdown values
                else {
                    //barangay
                    GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + "",
                        "divApplicantModal #ddlPSGCBarangay", "Value", "Text", "", "", false);
                }
            });

            $("#divApplicantModal #ddlPSGCCityMunicipality").change(function () {
                GenerateDropdownValues(GetBarangayDropDownByCityMunicipalityURL + "&Code=" + $("#divApplicantModal #ddlPSGCCityMunicipality").val(),
                    "divApplicantModal #ddlPSGCBarangay", "Value", "Text", "", "", false);

                if ($("#divApplicantModal #ddlPSGCCityMunicipality").val() != "") {
                    $("input[name='Applicant.PersonalInformation.PSGCCityMunicipalityCode']").val($("#divApplicantModal #ddlPSGCCityMunicipality").val());
                }
            });

            $("#divApplicantModal #ddlPSGCBarangay").change(function () {
                if ($("#divApplicantModal #ddlPSGCBarangay").val() != "") {
                    $("input[name='Applicant.PersonalInformation.PSGCBarangayCode']").val($("#divApplicantModal #ddlPSGCBarangay").val());
                }
            });

            $("#txtEmail").on("keyup", function () {
                //var email = $("#txtEmail").val();
                $("#divApplicantErrorMessage").html("");

                if (!ValidEmail($("#txtEmail"))) {
                    $("#txtEmail").addClass("errMessage");
                    $("#txtEmail").focus();
                    $("#divApplicantErrorMessage").append("<label class=\"errMessage\"><li>Email Address is invalid</li></label><br />");
                }
                return false;
            });

            $("#txtCellphoneNumber").on("keyup", function () {
                //var cellnumber = $("#txtCellphoneNumber").val();
                $("#divApplicantErrorMessage").html("");

                if (!validMobileNo($("#txtCellphoneNumber"))) {
                    $("#txtCellphoneNumber").addClass("errMessage");
                    $("#txtCellphoneNumber").focus();
                    $("#divApplicantErrorMessage").append("<label class=\"errMessage\"><li>Cellphone Number is invalid</li></label><br />");
                }
                else
                    $("#txtCellphoneNumber").removeClass("errMessage");

                if ($("#txtCellphoneNumber").val().length == 4)
                    $("#txtCellphoneNumber").val($("#txtCellphoneNumber").val() + "-");

                return false;
            });

            $("#btnAddApplicationSource").click(function () {
                var isSuccessFunction = function () {
                    $("#divReferenceModal .close").click(function () {
                        GenerateDropdownValues(GetApplicationSourceURL, "ddlApplicationSource", "Value", "Description", "", "", false);
                    });
                };
                LoadPartialSuccessFunction(AddReferenceURL, "divReferenceBodyModal", isSuccessFunction);
                $("#divReferenceModal #hdnReferenceCode").val("APPLICATION_SOURCE");
                $("#divReferenceModal #hdnReferenceField").val("btnAddApplicationSource");
                $("#divReferenceModal").modal("show");
            });

            $("#lnkType").click(function () {
                var isSuccessFunction = function () {
                    $("#divReferenceModal .close").click(function () {
                        var htmlId = $(".AttachmentDynamicFields:last").prop("id");
                        var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("AttachmentDynamicFields_", ""));
                        GenerateDropdownValues(AttachmentTypeDropDown, "ddlType_" + ctr, "Value", "Description", "", "", false);
                    });
                };
                LoadPartialSuccessFunction(AddReferenceURL, "divReferenceBodyModal", isSuccessFunction);
                $("#divReferenceModal #hdnReferenceCode").val("ATTACHMENT_TYPE");
                $("#divReferenceModal #hdnReferenceField").val("lnkType");
                $("#divReferenceModal").modal("show");
            });

            $("#btnAddCourse").click(function () {
                var isSuccessFunction = function () {
                    $("#divReferenceModal .close").click(function () {
                        GenerateDropdownValues(CourseDropDown, "ddlCourse", "Value", "Description", "", "", false);
                    });
                };
                LoadPartialSuccessFunction(AddReferenceURL, "divReferenceBodyModal", isSuccessFunction);
                $("#divReferenceModal #hdnReferenceCode").val("COURSE");
                $("#divReferenceModal #hdnReferenceField").val("btnAddCourse");
                $("#divReferenceModal").modal("show");
            });

            $("#divApplicantModal #btnSave").click(function () {
                var isNoBlankFunction = function () {
                    //if (parseFloat($("#txtExpectedSalary").val()) <= 0) {
                    //    $("#txtExpectedSalary").addClass("errMessage");
                    //    $("#txtExpectedSalary").focus();
                    //    $("#frmApplicant #divApplicantErrorMessage").html(
                    //        "<label class=\"errMessage\"><li>" + GREATERTHAN_ZERO.replace("{0}", $("#txtExpectedSalary")[0].title) + "</li></label><br />");
                    //    return false;
                    //
                    //}
                    if (!ValidEmail($("#txtEmail"))) {
                        $("#txtEmail").addClass("errMessage");
                        $("#txtEmail").focus();
                        $("#divApplicantErrorMessage").append("<label class=\"errMessage\"><li>Email Address is invalid</li></label><br />");
                        return false;
                    }
                    else if (!validMobileNo($("#txtCellphoneNumber"))) {
                        $("#txtCellphoneNumber").addClass("errMessage");
                        $("#txtCellphoneNumber").focus();
                        $("#divApplicantErrorMessage").append("<label class=\"errMessage\"><li>Cellphone Number is invalid</li></label><br />");
                        return false;
                    }
                    else if ($(".AttachmentDynamicFields").length <= 0) {
                        objEMSCommonJS.ChangeTab($(".tablinks:nth-child(2)"), 'tabAttachment');
                        $("#divApplicantErrorMessage").append("<label class=\"errMessage\"><li>Attachment" + SUFF_REQUIRED + "</li></label><br />");
                        return false;
                    }
                    else {
                        return true;
                    }
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmApplicant", "#divApplicantErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , ApplicantAddPostURL \
                        ,  objApplicantAddJS.GetFormData() \
                        , '#divApplicantErrorMessage' \
                        , '#divApplicantModal #btnSave' \
                        , objApplicantAddJS.AddSuccessFunction); ",
                        "function");
                }
                else {
                    if ($("#tabPersonalInformation .errMessage").length > 0 && $("#tabPersonalInformation .errMessage").length ==
                        ($("#divApplicantModal .errMessage").length - $("#divApplicantErrorMessage .errMessage").length)) {
                        objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabPersonalInformation');
                    }
                    else if ($("#tabAttachment .errMessage").length > 0 && $("#tabAttachment .errMessage").length ==
                        ($("#divApplicantModal .errMessage").length - $("#divApplicantErrorMessage .errMessage").length)) {
                        objEMSCommonJS.ChangeTab($(".tablinks:nth-child(2)"), 'tabAttachment');
                    }
                }
            });

            $("#txtLastName").keyup(function () {
                function toTitleCase(str) {
                    return str.replace(/(?:^|\s)\w/g, function (match) {
                        return match.toUpperCase();
                    });
                }
                $("#txtLastName").val(toTitleCase($("#txtLastName").val()));
            });
            $("#txtFirstName").keyup(function () {
                function toTitleCase(str) {
                    return str.replace(/(?:^|\s)\w/g, function (match) {
                        return match.toUpperCase();
                    });
                }
                $("#txtFirstName").val(toTitleCase($("#txtFirstName").val()));
            });
            $("#txtMiddleName").keyup(function () {
                function toTitleCase(str) {
                    return str.replace(/(?:^|\s)\w/g, function (match) {
                        return match.toUpperCase();
                    });
                }
                $("#txtMiddleName").val(toTitleCase($("#txtMiddleName").val()));
            });
            $("#txtSuffix").keyup(function () {
                function toTitleCase(str) {
                    return str.replace(/(?:^|\s)\w/g, function (match) {
                        return match.toUpperCase();
                    });
                }
                $("#txtSuffix").val(toTitleCase($("#txtSuffix").val()));
            });

            objEMSCommonJS.BindAutoComplete("divApplicantModal #txtReferredByUserIDDescription"
                , RecruitmentReferredByAutoComplete
                , 20, "divApplicantModal #hdnReferredByUserID", "ID", "Description");

            //$("#txtReferredByUserIDDescription").autocomplete({
            //    source: function (request, response) {
            //        $.ajax({
            //            url: ReferredByAutoComplete, // URL 
            //            type: "GET",
            //            dataType: "json",
            //            data: {
            //                Term: $("#txtReferredByUserIDDescription").val(),
            //                TopResults: 20
            //            },
            //            success: function (data) {
            //                if (data.IsSuccess) {
            //                    response($.map(data.Result, function (item) {
            //                        return {
            //                            label: item.Description,
            //                            value: item.ID
            //                        };
            //                    }))
            //                }
            //                else {
            //                    ModalAlert(MODAL_HEADER, data.Result);
            //                }
            //            },
            //            error: function (jqXHR, textStatus, errorThrown) {
            //                ModalAlert(MODAL_HEADER, jqXHR.responseText);
            //            }
            //        })
            //    },
            //    select: function (event, ui) { // Event - triggers after selection on list
            //        if (ui.item.label != null) {
            //        }
            //        return false;
            //    },
            //    change: function (event, ui) { // Event - triggers when the value of the textbox changed
            //        if (ui.item == null) {
            //            $("#hdnReferredByUserID").val(0);
            //            $(this).val("");
            //        } else {
            //            $("#hdnReferredByUserID").val(ui.item.value);
            //            $(this).val(ui.item.label);
            //        }
            //    },
            //    focus: function (event, ui) {
            //        $(this).val(ui.item.label);
            //        event.preventDefault(); // Prevent the default focus behavior.
            //    }
            //});
        },

        GetFormData: function () {
            $(".Attachment_File").each(function (index) {
                $(this).prop("name", "Applicant.Attachments[" + index + "].File");
            });
            $(".Attachment_Type").each(function (index) {
                $(this).prop("name", "Applicant.Attachments[" + index + "].AttachmentType");
            });
            $(".Attachment_Remarks").each(function (index) {
                $(this).prop("name", "Applicant.Attachments[" + index + "].Remarks");
            });

            var formData = new FormData($('#frmApplicant').get(0));
            $(".Attachment_Type").each(function (index) {
                formData.append("Applicant.Attachments[" + index + "].ApplicantID", $("#hdnID").val());
            });

            formData.append("Applicant.PersonalInformation.CellphoneNumber", /*"+63" +*/ $("#txtCellphoneNumber").val().replace(/-/g, ""));

            return formData;
        },

        AddSuccessFunction: function () {
            $("#divApplicantFilter #btnSearch").click();
            $("#frmApplicant").trigger("reset");
            $("#DivUploadAttachmentDynamicFields").html("");
            objApplicantAddJS.AddAttachmentDynamicFields();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabPersonalInformation');
        },

        AddAttachmentDynamicFields: function () {
            var s = this;
            var htmlId = $(".AttachmentDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("AttachmentDynamicFields_", "")) + 1;

            $("#DivUploadAttachmentDynamicFields").append(
                "<div class=\"form-group form-fields AttachmentDynamicFields\" id=\"AttachmentDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "    </div>"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objApplicantListJS.RemoveDynamicFields('#AttachmentDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <select id=\"ddlType_" + ctr + "\" class= \"form-control required-field Attachment_Type\" title=\"Attachment Type\"></select >"
                + "    </div>"
                + "    <div class=\"col-md-3-5 no-padding\">"
                + "        <input id=\"txtRemarks_" + ctr + "\" maxlength=\"255\" class=\"form-control Attachment_Remarks\" title=\"Remarks\">"
                + "    </div>"
                + "    <div class=\"col-md-3 no-padding\">"
                + "        <input type=\"file\" id=\"fileAttachment_" + ctr + "\" class=\"form-control required-field Attachment_File\" title=\"Attachment\" accept=\".pdf,.doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document\">"
                + "    </div>"
                + "</div>"
            );

            objEMSCommonJS.PopulateDropDown("#ddlType_" + ctr, AttachmentTypeDropDownOptions);
        },

        GetAttachmentTypeDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    AttachmentTypeDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(AttachmentTypeDropDown, {}, "", GetSuccessFunction);
        }

        //ValidateEmail: function (email) {
        //    //var re = /^[^@]+@[^@]+\.[a-z-A-Z]{2,4}$/;
        //    //var re = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
        //    var re = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        //    return re.test(email);
        //},

        //ValidateMobileNumber: function (number) {
        //    var re = /^(09)\d{9}$/;
        //    return re.test(number);
        //}

    };
    
     objApplicantAddJS.Initialize();
});