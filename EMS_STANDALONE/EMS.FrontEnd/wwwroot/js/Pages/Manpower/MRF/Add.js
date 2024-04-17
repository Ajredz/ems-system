var objMRFAddJS;

$(document).ready(function () {
    objMRFAddJS = {

        Initialize: function () {
            $("#divMRFBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divMRFModal #btnSave").show();
            $("#divMRFModal .form-control").attr("readonly", false);
            $("#txtVacancy").attr("readonly", true);
            $("#divMRFModal #btnDelete, #divMRFModal #btnBack").remove();
            $("#ddlOrgGroup").change();
            //$(".tablinks:nth-child(3)").remove();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApprovalHistory');

            // Temporarily disable other options, in preparation for next Phase
            //#ddlNatureOfEmployment option[value='PROJ_BASED']
            $("#ddlNatureOfEmployment option[value='PART_TIME']").prop('disabled', true);

            $(".tablinks:nth-child(2)").hide();

            $("#divMRFModal .tablinks").find("span:contains('Online Job Post')").parent("button").hide();

            $('#SummerNoteJobDescription').summernote({
                toolbar: [
                    // [groupName, [list of button]]
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'style', 'paragraph']],
                    ['height', ['height']]
                ]
            });
            $('#SummerNoteJobQualification').summernote({
                toolbar: [
                    // [groupName, [list of button]]
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph', 'style']],
                    ['height', ['height']]
                ]
            });
        },

        AddSuccessFunction: function () {
            $("#tblMRFFormApprovalHistoryList").jqGrid("GridUnload");
            $("#tblMRFFormApprovalHistoryList").jqGrid("GridDestroy");
            $("#divMRFFilter #btnSearch").click();
            $("#frmMRF").trigger("reset");
            $("#divSignatories").html("");
            $("#divManpowerCountDetails").html("");
            $(".tablinks:nth-child(2)").hide();
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApprovalHistory');
            $("#txtJobPosition").val("");
            $("#txtJobLocation").val("");
            $("#JobDescription").summernote("reset");
            $("#JobQualification").summernote("reset");
        },

        GetOnlineJobPost: function (data) {
            $('#SummerNoteJobDescription').summernote('reset');
            $('#SummerNoteJobQualification').summernote('reset');

            $("#txtJobPosition").val(data.Result["OnlinePosition"]);
            $("#txtJobLocation").val(data.Result["OnlineLocation"]);
            $('#SummerNoteJobDescription').summernote('code', data.Result["OnlineJobDescription"]);
            $('#SummerNoteJobQualification').summernote('code', data.Result["OnlineJobQualification"]);
        },

        AddSignatoriesDynamicFields: function (data) {
            $("#divSignatories").html("");
            $(data.Result).each(function (index, item) {
                var fontColor = item.ApprovalStatusCode == "PENDING" ?
                    "#FF6D0D" :
                    item.ApprovalStatusCode == "FOR_APPROVAL" ?
                        "#4949FF" :
                        item.ApprovalStatusCode == "APPROVED" ?
                            "#029902" :
                            item.ApprovalStatusCode == "REJECTED" ?
                                "#FF0000" : "";

                $("#divSignatories").append(
                    "<div class=\"form-group form-fields\">"
                    + " 	<div class=\"col-md-1-5 col-label\">"
                    + " 		<label class=\"control-label block-label\"> Approver <span class=\"unreqField\">* </span>" + item.HierarchyLevel + "</label>"
                    + " 	</div>"
                    + " 	<div class=\"col-md-1-5 col-label\">"
                    + " 		<label class=\"control-label block-label\" style=\"color:" + fontColor + "\">" + item.ApprovalStatus + "</label>"
                    + " 	</div>"
                    //+ " 	<div class=\"col-md-3 col-label\">"
                    //+ " 		<label class=\"control-label block-label\">" + item.ApprovedDate + "</label>"
                    //+ " 	</div>"
                    + " 	<div class=\"col-md-5 col-label\">"
                    + " 		<label class=\"control-label block-label\">" + ((item.ApproverName || "") == "" ? "" : "Approver: " + item.ApproverName + " " + item.ApprovedDate) + "</label>"
                    + " 	</div>"
                    + " </div>"
                );
            });
        },

        ElementBinding: function () {
            var s = this;
            NumberOnly($("#divMRFModal #txtTurnaroundTime"));

            $("#txtJobPosition").keyup(function () {
                /*function toTitleCase(str) {
                    return str.replace(/(?:^|\s)\w/g, function (match) {
                        return match.toUpperCase();
                    });
                }*/
                $("#txtJobPosition").val($("#txtJobPosition").val().toUpperCase());
            });
            $("#txtJobLocation").keyup(function () {
                /*function toTitleCase(str) {
                    return str.replace(/(?:^|\s)\w/g, function (match) {
                        return match.toUpperCase();
                    });
                }*/
                $("#txtJobLocation").val($("#txtJobLocation").val().toUpperCase());
            });

            $("#ddlOrgGroup").change(function () {
                if ($("#ddlOrgGroup").val() != "") {
                    GenerateDropdownValues(GetPositionDropDownWithCountURL + "&OrgGroupID=" + $("#ddlOrgGroup").val(),
                        "ddlPosition", "Value", "Text", "", "", false);

                    $("input[name='MRF.OrgGroupID']").val($("#ddlOrgGroup").val());

                    var GetSuccessFunction = function (data) {
                        if (data.Result == "BRN") {
                            $("#cbIsAvailableOnline").prop("checked", true);
                            $("#tabJobAppLink").show();
                        }
                        else {
                            $("#cbIsAvailableOnline").prop("checked", false);
                            $("#tabJobAppLink").hide();
                        }
                    };
                    objEMSCommonJS.GetAjaxNoLoading(GetGetOrgGroupTypeURL + "&OrgGroupID=" + $("#ddlOrgGroup").val(), "", "", GetSuccessFunction);
                }

                $("#divMRFErrorMessage").html("");
                $("#ddlPosition").removeClass("errMessage");
                $("#ddlPosition").val("");
                $("#divManpowerCountDetails").html("");
            });

            //$("#ddlPositionLevel").change(function () {
            //    GenerateDropdownValues(GetPositionDropDownURL +
            //        "&OrgGroupID=" + $("#ddlOrgGroup").val() +
            //        "&PositionLevelID=" + $("#ddlPositionLevel").val(),
            //        "ddlPosition", "Value", "Text", "", "", false);

            //    $("#divMRFErrorMessage").html("");
            //    $("#ddlPosition").removeClass("errMessage");
            //    $("#divManpowerCountDetails").html("");
            //});

            $("#ddlPosition").change(function () {
                if ($("#ddlPosition").val() != "" & $("#ddlOrgGroup").val() != "") {
                    var GetSuccessFunction = function (data) {
                        //s.AddSignatoriesDynamicFields(data);
                        objMRFListJS.LoadApprovalHistoryJQGrid({ RequestingPositionID: $("#ddlPosition").val(), RequestingOrgGroupID: $("#ddlOrgGroup").val()});
                    };
                    objEMSCommonJS.GetAjax(GetApprovalHistoryURL + "&RequestingPositionID=" + $("#ddlPosition").val() + "&RequestingOrgGroupID="
                        + $("#ddlOrgGroup").val(), {}, "", GetSuccessFunction);
                    $("input[name='MRF.PositionID']").val($("#ddlPosition").val());


                    var GetSuccessFunction = function (data) {
                        if (data.IsSuccess) {
                            $("#divMRFErrorMessage").html("");
                            $("#ddlPosition").removeClass("errMessage");
                        }
                        else {
                            $("#divMRFErrorMessage").html("<label class=\"errMessage\"><li>" + data.Result.MRFMessage + "</li></label><br />");
                            $("#ddlPosition").addClass("errMessage");
                        }
                      //$("#divManpowerCountDetails").html("");
                      //$("#divManpowerCountDetails").append(
                      //    "<div class=\"control-label block-label\">"
                      //  + "<label>Planned: </label>"
                      //  + "<label>" + data.Result.PlannedCount + "</label> |"
                      //  + "<label>Active: </label>"
                      //  + "<label>" + data.Result.ActiveCount + "</label> |"
                      //  + "<label>Inactive: </label>"
                      //  + "<label>" + data.Result.InactiveCount + "</label> |"
                      //  + "<label>Variance: </label>"
                      //  + "<label>" + data.Result.Variance + "</label>"
                      //  + "</div>"
                      //);
                    };

                    objEMSCommonJS.GetAjax(ValidateExistingActualURL
                        + "&OrgGroupID=" + $("#ddlOrgGroup").val()
                        + "&PositionID=" + $("#ddlPosition").val()
                        , {}
                        , ""
                        , GetSuccessFunction, null, true);

                    objEMSCommonJS.GetAjax(GetOnlineJobPositionURL
                        + "&ID=" + $("#ddlPosition").val()
                        , {}
                        , ""
                        , objMRFAddJS.GetOnlineJobPost);

                }
                else {
                    $("#divManpowerCountDetails").html("");
                    $("#divSignatories").html("");
                }

            });

            $("#divMRFModal #btnSave").click(function () {
                var isNoBlankFunction = function () {

                    if ($("#txtturnaroundtime").val() == "0") {
                        $("#txtturnaroundtime").addclass("errmessage");
                        $("#txtturnaroundtime").focus();
                        $("#frmmrf #divmrferrormessage").html(
                            "<label class=\"errmessage\"><li>" + greaterthan_zero.replace("{0}", $("#txtturnaroundtime")[0].title) + "</li></label><br />");

                    }
                    else
                        return true;
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmMRF", "#divMRFErrorMessage", isNoBlankFunction)) {

                    var GetSuccessFunction = function (data) {
                        if (data.IsSuccess) {
                            $("#divMRFErrorMessage").html("");
                            $("#ddlPosition").removeClass("errMessage");
                            ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                                "objEMSCommonJS.PostAjax(true \
                        , MRFAddPostURL \
                        , new FormData($('#frmMRF').get(0)) \
                        , '#divMRFErrorMessage' \
                        , '#divMRFModal #btnSave' \
                        , objMRFAddJS.AddSuccessFunction); ",
                                "function");
                        }
                        else {
                            $("#divMRFErrorMessage").html("<label class=\"errMessage\"><li>" + data.Result.MRFMessage + "</li></label><br />");
                            $("#ddlPosition").addClass("errMessage");
                        }

                    };

                    objEMSCommonJS.GetAjax(ValidateExistingActualURL
                        + "&OrgGroupID=" + $("#ddlOrgGroup").val()
                        + "&PositionID=" + $("#ddlPosition").val()
                        , {}
                        , ""
                        , GetSuccessFunction);
                }
            });
            $("#cbIsAvailableOnline").change(function () {
                if ($("#cbIsAvailableOnline").prop("checked") == true) {
                    $("#tabJobAppLink").show();
                    if ($("#ddlPosition :selected").val() != "") {
                        objEMSCommonJS.GetAjax(GetOnlineJobPositionURL
                            + "&ID=" + $("#ddlPosition").val()
                            , {}
                            , ""
                            , objMRFAddJS.GetOnlineJobPost);
                    }
                    $(".addReq").addClass("required-field");
                }
                else {
                    $("#tabJobAppLink").hide();
                    $(".addReq").removeClass("required-field");
                }
            });
        },

    };
    
     objMRFAddJS.Initialize();
});