var objEmployeeListJS;

const EmployeeListURL = "/LogActivity/EmployeeList?handler=List";
const EmploymentStatusDropDownURL = "/LogActivity/EmployeeList?handler=ReferenceValue&RefCode=EMPLOYMENT_STATUS";
const OrgGroupAutoCompleteURL = "/LogActivity/EmployeeList?handler=OrgTypeAutoComplete";
const PositionAutoCompleteURL = "/LogActivity/EmployeeList?handler=PositionAutoComplete";
const OrgGroupByOrgTypeAutoCompleteURL = "/LogActivity/EmployeeList?handler=OrgGroupByOrgTypeAutoComplete";
const GetCheckEmployeeExportListURL = "/LogActivity/EmployeeList?handler=CheckEmployeeExportList";
const DownloadEmployeeExportListURL = "/LogActivity/EmployeeList?handler=DownloadEmployeeExportList";

const DisplayFilter = "497,53,54,55,548";

$(document).ready(function () {
    objEmployeeListJS = {
        Initialize: function () {
            var s = this;

            s.ElementBinding();

            localStorage["ShowActiveEmployee"] = false;
            var param = {
                ID: localStorage["PlantillaEmployeeListID"],
                Code: localStorage["PlantillaEmployeeListCode"],
                Name: localStorage["PlantillaEmployeeListName"],
                OrgGroupDelimited: localStorage["PlantillaEmployeeListOrgGroupDelimited"],
                PositionDelimited: localStorage["PlantillaEmployeeListPositionDelimited"],
                EmploymentStatusDelimited: DisplayFilter,
                DateHiredFrom: localStorage["PlantillaEmployeeListDateHiredFrom"],
                DateHiredTo: localStorage["PlantillaEmployeeListDateHiredTo"],
                DateStatusFrom: localStorage["PlantillaEmployeeListDateStatusFrom"],
                DateStatusTo: localStorage["PlantillaEmployeeListDateStatusTo"],
                BirthDateFrom: localStorage["PlantillaEmployeeListBirthDateFrom"],
                BirthDateTo: localStorage["PlantillaEmployeeListBirthDateTo"],
                OldEmployeeID: localStorage["PlantillaEmployeeListOldEmployeeID"],
                ShowActiveEmployee: false
            };
            s.LoadJQGrid(param);
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterEmployeeID"));

            $("#txtFilterDateHiredFrom, #txtFilterDateHiredTo, #txtFilterStatusUpdateFrom, #txtFilterStatusUpdateTo, #txtFilterMovementDateFrom, #txtFilterMovementDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });
            $("#divEmployeeList #btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterEmployeeID").val(),
                    Percentage: objEMSCommonJS.GetMultiSelectList("multiSelectedPercentage").value,
                    Code: $("#txtFilterCode").val(),
                    Name: $("#txtFilterName").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    EmploymentStatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value == "" ? DisplayFilter : objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value,
                    //CurrentStepDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value,
                    //DateScheduledFrom: $("#txtFilterDateScheduledFrom").val(),
                    //DateScheduledTo: $("#txtFilterDateScheduledTo").val(),
                    //DateCompletedFrom: $("#txtFilterDateCompletedFrom").val(),
                    //DateCompletedTo: $("#txtFilterDateCompletedTo").val(),
                    //Remarks: $("#txtFilterRemarks").val(),
                    DateStatusFrom: $("#txtFilterStatusUpdateFrom").val(),
                    DateStatusTo: $("#txtFilterStatusUpdateTo").val(),
                    DateHiredFrom: $("#txtFilterDateHiredFrom").val(),
                    DateHiredTo: $("#txtFilterDateHiredTo").val(),
                    BirthDateFrom: $("#txtFilterBirthDateFrom").val(),
                    BirthDateTo: $("#txtFilterBirthDateTo").val(),
                    MovementDateFrom: $("#txtFilterMovementDateFrom").val(),
                    MovementDateTo: $("#txtFilterMovementDateTo").val(),
                    OldEmployeeID: $("#txtFilterOldEmployeeID").val(),
                    OrgGroupDelimitedClus: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupClus").value,
                    OrgGroupDelimitedArea: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupArea").value,
                    OrgGroupDelimitedReg: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupReg").value,
                    OrgGroupDelimitedZone: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupZone").value,
                    ShowActiveEmployee: objEmployeeListJS.ShowActiveEmployee()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaEmployeeList");
                s.LoadJQGrid(param);
            });

            $("#divEmployeeList #btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedPosition").html("");
                //$("#multiSelectedCurrentStep").html("");
                $("#multiSelectedEmploymentStatus").html("");
                $("#multiSelectedEmploymentStatusOption label, #multiSelectedEmploymentStatusOption input").prop("title", "add");
                $("#multiSelectedPercentage").html("");
                $("#multiSelectedPercentageOption label, #multiSelectedPercentageOption input").prop("title", "add");
                $("#multiSelectedOrgGroupClus").html("");
                $("#multiSelectedOrgGroupArea").html("");
                $("#multiSelectedOrgGroupReg").html("");
                $("#multiSelectedOrgGroupZone").html("");
                localStorage["PlantillaEmployeeListEmploymentStatusDelimited"] = DisplayFilter;
                $("#divEmployeeList #btnSearch").click();
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeListJS.ExportFunction()",
                    "function");
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedEmploymentStatus", EmploymentStatusDropDownURL);

            $("#lbl_535").remove("");
            $("#lbl_51").remove("");
            $("#lbl_490").remove("");
            $("#lbl_52").remove("");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPercentage"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedPercentage");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , PositionAutoCompleteURL, 20, "multiSelectedPosition");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroupClus"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=CLUS", 20, "multiSelectedOrgGroupClus");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroupArea"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=AREA", 20, "multiSelectedOrgGroupArea");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroupReg"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=REG", 20, "multiSelectedOrgGroupReg");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroupZone"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=ZONE", 20, "multiSelectedOrgGroupZone");
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaEmployeeList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaEmployeeList"));
            var moveFilterFields = function () {
                var intialHeight = $("#divEmployeeList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divEmployeeList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divEmployeeList #filterFieldsContainer");
                });
                $("#divEmployeeList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divEmployeeList div.filterFields").unbind("keyup");
                $("#divEmployeeList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divEmployeeList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblPlantillaEmployeeList").jqGrid("GridUnload");
            $("#tblPlantillaEmployeeList").jqGrid("GridDestroy");
            $("#tblPlantillaEmployeeList").jqGrid({
                url: EmployeeListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Clearance", "New Employee ID", "Name", "Org Group", "Position", "Employment Status", "Date Separated", "Date Hired"
                    , "Movement Updated Date", "Old Employee ID", "Cluster", "Area", "Region", "Zone"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objEmployeeListJS.AddLink },
                    { name: "Percent", index: "Percent", editable: true, align: "center", sortable: true },
                    { name: "Code", index: "Code", editable: true, align: "left", sortable: true },
                    { name: "Name", index: "Name", editable: true, align: "left", sortable: true },
                    { name: "OrgGroup", index: "OrgGroup", editable: true, align: "left", sortable: true },
                    { name: "Position", index: "Position", editable: true, align: "left", sortable: true },
                    { name: "EmploymentStatus", index: "EmploymentStatus", editable: true, align: "left", sortable: true },
                    { name: "DateStatus", index: "DateStatus", editable: true, align: "left", sortable: true },
                    { name: "DateHired", index: "DateHired", editable: true, align: "left", sortable: true },
                    { name: "MovementDate", index: "MovementDate", editable: true, align: "left", sortable: true },
                    { name: "OldEmployeeID", index: "OldEmployeeID", editable: true, align: "left", sortable: true },
                    { name: "Cluster", index: "Cluster", editable: true, align: "left", sortable: true },
                    { name: "Area", index: "Area", editable: true, align: "left", sortable: true },
                    { name: "Region", index: "Region", editable: true, align: "left", sortable: true },
                    { name: "Zone", index: "Zone", editable: true, align: "left", sortable: true },
                ],
                toppager: $("#divPager"),
                pager: $("#divPager"),
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
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblPlantillaEmployeeList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblPlantillaEmployeeList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divEmployeeList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divEmployeeList .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#divEmployeeList .jqgrid-id-link").click(function () {
                            $('#divEmployeeModal').modal('show');
                        });
                    }

                    if (localStorage["EmployeeListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["EmployeeListFilterOption"]));
                    }

                    if (localStorage["ShowActiveEmployee"] != undefined) {
                        $("#chkActive").prop('checked', JSON.parse(localStorage["ShowActiveEmployee"]));
                    }

                    objEmployeeListJS.ShowHideFilter();
                    objEmployeeListJS.ShowActiveEmployee();

                    $("#chkFilter").on('change', function () {
                        objEmployeeListJS.ShowHideFilter();
                        localStorage["EmployeeListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    $("#chkActive").on('change', function () {
                        objEmployeeListJS.ShowActiveEmployee()
                        localStorage["ShowActiveEmployee"] = $("#chkActive").is(":checked");
                        $("#btnSearch").click();
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divEmployeeList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

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
                    GetJQGridState("tblPlantillaEmployeeList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#divPager").css("width", "100%");
            $("#divPager").css("height", "100%");

            $("#tblPlantillaEmployeeList_toppager_center").hide();
            $("#tblPlantillaEmployeeList_toppager_right").hide();
            $("#tblPlantillaEmployeeList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filters</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divPager_custom_block_right").appendTo("#divPager_left");
            $("#divPager_center .ui-pg-table").appendTo("#divPager_right");
        },
        SetLocalStorage: function () {
            localStorage["PlantillaEmployeeListID"] = $("#txtFilterEmployeeID").val();
            localStorage["PlantillaEmployeeListCode"] = $("#txtFilterCode").val();
            localStorage["PlantillaEmployeeListName"] = $("#txtFilterName").val();
            localStorage["PlantillaEmployeeListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["PlantillaEmployeeListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;
            localStorage["PlantillaEmployeeListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["PlantillaEmployeeListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;
            localStorage["PlantillaEmployeeListEmploymentStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value;
            localStorage["PlantillaEmployeeListEmploymentStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").text;
            localStorage["PlantillaEmployeeListDateHiredFrom"] = $("#txtFilterDateHiredFrom").val();
            localStorage["PlantillaEmployeeListDateHiredTo"] = $("#txtFilterDateHiredTo").val();
            localStorage["PlantillaEmployeeListDateStatusFrom"] = $("#txtFilterDateStatusFrom").val();
            localStorage["PlantillaEmployeeListDateStatusTo"] = $("#txtFilterDateStatusTo").val();
            localStorage["PlantillaEmployeeListBirthDateFrom"] = $("#txtFilterBirthDateFrom").val();
            localStorage["PlantillaEmployeeListBirthDateTo"] = $("#txtFilterBirthDateTo").val();
            localStorage["PlantillaEmployeeListOldEmployeeID"] = $("#txtFilterOldEmployeeID").val();

            //localStorage["PlantillaEmployeeListCurrentStepDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value;
            //localStorage["PlantillaEmployeeListCurrentStepDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").text;
            //localStorage["PlantillaEmployeeListScheduledFrom"] = $("#txtFilterScheduledFrom").val();
            //localStorage["PlantillaEmployeeListScheduledTo"] = $("#txtFilterScheduledTo").val();
            //localStorage["PlantillaEmployeeListCompletedFrom"] = $("#txtFilterCompletedFrom").val();
            //localStorage["PlantillaEmployeeListCompletedTo"] = $("#txtFilterCompletedTo").val();
            //localStorage["PlantillaEmployeeListRemarks"] = $("#txtFilterRemarks").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterEmployeeID").val(localStorage["PlantillaEmployeeListID"]);
            $("#txtFilterCode").val(localStorage["PlantillaEmployeeListCode"]);
            $("#txtFilterName").val(localStorage["PlantillaEmployeeListName"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "PlantillaEmployeeListOrgGroupDelimited"
                , "PlantillaEmployeeListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "PlantillaEmployeeListPositionDelimited"
                , "PlantillaEmployeeListPositionDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedEmploymentStatus"
                , "PlantillaEmployeeListEmploymentStatusDelimited"
                , "PlantillaEmployeeListEmploymentStatusDelimitedText");

            $("#txtFilterDateHiredFrom").val(localStorage["PlantillaEmployeeListDateHiredFrom"]);
            $("#txtFilterDateHiredTo").val(localStorage["PlantillaEmployeeListDateHiredTo"]);
            $("#txtFilterDateStatusFrom").val(localStorage["PlantillaEmployeeListDateStatusFrom"]);
            $("#txtFilterDateStatusTo").val(localStorage["PlantillaEmployeeListDateStatusTo"]);
            $("#txtFilterBirthDateFrom").val(localStorage["PlantillaEmployeeListBirthDateFrom"]);
            $("#txtFilterBirthDateTo").val(localStorage["PlantillaEmployeeListBirthDateTo"]);
            $("#txtFilterOldEmployeeID").val(localStorage["PlantillaEmployeeListOldEmployeeID"]);

            //objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStep"
            //    , "PlantillaEmployeeListCurrentStepDelimited"
            //    , "PlantillaEmployeeListCurrentStepDelimitedText");

            //$("#txtFilterScheduledFrom").val(localStorage["PlantillaEmployeeListScheduledFrom"]);
            //$("#txtFilterScheduledTo").val(localStorage["PlantillaEmployeeListScheduledTo"]);
            //$("#txtFilterCompletedFrom").val(localStorage["PlantillaEmployeeListCompletedFrom"]);
            //$("#txtFilterCompletedTo").val(localStorage["PlantillaEmployeeListCompletedTo"]);
            //$("#txtFilterRemarks").val(localStorage["PlantillaEmployeeListApproverRemarks"]);
        },
        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },
        ShowActiveEmployee: function () {
            if ($("#chkActive").is(":checked")) {
                return true;
            }
            else if ($("#chkActive").is(":not(:checked)")) {
                return false;
            }
        },
        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblPlantillaEmployeeList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaEmployeeList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterEmployeeID").val()
                + "&Code=" + $("#txtFilterCode").val()
                + "&Name=" + $("#txtFilterName").val()
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                + "&OrgGroupDelimitedClus=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupClus").value
                + "&OrgGroupDelimitedArea=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupArea").value
                + "&OrgGroupDelimitedReg=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupReg").value
                + "&OrgGroupDelimitedZone=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupZone").value
                + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                + "&EmploymentStatusDelimited=" + (objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value == "" ? DisplayFilter : objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value)
                //+ "&CurrentStep=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value
                //+ "&DateScheduledFrom=" + $("#txtFilterDateScheduledFrom").val()
                //+ "&DateScheduledTo=" + $("#txtFilterDateScheduledTo").val()
                //+ "&DateCompletedFrom=" + $("#txtFilterDateCompletedFrom").val()
                //+ "&DateCompletedTo=" + $("#txtFilterDateCompletedTo").val()
                //+ "&Remarks=" + $("#txtFilterRemarks").val()
                + "&DateHiredFrom=" + $("#txtFilterDateHiredFrom").val()
                + "&DateHiredTo=" + $("#txtFilterDateHiredTo").val()
                + "&DateStatusFrom=" + $("#txtFilterDateStatusFrom").val()
                + "&DateStatusTo=" + $("#txtFilterDateStatusTo").val()
                + "&BirthDateFrom=" + $("#txtFilterBirthDateFrom").val()
                + "&BirthDateTo=" + $("#txtFilterBirthDateTo").val()
                + "&MovementDateFrom=" + $("#txtFilterMovementDateFrom").val()
                + "&MovementDateTo=" + $("#txtFilterMovementDateTo").val()
                + "&OldEmployeeID=" + $("#txtFilterOldEmployeeID").val();

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadEmployeeExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckEmployeeExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },
    };

    objEmployeeListJS.Initialize();
});