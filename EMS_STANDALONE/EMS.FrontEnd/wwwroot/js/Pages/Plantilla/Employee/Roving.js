var objRovingListJS;
var RovingtListURL = "/Plantilla/Employee/Movement?handler=List";

$(document).ready(function () {
    objRovingListJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            var param = {
                EmployeeID: $("#divEmployeeModal #hdnID").val(),
                EmployeeFieldDelimited: localStorage["EmployeeMovementListEmployeeFieldDelimited"],
                MovementTypeDelimited: localStorage["EmployeeMovementListMovementTypeDelimited"],
                From: localStorage["EmployeeMovementListFrom"],
                To: localStorage["EmployeeMovementListTo"],
                DateEffectiveFromFrom: localStorage["EmployeeMovementListDateEffectiveFromFrom"],
                DateEffectiveFromTo: localStorage["EmployeeMovementListDateEffectiveFromTo"],
                DateEffectiveToFrom: localStorage["EmployeeMovementListDateEffectiveToFrom"],
                DateEffectiveToTo: localStorage["EmployeeMovementListDateEffectiveToTo"],
                Reason: localStorage["EmployeeMovementListReason"],
                CreatedDateFrom: localStorage["EmployeeMovementListCreatedDateFrom"],
                CreatedDateTo: localStorage["EmployeeMovementListCreatedDateTo"],
                CreatedByDelimited: localStorage["EmployeeMovementListCreatedByDelimited"],
                HRDComments: localStorage["EmployeeMovementListHRDComments"],
                IsShowActiveOnly: $("#cbShowActiveOnly").prop("checked")
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();


        }




        };
    objRovingListJS.Initialize();
});