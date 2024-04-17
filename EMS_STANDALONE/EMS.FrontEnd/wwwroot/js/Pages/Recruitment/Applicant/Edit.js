var objApplicantEditJS;

$(document).ready(function () {
    objApplicantEditJS = {

        ID: $("#divApplicantModal #hdnID").val(),

        Initialize: function () {
            $("#divApplicantBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass(".unreqField");
            $(".reqField").removeClass(".reqField");
            $("#btnEdit").hide();
            $("#hdnEditable").hide();
            $("#divApplicantModal #btnSave, #divApplicantModal #btnBack, #btnAddPreloaded, #btnAddActivity").show();
            $("#divApplicantModal .form-control").attr("readonly", false);
            s.GetAttachmentTypeDropDownOptions();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabPersonalInformation', "#divApplicantModal");

            deletedAttachments = [];
            // Reset Tab First Load tags
            personalInfoTabFirstLoad = false;
            attachmentTabFirstLoad = false;
            appHistoryTabFirstLoad = false;
            logActivityTabFirstLoad = false;
            legalProfileTabFirstLoad = false;

            //var MRFIDDropdownOptions = [];
            //var GetSuccessFunction = function (data) {
            //    $(data.Result).each(function (index, item) {
            //        MRFIDDropdownOptions.push(
            //            {
            //                Value: item.Value,
            //                Text: item.Text,
            //            });
            //    });
            //    objEMSCommonJS.PopulateDropDown("#ddlMRF", MRFIDDropdownOptions);
            //    $('#ddlMRF option').filter(function () {
            //        return ($(this).text() == $("#hdnMRFTransactionID").val());
            //    }).prop('selected', true);
            //    $('#ddlMRF').change();
            //};
            //objEMSCommonJS.GetAjax(MRFIDDropdownURL + "&ApplicantID=" + $("#divApplicantModal #hdnID").val(), {}, "", GetSuccessFunction);
            
            //objLogActivityJS.LoadLogActivityJQGrid({
            //    ApplicantID: $("#divApplicantModal #hdnID").val()
            //});
            $(".Attachment_CreatedDate").attr("readonly", true);

            //document.getElementById("txtCellphoneNumber").value = $("#txtCellphoneNumber").val().substr(3);


            if ($("#divApplicantModal #ddlPSGCProvince").val() != "" & $("#divApplicantModal #ddlPSGCCityMunicipality").val() == "") {
                $("#divApplicantModal #ddlPSGCProvince").trigger("change");
            }

            $("#txtCellphoneNumber").val() == (null || "") ? "" : $("#txtCellphoneNumber").val($("#txtCellphoneNumber").val().slice(0, 4) + "-" + $("#txtCellphoneNumber").val().substr(4));

        },

        ElementBinding: function () {
            var s = this;
            AmountTextChange($("#divApplicantModal #txtExpectedSalary"));
            AmountOnly($("#divApplicantModal #txtExpectedSalary"));
            NumberOnly($("#divApplicantModal #txtCellphoneNumber"));
            $("#dpBirthDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });
            $("#dpDateApplied").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#ddlMRF").change(function () {
                if ($(this).val() != "") {
                    var arr = ($("#ddlMRF").val()).split(',');
                    objApplicantViewJS.WorkflowTransactionJQGrid({
                        WorkflowID: parseInt(arr[1]),
                        MRFApplicantID: parseInt(arr[0])
                    });
                }
                else {
                    $("#tblApplicantWorkflowTransactionList").jqGrid("GridUnload");
                    $("#tblApplicantWorkflowTransactionList").jqGrid("GridDestroy");
                }

            });

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
                    $("#divReferenceModal .close").unbind();
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

            $("#lnkViewMRF").click(function () {
                $("#divApplicationHistoryErrorMessage").html("");
                var MRFTransaction = $("#ddlMRF option:selected").text();

                if (MRFTransaction == "" || MRFTransaction == "- Select an Item -")
                {
                    $("#ddlMRF").addClass("errMessage");
                    $("#ddlMRF").focus();
                    $("#divApplicationHistoryErrorMessage").append("<label class=\"errMessage\"><li>Select Tagged MRF ID</li></label><br />");
                }
                else
                {
                    $("#divApplicationHistoryErrorMessage").html("");

                    var GetSuccessFunction = function (data) {
                        //LoadPartial(MRFAddApplicantModalURL + "?ID=" + data.Result.ID, "divMRFAddApplicantBodyModal");
                        //$("#divApplicantModal").modal("hide");
                        //$("#divMRFAddApplicantModal").modal("show");
                        localStorage["ApplicantSelectedMRFID"] = data.Result.ID;
                        window.open("/manpower/mrf/admin");
                    };
                    objEMSCommonJS.GetAjax(GetMRFIDByMRFTransactionIDURL + "&MRFTransactionID=" + MRFTransaction, {}, "", GetSuccessFunction);
                }
                
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
                        , ApplicantEditPostURL \
                        ,  objApplicantEditJS.GetFormData() \
                        , '#divApplicantErrorMessage' \
                        , '#divApplicantModal #btnSave' \
                        , objApplicantEditJS.EditSuccessFunction); ",
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

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , ApplicantDeleteURL + '?ID=' + objApplicantEditJS.ID \
                    , {} \
                    , '#divApplicantErrorMessage' \
                    , '#btnDelete' \
                    , objApplicantEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#divApplicantModal #btnBack").click(function () {
                LoadPartial(ApplicantViewURL + "?ID=" + objApplicantEditJS.ID, "divApplicantBodyModal");
            });

            $(".tablinks").find("span:contains('Application History')").parent("button").click(function () {
                if (!appHistoryTabFirstLoad) {
                    var MRFIDDropdownOptions = [];
                    var GetSuccessFunction = function (data) {
                        $(data.Result).each(function (index, item) {
                            MRFIDDropdownOptions.push(
                                {
                                    Value: item.Value,
                                    Text: item.Text,
                                });
                        });
                        objEMSCommonJS.PopulateDropDown("#ddlMRF", MRFIDDropdownOptions);
                        $('#ddlMRF option').filter(function () {
                            return ($(this).text() == $("#hdnMRFTransactionID").val());
                        }).prop('selected', true);
                        $('#ddlMRF').change();
                    };
                    objEMSCommonJS.GetAjax(MRFIDDropdownURL + "&ApplicantID=" + $("#divApplicantModal #hdnID").val(), {}, "", GetSuccessFunction);
                    appHistoryTabFirstLoad = true;
                }
            });

            $(".tablinks").find("span:contains('Task Checklist')").parent("button").click(function () {
                if (!logActivityTabFirstLoad) {
                    //objLogActivityJS.LoadLogActivityJQGrid({
                    //    ApplicantID: $("#divApplicantModal #hdnID").val()
                    //});
                    var isSuccessFunction = function () {
                        $("#divApplicantModal #tabLogActivity #btnAddPreloaded, \
                            #divApplicantModal #tabLogActivity #btnAddActivity,   \
                            #divApplicantModal #tabLogActivity #btnBatchAssign").show();
                    };
                    LoadPartialSuccessFunction(ApplicantLogActivityURL, "divApplicantModal #tabLogActivity", isSuccessFunction);
                    logActivityTabFirstLoad = true;
                    return false;
                }
            });


            $(".tablinks").find("span:contains('Legal Profile')").parent("button").click(function () {
                if (!legalProfileTabFirstLoad) {
                    var isSuccessFunction = function () {
                        objLegalProfileJS.IsViewMode = true;
                    };
                    LoadPartialSuccessFunction(ApplicantLegalProfileURL, "divApplicantModal #tabLegalProfile", isSuccessFunction);
                    legalProfileTabFirstLoad = true;
                    return false;
                }
            });
        },

        DeleteAttachmentFunction: function (serverFile) {
            deletedAttachments.push(serverFile);
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divApplicantModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#divApplicantModal #btnBack").click();
        },

        GetFormData: function () {
            $(".Attachment_File").each(function (index) {
                var ctr = parseInt(index) + parseInt($("#DivDownloadAttachmentDynamicFields .AttachmentDynamicFields").length);
                $(this).prop("name", "Applicant.Attachments[" + ctr + "].File");
            });
            $(".Attachment_Type").each(function (index) {
                $(this).prop("name", "Applicant.Attachments[" + index + "].AttachmentType");
            });
            $(".Attachment_Remarks").each(function (index) {
                $(this).prop("name", "Applicant.Attachments[" + index + "].Remarks");
            });
            $(".Attachment_SourceFile").each(function (index) {
                $(this).prop("name", "Applicant.Attachments[" + index + "].SourceFile");
            });
            $(".Attachment_ServerFile").each(function (index) {
                $(this).prop("name", "Applicant.Attachments[" + index + "].ServerFile");
            });

            var formData = new FormData($('#frmApplicant').get(0));
            $(".Attachment_Type").each(function (index) {
                formData.append("Applicant.Attachments[" + index + "].ApplicantID", $("#divApplicantModal #hdnID").val());
            });
            $(deletedAttachments).each(function (index, item) {
                formData.append("Applicant.DeletedAttachments[" + index + "].ServerFile", item);
            });

            formData.append("Applicant.PersonalInformation.CellphoneNumber", /*"+63" +*/ $("#txtCellphoneNumber").val().replace(/-/g, ""));

            return formData;
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
                + "    <div class=\"col-md-2-5 no-padding\">"
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
        },

        WorkflowTransactionJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblApplicantWorkflowTransactionList").jqGrid("GridUnload");
            $("#tblApplicantWorkflowTransactionList").jqGrid("GridDestroy");
            $("#tblApplicantWorkflowTransactionList").jqGrid({
                url: WorkflowTransactionURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Order", "Step", "Status", "Date Scheduled", "Date Completed",
                    "Timestamp", "Remarks"],
                colModel: [
                    { name: "", hidden: true },
                    { name: "Order", index: "Type", align: "left", sortable: false, hidden: true },
                    { name: "Step", index: "Title", align: "left", sortable: false },
                    { name: "Status", index: "Status", align: "left", sortable: false },
                    { name: "DateScheduled", index: "DateScheduled", align: "left", sortable: false },
                    { name: "DateCompleted", index: "DateCompleted", align: "left", sortable: false },
                    { name: "Timestamp", index: "Timestamp", align: "left", sortable: false },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false }
                ],
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblApplicantWorkflowTransactionList", data);

                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },
    };
    
     objApplicantEditJS.Initialize();
});