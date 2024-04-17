var objKPIPositionUploadJS;

$(document).ready(function () {
    objKPIPositionUploadJS = {

        Initialize: function () {
            //var s = this;
            //s.ElementBinding();
            //$("#divKPIPositionModal #btnSave").show();
            //$("#divKPIPositionModal .form-control").attr("readonly", false);
            //$("#divKPIPositionModal #btnDelete, #divKPIPositionModal #btnBack").remove();
            //s.GetKPIDropDownOptions();
        },

        UploadModal: function (url, header, sampleFormURL) {
            $("#divModalErrorMessage").html("");
            $('#fileUpload').removeClass("errMessage");

            var button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objKPIPositionUploadJS.UploadFile('" + url + "');\">Upload</button>";

            if (header != "")
                header += "<button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>";
            $("#pheader").html(header);

            if (sampleFormURL != "")
                button += "&nbsp <button type=\"button\" id=\"btnDownloadForm\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objEMSCommonJS.UploadModalDownloadTemplate('" + sampleFormURL + "');\">Download Template</button>";

            $("#fileUpload").val("");
            $("#divButtonUploadModal").html(button);
            $("#divUploadModal").modal("show");
        },

        UploadSuccessFunction: function () {
            $("#btnSearch").click();
        },

        UploadFile: function (url) {
            var fileExtension = ['xls', 'xlsx'];
            var filename = $('#fileUpload').val();
            var filedata = new FormData();
            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;

            //--- Validation for excel file---  
            if (filename.length == 0) {
                $("#divModalErrorMessage").html("<label class=\"errMessage\"><li>Please select a file.</li></label><br />");
                return false;
            }
            else {
                var extension = filename.replace(/^.*\./, '');
                if ($.inArray(extension, fileExtension) == -1) {
                    $("#divModalErrorMessage").html("<label class=\"errMessage\"><li>Please select only excel files.</li></label><br />");
                    return false;
                }
                else {
                    $("#divModalErrorMessage").html("");
                }
            }

            filedata.append(files[0].name, files[0]);

            objKPIPositionUploadJS.ValidateUploadAjax(true, url, filedata, "#divModalErrorMessage", "", objKPIPositionUploadJS.UploadSuccessFunction);
            $("#divUploadModal").modal("hide");
        },

        UploadValidatedFile: function (url) {
            var filedata = new FormData();
            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;

            filedata.append(files[0].name, files[0]);

            objEMSCommonJS.PostAjax(true, url, filedata, "#divModalErrorMessage", "", objEMSCommonJS.UploadSuccessFunction);
            $("#divConfirmUploadModal").modal("hide");
        },

        ValidateUploadAjax: function (isForm, url, input, divErrID, btnID, isSuccessFunction, isFailedFunction, NoModalAlert) {
            var contentType = isForm ? false : "application/json;";
            var processData = isForm ? false : true;

            var s = this;
            $(btnID).prop("disabled", true);
            Loading(true);
            $.ajax({
                url: url,
                type: "POST",
                data: input,
                dataType: "json",
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                contentType: contentType,
                processData: processData,
                success: function (data) {
                    if (data.IsSuccess) {
                        if (isSuccessFunction != null) {
                            if (data.Result == "true") {
                                objEMSCommonJS.PostAjax(true, "/IPM/KPIPosition/UploadInsert?handler=UploadInsertKPIPosition", input, "#divModalErrorMessage", "", objEMSCommonJS.UploadSuccessFunction);
                            }
                            else {
                                var url = "/IPM/KPIPosition/UploadInsert?handler=UploadInsertKPIPosition";
                                var button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objKPIPositionUploadJS.UploadValidatedFile('" + url + "');\">Yes</button>";
                                button += "&nbsp <button type=\"button\"  class=\"btnBlue formbtn button-width-auto\" data-dismiss=\"modal\">No</button>";
                                $("#divButtonConfirmUploadModal").html(button);
                                $("#divConfirmUploadModal").modal("show");
                            }
                        }

                        //if (!NoModalAlert)
                        //    ModalAlert(MODAL_HEADER, data.Result);

                        $(divErrID).html("");
                    }
                    else {
                        var msg = "";
                        if (data.IsListResult == true) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + "<br />";
                            }
                        } else {
                            msg += data.Result;
                        }
                        ModalAlert(MODAL_HEADER, msg);
                        if (isFailedFunction != null)
                            isFailedFunction();
                    }
                    $(btnID).prop("disabled", false);
                    Loading(false);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (isFailedFunction != null)
                        isFailedFunction();
                    Loading(false);
                    $(btnID).prop("disabled", false);
                    ModalAlert(MODAL_HEADER, jqXHR.responseText);
                },
            });
        }
    };

    objKPIPositionUploadJS.Initialize();
});