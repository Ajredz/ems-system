var objPreloadedListJS;

const PreloadedListURL = "/LogActivity/Preloaded?handler=List";
const PreloadedViewURL = "/LogActivity/Preloaded/View";
const PreloadedAddURL = "/LogActivity/Preloaded/Add";
const PreloadedAddPostURL = "/LogActivity/Preloaded/Add";
const PreloadedEditURL = "/LogActivity/Preloaded/Edit";
const PreloadedEditPostURL = "/LogActivity/Preloaded/Edit";
const PreloadedDeleteURL = "/LogActivity/Preloaded/Delete";

//const GetLogActivityDataURL = "/LogActivity/Preloaded?handler=LogActivityData";
const GetPreloadedItemsURL = "/LogActivity/Preloaded?handler=PreloadedItems";
const GetModuleDropDownURL = "/LogActivity/Preloaded?handler=ReferenceValue&RefCode=ACTIVITY_MODULE";
const GetTypeDropDownURL = "/LogActivity/Preloaded?handler=ReferenceValue&RefCode=ACTIVITY_TYPE";
const GetSubTypeDropDownURL = "/LogActivity/Preloaded?handler=SubType";
const AssignedUserAutoComplete = "/LogActivity/Preloaded?handler=AssignedUser";
const GetCheckExportListURL = "/LogActivity/Preloaded?handler=CheckExportList";
const DownloadExportListURL = "/LogActivity/Preloaded?handler=DownloadExportList";


$(document).ready(function () {
    objPreloadedListJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                ID: localStorage["PreloadedListID"],
                PreloadName: localStorage["PreloadedListPreloadName"],
                DateCreatedFrom: localStorage["PreloadedListDateCreatedFrom"],
                DateCreatedTo: localStorage["PreloadedListDateCreatedTo"]
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

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterPreloadedID").val(),
                    PreloadName: $("#txtFilterPreloadName").val(),
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPreloadedList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(PreloadedAddURL, "divPreloadedBodyModal");
                $("#divPreloadedModal").modal("show");
            });

            $("#btnExport").click(function () {

                var parameters = "&sidx=" + $("#tblPreloadedList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblPreloadedList").jqGrid("getGridParam", "sortorder")
                    + "&ID=" + $("#txtFilterPreloadedID").val()
                    + "&PreloadName=" + $("#txtFilterPreloadName").val()
                    + "&DateCreatedFrom=" + $("#txtFilterDateCreatedFrom").val()
                    + "&DateCreatedTo=" + $("#txtFilterDateCreatedTo").val()

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
            var tableInfo = localStorage.getItem("tblPreloadedList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPreloadedList"));
            
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

            $("#tblPreloadedList").jqGrid("GridUnload");
            $("#tblPreloadedList").jqGrid("GridDestroy");
            $("#tblPreloadedList").jqGrid({
                url: PreloadedListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Preloaded Task Name", "Created Date"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objPreloadedListJS.AddLink },
                    { name: "PreloadName", index: "PreloadName", editable: true, align: "left", sortable: true },
                    { name: "DateCreated", index: "DateCreated", editable: true, align: "left", sortable: true },

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
                    $("#tblPreloadedList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblPreloadedList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblPreloadedList .jqgrid-id-link").click(function () {
                            $('#divPreloadedModal').modal('show');
                        });

                    }

                    if (localStorage["PreloadedListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PreloadedListFilterOption"]));
                    }
                    objPreloadedListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objPreloadedListJS.ShowHideFilter();
                        localStorage["PreloadedListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblPreloadedList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            //jQuery("#tblPreloadedList").jqGrid('filterToolbar', { searchOnEnter: true, stringResult: true });
        },

        SetLocalStorage: function () {
            localStorage["PreloadedListID"] = $("#txtFilterPreloadedID").val();
            localStorage["PreloadedListName"] = $("#txtFilterPreloadName").val();
            localStorage["PreloadedListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["PreloadedListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterPreloadedID").val(localStorage["PreloadedListID"]);
            $("#txtFilterPreloadName").val(localStorage["PreloadedListName"]);
            $("#txtFilterDateCreatedFrom").val(localStorage["PreloadedDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["PreloadedDateCreatedTo"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + PreloadedViewURL + "?ID=" + rowObject.ID + "', 'divPreloadedBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID)+"</a>"; 
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
    
     objPreloadedListJS.Initialize();
});