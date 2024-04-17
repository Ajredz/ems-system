var objKRAGroupAddJS;

$(document).ready(function () {

    objKRAGroupAddJS = {

        Initialize: function () {
            $("#divKRAGroupBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divKRAGroupModal .form-control").attr("readonly", false);
        },

        AddSuccessFunction: function () {

            // Refresh KRA Group dropdown
            GenerateDropdownValues(GetKRAGroupDropdownURL,"ddlKRAGroup", "Value", "Text", "", "", false);

            $("#frmKRAGroup").trigger("reset");
            $("#divKRAGroupModal").modal("hide");
        },

        ElementBinding: function () {
            var s = this;   

            $("#divKRAGroupModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmKRAGroup", "#divKRAGroupErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , KRAGroupAddPostURL \
                        , new FormData($('#frmKRAGroup').get(0)) \
                        , '#divKRAGroupErrorMessage' \
                        , '#divKRAGroupModal #btnSave' \
                        , objKRAGroupAddJS.AddSuccessFunction);",
                        "function");
                }
            });
        },

    };

    objKRAGroupAddJS.Initialize();
});