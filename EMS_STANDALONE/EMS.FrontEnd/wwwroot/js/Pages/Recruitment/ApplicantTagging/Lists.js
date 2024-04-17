var objApplicantTaggingListJS;

const ApplicantTaggingListURL = "/Recruitment/ApplicantTagging?handler=List";
const ApplicantTaggingEditURL = "/Recruitment/ApplicantTagging/Edit";

const OrgGroupAutoCompleteURL = "/Recruitment/ApplicantTagging?handler=OrgLevelAutoComplete";
const PositionAutoCompleteURL = "/Recruitment/ApplicantTagging?handler=PositionAutoComplete";
const ReferredByAutoCompleteURL = "/Recruitment/ApplicantTagging?handler=ReferredByAutoComplete";
const GetApplicationSourceURL = "/Recruitment/ApplicantTagging?handler=ReferenceValue&RefCode=APPLICATION_SOURCE";
const ApplicantViewURL = "/Recruitment/Applicant/View";
const DownloadFileURL = "/Recruitment/Applicant?handler=DownloadFile";
const CheckFileIfExistsURL = "/Recruitment/Applicant?handler=CheckFileIfExists";

$(document).ready(function () {
    objApplicantTaggingListJS = {

        Initialize: function () {
            var s = this;
            
            s.ElementBinding();
            var param = {
                ID: localStorage["ApplicantTaggingID"],
                ApplicantName: localStorage["ApplicantTaggingApplicantName"],
                ApplicantSourceDelimited: localStorage["ApplicantTaggingApplicantSourceDelimited"],
                PositionRemarks: localStorage["ApplicantTaggingPositionRemarks"],
                //PositionDelimited: localStorage["ApplicantTaggingPositionDelimited"],
                //OrgGroupRemarks: localStorage["ApplicantTaggingOrgGroupRemarks"],
                //OrgGroupDelimited: localStorage["ApplicantTaggingOrgGroupDelimited"],
                //ReferredByRemarks: localStorage["ApplicantTaggingReferredByRemarks"],
                ReferredBy: localStorage["ApplicantTaggingReferredBy"],

            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterApplicantID").val(),
                    ApplicantName: $("#txtFilterApplicantName").val(),
                    ApplicationSourceDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").value,
                    PositionRemarks: $("#txtFilterPositionRemarks").val(),
                    //PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").value,
                    //OrgGroupRemarks: $("#txtFilterOrgGroupRemarks").val(),
                    //OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").value,
                    //ReferredByRemarks: $("#txtFilterReferredByRemarks").val(),
                    ReferredBy: objEMSCommonJS.GetMultiSelectList("multiSelectedReferredBy").value,
                };
                s.SetLocalStorage();
                ResetJQGridState("tblApplicantTaggingList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("div.filterFields input[type='search']").val("");
                $("#multiSelectedApplicationSource").html("");
                //$("#multiSelectedDesiredPosition").html("");
                //$("#multiSelectedDesiredOrgGroup").html("");
                $("#multiSelectedReferredBy").html("");
                $("#btnSearch").click();
            });


            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedApplicationSource", GetApplicationSourceURL);

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterDesiredOrgGroup"
            //    , OrgGroupAutoCompleteURL, 20, "multiSelectedDesiredOrgGroup");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterDesiredPosition"
            //    , PositionAutoCompleteURL, 20, "multiSelectedDesiredPosition");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterReferredBy"
                , ReferredByAutoCompleteURL, 20, "multiSelectedReferredBy");

        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblApplicantTaggingList") == "" ? "" : $.parseJSON(localStorage.getItem("tblApplicantTaggingList"));

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
            $("#tblApplicantTaggingList").jqGrid("GridUnload");
            $("#tblApplicantTaggingList").jqGrid("GridDestroy");
            $("#tblApplicantTaggingList").jqGrid({
                url: ApplicantTaggingListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "", "Applicant Name", "Application Source", "Desired Position (Remarks)", "Referred By (Tagged)"
                ],
                colModel: [
                    { hidden: true },
                    { width: 25, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objApplicantTaggingListJS.ViewLink },
                    { width: 15, key: true, name: "Tag", index: "ID", align: "center", sortable: true, formatter: objApplicantTaggingListJS.TagLink },
                    { name: "ApplicantName", index: "ApplicantName", align: "left", sortable: true },
                    { name: "ApplicationSource", index: "ApplicationSource", align: "left", sortable: true },
                    { name: "PositionRemarks", index: "PositionRemarks", align: "left", sortable: true },
                    //{ name: "DesiredPosition", index: "DesiredPosition", align: "left", sortable: true },
                    //{ name: "OrgGroupRemarks", index: "OrgGroupRemarks", align: "left", sortable: true },
                    //{ name: "DesiredOrgGroup", index: "DesiredOrgGroup", align: "left", sortable: true },
                    //{ name: "ReferredByRemarks", index: "ReferredByRemarks", align: "left", sortable: true },
                    { name: "ReferredBy", index: "ReferredBy", align: "left", sortable: true },
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
                multiselect: true,
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
                        AutoSizeColumnJQGrid("tblApplicantTaggingList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        //$("#tblApplicantTaggingList .jqgrid-id-link").click(function () {
                        //     $('#divApplicantTaggingModal').modal('show');
                        //});

                    }

                    if (localStorage["ApplicantTaggingListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["ApplicantTaggingListFilterOption"]));
                    }
                    objApplicantTaggingListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objApplicantTaggingListJS.ShowHideFilter();
                        localStorage["ApplicantTaggingListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblApplicantTaggingList");
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
            localStorage["ApplicantTaggingID"] = $("#txtFilterApplicantID").val();

            localStorage["ApplicantTaggingID"] = $("#txtFilterApplicantID").val();
            localStorage["ApplicantTaggingApplicantName"] = $("#txtFilterApplicantName").val();
            localStorage["ApplicantTaggingApplicationSourceDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").value;
            localStorage["ApplicantTaggingApplicationSourceDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").text;

            localStorage["ApplicantTaggingPositionRemarks"] = $("#txtFilterPositionRemarks").val();
            //localStorage["ApplicantTaggingDesiredPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").value;
            //localStorage["ApplicantTaggingDesiredPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").text;

            //localStorage["ApplicantTaggingOrgGroupRemarks"] = $("#txtFilterOrgGroupRemarks").val();
            //localStorage["ApplicantTaggingDesiredOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").value;
            //localStorage["ApplicantTaggingDesiredOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").text;

            //localStorage["ApplicantTaggingReferredByRemarks"] = $("#txtFilterReferredByRemarks").val();
            localStorage["ApplicantTaggingReferredBy"] = objEMSCommonJS.GetMultiSelectList("multiSelectedReferredBy").value;
            localStorage["ApplicantTaggingReferredByText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedReferredBy").text;


        },

        GetLocalStorage: function () {
            //objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
            //    , "RecruitmentApplicantListOrgGroupDelimited"
            //    , "RecruitmentApplicantListOrgGroupDelimitedText");

            $("#txtFilterApplicantID").val(localStorage["ApplicantListID"]);
            $("#txtFilterApplicantName").val(localStorage["ApplicantListApplicantName"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedApplicationSource"
                , "ApplicantListApplicationSourceDelimited"
                , "ApplicantListApplicationSourceDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStep"
                , "ApplicantListCurrentStepDelimited"
                , "ApplicantListCurrentStepDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedWorkflow"
                , "ApplicantListWorkflowDelimited"
                , "ApplicantListWorkflowDelimitedText");

            //$("#txtFilterOrgGroupRemarks").val(localStorage["ApplicantListOrgGroupRemarks"]);
            //objEMSCommonJS.SetMultiSelectList("multiSelectedDesiredOrgGroup"
            //    , "ApplicantListDesiredOrgGroupDelimited"
            //    , "ApplicantListDesiredOrgGroupDelimitedText");

            $("#txtFilterPositionRemarks").val(localStorage["ApplicantListPositionRemarks"]);
            //objEMSCommonJS.SetMultiSelectList("multiSelectedDesiredPosition"
            //    , "ApplicantListDesiredPositionDelimited"
            //    , "ApplicantListDesiredPositionDelimitedText");

            $("#txtFilterCourse").val(localStorage["ApplicantListCourse"]);
            $("#txtFilterCurrentPositionTitle").val(localStorage["ApplicantListCurrentPositionTitle"]);
            $("#txtFilterSalaryFrom").val(localStorage["ApplicantListExpectedSalaryFrom"]);
            $("#txtFilterSalaryTo").val(localStorage["ApplicantListExpectedSalaryTo"]);
        },

        ViewLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"objApplicantTaggingListJS.LoadApplicant('" + rowObject.ID + "'); return false;\">" + rowObject.ID + "</a>";
        },

        TagLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"objApplicantTaggingListJS.LoadTaggingModal('" + rowObject.ID + "'); return false;\">Tag</a>";
        },

        LoadApplicant: function (id) {
            var isSuccessFunction = function () {
                $("#divApplicantModal #btnDelete, #divApplicantModal #btnEdit").remove();
            };
            LoadPartialSuccessFunction(ApplicantViewURL + "?ID=" + id, "divApplicantBodyModal", isSuccessFunction);
            $('#divApplicantModal').modal('show');
        }, 

        LoadTaggingModal: function (id) {
            LoadPartial(ApplicantTaggingEditURL + "?ID=" + id, "divApplicantTaggingBodyModal");
            $('#divApplicantTaggingModal').modal('show');
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

    objApplicantTaggingListJS.Initialize();
});