var objIndexList;

const ClearedEmployeeListURL = "Cleared?handler=List";
const ClearedEmployeeByIDURL = "Cleared?handler=ClearedEmployeeByID";
const ClearedEmployeeChangeStatusURL = "Cleared?handler=ClearedEmployeeChangeStatus"
const ClearedEmployeeCommentsURL = "Cleared?handler=ClearedEmployeeComments";
const ClearedEmployeeComputationURL = "Cleared?handler=ClearedEmployeeComputation";
const ClearedEmployeeStatusHistoryURL = "Cleared?handler=ClearedEmployeeStatusHistory";
const EmployeeAccountabilityURL = "Cleared?handler=EmployeeAccountability";
const EmployeeMovementByEmployeeIDsURL = "Cleared?handler=EmployeeMovementByEmployeeIDs";
const EmployeeAutoCompleteURL = "Cleared?handler=EmployeeAutoComplete";
const OrgGroupAutoCompleteURL = "Cleared?handler=OrgGroupAutoComplete"; 
const PositionAutoCompleteURL = "Cleared?handler=PositionAutoComplete";
const SystemUserAutoCompleteURL = "Cleared?handler=SystemUserAutoComplete";
const StatusFilterURL = "Cleared?handler=StatusFilter";
const ExportListURL = "Cleared?handler=ExportList";

$(document).ready(function () {
    objIndexList = {

        Initialize: function () {
            $("#divClearedEmployeeModal .modal-header").mousedown(handle_mousedown);
            $("#divComputationModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            s.ElementBinding();

            s.LoadJQGrid();

            $('#txtComputation').summernote({
                toolbar: [
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'style', 'paragraph']],
                    ['height', ['height']]
                ],
                height: 200
            });
        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterStatusUpdatedDateFrom, #txtFilterStatusUpdatedDateTo, \
               #txtFilterLastCommentDateFrom,#txtFilterLastCommentDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterEmployeeName"
                , EmployeeAutoCompleteURL, 20, "multiSelectedEmployeeName");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , PositionAutoCompleteURL, 20, "multiSelectedPosition");
            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedStatus"
                , StatusFilterURL, "ID", "Description");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterStatusUpdatedBy"
                , SystemUserAutoCompleteURL, 20, "multiSelectedStatusUpdatedBy");

            $("#btnSearch").on("click", function () {
                //console.log(objIndexList.CheckIfComputationExist(6));
                s.LoadJQGrid(objIndexList.GetFilter());
            });
            $("#btnReset").on("click", function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedEmployeeName").html("");
                $("#multiSelectedEmployeeNameOption label, #multiSelectedEmployeeNameOption input").prop("title", "add");
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedOrgGroupOption label, #multiSelectedOrgGroupOption input").prop("title", "add");
                $("#multiSelectedPosition").html("");
                $("#multiSelectedPositionOption label, #multiSelectedPositionOption input").prop("title", "add");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#multiSelectedStatusUpdatedBy").html("");
                $("#multiSelectedStatusUpdatedByOption label, #multiSelectedStatusUpdatedByOption input").prop("title", "add");
                $("#btnSearch").click();
            });
            $("#btnSaveComment").click(function () {
                if ($("#txtAreaComments").val() != "") {
                    objEMSCommonJS.PostAjax(true
                        , ClearedEmployeeCommentsURL
                        , objIndexList.GetCommentFormData()
                        , '#divClearedEmployeeErrorMessage'
                        , '#btnSaveComment'
                        , objIndexList.GetComment, null, true);
                }
                else {
                    $("#txtAreaComments").focus();
                }
            });

            $("#btnSaveComputation").click(function () {
                if ($("#txtComputation").val() != "") {
                    var SuccessFunction = function () {
                        $('#idComputation').html($('#txtComputation').val());
                        $('#divComputationModal').modal('hide');
                    }

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , ClearedEmployeeComputationURL \
                        , objIndexList.GetComputationFormData() \
                        , '#divClearedEmployeeComputationErrorMessage' \
                        , '#btnSaveComputation' \
                        , "+ SuccessFunction +");",
                        "function");
                }
                else {
                    $("#txtComputation").focus();
                }
            });

            $("#btnComputation").on("click", function () {
                $('#divComputationModal').modal('show');
            });

            $("#divClearedEmployeeModal #btnChangeStatus").click(function () {
                if ($("#ChangeStatusModal").is(":visible"))
                    $("#ChangeStatusModal").hide();
                else {
                    $("#ddlDynamicChangeStatus,#txtDynamicChangeStatusRemarks").prop("disabled", false);
                    $("#ddlDynamicChangeStatus").addClass("required-field");
                    $("#ChangeStatusModal").show();

                    GenerateDropdownValues(ClearedEmployeeChangeStatusURL + "&CurrentStatus=" + $("#hdnStatus").val(), "divClearedEmployeeModal #ddlDynamicChangeStatus", "Value", "Text", "", "", false);
                }
            });

            $("#btnClearedEmployeeChangeStatus").click(function () {
                $('#divClearedEmployeeListErrorMessage').html('');
                var selRow = $("#tblClearedEmployeeList").jqGrid("getGridParam", "selarrrow");
                var firstValue = "";
                var isValid = true;
                if (selRow.length > 0) {
                    $(selRow).each(function (index, item) {
                        if (firstValue == "")
                            firstValue = $("#tblClearedEmployeeList").getRowData(item).StatusCode;
                        else if (firstValue != $("#tblClearedEmployeeList").getRowData(item).StatusCode)
                            isValid = false;
                    });
                    if (isValid) {
                        $(".editRequired").addClass("reqField");
                        $("#divChangeStatusModal").modal("show");
                        $('#divClearedEmployeeChangeStatusErrorMessage').html('');
                        $("#txtChangeStatusRemarks").val("");
                        $("#ClearedEmployeeChangeStatusID").val(selRow);

                        GenerateDropdownValues(ClearedEmployeeChangeStatusURL + "&CurrentStatus=" + firstValue, "ddlClearedEmployeeChangeStatus", "Value", "Text", "", "", false);
                    }
                    else
                        $("#divClearedEmployeeListErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_DIFF_STATUS + " </li></label><br />");
                }
                else {
                    $("#divClearedEmployeeListErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Accountability</li></label><br />");
                }
            });
            $("#btnSaveClearedEmployeeChangeStatus").click(function () {
                $('#divClearedEmployeeChangeStatusErrorMessage').html('');

                if ($("#ddlClearedEmployeeChangeStatus").val() != "") {
                    if ($("#ddlClearedEmployeeChangeStatus").val() == "CANCELLED" && $("#txtChangeStatusRemarks").val().trim() == "") {
                        $("#divClearedEmployeeChangeStatusErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_INPUT_REMARKS + "</li></label><br />");
                        $("#txtChangeStatusRemarks").addClass("required-field errMessage");
                        return;
                    }
                    $("#txtChangeStatusRemarks").removeClass("required-field errMessage");
                    var ID = $("#ClearedEmployeeChangeStatusID").val();

                    var SuccessFunction = function () {
                        $('#divChangeStatusModal').modal('hide');
                        $('#txtChangeStatusRemarks').val('');
                        $('#btnSearch').click();
                    }
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_UPDATE + SPAN_DELETE_START + " ID : " + ID + SPAN_END,
                        "objEMSCommonJS.PostAjax(true \
                        , '" + ClearedEmployeeChangeStatusURL + "&ID=" + ID + "&NewStatus=" + $("#ddlClearedEmployeeChangeStatus").val() + "&Remarks=" + $("#txtChangeStatusRemarks").val() + "' \
                        , {} \
                        , '#divClearedEmployeeChangeStatusErrorMessage' \
                        , '#btnSaveClearedEmployeeChangeStatus' \
                        , "+ SuccessFunction +");",
                        "function");
                }
                else
                    $("#divClearedEmployeeChangeStatusErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + "</li></label><br />");
            });

            $("#divClearedEmployeeModal #btnCancelDynamicChangeStatus").on("click", function () {
                $("#divClearedEmployeeModal #ChangeStatusModal").hide();
            });

            $("#divClearedEmployeeModal #btnSaveDynamicChangeStatus").click(function () {
                $("#divClearedEmployeeModal #divDynamicChangeStatusErrorMessage").html("");

                if ($("#divClearedEmployeeModal #ddlDynamicChangeStatus :selected").val() == "") {
                    $("#divClearedEmployeeModal #divDynamicChangeStatusErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_NO_SELECTED_STATUS + " Movement</li></label><br />");
                    return;
                }

                if ($("#ddlDynamicChangeStatus").val() == "CANCELLED" && $("#txtDynamicChangeStatusRemarks").val().trim() == "") {
                    $("#divDynamicChangeStatusErrorMessage").append("<label class=\"errMessage\"><li>" + ERR_INPUT_REMARKS + "</li></label><br />");
                    $("#txtDynamicChangeStatusRemarks").addClass("required-field errMessage");
                    return;
                }

                var isNoBlankFunction = function () {
                    return true;
                };
                if (objEMSCommonJS.ValidateBlankFields("#divClearedEmployeeModal #DynamicChangeStatusForm", "#divClearedEmployeeModal #divDynamicChangeStatusErrorMessage", isNoBlankFunction)) {
                    var SuccessFunction = function () {
                        $('#ChangeStatusModal').hide();
                        $('#btnSearch').click();
                    }
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , '" + ClearedEmployeeChangeStatusURL + "&ID=" + $("#idClearedEmployeeID").val() + "&NewStatus=" + $("#ddlDynamicChangeStatus :selected").val() + "&Remarks=" + $("#txtDynamicChangeStatusRemarks").val() + "' \
                        , null \
                        , '#divClearedEmployeeModal #divDynamicChangeStatusErrorMessage' \
                        , '#divClearedEmployeeModal #btnSaveDynamicChangeStatus' \
                        , "+SuccessFunction+");",
                        "function");
                }
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objIndexList.ExportFunction()",
                    "function");
            });
        },
        GetFilter: function () {
            var param = {
                ID: $("#txtFilterID").val(),
                EmployeeID: objEMSCommonJS.GetMultiSelectList("multiSelectedEmployeeName").value,
                OrgGroupID: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                PositionID: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                Status: objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value,
                StatusUpdatedByID: objEMSCommonJS.GetMultiSelectList("multiSelectedStatusUpdatedBy").value,
                StatusUpdatedDateFrom: $("#txtFilterStatusUpdatedDateFrom").val(),
                StatusUpdatedDateTo: $("#txtFilterStatusUpdatedDateTo").val(),
                StatusRemarks: $("#txtFilterStatusRemarks").val(),
                Computation: $("#txtFilterComputation").val(),
                LastComment: $("#txtFilterLastComment").val(),
                LastCommentFrom: $("#txtFilterLastCommentDateFrom").val(),
                LastCommentTo: $("#txtFilterLastCommentDateTo").val(),
            }
            return param;
        },
        GetCommentFormData: function () {
            var formData = new FormData($('#frmClearedEmployee').get(0));
            formData.append("param.ClearedEmployeeID", $("#idClearedEmployeeID").val());
            formData.append("param.Comments", $("#txtAreaComments").val());
            return formData;
        },
        GetComputationFormData: function () {
            var formData = new FormData($('#frmClearedEmployeeComputation').get(0));
            formData.append("param.ClearedEmployeeID", $("#idClearedEmployeeID").val());
            formData.append("param.Computation", $("#txtComputation").val());
            return formData;
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblClearedEmployeeList") == "" ? "" : $.parseJSON(localStorage.getItem("tblClearedEmployeeList"));

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
            };
            moveFilterFields();
            $("#tblClearedEmployeeList").jqGrid("GridUnload");
            $("#tblClearedEmployeeList").jqGrid("GridDestroy");
            $("#tblClearedEmployeeList").jqGrid({
                url: ClearedEmployeeListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: [
                    "ID", "Employee Name", "Org Group", "Position", "Accountability", "Status", "Status Updated By", "Status Updated Date",
                    "Status Remarks", "Computation", "Agreement", "Agreement Date", "Last Comment", "Last Comment Date", ""
                ],
                colModel: [
                    {
                        key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objIndexList.Open, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="border-left:10px solid ' + rowObject.StatusColor + ' !important;"';
                        }
                    },
                    { name: "FullName", index: "FullName", align: "left", sortable: true },
                    { name: "OrgGroup", index: "OrgGroup", align: "left", sortable: true },
                    { name: "Position", index: "Position", align: "left", sortable: true },
                    { name: "Accountability", index: "Accountability", align: "center", sortable: true },
                    {
                        name: "StatusDescription", index: "StatusDescription", align: "left", sortable: true, cellattr: function (rowId, val, rowObject, cm, rdata) {
                            return 'style="background-color:' + rowObject.StatusColor + '"';
                        }
                    },
                    { name: "StatusUpdatedBy", index: "StatusUpdatedBy", align: "left", sortable: true },
                    { name: "StatusUpdatedDate", index: "StatusUpdatedDate", align: "center", sortable: true },
                    { name: "StatusRemarks", index: "StatusRemarks", align: "left", sortable: true },
                    {
                        name: "Computation", index: "Computation", align: "left", sortable: true, formatter: function (cellvalue, options, rowObject) {
                            return (rowObject.Computation == null ? "" : rowObject.Computation.replace(/(<([^>]+)>)/gi, ' '));
                        }
                    },
                    { name: "Agreed", index: "Agreed", align: "left", sortable: true },
                    { name: "AgreedDate", index: "AgreedDate", align: "left", sortable: true },
                    { name: "LastComment", index: "LastComment", align: "left", sortable: true },
                    { name: "LastCommentDate", index: "LastCommentDate", align: "center", sortable: true },
                    { hidden: true, name: "StatusCode", index: "StatusCode", align: "center", sortable: true },
                ],
                toppager: $("#divPager"),
                pager: $("#divPager"),
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
                multiselect: true,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblClearedEmployeeList tr:nth-child(" + (iRow + 1) + ") .activity-id-link").click();
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

                        AutoSizeColumnJQGrid("tblClearedEmployeeList", data);

                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });
                    }

                    objIndexList.ShowHideFilter();
                    $("#chkFilter").on('change', function () {
                        objIndexList.ShowHideFilter();
                    });

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
                beforeSelectRow: function (id, e) {
                    var cbsdis = $("tr#" + id + ".jqgrow > td > input.cbox:disabled", $(this));

                    if (cbsdis.length == 0) {
                        return true;    // allow select the row
                    } else {
                        return false;   // not allow select the row
                    }
                },
                onSelectAll: function (aRowids, status) {
                    if (status) {
                        var grid = $("#tblClearedEmployeeList"), i;
                        // uncheck "protected" rows
                        var cbs = $("tr.jqgrow > td > input.cbox:disabled", grid[0]);
                        cbs.removeAttr("checked");

                        //modify the selarrrow parameter
                        grid[0].p.selarrrow = $(this).find("tr.jqgrow:has(td > input.cbox:checked)")
                            .map(function () { return this.id; }) // convert to set of ids
                            .get(); // convert to instance of Array

                        //deselect disabled rows
                        $(this).find("tr.jqgrow:has(td > input.cbox:disabled)")
                            .attr('aria-selected', 'false')
                            .removeClass('ui-state-highlight');
                    }
                },
                beforeRequest: function () {
                    GetJQGridState("tblClearedEmployeeList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#tblClearedEmployeeList_toppager_center").hide();
            $("#tblClearedEmployeeList_toppager_right").hide();
            $("#tblClearedEmployeeList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filters</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\" checked>");
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divPager_custom_block_right").appendTo("#divPager_left");
            $("#divPager_center .ui-pg-table").appendTo("#divPager_right");
            $("#divPager").css("width", "100%");
            $("#divPager").css("height", "100%");
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },
        Open: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return objIndexList.OpenModal('" + ClearedEmployeeByIDURL
                + "&ID=" + rowObject.ID + "');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },
        OpenModal: function (url) {
            $('#divClearedEmployeeModal').modal('show');

            var GetSuccessFunction = function(data) {
                $("#idTitle").html(objEMSCommonJS.JQGridIDFormat(data.Result.ID) + " | " + data.Result.FullName + " | " + data.Result.StatusDescription);
                $("#idAccountability").html(data.Result.Accountability);
                $("#idOrgGroup").html(data.Result.OrgGroup);
                $("#idPosition").html(data.Result.Position);
                $("#idDateHired").html(data.Result.DateHired);
                $("#idAgreed").html(data.Result.Agreed + (data.Result.AgreedDate == null ? "" : ( " - " + data.Result.AgreedDate)));
                $("#idComputation").html(data.Result.Computation);

                $('#txtComputation').summernote('reset');
                $("#txtComputation").summernote('code', data.Result.Computation);

                $("#idStatusUpdatedBy").html(data.Result.StatusUpdatedBy);
                $("#idStatusUpdatedDate").html(data.Result.StatusUpdatedDate);
                $("#idClearedEmployeeID").val(data.Result.ID);
                $("#idEmployeeID").val(data.Result.EmployeeID);
                $("#hdnStatus").val(data.Result.StatusCode);

                objIndexList.GetComment();
                objIndexList.GetStatusHistory();
                objIndexList.GetEmployeeAccountability();
                objIndexList.GetEmployeeMovement();

                objEMSCommonJS.ChangeTab($("span:contains('Comment')").parent(".tablinks"), 'tabComment');
            };

            objEMSCommonJS.GetAjax(url, {}, "", GetSuccessFunction, null, true);
            return false;
        },

        GetComment: function () {
            var input = {
                ClearedEmployeeID: $("#idClearedEmployeeID").val()
            };

            $("#txtAreaComments").val("");

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    $("#divCommentsContainer").html("");
                    if (data.Result.length > 0) {
                        $("#divCommentsContainer").append("<span class='comment-header'>-- Start --</span>");
                        $(data.Result).each(function (index, item) {
                            $("#divCommentsContainer").append("<span class='comment-details'>" + item.CreatedDate
                                + " " + item.Name + ": </span><span class='comment'> " + item.Comments + "</span><br>");
                            if (data.Result.length <= (index + 1)) {
                                setTimeout(function () { $('#divCommentsContainer').scrollTop($('#divCommentsContainer')[0].scrollHeight) }, 200);
                            }
                        });
                    }
                    else {
                        $("#divCommentsContainer").append("<span class='comment-header'>-- No comments found. --</span>");
                    }
                    $("#txtAreaComments").attr("readonly", false);
                    $("#txtAreaComments").prop("disabled", false);
                }
            };

            objEMSCommonJS.GetAjax(ClearedEmployeeCommentsURL, input, "", GetSuccessFunction);
        },
        GetStatusHistory: function () {
            var param = {
                ClearedEmployeeID: $("#idClearedEmployeeID").val()
            };
            var s = this;
            Loading(true);
            $("#tblClearedEmployeeStatusHistoryList").jqGrid("GridUnload");
            $("#tblClearedEmployeeStatusHistoryList").jqGrid("GridDestroy");
            $("#tblClearedEmployeeStatusHistoryList").jqGrid({
                url: ClearedEmployeeStatusHistoryURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["Status", "Updated Date", "Updated By", "Remarks"],
                colModel: [
                    { name: "Status", index: "Status", align: "left", sortable: false },
                    { name: "UpdatedDate", index: "UpdatedDate", align: "left", sortable: false },
                    { name: "UpdatedBy", index: "UpdatedBy", align: "left", sortable: false },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false },
                ], 
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
                            $("#divClearedEmployeeErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
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
        GetEmployeeAccountability: function () {
            var param = {
                EmployeeID: $("#idEmployeeID").val()
            };
            var s = this;
            Loading(true);
            $("#tblEmployeeAccountabilityList").jqGrid("GridUnload");
            $("#tblEmployeeAccountabilityList").jqGrid("GridDestroy");
            $("#tblEmployeeAccountabilityList").jqGrid({
                url: EmployeeAccountabilityURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["Title", "Clearing Department", "Status", "Status Updated Date", "Status Updated By", "Remarks"],
                colModel: [
                    { name: "Title", index: "Title", align: "left", sortable: false },
                    { name: "OrgGroup", index: "OrgGroup", align: "left", sortable: false },
                    { name: "Status", index: "Status", align: "left", sortable: false },
                    { name: "StatusUpdatedDate", index: "StatusUpdatedDate", align: "left", sortable: false },
                    { name: "StatusUpdatedBy", index: "StatusUpdatedBy", align: "left", sortable: false },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false },
                ],
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
                height: "300",
                sortable: true,
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divClearedEmployeeErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                            AutoSizeColumnJQGrid("tblEmployeeAccountabilityList", data);
                        }
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });
        },

        GetEmployeeMovement: function () {
            var param = {
                EmployeeID: $("#idEmployeeID").val()
            };
            var s = this;
            Loading(true);
            $("#tblMovement").jqGrid("GridUnload");
            $("#tblMovement").jqGrid("GridDestroy");
            $("#tblMovement").jqGrid({
                url: EmployeeMovementByEmployeeIDsURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["Movement Type", "Employee Field", "From", "To", "Date Effective From", "Date Effective To"],
                colModel: [
                    { name: "MovementType", index: "MovementType", align: "left", sortable: false },
                    { name: "EmployeeField", index: "EmployeeField", align: "left", sortable: false },
                    { name: "From", index: "From", align: "left", sortable: false },
                    { name: "To", index: "To", align: "left", sortable: false },
                    { name: "DateEffectiveFrom", index: "DateEffectiveFrom", align: "left", sortable: false },
                    { name: "DateEffectiveTo", index: "DateEffectiveTo", align: "left", sortable: false },
                ],
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
                        }

                        AutoSizeColumnJQGrid("tblMovement", data);
                        //$("#tblMovement_subgrid").width(20);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });
        },
        ExportFunction: function () {
            var param = objIndexList.GetFilter();
            param.rows = 1;
            param.IsExport = true;
            var parameter = "";
            $.each(param, function (key,value) {
                parameter += "&" + key + "=" + value;
            });

            Loading(true);
            var GetSuccessFunction = function (data) {
                Loading(false);
                if (data.total > 0) {
                    window.location = ExportListURL + parameter;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };
            objEMSCommonJS.GetAjax(ClearedEmployeeListURL, param, "#btnExport", GetSuccessFunction, null, true);
        },
    };

    objIndexList.Initialize();
});