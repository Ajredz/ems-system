var objKRASubGroupAddJS;

$(document).ready(function () {

    objKRASubGroupAddJS = {

        Initialize: function () {
            $("#divKRASubGroupBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divKRASubGroupModal .form-control").attr("readonly", false);
        },

        AddSuccessFunction: function () {

            // Refresh KRA Group dropdown
            GenerateDropdownValues(GetKRASubGroupDropdownURL + "&KRAGroup=" + $("#ddlKRAGroup").val(),
                "ddlKRASubGroup", "Value", "Text", "", "", false);

            $("#frmKRASubGroup").trigger("reset");
            $("#divKRASubGroupModal").modal("hide");
        },

        ElementBinding: function () {
            var s = this;   

            $("#divKRASubGroupModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmKRASubGroup", "#divKRASubGroupErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , KRASubGroupAddPostURL \
                        , new FormData($('#frmKRASubGroup').get(0)) \
                        , '#divKRASubGroupErrorMessage' \
                        , '#divKRASubGroupModal #btnSave' \
                        , objKRASubGroupAddJS.AddSuccessFunction);",
                        "function");
                }
            });
        },

    };

    objKRASubGroupAddJS.Initialize();
});