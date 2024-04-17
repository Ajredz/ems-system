var objHomeJS;
var MovementAllEmployeeFieldsChart;
var MovementExitChart;
var MovementPromotionChart;
var ApplicantsChart;

//const DashboardListURL = "/Plantilla/Dashboard?handler=List";
//const GetCheckEmployeeDashboardURL = "/Plantilla/Dashboard?handler=CheckEmployeeDashboard";
//const DownloadEmployeeDashboardURL = "/Plantilla/Dashboard?handler=DownloadEmployeeDashboard";

$(document).ready(function () {
    objHomeJS = {

        monthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ],

        Initialize: function () {
            var s = this;
            $(".main-dashboard").remove();
            $(".main-dashboard-btn").remove();
            s.ElementBinding();
            s.LoadPlantillaProbationary();
            s.LoadPlantillaBirthDays();
            s.LoadMRFRequests();
            
            // Set default dates, past 6 months
            var today = new Date();
            var toDate = (s.monthNames[today.getMonth()]) + ' ' + today.getFullYear();
            today.setMonth(today.getMonth() - 5);
            var fromDate = (s.monthNames[today.getMonth()]) + ' ' + today.getFullYear();

            $("#txtMovementAllFrom").val(fromDate);
            $("#txtMovementAllTo").val(toDate);
            $("#txtMovementExitFrom").val(fromDate);
            $("#txtMovementExitTo").val(toDate);
            $("#txtMovementPromotionFrom").val(fromDate);
            $("#txtMovementPromotionTo").val(toDate);
            $("#txtApplicantsFrom").val(fromDate);
            $("#txtApplicantsTo").val(toDate);

            $("#btnMovementAllSearch").click();
            $("#btnMovementExitSearch").click();
            $("#btnMovementPromotionSearch").click();
            $("#btnApplicantsSearch").click();

        },

        ElementBinding: function () {
            $("#txtMovementAllFrom, #txtMovementAllTo, #txtMovementExitFrom, #txtMovementExitTo \
                , #txtMovementPromotionFrom, #txtMovementPromotionTo \
                , #txtApplicantsFrom, #txtApplicantsTo").datetimepicker({
                useCurrent: false,
                format: 'MMM YYYY'
            });

            $("#btnMovementAllSearch").click(function () {
                if (MovementAllEmployeeFieldsChart != undefined)
                    MovementAllEmployeeFieldsChart.destroy();
                objHomeJS.LoadMovementAllEmployeeFields();
            });

            $("#btnMovementExitSearch").click(function () {
                if (MovementExitChart != undefined)
                    MovementExitChart.destroy();
                objHomeJS.LoadMovementExit();
            });

            $("#btnMovementPromotionSearch").click(function () {
                if (MovementPromotionChart != undefined)
                    MovementPromotionChart.destroy();
                objHomeJS.LoadMovementPromotion();
            });

            $("#btnApplicantsSearch").click(function () {
                if (ApplicantsChart != undefined)
                    ApplicantsChart.destroy();
                objHomeJS.LoadRecruitmentApplicants();
            });


            var acc = document.getElementsByClassName("main-dashboard-btn");
            var i;

            for (i = 0; i < acc.length; i++) {
                acc[i].addEventListener("click", function () {
                    this.classList.toggle("active");
                    var panel = this.nextElementSibling;
                    if (panel.style.maxHeight) {
                        panel.style.maxHeight = null;
                    } else {
                        panel.style.maxHeight = panel.scrollHeight + "px";
                    }
                });
            }
        },

        LoadPlantillaProbationary: function () {
            var cvsProbationary = document.getElementById('cvsProbationary').getContext('2d');
            var cvsProbationaryChart = new Chart(cvsProbationary, {
                type: 'bar',
                data: {
                    labels: ['EXPIRING IN 1 MONTH', 'BEYOND 6 MONTHS'],
                    datasets: [{
                        label: '',
                        data: [
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50)
                        ],
                        backgroundColor: [
                            'rgba(255, 165, 0, 0.6)',
                            'rgba(255, 0, 0, 0.6)'
                        ],
                        borderColor: [
                            'rgba(255, 165, 0, 1)',
                            'rgba(255, 0, 0, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'PROBATIONARY STATUS',
                        fontSize: 15,
                        fontFamily: 'Segoe UI',
                        fontColor: '#114aa1'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                //stepSize: 10,
                                //max: 123
                            }
                        }]
                    },
                    legend: {
                        display: false
                    },
                    hover: {
                        onHover: function (e, elements) {
                            $(e.currentTarget).css("cursor", elements[0] ? "pointer" : "default");
                        }
                    }
                }
            });
        },

        LoadPlantillaBirthDays: function () {
            var cvsBirthdays = document.getElementById('cvsBirthdays').getContext('2d');
            var cvsBirthdaysChart = new Chart(cvsBirthdays, {
                type: 'bar',
                data: {
                    labels: ['THIS MONTH', 'NEXT MONTH'],
                    datasets: [{
                        label: '',
                        data: [
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50)
                        ],
                        backgroundColor: [
                            'rgba(69, 176, 140, 0.6)',
                            'rgba(154, 205, 50, 0.6)'
                        ],
                        borderColor: [
                            'rgba(69, 176, 140, 1)',
                            'rgba(154, 205, 50, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'UPCOMMING BIRTHDAYS',
                        fontSize: 15,
                        fontFamily: 'Segoe UI',
                        fontColor: '#114aa1'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                //stepSize: 10,
                                //max: 123
                            }
                        }]
                    },
                    legend: {
                        display: false
                    },
                    hover: {
                        onHover: function (e, elements) {
                            $(e.currentTarget).css("cursor", elements[0] ? "pointer" : "default");
                        }
                    }
                }
            });
        },

        LoadMovementAllEmployeeFields: function () {
            var ctx = document.getElementById('cvsMovementAll').getContext('2d');
            MovementAllEmployeeFieldsChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: objHomeJS.GetDateNames($("#txtMovementAllFrom").val(), $("#txtMovementAllTo").val()),
                    datasets: [{
                        label: 'POSITION',
                        data: [
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50)
                        ],
                        backgroundColor: [
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)'
                        ],
                        borderColor: [
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)'
                        ],
                        borderWidth: 1,
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        pointBackgroundColor: 'rgba(69, 176, 140, 1)'
                    },
                    {
                        label: 'ORG GROUP',
                        data: [
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50)
                        ],
                        backgroundColor: [
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)'
                        ],
                        borderColor: [
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)'
                        ],
                        borderWidth: 1,
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        pointBackgroundColor: 'rgba(154, 205, 50, 1)'
                    },
                    {
                        label: 'EMPLOYEMENT STATUS',
                        data: [
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50)
                        ],
                        backgroundColor: [
                            'rgba(255, 0, 0, 0.2)',
                            'rgba(255, 0, 0, 0.2)',
                            'rgba(255, 0, 0, 0.2)',
                            'rgba(255, 0, 0, 0.2)',
                            'rgba(255, 0, 0, 0.2)',
                            'rgba(255, 0, 0, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)'
                        ],
                        borderWidth: 1,
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        pointBackgroundColor: 'rgba(255, 0, 0, 1)'
                    },
                    {
                        label: 'SECONDARY DESIGNATION',
                        data: [
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50)
                        ],
                        backgroundColor: [
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)'
                        ],
                        borderColor: [
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)'
                        ],
                        borderWidth: 1,
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        pointBackgroundColor: 'rgba(154, 205, 50, 1)'
                    }
                    ]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'MOVEMENTS PER FIELD',
                        fontSize: 15,
                        fontFamily: 'Segoe UI',
                        fontColor: '#114aa1'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                //stepSize: 10,
                                //max: 123
                            }
                        }]
                    },
                    legend: {
                        display: true
                    },
                    hover: {
                        onHover: function (e, elements) {
                            $(e.currentTarget).css("cursor", elements[0] ? "pointer" : "default");
                        }
                    }
                }
            });

        },

        LoadMovementExit: function () {
            var ctx = document.getElementById('cvsMovementExit').getContext('2d');
            MovementExitChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: objHomeJS.GetDateNames($("#txtMovementExitFrom").val(), $("#txtMovementExitTo").val()),
                    datasets: [{
                        label: 'RESIGNED',
                        data: [
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50)
                        ],
                        backgroundColor: [
                            'rgba(255, 165, 0, 0.6)',
                            'rgba(255, 165, 0, 0.6)',
                            'rgba(255, 165, 0, 0.6)',
                            'rgba(255, 165, 0, 0.6)',
                            'rgba(255, 165, 0, 0.6)',
                            'rgba(255, 165, 0, 0.6)'
                        ],
                        borderColor: [
                            'rgba(255, 165, 0, 1)',
                            'rgba(255, 165, 0, 1)',
                            'rgba(255, 165, 0, 1)',
                            'rgba(255, 165, 0, 1)',
                            'rgba(255, 165, 0, 1)',
                            'rgba(255, 165, 0, 1)'
                        ],
                        borderWidth: 1
                    },
                    {
                        label: 'TERMINATED',
                        data: [
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5)
                        ],
                        backgroundColor: [
                            'rgba(255, 0, 0, 0.6)',
                            'rgba(255, 0, 0, 0.6)',
                            'rgba(255, 0, 0, 0.6)',
                            'rgba(255, 0, 0, 0.6)',
                            'rgba(255, 0, 0, 0.6)',
                            'rgba(255, 0, 0, 0.6)'
                        ],
                        borderColor: [
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)',
                            'rgba(255, 0, 0, 1)'
                        ],
                        borderWidth: 1
                    }
                    ]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'RESIGNED / TERMINATED',
                        fontSize: 15,
                        fontFamily: 'Segoe UI',
                        fontColor: '#114aa1'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                //stepSize: 10,
                                //max: 123
                            }
                        }]
                    },
                    legend: {
                        display: true
                    },
                    hover: {
                        onHover: function(e, elements) {
                            $(e.currentTarget).css("cursor", elements[0] ? "pointer" : "default");
                        }
                    }
                }
            });

        },

        LoadMovementPromotion: function () {
            var ctx = document.getElementById('cvsMovementPromotion').getContext('2d');
            MovementPromotionChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: objHomeJS.GetDateNames($("#txtMovementPromotionFrom").val(), $("#txtMovementPromotionTo").val()),
                    datasets: [{
                        label: 'LATERAL TRANSFER',
                        data: [
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50),
                            Math.ceil(Math.random() * 50)
                        ],
                        backgroundColor: [
                            'rgba(69, 176, 140, 0.6)',
                            'rgba(69, 176, 140, 0.6)',
                            'rgba(69, 176, 140, 0.6)',
                            'rgba(69, 176, 140, 0.6)',
                            'rgba(69, 176, 140, 0.6)',
                            'rgba(69, 176, 140, 0.6)'
                        ],
                        borderColor: [
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)'
                        ],
                        borderWidth: 1
                    },
                        {
                            label: 'PROMOTION',
                            data: [
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50)
                            ],
                            backgroundColor: [
                                'rgba(154, 205, 50, 0.6)',
                                'rgba(154, 205, 50, 0.6)',
                                'rgba(154, 205, 50, 0.6)',
                                'rgba(154, 205, 50, 0.6)',
                                'rgba(154, 205, 50, 0.6)',
                                'rgba(154, 205, 50, 0.6)'
                            ],
                            borderColor: [
                                'rgba(154, 205, 50, 1)',
                                'rgba(154, 205, 50, 1)',
                                'rgba(154, 205, 50, 1)',
                                'rgba(154, 205, 50, 1)',
                                'rgba(154, 205, 50, 1)',
                                'rgba(154, 205, 50, 1)'
                            ],
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'LATERAL TRANSFER / PROMOTION',
                        fontSize: 15,
                        fontFamily: 'Segoe UI',
                        fontColor: '#114aa1'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                //stepSize: 10,
                                //max: 123
                            }
                        }]
                    },
                    legend: {
                        display: true
                    },
                    hover: {
                        onHover: function (e, elements) {
                            $(e.currentTarget).css("cursor", elements[0] ? "pointer" : "default");
                        }
                    }
                }
            });

        },

        LoadMRFRequests: function () {
            var cvsProbationary = document.getElementById('cvsMRFRequests').getContext('2d');
            var cvsProbationaryChart = new Chart(cvsProbationary, {
                type: 'bar',
                data: {
                    labels: ['WITHIN 15 DAYS', '16-30 DAYS', '31-45 DAYS', 'MORE THAN 45 DAYS'],
                    datasets: [
                        {
                            label: 'CLOSED',
                            data: [
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50)
                            ],
                            backgroundColor: [
                                'rgba(26, 34, 33, 0.2)',
                                'rgba(26, 34, 33, 0.2)',
                                'rgba(26, 34, 33, 0.2)',
                                'rgba(26, 34, 33, 0.2)',
                            ],
                            borderColor: [
                                'rgba(26, 34, 33, 0.3)',
                                'rgba(26, 34, 33, 0.3)',
                                'rgba(26, 34, 33, 0.3)',
                                'rgba(26, 34, 33, 0.3)',
                            ],
                            borderWidth: 1
                        },
                        {
                            label: 'OPEN',
                            data: [
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50),
                                Math.ceil(Math.random() * 50)
                            ],
                            backgroundColor: [
                                'rgba(59, 224, 30, 0.6)',
                                'rgba(255, 255, 0, 0.6)',
                                'rgba(255, 165, 0, 0.6)',
                                'rgba(255, 0, 0, 0.6)',
                            ],
                            borderColor: [
                                'rgba(59, 224, 30, 1)',
                                'rgba(255, 255, 0, 1)',
                                'rgba(255, 165, 0, 1)',
                                'rgba(255, 0, 0, 1)',
                            ],
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'MANPOWER REQUESTS',
                        fontSize: 15,
                        fontFamily: 'Segoe UI',
                        fontColor: '#114aa1'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                //stepSize: 10,
                                //max: 123
                            },
                            stacked: true
                        }],
                        xAxes: [{
                            stacked: true
                        }]
                    },
                    legend: {
                        display: true
                    },
                    hover: {
                        onHover: function (e, elements) {
                            $(e.currentTarget).css("cursor", elements[0] ? "pointer" : "default");
                        }
                    }
                }
            });
        },

        LoadRecruitmentApplicants: function () {
            var ctx = document.getElementById('cvsApplicants').getContext('2d');
            ApplicantsChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: objHomeJS.GetDateNames($("#txtApplicantsFrom").val(), $("#txtApplicantsTo").val()),
                    datasets: [{
                        label: 'NO. OF APPLICANTS',
                        data: [
                            Math.ceil(Math.random() * 50 + 5),
                            Math.ceil(Math.random() * 50 + 5),
                            Math.ceil(Math.random() * 50 + 5),
                            Math.ceil(Math.random() * 50 + 5),
                            Math.ceil(Math.random() * 50 + 5),
                            Math.ceil(Math.random() * 50 + 5)
                        ],
                        backgroundColor: [
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)',
                            'rgba(69, 176, 140, 0.2)'
                        ],
                        borderColor: [
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)',
                            'rgba(69, 176, 140, 1)'
                        ],
                        borderWidth: 1,
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        pointBackgroundColor: 'rgba(69, 176, 140, 1)'
                    },
                    {
                        label: 'NO. OF HIRED APPLICANTS',
                        data: [
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5),
                            Math.ceil(Math.random() * 5)
                        ],
                        backgroundColor: [
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)',
                            'rgba(154, 205, 50, 0.2)'
                        ],
                        borderColor: [
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)',
                            'rgba(154, 205, 50, 1)'
                        ],
                        borderWidth: 1,
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        pointBackgroundColor: 'rgba(154, 205, 50, 1)'
                    }]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'APPLICANTS',
                        fontSize: 15,
                        fontFamily: 'Segoe UI',
                        fontColor: '#114aa1'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                //stepSize: 10,
                                //max: 123
                            }
                        }]
                    },
                    legend: {
                        display: true
                    },
                    hover: {
                        onHover: function (e, elements) {
                            $(e.currentTarget).css("cursor", elements[0] ? "pointer" : "default");
                        }
                    }
                   
                }
            });

        },

        GetDateNames: function (startDate, endDate) {
            var startDate = moment(startDate);
            var endDate = moment(endDate);

            var result = [];

            if (endDate.isBefore(startDate)) {
                throw "End date must be greated than start date."
            }

            while (startDate.isBefore(endDate)) {
                var dateCtr = new Date(startDate);
                result.push((objHomeJS.monthNames[dateCtr.getMonth()]));
                startDate.add(1, 'month');
            }

            result.push((objHomeJS.monthNames[new Date(endDate).getMonth()]));

            return result;
        },

    };
    
     objHomeJS.Initialize();
});