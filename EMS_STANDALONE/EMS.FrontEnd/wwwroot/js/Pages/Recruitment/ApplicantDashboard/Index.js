var objApplicantDashboardJS;
var orgGroupList = [];
var positionList = [];
var applicantCountList = [];

const ApplicantCountByOrgGroupURL = "/Recruitment/ApplicantDashboard?handler=ApplicantCountByOrgGroup";

$(document).ready(function () {
    objApplicantDashboardJS = {

        Initialize: function () {
            var s = this;
            s.getChartData(); 
        },

        ShowChart: function () {
            var popCanvasName = document.getElementById("ApplicantDashboard");
            var barChartName = new Chart(popCanvasName, {
                type: 'bar',
                data: {
                labels: orgGroupList,
                    datasets: 
                    {
                        label: 'Sample',
                        //value: openValue,
                        data: applicantCountList,
                        backgroundColor: '#49e2ff'
                    }
                },
                options: {
                    responsive: false,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                stepSize: 1,
                                //max: Math.max(...openCountList) + Math.max(...closedCountList) + 2,
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
                    legend: {
                        display: true,
                        position: 'right',
                        align: 'start'
                    },
                }
            });

        },

        getChartData: function () {

            var s = this;
            var GetSuccessFunction = function (data) {

                $(data.Result).each(function (index, item) {
                    orgGroupList.push(item.OrgGroup);
                    positionList.push(item.PositionTitle);
                    applicantCountList.push(item.ApplicantCount);
                });

                s.ShowChart();
            };

            objEMSCommonJS.GetAjax(ApplicantCountByOrgGroupURL, {}, "", GetSuccessFunction);

        },
    };

    objApplicantDashboardJS.Initialize();

});