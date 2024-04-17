var objEmployeeCorporateEmailViewJS;

$(document).ready(function () {
    objEmployeeCorporateEmailViewJS = {

        ID: $("#divEmployeeModal #hdnID").val(),

        Initialize: function () {
            $("#divEmployeeBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            //s.LoadHierarchyUpward();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $('#divEmployeeBodyModal input[type="checkbox"]').prop('disabled', true);

            $("#cbShowDirectory").prop("checked", ($("#hdncbShowDirectory").val() == "True" ? true : false));

            $("#txtOfficeMobile").text() == (null || "") ? "" : $("#txtOfficeMobile").text($("#txtOfficeMobile").text().slice(0, 4) + "-" + $("#txtOfficeMobile").text().substr(4));
        },

        ElementBinding: function () {
            var s = this;

            $("#btnEdit").click(function () {
                LoadPartial(EmployeeEditURL + "?ID=" + objEmployeeCorporateEmailViewJS.ID, "divEmployeeBodyModal");
            });

        },

        LoadHierarchyUpward: function () {

            var GetSuccessFunction = function (data) {

                var length = data.Result.length;
                var codeValue = $("#hdnOrgCode").val();
                var descValue = $("#hdnOrgDescription").val();
                var parentValue = "";

                $("#divHierarchyUpward").html("");

                $(data.Result).each(function (idx, item) {
                    $("#divHierarchyUpward").append(item.Code + " - " + item.Description + " > ");
                    parentValue = item.Code + " - " + item.Description;
                });

                if (codeValue != "")
                    $("#divHierarchyUpward").append(codeValue + " - " + descValue);
            };

            objEMSCommonJS.GetAjax(OrgTypeHierarchyUpwardURL
                + "&ID=" + $("#hdnParentOrgID").val()
                , {}
                , ""
                , GetSuccessFunction);
        },

    };

    objEmployeeCorporateEmailViewJS.Initialize();
});