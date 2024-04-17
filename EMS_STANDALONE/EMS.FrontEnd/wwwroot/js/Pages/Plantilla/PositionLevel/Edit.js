﻿var objPositionLevelEditJS;

$(document).ready(function () {
    objPositionLevelEditJS = {
        
        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divPositionLevelBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass(".unreqField");
            $(".reqField").removeClass(".reqField");
            $("#btnEdit").hide();
            $("#btnSave, #btnBack").show();
            $("#divPositionLevelModal .form-control").attr("readonly", false);
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divPositionLevelModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , PositionLevelDeleteURL + '?ID=' + objPositionLevelEditJS.ID \
                    , {} \
                    , '#divPositionLevelErrorMessage' \
                    , '#btnDelete' \
                    , objPositionLevelEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmPositionLevel", "#divPositionLevelErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , PositionLevelEditPostURL \
                        , new FormData($('#frmPositionLevel').get(0)) \
                        , '#divPositionLevelErrorMessage' \
                        , '#btnSave' \
                        , objPositionLevelEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(PositionLevelViewURL + "?ID=" + objPositionLevelEditJS.ID, "divPositionLevelBodyModal");
            });

        },

    };
    
     objPositionLevelEditJS.Initialize();
});