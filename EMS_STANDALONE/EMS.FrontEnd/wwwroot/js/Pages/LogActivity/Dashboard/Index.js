var objChartJS;

const GetPlantillaDashboard = "/LogActivity/Dashboard?handler=AccountabilityDashboard";
const GetDashboardDataURL = "/LogActivity/Dashboard?handler=ReferenceValue&RefCode=DASHBOARD_ACC_DATA";
const GetDashboardTypeURL = "/LogActivity/Dashboard?handler=ReferenceValue&RefCode=DASHBOARD_ACC_TYPE";
const GetDashboardFilterURL = "/LogActivity/Dashboard?handler=ReferenceValue&RefCode=DASHBOARD_ACC_DATE_FILTER";
const GetDashboardTableURL = "/LogActivity/Dashboard";
const OrgGroupAutoCompleteURL = "/LogActivity/Dashboard?handler=OrgTypeAutoComplete";
const PositionAutoCompleteURL = "/LogActivity/Dashboard?handler=PositionAutoComplete";
const EmploymentStatusDropDownURL = "/LogActivity/Dashboard?handler=ReferenceValuePlantilla&RefCode=EMPLOYMENT_STATUS";

$(document).ready(function () {
    objChartJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            $("#ddlDashboardData option[value=OpenVsClose]").attr('selected', 'selected');
            $("#ddlDashboardType option[value=LayeredColumnChart]").attr('selected', 'selected');
            $('#ddlDashboardData option[value="OpenVsClose"]').insertAfter('#ddlDashboardData option[value=""]');

            var param = {
                DashboardData: $("#ddlDashboardData :selected").val(),
                OrgGroupID: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                PositionID: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                EmploymentStatus: objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value
            }
            s.LoadData(param);
        },

        ElementBinding: function () {
            var s = this;

            $("#txtDateFrom, #txtDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            GenerateDropdownValues(GetDashboardDataURL + "&Code=" + "",
                "ddlDashboardData", "Value", "Description", "", "", false);
            GenerateDropdownValues(GetDashboardTypeURL + "&Code=" + "",
                "ddlDashboardType", "Value", "Description", "", "", false);
            GenerateDropdownValues(GetDashboardFilterURL + "&Code=" + "",
                "ddlDate", "Value", "Description", "", "", false);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , PositionAutoCompleteURL, 20, "multiSelectedPosition");

            objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedEmploymentStatus", EmploymentStatusDropDownURL, "Value", "Description");

            $(".tablinks").find("span:contains('Table')").parent("button").click(function () {
                LoadPartial(GetDashboardTableURL, "tabDashboardTable");
            });

            $("#btnSearch").on("click", function () {
                var param = {
                    DashboardData: $("#ddlDashboardData :selected").val(),
                    OrgGroupID: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    PositionID: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    EmploymentStatus: objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value,
                    DateFilter: $("#ddlDate :selected").val(),
                    DateFrom: $("#txtDateFrom").val(),
                    DateTo: $("#txtDateTo").val()
                }
                s.LoadData(param);
            });

            $("#btnReset").on("click", function () {
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedOrgGroupOption label, #multiSelectedOrgGroupOption input").prop("title", "add");
                $("#multiSelectedPosition").html("");
                $("#multiSelectedPositionOption label, #multiSelectedPositionOption input").prop("title", "add");
                $("#multiSelectedEmploymentStatus").html("");
                $("#multiSelectedEmploymentStatusOption label, #multiSelectedEmploymentStatusOption input").prop("title", "add");
                $("#ddlDate").val(""),
                $("#txtDateFrom").val("");
                $("#txtDateTo").val("");
                $("#btnSearch").click();
            });

            $("#txtSearchData").on("change keyup keydown", function () {
                ListBoxSearch("ddlData", $(this).val(), false);
            });

            $("#ddlDashboardData").on("change", function () {
                $("#btnSearch").click();
            });
            $("#ddlDashboardType").on("change", function () {
                $("#btnSearch").click();
            });
        },

        LoadData: function (param) {
            Loading(true);
            $.ajax({
                url: GetPlantillaDashboard,
                data: param,
                type: "GET",
                success: function (data) {
                    Loading(false);

                    eval("objChartJS." + $("#ddlDashboardType").val() + "(data)");

                    if ($("#ddlDashboardData").val() == "OpenVsClose") {
                        var Param = [];
                        $(data.Result).each(function (index, item) {
                            var Object = {};
                            Object["Value"] = item.Description;
                            Object["Description"] = "(" + item.Actual + " / " + item.Target + ") - " + item.Description;
                            Param.push(Object);
                        });
                        objChartJS.DataList(Param);
                    }
                    if ($("#ddlDashboardData").val() == "CloseStatusByAge") {
                        var Week = [];
                        var WeekOne = {}, WeekTwo = {}, WeekThree = {}, WeekFour = {};
                        var WeekOneTotal = 0, WeekTwoTotal = 0, WeekThreeTotal = 0, WeekFourTotal = 0;
                        $(data.Result).each(function (index, item) {
                            if (item.Target == 1) {
                                WeekOneTotal += item.Actual
                            }
                            if (item.Target == 2) {
                                WeekTwoTotal += item.Actual
                            }
                            if (item.Target == 3) {
                                WeekThreeTotal += item.Actual
                            }
                            if (item.Target == 4) {
                                WeekFourTotal += item.Actual
                            }
                        });
                        WeekOne["Value"] = "WeekOne";
                        WeekOne["Description"] = WeekOneTotal + " - Within 7 Days";
                        WeekTwo["Value"] = "WeekTwo";
                        WeekTwo["Description"] = WeekTwoTotal + " - Within 8 - 22 Days";
                        WeekThree["Value"] = "WeekThree";
                        WeekThree["Description"] = WeekThreeTotal + " - Within 23 - 30 Days";
                        WeekFour["Value"] = "WeekFour";
                        WeekFour["Description"] = WeekFourTotal + " - More Than 30 Days";
                        Week.push(WeekOne);
                        Week.push(WeekTwo);
                        Week.push(WeekThree);
                        Week.push(WeekFour);
                        objChartJS.DataList(Week);
                    }
                        
                },
                error: function (error) {
                    console.log(`Error ${error}`);
                }
            });
        },
        DataList: function (data) {
            $("#ddlData").html("");
            $(data).each(function (index, item) {
                $("#ddlData").append($('<option/>', {
                    value: item.Value,
                    text: item.Description
                }));
            });
        },
        DISPOSECHART: function (divId) {
            am5.array.each(am5.registry.rootElements, function (root) {
                if (root.dom.id == divId) {
                    root.dispose();
                }
            });
        },
        LayeredColumnChart: function (data) {
            am5.ready(function () {

                objChartJS.DISPOSECHART("chartdiv");
                var root = am5.Root.new("chartdiv");
                root._logo.dispose();

                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);

                // Create chart
                // https://www.amcharts.com/docs/v5/charts/xy-chart/
                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                    panX: true,
                    panY: false,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    layout: root.verticalLayout
                }));

                // Add scrollbar
                // https://www.amcharts.com/docs/v5/charts/xy-chart/scrollbars/
                chart.set("scrollbarX", am5.Scrollbar.new(root, {
                    orientation: "horizontal"
                }));

                var dataResult = data.Result;

                // Create axes
                // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
                var xRenderer = am5xy.AxisRendererX.new(root, {
                    minGridDistance: 70
                });

                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    categoryField: "Description",
                    renderer: xRenderer,
                    tooltip: am5.Tooltip.new(root, {
                        themeTags: ["axis"],
                        animationDuration: 200
                    })
                }));

                xRenderer.grid.template.setAll({
                    location: 1
                })

                xAxis.data.setAll(dataResult);

                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    min: 0,
                    renderer: am5xy.AxisRendererY.new(root, {
                        strokeOpacity: 0.1
                    })
                }));

                // Add series
                // https://www.amcharts.com/docs/v5/charts/xy-chart/series/

                var series0 = chart.series.push(am5xy.ColumnSeries.new(root, {
                    name: "Income",
                    xAxis: xAxis,
                    yAxis: yAxis,
                    valueYField: "Target",
                    categoryXField: "Description",
                    clustered: false,
                    tooltip: am5.Tooltip.new(root, {
                        labelText: "Total: {valueY}"
                    })
                }));

                series0.columns.template.setAll({
                    width: am5.percent(80),
                    tooltipY: 0,
                    strokeOpacity: 0
                });

                series0.set("fill", am5.color("#D3D3D3"));
                series0.data.setAll(dataResult);


                var series1 = chart.series.push(am5xy.ColumnSeries.new(root, {
                    name: "Income",
                    xAxis: xAxis,
                    yAxis: yAxis,
                    valueYField: "Actual",
                    categoryXField: "Description",
                    clustered: false,
                    tooltip: am5.Tooltip.new(root, {
                        labelText: "Cleared: {valueY}"
                    })
                }));

                series1.columns.template.setAll({
                    width: am5.percent(75),
                    tooltipY: 0,
                    strokeOpacity: 0
                });

                series1.data.setAll(dataResult);

                var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {}));


                // Make stuff animate on load
                // https://www.amcharts.com/docs/v5/concepts/animations/
                chart.appear(1000, 100);
                series0.appear();
                series1.appear();

            }); // end am5.ready()
        },
        StackedColumnChartByPercent: function (data) {
            am5.ready(function () {

                objChartJS.DISPOSECHART("chartdiv");
                var root = am5.Root.new("chartdiv");
                root._logo.dispose();

                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);

                // Create chart
                // https://www.amcharts.com/docs/v5/charts/xy-chart/
                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                    panX: false,
                    panY: false,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    layout: root.verticalLayout
                }));

                // Add scrollbar
                // https://www.amcharts.com/docs/v5/charts/xy-chart/scrollbars/
                chart.set("scrollbarX", am5.Scrollbar.new(root, {
                    orientation: "horizontal"
                }));

                var dataResult = [];
                var WeekOne = {};
                var WeekTwo = {};
                var WeekThree = {};
                var WeekFour = {};
                var WeekOneTotal = 0,WeekTwoTotal = 0,WeekThreeTotal = 0,WeekFourTotal = 0;
                $.each(data.Result, function (index, item) {
                    if (item.Target == 1) {
                        WeekOne[item.Description] = item.Actual;
                        WeekOneTotal += item.Actual;
                    }
                    if (item.Target == 2) {
                        WeekTwo[item.Description] = item.Actual;
                        WeekTwoTotal += item.Actual;
                    }
                    if (item.Target == 3) {
                        WeekThree[item.Description] = item.Actual;
                        WeekThreeTotal += item.Actual;
                    }
                    if (item.Target == 4) {
                        WeekFour[item.Description] = item.Actual;
                        WeekFourTotal += item.Actual;
                    }
                });
                WeekOne["week"] = "Within 7 Days - Total: " + WeekOneTotal;
                WeekTwo["week"] = "Within 8 - 22 Days - Total: " + WeekTwoTotal;
                WeekThree["week"] = "Within 23 - 30 Days - Total: " + WeekThreeTotal;
                WeekFour["week"] = "More Than 30 Days - Total: " + WeekFourTotal;
                dataResult.push(WeekOne);
                dataResult.push(WeekTwo);
                dataResult.push(WeekThree);
                dataResult.push(WeekFour);

                // Create axes
                // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
                var xRenderer = am5xy.AxisRendererX.new(root, {});
                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    categoryField: "week",
                    renderer: xRenderer,
                    tooltip: am5.Tooltip.new(root, {})
                }));

                xRenderer.grid.template.setAll({
                    location: 1
                })

                xAxis.data.setAll(dataResult);

                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    min: 0,
                    max: 100,
                    numberFormat: "#'%'",
                    strictMinMax: true,
                    calculateTotals: true,
                    renderer: am5xy.AxisRendererY.new(root, {
                        strokeOpacity: 0.1
                    })
                }));


                // Add legend
                // https://www.amcharts.com/docs/v5/charts/xy-chart/legend-xy-series/
                var legend = chart.children.push(am5.Legend.new(root, {
                    centerX: am5.p50,
                    x: am5.p50
                }));


                // Add series
                // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
                function makeSeries(name, fieldName) {
                    var series = chart.series.push(am5xy.ColumnSeries.new(root, {
                        name: name,
                        stacked: true,
                        xAxis: xAxis,
                        yAxis: yAxis,
                        valueYField: fieldName,
                        valueYShow: "valueYTotalPercent",
                        categoryXField: "week"
                    }));

                    series.columns.template.setAll({
                        tooltipText: "{name}, {categoryX}:{valueYTotalPercent.formatNumber('#.#')}%",
                        tooltipY: am5.percent(10)
                    });
                    series.data.setAll(dataResult);

                    // Make stuff animate on load
                    // https://www.amcharts.com/docs/v5/concepts/animations/
                    series.appear();

                    series.bullets.push(function () {
                        return am5.Bullet.new(root, {
                            sprite: am5.Label.new(root, {
                                text: "{valueYTotalPercent.formatNumber('#.#')}%",
                                fill: root.interfaceColors.get("alternativeText"),
                                centerY: am5.p50,
                                centerX: am5.p50,
                                populateText: true
                            })
                        });
                    });

                    //legend.data.push(series);
                }

                var Series = [];
                $.each(data.Result, function (index, item) {
                    if (!Series.includes(item.Description)) {
                        Series.push(item.Description);
                        makeSeries(item.Description, item.Description);
                    }
                });

                // Make stuff animate on load
                // https://www.amcharts.com/docs/v5/concepts/animations/
                chart.appear(1000, 100);

            }); // end am5.ready()
        },
        StackedColumnChartByCount: function (data) {
            am5.ready(function () {

                objChartJS.DISPOSECHART("chartdiv");
                var root = am5.Root.new("chartdiv");
                root._logo.dispose();


                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);


                // Create chart
                // https://www.amcharts.com/docs/v5/charts/xy-chart/
                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                    panX: false,
                    panY: false,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    layout: root.verticalLayout
                }));

                // Add scrollbar
                // https://www.amcharts.com/docs/v5/charts/xy-chart/scrollbars/
                chart.set("scrollbarX", am5.Scrollbar.new(root, {
                    orientation: "horizontal"
                }));

                var dataResult = [];
                var WeekOne = {};
                var WeekTwo = {};
                var WeekThree = {};
                var WeekFour = {};
                var WeekOneTotal = 0, WeekTwoTotal = 0, WeekThreeTotal = 0, WeekFourTotal = 0;
                $.each(data.Result, function (index, item) {
                    if (item.Target == 1) {
                        WeekOne[item.Description] = item.Actual;
                        WeekOneTotal += item.Actual;
                    }
                    if (item.Target == 2) {
                        WeekTwo[item.Description] = item.Actual;
                        WeekTwoTotal += item.Actual;
                    }
                    if (item.Target == 3) {
                        WeekThree[item.Description] = item.Actual;
                        WeekThreeTotal += item.Actual;
                    }
                    if (item.Target == 4) {
                        WeekFour[item.Description] = item.Actual;
                        WeekFourTotal += item.Actual;
                    }
                });
                WeekOne["week"] = "Within 7 Days - Total: " + WeekOneTotal;
                WeekTwo["week"] = "Within 8 - 22 Days - Total: " + WeekTwoTotal;
                WeekThree["week"] = "Within 23 - 30 Days - Total: " + WeekThreeTotal;
                WeekFour["week"] = "More Than 30 Days - Total: " + WeekFourTotal;
                dataResult.push(WeekOne);
                dataResult.push(WeekTwo);
                dataResult.push(WeekThree);
                dataResult.push(WeekFour);


                // Create axes
                // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
                var xRenderer = am5xy.AxisRendererX.new(root, {});
                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    categoryField: "week",
                    renderer: xRenderer,
                    tooltip: am5.Tooltip.new(root, {})
                }));

                xRenderer.grid.template.setAll({
                    location: 1
                })

                xAxis.data.setAll(dataResult);

                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    min: 0,
                    renderer: am5xy.AxisRendererY.new(root, {
                        strokeOpacity: 0.1
                    })
                }));


                // Add legend
                // https://www.amcharts.com/docs/v5/charts/xy-chart/legend-xy-series/
                var legend = chart.children.push(am5.Legend.new(root, {
                    centerX: am5.p50,
                    x: am5.p50
                }));


                // Add series
                // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
                function makeSeries(name, fieldName) {
                    var series = chart.series.push(am5xy.ColumnSeries.new(root, {
                        name: name,
                        stacked: true,
                        xAxis: xAxis,
                        yAxis: yAxis,
                        valueYField: fieldName,
                        categoryXField: "week"
                    }));

                    series.columns.template.setAll({
                        tooltipText: "{name}: {valueY}",
                        tooltipY: am5.percent(10)
                    });
                    series.data.setAll(dataResult);

                    // Make stuff animate on load
                    // https://www.amcharts.com/docs/v5/concepts/animations/
                    series.appear();

                    series.bullets.push(function () {
                        return am5.Bullet.new(root, {
                            sprite: am5.Label.new(root, {
                                text: "{valueY}",
                                fill: root.interfaceColors.get("alternativeText"),
                                centerY: am5.p50,
                                centerX: am5.p50,
                                populateText: true
                            })
                        });
                    });

                    //legend.data.push(series);
                }

                var Series = [];
                $.each(data.Result, function (index, item) {
                    if (!Series.includes(item.Description)) {
                        Series.push(item.Description);
                        makeSeries(item.Description, item.Description);
                    }
                });


                // Make stuff animate on load
                // https://www.amcharts.com/docs/v5/concepts/animations/
                chart.appear(1000, 100);

            }); // end am5.ready()
        },
    }

    objChartJS.Initialize();
});