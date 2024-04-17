var objPreloadedViewJS;
var ModuleDropDownOptions = [];
var TypeDropDownOptions = [];
var SubTypeDropDownOptions = [];

$(document).ready(function () {
    objPreloadedViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divPreloadedBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divPreloadedModal .form-control").prop("disabled", true);
            $("#lnkLogActivity, #btnAddLogActivity").remove();
            //s.GetModuleDropDownOptions();
            s.GetTypeDropDownOptions();
            s.LoadPreloadedItems();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmPreloaded").trigger("reset");
            $("#divPreloadedModal").modal('hide');
        },

        ElementBinding: function () {

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , PreloadedDeleteURL + '?ID=' + objPreloadedViewJS.ID\
                    , {} \
                    , '#divPreloadedErrorMessage' \
                    , '#btnDelete' \
                    , objPreloadedViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(PreloadedEditURL + "?ID=" + objPreloadedViewJS.ID, "divPreloadedBodyModal");
            });
        },

        //GetModuleDropDownOptions: function () {
        //    var s = this;
        //    var GetSuccessFunction = function (data) {
        //        $(data.Result).each(function (index, item) {
        //            ModuleDropDownOptions.push(
        //                {
        //                    Value: item.Value,
        //                    Text: item.Description
        //                });
        //        });
        //    };

        //    objEMSCommonJS.GetAjax(GetModuleDropDownURL, {}, "", GetSuccessFunction);
        //},

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

        GetSubTypeDropDownOptions: function (type) {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    SubTypeDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(GetSubTypeDropDownURL + "&Type=" + type, {}, "", GetSuccessFunction);
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

        AddLogActivityItemDynamicFields: function () {
            var s = this;
            var htmlId = $(".LogActivityItemDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("LogActivityItemDynamicFields_", "")) + 1;

            $("#divLogActivityItemDynamicFields").append(
                "<div class=\"form-group form-fields LogActivityItemDynamicFields\" id=\"LogActivityItemDynamicFields_" + ctr + "\">"
                //+ "    <div class=\"col-md-2 no-padding\">"
                //+ "        <select id=\"ddlModule_" + ctr + "\" title=\"Module\" class=\"form-control required-field ModuleDynamicFields\"></select>"
                //+ "    </div>"
                + "    <div class=\"col-md-0-5 no-padding\">"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <select id=\"ddlType_" + ctr + "\" title=\"Type\" class=\"form-control required-field TypeDynamicFields\"></select>"
                + "    </div>"
                //+ "    <div class=\"col-md-1-5 no-padding\">"
                //+ "        <select id=\"ddlSubType_" + ctr + "\" title=\"Sub-Type\" class=\"form-control required-field SubTypeDynamicFields\"></select>"
                //+ "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtTitle_" + ctr + "\" class=\"form-control required-field TitleDynamicFields\" title=\"Title\" maxlength=\"100\">"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                //+ "        <input type=\"text\" id=\"txtDescription_" + ctr + "\" class=\"form-control DescriptionDynamicFields\" title=\"Description\" maxlength=\"255\">"
                + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\" id=\"txtDescription_" + ctr + "\" class=\"form-control\" maxlength=\"255\" title=\"Description\"></textarea>"
                + "    </div>"
                //+ "    <div class=\"col-md-1 text-align-center no-padding\">"
                //+ "        <input type=\"checkbox\" id=\"chkIsPassFail_" + ctr + "\" class=\"IsPassFailDynamicFields\" title=\"IsPassFail\">"
                //+ "    </div>"
                //+ "    <div class=\"col-md-1 text-align-center no-padding\">"
                //+ "        <input type=\"checkbox\" id=\"chkIsAssignment_" + ctr + "\" class=\"IsAssignmentDynamicFields\" title=\"IsAssignment\">"
                //+ "    </div>"
                + "    <div class=\"col-md-1-5 text-align-center no-padding\">"
                + "        <input type=\"checkbox\" id=\"chkIsVisible_" + ctr + "\" class=\"IsVisibleDynamicFields\" title=\"IsVisible\">"
                + "    </div>"
                + "    <div class=\"col-md-2-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtAssignedUser_" + ctr + "\" class=\"form-control\" title=\"Assigned User\" maxlength=\"100\">"
                + "        <input type=\"hidden\" id=\"hdnAssignedUserID_" + ctr + "\" class=\"form-control AssignedUserDynamicFields\">"
                + "    </div>"
                + "</div>"
            );

            //s.PopulateDropDown("#ddlModule_" + ctr, ModuleDropDownOptions);
            s.PopulateDropDown("#ddlType_" + ctr, TypeDropDownOptions);
            //s.PopulateDropDown("#ddlSubType_" + ctr, SubTypeDropDownOptions);
        },

        LoadPreloadedItems: function () {
            var s = this;
            $("#divLogActivityItemDynamicFields").html("");

            var input = { ID: objPreloadedViewJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    //s.GetSubTypeDropDownOptions(item.Type);
                    s.AddLogActivityItemDynamicFields();
                    //$("#ddlModule_" + idCtr).val(item.Module);
                    $("#ddlType_" + idCtr).val(item.Type);
                    //$("#ddlSubType_" + idCtr).val(item.SubType);
                    $("#txtTitle_" + idCtr).val(item.Title);
                    $("#txtDescription_" + idCtr).val(item.Description);
                    //$("#chkIsPassFail_" + idCtr).val(item.IsPassFail);
                    //$("#chkIsPassFail_" + idCtr).attr('checked', item.IsPassFail);
                    //$("#chkIsAssignment_" + idCtr).val(item.IsAssignment);
                    //$("#chkIsAssignment_" + idCtr).attr('checked', item.IsAssignment);
                    $("#chkIsVisible_" + idCtr).val(item.IsVisible);
                    $("#chkIsVisible_" + idCtr).attr('checked', item.IsVisible);
                    $("#txtAssignedUser_" + idCtr).val(item.AssignedUserName);

                    //$("#ddlModule_" + idCtr).prop("disabled", true);
                    $("#ddlType_" + idCtr).prop("disabled", true);
                    //$("#ddlSubType_" + idCtr).prop("disabled", true);
                    $("#txtTitle_" + idCtr).prop("disabled", true);
                    $("#txtDescription_" + idCtr).prop("disabled", true);
                    //$("#chkIsPassFail_" + idCtr).prop("disabled", true);
                    //$("#chkIsAssignment_" + idCtr).prop("disabled", true);
                    $("#chkIsVisible_" + idCtr).prop("disabled", true);
                    $("#txtAssignedUser_" + idCtr).prop("disabled", true);
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetPreloadedItemsURL, input, "", GetSuccessFunction);
        },

    };

    objPreloadedViewJS.Initialize();
});