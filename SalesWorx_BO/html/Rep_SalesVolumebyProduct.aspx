<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_SalesVolumebyProduct.aspx.vb" Inherits="SalesWorx_BO.Rep_SalesVolumebyProduct" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script src="../scripts/kendo.all.min.js"></script>
<script src="../scripts/kendo.dataviz.min.js"></script>
<link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />
     
  <script type="text/javascript">
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
        function alertCallBackFn(arg) {

        }

        function pageLoad(sender, args) {
            $('.rgMasterTable').find('th > a').attr("data-container", "body");
            $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
            $('[data-toggle="tooltip"]').tooltip();
        }

    </script>
 <Style>
        input[type="text"].rdfd_
{
    margin:0 !important;
    padding:0 !important;
    height:0 !important;
    width:0 !important;
}

        div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }   
    </Style>
      <style scoped>
        #MainContent_divsummary .col-sm-4:nth-child(1) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(1) .widgetblk{
	background-color: #4d5ec1;
}

#MainContent_divsummary .col-sm-4:nth-child(2) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(2) .widgetblk{
	background-color: #eb6357;
}

#MainContent_divsummary .col-sm-4:nth-child(3) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(3) .widgetblk{
	background-color: #14b4fc;
}

#MainContent_divsummary .col-sm-4:nth-child(4) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(4) .widgetblk{
	background-color: #97c95d;
}

#MainContent_divsummary .col-sm-4:nth-child(5) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(5) .widgetblk{
	background-color: #ef9933;
}

#MainContent_divsummary .col-sm-4:nth-child(6) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(6) .widgetblk{
	background-color: #607d8b;
}
        .chart-wrapper, .chart-wrapper .k-chart {
       

        }
             .rgExpand, .rgCollapse
  {
    display: none;
  }
  .RadGrid .rgPagerLabel, .RadGrid .rgPager .RadComboBox, .RadGrid .rgPager .RadInput {
margin: 0 4px 0 0;
vertical-align: middle;

visibility: hidden;
}


   #MainContent_divSummaryNet .col-sm-4:nth-child(1) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(1) .widgetblk{
	background-color: #4d5ec1;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(2) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(2) .widgetblk{
	background-color: #eb6357;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(3) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(3) .widgetblk{
	background-color: #14b4fc;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(4) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(4) .widgetblk{
	background-color: #97c95d;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(5) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(5) .widgetblk{
	background-color: #ef9933;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(6) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(6) .widgetblk{
	background-color: #607d8b;
}

  #clsAgency.rgFooter td {
            background:#fffdef !important;
        }
        #clsOutlet.rgFooter td {
            background:#ffefef !important;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Sales Volume By Product</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        
               <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2"/>
                           <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                       </UpdatedControls>
                    
                   </telerik:AjaxSetting>
                 
               </AjaxSettings>
           </telerik:RadAjaxManager>


     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server" >
     
 
	   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                         <div class="row">
                                             <div class="col-sm-10">
                                                <div class="row">
    
                                                 <div class="col-sm-6">
                                                     
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" AutoPostBack="true"   Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" >
                                        </telerik:RadComboBox>
                                            </div>
                                            </div>
                                                    <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Van</label>
                                                  <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van" EnableCheckAllItemsCheckBox="true" ID="ddl_Van" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >

                                                  

                                            </div>
                                          </div>
                                                  
                                                    </div>
                                                 <div class="row">  
                                                <div class="col-sm-3">
                                            <div class="form-group">
                                                <label><asp:Label runat="server" ID="lbl_from" Text="From Date"></asp:Label></label>
                                                <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                   
                                            </div>
                                          </div>
                                                    <div class="col-sm-3">
                                            <div class="form-group">
                                                <label><asp:Label runat="server" ID="lbl_to" Text="To Date"></asp:Label></label>
                                                <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                   
                                            </div>
                                          </div>
                                              <div class="col-sm-6">
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
           <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat ="server" >
         <div id="rptsect" runat="server" visible="false"  >
                              <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
               <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
                <p><strong> <asp:Label ID="lbl_fromtxt" runat="server" Text=""></asp:Label></strong> <asp:Label ID="lbl_Fromdt" runat="server" Text=""></asp:Label></p>
                <p><strong> <asp:Label ID="lbl_Totxt" runat="server" Text=""></asp:Label></strong> <asp:Label ID="lbl_Todt" runat="server" Text=""></asp:Label></p>
                <p><strong>Agency: </strong> <asp:Label ID="lbl_agency" runat="server" Text=""></asp:Label></p>
                <p><strong>Product: </strong> <asp:Label ID="lbl_SkU" runat="server" Text=""></asp:Label></p>
                <p><strong>Customer: </strong> <asp:Label ID="lbl_customer" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
       
                              
            <asp:HiddenField ID="hfCurrency" runat="server" />
                                 <asp:HiddenField ID="hfDecimal" runat="server" />
             
             <div class="table-responsive" id="Detailed"  runat="server">

                   <telerik:RadTabStrip ID="Salestab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Van Sale Detail Report" runat="server">
                        </telerik:RadTab>

                         <telerik:RadTab Text="Van Sale Summary " runat="server">
                        </telerik:RadTab>

                       
                        

                    </Tabs>
                </telerik:RadTabStrip>
                                            
     <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                       <telerik:RadPageView ID="RadPageView1" runat="server">
                                            <p style="padding-top:15px;"><asp:Label ID="lblDesc1" Text="" runat="server" ></asp:Label></p>


                        <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat ="server"  >
                                                                  <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" width="100%"
                                GridLines="None" >
                                                       
                                                      <GroupingSettings  GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="true"
                                                        PageSize="12" CommandItemDisplay="Top" >
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                                                        <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None"></PagerStyle>
                                                        <Columns>
                                                                <%--<telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VAN" HeaderText="VAN"
                                                                  SortExpression ="VAN" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>--%>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Product" HeaderText="Product"
                                                                  SortExpression ="Product" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="Product Code"
                                                                  SortExpression ="Item_Code" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="UOM" HeaderText="UOM<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="UOM"  HeaderTooltip="UOM">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Left"   />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left"  DataField="TotalSalesQty" HeaderText="Total Qty Sold<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="TotalSalesQty"  UniqueName="TotalSalesQty"  Aggregate="Sum"  DataFormatString="{0:#,##0.0###}"  HeaderTooltip="Total units sold"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>




                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" uniqueName="TotalSalesValue"  DataField="TotalSalesValue" HeaderText="Total Sales<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="TotalSalesValue" Aggregate="Sum" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Total value of sales">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"   />
                                                              </telerik:GridBoundColumn>


                                                                                  

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TotalReturnQty" HeaderText="Total Qty Returned<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="TotalReturnQty"  Aggregate="Sum" DataFormatString="{0:#,##0.0###}" HeaderTooltip="Total units returned">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TotalReturnValue" HeaderText="Total Returns<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="TotalReturnValue"  uniqueName="TotalReturnValue" Aggregate="Sum" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Total value of returns">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>


                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NetSalesQty" HeaderText="Net Sales Qty<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="NetSalesQty"  Aggregate="Sum" DataFormatString="{0:#,##0.0###}" HeaderTooltip="Net Sales Qty">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NetSales" HeaderText="Net Sales Value<i class='fa fa-info-circle'></i>"  
                                                                  SortExpression ="NetSales" uniqueName="NetSales"  Aggregate="Sum" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Net Sales value">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                        </Columns>

                                                    
                                                   <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="VAN" FieldName="VAN" 
                                                                   ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="VAN" SortOrder="Descending" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                         
                                                    </GroupByExpressions>

                                                        
                                                        
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                                                               </telerik:RadAjaxPanel>
                                         
                                             </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView2" runat="server">
                                             <p style="padding-top:15px;"><asp:Label ID="Label1" Text="" runat="server" ></asp:Label></p>

                                           <div id="divSummaryNet" runat="server" class="row"></div> 
                           <p><br /></p>
                                           

                           <div class="overflowx" >
                                                <div class="chart-wrapper padding0" style="" id="Chartwrapper" runat="server" >

                

 <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server"  >
   
 <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"      ChartTitle-Text="Net Sales Qty"
        PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black"  
         ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server" Visible="false" ID="Chart" Height="500" Transitions="true" Skin="Silk">
       <PlotArea>
                <Series >
                    <telerik:ColumnSeries DataFieldY="NetSalesQty" Name="Net Sales Qty" >
                        <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None"  ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                Net Sales Qty :   #=dataItem.NetSalesQty#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                    
                </Series>
                <XAxis DataLabelsField="VAN" >
                    <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                    <MinorGridLines Visible="false"></MinorGridLines>
                    <MajorGridLines Visible="false"></MajorGridLines>
                </XAxis>
                <YAxis>
                    <TitleAppearance Text="NetSalesQty" >
                        <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Bold="true" />
                    </TitleAppearance>
                    
                </YAxis>
            </PlotArea>
            <Legend>
                <Appearance Visible="true" Position="Top"></Appearance>
            </Legend>
           <%-- <ChartTitle Text="Outlets Covered vs Billed">
            </ChartTitle>--%>
        </telerik:RadHtmlChart>

  </telerik:RadAjaxPanel>

                    </div>
                                                </div>
                           <hr />
                                       
                                       <div class="row">
             <div class="col-sm-8">
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>
                                 <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div></div>
 <div class="overflowx">
     <telerik:RadAjaxPanel ID="RadAjaxPanel5" runat ="server"  >
                        <telerik:RadGrid id="gvRep_Summary" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="2" AllowPaging="True" runat="server" 
                                                                    GridLines="None"   >
                                                       
                                                                                         <GroupingSettings  CaseSensitive="false" RetainGroupFootersVisibility="false"   ></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" 
                                                        PageSize="2" CommandItemDisplay="Top" >
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                                                        <Columns>
                                                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VAN" HeaderText="VAN"
                                                                  SortExpression ="VAN" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                            
                                                     
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left"  DataField="TotalSalesQty" HeaderText="Total Qty Sold<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="TotalSalesQty"  UniqueName="TotalSalesQty"  DataType="System.Decimal" DataFormatString="{0:#,##0.0###}" HeaderTooltip="Total units sold"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>


                                                            


                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TotalSalesValue" HeaderText="Total Sales<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="TotalSalesValue"  UniqueName="TotalSalesValue" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Total value of sales">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TotalReturnQty" HeaderText="Total Qty Returned<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="TotalReturnQty" DataType="System.Decimal" DataFormatString="{0:#,##0.0###}" HeaderTooltip="Total units returned">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TotalReturnValue" HeaderText="Total Returns<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="TotalReturnValue" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Total value of returns">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>


                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NetSalesQty" HeaderText="Net Sales Qty<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="NetSalesQty" DataType="System.Decimal" DataFormatString="{0:#,##0.0###}" HeaderTooltip="Net Sales Qty">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NetSales" HeaderText="Net Sales Value<i class='fa fa-info-circle'></i>"  
                                                                  SortExpression ="NetSales" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Net Sales value">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                  

                                                        
                                                        

                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
         </telerik:RadPageView>
                                                               </telerik:RadAjaxPanel>
                                         </div>

                                
                                             </telerik:RadPageView>
                                        
                      </telerik:RadMultiPage>
     </div>
  </div>
     
    <asp:HiddenField ID="HUID" runat="server" />
    <asp:HiddenField ID="HCurrency" runat="server" Value="(AED)" />
    <asp:HiddenField ID="HDate" runat="server" />
<asp:HiddenField ID="HToDate" runat="server" />    
              <asp:HiddenField ID="HType" runat="server" />
         <asp:HiddenField ID="HAgency" runat="server" />
         <asp:HiddenField ID="Hcustomer" runat="server" />
         <asp:HiddenField ID="HItem" runat="server" />
         <asp:HiddenField ID="HVan" runat="server" />
         <asp:HiddenField ID="HorgID" runat="server" />
         <asp:HiddenField ID="HCount" runat="server" />
          <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                              
            </telerik:RadAjaxPanel> 
       </telerik:RadAjaxPanel>  


    

	       
  <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="Btn_LoadItem" runat="server" Text="Export" />
      <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
   </div>
	
    <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>
</asp:Content>