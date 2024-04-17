var objOrgHistoryViewJS;


$(document).ready(function () {
    objOrgHistoryViewJS = {

        Initialize: function () {
            $("#divOrgGroupHistoryBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            s.LoadJQGrid($("#hdnTDate").val(), $("#hdnIsLatest").val());

            $("#txtDateEffective").val($("#hdnTDate").val());

            $("#btnEdit").prop("hidden",false);
        },

        ElementBinding: function () {
            var s = this;

            $("#closeModal").click(function () {
                $("#divOrgGroupHistoryModal").modal("hide");
            });

            $("#btnEdit").click(function () {
                LoadPartial(OrgHistoryEditURL + "?TDate=" + $("#hdnTDate").val() + "&IsLatest=" + $("#hdnIsLatest").val(), 'divOrgGroupHistoryBodyModal');
            });
        },

        LoadJQGrid: function (TDate,IsLatest) {
            var s = this;
            var param = {
                TDate:TDate,
                IsLatest:IsLatest
            };

            Loading(true);

            var tableInfo = localStorage.getItem("tblOrgHistoryViewList") == "" ? "" : $.parseJSON(localStorage.getItem("tblOrgHistoryViewList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divOrgGroupTableViewList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divOrgGroupTableViewList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $("#tblOrgHistoryViewList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divOrgGroupTableViewList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();

            $("#tblOrgHistoryViewList").jqGrid("GridUnload");
            $("#tblOrgHistoryViewList").jqGrid("GridDestroy");
            $("#tblOrgHistoryViewList").jqGrid({
                url: OrgHistoryViewListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "", "", "", "", "Org Type", "Org Group", "Parent Org"],
                colModel: [
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center", sortable: true },
                    { hidden: true, name: "Code", index: "Code", editable: false, align: "left", sortable: true },
                    { hidden: true, name: "Description", index: "Description", editable: false, align: "left", sortable: true },
                    { hidden: true, name: "ParentOrgId", index: "ParentOrgId", editable: false, align: "left", sortable: true },
                    { hidden: true, name: "ParentDescription", index: "ParentDescription", editable: false, align: "left", sortable: true },
                    { name: "OrgType", index: "OrgType", editable: false, align: "center", sortable: true },
                    { name: "CodeDescription", index: "CodeDescription", editable: false, align: "left", sortable: true },
                    { name: "ParentCodeDescription", index: "ParentCodeDescription", align: "left", sortable: true },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: true,
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
                ignoreCase: true,
                cellEdit: true,
                cellsubmit: 'clientArray',
                /*ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblOrgHistoryViewList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                }, */
                loadComplete: function (data) {
                    Loading(false);

                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    //$("#tblOrgHistoryViewList").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, defaultSearch: "cn", ignoreCase: true });
                    //$("#gs_Code").addClass("form-control");

                    if (data.rows != null) {
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblOrgHistoryViewList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divOrgGroupTableList .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });
                    }

                    objOrgHistoryViewJS.ShowHideFilter($("#tblOrgHistoryViewList"));

                    $("#chkFilterOrg").on('change', function () {
                        objOrgHistoryViewJS.ShowHideFilter($("#tblOrgHistoryViewList"));
                        localStorage["OrgGroupListFilterOption"] = $("#chkFilterOrg").is(":checked");
                    });

                    //var localGridData = $("#tblOrgHistoryViewList").jqGrid('getGridParam', 'data');

                    $("#gs_ID").css("display", "none");
                    $("#gs_CodeDescription").addClass("form-control").css("width", "100%");
                    $("#gs_ParentCodeDescription").addClass("form-control").css("width", "100%");
                    $("#gs_OrgType").addClass("form-control").css("width", "100%");

                    // set minimum height to prevent multiselect tags from being hidden by the scroll
                    $(".ui-jqgrid-bdiv").css({ "min-height": "350px" });

                    $("#btnReset").click(function () {
                        $("#gs_CodeDescription").val("");
                        $("#gs_ParentCodeDescription").val("");
                        $("#tblOrgHistoryViewList").jqGrid('setGridParam', { search: false, postData: { "filters": "" } }).trigger("reloadGrid");
                    });
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblOrgHistoryViewList");
                    moveFilterFields();
                },
                afterSaveCell: function (rowid, cellname, value, iRow, iCol) {
                    
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            ).jqGrid('filterToolbar', {
                stringResult: true,
                searchOnEnter: false,
                defaultSearch: "cn"
            });
            $("#divOrgGroupHistoryModal .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            $("#divOrgGroupHistoryModal .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            $("#divOrgGroupHistoryModal #lblFilter").after("<input type=\"checkbox\" id=\"chkFilterOrg\" style=\"margin-right:15px;\"></div>");
        },

        ShowHideFilter: function (table) {
            if ($("#chkFilterOrg").is(":checked")) {
                $(".ui-search-toolbar").css("display", "");
            }
            else if ($("#chkFilterOrg").is(":not(:checked)")) {
                $(".ui-search-toolbar").css("display", "none");
            }

        }
    };

    objOrgHistoryViewJS.Initialize();
});