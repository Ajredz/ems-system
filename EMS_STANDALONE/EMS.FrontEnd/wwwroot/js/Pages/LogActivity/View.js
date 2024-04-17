var objLogActivityViewJS;

$(document).ready(function () {
    objLogActivityViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divLogActivityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divLogActivityModal .form-control, #divLogActivityModal input[type='checkbox']").prop("disabled", true);
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmLogActivity").trigger("reset");
            $("#divLogActivityModal").modal('hide');
        },

        ElementBinding: function () {

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , LogActivityDeleteURL + '?ID=' + objLogActivityViewJS.ID\
                    , {} \
                    , '#divLogActivityErrorMessage' \
                    , '#btnDelete' \
                    , objLogActivityViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(LogActivityEditURL + "?ID=" + objLogActivityViewJS.ID, "divLogActivityBodyModal");
            });
        },

    };

    objLogActivityViewJS.Initialize();
});