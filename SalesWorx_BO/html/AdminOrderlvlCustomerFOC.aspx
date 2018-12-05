<%@ Page Title="Customer FOC Discount Rules" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminOrderlvlCustomerFOC.aspx.vb" Inherits="SalesWorx_BO.AdminOrderlvlCustomerFOC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script type="text/javascript">

        function alertCallBackFn(arg) {

        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
        postBackElement.disabled = true;
    }

    function EndRequest(sender, args) {
        $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
        postBackElement.disabled = false;
    }
    function DisableValidation() {
        //            Page_ValidationActive = false;
        //            return true;

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
    function IntegerOnly(e) {

        var keycode;

        if (window.event) {
            keycode = window.event.keyCode;
        } else if (e) {
            keycode = e.which;
        }

        if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 0)
            return true;

        return false;
    }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Customer FOC Discount Rules</h4>

    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server" UpdateMode="conditional">
        <contenttemplate>
                               
                                   <div class="row">
                                            
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Organization</label>
                                            
                                                <telerik:RadComboBox  Skin="Simple"  ID="ddl_org" runat="server" width="100%" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true">
                                                <Items></Items>
                                                </telerik:RadComboBox>
                             
                                            </div>
                                          </div>
                                       </div>
                                
                    
                     <table  width="auto" border="0" cellspacing="6" cellpadding="6" id="tableformNrml">
                                <tr>
                                <td colspan="2">
                                 <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0"
            >
            <Tabs>
                <telerik:RadTab runat="server" Text="Customer Discount" PageViewID="PageView2" Selected="true" SelectedIndex="0" >
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="FOC Items" PageViewID="PageView1" >
                </telerik:RadTab>
               
            </Tabs>
        </telerik:RadTabStrip>
           <telerik:RadMultiPage ID="RadMultiPage1" runat="server"    Width="100%" SelectedIndex="0" BorderStyle="solid" BorderColor="Gray" BorderWidth="1px">
            <telerik:RadPageView ID="PageView2" runat="server" Width="100%" >
            <table  border="0" cellspacing="0" cellpadding="0" Width="100%">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode ="Conditional" >
                            <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                             <asp:PostBackTrigger  ControlID="lbLog"   />
                              <asp:PostBackTrigger  ControlID="lbLogFOC"   />
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                            </Triggers>
                                <ContentTemplate>
                                
                                        <table border="0" width ="100%" cellspacing="6" cellpadding="6">
                                         <tr>
                                <td class="txtSMBold"  >
                                  
                                  
                                  </td>
                                    <td class="txtSMBold" >
                                       </td>
                            
                               <td colspan ="4" align="right">
                                 <asp:Button ID="btnExport" 
                                                                                                              runat="server" CssClass="btn btn-warning" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-warning" Text="Import" TabIndex ="12" />
                                                   
                               
                               </td>
                                </tr>   
                                            <tr>
                                                <td colspan="6"> &nbsp;</td>
                                            </tr>
                            
                                            <tr>
                                                <td   class="txtSMBold" >
                                                   Customer:
                                                </td>
                                                <td colspan="3" >
                                             
    <telerik:RadComboBox ID="ddlCustomer" Filter="Contains"  EmptyMessage ="Please enter Customer code/Name"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server"
                                                    Height="200px" Width="300px" Skin="Simple">
                                                </telerik:RadComboBox>
                                                <asp:Label runat ="server" ID="lblLineID" Visible ="false" ></asp:Label>
                                                </td>
                                                    <td   class="txtSMBold" >
                                                    Discount Mode :
                                                </td>
                                                <td  >
                                                  <asp:DropDownList CssClass ="inputSM" ID="ddlType"  runat="server" TabIndex ="3" >
                                               <%--    <asp:ListItem Value ="V">VALUE</asp:ListItem>
                                                  <asp:ListItem Value ="P">PERCENTAGE</asp:ListItem>--%>
                                                   
                                                                                                         </asp:DropDownList>
                                                </td>
                                             
                                            </tr>
                                                       <tr>
                 <td  class="txtSMBold">
                  Minimum Order Value :</td>
                 <td  >
                 <asp:TextBox ID="txtvalue" runat="server"  TabIndex ="5" 
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTEOrdQty" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtvalue">
                                                        </ajaxToolkit:FilteredTextBoxExtender></td>
                        
                        <td class="txtSMBold"   >
                                                    Discount :
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtDisValue" runat="server" TabIndex ="7"
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                            FilterType="Numbers,Custom"  ValidChars ="." TargetControlID="txtDisValue">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
               
                         <td class="txtSMBold"   >
                                                    Transaction Type :
                                                </td>
                                                <td >
                                                       <asp:DropDownList CssClass ="inputSM" ID="ddl_transactiontype"  runat="server" TabIndex ="3" >
                                                <asp:ListItem Value ="0" Selected="True" >All</asp:ListItem>
                                                  <asp:ListItem Value ="CASH">CASH</asp:ListItem>
                                                 <asp:ListItem Value ="CREDIT">CREDIT</asp:ListItem>
                                                   
                                                     </asp:DropDownList>
                                                </td>
                        </tr>
                                           
                                                  
                                                <tr>
                                               <td></td>
                                                 <td colspan="4"  > 
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server" CssClass="btn btn-success" Text="Add" TabIndex ="8" />
                                                        <asp:Button ID="btnClear" runat="server"  CssClass="btn btn-default" Text="Reset" TabIndex ="9" />
                                                   
                                                
                                                
                                                    </td>
                                                   
                                                </tr>
                                             
                                        
                                          
                                    
                                        </table>
                                    
                    
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                </table>
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                         <ContentTemplate>
                        
                                   
                                        <table border="0"  cellspacing="0" cellpadding="0" width="100%">
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    
                                                    <asp:GridView Width="100%" ID="dgvItems" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                        AllowPaging="true" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" SortExpression ="Customer_no" DataField="Customer_no" HeaderText="Customer No"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                      
                                                           
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" SortExpression ="Customer_Name" DataField="Customer_Name" HeaderText="Customer Name"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                      
                                                            
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="minorder"
                                                                HeaderText="Minimum Order Value" SortExpression ="minorder"  DataFormatString="{0:F0}">
                                                                   <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Attrib_Value"
                                                                HeaderText="Discount"   SortExpression ="Attrib_Value" DataFormatString="{0:F2}">
                                                                 <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="DiscType"
                                                                HeaderText="Discount Mode" SortExpression ="DiscType">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="TransType"
                                                                HeaderText="Transaction Type" SortExpression ="TransType">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Edit" ID="btnEdit"   runat="server" CommandName="EditRecord"
                                                                        CausesValidation="false"   ImageUrl="~/images/edit-13.png"   />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                  <asp:Label ID="lblCustomer_ID" Visible ="false"  runat="server" Text='<%# Bind("Customer_ID") %>'></asp:Label>
                                                                  <asp:Label ID="lblSite_Use_ID" Visible ="false"  runat="server" Text='<%# Bind("Site_Use_ID") %>'></asp:Label>
                                                                    <asp:Label ID="lbl_Custom_Attribute_3" Visible ="false"  runat="server" Text='<%# Bind("Transaction_type")%>'></asp:Label>
                                                                    <asp:ImageButton ToolTip="Delete" ID="btnCan" runat="server" CommandName="DeleteRecord"
                                                                        CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected item?');" />
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
                         </ContentTemplate>
                         </asp:UpdatePanel>   
                             
            </telerik:RadPageView>
                                  <telerik:RadPageView ID="PageView1" runat="server" Width="100%">
                                  
                                 <table  border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode ="Conditional" >
                            <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExportFOC"   />
                          <asp:PostBackTrigger  ControlID="btnImportWindowFOC"  />
                        
                            </Triggers>
                                <ContentTemplate>
                                
                                        <table border="0" width ="100%" cellspacing="5" cellpadding="5"  Width="90%">
                                         <tr>
                                <td class="txtSMBold"  >
                                  
                                  
                                  </td>
                                    <td class="txtSMBold" >
                                       </td>
                            
                               <td align="right">
                                 <asp:Button ID="btnExportFOC" 
                                                                                                              runat="server"  CssClass="btn btn-warning" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindowFOC" runat="server" CssClass="btn btn-warning" Text="Import" TabIndex ="12" />
                                                   
                               
                               </td>
                                </tr>   
                            
                                            <tr>
                                                <td   class="txtSMBold" >
                                                   Items:
                                                </td>
                                                <td  >
                                             
    <telerik:RadComboBox ID="ddlItems" Filter="Contains"  EmptyMessage ="Please enter Item code/Name"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server"
                                                    Height="200px" Width="300px" Skin="Simple">
                                                </telerik:RadComboBox>
                                                
                                                </td>
                                            
                                            </tr>
                                                   <tr>
                                                   <td colspan="2">
                                                       <asp:Label ID="lbl_FOCItemMsg" runat="server" Text=""></asp:Label> </td>
                                                   </tr>
                                                <tr>
                                               <td></td>
                                                 <td >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                              <asp:Button ID="btnAddFOCItems" 
                                                                                                              runat="server"  CssClass="btn btn-success" Text="Add" TabIndex ="8" />
                                                        <asp:Button ID="btnFOCClear" runat="server" CssClass="btn btn-default" Text="Reset" TabIndex ="9" />
                                                   
                                                
                                                
                                                    </td>
                                                   
                                                </tr>
                                             
                                        
                                          
                                    
                                        </table>
                                    
                    
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                </table>
                         <asp:UpdatePanel ID="UpdatePanel7" runat="server" >
                         <ContentTemplate>
                        
                                   
                                        <table border="0"  cellspacing="0" cellpadding="0" width>
                                            <tr>
                                                <td>
                                                    
                                                    <asp:GridView Width="100%" ID="dgvItemsFOC" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                        AllowPaging="true" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" SortExpression ="Description" DataField="Description" HeaderText="Item Description"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                                                                                
                                                        
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                  <asp:Label ID="lblItem_ID" Visible ="false"  runat="server" Text='<%# Bind("ItemID") %>'></asp:Label>
                                                                    <asp:ImageButton ToolTip="Delete" ID="btnDelFOC" runat="server" CommandName="DeleteRecord"
                                                                        CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected item?');" />
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
                         </ContentTemplate>
                         </asp:UpdatePanel> 
                                  </telerik:RadPageView>
                            
                            </telerik:RadMultiPage>
                                </td>
                                </tr>
                                </table>
                               
     
         
         <%--  <div>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm"  runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    <table id="tableinPopupErr" cellpadding="10" style="background-color: White;"  width="100%">
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
                                                             
                                           </div>--%>
             
           
               
              
                                        
                                         
                                                   <div>
                                                <asp:Button ID="BtnImportHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPEImport" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="BtnImportHidden" PopupControlID="Pnl_import" CancelControlID="btnCancelImport"
                                                    Drag="true" />
                                                <asp:Panel ID="Pnl_import" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                      text-align: center; border: solid 1px #3399ff;  
                                                    padding: 3px; display: none;">
                                                    
                                                   <asp:Panel ID="Dragpnl2" runat="server" Style="cursor: move;font-family:Verdana,Tahoma; font-weight:bold; font-size:13px;
                          background-color:#3399ff;
                        text-align: center; border: solid 1px  #337AB7  ; color: White; padding: 3px"  >
                        Import Order level Customer Discount</asp:Panel>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table   border="0" cellspacing="2" cellpadding="2"  width="100%">
                  
                  <tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label6" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Blue"  Text =""></asp:Label>
                  
                  </td>
                  </tr>      
                       
		 <tr>
    <td class ="txtSMBold">Select a File :</td>
    <td> <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate><asp:FileUpload ID="ExcelFileUpload" runat="server" /></ContentTemplate>
                                                    </asp:UpdatePanel>    
          
         </td>
       
          </tr>
          <tr><td colspan ="2"><br /></td></tr>
          <tr>
          <td>
           
             
          </td>
          <td>
           <asp:Button ID="btnImport" runat="server" Text="Import"  CssClass="btn btn-warning" /> <asp:Button ID="btnCancelImport" CssClass="btn btn-success" TabIndex="5" runat="server"
                                    CausesValidation="false" Text="Close" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                 CssClass="btnInputBlue" />
           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
            <span style ="text-decoration: underline !important;Color:#337AB7;"> <asp:LinkButton ID="lbLog" Font-Bold ="true" Font-Size ="13px"   ForeColor  ="#337AB7"
              ToolTip ="Click here to download the uploaded log" runat ="server"
               Text ="View Log" Visible="false" ></asp:LinkButton></span>
           </td>
          </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr><td colspan ="2">
                        <asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" > </asp:Label></td></tr>
                        <tr>
                        <td colspan="2">
                       
                         <asp:GridView Width="100%" ID="dgvErros" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" Visible="false" 
                                                        AllowPaging="true" AllowSorting="false"  PageSize="15" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                       
                                                          
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="LogInfo"
                                                                HeaderText="Log Info">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                             
                                                        
                                                          
                                                        </Columns>
                                                        <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                        
                         
                        </td>
                        </tr>
                    </table>
                                                </asp:Panel>
                                             </div>
                                             
                                             
                                             <div>
                                                <asp:Button ID="BtnFOC" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPFOC" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="BtnFOC" PopupControlID="Pnl_importFOC" CancelControlID="btnCancelImport"
                                                    Drag="true" />
                                                <asp:Panel ID="Pnl_importFOC" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                     text-align: center; border: solid 1px #3399ff;  
                                                    padding: 3px; display: none;background-color:whitesmoke">
                                                    
                                                   <asp:Panel ID="Panel2" runat="server" Style="cursor: move;font-family:Verdana,Tahoma; font-weight:bold; font-size:13px;
                          background-color:  #337AB7  ;
                        text-align: center; border: solid 1px  #337AB7  ; color: White; padding: 3px"  >
                        Import FOC items</asp:Panel>
                    <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                    <table   border="0" cellspacing="2" cellpadding="2" width="100%" >
                  
                  <tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label2" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Blue"  Text =""></asp:Label>
                  
                  </td>
                  </tr>      
                       
		 <tr>
    <td class ="txtSMBold">Select a File :</td>
    <td> <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate><asp:FileUpload ID="FileUploadFOC" runat="server" /></ContentTemplate>
                                                    </asp:UpdatePanel>    
          
         </td>
       
          </tr>
          <tr><td colspan ="2"><br /></td></tr>
          <tr>
          <td>
           
             
          </td>
          <td>
           <asp:Button ID="btnImportFOC" runat="server" Text="Import" CssClass="btn btn-warning"  /> <asp:Button ID="Button3" CssClass="btn btn-success" TabIndex="5" runat="server"
                                    CausesValidation="false" Text="Close" />
                  <asp:Button ID="Button4" style="display:none"  runat="server" Text="Reimport" />
           <asp:Button ID="Button5" runat="server" Text="Reimport"  Visible ="false" 
                 CssClass="btnInputBlue" />
           <asp:Button ID="Button6" style="display:none"  runat="server" Text="Reimport" />
            <span style ="text-decoration: underline !important;Color:#337AB7;"> <asp:LinkButton ID="lbLogFOC" Font-Bold ="true" Font-Size ="13px"   ForeColor  ="#337AB7"
              ToolTip ="Click here to download the uploaded log" runat ="server"
               Text ="View Log" Visible="false"></asp:LinkButton></span>
           </td>
          </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr><td colspan ="2">
                        <asp:Label runat ="server" ID="lbl_msgFoc" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" > </asp:Label></td></tr>
                        <tr>
                        <td colspan="2">
                        
                          <asp:GridView Width="100%" ID="Gv_errorFOC"   runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true" Font-Size ="12px" CssClass="txtSM" AutoGenerateColumns="False" 
                                                        AllowPaging="true" AllowSorting="false" PageSize="15" CellPadding="6" Visible="false" >
                                                     <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="tdstyle"
                                                        Height="12px" Wrap="True" />
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <%-- <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColNo"
                                                                HeaderText="Col No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColName"
                                                                HeaderText="Colume Name">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>--%>
                                                          
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="LogInfo"
                                                                HeaderText="Log Info">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                        
                                                        
                                                          
                                                        </Columns>
                                                         <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                                                   
                        </td>
                        </tr>
                    </table>
                                                </asp:Panel>
                                             </div>
                                </contenttemplate>
        <triggers>
           
            <asp:PostBackTrigger ControlID="btnImportFOC" /> 
            <asp:PostBackTrigger ControlID="btnImport" /> 
	
        </triggers>
    </asp:UpdatePanel>





    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl1"
        runat="server">
        <progresstemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../images/Progress.gif" alt="Processing..." style="padding-left: 400px" />
                <span style="font-size: 12px; color: #666;">Processing... </span>
            </asp:Panel>
        </progresstemplate>
    </asp:UpdateProgress>


</asp:Content>
