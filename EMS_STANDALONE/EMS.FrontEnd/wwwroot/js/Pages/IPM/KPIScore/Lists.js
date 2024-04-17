var objKPIScoreListJS;
const KPIScoreListURL = "/IPM/KPIScore?handler=List";
const KPIScoreAddURL = "/IPM/KPIScore/Add";
const KPIScoreViewURL = "/IPM/KPIScore/View";
const KPIScoreEditURL = "/IPM/KPIScore/Edit";
const KPIScoreDeleteURL = "/IPM/KPIScore/Delete";
const KPIScoreAddPostURL = "/IPM/KPIScore/Add";
const KPIScoreEditPostURL = "/IPM/KPIScore/Edit";
const GetCheckExportListURL = "/IPM/KPIScore?handler=CheckExportList";
const DownloadExportListURL = "/IPM/KPIScore?handler=DownloadExportList";
const DownloadExportPerEmployeeListURL = "/IPM/KPIScore?handler=DownloadExportPerEmployeeList";

const ValidateUploadScoresURL = "/IPM/KPIScore?handler=ValidateUploadScores";
const UploadScoresURL = "/IPM/KPIScore?handler=UploadScores";
const DownloadFormURL = "/IPM/KPIScore?handler=DownloadGroupTemplate";
const GetEmployeeAutoCompleteURL = "/IPM/KPIScore?handler=EmployeeAutoComplete";

const ValidateUploadScoresPerEmployeeURL = "/IPM/KPIScore?handler=ValidateUploadScoresPerEmployee";
const UploadScoresPerEmployeeURL = "/IPM/KPIScore?handler=UploadScoresPerEmployee";
const DownloadFormPerEmployeeURL = "/IPM/KPIScore?handler=DownloadGroupPerEmployeeTemplate";

const GetPositionByOrgGroupDropdownURL = "/IPM/KPIScore?handler=PositionByOrgGroupDropdown";
const GetKPIByPositionURL = "/IPM/KPIScore?handler=KPIByPosition";

const GetOrgGroupAutoCompleteURL = "/IPM/KPIScore?handler=OrgTypeAutoComplete";
const GetKPIAutoCompleteURL = "/IPM/KPIScore?handler=KPIAutoComplete";
const GetPositionAutoCompleteURL = "/IPM/KPIScore?handler=PositionAutoComplete";

var filedata = new FormData();

$(document).ready(function() {
    objKPIScoreListJS = {
        Initialize: function () {
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();

            s.GetLocalStorage();

            $("#ddlKPIType").css({ "width": "380px" });
            $("#ddlKPIType").change();
        },

        ElementBinding: function() {
            var s = this;

            NumberOnly($("#txtFilterID"));

            $("#txtFilterPeriodFrom, #txtFilterPeriodTo").datetimepicker({
                useCurrent: false,
                format: 'MM/YYYY'
            });

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterID").val(),
                    ParentOrgGroup: $("#txtFilterParentOrgGroup").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    EmployeeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedName").value,
                    KPIDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value,
                    Target: $("#txtFilterTarget").val(),
                    Actual: $("#txtFilterActual").val(),
                    Rate: $("#txtFilterRate").val(),
                    PeriodFrom: $("#txtFilterPeriodFrom").val(),
                    PeriodTo: $("#txtFilterPeriodTo").val(),
                    KPIType: $("#ddlKPIType").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblIPMKPIScoreList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function() {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedName").html("");
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedKPI").html("");
                $("#btnSearch").click();
            });

            $("#btnUploadScores").click(function () {
                if ($("#ddlKPIType").val() == "QUANTITATIVE")
                    objKPIScoreListJS.UploadModal(UploadScoresURL, "Upload Scores", DownloadFormURL);
                else if ($("#ddlKPIType").val() == "QUALITATIVE")
                    objKPIScoreListJS.UploadModal(UploadScoresPerEmployeeURL, "Upload Scores", DownloadFormPerEmployeeURL);

                $('#divModalErrorMessage').html('');
            });

            $("#btnAdd").click(function() {
                LoadPartial(KPIScoreAddURL, "divKPIScoreBodyModal");
                $("#divKPIScoreModal").modal("show");
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterName"
                , GetEmployeeAutoCompleteURL, 20, "multiSelectedName");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , GetOrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterKPI"
                , GetKPIAutoCompleteURL, 20, "multiSelectedKPI");

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objKPIScoreListJS.ExportFunction()",
                    "function");
                
            });

            $("#ddlKPIType").change(function () {
                if (($(this).val() || "") != "") {
                    var param = {
                        ID: $("#txtFilterID").val(),
                        ParentOrgGroup: $("#txtFilterParentOrgGroup").val(),
                        EmployeeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedName").value,
                        OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                        KPIDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value,
                        Target: $("#txtFilterTarget").val(),
                        Actual: $("#txtFilterActual").val(),
                        Rate: $("#txtFilterRate").val(),
                        PeriodFrom: $("#txtFilterPeriodFrom").val(),
                        PeriodTo: $("#txtFilterPeriodTo").val(),
                        KPIType: $("#ddlKPIType").val()
                    };
                    s.LoadJQGrid(param);
                    s.SetLocalStorage();
                }
            });
        },

        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblIPMKPIScoreList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblIPMKPIScoreList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterID").val()
                + "&ParentOrgGroup=" + $("#txtFilterParentOrgGroup").val()
                + "&EmployeeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedName").value
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                + "&KPIDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value
                + "&Target=" + $("#txtFilterTarget").val()
                + "&Actual=" + $("#txtFilterActual").val()
                + "&Rate=" + $("#txtFilterRate").val()
                + "&PeriodFrom=" + $("#txtFilterPeriodFrom").val()
                + "&PeriodTo=" + $("#txtFilterPeriodTo").val()
                + "&KPIType=" + $("#ddlKPIType").val();

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location =
                        ($("#ddlKPIType").val() == "QUANTITATIVE" ? DownloadExportListURL : /*QUALITATIVE*/ DownloadExportPerEmployeeListURL) + parameters;
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
            var tableInfo = localStorage.getItem("tblIPMKPIScoreList") == "" ? "" : $.parseJSON(localStorage.getItem("tblIPMKPIScoreList"));
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
            $("#tblIPMKPIScoreList").jqGrid("GridUnload");
            $("#tblIPMKPIScoreList").jqGrid("GridDestroy");
            $("#tblIPMKPIScoreList").jqGrid({
                url: KPIScoreListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Period", "Employee", "Org. Group", "KPI", "Target", "Actual", "Rate", "Parent Org. Group"],
                colModel: [
                    { hidden: true },
                    { width: 60, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objKPIScoreListJS.ViewLink },
                    { width: 80, name: "Period", index: "Period", editable: true, align: "center" },
                    { width: 250, name: "Employee", index: "Employee", editable: true, align: "left" },
                    { width: 250, name: "OrgGroup", index: "OrgGroup", editable: true, align: "left" },
                    { width: 250, name: "KPI", index: "KPI", editable: true, align: "left" },
                    { width: 80, name: "Target", index: "Target", editable: true, align: "right" },
                    { width: 80, name: "Actual", index: "Actual", editable: true, align: "right" },
                    { width: 80, name: "Rate", index: "Rate", editable: true, align: "right" },
                    { width: 400, name: "ParentOrgGroup", index: "ParentOrgGroup", editable: true, align: "left" },
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
                    $("#tblIPMKPIScoreList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                            if (param.KPIType == "QUALITATIVE") {
                                $("#tblIPMKPIScoreList").jqGrid('showCol', ["Employee"]);
                                $("#tblIPMKPIScoreList").jqGrid('hideCol', ["OrgGroup"]);
                                $("#tblIPMKPIScoreList").jqGrid('hideCol', ["ParentOrgGroup"]);
                            }
                            else if (param.KPIType == "QUANTITATIVE") {
                                $("#tblIPMKPIScoreList").jqGrid('hideCol', ["Employee"]);
                                $("#tblIPMKPIScoreList").jqGrid('showCol', ["OrgGroup"]);
                                $("#tblIPMKPIScoreList").jqGrid('showCol', ["ParentOrgGroup"]);
                            }
                        }
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblIPMKPIScoreList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function(n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblIPMKPIScoreList .jqgrid-id-link").click(function() {
                            $('#divKPIScoreModal').modal('show');
                        });
                    }

                    if (localStorage["KPIListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["KPIListFilterOption"]));
                    }
                    objKPIScoreListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objKPIScoreListJS.ShowHideFilter();
                        localStorage["KPIListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#gview_tblIPMKPIScoreList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

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
                    GetJQGridState("tblIPMKPIScoreList");
                    if (param.KPIType == "QUALITATIVE") {
                        $("#tblIPMKPIScoreList").jqGrid('showCol', ["Employee"]);
                        $("#tblIPMKPIScoreList").jqGrid('hideCol', ["OrgGroup"]);
                        $("#tblIPMKPIScoreList").jqGrid('hideCol', ["ParentOrgGroup"]);
                    }
                    else if (param.KPIType == "QUANTITATIVE") {
                        $("#tblIPMKPIScoreList").jqGrid('hideCol', ["Employee"]);
                        $("#tblIPMKPIScoreList").jqGrid('showCol', ["OrgGroup"]);
                        $("#tblIPMKPIScoreList").jqGrid('showCol', ["ParentOrgGroup"]);
                    }
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
            localStorage["IPMKPIScoreListKPIType"] = $("#ddlKPIType").val();

            localStorage["IPMKPIScoreListID"] = $("#txtFilterID").val();
            localStorage["IPMKPIScoreListParentOrgGroup"] = $("#txtFilterParentOrgGroup").val();
            localStorage["IPMKPIScoreListEmployeeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedName").value;
            localStorage["IPMKPIScoreListEmployeeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedName").text;
            localStorage["IPMKPIScoreListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["IPMKPIScoreListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;
            localStorage["IPMKPIScoreListKPIDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").value;
            localStorage["IPMKPIScoreListKPIDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedKPI").text;
            localStorage["IPMKPIScoreListTarget"] = $("#txtFilterTarget").val();
            localStorage["IPMKPIScoreListActual"] = $("#txtFilterActual").val();
            localStorage["IPMKPIScoreListRate"] = $("#txtFilterRate").val();
            localStorage["IPMKPIScoreListPeriodFrom"] = $("#txtFilterPeriodFrom").val();
            localStorage["IPMKPIScoreListPeriodTo"] = $("#txtFilterPeriodTo").val();
            
        },

        GetLocalStorage: function () {
            $("#ddlKPIType").val(localStorage["IPMKPIScoreListKPIType"]);

            $("#txtFilterID").val(localStorage["IPMKPIScoreListID"]);
            $("#txtFilterParentOrgGroup").val(localStorage["IPMKPIScoreListParentOrgGroup"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedName"
                , "IPMKPIScoreListEmployeeDelimited"
                , "IPMKPIScoreListEmployeeDelimitedText");

              objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "IPMKPIScoreListOrgGroupDelimited"
                , "IPMKPIScoreListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedKPI"
                , "IPMKPIScoreListKPIDelimited"
                , "IPMKPIScoreListKPIDelimitedText");

            $("#txtFilterTarget").val(localStorage["IPMKPIScoreListTarget"]);
            $("#txtFilterActual").val(localStorage["IPMKPIScoreListActual"]);
            $("#txtFilterRate").val(localStorage["IPMKPIScoreListRate"]);
            $("#txtFilterPeriodFrom").val(localStorage["IPMKPIScoreListPeriodFrom"]);
            $("#txtFilterPeriodTo").val(localStorage["IPMKPIScoreListPeriodTo"]);

        },

        ViewLink: function (cellvalue, options, rowObject) {
            
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + KPIScoreViewURL + "?ID=" + rowObject.ID + "&KPIType=" + $("#ddlKPIType").val() + "', 'divKPIScoreBodyModal'); \">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },

        UploadModal: function (url, header, sampleFormURL) {
            $("#divModalErrorMessage").html("");
            $('#fileUpload').removeClass("errMessage");

            var button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objKPIScoreListJS.UploadFile();\">Upload</button>";

            if (header != "")
                header += "<button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>";
            $("#pheader").html(header);

            if (sampleFormURL != "")
                button += "&nbsp <button type=\"button\" id=\"btnDownloadForm\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objEMSCommonJS.UploadModalDownloadTemplate('" + sampleFormURL + "');\">Download Template</button>";

            $("#fileUpload").val("");
            $("#divButtonUploadModal").html(button);
            $("#divUploadModal").modal("show");
        },

        UploadValidateFunction: function (msg) {
            if ($("#ddlKPIType").val() == "QUANTITATIVE") {
                ModalConfirmation(MODAL_HEADER, msg,
                    "objEMSCommonJS.PostAjax(true \
                        , UploadScoresURL \
                        , filedata \
                        ,'#divModalErrorMessage' \
                        , '' \
                        , objKPIScoreListJS.UploadSuccessFunction);",
                    "function");
            }
            else if ($("#ddlKPIType").val() == "QUALITATIVE") {
                ModalConfirmation(MODAL_HEADER, msg,
                    "objEMSCommonJS.PostAjax(true \
                        , UploadScoresPerEmployeeURL \
                        , filedata \
                        ,'#divModalErrorMessage' \
                        , '' \
                        , objKPIScoreListJS.UploadSuccessFunction);",
                    "function");
            }
        },

        UploadSuccessFunction: function () {
            $("#btnSearch").click();
        },

        UploadFile: function () {
            filedata = new FormData();
            var fileExtension = ['xls', 'xlsx'];
            var filename = $('#fileUpload').val();
            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;
            
            //--- Validation for excel file---  
            if (filename.length == 0) {
                $("#divModalErrorMessage").html("<label class=\"errMessage\"><li>Please select a file.</li></label><br />");
                return false;
            }
            else {
                var extension = filename.replace(/^.*\./, '');
                if ($.inArray(extension, fileExtension) == -1) {
                    $("#divModalErrorMessage").html("<label class=\"errMessage\"><li>Please select only excel files.</li></label><br />");
                    return false;
                }
                else {
                    $("#divModalErrorMessage").html("");
                }
            }

            filedata.append(files[0].name, files[0]);

            //objKPIScoreListJS.ValidateUploadScores(true, ValidateUploadScoresURL, filedata, "#divModalErrorMessage");

            var successFunction = function (data) {
                if (data.Result != "Valid data") {

                    if (data.Result == "Override")
                        objKPIScoreListJS.UploadValidateFunction(MSG_OVERRIDE_SCORES);
                    else
                        ModalAlert(MODAL_HEADER, data.Result);
                }
                else
                    objEMSCommonJS.PostAjax(true, ($("#ddlKPIType").val() == "QUANTITATIVE" ? UploadScoresURL : UploadScoresPerEmployeeURL), filedata, "#divModalErrorMessage", "", objKPIScoreListJS.UploadSuccessFunction);
            };

            objEMSCommonJS.PostAjax(true, ($("#ddlKPIType").val() == "QUANTITATIVE" ? ValidateUploadScoresURL : ValidateUploadScoresPerEmployeeURL), filedata, "#divModalErrorMessage", "", successFunction, null, true, true);

            $("#divUploadModal").modal("hide");
        },

        ValidateUploadScores: function (isForm, url, input, divErrID) {
            var contentType = isForm ? false : "application/json;";
            var processData = isForm ? false : true;

            var s = this;
            
            Loading(true);
            $.ajax({
                url: url,
                type: "POST",
                data: input,
                dataType: "json",
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                contentType: contentType,
                processData: processData,
                success: function (data) {
                    if (data.IsSuccess) {

                        if (data.Result != "Valid data") {

                            if (data.Result == "Override")
                                objKPIScoreListJS.UploadValidateFunction(MSG_OVERRIDE_SCORES);
                            else
                                ModalAlert(MODAL_HEADER, data.Result);
                        }
                        else
                            objEMSCommonJS.PostAjax(true, UploadScoresURL, filedata, "#divModalErrorMessage", "", objKPIScoreListJS.UploadSuccessFunction);

                        $(divErrID).html("");
                    }
                    else {
                        var msg = "";
                        if (data.IsListResult == true) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + "<br />";
                            }
                        } else {
                            msg += data.Result;
                        }
                        ModalAlert(MODAL_HEADER, msg);
                    }
                    
                    Loading(false);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Loading(false);
                    
                    ModalAlert(MODAL_HEADER, jqXHR.responseText);
                },
            });
        },
    };

    objKPIScoreListJS.Initialize();
});