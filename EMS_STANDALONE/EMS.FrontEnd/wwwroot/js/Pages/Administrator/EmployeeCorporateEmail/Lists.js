var objCorporateEmailListJS;

const EmployeeListURL = "/Administrator/EmployeeCorporateEmail?handler=List";
const EmployeeViewURL = "/Administrator/EmployeeCorporateEmail/View";
const EmployeeEditURL = "/Administrator/EmployeeCorporateEmail/Edit";
const GetCheckEmployeeExportListURL = "/Administrator/EmployeeCorporateEmail?handler=CheckEmployeeExportList";
const DownloadEmployeeExportListURL = "/Administrator/EmployeeCorporateEmail?handler=DownloadEmployeeExportList";

const EmploymentStatusDropDownURL = "/Administrator/EmployeeCorporateEmail?handler=ReferenceValue&RefCode=EMPLOYMENT_STATUS";
const OrgGroupAutoCompleteURL = "/Plantilla/Employee?handler=OrgTypeAutoComplete";
const PositionAutoCompleteURL = "/Plantilla/Employee?handler=PositionAutoComplete";

const OrgTypeHierarchyUpwardURL = "/Administrator/EmployeeCorporateEmail?handler=OrgGroupHierarchy";

$(document).ready(function () {
    objCorporateEmailListJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                ID: localStorage["AdministratorCorporateEmailListID"],
                Code: localStorage["AdministratorCorporateEmailListCode"],
                Name: localStorage["AdministratorCorporateEmailListName"],
                CorporateEmail: localStorage["AdministratorCorporateEmailListCorporateEmail"],
                OfficeMobile: localStorage["AdministratorCorporateEmailListOfficeMobile"],
                OrgGroupDelimited: localStorage["AdministratorCorporateEmailListOrgGroupDelimited"],
                PositionDelimited: localStorage["AdministratorCorporateEmailListPositionDelimited"],
                EmploymentStatusDelimited: localStorage["AdministratorCorporateEmailListEmploymentStatusDelimited"],
                OldEmployeeID: localStorage["AdministratorCorporateEmailListOldEmployeeID"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterEmployeeID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterEmployeeID").val(),
                    Code: $("#txtFilterCode").val(),
                    Name: $("#txtFilterName").val(),
                    CorporateEmail: $("#txtFilterCorporateEmail").val(),
                    OfficeMobile: $("#txtFilterOfficeMobile").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    EmploymentStatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value,
                    OldEmployeeID: $("#txtFilterOldEmployeeID").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAdministratorCorporateEmailList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnExport").click(function () {
                var parameters = "&sidx=" + $("#tblAdministratorCorporateEmailList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblAdministratorCorporateEmailList").jqGrid("getGridParam", "sortorder")
                    + "&ID=" + $("#txtFilterEmployeeID").val()
                    + "&Code=" + $("#txtFilterCode").val()
                    + "&Name=" + $("#txtFilterName").val()
                    + "&CorporateEmail=" + $("#txtFilterCorporateEmail").val()
                    + "&OfficeMobile=" + $("#txtFilterOfficeMobile").val()
                    + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                    + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                    + "&EmploymentStatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value
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
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedEmploymentStatus", EmploymentStatusDropDownURL);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , PositionAutoCompleteURL, 20, "multiSelectedPosition");
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblAdministratorCorporateEmailList") == "" ? "" : $.parseJSON(localStorage.getItem("tblAdministratorCorporateEmailList"));
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
            $("#tblAdministratorCorporateEmailList").jqGrid("GridUnload");
            $("#tblAdministratorCorporateEmailList").jqGrid("GridDestroy");
            $("#tblAdministratorCorporateEmailList").jqGrid({
                url: EmployeeListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["","ID", "New Employee ID", "Full Name", "Corporate Email", "Mobile Number" , "Org Group" , "Position" , "Employment Status" ,"Show In Corporate Directory", "Old Employee ID"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objCorporateEmailListJS.AddLink },
                    { name: "Code", index: "Code", editable: true, align: "left", sortable: true },
                    { name: "Name", index: "Name", editable: true, align: "left", sortable: true },
                    { name: "CorporateEmail", index: "CorporateEmail", editable: true, align: "left", sortable: true },
                    { name: "OfficeMobile", index: "OfficeMobile", editable: true, align: "left", sortable: true },
                    { name: "OrgGroup", index: "OrgGroup", editable: true, align: "left", sortable: true },
                    { name: "Position", index: "Position", editable: true, align: "left", sortable: true },
                    { name: "EmploymentStatus", index: "EmploymentStatus", editable: true, align: "left", sortable: true },
                    { name: "IsDisplayDirectory", index: "IsDisplayDirectory", editable: true, align: "left", sortable: true },
                    { name: "OldEmployeeID", index: "OldEmployeeID", editable: true, align: "left", sortable: true },
                ],
                toppager: $("#divPager"),
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
                    $("#tblAdministratorCorporateEmailList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                                
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblAdministratorCorporateEmailList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblAdministratorCorporateEmailList .jqgrid-id-link").click(function () {
                            $('#divEmployeeModal').modal('show');
                        });

                    }

                    if (localStorage["SystemUserListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["SystemUserListFilterOption"]));
                    }
                    objCorporateEmailListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objCorporateEmailListJS.ShowHideFilter();
                        localStorage["SystemUserListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblAdministratorCorporateEmailList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage: function () {
            localStorage["AdministratorCorporateEmailListID"] = $("#txtFilterEmployeeID").val();
            localStorage["AdministratorCorporateEmailListCode"] = $("#txtFilterCode").val();
            localStorage["AdministratorCorporateEmailListName"] = $("#txtFilterName").val();
            localStorage["AdministratorCorporateEmailListCorporateEmail"] = $("#txtFilterCorporateEmail").val();
            localStorage["AdministratorCorporateEmailListOfficeMobile"] = $("#txtFilterOfficeMobile").val();
            localStorage["AdministratorCorporateEmailListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["AdministratorCorporateEmailListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;
            localStorage["AdministratorCorporateEmailListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["AdministratorCorporateEmailListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;
            localStorage["AdministratorCorporateEmailListEmploymentStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value;
            localStorage["AdministratorCorporateEmailListEmploymentStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").text;
            localStorage["AdministratorCorporateEmailListOldEmployeeID"] = $("#txtFilterOldEmployeeID").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterEmployeeID").val(localStorage["AdministratorCorporateEmailListID"]);
            $("#txtFilterCode").val(localStorage["AdministratorCorporateEmailListCode"]);
            $("#txtFilterName").val(localStorage["AdministratorCorporateEmailListName"]);
            $("#txtFilterName").val(localStorage["AdministratorCorporateEmailListCorporateEmail"]);
            $("#txtFilterName").val(localStorage["AdministratorCorporateEmailListOfficeMobile"]);
            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "AdministratorCorporateEmailListOrgGroupDelimited"
                , "AdministratorCorporateEmailListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "AdministratorCorporateEmailListPositionDelimited"
                , "AdministratorCorporateEmailListPositionDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedEmploymentStatus"
                , "AdministratorCorporateEmailListEmploymentStatusDelimited"
                , "AdministratorCorporateEmailListEmploymentStatusDelimitedText");

            $("#txtFilterOldEmployeeID").val(localStorage["AdministratorCorporateEmailListOldEmployeeID"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + EmployeeViewURL + "?ID=" + rowObject.ID + "', 'divEmployeeBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        }
    };
    
     objCorporateEmailListJS.Initialize();
});