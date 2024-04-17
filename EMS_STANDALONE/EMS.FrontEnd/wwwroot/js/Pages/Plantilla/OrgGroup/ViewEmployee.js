var objOrgGroupEmployeeListJS;

$(document).ready(function () {
    objOrgGroupEmployeeListJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                PositionDelimited: localStorage["OrgGroupPositionDelimited"],
                EmployeeName: localStorage["OrgGroupEmployeeName"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#tabOrgGroupEmployee #btnSearch").click(function () {
                var param = {
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("divOrgGroupEmployee #multiSelectedPosition").value,
                    EmployeeName: $("#divOrgGroupEmployee #txtFilterEmployeeName").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblOrgGroupEmployeeList");
                s.LoadJQGrid(param);
            });

            $("#divOrgGroupEmployee #btnReset").click(function () {
                $("#divOrgGroupEmployee div.filterFields input").val("");
                $("#divOrgGroupEmployee div.filterFields select").val("");
                $("#divOrgGroupEmployee #multiSelectedPosition").html("");
                $("#divOrgGroupEmployee #btnSearch").click();
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("divOrgGroupEmployee #txtFilterPosition"
                , PositionAutoCompleteURL, 20, "divOrgGroupEmployee #multiSelectedPosition");
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblOrgGroupEmployeeList") == "" ? "" : $.parseJSON(localStorage.getItem("tblOrgGroupEmployeeList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divOrgGroupEmployee .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divOrgGroupEmployee .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divOrgGroupEmployee #filterFieldsContainerEmployee");
                });

                $("#divOrgGroupEmployee .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divOrgGroupEmployee div.filterFields").unbind("keyup");
                $("#divOrgGroupEmployee div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divOrgGroupEmployee #btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#divOrgGroupEmployee #tblOrgGroupEmployeeList").jqGrid("GridUnload");
            $("#divOrgGroupEmployee #tblOrgGroupEmployeeList").jqGrid("GridDestroy");
            $("#divOrgGroupEmployee #tblOrgGroupEmployeeList").jqGrid({
                url: OrgTypeEmployeeListURL + "&ID=" + objOrgGroupEmployeeListJS.ID,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Position", "Name"],
                colModel: [
                    { hidden: true },
                    { name: "Position", index: "Position", align: "left", sortable: true, width: 115 },
                    { name: "EmployeeName", index: "EmployeeName", align: "left", sortable: true, width: 110}
                ],
                toppager: $("#divOrgGroupEmployee #divPager"),
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
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            //$("#tabOrgGroupEmployee #divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblOrgGroupEmployeeList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divOrgGroupEmployee #filterFieldsContainerEmployee .filterFields").each(function (n, element) {
                            $(this).appendTo("#divOrgGroupEmployee .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });


                    }

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divOrgGroupEmployee .ui-jqgrid-bdiv").css({ "min-height": "200px" });

                    $("#divOrgGroupEmployee table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#divOrgGroupEmployee table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#divOrgGroupEmployee table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divOrgGroupEmployee table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#divOrgGroupEmployee table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divOrgGroupEmployee table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#divOrgGroupEmployee table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divOrgGroupEmployee .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
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
                    GetJQGridState("tblOrgGroupEmployeeList");
                    moveFilterFields();
                },
            }).navGrid("#divOrgGroupEmployee #divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery("#divOrgGroupEmployee .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
        },

        SetLocalStorage: function () {
            
            localStorage["OrgGroupPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("divOrgGroupEmployee #multiSelectedPosition").value;
            localStorage["OrgGroupPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divOrgGroupEmployee #multiSelectedPosition").text;
            localStorage["OrgGroupEmployeeName"] = $("#divOrgGroupEmployee #txtFilterEmployeeName").val();
        },

        GetLocalStorage: function () {

            objEMSCommonJS.SetMultiSelectList("divOrgGroupEmployee #multiSelectedPosition"
              , "OrgGroupPositionDelimited"
              , "OrgGroupPositionDelimitedText");

            $("#divOrgGroupEmployee #txtFilterEmployeeName").val(localStorage["OrgGroupEmployeeName"]);
        },

    };

    objOrgGroupEmployeeListJS.Initialize();

});