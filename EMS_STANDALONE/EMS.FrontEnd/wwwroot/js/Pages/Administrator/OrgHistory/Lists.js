var objOrgHistoryListJS;

const OrgHistoryListURL = "/Administrator/OrgHistory?Handler=List";
const OrgHistoryAutoCompleteURL = "/Administrator/OrgHistory?handler=OrgGroupAutoComplete";
const OrgHistoryAddURL = "/Administrator/OrgHistory/Add";
const OrgHistoryViewURL = "/Administrator/OrgHistory/View";
const OrgHistoryEditURL = "/Administrator/OrgHistory/Edit";
const OrgHistoryAddListURL = "/Administrator/OrgHistory/Add?Handler=List";
const OrgHistoryViewListURL = "/Administrator/OrgHistory/View?Handler=List";
const OrgHistoryEditListURL = "/Administrator/OrgHistory/Edit?Handler=List";
var enumData = [];

$(document).ready(function () {
    objOrgHistoryListJS = {
        Initialize: function () {
            var s = this;
            s.ElementBinding();
            s.LoadJQGrid();
        }, 

        ElementBinding: function () {
            var s = this;

            $("#txtFilterFrom,#txtFilterTo").datetimepicker({
                useCurrent: false,
                format: 'DD-MMM-YYYY',
            });

            $("#btnReset").click(function () {
                $("#txtFilterFrom,#txtFilterTo").val("");
                $("#multiSelectedIsLatest").html("");
                $("#multiSelectedIsLatestOption label, #multiSelectedIsLatestOption input").prop("title", "add");
                s.LoadJQGrid();
            });
            
            $("#btnSearch").click(function () {
                var param = {
                    TDateFrom: $("#txtFilterFrom").val(),
                    TDateTo: $("#txtFilterTo").val(),
                    IsLatest: objEMSCommonJS.GetMultiSelectList("multiSelectedIsLatest").value
                }
                s.LoadJQGrid(param);
            });

            $("#btnAdd").click(function () {
                $('#divOrgGroupHistoryModal').modal('show');
                LoadPartial(OrgHistoryAddURL, "divOrgGroupHistoryBodyModal");
            });

            enumData.push({ ID: "YES", Description: "YES" });
            enumData.push({ ID: "NO", Description: "NO" });
            objEMSCommonJS.BindFilterMultiSelectEnumLocalData("multiSelectedIsLatest", enumData);
        },

        LoadJQGrid: function (param) {
            var s = this;

            Loading(true);

            var tableInfo = localStorage.getItem("tblOrgHistoryList") == "" ? "" : $.parseJSON(localStorage.getItem("tblOrgHistoryList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divOrgGroupTableList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divOrgGroupTableList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $("#divOrgGroupTableList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divOrgGroupTableList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();

            $("#tblOrgHistoryList").jqGrid("GridUnload");
            $("#tblOrgHistoryList").jqGrid("GridDestroy");
            $("#tblOrgHistoryList").jqGrid({
                url: OrgHistoryListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["","Effective Date", "Is Latest?"],
                colModel: [
                    { hidden:true,key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objOrgHistoryListJS.AddLink },
                    { name: "TDate", index: "TDate", editable: true, align: "center", sortable: true },
                    { name: "IsLatestDescription", index: "IsLatestDescription", editable: true, align: "center", sortable: true },
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
                    $("#tblOrgHistoryList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);
                    if (data.rows != null) {
                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divOrgGroupTableList .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblOrgHistoryList .jqgrid-id-link").click(function () {
                            $('#divOrgGroupHistoryModal').modal('show');
                        });
                    }

                    if (localStorage["OrgGroupListFilterOption"] != undefined) {
                        $("#chkFilterOrgList").prop('checked', JSON.parse(localStorage["OrgGroupListFilterOption"]));
                    }
                    objOrgHistoryListJS.ShowHideFilter();

                    $("#chkFilterOrgList").on('change', function () {
                        objOrgHistoryListJS.ShowHideFilter();
                        localStorage["OrgGroupListFilterOption"] = $("#chkFilterOrgList").is(":checked");
                    });

                    // set minimum height to prevent multiselect tags from being hidden by the scroll
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
                    GetJQGridState("tblOrgHistoryList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery(".ui-row-label").closest("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilterOrgList\" style=\"margin-right:15px;\"></div>");
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + OrgHistoryViewURL + "?TDate=" + rowObject.TDate + "&IsLatest=" + rowObject.IsLatestDescription + "', 'divOrgGroupHistoryBodyModal');\">View</a>";
        },

        ShowHideFilter: function () {
            if ($("#chkFilterOrgList").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilterOrgList").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        }
    };

    objOrgHistoryListJS.Initialize();
});