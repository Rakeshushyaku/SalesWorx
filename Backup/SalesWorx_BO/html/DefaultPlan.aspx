<%@ Page Title="Default Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="DefaultPlan.aspx.vb" Inherits="SalesWorx_BO.DefaultPlan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="../js/RoutePlanner.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	
		<div class="pgtitileposition">
	<span class="pgtitile3">Setup Default Plan</span></div>
	<div id="pagenote" >This screen may be used for setting up the default plans for all the vans. Holidays or days off for entire team can be setup from here.</div>	
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
	<tr><th></th></tr>
	
	<asp:Panel ID="sitePanel" runat="server" Visible="false" >
	  <tr>
    <%--<td><label>Site :</label> </td>--%>
    <td><label>Organization :</label> </td>
    <td> <asp:DropDownList ID="ddSite" AutoPostBack="true" runat="server" CssClass="inputSM" TabIndex ="1" Width="200px" DataTextField="Site"
    DataValueField="Site" > 
    
              </asp:DropDownList></td>
    </tr>
	</asp:Panel>
 <asp:Panel runat="server" ID="defplan" Visible=false >
 

  <tr>
    <td><label>Default Plans:</label> </td>
    <td> <asp:DropDownList ID="RP_ID" runat="server" CssClass="inputSM" TabIndex ="2" Width="200px" DataTextField="Details"
    DataValueField="Default_Plan_ID" > 
    
              </asp:DropDownList></td>
    </tr>
 
  <tr>
    <td colspan=2 align=right> <asp:Button CssClass="btnInputBlue" ID="AddBtn"  TabIndex ="2" runat="server" Text="Add" />&nbsp;
                <asp:Button CssClass="btnInputGreen" ID="ModBtn" runat="server" TabIndex ="4"  Text="View/Modify" 
                    CausesValidation="False"  />&nbsp;
                <asp:Button CssClass="btnInputRed" ID="DelBtn" runat="server" TabIndex ="5" Text="Delete"  /> </td>
    
    </tr>
     </asp:Panel>
    <tr><th></th></tr>
    </table>
	<br/>
	<br/>
	</td> <!-- "contentofpage" ends in this td -->
	</tr>
	<tr>

	</tr>
</table>
</asp:Content>
