var objSystemErrorLogListJS;
const SystemErrorLogListURL = "/Administrator/SystemErrorLog?handler=List";
const SystemErrorLogViewURL = "/Administrator/SystemErrorLog/View";
const UserAutoComplete = "/Administrator/SystemErrorLog?handler=UserAutoComplete";

const GetSystemErrorLogDropDownURL = "/Administrator/SystemErrorLog?handler=SystemErrorLogDropDown";
const GetSystemErrorLogRoleDropDownURL = "/Administrator/SystemErrorLog?handler=SystemErrorLogRoleDropDownByRoleID";
const GetCheckExportListURL = "/Administrator/SystemErrorLog?handler=CheckExportList";
const DownloadExportListURL = "/Administrator/SystemErrorLog?handler=DownloadExportList";


$(document).ready(function () {
    objSystemErrorLogListJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                ID: localStorage["AdministratorSystemErrorLogListID"],
                Class: localStorage["AdministratorSystemErrorLogListClass"],
                //Method: localStorage["AdministratorSystemErrorLogListMethod"],
                ErrorMessage: localStorage["AdministratorSystemErrorLogListErrorMessage"],
                UserIDDelimited: localStorage["AdministratorSystemErrorLogListUser"],
                DateCreatedFrom: localStorage["AdministratorSystemErrorLogListDateCreatedFrom"],
                DateCreatedTo: localStorage["AdministratorSystemErrorLogListDateCreatedTo"],
                ReportType: localStorage["AdministratorSystemErrorLogListReportType"],
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

            NumberOnly($("#txtFilterID"));
            
            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterID").val(),
                    Class: $("#txtFilterClass").val(),
                    //Method: $("#txtFilterMethod").val(),
                    ErrorMessage: $("#txtFilterErrorMessage").val(),
                    UserIDDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedUser").value,
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val(),
                    ReportType: $("#ddlReportType").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAdministratorSystemErrorLogList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("#multiSelectedUser").html("");
                $("#btnSearch").click();
            });

            $("#btnExport").click(function () {

                var parameters = "&sidx=" + $("#tblAdministratorSystemErrorLogList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblAdministratorSystemErrorLogList").jqGrid("getGridParam", "sortorder")
                    + "&ID=" + $("#txtFilterID").val()
                    + "&Class=" + $("#txtFilterClass").val()
                    //+ "&Method=" + $("#txtFilterMethod").val()
                    + "&ErrorMessage=" + $("#txtFilterErrorMessage").val()
                    + "&UserIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedUser").value
                    + "&DateCreatedFrom=" + $("#txtFilterDateCreatedFrom").val()
                    + "&DateCreatedTo=" + $("#txtFilterDateCreatedTo").val()
                    + "&ReportType=" + $("#ddlReportType").val()

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

            $("#ddlReportType").change(function () {
                var param = {
                    ID: $("#txtFilterID").val(),
                    Class: $("#txtFilterClass").val(),
                    //Method: $("#txtFilterMethod").val(),
                    ErrorMessage: $("#txtFilterErrorMessage").val(),
                    UserIDDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedUser").value,
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val(),
                    ReportType: $("#ddlReportType").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAdministratorSystemErrorLogList");
                s.LoadJQGrid(param);
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterUser"
                , UserAutoComplete, 20, "multiSelectedUser");
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblAdministratorSystemErrorLogList") == "" ? "" : $.parseJSON(localStorage.getItem("tblAdministratorSystemErrorLogList"));
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
            $("#tblAdministratorSystemErrorLogList").jqGrid("GridUnload");
            $("#tblAdministratorSystemErrorLogList").jqGrid("GridDestroy");
            $("#tblAdministratorSystemErrorLogList").jqGrid({
                url: SystemErrorLogListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Class", "Error Message", "User", "Created Date"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objSystemErrorLogListJS.AddLink },
                    { name: "Class", index: "Class", editable: true, align: "left" },
                    //{ name: "Method", index: "Method", editable: true, align: "left" },
                    { name: "ErrorMessage", index: "ErrorMessage", editable: true, align: "left" },
                    { name: "User", index: "User", editable: true, sortable: false, align: "left" },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "center" },
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
                    $("#tblAdministratorSystemErrorLogList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblAdministratorSystemErrorLogList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblAdministratorSystemErrorLogList .jqgrid-id-link").click(function () {
                            $('#divSystemErrorLogModal').modal('show');
                        });

                    }

                    if (localStorage["SystemErrorLogListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["SystemErrorLogListFilterOption"]));
                    }
                    objSystemErrorLogListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objSystemErrorLogListJS.ShowHideFilter();
                        localStorage["SystemErrorLogListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $(".ui-jqgrid-bdiv").css({ "min-height": "400px" });

                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblAdministratorSystemErrorLogList");
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
            localStorage["AdministratorSystemErrorLogListID"] = $("#txtFilterID").val();
            localStorage["AdministratorSystemErrorLogListClass"] = $("#txtFilterClass").val();
            //localStorage["AdministratorSystemErrorLogListMethod"] = $("#txtFilterMethod").val();
            localStorage["AdministratorSystemErrorLogListErrorMessage"] = $("#txtFilterErrorMessage").val();
            localStorage["AdministratorSystemErrorLogListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["AdministratorSystemErrorLogListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
            localStorage["AdministratorSystemErrorLogListReportType"] = $("#ddlReportType").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterID").val(localStorage["AdministratorSystemErrorLogListID"]);
            $("#txtFilterClass").val(localStorage["AdministratorSystemErrorLogListClass"]);
            //$("#txtFilterMethod").val(localStorage["AdministratorSystemErrorLogListMethod"]);
            $("#txtFilterErrorMessage").val(localStorage["AdministratorSystemErrorLogListErrorMessage"]);
            $("#txtFilterDateCreatedFrom").val(localStorage["AdministratorSystemErrorLogListDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["AdministratorSystemErrorLogListDateCreatedTo"]);
            $("#ddlReportType").val(localStorage["AdministratorSystemErrorLogListReportType"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + SystemErrorLogViewURL + "?ID=" + rowObject.ID + "&ReportType=" + $("#ddlReportType").val() + "', 'divSystemErrorLogBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID)+"</a>"; 
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
    
     objSystemErrorLogListJS.Initialize();
});