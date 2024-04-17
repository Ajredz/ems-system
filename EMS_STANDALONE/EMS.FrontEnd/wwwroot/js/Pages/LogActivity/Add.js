var objLogActivityAddJS;

$(document).ready(function () {
    objLogActivityAddJS = {

        Initialize: function () {
            $("#divLogActivityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divLogActivityModal #btnSave").show();
            $("#divLogActivityModal .form-control").attr("readonly", false);
            $("#divLogActivityModal #btnDelete, #divLogActivityModal #btnBack").remove();
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmLogActivity").trigger("reset");
        },

        ElementBinding: function () {

            $("#divLogActivityModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmLogActivity", "#divLogActivityErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , LogActivityAddPostURL \
                        , new FormData($('#frmLogActivity').get(0)) \
                        , '#divLogActivityErrorMessage' \
                        , '#divLogActivityModal #btnSave' \
                        , objLogActivityAddJS.AddSuccessFunction);",
                        "function");
                }
            });

            $("#ddlType").change(function () {
                GenerateDropdownValues(SubTypeDropdownChangeURL + "&Type=" + $("#ddlType").val(),
                    "ddlSubType", "Value", "Description", "", "", false);
            });
        },

    };

    objLogActivityAddJS.Initialize();
});