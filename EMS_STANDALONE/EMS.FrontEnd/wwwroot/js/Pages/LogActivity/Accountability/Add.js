var objAccountabilityAddJS;
var TypeDropDownOptions = [];

$(document).ready(function () {
    objAccountabilityAddJS = {

        Initialize: function () {
            $("#divAccountabilityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divAccountabilityModal #btnSave").show();
            $("#divAccountabilityModal .form-control").attr("readonly", false);
            $("#divAccountabilityModal #btnDelete, #divAccountabilityModal #btnBack").remove();
            s.GetTypeDropDownOptions();
            s.AddAccountabilityDynamicFields();
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmAccountability").trigger("reset");
            $("#divAccountabilityDynamicFields").html("");
            objAccountabilityAddJS.AddAccountabilityDynamicFields();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnAddAccountability").click(function () {
                var fields = $("#divAccountabilityDynamicFields .required-field");
                var addNewFields = true;

                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });

                if (addNewFields)
                    s.AddAccountabilityDynamicFields();
            });

            $("#divAccountabilityModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmAccountability", "#divAccountabilityErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , AccountabilityAddPostURL \
                        , objAccountabilityAddJS.GetFormData() \
                        , '#divAccountabilityErrorMessage' \
                        , '#divAccountabilityModal #btnSave' \
                        , objAccountabilityAddJS.AddSuccessFunction);",
                        "function");
                }
            });
        },

        GetFormData: function () {

            var formData = new FormData($('#frmAccountability').get(0));

            $(".TypeDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Accountability.AccountabilityDetailsList[" + index + "].Type", $(this).val());
                }
            });

            $(".TitleDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Accountability.AccountabilityDetailsList[" + index + "].Title", $(this).val());
                }
            });

            $(".DescriptionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Accountability.AccountabilityDetailsList[" + index + "].Description", $(this).val());
                }
            });

            $(".OrgGroupDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("Accountability.AccountabilityDetailsList[" + index + "].OrgGroupID", $(this).val());
                }
            });

            return formData;
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

        AddAccountabilityDynamicFields: function () {
            var s = this;
            var htmlId = $(".AccountabilityDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("AccountabilityDynamicFields_", "")) + 1;

            $("#divAccountabilityDynamicFields").append(
                "<div class=\"form-group form-fields AccountabilityDynamicFields\" id=\"AccountabilityDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-0-5\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objAccountabilityAddJS.RemoveDynamicFields('#AccountabilityDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <select id=\"ddlType_" + ctr + "\" title=\"Type\" class=\"form-control required-field TypeDynamicFields\"></select>"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <input type=\"text\" id=\"txtTitle_" + ctr + "\" class=\"form-control required-field TitleDynamicFields\" title=\"Title\" maxlength=\"100\">"
                + "    </div>"
                + "    <div class=\"col-md-2-5 no-padding\">"
                + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\" id=\"txtDescription_" + ctr + "\" class=\"form-control DescriptionDynamicFields\" maxlength=\"255\" title=\"Description\"></textarea>"
                + "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <input type=\"search\" placeholder=\"Search..\" id=\"txtOrgGroup_" + ctr + "\" class=\"form-control required-field\" title=\"Org Group\" maxlength=\"100\">"
                + "        <input type=\"hidden\" id=\"hdnOrgGroup_" + ctr + "\" class=\"OrgGroupDynamicFields\">"
                + "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <input type=\"search\" placeholder=\"Search..\" id=\"txtPosition_" + ctr + "\" class=\"form-control \" title=\"Position\" maxlength=\"100\">"
                + "        <input type=\"hidden\" id=\"hdnPosition_" + ctr + "\" class=\"PositionDynamicFields\">"
                + "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <input type=\"search\" placeholder=\"Search..\" id=\"txtEmployee_" + ctr + "\" class=\"form-control\" title=\"Employee Name\" maxlength=\"100\">"
                + "        <input type=\"hidden\" id=\"hdnEmployee_" + ctr + "\" class=\"EmployeeDynamicFields\">"
                + "    </div>"
                + "</div>"
            );

            s.PopulateDropDown("#ddlType_" + ctr, TypeDropDownOptions);

            objEMSCommonJS.BindAutoComplete("txtOrgGroup_" + ctr
                , OrgGroupAutoCompleteURL, 20, "hdnOrgGroup_" + ctr, "ID", "Description");

            objEMSCommonJS.BindAutoComplete("txtPosition_" + ctr
                , PositionAutoCompleteURL, 20, "hdnPosition_" + ctr, "ID", "Description");

            objEMSCommonJS.BindAutoComplete("txtEmployee_" + ctr
                , EmployeeAutoCompleteURL, 20, "hdnEmployee_" + ctr, "ID", "Description");
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
                    $("#AccountabilityDynamicFields_" + ctr + " #ddlType_" + ctr).removeClass("errMessage");
                    $("#AccountabilityDynamicFields_" + ctr + " #ddlSubType_" + ctr).removeClass("errMessage");
                    $("#AccountabilityDynamicFields_" + ctr + " #ddlTitle_" + ctr).removeClass("errMessage");

                    $(".ModuleDynamicFields").not(this).filter(function () {
                        if (this.value == currentVal) {
                            $("#AccountabilityDynamicFields_" + ctr + " #ddlModule_" + ctr).addClass("errMessage");
                            $("#AccountabilityDynamicFields_" + ctr + " #ddlType_" + ctr).addClass("errMessage");
                            $("#AccountabilityDynamicFields_" + ctr + " #ddlSubType_" + ctr).addClass("errMessage");
                            $("#AccountabilityDynamicFields_" + ctr + " #ddlTitle_" + ctr).addClass("errMessage");
                            isDuplicate++;
                        }
                    });
                }
            });

            if (isDuplicate > 0) {
                $("#divAccountabilityErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }

            return isValid;
        },

    };

    objAccountabilityAddJS.Initialize();
});