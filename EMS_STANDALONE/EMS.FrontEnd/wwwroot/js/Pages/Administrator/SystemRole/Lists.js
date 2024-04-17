var objSystemRoleListJS;
const SystemRoleListURL = "/Administrator/SystemRole?handler=List";
const SystemRoleAddURL = "/Administrator/SystemRole/Add";
const SystemRoleViewURL = "/Administrator/SystemRole/View";
const SystemRoleEditURL = "/Administrator/SystemRole/Edit";
const SystemRoleDeleteURL = "/Administrator/SystemRole/Delete";
const SystemRoleAddPostURL = "/Administrator/SystemRole/Add";
const SystemRoleEditPostURL = "/Administrator/SystemRole/Edit";

const GetSystemUserDropDownURL = "/Administrator/SystemRole?handler=SystemUserDropDown";
const GetSystemUserRoleDropDownURL = "/Administrator/SystemRole?handler=SystemUserRoleDropDownByRoleID";
const GetCheckExportListURL = "/Administrator/SystemRole?handler=CheckExportList";
const DownloadExportListURL = "/Administrator/SystemRole?handler=DownloadExportList";


$(document).ready(function () {
    objSystemRoleListJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                ID: localStorage["AdministratorSystemRoleListID"],
                RoleName: localStorage["AdministratorSystemRoleListRoleName"],
                DateCreatedFrom: localStorage["AdministratorSystemRoleListDateCreatedFrom"],
                DateCreatedTo: localStorage["AdministratorSystemRoleListDateCreatedTo"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterDateCreatedFrom, #txtFilterDateCreatedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            NumberOnly($("#txtFilterSystemRoleID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterSystemRoleID").val(),
                    RoleName: $("#txtFilterSystemRoleName").val(),
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAdministratorSystemRoleList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(SystemRoleAddURL, "divSystemRoleBodyModal");
                $("#divSystemRoleModal").modal("show");
            });

            $("#btnExport").click(function () {

                var parameters = "&sidx=" + $("#tblAdministratorSystemRoleList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblAdministratorSystemRoleList").jqGrid("getGridParam", "sortorder")
                    + "&ID=" + $("#txtFilterSystemRoleID").val()
                    + "&RoleName=" + $("#txtFilterSystemRoleName").val()
                    + "&DateCreatedFrom=" + $("#txtFilterDateCreatedFrom").val()
                    + "&DateCreatedTo=" + $("#txtFilterDateCreatedTo").val()

                var GetSuccessFunction = function (data) {
                    if (data.IsSuccess == true) {
                        window.location = DownloadExportListURL + parameters;
                    }
                    else {
                        ModalAlert(MODAL_HEADER, data.Result);
                    }
                };

                objEMSCommonJS.GetAjax(GetCheckExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
            });
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblAdministratorSystemRoleList") == "" ? "" : $.parseJSON(localStorage.getItem("tblAdministratorSystemRoleList"));
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
            $("#tblAdministratorSystemRoleList").jqGrid("GridUnload");
            $("#tblAdministratorSystemRoleList").jqGrid("GridDestroy");
            $("#tblAdministratorSystemRoleList").jqGrid({
                url: SystemRoleListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Role Name", "Created Date"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objSystemRoleListJS.AddLink },
                    { name: "RoleName", index: "RoleName", editable: true, align: "left" },
                    { name: "DateCreated", index: "DateCreated", editable: true, align: "left" },
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
                    $("#tblAdministratorSystemRoleList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblAdministratorSystemRoleList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblAdministratorSystemRoleList .jqgrid-id-link").click(function () {
                            $('#divSystemRoleModal').modal('show');
                        });

                    }

                    if (localStorage["SystemRoleListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["SystemRoleListFilterOption"]));
                    }
                    objSystemRoleListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objSystemRoleListJS.ShowHideFilter();
                        localStorage["SystemRoleListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblAdministratorSystemRoleList");
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
            localStorage["AdministratorSystemRoleListID"] = $("#txtFilterSystemRoleID").val();
            localStorage["AdministratorSystemRoleListRoleName"] = $("#txtFilterSystemRoleName").val();
            localStorage["AdministratorSystemRoleListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["AdministratorSystemRoleListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterSystemRoleID").val(localStorage["AdministratorSystemRoleListID"]);
            $("#txtFilterSystemRoleName").val(localStorage["AdministratorSystemRoleListRoleName"]);
            $("#txtFilterDateCreatedFrom").val(localStorage["AdministratorSystemRoleListDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["AdministratorSystemRoleListDateCreatedTo"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + SystemRoleViewURL + "?ID=" + rowObject.ID + "', 'divSystemRoleBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID)+"</a>"; 
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
    
     objSystemRoleListJS.Initialize();
});