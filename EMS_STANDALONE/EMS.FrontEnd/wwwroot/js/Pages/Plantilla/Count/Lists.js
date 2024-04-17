var objCountListJS;
const CountListURL = window.location.pathname + "?handler=List";
const OrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgGroupByOrgTypeAutoComplete";
const PositionAutoCompleteURL = window.location.pathname + "?handler=PositionAutoComplete";
const GetCheckExportCountByOrgTypeURL = window.location.pathname + "?handler=CheckExportCountByOrgType";
const DownloadExportCountByOrgTypeURL = window.location.pathname + "?handler=DownloadExportCountByOrgType";
$(document).ready(function () {
    objCountListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                OrgType: (localStorage["PlantillaCountListOrgType"] || ""),
                ScopeOrgType: $("#hdnOrgListFilter").val(),
                ScopeOrgGroupDelimited: localStorage["PlantillaCountListScopeOrgGroupDelimited"],
                OrgGroupDelimited: localStorage["PlantillaCountListOrgGroupDelimited"],
                PositionDelimited: localStorage["PlantillaCountListPositionDelimited"],
                PlannedMin: localStorage["PlantillaCountListPlannedMin"],
                PlannedMax: localStorage["PlantillaCountListPlannedMax"],
                ActiveMin: localStorage["PlantillaCountListActiveMin"],
                ActiveMax: localStorage["PlantillaCountListActiveMax"],
                InactiveMin: localStorage["PlantillaCountListInactiveMin"],
                InactiveMax: localStorage["PlantillaCountListInactiveMax"],
                VarianceMin: localStorage["PlantillaCountListVarianceMin"],
                VarianceMax: localStorage["PlantillaCountListVarianceMax"],
                IsExport: false /*Used for Export function*/

            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnlyWithNegative($("#txtFilterPlannedMin, #txtFilterPlannedMax, "+
                "#txtFilterActiveMin, #txtFilterActiveMax, " +
                "#txtFilterInactiveMin, #txtFilterInactiveMax, " +
                "#txtFilterVarianceMin, #txtFilterVarianceMax"
            ));

            $("#btnSearch").click(function () {
                $("#divPlantillaCountErrorMessage").html("");
                $("#ddlOrgType").removeClass("errMessage");
                var param = {
                    OrgType: $("#ddlOrgType").val(),
                    ScopeOrgType: $("#hdnOrgListFilter").val(),
                    ScopeOrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").value,
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    PlannedMin: $("#txtFilterPlannedMin").val(),
                    PlannedMax: $("#txtFilterPlannedMax").val(),
                    ActiveMin: $("#txtFilterActiveMin").val(),
                    ActiveMax: $("#txtFilterActiveMax").val(),
                    InactiveMin: $("#txtFilterInactiveMin").val(),
                    InactiveMax: $("#txtFilterInactiveMax").val(),
                    VarianceMin: $("#txtFilterVarianceMin").val(),
                    VarianceMax: $("#txtFilterVarianceMax").val(),
                    IsExport: false /*Used for Export function*/
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaCountList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedScopeOrgGroup").html("");
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedPosition").html("");
                $("#btnSearch").click();
            });


            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterScopeOrgGroup"
                , OrgGroupAutoCompleteURL + "&OrgType=" + $("#hdnOrgListFilter").val(), 20, "multiSelectedScopeOrgGroup");


            $("#ddlOrgType").change(function () {
                
                if ($("#txtFilterOrgGroup").prop('autocomplete') == "off") {
                    $("#txtFilterOrgGroup").autocomplete("destroy");
                    $("#txtFilterOrgGroup").removeData('autocomplete');
                }
                objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                    , OrgGroupAutoCompleteURL + "&OrgType=" + $("#ddlOrgType").val(), 20, "multiSelectedOrgGroup");

            });

            if ((localStorage["PlantillaCountListOrgType"] || "") != "") {
                objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                    , OrgGroupAutoCompleteURL + "&OrgType=" + localStorage["PlantillaCountListOrgType"], 20, "multiSelectedOrgGroup");
            }

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , PositionAutoCompleteURL, 20, "multiSelectedPosition");

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objCountListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {
            $("#divPlantillaCountErrorMessage").html("");
            $("#ddlOrgType").removeClass("errMessage");
            var parameters = "&sidx=" + $("#tblPlantillaCountList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaCountList").jqGrid("getGridParam", "sortorder")
                + "&OrgType=" + $("#ddlOrgType").val()
                + "&ScopeOrgType=" + $("#hdnOrgListFilter").val()
                + "&ScopeOrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").value
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                + "&PlannedMin=" + $("#txtFilterPlannedMin").val()
                + "&PlannedMax=" + $("#txtFilterPlannedMax").val()
                + "&ActiveMin=" + $("#txtFilterActiveMin").val()
                + "&ActiveMax=" + $("#txtFilterActiveMax").val()
                + "&InactiveMin=" + $("#txtFilterInactiveMin").val()
                + "&InactiveMax=" + $("#txtFilterInactiveMax").val()
                + "&VarianceMin=" + $("#txtFilterVarianceMin").val()
                + "&VarianceMax=" + $("#txtFilterVarianceMax").val();

            if (($("#ddlOrgType").val() || "") != "") {
                var GetSuccessFunction = function (data) {
                    if (data.IsSuccess) {
                        window.location = DownloadExportCountByOrgTypeURL + parameters
                        $("#divModal").modal("hide");
                    }
                    else {
                        ModalAlert(MODAL_HEADER, data.Result);
                    }
                };
                objEMSCommonJS.GetAjax(GetCheckExportCountByOrgTypeURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
            }
            else {
                $("#divPlantillaCountErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                $("#ddlOrgType").addClass("errMessage");
            }
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaCountList"));
            
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

            $("#tblPlantillaCountList").jqGrid("GridUnload");
            $("#tblPlantillaCountList").jqGrid("GridDestroy");
            $("#tblPlantillaCountList").jqGrid({
                url: CountListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Filter By Region", "Org Group", "Position", "Inactive", "Planned", "Active-REG", "Active-PROB", "Active-OUT", "Total Active",  "Variance"],
                colModel: [
                    { hidden: true },
                    { hidden: true, name: "ScopeOrgGroup", index: "ScopeOrgGroup", editable: true, align: "left", sortable: false },
                    { name: "OrgGroup", index: "OrgGroup", editable: true, align: "left", sortable: true },
                    { name: "Position", index: "Position", editable: true, align: "left", sortable: true },
                    { hidden: true,name: "InactiveCount", index: "InactiveCount", editable: true, align: "right" },
                    { name: "PlannedCount", index: "PlannedCount", editable: true, align: "right" },
                    { name: "ActiveCount", index: "ActiveCount", editable: true, align: "right" },
                    { name: "ActiveProbCount", index: "ActiveProbCount", editable: true, align: "right" },
                    { name: "OutgoingCount", index: "OutgoingCount", editable: true, align: "right" },
                    { name: "TotalActiveCount", index: "TotalActiveCount", editable: true, align: "right" },
                    { name: "VarianceCount", index: "VarianceCount", editable: true, align: "right" },
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
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        // Show Filter By Region Column if user is non admin
                        if (window.location.pathname.indexOf("admin") >= 0) {
                            $("#tblPlantillaCountList").jqGrid('showCol', ["ScopeOrgGroup"]);
                        }

                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {

                            }   
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblPlantillaCountList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblPlantillaCountList .jqgrid-id-link").click(function () {
                            $('#divCountModal').modal('show');
                        });

                        if (data.rows.length > 0) {
                            $("#jqgh_tblPlantillaCountList_InactiveCount").html("Inactive (" + data.rows[0]["TotalInactive"] + ")");
                            $("#jqgh_tblPlantillaCountList_PlannedCount").html("Planned (" + data.rows[0]["TotalPlanned"] + ")");
                            $("#jqgh_tblPlantillaCountList_ActiveCount").html("Active-REG (" + data.rows[0]["TotalActiveReg"] + ")");
                            $("#jqgh_tblPlantillaCountList_ActiveProbCount").html("Active-PROB (" + data.rows[0]["TotalActiveProb"] + ")");
                            $("#jqgh_tblPlantillaCountList_OutgoingCount").html("Active-OUT (" + data.rows[0]["TotalOutgoing"] + ")");
                            $("#jqgh_tblPlantillaCountList_TotalActiveCount").html("Total Active (" + data.rows[0]["TotalActive"] + ")");
                            $("#jqgh_tblPlantillaCountList_VarianceCount").html("Variance (" + data.rows[0]["TotalVariance"] + ")");
                        }

                        //$(".ui-jqgrid .ui-jqgrid-htable th").addCss("height","100px");
                    }

                    if (localStorage["CountListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["CountListFilterOption"]));
                    }
                    objCountListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objCountListJS.ShowHideFilter();
                        localStorage["CountListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblPlantillaCountList");
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
            localStorage["PlantillaCountListOrgType"] = $("#ddlOrgType").val();

            localStorage["PlantillaCountListScopeOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").value;
            localStorage["PlantillaCountListScopeOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").text;

            localStorage["PlantillaCountListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["PlantillaCountListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;

            localStorage["PlantillaCountListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["PlantillaCountListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;

            localStorage["PlantillaCountListPlannedMin"] = $("#txtFilterPlannedMin").val()
            localStorage["PlantillaCountListPlannedMax"] = $("#txtFilterPlannedMax").val()
            localStorage["PlantillaCountListActiveMin"] = $("#txtFilterActiveMin").val()
            localStorage["PlantillaCountListActiveMax"] = $("#txtFilterActiveMax").val()
            localStorage["PlantillaCountListInactiveMin"] = $("#txtFilterInactiveMin").val()
            localStorage["PlantillaCountListInactiveMax"] = $("#txtFilterInactiveMax").val()
            localStorage["PlantillaCountListVarianceMin"] = $("#txtFilterVarianceMin").val()
            localStorage["PlantillaCountListVarianceMax"] = $("#txtFilterVarianceMax").val()

        },

        GetLocalStorage: function () {
            $("#ddlOrgType").val(localStorage["PlantillaCountListOrgType"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "PlantillaCountListOrgGroupDelimited"
                , "PlantillaCountListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedScopeOrgGroup"
                , "PlantillaCountListScopeOrgGroupDelimited"
                , "PlantillaCountListScopeOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "PlantillaCountListPositionDelimited"
                , "PlantillaCountListPositionDelimitedText");

            $("#txtFilterPlannedMin").val(localStorage["PlantillaCountListPlannedMin"]);
            $("#txtFilterPlannedMax").val(localStorage["PlantillaCountListPlannedMax"]);
            $("#txtFilterActiveMin").val(localStorage["PlantillaCountListActiveMin"]);
            $("#txtFilterActiveMax").val(localStorage["PlantillaCountListActiveMax"]);
            $("#txtFilterInactiveMin").val(localStorage["PlantillaCountListInactiveMin"]);
            $("#txtFilterInactiveMax").val(localStorage["PlantillaCountListInactiveMax"]);
            $("#txtFilterVarianceMin").val(localStorage["PlantillaCountListVarianceMin"]);
            $("#txtFilterVarianceMax").val(localStorage["PlantillaCountListVarianceMax"]);
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
    
     objCountListJS.Initialize();
});