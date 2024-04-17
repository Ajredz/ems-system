var objAccountabilityListJS;

const AccountabilityListURL = "/LogActivity/Accountability?handler=List";
const AccountabilityViewURL = "/LogActivity/Accountability/View";
const AccountabilityAddURL = "/LogActivity/Accountability/Add";
const AccountabilityAddPostURL = "/LogActivity/Accountability/Add";
const AccountabilityEditURL = "/LogActivity/Accountability/Edit";
const AccountabilityEditPostURL = "/LogActivity/Accountability/Edit";
const AccountabilityDeleteURL = "/LogActivity/Accountability/Delete";

const GetAccountabilityDetailsURL = "/LogActivity/Accountability?handler=AccountabilityDetails";
const GetTypeDropDownURL = "/LogActivity/Accountability?handler=ReferenceValue&RefCode=ACCOUNTABILITY_TYPE";
const OrgGroupAutoCompleteURL = "/LogActivity/Accountability?handler=OrgTypeAutoComplete";
const GetCheckExportListURL = "/LogActivity/Accountability?handler=CheckExportList";
const DownloadExportListURL = "/LogActivity/Accountability?handler=DownloadExportList";

const PositionAutoCompleteURL = "/LogActivity/Accountability?handler=PositionAutoComplete";
const EmployeeAutoCompleteURL = "/LogActivity/Accountability?handler=EmployeeAutoComplete";


$(document).ready(function () {
    objAccountabilityListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["AccountabilityListID"],
                PreloadName: localStorage["AccountabilityListPreloadName"],
                DateCreatedFrom: localStorage["AccountabilityListDateCreatedFrom"],
                DateCreatedTo: localStorage["AccountabilityListDateCreatedTo"]
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
                    ID: $("#txtFilterAccountabilityID").val(),
                    PreloadName: $("#txtFilterPreloadName").val(),
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAccountabilityList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(AccountabilityAddURL, "divAccountabilityBodyModal");
                $("#divAccountabilityModal").modal("show");
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objAccountabilityListJS.ExportFunction()",
                    "function");
                
            });
        },


        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblAccountabilityList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblAccountabilityList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterAccountabilityID").val()
                + "&PreloadName=" + $("#txtFilterPreloadName").val()
                + "&DateCreatedFrom=" + $("#txtFilterDateCreatedFrom").val()
                + "&DateCreatedTo=" + $("#txtFilterDateCreatedTo").val()

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblAccountabilityList") == "" ? "" : $.parseJSON(localStorage.getItem("tblAccountabilityList"));
            
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

            $("#tblAccountabilityList").jqGrid("GridUnload");
            $("#tblAccountabilityList").jqGrid("GridDestroy");
            $("#tblAccountabilityList").jqGrid({
                url: AccountabilityListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Preload Name", "Created Date"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objAccountabilityListJS.AddLink },
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
                    $("#tblAccountabilityList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblAccountabilityList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblAccountabilityList .jqgrid-id-link").click(function () {
                            $('#divAccountabilityModal').modal('show');
                        });

                    }

                    if (localStorage["AccountabilityListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["AccountabilityListFilterOption"]));
                    }
                    objAccountabilityListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objAccountabilityListJS.ShowHideFilter();
                        localStorage["AccountabilityListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblAccountabilityList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            //jQuery("#tblAccountabilityList").jqGrid('filterToolbar', { searchOnEnter: true, stringResult: true });
        },

        SetLocalStorage: function () {
            localStorage["AccountabilityListID"] = $("#txtFilterAccountabilityID").val();
            localStorage["AccountabilityListName"] = $("#txtFilterPreloadName").val();
            localStorage["AccountabilityListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["AccountabilityListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterAccountabilityID").val(localStorage["AccountabilityListID"]);
            $("#txtFilterPreloadName").val(localStorage["AccountabilityListName"]);
            $("#txtFilterDateCreatedFrom").val(localStorage["AccountabilityDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["AccountabilityDateCreatedTo"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + AccountabilityViewURL + "?ID=" + rowObject.ID + "', 'divAccountabilityBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID)+"</a>"; 
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
    
     objAccountabilityListJS.Initialize();
});