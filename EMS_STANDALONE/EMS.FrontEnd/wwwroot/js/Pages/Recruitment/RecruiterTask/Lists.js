var objRecruiterTaskListJS;

const RecruiterTaskListURL = "/Recruitment/RecruiterTask?handler=List";
const RecruiterTaskViewURL = "/Recruitment/RecruiterTask/View";
const RecruiterTaskAddURL = "/Recruitment/RecruiterTask/Add";
const RecruiterTaskAddPostURL = "/Recruitment/RecruiterTask/Add";
const RecruiterTaskEditURL = "/Recruitment/RecruiterTask/Edit";
const RecruiterTaskEditPostURL = "/Recruitment/RecruiterTask/Edit";
const RecruiterTaskDeleteURL = "/Recruitment/RecruiterTask/Delete";
const GetStatusURL = "/Recruitment/RecruiterTask?handler=ReferenceValue&RefCode=TASK_STATUS";
const RecruiterAutoCompleteURL = "/Recruitment/RecruiterTask?handler=RecruiterAutoComplete";
const ApplicantAutoCompleteURL = "/Recruitment/RecruiterTask?handler=ApplicantAutoComplete";
const GetRecruiterURL = "/Recruitment/RecruiterTask?handler=RecruiterByID";
const GetApplicantURL = "/Recruitment/RecruiterTask?handler=ApplicantByID";

$(document).ready(function () {
    objRecruiterTaskListJS = {

        Initialize: function () {
            var s = this;
            $("#txtFilterDateCreatedFrom, #txtFilterDateCreatedTo, "
                + "#txtFilterDateModifiedFrom, #txtFilterDateModifiedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            s.ElementBinding();
            var param = {
                ID: localStorage["RecruiterTaskListID"],
                Recruiter: localStorage["RecruiterTaskListRecruiter"],
                Applicant: localStorage["RecruiterTaskListApplicant"],
                Description: localStorage["RecruiterTaskListDescription"],
                StatusDelimited: localStorage["RecruiterTaskListStatusDelimited"],
                DateCreatedFrom: localStorage["RecruiterTaskListDateCreatedFrom"],
                DateCreatedTo: localStorage["RecruiterTaskListDateCreatedTo"],
                DateApprovedFrom: localStorage["RecruiterTaskListDateModifiedFrom"],
                DateApprovedTo: localStorage["RecruiterTaskListDateModifiedTo"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterRecruiterTaskID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterRecruiterTaskID").val(),
                    Recruiter: $("#txtFilterRecruiter").val(),
                    Applicant: $("#txtFilterApplicant").val(),
                    Description: $("#txtFilterDescription").val(),
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value,
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val(),
                    DateModifiedFrom: $("#txtFilterDateModifiedFrom").val(),
                    DateModifiedTo: $("#txtFilterDateModifiedTo").val(),

                };
                s.SetLocalStorage();
                ResetJQGridState("tblRecruiterTaskList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("div.filterFields input[type='search']").val("");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(RecruiterTaskAddURL, "divRecruiterTaskBodyModal");
                $("#divRecruiterTaskModal").modal("show");
            });

          objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedStatus", GetStatusURL);

        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblRecruiterTaskList") == "" ? "" : $.parseJSON(localStorage.getItem("tblRecruiterTaskList"));
            
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

            $("#tblRecruiterTaskList").jqGrid("GridUnload");
            $("#tblRecruiterTaskList").jqGrid("GridDestroy");
            $("#tblRecruiterTaskList").jqGrid({
                url: RecruiterTaskListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Recruiter", "Applicant", "Description", "Status", "Created Date", "Modified Date"],
                colModel: [
                    { key: true, hidden: true, name: "Id", index: "Id", editable: false },
                    { width: 15, name: "ID", index: "ID", align: "center", sortable: true, formatter: objRecruiterTaskListJS.AddLink },
                    { name: "Recruiter", index: "Recruiter", editable: true, align: "left", sortable: true },
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
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblRecruiterTaskList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblRecruiterTaskList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblRecruiterTaskList .jqgrid-id-link").click(function () {
                            $('#divRecruiterTaskModal').modal('show');
                        });

                    }

                    if (localStorage["RecruiterTaskListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["RecruiterTaskListFilterOption"]));
                    }
                    objRecruiterTaskListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objRecruiterTaskListJS.ShowHideFilter();
                        localStorage["RecruiterTaskListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblRecruiterTaskList");
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
            localStorage["RecruiterTaskListID"] = $("#txtFilterRecruiterTaskID").val();

            localStorage["RecruiterTaskListRecruiter"] = $("#txtFilterRecruiter").val();
            localStorage["RecruiterTaskListApplicant"] = $("#txtFilterApplicant").val();
            localStorage["RecruiterTaskListDescription"] = $("#txtFilterDescription").val();

            localStorage["RecruiterTaskListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value;
            localStorage["RecruiterTaskListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").text;

            localStorage["RecruiterTaskListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["RecruiterTaskListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
            localStorage["RecruiterTaskListDateModifiedFrom"] = $("#txtFilterDateModifiedFrom").val();
            localStorage["RecruiterTaskListDateModifiedTo"] = $("#txtFilterDateModifiedTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterRecruiterTaskID").val(localStorage["RecruiterTaskListID"]);
            $("#txtFilterRecruiter").val(localStorage["RecruiterTaskListRecruiter"]);
            $("#txtFilterApplicant").val(localStorage["RecruiterTaskListApplicant"]);
            $("#txtFilterDescription").val(localStorage["RecruiterTaskListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedStatus"
              , "RecruiterTaskListStatusDelimited"
              , "RecruiterTaskListStatusDelimitedText");

            $("#txtFilterDateCreatedFrom").val(localStorage["ManpowerMRFListDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["ManpowerMRFListDateCreatedTo"]);
            $("#txtFilterDateModifiedFrom").val(localStorage["RecruiterTaskListDateModifiedFrom"]);
            $("#txtFilterDateModifiedTo").val(localStorage["RecruiterTaskListDateModifiedTo"]);

        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + RecruiterTaskViewURL + "?ID=" + rowObject.ID + "', 'divRecruiterTaskBodyModal');\">" + rowObject.ID + "</a>"; 
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
    
     objRecruiterTaskListJS.Initialize();
});