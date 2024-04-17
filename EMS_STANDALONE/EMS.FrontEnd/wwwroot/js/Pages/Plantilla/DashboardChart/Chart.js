var objChartJS;

const GetPlantillaDashboard = "/Plantilla/DashboardChart?handler=PlantillaDashboard";
const GetDashboardDataURL = "/Plantilla/DashboardChart?handler=ReferenceValue&RefCode=DASHBOARD_DATA";
const GetDashboardTypeURL = "/Plantilla/DashboardChart?handler=ReferenceValue&RefCode=DASHBOARD_TYPE";
const GetDashboardTableURL = "/Plantilla/Dashboard";
const OrgGroupAutoCompleteURL = "/Plantilla/DashboardChart?handler=OrgTypeAutoComplete";
const PositionAutoCompleteURL = "/Plantilla/DashboardChart?handler=PositionAutoComplete";
const EmploymentStatusDropDownURL = "/Plantilla/DashboardChart?handler=ReferenceValue&RefCode=EMPLOYMENT_STATUS";

$(document).ready(function () {
    objChartJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            $("#ddlDashboardData option[value=EmpStatusCount]").attr('selected', 'selected');
            $("#ddlDashboardType option[value=BAR]").attr('selected', 'selected');
            $('#ddlDashboardData option[value="EmpStatusCount"]').insertAfter('#ddlDashboardData option[value=""]');

            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabDashboardChart');

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

            GenerateDropdownValues(GetDashboardDataURL + "&Code=" + "",
                "ddlDashboardData", "Value", "Description", "", "", false);
            GenerateDropdownValues(GetDashboardTypeURL + "&Code=" + "",
                "ddlDashboardType", "Value", "Description", "", "", false);

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
                    EmploymentStatus: objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value
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
            });

            $("#txtSearchData").on("change keyup keydown", function () {
                ListBoxSearch("ddlData", $(this).val(), false);
            });

            $("#ddlDashboardData").on("change", function () {
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

                    $("#ddlDashboardType").on("change", function () {
                        eval("objChartJS." + $(this).val() + "(data)");
                    });

                    $("#ddlData").html("");
                    $(data.Result).each(function (index, item) {
                        $("#ddlData").append($('<option/>', {
                            value: item.Value,
                            text: item.Count + " - " + item.Value
                        }));
                    });
                },
                error: function (error) {
                    console.log(`Error ${error}`);
                }
            });
        },
        DISPOSECHART: function (divId) {
            am5.array.each(am5.registry.rootElements, function (root) {
                if (root.dom.id == divId) {
                    root.dispose();
                }
            });
        },
        BAR: function (data) {
            am5.ready(function () {

                objChartJS.DISPOSECHART("chartdiv");
                var root = am5.Root.new("chartdiv");
                root._logo.dispose();

                root.setThemes([
                    am5themes_Animated.new(root)
                ]);

                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                    panX: true,
                    panY: true,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    pinchZoomX: true
                }));

                var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {}));
                cursor.lineY.set("visible", false);

                var xRenderer = am5xy.AxisRendererX.new(root, { minGridDistance: 30 });
                xRenderer.labels.template.setAll({
                    rotation: -90,
                    centerY: am5.p50,
                    centerX: am5.p100,
                    paddingRight: 15
                });

                xRenderer.grid.template.setAll({
                    location: 1
                })

                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    maxDeviation: 0.3,
                    categoryField: "Value",
                    renderer: xRenderer,
                    tooltip: am5.Tooltip.new(root, {})
                }));

                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    maxDeviation: 0.3,
                    renderer: am5xy.AxisRendererY.new(root, {
                        strokeOpacity: 0.1
                    })
                }));


                var series = chart.series.push(am5xy.ColumnSeries.new(root, {
                    name: "Series 1",
                    xAxis: xAxis,
                    yAxis: yAxis,
                    valueYField: "Count",
                    sequencedInterpolation: true,
                    categoryXField: "Value",
                    tooltip: am5.Tooltip.new(root, {
                        labelText: "{valueY}"
                    })
                }));

                series.columns.template.setAll({ cornerRadiusTL: 5, cornerRadiusTR: 5, strokeOpacity: 0 });
                series.columns.template.adapters.add("fill", function (fill, target) {
                    return chart.get("colors").getIndex(series.columns.indexOf(target));
                });

                series.columns.template.adapters.add("stroke", function (stroke, target) {
                    return chart.get("colors").getIndex(series.columns.indexOf(target));
                });


                var dataResult = data.Result;

                xAxis.data.setAll(dataResult);
                series.data.setAll(dataResult);

                series.appear(1000);
                chart.appear(1000, 100);

                // Set up export and annotation
                var exporting = am5plugins_exporting.Exporting.new(root, {
                    menu: am5plugins_exporting.ExportingMenu.new(root, {})
                });
            }); // end am5.ready()
        },

        DOUBLEPIE: function (data) {
            am5.ready(function () {

                objChartJS.DISPOSECHART("chartdiv");
                var root = am5.Root.new("chartdiv");
                root._logo.dispose();

                // Create custom theme
                // https://www.amcharts.com/docs/v5/concepts/themes/#Quick_custom_theme
                var myTheme = am5.Theme.new(root);
                myTheme.rule("Label").set("fontSize", "0.8em");

                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root),
                    myTheme
                ]);

                // Create wrapper container
                var container = root.container.children.push(am5.Container.new(root, {
                    width: am5.p100,
                    height: am5.p100,
                    layout: root.horizontalLayout
                }));

                // Create first chart
                // https://www.amcharts.com/docs/v5/charts/percent-charts/pie-chart/
                var chart0 = container.children.push(am5percent.PieChart.new(root, {
                    innerRadius: am5.p50,
                    tooltip: am5.Tooltip.new(root, {})
                }));

                // Create series
                // https://www.amcharts.com/docs/v5/charts/percent-charts/pie-chart/#Series
                var series0 = chart0.series.push(am5percent.PieSeries.new(root, {
                    valueField: "Count",
                    categoryField: "Value",
                    alignLabels: false
                }));

                series0.labels.template.setAll({
                    textType: "circular",
                    templateField: "dummyLabelSettings"
                });

                series0.ticks.template.set("forceHidden", true);

                var sliceTemplate0 = series0.slices.template;
                sliceTemplate0.setAll({
                    draggable: true,
                    templateField: "settings",
                    cornerRadius: 5
                });

                // Separator line
                container.children.push(am5.Line.new(root, {
                    layer: 1,
                    height: am5.percent(60),
                    y: am5.p50,
                    centerY: am5.p50,
                    strokeDasharray: [4, 4],
                    stroke: root.interfaceColors.get("alternativeBackground"),
                    strokeOpacity: 0.5
                }));

                // Label
                container.children.push(am5.Label.new(root, {
                    layer: 1,
                    text: "Drag slices over the line",
                    y: am5.p50,
                    textAlign: "center",
                    rotation: -90,
                    isMeasured: false
                }));

                // Create second chart
                // https://www.amcharts.com/docs/v5/charts/percent-charts/pie-chart/
                var chart1 = container.children.push(am5percent.PieChart.new(root, {
                    innerRadius: am5.p50,
                    tooltip: am5.Tooltip.new(root, {})
                }));

                // Create series
                // https://www.amcharts.com/docs/v5/charts/percent-charts/pie-chart/#Series
                var series1 = chart1.series.push(am5percent.PieSeries.new(root, {
                    valueField: "Count",
                    categoryField: "Value",
                    alignLabels: false
                }));

                series1.labels.template.setAll({
                    textType: "circular",
                    radius: 20,
                    templateField: "dummyLabelSettings"
                });

                series1.ticks.template.set("forceHidden", true);

                var sliceTemplate1 = series1.slices.template;
                sliceTemplate1.setAll({
                    draggable: true,
                    templateField: "settings",
                    cornerRadius: 5
                });

                var previousDownSlice;

                // change layers when down
                sliceTemplate0.events.on("pointerdown", function (e) {
                    if (previousDownSlice) {
                        //  previousDownSlice.set("layer", 0);
                    }
                    e.target.set("layer", 1);
                    previousDownSlice = e.target;
                });

                sliceTemplate1.events.on("pointerdown", function (e) {
                    if (previousDownSlice) {
                        // previousDownSlice.set("layer", 0);
                    }
                    e.target.set("layer", 1);
                    previousDownSlice = e.target;
                });

                // when released, do all the magic
                sliceTemplate0.events.on("pointerup", function (e) {
                    series0.hideTooltip();
                    series1.hideTooltip();

                    var slice = e.target;
                    if (slice.x() > container.width() / 4) {
                        var index = series0.slices.indexOf(slice);
                        slice.dataItem.hide();

                        var series1DataItem = series1.dataItems[index];
                        series1DataItem.show();
                        series1DataItem.get("slice").setAll({ x: 0, y: 0 });

                        handleDummy(series0);
                        handleDummy(series1);
                    } else {
                        slice.animate({
                            key: "x",
                            to: 0,
                            duration: 500,
                            easing: am5.ease.out(am5.ease.cubic)
                        });
                        slice.animate({
                            key: "y",
                            to: 0,
                            duration: 500,
                            easing: am5.ease.out(am5.ease.cubic)
                        });
                    }
                });

                sliceTemplate1.events.on("pointerup", function (e) {
                    var slice = e.target;

                    series0.hideTooltip();
                    series1.hideTooltip();

                    if (slice.x() < container.width() / 4) {
                        var index = series1.slices.indexOf(slice);
                        slice.dataItem.hide();

                        var series0DataItem = series0.dataItems[index];
                        series0DataItem.show();
                        series0DataItem.get("slice").setAll({ x: 0, y: 0 });

                        handleDummy(series0);
                        handleDummy(series1);
                    } else {
                        slice.animate({
                            key: "x",
                            to: 0,
                            duration: 500,
                            easing: am5.ease.out(am5.ease.cubic)
                        });
                        slice.animate({
                            key: "y",
                            to: 0,
                            duration: 500,
                            easing: am5.ease.out(am5.ease.cubic)
                        });
                    }
                });

                // data
                var dataResult = [];
                dataResult.push(
                    {
                        Value: "Dummy",
                        Count: data.Total,
                        settings: {
                            fill: am5.color(0xdadada),
                            stroke: am5.color(0xdadada),
                            fillOpacity: 0.3,
                            strokeDasharray: [4, 4],
                            tooltipText: null,
                            draggable: false
                        },
                        dummyLabelSettings: {
                            forceHidden: true
                        }
                    });

                $.each(data.Result, function (index, item) {
                    if(item.Count != 0)
                        dataResult.push(item);
                });

                // show/hide dummy slice depending if there are other visible slices
                function handleDummy(series) {
                    // count visible data items
                    var visibleCount = 0;
                    am5.array.each(series.dataItems, function (dataItem) {
                        if (!dataItem.isHidden()) {
                            visibleCount++;
                        }
                    });
                    // if all hidden, show dummy
                    if (visibleCount == 0) {
                        series.dataItems[0].show();
                    } else {
                        series.dataItems[0].hide();
                    }
                }
                // set data
                series0.data.setAll(dataResult);
                series1.data.setAll(dataResult);
                // hide all except dummy
                am5.array.each(series1.dataItems, function (dataItem) {
                    if (dataItem.get("category") != "Dummy") {
                        dataItem.hide(0);
                    }
                });

                // hide dummy
                series0.dataItems[0].hide(0);

                // reveal container
                container.appear(1000, 100);

                // Set up export and annotation
                var exporting = am5plugins_exporting.Exporting.new(root, {
                    menu: am5plugins_exporting.ExportingMenu.new(root, {})
                });

            }); // end am5.ready()
        }
        
    }

    objChartJS.Initialize();
});