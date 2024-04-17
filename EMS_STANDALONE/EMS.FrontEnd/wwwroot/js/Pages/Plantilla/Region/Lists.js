var objRegionListJS;
const RegionListURL = "/Plantilla/Region?handler=List";
const RegionAddURL = "/Plantilla/Region/Add";
const RegionViewURL = "/Plantilla/Region/View";
const RegionEditURL = "/Plantilla/Region/Edit";
const RegionDeleteURL = "/Plantilla/Region/Delete";
const RegionAddPostURL = "/Plantilla/Region/Add";
const RegionEditPostURL = "/Plantilla/Region/Edit";
$(document).ready(function () {
    objRegionListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            var param = {
                ID: localStorage["PlantillaRegionListID"],
                Code: localStorage["PlantillaRegionListCode"],
                Description: localStorage["PlantillaRegionListDescription"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterRegionID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterRegionID").val(),
                    Code: $("#txtFilterCode").val(),
                    Description: $("#txtFilterDescription").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaRegionList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(RegionAddURL, "divRegionBodyModal");
                $("#divRegionModal").modal("show");
            });
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaRegionList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaRegionList"));
            
            var moveFilterFields = function() {
                var intialHeight = $(".jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $(".jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });

                $(".jqgfirstrow").css({ "height": intialHeight + "px"});

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            };
            moveFilterFields();

            $("#tblPlantillaRegionList").jqGrid("GridUnload");
            $("#tblPlantillaRegionList").jqGrid("GridDestroy");
            $("#tblPlantillaRegionList").jqGrid({
                url: RegionListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Code", "Description"],
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objRegionListJS.AddLink },
                    { name: "Code", index: "Code", editable: true, align: "left", sortable: true },
                    { name: "Description", index: "Description", editable: true, align: "left" },
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
                        AutoSizeColumnJQGrid("tblPlantillaRegionList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child("+(n+2)+")");
                        });

                        $("#tblPlantillaRegionList .jqgrid-id-link").click(function () {
                            $('#divRegionModal').modal('show');
                        });

                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblPlantillaRegionList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
        },

        SetLocalStorage: function () {
            localStorage["PlantillaRegionListID"] = $("#txtFilterRegionID").val();
            localStorage["PlantillaRegionListCode"] = $("#txtFilterCode").val();
            localStorage["PlantillaRegionListDescription"] = $("#txtFilterDescription").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterRegionID").val(localStorage["PlantillaRegionListID"]);
            $("#txtFilterCode").val(localStorage["PlantillaRegionListCode"]);
            $("#txtFilterDescription").val(localStorage["PlantillaRegionListDescription"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + RegionViewURL + "?ID=" + rowObject.ID + "', 'divRegionBodyModal');\">" + rowObject.ID + "</a>"; 
        },
    };
    
     objRegionListJS.Initialize();
});