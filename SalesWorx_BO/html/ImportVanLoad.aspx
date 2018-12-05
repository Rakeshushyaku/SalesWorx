<%@ Page Title="Import Van Load" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ImportVanLoad.aspx.vb" Inherits="SalesWorx_BO.ImportVanLoad" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">

    <style type="text/css">

.rcbSlide
{
	z-index: 100002 !important;
}
</style>

 

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

       
        function alertCallBackFn(arg) {
             
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


         
    </script>
     </asp:Content>
     <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <h4>Import Van/FSR Load</h4>
                    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                           
             
    
                
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <label>Organization</label>
                                            <div class="form-group">
                                                <telerik:RadComboBox Skin="Simple"  ID="ddl_org" Filter="Contains"   runat="server" width="100%" AutoPostBack="true"></telerik:RadComboBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Van/FSR</label>
                                            <div class="form-group">
                                                <telerik:RadComboBox Skin="Simple" ID="ddlVan"   Filter="Contains" runat="server" width="100%"></telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="hidden-xs"><br /></label>
                                            <div class="form-group">
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass ="btn btn-primary" TabIndex="4" Text="Search" />
                                                <asp:Button ID="BTn_Import" runat="server" CausesValidation="false" CssClass="btn btn-warning" OnClick="btnImport_Click" TabIndex="1" Text="Import" />
                                                <asp:Button ID="BTn_Export" runat="server" CausesValidation="false" CssClass ="btn btn-warning" TabIndex="1" Text="Export" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                
                                 <asp:PostBackTrigger ControlID="BTn_Export" />
                                </Triggers>
                            </asp:UpdatePanel>
                        
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    
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
                                                            <ItemTemplate >
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
                                            
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEDivConfig"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Width="650" CssClass="modalPopup" Style="display: none" >
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                             <div class="panelouterblk">
                                                                                                          <asp:Panel ID="DragPnl" runat="server" CssClass="popupbartitle">
                                            Import Load Van</asp:Panel>
                                                                 <asp:ImageButton ID="btn_close" runat="server"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton>    
                                                                 <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                       
                                           </div> 
                                        <table width="100%" cellpadding ="6" cellspacing ="6" border="0">
                                        <tr>
                                        <td colspan="2"> <asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Maroon"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td  class="txtSMBold" style="vertical-align: top;">
                                                Organization</td>
                                            <td class="txtSMBold">
                                                <div class="form-group">
                                                <telerik:RadComboBox Skin="Simple"  ID="ddl_Organization" Filter="Contains"  runat="server" width="250" >
                                                
                                                </telerik:RadComboBox>
                                                    </div>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold" style="vertical-align: top;">
                                                    File
                                                </td>
                                                <td>
                                                <asp:UpdatePanel ID="UpdatePanelF" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                            <div class="form-group">
                                                    <asp:FileUpload ID="file_import" runat="server"  /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="file_import" Display="Dynamic"  ErrorMessage="*Upload only .xls and .xlsx Files" ValidationExpression="^(.)+(.xls|.XLS|.xlsx|.XLSX)$"></asp:RegularExpressionValidator>
                                                  </div>
                                                            <div class="form-group">
                                                    <asp:Button ID="btnSave" CssClass ="btn btn-primary" TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Import" />
                                                    
                                                    <asp:Button ID="btnCancel"  CssClass ="btn btn-default"  TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel" />
                                                                </div>
                                                   </ContentTemplate>
                                                    </asp:UpdatePanel>    
                                                </td>
                                            </tr>     
                                            
                                             <tr>
                                             <td colspan="2">
                                               <asp:LinkButton id="BtnDownLoad" runat="server" Text="Download" Visible="false" ></asp:LinkButton>
                                               <asp:GridView Width="100%" ID="dgvErros" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true" Font-Size ="12px" CssClass="tablecellalign" AutoGenerateColumns="False" 
                                                        AllowPaging="true" AllowSorting="false" PageSize="10" CellPadding="6" >
                                                     
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
                                                          <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                      <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
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
                                     
                                    
                                                <asp:Button ID="BtnStockDetails" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPStockDetails" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="BtnStockDetails" PopupControlID="PnlStockDetails" CancelControlID="Btn_Cancelconfirm"
                                                    Drag="true" />
                                                <asp:Panel ID="PnlStockDetails" Width="610" runat="server" CssClass="modalPopup" Style="display: none">
                                                     <div class="panelouterblk">
                                           <asp:Panel ID="Panel1" runat="server" CssClass="popupbartitle">
                                            View</asp:Panel>
                                                                 <asp:ImageButton ID="ImageButton1" runat="server"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton>    
           

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
                                                                  AutoGenerateColumns="False" CellPadding="6" CssClass="tablecellalign"
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
                                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                                  <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                                                                      BorderWidth="1px" CssClass="headerstyle" />
                                                              </asp:GridView>
                                                              <asp:HiddenField ID="RowID" runat="server" />
                                                          </td>
                                                        <tr align="center">
                                                            
                                                            <td align="center" style="text-align: center;">
                                                                <asp:Button ID="btn_Confirm" runat="server" Text="Confirm" CssClass="btnInputBlue"  Visible="false"/>
                                                                 <asp:Button ID="Btn_Cancelconfirm" runat="server" Text="Cancel" CssClass ="btn btn-danger" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                     <asp:PostBackTrigger ControlID="BtnDownLoad" />
     
                                </Triggers>
                            </asp:UpdatePanel>
                        
                
                 </ContentTemplate>
                 </asp:UpdatePanel>
                
                
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
            
</asp:Content>
