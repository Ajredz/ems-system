var objTrainingViewJS;
var TypeDropDownOptions = [];

$(document).ready(function () {
    objTrainingViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divTrainingBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divTrainingModal .form-control").prop("disabled", true);
            $("#btnAddTrainingTemplateDetails").remove();
            s.GetTypeDropDownOptions();
            s.LoadTrainingTemplateDetails();
        },

        ElementBinding: function () {
            var s = this;

            $("#divTrainingModal #btnEdit").click(function () {
                LoadPartial(TrainingEditURL + "?ID=" + objTrainingViewJS.ID, "divTrainingBodyModal");
            });
        },
        LoadTrainingTemplateDetails: function () {
            var s = this;
            $("#divTrainingDynamicFields").html("");

            var input = { handler: "TrainingTemplateDetails", ID: objTrainingViewJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddTrainingDynamicFields();
                    $("#ddlType_" + idCtr).val(item.Type);
                    $("#txtTitle_" + idCtr).val(item.Title);
                    $("#txtDescription_" + idCtr).val(item.Description);

                    $("#ddlType_" + idCtr).prop("disabled", true);
                    $("#txtTitle_" + idCtr).prop("disabled", true);
                    $("#txtDescription_" + idCtr).prop("disabled", true);
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetTrainingTemplateDetailsURL, input, "", GetSuccessFunction);
        },
        AddTrainingDynamicFields: function () {
            var s = this;
            var htmlId = $(".TrainingDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("TrainingDynamicFields_", "")) + 1;

            $("#divTrainingDynamicFields").append(
                "<div class=\"form-group form-fields TrainingDynamicFields\" id=\"TrainingDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-0-75\">"
                + "    </div>"
                + "    <div class=\"col-md-3 no-padding\">"
                + "        <select id=\"ddlType_" + ctr + "\" title=\"Type\" class=\"form-control required-field TypeDynamicFields\"></select>"
                + "    </div>"
                + "    <div class=\"col-md-3 no-padding\">"
                + "        <input type=\"text\" id=\"txtTitle_" + ctr + "\" class=\"form-control required-field TitleDynamicFields\" title=\"Title\" maxlength=\"100\">"
                + "    </div>"
                + "    <div class=\"col-md-4-5 no-padding\">"
                + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\" id=\"txtDescription_" + ctr + "\" class=\"form-control DescriptionDynamicFields\" maxlength=\"255\" title=\"Description\"></textarea>"
                + "    </div>"
                + "</div>"
            );

            s.PopulateDropDown("#ddlType_" + ctr, TypeDropDownOptions);
        },
        GetTypeDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    TypeDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(GetTypeDropDownURL, {}, "", GetSuccessFunction);
        },
        PopulateDropDown: function (id, collection) {
            $(id).append($('<option/>', {
                value: "",
                text: "- Select an Item -"
            }));

            $(collection).each(function (index, item) {
                $(id).append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
        },
    }
    objTrainingViewJS.Initialize();
});