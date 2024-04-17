var objKPIListJS;
const KPIListURL = "/IPM/KPI?handler=List";
const KPIAddURL = "/IPM/KPI/Add";
const KPIViewURL = "/IPM/KPI/View";
const KPIEditURL = "/IPM/KPI/Edit";
const KPIDeleteURL = "/IPM/KPI/Delete";
const KPIAddPostURL = "/IPM/KPI/Add";
const KPIEditPostURL = "/IPM/KPI/Edit";
const GetCheckExportListURL = "/IPM/KPI?handler=CheckExportList";
const DownloadExportListURL = "/IPM/KPI?handler=DownloadExportList";
const UploadKPIURL = "/IPM/KPI/Upload?handler=UploadKPIList";
const DownloadKPIFormURL = "/IPM/KPI?handler=DownloadKPITemplate&ID=1";

const KRAGroupAddURL = "/IPM/KRAGroup/Add";
const KRAGroupAddPostURL = "/IPM/KRAGroup/Add";
const GetKRAGroupDropdownURL = "/IPM/KPI?handler=KRAGroupDropdown";

const KRASubGroupAddURL = "/IPM/KRASubGroup/Add";
const KRASubGroupAddPostURL = "/IPM/KRASubGroup/Add";
const GetKRASubGroupDropdownURL = "/IPM/KPI?handler=KRASubGroupDropdown";
const KRATypeDropDownURL = "/IPM/KPI?handler=ReferenceValue&RefCode=KRA_TYPE";
const KRAGroupAutoCompleteURL = "/IPM/KPI?handler=KRAGroupAutoComplete";
const KRASubGroupAutoCompleteURL = "/IPM/KPI?handler=KRASubGroupAutoComplete";
const KPITypeDropDownURL = "/IPM/KPI?handler=ReferenceValue&RefCode=KPI_TYPE";
const SourceTypeDropDownURL = "/IPM/KPI?handler=ReferenceValue&RefCode=KPI_SOURCE_TYPE";

$(document).ready(function() {
    objKPIListJS = {
        Initialize: function () {
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["IPMKPIListID"],
                Code: localStorage["IPMKPIListCode"],
                KRATypeDelimited: localStorage["IPMKPIListKRATypeDelimited"],
                KRAGroup: localStorage["IPMKPIListKRAGroup"],
                KRASubGroup: localStorage["IPMKPIListKRASubGroup"],
                Name: localStorage["IPMKPIListName"],
                OldKPICode: localStorage["IPMKPIListOldKPICode"],
                KPITypeDelimited: localStorage["IPMKPIListKPITypeDelimited"],
                SourceTypeDelimited: localStorage["IPMKPIListSourceTypeDelimited"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function() {
            var s = this;

            NumberOnly($("#txtFilterID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterID").val(),
                    Code: $("#txtFilterCode").val(),
                    KRATypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedKRAType").value,
                    KRAGroup: objEMSCommonJS.GetMultiSelectList("multiSelectedKRAGroup").value,
                    KRASubGroup: objEMSCommonJS.GetMultiSelectList("multiSelectedKRASubGroup").value,
                    Name: $("#txtFilterName").val(),
                    OldKPICode: $("#txtFilterOldCode").val(),
                    KPITypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedKPIType").value,
                    SourceTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedSourceType").value,
                };
                s.SetLocalStorage();
                ResetJQGridState("tblIPMKPIList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function() {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("#multiSelectedKRAType").html("");
                $("#multiSelectedKRAGroup").html("");
                $("#multiSelectedKRASubGroup").html("");
                $("#multiSelectedKRATypeOption label, #multiSelectedKRATypeOption input").prop("title", "add");
                $("#multiSelectedKRAGroupOption label, #multiSelectedKRAGroupOption input").prop("title", "add");
                $("#multiSelectedKRASubGroupOption label, #multiSelectedKRASubGroupOption input").prop("title", "add");
                $("#multiSelectedKPIType").html("");
                $("#multiSelectedKPITypeOption label, #multiSelectedKPITypeOption input").prop("title", "add");
                $("#multiSelectedSourceType").html("");
                $("#multiSelectedSourceTypeOption label, #multiSelectedSourceTypeOption input").prop("title", "add");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function() {
                LoadPartial(KPIAddURL, "divKPIBodyModal");
                $("#divKPIModal").modal("show");
            });

            $("#btnUpload").click(function () {
                objKPIUploadJS.UploadModal(UploadKPIURL, "Upload KPI", DownloadKPIFormURL);
                $('#divModalErrorMessage').html('');
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objKPIListJS.ExportFunction()",
                    "function");
               
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedKRAType", KRATypeDropDownURL);

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedKPIType", KPITypeDropDownURL);

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedSourceType", SourceTypeDropDownURL);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKRAGroup"
                , KRAGroupAutoCompleteURL, 20, "multiSelectedKRAGroup");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKRASubGroup"
                , KRASubGroupAutoCompleteURL, 20, "multiSelectedKRASubGroup");

        },

        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblIPMKPIList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblIPMKPIList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterID").val()
                + "&Code=" + $("#txtFilterCode").val()
                + "&KRATypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKRAType").value
                + "&KRAGroup=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKRAGroup").value
                + "&KRASubGroup=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKRASubGroup").value
                + "&Name=" + $("#txtFilterName").val()
                + "&OldKPICode=" + $("#txtFilterOldCode").val()
                + "&KPITypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKPIType").value
                + "&SourceTypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedSourceType").value

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
            var tableInfo = localStorage.getItem("tblIPMKPIList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMKPIList"));
            var moveFilterFields = function() {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function(n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function(e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMKPIList").jqGrid("GridUnload");
            $("#tblIPMKPIList").jqGrid("GridDestroy");
            $("#tblIPMKPIList").jqGrid({
                url: KPIListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Code", "KRA Type", "KRA Group", "KRA Sub Group", "KPI Name", "Old KPI Code", "KPI Type", "Source Type"],
                colModel: [
                    { hidden: true },
                    { width: 60, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objKPIListJS.ViewLink },
                    { width: 60, name: "Code", index: "Code", editable: true, align: "left" },
                    { width: 150, name: "KRAType", index: "KRAType", editable: true, align: "left" },
                    { width: 150, name: "KRAGroup", index: "KRAGroup", editable: true, align: "left" },
                    { width: 150, name: "KRASubGroup", index: "KRASubGroup", editable: true, align: "left" },
                    { width: 250, name: "Name", index: "Name", editable: true, align: "left" },
                    { width: 100, name: "OldKPICode", index: "OldKPICode", editable: true, align: "left" },
                    { width: 100, name: "KPIType", index: "KPIType", editable: true, align: "left", sortable: false },
                    { width: 100, name: "SourceType", index: "SourceType", editable: true, align: "left", sortable: false },
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
                    $("#tblIPMKPIList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function(data) {
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
                        //AutoSizeColumnJQGrid("tblIPMKPIList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function(n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblIPMKPIList .jqgrid-id-link").click(function() {
                            $('#divKPIModal').modal('show');
                        });
                    }

                    if (localStorage["KPIListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIListFilterOption"]));
                    }
                    objKPIListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objKPIListJS.ShowHideFilter();
                        localStorage["KPIListFilterOption"] = $("#chkFilter").is(":checked");
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
                loadError: function(xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function() {
                    GetJQGridState("tblIPMKPIList");
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
            localStorage["IPMKPIListID"] = $("#txtFilterID").val();
            localStorage["IPMKPIListCode"] = $("#txtFilterCode").val();

            localStorage["IPMKPIListKRATypeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKRAType").value;
            localStorage["IPMKPIListKRATypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKRAType").text;

            localStorage["IPMKPIListKRAGroup"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKRAGroup").value;
            localStorage["IPMKPIListKRAGroupText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKRAGroup").text;
            localStorage["IPMKPIListKRASubGroup"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKRASubGroup").value;
            localStorage["IPMKPIListKRASubGroupText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKRASubGroup").text;

            localStorage["IPMKPIListName"] = $("#txtFilterName").val();
            localStorage["IPMKPIListOldKPICode"] = $("#txtFilterOldCode").val();

            localStorage["IPMKPIListKPITypeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKPIType").value;
            localStorage["IPMKPIListKPITypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKPIType").text;

            localStorage["IPMKPIListSourceTypeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedSourceType").value;
            localStorage["IPMKPIListSourceTypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedSourceType").text;
        },

        GetLocalStorage: function () {
            $("#txtFilterID").val(localStorage["IPMKPIListID"]);
            $("#txtFilterCode").val(localStorage["IPMKPIListCode"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedKRAType"
                , "IPMKPIListKRATypeDelimited"
                , "IPMKPIListKRATypeDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedKRAGroup"
                , "IPMKPIListKRAGroup"
                , "IPMKPIListKRAGroupText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedKRASubGroup"
                , "IPMKPIListKRASubGroup"
                , "IPMKPIListKRASubGroupText");

            $("#txtFilterName").val(localStorage["IPMKPIListName"]);
            $("#txtFilterOldCode").val(localStorage["IPMKPIListOldKPICode"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedKPIType"
                , "IPMKPIListKPITypeDelimited"
                , "IPMKPIListKPITypeDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedSourceType"
                , "IPMKPIListSourceTypeDelimited"
                , "IPMKPIListSourceTypeDelimitedText");
        },

        ViewLink: function(cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + KPIViewURL + "?ID=" + rowObject.ID + "', 'divKPIBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
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

    objKPIListJS.Initialize();
});