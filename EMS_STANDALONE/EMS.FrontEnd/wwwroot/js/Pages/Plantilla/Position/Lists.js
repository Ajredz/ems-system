var objPositionListJS;
const PositionListURL = "/Plantilla/Position?handler=List";
const PositionLevelAutoCompleteURL = "/Plantilla/Position?handler=PositionLevelAutoComplete";
const ParentPositionAutoCompleteURL = "/Plantilla/Position?handler=ParentPositionAutoComplete";
const PositionAddURL = "/Plantilla/Position/Add";
const PositionViewURL = "/Plantilla/Position/View";
const PositionEditURL = "/Plantilla/Position/Edit";
const PositionDeleteURL = "/Plantilla/Position/Delete";
const PositionAddPostURL = "/Plantilla/Position/Add";
const PositionEditPostURL = "/Plantilla/Position/Edit";
const GetCheckExportListURL = "/Plantilla/Position?handler=CheckExportList";
const DownloadExportListURL = "/Plantilla/Position?handler=DownloadExportList";



$(document).ready(function() {
    objPositionListJS = {
        Initialize: function() {
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["PlantillaPositionListID"],
                PositionLevelIDs: localStorage["PlantillaPositionListPositionLevelIDs"],
                Code: localStorage["PlantillaPositionListCode"],
                Title: localStorage["PlantillaPositionListTitle"],
                //ParentPositionDelimited: localStorage["PlantillaPositionListParentPositionDelimited"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
            //$("#divPositionModal").mousedown(handle_mousedown);

        },

        ElementBinding: function() {
            var s = this;

            NumberOnly($("#txtFilterPositionID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterPositionID").val(),
                    PositionLevelIDs: objEMSCommonJS.GetMultiSelectList("multiSelectedPositionLevel").value,
                    Code: $("#txtFilterCode").val(),
                    Title: $("#txtFilterTitle").val(),
                    //ParentPositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedParentPosition").value,
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaPositionList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function() {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedPositionLevel").html("");
                //$("#multiSelectedParentPosition").html("");
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function() {
                LoadPartial(PositionAddURL, "divPositionBodyModal");
                $("#divPositionModal").modal("show");
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPositionLevel"
                , PositionLevelAutoCompleteURL, 20, "multiSelectedPositionLevel");

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterParentPosition"
            //    , ParentPositionAutoCompleteURL, 20, "multiSelectedParentPosition");

            $("#btnExport").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objPositionListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblPlantillaPositionList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaPositionList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterPositionID").val()
                + "&PositionLevelIDs=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPositionLevel").value
                + "&Code=" + $("#txtFilterCode").val()
                + "&Title=" + $("#txtFilterTitle").val()
            //+ "&ParentPositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedParentPosition").value

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
            var tableInfo = localStorage.getItem("tblPlantillaPositionList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaPositionList"));
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
            $("#tblPlantillaPositionList").jqGrid("GridUnload");
            $("#tblPlantillaPositionList").jqGrid("GridDestroy");
            $("#tblPlantillaPositionList").jqGrid({
                url: PositionListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Position Level", "Code", "Title"], //, "Parent Position"
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objPositionListJS.AddLink },
                    { name: "PositionLevelDescription", index: "PositionLevelDescription", editable: true, align: "left" },
                    { name: "Code", index: "Code", editable: true, align: "left" },
                    { name: "Title", index: "Title", editable: true, align: "left" },
                    //{ name: "ParentPositionDescription", index: "ParentPositionDescription", editable: true, align: "left" },
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
                    $("#tblPlantillaPositionList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblPlantillaPositionList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function(n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblPlantillaPositionList .jqgrid-id-link").click(function() {
                            $('#divPositionModal').modal('show');
                        });
                    }

                    if (localStorage["PositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["PositionListFilterOption"]));
                    }
                    objPositionListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objPositionListJS.ShowHideFilter();
                        localStorage["PositionListFilterOption"] = $("#chkFilter").is(":checked");
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
                loadError: function(xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function() {
                    GetJQGridState("tblPlantillaPositionList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage: function() {
            localStorage["PlantillaPositionListID"] = $("#txtFilterPositionID").val();
            localStorage["PlantillaPositionListPositionLevelIDs"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPositionLevel").value;
            localStorage["PlantillaPositionListPositionLevelIDsText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPositionLevel").text;
            localStorage["PlantillaPositionListCode"] = $("#txtFilterCode").val();
            localStorage["PlantillaPositionListTitle"] = $("#txtFilterTitle").val();
            //localStorage["PlantillaPositionListParentPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedParentPosition").value
            //localStorage["PlantillaPositionListParentPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedParentPosition").text
        },

        GetLocalStorage: function() {
            $("#txtFilterPositionID").val(localStorage["PlantillaPositionListID"]);
            $("#txtFilterCode").val(localStorage["PlantillaPositionListCode"]);
            $("#txtFilterTitle").val(localStorage["PlantillaPositionListTitle"]);

            //var positionLevelList, positionLevelListText = [];
            //positionLevelList = localStorage["PlantillaOrgGroupListPositionLevelIDs"] != undefined ? localStorage["PlantillaOrgGroupListPositionLevelIDs"].split(",") : [];
            //positionLevelListText = localStorage["PlantillaOrgGroupListPositionLevelIDsText"] != undefined ? localStorage["PlantillaOrgGroupListPositionLevelIDsText"].split(",") : [];
            //$(positionLevelList).each(function (index, item) {
            //    if (item != "" && item != undefined) {
            //        $("#multiSelectedPositionLevel").append('<label class="multiselect-item selected_' + item + '" title="delete">' + regionListText[index] +
            //            ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEMSCommonJS.RemoveFromMultiSelect(&quot;multiSelectedPositionLevel&quot;,&quot;' + item + '&quot;);"></span></label>');
            //        $("#multiSelectedPositionLevel").append('<input type="hidden" class="selected-item hdn_selected_' + item + '" value="' + item + '" />');
            //    }
            //});

            objEMSCommonJS.SetMultiSelectList("multiSelectedPositionLevel"
                , "PlantillaPositionListPositionLevelIDs"
                , "PlantillaPositionListPositionLevelIDsText");

            //objEMSCommonJS.SetMultiSelectList("multiSelectedParentPosition"
            //    , "PlantillaPositionListParentPositionDelimited"
            //    , "PlantillaPositionListParentPositionDelimitedText");

        },

        AddLink: function(cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + PositionViewURL + "?ID=" + rowObject.ID + "', 'divPositionBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
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

    objPositionListJS.Initialize();
});