var objEmployeeMovementListJS;
var EmployeeMovementListURL = "/Plantilla/Employee/Movement?handler=List";
var EmployeeMovementAddURL = "/Plantilla/Employee/MovementAdd";
var GetEmployeeMovementTypeURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=MOVEMENT_TYPE";
var GetEmployeeFieldURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=MOVEMENT_EMP_FIELD";
var GetPrintURL = "/Plantilla/Employee/Movement?handler=Print";
var CreatedByAutoCompleteURL = "/Plantilla/Employee/Movement?handler=CreatedByAutoComplete";
//var EmployeeMovementBulkDeleteListURL = "/Plantilla/Employee/Movement?handler=BulkDelete";
var GetStatusFilterURL = "/Plantilla/Employee/Movement?handler=StatusFilter";

//for movement checker, 9, 22, 66, 94-95, 170-244, 290, 299-319, 366-367, 369-370, 374-379, 385, 388, 435-437, 439, 445-449, 494-505, 507-511, 708-709, 731-733, 762-770
$(document).ready(function () {
    objEmployeeMovementListJS = {
        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                EmployeeID: $("#divEmployeeModal #hdnID").val(),
                EmployeeFieldDelimited: localStorage["EmployeeMovementListEmployeeFieldDelimited"],
                MovementTypeDelimited: localStorage["EmployeeMovementListMovementTypeDelimited"],
                StatusDelimited: localStorage["EmployeeMovementListStatusDelimited"],
                From: localStorage["EmployeeMovementListFrom"],
                To: localStorage["EmployeeMovementListTo"],
                DateEffectiveFromFrom: localStorage["EmployeeMovementListDateEffectiveFromFrom"],
                DateEffectiveFromTo: localStorage["EmployeeMovementListDateEffectiveFromTo"],
                DateEffectiveToFrom: localStorage["EmployeeMovementListDateEffectiveToFrom"],
                DateEffectiveToTo: localStorage["EmployeeMovementListDateEffectiveToTo"],
                Reason: localStorage["EmployeeMovementListReason"],
                CreatedDateFrom: localStorage["EmployeeMovementListCreatedDateFrom"],
                CreatedDateTo: localStorage["EmployeeMovementListCreatedDateTo"],
                CreatedByDelimited: localStorage["EmployeeMovementListCreatedByDelimited"],
                HRDComments: localStorage["EmployeeMovementListHRDComments"],
                IsShowActiveOnly: $("#cbShowActiveOnly").prop("checked")
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();


        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterDateEffectiveFromFrom, #txtFilterDateEffectiveFromTo, \
            #txtFilterDateEffectiveToFrom, #txtFilterDateEffectiveToTo, \
            #txtFilterCreatedDateFrom, #txtFilterCreatedDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            //$("#txtFilterDateEffectiveFromFrom, #txtFilterDateEffectiveFromTo, \
            //#txtFilterDateEffectiveToFrom, #txtFilterDateEffectiveToTo ").focusin(function () {
            //    $("#tabMovement .ui-jqgrid-bdiv").css({ "overflow": "visible" });

            //}).focusout(function () {

            //    $("#tabMovement .ui-jqgrid-bdiv").css({ "overflow": "auto" });
            //});

            $("#tabMovement #btnSearch").click(function () {
                var param = {
                    EmployeeID: $("#divEmployeeModal #hdnID").val(),
                    EmployeeFieldDelimited: objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedEmployeeField").value,
                    MovementTypeDelimited: objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedMovementType").value,
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedStatus").value,
                    From: $("#tabMovement #txtFilterFrom").val(),
                    To: $("#tabMovement #txtFilterTo").val(),
                    DateEffectiveFromFrom: $("#tabMovement #txtFilterDateEffectiveFromFrom").val(),
                    DateEffectiveFromTo: $("#tabMovement #txtFilterDateEffectiveFromTo").val(),
                    DateEffectiveToFrom: $("#tabMovement #txtFilterDateEffectiveToFrom").val(),
                    DateEffectiveToTo: $("#tabMovement #txtFilterDateEffectiveToTo").val(),
                    CreatedDateFrom: $("#tabMovement #txtFilterCreatedDateFrom").val(),
                    CreatedDateTo: $("#tabMovement #txtFilterCreatedDateTo").val(),
                    CreatedByDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedCreatedBy").value,
                    Reason: $("#tabMovement #txtFilterReason").val(),
                    HRDComments: $("#tabMovement #txtFilterHRDComments").val(),
                    IsShowActiveOnly: $("#tabMovement #cbShowActiveOnly").prop("checked")
                };
                s.SetLocalStorage();
                ResetJQGridState("tblEmployeeMovementList");
                s.LoadJQGrid(param);
            });

            $("#tabMovement #btnReset").click(function () {
                $("#tabMovement div.filterFields input[type='search']").val("");
                $("#tabMovement div.filterFields select").val("");
                $("#tabMovement div.filterFields input[type='checkbox']").prop("checked", true);
                $("#tabMovement #multiSelectedCreatedBy").html("");
                $("#tabMovement #multiSelectedEmployeeField").html("");
                $("#tabMovement #multiSelectedEmployeeFieldOption label, #tabMovement #multiSelectedEmployeeFieldOption input").prop("title", "add");
                $("#tabMovement #multiSelectedMovementType").html("");
                $("#tabMovement #multiSelectedMovementTypeOption label, #tabMovement #multiSelectedMovementTypeOption input").prop("title", "add");
                $("#tabMovement #multiSelectedStatus").html("");
                $("#tabMovement #multiSelectedStatusOption label, #tabMovement #multiSelectedStatusOption input").prop("title", "add");
                $("#tabMovement #btnSearch").click();
            });

            $("#tabMovement #btnAdd").click(function () {
                var SuccessFunction = function () {
                    $("#divMovementAddModal").modal("show");
                };
                LoadPartialSuccessFunction(EmployeeMovementAddURL + "?EmployeeID=" + $("#divEmployeeModal #hdnID").val(), "divMovementModalContainer", SuccessFunction);
            });

            $("#btnPrintMovement").click(function () {
                $("#divEmployeeMovementErrorMessage").hide();
                $("#divEmployeeMovementErrorMessage").html("");

                if ($("#tblEmployeeMovementList").jqGrid("getGridParam", "selarrrow").length == 0) {
                    $("#divEmployeeMovementErrorMessage").show();
                    $("#divEmployeeMovementErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "movement.</li></label><br />");
                }
                else {
                    var GetSuccessFunction = function (data) {

                        if (data != null) {
                            if (data.Result != null) {
                                $("#lblCompanyName").text(data.Result.CompanyName);
                                $("#lblIDNumber").text(data.Result.IDNumber);
                                $("#lblEmployeeName").text(data.Result.EmployeeName);
                                $("#lblDateHired").text(data.Result.DateHired);
                                $("#lblOrgGroup").text(data.Result.OrgGroup);
                                $("#lblPosition").text(data.Result.Position);
                                $("#lblReason").text(data.Result.Reason);
                                $("#lblHRDComments").text(data.Result.HRDComments);
                                $("#lblDateEffectiveFrom").text(data.Result.DateEffective);
                                $("#lblMovementType").text(data.Result.MovementType);

                                $("#idRegionalManager").html(data.Result.RegionalManager);
                                $("#idHrdManager").html(data.Result.HRDManager);
                                $("#idCompanyPresident").html(data.Result.CompanyPresident);

                                $("#printDetailsTable").html(" <tr> \
                                    <td style=\"width:30%\"></td> \
                                    <td style=\"width:33%\">From</td> \
                                    <td style=\"width:4%\"></td> \
                                    <td style=\"width:33%\">To</td> \
                                </tr>");
                                $(data.Result.PrintDetailsList).each(function (index, item) {
                                    if (item.DetailsLabel != "") {
                                        $("#printDetailsTable").append("<tr> \
                                        <td style=\"width:30%;font-family: Segoe UI;font-size: 13px;\" id=\"lblDetailsLabel_" + index + "\"></td> \
                                        <td style=\"width:33%;font-family: Segoe UI;font-size: 13px; border-bottom: 1px solid black; text-align: left\" id=\"lblFrom_" + index + "\"></td> \
                                        <td style=\"width:4%\"></td> \
                                        <td style=\"width:33%;font-family: Segoe UI;font-size: 13px; border-bottom: 1px solid black; text-align: left\" id=\"lblTo_" + index + "\"></td> \
                                    </tr>");
                                        $("#lblDetailsLabel_" + index).text(item.DetailsLabel);
                                        $("#lblFrom_" + index).text(item.From);
                                        $("#lblTo_" + index).text(item.To);
                                    }
                                });


                                $("#lblDateGenerated").text(data.Result.DateGenerated);

                                objEMSCommonJS.ShowPrintModal("divPrintMovement");
                            }
                        }
                    };

                    objEMSCommonJS.GetAjax(GetPrintURL
                        + "&IDDelimited=" + $("#tblEmployeeMovementList").jqGrid("getGridParam", "selarrrow").join()
                        , {}, "", GetSuccessFunction);


                }
            });

            $("#tabMovement #btnCancelDynamicChangeStatus").on("click", function () {
                $("#BulkChangeStatusModal").hide();
            });
            $("#tabMovement #ddlDynamicChangeStatus").on("change", function () {
                $("#divMovementAddBodyModal #divDynamicChangeStatusErrorMessage").html("");
                if ($("#tabMovement #ddlDynamicChangeStatus :selected").val() == "CANCELLED") {
                    $("#tabMovement #spnDynamicChangeStatus").addClass("reqField");
                    $("#tabMovement #spnDynamicChangeStatus").removeClass("unreqField");
                    $("#tabMovement #txtDynamicChangeStatusRemarks").addClass("required-field");
                }
                else {
                    $("#tabMovement #spnDynamicChangeStatus").addClass("unreqField");
                    $("#tabMovement #spnDynamicChangeStatus").removeClass("reqField");
                    $("#tabMovement #txtDynamicChangeStatusRemarks").removeClass("required-field");
                }
            });
            $("#tabMovement #btnMovementBulkChangeStatus").click(function () {
                $('#divEmployeeMovementErrorMessage').html('');
                var selRow = $("#tblEmployeeMovementList").jqGrid("getGridParam", "selarrrow");
                var firstValue = "";
                var isValid = true;
                if (selRow.length > 0) {
                    $(selRow).each(function (index, item) {
                        if (firstValue == "")
                            firstValue = $("#tblEmployeeMovementList").getRowData(item).Status;
                        else if (firstValue != $("#tblEmployeeMovementList").getRowData(item).Status)
                            isValid = false;
                    });
                    if (isValid) {
                        //firstValue = $(firstValue).text();

                        if ($("#BulkChangeStatusModal").is(":visible"))
                        {
                            $("#BulkChangeStatusModal").hide();
                            return;
                        }
                        else
                            $("#BulkChangeStatusModal").show();

                        $(".editRequired").addClass("reqField");
                        $('#divEmployeeMovementErrorMessage').html('');
                        $("#txtChangeStatusRemarks").val("");
                        $("#hdnDynamicChangeStatusID").val(selRow);
                        GenerateDropdownValues(MovementBulkChangeStatus + "&CurrentStatus=" + firstValue, "ddlDynamicChangeStatus", "Value", "Text", "", "", false);
                    }
                    else
                        $("#divEmployeeMovementErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_DIFF_STATUS + " </li></label><br />");
                }
                else {
                    $("#divEmployeeMovementErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Movement</li></label><br />");
                }
            });
            $("#tabMovement #btnSaveDynamicChangeStatus").on("click", function () {
                $("#tabMovement #divDynamicChangeStatusErrorMessage").html("");

                if ($("#tabMovement #ddlDynamicChangeStatus :selected").val() == "") {
                    $("#tabMovement #divDynamicChangeStatusErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + " Movement</li></label><br />");
                    return;
                }

                var isNoBlankFunction = function () {
                    return true;
                };
                if (objEMSCommonJS.ValidateBlankFields("#tabMovement #DynamicChangeStatusForm", "#tabMovement #divDynamicChangeStatusErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , MovementChangeStatusPostURL + "+ ("'&ID=" + $("#tabMovement #hdnDynamicChangeStatusID").val() + "'") + " \
                        , objEmployeeMovementListJS.ChangeStatusFormData() \
                        , '#tabMovement #divDynamicChangeStatusErrorMessage' \
                        , '#tabMovement #btnSaveDynamicChangeStatus' \
                        , objEmployeeMovementListJS.ChangeStatusSuccessFunction);",
                        "function");
                }
            });

            // Advised to remove Delete Function for Movement
            //$("#btnDeleteMovement").click(function () {
            //    $("#divEmployeeMovementErrorMessage").hide();
            //    $("#divEmployeeMovementErrorMessage").html("");

            //    if ($("#tblEmployeeMovementList").jqGrid("getGridParam", "selarrrow").length == 0) {
            //        $("#divEmployeeMovementErrorMessage").show();
            //        $("#divEmployeeMovementErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "movement.</li></label><br />");
            //    }
            //    else {
            //        var IDs = $("#tblEmployeeMovementList").jqGrid("getGridParam", "selarrrow");
            //        IDList = JSON.stringify({ 'ID': IDs });
            //        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE,
            //            "objEMSCommonJS.PostAjax(true \
            //            , EmployeeMovementBulkDeleteListURL \
            //            , objEmployeeMovementListJS.DeleteFormData() \
            //            , '#divEmployeeMovementErrorMessage' \
            //            , '#btnDeleteMovement' \
            //            ,objEmployeeMovementListJS.DeleteSuccessFunction); ",
            //            "function");


            //        //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE,
            //        //    "objEMSCommonJS.PostAjax(true \
            //        //        , EmployeeScoreBulkDeleteListURL \
            //        //        , objEmployeeScoreListJS.DeleteFormData() \
            //        //        , '#divEmployeeScoreErrorMessage' \
            //        //        , '#divEmployeeScoreModal #btnBulkDelete' \
            //        //        , objEmployeeScoreListJS.DeleteSuccessFunction);",
            //        //    "function");

            //        //ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
            //        //    "objEMSCommonJS.PostAjax(false \
            //        //, SystemRoleDeleteURL + '?ID=' + objSystemRoleEditJS.ID \
            //        //, {} \
            //        //, '#divSystemRoleErrorMessage' \
            //        //, '#btnDelete' \
            //        //, objSystemRoleEditJS.DeleteSuccessFunction);",
            //        //    "function");

            //    }
            //});

            objEMSCommonJS.BindFilterMultiSelectEnum("tabMovement #multiSelectedEmployeeField", GetEmployeeFieldURL);
            objEMSCommonJS.BindFilterMultiSelectEnum("tabMovement #multiSelectedMovementType", GetEmployeeMovementTypeURL);
            objEMSCommonJS.BindFilterMultiSelectEnum("tabMovement #multiSelectedStatus", GetStatusFilterURL);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("tabMovement #txtFilterCreatedBy"
                , CreatedByAutoCompleteURL, 20, "multiSelectedCreatedBy");

            $("#tabMovement #cbShowActiveOnly").click(function () {
                $("#tabMovement #btnSearch").click();
            });
        },
        ChangeStatusFormData: function () {
            var formData = new FormData($('#tabMovement #DynamicChangeStatusForm').get(0));

            formData.append("ChangeStatus.Status", $("#tabMovement #ddlDynamicChangeStatus :selected").val());
            formData.append("ChangeStatus.Remarks", $("#tabMovement #txtDynamicChangeStatusRemarks").val());
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
        },

        DeleteFormData: function () {
            var input = $("#tblEmployeeMovementList input[type='checkbox']:checked");
            var formData = new FormData();
            var ctr1 = 0;

            $(input).each(function (index, item) {
                var val = $(item).prop("id").replace("jqg_tblEmployeeMovementList_", "");
                formData.append("EmployeeMovement.IDs[" + index + "]", val);
            });

            return formData;
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblEmployeeMovementList") == "" ? "" : $.parseJSON(localStorage.getItem("tblEmployeeMovementList"));
            var moveFilterFields = function () {
                var intialHeight = $("#tabMovement .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#tabMovement .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#tabMovement #filterFieldsContainer");
                });
                $("#tabMovement .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#tabMovement div.filterFields").unbind("keyup");
                $("#tabMovement div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#tabMovement #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblEmployeeMovementList").jqGrid("GridUnload");
            $("#tblEmployeeMovementList").jqGrid("GridDestroy");
            $("#tblEmployeeMovementList").jqGrid({
                url: EmployeeMovementListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "", "Employee Field", "Movement Type", "", "Status", "From", "To", "Date Effective From", "Date Effective To",
                    "Modified Date", "Modified By"],
                colModel: [
                    { hidden: true, align:"center" },
                    { width: 55, key: true, name: "ID", index: "ID", align: "center", formatter: objEmployeeMovementListJS.EditLink },
                    { width: 45, name: "ID", index: "ID", align: "center", sortable: true, formatter: objEmployeeMovementListJS.AddPrintLink },
                    { name: "EmployeeField", index: "EmployeeField", editable: true, align: "left", sortable: true },
                    { name: "MovementType", index: "MovementType", editable: true, align: "left", sortable: true },
                    { hidden: true, name: "Status", index: "Status", editable: true, align: "left", sortable: true },
                    {
                        name: "StatusDescription", index: "StatusDescription", editable: true, align: "left", sortable: true, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="background-color:' + rowObject.StatusColor + '"';
                        }
                    },
                    { name: "From", index: "From", editable: true, align: "left", sortable: true },
                    { name: "To", index: "To", editable: true, align: "left", sortable: true },
                    { name: "DateEffectiveFrom", index: "DateEffectiveFrom", editable: true, align: "left", sortable: true },
                    { name: "DateEffectiveTo", index: "DateEffectiveTo", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                    { name: "CreatedByName", index: "CreatedByName", editable: true, align: "left", sortable: true }
                ],
                toppager: $("#divEmployeeMovementPager"),
                pager: $("#divEmployeeMovementPager"),
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
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblEmployeeMovementList tr:nth-child(" + (iRow + 1) + ") .movement-id-link").click();
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
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblEmployeeMovementList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#tabMovement #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#tabMovement .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                    }

                    if (localStorage["EmployeeMovementListFilterOption"] != undefined) {
                        $("#tabMovement #chkMovementFilter").prop('checked', JSON.parse(localStorage["EmployeeMovementListFilterOption"]));
                    }
                    if (localStorage["EmployeeMovementListShowActiveOnly"] != undefined) {
                        $("#tabMovement #cbShowActiveOnly").prop('checked', JSON.parse(localStorage["EmployeeMovementListShowActiveOnly"]));
                    }
                    objEmployeeMovementListJS.ShowHideFilter();
                    objEmployeeMovementListJS.ShowHideActiveOnly();

                    $("#tabMovement #chkMovementFilter").on('change', function () {
                        objEmployeeMovementListJS.ShowHideFilter();
                        localStorage["EmployeeMovementListFilterOption"] = $("#chkMovementFilter").is(":checked");
                    });
                    $("#tabMovement #cbShowActiveOnly").on('change', function () {
                        objEmployeeMovementListJS.ShowHideActiveOnly();
                        localStorage["EmployeeMovementListShowActiveOnly"] = $("#cbShowActiveOnly").is(":checked");
                        $("#tabMovement #btnSearch").click();
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#tabMovement .ui-jqgrid-bdiv").css({ "min-height": "400px"/*, "overflow": "visible" */ });

                    $("#tabMovement table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#tabMovement table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#tabMovement table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#tabMovement table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#tabMovement table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#tabMovement table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#tabMovement table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabMovement .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }

                    });
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblEmployeeMovementList");
                    moveFilterFields();
                },
            }).navGrid("#divEmployeeMovementPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            /*jQuery("#tabMovement .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery("#tabMovement .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblMovementFilter\">Show Filter</label>");
            jQuery("#lblMovementFilter").after("<input type=\"checkbox\" id=\"chkMovementFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");*/

            $("#divEmployeeMovementPager").css("width", "100%");
            $("#divEmployeeMovementPager").css("height", "100%");

            $("#tblEmployeeMovementList_toppager_center").hide();
            $("#tblEmployeeMovementList_toppager_right").hide();
            $("#tblEmployeeMovementList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblShowActiveOnly\">Show Active Only</label>");
            jQuery("#lblShowActiveOnly").after("<input type=\"checkbox\" id=\"cbShowActiveOnly\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery("#cbShowActiveOnly").after("<label class=\"ui-row-label\" id=\"lblMovementFilter\">Show Filters</label>");
            jQuery("#lblMovementFilter").after("<input type=\"checkbox\" id=\"chkMovementFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divEmployeeMovementPager_custom_block_right").appendTo("#divEmployeeMovementPager_left");
            $("#divEmployeeMovementPager_center .ui-pg-table").appendTo("#divEmployeeMovementPager_right");

        },

        AddPrintLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return objEmployeeMovementListJS.Print(" + rowObject.ID + ");\">" +
                "<span class=\"btn-glyph-dynamic\"><span class=\"glyphicon glyphicon-print\"></span></span></a>";
        },

        EditLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link movement-id-link' onclick=\"return objEmployeeMovementListJS.EditMode(" + rowObject.ID + ", '" + rowObject.From + "', &quot;" + rowObject.To + "&quot;,'" + "NO" + "','"+ rowObject.EmployeeField+"');\">" +
                "" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        Print: function (id) {
            var GetSuccessFunction = function (data) {
                if (data != null) {
                    if (data.Result != null) {
                        //console.log(data);
                        $("#divPrintMovement #lblMovementID").text(data.Result.CarNumber);
                        $("#divPrintMovement #lblCompanyName").text(data.Result.CompanyName);
                        $("#divPrintMovement #lblIDNumber").text(data.Result.IDNumber);
                        $("#divPrintMovement #lblNewIDNumber").text(data.Result.NewIDNumber);
                        $("#divPrintMovement #lblEmployeeName").text(data.Result.EmployeeName);
                        $("#divPrintMovement #lblDateHired").text(data.Result.DateHired);
                        $("#divPrintMovement #lblOrgGroup").text(data.Result.OrgGroup);
                        $("#divPrintMovement #lblPosition").text(data.Result.Position + " / " + data.Result.EmploymentStatus);
                        $("#divPrintMovement #lblReason").text(data.Result.Reason);
                        $("#divPrintMovement #lblHRDComments").text(data.Result.HRDComments);
                        $("#divPrintMovement #lblDateEffectiveFrom").text(data.Result.DateEffective);
                        $("#divPrintMovement #lblMovementType").text(data.Result.MovementType);

                        $("#divPrintMovement #idRegionalManager").html(data.Result.RegionalManager);
                        $("#divPrintMovement #idHrdManager").html(data.Result.HRDManager);
                        $("#divPrintMovement #idCompanyPresident").html(data.Result.CompanyPresident);

                        $("#divPrintMovement #idDetails").html("");
                        $("#divPrintMovement #idDetails").hide();
                        $("#divPrintMovement #idDetailsTable").show();
                        if (data.Result.SpecialCases == "SHOW_DETAILS") {
                            if (data.Result.Details != null) {
                                if (data.Result.Details.length > 0) {
                                    $("#divPrintMovement #idDetails").css("font-size", "11px");
                                    $("#divPrintMovement #idDetails").css("margin-top", "-1%");
                                }
                                if (data.Result.Details.length > 500) {
                                    $("#divPrintMovement #idDetails").css("font-size", "10px");
                                    $("#divPrintMovement #idDetails").css("margin-top", "-2%");
                                }
                                if (data.Result.Details.length > 1000) {
                                    $("#divPrintMovement #idDetails").css("font-size", "9px");
                                    $("#divPrintMovement #idDetails").css("margin-top", "-3%");
                                }
                                if (data.Result.Details.length > 1800) {
                                    $("#divPrintMovement #idDetails").css("font-size", "8px");
                                    $("#divPrintMovement #idDetails").css("margin-top", "-4%");
                                }
                            }
                            $("#divPrintMovement #idDetails").html(data.Result.Details);
                            $("#divPrintMovement #idDetails").show();
                            $("#divPrintMovement #idDetailsTable").hide();
                        }

                        //console.log(data);

                        $(data.Result.PrintDetailsList).each(function (index, item) {
                            $("#divPrintMovement #idPosDetailsFrom").html("**********");
                            $("#divPrintMovement #idPosDetailsTo").html(data.Result.Position);
                            $("#divPrintMovement #idBasicDetailsFrom").html("**********");
                            $("#divPrintMovement #idBasicDetailsTo").html("**********");
                            $("#divPrintMovement #idAllowanceDetailsFrom").html("**********");
                            $("#divPrintMovement #idAllowanceDetailsTo").html("**********");
                            $("#divPrintMovement #idBrnDetailsFrom").html("**********");
                            $("#divPrintMovement #idBrnDetailsTo").html(data.Result.OrgGroup);

                            if (item.DetailsLabel == $("#divPrintMovement #idPosDetails").html()) {
                                $("#divPrintMovement #idPosDetailsFrom").html(item.From);
                                $("#divPrintMovement #idPosDetailsTo").html(item.To);
                            }
                            if (item.DetailsLabel == $("#divPrintMovement #idBrnDetails").html()) {
                                $("#divPrintMovement #idBrnDetailsFrom").html(item.From);
                                $("#divPrintMovement #idBrnDetailsTo").html(item.To);
                            }
                        });

                        /*$("#printDetailsTable").html(" <tr> \
                                    <td style=\"width:30%\"></td> \
                                    <td style=\"width:33%; font-size: 11px;\">From</td> \
                                    <td style=\"width:4%\"></td> \
                                    <td style=\"width:33%; font-size: 11px;\">To</td> \
                                </tr>");*/
                        /*$(data.Result.PrintDetailsList).each(function (index, item) {
                            if (item.DetailsLabel != "") {
                                $("#printDetailsTable").append("<tr> \
                                        <td style=\"width:30%;font-family: Segoe UI;font-size: 11px;\" id=\"lblDetailsLabel_" + index + "\"></td> \
                                        <td style=\"width:33%;font-family: Segoe UI;font-size: 11px; border-bottom: 1px solid black; text-align: left\" id=\"lblFrom_" + index + "\"></td> \
                                        <td style=\"width:4%\"></td> \
                                        <td style=\"width:33%;font-family: Segoe UI;font-size: 11px; border-bottom: 1px solid black; text-align: left\" id=\"lblTo_" + index + "\"></td> \
                                    </tr>");
                                $("#lblDetailsLabel_" + index).text(item.DetailsLabel);
                                $("#lblFrom_" + index).text(item.From);
                                $("#lblTo_" + index).text(item.To);
                            }
                        });*/

                        $("#lblDateGenerated").text(data.Result.DateGenerated);

                        objEMSCommonJS.ShowPrintModal("divPrintMovement");
                    }
                }
            };
            objEMSCommonJS.GetAjax(GetPrintURL
                + "&IDDelimited=" + id, {}, "", GetSuccessFunction);

            return false;
        },

        EditMode: function (id, From, To, IsEdit,EmployeeFieldType) {

            var SuccessFunction = function () {
              
                $("#divMovementAddModal").modal("show");
                $('.modal-header p').each(function () {
                    var text = $(this).text();
                    $(this).text(text.replace('Add', 'View'));
                });

                // Btn Fields 
                $("#AddMovementField").hide();
                $("#EditMovementField").show();
                $("#UpdateMovementField").hide();
                // fields to disable
                $("#dpEffectiveDateFrom").attr("readonly", true);
                $("#dpEffectiveDateTo").attr("readonly", true);
                $("#cbUseCurrent").prop("disabled", true);
                $("#txtReason").attr("readonly", true);
                $("#txtHRDComments").attr("readonly", true);

                $("#btnAddField , #btnRemove").remove();


                var GetSuccessFunction = function (data) {
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
                        ////hide employee field, old value and new value fields
                        $("#oldValue").css("display", "none");
                        $("#newValue").css("display", "none");
                        $("#divAdditionalFields").css("display", "none");
                        $("#employeeFieldTable").css("display", "block");
                        ////remove class 'required-field' of hidden fields
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

                        objEmployeeMovementAddListJS.LoadJQGrid(true, From, To, EmployeeFieldType);
                    }
                };
                objEMSCommonJS.GetAjax(GetEmploymentMovementMappingURL + "&MovementType=" + $("#ddlMovementType").val(), {}, "", GetSuccessFunction);


                if ($("#ddlEmployeeField").val() == "SECONDARY_DESIG") {
                    $("#ddlEmployeeField").change();
                    $("#txtNewValue").val($("#hdnOrgGroupSecondaryOrgGroupDescription").val());
                    $("#txtNewValue2").val($("#hdnOrgGroupSecondaryPositionDescription").val());
                    $("#txtNewValue").prop("readonly", true);
                    $("#txtNewValue2").prop("readonly", true);
                }
            };
            LoadPartialSuccessFunction(EmployeeMovementAddURL + "?EmployeeID=" + id + "&IsEdit=true", "divMovementModalContainer", SuccessFunction);

            return false;
        },

        SetLocalStorage: function () {
            localStorage["EmployeeMovementListEmployeeFieldDelimited"] = objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedEmployeeField").value;
            localStorage["EmployeeMovementListEmployeeFieldDelimitedText"] = objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedEmployeeField").text;
            localStorage["EmployeeMovementListMovementTypeDelimited"] = objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedMovementType").value;
            localStorage["EmployeeMovementListMovementTypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedMovementType").text;
            localStorage["EmployeeMovementListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedStatus").value;
            localStorage["EmployeeMovementListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedStatus").text;
            localStorage["EmployeeMovementListFrom"] = $("#tabMovement #txtFilterFrom").val();
            localStorage["EmployeeMovementListTo"] = $("#tabMovement #txtFilterTo").val();
            localStorage["EmployeeMovementListDateEffectiveFromFrom"] = $("#tabMovement #txtFilterDateEffectiveFromFrom").val();
            localStorage["EmployeeMovementListDateEffectiveFromTo"] = $("#tabMovement #txtFilterDateEffectiveFromTo").val();
            localStorage["EmployeeMovementListDateEffectiveToFrom"] = $("#tabMovement #txtFilterDateEffectiveToFrom").val();
            localStorage["EmployeeMovementListDateEffectiveToTo"] = $("#tabMovement #txtFilterDateEffectiveToTo").val();
            localStorage["EmployeeMovementListCreatedDateFrom"] = $("#tabMovement #txtFilterCreatedDateFrom").val();
            localStorage["EmployeeMovementListCreatedDateTo"] = $("#tabMovement #txtFilterCreatedDateTo").val();
            localStorage["EmployeeMovementListCreatedByDelimited"] = objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedCreatedBy").value;
            localStorage["EmployeeMovementListCreatedByDelimitedText"] = objEMSCommonJS.GetMultiSelectList("tabMovement #multiSelectedCreatedBy").text;
            localStorage["EmployeeMovementListReason"] = $("#tabMovement #txtFilterReason").val();
            localStorage["EmployeeMovementListHRDComments"] = $("#tabMovement #txtFilterHRDComments").val();
        },

        GetLocalStorage: function () {
            objEMSCommonJS.SetMultiSelectList("tabMovement #multiSelectedEmployeeField"
                , "EmployeeMovementListEmployeeFieldDelimited"
                , "EmployeeMovementListEmployeeFieldDelimitedText");
            objEMSCommonJS.SetMultiSelectList("tabMovement #multiSelectedMovementType"
                , "EmployeeMovementListMovementTypeDelimited"
                , "EmployeeMovementListMovementTypeDelimitedText");
            objEMSCommonJS.SetMultiSelectList("tabMovement #multiSelectedStatus"
                , "EmployeeMovementListStatusDelimited"
                , "EmployeeMovementListStatusDelimitedText");

            $("#tabMovement #txtFilterFrom").val(localStorage["EmployeeMovementListFrom"]);
            $("#tabMovement #txtFilterTo").val(localStorage["EmployeeMovementListTo"]);

            $("#tabMovement #txtFilterDateEffectiveFromFrom").val(localStorage["EmployeeMovementListDateEffectiveFromFrom"]);
            $("#tabMovement #txtFilterDateEffectiveFromTo").val(localStorage["EmployeeMovementListDateEffectiveFromTo"]);
            $("#tabMovement #txtFilterDateEffectiveToFrom").val(localStorage["EmployeeMovementListDateEffectiveToFrom"]);
            $("#tabMovement #txtFilterDateEffectiveToTo").val(localStorage["EmployeeMovementListDateEffectiveToTo"]);

            $("#tabMovement #txtFilterCreatedDateFrom").val(localStorage["EmployeeMovementListCreatedDateFrom"]);
            $("#tabMovement #txtFilterCreatedDateTo").val(localStorage["EmployeeMovementListCreatedDateTo"]);
            objEMSCommonJS.SetMultiSelectList("tabMovement #multiSelectedCreatedBy"
                , "EmployeeMovementListCreatedByDelimited"
                , "EmployeeMovementListCreatedByDelimitedText");

            $("#tabMovement #txtFilterReason").val(localStorage["EmployeeMovementListReason"]);
            $("#tabMovement #txtFilterHRDComments").val(localStorage["EmployeeMovementListHRDComments"]);
        },

        ShowHideFilter: function () {
            if ($("#tabMovement #chkMovementFilter").is(":checked")) {
                $("#tabMovement .jqgfirstrow .filterFields").show();
            }
            else if ($("#tabMovement #chkMovementFilter").is(":not(:checked)")) {
                $("#tabMovement .jqgfirstrow .filterFields").hide();
            }
        },

        ShowHideActiveOnly: function () {
            if ($("#tabMovement #cbShowActiveOnly").is(":checked")) {
                localStorage["EmployeeMovementListShowActiveOnly"] = true;
            }
            else if ($("#tabMovement #cbShowActiveOnly").is(":not(:checked)")) {
                localStorage["EmployeeMovementListShowActiveOnly"] = false;
            }
        },

        DeleteSuccessFunction: function () {
            $("#divMovementFilter #btnSearch").click();
        },
    };

    objEmployeeMovementListJS.Initialize();
});