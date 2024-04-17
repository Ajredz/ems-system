var objKPIPositionViewJS;
var KPIDropDownOptions = [];

$(document).ready(function () {
    objKPIPositionViewJS = {

        ID: $("#hdnID").val(),
        EffectiveDate: $("#txtEffectiveDate").val(),

        Initialize: function () {
            $("#divKPIPositionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();

            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divKPIPositionModal .form-control").prop("disabled", true);
            $('#btnAddChildrenSetupFields').hide();
            s.GetKPIDropDownOptions();
            s.LoadKPIPosition();

        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divKPIPositionModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , KPIPositionDeleteURL + '?ID=' + objKPIPositionViewJS.ID\ + '&EffectiveDate=' + objKPIPositionViewJS.EffectiveDate\
                    , {} \
                    , '#divKPIPositionErrorMessage' \
                    , '#btnDelete' \
                    , objKPIPositionViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(KPIPositionEditURL + "?ID=" + objKPIPositionViewJS.ID + "&EffectiveDate=" + objKPIPositionViewJS.EffectiveDate, "divKPIPositionBodyModal");
            });

            /*$("#btnCopy").click(function () {
                LoadPartial(CopyKPIPositionModalURL, "divCopyKPIPositionBodyModal");
                $("#divCopyKPIPositionModal").modal("show");
            });*/
        },

        GetKPIDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    KPIDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Text
                        });
                });
                s.AddChildrenSetupDynamicFields();
            };
        },

        AddChildrenSetupDynamicFields: function () {
            var s = this;
            var htmlId = $(".ChildrenSetupDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenSetupDynamicFields_", "")) + 1;

            $("#DivChildrenSetupDynamicFields").append(
                "<tbody class=\"ChildrenSetupDynamicFields\" id=\"ChildrenSetupDynamicFields_" + ctr + "\"> \
                   <input type=\"hidden\" id=\"txtID_" + ctr + "\" readonly> \
                   <tr> \
                       <td class=\"col-md-0-5\"></td> \
                       <td class=\"text-align-center\"> \
                           <label id=\"txtKRAGroup_" + ctr + "\" class=\"KRAGroupDynamicFields rowLabel\" title=\"KRA Group\"></label> \
                       </td> \
                      <td class=\"text-align-center\"> \
                           <label id=\"txtKRASubGroup_" + ctr + "\" class=\"KRASubGroupDynamicFields rowLabel\" title=\"KRA Sub Group\"></label> \
                       </td> \
                       <td class=\"text-align-left\"> \
                           <label id=\"txtKPI_" + ctr + "\" class=\"KPIDynamicFields rowLabel\" title=\"KPI\"></label> \
                           <input type=\"hidden\" id=\"txtKPIID_" + ctr + "\" readonly> \
                       </td> \
                       <td class=\"text-align-left\"> \
                           <label id=\"txtDescription_" + ctr + "\" class=\"DescriptionDynamicFields rowLabel\" title=\"Description\"></label> \
                       </td> \
                       <td class=\"text-align-right\"> \
                           <label id=\"txtWeight_" + ctr + "\" class=\"WeightDynamicFields rowLabel\" title=\"Weight w/ Service Bay\"></label> \
                       </td> \
                       <td class=\"text-align-right\"> \
                           <label id=\"txtWeightNoServiceBay_" + ctr + "\" class=\"WeightNoServiceBayDynamicFields rowLabel\" title=\"Weight w/o Service Bay\"></label> \
                       </td> \
                   </tr> \
               </tbody>"
            );

            DecimalsFormat($(".WeightDynamicFields"));

            $("#txtWeight_" + ctr).keyup(function () {

                if ($("#txtWeight_" + ctr).val() > 100)
                    $("#txtWeight_" + ctr).val(0);

                s.CalculateTotal("WeightDynamicFields", "txtTotal");
            });
        },

        LoadKPIPosition: function () {
            var s = this;
            $("#DivChildrenSetupDynamicFields").html("");

            $(KPIPositionList).each(function (index, item) {
                s.AddChildrenSetupDynamicFields();
                $("#txtID_" + (index + 1)).val(item.ID);
                $("#txtKRAGroup_" + (index + 1)).html(item.KRAGroup);
                $("#txtKRASubGroup_" + (index + 1)).html(item.KRASubGroup);
                $("#txtKPI_" + (index + 1)).html(item.KPI);
                $("#txtKPIID_" + (index + 1)).val(item.KPIID);
                $("#txtDescription_" + (index + 1)).html(item.KPIDescription);
                $("#txtWeight_" + (index + 1)).html(item.Weight);
                $("#txtWeightNoServiceBay_" + (index + 1)).html(item.WeightNoServiceBay);

                $("#txtWeight_" + (index + 1)).val(item.Weight);
                $("#txtWeightNoServiceBay_" + (index + 1)).val(item.WeightNoServiceBay);

                s.CalculateTotal("WeightDynamicFields", "txtTotal");
                s.CalculateTotal("WeightNoServiceBayDynamicFields", "txtTotalNoServiceBay");
            });
        },

        CalculateTotal: function (input, totalID) {
            var total = 0;
            var arr = $("." + input);

            for (var i = 0; i < arr.length; i++) {
                if (parseFloat(arr[i].value))
                    total += parseFloat(arr[i].value || 0);
            }

            $("#" + totalID).text((Math.round(total * 100) / 100) + "%");
        }

    };

    objKPIPositionViewJS.Initialize();
});