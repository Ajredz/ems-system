var objKPIViewJS;

$(document).ready(function () {
    objKPIViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divKPIBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divKPIModal .form-control").prop("disabled", true);
            $("#btnAddKRAGroup").css("pointer-events", "none");
            $("#btnAddKRASubGroup").css("pointer-events", "none");

        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divKPIModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , KPIDeleteURL + '?ID=' + objKPIViewJS.ID\
                    , {} \
                    , '#divKPIErrorMessage' \
                    , '#btnDelete' \
                    , objKPIViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(KPIEditURL + "?ID=" + objKPIViewJS.ID, "divKPIBodyModal");
            });

        },

    };

    objKPIViewJS.Initialize();
});