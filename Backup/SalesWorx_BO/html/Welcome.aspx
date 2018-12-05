<%@ Page Title="Welcome" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="Welcome.aspx.vb" Inherits="SalesWorx_BO.Welcome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	
	<tr>
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	<div class="pgtitileposition">
	<span class="pgtitile3">Welcome</span></div>
	<div id="pagenote" >Welcome to SalesWorx v2.0. Use the menu above to access the route planning, product management and other areas of the application.</div>	
	<table width="50%" style="height:180px; background:#dedede url(../images/welcomebg.gif) no-repeat scroll top left;" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	  <tr> 
          <td align="center" valign="middle">
          <span style="color:#ffffff; font-size:22px; background-color:#3399ff; padding:4px 8px;">
          Welcome to <asp:Label ID="lblWelcome" runat="server" Text=""></asp:Label>
          </span>
          <br>
          <br>
          <span>
          Version: <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
          </span>
          </td>
        </tr>

    </table>
	<br/>
	<br/>
	</td> <!-- "contentofpage" ends in this td -->
	</tr>
	
</table>
</asp:Content>
