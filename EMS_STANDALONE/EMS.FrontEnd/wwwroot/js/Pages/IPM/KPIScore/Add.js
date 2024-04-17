var objKPIScoreAddJS;

$(document).ready(function () {
    objKPIScoreAddJS = {

        Initialize: function () {
            $("#divKPIScoreBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divKPIScoreModal #btnSave").show();
            $("#divKPIScoreModal .form-control").attr("readonly", false);
            $("#divKPIScoreModal #btnDelete, #divKPIScoreModal #btnBack").remove();

        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#DivChildrenScoreDynamicFields").html("");
            $("#frmKPIScore").trigger("reset");
        },

        AddChildrenScoreDynamicFields: function () {
            var s = this;
            var htmlId = $(".ChildrenScoreDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenScoreDynamicFields_", "")) + 1;

            $("#DivChildrenScoreDynamicFields").append(
                "<div class=\"form-group form-fields ChildrenScoreDynamicFields\" id=\"ChildrenScoreDynamicFields_" + ctr + "\"> \
                   <input type=\"hidden\" id=\"txtID_" + ctr + "\" class=\"IDGroupDynamicFields\" readonly> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtKRAGroup_" + ctr + "\" class=\"form-control KRAGroupDynamicFields\" title=\"KRA Group\" readonly> \
                   </div> \
                   <div class=\"col-md-1 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtKPICode_" + ctr + "\" class=\"form-control KPICodeDynamicFields\" title=\"KPI Code\" readonly> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtKPIName_" + ctr + "\" class=\"form-control KPINameDynamicFields\" title=\"KPI Name\" readonly> \
                   </div> \
                   <div class=\"col-md-3 text-align-center no-padding\"> \
                       <textarea id=\"txtKPIDescription_" + ctr + "\" style=\"resize: none;\" class=\"form-control KPIDescriptionDynamicFields\" title=\"KPI Description\" readonly></textarea> \
                   </div> \
                   <div class=\"col-md-1 text-align-center no-padding\"> \
                       <input type=\"number\" id=\"txtWeight_" + ctr + "\" class=\"form-control text-amount WeightDynamicFields\" title=\"Weight\" readonly> \
                   </div> \
                   <div class=\"col-md-1 text-align-center no-padding\"> \
                       <input type=\"number\" id=\"txtTarget_" + ctr + "\" class=\"form-control text-amount TargetDynamicFields\" title=\"Target\"> \
                   </div> \
                   <div class=\"col-md-1 text-align-center no-padding\"> \
                       <input type=\"number\" id=\"txtActual_" + ctr + "\" class=\"form-control text-amount ActualDynamicFields\" title=\"Actual\"> \
                   </div> \
                   <div class=\"col-md-1 text-align-center no-padding\"> \
                       <input type=\"number\" id=\"txtRate_" + ctr + "\" class=\"form-control text-amount RateDynamicFields\" title=\"Rate\"> \
                   </div> \
               </div>"
            );

            NumberOnly($(".TargetDynamicFields"));
            NumberOnly($(".ActualDynamicFields"));
            NumberOnly($(".RateDynamicFields"));
        },

        ElementBinding: function () {
            var s = this;

            $("#ddlOrgGroup").change(function () {
                if ($("#ddlOrgGroup").val() != "") {
                    GenerateDropdownValues(GetPositionByOrgGroupDropdownURL + "&OrgGroupID=" + $("#ddlOrgGroup").val(),
                        "ddlPosition", "Value", "Text", "", "", false);
                }
            });

            $("#ddlPosition").change(function () {
                if ($("#ddlPosition").val() != "") {

                    var positionID = $("#ddlPosition").val();
                    var tempKPIPosition = $.grep(KPIPositionList, function (e, i) {
                        return e.PositionID == positionID;
                    });

                    $("#DivChildrenScoreDynamicFields").html("");

                    if (tempKPIPosition.length != 0) {
                        $(tempKPIPosition).each(function (index, item) {
                            s.AddChildrenScoreDynamicFields();
                            $("#txtID_" + (index + 1)).val(item.ID);
                            $("#txtKRAGroup_" + (index + 1)).val(item.KRAGroup);
                            $("#txtKPICode_" + (index + 1)).val(item.KPICode);
                            $("#txtKPIName_" + (index + 1)).val(item.KPIName);
                            $("#txtKPIDescription_" + (index + 1)).val(item.KPIDescription);
                            $("#txtWeight_" + (index + 1)).val(item.Weight);
                        });
                    } else {
                        $("#ddlPosition").val("");
                        ModalAlert(MODAL_HEADER, "No KPI setup for this Position.");
                    }

                }
            });

            $("#divKPIScoreModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmKPIScore", "#divKPIScoreErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , KPIScoreAddPostURL \
                        , objKPIScoreAddJS.GetFormData() \
                        , '#divKPIScoreErrorMessage' \
                        , '#divKPIScoreModal #btnSave' \
                        , objKPIScoreAddJS.AddSuccessFunction);",
                        "function");
                }
            });
        },

        GetFormData: function () {

            var formData = new FormData($('#frmKPIScore').get(0));

            $(".IDGroupDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIScore.KPIScoreList[" + index + "].KPIPosition", $(this).val());
                }
            });

            $(".TargetDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIScore.KPIScoreList[" + index + "].Target", $(this).val());
                }
            });

            $(".ActualDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIScore.KPIScoreList[" + index + "].Actual", $(this).val());
                }
            });

            $(".RateDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIScore.KPIScoreList[" + index + "].Rate", $(this).val());
                }
            });

            return formData;

        },
    };

    objKPIScoreAddJS.Initialize();
});