var objKPIPositionListJS;
const KPIPositionListURL = "/IPM/KPIPosition?handler=List";
const KPIPositionAddURL = "/IPM/KPIPosition/Add";
const KPIPositionViewURL = "/IPM/KPIPosition/View";
const KPIPositionEditURL = "/IPM/KPIPosition/Edit";
const KPIPositionDeleteURL = "/IPM/KPIPosition/Delete";
const KPIPositionAddPostURL = "/IPM/KPIPosition/Add";
const KPIPositionEditPostURL = "/IPM/KPIPosition/Edit";
const ValidateUploadInsertKPIPositionURL = "/IPM/KPIPosition/UploadInsert?handler=ValidateUploadInsertKPIPosition";
const UploadInsertKPIPositionURL = "/IPM/KPIPosition/UploadInsert?handler=UploadInsertKPIPosition";
const DownloadKPIPositionFormURL = "/IPM/KPIPosition?handler=DownloadKPIPositionTemplate&ID=1";
const GetCheckExportListURL = "/IPM/KPIPosition?handler=CheckExportList";
const DownloadExportListURL = "/IPM/KPIPosition?handler=DownloadExportList";

const GetKPIAutoCompleteURL = "/IPM/KPIPosition?handler=KPIAutoComplete";
const GetKPIListURL = "/IPM/KPIPosition?handler=KPIList";
const GetPositionAutoCompleteURL = "/IPM/KPIPosition?handler=PositionAutoComplete";

const CopyKPIPositionModalURL = "/IPM/KPIPosition/CopyKpi";
const CopyKPIPositionAddPostURL = "/IPM/KPIPosition/CopyKpi";
const GetCopyPositionAutoCompleteURL = "/IPM/KPIPosition?handler=CopyPositionAutoComplete";
const GetCopyPositionResultAutoCompleteURL = "/IPM/KPIPosition?handler=CopyPositionResultAutoComplete";

$(document).ready(function() {
    objKPIPositionListJS = {
        Initialize: function () {
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["IPMKPIPositionListID"],
                PositionDelimited: localStorage["IPMKPIPositionListPositionDelimited"],
                Weight: localStorage["IPMKPIPositionListWeight"],
                DateEffectiveFrom: localStorage["IPMKPIPositionListDateEffectiveFrom"],
                DateEffectiveTo: localStorage["IPMKPIPositionListDateEffectiveTo"],
                IsShowRecentOnly: $("#cbShowRecentOnly").prop("checked")
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function() {
            var s = this;

            NumberOnly($("#txtFilterID"));

            $("#txtFilterDateEffectiveFrom, #txtFilterDateEffectiveTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterID").val(),
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    Weight: $("#txtFilterWeight").val(),
                    DateEffectiveFrom: $("#txtFilterDateEffectiveFrom").val(),
                    DateEffectiveTo: $("#txtFilterDateEffectiveTo").val(),
                    IsShowRecentOnly: $("#cbShowRecentOnly").prop("checked")
                };
                s.SetLocalStorage();
                ResetJQGridState("tblIPMKPIPositionList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function() {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedPosition").html("");
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function() {
                LoadPartial(KPIPositionAddURL, "divKPIPositionBodyModal");
                $("#divKPIPositionModal").modal("show");
            });

            $("#btnUploadInsert").click(function () {
                objKPIPositionUploadJS.UploadModal(ValidateUploadInsertKPIPositionURL, "Upload KPI per Position", DownloadKPIPositionFormURL);
                $('#divModalErrorMessage').html('');
            });

            $("#cbShowRecentOnly").click(function () {
                $("#btnSearch").click();
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , GetPositionAutoCompleteURL, 20, "multiSelectedPosition");

            $("#btnExport").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objKPIPositionListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblIPMKPIPositionList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblIPMKPIPositionList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterID").val()
                + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                + "&Weight=" + $("#txtFilterWeight").val()
                + "&DateEffectiveFrom=" + $("#txtFilterDateEffectiveFrom").val()
                + "&DateEffectiveTo=" + $("#txtFilterDateEffectiveTo").val()
                + "&IsShowRecentOnly=" + $("#cbShowRecentOnly").prop("checked")

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

        LoadJQGrid: function(param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMKPIPositionList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMKPIPositionList"));
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
            $("#tblIPMKPIPositionList").jqGrid("GridUnload");
            $("#tblIPMKPIPositionList").jqGrid("GridDestroy");
            $("#tblIPMKPIPositionList").jqGrid({
                url: KPIPositionListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Position", "Weight", "Effective Date"],
                colModel: [
                    { hidden: true },
                    { width: 24, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objKPIPositionListJS.ViewLink },
                    { width: 200, name: "Position", index: "Position", editable: true, align: "left" },
                    { width: 50, name: "Weight", index: "Weight", editable: true, align: "right" },
                    { width: 50, name: "EffectiveDate", index: "EffectiveDate", editable: true, align: "center" },
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
                    $("#tblIPMKPIPositionList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        //AutoSizeColumnJQGrid("tblIPMKPIPositionList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function(n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblIPMKPIPositionList .jqgrid-id-link").click(function() {
                            $('#divKPIPositionModal').modal('show');
                        });
                    }

                    if (localStorage["KPIPositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIPositionListFilterOption"]));
                    }
                    objKPIPositionListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objKPIPositionListJS.ShowHideFilter();
                        localStorage["KPIPositionListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblIPMKPIPositionList");
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
            localStorage["IPMKPIPositionListID"] = $("#txtFilterID").val();
            localStorage["IPMKPIPositionListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["IPMKPIPositionListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;
            localStorage["IPMKPIPositionListWeight"] = $("#txtFilterWeight").val();
            localStorage["IPMKPIPositionListDateEffectiveFrom"] = $("#txtFilterDateEffectiveFrom").val();
            localStorage["IPMKPIPositionListDateEffectiveTo"] = $("#txtFilterDateEffectiveTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterID").val(localStorage["IPMKPIPositionListID"]);
            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "IPMKPIPositionListPositionDelimited"
                , "IPMKPIPositionListPositionDelimitedText");
            $("#txtFilterWeight").val(localStorage["IPMKPIPositionListWeight"]);
            $("#txtFilterDateEffectiveFrom").val(localStorage["IPMKPIPositionListDateEffectiveFrom"]);
            $("#txtFilterDateEffectiveTo").val(localStorage["IPMKPIPositionListDateEffectiveTo"]);
        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + KPIPositionViewURL + "?ID=" + rowObject.ID + "&EffectiveDate=" + rowObject.EffectiveDate + "', 'divKPIPositionBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
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

    objKPIPositionListJS.Initialize();
});