var objSystemUserListJS;
const SystemUserListURL = "/Administrator/SystemUser?handler=List";
const SystemUserAddURL = "/Administrator/SystemUser/Add";
const SystemUserViewURL = "/Administrator/SystemUser/View";
const SystemUserEditURL = "/Administrator/SystemUser/Edit";
const SystemUserDeleteURL = "/Administrator/SystemUser/Delete";
const SystemUserAddPostURL = "/Administrator/SystemUser/Add";
const SystemUserEditPostURL = "/Administrator/SystemUser/Edit";
const SystemUserBatchResetPasswordPostURL = "/Administrator/SystemUser?handler=BatchResetPassword";
const SystemUserChangeStatusPostURL = "/Administrator/SystemUser?handler=ChangeStatus";
const SystemUserResetPasswordPostURL = "/Administrator/SystemUser?handler=ResetPassword";

const GetSystemRoleDropDownURL = "/Administrator/SystemUser?handler=SystemRoleDropDown";
const GetSystemUserRoleDropDownURL = "/Administrator/SystemUser?handler=SystemUserRoleDropDownByUserID";
const GetCheckExportListURL = "/Administrator/SystemUser?handler=CheckExportList";
const DownloadExportListURL = "/Administrator/SystemUser?handler=DownloadExportList";


$(document).ready(function () {
    objSystemUserListJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                ID: localStorage["AdministratorSystemUserListID"],
                UserName: localStorage["AdministratorSystemUserListUserName"],
                Name: localStorage["AdministratorSystemUserListName"],
                Status: localStorage["AdministratorSystemUserListStatus"],
                DateModifiedFrom: localStorage["AdministratorSystemUserListDateModifiedFrom"],
                DateModifiedTo: localStorage["AdministratorSystemUserListDateModifiedTo"],
                DateCreatedFrom: localStorage["AdministratorSystemUserListDateCreatedFrom"],
                DateCreatedTo: localStorage["AdministratorSystemUserListDateCreatedTo"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterDateCreatedFrom, #txtFilterDateCreatedTo, \
            #txtFilterDateModifiedFrom, #txtFilterDateModifiedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            NumberOnly($("#txtFilterSystemUserID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterSystemUserID").val(),
                    UserName: $("#txtFilterSysUserName").val(),
                    Name: $("#txtFilterSysName").val(),
                    Status: $("#txtFilterStatus").val(),
                    DateModifiedFrom: $("#txtFilterDateModifiedFrom").val(),
                    DateModifiedTo: $("#txtFilterDateModifiedTo").val(),
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAdministratorSystemUserList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(SystemUserAddURL, "divSystemUserBodyModal");
                $("#divSystemUserModal").modal("show");
            });

            $("#btnBatchResetPassword").click(function () {
                $("#divSystemUserErrorMessageList").hide();
                $("#divSystemUserErrorMessageList").html("");

                if ($("#tblAdministratorSystemUserList").jqGrid("getGridParam", "selarrrow").length == 0) {
                    $("#divSystemUserErrorMessageList").show();
                    $("#divSystemUserErrorMessageList").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "system user.</li></label><br />");
                }
                else if ($("#tblAdministratorSystemUserList").jqGrid("getGridParam", "selarrrow").length > 1) {
                    $("#divSystemUserErrorMessageList").show();
                    $("#divSystemUserErrorMessageList").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONLY_ONE + "system user.</li></label><br />");
                }
                else {
                    $("#divPasswordErrorMessage").hide();
                    $("#divPasswordErrorMessage").html("");
                    $("#txtPassword").removeClass("errMessage");

                    $('#divPasswordModal').modal('show');
                    /*ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                        "objEMSCommonJS.PostAjax(true \
                        , SystemUserBatchResetPasswordPostURL \
                        , objSystemUserListJS.GetFormData1() \
                        ,'#divSystemUserErrorMessage' \
                        , '#btnBatchResetPassword' \
                         );",
                        "function");*/
                }
            });

            $("#btnSavePassword").click(function () {
                $("#divPasswordErrorMessage").hide();
                $("#divPasswordErrorMessage").html("");
                if ($("#txtPassword").val() == "") {
                    $("#txtPassword").addClass("errMessage");
                    $("#divPasswordErrorMessage").show();
                    $("#divPasswordErrorMessage").html("<label class=\"errMessage\"><li>New Password " + SUFF_REQUIRED + "</li></label><br />");
                }
                else {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                        "objEMSCommonJS.PostAjax(true \
                        , SystemUserResetPasswordPostURL \
                        , objSystemUserListJS.GetFormData1() \
                        ,'#divSystemUserErrorMessage' \
                        , '#btnSavePassword' \
                        , objSystemUserListJS.ResetPasswordSuccessFunction);",
                        "function");
                }
            });

            $("#btnChangeStatus").click(function () {
                $("#divSystemUserErrorMessageList").hide();
                $("#divSystemUserErrorMessageList").html("");

                if ($("#tblAdministratorSystemUserList").jqGrid("getGridParam", "selarrrow").length == 0) {
                    $("#divSystemUserErrorMessageList").show();
                    $("#divSystemUserErrorMessageList").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + "system user.</li></label><br />");
                }
                else {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_PROCEED,
                        "objEMSCommonJS.PostAjax(true \
                        , SystemUserChangeStatusPostURL \
                        , objSystemUserListJS.GetFormData2() \
                        ,'#divSystemUserErrorMessageList' \
                        , '#btnChangeStatus' \
                        , objSystemUserListJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnExport").click(function () {

                var parameters = "&sidx=" + $("#tblAdministratorSystemUserList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblAdministratorSystemUserList").jqGrid("getGridParam", "sortorder")
                    + "&ID=" + $("#txtFilterSystemUserID").val()
                    + "&UserName=" + $("#txtFilterSysUserName").val()
                    + "&Name=" + $("#txtFilterSysName").val()
                    + "&Status=" + $("#txtFilterStatus").val()
                    + "&DateModifiedFrom=" + $("#txtFilterDateModifiedFrom").val()
                    + "&DateModifiedTo=" + $("#txtFilterDateModifiedTo").val()
                    + "&DateCreatedFrom=" + $("#txtFilterDateCreatedFrom").val()
                    + "&DateCreatedTo=" + $("#txtFilterDateCreatedTo").val()

                var GetSuccessFunction = function (data) {
                    if (data.IsSuccess == true) {
                        window.location = DownloadExportListURL + parameters;
                    }
                    else {
                        ModalAlert(MODAL_HEADER, data.Result);
                    }
                };

                objEMSCommonJS.GetAjax(GetCheckExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
            });
        },

        GetFormData1: function () {
            var formData = new FormData($('#frmResetPassword').get(0));

            $($("#tblAdministratorSystemUserList").jqGrid("getGridParam", "selarrrow")).each(function (index, item) {
                formData.append("ResetPassword.ID", item);
            });

            formData.append("ResetPassword.ForceChangePassword", $("#cbForce").is(":checked"));

            return formData;

        },

        GetFormData2: function () {
            var formData = new FormData($('#frmSystemUser').get(0));

            $($("#tblAdministratorSystemUserList").jqGrid("getGridParam", "selarrrow")).each(function (index, item) {
                formData.append("ChangeStatusForm.SystemUserIDs[" + index + "]", item);
            });

            return formData;

        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
        },

        ResetPasswordSuccessFunction: function () {
            $("#txtPassword").removeClass("errMessage");
            $("#txtPassword").val("");
            $('#divPasswordModal').modal('hide');
            $("#btnSearch").click();
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblAdministratorSystemUserList") == "" ? "" : $.parseJSON(localStorage.getItem("tblAdministratorSystemUserList"));
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
            $("#tblAdministratorSystemUserList").jqGrid("GridUnload");
            $("#tblAdministratorSystemUserList").jqGrid("GridDestroy");
            $("#tblAdministratorSystemUserList").jqGrid({
                url: SystemUserListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "User Name", "Name", "Status", "Modified Date", "Created Date"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objSystemUserListJS.AddLink },
                    { name: "Username", index: "Username", editable: true, align: "left" },
                    { name: "Name", index: "Name", editable: true, align: "left" },
                    { name: "Status", index: "Status", editable: true, align: "center" },
                    { name: "DateModified", index: "DateModified", editable: true, align: "left" },
                    { name: "DateCreated", index: "DateCreated", editable: true, align: "left" },
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
                multiselect: true,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblAdministratorSystemUserList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                                
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblAdministratorSystemUserList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblAdministratorSystemUserList .jqgrid-id-link").click(function () {
                            $('#divSystemUserModal').modal('show');
                        });

                    }

                    if (localStorage["SystemUserListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["SystemUserListFilterOption"]));
                    }
                    objSystemUserListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objSystemUserListJS.ShowHideFilter();
                        localStorage["SystemUserListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblAdministratorSystemUserList");
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
            localStorage["AdministratorSystemUserListID"] = $("#txtFilterSystemUserID").val();
            localStorage["AdministratorSystemUserListUserName"] = $("#txtFilterSysUserName").val();
            localStorage["AdministratorSystemUserListName"] = $("#txtFilterSysName").val();
            localStorage["AdministratorSystemUserListStatus"] = $("#txtFilterStatus").val();
            localStorage["AdministratorSystemUserListDateModifiedFrom"] = $("#txtFilterDateModifiedFrom").val();
            localStorage["AdministratorSystemUserListDateModifiedTo"] = $("#txtFilterDateModifiedTo").val();
            localStorage["AdministratorSystemUserListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["AdministratorSystemUserListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterSystemUserID").val(localStorage["AdministratorSystemUserListID"]);
            $("#txtFilterSysUserName").val(localStorage["AdministratorSystemUserListUserName"]);
            $("#txtFilterSysName").val(localStorage["AdministratorSystemUserListName"]);
            $("#txtFilterStatus").val(localStorage["AdministratorSystemUserListStatus"]);
            $("#txtFilterDateModifiedFrom").val(localStorage["AdministratorSystemUserListDateModifiedFrom"]);
            $("#txtFilterDateModifiedTo").val(localStorage["AdministratorSystemUserListDateModifiedTo"]);
            $("#txtFilterDateCreatedFrom").val(localStorage["AdministratorSystemUserListDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["AdministratorSystemUserListDateCreatedTo"]);
        },
        
        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + SystemUserViewURL + "?ID=" + rowObject.ID + "', 'divSystemUserBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID)+"</a>"; 
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
    
     objSystemUserListJS.Initialize();
});