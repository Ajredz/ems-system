var objApproverSetupListJS;
var enumData = [];

const ApproverSetupListURL = "/Manpower/ApproverSetup?handler=List";
const GetOrgTypeURL = "/Manpower/ApproverSetup?handler=ReferenceValue&RefCode=ORGGROUPTYPE";

const ApproverSetupEditURL = "/ManPower/ApproverSetup/Edit";
const ApproverSetupEditPostURL = "/ManPower/ApproverSetup/Edit?handler=Submit";

$(document).ready(function () {
    objApproverSetupListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                OrgGroup: localStorage["ApproverSetupListOrgGroup"],
                HasApprover: localStorage["ApproverSetupListHasApproverDelimited"],
                ModifiedDateFrom: localStorage["ApproverSetupListLastModifiedDateFrom"],
                ModifiedDateTo: localStorage["ApproverSetupListLastModifiedDateTo"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();


        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterLastModifiedDateFrom, #txtFilterLastModifiedDateTo").datetimepicker({
                    useCurrent: false,
                    format: 'MM/DD/YYYY'
                });

            $("#btnSearch").click(function () {
                var param = {
                    OrgGroup: $("#txtFilterOrgGroup").val(),
                    HasApprover: objEMSCommonJS.GetMultiSelectList("multiSelectedHasApprover").value,
                    ModifiedDateFrom: $("#txtFilterLastModifiedDateFrom").val(),
                    ModifiedDateTo: $("#txtFilterLastModifiedDateTo").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblApproverSetupList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedHasApprover").html("");
                $("#multiSelectedHasApproverOption label, #multiSelectedHasApproverOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            enumData.push({ID: "YES", Description: "YES"});
            enumData.push({ID: "NO", Description: "NO"});
            objEMSCommonJS.BindFilterMultiSelectEnumLocalData("multiSelectedHasApprover", enumData);

        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblApproverSetupList") == "" ? "" : $.parseJSON(localStorage.getItem("tblApproverSetupList"));

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
            $("#tblApproverSetupList").jqGrid("GridUnload");
            $("#tblApproverSetupList").jqGrid("GridDestroy");
            $("#tblApproverSetupList").jqGrid({
                url: ApproverSetupListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Org Group", "Has Approver", "Last Modified Date"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objApproverSetupListJS.AddLink },
                    { name: "OrgGroup", index: "OrgGroup", editable: true, align: "left", sortable: true },
                    { name: "HasApprover", index: "Description", editable: true, align: "center" },
                    { name: "LastModifiedDate", index: "LastModifiedDate", editable: true, align: "center" },
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
                    $("#tblApproverSetupList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblApproverSetupList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblApproverSetupList .jqgrid-id-link").click(function () {
                            $('#divApproverSetupModal').modal('show');
                        });

                    }

                    if (localStorage["ApproverSetupListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["ApproverSetupListFilterOption"]));
                    }
                    objApproverSetupListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objApproverSetupListJS.ShowHideFilter();
                        localStorage["ApproverSetupListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblApproverSetupList");
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
            localStorage["ApproverSetupListOrgGroup"] = $("#txtFilterOrgGroup").val();
            localStorage["ApproverSetupListLastModifiedDateFrom"] = $("#txtFilterLastModifiedDateFrom").val();
            localStorage["ApproverSetupListLastModifiedDateTo"] = $("#txtFilterLastModifiedDateTo").val();
            localStorage["ApproverSetupListHasApproverDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedHasApprover").value;
            localStorage["ApproverSetupListHasApproverDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedHasApprover").text;

        },

        GetLocalStorage: function () {
            $("#txtFilterOrgGroup").val(localStorage["ApproverSetupListOrgGroup"]);
            $("#txtFilterLastModifiedDateFrom").val(localStorage["ApproverSetupListLastModifiedDateFrom"]);
            $("#txtFilterLastModifiedDateTo").val(localStorage["ApproverSetupListLastModifiedDateTo"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedHasApprover"
                , "ApproverSetupListHasApproverDelimited"
                , "ApproverSetupListHasApproverDelimitedText");
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + ApproverSetupEditURL + "?ID=" + rowObject.ID + "', 'divApproverSetupBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
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

    objApproverSetupListJS.Initialize();
});