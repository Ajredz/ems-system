var objAuditLogsListJS;
const AuditLogsListURL = "/Administrator/AuditLogs?handler=List";
const AuditLogsViewURL = "/Administrator/AuditLogs/View";

const EventTypeAutoCompleteURL = "/Administrator/AuditLogs?handler=EventTypeAutoComplete";
const TableNameAutoCompleteURL = "/Administrator/AuditLogs?handler=TableNameAutoComplete";

$(document).ready(function () {
    objAuditLogsListJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                ID: localStorage["AdministratorAuditLogsListID"],
                EventTypeDelimited: localStorage["AdministratorAuditLogsListEventTypeDelimitedText"],
                TableNameDelimited: localStorage["AdministratorAuditLogsListTableNameDelimitedText"],
                Remarks: localStorage["AdministratorAuditLogsListRemarks"],
                Name: localStorage["AdministratorAuditLogsListName"],
                IPAddress: localStorage["AdministratorAuditLogsListIPAddress"],
                DateCreatedFrom: localStorage["AdministratorAuditLogsListDateCreatedFrom"],
                DateCreatedTo: localStorage["AdministratorAuditLogsListDateCreatedTo"],
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

            NumberOnly($("#txtFilterAuditLogsID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterAuditLogsID").val(),
                    EventTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEventType").text,
                    TableNameDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedTableName").text,
                    Remarks: $("#txtFilterRemarks").val(),
                    Name: $("#txtFilterName").val(),
                    IPAddress: $("#txtFilterIPAddress").val(),
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAdministratorAuditLogsList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedEventType").html("");
                $("#multiSelectedTableName").html("");
                $("#btnSearch").click();
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterEventType"
                , EventTypeAutoCompleteURL, 20, "multiSelectedEventType");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterTableName"
                , TableNameAutoCompleteURL, 20, "multiSelectedTableName");
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblAdministratorAuditLogsList") == "" ? "" : $.parseJSON(localStorage.getItem("tblAdministratorAuditLogsList"));
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
            $("#tblAdministratorAuditLogsList").jqGrid("GridUnload");
            $("#tblAdministratorAuditLogsList").jqGrid("GridDestroy");
            $("#tblAdministratorAuditLogsList").jqGrid({
                url: AuditLogsListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Type", "Table Name", "Remarks", "System User", "IP Address", "Created Date"],
                colModel: [
                    { hidden: true },
                    { hidden: true, width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true },
                    { width: 30, name: "Type", index: "Type", editable: true, align: "left" },
                    { width: 60, name: "TableName", index: "TableName", editable: true, align: "left" },
                    { width: 100, name: "Remarks", index: "Remarks", editable: true, align: "left", sortable: true },
                    { width: 70, name: "Name", index: "Name", editable: true, align: "left", sortable: true },
                    { width: 30, name: "IPAddress", index: "IPAddress", editable: true, align: "left", sortable: true },
                    { width: 50, name: "DateCreated", index: "DateCreated", editable: true, align: "center" },
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
                        //AutoSizeColumnJQGrid("tblAdministratorAuditLogsList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                        $("#tblAdministratorAuditLogsList .jqgrid-id-link").click(function () {
                            $('#divAuditLogsModal').modal('show');
                        });

                    }

                    if (localStorage["AuditLogsListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["AuditLogsListFilterOption"]));
                    }
                    objAuditLogsListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objAuditLogsListJS.ShowHideFilter();
                        localStorage["AuditLogsListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblAdministratorAuditLogsList");
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
            localStorage["AdministratorAuditLogsListID"] = $("#txtFilterAuditLogsID").val();
            localStorage["AdministratorAuditLogsListEventTypeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEventType").value;
            localStorage["AdministratorAuditLogsListEventTypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEventType").text;
            localStorage["AdministratorAuditLogsListTableNameDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedTableName").value;
            localStorage["AdministratorAuditLogsListTableNameDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedTableName").text;
            localStorage["AdministratorAuditLogsListRemarks"] = $("#txtFilterRemarks").val();
            localStorage["AdministratorAuditLogsListName"] = $("#txtFilterName").val();
            localStorage["AdministratorAuditLogsListIPAddress"] = $("#txtFilterIPAddress").val();
            localStorage["AdministratorAuditLogsListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["AdministratorAuditLogsListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterAuditLogsID").val(localStorage["AdministratorAuditLogsListID"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedEventType"
                , "AdministratorAuditLogsListEventTypeDelimited"
                , "AdministratorAuditLogsListEventTypeDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedTableName"
                , "AdministratorAuditLogsListTableNameDelimited"
                , "AdministratorAuditLogsListTableNameDelimitedText");

            $("#txtFilterRemarks").val(localStorage["AdministratorAuditLogsListRemarks"]);
            $("#txtFilterName").val(localStorage["AdministratorAuditLogsListName"]);
            $("#txtFilterIPAddress").val(localStorage["AdministratorAuditLogsListIPAddress"]);
            $("#txtFilterDateCreatedFrom").val(localStorage["AdministratorAuditLogsListDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["AdministratorAuditLogsListDateCreatedTo"]);
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
    
     objAuditLogsListJS.Initialize();
});