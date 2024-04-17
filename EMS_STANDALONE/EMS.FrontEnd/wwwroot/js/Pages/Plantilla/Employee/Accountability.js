var objAccountabilityJS;
var GetPrintCOEURL = "/Plantilla/Employee/AddAccountability?handler=printcoe";
var GetPrintClearanceURL = "/Plantilla/Employee?handler=Accountability";

var AccountabilityDeleteURL = "/Plantilla/Employee?handler=DeleteAccountability";
var AccountabilityChangeStatus = "/Plantilla/Employee?handler=AccountabilityChangeStatus";

const GetQuestionByIDURL = "/Plantilla/Employee?handler=QuestionByCategory&Category=EXITINTERVIEW";
 
$(document).ready(function () {
    objAccountabilityJS = {

        Initialize: function () {
            $("#divAccountabilityAddPreloadedBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divChangeStatusModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            GenerateDropdownValues(AccountabilityDropDownURL, "ddlPreloadedAccountability", "Value", "Text", "", "", false);
            $("#tblPreloadedAccountabilityList").jqGrid("GridUnload");
            $("#tblPreloadedAccountabilityList").jqGrid("GridDestroy");

            //$("#btnBulkChangeStatus").hide();
            //$("#btnDeleteAccountability").hide();

            s.ElementBinding();
        },

        SuccessFunction: function () {
            objAccountabilityJS.LoadAccountabilityJQGrid({
                EmployeeID: $("#hdnID").val()
            });
        },

        AccountabilityChangeStatusSuccessFunction: function () {
            $("#divChangeStatusModal").modal("hide");
            $("#txtChangeStatusRemarks").val("");
            objAccountabilityJS.LoadAccountabilityJQGrid({
                EmployeeID: $("#hdnID").val()
            });
        },

        ElementBinding: function () {
            var s = this;

            $("#btnAddPreAccountability").unbind("click");
            $("#btnAddPreAccountability").click(function () {
                $("#divAccountabilityAddPreloadedModal").modal("show");
            });

            $("#btnSavePreAccountability").unbind("click");
            $("#btnSavePreAccountability").click(function () {
                $(".errMessage").removeClass("errMessage");
                $("#divAccountabilityAddPreloadedErrorMessage").html("");
                if ($("#ddlPreloadedAccountability").val() != "") {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                        "objEMSCommonJS.PostAjax(true \
                        , AddPreLoadedAccountabilityURL + "
                        + ("'&EmployeeID=" + $("#divEmployeeModal #hdnID").val()
                            + "&AccountabilityPreloadedID=" + $("#ddlPreloadedAccountability").val()
                            + "&OrgGroupID=" + $("#ddlOrgGroup :selected").val()
                            + "&PositionID=" + $("#ddlPosition :selected").val() + "'") + " \
                        ,  { } \
                        , '#divAccountabilityAddPreloadedErrorMessage' \
                        , '#divAccountabilityAddPreloadedModal #btnAddPreAccountability' \
                        , objAccountabilityJS.AddPreAccountabilitySuccessFunction); ",
                        "function");
                }
                else {
                    $("#ddlPreloadedAccountability").addClass("errMessage");
                    $("#ddlPreloadedAccountability").focus();
                    $("#divAccountabilityAddPreloadedErrorMessage").append("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                }
            });

            $("#ddlPreloadedAccountability").unbind("change");
            $("#ddlPreloadedAccountability").change(function () {
                objAccountabilityJS.LoadPreloadedAccountabilityJQGrid({
                    AccountabilityID: $(this).val()
                });
            });

            $("#btnAddAccountability").unbind("click");
            $("#btnAddAccountability").click(function () {
                LoadPartial(AddAccountabilityURL, "divAccountabilityBodyModal");
                $("#divAccountabilityModal").modal("show");
            });

            $("#btnPrintCOE").unbind("click");
            $("#btnPrintCOE").click(function () {
                var GetSuccessFunction = function (data) {
                    if (data != null) {
                        if (data.Result != null) {
                            $("#lblCOEContent").text(data.Result.Content);
                            $("#lblCOEHREmployeeName").text(data.Result.HREmployeeName);
                            $("#lblCOEHRPosition").text(data.Result.HRPosition);

                            objEMSCommonJS.ShowPrintModal("divPrintCOE");

                            //$("#divPrintCOE").show();
                            //var divToPrint = document.getElementById("divPrintCOE");
                            //newWin = window.open("");
                            //newWin.document.write(divToPrint.outerHTML);
                            //$("#divPrintCOE").hide();
                            //newWin.print();
                            //newWin.close();
                        }
                    }
                };
                objEMSCommonJS.GetAjax(GetPrintCOEURL
                    + "&EmployeeID=" + $("#hdnID").val(), {}, "", GetSuccessFunction);


            });
            $("#btnPrintClearance").unbind("click");
            $("#btnPrintClearance").click(function () {
                var GetSuccessFunction = function (data) {
                    var employee = $("#lblEmployee").text();
                    $("#lblEmployeeNamePrint").text(employee);
                    $("#lblEmployeeNameSignaturePrint").text(employee);

                    var companyName = $("#lblCompany").text();
                    $("#lblCompanyPrint").text(companyName);

                    var address = $("#txtAddressLine1").val();
                    $("#lblAddressPrint").text(address);


                    var Month = ["January", "February", "March", "April", "May", "June", "July", "Augost", "September", "October", "November", "December"];
                    //var dPrint = $("#dpDateHired").val();
                    //var datePrint = new Date(dPrint);
                    //var formatted_date = Month[datePrint.getMonth()] + " " + datePrint.getDate() + ", " + datePrint.getFullYear()

                    var dateHiredPrint = new Date($("#dpDateHired").val());
                    var formatted_dateHired = Month[dateHiredPrint.getMonth()] + " " + dateHiredPrint.getDate() + ", " + dateHiredPrint.getFullYear()

                    //$("#txtDateHiredPrint").text(formatted_date);

                    if (data.Resignation != null) {
                        var dPrint = data.Resignation[0]["StatusUpdatedDate"]
                        var datePrint = new Date(dPrint);
                        var formatted_date = Month[datePrint.getMonth()] + " " + datePrint.getDate() + ", " + datePrint.getFullYear();
                        $("#txtDateHiredPrint").text(formatted_date);
                        $("#lblDateResigned").text(formatted_date);
                    }

                    // FILL UP FORM
                    $("#lblCompanyName").text(companyName);
                    $("#lblDepartmentBranch").text($("#ddlOrgGroup :selected").text());
                    $("#lblName").text(employee);
                    $("#lblEmployeeID").text($("#txtCode").val());
                    $("#lblPosition").text($("#ddlPosition :selected").text());
                    $("#lblImmediateSuperior").text(data.Supervisor);
                    $("#lblDateHired").text(formatted_dateHired);
                    $("#lblBranchCo").text("");
                    $("#lblDateOfTransfer").text("");
                    $("#lblActivePhone").text($("#txtCellphoneNumber").val());
                    $("#lblEmailAddress").text($("#txtEmail").val());
                    $("#lblPermanentAddress").text(address);

                    //$("#divPrintClearance").show(); // For Testing

                    if (data != null) {
                        if (data.rows != null) {
                            var Col1Width = "width: 5%";
                            var Col2Width = "width: 25%";
                            var Col3Width = "width: 18%";
                            var Col4Width = "width: 15%";
                            var Col5Width = "width: 15%";
                            var Col6Width = "width: 10%";
                            var tableHeader = "font-family: Segoe UI;font-size: 10px;width:21%;text-align: center; border: 1px solid black; vertical-align: middle; padding: 0; margin: 0; font-weight: bold";
                            $("#divForClearanceItems").html(
                                "<tr> \
	                                <td style='" + tableHeader + ";" + Col1Width + "'></td> \
                                    <td style='" + tableHeader + ";" + Col2Width + "'>ITEMS</td> \
	                                <td style='" + tableHeader + ";" + Col3Width + "'>CLEARED BY</td> \
	                                <td style='" + tableHeader + ";" + Col4Width + "'>CLEARED DATE</td> \
	                                <td style='" + tableHeader + ";" + Col5Width + "'>SIGNATURE</td> \
	                                <td style='" + tableHeader + ";" + Col6Width + "'>REMARKS</td> \
                                </tr>");

                            var Except = "CANCELLED";
                            var ItemID = 0;
                            $(data.rows).each(function (index, item) {

                                // IF CLEARANCE WANT TO DIPLAY ONLY CLEARED ACCOUNTABILITY
                                if (!Except.includes(item.Status))
                                {
                                    ItemID += 1;
                                    var style = "border: 1px solid black; font-family: Segoe UI; font-size: 10px;";
                                    var styleIndex = "border: 1px solid black; font-family: Segoe UI; font-size: 10px; text-align: center";
                                    $("#divForClearanceItems").append(
                                        "<tr> \
	                                <td style='"+ styleIndex + "'>" + (ItemID) + "</td> \
	                                <td style='"+ style + "'>" + (item.Title) + "</td> \
	                                <td style='"+ style + "'>" + (item.Status == "CLEARED" ? (item.StatusUpdatedByNameNoCode) : "") + "</td> \
	                                <td style='"+ style + " text-align: center;'>" + (item.Status == "CLEARED" ? (item.StatusUpdatedDate ?? "") : "") + "</td> \
                                    <td style='"+ style + " text-align: center;'>" + (item.Status == "CLEARED" ? ("CLEARED ONLINE") : "") + "</td> \
                                    <td style='"+ style + "'>" + (item.Status == "CLEARED" ? (item.StatusRemarks || "") : "") + "</td> \
                                </tr>");
                                }

                            });

                        }
                        objEMSCommonJS.ShowPrintModal("divPrintClearance");
                    }
                };
                objEMSCommonJS.GetAjax(GetPrintClearanceURL
                    + "&EmployeeID=" + $("#hdnID").val(), {}, "", GetSuccessFunction);
            });

            $("#btnUploadAccountability").click(function () {
                objEMSCommonJS.UploadModal(UploadAccountabilityURL + $("#divEmployeeModal #hdnID").val(), "Upload", DownloadAccountabilityFormURL)
                $('#divModalErrorMessage').html('');
            });

            $("#btnDeleteAccountability").click(function () {
                $('#divAccountabilityErrorMessage').html('');
                var selRow = $("#tblAccountabilityList").jqGrid("getGridParam", "selarrrow");
                if (selRow.length > 0) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE + SPAN_DELETE_START + " ID : " + selRow + SPAN_END,
                        "objEMSCommonJS.PostAjax(true \
                        , '" + AccountabilityDeleteURL + "&ID=" + selRow + "' \
                        , {} \
                        , '#divAccountabilityErrorMessage' \
                        , '#btnDeleteAccountability' \
                        , objAccountabilityJS.SuccessFunction);",
                        "function");
                }
                else {
                    $("#divAccountabilityErrorMessage").append("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Accountability</li></label><br />");
                }
            });

            $("#btnBulkChangeStatus").click(function () {
                $('#divAccountabilityErrorMessage').html('');
                var selRow = $("#tblAccountabilityList").jqGrid("getGridParam", "selarrrow");
                var firstValue = "";
                var isValid = true;
                if (selRow.length > 0) {
                    $(selRow).each(function (index, item) {
                        if (firstValue == "")
                            firstValue = $("#tblAccountabilityList").getRowData(item).Status;
                        else if (firstValue != $("#tblAccountabilityList").getRowData(item).Status)
                            isValid = false;
                    });
                    if (isValid) {
                        //firstValue = $(firstValue).text();

                        $(".editRequired").addClass("reqField");
                        $("#divChangeStatusModal").modal("show");
                        $('#divAccountabilityChangeStatusErrorMessage').html('');
                        $("#txtChangeStatusRemarks").val("");
                        $("#AccountabilityChangeStatusID").val(selRow);
                        GenerateDropdownValues(AccountabilityChangeStatus +"&CurrentStatus="+ firstValue, "ddlAccountabilityChangeStatus", "Value", "Text", "", "", false);
                    }
                    else
                        $("#divAccountabilityErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_DIFF_STATUS + " </li></label><br />");
                }
                else {
                    $("#divAccountabilityErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Accountability</li></label><br />");
                }
            });

            $("#ddlAccountabilityChangeStatus").change(function () {
                if ($("#ddlAccountabilityChangeStatus").val() == "NOT_CLEARED" || $("#ddlAccountabilityChangeStatus").val() == "CANCELLED") {
                    $("#remarksRequired").addClass("reqField");
                    $("#remarksRequired").removeClass("unreqField");
                }
                else {
                    $("#remarksRequired").addClass("unreqField");
                    $("#remarksRequired").removeClass("reqField");
                }

            });

            $("#btnSaveAccountabilityChangeStatus").click(function () {
                $('#divAccountabilityChangeStatusErrorMessage').html('');
                if ($("#ddlAccountabilityChangeStatus").val() != "") {
                    if (($("#ddlAccountabilityChangeStatus").val() == "NOT_CLEARED" || $("#ddlAccountabilityChangeStatus").val() == "CANCELLED") && $("#txtChangeStatusRemarks").val().trim() == "") {
                        $("#divAccountabilityChangeStatusErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_INPUT_REMARKS + "</li></label><br />");
                        $("#txtChangeStatusRemarks").addClass("required-field errMessage");
                        return;
                    }
                    $("#txtChangeStatusRemarks").removeClass("required-field errMessage");
                    var ID = $("#AccountabilityChangeStatusID").val();
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_UPDATE + SPAN_DELETE_START + " ID : " + ID + SPAN_END,
                        "objEMSCommonJS.PostAjax(true \
                        , '" + AccountabilityChangeStatus + "&ID=" + ID + "&NewStatus=" + $("#ddlAccountabilityChangeStatus").val() + "&Remarks=" + $("#txtChangeStatusRemarks").val() + "' \
                        , {} \
                        , '#divAccountabilityChangeStatusErrorMessage' \
                        , '#btnSaveAccountabilityChangeStatus' \
                        , objAccountabilityJS.AccountabilityChangeStatusSuccessFunction);",
                        "function");
                }
                else
                    $("#divAccountabilityChangeStatusErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + "</li></label><br />");
            });
        },

        LoadAccountabilityJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblAccountabilityList").jqGrid("GridUnload");
            $("#tblAccountabilityList").jqGrid("GridDestroy");
            $("#tblAccountabilityList").jqGrid({
                url: GetAccountabilityURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Clearing Department", "Title", "Status","Status Update Date", "Status Updated By", "Status Remarks", "Last Comment", "Last Comment Date", ""],
                colModel: [
                    { name: "", hidden: true },
                    {
                        key: true, name: "ID", index: "ID", align: "center", sortable: false, formatter: objAccountabilityJS.ViewLink, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="border-left:10px solid ' + rowObject.StatusColor + ' !important;"';
                        } },
                    { name: "OrgGroupDescription", index: "OrgGroupDescription", align: "left", sortable: false },
                    { name: "Title", index: "Title", align: "left", sortable: false },
                    {
                        name: "StatusDescription", index: "StatusDescription", align: "left", sortable: false, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="background-color:' + rowObject.StatusColor + '"';
                        }
                    },
                    { name: "StatusUpdatedDate", index: "StatusUpdatedDate", align: "left", sortable: false },
                    { name: "StatusUpdatedByName", index: "StatusUpdatedByName", align: "left", sortable: false },
                    { name: "StatusRemarks", index: "StatusRemarks", align: "left", sortable: false },
                    { name: "LastComment", index: "StatusCode", align: "left", sortable: false },
                    { name: "LastCommentDate", index: "LastUpdateDate", align: "left" },
                    { hidden: true, name: "Status", index: "Status", align: "left", sortable: false },
                ],
                rowList: SetRowList(),
                loadonce: false,
                toppager: true,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: true,
                rowNumbers: true,
                width: "100%",
                height: "300",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblAccountabilityList tr:nth-child(" + (iRow + 1) + ") .activity-id-link").click();
                },
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
                                if (data.rows[i].StatusCode == "CLEARED" || data.rows[i].StatusCode == "CANCELLED") {
                                    $("#jqg_tblMyAccountabilitiesList_" + data.rows[i].ID).prop("disabled", true);
                                }
                            }
                        }

                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblAccountabilityList", data);
                        //$("#tblAccountabilityList_subgrid").width(20);
                        //$(".jqgfirstrow td:first").width(20);
                        if (data.FinalStatus.length > 0)
                            $("#tblAccountabilityList_toppager_left").html("<div style='display: inline-flex'><div class='ui-paging-info'>Clearance Percentage: " + data.FinalStatus[0]["OverDone"] + " / " + data.FinalStatus[0]["OverAll"] + " (" + data.FinalStatus[0]["Status"] + ")</div></div>");
                    }

                    $("#tblAccountabilityList_toppager_center").hide();
                    $("#tblAccountabilityList_toppager_custom_block_right .ui-pg-selbox").hide();
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeSelectRow: function (id, e) {
                    var cbsdis = $("tr#" + id + ".jqgrow > td > input.cbox:disabled", $(this));

                    if (cbsdis.length == 0) {
                        return true;    // allow select the row
                    } else {
                        return false;   // not allow select the row
                    }
                },
                beforeRequest: function () {
                    GetJQGridState("tblAccountabilityList");
                },
            });

        },

        LoadPreloadedAccountabilityJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblPreloadedAccountabilityList").jqGrid("GridUnload");
            $("#tblPreloadedAccountabilityList").jqGrid("GridDestroy");
            $("#tblPreloadedAccountabilityList").jqGrid({
                url: AccountabilityDetailsByIDURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Type", "Title", "Description", "Org Group"],
                colModel: [
                    { name: "", hidden: true },
                    { name: "Type", index: "Type", align: "center", sortable: false },
                    { name: "Title", index: "Title", align: "left", sortable: false },
                    { hidden: true, name: "Description", index: "Description", align: "center", sortable: false },
                    { name: "OrgGroupDescription", index: "OrgGroupDescription", align: "left", sortable: false },
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
                        AutoSizeColumnJQGrid("tblPreloadedAccountabilityList", data);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return objAccountabilityJS.ViewUpdateStatusModal('" + UpdateAccountabilityURL + "?EmployeeAccountabilityID=" + rowObject.ID + "');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        AddPreAccountabilitySuccessFunction: function () {
            $("#divAccountabilityAddPreloadedModal").modal("hide");
            objAccountabilityJS.LoadAccountabilityJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
        },

        ViewUpdateStatusModal: function (url) {
            $('#divAccountabilityModal').modal('show');
            LoadPartial(url, 'divAccountabilityBodyModal');
            return false;
        },

        LoadQuestion: function (EmployeeID) {
            var GetSuccessFunction = function (data) {
                var questionsContainer = $('#divDisplayQuestion').html("");;

                $.each(data.Result, function (index, question) {
                    var defaultCol = 12;
                    var questionCol = defaultCol - question.Tab;
                    var addQuestionType = "";

                    if (question.ParentQuestionID != 0)
                        addQuestionType = '<input type="' + question.QuestionType + '" id="' + question.QuestionID + '" name="' + question.ParentQuestionID + '" disabled >';

                    questionsContainer.append(
                        '<div class="form-group form-fields">' +
                        '<div class="col-md-' + question.Tab + '">' +
                        '</div>' +
                        '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                        addQuestionType + '<label class="control-label">' + question.Question + ' </label>' +
                        '</div>' +
                        '</div>'
                    );

                    if (question.EmployeeAnswerID != null)
                        $("#" + question.QuestionID + ",#" + question.ParentQuestionID).prop("checked", true);

                    if (question.AnswerID == null && question.AnswerType == "TEXT") {
                        if (question.EmployeeAnswerDetails != null) {
                            $.each(question.EmployeeAnswerDetails.split('|'), function (index, item) {
                                questionsContainer.append(
                                    '<div class="form-group form-fields">' +
                                    '<div class="col-md-' + question.Tab + '">' +
                                    '</div>' +
                                    '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                                    '<input class="form-control" type="text" value="' + item + '" disabled>' +
                                    '</div>' +
                                    '</div>'
                                );
                            });
                        }
                    }
                    if (question.AnswerID == null && question.AnswerType == "MULTIPLE") {
                        if (question.EmployeeAnswerDetails != null) {
                            $.each(question.EmployeeAnswerDetails.split('&&'), function (index, item) {
                                $.each(item.split('|'), function (index, item) {
                                    questionsContainer.append(
                                        '<div class="col-md-2 form-group form-fields">' +
                                        '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                                        '<input class="form-control" type="text" value="' + item + '" disabled>' +
                                        '</div>' +
                                        '</div>'
                                    );
                                });
                            });
                        }
                    }

                    if (question.Answer != null) {
                        var answerCol = question.Tab + 1;
                        questionCol = defaultCol - answerCol;

                        $.each(question.Answer.split('|'), function (index, answer) {
                            var IsChecked = "";
                            var EmployeeAnswerIndex = 99;

                            var AnswerIDArr = question.AnswerID.split('|');
                            if (AnswerIDArr[index] == question.EmployeeAnswerID) {
                                IsChecked = "checked";
                                EmployeeAnswerIndex = index;
                            }

                            questionsContainer.append(
                                '<div class="form-group form-fields">' +
                                '<div class="col-md-' + answerCol + '">' +
                                '</div>' +
                                '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                                '<input type="' + question.AnswerType + '" name="' + question.Code + '" disabled ' + IsChecked + '><label class="control-label">' + answer + ' </label>' +
                                '</div>' +
                                '</div>'
                            );

                            var AddReasonArr = question.AddReason.split('|');
                            var Answer = "";
                            if (AddReasonArr[index] == "INPUT") {
                                if (index == EmployeeAnswerIndex) {
                                    Answer = (question.EmployeeAnswerDetails == null ? "" : question.EmployeeAnswerDetails);
                                }
                                questionsContainer.append(
                                    '<div class="form-group form-fields">' +
                                    '<div class="col-md-' + answerCol + '">' +
                                    '</div>' +
                                    '<div class="col-md-' + questionCol + '" style="display:inline-flex;">' +
                                    '<input class="form-control" type="text" value="' + Answer + '" disabled>' +
                                    '</div>' +
                                    '</div>'
                                );
                            }
                        });
                    }

                });
            };
            objEMSCommonJS.GetAjax(GetQuestionByIDURL + "&EmployeeID=" + EmployeeID, {}, "", GetSuccessFunction, null, true);
        }

    };

    objAccountabilityJS.Initialize();
});