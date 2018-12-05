<%@ Page Title="All Van Requisitions" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="VanRequisitionsListing.aspx.vb" Inherits="SalesWorx_BO.VanRequisitionsListing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">All Van Requisitions</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="1" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td >
                                                &nbsp;
                                            </td>
                                            <td  class="txtSMBold" >Filtered By :</td>
                                            <td class="txtSMBold" >
                                                <asp:DropDownList ID="ddFilterBy" runat="server" AutoPostBack="true" 
                                                    CssClass="inputSM" TabIndex="2">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    <asp:ListItem Value="SalesRep_Name">Van </asp:ListItem>
                                                    <asp:ListItem Value="Emp_Name">Employee </asp:ListItem>
                                                </asp:DropDownList>
                                                &nbsp;</td>
                                            <td  class="txtSMBold" width="10" >
                                                &nbsp;</td>
                                            <td class="txtSMBold">
                                                Value :</td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="inputSM"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="Button1" runat="server" CssClass="btnInput" Text="Search" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td >
                                                &nbsp;</td>
                                            <td  class="txtSMBold" >
                                                Request Date&nbsp; From:</td>
                                            <td class="txtSMBold" >
                                               <table border="0" cellpadding="0" cellspacing="0"><tr><td> <asp:TextBox ID="txtFrom" CssClass="inputSM" runat="server"></asp:TextBox></td><td> <asp:Image ID="imgFrom" runat="server" ImageUrl="~/images/btnCal.gif" /></td></tr></table>
                                               
                                            </td>
                                            <td  class="txtSMBold">
                                                &nbsp;</td>
                                            <td class="txtSMBold">
                                                To:</td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0"><tr><td> <asp:TextBox ID="txtTo" CssClass="inputSM" runat="server"></asp:TextBox></td><td> <asp:Image ID="imgTo" runat="server" ImageUrl="~/images/btnCal.gif" /></td></tr></table>
                                               
                                                </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                    </table>
                                    <ajaxToolkit:CalendarExtender ID="cal1" runat="server"  TargetControlID="txtFrom" PopupButtonID="imgFrom"></ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:CalendarExtender ID="cal2" runat="server"  TargetControlID="txtTo" PopupButtonID="imgTo"></ajaxToolkit:CalendarExtender>
                                                &nbsp;</td>
                                        </tr>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <table border="0" cellspacing="8" cellpadding="0" >
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="gvVanReq" runat="server" EmptyDataText="No details found."
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true" PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                    
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                       
                                                        <asp:BoundField DataField="StockRequisition_ID" HeaderText="Stock Requisition ID" SortExpression="StockRequisition_ID">
                                                            <ItemStyle Wrap="False"  />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField  DataField="Request_Date" HeaderText="Request Date" SortExpression="Request_Date">
                                                            <ItemStyle Wrap="False"  />
                                                             <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SalesRep_Name" HeaderText="Van" SortExpression="SalesRep_Name">
                                                            <ItemStyle Wrap="False"  />
                                                             <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Emp_Name" HeaderText="Employee" SortExpression="Emp_Name">
                                                            <ItemStyle Wrap="False"  />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         
                                                        <asp:TemplateField >
                                                         <ItemTemplate>
                                                            <asp:HyperLink  runat="server"  ID="hyp" Text="Details" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "StockRequisition_ID","VanRequisitionsListingDetail.aspx?id={0}")%>'></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
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
                                                                <asp:Button ID="btnClose" runat="server" Text="Ok"  CssClass="btnInput" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                               <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                             </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>