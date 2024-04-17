var objEmployeeScoreEditJS;
var KPIDropDownOptions = [];
var originalKPIScoresList = [];
var KPIScoresList = [];
var initialTotalScore = 0;
var initialTotalWeight = 0;
var firstLoad = true;
var isFormSaved = false;
const SourceTypeDropDownURL = "/IPM/EmployeeScore?handler=ReferenceValue&RefCode=KPI_SOURCE_TYPE";

$(document).ready(function () {
    objEmployeeScoreEditJS = {
        //_MaxScore: $("#hdnMaxValue").val(),
        ID: $("#hdnID").val(),
        OrgGroup: $("#hdnOrgGroup").val(),
        Position: $("#hdnPosition").val(),
        TDateFrom: $("#hdnDateFrom").val(),
        TDateTo: $("#hdnDateTo").val(),

        Initialize: function () {
            $("#divEmployeeScoreBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divEmployeeScoreModal .form-control").not("#ddlPosition").attr("readonly", false);
            $("#divEmployeeScoreModal #btnEdit").hide();
            $("#divEmployeeScoreModal #btnSave, #divEmployeeScoreModal #btnBack").show();
            var param = {
                ID: $("#hdnID").val()
            };
            s.LoadEmployeeScoreJQGrid(param);

            objEmployeeScoreEditJS.LoadEmployeeScoreStatusHistoryJQGrid({
                ID: $("#hdnID").val(),
            });

            $('#lblRunID').text(objEMSCommonJS.JQGridIDFormat($('#lblRunID').text()));

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabScore', '#divEmployeeScoreBodyModal');

            $('#ddlStatus option[value="WIP"]').remove();
        },

        EditSuccessFunction: function () {
            $("#divEmployeeScoreList #btnSearch").click();
            $("#btnBack").click();
        },

        SaveFormFunction: function () {
            isFormSaved = true;
            $("#divEmployeeScoreKPIScoreList  #btnReset").click();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnRater").click(function () {
                objEmployeeScoreListJS.LoadRater(objEmployeeScoreViewJS.ID);
                $("#txtSearchRater, #raterListview").prop("disabled", false);
            });

            $("#txtSearchRater").on("change keyup keydown", function () {
                ListBoxSearch("raterListview", $(this).val(), false);
            });

            $("#divEmployeeScoreModal #btnSave").click(function () {
                //if (objEMSCommonJS.ValidateBlankFields("#frmEmployeeScore", "#divEmployeeScoreErrorMessage", objEmployeeScoreEditJS.ValidateTotalScoreFields)) {
                //    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                //        "objEMSCommonJS.PostAjax(true \
                //        , EmployeeScoreEditPostURL \
                //        , objEmployeeScoreEditJS.GetFormData() \
                //        , '#divEmployeeScoreErrorMessage' \
                //        , '#divEmployeeScoreModal #btnSave' \
                //        , objEmployeeScoreEditJS.EditSuccessFunction);",
                //        "function");
                //}
                if (objEMSCommonJS.ValidateBlankFields("#frmEmployeeScore", "#divEmployeeScoreErrorMessage", objEmployeeScoreEditJS.ValidateTotalScoreFields)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEmployeeScoreEditJS.SaveFormFunction()",
                        "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(EmployeeScoreViewURL +
                    "?ID=" + objEmployeeScoreEditJS.ID +
                    "&OrgGroup=" + objEmployeeScoreEditJS.OrgGroup +
                    "&Position=" + objEmployeeScoreEditJS.Position +
                    "&DateFrom=" + objEmployeeScoreEditJS.TDateFrom +
                    "&DateTo=" + objEmployeeScoreEditJS.TDateTo, "divEmployeeScoreBodyModal");
            });

            $("#btnEmployeeExport").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeScoreEditJS.ExportFunction()",
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
                $("#divEmployeeScoreKPIScoreList #btnSearch").click();
                $("#divEmployeeScoreKPIScoreList #multiSelectedSourceType").html("");
                $("#divEmployeeScoreKPIScoreList #multiSelectedSourceTypeOption label, #multiSelectedSourceTypeOption input").prop("title", "add");
            });

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedGrade", GetRatingGradesDropdownURL, "Code", "Code");
            $("#multiSelectedGradeOption").css({ "min-width": "50px" });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedSourceType", SourceTypeDropDownURL);

            $("#divEmployeeScoreBodyModal .modal-body").scroll(function () {
                if ($("#divEmployeeScoreBodyModal .modal-body").scrollTop() >= 376) {

                    $("#divEmployeeScoreBodyModal .ui-jqgrid-htable").css('cssText'
                        , 'width: ' + $("#divEmployeeScoreBodyModal .ui-jqgrid-hbox").width() + 'px !important');

                    $("#divEmployeeScoreBodyModal .ui-jqgrid-htable").css({
                        "max-width": $("#divEmployeeScoreBodyModal .ui-jqgrid-hbox").width() + "px",
                        "position": "fixed",
                        "z-index": "999",
                        "top": $("#divEmployeeScoreModal").position().top > 0 ? "37px" : "67px"
                    });
                }
                else {
                    $("#divEmployeeScoreBodyModal .ui-jqgrid-htable").css({
                        "position": "relative",
                        "z-index": "auto",
                        "max-width": "auto",
                        "top": "auto"
                    });
                }
            });
        },

        ExportFunction: function () {
            var parameters = "&ID=" + objEmployeeScoreEditJS.ID +
                "&OrgGroup=" + objEmployeeScoreEditJS.OrgGroup +
                "&Position=" + objEmployeeScoreEditJS.Position +
                "&DateFrom=" + objEmployeeScoreEditJS.TDateFrom +
                "&DateTo=" + objEmployeeScoreEditJS.TDateTo

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

        ValidateTotalScoreFields: function () {
            var isValid = true;

            $(".RateDynamicFields").each(function (index) {
                if ($("#ddlStatus").val() == 'FOR_APPROVAL' && !$(this).prop("readonly") && ($(this).val() == "0" || $(this).val() == "" || $(this).val() == "0.00")) {
                    $(this).addClass("errMessage");
                    $("#divEmployeeScoreErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_MSG_KEYIN_FOR_APPROVAL + "</li></label><br />");
                    isValid = false;
                }
            });

            if (parseFloat(initialTotalWeight).toFixed(2) != 100) {
                $("#divEmployeeScoreErrorMessage").html("<label class=\"errMessage\"><li>" + "Total Weight is not 100%." + "</li></label><br />");
                isValid = false;
            }

            if (!isValid) {
                $(".RateDynamicFields.errMessage:first").focus();
            }

            return isValid;
        },

        GetFormData: function () {

            var formData = new FormData($('#frmEmployeeScore').get(0));

            $(".IDGroupDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeScore.EmployeeScoreList[" + index + "].ID", $(this).val());
                }
            });

            $(".WeightDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeScore.EmployeeScoreList[" + index + "].Weight", $(this).val());
                }
            });

            $(".TargetDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeScore.EmployeeScoreList[" + index + "].Target", $(this).val());
                }
            });

            $(".ActualDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeScore.EmployeeScoreList[" + index + "].Actual", $(this).val());
                }
            });

            $(".RateDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("EmployeeScore.EmployeeScoreList[" + index + "].Rate", $(this).val());
                }
            });

            formData.append("EmployeeScore.Status", $("#ddlStatus").val());
            formData.append("EmployeeScore.Remarks", $("#txtRemarks").val());
            formData.append("EmployeeScore.TotalScore", initialTotalScore);

            return formData;

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
                    { width: 20, name: "Weight", index: "Weight", align: "right", sortable: true, formatter: objEmployeeScoreEditJS.AddWeightField },
                    { width: 20, name: "Target", index: "Target", align: "right", sortable: true, formatter: objEmployeeScoreEditJS.AddTargetField },
                    { width: 20, name: "Actual", index: "Actual", align: "right", sortable: true, formatter: objEmployeeScoreEditJS.AddActualField },
                    { width: 20, name: "Rate", index: "Rate", align: "right", sortable: true, formatter: objEmployeeScoreEditJS.AddRateField },
                    { width: 20, name: "Total", index: "Total", align: "right", sortable: true, formatter: objEmployeeScoreEditJS.AddTotalField },
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
                    if (isFormSaved) {
                        objEMSCommonJS.PostAjax(true
                            , EmployeeScoreEditPostURL
                            , objEmployeeScoreEditJS.GetFormData()
                            , '#divEmployeeScoreErrorMessage'
                            , '#divEmployeeScoreModal #btnSave'
                            , objEmployeeScoreEditJS.EditSuccessFunction);

                        isFormSaved = false;
                    }

                    //WIP
                    //setTimeout(function () { $(".jqgrid-id-link:first").click(); }, 500);

                    if (firstLoad) {
                        KPIScoresList = [];
                        $(".WeightDynamicFields").each(function (index, item) {
                            var ctr = $(this).prop("id").replace("txtWeight_", "");
                            KPIScoresList.push({
                                ID: ctr,
                                Weight: $(item).val(),
                                Target: $("#txtTarget_" + ctr).val().noComma(),
                                Actual: $("#txtActual_" + ctr).val().noComma(),
                                Rate: $("#txtRate_" + ctr).val().noComma(),
                                Total: $("#txtTotal_" + ctr).val().noComma(),
                            });
                            if ($("#txtTotal_" + ctr).val() != "" & $("#txtTotal_" + ctr).val() != "."
                                & !isNaN($("#txtTotal_" + ctr).val())) {
                                initialTotalScore += parseFloat($("#txtTotal_" + ctr).val().noComma());
                            }
                            if ($("#txtWeight_" + ctr).val() != "" & $("#txtWeight_" + ctr).val() != "."
                                & !isNaN($("#txtWeight_" + ctr).val())) {
                                initialTotalWeight += parseFloat($("#txtWeight_" + ctr).val().noComma());
                            }
                        });
                        $("#empScoreHeader").text(parseFloat(initialTotalScore).toFixed(2) + "%");
                        $("#lblTotalWeight").text(parseFloat(initialTotalWeight).toFixed(2) + "%");
                        $("#lblTotalScore").text(parseFloat(initialTotalScore).toFixed(2) + "%");
                        originalKPIScoresList = KPIScoresList;
                        firstLoad = false;
                    }

                    //s.CalculateTotal("TotalDynamicFields", "empScoreHeader");
                    $("#divEmployeeScoreKPIScoreList td").css({ "font-size": "11px" });

                    if (data.rows != null) {
                        if (data.rows.length > 0) {

                            AmountTextChange($(".WeightDynamicFields"));
                            AmountTextChange($(".TargetDynamicFields"));
                            AmountTextChange($(".ActualDynamicFields"));
                            AmountTextChange($(".RateDynamicFields"));
                            AmountTextChange($(".TotalDynamicFields"));

                            $(".WeightDynamicFields").blur();
                            $(".TargetDynamicFields").blur();
                            $(".ActualDynamicFields").blur();
                            $(".RateDynamicFields").blur();
                            $(".TotalDynamicFields").blur();

                            for (var i = 0; i < data.rows.length; i++) {
                                if (data.rows[i].IsEditable) {
                                    $("#" + data.rows[i].ID).addClass("kpi-for-keyin");
                                }
                                else
                                    $("#" + data.rows[i].ID + " input").prop("readonly", true);

                                var ctr = data.rows[i].ID;
                                AmountOnly($("#txtWeight_" + ctr));
                                AmountOnly($("#txtTarget_" + ctr));
                                AmountOnly($("#txtActual_" + ctr));
                                AmountOnly($("#txtRate_" + ctr));

                                $("#txtTarget_" + ctr + ", #txtActual_" + ctr).keyup(function () {
                                    var ctr = $(this).prop("id").replace("txtTarget_", "").replace("txtActual_", "");
                                    if ($("#txtTarget_" + ctr).val() != "" & $("#txtActual_" + ctr).val() != ""
                                        & $("#txtTarget_" + ctr).val() != "." & $("#txtActual_" + ctr).val() != ".") {
                                        var target = parseFloat($("#txtTarget_" + ctr).val().noComma());
                                        var actual = parseFloat($("#txtActual_" + ctr).val().noComma());

                                        if ((actual / target).toFixed(2) > (parseFloat(_MaxScore)).toFixed(2)) {
                                            $("#txtRate_" + (ctr)).val((parseFloat(_MaxScore)).toFixed(2));
                                        }
                                        else {
                                            $("#txtRate_" + (ctr)).val((actual / target).toFixed(2));
                                        }

                                        $("#hdnRateLong_" + (ctr)).val((actual / target));
                                        $("#txtRate_" + ctr).keyup();
                                    }
                                    else {
                                        $("#hdnRateLong_" + (ctr)).val("0.00");
                                        $("#txtRate_" + (ctr)).val("0.00");
                                        $("#txtRate_" + ctr).keyup();
                                    }
                                });

                                $("#txtTarget_" + ctr + ", #txtActual_" + ctr).blur(function () {
                                    var ctr = $(this).prop("id").replace("txtTarget_", "").replace("txtActual_", "");

                                    if ($("#txtTarget_" + ctr).val() == "" || $("#txtTarget_" + ctr).val() == ".") {
                                        $("#txtTarget_" + ctr).val("0.00");
                                    }

                                    if ($("#txtActual_" + ctr).val() == "" || $("#txtActual_" + ctr).val() == ".") {
                                        $("#txtActual_" + ctr).val("0.00");
                                    }
                                });

                                $("#txtWeight_" + ctr).blur(function () {
                                    var ctr = $(this).prop("id").replace("txtWeight_", "");

                                    if ($("#txtWeight_" + ctr).val() == "" || $("#txtWeight_" + ctr).val() == ".") {
                                        $("#txtWeight_" + ctr).val("0.00");
                                    }
                                    objEmployeeScoreEditJS.UpdateKPI(ctr);
                                });

                                $("#txtWeight_" + ctr).keyup(function () {
                                    var ctr = $(this).prop("id").replace("txtWeight_", "");
                                    if ($("#txtWeight_" + ctr).val() == "" || $("#txtWeight_" + ctr).val() == ".") {
                                        $("#txtWeight_" + ctr).val("0.00");
                                    }

                                    var rate = parseFloat($("#txtRate_" + ctr).val().noComma());
                                    var weight = parseFloat($("#txtWeight_" + ctr).val().noComma());

                                    var total = isNaN((rate * (weight)).toFixed(2)) ? 0.00 : (rate * (weight)).toFixed(2);
                                    $("#txtTotal_" + (ctr)).val(total);
                                    s.CalculateTotal("WeightDynamicFields", "txtTotalWeight");
                                    s.CalculateTotal("TotalDynamicFields", "txtTotalScore");

                                    objEmployeeScoreEditJS.UpdateKPI(ctr);
                                });

                                $("#txtRate_" + ctr).blur(function () {
                                    var ctr = $(this).prop("id").replace("txtRate_", "");
                                    if ($("#txtRate_" + ctr).val() == "" || $("#txtRate_" + ctr).val() == ".") {
                                        $("#txtRate_" + ctr).val("0.00");
                                    }


                                    if ($(this).val() > (parseFloat(_MaxScore)).toFixed(2))
                                        $(this).val((parseFloat(_MaxScore)).toFixed(2));

                                    if ($("#txtTarget_" + ctr).val() != "" & $("#txtRate_" + ctr).val() != "") {
                                        var target = parseFloat($("#txtTarget_" + ctr).val().noComma());
                                        var rate = parseFloat($("#hdnRateLong_" + ctr).val().noComma());
                                        if ($("#txtRate_" + (ctr)).val() <= (parseFloat(_MaxScore)).toFixed(2)) {
                                            var actual = isNaN((rate * target).toFixed(2)) ? 0.00 : (rate * target).toFixed(2);

                                            $("#txtActual_" + (ctr)).val(actual);
                                            $("#txtActual_" + (ctr)).val($("#txtActual_" + (ctr)).val().withComma());
                                        }
                                    }
                                    else {
                                        $("#txtActual_" + (ctr)).val("0.00");
                                    }

                                    objEmployeeScoreEditJS.UpdateKPI(ctr);
                                });

                                $("#txtRate_" + ctr).keyup(function () {
                                    var ctr = $(this).prop("id").replace("txtRate_", "");

                                    var rate = parseFloat($("#txtRate_" + ctr).val().noComma());
                                    if ($(this).val() > (parseFloat(_MaxScore)).toFixed(2))
                                        rate = (parseFloat(_MaxScore)).toFixed(2);
                                    var rateLong = isNaN(parseFloat($("#hdnRateLong_" + ctr).val())) ? 0.00 : parseFloat($("#hdnRateLong_" + ctr).val());
                                    var weight = parseFloat($("#txtWeight_" + ctr).val().noComma());

                                    if ($("#txtRate_" + ctr).val() == "" || $("#txtRate_" + ctr).val() == ".") {
                                        $("#hdnRateLong_" + (ctr)).val("0.00");
                                    }
                                    else
                                        if (parseFloat(rate).toFixed(2) != rateLong.toFixed(2)) {
                                            $("#hdnRateLong_" + (ctr)).val(rate);
                                        }

                                    var total = isNaN((parseFloat(rate) * (weight)).toFixed(2)) ? 0.00 : (parseFloat(rate) * (weight)).toFixed(2);

                                    $("#txtTotal_" + (ctr)).val(total);
                                    s.CalculateTotal("TotalDynamicFields", "txtTotalScore");
                                    //s.CalculateTotal("TotalDynamicFields", "empScoreHeader");
                                    objEmployeeScoreEditJS.UpdateKPI(ctr);
                                });

                            }
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
                    objEmployeeScoreEditJS.ShowHideFilter();

                    $("#divEmployeeScoreKPIScoreList #chkFilter").on('change', function () {
                        objEmployeeScoreEditJS.ShowHideFilter();
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

        UpdateKPI: function (ctr) {
            $(KPIScoresList).each(function (index, item) {
                if (item.ID == ctr) {
                    if ($("#txtTotal_" + ctr).val() != "" & $("#txtTotal_" + ctr).val() != "."
                        & !isNaN($("#txtTotal_" + ctr).val())) {
                        initialTotalScore = initialTotalScore + (parseFloat($("#txtTotal_" + ctr).val().noComma()) - item.Total);
                        $("#empScoreHeader").text(parseFloat(initialTotalScore).toFixed(2) + "%");
                        $("#lblTotalScore").text(parseFloat(initialTotalScore).toFixed(2) + "%");
                    }
                    if ($("#txtWeight_" + ctr).val() != "" & $("#txtWeight_" + ctr).val() != "."
                        & !isNaN($("#txtWeight_" + ctr).val())) {
                        initialTotalWeight = initialTotalWeight + (parseFloat($("#txtWeight_" + ctr).val().noComma()) - item.Weight);
                        $("#lblTotalWeight").text(parseFloat(initialTotalWeight).toFixed(2) + "%");
                    }
                    item.Weight = $("#txtWeight_" + ctr).val().noComma();
                    item.Target = $("#txtTarget_" + ctr).val().noComma();
                    item.Actual = $("#txtActual_" + ctr).val().noComma();
                    item.Rate = $("#txtRate_" + ctr).val().noComma();
                    item.Total = $("#txtTotal_" + ctr).val().noComma();
                }
            });
        },

        AddWeightField: function (cellvalue, options, rowObject) {
            var fieldValue = cellvalue;
            if (!firstLoad) fieldValue = jQuery.grep(KPIScoresList, function (n, i) { return (n.ID == rowObject.ID); })[0].Weight;
            return "<input type=\"text\" id=\"txtWeight_" + rowObject.ID + "\" value=\"" + parseFloat(fieldValue).toFixed(2) + "\" class=\"form-control text-amount WeightDynamicFields\" maxlength=\"15\" title=\"Weight\" " + (rowObject.IsEditable ? "" : " tabindex=\"-1\" ") + ">";
        },
        AddTargetField: function (cellvalue, options, rowObject) {
            var fieldValue = cellvalue;
            if (!firstLoad) fieldValue = jQuery.grep(KPIScoresList, function (n, i) { return (n.ID == rowObject.ID); })[0].Target;
            return "<input type=\"text\" id=\"txtTarget_" + rowObject.ID + "\" value=\"" + parseFloat(fieldValue).toFixed(2) + "\" class=\"form-control text-amount TargetDynamicFields\" maxlength=\"15\" title=\"Target\" " + (rowObject.IsEditable ? "" : " tabindex=\"-1\" ") + ">";
        },
        AddActualField: function (cellvalue, options, rowObject) {
            var fieldValue = cellvalue;
            if (!firstLoad) fieldValue = jQuery.grep(KPIScoresList, function (n, i) { return (n.ID == rowObject.ID); })[0].Actual;
            return "<input type=\"text\" id=\"txtActual_" + rowObject.ID + "\" value=\"" + parseFloat(fieldValue).toFixed(2) + "\" class=\"form-control text-amount ActualDynamicFields\" maxlength=\"15\" title=\"Actual\" " + (rowObject.IsEditable ? "" : " tabindex=\"-1\" ") + ">";
        },
        AddRateField: function (cellvalue, options, rowObject) {
            var fieldValue = cellvalue;
            if (!firstLoad) fieldValue = jQuery.grep(KPIScoresList, function (n, i) { return (n.ID == rowObject.ID); })[0].Rate;
            return "<input type=\"text\" id=\"txtRate_" + rowObject.ID + "\" value=\"" + parseFloat(fieldValue).toFixed(2) + "\" class=\"form-control text-amount RateDynamicFields\" maxlength=\"15\" title=\"Rate\" " + (rowObject.IsEditable ? "" : " tabindex=\"-1\" ") + ">"
                + "<input type=\"hidden\" id=\"hdnRateLong_" + rowObject.ID + "\" value=\"" + parseFloat(fieldValue).toFixed(4) + "\">"
                + "<input type=\"hidden\" class=\"IDGroupDynamicFields\" id=\"hdnID_" + rowObject.ID + "\" value=\"" + rowObject.ID + "\">";
        },
        AddTotalField: function (cellvalue, options, rowObject) {
            var fieldValue = cellvalue;
            if (!firstLoad) fieldValue = jQuery.grep(KPIScoresList, function (n, i) { return (n.ID == rowObject.ID); })[0].Total;
            return "<input type=\"text\" id=\"txtTotal_" + rowObject.ID + "\" value=\"" + parseFloat(fieldValue).toFixed(2) + "\" class=\"form-control text-amount TotalDynamicFields\" title=\"Total\" tabindex=\"-1\" readonly>";
        },

        CalculateTotal: function (input, totalID) {
            var total = 0;
            var arr = $("." + input);

            for (var i = 0; i < arr.length; i++) {
                if (parseFloat(arr[i].value))
                    total += parseFloat(arr[i].value || 0);
            }

            $("#txtTotalScore").val(total);

            $("#" + totalID).text(total.toFixed(2) + "%");
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

        },
    };

    objEmployeeScoreEditJS.Initialize();
});