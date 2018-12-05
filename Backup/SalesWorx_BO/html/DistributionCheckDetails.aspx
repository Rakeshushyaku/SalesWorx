<%@ Page Title="Distribution Check Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="DistributionCheckDetails.aspx.vb" Inherits="SalesWorx_BO.DistributionCheckDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
<tr>
<td id="contentofpage" width="100%" height="76%" class="topshadow">
    <div class="pgtitileposition">
        <span class="pgtitile3">Distribution Check Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
        <tr>
            <td style="padding: 6px 12px">
                <table width="100%" cellspacing="3" cellpadding="3" >                                                                                             
                    <tr><td class='txtSMBold' >Customer Name: &nbsp;<span class='txtSM'>
                    <asp:Label ID="lblCustName" runat="server" Text=""></asp:Label></span></td> <td align=right  > <div class="no-print"><a href='javascript:print();' class='txtLinkSM'><img src='../images/iconPrinter.gif' border=0 alt='Print'></a></div> </td></tr>
                    <tr><td  class='txtSMBold' colspan="2">Emp Code :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblEmp_Code" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                    <tr runat="server" id="trstat"><td class='txtSMBold' colspan="2">Status :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td class='txtSMBold' colspan="2">Checked On :&nbsp; <span class='txtSM'>
                    <asp:Label ID="lblCheckedDate" runat="server" Text=""></asp:Label></span></td></tr>
                    <tr><td colspan="2"></td></tr>
                    <tr><td colspan="2" class='txtSMBold'>Distribution Items</td></tr>
                    <tr><td class='txtSMBold' colspan="2">
                     <asp:GridView  width="100%" ID="GVDistributionDetail" runat="server" 
                            EmptyDataText="No orders detail to display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />

<EmptyDataRowStyle Font-Bold="True"></EmptyDataRowStyle>
                  <Columns>
                     <asp:BoundField DataField="Item_Code" HeaderText="Item Code"  SortExpression="Item_Code"  NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                      <ItemStyle Wrap="False" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Item Description"  SortExpression="Description" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left"  >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                      <ItemStyle Wrap="False" HorizontalAlign="Left"/>
                    </asp:BoundField> 
                   <%--  <asp:BoundField DataField="IsMSL" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="center" HeaderText="Is MSL" >
<HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>
                     <asp:BoundField DataField="Is_Available" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="center" HeaderText="Is Available" >
<HeaderStyle Wrap="False"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>--%>
                       <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                       <HeaderTemplate ><table width="100">
                       <tr><td colspan="2" align="left">&nbsp;Availability</td></tr>
                       <tr><td align="left" width="50" >Entry</td><td align="left" width="50" >Exit</td></tr> </table> </HeaderTemplate>
                      <ItemTemplate >
                      <table width="100"><tr><td align="left" width="50">  <asp:Image ID="imgEntryAvail" runat="server" />
                       <%-- <asp:Label ID="lblEntryAvail" runat="server" Text=""></asp:Label>--%>
                       </td><td align="left" width="50"> 
                       <asp:Image ID="imgAvail" runat="server" />
                       <%-- <asp:Label ID="lblAvail" runat="server" Text=""></asp:Label>--%>
                        </td></tr></table>
                        
                      </ItemTemplate>
                      </asp:TemplateField>
                    <asp:BoundField DataField="Qty" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" HeaderText="Qty" SortExpression="Qty" DataFormatString="{0:N2}">
<HeaderStyle Wrap="False"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" />
                     </asp:BoundField>
                     <asp:BoundField DataField="Display_UOM" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderText="UOM" SortExpression="Display_UOM">
<HeaderStyle Wrap="False"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" />
                     </asp:BoundField>
                     <asp:BoundField DataField="Expiry_Dt" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left" HeaderText="Expiry Date" SortExpression="Expiry_Dt" DataFormatString = "{0:dd/MM/yyyy}">
<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" />
                     </asp:BoundField>
                       
                     <%-- <asp:TemplateField HeaderText="Exit Availability">
                      <ItemTemplate >
                          <asp:Label ID="lblAvail" runat="server" Text=""></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>--%>
                       
                  </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>                    
                    </td></tr>
                    <tr><td colspan="2"></td></tr>
                </table>
                <div class="no-print">  
                        <asp:HiddenField ID="hdnDistribution_Check_ID" runat="server" Value=""/>
                    <asp:Button ID="btnBack" CssClass="btnInput" runat="server" Text="Back"/>        
                </div>       
            </td>
        </tr>
    </table>
</td>
</tr>
</table>
</asp:Content>
