<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Rep_DailyStockReconciliation.aspx.vb" Inherits="SalesWorx_BO.Rep_DailyStockReconciliation" MasterPageFile="~/html/Site.Master" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function alertCallBackFn(arg) {

        }
        function clickExportBiffExcel() {

            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }
        function clickExportExcel() {
            $("#MainContent_BtnExportExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }
        function OpenViewWindowOpening_S(SPID, InvID) {
        }
    </script>
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }
        .rgMasterTable > thead > tr > th, .rgMasterTable > tbody > tr > td
        {
            vertical-align: middle !important;
        }
        .label-width
        {
            width:150px;
            display:block;
        }
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
 
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Daily Stock Report</h4>	
    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
    </telerik:RadWindowManager>

        <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
	
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
            	 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
              ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                    <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
            <td> <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                  </div>
                                          </div>
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR<em><span>&nbsp;</span>*</em></label>
                                                  <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Van" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  EmptyMessage="Select Van/FSR" 
                    runat="server" DataTextField="SalesRep_Name"  Width ="100%" DataValueField="SalesRep_ID">
                </telerik:RadComboBox>
                                            </div>
                                          </div>

                                                 
                                                    
                                                    <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label>Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate" Width="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    </div>
                                                 </div>
                                                 
                                                   <div class="col-sm-8">
                                            <div class="form-group">
                                                <label>Item</label>
                                                 
                                                <telerik:RadComboBox ID="ddl_item"   Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type product code/ name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
                                            </div>
                                          </div>  
                                                     
                                            </div>
                                                 </div>
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
                                                    <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                    </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                
                                            </div>
                                            </div>
                                       
                                            </div>
                                </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 

            <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong><asp:Label ID="lbl_Van" runat="server" Text=""></asp:Label></p>
              <p><strong>Product: </strong> <asp:Label ID="lbl_SKU" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong> <asp:Label ID="lbl_From" runat="server" Text=""></asp:Label></p>  
              <p><strong>To Date: </strong> <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
             
            </span>
            </i>      
        </div>
    </div>
           
              <div class="overflowx">
            <telerik:RadGrid ID="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="6" AllowPaging="True" runat="server"
                GridLines="None" GroupingSettings-GroupContinuesFormatString="">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                 
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="6" CommandItemDisplay="Top">
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>

                   
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>
                     

                     <telerik:GridTemplateColumn uniqueName="Product"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Item" SortExpression ="Product"
                                                                HeaderText="Product" >
                                                            <ItemTemplate>
                                                                
                                                                <asp:Label CssClass="label-width" ID="Product" runat="server" Text='<%# Bind("Item")%>'></asp:Label>
                                                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="left" Width="200" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>
                        
                          <telerik:GridTemplateColumn uniqueName="UOM"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="UOM" SortExpression ="UOM"
                                                                HeaderText="UOM" >
                                                            <ItemTemplate>
                                                                
                                                                <asp:Label CssClass="label-width" ID="lbl_UOM" runat="server" Text='<%# Bind("UOM")%>'></asp:Label>
                                                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="left" Width="200" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>

                          <telerik:GridTemplateColumn uniqueName="Opening_S"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Opening_S" SortExpression ="Opening_S"
                                                                HeaderText="Opening<br/>Stock<br/>(Sellable)" >
                                                            <ItemTemplate>
                                                                
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Opening_S" runat="server" Text='<%# Bind("Opening_S", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowOpening_S(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="Right" Width="100" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>
                     
                             <telerik:GridTemplateColumn uniqueName="Opening_N"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Opening_N" SortExpression ="Opening_N"
                                                                HeaderText="Opening Stock<br/>(Non Sellable)" >
                                                            <ItemTemplate>
                                                               
                                                                 <asp:LinkButton Enabled = "false" Width="50"  ID="Lnk_Opening_N" runat="server" Text='<%# Bind("Opening_N", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowOpening_N(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                 <ItemStyle HorizontalAlign="Right" Width="100" />
                                  <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn> 
                        
                           <telerik:GridTemplateColumn uniqueName="Loaded"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Loaded" SortExpression ="Loaded"
                                                                HeaderText="Loaded" >
                                                            <ItemTemplate>
                                                                
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Loaded" runat="server" Text='<%# Bind("Loaded", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowLoad(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Width="100" />
                                <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>        

                         <telerik:GridTemplateColumn uniqueName="Sold"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Sold" SortExpression ="Sold"
                                                                HeaderText="Sold" >
                                                            <ItemTemplate>
                                                              
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Sold" runat="server" Text='<%# Bind("Sold", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowSold(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="Right" Width="100" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>     
                            <telerik:GridTemplateColumn uniqueName="Returned_S"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Returned_S" SortExpression ="Returned_S"
                                                                HeaderText="Returned<br/>(Sellable)" >
                                                            <ItemTemplate>
                                                                 
                                                                 <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Returned_S" runat="server" Text='<%# Bind("Returned_S", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowReturned_S(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Width="100" />
                                 <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn uniqueName="Returned_N"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Returned_N" SortExpression ="Returned_N"
                                                                HeaderText="Returned<br/>(Non Sellable)" >
                                                            <ItemTemplate>
                                                               
                                                                 <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Returned_N" runat="server" Text='<%# Bind("Returned_N", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowReturned_N(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" Width="100" />
                             <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>  
                         <telerik:GridTemplateColumn uniqueName="Unload_S"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Unload_S" SortExpression ="Unload_S"
                                                                HeaderText="Unloaded<br/>(Sellable)" >
                                                            <ItemTemplate>
                                                               
                                                                 <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Unload_S" runat="server" Text='<%# Bind("Unload_S", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowUnload_S(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Width="100" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn> 
                        <telerik:GridTemplateColumn uniqueName="Unload_N"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Unload_N" SortExpression ="Unload_N"
                                                                HeaderText="Unloaded<br/>(Non Sellable)" >
                                                            <ItemTemplate>
                                                                 
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Unload_N" runat="server" Text='<%# Bind("Unload_N", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowUnload_N(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="100" />
                             <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>   
                        <telerik:GridTemplateColumn uniqueName="Closing_S"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Closing_S" SortExpression ="Closing_S"
                                                                HeaderText="Closing<br/>Stock<br/>(Sellable)" >
                                                            <ItemTemplate>
                                                                
                                                                 <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Closing_S" runat="server" Text='<%# Bind("Closing_S", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowClosing_S(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="100" />
                             <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>
                     
                             <telerik:GridTemplateColumn uniqueName="Closing_N"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Closing_N" SortExpression ="Closing_N"
                                                                HeaderText="Closing Stock<br/>(Non Sellable)" >
                                                            <ItemTemplate>
                                                              
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Closing_N" runat="server" Text='<%# Bind("Closing_N", "{0:N2}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowClosing_N(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Right" Width="100" />
                                  <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn> 
                    </Columns>
                    <GroupByExpressions>

                                            <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                     
                                                                <telerik:GridGroupByField HeaderText="Van/FSR" FieldName="Salesrep_name"   
                                                                   ></telerik:GridGroupByField>
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                             
                                                                     <telerik:GridGroupByField FieldName="Salesrep_name"  >

                                                                     </telerik:GridGroupByField>
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                      
                                                         
                                                    </GroupByExpressions>
                </MasterTableView>
            </telerik:RadGrid>
            </div>

            </contenttemplate>
         </asp:UpdatePanel>
     <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
        
   </div>
  
     <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
                 
         
    </ProgressTemplate>
            </asp:UpdateProgress>    
    </asp:Content>