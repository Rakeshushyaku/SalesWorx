<%@ Page Title="Orders Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="OrdersDetails.aspx.vb" Inherits="SalesWorx_BO.OrdersDetails" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
<tr>
<td id="contentofpage" width="100%" height="76%" class="topshadow">
    <div class="pgtitileposition">
        <span class="pgtitile3">Orders Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
        <tr>
            <td style="padding: 6px 12px">
                <table width="100%" cellspacing="3" cellpadding="3" >                                                                                             
                    <tr><td class='txtSMBold' >Customer Name: &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblCustName" runat="server" Text=""></asp:Label></span></td> <td align=right  > <div class="no-print"><a href='javascript:print();' class='txtLinkSM'><img src='../images/iconPrinter.gif' border=0 alt='Print'></a></div> </td></tr>
                    <tr><td  class='txtSMBold' colspan="2">Doc Reference :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblOrderRef" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                     <tr><td class='txtSMBold'  colspan="2">Creation Date : &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblOrderDate" runat="server" Text=""></asp:Label></span></tr>
                    <tr runat="server" id="trstat"><td class='txtSMBold' colspan="2">Status :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td class='txtSMBold' colspan="2">Order Type :
                        <asp:Label ID="lblOrderType" runat="server"></asp:Label>
                        </td></tr>
                    <tr><td colspan="2"></td></tr>
                    <tr><td colspan="2" class='txtSMBold'>Order Items</td></tr>
                    <tr><td class='txtSMBold' colspan="2">
                     <asp:GridView  width="100%" ID="GVOrdersDetail" runat="server" 
                            EmptyDataText="No orders detail to display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />

<EmptyDataRowStyle Font-Bold="True"></EmptyDataRowStyle>
                  <Columns>
                     <asp:BoundField DataField="Item_Code" HeaderText="Item Code"  SortExpression="Item_Code" NullDisplayText="N/A" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"  >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Item Description"  SortExpression="Description" NullDisplayText="N/A" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"  >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                      <ItemStyle Wrap="False" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Ordered_Quantity" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderText="Qty" SortExpression="Ordered_Quantity" DataFormatString="{0:N2}">
<HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                              </asp:BoundField>
                      <asp:BoundField DataField="Display_UOM" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderText="UOM" >
                      
<HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                      
                     </asp:BoundField>
                 <asp:BoundField DataField="Def_Bonus" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" HeaderText="Def. Bns" Visible="false"  >
<HeaderStyle Wrap="False"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField> 
                      <asp:BoundField DataField="ItemPrice" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"  HeaderText="Price"   >
<HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

                      <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>     
                  </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>                    
                    </td></tr>
                    <tr><td colspan="2" align="right" class="txtSMBold">Order Amount :
                        <asp:Label ID="lblOrderAmount" runat="server"></asp:Label>
&nbsp;&nbsp; &nbsp; &nbsp;  </td></tr>
                    <tr><td colspan="2" class='txtSMBold'>Additional Information</td></tr>
                    <tr><td class='txtSMBold'>Customer PO : &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblCustomerPO" runat="server" Text=""></asp:Label></span></td> <td align=right  >  </td></tr>
                    <%--<tr><td  class='txtSMBold' colspan="2">Ship To :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblShipTo" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                    <tr><td class='txtSMBold' colspan="2">Invoice To :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblInvoiceTo" runat="server" Text=""></asp:Label></span></td></tr>--%>
                   
                    <!--  <tr><td  class='txtSMBold' colspan="2">Request Date :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblRequestDate" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                    <tr><td class='txtSMBold' colspan="2">Ship Date :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblShipDate" runat="server" Text=""></asp:Label></span></td></tr> -->
                     <tr><td class='txtSMBold' colspan="2">Start Time :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblStartTime" runat="server" Text=""></asp:Label></span></td></tr>
                     <tr><td class='txtSMBold' colspan="2">End Time :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblEndTime" runat="server" Text=""></asp:Label></span></td></tr>
                   <!-- <tr><td class='txtSMBold' colspan="2">Shipping Instructions :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblShipInstr" runat="server" Text=""></asp:Label></span></td></tr> -->
                    <tr><td class='txtSMBold' colspan="2">Comments :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblPackInstr" runat="server" Text=""></asp:Label></span></td></tr>
                   
                    <tr><td class='txtSMBold' colspan="2">Signed By :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblSignedBy" runat="server" Text=""></asp:Label>
                    </span></td></tr>
                      <tr runat="server" id="trSign"><td class='txtSMBold' colspan="2" >
                          <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>Signature :&nbsp; <span class='txtSM'>
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
