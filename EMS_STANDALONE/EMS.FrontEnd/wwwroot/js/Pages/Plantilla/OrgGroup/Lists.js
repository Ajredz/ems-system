var enumData = [];
var objOrgGroupListJS;
const OrgGroupListURL = window.location.pathname + "?handler=List";
const OrgTypeAutoCompleteURL = window.location.pathname + "?handler=OrgTypeAutoComplete";
const OrgGroupAddURL = window.location.pathname.replace("/list", "") + "/Add";
const OrgGroupViewURL = window.location.pathname.replace("/list", "") + "/View";
const GetOrgGroupChildrenURL = window.location.pathname.replace("/list", "") + "/View?handler=ChildrenOrgDropDown";
const GetOrgGroupPositionURL = window.location.pathname.replace("/list", "") + "/View?handler=OrgGroupPosition";
const GetOrgGroupNPRFURL = window.location.pathname.replace("/list", "") + "/View?handler=OrgGroupNPRF";
const OrgGroupEditURL = window.location.pathname.replace("/list", "") + "/Edit";
const OrgGroupDeleteURL = window.location.pathname.replace("/list", "") + "/Delete";
const OrgGroupAddPostURL = window.location.pathname.replace("/list", "") + "/Add";
const OrgGroupEditPostURL = window.location.pathname.replace("/list", "") + "/Edit";
const OrgGroupEditDetailsURL = window.location.pathname.replace("/list", "") + "/EditDetails";
const PositionAddURL = "/Plantilla/Position/Add";
const PositionAddPostURL = "/Plantilla/Position/Add";
const ViewPositionByIDURL = window.location.pathname.replace("/list", "") + "/View?handler=PositionDropDownByID";
const ViewOrgGroupDropDownURL = window.location.pathname.replace("/list", "") + "/View?handler=OrgGroupDropDown";
const ViewPositionLevelDropDownURL = window.location.pathname.replace("/list", "") + "/View?handler=PositionLevelDropDown";
const PositionDropDownURL = window.location.pathname.replace("/list","") + "/View?handler=PositionDropDown";
const EditPositionByIDURL = window.location.pathname.replace("/list","") + "/View?handler=PositionDropDownByID";
const GetOrgTypeURL = window.location.pathname + "?handler=ReferenceValue&RefCode=ORGGROUPTYPE";
const GetOrgGroupTagsByOrgGroupTypeURL = window.location.pathname + "?handler=OrgGroupTagsByOrgGroupType&RefCode=";
const DownloadOrgGroupFormURL = window.location.pathname + "?handler=DownloadTemplate&ID=1";
const UploadInsertOrgGroupURL = window.location.pathname.replace("/list", "") + "/UploadInsert?handler=UploadInsertOrgGroup";
const UploadEditOrgGroupURL = window.location.pathname.replace("/list", "") + "/UploadEdit?handler=UploadEditOrgGroup";
const GetCheckOrgGroupExportListURL = window.location.pathname + "?handler=CheckOrgGroupExportList";
const DownloadOrgGroupExportListURL = window.location.pathname + "?handler=DownloadOrgGroupExportList";
const OrgTypeHierarchyUpwardURL = window.location.pathname + "?handler=OrgGroupHierarchy";
const OrgTypeEmployeeListURL = window.location.pathname.replace("/list", "") + "/View?handler=OrgGroupEmployeeList";
const PositionAutoCompleteURL = window.location.pathname.replace("/list","") + "/View?handler=PositionAutoComplete";
const OrgGroupNPRFListURL = window.location.pathname.replace("/list","") + "/View?handler=OrgGroupNPRFList";
const OrgGroupPlantillaCountUpdateURL = window.location.pathname.replace("/list", "") + "/UpdatePlantilla";
var PositionWithLevelAutoCompleteURL = window.location.pathname + "?handler=PositionWithLevelByAutoComplete";
var ParentOrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgGroupAutoComplete";

const UserOrgTypeHierarchyUpwardURL = window.location.pathname + "?handler=UserOrgGroup";
const UserChildrenOrgDropDownURL = window.location.pathname + "?handler=ChildrenOrgDropDown";

var OrgTypeFilterValue = ["REG", "AREA", "CLUS"];

$(document).ready(function () {
    objOrgGroupListJS = {

        Initialize: function () {
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["PlantillaOrgGroupListID"],
                Code: localStorage["PlantillaOrgGroupListCode"],
                Description: localStorage["PlantillaOrgGroupListDescription"],
                OrgTypeDelimited: localStorage["PlantillaOrgGroupListOrgTypeDelimited"],
                ParentOrgDescription: localStorage["PlantillaOrgGroupListParentOrg"],
                ServiceBayCountMin: localStorage["PlantillaOrgGroupListServiceBayCountMin"],
                ServiceBayCountMax: localStorage["PlantillaOrgGroupListServiceBayCountMax"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            //TO GET USER DEFAULT ORG GROUP
            s.LoadUserOrgGroup();
            //TO DISPLAY ORG GROUP HIERARCHY
            //s.LoadHierarchyUpward();
            //TO GET CHILD LIST
            s.LoadChildrenOrgGroup($("#hdnOrgID").val(), $("#hdnOrgType").val());

            //TO HIDE ORG GROUP FILTER IF NOT IN MANAGER
            if (OrgTypeFilterValue.includes($("#hdnOrgType").val())) {
                $("#hdnOrgGroupFilter").show();
            }

            /*================= WIP ===================*/
            //setTimeout(function () { $("#btnAdd").click();},200);
            /*================= WIP ===================*/
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterOrgGroupID"));

            $("#divOrgGroupTableList #btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterOrgGroupID").val(),
                    Code: $("#txtFilterCode").val(),
                    Description: $("#txtFilterDescription").val(),
                    OrgTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").value,
                    ParentOrgDescription: $("#txtFilterParentOrg").val(),
                    IsBranchActive: objEMSCommonJS.GetMultiSelectList("multiSelectedIsBranchActive").value,
                    ServiceBayCountMin: $("#txtServiceBayCountMin").val(),
                    ServiceBayCountMax: $("#txtServiceBayCountMax").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaOrgGroupList");
                s.LoadJQGrid(param);
            });

            $("#divOrgGroupTableList #btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedOrgType").html("");
                $("#multiSelectedOrgTypeOption label, #multiSelectedOrgTypeOption input").prop("title", "add");
                $("#multiSelectedIsBranchActive").html("");
                $("#multiSelectedIsBranchActiveOption label, #multiSelectedIsBranchActiveOption input").prop("title", "add");
                $("#divOrgGroupTableList #btnSearch").click();

                $("#ddlRegion").empty();
                $("#ddlArea").empty();
                $("#ddlCluster").empty();
                $("#ddlBranch").empty();
                $("#ddlRegion").html("<option selected>- Select an item -</option>")
                $("#ddlArea").html("<option selected>- Select an item -</option>")
                $("#ddlCluster").html("<option selected>- Select an item -</option>")
                $("#ddlBranch").html("<option selected>- Select an item -</option>")
                s.LoadUserOrgGroup();
                s.LoadChildrenOrgGroup($("#hdnOrgID").val(), $("#hdnOrgType").val());
            });
            enumData.push({ ID: "YES", Description: "YES" });
            enumData.push({ ID: "NO", Description: "NO" });
            objEMSCommonJS.BindFilterMultiSelectEnumLocalData("multiSelectedIsBranchActive", enumData);

          objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedOrgType", GetOrgTypeURL);

            $("#btnAdd").click(function () {
                LoadPartial(OrgGroupAddURL, "divOrgGroupBodyModal");
                $("#divOrgGroupModal").modal("show");
            });

            $("#btnUploadInsert").click(function () {
                objEMSCommonJS.UploadModal(UploadInsertOrgGroupURL, "Upload (Insert) Org Group", DownloadOrgGroupFormURL);
                $('#divModalErrorMessage').html('');
            });

            $("#btnUploadEdit").click(function () {
                objEMSCommonJS.UploadModal(UploadEditOrgGroupURL, "Upload (Edit) Org Group", "");
                $('#divModalErrorMessage').html('');
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objOrgGroupListJS.ExportFunction()",
                    "function");
            });

            $('#ddlArea').change(function () {
                var ddlValue = $("#ddlArea :selected").val();
                $("#ddlCluster").empty();
                $("#ddlCluster").html("<option selected>- Select an item -</option>")
                s.LoadChildrenOrgGroup(ddlValue, "AREA");

                var param = {
                    ID: $("#txtFilterOrgGroupID").val(),
                    Code: $("#txtFilterCode").val(),
                    Description: $("#txtFilterDescription").val(),
                    OrgTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").value,
                    ParentOrgDescription: $("#ddlArea :selected").text(),
                    IsBranchActive: objEMSCommonJS.GetMultiSelectList("multiSelectedIsBranchActive").value,
                    ServiceBayCountMin: $("#txtServiceBayCountMin").val(),
                    ServiceBayCountMax: $("#txtServiceBayCountMax").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaOrgGroupList");
                s.LoadJQGrid(param);
            });

            $('#ddlCluster').change(function () {
                var ddlValue = $("#ddlCluster :selected").val();
                $("#ddlBranch").empty();
                $("#ddlBranch").html("<option selected>- Select an item -</option>")
                s.LoadChildrenOrgGroup(ddlValue, "CLUS");

                var param = {
                    ID: $("#txtFilterOrgGroupID").val(),
                    Code: $("#txtFilterCode").val(),
                    Description: $("#txtFilterDescription").val(),
                    OrgTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").value,
                    ParentOrgDescription: $("#ddlCluster :selected").text(),
                    IsBranchActive: objEMSCommonJS.GetMultiSelectList("multiSelectedIsBranchActive").value,
                    ServiceBayCountMin: $("#txtServiceBayCountMin").val(),
                    ServiceBayCountMax: $("#txtServiceBayCountMax").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaOrgGroupList");
                s.LoadJQGrid(param);
            });

            $('#ddlBranch').change(function () {
                var ddlValue = $("#ddlBranch :selected").val();

                var param = {
                    ID: ddlValue,
                    Code: $("#txtFilterCode").val(),
                    Description: $("#txtFilterDescription").val(),
                    OrgTypeDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").value,
                    ParentOrgDescription: $("#txtFilterParentOrg").val(),
                    IsBranchActive: objEMSCommonJS.GetMultiSelectList("multiSelectedIsBranchActive").value,
                    ServiceBayCountMin: $("#txtServiceBayCountMin").val(),
                    ServiceBayCountMax: $("#txtServiceBayCountMax").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaOrgGroupList");
                s.LoadJQGrid(param);
            });
        },

        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblPlantillaOrgGroupList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaOrgGroupList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterOrgGroupID").val()
                + "&Code=" + $("#txtFilterCode").val()
                + "&Description=" + $("#txtFilterDescription").val()
                + "&OrgTypeDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").value
                + "&ParentOrgDescription=" + $("#txtFilterParentOrg").val()
                + "&IsBranchActive=" + objEMSCommonJS.GetMultiSelectList("multiSelectedIsBranchActive").value
                + "&ServiceBayCountMin=" + $("#txtServiceBayCountMin").val()
                + "&ServiceBayCountMax=" + $("#txtServiceBayCountMax").val();
                   
                

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    Loading(true);
                    window.location = DownloadOrgGroupExportListURL + parameters;
                    $("#divModal").modal("hide");
                    Loading(false);
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckOrgGroupExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaOrgGroupList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaOrgGroupList"));

            var moveFilterFields = function() {
                var intialHeight = $("#divOrgGroupTableList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divOrgGroupTableList .jqgfirstrow td .filterFields").each(function(n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $("#divOrgGroupTableList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function(e) {
                    if (e.keyCode == "13") {
                      $("#divOrgGroupTableList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();

            $("#tblPlantillaOrgGroupList").jqGrid("GridUnload");
            $("#tblPlantillaOrgGroupList").jqGrid("GridDestroy");
            $("#tblPlantillaOrgGroupList").jqGrid({
                url: OrgGroupListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Plantilla", "Code", "Description", "Organizational Type", "Parent Organization", "Is Branch Active?", "Service Bay Count"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objOrgGroupListJS.AddLink },
                    { hidden:true,name: "UpdatePlantilla", index: "", width: 40, align: "center", sortable: true, formatter: objOrgGroupListJS.UpdatePlantillaLink },
                    { name: "Code", index: "Code", editable: true, align: "left", sortable: true },
                    { name: "Description", index: "Description", editable: true, align: "left" },
                    { name: "OrgTypeDescription", index: "OrgTypeDescription", editable: true, align: "left" },
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
                    $("#tblPlantillaOrgGroupList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (OrgTypeFilterValue.includes($("#hdnOrgType").val())) {
                        $(this).jqGrid('hideCol', ["UpdatePlantilla"]);
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                                
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblPlantillaOrgGroupList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divOrgGroupTableList .jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblPlantillaOrgGroupList .jqgrid-id-link").click(function () {
                            $('#divOrgGroupModal').modal('show');
                        });

                        //$("#tblPlantillaOrgGroupList .jqgrid-update-plantilla-link").click(function () {
                            
                        //});

                    }

                    if (localStorage["OrgGroupListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["OrgGroupListFilterOption"]));
                    }
                    objOrgGroupListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objOrgGroupListJS.ShowHideFilter();
                        localStorage["OrgGroupListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    // set minimum height to prevent multiselect tags from being hidden by the scroll
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
                    GetJQGridState("tblPlantillaOrgGroupList");
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
            localStorage["PlantillaOrgGroupListID"] = $("#txtFilterOrgGroupID").val();
            localStorage["PlantillaOrgGroupListCode"] = $("#txtFilterCode").val();
            localStorage["PlantillaOrgGroupListDescription"] = $("#txtFilterDescription").val();
            localStorage["PlantillaOrgGroupListParentOrg"] = $("#txtFilterParentOrg").val();
            localStorage["PlantillaOrgGroupListOrgTypeDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").value;
            localStorage["PlantillaOrgGroupListOrgTypeDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgType").text;
            localStorage["PlantillaOrgGroupListServiceBayCountMin"] = $("#txtServiceBayCountMin").val();
            localStorage["PlantillaOrgGroupListServiceBayCountMax"] = $("#txtServiceBayCountMax").val();

        },

        GetLocalStorage: function () {
            $("#txtFilterOrgGroupID").val(localStorage["PlantillaOrgGroupListID"]);
            $("#txtFilterCode").val(localStorage["PlantillaOrgGroupListCode"]);
            $("#txtFilterDescription").val(localStorage["PlantillaOrgGroupListDescription"]);
            $("#txtFilterParentOrg").val(localStorage["PlantillaOrgGroupListParentOrg"]);
            $("#txtServiceBayCountMin").val(localStorage["PlantillaOrgGroupListServiceBayCountMin"]);
            $("#txtServiceBayCountMax").val(localStorage["PlantillaOrgGroupListServiceBayCountMax"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgType"
              , "PlantillaOrgGroupListOrgTypeDelimited"
              , "PlantillaOrgGroupListOrgTypeDelimitedText");

        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + OrgGroupViewURL + "?ID=" + rowObject.ID + "', 'divOrgGroupBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>"; 
        },

        CalculateTotal: function (input, totalID) {
            var total = 0;
            var arr = $("." + input);
            
            for (var i = 0; i < arr.length; i++) {
                if (parseInt(arr[i].value))
                    total += parseInt(arr[i].value || 0);
            }

            $("#" + totalID).text(total);
        },

        UpdatePlantillaLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-update-plantilla-link' onclick=\"return objOrgGroupListJS.UpdatePlantillaClick('" +
                OrgGroupPlantillaCountUpdateURL + "'," + rowObject.ID + ",'" + (rowObject.Code + " - " + rowObject.Description)+"'); \"><span class=\"btn-glyph-dynamic\"><span class=\"glyphicon glyphicon-user\"></span></span></a>";
        },

        UpdatePlantillaClick: function (URL, OrgGroupID, OrgGroupDescription) {
            var successFunction = function (OrgGroupDescription) {
                setTimeout(function () {
                    $("#divUpdatePlantillaModal #lblOrgGroupDescription").text(OrgGroupDescription)
                }, 300);
            };
            $("#divUpdatePlantillaModal").modal("show");
            LoadPartialSuccessFunction(URL + "?OrgGroupID=" + OrgGroupID, "divUpdatePlantillaBodyModal", successFunction(OrgGroupDescription));
            return false;
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },

        LoadUserOrgGroup: function () {
            var GetSuccessFunction = function (data) {
                var OrgGroupTitle;
                var OrgGroupId;
                var OrgGroupParent;
                var OrgGroupCode;
                var OrgGroupDescription

                $(data.Result).each(function (idx, item) {
                    OrgGroupTitle = item.OrgType;
                    OrgGroupId = item.ID;
                    OrgGroupCode = item.Code;
                    OrgGroupDescription = item.Description;
                });

                $("#orgGroupTitle").html(OrgGroupTitle);
                $("#hdnOrgID").val(OrgGroupId);
                $("#hdnOrgType").val(OrgGroupTitle);

                if (OrgGroupTitle == "REG") {
                    $("#ddlRegion").prop("disabled", true);
                    $("#ddlRegion").html($('<option/>', {
                        value: OrgGroupCode,
                        text: OrgGroupCode + " - " + OrgGroupDescription
                    }));
                }
                else if (OrgGroupTitle=="AREA") {
                    $(".HideRegion").hide();
                    $("#ddlArea").prop("disabled", true);
                    $("#ddlArea").html($('<option/>', {
                        value: OrgGroupCode,
                        text: OrgGroupCode + " - " + OrgGroupDescription
                    }));
                }
                else if (OrgGroupTitle == "CLUS") {
                    $(".HideRegion").hide();
                    $(".HideArea").hide();
                    $("#ddlCluster").prop("disabled", true);
                    $("#ddlCluster").html($('<option/>', {
                        value: OrgGroupCode,
                        text: OrgGroupCode + " - " + OrgGroupDescription
                    }));
                }
            };

            objEMSCommonJS.GetAjax(UserOrgTypeHierarchyUpwardURL
                + "" 
                , {}
                , ""
                , GetSuccessFunction);
        },

        LoadChildrenOrgGroup: function (OrgId,OrgType) {
            var input = { ID: OrgId };

            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    if (OrgType == "REG") {
                        $("#ddlArea").append($('<option/>', {
                            value: item.Value,
                            text: item.Text
                        }));
                    }
                    if (OrgType == "AREA") {
                        $("#ddlCluster").append($('<option/>', {
                            value: item.Value,
                            text: item.Text
                        }));
                    }
                    if (OrgType == "CLUS") {
                        $("#ddlBranch").append($('<option/>', {
                            value: item.Value,
                            text: item.Text
                        }));
                    }
                });
            };

            objEMSCommonJS.GetAjax(UserChildrenOrgDropDownURL, input, "", GetSuccessFunction);
        },

        //LoadHierarchyUpward: function () {
        //    var GetSuccessFunction = function (data) {
        //        var length = data.Result.length;
        //        var codeValue = $("#txtCode").val();
        //        var descValue = $("#txtDescription").val();

        //        $("#divUserHierarchyUpward").html("");

        //        $(data.Result).each(function (idx, item) {
        //            $("#divUserHierarchyUpward").append(item.Code + " - " + item.Description + " > ");
        //        });

        //        if (codeValue != "")
        //            $("#divUserHierarchyUpward").append(codeValue + " - " + descValue);
        //    };

        //    objEMSCommonJS.GetAjax(OrgTypeHierarchyUpwardURL
        //        + "&ID=" + $("#hdnParentOrgID").val()
        //        , {}
        //        , ""
        //        , GetSuccessFunction);
        //}
    };
    
     objOrgGroupListJS.Initialize();
});