<%@ Page Title="Customer Listing" Language="vb" AutoEventWireup="false" EnableEventValidation="false"  MasterPageFile="~/html/ReportMasterForASPX.Master" CodeBehind="CustomerListing.aspx.vb" Inherits="SalesWorx_BO.CustomerListing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .style1
    {
        font-size: 11px;
        color: #000000;
        text-decoration: none;
        font-weight: bold;
        height: 32px;
    }
    .style2
    {
        height: 32px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Customer Listing</span><asp:ImageButton ID="imgPrint" ImageUrl="~/images/iconPrinter.gif" runat="server" ImageAlign="Right" BorderWidth="0" BorderStyle="None" AlternateText="Print"   /></div>
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="105" class="style1">Organization :</td>
            <td class="style2"><asp:DropDownList CssClass="inputSM" ID="ddlOrganization" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                </asp:DropDownList>
              </td>
                 <td width="105" class="style1">Customer No:</td>
            <td class="style2"><asp:TextBox ID="txtCustomerNo" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>
          <tr> 
            <td width="105" class="txtSMBold">Segment :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlSegment" 
                    runat="server" DataTextField="Description" DataValueField="Customer_Segment_ID">
                </asp:DropDownList>
            </td>
            <td width="105" class="txtSMBold">Sales District:</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlSalesDist" 
                    runat="server" DataTextField="Description" DataValueField="Sales_District_ID">
                </asp:DropDownList>
            </td>       
          </tr>         
          <tr> 
            <td width="105" class="txtSMBold">Name :</td>
            <td>
               <asp:TextBox ID="txtCustomerName" CssClass="inputSM" runat="server"></asp:TextBox>
            </td>
            <td width="105" class="txtSMBold">Customer Status :</td>
            <td>    
                <asp:DropDownList ID="ddlCust_Stat" CssClass="inputSM" runat="server">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Normal</asp:ListItem>
                    <asp:ListItem Value="2">Blocked</asp:ListItem>
                    <asp:ListItem Value="3">Cash Customer</asp:ListItem>
                </asp:DropDownList>
            </td>       
          </tr> 
         <%-- <tr> 
            <td width="105" class="txtSMBold">&nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="105" class="txtSMBold">Type :</td>
            <td colspan="3">
                <asp:DropDownList CssClass="inputSM" ID="ddlType" 
                    runat="server" DataTextField="Customer_Type" DataValueField="Customer_Type">
                </asp:DropDownList>
            </td>       
          </tr> --%>
        </table>
 </ContentTemplate> </asp:UpdatePanel> 
</td>
</tr>
	<tr>
       <td>
        <asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
        <table  border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td>
              <asp:GridView  width="100%" ID="GVCustomerList" runat="server" 
                  EmptyDataText="No Customers to Display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                  PageSize="25" CellPadding="6" HorizontalAlign="Left" >
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" HorizontalAlign="Left"  />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                     <asp:BoundField DataField="Customer_No" HeaderText="Customer No" HeaderStyle-HorizontalAlign="Left"  SortExpression="Customer_No" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                      <asp:TemplateField HeaderText="Customer Name" SortExpression="Customer_Name" HeaderStyle-HorizontalAlign="Left">
                          <ItemTemplate>
                              <asp:Label ID="Label1" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label> 
                              <asp:Image ID="imgCashCust" ImageUrl="~/images/cashcustomer_icon.gif" runat="server" />
                          </ItemTemplate>
                          <ItemStyle Wrap="False" />
                      </asp:TemplateField>
                    <asp:BoundField DataField="Address" ItemStyle-HorizontalAlign="Left" HeaderText="Address" HeaderStyle-HorizontalAlign="Left" >
                        <ItemStyle HorizontalAlign="Left" />
                     </asp:BoundField>
                     <asp:BoundField DataField="City" ItemStyle-HorizontalAlign="Left" HeaderText="City" HeaderStyle-HorizontalAlign="Left" >
                        <ItemStyle HorizontalAlign="Left" />
                     </asp:BoundField>
                     <asp:BoundField DataField="Postal_Code" ItemStyle-HorizontalAlign="Left" HeaderText="Postal Code" HeaderStyle-HorizontalAlign="Left" >
                        <ItemStyle HorizontalAlign="Left" />
                     </asp:BoundField>                      
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Credit_Limit" HeaderText="Credit Limit"  
                          SortExpression="Credit_Limit" ItemStyle-HorizontalAlign="Right" >
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Credit_Hold" HeaderText="Credit Hold" >
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Cust_Status" HeaderText="Status"  SortExpression="Cust_Status" ItemStyle-HorizontalAlign="Left" >
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                    <%--  <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Cash_Cust" HeaderText="Cash Customer">
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>--%>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Allow_FOC" HeaderText="Allow FOC" ItemStyle-HorizontalAlign="Left" >
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Customer_Segment" HeaderText="Segment"  
                          SortExpression="Customer_Segment" ItemStyle-HorizontalAlign="Left">
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Sales_District" HeaderText="Sales District" 
                          SortExpression="Sales_District" ItemStyle-HorizontalAlign="Left" >
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                  </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>
          </td>
          </tr>
          
        </table>
         <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxtoolkit:modalpopupextender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                                            <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
          </Triggers>
        </asp:UpdatePanel> 
         <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>                  
       </td>              
	</tr>
  
    </table>
      
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>
