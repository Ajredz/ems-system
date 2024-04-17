var objTrainingAddJS;
var TypeDropDownOptions = [];

$(document).ready(function () {
    objTrainingAddJS = {

        Initialize: function () {
            $("#divTrainingBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divTrainingModal #btnSave").show();
            $("#divTrainingModal .form-control").attr("readonly", false);
            s.GetTypeDropDownOptions();
            s.AddTrainingDynamicFields();
        },

        ElementBinding: function () {
            var s = this;

            AutoCapitalFirst($("#txtTemplateName"));

            $("#divTrainingModal #btnAddTrainingTemplateDetails").click(function () {
                var fields = $("#divTrainingDynamicFields .required-field");
                var addNewFields = true;

                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });

                if (addNewFields)
                    s.AddTrainingDynamicFields();
            });

            $("#divTrainingModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#formTraining", "#divTrainingErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , TrainingAddURL \
                        , objTrainingAddJS.GetFormData() \
                        , '#divTrainingErrorMessage' \
                        , '#divTrainingModal #btnSave' \
                        , objTrainingAddJS.AddSuccessFunction);",
                        "function");
                }
            });
           

        },
        AddTrainingDynamicFields: function () {
            var s = this;
            var htmlId = $(".TrainingDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("TrainingDynamicFields_", "")) + 1;

            $("#divTrainingDynamicFields").append(
                "<div class=\"form-group form-fields TrainingDynamicFields\" id=\"TrainingDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-0-75\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objTrainingAddJS.RemoveDynamicFields('#TrainingDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
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
            AutoCapitalFirst($("#txtTitle_" + ctr));
        },

        GetFormData: function () {
            var formData = new FormData($('#formTraining').get(0));

            $(".TypeDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Training.TrainingTempateDetailsInputList[" + index + "].Type", $(this).val());
                }
            });
            $(".TitleDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Training.TrainingTempateDetailsInputList[" + index + "].Title", $(this).val());
                }
            });
            $(".DescriptionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Training.TrainingTempateDetailsInputList[" + index + "].Description", $(this).val());
                }
            });

            return formData;
        },
        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#formTraining").trigger("reset");
            $("#divTrainingDynamicFields").html("");
            objTrainingAddJS.AddTrainingDynamicFields();
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
        RemoveDynamicFields: function (id) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },
        ValidateDuplicateFields: function () {
            var isValid = true;
            var isDuplicate = 0;

            $(".ModuleDynamicFields").each(function (n1, x1) {
                if (this.value != "") {
                    var currentVal = this.value;
                    var ctr = $(this).prop("id").replace("ddlModule_", "");
                    $("#TrainingDynamicFields_" + ctr + " #ddlType_" + ctr).removeClass("errMessage");
                    $("#TrainingDynamicFields_" + ctr + " #ddlSubType_" + ctr).removeClass("errMessage");
                    $("#TrainingDynamicFields_" + ctr + " #ddlTitle_" + ctr).removeClass("errMessage");

                    $(".ModuleDynamicFields").not(this).filter(function () {
                        if (this.value == currentVal) {
                            $("#TrainingDynamicFields_" + ctr + " #ddlModule_" + ctr).addClass("errMessage");
                            $("#TrainingDynamicFields_" + ctr + " #ddlType_" + ctr).addClass("errMessage");
                            $("#TrainingDynamicFields_" + ctr + " #ddlSubType_" + ctr).addClass("errMessage");
                            $("#TrainingDynamicFields_" + ctr + " #ddlTitle_" + ctr).addClass("errMessage");
                            isDuplicate++;
                        }
                    });
                }
            });

            if (isDuplicate > 0) {
                $("#divTrainingErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }

            return isValid;
        },
    }
    objTrainingAddJS.Initialize();
});