
document.write("<link rel=\"stylesheet\" href=\"../cal/themes/winter.css\" />");

document.write("<script type=\"text/javascript\" src=\"../cal/utils.js\"></script>");
document.write("<script type=\"text/javascript\" src=\"../cal/calendar.js\"></script>");
document.write("<script type=\"text/javascript\" src=\"../cal/calendar-setup.js\"></script>");

document.write("<script type=\"text/javascript\" src=\"../cal/lang/calendar-en.js\"></script>");


function getCalendar(InputFld,TriggerBtn)
{
//<![CDATA[
      Zapatec.Calendar.setup({
        firstDay          : 6,
        weekNumbers       : false,
        showOthers        : false,
        showsTime         : false,
        timeFormat        : "24",
        step              : 1,
        range             : [2000.01, 2100.12],
        electric          : false,
        singleClick       : true,
        inputField        : InputFld,
        button            : TriggerBtn,
        ifFormat          : "%m/%d/%Y",
        daFormat          : "%Y/%m/%d",
        align             : "Br"
      });
    //]]>
}
