var objMRFAdminDashboardJS;
var openCountList = [];
var closedCountList = [];
var ageList = [];
var openDescription = "";
var openValue = "";
var closedDescription = "";
var closedValue = "";
var MRFRequestPerOrgGroup = [];

const MRFAdminDashboardByAgeURL = "/ManPower/Dashboard/Admin?handler=MRFDashboardByAge";
const MRFListURL = "/Manpower/MRF/Admin";

$(document).ready(function () {
    objMRFAdminDashboardJS = {

        Initialize: function () {
            var s = this;
            s.getChartData();

        },

        ShowChartByAge: function () {
            var popCanvasName = document.getElementById("MRFDashboardByAge");
            var barChartName = new Chart(popCanvasName, {
                type: 'bar',
                data: {
                    labels: ageList,
                    datasets:
                        [{
                            label: openDescription,
                            value: openValue,
                            data: openCountList,
                            backgroundColor:
                                [
                                    'rgba(59, 224, 30, 0.5)',
                                    'rgba(245, 230, 127, 0.75)',
                                    'rgba(255, 174, 128, 0.75)',
                                    'rgba(224, 120, 119, 0.75)',
                                ]
                        },
                        {
                            label: closedDescription,
                            value: closedValue,
                            data: closedCountList,
                            backgroundColor: 'rgba(26, 34, 33, 0.3)'
                        }]
                },
                options: {
                    responsive: false,

                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                stepSize: 1,
                                //max: Math.max(...openCountList.concat(...closedCountList)) + 2,
                                max: Math.max(...openCountList) + Math.max(...closedCountList) + 2,
                                fontColor: '#1F3B84',
                                fontStyle: 'bold',
                                fontSize: 14
                            },
                            gridLines: {
                                color: 'rgba(165, 160, 151, 0.5)',
                            },
                            stacked: true
                        }],
                        xAxes: [{
                            ticks: {
                                fontColor: '#1F3B84',
                                fontStyle: 'bold',
                                fontSize: 14
                            },
                            gridLines: {
                                color: 'rgba(165, 160, 151, 0.5)',
                            },
                            stacked: true
                        }]
                    },
                    //legend: {
                    //    display: false
                    //}
                    onClick: function (evt, obj) {

                        if (obj.length > 0) {

                            var activePoint = barChartName.getElementAtEvent(evt)[0];
                            var data = activePoint._chart.data;
                            var datasetIndex = activePoint._datasetIndex;

                            var mrf_age = data.labels[activePoint._index];
                            var mrf_status_val = data.datasets[datasetIndex].value;
                            var mrf_status_text = data.datasets[datasetIndex].label;

                            switch (mrf_age) {
                                case 'Within 15 Days':
                                    localStorage["DashboardFilterAgeMin"] = "0";
                                    localStorage["DashboardFilterAgeMax"] = "15";
                                    localStorage["DashboardFilterStatus"] = mrf_status_val;
                                    localStorage["DashboardFilterStatusText"] = mrf_status_text;
                                    break;
                                case '16 - 30 Days':
                                    localStorage["DashboardFilterAgeMin"] = "16";
                                    localStorage["DashboardFilterAgeMax"] = "30";
                                    localStorage["DashboardFilterStatus"] = mrf_status_val;
                                    localStorage["DashboardFilterStatusText"] = mrf_status_text;
                                    break;
                                case '31 - 45 Days':
                                    localStorage["DashboardFilterAgeMin"] = "31";
                                    localStorage["DashboardFilterAgeMax"] = "45";
                                    localStorage["DashboardFilterStatus"] = mrf_status_val;
                                    localStorage["DashboardFilterStatusText"] = mrf_status_text;
                                    break;
                                case 'More than 45 Days':
                                    localStorage["DashboardFilterAgeMin"] = "46";
                                    localStorage["DashboardFilterAgeMax"] = "";
                                    localStorage["DashboardFilterStatus"] = mrf_status_val;
                                    localStorage["DashboardFilterStatusText"] = mrf_status_text;
                                    break;
                            }

                            localStorage["DashboardFilterDateApprovedFrom"] = "01/01/1900";
                            localStorage["DashboardFilterDateApprovedTo"] = (new Date().getMonth() + 1) + "/" +
                                (new Date().getDate()) + "/" +
                                (new Date().getFullYear());

                            window.open(MRFListURL);
                        }
                    }
                }
            });

        },

        //ShowChartByOrgGroup: function () {
        //    var popCanvasName = document.getElementById("MRFDashboardByOrgGroup");
        //    var barChartName = new Chart(popCanvasName, {
        //        type: 'horizontalBar',
        //        data: {
        //            labels: ["region 1","region 2", "region 3"],
        //            datasets: MRFRequestPerOrgGroup
        //        },
        //        options: {
        //            responsive: false,
        //            scales: {
        //                yAxes: [{
        //                    ticks: {
        //                        fontColor: '#1F3B84',
        //                        fontStyle: 'bold',
        //                        fontSize: 14
        //                    },
        //                    gridLines: {
        //                        color: 'rgba(165, 160, 151, 0.5)',
        //                    },
        //                    stacked: false
        //                }],
        //                xAxes: [{
        //                    ticks: {
        //                        beginAtZero: true,
        //                        stepSize: 1,
        //                        max: 10 ,//Math.max(...openCountList) + Math.max(...closedCountList) + 2,
        //                        fontColor: '#1F3B84',
        //                        fontStyle: 'bold',
        //                        fontSize: 14
        //                    },
        //                    gridLines: {
        //                        color: 'rgba(165, 160, 151, 0.5)',
        //                    },
        //                    stacked: false
        //                }]
        //            },
        //            legend: {
        //                position: 'right',
        //            },
        //            onClick: function (evt, obj) {

        //                if (obj.length > 0) {

        //                    var activePoint = barChartName.getElementAtEvent(evt)[0];
        //                    var data = activePoint._chart.data;
        //                    var datasetIndex = activePoint._datasetIndex;

        //                    var mrf_age = data.labels[activePoint._index];
        //                    var mrf_status_val = data.datasets[datasetIndex].value;
        //                    var mrf_status_text = data.datasets[datasetIndex].label;

        //                    switch (mrf_age) {
        //                        case 'Within 15 Days':
        //                            localStorage["DashboardFilterAgeMin"] = "0";
        //                            localStorage["DashboardFilterAgeMax"] = "15";
        //                            localStorage["DashboardFilterStatus"] = mrf_status_val;
        //                            localStorage["DashboardFilterStatusText"] = mrf_status_text;
        //                            break;
        //                        case '16 - 30 Days':
        //                            localStorage["DashboardFilterAgeMin"] = "16";
        //                            localStorage["DashboardFilterAgeMax"] = "30";
        //                            localStorage["DashboardFilterStatus"] = mrf_status_val;
        //                            localStorage["DashboardFilterStatusText"] = mrf_status_text;
        //                            break;
        //                        case '31 - 45 Days':
        //                            localStorage["DashboardFilterAgeMin"] = "31";
        //                            localStorage["DashboardFilterAgeMax"] = "45";
        //                            localStorage["DashboardFilterStatus"] = mrf_status_val;
        //                            localStorage["DashboardFilterStatusText"] = mrf_status_text;
        //                            break;
        //                        case 'More than 45 Days':
        //                            localStorage["DashboardFilterAgeMin"] = "46";
        //                            localStorage["DashboardFilterAgeMax"] = "";
        //                            localStorage["DashboardFilterStatus"] = mrf_status_val;
        //                            localStorage["DashboardFilterStatusText"] = mrf_status_text;
        //                            break;
        //                    }

        //                    localStorage["DashboardFilterDateApprovedFrom"] = "01/01/1900";
        //                    localStorage["DashboardFilterDateApprovedTo"] = (new Date().getMonth() + 1) + "/" +
        //                        (new Date().getDate()) + "/" +
        //                        (new Date().getFullYear());

        //                    window.open(MRFListURL);
        //                }
        //            }
        //        }
        //    });

        //},

        getChartData: function () {

            var s = this;
            var GetSuccessFunction = function (data) {

                openCountList = data.Result.OpenCountList;
                closedCountList = data.Result.ClosedCountList;
                ageList = data.Result.AgeList;
                openDescription = data.Result.OpenDescription;
                openValue = data.Result.OpenValue;
                closedDescription = data.Result.ClosedDescription;
                closedValue = data.Result.ClosedValue;

                s.ShowChartByAge();

                //MRFRequestPerOrgGroup.push({
                //    label: "position 1",
                //    data: [5, , 1],
                //    backgroundColor: 'rgba(26, 34, 33, 0.3)'

                //});

                //MRFRequestPerOrgGroup.push(
                //    {
                //        label: "position 2",
                //        data: [1, 3,],
                //        backgroundColor: 'rgba(26, 34, 33, 0.3)'
                //    });


                //s.ShowChartByOrgGroup();

            };

            objEMSCommonJS.GetAjax(MRFAdminDashboardByAgeURL, {}, "", GetSuccessFunction);

        },
    };

    objMRFAdminDashboardJS.Initialize();

});