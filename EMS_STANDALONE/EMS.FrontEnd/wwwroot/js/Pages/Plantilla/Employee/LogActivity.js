var objLogActivityJS;

$(document).ready(function () {
    objLogActivityJS = {

        IsViewMode: false,

        Initialize: function () {
            var s = this;

            GenerateDropdownValues(EmployeeLogActivityPreloadedDropDownURL, "ddlPreloaded", "Value", "Text", "", "", false);
            $("#tblPreloadedLogActivityItemsList").jqGrid("GridUnload");
            $("#tblPreloadedLogActivityItemsList").jqGrid("GridDestroy");
            $("#tblLogActivityList").jqGrid("GridUnload");
            $("#tblLogActivityList").jqGrid("GridDestroy");

            $("#divEmployeeModal #btnAddPreloaded, #divEmployeeModal #btnAddActivity,   \
                #divEmployeeModal #btnBatchAssign").show();

            s.ElementBinding();
            var param = {
                EmployeeID: $("#divEmployeeModal #hdnID").val(),
                TypeDelimited: localStorage["EmpLoyeeLogActivityListType"],
                Title: localStorage["EmpLoyeeLogActivityListTitle"],
                //Description: localStorage["EmpLoyeeLogActivityListDescription"],
                CurrentStatusDelimited: localStorage["EmpLoyeeLogActivityListCurrentStatus"],
                CurrentTimestampFrom: localStorage["EmpLoyeeLogActivityListCurrentTimestampFrom"],
                CurrentTimestampTo: localStorage["EmpLoyeeLogActivityListCurrentTimestampTo"],
                DueDateFrom: localStorage["EmpLoyeeLogActivityListDueDateFrom"],
                DueDateTo: localStorage["EmpLoyeeLogActivityListDueDateTo"],
                AssignedByDelimited: localStorage["EmpLoyeeLogActivityListAssignedBy"],
                AssignedToDelimited: localStorage["EmpLoyeeLogActivityListAssignedTo"]
            };
            s.LoadLogActivityJQGrid(param);
            s.GetLocalStorage();

        },

        ElementBinding: function () {
            var s = this;

            $("#btnAddPreloaded").unbind("click");
            $("#btnAddPreloaded").click(function () {
                $("#divEmployeeLogActivityErrorMessage").html("");
                $("#divLogActivityAddPreloadedModal").modal("show");
            });

            $("#btnSavePreloaded").unbind("click");
            $("#btnSavePreloaded").click(function () {
                $(".errMessage").removeClass("errMessage");
                $("#divLogActivityAddPreloadedErrorMessage").html("");
                if ($("#ddlPreloaded").val() != "") {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeAddPreLoadedActivitiesURL + "
                        + ("'&EmployeeID=" + $("#divEmployeeModal #hdnID").val() + "&LogActivityPreloadedID=" + $("#ddlPreloaded").val() + "'") + " \
                        ,  { } \
                        , '#divLogActivityAddPreloadedErrorMessage' \
                        , '#divLogActivityAddPreloadedModal #btnAddPreloaded' \
                        , objLogActivityJS.AddPreloadedSuccessFunction); ",
                        "function");
                }
                else {
                    $("#ddlPreloaded").addClass("errMessage");
                    $("#ddlPreloaded").focus();
                    $("#divLogActivityAddPreloadedErrorMessage").append("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                }
            });

            $("#ddlPreloaded").unbind("change");
            $("#ddlPreloaded").change(function () {
                objLogActivityJS.LoadPreloadedLogActivityItemsJQGrid({
                    LogActivityPreloadedID: $(this).val()
                });
            });

            $("#btnAddActivity").unbind("click");
            $("#btnAddActivity").click(function () {
                $("#divEmployeeLogActivityErrorMessage").html("");
                LoadPartial(EmployeeAddLogActivityURL, "divLogActivityBodyModal");
                $("#divLogActivityModal").modal("show");
            });

            $("#btnBatchAssign").click(function () {

                $("#divEmployeeLogActivityErrorMessage").hide();
                $("#divEmployeeLogActivityErrorMessage").html("");

                if ($("#tblLogActivityList").jqGrid("getGridParam", "selarrrow").length == 0) {
                    $("#divEmployeeLogActivityErrorMessage").show();
                    $("#divEmployeeLogActivityErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "activity.</li></label><br />");
                }
                else {
                    $('#divAssignedUserModal').modal('show');
                    $("#divAssignedUserModal #txtBatchAssignedUser").val("");
                    $("#divAssignedUserModal #hdnBatchAssignedUserID").val("");
                    $("#divAssignedUserModal #dpDueDate").val("");
                    $("#divAssignedUserModal #txtBatchAssignedUser ").prop("disabled", false);
                    $("#divAssignedUserModal #cbIsAssignToSelf").removeAttr('checked');
                    $("#divAssignedUserModal #txtBatchAssignedUser").removeClass("errMessage");
                    $("#divAssignedUserModal #dpDueDate").removeClass("errMessage");

                    $("#divAssignedUserModal #dpDueDate").datetimepicker({
                        useCurrent: false,
                        format: 'MM/DD/YYYY'
                    });

                    $('#divAssignedUserModal #dpDueDate').datetimepicker().on('dp.show', function () {
                        $('#divAssignedUserModal .modal-body').css({ 'overflow': 'visible' });
                        $('#divAssignedUserModal.modal').css({ 'overflow': 'visible' });
                    }).on('dp.hide', function () {
                        $('#divAssignedUserModal .modal-body').css({ 'overflow': 'auto' });
                        $('#divAssignedUserModal.modal').css({ 'overflow': 'auto' });
                    })
                }
            });

            $("#divAssignedUserModal #cbIsAssignToSelf").on("click", function () {
                if ($(this).is(':checked')) {
                    $("#divAssignedUserModal #txtBatchAssignedUser ").prop("disabled", true);
                } else {
                    $("#divAssignedUserModal #txtBatchAssignedUser ").prop("disabled", false);
                }
                $("#txtBatchAssignedUser").val("");
                $("#hdnBatchAssignedUserID").val("");
            });

            $("#divAssignedUserModal #btnAssignedUserSave").click(function () {

                if ($("#txtBatchAssignedUser").val() != "" || $("#dpDueDate").val() != "" || $("#cbIsAssignToSelf").is(':checked')) {

                    $("#txtBatchAssignedUser").removeClass("errMessage");
                    $("#dpDueDate").removeClass("errMessage");

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeUpdateAssignedUserURL \
                        , objLogActivityJS.GetFormData() \
                        , '' \
                        , '#divAssignedUserModal #btnAssignedUserSave' \
                        , objLogActivityJS.AddSuccessFunction);", "function");
                }
                else {
                    $("#txtBatchAssignedUser").addClass("errMessage");
                    $("#dpDueDate").addClass("errMessage");
                    //$("#txtBatchAssignedUser").focus();
                    //$("#dpDueDate").focus();
                    //$("#divAssignedUserErrorMessage").append("<label class=\"errMessage\"><li>Assigned User is required</li></label><br />");
                }
            });

            $("#divAssignedUserModal .close").click(function () {
                $("#divAssignedUserModal #txtBatchAssignedUser").val("");
            });

            $("#btnUploadActivity").click(function () {
                objEMSCommonJS.UploadModal(UploadLogActivityURL + $("#divEmployeeModal #hdnID").val(), "Upload", DownloadLogActivityFormURL);
                $('#divModalErrorMessage').html('');
            });

            objEMSCommonJS.BindAutoComplete("divAssignedUserModal #txtBatchAssignedUser"
                , ReferredByAutoComplete
                , 20, "divAssignedUserModal #hdnBatchAssignedUserID", "ID", "Description");

            $("#tabLogActivity #txtFilterActivityCurrentTimestampFrom, #tabLogActivity #txtFilterActivityCurrentTimestampTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#tabLogActivity #txtFilterActivityDueDateFrom, #tabLogActivity #txtFilterActivityDueDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("tabLogActivity #multiSelectedActivityType", EmployeeLogActivityGetTypeURL, "Value", "Description");
            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("tabLogActivity #multiSelectedActivityCurrentStatus", EmployeeLogActivityGetCurrentStatusURL, "Value", "Description");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("tabLogActivity #txtFilterActivityAssignedBy"
                , ReferredByAutoComplete, 20, "multiSelectedActivityAssignedBy");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("tabLogActivity #txtFilterActivityAssignedTo"
                , ReferredByAutoComplete, 20, "multiSelectedActivityAssignedTo");

            $("#tabLogActivity #btnSearch").click(function () {
                var param = {
                    EmployeeID: $("#divEmployeeModal #hdnID").val(),
                    TypeDelimited: objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityType").value,
                    Title: $("#tabLogActivity #txtFilterActivityTitle").val(),
                    //Description: $("#tabLogActivity #txtFilterActivityDescription").val(),
                    CurrentStatusDelimited: objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityCurrentStatus").value,
                    CurrentTimestampFrom: $("#tabLogActivity #txtFilterActivityCurrentTimestampFrom").val(),
                    CurrentTimestampTo: $("#tabLogActivity #txtFilterActivityCurrentTimestampTo").val(),
                    DueDateFrom: $("#tabLogActivity #txtFilterActivityDueDateFrom").val(),
                    DueDateTo: $("#tabLogActivity #txtFilterActivityDueDateTo").val(),
                    AssignedByDelimited: objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityAssignedBy").value,
                    AssignedToDelimited: objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityAssignedTo").value
                };
                s.SetLocalStorage();
                ResetJQGridState("tblLogActivityList");
                s.LoadLogActivityJQGrid(param);
                $("#divEmployeeLogActivityErrorMessage").html("");
            });

            $("#tabLogActivity #btnReset").click(function () {
                $("#tabLogActivity div.filterFields input[type='search']").val("");
                $("#tabLogActivity div.filterFields select").val("");
                $("#tabLogActivity div.filterFields input[type='checkbox']").prop("checked", true);
                $("#tabLogActivity #multiSelectedActivityType").html("");
                $("#tabLogActivity #multiSelectedActivityTypeOption label, #tabLogActivity #multiSelectedActivityTypeOption input").prop("title", "add");
                $("#tabLogActivity #multiSelectedActivityCurrentStatus").html("");
                $("#tabLogActivity #multiSelectedActivityCurrentStatusOption label, #tabLogActivity #multiSelectedActivityCurrentStatusOption input").prop("title", "add");
                $("#tabLogActivity #multiSelectedActivityAssignedBy").html("");
                $("#tabLogActivity #multiSelectedActivityAssignedByOption label, #tabLogActivity #multiSelectedActivityAssignedByOption input").prop("title", "add");
                $("#tabLogActivity #multiSelectedActivityAssignedTo").html("");
                $("#tabLogActivity #multiSelectedActivityAssignedToOption label, #tabLogActivity #multiSelectedActivityAssignedToOption input").prop("title", "add");
                $("#divEmployeeLogActivityErrorMessage").html("");
                $("#tabLogActivity #btnSearch").click();
            });
        },

        GetFormData: function () {
            var formData = new FormData($('#frmEmployeeLogActivityBatchAssign').get(0));

            if ($("#divAssignedUserModal #hdnBatchAssignedUserID").val() != "") {
                formData.append("AssignedUser.AssignedUserID", $("#divAssignedUserModal #hdnBatchAssignedUserID").val());
            }

            if ($("#divAssignedUserModal #dpDueDate").val() != "") {
                formData.append("AssignedUser.DueDate", $("#divAssignedUserModal #dpDueDate").val());
            }

            $($("#tblLogActivityList").jqGrid("getGridParam", "selarrrow")).each(function (index, item) {
                formData.append("AssignedUser.LogActivityIDs[" + index + "]", item);
            });

            if ($("#divAssignedUserModal #cbIsAssignToSelf").is(':checked')) {
                formData.append("AssignedUser.IsAssignToSelf", true);
            }
            else {
                formData.append("AssignedUser.IsAssignToSelf", false);
            }

            return formData;
        },

        AddSuccessFunction: function () {
            $("#divAssignedUserModal").modal("hide");
            objLogActivityJS.LoadLogActivityJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
        },

        LoadLogActivityJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblLogActivityList") == "" ? "" : $.parseJSON(localStorage.getItem("tblLogActivityList"));

            var moveFilterFields = function () {
                var intialHeight = $("#tabLogActivity .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#tabLogActivity .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#tabLogActivity #filterFieldsContainer");
                });

                $("#tabLogActivity .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#tabLogActivity div.filterFields").unbind("keyup");
                $("#tabLogActivity div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#tabLogActivity #btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#tblLogActivityList").jqGrid("GridUnload");
            $("#tblLogActivityList").jqGrid("GridDestroy");
            $("#tblLogActivityList").jqGrid({
                url: EmployeeGetLogActivitiesURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Task Type", "Title", "Description", "Assigned By", "Assigned To", "Due Date", "Current Status", "Timestamp", ""
                ],
                colModel: [
                    { key: true, name: "ID", index: "ID", align: "center", sortable: false, formatter: objLogActivityJS.ViewLink },
                    { name: "Type", index: "Type", align: "left", sortable: false },
                    { name: "Title", index: "Title", align: "left", sortable: false },
                    { hidden: true, name: "Description", index: "Description", align: "left", sortable: false },
                    { name: "AssignedBy", index: "AssignedBy", align: "left", sortable: false },
                    { name: "AssignedTo", index: "AssignedTo", align: "left", sortable: false },
                    { name: "DueDate", index: "DueDate", align: "left", sortable: false },
                    { name: "CurrentStatus", index: "CurrentStatus", align: "left", sortable: false },
                    { name: "CurrentTimestamp", index: "CurrentTimestamp", align: "left", sortable: false },
                    { hidden: true, name: "IsAssignment", index: "IsAssignment" },
                ],
                toppager: $("#divEmployeeLogActivityPager"),
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
                    $("#tblLogActivityList tr:nth-child(" + (iRow + 1) + ") .activity-id-link").click();
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
                                if (data.rows[i].CurrentStatus == "DONE" || data.rows[i].CurrentStatus == "CANCELLED") {
                                    $("#divEmployeeModal #jqg_tblLogActivityList_" + data.rows[i].ID).attr("disabled", true);
                                }
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblLogActivityList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#tabLogActivity #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#tabLogActivity .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tabLogActivity #tblLogActivityList .jqgrid-id-link").click(function () {
                            $('#divLogActivityModal').modal('show');
                        });
                    }

                    if (objLogActivityJS.IsViewMode == true) {
                        $("#tblLogActivityList").jqGrid("hideCol", "cb");
                        $("#tblLogActivityList").jqGrid("hideCol", "ID");
                    }

                    if (localStorage["EmpLoyeeLogActivityListFilterOption"] != undefined) {
                        $("#tabLogActivity #chkEmpLogActivityFilter").prop('checked', JSON.parse(localStorage["EmpLoyeeLogActivityListFilterOption"]));
                    }
                    objLogActivityJS.ShowHideFilter();

                    $("#tabLogActivity #chkEmpLogActivityFilter").on('change', function () {
                        objLogActivityJS.ShowHideFilter();
                        localStorage["EmpLoyeeLogActivityListFilterOption"] = $("#chkEmpLogActivityFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#tabLogActivity .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#tabLogActivity table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#tabLogActivity table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#tabLogActivity table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#tabLogActivity table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#tabLogActivity table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#tabLogActivity table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#tabLogActivity table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabLogActivity .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                    });
                },
                onSelectAll: function (aRowids, status) {
                    if (status)
                    {
                        // uncheck "protected" rows
                        var cbs = $("tr.jqgrow > td > input.cbox:disabled", $(this));
                        cbs.removeAttr("checked");

                        //modify the selarrrow parameter 
                        $(this)[0].p.selarrrow = $(this).find("tr.jqgrow:has(td > input.cbox:checked)")
                            .map(function () { return this.id; }) // convert to set of ids 
                            .get(); // convert to instance of Array 
                    }
                },
                beforeSelectRow: function (id, e) {
                    var cbsdis = $("tr#" + id + ".jqgrow > td > input.cbox:disabled", $(this));
                        if (cbsdis.length == 0) {
                            return true; // allow select the row 
                        }
                        else {
                            return false; // not allow select the row 
                        } 
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblLogActivityList");
                    moveFilterFields();
                },
            }).navGrid("#divEmployeeLogActivityPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery("#tabLogActivity .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery("#tabLogActivity .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#tabLogActivity #lblFilter").after("<input type=\"checkbox\" id=\"chkEmpLogActivityFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");

        },

        SetLocalStorage: function () {
            localStorage["EmpLoyeeLogActivityListType"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityType").value;
            localStorage["EmpLoyeeLogActivityListTypeText"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityType").text;
            localStorage["EmpLoyeeLogActivityListSubType"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedSubType").value;
            localStorage["EmpLoyeeLogActivityListSubTypeText"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedSubType").text;

            localStorage["EmpLoyeeLogActivityListTitle"] = $("#tabLogActivity #txtFilterActivityTitle").val();
            //localStorage["EmpLoyeeLogActivityListDescription"] = $("#tabLogActivity #txtFilterActivityDescription").val();

            localStorage["EmpLoyeeLogActivityListCurrentStatus"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityCurrentStatus").value;
            localStorage["EmpLoyeeLogActivityListCurrentStatusText"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityCurrentStatus").text;

            localStorage["EmpLoyeeLogActivityListCurrentTimestampFrom"] = $("#tabLogActivity #txtFilterActivityCurrentTimestampFrom").val();
            localStorage["EmpLoyeeLogActivityListCurrentTimestampTo"] = $("#tabLogActivity #txtFilterActivityCurrentTimestampTo").val();

            localStorage["EmpLoyeeLogActivityListDueDateFrom"] = $("#tabLogActivity #txtFilterActivityDueDateFrom").val();
            localStorage["EmpLoyeeLogActivityListDueDateTo"] = $("#tabLogActivity #txtFilterActivityDueDateTo").val();

            localStorage["EmpLoyeeLogActivityListAssignedBy"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityAssignedBy").value;
            localStorage["EmpLoyeeLogActivityListAssignedByText"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityAssignedBy").text;

            localStorage["EmpLoyeeLogActivityListAssignedTo"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityAssignedTo").value;
            localStorage["EmpLoyeeLogActivityListAssignedToText"] = objEMSCommonJS.GetMultiSelectList("tabLogActivity #multiSelectedActivityAssignedTo").text;

            //localStorage["EmpLoyeeLogActivityListRemarks"] = $("#tabLogActivity #txtFilterRemarks").val();
        },

        GetLocalStorage: function () {

            objEMSCommonJS.SetMultiSelectList("tabLogActivity #multiSelectedActivityType"
                , "EmpLoyeeLogActivityListType"
                , "EmpLoyeeLogActivityListTypeText");
            objEMSCommonJS.SetMultiSelectList("#tabLogActivity #multiSelectedSubType"
                , "EmpLoyeeLogActivityListSubType"
                , "EmpLoyeeLogActivityListSubTypeText");

            $("#tabLogActivity #txtFilterActivityTitle").val(localStorage["EmpLoyeeLogActivityListTitle"]);
            //$("#tabLogActivity #txtFilterActivityDescription").val(localStorage["EmpLoyeeLogActivityListDescription"]);

            objEMSCommonJS.SetMultiSelectList("tabLogActivity #multiSelectedActivityCurrentStatus"
                , "EmpLoyeeLogActivityListCurrentStatus"
                , "EmpLoyeeLogActivityListCurrentStatusText");

            objEMSCommonJS.SetMultiSelectList("tabLogActivity #multiSelectedActivityAssignedBy"
                , "EmpLoyeeLogActivityListAssignedBy"
                , "EmpLoyeeLogActivityListAssignedByText");

            $("#tabLogActivity #txtFilterActivityCurrentTimestampFrom").val(localStorage["EmpLoyeeLogActivityListCurrentTimestampFrom"]);
            $("#tabLogActivity #txtFilterActivityCurrentTimestampTo").val(localStorage["EmpLoyeeLogActivityListCurrentTimestampTo"]);

            $("#tabLogActivity #txtFilterActivityDueDateFrom").val(localStorage["EmpLoyeeLogActivityListDueDateFrom"]);
            $("#tabLogActivity #txtFilterActivityDueDateTo").val(localStorage["EmpLoyeeLogActivityListDueDateTo"]);

            objEMSCommonJS.SetMultiSelectList("tabLogActivity #multiSelectedActivityAssignedTo"
                , "EmpLoyeeLogActivityListAssignedTo"
                , "EmpLoyeeLogActivityListAssignedToText");

            //$("#tabLogActivity #txtFilterRemarks").val(localStorage["EmpLoyeeLogActivityListRemarks"]);
        },

        ShowHideFilter: function () {
            if ($("#tabLogActivity #chkEmpLogActivityFilter").is(":checked")) {
                $("#tabLogActivity .jqgfirstrow .filterFields").show();
            }
            else if ($("#chkEmpLogActivityFilter").is(":not(:checked)")) {
                $("#tabLogActivity .jqgfirstrow .filterFields").hide();
            }
        },

        LoadPreloadedLogActivityItemsJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblPreloadedLogActivityItemsList").jqGrid("GridUnload");
            $("#tblPreloadedLogActivityItemsList").jqGrid("GridDestroy");
            $("#tblPreloadedLogActivityItemsList").jqGrid({
                url: EmployeeLogActivityByPreloadedIDURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Task Type",/* "Sub-Type",*/ "Title", "Description"],
                colModel: [
                    { name: "", hidden: true },
                    { name: "Type", index: "Type", align: "left", sortable: false },
                    //{ name: "SubType", index: "SubType", align: "center", sortable: false },
                    { name: "Title", index: "Title", align: "left", sortable: false },
                    { name: "Description", index: "Description", align: "left", sortable: false },
                ],
                toppager: $("#divEmployeeLogActivityPager"), 
                rowList: SetRowList(),
                loadonce: true,
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
                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblPreloadedLogActivityItemsList", data);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return objLogActivityJS.ViewUpdateStatusModal('" + EmployeeUpdateLogActivityURL + "?EmployeeLogActivityID=" + rowObject.ID + "');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID)+"</a>";
        },

        AddPreloadedSuccessFunction: function () {
            $("#divLogActivityAddPreloadedModal").modal("hide");
            objLogActivityJS.LoadLogActivityJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
        },

        ViewUpdateStatusModal: function (url) {
            $('#divLogActivityModal').modal('show');
            LoadPartial(url, 'divLogActivityBodyModal');
            return false;
        }


    };

    objLogActivityJS.Initialize();
});