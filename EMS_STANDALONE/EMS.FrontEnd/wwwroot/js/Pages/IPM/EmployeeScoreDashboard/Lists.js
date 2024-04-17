var objDashboardListJS;
//const GetBranchAutoCompleteURL = "/IPM/EmployeeScoreDashboard?handler=BranchAutoComplete";
const GetRegionAutoCompleteURL = "/IPM/EmployeeScoreDashboard?handler=OrgGroupAutoComplete&Filter=REG";
const GetBranchAutoCompleteURL = "/IPM/EmployeeScoreDashboard?handler=OrgGroupAutoComplete&Filter=BRN";
const GetClusterAutoCompleteURL = "/IPM/EmployeeScoreDashboard?handler=OrgGroupAutoComplete&Filter=CLUS";
const GetPositionAutoCompleteURL = "/IPM/EmployeeScoreDashboard?handler=PositionAutoComplete";
const GetKRAAutoCompleteURL = "/IPM/EmployeeScoreDashboard?handler=KRAAutoComplete";
const GetKPIAutoCompleteURL = "/IPM/EmployeeScoreDashboard?handler=KPIAutoComplete";
const GetRunIDAutoCompleteURL = "/IPM/EmployeeScoreDashboard?handler=RunIDAutoComplete";

const SummaryForEvaluationURL = "/IPM/EmployeeScoreDashboard?handler=ListSummaryForEvaluation";
const GetCheckSummaryForEvaluationDashboardURL = "/IPM/EmployeeScoreDashboard?handler=CheckExportSummaryForEvaluationList";
const DownloadSummaryForEvaluationDashboardURL = "/IPM/EmployeeScoreDashboard?handler=DownloadExportSummaryForEvaluationList";

const SummaryForApprovalURL = "/IPM/EmployeeScoreDashboard?handler=ListSummaryForApproval";
const SummaryForApprovalBRNURL = "/IPM/EmployeeScoreDashboard?handler=ListSummaryForApprovalBRN";
const SummaryForApprovalCLUURL = "/IPM/EmployeeScoreDashboard?handler=ListSummaryForApprovalCLU";
const GetCheckSummaryForApprovalDashboardURL = "/IPM/EmployeeScoreDashboard?handler=CheckExportSummaryForApprovalList";
const DownloadSummaryForApprovalDashboardURL = "/IPM/EmployeeScoreDashboard?handler=DownloadExportSummaryForApprovalList";

const GetCheckSummaryForApprovalBRNDashboardURL = "/IPM/EmployeeScoreDashboard?handler=CheckExportSummaryForApprovalBRNList";
const DownloadSummaryForApprovalBRNDashboardURL = "/IPM/EmployeeScoreDashboard?handler=DownloadExportSummaryForApprovalBRNList";
const GetCheckSummaryForApprovalCLUDashboardURL = "/IPM/EmployeeScoreDashboard?handler=CheckExportSummaryForApprovalCLUList";
const DownloadSummaryForApprovalCLUDashboardURL = "/IPM/EmployeeScoreDashboard?handler=DownloadExportSummaryForApprovalCLUList";

const RegionalWithPositionURL = "/IPM/EmployeeScoreDashboard?handler=ListRegionalWithPosition";
const GetCheckRegionalWithPositionDashboardURL = "/IPM/EmployeeScoreDashboard?handler=CheckExportRegionalWithPositionList";
const DownloadRegionalWithPositionDashboardURL = "/IPM/EmployeeScoreDashboard?handler=DownloadExportRegionalWithPositionList";

const BranchesWithPositionURL = "/IPM/EmployeeScoreDashboard?handler=ListBranchesWithPosition";
const GetCheckBranchesWithPositionDashboardURL = "/IPM/EmployeeScoreDashboard?handler=CheckExportBranchesWithPositionList";
const DownloadBranchesWithPositionDashboardURL = "/IPM/EmployeeScoreDashboard?handler=DownloadExportBranchesWithPositionList";

const PositionOnlyURL = "/IPM/EmployeeScoreDashboard?handler=ListPositionOnly";
const GetCheckPositionOnlyDashboardURL = "/IPM/EmployeeScoreDashboard?handler=CheckExportPositionOnlyList";
const DownloadPositionOnlyDashboardURL = "/IPM/EmployeeScoreDashboard?handler=DownloadExportPositionOnlyList";



var divAdditionalFilterHeight = 0;

$(document).ready(function () {
    objDashboardListJS = {
        Initialize: function () {
            var s = this;
            s.ElementBinding();
            s.GetLocalStorage();

            $("#ddlDashboard").css({ "width": "380px" });
            $("#ddlDashboard").change();

        },

        ElementBinding: function () {
            var s = this;

            $("#btnSearch").click(function () {
                $("#txtFilterRunID").removeClass("errMessage");
                $("#divEmployeeScoreDashboardListErrorMessage").html("");

                if (objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value == ""){
                    $("#divEmployeeScoreDashboardListErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                    $("#txtFilterRunID").addClass("errMessage");
                }

                objDashboardListJS.LoadDashboardData($("#ddlDashboard").val());
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedBranch").html("");
                $("#multiSelectedKRA").html("");
                $("#multiSelectedKPI").html("");
                $("#multiSelectedPosition").html("");
                $("#multiSelectedRegion").html("");
                $("#multiSelectedBranch").html("");
                $("#multiSelectedCluster").html("");
                //$("#multiSelectedRunID").html("");
                $("#btnSearch").click();
            });

            $("#ddlDashboard").change(function () {
                //objEmployeeDashboardListJS.LoadReportData($(this).val());
                $("#gbox_tblIPMDashboardCountList").css({ "top": "0px" });
                ResetJQGridState("tblIPMDashboardCountList");

                $("#divAdditionalFilter").html(
                    " <input type=\"search\" placeholder=\"- Filter by Run ID -\" id=\"txtFilterRunID\" maxlength=\"255\" class=\"form-control\" />"
                    + " <div id=\"multiSelectedRunID\" class=\"form-control multiselect-control\"></div>"
                );

                divAdditionalFilterHeight = $("#divAdditionalFilter").height();

                objDashboardListJS.BindFilterMultiSelectAutoCompleteRunID("txtFilterRunID"
                    , GetRunIDAutoCompleteURL, 20, "multiSelectedRunID");

                objEMSCommonJS.SetMultiSelectList("multiSelectedRunID"
                    , "DashboardRunID"
                    , "DashboardRunIDText");

                $("#filterFieldsContainer").html("");
                $(".jqgfirstrow td").html("");

                $("#txtFilterRunID").removeClass("errMessage");
                $("#divEmployeeScoreDashboardListErrorMessage").html("");

                if (objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value == "") {
                    $("#divEmployeeScoreDashboardListErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                    $("#txtFilterRunID").addClass("errMessage");
                }
                
                if ($(this).val() == "SUMMARY_FOR_EVAL") {
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterRegion\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedRegion\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtFilterWithCompelteScoreMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtFilterWithCompelteScoreMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtFilterWithMissingScoreMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtFilterWithMissingScoreMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtFilterNoScoreMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtFilterNoScoreMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtFilterOnGoingEvalMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtFilterOnGoingEvalMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterRegion"
                        , GetRegionAutoCompleteURL, 20, "multiSelectedRegion");

                    s.LoadJQGridSummaryForEvaluation(objDashboardListJS.GetParameters($(this).val()));
                }
                else if ($(this).val() == "SUMMARY_FOR_APPR" || $(this).val() == "SUMMARY_FOR_APPR_BRN" || $(this).val() == "SUMMARY_FOR_APPR_CLU") {
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterRegion\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedRegion\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + ($(this).val() == "SUMMARY_FOR_APPR_BRN" ? "<div class=\"filterFields\">"
                            + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterBranch\" maxlength=\"255\" class=\"form-control\" />"
                            + " <div id=\"multiSelectedBranch\" class=\"form-control multiselect-control\"></div>" + "</div>" : "")
                        + ($(this).val() == "SUMMARY_FOR_APPR_CLU" ? "<div class=\"filterFields\">"
                            + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterCluster\" maxlength=\"255\" class=\"form-control\" />"
                            + " <div id=\"multiSelectedCluster\" class=\"form-control multiselect-control\"></div>" + "</div>" : "")
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtFilterNoKeyInMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtFilterNoKeyInMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtFilterForApprovalMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtFilterForApprovalMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"search\" placeholder=\"Min\" id=\"txtFilterFinalizedMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"search\" placeholder=\"Max\" id=\"txtFilterFinalizedMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterRegion"
                        , GetRegionAutoCompleteURL, 20, "multiSelectedRegion");

                    if ($(this).val() == "SUMMARY_FOR_APPR") {
                        s.LoadJQGridSummaryForApproval(objDashboardListJS.GetParameters($(this).val()));
                    }

                    if ($(this).val() == "SUMMARY_FOR_APPR_BRN") {
                        objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterBranch"
                            , GetBranchAutoCompleteURL, 20, "multiSelectedBranch");

                        s.LoadJQGridSummaryForApprovalBRN(objDashboardListJS.GetParameters($(this).val()));
                    }

                    if ($(this).val() == "SUMMARY_FOR_APPR_CLU") {
                        objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterCluster"
                            , GetClusterAutoCompleteURL, 20, "multiSelectedCluster");

                        s.LoadJQGridSummaryForApprovalCLU(objDashboardListJS.GetParameters($(this).val()));
                    }
                }
                else if ($(this).val() == "SUMMARY_RESULTS_REG") {
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterRegion\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedRegion\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterPosition\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedPosition\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterKRA\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedKRA\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterKPI\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedKPI\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterEEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterEEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterMEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterMEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterSBEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterSBEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterBEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterBEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterRegion"
                        , GetRegionAutoCompleteURL, 20, "multiSelectedRegion");

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                        , GetPositionAutoCompleteURL, 20, "multiSelectedPosition");

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKRA"
                        , GetKRAAutoCompleteURL, 20, "multiSelectedKRA");

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKPI"
                        , GetKPIAutoCompleteURL, 20, "multiSelectedKPI");

                    s.LoadJQGridRegionalWithPosition(objDashboardListJS.GetParameters($(this).val()));
                }
                else if ($(this).val() == "SUMMARY_RESULTS_BRN") {
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterBranch\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedBranch\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterPosition\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedPosition\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterKRA\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedKRA\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterKPI\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedKPI\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterEEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterEEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterMEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterMEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterSBEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterSBEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterBEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterBEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterBranch"
                        , GetBranchAutoCompleteURL, 20, "multiSelectedBranch");

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                        , GetPositionAutoCompleteURL, 20, "multiSelectedPosition");

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKRA"
                        , GetKRAAutoCompleteURL, 20, "multiSelectedKRA");

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKPI"
                        , GetKPIAutoCompleteURL, 20, "multiSelectedKPI");

                    s.LoadJQGridBranchesWithPosition(objDashboardListJS.GetParameters($(this).val()));
                }
                else if ($(this).val() == "SUMMARY_RESULTS_POS") {
                    $("#filterFieldsContainer").append(
                        "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterPosition\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedPosition\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterKRA\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedKRA\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + " <input type=\"search\" placeholder=\"Search..\" id=\"txtFilterKPI\" maxlength=\"255\" class=\"form-control\" />"
                        + " <div id=\"multiSelectedKPI\" class=\"form-control multiselect-control\"></div>"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterEEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterEEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterMEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterMEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterSBEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterSBEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                        + "<div class=\"filterFields\">"
                        + "    <input type=\"number\" placeholder=\"Min\" id=\"txtFilterBEMin\" maxlength=\"10\" class=\"form-control\" />"
                        + "    <input type=\"number\" placeholder=\"Max\" id=\"txtFilterBEMax\" maxlength=\"10\" class=\"form-control\" />"
                        + "</div>"
                    );

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                        , GetPositionAutoCompleteURL, 20, "multiSelectedPosition");

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKRA"
                        , GetKRAAutoCompleteURL, 20, "multiSelectedKRA");

                    objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKPI"
                        , GetKPIAutoCompleteURL, 20, "multiSelectedKPI");

                    s.LoadJQGridPositionOnly(objDashboardListJS.GetParameters($(this).val()));
                }

                s.SetLocalStorage();
            });


            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objDashboardListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {
            if (($("#ddlDashboard").val() || "") != "") {

                var ParameterSummaryForEvaluation = "&sidx=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortorder")
                    + "&RunIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value
                    + "&RegionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value
                    + "&WithCompleteScoreMin=" + $("#txtFilterWithCompelteScoreMin").val()
                    + "&WithCompleteScoreMax=" + $("#txtFilterWithCompelteScoreMax").val()
                    + "&WithMissingScoreMin=" + $("#txtFilterWithMissingScoreMin").val()
                    + "&WithMissingScoreMax=" + $("#txtFilterWithMissingScoreMax").val()
                    + "&NoScoreMin=" + $("#txtFilterNoScoreMin").val()
                    + "&NoScoreMax=" + $("#txtFilterNoScoreMax").val()
                    + "&OnGoingEvalMin=" + $("#txtFilterOnGoingEvalMin").val()
                    + "&OnGoingEvalMax=" + $("#txtFilterOnGoingEvalMax").val();

                var ParameterSummaryForApproval = "&sidx=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortorder")
                    + "&RunIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value
                    + "&RegionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value
                    + "&NoKeyInMin=" + $("#txtFilterNoKeyInMin").val()
                    + "&NoKeyInMax=" + $("#txtFilterNoKeyInMax").val()
                    + "&ForApprovalMin=" + $("#txtFilterForApprovalMin").val()
                    + "&ForApprovalMax=" + $("#txtFilterForApprovalMax").val()
                    + "&FinalizedMin=" + $("#txtFilterFinalizedMin").val()
                    + "&FinalizedMax=" + $("#txtFilterFinalizedMax").val();

                var ParameterSummaryForApprovalBRN = "&sidx=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortorder")
                    + "&RunIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value
                    + "&RegionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value
                    + "&BranchDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedBranch").value
                    + "&NoKeyInMin=" + $("#txtFilterNoKeyInMin").val()
                    + "&NoKeyInMax=" + $("#txtFilterNoKeyInMax").val()
                    + "&ForApprovalMin=" + $("#txtFilterForApprovalMin").val()
                    + "&ForApprovalMax=" + $("#txtFilterForApprovalMax").val()
                    + "&FinalizedMin=" + $("#txtFilterFinalizedMin").val()
                    + "&FinalizedMax=" + $("#txtFilterFinalizedMax").val();

                var ParameterSummaryForApprovalCLU = "&sidx=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortorder")
                    + "&RunIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value
                    + "&RegionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value
                    + "&ClusterDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCluster").value
                    + "&NoKeyInMin=" + $("#txtFilterNoKeyInMin").val()
                    + "&NoKeyInMax=" + $("#txtFilterNoKeyInMax").val()
                    + "&ForApprovalMin=" + $("#txtFilterForApprovalMin").val()
                    + "&ForApprovalMax=" + $("#txtFilterForApprovalMax").val()
                    + "&FinalizedMin=" + $("#txtFilterFinalizedMin").val()
                    + "&FinalizedMax=" + $("#txtFilterFinalizedMax").val();

                var ParameterRegionalWithPosition = "&sidx=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortorder")
                    + "&RunIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value
                    + "&RegionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value
                    + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                    + "&KRADelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKRA").value
                    + "&KPIDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value
                    + "&EEMin=" + $("#txtFilterEEMin").val()
                    + "&EEMax=" + $("#txtFilterEEMax").val()
                    + "&MEMin=" + $("#txtFilterMEMin").val()
                    + "&MEMax=" + $("#txtFilterMEMax").val()
                    + "&SBEMin=" + $("#txtFilterSBEMin").val()
                    + "&SBEMax=" + $("#txtFilterSBEMax").val()
                    + "&BEMin=" + $("#txtFilterBEMin").val()
                    + "&BEMax=" + $("#txtFilterBEMax").val();

                var ParameterBranchesWithPosition = "&sidx=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortorder")
                    + "&RunIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value
                    + "&BranchDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedBranch").value
                    + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                    + "&KRADelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKRA").value
                    + "&KPIDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value
                    + "&EEMin=" + $("#txtFilterEEMin").val()
                    + "&EEMax=" + $("#txtFilterEEMax").val()
                    + "&MEMin=" + $("#txtFilterMEMin").val()
                    + "&MEMax=" + $("#txtFilterMEMax").val()
                    + "&SBEMin=" + $("#txtFilterSBEMin").val()
                    + "&SBEMax=" + $("#txtFilterSBEMax").val()
                    + "&BEMin=" + $("#txtFilterBEMin").val()
                    + "&BEMax=" + $("#txtFilterBEMax").val();

                var ParameterPositionOnly = "&sidx=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblIPMDashboardCountList").jqGrid("getGridParam", "sortorder")
                    + "&RunIDDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value
                    + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                    + "&KRADelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKRA").value
                    + "&KPIDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value
                    + "&EEMin=" + $("#txtFilterEEMin").val()
                    + "&EEMax=" + $("#txtFilterEEMax").val()
                    + "&MEMin=" + $("#txtFilterMEMin").val()
                    + "&MEMax=" + $("#txtFilterMEMax").val()
                    + "&SBEMin=" + $("#txtFilterSBEMin").val()
                    + "&SBEMax=" + $("#txtFilterSBEMax").val()
                    + "&BEMin=" + $("#txtFilterBEMin").val()
                    + "&BEMax=" + $("#txtFilterBEMax").val();


                if ($("#ddlDashboard").val() == "SUMMARY_FOR_EVAL") {
                    objEMSCommonJS.GetAjax(GetCheckSummaryForEvaluationDashboardURL + ParameterSummaryForEvaluation, {}, "#btnExport", function (data) {
                        if (data.IsSuccess) {
                            window.location = DownloadSummaryForEvaluationDashboardURL + ParameterSummaryForEvaluation;
                            $("#divModal").modal("hide");
                        }
                        else ModalAlert(MODAL_HEADER, data.Result);
                    }, null, true);
                }
                else if ($("#ddlDashboard").val() == "SUMMARY_FOR_APPR") {
                    objEMSCommonJS.GetAjax(GetCheckSummaryForApprovalDashboardURL + ParameterSummaryForApproval, {}, "#btnExport", function (data) {
                        if (data.IsSuccess) {
                            window.location = DownloadSummaryForApprovalDashboardURL + ParameterSummaryForApproval;
                            $("#divModal").modal("hide");
                        }
                        else ModalAlert(MODAL_HEADER, data.Result);
                    }, null, true);
                }
                else if ($("#ddlDashboard").val() == "SUMMARY_FOR_APPR_BRN") {
                    objEMSCommonJS.GetAjax(GetCheckSummaryForApprovalBRNDashboardURL + ParameterSummaryForApprovalBRN, {}, "#btnExport", function (data) {
                        if (data.IsSuccess) {
                            window.location = DownloadSummaryForApprovalBRNDashboardURL + ParameterSummaryForApprovalBRN;
                            $("#divModal").modal("hide");
                        }
                        else ModalAlert(MODAL_HEADER, data.Result);
                    }, null, true);
                }
                else if ($("#ddlDashboard").val() == "SUMMARY_FOR_APPR_CLU") {
                    objEMSCommonJS.GetAjax(GetCheckSummaryForApprovalCLUDashboardURL + ParameterSummaryForApprovalCLU, {}, "#btnExport", function (data) {
                        if (data.IsSuccess) {
                            window.location = DownloadSummaryForApprovalCLUDashboardURL + ParameterSummaryForApprovalCLU;
                            $("#divModal").modal("hide");
                        }
                        else ModalAlert(MODAL_HEADER, data.Result);
                    }, null, true);
                }
                else if ($("#ddlDashboard").val() == "SUMMARY_RESULTS_REG") {
                    objEMSCommonJS.GetAjax(GetCheckRegionalWithPositionDashboardURL + ParameterRegionalWithPosition, {}, "#btnExport", function (data) {
                        if (data.IsSuccess) {
                            window.location = DownloadRegionalWithPositionDashboardURL + ParameterRegionalWithPosition;
                            $("#divModal").modal("hide");
                        }
                        else ModalAlert(MODAL_HEADER, data.Result);
                    }, null, true);
                }
                else if ($("#ddlDashboard").val() == "SUMMARY_RESULTS_BRN") {
                    objEMSCommonJS.GetAjax(GetCheckBranchesWithPositionDashboardURL + ParameterBranchesWithPosition, {}, "#btnExport", function (data) {
                        if (data.IsSuccess) {
                            window.location = DownloadBranchesWithPositionDashboardURL + ParameterBranchesWithPosition;
                            $("#divModal").modal("hide");
                        }
                        else ModalAlert(MODAL_HEADER, data.Result);
                    }, null, true);
                }
                else if ($("#ddlDashboard").val() == "SUMMARY_RESULTS_POS") {
                    objEMSCommonJS.GetAjax(GetCheckPositionOnlyDashboardURL + ParameterPositionOnly, {}, "#btnExport", function (data) {
                        if (data.IsSuccess) {
                            window.location = DownloadPositionOnlyDashboardURL + ParameterPositionOnly;
                            $("#divModal").modal("hide");
                        }
                        else ModalAlert(MODAL_HEADER, data.Result);
                    }, null, true);
                }


            }
            else {
                $("#divEmployeeScoreDashboardListErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                $("#ddlDashboard").addClass("errMessage");
            }
        },


        GetParameters: function (type) {
            if (type == "SUMMARY_FOR_EVAL") {
                return {
                    'RunIDDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value,
                    'RegionDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value,
                    'WithCompleteScoreMin': $("#txtFilterWithCompelteScoreMin").val(),
                    'WithCompleteScoreMax': $("#txtFilterWithCompelteScoreMax").val(),
                    'WithMissingScoreMin': $("#txtFilterWithMissingScoreMin").val(),
                    'WithMissingScoreMax': $("#txtFilterWithMissingScoreMax").val(),
                    'NoScoreMin': $("#txtFilterNoScoreMin").val(),
                    'NoScoreMax': $("#txtFilterNoScoreMax").val(),
                    'OnGoingEvalMin': $("#txtFilterOnGoingEvalMin").val(),
                    'OnGoingEvalMax': $("#txtFilterOnGoingEvalMax").val(),
                };
            }
            else if (type == "SUMMARY_FOR_APPR") {
                return {
                    'RunIDDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value,
                    'RegionDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value,
                    'WithCompleteScoreMin': $("#txtFilterWithCompelteScoreMin").val(),
                    'WithCompleteScoreMax': $("#txtFilterWithCompelteScoreMax").val(),
                    'WithMissingScoreMin': $("#txtFilterWithMissingScoreMin").val(),
                    'WithMissingScoreMax': $("#txtFilterWithMissingScoreMax").val(),
                    'NoScoreMin': $("#txtFilterNoScoreMin").val(),
                    'NoScoreMax': $("#txtFilterNoScoreMax").val(),
                    'NoKeyInMin': $("#txtFilterNoKeyInMin").val(),
                    'NoKeyInMax': $("#txtFilterNoKeyInMax").val(),
                };
            }
            else if (type == "SUMMARY_FOR_APPR_BRN") {
                return {
                    'RunIDDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value,
                    'RegionDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value,
                    'BranchDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedBranch").value,
                    'WithCompleteScoreMin': $("#txtFilterWithCompelteScoreMin").val(),
                    'WithCompleteScoreMax': $("#txtFilterWithCompelteScoreMax").val(),
                    'WithMissingScoreMin': $("#txtFilterWithMissingScoreMin").val(),
                    'WithMissingScoreMax': $("#txtFilterWithMissingScoreMax").val(),
                    'NoScoreMin': $("#txtFilterNoScoreMin").val(),
                    'NoScoreMax': $("#txtFilterNoScoreMax").val(),
                    'NoKeyInMin': $("#txtFilterNoKeyInMin").val(),
                    'NoKeyInMax': $("#txtFilterNoKeyInMax").val(),
                };
            }
            else if (type == "SUMMARY_FOR_APPR_CLU") {
                return {
                    'RunIDDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value,
                    'RegionDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value,
                    'ClusterDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedCluster").value,
                    'WithCompleteScoreMin': $("#txtFilterWithCompelteScoreMin").val(),
                    'WithCompleteScoreMax': $("#txtFilterWithCompelteScoreMax").val(),
                    'WithMissingScoreMin': $("#txtFilterWithMissingScoreMin").val(),
                    'WithMissingScoreMax': $("#txtFilterWithMissingScoreMax").val(),
                    'NoScoreMin': $("#txtFilterNoScoreMin").val(),
                    'NoScoreMax': $("#txtFilterNoScoreMax").val(),
                    'NoKeyInMin': $("#txtFilterNoKeyInMin").val(),
                    'NoKeyInMax': $("#txtFilterNoKeyInMax").val(),
                };
            }
            else if (type == "SUMMARY_RESULTS_REG") {
                return {
                    'RunIDDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value,
                    'RegionDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRegion").value,
                    'PositionDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    'KRAGroupDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedKRA").value,
                    'KPIDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value,
                    'EEMin': $("#txtFilterEEMin").val(),
                    'EEMax': $("#txtFilterEEMax").val(),
                    'MEMin': $("#txtFilterMEMin").val(),
                    'MEMax': $("#txtFilterMEMax").val(),
                    'SBEMin': $("#txtFilterSBEMin").val(),
                    'SBEMax': $("#txtFilterSBEMax").val(),
                    'BEMin': $("#txtFilterBEMin").val(),
                    'BEMax': $("#txtFilterBEMax").val(),
                };
            }
            else if (type == "SUMMARY_RESULTS_BRN") {
                return {
                    'RunIDDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value,
                    'BranchDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedBranch").value,
                    'PositionDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    'KRAGroupDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedKRA").value,
                    'KPIDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value,
                    'EEMin': $("#txtFilterEEMin").val(),
                    'EEMax': $("#txtFilterEEMax").val(),
                    'MEMin': $("#txtFilterMEMin").val(),
                    'MEMax': $("#txtFilterMEMax").val(),
                    'SBEMin': $("#txtFilterSBEMin").val(),
                    'SBEMax': $("#txtFilterSBEMax").val(),
                    'BEMin': $("#txtFilterBEMin").val(),
                    'BEMax': $("#txtFilterBEMax").val(),
                };
            }
            else if (type == "SUMMARY_RESULTS_POS") {
                return {
                    'RunIDDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value,
                    'PositionDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    'KRAGroupDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedKRA").value,
                    'KPIDelimited': objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value,
                    'EEMin': $("#txtFilterEEMin").val(),
                    'EEMax': $("#txtFilterEEMax").val(),
                    'MEMin': $("#txtFilterMEMin").val(),
                    'MEMax': $("#txtFilterMEMax").val(),
                    'SBEMin': $("#txtFilterSBEMin").val(),
                    'SBEMax': $("#txtFilterSBEMax").val(),
                    'BEMin': $("#txtFilterBEMin").val(),
                    'BEMax': $("#txtFilterBEMax").val(),
                };
            }
        },

        LoadDashboardData: function (type) {
            var s = this;
            s.SetLocalStorage();
            ResetJQGridState("tblIPMDashboardCountList");
            if (type == "SUMMARY_FOR_EVAL") {
                s.LoadJQGridSummaryForEvaluation(s.GetParameters(type));
            }
            else if (type == "SUMMARY_FOR_APPR") {
                s.LoadJQGridSummaryForApproval(s.GetParameters(type));
            }
            else if (type == "SUMMARY_FOR_APPR_BRN") {
                s.LoadJQGridSummaryForApprovalBRN(s.GetParameters(type));
            }
            else if (type == "SUMMARY_FOR_APPR_CLU") {
                s.LoadJQGridSummaryForApprovalCLU(s.GetParameters(type));
            }
            else if (type == "SUMMARY_RESULTS_REG") {
                s.LoadJQGridRegionalWithPosition(s.GetParameters(type));
            }
            else if (type == "SUMMARY_RESULTS_BRN") {
                s.LoadJQGridBranchesWithPosition(s.GetParameters(type));
            }
            else if (type == "SUMMARY_RESULTS_POS") {
                s.LoadJQGridPositionOnly(s.GetParameters(type));
            }

        },

        LoadJQGridSummaryForEvaluation: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: SummaryForEvaluationURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Region", "With Complete Score", "With Missing Score", "No Score", "On Going Evaluation"],
                colModel: [
                    { name: "Region", index: "Region", editable: false, align: "left", width: 150 },
                    { name: "WithCompleteScore", index: "WithCompleteScore", editable: false, align: "right" },
                    { name: "WithMissingScore", index: "WithMissingScore", editable: false, align: "right" },
                    { name: "NoScore", index: "NoScore", editable: false, align: "right" },
                    { name: "OnGoingEvaluation", index: "OnGoingEvaluation", editable: false, align: "right" },
                ],
                toppager: $("#divPager2"),
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
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMDashboardCountList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                    }

                    if (localStorage["KPIPositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIPositionListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objDashboardListJS.ShowHideFilter();
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
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridSummaryForApproval: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: SummaryForApprovalURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Region", "No Key In", "For Approval", "Finalized/Approved" ],
                colModel: [
                    { name: "Region", index: "Region", editable: false, align: "left", width: 150 },
                    { name: "NoKeyIn", index: "NoKeyIn", editable: false, align: "right" },
                    { name: "ForApproval", index: "ForApproval", editable: false, align: "right" },
                    { name: "Finalized", index: "Finalized", editable: false, align: "right" },
                ],
                toppager: $("#divPager2"),
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
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMDashboardCountList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                    }

                    if (localStorage["KPIPositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIPositionListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objDashboardListJS.ShowHideFilter();
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
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridSummaryForApprovalBRN: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: SummaryForApprovalBRNURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Region", "Branch", "No Key In", "For Approval", "Finalized/Approved" ],
                colModel: [
                    { name: "Region", index: "Region", editable: false, align: "left", width: 150 },
                    { name: "Branch", index: "Branch", editable: false, align: "left" },
                    { name: "NoKeyIn", index: "NoKeyIn", editable: false, align: "right" },
                    { name: "ForApproval", index: "ForApproval", editable: false, align: "right" },
                    { name: "Finalized", index: "Finalized", editable: false, align: "right" },
                ],
                toppager: $("#divPager2"),
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
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMDashboardCountList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                    }

                    if (localStorage["KPIPositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIPositionListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objDashboardListJS.ShowHideFilter();
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
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridSummaryForApprovalCLU: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: SummaryForApprovalCLUURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Region", "Cluster", "No Key In", "For Approval", "Finalized/Approved" ],
                colModel: [
                    { name: "Region", index: "Region", editable: false, align: "left", width: 150 },
                    { name: "Cluster", index: "Cluster", editable: false, align: "left" },
                    { name: "NoKeyIn", index: "NoKeyIn", editable: false, align: "right" },
                    { name: "ForApproval", index: "ForApproval", editable: false, align: "right" },
                    { name: "Finalized", index: "Finalized", editable: false, align: "right" },
                ],
                toppager: $("#divPager2"),
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
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMDashboardCountList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                    }

                    if (localStorage["KPIPositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIPositionListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objDashboardListJS.ShowHideFilter();
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
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridRegionalWithPosition: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: RegionalWithPositionURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Region", "Position", "KRA", "KPI Name", "EE", "ME", "SBE", "BE"],
                colModel: [
                    { name: "Region", index: "Region", editable: false, align: "left", width: 150 },
                    { name: "Position", index: "Position", editable: false, align: "left", width: 150 },
                    { name: "KRAGroup", index: "KRAGroup", editable: false, align: "left", width: 150 },
                    { name: "KPI", index: "KPI", editable: false, align: "left", width: 150 },
                    { name: "EE", index: "EE", editable: false, align: "right", width: 50 },
                    { name: "ME", index: "ME", editable: false, align: "right", width: 50 },
                    { name: "SBE", index: "SBE", editable: false, align: "right", width: 50 },
                    { name: "BE", index: "BE", editable: false, align: "right", width: 50 },
                ],
                toppager: $("#divPager2"),
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
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMDashboardCountList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    if (localStorage["KPIPositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIPositionListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objDashboardListJS.ShowHideFilter();
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
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridBranchesWithPosition: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: BranchesWithPositionURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Branches", "Position", "KRA", "KPI Name", "EE", "ME", "SBE", "BE"],
                colModel: [
                    { name: "Branch", index: "Branch", editable: false, align: "left", width: 150 },
                    { name: "Position", index: "Position", editable: false, align: "left", width: 150 },
                    { name: "KRAGroup", index: "KRAGroup", editable: false, align: "left", width: 150 },
                    { name: "KPI", index: "KPI", editable: false, align: "left", width: 150 },
                    { name: "EE", index: "EE", editable: false, align: "right", width: 50  },
                    { name: "ME", index: "ME", editable: false, align: "right", width: 50 },
                    { name: "SBE", index: "SBE", editable: false, align: "right", width: 50 },
                    { name: "BE", index: "BE", editable: false, align: "right", width: 50 },
                ],
                toppager: $("#divPager2"),
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
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMDashboardCountList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                    }

                    if (localStorage["KPIPositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIPositionListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objDashboardListJS.ShowHideFilter();
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
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    GetJQGridState("tblIPMDashboardCountList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        LoadJQGridPositionOnly: function (param) {
            Loading(true);
            var tableInfo = localStorage.getItem("tblIPMDashboardCountList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMDashboardCountList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $(".jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblIPMDashboardCountList").jqGrid("GridUnload");
            $("#tblIPMDashboardCountList").jqGrid("GridDestroy");
            $("#tblIPMDashboardCountList").jqGrid({
                url: PositionOnlyURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["Position", "KRA", "KPI Name", "EE", "ME", "SBE", "BE"],
                colModel: [
                    { name: "Position", index: "Position", editable: false, align: "left", width: 150 },
                    { name: "KRAGroup", index: "KRAGroup", editable: false, align: "left", width: 150 },
                    { name: "KPI", index: "KPI", editable: false, align: "left", width: 150 },
                    { name: "EE", index: "EE", editable: false, align: "right", width: 50 },
                    { name: "ME", index: "ME", editable: false, align: "right", width: 50 },
                    { name: "SBE", index: "SBE", editable: false, align: "right", width: 50 },
                    { name: "BE", index: "BE", editable: false, align: "right", width: 50 },
                ],
                toppager: $("#divPager2"),
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
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMDashboardCountList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                    }

                    if (localStorage["KPIPositionListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIPositionListFilterOption"]));
                    }
                    objDashboardListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objDashboardListJS.ShowHideFilter();
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
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    GetJQGridState("tblIPMDashboardCountList");
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
            localStorage["DashboardCountListType"] = $("#ddlDashboard").val();

            localStorage["DashboardRunID"] = objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").value;
            localStorage["DashboardRunIDText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedRunID").text;
        },

        GetLocalStorage: function () {
            $("#ddlDashboard").val(localStorage["DashboardCountListType"]);
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },

        BindFilterMultiSelectAutoCompleteRunID: function (id, url, noOfReturnedResults, selectedDivId) {
            var _noOfReturnedResults = noOfReturnedResults;
            var _incrementDisplayedBy = 20;

            // isFocus - to prevent overlapping of focus and click events
            var isFocus = false;
            $("#" + id).focus(function () {
                _noOfReturnedResults = noOfReturnedResults;
                $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                isFocus = true;
                $("#" + id).unbind("click");
                $("#" + id).click(function () {
                    if (!isFocus) {
                        _noOfReturnedResults = noOfReturnedResults;
                        $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                    }
                    isFocus = false;
                });
            });


            $("#" + id).keyup(function () {
                _noOfReturnedResults = noOfReturnedResults;
            });

            $("#" + id).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: url,
                        type: "GET",
                        dataType: "json",
                        data: {
                            term: request.term,
                            topResults: _noOfReturnedResults // No of returned results
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                response($.map(data.Result, function (item) {
                                    return {
                                        label: item.Description,
                                        value: item.ID
                                    };
                                }))
                            }
                            else {
                                ModalAlert(MODAL_HEADER, data.Result);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            ModalAlert(MODAL_HEADER, jqXHR.responseText);
                        }
                    });
                },
                select: function (event, ui) { // Event - triggers after selection on list
                    if (ui.item.label != null) {
                        $(this).val("");
                        var id = ui.item.value;
                        $("#" + selectedDivId).focus();
                        $("#" + selectedDivId).append('<label class="multiselect-item selected_' + id + '" title="delete">' + ui.item.label +
                            ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEMSCommonJS.RemoveFromMultiSelect(&quot;' + selectedDivId + '&quot;,&quot;' + id + '&quot;);"></span></label>');
                        $("#" + selectedDivId).append('<input type="hidden" class="selected-item hdn_selected_' + id + '" value="' + id + '" />');
                        $("#gbox_tblIPMDashboardCountList").css({ "top": ($("#divAdditionalFilter").height() - divAdditionalFilterHeight + 10) + "px" });
                    }
                    return false;
                },
                focus: function (event, ui) {
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            // reload additional results if lowest scroll is detected 
            $("#" + id).autocomplete("widget").scroll(function () {
                //    $(this).scrollTop() + (window.devicePixelRatio * 100 - 100 /*Subtract pixel ration from Zooming the page*/)
                //) + " | innerHeight = " + $(this).innerHeight() + " | scrollHeight = " + $(this)[0].scrollHeight);
                if (
                    (
                        (
                            $(this).scrollTop() + Math.abs(window.devicePixelRatio * 100 - 100 /*Subtract pixel ration from Zooming the page*/)
                        ) + $(this).innerHeight()
                    ) >= $(this)[0].scrollHeight) {
                    _noOfReturnedResults = _noOfReturnedResults + _incrementDisplayedBy;
                    $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                }
            });

            $("#" + id).autocomplete("widget").attr('style', 'max-height: 400px; overflow-y: auto; overflow-x: hidden;');
        },
    };

    objDashboardListJS.Initialize();
});