var objEmployeeMovementAddListJS;
var AutoCompleteByMovementTypeURL = "/Plantilla/Employee/MovementAdd?handler=AutoCompleteByMovementType";
var AutoPopulateByMovementTypeURL = "/Plantilla/Employee/MovementAdd?handler=AutoPopulateByMovementType";
var MovementAddPostURL = "/Plantilla/Employee/MovementAdd";
var EmployeeFieldAddPostURL = "/Plantilla/Employee/AddEmployeeField";
var EmployeeFieldBulkDeleteListURL = "/Plantilla/Employee/MovementAdd?handler=BulkDelete";
var MovementUpdatePostURL = "/Plantilla/Employee/MovementAdd?handler=Update";
var MovementUpdateDateToPostURL = "/Plantilla/Employee/MovementAdd?handler=UpdateDateTo";

var isEdit = false;
var EmployeeFieldValue;
//For movement checker 8, 71-119, 337-349, 440-463, 880, 902-906
$(document).ready(function () {
    objEmployeeMovementAddListJS = {
        Initialize: function () {
            $("#divMovementAddBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            $("#AddMovementField").show();
            $("#EditMovementField").hide();
            $("#UpdateMovementField").hide();
            
            s.ElementBinding();
            if (($("#hdnIsEdit").val() || "").toLowerCase() == "true") {
                $('#ddlEmployeeField option:not(:selected)').remove();
                $('#ddlMovementType option:not(:selected)').remove();
                $("#ddlEmployeeField").attr("readonly", true);
                $("#ddlMovementType").attr("readonly", true);
                $("#txtNewValue").attr("readonly", true);
                $("#dpEffectiveDateFrom").attr("readonly", true);
                $("#dpEffectiveDateTo").attr("readonly", true);
                $('#idInputDetails').summernote({
                    toolbar: [
                    ]
                });
                $('#idInputDetails').summernote('disable');
            }
            else {
                s.LoadJQGrid(false, "", "");
                $("#idInputDetails").on("summernote.enter", function (we, e) {
                    $(this).summernote("pasteHTML", "<br>");
                    e.preventDefault();
                });
                $('#idInputDetails').summernote({
                    toolbar: [
                        ['style', ['clear', 'bold', 'italic', 'underline']]
                    ]
                });
            }
            $("#cbUseCurrent").prop("checked", $("#hdnUseCurrent").val() == "true");
            $("#cbUseCurrent").change();
            $("#employeeFieldTable").css("display", "none");
            $("#oldValue").css("display", "none");
            $("#newValue").css("display", "none");


            if ($("#divMovementAddModal #ddlMovementType").val() != "" && $("#idMovementSpecialCases").val().includes($("#divMovementAddModal #ddlMovementType").val()))
                $("#idDivDetails").show();
            else
                $("#idDivDetails").hide();


        },

        ElementBinding: function () {
            var s = this;

            $("#employeeField").css("display", "none");
            $("#compensationInput").val("");

            $("#divMovementAddBodyModal #btnChangeStatus").on("click", function () {
                if ($("#divMovementAddBodyModal #ChangeStatusModal").is(":visible"))
                    $("#divMovementAddBodyModal #ChangeStatusModal").hide();
                else
                {
                    $("#divMovementAddBodyModal #ChangeStatusModal").show();
                    GenerateDropdownValues(MovementBulkChangeStatus + "&CurrentStatus=" + $("#hdnMovementStatus").val(), "divMovementAddBodyModal #ddlDynamicChangeStatus", "Value", "Text", "", "", false);
                }

            });
            $("#divMovementAddBodyModal #btnCancelDynamicChangeStatus").on("click", function () {
                $("#divMovementAddBodyModal #ChangeStatusModal").hide();
            });
            $("#divMovementAddBodyModal #ddlDynamicChangeStatus").on("change", function () {
                $("#divMovementAddBodyModal #divDynamicChangeStatusErrorMessage").html("");
                if ($("#divMovementAddBodyModal #ddlDynamicChangeStatus :selected").val() == "CANCELLED") {
                    $("#divMovementAddBodyModal #spnDynamicChangeStatus").addClass("reqField");
                    $("#divMovementAddBodyModal #spnDynamicChangeStatus").removeClass("unreqField");
                    $("#divMovementAddBodyModal #txtDynamicChangeStatusRemarks").addClass("required-field");
                }
                else {
                    $("#divMovementAddBodyModal #spnDynamicChangeStatus").addClass("unreqField");
                    $("#divMovementAddBodyModal #spnDynamicChangeStatus").removeClass("reqField");
                    $("#divMovementAddBodyModal #txtDynamicChangeStatusRemarks").removeClass("required-field");
                }
            });
            $("#divMovementAddBodyModal #btnSaveDynamicChangeStatus").on("click", function () {
                $("#divMovementAddBodyModal #divDynamicChangeStatusErrorMessage").html("");

                if ($("#divMovementAddBodyModal #ddlDynamicChangeStatus :selected").val() == "") {
                    $("#divMovementAddBodyModal #divDynamicChangeStatusErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + " Movement</li></label><br />");
                    return;
                }

                var isNoBlankFunction = function () {
                    return true;
                };
                if (objEMSCommonJS.ValidateBlankFields("#divMovementAddModal #divMovementAddBodyModal #frmMovement", "#divMovementAddModal #divMovementAddBodyModal #divDynamicChangeStatusErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , MovementChangeStatusPostURL + "+("'&ID="+ $("#hdnMovementID").val() +"'")+" \
                        , objEmployeeMovementAddListJS.ChangeStatusFormData() \
                        , '#divMovementAddBodyModal #divDynamicChangeStatusErrorMessage' \
                        , '#divMovementAddBodyModal #btnSaveDynamicChangeStatus' \
                        , objEmployeeMovementAddListJS.ChangeStatusSuccessFunction);",
                        "function");
                }
            });

            if ($("#divMovementAddModal #ddlEmployeeField").val() != "") {

                var movementType = $("#ddlMovementType option:selected").text();
                movementType = movementType.toLowerCase().replace(/\b[a-z]/g, function (letter) {
                    return letter.toUpperCase();
                });
                $("label[for='txtOldValue']").html("<span class='unreqField'>* </span>Old '" + movementType + "' Value");
                $("label[for='txtNewValue']").html("<span class='reqField'>* </span>New '" + movementType + "' Value");
            }

            $("#divMovementAddModal #dpEffectiveDateFrom, #divMovementAddModal #dpEffectiveDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY',
                minDate: $("#dpDateHired").val(),
                //maxDate: new Date()
            });

            $('#divMovementAddModal #dpEffectiveDateFrom, #divMovementAddModal #dpEffectiveDateTo').datetimepicker().on('dp.show', function () {
                $('#divMovementAddModal .modal-body').css({ 'overflow': 'visible' });
                $('#divMovementAddModal.modal').css({ 'overflow': 'visible' });
            }).on('dp.hide', function () {
                $('#divMovementAddModal .modal-body').css({ 'overflow': 'auto' });
                $('#divMovementAddModal.modal').css({ 'overflow': 'auto' });
            });

            $("#divMovementAddModal #dpEffectiveDateFrom").on("dp.change", function (e) {
                $('#divMovementAddModal #dpEffectiveDateTo').data("DateTimePicker").minDate(e.date);
            });
            $("#divMovementAddModal #dpEffectiveDateTo").on("dp.change", function (e) {
                $('#divMovementAddModal #dpEffectiveDateFrom').data("DateTimePicker").maxDate(e.date);
            });

            $("#divMovementAddModal #ddlMovementType").change(function () {

                if ($("#idMovementSpecialCases").val().includes($(this).val()))
                    $("#idDivDetails").show();
                else
                    $("#idDivDetails").hide();

                if ($("#ddlMovementType").val() != "") {
                    var GetSuccessFunction = function (data) {
                        //s.AddSignatoriesDynamicFields(data);
                        if (data.condition == false) {
                            $("#oldValue").css("display", "none");
                            $("#newValue").css("display", "none");
                            $("#employeeFieldTable").css("display", "none");
                            $("#tblEmployeeFieldList td").html("");
                            $("#tblEmployeeFieldList td input").val("");
                            $("#ddlEmployeeField").val(data.empField).change();
                        }
                        else {
                            $("#divMovementEmployeeFieldErrorMessage").html("");
                            $("#divMovementEmployeeFieldErrorMessage").css("display", "none");
                            //hide employee field, old value and new value fields
                            $("#oldValue").css("display", "none");
                            $("#newValue").css("display", "none");
                            $("#divAdditionalFields").css("display", "none");
                            $("#employeeFieldTable").css("display", "block");
                            //remove class 'required-field' of hidden fields
                            $("#ddlEmployeeField").removeClass("required-field");
                            $("#ddlEmployeeField").val("").change();
                            $("#txtNewValue").removeClass("required-field");

                            if ($("#ddlMovementType").val() == "ADD_BRANCH_ASSIGN"
                                || $("#ddlMovementType").val() == "TEMP_BRANCH_ASSIGN"
                            ) {
                                $("#btnAddField").hide();
                                $("#btnRemove").hide();
                            }
                            else {
                                $("#btnAddField").show();
                                $("#btnRemove").show();
                            }

                            s.LoadJQGrid(false, "", "","");
                        }
                    };
                    objEMSCommonJS.GetAjax(GetEmploymentMovementMappingURL + "&MovementType=" + $("#ddlMovementType").val(), {}, "", GetSuccessFunction);

                }
                else {
                    $("#oldValue").css("display", "block");
                    $("#newValue").css("display", "block");
                    $("#employeeFieldTable").css("display", "none");
                    $("#tblEmployeeFieldList td").html("");
                    $("#tblEmployeeFieldList td input").val("");
                    $("#ddlEmployeeField").val("").change();
                }
            });

            $("#divMovementAddModal #btnSaveEmployeeMovement").click(function () {
                if ($("#employeeFieldTable").css("display") == "block") {
                    if ($("#tblEmployeeFieldList").getGridParam("reccount") > 0) {

                        if (objEMSCommonJS.ValidateBlankFields("#frmMovement", "#divMovementErrorMessage")) {
                            ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                                "objEMSCommonJS.PostAjax(true \
                            , MovementAddPostURL \
                            , objEmployeeMovementAddListJS.GetFormData() \
                            , '#divMovementErrorMessage' \
                            , '#divMovementAddModal #btnSaveEmployeeMovement' \
                            , objEmployeeMovementAddListJS.AddSuccessFunction);",
                                "function");
                        }
                    }
                    else {
                        $("#divMovementEmployeeFieldErrorMessage").css("display", "block");
                        //ModalAlert(MODAL_HEADER, "Please select atleast one task.");
                        $("#divMovementEmployeeFieldErrorMessage").html("<label class=\"errMessage\"><li>" + "Please add atleast one employee field." + "</li></label><br />");
                    }
                }

                else {
                    if (objEMSCommonJS.ValidateBlankFields("#frmMovement", "#divMovementErrorMessage")) {
                        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                            "objEMSCommonJS.PostAjax(true \
                            , MovementAddPostURL \
                            , objEmployeeMovementAddListJS.GetFormData() \
                            , '#divMovementErrorMessage' \
                            , '#divMovementAddModal #btnSaveEmployeeMovement' \
                            , objEmployeeMovementAddListJS.AddSuccessFunction);",
                            "function");
                        }
                }
            });

            $("#divMovementAddModal #btnEditEmployeeMovement").click(function () {
                        
                $('.modal-header p').each(function () {
                    var text = $(this).text();
                    $(this).text(text.replace('View', 'Edit'));
                });

                isEdit = true;

                $("#idInputDetails").on("summernote.enter", function (we, e) {
                    $(this).summernote("pasteHTML", "<br>");
                    e.preventDefault();
                });
                $('#idInputDetails').summernote('destroy');
                $('#idInputDetails').summernote({
                    toolbar: [
                        ['style', ['clear', 'bold', 'italic', 'underline']],
                    ]
                });

                // Btn Fields 
                $("#AddMovementField").hide();
                $("#EditMovementField").hide();
                $("#UpdateMovementField").show();

                if ($("#hdnMovementStatus").val() == "APPROVED") {
                    $("#dpEffectiveDateTo").attr("readonly", false);
                    $("#cbUseCurrent").prop("disabled", false);
                    return;
                }

                // fields to disable
                $("#dpEffectiveDateFrom").attr("readonly", false);
                $("#dpEffectiveDateTo").attr("readonly", false);
                $("#cbUseCurrent").prop("disabled", false);
                $("#txtReason").attr("readonly", false);
                $("#txtHRDComments").attr("readonly", false);

                if ($("#ddlMovementType").val() != "") {
                    var GetSuccessFunction = function (data) {
                        //s.AddSignatoriesDynamicFields(data);
                        if (data.condition == false) {
                            $("#oldValue").css("display", "none");
                            $("#newValue").css("display", "none");
                            $("#employeeFieldTable").css("display", "none");
                            $("#tblEmployeeFieldList td").html("");
                            $("#tblEmployeeFieldList td input").val("");
                            $("#ddlEmployeeField").val(data.empField).change();
                        }
                        else {
                            $("#divMovementEmployeeFieldErrorMessage").html("");
                            $("#divMovementEmployeeFieldErrorMessage").css("display", "none");
                            //hide employee field, old value and new value fields
                            $("#oldValue").css("display", "none");
                            $("#newValue").css("display", "none");
                            $("#divAdditionalFields").css("display", "none");
                            $("#employeeFieldTable").css("display", "block");
                            //remove class 'required-field' of hidden fields
                            $("#ddlEmployeeField").removeClass("required-field");
                            $("#ddlEmployeeField").val("").change();
                            $("#txtNewValue").removeClass("required-field");

                            if ($("#ddlMovementType").val() == "ADD_BRANCH_ASSIGN"
                                || $("#ddlMovementType").val() == "TEMP_BRANCH_ASSIGN"
                            ) {
                                $("#btnAddField").hide();
                                $("#btnRemove").hide();
                            }
                            else {
                                $("#btnAddField").show();
                                $("#btnRemove").show();
                            }
                            s.LoadJQGrid(true, "", "", EmployeeFieldValue);
                        }
                    };
                    objEMSCommonJS.GetAjax(GetEmploymentMovementMappingURL + "&MovementType=" + $("#ddlMovementType").val(), {}, "", GetSuccessFunction);

                }
                else {
                    $("#oldValue").css("display", "block");
                    $("#newValue").css("display", "block");
                    $("#employeeFieldTable").css("display", "none");
                    $("#tblEmployeeFieldList td").html("");
                    $("#tblEmployeeFieldList td input").val("");
                    $("#ddlEmployeeField").val("").change();
                }

            });


            $("#divMovementAddModal #btnUpdateEmployeeMovement").click(function () {

                if ($("#hdnMovementStatus").val() == "APPROVED") {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , MovementUpdateDateToPostURL \
                        , objEmployeeMovementAddListJS.GetFormData() \
                        , '#divMovementAddBodyModal #divMovementErrorMessage' \
                        , '#divMovementAddBodyModal #btnUpdateEmployeeMovement' \
                        , objEmployeeMovementAddListJS.AddSuccessFunction);",
                        "function");
                    return;
                }

                if ($("#employeeFieldTable").css("display") == "block") {
                    if ($("#tblEmployeeFieldList").getGridParam("reccount") > 0) {

                        if (objEMSCommonJS.ValidateBlankFields("#frmMovement", "#divMovementErrorMessage")) {

                            ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                                "objEMSCommonJS.PostAjax(true \
                            , MovementUpdatePostURL \
                            , objEmployeeMovementAddListJS.GetFormData() \
                            , '#divMovementErrorMessage' \
                            , '#divMovementAddModal #btnUpdateEmployeeMovement' \
                            , objEmployeeMovementAddListJS.DeleteSuccessFunction);",
                                "function");
                        }
                    }
                    else {
                        $("#divMovementEmployeeFieldErrorMessage").css("display", "block");
                        //ModalAlert(MODAL_HEADER, "Please select atleast one task.");
                        $("#divMovementEmployeeFieldErrorMessage").html("<label class=\"errMessage\"><li>" + "Please add atleast one employee field." + "</li></label><br />");
                    }
                }

                else {
                    if (objEMSCommonJS.ValidateBlankFields("#frmMovement", "#divMovementErrorMessage")) {

                        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                            "objEMSCommonJS.PostAjax(true \
                            , MovementUpdatePostURL \
                            , objEmployeeMovementAddListJS.GetFormData() \
                            , '#divMovementErrorMessage' \
                            , '#divMovementAddModal #btnUpdateEmployeeMovement' \
                            , objEmployeeMovementAddListJS.DeleteSuccessFunction);",

                            "function");
                       // , objEmployeeMovementAddListJS.DeleteSuccessFunction); ",

                    }
                }
            });


            $("#cbUseCurrent").change(function () {
                if ($(this).prop("checked")) {
                    $('#divMovementAddModal #dpEffectiveDateTo').val("");
                    $('#divMovementAddModal #dpEffectiveDateTo').attr("readonly", true);
                    $("#divMovementAddModal #dpEffectiveDateTo").removeClass("required-field");
                    $("label[for='dpEffectiveDateTo'] > span").removeClass("reqField");
                    $("label[for='dpEffectiveDateTo'] > span").addClass("unreqField");
                }
                else {
                    //$('#divMovementAddModal #dpEffectiveDateTo').val("");
                    $('#divMovementAddModal #dpEffectiveDateTo').attr("readonly", false);
                    $("#divMovementAddModal #dpEffectiveDateTo").addClass("required-field");
                    $("label[for='dpEffectiveDateTo'] > span").addClass("reqField");
                    $("label[for='dpEffectiveDateTo'] > span").removeClass("unreqField");
                }
            });

            $("#employeeFieldTable #btnAddField").click(function () {
                LoadPartial(EmployeeFieldAddURL, "divEmployeeFieldBodyModal");
                $("#divEmployeeFieldModal").modal("show");
            });

            $("#btnRemove").click(function () {

                //var input = $('#tblEmployeeFieldList').jqGrid('getGridParam', 'selarrrow');
                var input = $("#tblEmployeeFieldList input[type='checkbox']:checked");

                if (input.length == 0) {
                    $("#divMovementEmployeeFieldErrorMessage").css("display", "block");
                    //ModalAlert(MODAL_HEADER, "Please select atleast one task.");
                    $("#divMovementEmployeeFieldErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select atleast one record" + "</li></label><br />");
                }

                else {
                    $("#divMovementEmployeeFieldErrorMessage").html("");
                    $("#divMovementEmployeeFieldErrorMessage").css("display", "none");
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                            , EmployeeFieldBulkDeleteListURL \
                            , objEmployeeMovementAddListJS.DeleteFormData() \
                            , '#divMovementEmployeeFieldErrorMessage' \
                            , '#employeeFieldTable #btnRemove' \
                            , objEmployeeMovementAddListJS.DeleteSuccessFunction);",
                        "function");
                }
            });

        },

        ChangeStatusFormData: function () {
            var formData = new FormData($('#divMovementAddModal #DynamicChangeStatusForm').get(0));

            formData.append("ChangeStatus.Status", $("#divMovementAddModal #ddlDynamicChangeStatus :selected").val());
            formData.append("ChangeStatus.Remarks", $("#divMovementAddModal #txtDynamicChangeStatusRemarks").val());
            return formData;
        },
        ChangeStatusSuccessFunction: function () {
            $("#tabMovement #btnSearch").click();
            $("#divEmployeeList #btnSearch").click();
            // View mode
            var successFunction = function () {
                $(".tablinks").find("span:contains('Movement')").parent("button").click();
            };
            if ($("#frmEmployee #btnSave").css("display") == "none") {
                LoadPartialSuccessFunction(EmployeeViewURL + "?ID=" + $("#frmEmployee #hdnID").val(), "divEmployeeBodyModal", successFunction);
            }
            else {
                LoadPartialSuccessFunction(EmployeeViewURL + "?ID=" + $("#frmEmployee #hdnID").val(), "divEmployeeBodyModal");
            }

            $("#divMovementAddModal").modal("hide");
        },

        LoadJQGrid: function (isEditMode, FromValue, ToValue, EmployeeFieldType) {
            //GET EMPLOYEE FIELD EMBEDDED
            EmployeeFieldValue = EmployeeFieldType;
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblEmployeeFieldList") == "" ? "" : $.parseJSON(localStorage.getItem("tblEmployeeFieldList"));
            $("#tblEmployeeFieldList").jqGrid("GridUnload");
            $("#tblEmployeeFieldList").jqGrid("GridDestroy");
            $("#tblEmployeeFieldList").jqGrid({
                url: EmployeeFieldListURL + $("#ddlMovementType").val() + "&EmployeeFieldType=" + EmployeeFieldType,
                postData: "",
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "", "Code", "Employee Field", "<span class='reqField'>* </span> Old Value", "<span class='reqField'>* </span> New Value"],
                colModel: [
                    { hidden: true },
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center" },
                    { hidden: true, name: "EmployeeFieldCode", index: "EmployeeFieldCode", align: "center" },
                    { name: "EmployeeField", index: "EmployeeField", align: "center" },
                    { name: "OldValue", index: "OldValue", align: "center", editable: true },
                    { name: "NewValue", index: "NewValue", align: "center", editable: true }

                ],
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false
                },
                emptyrecords: "No records to display",
                multiselect: true,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: false,
                cellEdit: false,
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

                        $.each(data.rows, function (i, index) {
                            if (index.EmployeeFieldCode == "SECONDARY_DESIG")
                                $("#tblEmployeeFieldList").hideCol("OldValue");
                        });


                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblEmployeeFieldList", data);
                        
                        if (!isEditMode) {

                            s.LoadMovementTable($(this), false);
                        }
                        else {
                            // IF EDIT
                            if (isEdit == true) {
                                s.LoadMovementTable($(this),true);
                            }
                            else {
                                $("td[aria-describedby='tblEmployeeFieldList_OldValue']").text(FromValue);
                                $("td[aria-describedby='tblEmployeeFieldList_NewValue']").text(ToValue);
                                $("#cbUseCurrent").prop("disabled", true);
                            }
                        }
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblEmployeeFieldList");
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
        },

        LoadMovementTable: function (table,isEdit) {
            var $this = table, ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
            for (i = 0; i < l; i++) {
                $this.jqGrid('editRow', ids[i], true);
            }

            $("#tblEmployeeFieldList tr").attr("id", "txtRowEmployeeField");
            $("#tblEmployeeFieldList td[aria-describedby='tblEmployeeFieldList_EmployeeFieldCode']").attr("id", "txtEmployeeField");
            //$("#tblEmployeeFieldList td[aria-describedby='tblEmployeeFieldList_OldValue']").attr("id", "txtOldValueTbl");
            $("#tblEmployeeFieldList td[aria-describedby='tblEmployeeFieldList_NewValue'] input").addClass("form-control")
                .addClass("required-field")
                .addClass("newValueInput")
                .blur();

            $("input.newValueInput").each(function () {
                $(this).after("<input type=\"hidden\" title=\"New Value\" class=\"newValueInputID\" />");
            });
            // test start
            $("#tblEmployeeFieldList td[aria-describedby='tblEmployeeFieldList_OldValue'] input").addClass("form-control")
                .addClass("oldValueInput")
                .blur();

            $("input.oldValueInput").each(function () {
                $(this).after("<input type=\"hidden\" title=\"Old Value\" class=\"oldValueInputID\" />");
            });
            // test end 

            $("tr#txtRowEmployeeField").each(function () {
                //POSITION
                if (!$("#ddlPosition").val() == "") {
                    $("td#txtEmployeeField:contains(POSITION) + td + td + td .newValueInput").attr("id", "positionInput").attr("title", "New Position");
                    $("td#txtEmployeeField:contains(POSITION) + td + td + td .newValueInputID").attr("id", "hdnNewValueID_position");

                    $("td#txtEmployeeField:contains(POSITION) + td + td .oldValueInput").attr("id", "positionOldInput").attr("title", "Old Position");
                    $("td#txtEmployeeField:contains(POSITION) + td + td .oldValueInputID").attr("id", "hdnOldValueID_position");
                }

                //ORG GROUP
                if (!$("#ddlOrgGroup").val() == "") {
                    $("td#txtEmployeeField:contains(ORG_GROUP) + td + td + td .newValueInput").attr("id", "orgGroupNewInput").attr("title", "New Org Group");
                    $("td#txtEmployeeField:contains(ORG_GROUP) + td + td + td .newValueInputID").attr("id", "hdnNewValueID_orgGroup");

                    $("td#txtEmployeeField:contains(ORG_GROUP) + td + td .oldValueInput").attr("id", "orgGroupOldInput").attr("title", "Old Org Group");
                    $("td#txtEmployeeField:contains(ORG_GROUP) + td + td .oldValueInputID").attr("id", "hdnOldValueID_orgGroup");
                }


                //EMPLOYMENT STATUS
                if (!$("#ddlStatus").val() == "") {
                    $("td#txtEmployeeField:contains(EMPLOYMENT_STATUS) + td + td + td .newValueInput").attr("id", "employmentStatusInput").attr("asp-for","EmployeeMovement.NewValue").attr("title", "New Employment Status");
                    $("td#txtEmployeeField:contains(EMPLOYMENT_STATUS) + td + td + td .newValueInputID").attr("id", "hdnNewValueID_empStatus");

                    $("td#txtEmployeeField:contains(EMPLOYMENT_STATUS) + td + td .oldValueInput").attr("id", "employmentStatusOldInput").attr("title", "Old Employment Status");
                    $("td#txtEmployeeField:contains(EMPLOYMENT_STATUS) + td + td .oldValueInputID").attr("id", "hdnOldValueID_empStatus");
                }

                //COMPANY
                if (!$("#ddlCompany").val() == "") {
                    $("td#txtEmployeeField:contains(COMPANY) + td + td + td .newValueInput").attr("id", "companyInput").attr("title", "New Company");
                    $("td#txtEmployeeField:contains(COMPANY) + td + td + td .newValueInputID").attr("id", "hdnNewValueID_company");

                    $("td#txtEmployeeField:contains(COMPANY) + td + td .oldValueInput").attr("id", "companyOldInput").attr("title", "Old Company");
                    $("td#txtEmployeeField:contains(COMPANY) + td + td .oldValueInputID").attr("id", "hdnOldValueID_company");
                }

                //CIVIL STATUS
                if (!$("#ddlCivilStatusCode").val() == "") {
                    $("td#txtEmployeeField:contains(CIVIL_STATUS) + td + td + td .newValueInput").attr("id", "civilStatusInput").attr("title", "New Civil Status");
                    $("td#txtEmployeeField:contains(CIVIL_STATUS) + td + td + td .newValueInputID").attr("id", "hdnNewValueID_civilStatus");

                    $("td#txtEmployeeField:contains(CIVIL_STATUS) + td + td .oldValueInput").attr("id", "civilStatusOldInput").attr("title", "Old Civil Status");
                    $("td#txtEmployeeField:contains(CIVIL_STATUS) + td + td .oldValueInputID").attr("id", "hdnOldValueID_civilStatus");
                }

                //EXEMPTION STATUS
                if (!$("#ddlExemptionStatus").val() == "") {
                    $("td#txtEmployeeField:contains(EXEMPTION_STATUS) + td + td + td .newValueInput").attr("id", "exemptionStatusInput");
                    $("td#txtEmployeeField:contains(EXEMPTION_STATUS) + td + td + td .newValueInputID").attr("id", "hdnNewValueID_exempStatus");

                    $("td#txtEmployeeField:contains(EXEMPTION_STATUS) + td + td .oldValueInput").attr("id", "exemptionStatusOldInput");
                    $("td#txtEmployeeField:contains(EXEMPTION_STATUS) + td + td .oldValueInputID").attr("id", "hdnOldValueID_exempStatus");
                }

                //SECONDARY DESIGNATION
                $("td#txtEmployeeField:contains(SECONDARY_DESIG) + td + td + td .newValueInput").attr("id", "orgGroupInput2").attr("placeholder", "Org Group").attr("name", "EmployeeMovement.NewValue").attr("title", "New Org Group");
                $("td#txtEmployeeField:contains(SECONDARY_DESIG) + td + td + td .newValueInputID").attr("id", "hdnNewValueID_orgGroup2");

                //test NewValue - OldValue
                $("td#txtEmployeeField:contains(SECONDARY_DESIG) + td + td .oldValueInput").attr("id", "orgGroupOldInput2").attr("placeholder", "Org Group").attr("name", "EmployeeMovement.OldValue").attr("title", "Old Org Group");
                $("td#txtEmployeeField:contains(SECONDARY_DESIG) + td + td .oldValueInputID").attr("id", "hdnOldValueID_orgGroup2");

                //COMPENSATION
                $("td#txtEmployeeField:contains(COMPENSATION) + td + td").addClass("compensation_oldValue");
                if ($(".tablinks").find("span:contains('Compensation')").length > 0) {
                    $(".compensation_oldValue").html($("#txtMonthlySalary").val());
                }
                else
                    $(".compensation_oldValue").html("*****");
                $("td#txtEmployeeField:contains(COMPENSATION) + td + td + td .newValueInput").attr("id", "compensationInput");

                AmountOnly($("#compensationInput"));
                $("#compensationInput").blur(function () {
                    if ($(this).val() != "") {
                        var m = parseFloat($(this).val().noComma());
                        $(this).val(m.toFixed(2).commaOnAmount());
                    }
                });

            });

            $("#hdnNewValueID_orgGroup2").after(
                "<input type=\"text\" id=\"txtNewValue2\" maxlength=\"255\" class=\"form-control required-field\" placeholder=\"Position\" title=\"Position\" /> \
                                    <input type=\"hidden\" id=\"hdnNewValueID2\" /> \
                                    <div hidden><input type=\"radio\" id=\"add\" name=\"add_remove\" value=\"add\" checked> \
                                    <label for=\"add\" class=\"control-label block-label\">Add</label> \
                                    &nbsp; &nbsp; \
                                    <input type=\"radio\" id=\"remove\" name=\"add_remove\" value=\"remove\"> \
                                    <label for=\"remove\" class=\"control-label block-label\">Remove</label></div>");

            //AUTOCOMPLETE
            /* POSITION */
            objEMSCommonJS.BindAutoComplete("employeeFieldTable #positionInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=POSITION&OrgGroup=" + $("#divEmployeeBodyModal #ddlOrgGroup option:selected").val()
                , 20, "employeeFieldTable #hdnNewValueID_position");

            objEMSCommonJS.BindAutoComplete("employeeFieldTable #positionOldInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=POSITION&OrgGroup=" + $("#divEmployeeBodyModal #ddlOrgGroup option:selected").val()
                , 20, "employeeFieldTable #hdnOldValueID_position");

            /* ORG GROUP */
            objEMSCommonJS.BindAutoComplete("employeeFieldTable #orgGroupNewInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=ORG_GROUP"
                , 20, "employeeFieldTable #hdnNewValueID_orgGroup");

            objEMSCommonJS.BindAutoComplete("employeeFieldTable #orgGroupOldInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=ORG_GROUP"
                , 20, "employeeFieldTable #hdnOldValueID_orgGroup");

            /* EMPLOYMENT STATUS */
            objEMSCommonJS.BindAutoComplete("employeeFieldTable #employmentStatusInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=EMPLOYMENT_STATUS"
                , 20, "employeeFieldTable #hdnNewValueID_empStatus");

            objEMSCommonJS.BindAutoComplete("employeeFieldTable #employmentStatusOldInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=EMPLOYMENT_STATUS"
                , 20, "employeeFieldTable #hdnOldValueID_empStatus");

            /* COMPANY */
            objEMSCommonJS.BindAutoComplete("employeeFieldTable #companyInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=COMPANY"
                , 20, "employeeFieldTable #hdnNewValueID_company");

            objEMSCommonJS.BindAutoComplete("employeeFieldTable #companyOldInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=COMPANY"
                , 20, "employeeFieldTable #hdnOldValueID_company");

            /* CIVIL STATUS */
            objEMSCommonJS.BindAutoComplete("employeeFieldTable #civilStatusInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=CIVIL_STATUS"
                , 20, "employeeFieldTable #hdnNewValueID_civilStatus");

            objEMSCommonJS.BindAutoComplete("employeeFieldTable #civilStatusOldInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=CIVIL_STATUS"
                , 20, "employeeFieldTable #hdnOldValueID_civilStatus");

            /* EXEMPTION STATUS */
            objEMSCommonJS.BindAutoComplete("employeeFieldTable #exemptionStatusInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=EXEMPTION_STATUS"
                , 20, "employeeFieldTable #hdnNewValueID_exempStatus");

            objEMSCommonJS.BindAutoComplete("employeeFieldTable #exemptionStatusOldInput"
                , AutoCompleteByMovementTypeURL + "&MovementType=EXEMPTION_STATUS"
                , 20, "employeeFieldTable #hdnOldValueID_exempStatus");

            var GetAutoCompleteSuccessFunctionByOrgGroup = function (ID) {
                /* SECONDARY DESIGNATION - POSITION */
                objEMSCommonJS.BindAutoComplete("divMovementAddModal #txtNewValue2"
                    , AutoCompleteByMovementTypeURL + "&MovementType=POSITION&OrgGroup=" + ID
                    , 20, "employeeFieldTable #hdnNewValueID2");
            };

            /* SECONDARY DESIGNATION - ORG GROUP */
            objEMSCommonJS.BindAutoComplete("divMovementAddModal #orgGroupInput2"
                , AutoCompleteByMovementTypeURL + "&MovementType=ORG_GROUP"
                , 20, "employeeFieldTable #hdnNewValueID_orgGroup2", "", "", GetAutoCompleteSuccessFunctionByOrgGroup);

            objEMSCommonJS.BindAutoComplete("divMovementAddModal #orgGroupOldInput2"
                , AutoCompleteByMovementTypeURL + "&MovementType=ORG_GROUP"
                , 20, "employeeFieldTable #hdnOldValueID_orgGroup2");

            if (isEdit == false) {
                $("#orgGroupOldInput2").val("");
                $("#hdnOldValueID_orgGroup2").val("");

                $("#orgGroupOldInput").val($("#divEmployeeBodyModal #ddlOrgGroup option:selected").text());
                $("#hdnOldValueID_orgGroup").val($("#divEmployeeBodyModal #ddlOrgGroup option:selected").val());

                $("#positionOldInput").val($("#divEmployeeBodyModal #ddlPosition option:selected").text());
                $("#hdnOldValueID_position").val($("#divEmployeeBodyModal #ddlPosition option:selected").val());

                $("#civilStatusOldInput").val($("#divEmployeeBodyModal #ddlCivilStatusCode option:selected").text());
                $("#hdnOldValueID_civilStatus").val($("#divEmployeeBodyModal #ddlCivilStatusCode option:selected").text());

                $("#companyOldInput").val($("#divEmployeeBodyModal #ddlCompany option:selected").text());
                $("#hdnOldValueID_company").val($("#divEmployeeBodyModal #ddlCompany option:selected").val());

                $("#employmentStatusOldInput").val($("#divEmployeeBodyModal #ddlStatus option:selected").text());
                $("#hdnOldValueID_empStatus").val($("#divEmployeeBodyModal #ddlStatus option:selected").val());

                if ($("#ddlMovementType option:selected").val() == "AWOL_PRESENT") {
                    $("#employmentStatusInput").val("AWOL");
                    $("#hdnNewValueID_empStatus").val("AWOL");
                }
                if ($("#ddlMovementType option:selected").val() == "TERMINATED") {
                    $("#employmentStatusInput").val("TERMINATED");
                    $("#hdnNewValueID_empStatus").val("TERMINATED");
                }
                if ($("#ddlMovementType option:selected").val() == "RESIGNATION") {
                    $("#employmentStatusInput").val("RESIGNED");
                    $("#hdnNewValueID_empStatus").val("RESIGNED");
                }
                if ($("#ddlMovementType option:selected").val() == "NOTICE_OF_ACCEPT" || $("#ddlMovementType option:selected").val() == "EXTENSION_OF_SERVICE") {
                    $("#employmentStatusInput").val("OUTGOING");
                    $("#hdnNewValueID_empStatus").val("OUTGOING");
                }
                if ($("#ddlMovementType option:selected").val() == "BACKOUT") {
                    $("#employmentStatusInput").val("BACK OUT");
                    $("#hdnNewValueID_empStatus").val("BACKOUT");
                }

            }
            else if (isEdit == true) {

                var OldValue = $("#hdnfrom").val();
                var OldValueID = $("#hdnfromID").val();
                var NewValue = $("#hdnto").val();
                var NewValueID = $("#hdntoID").val();

                //THIS IS FOR SECONDARY ASSIGNMENT
                //START
                var OrgGroup = $("#hdnto").val().split(',')[0];
                var Position = $("#hdnto").val().split(',')[1];

                $("#orgGroupOldInput2").val(OldValue);
                $("#hdnOldValueID_orgGroup2").val(OldValueID);

                $("#orgGroupInput2").val($("#hdnOrgGroupSecondaryOrgGroupDescription").val());
                $("#hdnNewValueID_orgGroup2").val(OrgGroup);

                $("#txtNewValue2").val($("#hdnOrgGroupSecondaryPositionDescription").val());
                $("#hdnNewValueID2").val(Position);
                //END


                //THIS IS FOR SECONDARY ASSIGNMENT
                //START
                $("#employmentStatusOldInput").val(OldValue);
                $("#hdnOldValueID_empStatus").val(OldValueID);

                $("#employmentStatusInput").val(NewValue);
                $("#hdnNewValueID_empStatus").val(NewValueID);
                //END

                //THIS IS FOR COMPANY TRANSFER
                //START
                $("#companyOldInput").val(OldValue);
                $("#hdnOldValueID_company").val(OldValueID);

                $("#companyInput").val(NewValue);
                $("#hdnNewValueID_company").val(NewValueID);
                //END

                //THIS IS FOR POSITION
                //START
                $("#positionOldInput").val(OldValue);
                $("#hdnOldValueID_position").val(OldValueID);

                $("#positionInput").val(NewValue);
                $("#hdnNewValueID_position").val(NewValueID);
                //END

                //THIS IS FOR BRANCH
                //START
                $("#orgGroupOldInput").val(OldValue);
                $("#hdnOldValueID_orgGroup").val(OldValueID);

                $("#orgGroupNewInput").val(NewValue);
                $("#hdnNewValueID_orgGroup").val(NewValueID);
                //END
            }
        },

        DeleteFormData: function () {
            //var input = $("#tblEmployeeFieldList").jqGrid("getGridParam", "selarrrow");
            var input = $("#tblEmployeeFieldList input[type='checkbox']:checked");
            var formData = new FormData($('#frmMovement').get(0));
            var ctr1 = 0;

            $(input).each(function (index, item) {
                var val = $(item).prop("id").replace("jqg_tblEmployeeFieldList_", "");
                formData.append("BulkRemove.IDs[" + index + "]", val);
            });

            //for (var x = 0; x < input.length; x++) {
            //    var val = input[x].prop("id").replace("jqg_tblEmployeeFieldList_", "");
            //    formData.append("BulkRemove.IDs[" + ctr1 + "]", val);
            //    ctr1++;
            //}

            return formData;
        },

        DeleteSuccessFunction: function () {
            //objEmployeeMovementAddListJS.LoadJQGrid(false, "", "");
            $("#divMovementAddModal").modal("hide");
            $("#divEmployeeList #btnSearch").click();
            $("#btnReset").click();

        },

        AddSuccessFunction: function () {
            if (($("#hdnIsEdit").val() || "").toLowerCase() == "true") {
                $("#tabMovement #btnSearch").click();
                $("#divEmployeeList #btnSearch").click();
                $("#divMovementAddModal").modal("hide");
            }
            else {
                $("#tabMovement #btnSearch").click();
                $("#divEmployeeList #btnSearch").click();
                // View mode
                var successFunction = function () {
                    $(".tablinks").find("span:contains('Movement')").parent("button").click();
                };
                if ($("#frmEmployee #btnSave").css("display") == "none") {
                    LoadPartialSuccessFunction(EmployeeViewURL + "?ID=" + $("#frmEmployee #hdnID").val(), "divEmployeeBodyModal", successFunction);
                }
                else {
                    LoadPartialSuccessFunction(EmployeeViewURL + "?ID=" + $("#frmEmployee #hdnID").val(), "divEmployeeBodyModal");
                }

                $("#divMovementAddModal").modal("hide");
            }
        },

        GetUpdateFormData: function () {
            var formData = new FormData($('#frmMovement').get(0));
            return formData;
        },

        GetFormData: function () {

            if ($("td#txtEmployeeField").html() == "SECONDARY_DESIG") {

                $("#orgGroupInput2").val(
                    $("#hdnNewValueID_orgGroup2").val() + "," +
                    $("#hdnNewValueID2").val() + "," +
                    $("input[name='add_remove']:checked").val()
                );

            }

            var formData = new FormData($('#frmMovement').get(0));
            
            $("#tblEmployeeFieldList td#txtEmployeeField").each(function (index) {
                if ($(this).html() != "") {
                    formData.append("EmployeeMovement.EmployeeFieldList[" + index + "].EmployeeField", $(this).text());
                }
            });

            $("#tblEmployeeFieldList td#txtOldValueTbl").each(function (index) {
                if ($(this).html() != "") {
                    formData.append("EmployeeMovement.EmployeeFieldList[" + index + "].OldValue", $(this).text());
                }
            });

            $("#tblEmployeeFieldList td input.oldValueInput").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeMovement.EmployeeFieldList[" + index + "].OldValue", $(this).val());
                }
            });

            $("#tblEmployeeFieldList td input.oldValueInputID").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeMovement.EmployeeFieldList[" + index + "].OldValueID", $(this).val());
                }
            });

            $("#tblEmployeeFieldList td input.newValueInput").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeMovement.EmployeeFieldList[" + index + "].NewValue", $(this).val());
                }
            });

            $("#tblEmployeeFieldList td input.newValueInputID").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeMovement.EmployeeFieldList[" + index + "].NewValueID", $(this).val());
                }
            });
            
            return formData;

        },

    };

    objEmployeeMovementAddListJS.Initialize();
});