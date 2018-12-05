<%@ Page Title="Item Level Discount Rule" Language="vb" AutoEventWireup="false" EnableEventValidation="false"    
 MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="DiscountDefinition.aspx.vb"  Inherits="SalesWorx_BO.DiscountDefinition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
     .rcTimePopup
 {
   display:none ! important;
 }
 </style> 

  <script language="javascript" type="text/javascript">

     

    </script>



    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Item Level Discount Rule</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                            <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                            </Triggers>
                                <ContentTemplate>
                                
                                                
                                  
                                
                                
                                
                                
                        
                                   
                                
                                        <table border="0" width ="100%" cellspacing="2" cellpadding="2">
                                         <tr>
                                <td class="txtSMBold"  >
                                    Organization :
                                  
                                  </td>
                                    <td class="txtSMBold" >
                                        <asp:DropDownList ID="ddl_org" runat="server" Width="300px" TabIndex ="1"  CssClass="inputSM"  AutoPostBack="true">
        </asp:DropDownList>
                                  
                                    
                                </td>
                            
                               <td colspan ="2">
                                 <asp:Button ID="btnExport" 
                                                                                                              runat="server" CssClass="btnInputOrange" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btnInputGreen" Text="Import" TabIndex ="12" />
                                                   
                               
                               </td>
                                </tr>   
                            
                                            <tr>
                                                <td   class="txtSMBold" >
                                                   Item Code/Desc. :
                                                </td>
                                                <td  >
                                             
    <telerik:RadComboBox ID="ddlItem" Filter="Contains"  EmptyMessage ="Please enter item code/description"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server"
                                                    Height="200px" Width="300px">
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
                  Valid From :</td>
                 <td  >
                  <telerik:RadDateTimePicker ID="StartTime"  MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"    Width="100px" TabIndex ="4"    runat="server" 
                                    >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                  <asp:RequiredFieldValidator runat="server" Visible ="false" Width ="3px" ID="RequiredFieldValidator1" ControlToValidate="StartTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator></td>
                        
                        <td class="txtSMBold"   >
                                                   Minimum Qty :
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtFromQty" runat="server"  TabIndex ="5" 
                                                            CssClass="inputSM" Width="70px"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTEOrdQty" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtFromQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
               
                        
                        </tr>
                                           
                                            <tr>
                                                <td  class="txtSMBold" >         
                     Valid To :</td><td >
                 <telerik:RadDateTimePicker ID="EndTime" MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"    Width="100px" TabIndex ="6"   runat="server" 
                                    >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                 <asp:RequiredFieldValidator runat="server" Visible ="false" ID="Requiredfieldvalidator2" Width ="3px"  ControlToValidate="EndTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                       
                     
                        </td> 
                                                
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
                                             
                                               
                                                </tr>
                                                
                                       
                                             
                                               
                                   
                                            
                                                   <tr>
                                                    <td colspan="3">
                                                          <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="EndTime"
                        ControlToCompare="StartTime" Operator="GreaterThan"    Type="String"
                        ErrorMessage="To date > From date"> </asp:CompareValidator>
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                               <td></td>
                                                 <td colspan="3"  >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server" CssClass="btnInputBlue" Text="Add" TabIndex ="8" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass="btnInputRed" Text="Reset" TabIndex ="9" />
                                                   
                                                
                                                
                                                    </td>
                                                   
                                                </tr>
                                             
                                        
                                          
                                    
                                        </table>
                                    
                    
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                </table>
                            <asp:UpdatePanel ID="Panel" runat="server" >
                                <ContentTemplate>
                                             <asp:GridView Width="100%" ID="dgvErros" Visible ="false"  runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true" Font-Size ="12px" CssClass="txtSM" AutoGenerateColumns="False" 
                                                        AllowPaging="false" AllowSorting="false" PageSize="25" CellPadding="6" >
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
                                                          <PagerStyle CssClass="pagernumberlink" />
                                                    <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                                        CssClass="headerstyle" />
                                                    </asp:GridView>
                                   
                                        <table border="0"  cellspacing="0" cellpadding="0" width>
                                            <tr>
                                                <td>
                                                    <asp:GridView Width="100%" ID="dgvItems" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" DataKeyNames ="LineId"
                                                        AllowPaging="true" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" SortExpression ="Item_Code" DataField="Item_Code" HeaderText="Item Code"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                      
                                                           
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" SortExpression ="ItemName" DataField="ItemName" HeaderText="Description"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="UOM"
                                                                HeaderText="UOM" SortExpression ="UOM">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                      
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="DisType"
                                                                HeaderText="Discount Mode" SortExpression ="DisType">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="FromQty"
                                                                HeaderText="From Qty" SortExpression ="FromQty"  DataFormatString="{0:F0}">
                                                                   <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="DisValue"
                                                                HeaderText="Value"   SortExpression ="DisValue" DataFormatString="{0:F2}">
                                                                 <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                                                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_From"
                                                                HeaderText="Valid From"  SortExpression ="Valid_From"   DataFormatString="{0:MM-dd-yyyy}" >
                                                               <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_To"
                                                                HeaderText="Valid To" SortExpression ="Valid_To"   DataFormatString="{0:MM-dd-yyyy}" >
                                                               <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
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
                                                                  <asp:Label ID="lblDType" Visible ="false"  runat="server" Text='<%# Bind("Cond_UT") %>'></asp:Label>
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
                                         <asp:Panel ID="PnlGridData" runat="server"  >
                                    </asp:Panel>
                                   
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
                                                       <asp:Label ID="lblinfo" runat="server" Font-Size ="13px" Font-Bold ="true"   ></asp:Label>
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
                                                        <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                         
                     
                                </ContentTemplate>
                            </asp:UpdatePanel>
                              <table>
        <tr>
            <td>
                <asp:Button ID="btnImportHidden" CssClass="btnInput" runat="Server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEImport"
                    runat="server" PopupControlID="DetailPnl" TargetControlID="btnImportHidden" CancelControlID="btnCancelImport">
                </ajaxToolkit:ModalPopupExtender>
               <asp:Panel ID="DetailPnl" runat="server" Style="display: none" Width="450" CssClass="modalPopup2">
                                      <asp:Panel ID="DragPnl" Font-Size ="13px"  runat="server" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px" Width="445">
                                           Import Discount Data</asp:Panel>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table  width ="450px" cellpadding ="2" cellspacing ="2">
<%--<tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label3" CssClass ="txtSM"   ForeColor="#0090d9"   Text ="Note: Uploading a product discount data removes any existing data from the table."></asp:Label>
                  
                  </td>
                  </tr> --%>
<tr><td colspan ="2"><br /></td></tr>
                        <tr>
                            <td  class="txtSMBold">
                                Select File :
                            </td>
                            <td>
                                <asp:FileUpload ID="ExcelFileUpload" runat="server" />
                            </td>
                         
                        </tr>
                       <tr><td colspan ="2"><br /></td></tr>

                 
                        <tr>
                        <td class="txtSMBold"></td><td >
                         <asp:Button ID="btnImportSave" CssClass="btnInputGreen" TabIndex="1" CausesValidation="false"
                                    OnClientClick="return DisableValidation()" runat="server" Text="Import" />
                                <asp:Button ID="DummyImBtn" Style="display: none" runat="server" Text="Import" CausesValidation="false"
                                    OnClientClick="return DisableValidation()" />
                                <asp:Button ID="btnCancelImport" CssClass="btnInputRed" TabIndex="2" OnClientClick="return DisableValidation()"
                                    runat="server" CausesValidation="false" Text="Cancel" />
                                     <asp:LinkButton ID="lbLog" Font-Bold ="true" Font-Size ="13px" ForeColor  ="#337AB7" ToolTip ="Click here to see the uploaded log" runat ="server" Text ="View Log" OnClick ="lbLog_Click"></asp:LinkButton>
                        </td></tr>
                        
                        <tr><td colspan ="2"><asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" ></asp:Label></td></tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="Panel" runat="server" 
                                DisplayAfter="10">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                                        <img src="../images/Progress.gif" alt="Processing..." style="vertical-align: middle;" />
                                        <span style="font-size: 12px; font-weight:700px;color: #3399ff;">Processing...
                                        
                                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        </span>
                                    </asp:Panel>
                                    </div>
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
                    
              
            </td>
        </tr>
    </table>
</asp:Content>
