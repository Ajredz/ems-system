var objOrgGroupNPRFListJS;

$(document).ready(function () {
    objOrgGroupNPRFListJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            var s = this;
            $("#txtFilterDateApprovedFrom, #txtFilterDateApprovedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            s.ElementBinding();
            var param = {
                NPRFNumber: localStorage["OrgGroupNPRFNumber"],
                DateApprovedFrom: localStorage["OrgGroupDateApprovedFrom"],
                DateApprovedTo: localStorage["OrgGroupDateApprovedTo"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#tabOrgGroupNPRF #btnSearch").click(function () {
                var param = {
                    NPRFNumber: $("#txtFilterNPRFNumber").val(),
                    DateApprovedFrom: $("#txtFilterDateApprovedFrom").val(),
                    DateApprovedTo: $("#txtFilterDateApprovedTo").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblOrgGroupNPRFList");
                s.LoadJQGrid(param);
            });

            $("#divOrgGroupNPRF #btnReset").click(function () {
                $("#divOrgGroupNPRF div.filterFields input").val("");
                $("#divOrgGroupNPRF div.filterFields select").val("");
                $("#divOrgGroupNPRF #btnSearch").click();
            });
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblOrgGroupNPRFList") == "" ? "" : $.parseJSON(localStorage.getItem("tblOrgGroupNPRFList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divOrgGroupNPRF .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divOrgGroupNPRF .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divOrgGroupNPRF #filterFieldsContainerNPRF");
                });

                $("#divOrgGroupNPRF .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divOrgGroupNPRF div.filterFields").unbind("keyup");
                $("#divOrgGroupNPRF div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divOrgGroupNPRF #btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#divOrgGroupNPRF #tblOrgGroupNPRFList").jqGrid("GridUnload");
            $("#divOrgGroupNPRF #tblOrgGroupNPRFList").jqGrid("GridDestroy");
            $("#divOrgGroupNPRF #tblOrgGroupNPRFList").jqGrid({
                url: OrgGroupNPRFListURL + "&ID=" + objOrgGroupNPRFListJS.ID,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "NPRF Number", "Approval Date"],
                colModel: [
                    { hidden: true },
                    { name: "NPRFNumber", index: "NPRFNumber", align: "left", sortable: true, width: 140 },
                    { name: "ApprovedDate", index: "ApprovedDate", align: "left", sortable: true, width: 140}
                ],
                toppager: $("#divOrgGroupNPRF #divPager"),
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
                            //$("#tabOrgGroupNPRF #divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblOrgGroupNPRFList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divOrgGroupNPRF #filterFieldsContainerNPRF .filterFields").each(function (n, element) {
                            $(this).appendTo("#divOrgGroupNPRF .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });


                    }

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divOrgGroupNPRF .ui-jqgrid-bdiv").css({ "min-height": "200px" });

                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblOrgGroupNPRFList");
                    moveFilterFields();
                },
            }).navGrid("#divOrgGroupNPRF #divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery("#divOrgGroupNPRF .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
        },

        SetLocalStorage: function () {
            
            localStorage["OrgGroupNPRFNumber"] = $("#divOrgGroupNPRF #txtFilterNPRFNumber").val();
            localStorage["OrgGroupDateApprovedFrom"] = $("#divOrgGroupNPRF #txtFilterDateApprovedFrom").val();
            localStorage["OrgGroupDateApprovedTo"] = $("#divOrgGroupNPRF #txtFilterDateApprovedTo").val();
        },

        GetLocalStorage: function () {

            $("#divOrgGroupNPRF #txtFilterNPRFName").val(localStorage["OrgGroupNPRFNumber"]);
            $("#divOrgGroupNPRF #txtFilterDateApprovedFrom").val(localStorage["OrgGroupDateApprovedFrom"]);
            $("#divOrgGroupNPRF #txtFilterDateApprovedTo").val(localStorage["OrgGroupDateApprovedTo"]);
        },

    };

    objOrgGroupNPRFListJS.Initialize();

});