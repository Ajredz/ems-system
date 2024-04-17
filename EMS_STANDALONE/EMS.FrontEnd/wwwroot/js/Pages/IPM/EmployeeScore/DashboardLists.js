var objDashboardListJS;
const DashboardListURL = "/IPM/EmployeeScore?handler=DashboardList";

$(document).ready(function () {
    objDashboardListJS = {
        Initialize: function () {
            var s = this;
            s.ElementBinding();
            s.GetLocalStorage2();

            $("#ddlDashboard").change();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSearch2").click(function () {
                objDashboardListJS.LoadDashboardData($("#ddlDashboard").val());
            });

            $("#btnReset2").click(function () {
                $("div.filterFields2 input").val("");
                $("div.filterFields2 select").val("");
                $("#btnSearch2").click();
            });

            $("#ddlDashboard").change(function () {
                //objEmployeeDashboardListJS.LoadReportData($(this).val());
                if ($(this).val() == "POSITION") {
                    var param = {
                        'DashboardType': $("#ddlDashboard").val(),
                        'Dashboard1Input.Position': $("#txtFilterDashboardField").val(),
                        'Dashboard1Input.CountMin': $("#txtFilterDashboardCountMin").val(),
                        'Dashboard1Input.CountMax': $("#txtFilterDashboardCountMax").val()
                    };
                    s.SetLocalStorage2();
                    ResetJQGridState("tblIPMDashboardCountList");
                    s.LoadJQGridDashboard1(param);
                }
                else if ($(this).val() == "AREA") {
                    var param = {
                        'DashboardType': $("#ddlDashboard").val(),
                        'Dashboard2Input.Area': $("#txtFilterDashboardField").val(),
                        'Dashboard2Input.CountMin': $("#txtFilterDashboardCountMin").val(),
                        'Dashboard2Input.CountMax': $("#txtFilterDashboardCountMax").val()
                    };
                    s.SetLocalStorage2();
                    ResetJQGridState("tblIPMDashboardCountList");
                    s.LoadJQGridDashboard2(param);
                }
                else if ($(this).val() == "REGION") {
                    var param = {
                        'DashboardType': $("#ddlDashboard").val(),
                        'Dashboard3Input.Region': $("#txtFilterDashboardField").val(),
                        'Dashboard3Input.CountMin': $("#txtFilterDashboardCountMin").val(),
                        'Dashboard3Input.CountMax': $("#txtFilterDashboardCountMax").val()
                    };
                    s.SetLocalStorage2();
                    ResetJQGridState("tblIPMDashboardCountList");
                    s.LoadJQGridDashboard3(param);
                }
            });
        },

        LoadDashboardData: function (type) {
            var s = this;
            if (type == "POSITION") {
                var param = {
                    'DashboardType': $("#ddlDashboard").val(),
                    'Dashboard1Input.Position': $("#txtFilterDashboardField").val(),
                    'Dashboard1Input.CountMin': $("#txtFilterDashboardCountMin").val(),
                    'Dashboard1Input.CountMax': $("#txtFilterDashboardCountMax").val()
                };
                s.SetLocalStorage2();
                ResetJQGridState("tblIPMDashboardCountList");
                s.LoadJQGridDashboard1(param);
            }
            else if (type == "AREA") {
                var param = {
                    'DashboardType': $("#ddlDashboard").val(),
                    'Dashboard2Input.Area': $("#txtFilterDashboardField").val(),
                    'Dashboard2Input.CountMin': $("#txtFilterDashboardCountMin").val(),
                    'Dashboard2Input.CountMax': $("#txtFilterDashboardCountMax").val()
                };
                s.SetLocalStorage2();
                ResetJQGridState("tblIPMDashboardCountList");
                s.LoadJQGridDashboard2(param);
            }
            else if (type == "REGION") {
                var param = {
                    'DashboardType': $("#ddlDashboard").val(),
                    'Dashboard3Input.Region': $("#txtFilterDashboardField").val(),
                    'Dashboard3Input.CountMin': $("#txtFilterDashboardCountMin").val(),
                    'Dashboard3Input.CountMax': $("#txtFilterDashboardCountMax").val()
                };
                s.SetLocalStorage2();
                ResetJQGridState("tblIPMDashboardCountList");
                s.LoadJQGridDashboard3(param);
            }
        },

        LoadJQGridDashboard1: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            NumberOnlyWithNegative($("#txtDashboard2CountMin, #txtDashboard2CountMax"));
            var moveFilterFields2 = function () {
                $("#tblIPMDashboardCountList").find("tr:first").addClass("dashboardrow");
                $(".dashboardrow").removeClass("jqgfirstrow");

                var intialHeight = $(".dashboardrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#tblIPMDashboardCountList .dashboardrow td .filterFields2").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer2");
                });
                $(".dashboardrow").css({ "height": intialHeight + "px" });

                $("div.filterFields2").unbind("keyup");
                $("div.filterFields2").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch2").click();
                    }
                });
            }
            moveFilterFields2();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Position", "Count"],
                colModel: [
                    { name: "Position", index: "Position", editable: true, align: "center", width: "100" },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100" },
                ],
                toppager: $("#divPager2"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Dashboard1Output",
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
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        //Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblPlantillaDashboardList", data);

                        //Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer2 .filterFields2").each(function (n, element) {
                            $(this).appendTo(".dashboardrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["EmployeeScoreDashboardListFilterOption"] != undefined) {
                        $("#chkFilter2").prop('checked', JSON.parse(localStorage["EmployeeScoreDashboardListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter2();

                    $("#chkFilter2").on('change', function () {
                        objDashboardListJS.ShowHideFilter2();
                        localStorage["EmployeeScoreDashboardListFilterOption"] = $("#chkFilter2").is(":checked");
                    });
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields2();
                },
            }).navGrid("#divPager2",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#tblIPMDashboardCountList_toppager_custom_block_right > .ui-pg-selbox").addClass("dashboardSelect");
            jQuery(".ui-pg-selbox.dashboardSelect").closest("select").before("<label class=\"ui-row-label label2\">Show</label>");
            jQuery(".ui-row-label.label2").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter2\">Show Filter</label>");
            jQuery("#lblFilter2").after("<input type=\"checkbox\" id=\"chkFilter2\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridDashboard2: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            NumberOnlyWithNegative($("#txtDashboard2CountMin, #txtDashboard2CountMax"));
            var moveFilterFields2 = function () {
                $("#tblIPMDashboardCountList").find("tr:first").addClass("dashboardrow");
                $(".dashboardrow").removeClass("jqgfirstrow");

                var intialHeight = $(".dashboardrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#tblIPMDashboardCountList .dashboardrow td .filterFields2").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer2");
                });
                $(".dashboardrow").css({ "height": intialHeight + "px" });

                $("div.filterFields2").unbind("keyup");
                $("div.filterFields2").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch2").click();
                    }
                });
            }
            moveFilterFields2();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Area", "Count"],
                colModel: [
                    { name: "Area", index: "Area", editable: true, align: "center", width: "100" },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100" },
                ],
                toppager: $("#divPager2"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Dashboard2Output",
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
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        //Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblPlantillaDashboardList", data);

                        //Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer2 .filterFields2").each(function (n, element) {
                            $(this).appendTo(".dashboardrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["EmployeeScoreDashboardListFilterOption"] != undefined) {
                        $("#chkFilter2").prop('checked', JSON.parse(localStorage["EmployeeScoreDashboardListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter2();

                    $("#chkFilter2").on('change', function () {
                        objDashboardListJS.ShowHideFilter2();
                        localStorage["EmployeeScoreDashboardListFilterOption"] = $("#chkFilter2").is(":checked");
                    });
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields2();
                },
            }).navGrid("#divPager2",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#tblIPMDashboardCountList_toppager_custom_block_right > .ui-pg-selbox").addClass("dashboardSelect");
            jQuery(".ui-pg-selbox.dashboardSelect").closest("select").before("<label class=\"ui-row-label label2\">Show</label>");
            jQuery(".ui-row-label.label2").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter2\">Show Filter</label>");
            jQuery("#lblFilter2").after("<input type=\"checkbox\" id=\"chkFilter2\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridDashboard3: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            NumberOnlyWithNegative($("#txtDashboard3CountMin, #txtDashboard3CountMax"));
            var moveFilterFields2 = function () {
                $("#tblIPMDashboardCountList").find("tr:first").addClass("dashboardrow");
                $(".dashboardrow").removeClass("jqgfirstrow");

                var intialHeight = $(".dashboardrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#tblIPMDashboardCountList .dashboardrow td .filterFields2").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer2");
                });
                $(".dashboardrow").css({ "height": intialHeight + "px" });

                $("div.filterFields2").unbind("keyup");
                $("div.filterFields2").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch2").click();
                    }
                });
            }
            moveFilterFields2();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Region", "Count"],
                colModel: [
                    { name: "Region", index: "Region", editable: true, align: "center", width: "100" },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100" },
                ],
                toppager: $("#divPager2"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Dashboard3Output",
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
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        //Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblPlantillaDashboardList", data);

                        //Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer2 .filterFields2").each(function (n, element) {
                            $(this).appendTo(".dashboardrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["EmployeeScoreDashboardListFilterOption"] != undefined) {
                        $("#chkFilter2").prop('checked', JSON.parse(localStorage["EmployeeScoreDashboardListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter2();

                    $("#chkFilter2").on('change', function () {
                        objDashboardListJS.ShowHideFilter2();
                        localStorage["EmployeeScoreDashboardListFilterOption"] = $("#chkFilter2").is(":checked");
                    });
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields2();
                },
            }).navGrid("#divPager2",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#tblIPMDashboardCountList_toppager_custom_block_right > .ui-pg-selbox").addClass("dashboardSelect");
            jQuery(".ui-pg-selbox.dashboardSelect").closest("select").before("<label class=\"ui-row-label label2\">Show</label>");
            jQuery(".ui-row-label.label2").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter2\">Show Filter</label>");
            jQuery("#lblFilter2").after("<input type=\"checkbox\" id=\"chkFilter2\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage2: function () {
            localStorage["DashboardCountListType"] = $("#ddlDashboard").val();
        },

        GetLocalStorage2: function () {
            $("#ddlDashboard").val(localStorage["DashboardCountListType"]);
        },

        ShowHideFilter2: function () {
            if ($("#chkFilter2").is(":checked")) {
                $(".dashboardrow .filterFields2").show();
            }
            else if ($("#chkFilter2").is(":not(:checked)")) {
                $(".dashboardrow .filterFields2").hide();
            }
        }
    };

    objDashboardListJS.Initialize();
});