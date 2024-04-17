var objEmployeeDashboardListJS;
const DashboardListURL = "/Plantilla/Dashboard?handler=List";
const GetCheckEmployeeDashboardURL = "/Plantilla/Dashboard?handler=CheckEmployeeDashboard";
const DownloadEmployeeDashboardURL = "/Plantilla/Dashboard?handler=DownloadEmployeeDashboard";

$(document).ready(function () {
    objEmployeeDashboardListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            s.GetLocalStorage();
            $("#ddlReportType").change();
            //s.LoadReportData(localStorage["DashboardListReportType"]);

            
        },

        ElementBinding: function () {
            var s = this;

            NumberOnlyWithNegative($("#txtReport1CountMin, #txtReport1CountMax, #txtReport2CountMin, #txtReport2CountMax"));

            $("#btnSearch").click(function () {
                objEmployeeDashboardListJS.LoadReportData($("#ddlReportType").val());
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("#btnSearch").click();
            });

            $("#ddlReportType").change(function () {
                //objEmployeeDashboardListJS.LoadReportData($(this).val());
                if ($(this).val() == "PRO_EMP_PER_GROUP") {
                    var param = {
                        'ReportType': $("#ddlReportType").val(),
                        'Report1Input.OrgGroup': $("#txtReport1OrgGroup").val(),
                        'Report1Input.CountMin': $("#txtReport1CountMin").val(),
                        'Report1Input.CountMax': $("#txtReport1CountMax").val(),
                        'Report1Input.IsExport': false
                    };
                    s.SetLocalStorage();
                    ResetJQGridState("tblPlantillaDashboardList");
                    s.LoadJQGridReport1(param);
                }
                else if ($(this).val() == "PRO_EMP_PER_POSITION") {
                    $("#filterFieldsContainer").html("");
                    $(".jqgfirstrow td").html("");
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + "   <input type=\"search\" placeholder=\"Search..\" id=\"txtReport2Position\" maxlength=\"70\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtReport2CountMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtReport2CountMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );
                    var param = {
                        'ReportType': $("#ddlReportType").val(),
                        'Report2Input.Position': $("#txtReport2Position").val(),
                        'Report2Input.CountMin': $("#txtReport2CountMin").val(),
                        'Report2Input.CountMax': $("#txtReport2CountMax").val(),
                        'Report2Input.IsExport': false
                    };
                    s.SetLocalStorage();
                    ResetJQGridState("tblPlantillaDashboardList");
                    s.LoadJQGridReport2(param);
                }
                else if ($(this).val() == "PRO_STATUS_BEYOND_6") {
                    $("#filterFieldsContainer").html("");
                    $(".jqgfirstrow td").html("");
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + "   <input type=\"search\" placeholder=\"Search..\" id=\"txtReport3Status\" maxlength=\"70\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtReport3CountMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtReport3CountMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );
                    var param = {
                        'ReportType': $("#ddlReportType").val(),
                        'Report3Input.Status': $("#txtReport3Status").val(),
                        'Report3Input.CountMin': $("#txtReport3CountMin").val(),
                        'Report3Input.CountMax': $("#txtReport3CountMax").val(),
                        'Report3Input.IsExport': false
                    };
                    s.SetLocalStorage();
                    ResetJQGridState("tblPlantillaDashboardList");
                    s.LoadJQGridReport3(param);
                }
                else if ($(this).val() == "PRO_STATUS_EXPIRING") {
                    $("#filterFieldsContainer").html("");
                    $(".jqgfirstrow td").html("");
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + "   <input type=\"search\" placeholder=\"Search..\" id=\"txtReport4Status\" maxlength=\"70\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtReport4CountMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtReport4CountMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );
                    var param = {
                        'ReportType': $("#ddlReportType").val(),
                        'Report4Input.Status': $("#txtReport4Status").val(),
                        'Report4Input.CountMin': $("#txtReport4CountMin").val(),
                        'Report4Input.CountMax': $("#txtReport4CountMax").val(),
                        'Report4Input.IsExport': false
                    };
                    s.SetLocalStorage();
                    ResetJQGridState("tblPlantillaDashboardList");
                    s.LoadJQGridReport4(param);
                }
                else if ($(this).val() == "BIRTHDAY_PER_MONTH") {
                    $("#filterFieldsContainer").html("");
                    $(".jqgfirstrow td").html("");
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + "   <input type=\"search\" placeholder=\"Search..\" id=\"txtReport5Month\" maxlength=\"70\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtReport5CountMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtReport5CountMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );
                    var param = {
                        'ReportType': $("#ddlReportType").val(),
                        'Report5Input.Month': $("#txtReport5Month").val(),
                        'Report5Input.CountMin': $("#txtReport5CountMin").val(),
                        'Report5Input.CountMax': $("#txtReport5CountMax").val(),
                        'Report5Input.IsExport': false
                    };
                    s.SetLocalStorage();
                    ResetJQGridState("tblPlantillaDashboardList");
                    s.LoadJQGridReport5(param);
                }
                else if ($(this).val() == "ACT_EMP_PER_GROUP") {
                    $("#filterFieldsContainer").html("");
                    $(".jqgfirstrow td").html("");
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + "   <input type=\"search\" placeholder=\"Search..\" id=\"txtReport6OrgGroup\" maxlength=\"70\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtReport6CountMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtReport6CountMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );
                    var param = {
                        'ReportType': $("#ddlReportType").val(),
                        'Report6Input.OrgGroup': $("#txtReport6OrgGroup").val(),
                        'Report6Input.CountMin': $("#txtReport6CountMin").val(),
                        'Report6Input.CountMax': $("#txtReport6CountMax").val(),
                        'Report6Input.IsExport': false
                    };
                    s.SetLocalStorage();
                    ResetJQGridState("tblPlantillaDashboardList");
                    s.LoadJQGridReport6(param);
                }
                else if ($(this).val() == "ACT_EMP_PER_POSITION") {
                    $("#filterFieldsContainer").html("");
                    $(".jqgfirstrow td").html("");
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + "   <input type=\"search\" placeholder=\"Search..\" id=\"txtReport7Position\" maxlength=\"70\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtReport7CountMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtReport7CountMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );
                    var param = {
                        'ReportType': $("#ddlReportType").val(),
                        'Report7Input.Position': $("#txtReport7Position").val(),
                        'Report7Input.CountMin': $("#txtReport7CountMin").val(),
                        'Report7Input.CountMax': $("#txtReport7CountMax").val(),
                        'Report7Input.IsExport': false
                    };
                    s.SetLocalStorage();
                    ResetJQGridState("tblPlantillaDashboardList");
                    s.LoadJQGridReport7(param);
                }
                else if ($(this).val() == "INACTIVE_EMPLOYEE") {
                    $("#filterFieldsContainer").html("");
                    $(".jqgfirstrow td").html("");
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + "   <input type=\"search\" placeholder=\"Search..\" id=\"txtReport8Status\" maxlength=\"70\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtReport8CountMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtReport8CountMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );
                    var param = {
                        'ReportType': $("#ddlReportType").val(),
                        'Report8Input.Status': $("#txtReport8Status").val(),
                        'Report8Input.CountMin': $("#txtReport8CountMin").val(),
                        'Report8Input.CountMax': $("#txtReport8CountMax").val(),
                        'Report8Input.IsExport': false
                    };
                    s.SetLocalStorage();
                    ResetJQGridState("tblPlantillaDashboardList");
                    s.LoadJQGridReport8(param);
                }
            });


            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeDashboardListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {
            $("#divPlantillaDashboardErrorMessage").html("");
            $("#ddlReportType").removeClass("errMessage");

            var parameter1 = "&sidx=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortorder")
                + "&ReportType=" + $("#ddlReportType").val()
                + "&Report1Input.OrgGroup=" + $("#txtReport1OrgGroup").val()
                + "&Report1Input.CountMin=" + $("#txtReport1CountMin").val()
                + "&Report1Input.CountMax=" + $("#txtReport1CountMax").val();

            var parameter2 = "&sidx=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortorder")
                + "&ReportType=" + $("#ddlReportType").val()
                + "&Report2Input.Position=" + $("#txtReport2Position").val()
                + "&Report2Input.CountMin=" + $("#txtReport2CountMin").val()
                + "&Report2Input.CountMax=" + $("#txtReport2CountMax").val();

            var parameter3 = "&sidx=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortorder")
                + "&ReportType=" + $("#ddlReportType").val()
                + "&Report3Input.Status=" + $("#txtReport3Status").val()
                + "&Report3Input.CountMin=" + $("#txtReport3CountMin").val()
                + "&Report3Input.CountMax=" + $("#txtReport3CountMax").val();

            var parameter4 = "&sidx=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortorder")
                + "&ReportType=" + $("#ddlReportType").val()
                + "&Report4Input.Status=" + $("#txtReport4Status").val()
                + "&Report4Input.CountMin=" + $("#txtReport4CountMin").val()
                + "&Report4Input.CountMax=" + $("#txtReport4CountMax").val();

            var parameter5 = "&sidx=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortorder")
                + "&ReportType=" + $("#ddlReportType").val()
                + "&Report5Input.Month=" + $("#txtReport5Month").val()
                + "&Report5Input.CountMin=" + $("#txtReport5CountMin").val()
                + "&Report5Input.CountMax=" + $("#txtReport5CountMax").val();

            var parameter6 = "&sidx=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortorder")
                + "&ReportType=" + $("#ddlReportType").val()
                + "&Report6Input.OrgGroup=" + $("#txtReport6OrgGroup").val()
                + "&Report6Input.CountMin=" + $("#txtReport6CountMin").val()
                + "&Report6Input.CountMax=" + $("#txtReport6CountMax").val();

            var parameter7 = "&sidx=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortorder")
                + "&ReportType=" + $("#ddlReportType").val()
                + "&Report7Input.Position=" + $("#txtReport7Position").val()
                + "&Report7Input.CountMin=" + $("#txtReport7CountMin").val()
                + "&Report7Input.CountMax=" + $("#txtReport7CountMax").val();

            var parameter8 = "&sidx=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaDashboardList").jqGrid("getGridParam", "sortorder")
                + "&ReportType=" + $("#ddlReportType").val()
                + "&Report8Input.Status=" + $("#txtReport8Status").val()
                + "&Report8Input.CountMin=" + $("#txtReport8CountMin").val()
                + "&Report8Input.CountMax=" + $("#txtReport8CountMax").val();

            if (($("#ddlReportType").val() || "") != "") {
                var GetSuccessFunction = function (data) {
                    if (data.IsSuccess) {
                        if ($("#ddlReportType").val() == "PRO_EMP_PER_GROUP") {
                            window.location = DownloadEmployeeDashboardURL + parameter1;
                        }
                        else if ($("#ddlReportType").val() == "PRO_EMP_PER_POSITION") {
                            window.location = DownloadEmployeeDashboardURL + parameter2;
                        }
                        else if ($("#ddlReportType").val() == "PRO_STATUS_BEYOND_6") {
                            window.location = DownloadEmployeeDashboardURL + parameter3;
                        }
                        else if ($("#ddlReportType").val() == "PRO_STATUS_EXPIRING") {
                            window.location = DownloadEmployeeDashboardURL + parameter4;
                        }
                        else if ($("#ddlReportType").val() == "BIRTHDAY_PER_MONTH") {
                            window.location = DownloadEmployeeDashboardURL + parameter5;
                        }
                        else if ($("#ddlReportType").val() == "ACT_EMP_PER_GROUP") {
                            window.location = DownloadEmployeeDashboardURL + parameter6;
                        }
                        else if ($("#ddlReportType").val() == "ACT_EMP_PER_POSITION") {
                            window.location = DownloadEmployeeDashboardURL + parameter7;
                        }
                        else if ($("#ddlReportType").val() == "INACTIVE_EMPLOYEE") {
                            window.location = DownloadEmployeeDashboardURL + parameter8;
                        }
                        $("#divModal").modal("hide");
                    }
                    else {
                        ModalAlert(MODAL_HEADER, data.Result);
                    }
                };
                if ($("#ddlReportType").val() == "PRO_EMP_PER_GROUP") {
                    objEMSCommonJS.GetAjax(GetCheckEmployeeDashboardURL + parameter1, {}, "#btnExport", GetSuccessFunction, null, true);
                }
                else if ($("#ddlReportType").val() == "PRO_EMP_PER_POSITION") {
                    objEMSCommonJS.GetAjax(GetCheckEmployeeDashboardURL + parameter2, {}, "#btnExport", GetSuccessFunction, null, true);
                }
                else if ($("#ddlReportType").val() == "PRO_STATUS_BEYOND_6") {
                    objEMSCommonJS.GetAjax(GetCheckEmployeeDashboardURL + parameter3, {}, "#btnExport", GetSuccessFunction, null, true);
                }
                else if ($("#ddlReportType").val() == "PRO_STATUS_EXPIRING") {
                    objEMSCommonJS.GetAjax(GetCheckEmployeeDashboardURL + parameter4, {}, "#btnExport", GetSuccessFunction, null, true);
                }
                else if ($("#ddlReportType").val() == "BIRTHDAY_PER_MONTH") {
                    objEMSCommonJS.GetAjax(GetCheckEmployeeDashboardURL + parameter5, {}, "#btnExport", GetSuccessFunction, null, true);
                }
                else if ($("#ddlReportType").val() == "ACT_EMP_PER_GROUP") {
                    objEMSCommonJS.GetAjax(GetCheckEmployeeDashboardURL + parameter6, {}, "#btnExport", GetSuccessFunction, null, true);
                }
                else if ($("#ddlReportType").val() == "ACT_EMP_PER_POSITION") {
                    objEMSCommonJS.GetAjax(GetCheckEmployeeDashboardURL + parameter7, {}, "#btnExport", GetSuccessFunction, null, true);
                }
                else if ($("#ddlReportType").val() == "INACTIVE_EMPLOYEE") {
                    objEMSCommonJS.GetAjax(GetCheckEmployeeDashboardURL + parameter8, {}, "#btnExport", GetSuccessFunction, null, true);
                }
            }
            else {
                $("#divPlantillaDashboardErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                $("#ddlReportType").addClass("errMessage");
            }
        },

        LoadJQGridReport1: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaDashboardList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaDashboardList"));
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
            $("#tblPlantillaDashboardList").jqGrid("GridUnload");
            $("#tblPlantillaDashboardList").jqGrid("GridDestroy");
            $("#tblPlantillaDashboardList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Org Group", "Count"],
                colModel: [
                    { name: "OrgGroup", index: "OrgGroup", editable: true, align: "left", width: "100", sortable: true },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Report1Output",
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
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+1)+")");
                        });
                    }

                    if (localStorage["PlantillaDashboardListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PlantillaDashboardListFilterOption"]));
                    }
                    objEmployeeDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objEmployeeDashboardListJS.ShowHideFilter();
                        localStorage["PlantillaDashboardListFilterOption"] = $("#chkFilter").is(":checked");
                    });

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
                    GetJQGridState("tblPlantillaDashboardList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridReport2: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaDashboardList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaDashboardList"));
            NumberOnlyWithNegative($("#txtReport2CountMin, #txtReport2CountMax"));
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
            $("#tblPlantillaDashboardList").jqGrid("GridUnload");
            $("#tblPlantillaDashboardList").jqGrid("GridDestroy");
            $("#tblPlantillaDashboardList").jqGrid({
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
                    { name: "Position", index: "Position", editable: true, align: "left", width: "100", sortable: true },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Report2Output",
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
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["PlantillaDashboardListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PlantillaDashboardListFilterOption"]));
                    }
                    objEmployeeDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objEmployeeDashboardListJS.ShowHideFilter();
                        localStorage["PlantillaDashboardListFilterOption"] = $("#chkFilter").is(":checked");
                    });

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
                    GetJQGridState("tblPlantillaDashboardList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridReport3: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaDashboardList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaDashboardList"));
            NumberOnlyWithNegative($("#txtReport3CountMin, #txtReport3CountMax"));
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
            $("#tblPlantillaDashboardList").jqGrid("GridUnload");
            $("#tblPlantillaDashboardList").jqGrid("GridDestroy");
            $("#tblPlantillaDashboardList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Status", "Count"],
                colModel: [
                    { name: "Status", index: "Status", editable: true, align: "left", width: "100", sortable: true },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Report3Output",
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
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["PlantillaDashboardListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PlantillaDashboardListFilterOption"]));
                    }
                    objEmployeeDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objEmployeeDashboardListJS.ShowHideFilter();
                        localStorage["PlantillaDashboardListFilterOption"] = $("#chkFilter").is(":checked");
                    });

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
                    GetJQGridState("tblPlantillaDashboardList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridReport4: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaDashboardList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaDashboardList"));
            NumberOnlyWithNegative($("#txtReport4CountMin, #txtReport4CountMax"));
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
            $("#tblPlantillaDashboardList").jqGrid("GridUnload");
            $("#tblPlantillaDashboardList").jqGrid("GridDestroy");
            $("#tblPlantillaDashboardList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Status", "Count"],
                colModel: [
                    { name: "Status", index: "Status", editable: true, align: "left", width: "100", sortable: true },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Report4Output",
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
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["PlantillaDashboardListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PlantillaDashboardListFilterOption"]));
                    }
                    objEmployeeDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objEmployeeDashboardListJS.ShowHideFilter();
                        localStorage["PlantillaDashboardListFilterOption"] = $("#chkFilter").is(":checked");
                    });

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
                    GetJQGridState("tblPlantillaDashboardList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridReport5: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaDashboardList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaDashboardList"));
            NumberOnlyWithNegative($("#txtReport5CountMin, #txtReport5CountMax"));
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
            $("#tblPlantillaDashboardList").jqGrid("GridUnload");
            $("#tblPlantillaDashboardList").jqGrid("GridDestroy");
            $("#tblPlantillaDashboardList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Month", "Count"],
                colModel: [
                    { name: "Month", index: "Month", editable: true, align: "left", width: "100", sortable: true },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Report5Output",
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
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["PlantillaDashboardListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PlantillaDashboardListFilterOption"]));
                    }
                    objEmployeeDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objEmployeeDashboardListJS.ShowHideFilter();
                        localStorage["PlantillaDashboardListFilterOption"] = $("#chkFilter").is(":checked");
                    });

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
                    GetJQGridState("tblPlantillaDashboardList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridReport6: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaDashboardList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaDashboardList"));
            NumberOnlyWithNegative($("#txtReport6CountMin, #txtReport6CountMax"));
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
            $("#tblPlantillaDashboardList").jqGrid("GridUnload");
            $("#tblPlantillaDashboardList").jqGrid("GridDestroy");
            $("#tblPlantillaDashboardList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Org Group", "Count"],
                colModel: [
                    { name: "OrgGroup", index: "OrgGroup", editable: true, align: "left", width: "100", sortable: true },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Report6Output",
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
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["PlantillaDashboardListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PlantillaDashboardListFilterOption"]));
                    }
                    objEmployeeDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objEmployeeDashboardListJS.ShowHideFilter();
                        localStorage["PlantillaDashboardListFilterOption"] = $("#chkFilter").is(":checked");
                    });

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
                    GetJQGridState("tblPlantillaDashboardList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridReport7: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaDashboardList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaDashboardList"));
            NumberOnlyWithNegative($("#txtReport7CountMin, #txtReport7CountMax"));
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
            $("#tblPlantillaDashboardList").jqGrid("GridUnload");
            $("#tblPlantillaDashboardList").jqGrid("GridDestroy");
            $("#tblPlantillaDashboardList").jqGrid({
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
                    { name: "Position", index: "Position", editable: true, align: "left", width: "100", sortable: true },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Report7Output",
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
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["PlantillaDashboardListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PlantillaDashboardListFilterOption"]));
                    }
                    objEmployeeDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objEmployeeDashboardListJS.ShowHideFilter();
                        localStorage["PlantillaDashboardListFilterOption"] = $("#chkFilter").is(":checked");
                    });

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
                    GetJQGridState("tblPlantillaDashboardList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridReport8: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaDashboardList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaDashboardList"));
            NumberOnlyWithNegative($("#txtReport7CountMin, #txtReport7CountMax"));
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
            $("#tblPlantillaDashboardList").jqGrid("GridUnload");
            $("#tblPlantillaDashboardList").jqGrid("GridDestroy");
            $("#tblPlantillaDashboardList").jqGrid({
                url: DashboardListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Status", "Count"],
                colModel: [
                    { name: "Status", index: "Status", editable: true, align: "left", width: "100", sortable: true },
                    { name: "Count", index: "Count", editable: true, align: "center", width: "100", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows.Report8Output",
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
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["PlantillaDashboardListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PlantillaDashboardListFilterOption"]));
                    }
                    objEmployeeDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objEmployeeDashboardListJS.ShowHideFilter();
                        localStorage["PlantillaDashboardListFilterOption"] = $("#chkFilter").is(":checked");
                    });

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
                    GetJQGridState("tblPlantillaDashboardList");
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
            localStorage["DashboardListReportType"] = $("#ddlReportType").val();
        },

        GetLocalStorage: function () {
            $("#ddlReportType").val(localStorage["DashboardListReportType"]);
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },

        LoadReportData: function (type) {
            var s = this;
            if (type == "PRO_EMP_PER_GROUP") {
                var param = {
                    'ReportType': $("#ddlReportType").val(),
                    'Report1Input.OrgGroup': $("#txtReport1OrgGroup").val(),
                    'Report1Input.CountMin': $("#txtReport1CountMin").val(),
                    'Report1Input.CountMax': $("#txtReport1CountMax").val(),
                    'Report1Input.IsExport': false
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaDashboardList");
                s.LoadJQGridReport1(param);
            }
            else if (type == "PRO_EMP_PER_POSITION") {
                var param = {
                    'ReportType': $("#ddlReportType").val(),
                    'Report2Input.Position': $("#txtReport2Position").val(),
                    'Report2Input.CountMin': $("#txtReport2CountMin").val(),
                    'Report2Input.CountMax': $("#txtReport2CountMax").val(),
                    'Report2Input.IsExport': false
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaDashboardList");
                s.LoadJQGridReport2(param);
            }
            else if (type == "PRO_STATUS_BEYOND_6") {
                var param = {
                    'ReportType': $("#ddlReportType").val(),
                    'Report3Input.Status': $("#txtReport3Status").val(),
                    'Report3Input.CountMin': $("#txtReport3CountMin").val(),
                    'Report3Input.CountMax': $("#txtReport3CountMax").val(),
                    'Report3Input.IsExport': false
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaDashboardList");
                s.LoadJQGridReport3(param);
            }
            else if (type == "PRO_STATUS_EXPIRING") {
                var param = {
                    'ReportType': $("#ddlReportType").val(),
                    'Report4Input.Status': $("#txtReport4Status").val(),
                    'Report4Input.CountMin': $("#txtReport4CountMin").val(),
                    'Report4Input.CountMax': $("#txtReport4CountMax").val(),
                    'Report4Input.IsExport': false
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaDashboardList");
                s.LoadJQGridReport4(param);
            }
            else if (type == "BIRTHDAY_PER_MONTH") {
                var param = {
                    'ReportType': $("#ddlReportType").val(),
                    'Report5Input.Month': $("#txtReport5Month").val(),
                    'Report5Input.CountMin': $("#txtReport5CountMin").val(),
                    'Report5Input.CountMax': $("#txtReport5CountMax").val(),
                    'Report5Input.IsExport': false
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaDashboardList");
                s.LoadJQGridReport5(param);
            }
            else if (type == "ACT_EMP_PER_GROUP") {
                var param = {
                    'ReportType': $("#ddlReportType").val(),
                    'Report6Input.OrgGroup': $("#txtReport6OrgGroup").val(),
                    'Report6Input.CountMin': $("#txtReport6CountMin").val(),
                    'Report6Input.CountMax': $("#txtReport6CountMax").val(),
                    'Report6Input.IsExport': false
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaDashboardList");
                s.LoadJQGridReport6(param);
            }
            else if (type == "ACT_EMP_PER_POSITION") {
                var param = {
                    'ReportType': $("#ddlReportType").val(),
                    'Report7Input.Position': $("#txtReport7Position").val(),
                    'Report7Input.CountMin': $("#txtReport7CountMin").val(),
                    'Report7Input.CountMax': $("#txtReport7CountMax").val(),
                    'Report7Input.IsExport': false
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaDashboardList");
                s.LoadJQGridReport7(param);
            }
            else if (type == "INACTIVE_EMPLOYEE") {
                var param = {
                    'ReportType': $("#ddlReportType").val(),
                    'Report8Input.Status': $("#txtReport8Status").val(),
                    'Report8Input.CountMin': $("#txtReport8CountMin").val(),
                    'Report8Input.CountMax': $("#txtReport8CountMax").val(),
                    'Report8Input.IsExport': false
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaDashboardList");
                s.LoadJQGridReport8(param);
            }
        }

    };
    
     objEmployeeDashboardListJS.Initialize();
});