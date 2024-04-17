var objKPIScoreEditJS;

$(document).ready(function () {
    objKPIScoreEditJS = {

        OrgID: $("#hdnOrgID").val(),
        PositionID: $("#hdnPositionID").val(),
        Year: $("#hdnTDate").val(),

        Initialize: function () {
            $("#divKPIScoreBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass(".unreqField");
            $(".reqField").removeClass(".reqField");
            $("#btnEdit").hide();
            $("#btnSave, #btnBack").show();
            $("#divKPIScoreModal .form-control").not("#ddlOrgGroup, #ddlPosition").attr("readonly", false);
            s.LoadKPIScore();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divKPIScoreModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;
            Code($("#txtCode"));
            PreventSpace($("#txtCode"));
            $('#txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmKPIScore", "#divKPIScoreErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , KPIScoreEditPostURL \
                        , objKPIScoreEditJS.GetFormData() \
                        , '#divKPIScoreErrorMessage' \
                        , '#btnSave' \
                        , objKPIScoreEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(KPIScoreViewURL + "?OrgID=" + objKPIScoreEditJS.OrgID +
                                              "&PositionID=" + objKPIScoreEditJS.PositionID +
                                              "&Year=" + objKPIScoreEditJS.Year, "divKPIScoreBodyModal");
            });

        },

        GetFormData: function () {

            var formData = new FormData($('#frmKPIScore').get(0));

            $(".IDDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIScore.KPIScoreList[" + index + "].ID", $(this).val());
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

        AddChildrenScoreDynamicFields: function () {
            var s = this;
            var htmlId = $(".ChildrenScoreDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenScoreDynamicFields_", "")) + 1;

            $("#DivChildrenScoreDynamicFields").append(
                "<div class=\"form-group form-fields ChildrenScoreDynamicFields\" id=\"ChildrenScoreDynamicFields_" + ctr + "\"> \
                   <input type=\"hidden\" id=\"txtID_" + ctr + "\" class=\"IDDynamicFields\" readonly> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtKRAGroup_" + ctr + "\" class=\"form-control KRAGroupDynamicFields\" title=\"KRA Group\" readonly> \
                   </div> \
                   <div class=\"col-md-1 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtKPICode_" + ctr + "\" class=\"form-control KPICodeDynamicFields\" title=\"KPI Code\" readonly> \
                   </div> \
                   <div class=\"col-md-2 text-align-center no-padding\"> \
                       <input type=\"text\" id=\"txtKPIName_" + ctr + "\" class=\"form-control KPINameDynamicFields\" title=\"KPI Name\" readonly> \
                   </div> \
                   <div class=\"col-md-4 text-align-center no-padding\"> \
                       <textarea id=\"txtKPIDescription_" + ctr + "\" style=\"resize: none;\" class=\"form-control KPIDescriptionDynamicFields\" title=\"KPI Description\" readonly></textarea> \
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

        LoadKPIScore: function () {
            var s = this;
            $("#DivChildrenScoreDynamicFields").html("");

            $(KPIScoreList).each(function (index, item) {
                s.AddChildrenScoreDynamicFields();
                $("#txtID_" + (index + 1)).val(item.ID);
                $("#txtKRAGroup_" + (index + 1)).val(item.KRAGroup);
                $("#txtKPICode_" + (index + 1)).val(item.KPICode);
                $("#txtKPIName_" + (index + 1)).val(item.KPIName);
                $("#txtKPIDescription_" + (index + 1)).val(item.KPIDescription);
                $("#txtTarget_" + (index + 1)).val(item.Target);
                $("#txtActual_" + (index + 1)).val(item.Actual);
                $("#txtRate_" + (index + 1)).val(item.Rate);

                //s.CalculateTotal("RateDynamicFields", "txtTotalRate");
            });
        }

    };

    objKPIScoreEditJS.Initialize();
});