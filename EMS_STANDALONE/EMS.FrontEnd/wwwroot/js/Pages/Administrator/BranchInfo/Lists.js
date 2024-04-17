var objBranchInfoListJS;

const BranchInfoListURL = "/Administrator/BranchInfo?handler=List";
const BranchInfoViewURL = "/Administrator/BranchInfo/View";
const BranchInfoEditURL = "/Administrator/BranchInfo/Edit";
const GetOrgTypeURL = "/Administrator/BranchInfo?handler=ReferenceValue&RefCode=ORGGROUPTYPE";
const DownloadOrgGroupExportListURL = "/Administrator/BranchInfo?handler=DownloadOrgGroupExportList";
const GetCheckOrgGroupExportListURL = "/Administrator/BranchInfo?handler=CheckOrgGroupExportList";
const OrgGroupAutoCompleteURL = "/Administrator/BranchInfo?handler=OrgGroupAutoComplete";

const OrgTypeHierarchyUpwardURL = "/Administrator/BranchInfo?handler=OrgGroupHierarchy";

var PSGCAutoCompleteURL = "/Administrator/BranchInfo?handler=PSGCAutoComplete";

var enumData = [];

$(document).ready(function () {
    objBranchInfoListJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                ID: localStorage["BranchInfoListID"],
                Code: localStorage["BranchInfoListCode"],
                Description: localStorage["BranchInfoListDescription"],
                Address: localStorage["BranchInfoListAddress"],
                OrgTypeDelimited: 2,
                ParentOrgDescription: localStorage["BranchInfoListParentOrg"],
                ServiceBayCountMin: localStorage["BranchInfoListServiceBayCountMin"],
                ServiceBayCountMax: localStorage["BranchInfoListServiceBayCountMax"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterOrgGroupID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterOrgGroupID").val(),
                    Code: $("#txtFilterCode").val(),
                    Description: $("#txtFilterDescription").val(),
                    Category: $("#txtFilterCategory").val(),
                    Email: $("#txtFilterEmail").val(),
                    Number: $("#txtFilterNumber").val(),
                    Address: $("#txtFilterAddress").val(),
                    OrgTypeDelimited: 2,
                    ParentOrgDescription: $("#txtFilterParentOrg").val(),
                    IsBranchActive: objEMSCommonJS.GetMultiSelectList("multiSelectedIsBranchActive").value,
                    ServiceBayCountMin: $("#txtServiceBayCountMin").val(),
                    ServiceBayCountMax: $("#txtServiceBayCountMax").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblAdministratorBranchInfoList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedOrgType").html("");
                $("#multiSelectedOrgTypeOption label, #multiSelectedOrgTypeOption input").prop("title", "add");
                $("#multiSelectedIsBranchActive").html("");
                $("#multiSelectedIsBranchActiveOption label, #multiSelectedIsBranchActiveOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnExport").click(function () {

                var parameters = "&sidx=" + $("#tblAdministratorBranchInfoList").jqGrid("getGridParam", "sortname")
                    + "&sord=" + $("#tblAdministratorBranchInfoList").jqGrid("getGridParam", "sortorder")
                    + "&rows=" + $("#tblAdministratorBranchInfoList").jqGrid("getGridParam", "records")
                    + "&pageNumber=" + $("#tblAdministratorBranchInfoList").jqGrid("getGridParam", "page")
                    + "&ID=" + $("#txtFilterOrgGroupID").val()
                    + "&Code=" + $("#txtFilterCode").val()
                    + "&Description=" + $("#txtFilterDescription").val()
                    + "&Category=" + $("#txtFilterCategory").val()
                    + "&Email=" + $("#txtFilterEmail").val()
                    + "&Number=" + $("#txtFilterNumber").val()
                    + "&Address=" + $("#txtFilterAddress").val()
                    + "&OrgTypeDelimited=" + 2
                    + "&ParentOrgDescription=" + $("#txtFilterParentOrg").val()
                    + "&IsBranchActive=" + objEMSCommonJS.GetMultiSelectList("multiSelectedIsBranchActive").value
                    + "&ServiceBayCountMin=" + $("#txtServiceBayCountMin").val()
                    + "&ServiceBayCountMax=" + $("#txtServiceBayCountMax").val();

                var GetSuccessFunction = function (data) {

                    if (data.IsSuccess == true) {
                        window.location = DownloadOrgGroupExportListURL + parameters;
                        $("#divModal").modal("hide");
                    }
                    else {
                        ModalAlert(MODAL_HEADER, data.Result);
                    }
                };

                objEMSCommonJS.GetAjax(GetCheckOrgGroupExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
            });

            enumData.push({ ID: "YES", Description: "YES" });
            enumData.push({ ID: "NO", Description: "NO" });
            objEMSCommonJS.BindFilterMultiSelectEnumLocalData("multiSelectedIsBranchActive", enumData);
            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedOrgType", GetOrgTypeURL);
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblAdministratorBranchInfoList") == "" ? "" : $.parseJSON(localStorage.getItem("tblAdministratorBranchInfoList"));
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
            $("#tblAdministratorBranchInfoList").jqGrid("GridUnload");
            $("#tblAdministratorBranchInfoList").jqGrid("GridDestroy");
            $("#tblAdministratorBranchInfoList").jqGrid({
                url: BranchInfoListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Branch Code", "Branch Name", "Category", "Email", "Mobile Number","Address", "Organizational Type", "Parent Organization", "Is Branch Active?", "Service Bay Count"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objBranchInfoListJS.AddLink },
                    { name: "Code", index: "Code", editable: true, align: "left", sortable: true },
                    { name: "Description", index: "Description", editable: true, align: "left" },
                    { name: "Category", index: "Category", editable: true, align: "left" },
                    { name: "Email", index: "Email", editable: true, align: "left" },
                    { name: "Number", index: "Number", editable: true, align: "left" },
                    { name: "Address", index: "Address", editable: true, align: "left" },
                    { hidden:true, name: "OrgTypeDescription", index: "OrgTypeDescription", editable: true, align: "left" },
                    { name: "ParentOrgDescription", index: "ParentOrgDescription", editable: true, align: "left" },
                    { name: "IsBranchActive", index: "IsBranchActive", editable: true, align: "left" },
                    { name: "ServiceBayCount", index: "ServiceBayCount", editable: true, align: "left" },
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
                    $("#tblAdministratorBranchInfoList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblAdministratorBranchInfoList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblAdministratorBranchInfoList .jqgrid-id-link").click(function () {
                            $('#divBranchInfoModal').modal('show');
                        });

                    }

                    if (localStorage["SystemUserListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["SystemUserListFilterOption"]));
                    }
                    objBranchInfoListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objBranchInfoListJS.ShowHideFilter();
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
                    GetJQGridState("tblAdministratorBranchInfoList");
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
            localStorage["BranchInfoListID"] = $("#txtFilterOrgGroupID").val();
            localStorage["BranchInfoListCode"] = $("#txtFilterCode").val();
            localStorage["BranchInfoListDescription"] = $("#txtFilterDescription").val();
            localStorage["BranchInfoListCategory"] = $("#txtFilterCategory").val();
            localStorage["BranchInfoListEmail"] = $("#txtFilterEmail").val();
            localStorage["BranchInfoListNumber"] = $("#txtFilterNumber").val();
            localStorage["BranchInfoListAddress"] = $("#txtFilterAddress").val();
            localStorage["BranchInfoListParentOrg"] = $("#txtFilterParentOrg").val();
            localStorage["BranchInfoListOrgTypeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").value;
            localStorage["BranchInfoListOrgTypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").text;
            localStorage["BranchInfoListServiceBayCountMin"] = $("#txtServiceBayCountMin").val();
            localStorage["BranchInfoListServiceBayCountMax"] = $("#txtServiceBayCountMax").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterOrgGroupID").val(localStorage["BranchInfoListID"]);
            $("#txtFilterCode").val(localStorage["BranchInfoListCode"]);
            $("#txtFilterDescription").val(localStorage["BranchInfoListDescription"]);
            $("#txtFilterCategory").val(localStorage["BranchInfoListCategory"]);
            $("#txtFilterEmail").val(localStorage["BranchInfoListEmail"]);
            $("#txtFilterNumber").val(localStorage["BranchInfoListNumber"]);
            $("#txtFilterAddress").val(localStorage["BranchInfoListAddress"]);
            $("#txtFilterParentOrg").val(localStorage["BranchInfoListParentOrg"]);
            $("#txtServiceBayCountMin").val(localStorage["BranchInfoListServiceBayCountMin"]);
            $("#txtServiceBayCountMax").val(localStorage["BranchInfoListServiceBayCountMax"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgType"
                , "BranchInfoListOrgTypeDelimited"
                , "BranchInfoListOrgTypeDelimitedText");

        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + BranchInfoViewURL + "?ID=" + rowObject.ID + "', 'divBranchInfoBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
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
    
     objBranchInfoListJS.Initialize();
});