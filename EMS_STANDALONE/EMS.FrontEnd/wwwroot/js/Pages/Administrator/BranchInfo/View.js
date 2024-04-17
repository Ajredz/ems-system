var objBranchInfoViewJS;

$(document).ready(function () {
    objBranchInfoViewJS = {

        ID: $("#divBranchInfoModal #hdnID").val(),

        Initialize: function () {
            $("#divBranchInfoBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            s.LoadHierarchyUpward();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $('#divBranchInfoModal input[type="checkbox"],#divBranchInfoModal select').prop('disabled', true);

            $("#cbIsBranchActive").prop("checked", ($("#hdncbBranchActive").val() == "True" ? true : false));
        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtServiceBay"));

            $("#btnEdit").click(function () {
                LoadPartial(BranchInfoEditURL + "?ID=" + objBranchInfoViewJS.ID, "divBranchInfoBodyModal");
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
                    $("#divHierarchyUpward").append(item.Code + " - " + item.Description + "<span style='color:#395B77;font-size:14px;font-weight: bold;'> > </span>");
                    parentValue = item.Code + " - " + item.Description;
                });

                if (codeValue != "")
                    $("#divHierarchyUpward").append("<span style='color:#395B77;font-weight: bold;font-size:12px;'>" + codeValue + " - " + descValue + "</span>");
            };

            objEMSCommonJS.GetAjax(OrgTypeHierarchyUpwardURL
                + "&ID=" + $("#hdnParentOrgID").val()
                , {}
                , ""
                , GetSuccessFunction);
        },
    };

    objBranchInfoViewJS.Initialize();
});