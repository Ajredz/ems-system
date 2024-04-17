var objWorkflowListJS;

const WorkflowListURL = "/Workflow/Maintenance?handler=List";
const WorkflowViewURL = "/Workflow/Maintenance/View";
const WorkflowAddURL = "/Workflow/Maintenance/Add";
const WorkflowAddPostURL = "/Workflow/Maintenance/Add";
const WorkflowEditURL = "/Workflow/Maintenance/Edit";
const WorkflowEditPostURL = "/Workflow/Maintenance/Edit";
const WorkflowDeleteURL = "/Workflow/Maintenance/Delete";
const AddApproverRoleDropDownURL = "/Workflow/Maintenance/Add?handler=ApproverRoleDropDown";
const ViewApproverRoleDropDownURL = "/Workflow/Maintenance/View?handler=ApproverRoleDropDown";
const EditApproverRoleDropDownURL = "/Workflow/Maintenance/Edit?handler=ApproverRoleDropDown";
const AddResultTypeDropDownURL = "/Workflow/Maintenance/Add?handler=ResultTypeDropDown";
const ViewResultTypeDropDownURL = "/Workflow/Maintenance/View?handler=ResultTypeDropDown";
const EditResultTypeDropDownURL = "/Workflow/Maintenance/Edit?handler=ResultTypeDropDown";
const GetCheckExportListURL = "/Workflow/Maintenance?handler=CheckExportList";
const DownloadExportListURL = "/Workflow/Maintenance?handler=DownloadExportList";


$(document).ready(function () {
    objWorkflowListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["WorkflowListID"],
                Code: localStorage["WorkflowListCode"],
                Description: localStorage["WorkflowListDescription"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterWorkflowID").val(),
                    Code: $("#txtFilterCode").val(),
                    Description: $("#txtFilterDescription").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblWorkflowList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(WorkflowAddURL, "divWorkflowBodyModal");
                $("#divWorkflowModal").modal("show");
            });

            $("#btnExport").click(function () {

                var parameters = "&sidx=" + $("#tblWorkflowList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblWorkflowList").jqGrid("getGridParam", "sortorder")
                    + "&ID=" + $("#txtFilterWorkflowID").val()
                    + "&Code=" + $("#txtFilterCode").val()
                    + "&Description=" + $("#txtFilterDescription").val()

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
            var tableInfo = localStorage.getItem("tblWorkflowList") == "" ? "" : $.parseJSON(localStorage.getItem("tblWorkflowList"));
            
            var moveFilterFields = function() {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });

                $(".jqgfirstrow").css({ "height": intialHeight + "px"});

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            };
            moveFilterFields();

            $("#tblWorkflowList").jqGrid("GridUnload");
            $("#tblWorkflowList").jqGrid("GridDestroy");
            $("#tblWorkflowList").jqGrid({
                url: WorkflowListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Code", "Description"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objWorkflowListJS.AddLink },
                    { name: "Code", index: "Code", editable: true, align: "left", sortable: true },
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
                    $("#tblWorkflowList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblWorkflowList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblWorkflowList .jqgrid-id-link").click(function () {
                            $('#divWorkflowModal').modal('show');
                        });

                    }

                    if (localStorage["WorkflowListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["WorkflowListFilterOption"]));
                    }
                    objWorkflowListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objWorkflowListJS.ShowHideFilter();
                        localStorage["WorkflowListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblWorkflowList");
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
            localStorage["WorkflowListID"] = $("#txtFilterWorkflowID").val();
            localStorage["WorkflowListCode"] = $("#txtFilterCode").val();
            localStorage["WorkflowListDescription"] = $("#txtFilterDescription").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterWorkflowID").val(localStorage["WorkflowListID"]);
            $("#txtFilterCode").val(localStorage["WorkflowListCode"]);
            $("#txtFilterDescription").val(localStorage["WorkflowListDescription"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + WorkflowViewURL + "?ID=" + rowObject.ID + "', 'divWorkflowBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>"; 
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
    
     objWorkflowListJS.Initialize();
});