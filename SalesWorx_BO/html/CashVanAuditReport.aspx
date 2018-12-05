<%@ Page Title="Cash Van Audit Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="CashVanAuditReport.aspx.vb" Inherits="SalesWorx_BO.CashVanAuditReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	        <span class="pgtitile3">Cash Van Audit Report</span><asp:ImageButton 
                ID="imgPrint" ImageUrl="~/images/iconPrinter.gif" runat="server" 
                ImageAlign="Right" BorderWidth="0" BorderStyle="None" AlternateText="Print" 
                style="width: 14px"   /></div>
	
	<table width="80%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td >&nbsp;</td>
            <td class="txtSMBold">&nbsp;</td>
                 <td class="txtSMBold" ><asp:Label ID="lblCustVan" runat="server" Text="Van :"></asp:Label></td>
                 <td width="105" class="txtSMBold">
                <asp:DropDownList CssClass="inputSM" ID="ddlVan" runat="server" 
                         DataTextField="SalesRep_Name" DataValueField="SalesRep_ID"/>                        
              </td>
            <td>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>
            </table>
</td>
</tr>
<tr><td>
  
    </td></tr>
	<tr>
       <td style='padding:5px'>
       <br />
        <asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
        <table  border="0" cellspacing="0" cellpadding="0" width='100%' >
        <tr>
        <td  class="tdstyle">
         
       <%--   <asp:Literal ID="Literal1" runat="server" ></asp:Literal>--%>
      <%=DivHTML%>
         
     <%--         <asp:GridView  width="100%" ID="GVVanAudit" runat="server" 
                  EmptyDataText="No survey to display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                     <asp:BoundField DataField="Question_Text" HeaderText="Question"   NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                      <asp:BoundField DataField="ResponseText" HeaderText="Response"   NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    
                  </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>--%>
          </td>
          </tr>
          <tr>
          <td>
             
          </td>
          </tr>
          
        </table>
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
          </Triggers>
        </asp:UpdatePanel>                    
       </td>              
	</tr>
  
    </table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>
