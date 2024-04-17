var objEmployeeAddSkillsJS;

$(document).ready(function () {
    objEmployeeAddSkillsJS = {

        Initialize: function () {

            $("#hdnEmployeeID").val($("#divEmployeeModal #hdnID").val());

            $("#divSkillsBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            $("#divSkillsModal #btnSaveSkills").show();
            $("#divSkillsModal .form-control").attr("readonly", false);
            s.ElementBinding();

        },

        ElementBinding: function () {
            var s = this;

            $("#btnSaveSkills").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmSkills", "#divSkillsErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , AddSkillsPostURL \
                        , objEmployeeAddSkillsJS.GetFormData() \
                        , '#divSkillsErrorMessage' \
                        , '#btnSaveSkills' \
                        , objEmployeeAddSkillsJS.AddSuccessFunction);",
                        "function");
                }
            });

			//// Declare some global variables for later use:

			//var container = $("#rating-container");
			//var index = -1;


			//// 1.  Capture the hover event over the div (circle)

			//$(".rating-circle").hover(
			//	// When the mouse hover over any circle. All the circles to the left change color to yellow
			//	function () {
			//		$(this).removeClass("rating-chosen").addClass("rating-hover");
			//		$(this).prevAll().removeClass("rating-chosen").addClass("rating-hover");
			//		$(this).nextAll().removeClass("rating-chosen");
			//	},
			//	//When the mouse move away, the color yellow disappears:	
			//	function () {
			//		$(this).removeClass("rating-hover");
			//		$(this).prevAll().removeClass("rating-hover");

			//		if (index >= 0) {
			//			//Return the previously chosen choice (if any) back in green
			//			// Recall the choice using its index
			//			// "get" returns a DOM element, NOT a jQuery object
			//			var chosenCircle = container.children().get(index);
			//			//Convert to jQuery object
			//			var $rating = $(chosenCircle);

			//			//Make them green again
			//			$rating.addClass("rating-chosen");
			//			$rating.prevAll().addClass("rating-chosen");
			//		}
			//	}
			//);


			//// 2. Capture the click event when the user click on a circle.
			//// All the circles to the left change color to green 
			//// The color stays green as the mouse move away

			//$(".rating-circle").click(
			//	function () {
			//		$(this).addClass("rating-chosen");
			//		$(this).prevAll().addClass("rating-chosen");
			//		// Remember the position of the click so it can be retrieved later
			//		index = $(this).index();
			//		//console.log(index);
			//		$("#ratescore").html((index + 1) + "/10");
			//		$("#getratescore").val(index + 1);
			//	}
			//);



            $("#btnAddSkillsCode").click(function () {
                var isSuccessFunction = function () {
                    $("#divSkillsReferenceModal .close").click(function () {
                        GenerateDropdownValues(GetSkillsRereferenceNewURL, "ddlSkills", "Value", "Description", "", "", false);
                    });
                };
                LoadPartialSuccessFunction(AddAddSkillsReferenceURL, "divSkillsReferenceBodyModal", isSuccessFunction);
                $("#divSkillsReferenceModal #hdnSkillsReferenceCode").val("SKILLS");
                $("#divSkillsReferenceModal #hdnSkillsReferenceField").val("btnAddSkillsCode");
                $("#divSkillsReferenceModal").modal("show");
            });

            var volume = {

                init: function () {
                    $('#volume').on('click', volume.change);
                    $('#volume .control').on('mousedown', volume.drag);
                },

                change: function (e) {
                    e.preventDefault();
                    var percent = helper.getFrac(e, $(this)) * 100;
                    $('#volume .control').animate({ width: percent + '%' }, 100);
                    volume.update(percent);
                },

                update: function (percent) {
                    var ratepercentage = Math.round(percent);
                    $('.vol-box').text(ratepercentage + "%");
                    $("#getratescore").val(ratepercentage);
                    //console.log(percent);
                },

                drag: function (e) {
                    e.preventDefault();
                    $(document).on('mousemove', volume.moveHandler);
                    $(document).on('mouseup', volume.stopHandler);
                },

                moveHandler: function (e) {
                    var holderOffset = $('#volume').offset().left,
                        sliderWidth = $('#volume').width(),
                        posX = Math.min(Math.max(0, e.pageX - holderOffset), sliderWidth);

                    $('#volume .control').width(posX);
                    volume.update(posX / sliderWidth * 100);
                },

                stopHandler: function () {
                    $(document).off('mousemove', volume.moveHandler);
                    $(document).off('mouseup', volume.stopHandler);
                }

            }

            var helper = {
                getFrac: function (e, $this) {
                    return (e.pageX - $this.offset().left) / $this.width();
                }
            }

            volume.init();
        },



        GetFormData: function () {
            var formData = new FormData($('#frmSkills').get(0));
            formData.append("EmployeeSkillsForm.EmployeeID", $("#divEmployeeModal #hdnID").val());
            return formData;
        },

        AddSuccessFunction: function () {
            objSkillsJS.LoadSkillsJQGrid({
                EmployeeID: $("#divEmployeeModal #hdnID").val()
            });
            $("#frmSkills").trigger("reset");
        },
    };

    objEmployeeAddSkillsJS.Initialize();
});