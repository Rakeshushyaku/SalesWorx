<%@ Page Title="Survey Responses" Language="vb" AutoEventWireup="false" CodeBehind="SurveyResponses.aspx.vb" Inherits="SalesWorx_BO.SurveyResponses" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">

    <title>Survey Responses</title>
    <link href="../styles/salesworx.css" rel="stylesheet" type="text/css"/>
      <link href="../styles/UpdateProgress.css" rel="stylesheet" type="text/css"/>
      <link href="../styles/swxstyle.css" rel="stylesheet" type="text/css"/>
</head>
<body style="font:Trebuchet MS ;font-family:Calibri,Arial,Helvetica;font-size:11px;background-color: #e2e2e2">
    <form id="frmPopupCustList" runat="server" class="outerform">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr><td>
<%=DisplayHTML%>
</td></tr> 
</table> 
<asp:HiddenField ID="hdn_QuestionID" runat="server" Value="" />

</form>
</body>
</html>
