<%@ Page Title="Survey Statistics" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="SurveyStatistics.aspx.vb" Inherits="SalesWorx_BO.SurveyStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript">
function openPopup(contentPage, wndName, w, h, scroll) 
	{
		var winl = (screen.width - w) / 2;
		var wint = (screen.height - h) / 2;
		var winprops = 'height='+h+',width='+w+',top='+wint+',left='+winl+',scrollbars='+scroll+',resizable=no'
		wndHandle = window.open(contentPage, wndName, winprops)
		if (parseInt(navigator.appVersion) >= 4) { wndHandle.window.focus(); }
		return wndHandle;
	}

	function showResponses(question_id,typeCode)
	{
	    openPopup('SurveyResponses.aspx?Question_ID=' + question_id + '&typecode=' + typeCode, 'SurveyQuestResp', 700, 700, '1')
	}	
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Survey Statistics</span><asp:ImageButton ID="imgPrint" ImageUrl="~/images/iconPrinter.gif" runat="server" ImageAlign="Right" BorderWidth="0" BorderStyle="None" AlternateText="Print"   /></div>
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr><td colspan="2"><asp:Label ID="lbl_Msg" runat="server" Text=""></asp:Label></td></tr>
	<tr>
<td>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="105" class="txtSMBold">Survey :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlSurvey" runat="server" AutoPostBack="true" DataTextField="Survey_title" DataValueField="Survey_Id"/>
            </td>
                 <td width="105" class="txtSMBold">
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
            <td colspan="3">&nbsp;
                     </td>
          </tr>
            <tr>
                <td width="105" class="txtSMBold">
                <asp:Label ID="lblStartDatetxt" runat="server" Text="Start Date :" Visible="false"></asp:Label></td>
                <td>
                <asp:Label ID="lblStartDateval" runat="server" Text=""></asp:Label>
                </td>
                <td width="105" class="txtSMBold"><asp:Label ID="lblEndDatetxt" runat="server" Text="End Date :" Visible="false"></asp:Label></td>
                <td><asp:Label ID="lblEndDateval" runat="server" Text=""></asp:Label>
                </td>
            </tr>      
        </table>
</td>
</tr>
    <tr><td></td></tr>
	<tr><td><div><%=DisplayHTML%></div></td></tr>
	</table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>	
</asp:Content>
