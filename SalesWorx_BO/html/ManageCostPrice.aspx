<%@ Page Title="Manage Cost Price" Language="vb" AutoEventWireup="false" EnableEventValidation="false"    
 MasterPageFile="~/html/Site.Master" CodeBehind="ManageCostPrice.aspx.vb"  Inherits="SalesWorx_BO.ManageCostPrice" %>

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
     html body .RadInput_Simple .riTextBox, html body .RadInputMgr_Simple {
    border-color: #616161;
    background: #fff;
    color: black;
    font: normal 12px "Segoe UI",Arial,Helvetica,sans-serif;
    text-align: right;
}
 </style> 
      <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }

        .rgGroupHeader td p
        {
            display: inline;
            margin: 0;
            padding: 0 10px;
            color: #000 !Important;
            font-weight: bold !Important;
        }

        .rgGroupHeader td
        {
            padding-left: 8px;
            padding-bottom: 2px;
            background-color: #eff9ff !Important;
            color: #000 !Important;
        }
         .RadGrid_Simple .rgCommandRow
        {
            background: whitesmoke;
            color: #000;
            /* height: 15px !important; */
        }

        .rgFooter td
        {
            border-top: 1px solid;
            border-color: #999 #c3c3c3;
            color: #000 !Important;
            background-color: #ebe8e8 !Important;
            font-weight: bold !Important;
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
   <h4>Manage Cost Price</h4>
                
             
                             <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                             <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                            
                              
                     <%--     <asp:PostBackTrigger  ControlID="btnImportWindow"  />--%>
                        
                            </Triggers>          
                                  
                                <ContentTemplate>
                                
                                     <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                    <telerik:RadComboBox Skin="Simple" ID="ddl_org" runat="server" Width="100%" TabIndex ="1"  CssClass="inputSM" 
                                                         AutoPostBack="true"></telerik:RadComboBox>
                                            </div>
                                         </div>
                                         <div class="col-sm-4">
                                             <div class="form-group">
                                                    <label class="hidden-xs"><br /></label>
                                                        <asp:Button ID="btnExport" runat="server" CssClass ="btn btn-warning" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-warning" Text="Import" TabIndex ="12" />
                                                </div>
                                         </div>
                                     </div>
                                       
                             
                                 
                                        
                                    
                    
                         
                       
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
                                           
                                                    <telerik:RadGrid ID="gvSKU" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="6" AllowPaging="true" runat="server" DataSourceID="SqlDataSource1"  AllowFilteringByColumn ="true"  ShowFooter ="true" 
              
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto"  AllowFilteringByColumn ="true"  ShowFooter ="true" 
                     Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource1"  CommandItemDisplay="Top"
                     PageSize="6">
                       <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear all filter" ToolTip="Clear all filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    
                    <Columns>



                        <telerik:GridBoundColumn  DataField="SKU" HeaderText="SKU" 
                            SortExpression="SKU"   AllowFiltering ="true" ShowFilterIcon ="false" 
                            CurrentFilterFunction ="Contains" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the item code/name and press enter" >
                             <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="false" HorizontalAlign ="Left"  />

                       
                        </telerik:GridBoundColumn>
                       
                          <telerik:GridBoundColumn  DataField="UOM" HeaderText="UOM"
                            SortExpression="UOM" ShowFilterIcon ="false" AllowFiltering ="false"  >
                                <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="false" HorizontalAlign ="Left"  />
                             <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="Agency" HeaderText="Agency"
                            SortExpression="Agency" AllowFiltering ="false" ShowFilterIcon ="false" CurrentFilterFunction ="StartsWith" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the agency name and press enter">
                              <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="false" HorizontalAlign ="Left"  />
                          
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Brand_Code" HeaderText="Brand"
                            SortExpression="Brand_Code" AllowFiltering ="false" ShowFilterIcon ="false" CurrentFilterFunction ="StartsWith" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the brand name and press enter">
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                            <HeaderStyle Wrap="true" HorizontalAlign ="left"  />
                             <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                           <telerik:GridTemplateColumn UniqueName="CostPrice" AllowFiltering="false" DataField ="CostPrice"
                                                    InitializeTemplatesFirst="false" HeaderText="Cost Price" SortExpression ="CostPrice">


                                                    <ItemTemplate>

                                                    
                                                      <telerik:RadNumericTextBox runat="server" ID="txtCostPrice"  Width="40%"
                                              MinValue="0" Skin ="Simple" TabIndex ="4" MaxLength ="7"   Text ='<%# Bind("CostPrice")%>'
                                            autocomplete="off" NumberFormat-DecimalDigits="2" Enabled ="true" 
                                            AllowOutOfRangeAutoCorrect="false" 

                                            IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                        </telerik:RadNumericTextBox>
                                                        <asp:Label runat ="server" ID="lblItemID" Visible ="false" Text ='<%# Bind("Inventory_Item_ID")%>'></asp:Label>
                                                    </ItemTemplate>

                                                  
                                                   <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn UniqueName="EditColumn"  AllowFiltering ="false"   
                    InitializeTemplatesFirst="false">
                   
                  
                    <ItemTemplate>
                        
                               <asp:ImageButton ID="btnEdit" ToolTip="Update Cost Price" Width ="24px" Height ="24px" runat="server" OnClick ="btnEdit_Click" CausesValidation="false"  
                                                       ImageUrl="~/images/update.png" />
                                                                   
                    </ItemTemplate>
       <HeaderStyle  />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </telerik:GridTemplateColumn>
                      
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
      
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
            SelectCommand="app_GetProductListByOrg" SelectCommandType="StoredProcedure">

            <SelectParameters>
                <asp:ControlParameter ControlID="ddl_org" Name="OrgID" DefaultValue="0" />
                
            </SelectParameters>

        </asp:SqlDataSource>

                                           
                                            </div>    
                                     
                                   
                                      </ContentTemplate>
                            </asp:UpdatePanel>
                         
                     
                        
                              
         
            
                 
            
           
                           <asp:UpdatePanel ID="Panel" runat="server" >
                             <Triggers>
           
      
            
	
        </Triggers>
                                <ContentTemplate>
                                

                                                  <telerik:RadWindow ID="MPEImport" Title= "Import Product Cost Price" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" Width ="450px" Height ="200px" VisibleStatusbar="false" Overlay="true" Modal="true">
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
                                                                    <asp:Button ID="btnImportSave" CssClass ="btn btn-primary2"  TabIndex="1" CausesValidation="false"
                                                                        OnClientClick="return DisableValidation()" runat="server" Text="Import" />
                                                                    <asp:Button ID="DummyImBtn" Style="display: none" runat="server" Text="Import" CausesValidation="false"
                                                                        OnClientClick="return DisableValidation()" />
                                                                    <asp:Button ID="btnCancelImport"  CssClass ="btn btn-default" Visible ="false"   TabIndex="2" OnClientClick="return DisableValidation()"
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
                                              </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="UpdateProgress2"  DisplayAfter="10" runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                                        <span>Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                    
              
           
</asp:Content>
