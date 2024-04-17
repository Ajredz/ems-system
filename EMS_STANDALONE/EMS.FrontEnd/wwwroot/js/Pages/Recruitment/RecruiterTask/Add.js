var objRecruiterTaskAddJS;
var ResultTypeDropDownOptions = [];

$(document).ready(function () {
    objRecruiterTaskAddJS = {

        Initialize: function () {
            $("#divRecruiterTaskBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#btnSave").show();
            $("#divRecruiterTaskModal .form-control").attr("readonly", false);
            $("#btnDelete #btnBack").remove();
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmRecruiterTask").trigger("reset");
        },

        ElementBinding: function () {
            var s = this;

            $("#txtRecruiter").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: RecruiterAutoCompleteURL, // URL 
                        type: "GET",
                        dataType: "json",
                        data: {
                            Term: $("#txtRecruiter").val(),
                            TopResults: 20
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                response($.map(data.Result, function (item) {
                                    return {
                                        label: item.Description,
                                        value: item.ID
                                    };
                                }))
                            }
                            else {
                                ModalAlert(MODAL_HEADER, data.Result);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            ModalAlert(MODAL_HEADER, jqXHR.responseText);
                        }
                    })
                },
                select: function (event, ui) { // Event - triggers after selection on list
                    if (ui.item.label != null) {
                    }
                    return false;
                },
                change: function (event, ui) { // Event - triggers when the value of the textbox changed
                    if (ui.item == null) {
                        $("#hdnRecruiterID").val(0)
                        $(this).val("");
                    } else {
                        $("#hdnRecruiterID").val(ui.item.value)
                        $(this).val(ui.item.label)
                    }
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            $("#txtApplicant").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: ApplicantAutoCompleteURL, // URL 
                        type: "GET",
                        dataType: "json",
                        data: {
                            Term: $("#txtApplicant").val(),
                            TopResults: 20
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                response($.map(data.Result, function (item) {
                                    return {
                                      label: item.Description,
                                      value: item.ID
                                    };
                                }))
                            }
                            else {
                              ModalAlert(MODAL_HEADER, data.Result);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            ModalAlert(MODAL_HEADER, jqXHR.responseText);
                        }
                    })
                },
                select: function (event, ui) { // Event - triggers after selection on list
                    if (ui.item.label != null) {
                    }
                    return false;
                },
                change: function (event, ui) { // Event - triggers when the value of the textbox changed
                    if (ui.item == null) {
                        $("#hdnApplicantID").val(0)
                        $(this).val("");
                    } else {
                        $("#hdnApplicantID").val(ui.item.value)
                        $(this).val(ui.item.label)
                    }
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            $("#divRecruiterTaskModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmRecruiterTask", "#divRecruiterTaskErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                    "objEMSCommonJS.PostAjax(true \
                            , RecruiterTaskAddPostURL \
                            , new FormData($('#frmRecruiterTask').get(0)) \
                            , '#divRecruiterTaskErrorMessage' \
                            , '#divRecruiterTaskModal #btnSave' \
                            , objRecruiterTaskAddJS.AddSuccessFunction);", "function");
                }
            });
        },
    };

    objRecruiterTaskAddJS.Initialize();
});