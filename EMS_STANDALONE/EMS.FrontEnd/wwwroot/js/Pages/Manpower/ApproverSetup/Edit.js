var objApproverSetupEditJS;
var SaveApproverSetupURL = "/Manpower/ApproverSetup/Edit";
var GetPositionListURL = "/Manpower/ApproverSetup/Edit?handler=PositionList";
var GetPositionAutoCompleteURL = "/Manpower/ApproverSetup/Edit?handler=PositionAutoComplete";
var GetOrgGroupAutoCompleteURL = "/Manpower/ApproverSetup/Edit?handler=OrgGroupAutoComplete";
var uniquePositionsList = [];

$(document).ready(function () {
    objApproverSetupEditJS = {
        
        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divApproverSetupBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            s.LoadPosition();
            $("#btnSave").show();    
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSave").click(function () {
                var incompleteCount = 0;
                var greaterThanZeroCount = 0;
                $(" .errMessage").removeClass("errMessage");
                $("#divApproverSetupErrorMessage").html("");

                $(uniquePositionsList).each(function (index, item) {
                    $($(".ApproversDynamicFields_" + item.ID)).each(function (index1, item1) {
                        if (
                            $("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).val() != "" &
                            (
                                $("#txtLevel_" + item.ID + "_" + (index1 + 1)).val() == "" ||
                                $("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() == ""
                            )
                        ) {
                            incompleteCount++;
                            if ($("#txtLevel_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtLevel_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            if ($("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                        }
                        else if (
                            $("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() != "" &
                            (
                                $("#txtLevel_" + item.ID + "_" + (index1 + 1)).val() == "" ||
                                $("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).val() == ""
                            )
                        ) {
                            incompleteCount++;
                            if ($("#txtLevel_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtLevel_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            if ($("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                        }
                        else if (
                            $("#txtAltApproverPosition_" + item.ID + "_" + (index1 + 1)).val() != "" &
                            (
                                $("#txtLevel_" + item.ID + "_" + (index1 + 1)).val() == "" ||
                                $("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).val() == "" ||
                                $("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() == "" ||
                                $("#txtAltApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() == ""
                            )
                        ) {
                            incompleteCount++;
                            if ($("#txtLevel_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtLevel_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            if ($("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            if ($("#txtAltApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtAltApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            if ($("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                        }
                        else if (
                            $("#txtAltApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() != "" &
                            (
                                $("#txtLevel_" + item.ID + "_" + (index1 + 1)).val() == "" ||
                                $("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).val() == "" ||
                                $("#txtAltApproverPosition_" + item.ID + "_" + (index1 + 1)).val() == "" ||
                                $("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() == ""
                            )
                        ) {
                            incompleteCount++;
                            if ($("#txtLevel_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtLevel_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            if ($("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtApproverPosition_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            if ($("#txtAltApproverPosition_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtAltApproverPosition_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            if ($("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).val() == "")
                                $("#txtApproverOrgGroup_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                        }

                        if (parseInt($("#txtLevel_" + item.ID + "_" + (index1 + 1)).val()) <= 0) {
                            $("#txtLevel_" + item.ID + "_" + (index1 + 1)).addClass("errMessage");
                            $("#divApproverSetupErrorMessage").html("<label class=\"errMessage\"><li>" + GREATERTHAN_ZERO.replace("{0}", "Level") + "</li></label><br />");
                            greaterThanZeroCount++;
                        }

                        });
                });

                if (incompleteCount > 0) {
                    $("#divApproverSetupErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                }
                else if (greaterThanZeroCount > 0) {
                    
                }
                else {
                   
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , SaveApproverSetupURL \
                        , objApproverSetupEditJS.GetFormData() \
                        , '#divApproverSetupErrorMessage' \
                        , '#btnSave' \
                        , objApproverSetupEditJS.EditSuccessFunction);", "function");
                }
            });

        },

        GetFormData: function () {

            var formData = new FormData($('#frmApproverSetup').get(0));
            var ctr = 0;
            $(uniquePositionsList).each(function (index, item) {
                $(".ApproversDynamicFields_" + item.ID).each(function (index1, item1) {
                    var c = index1 + 1;
                    if (($("#txtApproverPosition_" + item.ID + "_" + c).val() || "" ) != "" &&
                        ($("#txtApproverOrgGroup_" + item.ID + "_" + c).val() || "" ) != ""
                        ) {
                        formData.append("ApproverSetup.Approvers[" + ctr + "].PositionID", $("#hdnPosition_" + item.ID).val());
                        formData.append("ApproverSetup.Approvers[" + ctr + "].HierarchyLevel", $("#txtLevel_" + item.ID + "_" + c).val());
                        formData.append("ApproverSetup.Approvers[" + ctr + "].ApproverPositionID", $("#hdnApproverPosition_" + item.ID + "_" + c).val());
                        formData.append("ApproverSetup.Approvers[" + ctr + "].ApproverOrgGroupID", $("#hdnApproverOrgGroup_" + item.ID + "_" + c).val());
                        formData.append("ApproverSetup.Approvers[" + ctr + "].AltApproverPositionID", $("#hdnAltApproverPosition_" + item.ID + "_" + c).val());
                        formData.append("ApproverSetup.Approvers[" + ctr + "].AltApproverOrgGroupID", $("#hdnAltApproverOrgGroup_" + item.ID + "_" + c).val());
                        ctr++;
                    }
                });

            });

            return formData;
        },

        EditSuccessFunction: function () {
            //$("#tblMRFFormApprovalHistoryList").jqGrid("GridUnload");
            //$("#tblMRFFormApprovalHistoryList").jqGrid("GridDestroy");
            //$("#txtApprovalRemarks").val("");
            $("#btnSearch").click();
            $("#divApproverSetupModal .close").click();
            //LoadPartial("" + MRFApprovalEditURL + "?ID=" + objApproverSetupEditJS.ID + "&RecordStatus=" + $("#lblMRFStatus").text() + ""
            //    , "divMRFApprovalBodyModal");
            //$("#divMRFApprovalModal .close").click();
        },

        AddApproverDynamicFields: function (data) {
            var htmlId = $(".ChildrenManPowerDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenManPowerDynamicFields_", "")) + 1;
            //var PositionId = $('#ddlPosition_' + ctr).val();

            $("#DivChildrenManPowerDynamicFields").append(
                "<div class=\"form-group form-fields ChildrenManPowerDynamicFields\" id=\"ChildrenManPowerDynamicFields_" + ctr + "\"> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <select id=\"ddlPositionLevel_" + ctr + "\" title=\"Position Level\" class=\"form-control PositionLevelDynamicFields\"></select> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <select id=\"ddlPosition_" + ctr + "\" title=\"Position\" class=\"form-control PositionDynamicFields\"><option value = \"\">- Select an item -</option></select> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtPlanned_" + ctr + "\" class=\"form-control text-amount PlannedDynamicFields\" title=\"Planned\"> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtActive_" + ctr + "\" class=\"form-control text-amount ActiveDynamicFields\" title=\"Active\" readonly> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtInactive_" + ctr + "\" class=\"form-control text-amount InactiveDynamicFields\" title=\"Inactive\" readonly> \
                   </div> \
                   <div class=\"col-md-0-75 text-align-right no-padding\"> \
                       <label id=\"txtVariance_" + ctr + "\" class=\"control-label block-label remarks-label text-amount total-label VarianceDynamicFields\"></label> \
                   </div> \
                   <div class=\"col-md-0-5 text-align-center no-padding\"> \
                   </div> \
               </div>"
            );
            //<span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
            //onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objApproverSetupEditJS.RemoveDynamicFields('#ChildrenManPowerDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
        },

        LoadPosition: function () {
            var s = this;
            $("#DivApproversDynamicFields").html("");

            var input = { ID: objApproverSetupEditJS.ID };
            var GetSuccessFunction = function (data) {
                $("#txtOrgGroup").val(data.Result.OrgGroup);
                var addDynamicFields = function (item, c) {
                    $("#DivApproversDynamicFields").append(
                        "<div class=\"form-group form-fields ApproversDynamicFields_" + item.ID + "\" id=\"ApproversDynamicFields_" + item.ID + "_" + c + "\"> \
                            <div class=\"col-md-2-5 text-align-center no-padding\"> \
                            </div> \
                            <div class=\"col-md-0-75 text-align-center no-padding\"> \
                                <input type=\"number\" id=\"txtLevel_" + item.ID + "_" + c + "\" class=\"required-field form-control LevelDynamicFields_" + item.ID + "\" title=\"Level\" value=\"" + c + "\" readonly> \
                           </div> \
                            <div class=\"col-md-2 text-align-center no-padding\"> \
                                <input type=\"search\" id=\"txtApproverPosition_" + item.ID + "_" + c + "\" class=\"required-field form-control ApproverPositionTextDynamicFields_" + item.ID + "\" title=\"Position\" placeholder=\"Search..\"> \
                                <input type=\"hidden\" id=\"hdnApproverPosition_" + item.ID + "_" + c + "\" class=\"ApproverPositionDynamicFields_" + item.ID + "\"/> \
                            </div> \
                            <div class=\"col-md-2 text-align-center no-padding\"> \
                               <input type=\"search\" id=\"txtApproverOrgGroup_" + item.ID + "_" + c + "\" class=\"required-field form-control ApproverOrgGroupTextDynamicFields_" + item.ID + "\" title=\"Position\" placeholder=\"Search..\"> \
                                <input type=\"hidden\" id=\"hdnApproverOrgGroup_" + item.ID + "_" + c +"\" class=\"ApproverOrgGroupDynamicFields_" + item.ID + "\"/> \
                            </div> \
                            <div class=\"col-md-2 text-align-center no-padding\"> \
                                <input type=\"search\" id=\"txtAltApproverPosition_" + item.ID + "_" + c + "\" class=\"form-control AltApproverPositionTextDynamicFields_" + item.ID + "\" title=\"Position\" placeholder=\"Search..\"> \
                                <input type=\"hidden\" id=\"hdnAltApproverPosition_" + item.ID + "_" + c +"\" class=\"AltApproverPositionDynamicFields_" + item.ID + "\"/> \
                            </div> \
                            <div class=\"col-md-2 text-align-center no-padding\"> \
                               <input type=\"search\" id=\"txtAltApproverOrgGroup_" + item.ID + "_" + c + "\" class=\"form-control AltApproverOrgGroupTextDynamicFields_" + item.ID + "\" title=\"Position\" placeholder=\"Search..\"> \
                                <input type=\"hidden\" id=\"hdnAltApproverOrgGroup_" + item.ID + "_" + c +"\" class=\"AltApproverOrgGroupDynamicFields_" + item.ID + "\"/> \
                            </div> \
                        </div>"
                    );

                    $("#ApproversDynamicFields_" + item.ID + "_" + c).insertAfter($(".ApproversDynamicFields_" + item.ID).eq(-2));
                    
                };

                var positionIDs = [];
                
                function unique(list) {
                    var result = [];
                    $.each(list, function (i, e) {
                        if ($.inArray(e, result) == -1) result.push(e);
                    });
                    return result;
                }

                $(data.Result.Approvers).each(function (index, item) {
                    positionIDs.push(item.PositionID);
                });

                $(unique(positionIDs)).each(function (index1, item1) {
                    $(data.Result.Approvers).each(function (index, item) {
                        if (item1 == item.PositionID) {
                            uniquePositionsList.push({
                                ID: item1,
                                Title: item.Position
                            });
                            return false;
                        }
                    });

                });
                
                //var ctr = 1;

                $(uniquePositionsList).each(function (index, item) {
                    //populateFields(item, ctr); ctr++;
                    //PositionPlannedOld.push(item.PositionID + '-' + item.PlannedCount);
                    $("#DivApproversDynamicFields").append(
                        "<div class=\"form-group form-fields ApproversDynamicFields_" + item.ID + "\" id=\"ApproversDynamicFields_" + item.ID + "_1\"> \
                            <div class=\"col-md-2-5 text-align-center no-padding\"> \
                                <input type=\"search\" id=\"txtPosition_" + item.ID + "\" class=\"form-control PositionDynamicFields\" value=\"" + item.Title + "\" title=\"Position\" readonly> \
                                <input type=\"hidden\" id=\"hdnPosition_" + item.ID + "\" value=\"" + item.ID + "\"/> \
                            </div> \
                            <div class=\"col-md-0-75 text-align-center no-padding\"> \
                                <input type=\"number\" id=\"txtLevel_" + item.ID + "_1\" class=\"required-field form-control\" title=\"Level\" value=\"1\" readonly> \
                           </div> \
                            <div class=\"col-md-2 text-align-center no-padding\"> \
                                <input type=\"search\" id=\"txtApproverPosition_" + item.ID + "_1\" class=\"required-field form-control ApproverPositionTextDynamicFields_" + item.ID + "\" title=\"Position\" placeholder=\"Search..\"> \
                                <input type=\"hidden\" id=\"hdnApproverPosition_" + item.ID + "_1\" class=\"ApproverPositionDynamicFields_" + item.ID + "\"/> \
                            </div> \
                            <div class=\"col-md-2 text-align-center no-padding\"> \
                               <input type=\"search\" id=\"txtApproverOrgGroup_" + item.ID + "_1\" class=\"required-field form-control ApproverOrgGroupTextDynamicFields_" + item.ID + "\" title=\"Position\" placeholder=\"Search..\"> \
                                <input type=\"hidden\" id=\"hdnApproverOrgGroup_" + item.ID + "_1\" class=\"ApproverOrgGroupDynamicFields_" + item.ID + "\"/> \
                            </div> \
                            <div class=\"col-md-2 text-align-center no-padding\"> \
                                <input type=\"search\" id=\"txtAltApproverPosition_" + item.ID + "_1\" class=\"form-control AltApproverPositionTextDynamicFields_" + item.ID + "\" title=\"Position\" placeholder=\"Search..\"> \
                                <input type=\"hidden\" id=\"hdnAltApproverPosition_" + item.ID + "_1\" class=\"AltApproverPositionDynamicFields_" + item.ID + "\"/> \
                            </div> \
                            <div class=\"col-md-2 text-align-center no-padding\"> \
                               <input type=\"search\" id=\"txtAltApproverOrgGroup_" + item.ID + "_1\" class=\"form-control AltApproverOrgGroupTextDynamicFields_" + item.ID + "\" title=\"Position\" placeholder=\"Search..\"> \
                                <input type=\"hidden\" id=\"hdnAltApproverOrgGroup_" + item.ID + "_1\" class=\"AltApproverOrgGroupDynamicFields_" + item.ID + "\"/> \
                            </div> \
                            <div class=\"col-md-0-5\"> \
                            </div> \
                        </div>"
                        //< span class=\"btn-glyph-dynamic glyphicon glyphicon-plus-sign btnAddApproverFields\" id=\"btnAddApproverFields_" + item.ID + "\"></span> \
                    );

                    //if (item.HierarchyLevel > 1) {
                    addDynamicFields(item, 2)

                    objEMSCommonJS.BindAutoComplete("txtApproverPosition_" + item.ID + "_" + 1
                        , GetPositionAutoCompleteURL, 20, "hdnApproverPosition_" + item.ID + "_" + 1, "ID", "Description");
                    objEMSCommonJS.BindAutoComplete("txtApproverOrgGroup_" + item.ID + "_" + 1
                        , GetOrgGroupAutoCompleteURL, 20, "hdnApproverOrgGroup_" + item.ID + "_" + 1, "ID", "Description");

                    objEMSCommonJS.BindAutoComplete("txtApproverPosition_" + item.ID + "_" + 2
                        , GetPositionAutoCompleteURL, 20, "hdnApproverPosition_" + item.ID + "_" + 2, "ID", "Description");
                    objEMSCommonJS.BindAutoComplete("txtApproverOrgGroup_" + item.ID + "_" + 2
                        , GetOrgGroupAutoCompleteURL, 20, "hdnApproverOrgGroup_" + item.ID + "_" + 2, "ID", "Description");

                    objEMSCommonJS.BindAutoComplete("txtAltApproverPosition_" + item.ID + "_" + 1
                        , GetPositionAutoCompleteURL, 20, "hdnAltApproverPosition_" + item.ID + "_" + 1, "ID", "Description");
                    objEMSCommonJS.BindAutoComplete("txtAltApproverOrgGroup_" + item.ID + "_" + 1
                        , GetOrgGroupAutoCompleteURL, 20, "hdnAltApproverOrgGroup_" + item.ID + "_" + 1, "ID", "Description");

                    objEMSCommonJS.BindAutoComplete("txtAltApproverPosition_" + item.ID + "_" + 2
                        , GetPositionAutoCompleteURL, 20, "hdnAltApproverPosition_" + item.ID + "_" + 2, "ID", "Description");
                    objEMSCommonJS.BindAutoComplete("txtAltApproverOrgGroup_" + item.ID + "_" + 2
                        , GetOrgGroupAutoCompleteURL, 20, "hdnAltApproverOrgGroup_" + item.ID + "_" + 2, "ID", "Description");

                    //}
                });

                $(data.Result.Approvers).each(function (index, item) {
                    if (item.HierarchyLevel > 0) {
                        var c = item.HierarchyLevel;

                        $("#txtLevel_" + item.PositionID + "_" + c).val(item.HierarchyLevel);
                        $("#txtApproverPosition_" + item.PositionID + "_" + c).val(item.ApproverPosition);
                        $("#hdnApproverPosition_" + item.PositionID + "_" + c).val(item.ApproverPositionID);
                        $("#txtApproverOrgGroup_" + item.PositionID + "_" + c).val(item.ApproverOrgGroup);
                        $("#hdnApproverOrgGroup_" + item.PositionID + "_" + c).val(item.ApproverOrgGroupID);
                        $("#txtAltApproverPosition_" + item.PositionID + "_" + c).val(item.AltApproverPosition);
                        $("#hdnAltApproverPosition_" + item.PositionID + "_" + c).val(item.AltApproverPositionID);
                        $("#txtAltApproverOrgGroup_" + item.PositionID + "_" + c).val(item.AltApproverOrgGroup);
                        $("#hdnAltApproverOrgGroup_" + item.PositionID + "_" + c).val(item.AltApproverOrgGroupID);
                    }
                });

                $(".btnAddApproverFields").click(function () {
                    $(" .errMessage").removeClass("errMessage");
                    $("#divApproverSetupErrorMessage").html("");
                    var id = ($(this).prop("id")).replace("btnAddApproverFields_", "");

                    $(".ApproversDynamicFields_" + 290).last().find()
                    var blankFields = 0;

                    $($(".ApproverPositionTextDynamicFields_" + id).toArray()).each(function (index, item) {
                        if ($(item).val() == "") {
                            blankFields++;
                            $(item).addClass("errMessage");
                        }
                    });
                    $($(".ApproverOrgGroupTextDynamicFields_" + id).toArray()).each(function (index, item) {
                        if ($(item).val() == "") {
                            blankFields++;
                            $(item).addClass("errMessage");
                        }
                    });


                    if (blankFields == 0) {
                        $(uniquePositionsList).each(function (index, item) {
                            if (item.ID == id) {
                                var level = $(".ApproversDynamicFields_" + id).length + 1;
                                addDynamicFields(item, level);
                                objEMSCommonJS.BindAutoComplete("txtApproverPosition_" + id + "_" + level
                                    , GetPositionAutoCompleteURL, 20, "hdnApproverPosition_" + id + "_" + level, "ID", "Description");
                                objEMSCommonJS.BindAutoComplete("txtApproverOrgGroup_" + id + "_" + level
                                    , GetOrgGroupAutoCompleteURL, 20, "hdnApproverOrgGroup_" + id + "_" + level, "ID", "Description");
                                objEMSCommonJS.BindAutoComplete("txtAltApproverPosition_" + id + "_" + level
                                    , GetPositionAutoCompleteURL, 20, "hdnAltApproverPosition_" + id + "_" + level, "ID", "Description");
                                objEMSCommonJS.BindAutoComplete("txtAltApproverOrgGroup_" + id + "_" + level
                                    , GetOrgGroupAutoCompleteURL, 20, "hdnAltApproverOrgGroup_" + id + "_" + level, "ID", "Description");
                            }
                        });
                    }
                    else {
                        $(".errMessage:first").focus();
                    }

                });
            };

            objEMSCommonJS.GetAjax(GetPositionListURL, input, "", GetSuccessFunction);
        },

    };
    
     objApproverSetupEditJS.Initialize();
});