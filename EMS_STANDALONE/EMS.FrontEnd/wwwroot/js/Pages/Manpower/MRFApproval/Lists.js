var objMRFApprovalListJS;
const MRFApprovalListURL = "/Manpower/MRFApproval?handler=List";
const PositionLevelAutoCompleteURL = "/Manpower/MRFApproval?handler=PositionLevelAutoComplete";
const OrgGroupAutoCompleteURL = "/Manpower/MRFApproval?handler=OrgTypeAutoComplete";
const PositionAutoCompleteURL = "/Manpower/MRFApproval?handler=PositionAutoComplete";
const GetNatureOfEmploymentURL = "/Manpower/MRFApproval?handler=ReferenceValue&RefCode=NATURE_OF_EMPLOYMENT";
const GetStatusURL = "/Manpower/MRFApproval?handler=ReferenceValue&RefCode=MRF_APPROVER_STATUS";
const GetPositionLevelDropDownByOrgGroupIDURL = "/Manpower/MRFApproval/Edit?handler=PositionLevelDropDownByOrgGroupID";
const GetPositionDropDownURL = "/Manpower/MRFApproval/Edit?handler=PositionDropDown";
const GetSignatoriesURL = "/Manpower/MRFApproval/Edit?handler=Signatories";
const GetRegionByIDURL = "/Manpower/MRFApproval/Edit?handler=RegionByID";
const GetApprovalHistoryURL = "/Manpower/MRFApproval?handler=MRFApprovalHistory";

const MRFApprovalEditURL = "/ManPower/MRFApproval/Edit";
const MRFApprovalEditPostURL = "/ManPower/MRFApproval/Edit?handler=Submit";

const GetCommentsURL = "/Manpower/MRFApproval/Edit?handler=MRFComments";
const SaveCommentsURL = "/Manpower/MRFApproval/Edit?handler=SaveComments";

$(document).ready(function () {
    objMRFApprovalListJS = {

        Initialize: function () {
            var s = this;
            $("#txtFilterDateCreatedFrom, #txtFilterDateCreatedTo, "
                + "#txtFilterDateApprovedFrom, #txtFilterDateApprovedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            s.ElementBinding();
            var param = {
                ID: localStorage["ManpowerMRFApprovalListID"],
                MRFTransactionID: localStorage["ManpowerMRFApprovalListMRFTransactionID"],
                OrgGroupDelimited: localStorage["ManpowerMRFApprovalListOrgGroupDelimited"],
                PositionLevelDelimited: localStorage["ManpowerMRFApprovalListPositionLevelDelimited"],
                PositionDelimited: localStorage["ManpowerMRFApprovalListPositionDelimited"],
                NatureOfEmploymentDelimited: localStorage["ManpowerMRFApprovalListNatureOfEmploymentDelimited"],
                NoOfApplicantMin: localStorage["ManpowerMRFApprovalListNoOfApplicantMin"],
                NoOfApplicantMax: localStorage["ManpowerMRFApprovalListNoOfApplicantMax"],
                StatusDelimited: localStorage["ManpowerMRFApprovalListStatusDelimited"],
                DateCreatedFrom: localStorage["ManpowerMRFApprovalListDateCreatedFrom"],
                DateCreatedTo: localStorage["ManpowerMRFApprovalListDateCreatedTo"],
                DateApprovedFrom: localStorage["ManpowerMRFApprovalListDateApprovedFrom"],
                DateApprovedTo: localStorage["ManpowerMRFApprovalListDateApprovedTo"],
                AgeMin: localStorage["ManpowerMRFApprovalListAgeMin"],
                AgeMax: localStorage["ManpowerMRFApprovalListAgeMax"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();


        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterMRFID"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterMRFID").val(),
                    MRFTransactionID: $("#txtFilterMRFTransactionID").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    PositionLevelDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPositionLevel").value,
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    NatureOfEmploymentDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedNatureOfEmployment").value,
                    NoOfApplicantMin: $("#txtFilterNoOfApplicantMin").val(),
                    NoOfApplicantMax: $("#txtFilterNoOfApplicantMax").val(),
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value,
                    DateCreatedFrom: $("#txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#txtFilterDateCreatedTo").val(),
                    DateApprovedFrom: $("#txtFilterDateApprovedFrom").val(),
                    DateApprovedTo: $("#txtFilterDateApprovedTo").val(),
                    AgeMin: $("#txtFilterAgeMin").val(),
                    AgeMax: $("#txtFilterAgeMax").val(),
                };
                s.SetLocalStorage();
                ResetJQGridState("tblManpowerMRFApprovalList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedPositionLevel").html("");
                $("#multiSelectedPosition").html("");
                $("#multiSelectedNatureOfEmployment").html("");
                $("#multiSelectedNatureOfEmploymentOption label, #multiSelectedNatureOfEmploymentOption input").prop("title", "add");
                $("#multiSelectedStatus").html("");
                $("#multiSelectedStatusOption label, #multiSelectedStatusOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPositionLevel"
                , PositionLevelAutoCompleteURL, 20, "multiSelectedPositionLevel");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , PositionAutoCompleteURL, 20, "multiSelectedPosition");

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedNatureOfEmployment", GetNatureOfEmploymentURL);

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedStatus", GetStatusURL);

        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblManpowerMRFApprovalList") == "" ? "" : $.parseJSON(localStorage.getItem("tblManpowerMRFApprovalList"));

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
            $("#tblManpowerMRFApprovalList").jqGrid("GridUnload");
            $("#tblManpowerMRFApprovalList").jqGrid("GridDestroy");
            $("#tblManpowerMRFApprovalList").jqGrid({
                url: MRFApprovalListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "MRF ID", "Organizational Group", "Position Level", "Position", "Nature of Employment", "No. of Applicant", "Status", "Date Created", "Date Approved", "Age"],
                colModel: [
                    { hidden: true, width: 25, key: true, name: "ID", index: "ID", align: "center", sortable: true },
                    { name: "MRFID", index: "MRFID", align: "center", sortable: true, formatter: objMRFApprovalListJS.AddLink },
                    { name: "OrgGroupDescription", index: "OrgGroupDescription", editable: true, align: "left", sortable: true },
                    { name: "PositionLevelDescription", index: "PositionLevelDescription", editable: true, align: "left", sortable: true },
                    { name: "PositionDescription", index: "PositionDescription", editable: true, align: "left", sortable: true },
                    { name: "NatureOfEmployment", index: "NatureOfEmployment", editable: true, align: "left", sortable: true },
                    { name: "NoOfApplicant", index: "NoOfApplicant", editable: true, align: "center", sortable: true },
                    { name: "Status", index: "Status", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                    { name: "ApprovedDate", index: "ApprovedDate", editable: true, align: "left", sortable: true },
                    { name: "Age", index: "Age", editable: true, align: "center", sortable: true },
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
                    $("#tblManpowerMRFApprovalList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
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
                        AutoSizeColumnJQGrid("tblManpowerMRFApprovalList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo(".jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblManpowerMRFApprovalList .jqgrid-id-link").click(function () {
                            $('#divMRFApprovalModal').modal('show');
                        });

                    }

                    if (localStorage["MRFApprovalListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["MRFApprovalListFilterOption"]));
                    }
                    objMRFApprovalListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objMRFApprovalListJS.ShowHideFilter();
                        localStorage["MRFApprovalListFilterOption"] = $("#chkFilter").is(":checked");
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
                    GetJQGridState("tblManpowerMRFApprovalList");
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
            localStorage["ManpowerMRFApprovalListID"] = $("#txtFilterMRFID").val();
            localStorage["ManpowerMRFApprovalListMRFTransactionID"] = $("#txtFilterMRFTransactionID").val();

            localStorage["ManpowerMRFApprovalListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["ManpowerMRFApprovalListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;

            localStorage["ManpowerMRFApprovalListPositionLevelDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPositionLevel").value;
            localStorage["ManpowerMRFApprovalListPositionLevelDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPositionLevel").text;

            localStorage["ManpowerMRFApprovalListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["ManpowerMRFApprovalListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;

            localStorage["ManpowerMRFApprovalListNatureOfEmploymentDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedNatureOfEmployment").value;
            localStorage["ManpowerMRFApprovalListNatureOfEmploymentDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedNatureOfEmployment").text;

            localStorage["ManpowerMRFApprovalListNoOfApplicantMin"] = $("#txtFilterNoOfApplicantMin").val();
            localStorage["ManpowerMRFApprovalListNoOfApplicantMax"] = $("#txtFilterNoOfApplicantMax").val();

            localStorage["ManpowerMRFApprovalListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").value;
            localStorage["ManpowerMRFApprovalListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedStatus").text;

            localStorage["ManpowerMRFApprovalListDateCreatedFrom"] = $("#txtFilterDateCreatedFrom").val();
            localStorage["ManpowerMRFApprovalListDateCreatedTo"] = $("#txtFilterDateCreatedTo").val();
            localStorage["ManpowerMRFApprovalListDateApprovedFrom"] = $("#txtFilterDateApprovedFrom").val();
            localStorage["ManpowerMRFApprovalListDateApprovedTo"] = $("#txtFilterDateApprovedTo").val();
            localStorage["ManpowerMRFApprovalListAgeMin"] = $("#txtFilterAgeMin").val();
            localStorage["ManpowerMRFApprovalListAgeMax"] = $("#txtFilterAgeMax").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterMRFID").val(localStorage["ManpowerMRFApprovalListID"]);
            $("#txtFilterMRFTransactionID").val(localStorage["ManpowerMRFApprovalListMRFTransactionID"]);
            
            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "ManpowerMRFApprovalListOrgGroupDelimited"
                , "ManpowerMRFApprovalListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPositionLevel"
                , "ManpowerMRFApprovalListPositionLevelDelimited"
                , "ManpowerMRFApprovalListPositionLevelDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "ManpowerMRFApprovalListPositionDelimited"
                , "ManpowerMRFApprovalListPositionDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedNatureOfEmployment"
                , "ManpowerMRFApprovalListNatureOfEmploymentDelimited"
                , "ManpowerMRFApprovalListNatureOfEmploymentDelimitedText");

            $("#txtFilterNoOfApplicantMin").val(localStorage["ManpowerMRFApprovalListNoOfApplicantMin"]);
            $("#txtFilterNoOfApplicantMax").val(localStorage["ManpowerMRFApprovalListNoOfApplicantMax"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedStatus"
                , "ManpowerMRFApprovalListStatusDelimited"
                , "ManpowerMRFApprovalListStatusDelimitedText");

            $("#txtFilterDateCreatedFrom").val(localStorage["ManpowerMRFApprovalListDateCreatedFrom"]);
            $("#txtFilterDateCreatedTo").val(localStorage["ManpowerMRFApprovalListDateCreatedTo"]);
            $("#txtFilterDateApprovedFrom").val(localStorage["ManpowerMRFApprovalListDateApprovedFrom"]);
            $("#txtFilterDateApprovedTo").val(localStorage["ManpowerMRFApprovalListDateApprovedTo"]);
            $("#txtFilterAgeMin").val(localStorage["ManpowerMRFApprovalListAgeMin"]);
            $("#txtFilterAgeMax").val(localStorage["ManpowerMRFApprovalListAgeMax"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + MRFApprovalEditURL + "?ID=" + rowObject.ID + "&RecordStatus=" + rowObject.Status + "', 'divMRFApprovalBodyModal');\">" + rowObject.MRFID + "</a>";
        },

        LoadApprovalHistoryJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblMRFFormApprovalHistoryList").jqGrid("GridUnload");
            $("#tblMRFFormApprovalHistoryList").jqGrid("GridDestroy");
            $("#tblMRFFormApprovalHistoryList").jqGrid({
                url: GetApprovalHistoryURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10,
                datatype: "json",
                mtype: "GET",
                colNames: ["Level", "Approver Name", "Status", "Remarks", "Timestamp", "", "", ""],
                colModel: [
                    { name: "HierarchyLevel", index: "HierarchyLevel", align: "left", sortable: false },
                    { name: "ApproverName", index: "ApproverName", align: "left", sortable: false },
                    { name: "ApprovalStatus", index: "ApprovalStatus", align: "left", sortable: false },
                    { name: "ApprovalRemarks", index: "ApprovalRemarks", align: "left", sortable: false },
                    { name: "ApprovedDate", index: "ApprovedDate", align: "left", sortable: false },
                    { hidden: true, name: "PositionID", index: "PositionID", align: "left", sortable: false },
                    { hidden: true, name: "OrgGroupID", index: "OrgGroupID", align: "left", sortable: false },
                    { hidden: true, name: "ApprovalStatusCode", index: "ApprovalStatusCode", align: "left", sortable: false },
                ],
                //toppager: $("#divMRFFormApprovalHistoryPager"),   
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
                                //if (data.rows[i].ApprovalStatusCode == "FOR_APPROVAL" ||
                                //    (
                                //        // Revised MRF
                                //        data.rows[i].ApprovalStatusCode == "REJECTED" &
                                //        $("#hdnStatus").val() == "FOR APPROVAL"
                                //    )
                                //) {
                                //    // Get next approvers position and org group
                                //    $("#hdnNextApproverPositionID").val(data.rows[i + 1] == undefined ? "" : data.rows[i + 1].PositionID);
                                //    $("#hdnNextApproverOrgGroupID").val(data.rows[i + 1] == undefined ? "" : data.rows[i + 1].OrgGroupID);
                                //    // Get Current level of approval
                                //    $("#hdnLevelOfApproval").val(data.rows[i].HierarchyLevel);
                                //    return false;
                                //}
                                //else if (
                                    
                                //) {
                                //    $("#hdnNextApproverPositionID").val(data.rows[i] == undefined ? "" : data.rows[i].PositionID);
                                //    $("#hdnNextApproverOrgGroupID").val(data.rows[i] == undefined ? "" : data.rows[i].OrgGroupID);
                                //    // Get Current level of approval
                                //    $("#hdnLevelOfApproval").val(data.rows[i].HierarchyLevel - 0);
                                //}
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblMRFFormApprovalHistoryList", data);

                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        GetComment: function () {
            var input = {
                MRFID: $("#hdnID").val()
            };

            $("#txtAreaComments").val("");

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    $("#divCommentsContainer").html("");
                    if (data.Result.length > 0) {
                        $("#divCommentsContainer").append("<span class='comment-details'>-- Start --</span><br>");
                        $(data.Result).each(function (index, item) {
                            $("#divCommentsContainer").append("<span class='comment-details'>" + item.Timestamp
                                + " " + item.Sender + ": </span><span class='comment'> " + item.Comments + "</span><br>");
                        });
                        if (data.Result.length <= (index + 1)) {
                            setTimeout(function () { $('#divCommentsContainer').scrollTop($('#divCommentsContainer')[0].scrollHeight) }, 300);
                        }
                    }
                    else {
                        $("#divCommentsContainer").append("<span class='comment-details'>-- No comments found. --</span><br>");
                    }
                    $("#txtAreaComments").attr("readonly", false);
                    $("#txtAreaComments").prop("disabled", false);
                }
            };

            objEMSCommonJS.GetAjax(GetCommentsURL, input, "", GetSuccessFunction);
        },

        GetComment: function () {
            var input = {
                MRFID: $("#hdnID").val()
            };

            $("#txtAreaComments").val("");

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    $("#divCommentsContainer").html("");
                    if (data.Result.length > 0) {
                        $("#divCommentsContainer").append("<span class='comment-header'>-- Start --</span>");
                        $(data.Result).each(function (index, item) {
                            $("#divCommentsContainer").append("<span class='comment-details'>" + item.Timestamp
                                + " " + item.Sender + ": </span><span class='comment'> " + item.Comments + "</span><br>");
                        });
                        $('#divCommentsContainer').scrollTop($('#divCommentsContainer')[0].scrollHeight);
                    }
                    else {
                        $("#divCommentsContainer").append("<span class='comment-header'>-- No comments found. --</span>");
                    }
                    $("#txtAreaComments").attr("readonly", false);
                    $("#txtAreaComments").prop("disabled", false);
                }
            };

            objEMSCommonJS.GetAjax(GetCommentsURL, input, "", GetSuccessFunction);
        },

        GetCommentSectionFormData: function () {
            var formData = new FormData($('#frmMRFApproval').get(0));
            formData.append("CommentsForm.MRFID", $("#hdnID").val());
            formData.append("CommentsForm.Comments", $("#txtAreaComments").val());
            return formData;
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

    objMRFApprovalListJS.Initialize();
});