var objLogActivityEditJS;

$(document).ready(function () {
    objLogActivityEditJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divLogActivityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divLogActivityModal .form-control").attr("readonly", false);
            $("#divLogActivityModal #btnEdit").hide();
            $("#divLogActivityModal #btnSave, #divLogActivityModal #btnBack").show();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $("#divLogActivityModal").modal('hide');
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function () {

            $("#btnBack").click(function () {
                LoadPartial(LogActivityViewURL + "?ID=" + objLogActivityEditJS.ID, "divLogActivityBodyModal");
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , LogActivityDeleteURL + '?ID=' + objLogActivityEditJS.ID\
                    , {} \
                    , '#divLogActivityErrorMessage' \
                    , '#btnDelete' \
                    , objLogActivityEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#divLogActivityModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmLogActivity", "#divLogActivityErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , LogActivityEditURL \
                        , new FormData($('#frmLogActivity').get(0)) \
                        , '#divLogActivityErrorMessage' \
                        , '#divLogActivityModal #btnSave' \
                        , objLogActivityEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#ddlType").change(function () {
                GenerateDropdownValues(SubTypeDropdownChangeURL + "&Type=" + $("#ddlType").val(),
                    "ddlSubType", "Value", "Description", "", "", false);
            });
        },
    };

    objLogActivityEditJS.Initialize();
});