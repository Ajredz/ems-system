var objEmployeeEditJS;

$(document).ready(function () {
    objEmployeeEditJS = {

        ID: $("#divEmployeeModal #hdnID").val(),

        Initialize: function () {
            $("#divEmployeeBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            //$("#divEmployeeModal .form-control").attr("readonly", false);
            $("#divEmployeeModal #btnEdit").hide();
            $("#divEmployeeModal #btnSave, #divEmployeeModal #btnBack").show();

            $("#cbShowDirectory").prop("checked", ($("#hdncbShowDirectory").val() == "True" ? true : false));
            $("#cbShowDirectory").prop("disabled", ($("#hdnHasEditDisplayDirectory").val() == "true" ? false : true));

            $("#txtOfficeMobile").text() == (null || "") ? "" : $("#txtOfficeMobile").text($("#txtOfficeMobile").text().slice(0, 4) + "-" + $("#txtOfficeMobile").text().substr(4));
            $("#txtOfficeMobile").val() == (null || "") ? "" : $("#txtOfficeMobile").val($("#txtOfficeMobile").val().slice(0, 4) + "-" + $("#txtOfficeMobile").val().substr(4));
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function() {
            var s = this;
            NumberOnly($("#divEmployeeModal #txtOfficeMobile"));
            NumberDashOnly($("#divEmployeeModal #txtOfficeLandline"));

            $("#cbShowDirectory").click(function () {
                if ($("#cbShowDirectory").prop("checked"))
                    $("#hdncbShowDirectory").val(true);
                else
                    $("#hdncbShowDirectory").val(false);
            });

            $("#divEmployeeModal #txtOfficeMobile").on("keyup", function () {
                $("#divEmployeeCorporateEmailErrorMessage").html("");

                if (!validMobileNo($("#divEmployeeModal #txtOfficeMobile"))) {
                    $("#divEmployeeCorporateEmailErrorMessage").html("");
                    $("#divEmployeeModal #txtOfficeMobile").addClass("errMessage");
                    $("#divEmployeeModal #txtOfficeMobile").focus();
                    $("#divEmployeeCorporateEmailErrorMessage").append("<label class=\"errMessage\"><li>Office Mobile Number is invalid</li></label><br />");
                    $("#btnSave").prop("disabled",true);
                }
                else {
                    $("#txtOfficeMobile").removeClass("errMessage");
                    $("#btnSave").prop("disabled",false);
                }

                if ($("#txtOfficeMobile").val().length == 4)
                    $("#txtOfficeMobile").val($("#txtOfficeMobile").val() + "-");

                return false;
            });

            $("#divEmployeeModal #txtCorporateEmail").on("keyup", function () {
                $("#divEmployeeCorporateEmailErrorMessage").html("");

                if (!ValidEmail($('#txtCorporateEmail'))) {
                    $("#divEmployeeCorporateEmailErrorMessage").html("");
                    $("#txtCorporateEmail").addClass("errMessage");
                    $("#txtCorporateEmail").focus();
                    $("#divEmployeeCorporateEmailErrorMessage").append("<label class=\"errMessage\"><li>Please input valid Email</li></label><br />");
                    $("#btnSave").prop("disabled", true);
                }
                else
                    $("#btnSave").prop("disabled", false);
                return false;
            });
            $("#btnSave").click(function () {
                var isNoBlankFunction = function () {
                    return true;
                };

                if (objEMSCommonJS.ValidateBlankFields("#frmEmployeeCorporateEmail", "#divEmployeeCorporateEmailErrorMessage", isNoBlankFunction)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , EmployeeEditURL \
                        , objEmployeeEditJS.GetFormData() \
                        ,'#divEmployeeCorporateEmailErrorMessage' \
                        , '#btnSave' \
                        , objEmployeeEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(EmployeeViewURL + "?ID=" + objEmployeeEditJS.ID, "divEmployeeBodyModal");
            });

            objEMSCommonJS.BindAutoComplete("divEmployeeModal #txtHomeBranch"
                , OrgGroupAutoCompleteURL
                , 20, "divEmployeeModal #hdnHomeBranch", "ID", "Description");
        },

        GetFormData: function () {
            var formData = new FormData($('#frmEmployeeCorporateEmail').get(0));

            formData.append("Employee.OfficeMobile", $("#txtOfficeMobile").val().replace(/-/g, ""));
            return formData;
        },

    };

    objEmployeeEditJS.Initialize();
});