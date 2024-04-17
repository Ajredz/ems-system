var objApplicantViewJS;

var AttachmentTypeDropDownOptions = [];
$(document).ready(function () {
    objApplicantViewJS = {

        ID: $("#divApplicantModal #hdnID").val(),

        Initialize: function () {
            $("#divApplicantBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divApplicantBodyModal .reqField").addClass("unreqField");
            $("#divApplicantBodyModal .reqField").removeClass("reqField");
            $("#btnAddAttachmentFields, .deleteAttachmentButtons").remove();
            $("#btnAddApplicationSource, #btnAddCourse, #lnkType, #lnkViewMRF").remove();
            $("#divApplicantBodyModal .required-field").removeClass("required-field");
            $("#divApplicantBodyModal .unreqField").remove();
            $("#divApplicantModal .form-control:not('#ddlMRF')").prop("disabled", true);
            $(".AttachmentDynamicFields .form-control").attr("readonly", true);
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabPersonalInformation', "#divApplicantModal");

            // Reset Tab First Load tags
            personalInfoTabFirstLoad = false;
            attachmentTabFirstLoad = false;
            appHistoryTabFirstLoad = false;
            logActivityTabFirstLoad = false;
            legalProfileTabFirstLoad = false;
            
            //objLogActivityJS.LoadLogActivityJQGrid({
            //    ApplicantID: $("#divApplicantModal #hdnID").val()
            //});

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

            //GenerateDropdownValues(MRFIDDropdownURL + "&ApplicantID=" + $("#hdnID").val(), "ddlMRF", "Value", "Text", "", "", false);

            //$("#tblLogActivityList").jqGrid('hideCol', ["ID"]);
            //document.getElementById("txtCellphoneNumber").value = $("#txtCellphoneNumber").val().substr(3);

            $("#txtCellphoneNumber").val() == (null || "") ? "" : $("#txtCellphoneNumber").val($("#txtCellphoneNumber").val().slice(0, 4) + "-" + $("#txtCellphoneNumber").val().substr(4));


        },

        ElementBinding: function () {
            var s = this;
            //$("#divApplicantModal #txtExpectedSalary").val(AddZeroes($("#divApplicantModal #txtExpectedSalary").val()).withComma());
            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , ApplicantDeleteURL + '?ID=' + objApplicantViewJS.ID\
                    , {} \
                    , '#divApplicantErrorMessage' \
                    , '#btnDelete' \
                    , objApplicantViewJS.DeleteSuccessFunction);",
                    "function");
            });

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

            $("#btnEdit").click(function () {
                LoadPartial(ApplicantEditURL + "?ID=" + objApplicantViewJS.ID, "divApplicantBodyModal");
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
                    //$("#tblLogActivityList").jqGrid('hideCol', ["ID"]);
                    var isSuccessFunction = function () {
                        objLogActivityJS.IsViewMode = true;
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

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divApplicantModal').modal('hide');
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
    
     objApplicantViewJS.Initialize();
});