<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="ReconcileReturns.aspx.vb" Inherits="SalesWorx_BO.ReconcileReturns" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function RowSelectedReturn(sender, eventArgs) {
            
            var grid = sender;
            var MasterTable = grid.get_masterTableView(); var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
            var cell = MasterTable.getCellByColumnUniqueName(row, "Pending_Amount");
            findSettlementAmount(cell.innerHTML)
        }

        function RowSelectedOrders(sender, eventArgs) {
            var grid = sender;
            var MasterTable = grid.get_masterTableView(); var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
            var cell = MasterTable.getCellByColumnUniqueName(row, "Pending_Amount");
            findSettlementAmount(cell.innerHTML)
        }
        function findSettlementAmount(v) {
            var oldval = $("#ctl00_ContentPlaceHolder1_txtAmt").val()
            if(oldval=="")
                $("#ctl00_ContentPlaceHolder1_txtAmt").val(parseFloat(v))
            else
            $("#ctl00_ContentPlaceHolder1_txtAmt").val(Math.min(parseFloat(oldval), parseFloat(v)))
    }
    function NumericOnly(e) {

        var keycode;

        if (window.event) {
            keycode = window.event.keyCode;
        } else if (e) {
            keycode = e.which;
        }
        if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0)
            return true;

        return false;
    }
    
    </script>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Reconciliation of Returns</span></div>
               
                <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server"  UpdateMode="conditional">
                    <ContentTemplate>
                        
                        <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%" >
                                    <table width="100%" cellpadding="4" cellspacing="1" >
                                        <tr>
                                            <td>
                                                <table border="0" cellspacing="2" cellpadding="2"  width="100%">
                                                    <tr>
                                                        <td class="txtSMBold" width="10%">Organization :</td>
                                                        
                                                        <td width="25%">
                                                           <asp:DropDownList CssClass="inputSM" ID="ddlOrganization"  Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"></asp:DropDownList></td>
                                                        <td width="10%">
                                                            &nbsp;</td> 
                                                            <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="txtSMBold">Customer :</td>
                                                        
                                                        <td>
                                                           <asp:DropDownList CssClass="inputSM" ID="ddl_customer"  Width ="200px"
                    runat="server" AutoPostBack="true" ></asp:DropDownList></td>
                                                        <td>
                                                            &nbsp;</td>
                                                           <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="txtSMBold">From Date:</td>
                                                        <td>
                                                            <asp:TextBox  ID="txtfromDate"  CssClass="inputSM" runat="server"></asp:TextBox> 
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtfromDate" PopupButtonID="txtfromDate"  />  </td>
                                                        <td class="txtSMBold">To Date:</td>
                                                        <td>
                                                            <asp:TextBox  ID="txtToDate"   CssClass="inputSM" runat="server"></asp:TextBox> 
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender2" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate"  />  
                                                            <asp:Button ID="Btn_Search" runat="server" CssClass="btnInputBlue" Text="Search" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                         <asp:Panel ID="Pnl_details" runat="server" Visible="false">
                                        <tr>
                                        <td>
                                                <table border="0" cellspacing="2" cellpadding="2"  width="100%">
                                                    <tr>
                                            <td valign="top" width="50%">
                                            <asp:Label ID="Ret" Text="List of Returns" runat="server"  Font-Size="Medium" ForeColor="Brown"></asp:Label>
                                            <br />
                                                <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" BorderStyle="Groove"
                                        BorderWidth="1px" Width="514px">
                                        <telerik:RadGrid ID="Grd_Returns" runat="server" Skin="Vista" AllowRowSelect="true" allowmultirowselection="false" 
                                            AutoGenerateColumns="False"  AllowPaging="true" AllowFilteringByColumn="True" PageSize="10" 
                                            DataKeyNames="Orig_Sys_Document_Ref"  ClientDataKeyNames="Orig_Sys_Document_Ref">
                                            <GroupingSettings CaseSensitive="false" />
                                            <mastertableview width="100%" summary="RadGridtable" EditMode="InPlace" AllowFilteringByColumn="true" 
                                            DataKeyNames="Orig_Sys_Document_Ref"  ClientDataKeyNames="Orig_Sys_Document_Ref">
                                            <NoRecordsTemplate><div>There are no returns to display</div></NoRecordsTemplate>
                                            <Columns>
                                            <telerik:GridBoundColumn  UniqueName="Orig_Sys_Document_Ref" DataField="Orig_Sys_Document_Ref" HeaderText ="RefNo." 
                                             AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true" ><HeaderStyle Width="150px" />
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn UniqueName="Invoice_Date"  DataField="Invoice_Date" HeaderText ="Date" DataFormatString="{0:dd-MMM-yyyy}"
                                              AllowFiltering="false"  ShowFilterIcon="false" ><HeaderStyle Width="200px" />
                                              </telerik:GridBoundColumn><telerik:GridBoundColumn UniqueName="Pending_Amount"  DataFormatString="{0:###0.00}"
                                              DataField="Pending_Amount" HeaderText ="Amount"  AllowFiltering="false" ShowFilterIcon="false" >
                                              <HeaderStyle Width="150px" /></telerik:GridBoundColumn></Columns></mastertableview><ClientSettings>
                                              <Selecting AllowRowSelect="True"></Selecting>
                                               <ClientEvents OnRowSelected="RowSelectedReturn" />
                                              </ClientSettings>
                                              <pagerstyle mode="NextPrevAndNumeric">
                                              </pagerstyle>
                                              </telerik:RadGrid>
                                    </asp:Panel>
                                            </td>
                                            <td valign="top" width="50%">
                                           <asp:Label ID="Label1" Text="List of Invoices" runat="server" Font-Size="Medium" ForeColor="Brown"></asp:Label>
                                            <br />
                                                <asp:Panel ID="Panel2" runat="server" Height="300px" ScrollBars="Auto" BorderStyle="Groove"
                                        BorderWidth="1px"  Width="514px">
                                        <telerik:RadGrid ID="Grd_Orders" runat="server" Skin="Vista" AllowRowSelect="true" allowmultirowselection="false" 
                                            AutoGenerateColumns="False"  AllowPaging="true" AllowFilteringByColumn="True" PageSize="10"
                                             DataKeyNames="Orig_Sys_Document_Ref"  ClientDataKeyNames="Orig_Sys_Document_Ref">
                                             <GroupingSettings CaseSensitive="false" />
                                             <mastertableview width="100%" summary="RadGrid table" EditMode="InPlace" 
                                             AllowFilteringByColumn="true" DataKeyNames="Orig_Sys_Document_Ref"  ClientDataKeyNames="Orig_Sys_Document_Ref">
                                             <NoRecordsTemplate><div>There are no Invoices to display</div></NoRecordsTemplate>
                                             <Columns>
                                             <telerik:GridBoundColumn  UniqueName="Orig_Sys_Document_Ref" DataField="Orig_Sys_Document_Ref" 
                                             HeaderText ="RefNo."   AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true" >
                                             <HeaderStyle Width="150px" />
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn UniqueName="Invoice_Date"  DataField="Invoice_Date" HeaderText ="Date"  DataFormatString="{0:dd-MMM-yyyy}"
                                             AllowFiltering="false" ShowFilterIcon="false" ><HeaderStyle Width="200px" />
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn UniqueName="Pending_Amount"  DataField="Pending_Amount" HeaderText ="Amount"  DataFormatString="{0:###0.00}"
                                             AllowFiltering="false" ShowFilterIcon="false" >
                                             <HeaderStyle Width="150px" />
                                             </telerik:GridBoundColumn>
                                             </Columns></mastertableview>
                                             <ClientSettings><Selecting AllowRowSelect="True"></Selecting>
                                             <ClientEvents OnRowSelected="RowSelectedOrders" />
                                             </ClientSettings>
                                             <pagerstyle mode="NextPrevAndNumeric"></pagerstyle></telerik:RadGrid>
                                    </asp:Panel></td>
                                        </tr>
                                        <tr>
                                            <td class="txtSM"  style="text-align:right">
                                                
                                                <asp:Label ID="lblMsg1" runat="server" Font-Bold="True" ForeColor="Maroon" 
                                                    Text="Settlement Amount:" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td align="left" class="txtSM">
                                                <asp:TextBox ID="txtAmt" runat="server" CssClass="inputSM" Width="150px" onKeypress='return NumericOnly(event)'></asp:TextBox>
                                                
                                                &nbsp;<asp:Button ID="Btn_Save" runat="server" CssClass="btnInputGreen" 
                                                    Text="Save" />
                                            </td>
                                        </tr>
                                        </table></td>
                                        </tr>
                                        
                                        </asp:Panel>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                            TargetControlID="btnHidden" PopupControlID="pnlmsg" CancelControlID="btnClose"
                            Drag="true" />
                        <asp:Panel ID="pnlmsg" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                            <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                <tr align="center">
                                    <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                        border: solid 1px #3399ff; color: White; padding: 3px">
                                        <asp:Label ID="lblinfo" runat="server" Font-Size ="14px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center">
                                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                       <asp:Label ID="lblMessage" runat="server"  Font-Size ="13px"    ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center;">
                                        <asp:Button ID="btnClose" runat="server" Text="Ok"  CssClass="btnInputBlue"  />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <asp:Button ID="btn_confirm" runat="Server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="MPSettle" runat="server" BackgroundCssClass="modalBackground"
                            TargetControlID="btn_confirm" PopupControlID="pnlConfirm" CancelControlID="btnCloseconfirm"
                            Drag="true" />
                        <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                            <table id="table1" width="400" cellpadding="10" style="background-color: White;">
                                
                                <tr>
                                    <td align="center" style="text-align: center">
                                       <asp:Label ID="Label3" runat="server"  Font-Size ="13px"  Text="Are you sure to settle the amount?" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center;">
                                        <asp:Button ID="btn_Yes" runat="server" Text="Yes"  CssClass="btnInputGreen"  />
                                        <asp:Button ID="btnCloseconfirm" runat="server" Text="No"  CssClass="btnInputGrey"  />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    
     <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl1"
        runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../images/Progress.gif" alt="Processing..." style="padding-left: 400px" />
                <span style="font-size: 12px; color: #666;">Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
</asp:Content>

