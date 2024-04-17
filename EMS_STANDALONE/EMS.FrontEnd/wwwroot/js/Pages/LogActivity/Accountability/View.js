var objAccountabilityViewJS;
var TypeDropDownOptions = [];

$(document).ready(function () {
    objAccountabilityViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divAccountabilityBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divAccountabilityModal .form-control").prop("disabled", true);
            $("#btnAddAccountability").remove();
            s.GetTypeDropDownOptions();
            s.LoadAccountabilityDetails();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmAccountability").trigger("reset");
            $("#divAccountabilityModal").modal('hide');
        },

        ElementBinding: function () {

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , AccountabilityDeleteURL + '?ID=' + objAccountabilityViewJS.ID\
                    , {} \
                    , '#divAccountabilityErrorMessage' \
                    , '#btnDelete' \
                    , objAccountabilityViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(AccountabilityEditURL + "?ID=" + objAccountabilityViewJS.ID, "divAccountabilityBodyModal");
            });
        },

        GetTypeDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    TypeDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(GetTypeDropDownURL, {}, "", GetSuccessFunction);
        },

        PopulateDropDown: function (id, collection) {
            $(id).append($('<option/>', {
                value: "",
                text: "- Select an Item -"
            }));

            $(collection).each(function (index, item) {
                $(id).append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
        },

        AddAccountabilityDynamicFields: function () {
            var s = this;
            var htmlId = $(".AccountabilityItemDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("AccountabilityItemDynamicFields_", "")) + 1;

            $("#divAccountabilityDynamicFields").append(
                "<div class=\"form-group form-fields AccountabilityItemDynamicFields\" id=\"AccountabilityItemDynamicFields_" + ctr + "\">"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <select id=\"ddlType_" + ctr + "\" title=\"Type\" class=\"form-control required-field TypeDynamicFields\"></select>"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <input type=\"text\" id=\"txtTitle_" + ctr + "\" class=\"form-control required-field TitleDynamicFields\" title=\"Title\" maxlength=\"100\">"
                + "    </div>"
                + "    <div class=\"col-md-2-5 no-padding\">"
                + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\" id=\"txtDescription_" + ctr + "\" class=\"form-control\" maxlength=\"255\" title=\"Description\"></textarea>"
                + "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtOrgGroup_" + ctr + "\" class=\"form-control required-field\" title=\"Org Group\" maxlength=\"100\">"
                + "        <input type=\"hidden\" id=\"hdnOrgGroup_" + ctr + "\" class=\"OrgGroupDynamicFields\">"
                + "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtPosition_" + ctr + "\" class=\"form-control\" title=\"Position\" maxlength=\"100\">"
                + "        <input type=\"hidden\" id=\"hdnPosition_" + ctr + "\" class=\"PositionDynamicFields\">"
                + "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtEmployee_" + ctr + "\" class=\"form-control\" title=\"Employee Name\" maxlength=\"100\">"
                + "        <input type=\"hidden\" id=\"hdnEmployee_" + ctr + "\" class=\"EmployeeDynamicFields\">"
                + "    </div>"
                + "</div>"
            );

            s.PopulateDropDown("#ddlType_" + ctr, TypeDropDownOptions);
        },

        LoadAccountabilityDetails: function () {
            var s = this;
            $("#divAccountabilityDynamicFields").html("");

            var input = { handler: "AccountabilityDetails", ID: objAccountabilityViewJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    s.AddAccountabilityDynamicFields();
                    $("#ddlType_" + idCtr).val(item.Type);
                    $("#txtTitle_" + idCtr).val(item.Title);
                    $("#txtDescription_" + idCtr).val(item.Description);
                    $("#txtOrgGroup_" + idCtr).val(item.OrgGroupDescription);
                    $("#hdnOrgGroup_" + idCtr).val(item.OrgGroupID);
                    $("#txtPosition_" + idCtr).val(item.PositionDescription);
                    $("#hdnPosition_" + idCtr).val(item.PositionID);
                    $("#txtEmployee_" + idCtr).val(item.EmployeeName);
                    $("#hdnEmployee_" + idCtr).val(item.EmployeeID);

                    $("#ddlType_" + idCtr).prop("disabled", true);
                    $("#txtTitle_" + idCtr).prop("disabled", true);
                    $("#txtDescription_" + idCtr).prop("disabled", true);
                    $("#txtOrgGroup_" + idCtr).prop("disabled", true);
                    $("#txtPosition_" + idCtr).prop("disabled", true);
                    $("#txtEmployee_" + idCtr).prop("disabled", true);
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetAccountabilityDetailsURL, input, "", GetSuccessFunction);
        },

    };

    objAccountabilityViewJS.Initialize();
});