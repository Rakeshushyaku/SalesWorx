<%@ Page Title="Plans For Approval" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="PlansForApproval.aspx.vb" Inherits="SalesWorx_BO.PlansForApproval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Plans for Approval</span></div>
	<div id="pagenote" >Plans for Approval shows a list of plans awaiting apporval. Click the Approve link to approve a route plan.</div>	
		
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
		<td ><asp:Label runat="server" ID="ConfirmationMsg"></asp:Label></td>
	</tr>
	
	<tr>
	       <td>
                <asp:GridView width="100%" ID="ApprovalPlans" DataKeyNames="FSR_Plan_ID" 
                    runat="server" AutoGenerateColumns="False"  RowStyle-Wrap="false" 
                    PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" >
                   
                    <Columns>
                        <asp:BoundField  DataField="FSR_Plan_ID" 
                            ShowHeader="False" Visible="False" >
<ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Van" HeaderStyle-HorizontalAlign ="Left">
                            <ItemTemplate>
                                <%--<asp:Label ID="Label1" runat="server" Text='<%# Bind("SalesRep_Name") %>'></asp:Label>--%>
                                <asp:LinkButton CssClass="txtLinkSM" CommandName="Approve" ID="SalesLink" runat="server" Text='<%# Bind("SalesRep_Name") %>' ToolTip="View/Approve Plan"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Route_Plan"  HeaderText="Route Plan" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                         <asp:BoundField DataField="Site"  HeaderText="Site" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="No_Of_Working_Days" HeaderText="Working Days" />
                        <asp:BoundField DataField="No_Of_Visits" HeaderText="No Of Visits" />
                        <asp:TemplateField HeaderText="Approval">
                            <ItemTemplate>
                                <%--<asp:Label ID="Label1" runat="server" Text='<%# Bind("SalesRep_Name") %>'></asp:Label>--%>
                                <asp:LinkButton CssClass="txtLinkSM" CommandName="Approval" ID="SalesApprovalLink" runat="server" Text="Approve" ToolTip="Approve Plan"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                    </Columns>
                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                </asp:GridView>
                
            </td>
              <td><asp:Label ID="MsgLbl" runat="server" Cssclass='txtSM'></asp:Label></td>
	</tr>
  
    </table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>
