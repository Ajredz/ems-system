var objTrainingListJS;

const TrainingListURL = "/Administrator/Training?handler=List";
const TrainingViewURL = "/Administrator/Training/View";
const TrainingAddURL = "/Administrator/Training/Add";
const TrainingEditURL = "/Administrator/Training/Edit";
const GetTypeDropDownURL = "/Administrator/Training?handler=ReferenceValue&RefCode=TRAINING_TYPE";
const GetTrainingTemplateDetailsURL = "/Administrator/Training?handler=TrainingTemplateDetails";

$(document).ready(function () {
    objTrainingListJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();

            var param = {
                ID: localStorage["TrainingListID"],
                PreloadName: localStorage["TrainingListTemplateName"],
                CreatedDateFrom: localStorage["TrainingListCreatedDateFrom"],
                CreatedDateTo: localStorage["TrainingListCreatedDateTo"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            NumericFormat($("#txtFilterID"));

            $("#txtFilterCreatedDateFrom, #txtFilterCreatedDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#btnSearch").click();
            });
            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterID").val(),
                    TemplateName: $("#txtFilterTemplateName").val(),
                    CreatedDateFrom: $("#txtFilterCreatedDateFrom").val(),
                    CreatedDateTo: $("#txtFilterCreatedDateTo").val()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblTrainingList");
                s.LoadJQGrid(param);
            });
            $("#btnAdd").click(function () {
                LoadPartial(TrainingAddURL, "divTrainingBodyModal");
                $("#divTrainingModal").modal("show");
            });
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);

            var tableInfo = localStorage.getItem("tblTrainingList") == "" ? "" : $.parseJSON(localStorage.getItem("tblTrainingList"));
            var moveFilterFields = function () {
                var intialHeight = $(".jqgfirstrow").height();
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

            $("#tblTrainingList").jqGrid("GridUnload");
            $("#tblTrainingList").jqGrid("GridDestroy");
            $("#tblTrainingList").jqGrid({
                url: TrainingListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Template Name", "Created Date"],
                colModel: [
                    { width: 30, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objTrainingListJS.ViewID },
                    { name: "TemplateName", index: "TemplateName", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true }
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
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblTrainingList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblTrainingList", data);

                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 1) + ")");
                        });

                        $("#tblTrainingList .jqgrid-id-link").click(function () {
                            $('#divTrainingModal').modal('show');
                        });

                    }

                    if (localStorage["TrainingListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["TrainingListFilterOption"]));
                    }
                    objTrainingListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objTrainingListJS.ShowHideFilter();
                        localStorage["TrainingListFilterOption"] = $("#chkFilter").is(":checked");
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
                beforeRequest: function () {
                    GetJQGridState("tblTrainingList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#divPager").css("width", "100%");
            $("#divPager").css("height", "100%");

            $("#tblTrainingList_toppager_center").hide();
            $("#tblTrainingList_toppager_right").hide();
            $("#tblTrainingList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filters</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divPager_custom_block_right").appendTo("#divPager_left");
            $("#divPager_center .ui-pg-table").appendTo("#divPager_right");
        },
        ViewID: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + TrainingViewURL + "?ID=" + rowObject.ID + "', 'divTrainingBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },

        SetLocalStorage: function () {
            localStorage["TrainingListID"] = $("#txtFilterID").val();
            localStorage["TrainingListName"] = $("#txtFilterTemplateName").val();
            localStorage["TrainingListCreatedDateFrom"] = $("#txtFilterCreatedDateFrom").val();
            localStorage["TrainingListCreatedDateTo"] = $("#txtFilterCreatedDateTo").val();
        },
        GetLocalStorage: function () {
            $("#txtFilterID").val(localStorage["TrainingListID"]);
            $("#txtFilterTemplateName").val(localStorage["TrainingListName"]);
            $("#txtFilterCreatedDateFrom").val(localStorage["TrainingListCreatedDateFrom"]);
            $("#txtFilterCreatedDateTo").val(localStorage["TrainingListCreatedDateTo"]);
        },
    };

    objTrainingListJS.Initialize();
});