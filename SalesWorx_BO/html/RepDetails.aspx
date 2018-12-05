<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RepDetails.aspx.vb" Inherits="SalesWorx_BO.RepDetails" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body { margin:0; }
        .RadPanelBar_Simple a.rpLink, .RadPanelBar_Simple div.rpLink, .RadPanelBar_Simple a.rpLink:hover,
        .RadPanelBar_Simple a.rpSelected, .RadPanelBar_Simple div.rpSelected, .RadPanelBar_Simple a.rpSelected:hover  {
    background-color: #999 !important;
    border-color: #999 !important;
    color:#fff !important;
}
 
    .RadPanelBar_Simple .rpExpandable span.rpExpandHandle{
    background-position: 100% -5px !important;
}

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
}
        #RVMain
        {
            display:block !important;
        }


    </style>

</head>
<body>
    <form id="form1" runat="server">
    
	  <div style="overflow:auto; height:530px; border:#ccc solid 0px;" id="ReportWrapper" runat="server">
  <rsweb:ReportViewer ID="RVMain" runat="server"      ShowBackButton ="true" 
                  ProcessingMode="Remote" 
                    DocumentMapWidth="100%"  AsyncRendering="false" SizeToReportContent="true" > 
              </rsweb:ReportViewer>    
	 </div> 
    </form>
</body>
</html>
