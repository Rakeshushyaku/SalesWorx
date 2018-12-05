<%@ Page Title="Welcome" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Welcome.aspx.vb" Inherits="SalesWorx_BO.Welcome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Welcome </h4>
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
	 
</asp:Content>
