//Default Plan Validation
function CheckDropDownDefault() {
      var RP_ID = document.getElementById("ctl00_ContentPlaceHolder1_RP_ID");
    if (RP_ID.selectedIndex == 0) {
        alert('Please select a plan.');
        return false;
    }
}

function CheckDelItemDefault() {

    var RP_ID = document.getElementById("ctl00_ContentPlaceHolder1_RP_ID");
    if (RP_ID.selectedIndex == 0) {
        alert('Please select a plan.');
        return false;
    }

    if (RP_ID.options[RP_ID.selectedIndex].style.color.toUpperCase() == "BLUE") {
        alert("This plan is in use and cannot be deleted.");
        RP_ID.focus();
        return false;
    }

    var Ret = confirm('Are you sure to delete this item?');
    if (Ret == true) {
        return true;
    }
    else {
        return false;
    }
}

//Route Plan Validation
function CheckDropDownRoute() {

    var DP_ID = document.getElementById("ctl00_ContentPlaceHolder1_Default_Plan_DD");
    if (DP_ID.selectedIndex == 0) {
        alert('Please select a plan.');
        return false;
    }
}


function CheckEmpty() {
    var TargetBaseControl = null;
    var TargetChildControl = document.getElementById('ctl00_ContentPlaceHolder1_CommentsTxt');

    if (TargetChildControl.value == "") {
        alert('Please Enter Comments')
        return false
    }
    else
        return true
}

function CheckDropDown() {
    var Route_ID = document.getElementById("ctl00_ContentPlaceHolder1_Route_ID");
    if (Route_ID.selectedIndex == 0) {
        alert('Please select a plan.');
        return false;
    }
}

function CheckDelItem() {

    var Route_ID = document.getElementById("ctl00_ContentPlaceHolder1_Route_ID");
    if (Route_ID.selectedIndex == 0) {
        alert('Please select a plan.');
        return false;
    }

    if (Route_ID.options[Route_ID.selectedIndex].style.color.toUpperCase() == "GREEN") {
        alert("This plan is in use and cannot be deleted.");
        Route_ID.focus();
        return false;
    }

    var Ret = confirm('Are you sure to delete this item?');
    if (Ret == true) {
        return true;
    }
    else {
        return false;
    }
}

//Validation On Copy Route Plan

function ValidateForm() {
    objForm = document.getElementById("aspnetForm");
    if (objForm.ctl00_ContentPlaceHolder1_Route_ID.selectedIndex == 0) {
        alert("Please select a route plan to copy from.");
        objForm.ctl00_ContentPlaceHolder1_Route_ID.focus();
        return false;
    }
    if (objForm.ctl00_ContentPlaceHolder1_RP_ID.selectedIndex == 0) {
        alert("Please select a default plan to copy to.");
        objForm.ctl00_ContentPlaceHolder1_RP_ID.focus();
        return false;
    }

    if (planExists(objForm, objForm.ctl00_ContentPlaceHolder1_RP_ID.value)) {
        if (!confirm("You have already defined a route plan for the selected default plan. Are you sure you want to overwrite ?")) {
            objForm.ctl00_ContentPlaceHolder1_RP_ID.focus();
            return false;
        }
    }

    //  return true 
}

function planExists(objForm, rp_id) {
    var oRoutePlans = objForm.ctl00_ContentPlaceHolder1_Route_ID.options;
    var i;
    for (i = 0; i < oRoutePlans.length; i++) {
        if (oRoutePlans[i].Default_Plan_id == rp_id) {
            return true;
        }
    }
    return false;
}



function ValidateFormWeekDays() {
    objForm = document.getElementById("aspnetForm");
    if (objForm.ctl00_ContentPlaceHolder1_Route_ID.selectedIndex == 0) {
                alert("Please select a route plan to copy from.");
                objForm.ctl00_ContentPlaceHolder1_Route_ID.focus();
                return false;
            }
            if (objForm.ctl00_ContentPlaceHolder1_RP_ID.selectedIndex == 0) {
                alert("Please select a default plan to copy to.");
                objForm.ctl00_ContentPlaceHolder1_RP_ID.focus();
                return false;
            }


            if (planExists(objForm, objForm.ctl00_ContentPlaceHolder1_RP_ID.value)) {
                if (!confirm("You have already defined a route plan for the selected default plan. Are you sure you want to overwrite ?")) {
                    objForm.RP_ID.focus();
                    return false;
                }
            }
                
              //  return true 
        }

        function planExistsWeekDays(objForm, rp_id) {
            var oRoutePlans = objForm.ctl00_ContentPlaceHolder1_Route_ID.options;
            var i;
            for (i = 0; i < oRoutePlans.length; i++) {
                if (oRoutePlans[i].Default_Plan_id == rp_id) {
                    return true;
                }
            }
            return false;
        }
