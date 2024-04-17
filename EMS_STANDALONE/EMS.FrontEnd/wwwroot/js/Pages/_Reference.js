var objReferenceJS;
        
const ReferenceValueURL = window.location.pathname;
const ReferencePostURL = window.location.pathname + "?refcode=";

$(document).ready(function () {
    objReferenceJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
            s.AddReferenceValueDynamicFields();
        },

        ElementBinding: function () {
            var s = this;

            $("#ddlReferenceCode").change(function () {
                if ($("#ddlReferenceCode").val() != "") {
                    objReferenceJS.LoadReferenceValues();
                }
                else {
                    $("#DivReferenceValueDynamicFields").html("");
                    objReferenceJS.AddReferenceValueDynamicFields();
                }
            });

            $("#btnAddReferenceValueFields").click(function () {
                var fields = $("#DivReferenceValueDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddReferenceValueDynamicFields();
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#divReferenceErrorMessage", objReferenceJS.ValidateDuplicateFields)) {
                    $(".ValueDynamicFields").each(function (index) {
                        $(this).prop("name", "ReferenceValue[" + index + "].Value");
                    });
                    $(".DescriptionDynamicFields").each(function (index) {
                        $(this).prop("name", "ReferenceValue[" + index + "].Description");
                    });

                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , ReferencePostURL + $('#ddlReferenceCode').val() \
                        , new FormData($('#frmReference').get(0)) \
                        , '#divReferenceErrorMessage' \
                        , '#btnSave' \
                        , objReferenceJS.SaveSuccessFunction); ", "function");
                }
            });

        },

        SaveSuccessFunction: function () {
            objReferenceJS.LoadReferenceValues();
        },

        AddReferenceValueDynamicFields: function () {
            var htmlId = $(".ReferenceValueDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ReferenceValueDynamicFields_", "")) + 1;

            $("#DivReferenceValueDynamicFields").append(
                "<div class=\"form-group form-fields ReferenceValueDynamicFields\" id=\"ReferenceValueDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-4 no-padding\">"
                + "        <input id=\"txtValue_" + ctr + "\" maxlength=\"20\" class=\"form-control required-field ValueDynamicFields\" title=\"Value\">"
                + "    </div>"
                + "    <div class=\"col-md-7 no-padding\">"
                + "        <input id=\"txtDescription_" + ctr + "\" maxlength=\"255\" class=\"form-control required-field DescriptionDynamicFields\" title=\"Description\">"
                + "    </div>"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objReferenceJS.RemoveDynamicFields('#ReferenceValueDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
                + "    </div>"
                + "</div>"
            );
        },

        RemoveDynamicFields: function (id) {
            var ctrToDelete = parseInt(id == undefined ? 0 : id.replace("#ReferenceValueDynamicFields_", ""));
            $(".ReferenceValueDynamicFields").each(function () {
                var ctr = $(this).prop("id").replace("ReferenceValueDynamicFields_", "");
                if (ctrToDelete < ctr)
                    $("#txtOrder_" + ctr).val($("#txtOrder_" + ctr).val() - 1);
            });

            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },

        ValidateDuplicateFields: function () {
            var isValid = true;

            var isDuplicate = 0;

            $(".ValueDynamicFields").each(function (n1, x1) {
                if (this.value != "") {
                    var currentVal = this.value;
                    var ctr = $(this).prop("id").replace("txtValue_", "");
                    $("#ReferenceValueDynamicFields_" + ctr + " input").removeClass("errMessage");
                    $(".ValueDynamicFields").not(this).filter(function () {
                        if (this.value == currentVal) {
                            $("#ReferenceValueDynamicFields_" + ctr + " input").addClass("errMessage");
                            isDuplicate++;
                        }
                    });
                }
            });

            if (isDuplicate > 0) {
                $("#divReferenceErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }

            if (!isValid)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return isValid;
        },

        LoadReferenceValues: function () {
            var s = this;
            $("#DivReferenceValueDynamicFields").html("");

            var input = { handler: "ReferenceValues", RefCode: $("#ddlReferenceCode").val() };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddReferenceValueDynamicFields();
                    $("#txtValue_" + idCtr).val(item.Value);
                    $("#txtValue_" + idCtr).attr("readonly", true);
                    $("#txtDescription_" + idCtr).val(item.Description);
                };
                var ctr = 1;
                $(data).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(ReferenceValueURL, input, "", GetSuccessFunction);
        },
    };

    objReferenceJS.Initialize();
});