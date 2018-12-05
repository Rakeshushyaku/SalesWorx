<%@ Page Title="Order Return Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="OrderReturnDetails.aspx.vb" Inherits="SalesWorx_BO.OrderReturnDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
<tr>
<td id="contentofpage" width="100%" height="76%" class="topshadow">
    <div class="pgtitileposition">
        <span class="pgtitile3">Credit Notes Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
        <tr>
            <td style="padding: 6px 12px">
                <table width="100%" cellspacing="3" cellpadding="3" >                                                                                             
                    <tr><td class='txtSMBold' >Customer Name: &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblCustName" runat="server" Text=""></asp:Label></span></td> <td align="right"> <div class="no-print"><a href='javascript:print();' class='txtLinkSM'><img src='../images/iconPrinter.gif' border=0 alt='Print'></a></div> </td></tr>
                    <tr><td  class='txtSMBold' colspan="2">Doc Reference :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblOrderRef" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                     <tr><td class='txtSMBold' colspan="2">Customer Ref No :<asp:Label ID="lblCustRefNo" runat="server" Text=""></asp:Label> &nbsp;</td></tr>
                    <tr><td  class='txtSMBold' colspan="2">Creation Date : &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblDate" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                    <tr runat="server" id="trstat"><td class='txtSMBold' colspan="2">Status :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td colspan="2"></td></tr>
                    <tr><td colspan="2" class='txtSMBold'>Return Items</td></tr>
                    <tr><td class='txtSMBold' colspan="2">
                     <asp:GridView  width="100%" ID="GVOrdersDetail" runat="server" 
                            EmptyDataText="No orders return detail to display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />

<EmptyDataRowStyle Font-Bold="True"></EmptyDataRowStyle>
                  <Columns>
                     <asp:BoundField DataField="Item_Code" HeaderText="Item Code"  SortExpression="Item_Code" NullDisplayText="N/A" >
                      <HeaderStyle HorizontalAlign="Left" />
                      <ItemStyle Wrap="False" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Item Description"  SortExpression="Description" NullDisplayText="N/A" >
                      <HeaderStyle HorizontalAlign="Left" />
                      <ItemStyle Wrap="False" HorizontalAlign="Left" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Returned_Quantity" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="center" HeaderText="Qty" SortExpression="Returned_Quantity" DataFormatString="{0:N2}">
<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>
                   <asp:BoundField DataField="Display_UOM" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="center" HeaderText="UOM" >
<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>
                    <%-- <asp:BoundField DataField="Unit_Selling_Price" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="center" HeaderText="Unit Selling Price" Visible="false"  >
<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField> --%>   
                  </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>                    
                    </td></tr>
                    <tr><td colspan="2"></td></tr>
                    <tr><td colspan="2" class='txtSMBold'>Additional Information</td></tr>
                    <tr><td class='txtSMBold'>Credit To : &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblCreditTo" runat="server" Text=""></asp:Label></span></td> <td align=right  > <div class="no-print"></div> </td></tr>
                    <%--<tr><td  class='txtSMBold' colspan="2">Ship To :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblShipTo" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                    <tr><td class='txtSMBold' colspan="2">Invoice To :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblInvoiceTo" runat="server" Text=""></asp:Label></span></td></tr>--%>
                   
                    <tr><td class='txtSMBold' colspan="2">Start Time :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblStartTime" runat="server" Text=""></asp:Label></span></td></tr>
                     <tr><td class='txtSMBold' colspan="2">End Time :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblEndTime" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td class='txtSMBold' colspan="2">Amount :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblOrdAmt" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td class='txtSMBold' colspan="2">Invoice Ref No :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblInvRefNo" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td class='txtSMBold' colspan="2">Reason for return :&nbsp;<asp:Label ID="lblReason" 
                            runat="server"></asp:Label>
                        </td></tr>
                    <tr><td class='txtSMBold' colspan="2">Comments :&nbsp;<asp:Label ID="lblCommemts" 
                            runat="server"></asp:Label>
                        </td></tr>
                    <tr><td class='txtSMBold' colspan="2">Signed By :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblSignedBy" runat="server" Text=""></asp:Label>                    
                    </span></td></tr>
                     <tr><td class='txtSMBold' colspan="2">Signature :&nbsp; <span class='txtSM'>
                    <asp:Image ID="imgSig" runat="server"/>
                    </span></td></tr> 
                </table>
                <div class="no-print">  
                        <asp:HiddenField ID="hdnOrigRef" runat="server" Value=""/>
                    <asp:Button ID="btnBack" CssClass="btnInput" runat="server" Text="Back"/>        
                </div>       
            </td>
        </tr>
    </table>
</td>
</tr>
</table>
</asp:Content>
