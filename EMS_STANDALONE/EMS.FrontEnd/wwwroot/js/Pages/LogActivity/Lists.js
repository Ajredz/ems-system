var objLogActivityListJS;

const LogActivityListURL = "/LogActivity/Maintenance?handler=List";
const LogActivityViewURL = "/LogActivity/Maintenance/View";
const LogActivityAddURL = "/LogActivity/Maintenance/Add";
const LogActivityAddPostURL = "/LogActivity/Maintenance/Add";
const LogActivityEditURL = "/LogActivity/Maintenance/Edit";
const LogActivityEditPostURL = "/LogActivity/Maintenance/Edit";
const LogActivityDeleteURL = "/LogActivity/Maintenance/Delete";

const ModuleAutoCompleteURL = "/LogActivity/Maintenance?handler=ModuleAutoComplete";
const TypeAutoCompleteURL = "/LogActivity/Maintenance?handler=TypeAutoComplete";
const GetYesNoOptionURL = "/LogActivity/Maintenance?handler=ReferenceValue&RefCode=YES_NO";
const GetSubTypeURL = "/LogActivity/Maintenance?handler=LogActivitySubType";
const SubTypeDropdownChangeURL = "/LogActivity/Maintenance?handler=SubType";

$(document).ready(function () {
    objLogActivityListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                ModuleDelimited: localStorage["LogActivityListModuleDelimited"],
                TypeDelimited: localStorage["LogActivityListTypeDelimited"],
                SubTypeDelimited: localStorage["LogActivityListSubType"],
                Title: localStorage["LogActivityListTitle"],
                Description: localStorage["LogActivityListDescription"],
                IsPassFail: localStorage["LogActivityListIsPassFail"],
                IsAssignment: localStorage["LogActivityListIsAssignment"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSearch").click(function () {
                var param = {
                    ModuleDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedModule").value,
                    TypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedType").value,
                    SubTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").value,
                    Title: $("#txtFilterTitle").val(),
                    Description: $("#txtFilterDescription").val(),
                    IsPassFail: objEMSCommonJS.GetMultiSelectList("multiSelectedIsPassFail").value,
                    IsAssignment: objEMSCommonJS.GetMultiSelectList("multiSelectedIsAssignment").value
                };
                s.SetLocalStorage();
                ResetJQGridState("tblLogActivityList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedModule").html("");
                $("#multiSelectedType").html("");
                $("#multiSelectedSubType").html("");
                $("#multiSelectedSubTypeOption label, #multiSelectedSubTypeOption input").prop("title", "add");
                $("#multiSelectedIsPassFail").html("");
                $("#multiSelectedIsPassFailOption label, multiSelectedIsPassFailOption input").prop("title", "add");
                $("#multiSelectedIsAssignment").html("");
                $("#multiSelectedIsAssignmentOption label, multiSelectedIsAssignmentOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(LogActivityAddURL, "divLogActivityBodyModal");
                $("#divLogActivityModal").modal("show");
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterModule", ModuleAutoCompleteURL, 20, "multiSelectedModule");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterType", TypeAutoCompleteURL, 20, "multiSelectedType");

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedIsPassFail", GetYesNoOptionURL);
            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedIsAssignment", GetYesNoOptionURL);
            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedSubType", GetSubTypeURL, "Value", "Description");

        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblLogActivityList") == "" ? "" : $.parseJSON(localStorage.getItem("tblLogActivityList"));
            
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

            $("#tblLogActivityList").jqGrid("GridUnload");
            $("#tblLogActivityList").jqGrid("GridDestroy");
            $("#tblLogActivityList").jqGrid({
                url: LogActivityListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "", "Module", "Task Type", "Sub-Type", "Title", "Description", "Is with Pass/Fail ?", "Is with Assignment ?"],
                colModel: [
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center", sortable: true },
                    { name: "", index: "View", width: 40, align: "center", sortable: true, formatter: objLogActivityListJS.AddLink },
                    { name: "Module", index: "Module", editable: true, align: "left", sortable: true },
                    { name: "Type", index: "Type", editable: true, align: "left", sortable: true },
                    { name: "SubType", index: "SubType", align: "left", sortable: false },
                    { name: "Title", index: "Title", editable: true, align: "left", sortable: true },
                    { name: "Description", index: "Description", editable: true, align: "left", sortable: true },
                    { name: "IsPassFail", index: "IsPassFail", editable: true, align: "center", sortable: true },
                    { name: "IsAssignment", index: "IsAssignment", editable: true, align: "center", sortable: true }
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
                    $("#tblLogActivityList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblLogActivityList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblLogActivityList .jqgrid-id-link").click(function () {
                            $('#divLogActivityModal').modal('show');
                        });

                    }

                    if (localStorage["LogActivityListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["LogActivityListFilterOption"]));
                    }
                    objLogActivityListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objLogActivityListJS.ShowHideFilter();
                        localStorage["LogActivityListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblLogActivityList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            //jQuery("#tblLogActivityList").jqGrid('filterToolbar', { searchOnEnter: true, stringResult: true });
        },

        SetLocalStorage: function () {
            localStorage["LogActivityListTitle"] = $("#txtFilterTitle").val();
            localStorage["LogActivityListDescription"] = $("#txtFilterDescription").val();

            localStorage["LogActivityListModuleDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedModule").value;
            localStorage["LogActivityListModuleDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedModule").text;

            localStorage["LogActivityListTypeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedType").value;
            localStorage["LogActivityListTypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedType").text;

            localStorage["LogActivityListSubType"] = objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").value;
            localStorage["LogActivityListSubTypeText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedSubType").text;

            localStorage["LogActivityListIsPassFail"] = objEMSCommonJS.GetMultiSelectList("multiSelectedIsPassFail").value;
            localStorage["LogActivityListIsPassFailText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedIsPassFail").text;

            localStorage["LogActivityListIsAssignment"] = objEMSCommonJS.GetMultiSelectList("multiSelectedIsAssignment").value;
            localStorage["LogActivityListIsAssignmentText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedIsAssignment").text;

            localStorage["LogActivityListIsPreLoaded"] = objEMSCommonJS.GetMultiSelectList("multiSelectedIsPreLoaded").value;
            localStorage["LogActivityListIsPreLoadedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedIsPreLoaded").text;
        },

        GetLocalStorage: function () {
            $("#txtFilterTitle").val(localStorage["LogActivityListTitle"]);
            $("#txtFilterDescription").val(localStorage["LogActivityListDescription"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedModule"
                , "LogActivityListModuleDelimited"
                , "LogActivityListModuleDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedType"
                , "LogActivityListTypeDelimited"
                , "LogActivityListTypeDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedSubType"
                , "LogActivityListSubType"
                , "LogActivityListSubTypeText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedIsPassFail"
                , "LogActivityListIsPassFail"
                , "LogActivityListIsPassFailText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedIsAssignment"
                , "LogActivityListIsAssignment"
                , "LogActivityListIsAssignmentText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedIsPreLoaded"
                , "LogActivityListIsPreLoaded"
                , "LogActivityListIsPreLoadedText");
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + LogActivityViewURL + "?ID=" + rowObject.ID + "', 'divLogActivityBodyModal');\">View</a>"; 
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
    
     objLogActivityListJS.Initialize();
});