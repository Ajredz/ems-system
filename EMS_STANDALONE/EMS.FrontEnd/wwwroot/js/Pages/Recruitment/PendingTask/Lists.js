var objPendingTaskListJS;

const PendingTaskListURL = "/Recruitment/PendingTask?handler=List";
const PendingTaskViewURL = "/Recruitment/PendingTask/View";
const PendingTaskEditURL = "/Recruitment/PendingTask/Edit";
const PendingTaskEditPostURL = "/Recruitment/PendingTask/Edit";
const BatchTaskEditURL = "/Recruitment/PendingTask/BatchEdit";
const BatchTaskEditPostURL = "/Recruitment/PendingTask/BatchEdit";
const GetStatusURL = "/Recruitment/PendingTask?handler=ReferenceValue&RefCode=TASK_STATUS";
const GetApplicantURL = "/Recruitment/PendingTask?handler=ApplicantByID";


$(document).ready(function () {
    objPendingTaskListJS = {

        Initialize: function () {
            var s = this;
            $("#txtFilterDateCreatedFrom, #txtFilterDateCreatedTo, "
                + "#txtFilterDateModifiedFrom, #txtFilterDateModifiedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            s.ElementBinding();
            var param = {
                ID: localStorage["PendingTaskListID"],
                Applicant: localStorage["PendingTaskListApplicant"],
                Description: localStorage["PendingTaskListDescription"],
                StatusDelimited: localStorage["PendingTaskListStatusDelimited"],
                DateCreatedFrom: localStorage["PendingTaskListDateCreatedFrom"],
                DateCreatedTo: localStorage["PendingTaskListDateCreatedTo"],
                DateApprovedFrom: localStorage["PendingTaskListDateModifiedFrom"],
                DateApprovedTo: localStorage["PendingTaskListDateModifiedTo"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterPendingTaskID"));

            $("#btnSearch").click(function () {
                $("#divPendingListErrorMessage").html("");
                var param = {
                    ID: $("#txtFilterPendingTaskID").val(),
                    Applicant: $("#txtFilterApplicant").val(),
                    Description: $("#txtFilterDescription").val(),
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value,
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val(),
                    DateModifiedFrom: $("#txtFilterDateModifiedFrom").val(),
                    DateModifiedTo: $("#txtFilterDateModifiedTo").val(),

                };
                s.SetLocalStorage();
                ResetJQGridState("tblPendingTaskList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("#divPendingListErrorMessage").html("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("div.filterFields input[type='search']").val("");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnBatchEdit").click(function () {
                var input = $("#tblPendingTaskList").jqGrid("getGridParam", "selarrrow");

                if (input.length == 0) {
                    $("#divPendingListErrorMessage").html("<label class=\"errMessage\"><li>" + "Please select a task" + "</li></label><br />");
                }
                else {
                    $("#divPendingListErrorMessage").html("");
                    LoadPartial(BatchTaskEditURL, "divPendingTaskBodyModal");
                    $("#divPendingTaskModal").modal("show");
                }
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedStatus", GetStatusURL);

        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblPendingTaskList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPendingTaskList"));
            
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

            $("#tblPendingTaskList").jqGrid("GridUnload");
            $("#tblPendingTaskList").jqGrid("GridDestroy");
            $("#tblPendingTaskList").jqGrid({
                url: PendingTaskListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Applicant", "Description", "Status", "Created Date", "Modified Date"],
                colModel: [
                    { hidden: true},
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objPendingTaskListJS.ViewLink },
                    { name: "Applicant", index: "Applicant", editable: true, align: "left" },
                    { name: "Description", index: "Description", editable: true, align: "left" },
                    { name: "Status", index: "Status", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                    { name: "ModifiedDate", index: "ModifiedDate", editable: true, align: "left", sortable: true },
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
                multiselect: true,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblPendingTaskList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblPendingTaskList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblPendingTaskList .jqgrid-id-link").click(function () {
                            $("#divPendingListErrorMessage").html("");
                            $('#divPendingTaskModal').modal('show');
                        });

                        $("#tblPendingTaskList .cbox, #cb_tblPendingTaskList, .ui-jqgrid-sortable").click(function () {
                            $("#divPendingListErrorMessage").html("");
                        });

                    }

                    if (localStorage["PendingTaskListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PendingTaskListFilterOption"]));
                    }
                    objPendingTaskListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objPendingTaskListJS.ShowHideFilter();
                        localStorage["PendingTaskListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblPendingTaskList");
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
            localStorage["PendingTaskListID"] = $("#txtFilterPendingTaskID").val();

            localStorage["PendingTaskListApplicant"] = $("#txtFilterApplicant").val();
            localStorage["PendingTaskListDescription"] = $("#txtFilterDescription").val();

            localStorage["PendingTaskListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value;
            localStorage["PendingTaskListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").text;

            localStorage["PendingTaskListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["PendingTaskListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
            localStorage["PendingTaskListDateModifiedFrom"] = $("#txtFilterDateModifiedFrom").val();
            localStorage["PendingTaskListDateModifiedTo"] = $("#txtFilterDateModifiedTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterPendingTaskID").val(localStorage["PendingTaskListID"]);
            $("#txtFilterApplicant").val(localStorage["PendingTaskListApplicant"]);
            $("#txtFilterDescription").val(localStorage["PendingTaskListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedStatus"
              , "PendingTaskListStatusDelimited"
              , "PendingTaskListStatusDelimitedText");

            $("#txtFilterDateCreatedFrom").val(localStorage["ManpowerMRFListDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["ManpowerMRFListDateCreatedTo"]);
            $("#txtFilterDateModifiedFrom").val(localStorage["PendingTaskListDateModifiedFrom"]);
            $("#txtFilterDateModifiedTo").val(localStorage["PendingTaskListDateModifiedTo"]);

        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + PendingTaskViewURL + "?ID=" + rowObject.ID + "', 'divPendingTaskBodyModal');\">" + rowObject.ID + "</a>"; 
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
    
     objPendingTaskListJS.Initialize();
});