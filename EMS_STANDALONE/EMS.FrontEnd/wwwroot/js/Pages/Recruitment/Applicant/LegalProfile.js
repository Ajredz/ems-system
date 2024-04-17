var objLegalProfileJS;

$(document).ready(function () {
    objLegalProfileJS = {

        ID: $("#divApplicantModal #hdnID").val(),
        IsViewMode: false,

        Initialize: function () {
            var s = this;

            s.LoadLegalProfile(objLegalProfileJS.ID);
        },

        ElementBinding: function () {
            var s = this;

        },

        LoadLegalProfile: function (ID) {
            var s = this
            Loading(true);
            $.ajax({
                url: GetLegalProfileQuestionURL,
                type: "GET",
                data: { ApplicantId: ID },
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                success: function (data) {
                    if (data.IsSuccess) {
                        objLegalProfileJS.PopulateLegalProfileQuestion(data);
                    }
                    else {
                    }
                    Loading(false);
                },
            });
        },

        PopulateLegalProfileQuestion: function (data) {
            $.each(data.Result, function (index, value) {
                $("#DivLegalProfileQuestion").append(''
                    + '<div class="form-group form-fields">'
                    + '<div class="col-md-1 col-label">'
                    + '     <label class="control-label block-label">' + value.RowNum+'. </label>'
                    + '</div>'
                    + '<div class="col-md-6">'
                    + '     <label class="control-label block-label" style="text-indent:0px;">' + value.Description + ' <span class="block-label" style="text-decoration: underline;color:black;"> ' + (value.LegalAnswer == null ? "" : value.LegalAnswer) +'</span></label> '
                    + '</div>'
                    + '<div class="col-md-1">'
                    + '</div>'
                    + '<div class="col-md-4">'
                    + '     <label class="control-label block-label">' + (value.LegalAnswer == null ? "( ) <span style='color:red;'>YES</span> &nbsp; (&#10003;) <span style='color:green;'>NO</span>" :"(&#10003;) <span style='color:red;'>YES</span> &nbsp; ( ) <span style='color:green;'>NO</span>")+'</label>'
                    + '</div>'
                    + '</div>');
            });
        },
    };
    objLegalProfileJS.Initialize();
});