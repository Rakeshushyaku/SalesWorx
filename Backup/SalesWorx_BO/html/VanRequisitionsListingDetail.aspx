<%@ Page Title="Van Requisition Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="VanRequisitionsListingDetail.aspx.vb" Inherits="SalesWorx_BO.VanRequisitionsListingDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Van Requisition Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                         <table width="100%" cellspacing="3" cellpadding="3"  >
                                                                                             
<%--                                                <tr><td class='txtSMBold' >Stock Requisition ID: &nbsp;<span class='txtSM'>
                                                     <asp:Label ID="lblReID" runat="server" Text=""></asp:Label></span></td> <td align=right  > <div class="no-print"><a href='javascript:print();' class='txtLinkSM'><img src='../images/iconPrinter.gif' border=0 alt='Print'></a></div> </td></tr>--%>
                                                     <tr><td class='txtSMBold' >Request Date:&nbsp; <span class='txtSM'>
                                                     <asp:Label ID="lblReqDate" runat="server" Text=""></asp:Label></span></td> <td align=right  > <div class="no-print"><a href='javascript:print();' class='txtLinkSM'><img src='../images/iconPrinter.gif' border=0 alt='Print'></a></div> </td></tr>
                                                     <tr><td  class='txtSMBold' colspan=2>Van:&nbsp; <span class='txtSM'>
                                                 <asp:Label ID="lblSalesRep" runat="server" Text=""></asp:Label></span></td>
                                                </tr>
                                                 <tr><td class='txtSMBold' colspan=2>Employee:&nbsp; <span class='txtSM'>
                                                     <asp:Label ID="lblEmpName" runat="server" Text=""></asp:Label></span></td></tr>
                                                 <tr><td class='txtSMBold' colspan=2>&nbsp;</td></tr>
                                                 <tr><td class='txtSMBold' colspan=2>Product Details</td></tr>
                                                 <tr><td  colspan=2>
                                                  <asp:GridView Width="100%" ID="gvVanReq" runat="server" EmptyDataText="No product details found."
                                                    EmptyDataRowStyle-Font-Bold="False" EmptyDataRowStyle-HorizontalAlign="Center"   CssClass="oldpagetable" AutoGenerateColumns="False"
                                                    AllowPaging="False"  CellPadding="6">
                                                       <HeaderStyle BackColor="#CFCFCF" />                                              
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                         <asp:BoundField DataField="Item_Code" HeaderText="Item Code" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  DataField="Description" HeaderText="Item" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemStyle Wrap="False" Width="200px" HorizontalAlign="Left"  />
                                                        </asp:BoundField>
                                                       <%-- <asp:BoundField DataField="Brand_Code" HeaderText="Brand Code" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                        </asp:BoundField>--%>
                                                         <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Item_UOM" HeaderText="Item UOM" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                                 </td></tr>
                                                  <tr><td  colspan=2 >
                                                      <asp:HiddenField ID="hfReqID" runat="server" />
                                                      </td></tr> 
                                                  <tr><td  colspan=2 ><div class='txtSMBold' style="display:inline">Comments: </div>
                                                     &nbsp; <asp:Label ID="lblComments" runat="server" Text=""></asp:Label></td></tr> 
                                                   <tr><td  colspan=2 ></td> </tr> 
                                                  <tr><td class='txtSMBold' colspan=2 ><table border="0" cellpadding="0" cellspacing="0"><tr><td align="center">Signature :</td><td><asp:Image ID="imgSig" 
                                                          runat="server"  /></td></tr></table>
                                                      </td></tr> 
                                                     <tr><td class='txtSMBold' colspan=2 align="center"  >
                                                     <div class="no-print">
                                                          <asp:Button ID="Button1" runat="server" CssClass="btnInput" 
                                                             PostBackUrl="~/html/VanRequisitionsListing.aspx" Text="Cancel" /></div>
                                                         </td></tr>
                                                          <tr><td  colspan=2 >&nbsp;</td> </tr> 
                                                         <tr><td  colspan=2 class="no-print">
                                                         <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                          <ajaxtoolkit:modalpopupextender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                                            <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                  <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="Panel1" 
                                                    Drag="true" />
                                                <asp:Panel ID="Panel1" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="table1" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo1" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage1" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                                <asp:Button ID="Button2" runat="server" Text="Ok"  CssClass="btnInput" PostBackUrl="~/html/VanRequisitions.aspx" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                         </td></tr>
                                                </table>
                        
                         
                                               
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
   
</asp:Content>
