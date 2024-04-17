var objPositionLevelListJS;
const PositionLevelListURL = "/Plantilla/PositionLevel?handler=List";
const PositionLevelAddURL = "/Plantilla/PositionLevel/Add";
const PositionLevelViewURL = "/Plantilla/PositionLevel/View";
const PositionLevelEditURL = "/Plantilla/PositionLevel/Edit";
const PositionLevelDeleteURL = "/Plantilla/PositionLevel/Delete";
const PositionLevelAddPostURL = "/Plantilla/PositionLevel/Add";
const PositionLevelEditPostURL = "/Plantilla/PositionLevel/Edit";
const GetCheckExportListURL = "/Plantilla/PositionLevel?handler=CheckExportList";
const DownloadExportListURL = "/Plantilla/PositionLevel?handler=DownloadExportList";


$(document).ready(function () {
    objPositionLevelListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["PlantillaPositionLevelListID"],
                Description: localStorage["PlantillaPositionLevelListDescription"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterPositionLevelID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterPositionLevelID").val(),
                    Description: $("#txtFilterDescription").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaPositionLevelList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(PositionLevelAddURL, "divPositionLevelBodyModal");
                $("#divPositionLevelModal").modal("show");
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objPositionLevelListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblPlantillaPositionLevelList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaPositionLevelList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterPositionLevelID").val()
                + "&Description=" + $("#txtFilterDescription").val()

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaPositionLevelList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaPositionLevelList"));
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

            $("#tblPlantillaPositionLevelList").jqGrid("GridUnload");
            $("#tblPlantillaPositionLevelList").jqGrid("GridDestroy");
            $("#tblPlantillaPositionLevelList").jqGrid({
                url: PositionLevelListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Description"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objPositionLevelListJS.AddLink },
                    { name: "Description", index: "Description", editable: true, align: "left" },
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
                    $("#tblPlantillaPositionLevelList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblPlantillaPositionLevelList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblPlantillaPositionLevelList .jqgrid-id-link").click(function () {
                            $('#divPositionLevelModal').modal('show');
                        });

                    }

                    if (localStorage["PositionLevelListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PositionLevelListFilterOption"]));
                    }
                    objPositionLevelListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objPositionLevelListJS.ShowHideFilter();
                        localStorage["PositionLevelListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblPlantillaPositionLevelList");
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage: function () {
            localStorage["PlantillaPositionLevelListID"] = $("#txtFilterPositionLevelID").val();
            localStorage["PlantillaPositionLevelListDescription"] = $("#txtFilterDescription").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterPositionLevelID").val(localStorage["PlantillaPositionLevelListID"]);
            $("#txtFilterDescription").val(localStorage["PlantillaPositionLevelListDescription"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + PositionLevelViewURL + "?ID=" + rowObject.ID + "', 'divPositionLevelBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>"; 
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
    
     objPositionLevelListJS.Initialize();
});