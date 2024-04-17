var objSkillsJS;

$(document).ready(function () {
    objSkillsJS = {

        IsViewMode: false,

        Initialize: function () {
            var s = this;

            //GenerateDropdownValues(EmployeeSkillsPreloadedDropDownURL, "ddlPreloaded", "Value", "Text", "", "", false);
            $("#tblPreloadedSkillsItemsList").jqGrid("GridUnload");
            $("#tblPreloadedSkillsItemsList").jqGrid("GridDestroy");
            $("#tblSkillsList").jqGrid("GridUnload");
            $("#tblSkillsList").jqGrid("GridDestroy");

            s.ElementBinding();
            var param = {
                EmployeeID: $("#divEmployeeModal #hdnID").val(),
                TypeDelimited: localStorage["EmpLoyeeSkillsListType"],
                Title: localStorage["EmpLoyeeSkillsListTitle"],
                //Description: localStorage["EmpLoyeeSkillsListDescription"],
                CurrentStatusDelimited: localStorage["EmpLoyeeSkillsListCurrentStatus"],
                CurrentTimestampFrom: localStorage["EmpLoyeeSkillsListCurrentTimestampFrom"],
                CurrentTimestampTo: localStorage["EmpLoyeeSkillsListCurrentTimestampTo"],
                DueDateFrom: localStorage["EmpLoyeeSkillsListDueDateFrom"],
                DueDateTo: localStorage["EmpLoyeeSkillsListDueDateTo"],
                AssignedByDelimited: localStorage["EmpLoyeeSkillsListAssignedBy"],
                AssignedToDelimited: localStorage["EmpLoyeeSkillsListAssignedTo"]
            };
            s.LoadSkillsJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            //$("#btnAddPreloaded").unbind("click");
            //$("#btnAddPreloaded").click(function () {
            //    $("#divEmployeeSkillsErrorMessage").html("");
            //    $("#divSkillsAddPreloadedModal").modal("show");
            //});

            //$("#btnSavePreloaded").unbind("click");
            //$("#btnSavePreloaded").click(function () {
            //    $(".errMessage").removeClass("errMessage");
            //    $("#divSkillsAddPreloadedErrorMessage").html("");
            //    if ($("#ddlPreloaded").val() != "") {
            //        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
            //            "objEMSCommonJS.PostAjax(true \
            //            , EmployeeAddPreLoadedActivitiesURL + "
            //            + ("'&EmployeeID=" + $("#divEmployeeModal #hdnID").val() + "&SkillsPreloadedID=" + $("#ddlPreloaded").val() + "'") + " \
            //            ,  { } \
            //            , '#divSkillsAddPreloadedErrorMessage' \
            //            , '#divSkillsAddPreloadedModal #btnAddPreloaded' \
            //            , objSkillsJS.AddPreloadedSuccessFunction); ",
            //            "function");
            //    }
            //    else {
            //        $("#ddlPreloaded").addClass("errMessage");
            //        $("#ddlPreloaded").focus();
            //        $("#divSkillsAddPreloadedErrorMessage").append("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
            //    }
            //});

            //$("#ddlPreloaded").unbind("change");
            //$("#ddlPreloaded").change(function () {
            //    objSkillsJS.LoadPreloadedSkillsItemsJQGrid({
            //        SkillsPreloadedID: $(this).val()
            //    });
            //});

            $("#btnAddSkills").unbind("click");
            $("#btnAddSkills").click(function () {
                LoadPartial(EmployeeAddSkillsURL, "divSkillsBodyModal");
                $("#divSkillsModal").modal("show");
            });

            $("#btnBatchAssign").click(function () {

                $("#divEmployeeSkillsErrorMessage").hide();
                $("#divEmployeeSkillsErrorMessage").html("");

                if ($("#tblSkillsList").jqGrid("getGridParam", "selarrrow").length == 0) {
                    $("#divEmployeeSkillsErrorMessage").show();
                    $("#divEmployeeSkillsErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "activity.</li></label><br />");
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
                        , objSkillsJS.GetFormData() \
                        , '' \
                        , '#divAssignedUserModal #btnAssignedUserSave' \
                        , objSkillsJS.AddSuccessFunction);", "function");
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

            objEMSCommonJS.BindAutoComplete("divAssignedUserModal #txtBatchAssignedUser"
                , ReferredByAutoComplete
                , 20, "divAssignedUserModal #hdnBatchAssignedUserID", "ID", "Description");

            $("#tabSkills #txtFilterActivityCurrentTimestampFrom, #tabSkills #txtFilterActivityCurrentTimestampTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#tabSkills #txtFilterActivityDueDateFrom, #tabSkills #txtFilterActivityDueDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("tabSkills #multiSelectedActivityType", EmployeeLogActivityGetTypeURL, "Value", "Description");
            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("tabSkills #multiSelectedActivityCurrentStatus", EmployeeLogActivityGetCurrentStatusURL, "Value", "Description");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("tabSkills #txtFilterActivityAssignedBy"
                , ReferredByAutoComplete, 20, "multiSelectedActivityAssignedBy");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("tabSkills #txtFilterActivityAssignedTo"
                , ReferredByAutoComplete, 20, "multiSelectedActivityAssignedTo");

            $("#tabSkills #btnSearch").click(function () {
                var param = {
                    EmployeeID: $("#divEmployeeModal #hdnID").val(),
                    TypeDelimited: objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityType").value,
                    Title: $("#tabSkills #txtFilterActivityTitle").val(),
                    //Description: $("#tabSkills #txtFilterActivityDescription").val(),
                    CurrentStatusDelimited: objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityCurrentStatus").value,
                    CurrentTimestampFrom: $("#tabSkills #txtFilterActivityCurrentTimestampFrom").val(),
                    CurrentTimestampTo: $("#tabSkills #txtFilterActivityCurrentTimestampTo").val(),
                    DueDateFrom: $("#tabSkills #txtFilterActivityDueDateFrom").val(),
                    DueDateTo: $("#tabSkills #txtFilterActivityDueDateTo").val(),
                    AssignedByDelimited: objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityAssignedBy").value,
                    AssignedToDelimited: objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityAssignedTo").value
                };
                s.SetLocalStorage();
                ResetJQGridState("tblSkillsList");
                s.LoadSkillsJQGrid(param);
                $("#divEmployeeSkillsErrorMessage").html("");
            });

            $("#tabSkills #btnReset").click(function () {
                $("#tabSkills div.filterFields input[type='search']").val("");
                $("#tabSkills div.filterFields select").val("");
                $("#tabSkills div.filterFields input[type='checkbox']").prop("checked", true);
                $("#tabSkills #multiSelectedActivityType").html("");
                $("#tabSkills #multiSelectedActivityTypeOption label, #tabSkills #multiSelectedActivityTypeOption input").prop("title", "add");
                $("#tabSkills #multiSelectedActivityCurrentStatus").html("");
                $("#tabSkills #multiSelectedActivityCurrentStatusOption label, #tabSkills #multiSelectedActivityCurrentStatusOption input").prop("title", "add");
                $("#tabSkills #multiSelectedActivityAssignedBy").html("");
                $("#tabSkills #multiSelectedActivityAssignedByOption label, #tabSkills #multiSelectedActivityAssignedByOption input").prop("title", "add");
                $("#tabSkills #multiSelectedActivityAssignedTo").html("");
                $("#tabSkills #multiSelectedActivityAssignedToOption label, #tabSkills #multiSelectedActivityAssignedToOption input").prop("title", "add");
                $("#divEmployeeSkillsErrorMessage").html("");
                $("#tabSkills #btnSearch").click();
            });
        },

        GetFormData: function () {
            var formData = new FormData($('#frmEmployeeSkillsBatchAssign').get(0));

            if ($("#divAssignedUserModal #hdnBatchAssignedUserID").val() != "") {
                formData.append("AssignedUser.AssignedUserID", $("#divAssignedUserModal #hdnBatchAssignedUserID").val());
            }

            if ($("#divAssignedUserModal #dpDueDate").val() != "") {
                formData.append("AssignedUser.DueDate", $("#divAssignedUserModal #dpDueDate").val());
            }

            $($("#tblSkillsList").jqGrid("getGridParam", "selarrrow")).each(function (index, item) {
                formData.append("AssignedUser.SkillsIDs[" + index + "]", item);
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
            objSkillsJS.LoadSkillsJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
        },

        LoadSkillsJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblSkillsList") == "" ? "" : $.parseJSON(localStorage.getItem("tblSkillsList"));

            var moveFilterFields = function () {
                var intialHeight = $("#tabSkills .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#tabSkills .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#tabSkills #filterFieldsContainer");
                });

                $("#tabSkills .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#tabSkills div.filterFields").unbind("keyup");
                $("#tabSkills div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#tabSkills #btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#tblSkillsList").jqGrid("GridUnload");
            $("#tblSkillsList").jqGrid("GridDestroy");
            $("#tblSkillsList").jqGrid({
                url: EmployeeGetSkillsListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Skills", "Rate", "Remarks", "Created By","Created Date", "Modified By", "Modified Date",""
                ],
                colModel: [
                    { key: true, name: "ID", index: "ID", align: "center", sortable: false, formatter: objSkillsJS.ViewLink },
                    { name: "SkillsDescription", index: "SkillsDescription", align: "left", sortable: false },
                    { name: "Rate", index: "Rate", align: "center", sortable: false, formatter: objSkillsJS.AppendPercent },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false },
                    { name: "CreatedBy", index: "CreatedBy", align: "left", sortable: false },
                    { name: "CreatedDate", index: "CreatedDate", align: "left", sortable: false },
                    { name: "ModifiedBy", index: "ModifiedBy", align: "left", sortable: false },
                    { name: "ModifiedDate", index: "ModifiedDate", align: "left", sortable: false },
                    { hidden: true, name: "IsActive", index: "IsActive" },
                ],
                //toppager: $("#divEmployeeSkillsPager"),
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
                    $("#tblSkillsList tr:nth-child(" + (iRow + 1) + ") .activity-id-link").click();
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
                                    $("#divEmployeeModal #jqg_tblSkillsList_" + data.rows[i].ID).attr("disabled", true);
                                }
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblSkillsList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#tabSkills #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#tabSkills .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tabSkills #tblSkillsList .jqgrid-id-link").click(function () {
                            $('#divSkillsModal').modal('show');
                        });
                    }

                    if (objSkillsJS.IsViewMode == true) {
                        $("#tblSkillsList").jqGrid("hideCol", "cb");
                        $("#tblSkillsList").jqGrid("hideCol", "ID");
                    }

                    if (localStorage["EmpLoyeeSkillsListFilterOption"] != undefined) {
                        $("#tabSkills #chkEmpSkillsFilter").prop('checked', JSON.parse(localStorage["EmpLoyeeSkillsListFilterOption"]));
                    }
                    objSkillsJS.ShowHideFilter();

                    $("#tabSkills #chkEmpSkillsFilter").on('change', function () {
                        objSkillsJS.ShowHideFilter();
                        localStorage["EmpLoyeeSkillsListFilterOption"] = $("#chkEmpSkillsFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#tabSkills .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("#tabSkills table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("#tabSkills table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("#tabSkills table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#tabSkills table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("#tabSkills table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("#tabSkills table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("#tabSkills table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $("#tabSkills .ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                    });
                },
                onSelectAll: function (aRowids, status) {
                    if (status) {
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
                    GetJQGridState("tblSkillsList");
                    moveFilterFields();
                },
            }).navGrid("#divEmployeeSkillsPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            //jQuery("#tabSkills .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            //jQuery("#tabSkills .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            //jQuery("#tabSkills #lblFilter").after("<input type=\"checkbox\" id=\"chkEmpSkillsFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");

        },

        SetLocalStorage: function () {
            localStorage["EmpLoyeeSkillsListType"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityType").value;
            localStorage["EmpLoyeeSkillsListTypeText"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityType").text;
            localStorage["EmpLoyeeSkillsListSubType"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedSubType").value;
            localStorage["EmpLoyeeSkillsListSubTypeText"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedSubType").text;

            localStorage["EmpLoyeeSkillsListTitle"] = $("#tabSkills #txtFilterActivityTitle").val();
            //localStorage["EmpLoyeeSkillsListDescription"] = $("#tabSkills #txtFilterActivityDescription").val();

            localStorage["EmpLoyeeSkillsListCurrentStatus"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityCurrentStatus").value;
            localStorage["EmpLoyeeSkillsListCurrentStatusText"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityCurrentStatus").text;

            localStorage["EmpLoyeeSkillsListCurrentTimestampFrom"] = $("#tabSkills #txtFilterActivityCurrentTimestampFrom").val();
            localStorage["EmpLoyeeSkillsListCurrentTimestampTo"] = $("#tabSkills #txtFilterActivityCurrentTimestampTo").val();

            localStorage["EmpLoyeeSkillsListDueDateFrom"] = $("#tabSkills #txtFilterActivityDueDateFrom").val();
            localStorage["EmpLoyeeSkillsListDueDateTo"] = $("#tabSkills #txtFilterActivityDueDateTo").val();

            localStorage["EmpLoyeeSkillsListAssignedBy"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityAssignedBy").value;
            localStorage["EmpLoyeeSkillsListAssignedByText"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityAssignedBy").text;

            localStorage["EmpLoyeeSkillsListAssignedTo"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityAssignedTo").value;
            localStorage["EmpLoyeeSkillsListAssignedToText"] = objEMSCommonJS.GetMultiSelectList("tabSkills #multiSelectedActivityAssignedTo").text;

            //localStorage["EmpLoyeeSkillsListRemarks"] = $("#tabSkills #txtFilterRemarks").val();
        },

        GetLocalStorage: function () {

            objEMSCommonJS.SetMultiSelectList("tabSkills #multiSelectedActivityType"
                , "EmpLoyeeSkillsListType"
                , "EmpLoyeeSkillsListTypeText");
            objEMSCommonJS.SetMultiSelectList("#tabSkills #multiSelectedSubType"
                , "EmpLoyeeSkillsListSubType"
                , "EmpLoyeeSkillsListSubTypeText");

            $("#tabSkills #txtFilterActivityTitle").val(localStorage["EmpLoyeeSkillsListTitle"]);
            //$("#tabSkills #txtFilterActivityDescription").val(localStorage["EmpLoyeeSkillsListDescription"]);

            objEMSCommonJS.SetMultiSelectList("tabSkills #multiSelectedActivityCurrentStatus"
                , "EmpLoyeeSkillsListCurrentStatus"
                , "EmpLoyeeSkillsListCurrentStatusText");

            objEMSCommonJS.SetMultiSelectList("tabSkills #multiSelectedActivityAssignedBy"
                , "EmpLoyeeSkillsListAssignedBy"
                , "EmpLoyeeSkillsListAssignedByText");

            $("#tabSkills #txtFilterActivityCurrentTimestampFrom").val(localStorage["EmpLoyeeSkillsListCurrentTimestampFrom"]);
            $("#tabSkills #txtFilterActivityCurrentTimestampTo").val(localStorage["EmpLoyeeSkillsListCurrentTimestampTo"]);

            $("#tabSkills #txtFilterActivityDueDateFrom").val(localStorage["EmpLoyeeSkillsListDueDateFrom"]);
            $("#tabSkills #txtFilterActivityDueDateTo").val(localStorage["EmpLoyeeSkillsListDueDateTo"]);

            objEMSCommonJS.SetMultiSelectList("tabSkills #multiSelectedActivityAssignedTo"
                , "EmpLoyeeSkillsListAssignedTo"
                , "EmpLoyeeSkillsListAssignedToText");

            //$("#tabSkills #txtFilterRemarks").val(localStorage["EmpLoyeeSkillsListRemarks"]);
        },

        ShowHideFilter: function () {
            if ($("#tabSkills #chkEmpSkillsFilter").is(":checked")) {
                $("#tabSkills .jqgfirstrow .filterFields").show();
            }
            else if ($("#chkEmpSkillsFilter").is(":not(:checked)")) {
                $("#tabSkills .jqgfirstrow .filterFields").hide();
            }
        },

        LoadPreloadedSkillsItemsJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblPreloadedSkillsItemsList").jqGrid("GridUnload");
            $("#tblPreloadedSkillsItemsList").jqGrid("GridDestroy");
            $("#tblPreloadedSkillsItemsList").jqGrid({
                url: EmployeeSkillsByPreloadedIDURL,
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
                toppager: $("#divEmployeeSkillsPager"),
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
                        //AutoSizeColumnJQGrid("tblPreloadedSkillsItemsList", data);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return objSkillsJS.ViewUpdateStatusModal('" + EmployeeUpdateSkillsURL + "?Id=" + rowObject.ID + "');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        AppendPercent: function (cellvalue, options, rowObject) {
            return cellvalue+"%";
        },

        ViewUpdateStatusModal: function (url) {
            $('#divUpdateSkillsModal').modal('show');
            LoadPartial(url, 'divUpdateSkillsBodyModal');
            return false;
        }


    };

    objSkillsJS.Initialize();
});