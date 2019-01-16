<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="ViewReports.aspx.vb" Inherits="SalesWorx_BO.ViewReports" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="//code.jquery.com/ui/1.11.0/themes/smoothness/jquery-ui.css">
<script src="../scripts/jquery-1.3.2.min.js"></script> 
<script src="//code.jquery.com/jquery-1.10.2.js"></script>
  <script src="//code.jquery.com/ui/1.11.0/jquery-ui.js"></script>
<script type="text/javascript">
    

    function pageLoad() {
        if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
        
            $($(":hidden[id*='DatePickers']").val().split(",")).each(function(i, item) {
                if (item != '') {
                    var h = $("table[id*='ParametersGrid'] span").filter(function(i) {
                        var v = "[" + $(this).text() + "]";
                        return (v != null && v.indexOf(item) >= 0);
                    }).parent("td").next("td").find("input").datepicker({
                        showOn: "button"
           , buttonImage: 'http://localhost/swx_vs/images/btnCal.gif'
           , buttonImageOnly: true
           , dateFormat: 'dd-M-yy'
           , changeMonth: true
           , changeYear: true
           , onSelect: function() { return false }
                    });
                }
            });
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" >
	<asp:HiddenField ID="DatePickers" runat="server" />
	<tr>
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	<div class="pgtitileposition">
	<span class="pgtitile3"><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></span></div>
	<table width="100%" id="tableformNrml" style="height:180px; background:#ffffff;" border="0" cellspacing="0" cellpadding="0" >
	  <tr> 
          <td valign="top"  >
        
            <%--  <rsweb:ReportViewer ID="RVMain" runat="server" BorderStyle="Groove" 
                  ProcessingMode="Remote" Width="100%"
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>--%>
        
          </td>
        </tr>

    </table>
	<br/>
	<br/>
	</td> <!-- "contentofpage" ends in this td -->
	</tr>
	
</table>

   
    </asp:Content>
