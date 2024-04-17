var objEmployeeScoreApprovalViewJS;
var KPIDropDownOptions = [];
var initialTotalScore = 0;
var initialTotalWeight = 0;
var firstLoad = true;
const SourceTypeDropDownURL = "/IPM/EmployeeScoreKeyIn?handler=ReferenceValue&RefCode=KPI_SOURCE_TYPE";

$(document).ready(function () {
    objEmployeeScoreApprovalViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divEmployeeScoreApprovalBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divEmployeeScoreApprovalModal .form-control").prop("disabled", true);
            $("#divEmployeeScoreKPIScoreList .form-control").prop("disabled", false);

            $("#txtRemarks").css("resize", "none");
            var param = {
                ID: $("#hdnID").val()
            };
            s.LoadEmployeeScoreJQGrid(param);

            objEmployeeScoreApprovalViewJS.LoadEmployeeScoreStatusHistoryJQGrid({
                ID: $("#hdnID").val(),
            });

            $('#lblRunID').text(objEMSCommonJS.JQGridIDFormat($('#lblRunID').text()));

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabScore', '#divEmployeeScoreApprovalBodyModal');
        },

        DeleteSuccessFunction: function () {
            $("##divEmployeeScoreApprovalList #btnSearch").click();
            $('#divEmployeeScoreApprovalModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnEdit").click(function () {
                LoadPartial(EmployeeScoreApprovalEditURL +
                    "?ID=" + objEmployeeScoreApprovalViewJS.ID, "divEmployeeScoreApprovalBodyModal");
            });

            $("#btnEmployeeExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeScoreApprovalViewJS.ExportFunction()",
                    "function");
            });


            $("#divEmployeeScoreApprovalBodyModal .modal-body").scroll(function () {
                if ($("#divEmployeeScoreApprovalBodyModal .modal-body").scrollTop() >= 376) {

                    $("#divEmployeeScoreApprovalBodyModal .ui-jqgrid-htable").css('cssText'
                        , 'width: ' + $("#divEmployeeScoreApprovalBodyModal .ui-jqgrid-hbox").width() + 'px !important');

                    $("#divEmployeeScoreApprovalBodyModal .ui-jqgrid-htable").css({
                        "max-width": $("#divEmployeeScoreApprovalBodyModal .ui-jqgrid-hbox").width() + "px",
                        "position": "fixed",
                        "z-index": "999",
                        "top": $("#divEmployeeScoreApprovalModal").position().top > 0 ? "37px" : "67px"
                    });
                }
                else {
                    $("#divEmployeeScoreApprovalBodyModal .ui-jqgrid-htable").css({
                        "position": "relative",
                        "z-index": "auto",
                        "max-width": "auto",
                        "top": "auto"
                    });
                }
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , EmployeeScoreDeleteURL + '?ID=' + objEmployeeScoreApprovalViewJS.ID\
                    , {} \
                    , '#divEmployeeScoreErrorMessage' \
                    , '#btnDelete' \
                    , objEmployeeScoreApprovalViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#divEmployeeScoreKPIScoreList #btnSearch").click(function () {
                var param = {
                    ID: $("#hdnID").val(),
                    KRAGroup: $("#txtFilterKRAGroup").val(),
                    KPICode: $("#txtFilterKPICode").val(),
                    KPIName: $("#txtFilterKPIName").val(),
                    KPIDescription: $("#txtFilterKPIDescription").val(),
                    KPIGuidelines: $("#txtFilterKPIGuidelines").val(),
                    WeightMin: $("#txtFilterWeightMin").val(),
                    WeightMax: $("#txtFilterWeightMax").val(),
                    TargetMin: $("#txtFilterTargetMin").val(),
                    TargetMax: $("#txtFilterTargetMax").val(),
                    ActualMin: $("#txtFilterActualMin").val(),
                    ActualMax: $("#txtFilterActualMax").val(),
                    RateMin: $("#txtFilterRateMin").val(),
                    RateMax: $("#txtFilterRateMax").val(),
                    TotalMin: $("#txtFilterTotalMin").val(),
                    TotalMax: $("#txtFilterTotalMax").val(),
                    GradeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedGrade").value,
                    SourceTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedSourceType").value,

                };
                s.LoadEmployeeScoreJQGrid(param);
            });

            AmountOnly($("#txtFilterWeightMin"));
            AmountOnly($("#txtFilterWeightMax"));
            AmountOnly($("#txtFilterTargetMin"));
            AmountOnly($("#txtFilterTargetMax"));
            AmountOnly($("#txtFilterActualMin"));
            AmountOnly($("#txtFilterActualMax"));
            AmountOnly($("#txtFilterRateMin"));
            AmountOnly($("#txtFilterRateMax"));
            AmountOnly($("#txtFilterTotalMin"));
            AmountOnly($("#txtFilterTotalMax"));

            $("#divEmployeeScoreKPIScoreList #btnReset").click(function () {
                $("#divEmployeeScoreKPIScoreList div.filterFields input[type='search']").val("");
                $("#divEmployeeScoreKPIScoreList div.filterFields select").val("");
                $("#divEmployeeScoreKPIScoreList div.filterFields input[type='checkbox']").prop("checked", true);
                $("#divEmployeeScoreKPIScoreList #multiSelectedGrade").html("");
                $("#divEmployeeScoreKPIScoreList #multiSelectedGradeOption label, #divEmployeeScoreKPIScoreList #multiSelectedGradeOption input").prop("title", "add");
                $("#multiSelectedSourceType").html("");
                $("#multiSelectedSourceTypeOption label, #multiSelectedSourceTypeOption input").prop("title", "add");
                $("#divEmployeeScoreKPIScoreList #btnSearch").click();
            });

            //$("#btnApprove").click(function () {
            //    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_APPROVE,
            //        "objEMSCommonJS.PostAjax(true \
            //              , EmployeeScoreApprovalURL \
            //              , objEmployeeScoreApprovalViewJS.GetFormData() \
            //              , '#divEmployeeScoreErrorMessage' \
            //              , '#btnSave' \
            //              , objEmployeeScoreApprovalViewJS.ApproveSuccessFunction);", "function");
            //});
            

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedGrade", GetRatingGradesDropdownURL, "Code", "Code");
            $("#multiSelectedGradeOption").css({ "min-width": "50px" });

           objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedSourceType", SourceTypeDropDownURL);
        },

        ExportFunction: function () {
            var parameters = "&ID=" + objEmployeeScoreApprovalViewJS.ID

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadExportEmployeeURL + parameters;
                    $("#divModal").modal("hide");

                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(CheckExportEmployeeURL + parameters, {}, "#btnEmployeeExport", GetSuccessFunction, null, true);
        },


        AddChildrenScoreDynamicFields: function () {
            var s = this;
            var htmlId = $(".ChildrenScoreDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenScoreDynamicFields_", "")) + 1;

            $("#DivChildrenScoreDynamicFields").append(
                "<tbody class=\"ChildrenScoreDynamicFields\" id=\"ChildrenScoreDynamicFields_" + ctr + "\"> \
                   <input type=\"hidden\" id=\"txtID_" + ctr + "\" readonly> \
                   <tr> \
                       <td class=\"text-align-center\"> \
                           <span id=\"txtKRAGroup_" + ctr + "\" class=\"KRAGroupDynamicFields rowLabel\" title=\"KRA Group\"></span> \
                       </td> \
                       <td class=\"text-align-center\"> \
                           <span id=\"txtKPICode_" + ctr + "\" class=\"KPICodeDynamicFields rowLabel\" title=\"KPI Code\"></span> \
                       </td> \
                       <td class=\"text-align-left\"> \
                           <span id=\"txtKPIName_" + ctr + "\" class=\"KPINameDynamicFields rowLabel\" title=\"KPI Name\"></span> \
                       </td> \
                       <td class=\"text-align-left\"> \
                           <span id=\"txtKPIDescription_" + ctr + "\" class=\"KPIDescriptionDynamicFields rowLabel\" title=\"KPI Description\"></span> \
                       </td> \
                       <td class=\"text-align-left\"> \
                           <span id=\"txtKPIGuidelines_" + ctr + "\" class=\"KPIGuidelinesDynamicFields rowLabel\" title=\"KPI Guidelines\"></span> \
                       </td> \
                       <td class=\"text-align-right\"> \
                           <span id=\"txtWeight_" + ctr + "\" class=\"WeightDynamicFields rowLabel\" title=\"Weight\"></span> \
                       </td> \
                       <td class=\"text-align-right\"> \
                           <span id=\"txtTarget_" + ctr + "\" class=\"TargetDynamicFields rowLabel\" title=\"Target\"></span> \
                       </td> \
                       <td class=\"text-align-right\"> \
                           <span id=\"txtActual_" + ctr + "\" class=\"ActualDynamicFields rowLabel\" title=\"Actual\"></span> \
                       </td> \
                       <td class=\"text-align-right\"> \
                           <span id=\"txtRate_" + ctr + "\" class=\"RateDynamicFields rowLabel\" title=\"Rate\"></span> \
                       </td> \
                       <td class=\"text-align-right\"> \
                           <span id=\"txtTotal" + ctr + "\" class=\"TotalDynamicFields rowLabel\" title=\"Total\"></span> \
                       </td> \
                   </tr> \
               </tbody>"
            );

            NumberOnly($(".WeightDynamicFields"));
            NumberOnly($(".TargetDynamicFields"));
            NumberOnly($(".ActualDynamicFields"));
            NumberOnly($(".RateDynamicFields"));
            NumberOnly($(".TotalDynamicFields"));
        },

        LoadEmployeeScoreJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMEmployeeScoreKPIScoreList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMEmployeeScoreKPIScoreList"));
            var moveFilterFields = function () {
                var intialHeight = $("#divEmployeeScoreKPIScoreList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divEmployeeScoreKPIScoreList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divEmployeeScoreKPIScoreList #filterFieldsContainer");
                });

                $("#divEmployeeScoreKPIScoreList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divEmployeeScoreKPIScoreList div.filterFields").unbind("keyup");
                $("#divEmployeeScoreKPIScoreList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divEmployeeScoreKPIScoreList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMEmployeeScoreKPIScoreList").jqGrid("GridUnload");
            $("#tblIPMEmployeeScoreKPIScoreList").jqGrid("GridDestroy");
            $("#tblIPMEmployeeScoreKPIScoreList").jqGrid({
                url: EmployeeKPIScoreListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: 10000, //tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "KRA Group", "KPI Code", "KPI Name", "KPI Description", "KPI Guidelines"
                    , "Weight", "Target", "Actual", "Rate", "Total", "Grade", "Source Type", ""],
                colModel: [
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center" },
                    { width: 30, name: "KRAGroup", index: "KRAGroup", align: "left", sortable: true },
                    { width: 25, name: "KPICode", index: "KPICode", align: "left", sortable: true },
                    { width: 30, name: "KPIName", index: "KPIName", align: "left", sortable: true },
                    { width: 60, name: "KPIDescription", index: "KPIDescription", align: "left", sortable: true },
                    { width: 60, name: "KPIGuidelines", index: "KPIGuidelines", align: "left", sortable: true },
                    { width: 20, name: "Weight", index: "Weight", align: "right", sortable: true, formatter: 'currency', formatoptions: { thousandsSeparator: ',' } },
                    { width: 20, name: "Target", index: "Target", align: "right", sortable: true, formatter: 'currency', formatoptions: { thousandsSeparator: ',' } },
                    { width: 20, name: "Actual", index: "Actual", align: "right", sortable: true, formatter: 'currency', formatoptions: { thousandsSeparator: ',' } },
                    { width: 20, name: "Rate", index: "Rate", align: "right", sortable: true, formatter: 'currency', formatoptions: { thousandsSeparator: ',' } },
                    { width: 20, name: "Total", index: "Total", align: "right", sortable: true, formatter: 'currency', formatoptions: { thousandsSeparator: ',' } },
                    { width: 20, name: "Grade", index: "Grade", align: "center", sortable: true },
                    { width: 38, name: "SourceType", index: "SourceType", align: "center", sortable: true },
                    { hidden: true, name: "IsEditable", index: "IsEditable", align: "center" },

                ],
                toppager: $("#divPagerKPIScoreList"),
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
                    $("#tblIPMEmployeeScoreKPIScoreList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    //s.CalculateTotal("TotalDynamicFields", "empScoreHeader");
                    $("#divEmployeeScoreKPIScoreList td").css({ "font-size": "11px" });

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                                if (data.rows[i].IsEditable) {
                                    $("#" + data.rows[i].ID).addClass("kpi-for-keyin");
                                }
                                initialTotalScore += parseFloat(data.rows[i].Total);
                                initialTotalWeight += parseFloat(data.rows[i].Weight);
                            }
                        }

                        if (firstLoad) {
                            $("#empScoreHeader").text(parseFloat(initialTotalScore).toFixed(2) + "%");
                            $("#lblTotalWeight").text(parseFloat(initialTotalWeight).toFixed(2) + "%");
                            $("#lblTotalScore").text(parseFloat(initialTotalScore).toFixed(2) + "%");
                            firstLoad = false;
                        }

                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMEmployeeScoreKPIScoreList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divEmployeeScoreKPIScoreList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divEmployeeScoreKPIScoreList .jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["EmployeeScoreListFilterOption"] != undefined) {
                        $("#divEmployeeScoreKPIScoreList #chkFilter").prop('checked', JSON.parse(localStorage["EmployeeScoreListFilterOption"]));
                    }
                    objEmployeeScoreApprovalViewJS.ShowHideFilter();

                    $("#divEmployeeScoreKPIScoreList #chkFilter").on('change', function () {
                        objEmployeeScoreApprovalViewJS.ShowHideFilter();
                        localStorage["EmployeeScoreListFilterOption"] = $("#divEmployeeScoreKPIScoreList #chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divEmployeeScoreKPIScoreList #gview_tblIPMEmployeeScoreList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#divEmployeeScoreKPIScoreList table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#divEmployeeScoreKPIScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#divEmployeeScoreKPIScoreList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeScoreKPIScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#divEmployeeScoreKPIScoreList table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#divEmployeeScoreKPIScoreList table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#divEmployeeScoreKPIScoreList table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#divEmployeeScoreKPIScoreList .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
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
                    GetJQGridState("tblIPMEmployeeScoreKPIScoreList");
                    moveFilterFields();
                },
            }).navGrid("#divEmployeeScoreKPIScoreList #divPagerKPIScoreList",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#divEmployeeScoreKPIScoreList .ui-row-label").remove();
            $("#divEmployeeScoreKPIScoreList .ui-pg-selbox").remove();
            jQuery("#divEmployeeScoreKPIScoreList .ui-paging-info").closest("div").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#divEmployeeScoreKPIScoreList #lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");

        },

        CalculateTotal: function (input, totalID) {
            var total = 0;
            var arr = $("." + input);

            for (var i = 0; i < arr.length; i++) {
                if (parseFloat(arr[i].value))
                    total += parseFloat(arr[i].value || 0);
            }

            $("#" + totalID).text(total.toFixed(2) + "%");
        },

        ApproveSuccessFunction: function () {
            $("#divEmployeeScoreApprovalList #btnSearch").click();
            $("#txtApprovalRemarks").val("");
            $("#divEmployeeScoreApprovalModal").modal('hide');
        },

        GetFormData: function () {

            var formData = new FormData();

            formData.append("TID", objEmployeeScoreApprovalViewJS.ID);

            return formData;
        },

        ShowHideFilter: function () {
            if ($("#divEmployeeScoreKPIScoreList #chkFilter").is(":checked")) {
                $("#divEmployeeScoreKPIScoreList .jqgfirstrow .filterFields").show();
            }
            else if ($("#divEmployeeScoreKPIScoreList #chkFilter").is(":not(:checked)")) {
                $("#divEmployeeScoreKPIScoreList .jqgfirstrow .filterFields").hide();
            }
        },


        LoadEmployeeScoreStatusHistoryJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblEmployeeScoreStatusHistoryList").jqGrid("GridUnload");
            $("#tblEmployeeScoreStatusHistoryList").jqGrid("GridDestroy");
            $("#tblEmployeeScoreStatusHistoryList").jqGrid({
                url: GetEmployeeScoreStatusHistoryURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Status", "Timestamp", "User", "Remarks"],
                colModel: [
                    { name: "ID", index: "", hidden: true },
                    { name: "Status", index: "Status", align: "left", sortable: false },
                    { name: "Timestamp", index: "Timestamp", align: "left", sortable: false },
                    { name: "User", index: "User", align: "left", sortable: false },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false },
                ],
                //toppager: $("#divMRFFormApprovalHistoryPager"),   
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
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
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                                //if (data.rows[i].ApprovalStatusCode == "FOR_APPROVAL") {
                                //    $("#hdnApproverPositionID").val(data.rows[i].PositionID);
                                //    $("#hdnApproverOrgGroupID").val(data.rows[i].OrgGroupID);
                                //}
                            }
                        }
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        }
    };

    objEmployeeScoreApprovalViewJS.Initialize();
});