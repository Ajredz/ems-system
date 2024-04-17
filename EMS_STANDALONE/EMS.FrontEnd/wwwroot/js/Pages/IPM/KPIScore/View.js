var objKPIScoreViewJS;

$(document).ready(function () {
    objKPIScoreViewJS = {

        OrgID: $("#hdnOrgID").val(),
        PositionID: $("#hdnPositionID").val(),
        Year: $("#hdnTDate").val(),

        Initialize: function () {
            $("#divKPIScoreBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divKPIScoreModal .form-control").prop("disabled", true);
            s.LoadKPIScore();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divKPIScoreModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , KPIScoreDeleteURL + '?ID=' + objKPIScoreViewJS.ID\
                    , {} \
                    , '#divKPIScoreErrorMessage' \
                    , '#btnDelete' \
                    , objKPIScoreViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(KPIScoreEditURL + "?OrgID=" + objKPIScoreViewJS.OrgID +
                                              "&PositionID=" + objKPIScoreViewJS.PositionID +
                                              "&Year=" + objKPIScoreViewJS.Year, "divKPIScoreBodyModal");
            });

        },

        AddChildrenScoreDynamicFields: function () {
            var s = this;
            var htmlId = $(".ChildrenScoreDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenScoreDynamicFields_", "")) + 1;

            $("#DivChildrenScoreDynamicFields").append(
                "<div class=\"form-group form-fields ChildrenScoreDynamicFields\" id=\"ChildrenScoreDynamicFields_" + ctr + "\"> \
                   <input type=\"hidden\" id=\"txtID_" + ctr + "\" readonly> \
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
                       <input type=\"number\" id=\"txtTarget_" + ctr + "\" class=\"form-control text-amount TargetDynamicFields\" title=\"Target\" readonly> \
                   </div> \
                   <div class=\"col-md-1 text-align-center no-padding\"> \
                       <input type=\"number\" id=\"txtActual_" + ctr + "\" class=\"form-control text-amount ActualDynamicFields\" title=\"Actual\" readonly> \
                   </div> \
                   <div class=\"col-md-1 text-align-center no-padding\"> \
                       <input type=\"number\" id=\"txtRate_" + ctr + "\" class=\"form-control text-amount RateDynamicFields\" title=\"Rate\" readonly> \
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

    objKPIScoreViewJS.Initialize();
});