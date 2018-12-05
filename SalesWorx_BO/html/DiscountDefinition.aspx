<%@ Page Title="Item Level Discount Rule" Language="vb" AutoEventWireup="false" EnableEventValidation="false"    
 MasterPageFile="~/html/Site.Master" CodeBehind="DiscountDefinition.aspx.vb"  Inherits="SalesWorx_BO.DiscountDefinition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
     .rcTimePopup
 {
   display:none ! important;
 }
 </style> 

  <script language="javascript" type="text/javascript">

      function alertCallBackFn(arg) {
          HideRadWindow()
      }

      function HideRadWindow() {

          var elem = $('a[class=rwCloseButton');

          if (elem != null && elem != undefined) {
              $('a[class=rwCloseButton')[0].click();
          }

          $("#frm").find("iframe").hide();
      }
    </script>

    </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
   <h4>Item Level Discount Rule</h4>
                
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                            <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                         <asp:PostBackTrigger  ControlID="btndownloadTemp"  />
                            </Triggers>
                                <ContentTemplate>
                                 <asp:HiddenField ID="HDiscount_Plan_ID" runat="server" Value="0" />  
                                     <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization</label>
                                                    <telerik:RadComboBox Skin="Simple" ID="ddl_org" runat="server" Width="100%" TabIndex ="1"  CssClass="inputSM"  AutoPostBack="true"></telerik:RadComboBox>
                                            </div>
                                         </div>
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label><asp:Label ID="lbl_Plantxt" runat="server" Text="Plan:" Visible="false" ></asp:Label></label>
                                                    <asp:Label ID="lbl_planName" runat="server" Text=""  Visible="false"></asp:Label>
                                            </div>
                                         </div>
                                         <div class="col-sm-4">
                                             <div class="form-group">
                                                    <label class="hidden-xs"><br /></label>
                                                 <asp:Button ID="btn_back"  runat="server" CssClass="btn btn-danger" Text="Go back" Visible="false" />
                                                        <asp:Button ID="btnExport" runat="server" CssClass ="btn btn-warning" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-success" Text="Import" TabIndex ="12" />
                                                         <asp:Button ID="btndownloadTemp" runat="server" CssClass ="btn btn-primary" Text="Download Template" TabIndex ="11" />
                                                </div>
                                         </div>
                                     </div>
                                       
                             
                                     <div class="row">
                                          <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Item Code/Desc.</label>
                                                    <telerik:RadComboBox ID="ddlItem" Skin="Simple" Filter="Contains"  EmptyMessage ="Please enter item code/description"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" ShowToggleImage="false" ShowDropDownOnTextboxClick="false"
                                                    Width="100%">
                                                </telerik:RadComboBox>
                                                <asp:Label runat ="server" ID="lblLineID" Visible ="false" ></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                <label>Discount Mode</label>
                                                  <telerik:RadComboBox  Skin="Simple" ID="ddlType"  runat="server" TabIndex ="3" Width="100%">
                                               <%--    <asp:ListItem Value ="V">VALUE</asp:ListItem>
                                                  <asp:ListItem Value ="P">PERCENTAGE</asp:ListItem>--%>
                                                  </telerik:RadComboBox>
                                               </div>
                                            </div>
                                                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>  
                                                   Minimum Qty 
                                               </label>
                                                    <asp:TextBox ID="txtFromQty" runat="server"  TabIndex ="5" 
                                                            CssClass="inputSM" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTEOrdQty" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtFromQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </div>
                                                     </div>
                
                                         </div>
                                    <div class="row">
                                                 <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>     
                      
                                                 
                                                    Discount 
                                               </label>
                                                    <asp:TextBox ID="txtDisValue" runat="server" TabIndex ="7"
                                                            CssClass="inputSM" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                            FilterType="Numbers,Custom"  ValidChars ="." TargetControlID="txtDisValue">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </div>
                                                </div> 
                                            <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>       
                  Valid From </label>
                 
                  <telerik:RadDateTimePicker ID="StartTime"  MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"   Width="100%" TabIndex ="4"    runat="server" 
                                    >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                  <asp:RequiredFieldValidator runat="server" Visible ="false" Width ="3px" ID="RequiredFieldValidator1" ControlToValidate="StartTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator> 
</div>
                                      </div>
                                           <div class="col-sm-4">
                                            <div class="">
                                                <label>         
                     Valid To </label>
                 <telerik:RadDateTimePicker ID="EndTime" MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"    Width="100%" TabIndex ="6"   runat="server" 
                                    >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                 <asp:RequiredFieldValidator runat="server" Visible ="false" ID="Requiredfieldvalidator2" Width ="3px"  ControlToValidate="EndTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                       <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="EndTime"
                        ControlToCompare="StartTime" Operator="GreaterThan"    Type="String"
                        ErrorMessage="To date > From date"> </asp:CompareValidator>
                     </div>
                                               </div>

                                          
                                              
                                               
                                               
                                             </div>
                                         
                                               
                                   
                                            <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                                                    
                                                  
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server" CssClass ="btn btn-primary" Text="Add" TabIndex ="8" />
                                                      &nbsp;
                                                        <asp:Button ID="btnClear" runat="server"  CssClass ="btn btn-default"   Text="Reset" TabIndex ="9" />
                                                   
                                                
                                                
                                                   </div>
                        </div>
                                                </div>
                                        
                                    
                    
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
                         
                            <asp:UpdatePanel ID="Panel" runat="server" >
                                <ContentTemplate>
                                    <div class="table-responsive">
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
                                                            <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="headerstyle" />
                                                    </asp:GridView>
                                   </div>
                                        <div class="table-responsive">
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
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Edit" ID="btnEdit"   runat="server" CommandName="EditRecord"
                                                                        CausesValidation="false"   ImageUrl="~/images/edit-13.png"   />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Center" />
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
                                            </div>    
                                         <asp:Panel ID="PnlGridData" runat="server"  >
                                    </asp:Panel>
                                   
                                 
                         
                     
                                </ContentTemplate>
                            </asp:UpdatePanel>
                              
        
            
                   <telerik:RadWindow ID="MPEImport" Title= "Import Discount Data" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                                                  <div class="popupcontentblk">
                                                      <%--<p><asp:Label runat ="server" ID="Label3" ForeColor="Red"   Text ="Note: Uploading a product discount data removes any existing data from the table."></asp:Label></p>--%>
                                                      <p><asp:Label runat ="server" ID="lblUpMsg" ForeColor ="Red" ></asp:Label></p>
                                                      <div class="row">
                                                          <div class="col-sm-5">
                                                              <label>Select File</label>
                                                          </div>
                                                          <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <asp:FileUpload ID="ExcelFileUpload" runat="server" />
                                                                </div>
                                                          </div>
                                                      </div>
                                                      <div class="row">
                                                          <div class="col-sm-5"></div>
                                                          <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <asp:Button ID="btnImportSave" CssClass ="btn btn-warning"  TabIndex="1" CausesValidation="false"
                                                                        OnClientClick="return DisableValidation()" runat="server" Text="Import" />
                                                                    <asp:Button ID="DummyImBtn" Style="display: none" runat="server" Text="Import" CausesValidation="false"
                                                                        OnClientClick="return DisableValidation()" />
                                                                    <asp:Button ID="btnCancelImport"  CssClass ="btn btn-default"  TabIndex="2" OnClientClick="return DisableValidation()"
                                                                        runat="server" CausesValidation="false" Text="Cancel" />
                                                                         <asp:LinkButton ID="lbLog" ToolTip ="Click here to see the uploaded log" runat ="server" Text ="View Log" OnClick ="lbLog_Click"></asp:LinkButton>
                                                                </div>
                                                          </div>
                                                      </div>
                                                      <div>
                                                          <asp:UpdatePanel runat="server" ID="UpPanel">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                      </div>
                                                  </div>

                    
                 </ContentTemplate>
                       </telerik:RadWindow>
           
                           
                            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10" runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                                        <span>Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                    
              
           
</asp:Content>
