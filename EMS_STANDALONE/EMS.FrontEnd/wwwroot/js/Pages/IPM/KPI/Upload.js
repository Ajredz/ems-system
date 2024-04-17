var objKPIUploadJS;

$(document).ready(function () {
    objKPIUploadJS = {

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

            var button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objKPIUploadJS.UploadFile('" + url + "');\">Upload</button>";

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

            objEMSCommonJS.PostAjax(true, url, filedata, "#divModalErrorMessage", "", objKPIUploadJS.UploadSuccessFunction);
            $("#divUploadModal").modal("hide");
        }
    };

    objKPIUploadJS.Initialize();
});