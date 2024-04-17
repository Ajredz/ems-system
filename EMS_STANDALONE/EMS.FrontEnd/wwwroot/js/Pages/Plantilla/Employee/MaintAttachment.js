var objEmployeeMovementListJS;
var EmployeeMovementListURL = "/Plantilla/Employee/Movement?handler=List";
var EmployeeMovementAddURL = "/Plantilla/Employee/MovementAdd";
var GetEmployeeMovementTypeURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=MOVEMENT_TYPE";
var GetEmployeeFieldURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=MOVEMENT_EMP_FIELD";
var GetPrintURL = "/Plantilla/Employee/Movement?handler=Print";
var CreatedByAutoCompleteURL = "/Plantilla/Employee/Movement?handler=CreatedByAutoComplete";


$(document).ready(function () {
    objEmployeeMovementListJS = {
        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                EmployeeID: $("#divEmployeeModal #hdnID").val(),
                EmployeeFieldDelimited: localStorage["EmployeeMovementListEmployeeFieldDelimited"],
                MovementTypeDelimited: localStorage["EmployeeMovementListMovementTypeDelimited"],
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

            objEMSCommonJS.BindFilterMultiSelectEnum("tabMovement #multiSelectedEmployeeField", GetEmployeeFieldURL);
            objEMSCommonJS.BindFilterMultiSelectEnum("tabMovement #multiSelectedMovementType", GetEmployeeMovementTypeURL);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("tabMovement #txtFilterCreatedBy"
                , CreatedByAutoCompleteURL, 20, "multiSelectedCreatedBy");


            $("#tabMovement #cbShowActiveOnly").click(function () {
                $("#tabMovement #btnSearch").click();
            });

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
                colNames: ["", "", "", "Employee Field", "Movement Type", "From", "To", "Date Effective From", "Date Effective To",
                    "Modified Date", "Modified By",
                    "Remarks", "Additional Remarks"],
                colModel: [
                    { hidden: true },
                    { width: 55, key: true, name: "ID", index: "ID", formatter: objEmployeeMovementListJS.EditLink },
                    { width: 45, name: "ID", index: "ID", align: "center", sortable: true, formatter: objEmployeeMovementListJS.AddPrintLink },
                    { name: "EmployeeField", index: "EmployeeField", editable: true, align: "left", sortable: true },
                    { name: "MovementType", index: "MovementType", editable: true, align: "left", sortable: true },
                    { name: "From", index: "From", editable: true, align: "left", sortable: true },
                    { name: "To", index: "To", editable: true, align: "left", sortable: true },
                    { name: "DateEffectiveFrom", index: "DateEffectiveFrom", editable: true, align: "left", sortable: true },
                    { name: "DateEffectiveTo", index: "DateEffectiveTo", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                    { name: "CreatedByName", index: "CreatedByName", editable: true, align: "left", sortable: true },
                    { hidden: true, name: "Reason", index: "Reason", editable: true, align: "left", sortable: true },
                    { hidden: true, name: "HRDComments", index: "HRDComments", editable: true, align: "left", sortable: true },
                ],
                toppager: $("#divEmployeeMovementPager"),
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
                    objEmployeeMovementListJS.ShowHideFilter();

                    $("#tabMovement #chkMovementFilter").on('change', function () {
                        objEmployeeMovementListJS.ShowHideFilter();
                        localStorage["EmployeeMovementListFilterOption"] = $("#chkMovementFilter").is(":checked");
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
            jQuery("#tabMovement .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery("#tabMovement .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblMovementFilter\">Show Filter</label>");
            jQuery("#lblMovementFilter").after("<input type=\"checkbox\" id=\"chkMovementFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        AddPrintLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return objEmployeeMovementListJS.Print(" + rowObject.ID + ");\">" +
                "<span class=\"btn-glyph-dynamic\"><span class=\"glyphicon glyphicon-print\"></span></span></a>";
        },

        EditLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link movement-id-link' onclick=\"return objEmployeeMovementListJS.EditMode(" + rowObject.ID + ");\">" +
                "" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        Print: function (id) {

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
                + "&IDDelimited=" + id, {}, "", GetSuccessFunction);


            return false;
        },

        EditMode: function (id) {

            var SuccessFunction = function () {
                $("#divMovementAddModal").modal("show");
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
    };

    objEmployeeMovementListJS.Initialize();
});