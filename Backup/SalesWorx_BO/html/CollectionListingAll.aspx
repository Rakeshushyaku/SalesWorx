<%@ Page Title="Collections" Language="vb" AutoEventWireup="false" EnableEventValidation="false"  MasterPageFile="~/html/ReportMasterForASPX.Master"
    CodeBehind="CollectionListingAll.aspx.vb" Inherits="SalesWorx_BO.CollectionListingAll" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var TargetBaseControl = null;

        window.onload = function() {
            try {
                TargetBaseControl =
           document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');
            }
            catch (err) {
                TargetBaseControl = null;
            }
        }        
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Collections</span><asp:ImageButton ID="imgPrint" ImageUrl="~/images/iconPrinter.gif" runat="server" ImageAlign="Right" BorderWidth="0" BorderStyle="None" AlternateText="Print"   /></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="105" class="txtSMBold">
                                        Organization:
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="inputSM" ID="ddlOrganization" runat="server" DataTextField="Description"
                                            DataValueField="MAS_Org_ID" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="105" class="txtSMBold">
                                       Van:
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="inputSM" ID="ddVan" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                                        </asp:DropDownList> &nbsp;
                                        <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="105" class="txtSMBold">
                                        From Date :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                                        <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd/MM/yyyy"
                                            runat="server" TargetControlID="txtFromDate" PopupButtonID="txtFromDate" />
                                    </td>
                                    <td width="105" class="txtSMBold">
                                        To Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                                        <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd/MM/yyyy"
                                            runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td width="105" class="txtSMBold">
                                                                         Collection Ref. No:</td>
                                    <td><asp:TextBox ID="txtCollectionRefNo" CssClass="inputSM" runat="server"></asp:TextBox>
                                       </td>
                                    <td width="105" class="txtSMBold">
                                     
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                           </ContentTemplate> </asp:UpdatePanel> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="GVCollectionList" runat="server" DataKeyNames="Collection_ID"
                                                    EmptyDataText="No Collections to Display" EmptyDataRowStyle-Font-Bold="true"
                                                    AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
                                                    CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="Collection_ID">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnCollection_ID" runat="server" Value='<%# Bind("Collection_ID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ref. No." SortExpression="Collection_Ref_No">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnCollection_Ref_No" runat="server" Text='<%# Bind("Collection_Ref_No") %>'
                                                                    OnClick="btnDetail_Click" Font-Bold="true" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Collected_On" HeaderText="Collected On" SortExpression="Collected_On"
                                                            NullDisplayText="N/A" DataFormatString="{0:dd/MM/yyyy}">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Collected_By"
                                                            HeaderText="Collected By" SortExpression="Collected_By">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Collection_Type" ItemStyle-HorizontalAlign="center" HeaderText="Payment Type">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Customer_Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px"
                                                            HeaderText="Customer Name" SortExpression="Customer_Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Customer_No" ItemStyle-HorizontalAlign="Left" HeaderText="Customer No"
                                                            SortExpression="Customer_No">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# GetPrice(Eval("Amount"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Cheque_No" ItemStyle-HorizontalAlign="center" HeaderText="Cheque No">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Cheque_Date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Cheque Dt."
                                                            SortExpression="Cheque_Date">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Bank_Name"
                                                            HeaderText="Bank Name" SortExpression="Bank_Name">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Bank_Branch"
                                                            HeaderText="Bank Branch" SortExpression="Bank_Branch">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Emp_Code"
                                                            HeaderText="Emp Code">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                     <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                                <asp:HiddenField ID="hdnCollectionID" runat="server" Value="" />
                                                <asp:HiddenField ID="hfCurrency" runat="server" Value="AED" />
                                                <asp:HiddenField ID="hfDC" runat="server" Value="2" />
                                            </td>
                                        </tr>
                                        <tr><td style="padding-left:5px">
                                        <table runat="server"  id="tblSummary" visible="false" >
                                        <tr><td class="tdstyle">Total Cash : </td><td class="txtSMBold" align="right" ><asp:Label CssClass="inputSM" ID="lblTotCash" runat="server"></asp:Label> </td></tr>
                                        <tr><td class="tdstyle">Total PDC : </td><td class="txtSMBold" align="right"><asp:Label CssClass="inputSM" ID="lblTotPDC" runat="server"></asp:Label> </td></tr>
                                         <tr><td class="tdstyle">Total CDC : </td><td class="txtSMBold" align="right"><asp:Label CssClass="inputSM" ID="lblTotCDC" runat="server"></asp:Label> </td></tr>
                                          <tr><td class="tdstyle">Total : </td><td class="txtSMBold" align="right"><asp:Label CssClass="inputSM" ID="lblTot" runat="server"></asp:Label> </td></tr>
                                        </table>
                                        
                                        </td></tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPECollection"
                                                    runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                                    CancelControlID="btnCancel">
                                                </ajaxToolkit:ModalPopupExtender>
                                                <asp:Panel ID="DetailPnl" runat="server" Style="display: none" Width="380" CssClass="modalPopup">
                                                    <asp:Panel ID="DragPnl" runat="server" Width="375" Style="cursor: move; background-color: #3399ff;
                                                        text-align: center; border: solid 1px #3399ff; color: White; padding: 3px">
                                                        Collection Details</asp:Panel>
                                                    <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                                    <table>
                                                      
                                                        <tr>
                                                            <td>
                                                                <asp:GridView Width="100%" ID="GVCollDtlList" runat="server" EmptyDataText="No details to Display"
                                                                    EmptyDataRowStyle-Font-Bold="true" CssClass="txtSM" AutoGenerateColumns="False"
                                                                    AllowPaging="True" AllowSorting="true" PageSize="25" CellPadding="6">
                                                                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="tdstyle"
                                                                        Height="12px" Wrap="True" />
                                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                                    <Columns>
                                                                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Collection_Line_Ref"
                                                                            SortExpression="Collection_Line_Ref" HeaderText="Collection Line Ref" />
                                                                        <asp:BoundField DataField="Invoice_No" HeaderText="Invoice No" SortExpression="Invoice_No"
                                                                            NullDisplayText="N/A">
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                      <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# GetPrice(Eval("Amount"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="ERP_Status"
                                                                            HeaderText="ERP Status" SortExpression="ERP_Status" />
                                                                    </Columns>
                                                                    <PagerStyle CssClass="pagernumberlink" />
                                                                    <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                                                        CssClass="headerstyle" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                          <tr>
                                                            <td align="center">
                                                                <br />
                                                                <br />
                                                                <asp:Button ID="btnCancel" CssClass="btnInput" runat="server" CausesValidation="false"
                                                                    Text="Cancel" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
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
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
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
                  <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
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
