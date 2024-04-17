var objChangePassword;

const ChangePasswordURL = "/EmployeeProfile/ChangePassword";


$(document).ready(function () {
    objChangePassword = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();

        },
        ElementBinding: function () {
            var s = this;

            $("#btnEdit").on("click", function(){
                $(".form-control").prop("readonly", false);
                $("#btnEdit").hide();
                $("#btnChangePassword,#btnCancel").show();
            });

            $("#btnCancel").on("click", function () {
                $(".form-control").prop("readonly", true);
                $("#btnEdit").show();
                $("#btnChangePassword,#btnCancel").hide();
            });

            $("#btnChangePassword").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmManageAcountChangePassword", "#divChangePasswordErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_CHANGE_PASSWORD,
                        "objEMSCommonJS.PostAjax(true \
                        , ChangePasswordURL \
                        , new FormData($('#frmManageAcountChangePassword').get(0)) \
                        , '#divChangePasswordErrorMessage' \
                        , '#btnSave' \
                        , objChangePassword.ChangePasswordSuccessFunction, null, true);",
                        "function");
                }
            });
        },
        ChangePasswordSuccessFunction: function (data) {
            ModalAlertRedirect(MODAL_HEADER, data.Result, '/Login?handler=Logout');
            $("#frmManageAcountChangePassword").trigger("reset");
        },
    },
        objChangePassword.Initialize();
});