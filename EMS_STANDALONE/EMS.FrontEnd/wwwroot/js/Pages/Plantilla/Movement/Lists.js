var objMovementListJS;
var MovementListURL = "/Plantilla/Movement?handler=List";
var EmployeeMovementAddURL = "/Plantilla/Employee/MovementAdd";
var GetEmployeeMovementTypeURL = "/Plantilla/Movement?handler=ReferenceValue&RefCode=MOVEMENT_TYPE";
var GetEmployeeFieldURL = "/Plantilla/Movement?handler=ReferenceValue&RefCode=MOVEMENT_EMP_FIELD";
var UploadInsertURL = "/Plantilla/Movement?handler=UploadInsert";
var DownloadFormURL = "/Plantilla/Movement?handler=DownloadMovementTemplate";
var GetCheckMovementExportListURL = "/Plantilla/Movement?handler=CheckMovementExportList";
var DownloadMovementExportListURL = "/Plantilla/Movement?handler=DownloadMovementExportList";
var GetPrintURL = "/Plantilla/Movement?handler=Print";
var OrgGroupAutoCompleteURL = "/Plantilla/Movement?handler=OrgGroupAutoComplete";
var CreatedByAutoCompleteURL = "/Plantilla/Movement?handler=CreatedByAutoComplete";
var GetStatusURL = "/Plantilla/Movement?handler=StatusFilter";

// For movement checker, 13, 31, 68, 95, 98, 111 , 144, 209, 212, 215-220, 225, 231-232, 279, 283, 289-293, 337-349, 473-474, 503-506, 533-540

$(document).ready(function () {
    objMovementListJS = {
        Initialize: function () {
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            
            s.ElementBinding();
            var param = {
                EmployeeName: localStorage["MovementListEmployeeNameDelimited"],
                OldEmployeeID: localStorage["MovementListOldEmployeeID"],
                OrgGroupDelimited: localStorage["MovementListOrgGroupDelimited"],
                EmployeeFieldDelimited: localStorage["MovementListEmployeeFieldDelimited"],
                MovementTypeDelimited: localStorage["MovementListMovementTypeDelimited"],
                StatusDelimited: localStorage["MovementListStatusDelimited"],
                From: localStorage["MovementListFrom"],
                To: localStorage["MovementListTo"],
                DateEffectiveFromFrom: localStorage["MovementListDateEffectiveFromFrom"],
                DateEffectiveFromTo: localStorage["MovementListDateEffectiveFromTo"],
                DateEffectiveToFrom: localStorage["MovementListDateEffectiveToFrom"],
                DateEffectiveToTo: localStorage["MovementListDateEffectiveToTo"],
                Reason: localStorage["MovementListReason"],
                CreatedDateFrom: localStorage["MovementListCreatedDateFrom"],
                CreatedDateTo: localStorage["MovementListCreatedDateTo"],
                CreatedByDelimited: localStorage["MovementListCreatedByDelimited"],
                HRDComments: localStorage["MovementListHRDComments"],
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

            $("#btnSearch").click(function () {
                var param = {
                    EmployeeName: $("#txtFilterEmployeeName").val(),
                    OldEmployeeID: $("#txtFilterOldEmployeeID").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    EmployeeFieldDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeField").value,
                    MovementTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedMovementType").value,
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value,
                    From: $("#txtFilterFrom").val(),
                    To: $("#txtFilterTo").val(),
                    DateEffectiveFromFrom: $("#txtFilterDateEffectiveFromFrom").val(),
                    DateEffectiveFromTo: $("#txtFilterDateEffectiveFromTo").val(),
                    DateEffectiveToFrom: $("#txtFilterDateEffectiveToFrom").val(),
                    DateEffectiveToTo: $("#txtFilterDateEffectiveToTo").val(),
                    CreatedDateFrom: $("#txtFilterCreatedDateFrom").val(),
                    CreatedDateTo: $("#txtFilterCreatedDateTo").val(),
                    CreatedByDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedCreatedBy").value,
                    Reason: $("#txtFilterReason").val(),
                    HRDComments: $("#txtFilterHRDComments").val(),
                    IsShowActiveOnly: $("#cbShowActiveOnly").prop("checked")
                };
                s.SetLocalStorage();
                ResetJQGridState("tblMovementList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedCreatedBy").html("");
                $("#multiSelectedEmployeeField").html("");
                $("#multiSelectedMovementType").html("");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedEmployeeFieldOption label, #multiSelectedEmployeeFieldOption input").prop("title", "add");
                $("#multiSelectedMovementTypeOption label, #multiSelectedMovementTypeOption input").prop("title", "add");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                var SuccessFunction = function () {
                    $("#divMovementAddModal").modal("show");
                };
                LoadPartialSuccessFunction(EmployeeMovementAddURL + "?EmployeeID=" + $("#divEmployeeModal #hdnID").val(), "divMovementModalContainer", SuccessFunction);
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedEmployeeField", GetEmployeeFieldURL);
            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedMovementType", GetEmployeeMovementTypeURL);
            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedStatus", GetStatusURL);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterCreatedBy"
                , CreatedByAutoCompleteURL, 20, "multiSelectedCreatedBy");


            $("#cbShowActiveOnly").click(function () {
                $("#btnSearch").click();
            });

            $("#btnUploadInsert").click(function () {
                objEMSCommonJS.UploadModal(UploadInsertURL, "Upload (Insert)", DownloadFormURL);
                $('#divModalErrorMessage').html('');
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objMovementListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblMovementList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblMovementList").jqGrid("getGridParam", "sortorder")
                + "&EmployeeName=" + $("#txtFilterEmployeeName").val()
                + "&OldEmployeeID=" + $("#txtFilterOldEmployeeID").val()
                + "&OrgGroup=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeField").value
                + "&EmployeeFieldDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeField").value
                + "&MovementTypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedMovementType").value
                + "&StatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value
                + "&From=" + $("#txtFilterFrom").val()
                + "&To=" + $("#txtFilterTo").val()
                + "&DateEffectiveFromFrom=" + $("#txtFilterDateEffectiveFromFrom").val()
                + "&DateEffectiveFromTo=" + $("#txtFilterDateEffectiveFromTo").val()
                + "&DateEffectiveToFrom=" + $("#txtFilterDateEffectiveToFrom").val()
                + "&DateEffectiveToTo=" + $("#txtFilterDateEffectiveToTo").val()
                + "&Reason=" + $("#txtFilterReason").val()
                + "&CreatedDateFrom=" + $("#txtFilterCreatedDateFrom").val()
                + "&CreatedDateTo=" + $("#txtFilterCreatedDateTo").val()
                + "&CreatedByDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCreatedBy").value
                + "&HRDComments=" + $("#txtFilterHRDComments").val()
                + "&IsShowActiveOnly=" + $("#cbShowActiveOnly").prop("checked")
                ;

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadMovementExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckMovementExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },
        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblMovementList") == "" ? "" : $.parseJSON(localStorage.getItem("tblMovementList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblMovementList").jqGrid("GridUnload");
            $("#tblMovementList").jqGrid("GridDestroy");
            $("#tblMovementList").jqGrid({
                url: MovementListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "", "Employee Name", "Org Group", "Employee Field", "Movement Type", "", "Status",
                    "From", "To", "Date Effective From", "Date Effective To", "Old Employee ID",
                    "Modified Date", "Modified By",
                    "Remarks", "Additional Remarks"],
                colModel: [
                    { hidden: true },
                    { key: true, name: "ID", index: "ID", align: "center", formatter: objMovementListJS.EditLink },
                    { width: 45, name: "ID", index: "ID", align: "center", sortable: true, formatter: objMovementListJS.AddPrintLink },
                    { name: "EmployeeName", index: "EmployeeName", editable: true, align: "left", sortable: true },
                    { hidden: true, name: "OrgGroup", index: "OrgGroup", editable: true, align: "left", sortable: true },
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
                    { hidden: true, name: "OldEmployeeID", index: "OldEmployeeID", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                    { name: "CreatedByName", index: "CreatedByName", editable: true, align: "left", sortable: true },
                    { hidden: true, name: "Reason", index: "Reason", editable: true, align: "left", sortable: true },
                    { hidden: true, name: "HRDComments", index: "HRDComments", editable: true, align: "left", sortable: true },
                ],
                toppager: $("#divMovementPager"),
                pager: $("#divMovementPager"),
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
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                loadComplete: function (data) {
                    Loading(false);
                    //WIP
                    //setTimeout(function () { $(".glyphicon-print:first").click(); }, 500);
                    //WIP
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
                        AutoSizeColumnJQGrid("tblMovementList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                    }

                    if (localStorage["MovementListFilterOption"] != undefined) {
                        $("#chkMovementFilter").prop('checked', JSON.parse(localStorage["MovementListFilterOption"]));
                    }
                    if (localStorage["MovementListShowActiveOnly"] != undefined) {
                        $("#cbShowActiveOnly").prop('checked', JSON.parse(localStorage["MovementListShowActiveOnly"]));
                    }
                    objMovementListJS.ShowHideFilter();
                    objMovementListJS.ShowHideActiveOnly();

                    $("#chkMovementFilter").on('change', function () {
                        objMovementListJS.ShowHideFilter();
                        localStorage["MovementListFilterOption"] = $("#chkMovementFilter").is(":checked");
                    });
                    $("#cbShowActiveOnly").on('change', function () {
                        objMovementListJS.ShowHideActiveOnly();
                        localStorage["MovementListShowActiveOnly"] = $("#cbShowActiveOnly").is(":checked");
                        $("#btnSearch").click();
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $(".ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
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
                    GetJQGridState("tblMovementList");
                    moveFilterFields();
                },
            }).navGrid("#divEmployeeMovementPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#divMovementPager").css("width", "100%");
            $("#divMovementPager").css("height", "100%");

            $("#tblMovementList_toppager_center").hide();
            $("#tblMovementList_toppager_right").hide();
            $("#tblMovementList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblShowActiveOnly\">Show Active Only</label>");
            jQuery("#lblShowActiveOnly").after("<input type=\"checkbox\" id=\"cbShowActiveOnly\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery("#cbShowActiveOnly").after("<label class=\"ui-row-label\" id=\"lblMovementFilter\">Show Filters</label>");
            jQuery("#lblMovementFilter").after("<input type=\"checkbox\" id=\"chkMovementFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divMovementPager_custom_block_right").appendTo("#divMovementPager_left");
            $("#divMovementPager_center .ui-pg-table").appendTo("#divMovementPager_right");
        },

        AddPrintLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return objMovementListJS.Print(" + rowObject.ID + ");\">" +
                "<span class=\"btn-glyph-dynamic\"><span class=\"glyphicon glyphicon-print\"></span></span></a>";
        },

        Print: function (id) {

            var GetSuccessFunction = function (data) {
                
                if (data != null) {
                    if (data.Result != null) {
                        $("#lblMovementID").text(data.Result.CarNumber);
                        $("#lblCompanyName").text(data.Result.CompanyName);
                        $("#lblIDNumber").text(data.Result.IDNumber);
                        $("#lblNewIDNumber").text(data.Result.NewIDNumber);
                        $("#lblEmployeeName").text(data.Result.EmployeeName);
                        $("#lblDateHired").text(data.Result.DateHired);
                        $("#lblOrgGroup").text(data.Result.OrgGroup);
                        $("#lblPosition").text(data.Result.Position + " / " + data.Result.EmploymentStatus);
                        $("#lblReason").text(data.Result.Reason);
                        $("#lblHRDComments").text(data.Result.HRDComments);
                        $("#lblDateEffectiveFrom").text(data.Result.DateEffective);
                        $("#lblMovementType").text(data.Result.MovementType);

                        $("#idRegionalManager").html(data.Result.RegionalManager);
                        $("#idHrdManager").html(data.Result.HRDManager);
                        $("#idCompanyPresident").html(data.Result.CompanyPresident);

                        $("#idDetails").html("");
                        $("#idDetails").hide();
                        $("#idDetailsTable").show();
                        if (data.Result.SpecialCases == "SHOW_DETAILS") {
                            if (data.Result.Details != null) {
                                if (data.Result.Details.length > 0) {
                                    $("#idDetails").css("font-size", "11px");
                                    $("#idDetails").css("margin-top", "-1%");
                                }
                                if (data.Result.Details.length > 500) {
                                    $("#idDetails").css("font-size", "10px");
                                    $("#idDetails").css("margin-top", "-2%");
                                }
                                if (data.Result.Details.length > 1000) {
                                    $("#idDetails").css("font-size", "9px");
                                    $("#idDetails").css("margin-top", "-3%");
                                }
                                if (data.Result.Details.length > 1800) {
                                    $("#idDetails").css("font-size", "8px");
                                    $("#idDetails").css("margin-top", "-4%");
                                }
                            }
                            $("#idDetails").html(data.Result.Details);
                            $("#idDetails").show();
                            $("#idDetailsTable").hide();
                        }

                        //console.log(data);

                        $(data.Result.PrintDetailsList).each(function (index, item) {
                            $("#idPosDetailsFrom").html("**********");
                            $("#idPosDetailsTo").html(data.Result.Position);
                            $("#idBasicDetailsFrom").html("**********");
                            $("#idBasicDetailsTo").html("**********");
                            $("#idAllowanceDetailsFrom").html("**********");
                            $("#idAllowanceDetailsTo").html("**********");
                            $("#idBrnDetailsFrom").html("**********");
                            $("#idBrnDetailsTo").html(data.Result.OrgGroup);

                            if (item.DetailsLabel == $("#idPosDetails").html()) {
                                $("#idPosDetailsFrom").html(item.From);
                                $("#idPosDetailsTo").html(item.To);
                            }
                            if (item.DetailsLabel == $("#idBrnDetails").html()) {
                                $("#idBrnDetailsFrom").html(item.From);
                                $("#idBrnDetailsTo").html(item.To);
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

            //objEMSCommonJS.GetAjax(GetPrintDownloadURL, {}, "", null);


            return false;
        },

        SetLocalStorage: function () {
            localStorage["MovementListEmployeeName"] = $("#txtFilterEmployeeName").val();
            localStorage["MovementListOldEmployeeID"] = $("#txtFilterOldEmployeeID").val();
            localStorage["MovementListEmployeeFieldDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeField").value;
            localStorage["MovementListEmployeeFieldDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeField").text;
            localStorage["MovementListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["MovementListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;
            localStorage["MovementListMovementTypeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedMovementType").value;
            localStorage["MovementListMovementTypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedMovementType").text;
            localStorage["MovementListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value;
            localStorage["MovementListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").text;
            localStorage["MovementListFrom"] = $("#txtFilterFrom").val();
            localStorage["MovementListTo"] = $("#txtFilterTo").val();
            localStorage["MovementListDateEffectiveFromFrom"] = $("#txtFilterDateEffectiveFromFrom").val();
            localStorage["MovementListDateEffectiveFromTo"] = $("#txtFilterDateEffectiveFromTo").val();
            localStorage["MovementListDateEffectiveToFrom"] = $("#txtFilterDateEffectiveToFrom").val();
            localStorage["MovementListDateEffectiveToTo"] = $("#txtFilterDateEffectiveToTo").val();
            localStorage["MovementListCreatedDateFrom"] = $("#txtFilterCreatedDateFrom").val();
            localStorage["MovementListCreatedDateTo"] = $("#txtFilterCreatedDateTo").val();
            localStorage["MovementListCreatedByDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCreatedBy").value;
            localStorage["MovementListCreatedByDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCreatedBy").text;
            localStorage["MovementListReason"] = $("#txtFilterReason").val();
            localStorage["MovementListHRDComments"] = $("#txtFilterHRDComments").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterEmployeeName").val(localStorage["MovementListEmployeeName"]);

            $("#txtFilterOldEmployeeID").val(localStorage["MovementListOldEmployeeID"]);
            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "MovementListOrgGroupDelimited"
                , "MovementListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedEmployeeField"
                , "MovementListEmployeeFieldDelimited"
                , "MovementListEmployeeFieldDelimitedText");
            objEMSCommonJS.SetMultiSelectList("multiSelectedMovementType"
                , "MovementListMovementTypeDelimited"
                , "MovementListMovementTypeDelimitedText");
            objEMSCommonJS.SetMultiSelectList("multiSelectedStatus"
                , "MovementListStatusDelimited"
                , "MovementListStatusDelimitedText");

            $("#txtFilterFrom").val(localStorage["MovementListFrom"]);
            $("#txtFilterTo").val(localStorage["MovementListTo"]);

            $("#txtFilterDateEffectiveFromFrom").val(localStorage["MovementListDateEffectiveFromFrom"]);
            $("#txtFilterDateEffectiveFromTo").val(localStorage["MovementListDateEffectiveFromTo"]);
            $("#txtFilterDateEffectiveToFrom").val(localStorage["MovementListDateEffectiveToFrom"]);
            $("#txtFilterDateEffectiveToTo").val(localStorage["MovementListDateEffectiveToTo"]);

            $("#txtFilterCreatedDateFrom").val(localStorage["MovementListCreatedDateFrom"]);
            $("#txtFilterCreatedDateTo").val(localStorage["MovementListCreatedDateTo"]);
            objEMSCommonJS.SetMultiSelectList("multiSelectedCreatedBy"
                , "MovementListCreatedByDelimited"
                , "MovementListCreatedByDelimitedText");

            $("#txtFilterReason").val(localStorage["MovementListReason"]);
            $("#txtFilterHRDComments").val(localStorage["MovementListHRDComments"]);
        },

        ShowHideFilter: function () {
            if ($("#chkMovementFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkMovementFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },
        ShowHideActiveOnly: function () {
            if ($("#cbShowActiveOnly").is(":checked")) {
                localStorage["MovementListShowActiveOnly"] = true;
            }
            else if ($("#cbShowActiveOnly").is(":not(:checked)")) {
                localStorage["MovementListShowActiveOnly"] = false;
            }
        },
    };

    objMovementListJS.Initialize();
});