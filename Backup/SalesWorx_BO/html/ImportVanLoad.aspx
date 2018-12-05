<%@ Page Title="Import Van Load" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="ImportVanLoad.aspx.vb" Inherits="SalesWorx_BO.ImportVanLoad" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

.rcbSlide
{
	z-index: 100002 !important;
}
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();

            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;
                var myRegExp1 = /btnSaveAcc/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
                if (AddString != -1 || EditString != -1) {
                    $get('<%= Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'block';
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
                }
                postBackElement.disabled = true;
            }
        }


        function EndRequest(sender, args) {
            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;
                var myRegExp1 = /btnSaveAcc/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);

                if (AddString != -1 || EditString != -1) {
                    $get('<%=Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'none';
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
            }
        }

       

    </script>

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
        function TestCheckBox() {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                return confirm('Would you like to confirm the selected Stock Requisitions?');
                return true;
            }
            alert('Select at least one Stock Requisition!');
            return false;

        }

        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = cbSelectAll.checked;
            }

        }


        //     function Validate() {
        //         Page_ClientValidate();
        //         if (!Page_IsValid) {
        //             $find('<%=MpInfoError.ClientID%>').show();
        //             var Info = document.getElementById('<%=lblinfo.ClientID%>');
        //             Info.innerHTML = "Validation";
        //             document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
        //             return false;
        //         }
        //     }

        //     function DisableValidation() {
        //         Page_ValidationActive = false;
        //         return true;

        //     }
    </script>

   
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Import Van Load</span></div>
                    
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                           
             
    
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                           
                                            <td  class="txtSMBold">
                                                &nbsp;Organization: &nbsp;</td>
                                            <td class="txtSMBold" width="135">
                                                <asp:DropDownList ID="ddl_org" runat="server" width="250" AutoPostBack="true">
                                                
                                                </asp:DropDownList> 
                                            </td><td class="txtSMBold" width="75">
                                                &nbsp;Van:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlVan" runat="server" width="250">
                                                    
                                                    </asp:DropDownList>
                                                </td>
                                                <td><asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    TabIndex="4" Text="Search" />
                                                     
                                                     <asp:Button ID="BTn_Import" runat="server" CausesValidation="false" CssClass="btnInputGreen"
                                                    OnClick="btnImport_Click" TabIndex="1" Text="Import" />
                                                    
                                                     <asp:Button ID="BTn_Export" runat="server" CausesValidation="false" CssClass="btnInputBlue"
                                                     TabIndex="1" Text="Export" />
                                                    </td>
                                                </table>
                                </ContentTemplate>
                                <Triggers>
                                
                                 <asp:PostBackTrigger ControlID="BTn_Export" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0" Width="90%"  style="padding:10px">
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="GvStockRequ" runat="server" EmptyDataText="No stock requisition to be confirmed."
                                                    EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Confirm" ID="btnConfirmAll" runat="server"
                                                                    OnClick="btnConfirmAll_Click" CausesValidation="false" ImageUrl="~/images/tick.jpg"
                                                                    OnClientClick="return TestCheckBox()" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HRowID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"StockRequisition_ID") %>'/>
                                                                                                                             
                                                                <asp:CheckBox ID="chkDelete" runat="server" Visible='<%# DataBinder.Eval(Container.DataItem,"ShowConfirm") %>' />
                                                                <asp:ImageButton ToolTip="Confirm" ID="btnConfirm" Visible='<%# DataBinder.Eval(Container.DataItem,"ShowConfirm") %>'
                                                                  OnClick="btnConform_Click"   runat="server" CausesValidation="false" ImageUrl="~/images/tick.jpg"
                                                                    OnClientClick="javascript:return confirm('Would you like to confirm this stock requisition?');" />
                                                               
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Request_Date" HeaderText="Date" SortExpression="Request_Date">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SalesRep_name" HeaderText="Sales Man" SortExpression="SalesRep_name">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                          <asp:TemplateField HeaderText="View">
                                                         <ItemTemplate>
                                                          <asp:LinkButton ToolTip="View" ID="btnView" 
                                                                    OnClick="btnView_Click" runat="server" CausesValidation="false"  Text="View"
                                                                     />
                                                        
                                                         </ItemTemplate>
                                                         
                                                         </asp:TemplateField> 
                                                          <asp:TemplateField HeaderText="Download">
                                                         <ItemTemplate>
                                                          <asp:LinkButton ToolTip="Download" ID="btnDownloadSt" 
                                                                    OnClick="btnDownloadSt_Click" runat="server" CausesValidation="false"  Text="Download"
                                                                     />
                                                        
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
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEDivConfig"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Width="600" CssClass="modalPopup" Style="display: none" >
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                        <asp:Panel ID="DragPnl" runat="server" Width="591px" Font-Size ="13px" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px">
                                            Import Load Van</asp:Panel><asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                       
                                        
                                        <table width="100%" cellpadding ="6" cellspacing ="6" border="1">
                                        <tr>
                                        <td colspan="2"> <asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Maroon"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td  class="txtSMBold">
                                                Organization:</td>
                                            <td class="txtSMBold">
                                                <asp:DropDownList ID="ddl_Organization" runat="server" width="250" >
                                                
                                                </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    File:
                                                </td>
                                                <td>
                                                <asp:UpdatePanel ID="UpdatePanelF" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                    <asp:FileUpload ID="file_import" runat="server"  /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="file_import" Display="Dynamic"  ErrorMessage="*Please upload only .xls and .xlsx Files" ValidationExpression="^(.)+(.xls|.XLS|.xlsx|.XLSX)$"></asp:RegularExpressionValidator>
                            
                                                    <asp:Button ID="btnSave" CssClass="btnInputGreen" TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Import" />
                                                    
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel" />
                                                   </ContentTemplate>
                                                    </asp:UpdatePanel>    
                                                </td>
                                            </tr>     
                                            
                                             <tr>
                                             <td colspan="2">
                                               <asp:LinkButton id="BtnDownLoad" runat="server" Text="Download" Visible="false" ></asp:LinkButton>
                                               <asp:GridView Width="100%" ID="dgvErros" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true" Font-Size ="12px" CssClass="txtSM" AutoGenerateColumns="False" 
                                                        AllowPaging="true" AllowSorting="false" PageSize="15" CellPadding="6" >
                                                     <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="tdstyle"
                                                        Height="12px" Wrap="True" />
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="RowNo">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Van_location"
                                                                HeaderText="Van">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                    
                                                             <asp:BoundField  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="item_Code"
                                                                HeaderText="Item_Code">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                
                                                            </asp:BoundField>
                                                         
                                                            <asp:BoundField  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Load_Quantity"
                                                                HeaderText="Qty">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                
                                                            </asp:BoundField>    
                                                        
                                                         
                                                             <asp:BoundField  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Reason"
                                                                HeaderText="Error Text">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                
                                                            </asp:BoundField>    
                                                        
                                                        </Columns>
                                                          <PagerStyle CssClass="pagernumberlink" />
                                                    <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                                        CssClass="headerstyle" />
                                                    </asp:GridView>
                                             </td>
                                             </tr>                      
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSave" />
                                            
                                            </Triggers>
                                                    </asp:UpdatePanel> 
                                    </asp:Panel>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" cellpadding="10" style="background-color: White;width:100%">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
                                                            <td align="center" style="text-align: center;">
                                                                <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
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
                                                <asp:Button ID="BtnStockDetails" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPStockDetails" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="BtnStockDetails" PopupControlID="PnlStockDetails" CancelControlID="Btn_Cancelconfirm"
                                                    Drag="true" />
                                                <asp:Panel ID="PnlStockDetails" Width="600" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table  width="600" cellpadding="10" style="background-color: White;">
                                                     <tr>
                                                          <td>
                                                          <table>
                                                          <tr>
                                                          <td class="txtSMBold">FSR:</td><td class="txtSMBold"><asp:Label ID="lbl_Selvan" runat="server"></asp:Label></td>
                                                          </tr>
                                                          </table>
                                                          
                                                          </td>
                                                      <tr>
                                                          <td>
                                                              <asp:GridView ID="GVItems" runat="server" AllowPaging="true" 
                                                                  AutoGenerateColumns="False" CellPadding="6" CssClass="txtSM" 
                                                                  EmptyDataRowStyle-Font-Bold="true" EmptyDataText="No items to display" 
                                                                  Font-Size="12px" PageSize="10" Width="100%">
                                                                  <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                                                                      CssClass="tdstyle" Height="12px" Wrap="True" />
                                                                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                                  <EmptyDataRowStyle Font-Bold="True" />
                                                                  <Columns>
                                                                      <asp:BoundField DataField="Van" HeaderStyle-HorizontalAlign="Left" 
                                                                          HeaderStyle-Wrap="false" HeaderText="Van Org">
                                                                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                      </asp:BoundField>
                                                                      <asp:BoundField DataField="Item" HeaderStyle-HorizontalAlign="Left" 
                                                                          HeaderStyle-Wrap="false" HeaderText="Item">
                                                                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                      </asp:BoundField>
                                                                      <asp:BoundField DataField="UOM" HeaderStyle-HorizontalAlign="Left" 
                                                                          HeaderStyle-Wrap="false" HeaderText="UOM">
                                                                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                      </asp:BoundField>
                                                                      <asp:BoundField DataFormatString="{0:#,###.00}" DataField="Qty" HeaderStyle-HorizontalAlign="Left"  
                                                                          HeaderStyle-Wrap="false" HeaderText="Qty" >
                                                                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                      </asp:BoundField>
                                                                  </Columns>
                                                                  <PagerStyle CssClass="pagernumberlink" />
                                                                  <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                                                                      BorderWidth="1px" CssClass="headerstyle" />
                                                              </asp:GridView>
                                                              <asp:HiddenField ID="RowID" runat="server" />
                                                          </td>
                                                        <tr align="center">
                                                            
                                                            <td align="center" style="text-align: center;">
                                                                <asp:Button ID="btn_Confirm" runat="server" Text="Confirm" CssClass="btnInputBlue"  Visible="false"/>
                                                                 <asp:Button ID="Btn_Cancelconfirm" runat="server" Text="Cancel" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                     <asp:PostBackTrigger ControlID="BtnDownLoad" />
     
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                
                 </ContentTemplate>
                 </asp:UpdatePanel>
                
                
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
